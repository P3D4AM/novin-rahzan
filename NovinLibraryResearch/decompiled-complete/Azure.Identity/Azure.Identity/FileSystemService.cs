using System.IO;

namespace Azure.Identity;

internal class FileSystemService : IFileSystemService
{
	public static IFileSystemService Default { get; } = new FileSystemService();


	private FileSystemService()
	{
	}

	public bool FileExists(string path)
	{
		return File.Exists(path);
	}

	public string ReadAllText(string path)
	{
		return File.ReadAllText(path);
	}
}
