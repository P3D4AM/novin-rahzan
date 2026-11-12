namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public class TemporaryGuidValueGenerator : GuidValueGenerator
{
	public override bool GeneratesTemporaryValues => true;
}
