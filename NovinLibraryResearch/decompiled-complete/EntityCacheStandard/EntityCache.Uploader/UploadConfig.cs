using ClientHub;

namespace EntityCache.Uploader;

public class UploadConfig
{
	public Buffer BufferClient { get; set; }

	public int MaxChunkKb { get; set; }

	public int MinChunkKb { get; set; }

	public bool RetryOnFault { get; set; }

	public string TempPath { get; set; }
}
