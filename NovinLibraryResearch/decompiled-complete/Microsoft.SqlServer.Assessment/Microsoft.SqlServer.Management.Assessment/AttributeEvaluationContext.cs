using System;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment;

internal class AttributeEvaluationContext : EvaluationContext<object>
{
	public CharacteristicCollection KnownCharacteristics { get; } = new CharacteristicCollection();


	public AttributeEvaluationContext(string name, IEvaluationContext baseContext)
		: base(name, baseContext)
	{
	}

	protected override bool TryGetDataCore(string column, out object value)
	{
		throw new NotImplementedException();
	}
}
