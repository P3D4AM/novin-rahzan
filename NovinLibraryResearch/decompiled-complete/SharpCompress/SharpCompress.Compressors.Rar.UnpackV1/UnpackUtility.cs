using System;
using SharpCompress.Compressors.Rar.UnpackV1.Decode;
using SharpCompress.Compressors.Rar.VM;

namespace SharpCompress.Compressors.Rar.UnpackV1;

internal static class UnpackUtility
{
	internal static uint DecodeNumber(this BitInput input, SharpCompress.Compressors.Rar.UnpackV1.Decode.Decode dec)
	{
		return (uint)input.decodeNumber(dec);
	}

	internal static int decodeNumber(this BitInput input, SharpCompress.Compressors.Rar.UnpackV1.Decode.Decode dec)
	{
		long num = input.GetBits() & 0xFFFE;
		int[] decodeLen = dec.DecodeLen;
		int num2 = ((num < decodeLen[8]) ? ((num < decodeLen[4]) ? ((num < decodeLen[2]) ? ((num < decodeLen[1]) ? 1 : 2) : ((num >= decodeLen[3]) ? 4 : 3)) : ((num < decodeLen[6]) ? ((num >= decodeLen[5]) ? 6 : 5) : ((num >= decodeLen[7]) ? 8 : 7))) : ((num < decodeLen[12]) ? ((num < decodeLen[10]) ? ((num >= decodeLen[9]) ? 10 : 9) : ((num >= decodeLen[11]) ? 12 : 11)) : ((num >= decodeLen[14]) ? 15 : ((num >= decodeLen[13]) ? 14 : 13))));
		input.AddBits(num2);
		int num3 = dec.DecodePos[num2] + Utility.URShift((int)num - decodeLen[num2 - 1], 16 - num2);
		if (num3 >= dec.MaxNum)
		{
			num3 = 0;
		}
		return dec.DecodeNum[num3];
	}

	internal static void makeDecodeTables(byte[] lenTab, int offset, SharpCompress.Compressors.Rar.UnpackV1.Decode.Decode dec, int size)
	{
		Span<int> span = stackalloc int[16];
		Span<int> span2 = stackalloc int[16];
		new Span<int>(dec.DecodeNum).Clear();
		for (int i = 0; i < size; i++)
		{
			span[lenTab[offset + i] & 0xF]++;
		}
		span[0] = 0;
		span2[0] = 0;
		dec.DecodePos[0] = 0;
		dec.DecodeLen[0] = 0;
		long num = 0L;
		for (int i = 1; i < 16; i++)
		{
			num = 2 * (num + span[i]);
			long num2 = num << 15 - i;
			if (num2 > 65535)
			{
				num2 = 65535L;
			}
			dec.DecodeLen[i] = (int)num2;
			span2[i] = (dec.DecodePos[i] = dec.DecodePos[i - 1] + span[i - 1]);
		}
		for (int i = 0; i < size; i++)
		{
			if (lenTab[offset + i] != 0)
			{
				dec.DecodeNum[span2[lenTab[offset + i] & 0xF]++] = i;
			}
		}
		dec.MaxNum = size;
	}
}
