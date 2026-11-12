using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUdtInstanceDataMemberExpression : SqlUdtInstanceMemberExpression
{
	internal override bool IsVisitable => true;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.UdtInstance;
			yield return base.MemberName;
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
		if (boundScalar == null)
		{
			return;
		}
		IScalarDataType scalarDataType = BindUtils.DataType.AsScalarDataType(boundScalar.DataType);
		IClrDataType boundUdt;
		IXmlDataType dataType;
		if ((boundUdt = BindUtils.ScalarDataType.AsClrDataType(scalarDataType)) != null)
		{
			BindUdtDataMember(bindingContext, boundUdt, isStatic: false);
		}
		else if ((dataType = BindUtils.ScalarDataType.AsXmlDataType(scalarDataType)) != null)
		{
			BindXmlMethod(bindingContext, dataType, null);
			if (BoundScalar != null)
			{
				ReportBindError(SR.IncorrectSyntaxToInvokeXmlMethod(base.MemberName.ToString()));
			}
		}
		else if (!scalarDataType.IsUnknown)
		{
			base.UdtInstance.ReportBindError(SR.CannotCallMethodsOnType(scalarDataType.Name), ErrorFlags.UnresolvedReference);
		}
	}

	internal SqlUdtInstanceDataMemberExpression(SqlScalarExpression instance, SqlIdentifier fieldOrPropertyName)
		: base(instance, fieldOrPropertyName)
	{
	}
}
