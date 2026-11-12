using System.IO;

namespace Serilog.Sinks.Http.Private.IO;

public class DirectoryService
{
	public virtual string[] GetFiles(string path, string searchPattern)
	{
		return Directory.GetFiles(path, searchPattern);
	}
}
