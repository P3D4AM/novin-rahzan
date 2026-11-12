using SharpCompress.Common;
using SharpCompress.Compressors.Deflate;

namespace SharpCompress.Writers.Zip;

public class ZipWriterOptions : WriterOptions
{
	public CompressionLevel DeflateCompressionLevel { get; set; } = CompressionLevel.Default;


	public string? ArchiveComment { get; set; }

	public bool UseZip64 { get; set; }

	public ZipWriterOptions(CompressionType compressionType)
		: base(compressionType)
	{
	}

	internal ZipWriterOptions(WriterOptions options)
		: base(options.CompressionType)
	{
		base.LeaveStreamOpen = options.LeaveStreamOpen;
		base.ArchiveEncoding = options.ArchiveEncoding;
		if (options is ZipWriterOptions zipWriterOptions)
		{
			UseZip64 = zipWriterOptions.UseZip64;
			DeflateCompressionLevel = zipWriterOptions.DeflateCompressionLevel;
			ArchiveComment = zipWriterOptions.ArchiveComment;
		}
	}
}
