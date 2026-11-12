using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlMaxDegreeOfParallelismIndexOption : SqlIndexOption
{
	private readonly int degreeOfParallelism;

	private readonly bool isValid = true;

	private const int MaxDegreeOfParallelism = 64;

	private const int MinDegreeOfParallelism = 0;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "MAXDOP";

	public int DegreeOfParallelism => degreeOfParallelism;

	internal bool IsValid => isValid;

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

	internal SqlMaxDegreeOfParallelismIndexOption(SqlLiteralExpression degreeOfParallelism)
		: base(SqlIndexOptionType.MaxDegreeOfParallelism)
	{
		if (degreeOfParallelism != null)
		{
			this.degreeOfParallelism = degreeOfParallelism.AsInt32().Value;
			if (this.degreeOfParallelism > 64 || this.degreeOfParallelism < 0)
			{
				ReportParseError(SR.OutOfRangeDegreeOfParallelism(degreeOfParallelism.Value));
				isValid = false;
			}
		}
		else
		{
			isValid = false;
		}
	}

	internal SqlMaxDegreeOfParallelismIndexOption(SqlLiteralExpression degreeOfParallelism, bool isValid)
		: base(SqlIndexOptionType.MaxDegreeOfParallelism)
	{
		int? num = degreeOfParallelism.AsInt32();
		if (num.HasValue)
		{
			this.degreeOfParallelism = num.Value;
		}
		this.isValid = isValid;
	}
}
