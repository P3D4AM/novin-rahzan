using Microsoft.SqlServer.Management.Assessment.Expressions;

namespace Microsoft.SqlServer.Management.Assessment;

internal class PermutationContext : DerivedEvaluationContext
{
	public ProbeContext ProbeContext { get; }

	public override ConcurrentSet<Expression> CallStack => ProbeContext.CallStack;

	private int[] Indices { get; }

	public PermutationContext(ProbeContext probeContext, int[] indices)
		: base(probeContext.Name, probeContext.BaseContext)
	{
		ProbeContext = probeContext;
		Indices = indices;
	}

	protected override bool TryGetDataCore(string name, out object value)
	{
		return ProbeContext.TryGetData(name, out value);
	}

	protected override bool TryGetOwnData(string column, out object value)
	{
		if (base.TryGetOwnData(column, out value))
		{
			return true;
		}
		for (int i = 0; i < Indices.Length; i++)
		{
			IProbeResult probeResult = ProbeContext.ProbeResults[i];
			if (probeResult.Data[Indices[i]].TryGetData(column, out value))
			{
				return true;
			}
		}
		value = null;
		return false;
	}
}
