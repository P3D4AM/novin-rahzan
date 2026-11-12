using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlAllowPageLocksIndexOption : SqlIndexOption
{
	private readonly SqlOnOffValue onOffValue;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "ALLOW_PAGE_LOCKS";

	public SqlOnOffValue OnOffValue => onOffValue;

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

	internal SqlAllowPageLocksIndexOption(SqlOnOffValue onOffValue)
		: base(SqlIndexOptionType.AllowPageLocks)
	{
		this.onOffValue = onOffValue;
	}
}
