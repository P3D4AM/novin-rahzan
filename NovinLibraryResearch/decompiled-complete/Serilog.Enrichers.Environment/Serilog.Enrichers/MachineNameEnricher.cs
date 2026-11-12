using System;
using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers;

public class MachineNameEnricher : CachedPropertyEnricher
{
	public const string MachineNamePropertyName = "MachineName";

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
	{
		string machineName = Environment.MachineName;
		return propertyFactory.CreateProperty("MachineName", machineName);
	}
}
