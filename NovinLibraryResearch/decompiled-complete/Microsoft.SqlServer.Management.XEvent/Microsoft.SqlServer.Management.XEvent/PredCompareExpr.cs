using System;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredCompareExpr : PredExpr
{
	public enum ComparatorType
	{
		EQ,
		NE,
		GT,
		GE,
		LT,
		LE
	}

	private ComparatorType type;

	private PredOperand operand;

	private PredValue value;

	public PredOperand Operand => operand;

	public PredValue Value => value;

	public ComparatorType Operator => type;

	public PredCompareExpr(ComparatorType type, PredOperand operand, PredValue value)
	{
		if (operand == null)
		{
			throw new ArgumentNullException("operand");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		this.type = type;
		this.operand = operand;
		this.value = value;
	}
}
