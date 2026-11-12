using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlTableUdtInstanceMethodExpression : SqlTableUdtInstanceMemberExpression
{
	private readonly SqlScalarExpressionCollection arguments;

	private readonly SqlIdentifier alias;

	private readonly SqlIdentifierCollection columnList;

	internal override bool IsVisitable => true;

	internal override TableExpressionName ExpressionName => new TableExpressionName((!SqlIdentifier.IsNullOrEmpty(alias)) ? alias.Value : string.Empty);

	public SqlScalarExpressionCollection Arguments => arguments;

	public SqlIdentifier Alias => alias;

	public SqlIdentifierCollection ColumnList => columnList;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.UdtInstance;
			yield return base.MemberName;
			if (Arguments != null)
			{
				foreach (SqlScalarExpression argument in Arguments)
				{
					yield return argument;
				}
			}
			if (alias != null)
			{
				yield return alias;
			}
			if (columnList == null)
			{
				yield break;
			}
			foreach (SqlIdentifier column in columnList)
			{
				yield return column;
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

	internal override void Bind(IBindingContext bindingContext)
	{
		base.UdtInstance.Bind(bindingContext);
		base.BindingState |= base.UdtInstance.BindingState;
		IScalar boundScalar = base.UdtInstance.BoundScalar;
		if (boundScalar != null)
		{
			IScalarDataType scalarDataType = BindUtils.DataType.AsScalarDataType(boundScalar.DataType);
			IXmlDataType dataType;
			if ((dataType = BindUtils.ScalarDataType.AsXmlDataType(scalarDataType)) != null)
			{
				BindXmlMethod(bindingContext, dataType, arguments);
			}
			else if (!scalarDataType.IsUnknown)
			{
				base.UdtInstance.ReportBindError(SR.CannotCallMethodsOnType(scalarDataType.Name), ErrorFlags.UnresolvedReference);
			}
		}
	}

	private void BindXmlMethod(IBindingContext bindingContext, IXmlDataType dataType, SqlScalarExpressionCollection arguments)
	{
		IFunction function = dataType.TableMethods[base.MemberName.Value];
		if (function != null)
		{
			IMetadataFactory metadataFactory = bindingContext.MetadataFactory;
			CollationInfo databaseCollation = bindingContext.DatabaseCollation;
			IMutableTabular table = metadataFactory.Tabular.CreateDerivedTable((!SqlIdentifier.IsNullOrEmpty(alias)) ? alias.Value : string.Empty, databaseCollation);
			if (columnList != null)
			{
				BindUtils.XmlDataType.PopulateTabularColumns(bindingContext, alias, columnList, table);
			}
			SetBoundTable(bindingContext, table, (SqlCodeObject)(((object)alias) ?? ((object)this)));
			base.BindingState |= BindingState.BindSucceeded;
		}
		else
		{
			base.MemberName.ReportBindError(SR.NotValidFunctionOrProperty(base.MemberName.ToString()), ErrorFlags.UnresolvedReference);
			base.BindingState |= BindingState.BindFailed;
		}
		if (arguments == null)
		{
			return;
		}
		if (function != null)
		{
			base.BindingState |= BindUtils.Function.BindArguments(this, arguments, bindingContext, function);
			return;
		}
		foreach (SqlScalarExpression argument in arguments)
		{
			argument.Bind(bindingContext);
			base.BindingState |= argument.BindingState;
		}
	}

	internal SqlTableUdtInstanceMethodExpression(SqlScalarExpression instance, SqlIdentifier methodName, List<SqlScalarExpression> arguments, SqlIdentifier alias, List<SqlIdentifier> columnList)
		: base(instance, methodName)
	{
		this.arguments = ((arguments != null) ? new SqlScalarExpressionCollection(arguments) : null);
		this.alias = alias;
		if (columnList != null)
		{
			this.columnList = new SqlIdentifierCollection(columnList);
		}
		if (alias == null || columnList == null)
		{
			ReportParseError(SR.TVFMethodMustBeAliased);
		}
	}
}
