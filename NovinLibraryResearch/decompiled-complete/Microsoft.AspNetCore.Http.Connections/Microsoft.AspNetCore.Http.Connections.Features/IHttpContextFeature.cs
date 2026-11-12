namespace Microsoft.AspNetCore.Http.Connections.Features;

public interface IHttpContextFeature
{
	HttpContext HttpContext { get; set; }
}
