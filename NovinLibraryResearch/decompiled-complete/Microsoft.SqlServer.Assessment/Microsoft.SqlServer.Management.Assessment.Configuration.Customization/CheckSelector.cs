using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal abstract class CheckSelector
{
	public abstract bool IsMatch(Check check);
}
