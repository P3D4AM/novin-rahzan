using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlStatisticsOnlyIndexOption : SqlIndexOption
{
	private readonly int value;

	private readonly SqlOnOffValue onOffValue;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "STATISTICS_ONLY";

	public SqlOnOffValue OnOffValue => onOffValue;

	public int Value => value;

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

	internal SqlStatisticsOnlyIndexOption(SqlLiteralExpression value)
		: base(SqlIndexOptionType.StatisticsOnly)
	{
		int? num = value.AsInt32();
		if (num.HasValue)
		{
			this.value = num.Value;
		}
		onOffValue = SqlOnOffValue.None;
	}

	internal SqlStatisticsOnlyIndexOption(SqlOnOffValue onOffValue)
		: base(SqlIndexOptionType.StatisticsOnly)
	{
		this.onOffValue = onOffValue;
	}
}
