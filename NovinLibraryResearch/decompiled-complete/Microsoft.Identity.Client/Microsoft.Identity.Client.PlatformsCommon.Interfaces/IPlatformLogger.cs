namespace Microsoft.Identity.Client.PlatformsCommon.Interfaces;

internal interface IPlatformLogger
{
	void Error(string message);

	void Warning(string message);

	void Verbose(string message);

	void Information(string message);
}
