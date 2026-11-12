using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

internal class ProbeContext : EvaluationContext<Expression>
{
	public List<IProbeResult> ProbeResults { get; } = new List<IProbeResult>();


	public ProbeContext(string name, IEvaluationContext baseContext)
		: base(name, baseContext)
	{
	}

	public void Add(IProbeResult result)
	{
		ProbeResults.Add(result);
	}

	public void Add(IEnumerable<IProbeResult> results)
	{
		ProbeResults.AddRange(results);
	}

	protected override object ToObject(Expression expression)
	{
		return expression.Evaluate(this);
	}
}
