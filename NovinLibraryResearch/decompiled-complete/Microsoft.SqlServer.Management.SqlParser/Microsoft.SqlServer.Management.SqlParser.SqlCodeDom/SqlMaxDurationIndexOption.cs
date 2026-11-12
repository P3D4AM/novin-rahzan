using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMaxDurationIndexOption : SqlIndexOption
{
	private readonly int maxDuration;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "MAX_DURATION";

	public int MaxDuration => maxDuration;

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

	internal SqlMaxDurationIndexOption(SqlLiteralExpression maxDuration)
		: base(SqlIndexOptionType.MaxDuration)
	{
		if (maxDuration != null)
		{
			this.maxDuration = maxDuration.AsInt32().Value;
		}
	}
}
