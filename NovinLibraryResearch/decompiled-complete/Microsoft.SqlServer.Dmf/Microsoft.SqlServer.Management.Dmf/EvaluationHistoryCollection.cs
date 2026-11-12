using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class EvaluationHistoryCollection : SfcDictionaryCollection<EvaluationHistory, EvaluationHistory.Key, Policy>
{
	public EvaluationHistory this[long id] => base[new EvaluationHistory.Key(id)];

	public EvaluationHistoryCollection(Policy parent)
		: base(parent)
	{
	}

	public bool Contains(long id)
	{
		return Contains(new EvaluationHistory.Key(id));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return EvaluationHistory.GetObjectFactory();
	}
}
