using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class DeflateCompressionProvider : ICompressionProvider
{
	public string Algorithm => "DEF";

	public CompressionLevel CompressionLevel { get; private set; }

	public DeflateCompressionProvider()
	{
	}

	public DeflateCompressionProvider(CompressionLevel compressionLevel)
	{
		CompressionLevel = compressionLevel;
	}

	public byte[] Decompress(byte[] value)
	{
		if (value == null)
		{
			throw LogHelper.LogArgumentNullException("value");
		}
		using MemoryStream stream = new MemoryStream(value);
		using DeflateStream stream2 = new DeflateStream(stream, CompressionMode.Decompress);
		using StreamReader streamReader = new StreamReader(stream2, Encoding.UTF8);
		return Encoding.UTF8.GetBytes(streamReader.ReadToEnd());
	}

	public byte[] Compress(byte[] value)
	{
		if (value == null)
		{
			throw LogHelper.LogArgumentNullException("value");
		}
		using MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream stream = new DeflateStream(memoryStream, CompressionLevel))
		{
			using StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
			streamWriter.Write(Encoding.UTF8.GetString(value));
		}
		return memoryStream.ToArray();
	}

	public bool IsSupportedAlgorithm(string algorithm)
	{
		return Algorithm.Equals(algorithm, StringComparison.Ordinal);
	}
}
