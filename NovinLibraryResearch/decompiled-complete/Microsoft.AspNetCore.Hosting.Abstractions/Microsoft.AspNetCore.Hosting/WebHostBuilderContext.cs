using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting;

public class WebHostBuilderContext
{
	public IHostingEnvironment HostingEnvironment { get; set; }

	public IConfiguration Configuration { get; set; }
}
