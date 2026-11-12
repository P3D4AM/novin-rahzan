using System;
using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers;

public class EnvironmentNameEnricher : CachedPropertyEnricher
{
	public const string EnvironmentNamePropertyName = "EnvironmentName";

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
	{
		string value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		if (string.IsNullOrWhiteSpace(value))
		{
			value = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			value = "Production";
		}
		return propertyFactory.CreateProperty("EnvironmentName", value);
	}
}
