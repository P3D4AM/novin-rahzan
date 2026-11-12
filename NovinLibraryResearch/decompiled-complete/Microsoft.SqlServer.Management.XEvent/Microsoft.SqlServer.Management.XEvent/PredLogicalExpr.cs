using System;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredLogicalExpr : PredExpr
{
	public enum LogicalOperatorType
	{
		Not,
		And,
		Or
	}

	private readonly LogicalOperatorType type;

	private readonly PredExpr leftPredExpr;

	private readonly PredExpr rightPredExpr;

	public PredExpr LeftExpr => leftPredExpr;

	public PredExpr RightExpr => rightPredExpr;

	public LogicalOperatorType Operator => type;

	public PredLogicalExpr(LogicalOperatorType type, PredExpr predExpr1, PredExpr predExpr2)
	{
		if (predExpr1 == null)
		{
			throw new ArgumentNullException("predExpr1");
		}
		if (type == LogicalOperatorType.Not)
		{
			if (predExpr2 != null)
			{
				throw new XEventException(ExceptionTemplates.Expression2NotNull);
			}
		}
		else if (predExpr2 == null)
		{
			throw new ArgumentNullException("predExpr2");
		}
		this.type = type;
		leftPredExpr = predExpr1;
		rightPredExpr = predExpr2;
	}
}
