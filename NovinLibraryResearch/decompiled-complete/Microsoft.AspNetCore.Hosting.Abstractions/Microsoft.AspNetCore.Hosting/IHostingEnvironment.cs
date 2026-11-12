using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Hosting;

public interface IHostingEnvironment
{
	string EnvironmentName { get; set; }

	string ApplicationName { get; set; }

	string WebRootPath { get; set; }

	IFileProvider WebRootFileProvider { get; set; }

	string ContentRootPath { get; set; }

	IFileProvider ContentRootFileProvider { get; set; }
}
