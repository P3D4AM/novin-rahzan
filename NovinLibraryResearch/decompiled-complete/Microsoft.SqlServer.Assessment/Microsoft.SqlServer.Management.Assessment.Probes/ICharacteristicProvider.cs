using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal interface ICharacteristicProvider
{
	Regex Pattern { get; }

	int Priority { get; }

	Task UpdateCharacteristics(RunSpace runSpace, string key, Dictionary<string, string> args, HashSet<string> specification);

	CharacteristicCollection GetDependencies(RunSpace runSpace, Dictionary<string, string> arguments, HashSet<string> specification);

	string GetDisplayName(string key, Dictionary<string, string> arguments, HashSet<string> specification);
}
