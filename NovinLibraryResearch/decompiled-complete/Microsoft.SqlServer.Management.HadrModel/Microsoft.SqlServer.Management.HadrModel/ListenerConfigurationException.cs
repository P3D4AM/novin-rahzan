namespace Microsoft.SqlServer.Management.HadrModel;

public class ListenerConfigurationException : HadrValidationErrorException
{
	public ListenerConfigurationException()
		: base(Resource.ListenerConfigurationWarning)
	{
	}
}
