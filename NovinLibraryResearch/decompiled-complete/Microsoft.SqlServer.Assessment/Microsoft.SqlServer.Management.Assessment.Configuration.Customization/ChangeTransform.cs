namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal abstract class ChangeTransform<TValue>
{
	public abstract TValue Apply(TValue original);
}
