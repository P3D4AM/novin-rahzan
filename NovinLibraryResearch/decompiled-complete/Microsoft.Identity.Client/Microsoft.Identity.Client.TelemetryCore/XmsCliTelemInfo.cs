namespace Microsoft.Identity.Client.TelemetryCore;

internal class XmsCliTelemInfo
{
	public string Version { get; set; }

	public string ServerErrorCode { get; set; }

	public string ServerSubErrorCode { get; set; }

	public string TokenAge { get; set; }

	public string SpeInfo { get; set; }
}
