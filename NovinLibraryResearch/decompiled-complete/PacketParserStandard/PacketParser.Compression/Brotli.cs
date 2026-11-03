using System;
using System.Threading.Tasks;
using Brotli;
using PacketParser.Entities;

namespace PacketParser.Compression;

public class Brotli
{
	public static async Task<byte[]> Decompress(byte[] data)
	{
		try
		{
			if (data == null)
			{
				return null;
			}
			WebErrorLog.Instance.Trace("Decompressing Data......", "", "Decompress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\Brotli.cs", 22);
			return BrotliExtensions.DecompressFromBrotli(data);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Warning(ex, "Decompression Error", "Decompress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\Brotli.cs", 27);
			return data;
		}
		finally
		{
			WebErrorLog.Instance.Trace("Decompression finished", "", "Decompress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\Brotli.cs", 30);
		}
	}

	public static async Task<byte[]> Compress(byte[] data, uint level = 6u)
	{
		if (level > 11)
		{
			throw new ArgumentException("Brotli level is from 1 through 11");
		}
		try
		{
			if (data == null)
			{
				return null;
			}
			WebErrorLog.Instance.Trace("Compressing Data.....", "", "Compress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\Brotli.cs", 48);
			return BrotliExtensions.CompressToBrotli(data, level, 22u);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Warning(ex, "Compression error", "Compress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\Brotli.cs", 53);
			return null;
		}
		finally
		{
			WebErrorLog.Instance.Trace("Compression finished", "", "Compress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Compression\\Brotli.cs", 58);
		}
	}
}
