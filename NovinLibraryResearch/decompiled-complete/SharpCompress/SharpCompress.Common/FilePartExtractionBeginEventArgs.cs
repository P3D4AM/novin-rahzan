using System;

namespace SharpCompress.Common;

public sealed class FilePartExtractionBeginEventArgs : EventArgs
{
	public string Name { get; }

	public long Size { get; }

	public long CompressedSize { get; }

	public FilePartExtractionBeginEventArgs(string name, long size, long compressedSize)
	{
		Name = name;
		Size = size;
		CompressedSize = compressedSize;
	}
}
