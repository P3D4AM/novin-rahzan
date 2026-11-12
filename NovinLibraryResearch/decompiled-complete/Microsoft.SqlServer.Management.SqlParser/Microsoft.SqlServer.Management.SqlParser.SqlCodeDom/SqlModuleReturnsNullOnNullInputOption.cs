using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public sealed class SqlModuleReturnsNullOnNullInputOption : SqlModuleOption
{
	internal override bool IsVisitable => true;

	internal override string DisplayName => "RETURNS NULL ON NULL INPUT";

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

	internal SqlModuleReturnsNullOnNullInputOption()
		: base(SqlModuleOptionType.ReturnsNullOnNullInput)
	{
	}
}
