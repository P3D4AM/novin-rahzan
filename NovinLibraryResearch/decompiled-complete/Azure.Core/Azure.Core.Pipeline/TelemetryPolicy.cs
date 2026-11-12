using System.Runtime.InteropServices;

namespace Azure.Core.Pipeline;

internal class TelemetryPolicy : HttpPipelineSynchronousPolicy
{
	private readonly string _header;

	public TelemetryPolicy(string componentName, string componentVersion, string? applicationId)
	{
		string text = "(" + RuntimeInformation.FrameworkDescription + "; " + RuntimeInformation.OSDescription + ")";
		if (applicationId != null)
		{
			_header = applicationId + " azsdk-net-" + componentName + "/" + componentVersion + " " + text;
		}
		else
		{
			_header = "azsdk-net-" + componentName + "/" + componentVersion + " " + text;
		}
	}

	public override void OnSendingRequest(HttpMessage message)
	{
		message.Request.Headers.Add(HttpHeader.Names.UserAgent, _header);
	}
}
