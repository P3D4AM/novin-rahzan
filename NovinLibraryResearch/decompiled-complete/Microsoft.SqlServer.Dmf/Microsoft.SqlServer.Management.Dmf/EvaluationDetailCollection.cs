using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class EvaluationDetailCollection : SfcDictionaryCollection<EvaluationDetail, EvaluationDetail.Key, ConnectionEvaluationHistory>
{
	public EvaluationDetail this[int id] => base[new EvaluationDetail.Key(id)];

	public EvaluationDetailCollection(ConnectionEvaluationHistory parent)
		: base(parent)
	{
	}

	public bool Contains(int id)
	{
		return Contains(new EvaluationDetail.Key(id));
	}

	protected override SfcObjectFactory GetElementFactoryImpl()
	{
		return EvaluationDetail.GetObjectFactory();
	}
}
