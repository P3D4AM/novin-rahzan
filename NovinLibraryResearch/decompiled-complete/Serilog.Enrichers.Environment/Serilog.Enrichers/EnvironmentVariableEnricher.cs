using System;
using System.Runtime.CompilerServices;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers;

public class EnvironmentVariableEnricher : CachedPropertyEnricher
{
	private readonly string _envVarName;

	public string EnvironmentVariablePropertyName { get; }

	public EnvironmentVariableEnricher(string envVarName, string propertyName)
	{
		_envVarName = envVarName;
		EnvironmentVariablePropertyName = propertyName ?? envVarName;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
	{
		string environmentVariable = Environment.GetEnvironmentVariable(_envVarName);
		return propertyFactory.CreateProperty(EnvironmentVariablePropertyName, environmentVariable);
	}
}
