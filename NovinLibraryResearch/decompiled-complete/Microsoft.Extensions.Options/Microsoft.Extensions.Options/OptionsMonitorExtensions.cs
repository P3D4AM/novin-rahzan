using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Options;

public static class OptionsMonitorExtensions
{
	public static IDisposable? OnChange<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions>(this IOptionsMonitor<TOptions> monitor, Action<TOptions> listener)
	{
		Action<TOptions> listener2 = listener;
		return monitor.OnChange(delegate(TOptions o, string _)
		{
			listener2(o);
		});
	}
}
