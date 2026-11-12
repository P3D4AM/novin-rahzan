using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUdtInstanceMethodExpression : SqlUdtInstanceMemberExpression
{
	private readonly SqlScalarExpressionCollection arguments;

	internal override bool IsVisitable => true;

	public SqlScalarExpressionCollection Arguments => arguments;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.UdtInstance;
			yield return base.MemberName;
			if (Arguments == null)
			{
				yield break;
			}
			foreach (SqlScalarExpression argument in Arguments)
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

	internal override void Bind(IBindingContext bindingContext)
	{
		base.UdtInstance.Bind(bindingContext);
		base.BindingState |= base.UdtInstance.BindingState;
		IScalar boundScalar = base.UdtInstance.BoundScalar;
		if (boundScalar != null)
		{
			IScalarDataType scalarDataType = BindUtils.DataType.AsScalarDataType(boundScalar.DataType);
			IClrDataType boundUdt;
			IXmlDataType dataType;
			if ((boundUdt = BindUtils.ScalarDataType.AsClrDataType(scalarDataType)) != null)
			{
				BindUdtMethod(bindingContext, boundUdt, isStatic: false, arguments);
			}
			else if ((dataType = BindUtils.ScalarDataType.AsXmlDataType(scalarDataType)) != null)
			{
				BindXmlMethod(bindingContext, dataType, arguments);
			}
			else if (!scalarDataType.IsUnknown)
			{
				base.UdtInstance.ReportBindError(SR.CannotCallMethodsOnType(scalarDataType.Name), ErrorFlags.UnresolvedReference);
			}
		}
	}

	internal SqlUdtInstanceMethodExpression(SqlScalarExpression instance, SqlIdentifier methodName, List<SqlScalarExpression> arguments)
		: base(instance, methodName)
	{
		this.arguments = ((arguments != null) ? new SqlScalarExpressionCollection(arguments) : null);
	}
}
