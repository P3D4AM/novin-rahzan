using System.IO;

namespace SharpCompress.Common;

public abstract class FilePart
{
	internal ArchiveEncoding ArchiveEncoding { get; }

	internal abstract string FilePartName { get; }

	internal bool Skipped { get; set; }

	protected FilePart(ArchiveEncoding archiveEncoding)
	{
		ArchiveEncoding = archiveEncoding;
	}

	internal abstract Stream GetCompressedStream();

	internal abstract Stream? GetRawStream();
}
