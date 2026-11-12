using System;
using System.IO;
using SharpCompress.Common.SevenZip;
using SharpCompress.Compressors.LZMA.Utilites;
using SharpCompress.IO;

namespace SharpCompress.Compressors.LZMA;

internal static class DecoderStreamHelper
{
	private static int FindCoderIndexForOutStreamIndex(CFolder folderInfo, int outStreamIndex)
	{
		for (int i = 0; i < folderInfo._coders.Count; i++)
		{
			CCoderInfo cCoderInfo = folderInfo._coders[i];
			outStreamIndex -= cCoderInfo._numOutStreams;
			if (outStreamIndex < 0)
			{
				return i;
			}
		}
		throw new InvalidOperationException("Could not link output stream to coder.");
	}

	private static void FindPrimaryOutStreamIndex(CFolder folderInfo, out int primaryCoderIndex, out int primaryOutStreamIndex)
	{
		bool flag = false;
		primaryCoderIndex = -1;
		primaryOutStreamIndex = -1;
		int num = 0;
		for (int i = 0; i < folderInfo._coders.Count; i++)
		{
			int num2 = 0;
			while (num2 < folderInfo._coders[i]._numOutStreams)
			{
				if (folderInfo.FindBindPairForOutStream(num) < 0)
				{
					if (flag)
					{
						throw new NotSupportedException("Multiple output streams.");
					}
					flag = true;
					primaryCoderIndex = i;
					primaryOutStreamIndex = num;
				}
				num2++;
				num++;
			}
		}
		if (!flag)
		{
			throw new NotSupportedException("No output stream.");
		}
	}

	private static Stream CreateDecoderStream(Stream[] packStreams, long[] packSizes, Stream[] outStreams, CFolder folderInfo, int coderIndex, IPasswordProvider pass)
	{
		CCoderInfo cCoderInfo = folderInfo._coders[coderIndex];
		if (cCoderInfo._numOutStreams != 1)
		{
			throw new NotSupportedException("Multiple output streams are not supported.");
		}
		int num = 0;
		for (int i = 0; i < coderIndex; i++)
		{
			num += folderInfo._coders[i]._numInStreams;
		}
		int num2 = 0;
		for (int j = 0; j < coderIndex; j++)
		{
			num2 += folderInfo._coders[j]._numOutStreams;
		}
		Stream[] array = new Stream[cCoderInfo._numInStreams];
		int num3 = 0;
		while (num3 < array.Length)
		{
			int num4 = folderInfo.FindBindPairForInStream(num);
			if (num4 >= 0)
			{
				int outIndex = folderInfo._bindPairs[num4]._outIndex;
				if (outStreams[outIndex] != null)
				{
					throw new NotSupportedException("Overlapping stream bindings are not supported.");
				}
				int coderIndex2 = FindCoderIndexForOutStreamIndex(folderInfo, outIndex);
				array[num3] = CreateDecoderStream(packStreams, packSizes, outStreams, folderInfo, coderIndex2, pass);
				if (outStreams[outIndex] != null)
				{
					throw new NotSupportedException("Overlapping stream bindings are not supported.");
				}
				outStreams[outIndex] = array[num3];
			}
			else
			{
				int num5 = folderInfo.FindPackStreamArrayIndex(num);
				if (num5 < 0)
				{
					throw new NotSupportedException("Could not find input stream binding.");
				}
				array[num3] = packStreams[num5];
			}
			num3++;
			num++;
		}
		long limit = folderInfo._unpackSizes[num2];
		return DecoderRegistry.CreateDecoderStream(cCoderInfo._methodId, array, cCoderInfo._props, pass, limit);
	}

	internal static Stream CreateDecoderStream(Stream inStream, long startPos, long[] packSizes, CFolder folderInfo, IPasswordProvider pass)
	{
		if (!folderInfo.CheckStructure())
		{
			throw new NotSupportedException("Unsupported stream binding structure.");
		}
		Stream[] array = new Stream[folderInfo._packStreams.Count];
		for (int i = 0; i < folderInfo._packStreams.Count; i++)
		{
			array[i] = new BufferedSubStream(inStream, startPos, packSizes[i]);
			startPos += packSizes[i];
		}
		Stream[] outStreams = new Stream[folderInfo._unpackSizes.Count];
		FindPrimaryOutStreamIndex(folderInfo, out var primaryCoderIndex, out var _);
		return CreateDecoderStream(array, packSizes, outStreams, folderInfo, primaryCoderIndex, pass);
	}
}
