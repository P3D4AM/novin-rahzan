using System;

namespace SharpCompress.Common;

public sealed class CompressedBytesReadEventArgs : EventArgs
{
	public long CompressedBytesRead { get; }

	public long CurrentFilePartCompressedBytesRead { get; }

	public CompressedBytesReadEventArgs(long compressedBytesRead, long currentFilePartCompressedBytesRead)
	{
		CompressedBytesRead = compressedBytesRead;
		CurrentFilePartCompressedBytesRead = currentFilePartCompressedBytesRead;
	}
}
