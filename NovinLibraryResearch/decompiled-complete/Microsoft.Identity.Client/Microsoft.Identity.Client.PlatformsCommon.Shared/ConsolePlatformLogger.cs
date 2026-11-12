using System;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;

namespace Microsoft.Identity.Client.PlatformsCommon.Shared;

internal class ConsolePlatformLogger : IPlatformLogger
{
	public void Error(string message)
	{
		Console.WriteLine(message);
	}

	public void Warning(string message)
	{
		Console.WriteLine(message);
	}

	public void Verbose(string message)
	{
		Console.WriteLine(message);
	}

	public void Information(string message)
	{
		Console.WriteLine(message);
	}
}
