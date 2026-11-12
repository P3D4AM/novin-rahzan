namespace Microsoft.AspNetCore.Http.Connections.Features;

public interface IHttpTransportFeature
{
	HttpTransportType TransportType { get; }
}
