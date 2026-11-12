using SharpCompress.Common;

namespace SharpCompress.Writers.Tar;

public class TarWriterOptions : WriterOptions
{
	public bool FinalizeArchiveOnClose { get; }

	public TarWriterOptions(CompressionType compressionType, bool finalizeArchiveOnClose)
		: base(compressionType)
	{
		FinalizeArchiveOnClose = finalizeArchiveOnClose;
	}

	internal TarWriterOptions(WriterOptions options)
		: this(options.CompressionType, finalizeArchiveOnClose: true)
	{
		base.ArchiveEncoding = options.ArchiveEncoding;
	}
}
