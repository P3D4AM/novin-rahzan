using System;
using System.Collections.Generic;
using Babel;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUserDefinedScalarFunctionCallExpression : SqlScalarFunctionCallExpression
{
	private IScalarFunction boundFunction;

	private SqlObjectIdentifier objectIdentifier;

	internal override bool IsVisitable => true;

	internal sealed override bool IsValidFilterExpressionConstant => false;

	public override IScalarFunction BoundFunction => boundFunction;

	internal override ScalarExpressionType ExpressionType => ScalarExpressionType.Function;

	public SqlObjectIdentifier ObjectIdentifier => objectIdentifier;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return ObjectIdentifier;
			if (base.Arguments == null)
			{
				yield break;
			}
			foreach (SqlScalarExpression argument in base.Arguments)
			{
				yield return argument;
			}
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

	private static bool IsUdtOrXmlColumn(IColumn column)
	{
		IScalarDataType scalarDataType = BindUtils.DataType.AsScalarDataType(column.DataType);
		if (!scalarDataType.IsClr)
		{
			return scalarDataType.IsXml;
		}
		return true;
	}

	internal override void Bind(IBindingContext bindingContext)
	{
		BatchBindingContext batch = bindingContext.Batch;
		int count = objectIdentifier.Count;
		bool flag = false;
		if (count <= 3)
		{
			string value = objectIdentifier.DatabaseName.Value;
			string value2 = objectIdentifier.SchemaName.Value;
			string value3 = objectIdentifier.ObjectName.Value;
			LexicalScope lexicalScope = GetLexicalScope();
			boundFunction = batch.FindScalarValuedFunction(value, value2, value3, lexicalScope);
			if (boundFunction == null)
			{
				boundFunction = batch.FindUserDefinedAggregate(value, value2, value3, lexicalScope);
			}
			if (boundFunction != null)
			{
				batch.SetBoundObject(objectIdentifier.ObjectName, boundFunction);
				flag = true;
			}
		}
		IColumn boundColumn = null;
		ITabular boundTable = null;
		int num = -1;
		BindUtils.Column.LookupResult lookupResult = BindUtils.Column.LookupResult.None;
		if (!flag && count >= 2)
		{
			lookupResult = BindUtils.Column.FindColumn(bindingContext, objectIdentifier[0], out boundColumn);
			if (boundColumn != null)
			{
				if (IsUdtOrXmlColumn(boundColumn) || count > 4)
				{
					num = 0;
					flag = true;
				}
				else
				{
					boundColumn = null;
				}
			}
			else if (lookupResult == BindUtils.Column.LookupResult.Ambiguous)
			{
				foreach (IColumn item in bindingContext.FindAllColumns(objectIdentifier[0].Value))
				{
					if (IsUdtOrXmlColumn(item))
					{
						num = 0;
						flag = true;
					}
				}
			}
		}
		if (!flag && count >= 3)
		{
			lookupResult = BindUtils.Column.FindColumn(bindingContext, objectIdentifier, 1, out boundTable, out boundColumn);
			num = ((boundTable != null) ? 1 : 0);
		}
		if (boundTable != null)
		{
			bindingContext.Batch.SetBoundObject(objectIdentifier[0], boundTable);
		}
		if (boundColumn != null)
		{
			bindingContext.Batch.SetBoundObject(objectIdentifier[num], boundColumn);
			IScalar scalar = boundColumn;
			for (int i = num + 1; i < count; i++)
			{
				IDataType dataType = scalar.DataType;
				if (BindUtils.DataType.IsNullOrUnknown(dataType))
				{
					break;
				}
				IScalarDataType scalarDataType = BindUtils.DataType.AsScalarDataType(dataType);
				SqlIdentifier sqlIdentifier = objectIdentifier[i];
				bool flag2 = i == count - 1;
				IClrDataType clrDataType;
				IXmlDataType xmlDataType;
				if ((clrDataType = BindUtils.ScalarDataType.AsClrDataType(scalarDataType)) != null)
				{
					IUdtMethod udtMethod;
					IUdtMember udtMember;
					if (flag2)
					{
						udtMethod = clrDataType.Methods[sqlIdentifier.Value];
						udtMember = udtMethod;
					}
					else
					{
						udtMember = clrDataType.DataMembers[sqlIdentifier.Value];
						udtMethod = null;
					}
					if (udtMember != null)
					{
						if (udtMember.IsStatic)
						{
							sqlIdentifier.ReportBindError(SR.UdtPropertyIsStatic(sqlIdentifier.Value, clrDataType.ClassName, clrDataType.AssemblyName), ErrorFlags.UnresolvedReference);
							scalar = null;
						}
						else
						{
							bindingContext.Batch.SetBoundObject(sqlIdentifier, udtMember);
							scalar = udtMember;
							boundFunction = udtMethod;
						}
					}
					else
					{
						if (clrDataType.IsSystem)
						{
							sqlIdentifier.ReportBindError(SR.CouldNotFindMethod(sqlIdentifier.Value, clrDataType.ClassName, clrDataType.AssemblyName), ErrorFlags.UnresolvedReference);
						}
						scalar = null;
					}
				}
				else if ((xmlDataType = BindUtils.ScalarDataType.AsXmlDataType(scalarDataType)) != null)
				{
					if (flag2)
					{
						boundFunction = xmlDataType.Methods[sqlIdentifier.Value];
						scalar = boundFunction;
						if (boundFunction == null)
						{
							sqlIdentifier.ReportBindError(SR.NotValidFunctionOrProperty(sqlIdentifier.ToString()), ErrorFlags.UnresolvedReference);
						}
						else
						{
							if (xmlDataType.IsXmlNode && !BindUtils.XmlDataType.IsValidMethodForXmlNode(boundFunction))
							{
								ReportBindError(SR.InvalidColumnXmlNodeUse(boundColumn.Name));
							}
							batch.SetBoundObject(objectIdentifier.ObjectName, boundFunction);
						}
					}
					else
					{
						sqlIdentifier.ReportBindError(SR.NotValidFunctionOrProperty(sqlIdentifier.Value), ErrorFlags.UnresolvedReference);
						scalar = null;
					}
				}
				else
				{
					objectIdentifier[i - 1].ReportBindError(SR.CannotCallMethodsOnType(dataType.Name), ErrorFlags.UnresolvedReference);
					scalar = null;
				}
				if (scalar == null)
				{
					base.BindingState |= BindingState.BindFailed;
					break;
				}
			}
		}
		if (boundFunction == null)
		{
			base.BindingState = BindingState.BindFailed;
			if (lookupResult == BindUtils.Column.LookupResult.Ambiguous)
			{
				SqlIdentifier sqlIdentifier2 = objectIdentifier[0];
				sqlIdentifier2.ReportBindError(SR.AmbiguousColumnName(sqlIdentifier2.ToString()));
				return;
			}
			if (count == 1)
			{
				objectIdentifier.ReportBindError(SR.NotRecognizedFunctionName(objectIdentifier.ToString()), ErrorFlags.UnresolvedReference);
			}
			if (count == 4)
			{
				if (boundColumn == null || IsUdtOrXmlColumn(boundColumn))
				{
					objectIdentifier.ReportBindError(SR.RemoteFunctionRefIsNotAllowed(objectIdentifier.ToString(), objectIdentifier[num].ToString()), ErrorFlags.UnresolvedReference);
				}
			}
			else if (boundColumn == null)
			{
				if (boundTable != null || count > 4)
				{
					SqlIdentifier sqlIdentifier3 = objectIdentifier[num];
					sqlIdentifier3.ReportBindError(SR.InvalidColumnName(sqlIdentifier3.ToString()), ErrorFlags.UnresolvedReference);
				}
				else
				{
					objectIdentifier.ReportBindError(SR.CannotFindFunctionOrAggregate(objectIdentifier[0].ToString(), objectIdentifier.ToString()), ErrorFlags.UnresolvedDatabaseObjectReference);
				}
			}
		}
		else
		{
			base.BindingState = BindingState.BindSucceeded;
			base.BindingState |= BindUtils.Function.BindArguments(this, bindingContext, boundFunction);
		}
	}

	internal override void Unbind()
	{
		boundFunction = null;
		base.Unbind();
	}

	internal override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		string result = null;
		SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
		if (sqlCodeObject != null)
		{
			result = ((sqlCodeObject != objectIdentifier) ? sqlCodeObject.GetQuickInfoText(cursorLocation, displayInfoProvider) : objectIdentifier.GetQuickInfoText(cursorLocation, displayInfoProvider));
		}
		return result;
	}

	internal override MethodNameAndParamLocations GetMyMethodNameAndParams(LexLocation caretLocation)
	{
		MethodNameAndParamLocations result = null;
		if (BoundFunction != null)
		{
			TokenManager tokenManager = TokenManager;
			MethodNameAndParamLocations methodNameAndParamLocations = new MethodNameAndParamLocations(BoundObject.Name)
			{
				NameStartLocation = base.StartLocation,
				NameEndLocation = base.EndLocation,
				ParamSeperatorLocations = new List<Location>()
			};
			BuildMethodNameAndParams(arguments: GetSqlCodeObjectList(base.Arguments), methodNameAndParams: methodNameAndParamLocations, nameEndPosition: ObjectIdentifier.Position.endTokenNumber);
			Location location = tokenManager.GetLocation(caretLocation.startTokenNumber);
			if (methodNameAndParamLocations.IsLocationInArgumentList(location))
			{
				result = methodNameAndParamLocations;
			}
		}
		return result;
	}

	internal SqlUserDefinedScalarFunctionCallExpression(SqlObjectIdentifier name, IEnumerable<SqlScalarExpression> arguments)
		: base(arguments)
	{
		objectIdentifier = name;
	}
}
