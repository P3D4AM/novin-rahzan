using Microsoft.SqlServer.Management.Assessment.Checks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment;

internal class CheckContext : ProbeContext
{
	public Check Check { get; }

	public CharacteristicCollection Requirements { get; set; }

	public CheckContext(IEvaluationContext baseContext, Check check)
		: base(check.Id, baseContext)
	{
		Check = check;
	}

	protected override bool TryGetDataCore(string column, out object value)
	{
		if (Check.Parameters.TryGetValue(column, out var value2))
		{
			value = ToObject(value2);
			return true;
		}
		return base.TryGetDataCore(column, out value);
	}
}
