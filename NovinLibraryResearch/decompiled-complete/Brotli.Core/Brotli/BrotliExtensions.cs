using System;
using System.IO;
using System.IO.Compression;

namespace Brotli;

public static class BrotliExtensions
{
	public static byte[] CompressToBrotli(this byte[] rawData, uint quality = 5u, uint window = 22u)
	{
		if (rawData == null)
		{
			throw new ArgumentNullException("rawData");
		}
		using MemoryStream inStream = new MemoryStream(rawData);
		return inStream.CompressToBrotli(quality, window);
	}

	public static byte[] CompressToBrotli(this Stream inStream, uint quality = 5u, uint window = 22u)
	{
		using MemoryStream memoryStream = new MemoryStream();
		inStream.CompressToBrotli(memoryStream, quality, window);
		return memoryStream.ToArray();
	}

	public static void CompressToBrotli(this Stream inStream, Stream destStream, uint quality = 5u, uint window = 22u)
	{
		using BrotliStream brotliStream = new BrotliStream(destStream, CompressionMode.Compress);
		brotliStream.SetQuality(quality);
		brotliStream.SetWindow(window);
		inStream.CopyTo(brotliStream);
		brotliStream.Close();
	}

	public static byte[] DecompressFromBrotli(this byte[] rawData)
	{
		if (rawData == null)
		{
			throw new ArgumentNullException("rawData");
		}
		using MemoryStream inStream = new MemoryStream(rawData);
		return inStream.DecompressFromBrotli();
	}

	public static byte[] DecompressFromBrotli(this Stream inStream)
	{
		using MemoryStream memoryStream = new MemoryStream();
		inStream.DecompressFromBrotli(memoryStream);
		return memoryStream.ToArray();
	}

	public static void DecompressFromBrotli(this Stream inStream, Stream destStream)
	{
		using BrotliStream brotliStream = new BrotliStream(inStream, CompressionMode.Decompress);
		brotliStream.CopyTo(destStream);
		destStream.Flush();
	}
}
