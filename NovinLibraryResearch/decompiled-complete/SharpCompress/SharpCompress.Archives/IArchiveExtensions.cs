using System.Linq;
using SharpCompress.Common;

namespace SharpCompress.Archives;

public static class IArchiveExtensions
{
	public static void WriteToDirectory(this IArchive archive, string destinationDirectory, ExtractionOptions? options = null)
	{
		foreach (IArchiveEntry item in archive.Entries.Where((IArchiveEntry x) => !x.IsDirectory))
		{
			item.WriteToDirectory(destinationDirectory, options);
		}
	}
}
