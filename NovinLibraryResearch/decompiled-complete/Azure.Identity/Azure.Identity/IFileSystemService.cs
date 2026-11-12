namespace Azure.Identity;

internal interface IFileSystemService
{
	bool FileExists(string path);

	string ReadAllText(string path);
}
