using System;
using System.Globalization;

namespace Microsoft.SqlServer.Management.XEvent;

public sealed class PredFunctionExpr : PredExpr
{
	private PredOperand operand;

	private PredValue opvalue;

	private PredCompareInfo func;

	public PredOperand Operand => operand;

	public PredValue Value => opvalue;

	public PredCompareInfo Operator => func;

	public PredFunctionExpr(PredCompareInfo func, PredOperand operand, PredValue value)
	{
		using MethodTraceContext methodTraceContext = TraceHelper.TraceContext.GetMethodContext("Constructor");
		methodTraceContext.TraceParameterIn("func", func);
		methodTraceContext.TraceParameterIn("operand", operand);
		methodTraceContext.TraceParameterIn("value", value);
		CheckNotNull(methodTraceContext, func, "func");
		CheckNotNull(methodTraceContext, operand, "operand");
		CheckNotNull(methodTraceContext, value, "value");
		this.operand = operand;
		this.func = func;
		opvalue = value;
	}

	private void CheckNotNull(MethodTraceContext tm, object param, string paramName)
	{
		if (param == null)
		{
			tm.TraceError(string.Format(CultureInfo.InvariantCulture, "{0} should not be null", paramName));
			throw new ArgumentNullException(paramName, "should not be null.");
		}
	}
}
