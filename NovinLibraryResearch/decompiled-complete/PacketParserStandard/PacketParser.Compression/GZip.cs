using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.Compression;

public static class GZip
{
	public static async Task<byte[]> Decompress(byte[] data)
	{
		try
		{
			if (data == null)
			{
				return null;
			}
			using MemoryStream compressedStream = new MemoryStream(data);
			using GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
			using MemoryStream resultStream = new MemoryStream();
			await zipStream.CopyToAsync(resultStream);
			return resultStream.ToArray();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "Decompression Error", "Decompress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\GZip.cs", 37);
			throw;
		}
	}

	public static async Task<byte[]> Compress(byte[] data)
	{
		try
		{
			if (data == null)
			{
				return null;
			}
			using MemoryStream compressedStream = new MemoryStream();
			using (GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
			{
				await zipStream.WriteAsync(data, 0, data.Length);
			}
			return compressedStream.ToArray();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "Compression error", "Compress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\GZip.cs", 59);
			throw;
		}
	}
}
