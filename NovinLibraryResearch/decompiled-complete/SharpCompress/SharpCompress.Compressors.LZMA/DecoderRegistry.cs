using System;
using System.IO;
using System.Linq;
using SharpCompress.Common.SevenZip;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Compressors.Deflate;
using SharpCompress.Compressors.Filters;
using SharpCompress.Compressors.LZMA.Utilites;
using SharpCompress.Compressors.PPMd;

namespace SharpCompress.Compressors.LZMA;

internal static class DecoderRegistry
{
	private const uint K_COPY = 0u;

	private const uint K_DELTA = 3u;

	private const uint K_LZMA2 = 33u;

	private const uint K_LZMA = 196865u;

	private const uint K_PPMD = 197633u;

	private const uint K_BCJ = 50528515u;

	private const uint K_BCJ2 = 50528539u;

	private const uint K_DEFLATE = 262408u;

	private const uint K_B_ZIP2 = 262658u;

	internal static Stream CreateDecoderStream(CMethodId id, Stream[] inStreams, byte[] info, IPasswordProvider pass, long limit)
	{
		switch (id._id)
		{
		case 0uL:
			if (info != null)
			{
				throw new NotSupportedException();
			}
			return inStreams.Single();
		case 33uL:
		case 196865uL:
			return new LzmaStream(info, inStreams.Single(), -1L, limit);
		case 116459265uL:
			return new AesDecoderStream(inStreams.Single(), info, pass, limit);
		case 50528515uL:
			return new BCJFilter(isEncoder: false, inStreams.Single());
		case 50528539uL:
			return new Bcj2DecoderStream(inStreams, info, limit);
		case 262658uL:
			return new BZip2Stream(inStreams.Single(), CompressionMode.Decompress, decompressConcatenated: true);
		case 197633uL:
			return new PpmdStream(new PpmdProperties(info), inStreams.Single(), compress: false);
		case 262408uL:
			return new DeflateStream(inStreams.Single(), CompressionMode.Decompress);
		default:
			throw new NotSupportedException();
		}
	}
}
