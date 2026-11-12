using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public class SqlFillFactorIndexOption : SqlIndexOption
{
	private readonly int fillFactor;

	private readonly bool isValid = true;

	private const int MinPercentage = 1;

	private const int MaxPercentage = 100;

	internal override bool IsVisitable => true;

	internal sealed override string Phrase => "FILLFACTOR";

	public int FillFactor => fillFactor;

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

	internal SqlFillFactorIndexOption(SqlLiteralExpression fillFactor)
		: base(SqlIndexOptionType.FillFactor)
	{
		if (fillFactor != null)
		{
			this.fillFactor = fillFactor.AsInt32().Value;
			if (this.fillFactor < 1 || this.fillFactor > 100)
			{
				ReportParseError(SR.InvalidFillFactorPercentage(fillFactor.Value));
				isValid = false;
			}
		}
		else
		{
			isValid = false;
		}
	}
}
