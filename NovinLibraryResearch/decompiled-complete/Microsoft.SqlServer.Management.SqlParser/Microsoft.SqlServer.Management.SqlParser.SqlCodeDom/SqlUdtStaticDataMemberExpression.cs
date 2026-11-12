using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlUdtStaticDataMemberExpression : SqlUdtStaticMemberExpression
{
	internal override bool IsVisitable => true;

	public override IEnumerable<SqlCodeObject> Children
	{
		get
		{
			yield return base.Type;
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
		IClrDataType clrDataType = BindType(bindingContext);
		if (clrDataType != null)
		{
			BindUdtDataMember(bindingContext, clrDataType, isStatic: true);
		}
	}

	internal SqlUdtStaticDataMemberExpression(SqlDataTypeSpecification type, SqlIdentifier fieldOrPropertyName)
		: base(type, fieldOrPropertyName)
	{
	}
}
