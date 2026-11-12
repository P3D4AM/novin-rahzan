using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Probes;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

public interface ITool
{
	ICharacteristicCollection GetDependencies(RunSpace runSpace);

	Task<bool> IsAvailable(RunSpace runSpace);
}
