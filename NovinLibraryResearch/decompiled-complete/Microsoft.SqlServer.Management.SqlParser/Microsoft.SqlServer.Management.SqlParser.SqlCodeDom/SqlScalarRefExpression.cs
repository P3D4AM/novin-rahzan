using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlScalarRefExpression : SqlScalarExpression
{
	private class SqlColumnOrPropertyRefExpression : SqlScalarRefExpression
	{
		public SqlIdentifier ColumnOrPropertyName
		{
			get
			{
				int index = multipartIdentifier.Count - 1;
				return multipartIdentifier[index];
			}
		}

		internal override void Bind(IBindingContext bindingContext)
		{
			IColumn boundColumn = null;
			ITabular boundTable = null;
			int columnIndex;
			BindUtils.Column.LookupResult result = BindUtils.Column.LookupColumnPrefix(bindingContext, multipartIdentifier, out columnIndex, out boundTable, out boundColumn);
			if (boundTable != null)
			{
				int index = columnIndex - 1;
				bindingContext.Batch.SetBoundObject(multipartIdentifier[index], boundTable);
			}
			if (boundColumn != null)
			{
				bindingContext.Batch.SetBoundObject(multipartIdentifier[columnIndex], boundColumn);
				base.BindingState |= BindingState.BindSucceeded;
				IScalar scalar = boundColumn;
				for (int i = columnIndex + 1; i < multipartIdentifier.Count; i++)
				{
					IDataType dataType = scalar.DataType;
					if (BindUtils.DataType.IsNullOrUnknown(dataType))
					{
						break;
					}
					IClrDataType clrDataType = BindUtils.ScalarDataType.AsClrDataType(BindUtils.DataType.AsScalarDataType(dataType));
					if (clrDataType != null)
					{
						SqlIdentifier sqlIdentifier = multipartIdentifier[i];
						IUdtDataMember udtDataMember = clrDataType.DataMembers[sqlIdentifier.Value];
						if (udtDataMember != null)
						{
							if (udtDataMember.IsStatic)
							{
								sqlIdentifier.ReportBindError(SR.UdtPropertyIsStatic(sqlIdentifier.Value, clrDataType.ClassName, clrDataType.AssemblyName));
								base.BindingState |= BindingState.BindFailed;
								break;
							}
							bindingContext.Batch.SetBoundObject(sqlIdentifier, udtDataMember);
							scalar = udtDataMember;
							continue;
						}
						if (clrDataType.IsSystem)
						{
							sqlIdentifier.ReportBindError(SR.CouldNotFindPropertyOrField(sqlIdentifier.Value, clrDataType.ClassName, clrDataType.AssemblyName), ErrorFlags.UnresolvedReference);
						}
						base.BindingState |= BindingState.BindFailed;
						break;
					}
					multipartIdentifier[i - 1].ReportBindError(SR.CannotCallMethodsOnType(dataType.Name), ErrorFlags.UnresolvedReference);
					base.BindingState |= BindingState.BindFailed;
					break;
				}
			}
			CheckAndReportErrors(result, columnIndex, isPrefixed: true, boundColumn, boundTable);
		}

		public SqlColumnOrPropertyRefExpression(SqlMultipartIdentifier multipartIdentifier)
			: base(multipartIdentifier)
		{
		}
	}

	private readonly SqlMultipartIdentifier multipartIdentifier;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public sealed override IScalar BoundScalar
	{
		get
		{
			int index = multipartIdentifier.Count - 1;
			return GetBoundObjectForIdentifier(index) as IScalar;
		}
	}

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.ColumnReference;

	internal override bool IsConstantExpression => false;

	internal bool IsPrefixed => multipartIdentifier.Count > 1;

	public SqlMultipartIdentifier MultipartIdentifier => multipartIdentifier;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return multipartIdentifier;
		}
	}

	public override void Accept(ISqlCodeObjectVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override T Accept<T>(ISqlCodeObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public override void Accept<C>(ISqlCodeObjectContextVisitor<C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this, context);
	}

	public override T Accept<T, C>(ISqlCodeObjectContextVisitor<T, C> visitor, C context)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this, context);
	}

	internal void CheckAndReportErrors(BindUtils.Column.LookupResult result, int columnIndex, bool isPrefixed, IColumn boundColumn, ITabular boundTable)
	{
		if (result == BindUtils.Column.LookupResult.EmptyName)
		{
			base.BindingState |= BindingState.BindFailed;
			return;
		}
		if (boundColumn == null)
		{
			SqlIdentifier sqlIdentifier = MultipartIdentifier[columnIndex];
			if (result == BindUtils.Column.LookupResult.Ambiguous)
			{
				sqlIdentifier.ReportBindError(SR.AmbiguousColumnName(sqlIdentifier.ToString()), ErrorFlags.UnresolvedReference);
			}
			else if (!HasAnyBindSkipped)
			{
				if (!isPrefixed || boundTable != null)
				{
					sqlIdentifier.ReportBindError(SR.InvalidColumnName(sqlIdentifier.ToString()), ErrorFlags.UnresolvedReference);
				}
				else
				{
					ReportBindError(SR.MultiPartIdentifierBindingError(ToString()), ErrorFlags.UnresolvedReference);
				}
			}
			base.BindingState = BindingState.BindFailed;
			return;
		}
		if (boundColumn.DataType != null && boundColumn.DataType.IsXml)
		{
			IXmlDataType xmlDataType = (IXmlDataType)boundColumn.DataType;
			if (xmlDataType.IsXmlNode && !IsValidXmlNodeUse(xmlDataType))
			{
				ReportBindError(SR.InvalidColumnXmlNodeUse(ToString()));
			}
		}
		base.BindingState = BindingState.BindSucceeded;
	}

	internal IMetadataObject GetBoundObjectForIdentifier(int index)
	{
		IMetadataObject result = null;
		if (base.IsBound)
		{
			SqlIdentifier key = multipartIdentifier[index];
			result = BindingContext.Batch.GetBoundObject(key);
		}
		return result;
	}

	private bool IsValidXmlNodeUse(IXmlDataType dataType)
	{
		bool result = false;
		if (base.Parent is SqlUdtInstanceMethodExpression)
		{
			SqlUdtInstanceMethodExpression sqlUdtInstanceMethodExpression = (SqlUdtInstanceMethodExpression)base.Parent;
			if (sqlUdtInstanceMethodExpression.UdtInstance == this && !SqlIdentifier.IsNullOrEmpty(sqlUdtInstanceMethodExpression.MemberName))
			{
				IXmlDataTypeMethod xmlDataTypeMethod = dataType.Methods[sqlUdtInstanceMethodExpression.MemberName.Value];
				if (xmlDataTypeMethod != null && BindUtils.XmlDataType.IsValidMethodForXmlNode(xmlDataTypeMethod))
				{
					result = true;
				}
			}
		}
		else if (base.Parent is SqlTableUdtInstanceMethodExpression)
		{
			SqlTableUdtInstanceMethodExpression sqlTableUdtInstanceMethodExpression = (SqlTableUdtInstanceMethodExpression)base.Parent;
			if (sqlTableUdtInstanceMethodExpression.UdtInstance == this && !SqlIdentifier.IsNullOrEmpty(sqlTableUdtInstanceMethodExpression.MemberName))
			{
				IXmlDataTypeTableMethod xmlDataTypeTableMethod = dataType.TableMethods[sqlTableUdtInstanceMethodExpression.MemberName.Value];
				if (xmlDataTypeTableMethod != null && BindUtils.XmlDataType.IsValidMethodForXmlNode(xmlDataTypeTableMethod))
				{
					result = true;
				}
			}
		}
		else if (base.Parent is SqlIsNullBooleanExpression && ((SqlIsNullBooleanExpression)base.Parent).Expression == this)
		{
			result = true;
		}
		return result;
	}

	internal SqlScalarRefExpression(SqlMultipartIdentifier multipartIdentifier)
	{
		this.multipartIdentifier = multipartIdentifier;
	}

	internal static SqlScalarRefExpression ColumnOrPropertyRefExpression(SqlMultipartIdentifier multipartIdentifier)
	{
		return new SqlColumnOrPropertyRefExpression(multipartIdentifier);
	}

	public override string ToString()
	{
		return multipartIdentifier.ToString();
	}
}
