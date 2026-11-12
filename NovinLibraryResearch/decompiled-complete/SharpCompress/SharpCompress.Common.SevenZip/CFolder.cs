using System;
using System.Collections.Generic;
using SharpCompress.Compressors.LZMA;

namespace SharpCompress.Common.SevenZip;

internal class CFolder
{
	internal List<CCoderInfo> _coders = new List<CCoderInfo>();

	internal List<CBindPair> _bindPairs = new List<CBindPair>();

	internal List<int> _packStreams = new List<int>();

	internal int _firstPackStreamId;

	internal List<long> _unpackSizes = new List<long>();

	internal uint? _unpackCrc;

	internal bool UnpackCrcDefined => _unpackCrc.HasValue;

	public long GetUnpackSize()
	{
		if (_unpackSizes.Count == 0)
		{
			return 0L;
		}
		for (int num = _unpackSizes.Count - 1; num >= 0; num--)
		{
			if (FindBindPairForOutStream(num) < 0)
			{
				return _unpackSizes[num];
			}
		}
		throw new Exception();
	}

	public int GetNumOutStreams()
	{
		int num = 0;
		for (int i = 0; i < _coders.Count; i++)
		{
			num += _coders[i]._numOutStreams;
		}
		return num;
	}

	public int FindBindPairForInStream(int inStreamIndex)
	{
		for (int i = 0; i < _bindPairs.Count; i++)
		{
			if (_bindPairs[i]._inIndex == inStreamIndex)
			{
				return i;
			}
		}
		return -1;
	}

	public int FindBindPairForOutStream(int outStreamIndex)
	{
		for (int i = 0; i < _bindPairs.Count; i++)
		{
			if (_bindPairs[i]._outIndex == outStreamIndex)
			{
				return i;
			}
		}
		return -1;
	}

	public int FindPackStreamArrayIndex(int inStreamIndex)
	{
		for (int i = 0; i < _packStreams.Count; i++)
		{
			if (_packStreams[i] == inStreamIndex)
			{
				return i;
			}
		}
		return -1;
	}

	public bool IsEncrypted()
	{
		for (int num = _coders.Count - 1; num >= 0; num--)
		{
			if (_coders[num]._methodId == CMethodId.K_AES)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckStructure()
	{
		if (_coders.Count > 32 || _bindPairs.Count > 32)
		{
			return false;
		}
		BitVector bitVector = new BitVector(_bindPairs.Count + _packStreams.Count);
		for (int i = 0; i < _bindPairs.Count; i++)
		{
			if (bitVector.GetAndSet(_bindPairs[i]._inIndex))
			{
				return false;
			}
		}
		for (int j = 0; j < _packStreams.Count; j++)
		{
			if (bitVector.GetAndSet(_packStreams[j]))
			{
				return false;
			}
		}
		BitVector bitVector2 = new BitVector(_unpackSizes.Count);
		for (int k = 0; k < _bindPairs.Count; k++)
		{
			if (bitVector2.GetAndSet(_bindPairs[k]._outIndex))
			{
				return false;
			}
		}
		uint[] array = new uint[32];
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int l = 0; l < _coders.Count; l++)
		{
			CCoderInfo cCoderInfo = _coders[l];
			for (int m = 0; m < cCoderInfo._numInStreams; m++)
			{
				list.Add(l);
			}
			for (int n = 0; n < cCoderInfo._numOutStreams; n++)
			{
				list2.Add(l);
			}
		}
		for (int num = 0; num < _bindPairs.Count; num++)
		{
			CBindPair cBindPair = _bindPairs[num];
			array[list[cBindPair._inIndex]] |= (uint)(1 << list2[cBindPair._outIndex]);
		}
		for (int num2 = 0; num2 < 32; num2++)
		{
			for (int num3 = 0; num3 < 32; num3++)
			{
				if (((uint)(1 << num3) & array[num2]) != 0)
				{
					array[num2] |= array[num3];
				}
			}
		}
		for (int num4 = 0; num4 < 32; num4++)
		{
			if (((uint)(1 << num4) & array[num4]) != 0)
			{
				return false;
			}
		}
		return true;
	}
}
