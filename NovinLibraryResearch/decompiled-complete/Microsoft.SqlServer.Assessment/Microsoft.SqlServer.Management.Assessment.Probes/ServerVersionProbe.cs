using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Probes;

internal class ServerVersionProbe : Probe
{
	public override IEnumerable<Type> ToolTypes => Enumerable.Empty<Type>();

	protected override Task<IDataView> GetDataImpl(RunSpace runSpace, IReadOnlyDictionary<string, object> parameters)
	{
		Measurement measurement = new Measurement
		{
			{
				"ProductVersion",
				runSpace.Target.Version
			},
			{
				"ServerMajorVersion",
				runSpace.Target.Version.Major
			}
		};
		return Task.FromResult((IDataView)new MeasurementCollection(measurement));
	}
}
