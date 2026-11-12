using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlIgnoreDupKeyIndexOption : SqlIndexOption
{
	private readonly SqlOnOffValue onOffValue;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "IGNORE_DUP_KEY";

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

	internal SqlIgnoreDupKeyIndexOption(SqlOnOffValue onOffValue)
		: base(SqlIndexOptionType.IgnoreDupKey)
	{
		this.onOffValue = onOffValue;
	}
}
