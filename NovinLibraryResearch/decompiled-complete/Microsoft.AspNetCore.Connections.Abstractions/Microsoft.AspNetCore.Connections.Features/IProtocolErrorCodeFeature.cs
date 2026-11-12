namespace Microsoft.AspNetCore.Connections.Features;

public interface IProtocolErrorCodeFeature
{
	long Error { get; set; }
}
