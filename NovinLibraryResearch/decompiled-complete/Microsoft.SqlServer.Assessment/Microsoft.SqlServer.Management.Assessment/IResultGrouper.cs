using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment;

internal interface IResultGrouper
{
	void Consolidate(CharacteristicProviderSource providerSource, List<IAssessmentResult> results);
}
