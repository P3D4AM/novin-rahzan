namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

public interface IItemChange<in TTarget>
{
	void ApplyTo(TTarget target);
}
