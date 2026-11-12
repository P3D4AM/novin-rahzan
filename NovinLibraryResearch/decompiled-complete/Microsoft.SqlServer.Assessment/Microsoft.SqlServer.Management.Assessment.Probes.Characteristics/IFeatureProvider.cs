using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Probes.Characteristics;

internal interface IFeatureProvider
{
	string Key { get; }

	Task<bool> IsPresent(RunSpace runSpace);

	CharacteristicCollection GetDependencies();
}
