using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Configuration;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

public interface IProbe
{
	SqlObjectLocatorPattern Target { get; set; }

	string FamilyName { get; set; }

	IDataMorph Morph { get; set; }

	ICharacteristicCollection Subject { get; }

	ICharacteristicCollection Requirements { get; }

	IEnumerable<Type> ToolTypes { get; }

	Task<IDataView> GetDataAsync(RunSpace runSpace, IReadOnlyDictionary<string, object> parameters);
}
