using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment;

internal struct CharacteristicFailure
{
	public CharacteristicCollection Subject;

	public CharacteristicCollection Reason;
}
