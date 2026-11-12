namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

public abstract class InPlaceTransform<TTarget>
{
	public abstract void ApplyTo(TTarget target);
}
