using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlResumableIndexOption : SqlIndexOption
{
	private SqlOnOffValue onOffValue;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "RESUMABLE";

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

	internal SqlResumableIndexOption(SqlOnOffValue onOffValue)
		: base(SqlIndexOptionType.Resumable)
	{
		this.onOffValue = onOffValue;
	}
}
