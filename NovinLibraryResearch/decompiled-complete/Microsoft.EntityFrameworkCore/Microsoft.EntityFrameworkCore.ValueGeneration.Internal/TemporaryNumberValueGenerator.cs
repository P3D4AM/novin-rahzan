namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public abstract class TemporaryNumberValueGenerator<TValue> : ValueGenerator<TValue>
{
	public override bool GeneratesTemporaryValues => true;
}
