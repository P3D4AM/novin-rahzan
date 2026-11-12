namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal interface IChangeAccessor<in TTarget, TValue>
{
	void SetValue(TTarget target, TValue newValue);

	TValue GetValue(TTarget target);
}
