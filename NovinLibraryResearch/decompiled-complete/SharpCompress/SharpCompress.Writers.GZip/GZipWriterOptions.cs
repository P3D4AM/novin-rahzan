using SharpCompress.Common;
using SharpCompress.Compressors.Deflate;

namespace SharpCompress.Writers.GZip;

public class GZipWriterOptions : WriterOptions
{
	public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Default;


	public GZipWriterOptions()
		: base(CompressionType.GZip)
	{
	}

	internal GZipWriterOptions(WriterOptions options)
		: base(options.CompressionType)
	{
		base.LeaveStreamOpen = options.LeaveStreamOpen;
		base.ArchiveEncoding = options.ArchiveEncoding;
		if (options is GZipWriterOptions gZipWriterOptions)
		{
			CompressionLevel = gZipWriterOptions.CompressionLevel;
		}
	}
}
