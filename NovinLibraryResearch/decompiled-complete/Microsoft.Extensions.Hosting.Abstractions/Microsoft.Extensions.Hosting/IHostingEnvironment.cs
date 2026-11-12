using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Hosting;

public interface IHostingEnvironment
{
	string EnvironmentName { get; set; }

	string ApplicationName { get; set; }

	string ContentRootPath { get; set; }

	IFileProvider ContentRootFileProvider { get; set; }
}
