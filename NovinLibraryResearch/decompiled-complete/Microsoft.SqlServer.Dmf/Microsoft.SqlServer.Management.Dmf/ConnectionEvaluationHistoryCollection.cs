using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class ConnectionEvaluationHistoryCollection : SfcDictionaryCollection<ConnectionEvaluationHistory, ConnectionEvaluationHistory.Key, EvaluationHistory>
{
	public ConnectionEvaluationHistory this[int id] => base[new ConnectionEvaluationHistory.Key(id)];

	public ConnectionEvaluationHistoryCollection(EvaluationHistory parent)
		: base(parent)
	{
	}

	public bool Contains(int id)
	{
		return Contains(new ConnectionEvaluationHistory.Key(id));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return ConnectionEvaluationHistory.GetObjectFactory();
	}
}
