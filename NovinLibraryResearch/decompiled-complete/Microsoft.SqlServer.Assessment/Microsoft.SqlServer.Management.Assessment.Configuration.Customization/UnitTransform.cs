namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class UnitTransform<TValue> : ChangeTransform<TValue>
{
	public override TValue Apply(TValue original)
	{
		return original;
	}
}
