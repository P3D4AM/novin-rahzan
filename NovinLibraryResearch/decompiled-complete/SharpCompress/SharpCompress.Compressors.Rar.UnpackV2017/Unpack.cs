using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Common.Rar.Headers;

namespace SharpCompress.Compressors.Rar.UnpackV2017;

internal sealed class Unpack : BitInput, IRarUnpack
{
	internal static class Unpack15Local
	{
		public static readonly uint[] ShortLen1 = new uint[16]
		{
			1u, 3u, 4u, 4u, 5u, 6u, 7u, 8u, 8u, 4u,
			4u, 5u, 6u, 6u, 4u, 0u
		};

		public static readonly uint[] ShortXor1 = new uint[15]
		{
			0u, 160u, 208u, 224u, 240u, 248u, 252u, 254u, 255u, 192u,
			128u, 144u, 152u, 156u, 176u
		};

		public static readonly uint[] ShortLen2 = new uint[16]
		{
			2u, 3u, 3u, 3u, 4u, 4u, 5u, 6u, 6u, 4u,
			4u, 5u, 6u, 6u, 4u, 0u
		};

		public static readonly uint[] ShortXor2 = new uint[15]
		{
			0u, 64u, 96u, 160u, 208u, 224u, 240u, 248u, 252u, 192u,
			128u, 144u, 152u, 156u, 176u
		};
	}

	internal static class Unpack20Local
	{
		public static readonly byte[] LDecode = new byte[28]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 10,
			12, 14, 16, 20, 24, 28, 32, 40, 48, 56,
			64, 80, 96, 112, 128, 160, 192, 224
		};

		public static readonly byte[] LBits = new byte[28]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
			1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
			4, 4, 4, 4, 5, 5, 5, 5
		};

		public static readonly uint[] DDecode = new uint[48]
		{
			0u, 1u, 2u, 3u, 4u, 6u, 8u, 12u, 16u, 24u,
			32u, 48u, 64u, 96u, 128u, 192u, 256u, 384u, 512u, 768u,
			1024u, 1536u, 2048u, 3072u, 4096u, 6144u, 8192u, 12288u, 16384u, 24576u,
			32768u, 49152u, 65536u, 98304u, 131072u, 196608u, 262144u, 327680u, 393216u, 458752u,
			524288u, 589824u, 655360u, 720896u, 786432u, 851968u, 917504u, 983040u
		};

		public static readonly byte[] DBits = new byte[48]
		{
			0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
			4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
			9, 9, 10, 10, 11, 11, 12, 12, 13, 13,
			14, 14, 15, 15, 16, 16, 16, 16, 16, 16,
			16, 16, 16, 16, 16, 16, 16, 16
		};

		public static readonly byte[] SDDecode = new byte[8] { 0, 4, 8, 16, 32, 64, 128, 192 };

		public static readonly byte[] SDBits = new byte[8] { 2, 2, 3, 4, 5, 6, 6, 6 };
	}

	private FileHeader fileHeader;

	private Stream readStream;

	private Stream writeStream;

	private const int STARTL1 = 2;

	private static readonly uint[] DecL1 = new uint[11]
	{
		32768u, 40960u, 49152u, 53248u, 57344u, 59904u, 60928u, 61440u, 61952u, 61952u,
		65535u
	};

	private static readonly uint[] PosL1 = new uint[13]
	{
		0u, 0u, 0u, 2u, 3u, 5u, 7u, 11u, 16u, 20u,
		24u, 32u, 32u
	};

	private const int STARTL2 = 3;

	private static readonly uint[] DecL2 = new uint[10] { 40960u, 49152u, 53248u, 57344u, 59904u, 60928u, 61440u, 61952u, 62016u, 65535u };

	private static readonly uint[] PosL2 = new uint[13]
	{
		0u, 0u, 0u, 0u, 5u, 7u, 9u, 13u, 18u, 22u,
		26u, 34u, 36u
	};

	private const int STARTHF0 = 4;

	private static readonly uint[] DecHf0 = new uint[9] { 32768u, 49152u, 57344u, 61952u, 61952u, 61952u, 61952u, 61952u, 65535u };

	private static readonly uint[] PosHf0 = new uint[13]
	{
		0u, 0u, 0u, 0u, 0u, 8u, 16u, 24u, 33u, 33u,
		33u, 33u, 33u
	};

	private const int STARTHF1 = 5;

	private static readonly uint[] DecHf1 = new uint[8] { 8192u, 49152u, 57344u, 61440u, 61952u, 61952u, 63456u, 65535u };

	private static readonly uint[] PosHf1 = new uint[13]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 4u, 44u, 60u, 76u,
		80u, 80u, 127u
	};

	private const int STARTHF2 = 5;

	private static readonly uint[] DecHf2 = new uint[8] { 4096u, 9216u, 32768u, 49152u, 64000u, 65535u, 65535u, 65535u };

	private static readonly uint[] PosHf2 = new uint[13]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 2u, 7u, 53u, 117u,
		233u, 0u, 0u
	};

	private const int STARTHF3 = 6;

	private static readonly uint[] DecHf3 = new uint[7] { 2048u, 9216u, 60928u, 65152u, 65535u, 65535u, 65535u };

	private static readonly uint[] PosHf3 = new uint[13]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 2u, 16u, 218u,
		251u, 0u, 0u
	};

	private const int STARTHF4 = 8;

	private static readonly uint[] DecHf4 = new uint[6] { 65280u, 65535u, 65535u, 65535u, 65535u, 65535u };

	private static readonly uint[] PosHf4 = new uint[13]
	{
		0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 255u,
		0u, 0u, 0u
	};

	private byte[] FilterSrcMemory = Array.Empty<byte>();

	private byte[] FilterDstMemory = Array.Empty<byte>();

	private readonly List<UnpackFilter> Filters = new List<UnpackFilter>();

	private readonly uint[] OldDist = new uint[4];

	private uint OldDistPtr;

	private uint LastLength;

	private uint LastDist;

	private uint UnpPtr;

	private uint WrPtr;

	private int ReadTop;

	private int ReadBorder;

	private UnpackBlockHeader BlockHeader;

	private UnpackBlockTables BlockTables;

	private uint WriteBorder;

	private byte[] Window;

	private readonly FragmentedWindow FragWindow = new FragmentedWindow();

	private bool Fragmented;

	private long DestUnpSize;

	private bool UnpAllBuf;

	private bool UnpSomeRead;

	private long WrittenFileSize;

	private bool FileExtracted;

	private readonly ushort[] ChSet = new ushort[256];

	private readonly ushort[] ChSetA = new ushort[256];

	private readonly ushort[] ChSetB = new ushort[256];

	private readonly ushort[] ChSetC = new ushort[256];

	private readonly byte[] NToPl = new byte[256];

	private readonly byte[] NToPlB = new byte[256];

	private readonly byte[] NToPlC = new byte[256];

	private uint FlagBuf;

	private uint AvrPlc;

	private uint AvrPlcB;

	private uint AvrLn1;

	private uint AvrLn2;

	private uint AvrLn3;

	private int Buf60;

	private int NumHuf;

	private int StMode;

	private int LCount;

	private int FlagsCnt;

	private uint Nhfb;

	private uint Nlzb;

	private uint MaxDist3;

	private DecodeTable[] MD = new DecodeTable[4];

	private readonly byte[] UnpOldTable20 = new byte[1028];

	private bool UnpAudioBlock;

	private uint UnpChannels;

	private uint UnpCurChannel;

	private int UnpChannelDelta;

	private AudioVariables[] AudV = new AudioVariables[4];

	public const int BLOCK_LZ = 0;

	public const int BLOCK_PPM = 1;

	private int PrevLowDist;

	private int LowDistRepCount;

	private int PPMEscChar;

	private readonly byte[] UnpOldTable = new byte[404];

	private int UnpBlockType;

	private bool TablesRead2;

	private bool TablesRead3;

	private bool TablesRead5;

	private readonly BitInput VMCodeInp = new BitInput(AllocBuffer: true);

	private readonly List<UnpackFilter30> Filters30 = new List<UnpackFilter30>();

	private readonly List<UnpackFilter30> PrgStack = new List<UnpackFilter30>();

	private readonly List<int> OldFilterLengths = new List<int>();

	private int LastFilter;

	private uint MaxWinSize;

	private uint MaxWinMask;

	public bool Suspended { get; set; }

	public long DestSize => DestUnpSize;

	public int Char
	{
		get
		{
			if (InAddr > 32738)
			{
				UnpReadBuf();
			}
			return InBuf[InAddr++];
		}
	}

	public int PpmEscChar
	{
		get
		{
			return PPMEscChar;
		}
		set
		{
			PPMEscChar = value;
		}
	}

	private BitInput Inp => this;

	private void _UnpackCtor()
	{
		for (int i = 0; i < AudV.Length; i++)
		{
			AudV[i] = new AudioVariables();
		}
	}

	private int UnpIO_UnpRead(byte[] buf, int offset, int count)
	{
		return readStream.Read(buf, offset, count);
	}

	private void UnpIO_UnpWrite(byte[] buf, uint offset, uint count)
	{
		checked
		{
			writeStream.Write(buf, (int)offset, (int)count);
		}
	}

	public void DoUnpack(FileHeader fileHeader, Stream readStream, Stream writeStream)
	{
		DestUnpSize = fileHeader.UncompressedSize;
		this.fileHeader = fileHeader;
		this.readStream = readStream;
		this.writeStream = writeStream;
		if (!fileHeader.IsStored)
		{
			Init(fileHeader.WindowSize, fileHeader.IsSolid);
		}
		Suspended = false;
		DoUnpack();
	}

	public void DoUnpack()
	{
		if (fileHeader.IsStored)
		{
			UnstoreFile();
		}
		else
		{
			DoUnpack(fileHeader.CompressionAlgorithm, fileHeader.IsSolid);
		}
	}

	private void UnstoreFile()
	{
		byte[] array = new byte[65536];
		do
		{
			int num = readStream.Read(array, 0, (int)Math.Min(array.Length, DestUnpSize));
			if (num != 0)
			{
				writeStream.Write(array, 0, num);
				DestUnpSize -= num;
				continue;
			}
			break;
		}
		while (!Suspended);
	}

	public static byte[] EnsureCapacity(byte[] array, int length)
	{
		if (array.Length >= length)
		{
			return array;
		}
		return new byte[length];
	}

	private uint RawGet4(byte[] D, int offset)
	{
		return (uint)(D[offset] + (D[offset + 1] << 8) + (D[offset + 2] << 16) + (D[offset + 3] << 24));
	}

	private void RawPut4(uint Field, byte[] D, int offset)
	{
		D[offset] = (byte)Field;
		D[offset + 1] = (byte)(Field >> 8);
		D[offset + 2] = (byte)(Field >> 16);
		D[offset + 3] = (byte)(Field >> 24);
	}

	private void Unpack15(bool Solid)
	{
		UnpInitData(Solid);
		UnpInitData15(Solid);
		UnpReadBuf();
		if (!Solid)
		{
			InitHuff();
			UnpPtr = 0u;
		}
		else
		{
			UnpPtr = WrPtr;
		}
		DestUnpSize--;
		if (DestUnpSize >= 0)
		{
			GetFlagsBuf();
			FlagsCnt = 8;
		}
		while (DestUnpSize >= 0)
		{
			UnpPtr &= MaxWinMask;
			if (Inp.InAddr > ReadTop - 30 && !UnpReadBuf())
			{
				break;
			}
			if (((WrPtr - UnpPtr) & MaxWinMask) < 270 && WrPtr != UnpPtr)
			{
				UnpWriteBuf20();
			}
			if (StMode != 0)
			{
				HuffDecode();
				continue;
			}
			if (--FlagsCnt < 0)
			{
				GetFlagsBuf();
				FlagsCnt = 7;
			}
			if ((FlagBuf & 0x80u) != 0)
			{
				FlagBuf <<= 1;
				if (Nlzb > Nhfb)
				{
					LongLZ();
				}
				else
				{
					HuffDecode();
				}
				continue;
			}
			FlagBuf <<= 1;
			if (--FlagsCnt < 0)
			{
				GetFlagsBuf();
				FlagsCnt = 7;
			}
			if ((FlagBuf & 0x80u) != 0)
			{
				FlagBuf <<= 1;
				if (Nlzb > Nhfb)
				{
					HuffDecode();
				}
				else
				{
					LongLZ();
				}
			}
			else
			{
				FlagBuf <<= 1;
				ShortLZ();
			}
		}
		UnpWriteBuf20();
	}

	private uint GetShortLen1(uint pos)
	{
		if (pos != 1)
		{
			return Unpack15Local.ShortLen1[pos];
		}
		return (uint)(Buf60 + 3);
	}

	private uint GetShortLen2(uint pos)
	{
		if (pos != 3)
		{
			return Unpack15Local.ShortLen2[pos];
		}
		return (uint)(Buf60 + 3);
	}

	private void ShortLZ()
	{
		NumHuf = 0;
		uint num = Inp.fgetbits();
		if (LCount == 2)
		{
			Inp.faddbits(1u);
			if (num >= 32768)
			{
				CopyString15(LastDist, LastLength);
				return;
			}
			num <<= 1;
			LCount = 0;
		}
		num >>= 8;
		uint num2;
		if (AvrLn1 < 37)
		{
			for (num2 = 0u; ((num ^ Unpack15Local.ShortXor1[num2]) & ~(255 >> (int)GetShortLen1(num2))) != 0L; num2++)
			{
			}
			Inp.faddbits(GetShortLen1(num2));
		}
		else
		{
			for (num2 = 0u; ((num ^ Unpack15Local.ShortXor2[num2]) & ~(255 >> (int)GetShortLen2(num2))) != 0L; num2++)
			{
			}
			Inp.faddbits(GetShortLen2(num2));
		}
		uint num4;
		switch (num2)
		{
		case 9u:
			LCount++;
			CopyString15(LastDist, LastLength);
			return;
		case 14u:
			LCount = 0;
			num2 = DecodeNum(Inp.fgetbits(), 3u, DecL2, PosL2) + 5;
			num4 = (Inp.fgetbits() >> 1) | 0x8000u;
			Inp.faddbits(15u);
			LastLength = num2;
			LastDist = num4;
			CopyString15(num4, num2);
			return;
		case 0u:
		case 1u:
		case 2u:
		case 3u:
		case 4u:
		case 5u:
		case 6u:
		case 7u:
		case 8u:
		{
			LCount = 0;
			AvrLn1 += num2;
			AvrLn1 -= AvrLn1 >> 4;
			int num3 = (int)(DecodeNum(Inp.fgetbits(), 5u, DecHf2, PosHf2) & 0xFF);
			num4 = ChSetA[num3];
			if (--num3 != -1)
			{
				uint num5 = ChSetA[num3];
				ChSetA[num3 + 1] = (ushort)num5;
				ChSetA[num3] = (ushort)num4;
			}
			num2 += 2;
			num4 = (OldDist[OldDistPtr++] = num4 + 1);
			OldDistPtr &= 3u;
			LastLength = num2;
			LastDist = num4;
			CopyString15(num4, num2);
			return;
		}
		}
		LCount = 0;
		uint num6 = num2;
		num4 = OldDist[(OldDistPtr - (num2 - 9)) & 3];
		num2 = DecodeNum(Inp.fgetbits(), 2u, DecL1, PosL1) + 2;
		if (num2 == 257 && num6 == 10)
		{
			Buf60 ^= 1;
			return;
		}
		if (num4 > 256)
		{
			num2++;
		}
		if (num4 >= MaxDist3)
		{
			num2++;
		}
		OldDist[OldDistPtr++] = num4;
		OldDistPtr &= 3u;
		LastLength = num2;
		LastDist = num4;
		CopyString15(num4, num2);
	}

	private void LongLZ()
	{
		NumHuf = 0;
		Nlzb += 16u;
		if (Nlzb > 255)
		{
			Nlzb = 144u;
			Nhfb >>= 1;
		}
		uint avrLn = AvrLn2;
		uint num = Inp.fgetbits();
		uint num2;
		if (AvrLn2 >= 122)
		{
			num2 = DecodeNum(num, 3u, DecL2, PosL2);
		}
		else if (AvrLn2 >= 64)
		{
			num2 = DecodeNum(num, 2u, DecL1, PosL1);
		}
		else if (num < 256)
		{
			num2 = num;
			Inp.faddbits(16u);
		}
		else
		{
			for (num2 = 0u; ((num << (int)num2) & 0x8000) == 0; num2++)
			{
			}
			Inp.faddbits(num2 + 1);
		}
		AvrLn2 += num2;
		AvrLn2 -= AvrLn2 >> 5;
		num = Inp.fgetbits();
		uint num3 = ((AvrPlcB > 10495) ? DecodeNum(num, 5u, DecHf2, PosHf2) : ((AvrPlcB <= 1791) ? DecodeNum(num, 4u, DecHf0, PosHf0) : DecodeNum(num, 5u, DecHf1, PosHf1)));
		AvrPlcB += num3;
		AvrPlcB -= AvrPlcB >> 8;
		uint num5;
		uint num4;
		while (true)
		{
			num4 = ChSetB[num3 & 0xFF];
			num5 = NToPlB[num4++ & 0xFF]++;
			if ((num4 & 0xFF) == 0)
			{
				break;
			}
			CorrHuff(ChSetB, NToPlB);
		}
		ChSetB[num3 & 0xFF] = ChSetB[num5];
		ChSetB[num5] = (ushort)num4;
		num4 = ((num4 & 0xFF00) | (Inp.fgetbits() >> 8)) >> 1;
		Inp.faddbits(7u);
		uint avrLn2 = AvrLn3;
		if (num2 != 1 && num2 != 4)
		{
			if (num2 == 0 && num4 <= MaxDist3)
			{
				AvrLn3++;
				AvrLn3 -= AvrLn3 >> 8;
			}
			else if (AvrLn3 != 0)
			{
				AvrLn3--;
			}
		}
		num2 += 3;
		if (num4 >= MaxDist3)
		{
			num2++;
		}
		if (num4 <= 256)
		{
			num2 += 8;
		}
		if (avrLn2 > 176 || (AvrPlc >= 10752 && avrLn < 64))
		{
			MaxDist3 = 32512u;
		}
		else
		{
			MaxDist3 = 8193u;
		}
		OldDist[OldDistPtr++] = num4;
		OldDistPtr &= 3u;
		LastLength = num2;
		LastDist = num4;
		CopyString15(num4, num2);
	}

	private void HuffDecode()
	{
		uint num = Inp.fgetbits();
		int num2 = (int)((AvrPlc > 30207) ? DecodeNum(num, 8u, DecHf4, PosHf4) : ((AvrPlc > 24063) ? DecodeNum(num, 6u, DecHf3, PosHf3) : ((AvrPlc > 13823) ? DecodeNum(num, 5u, DecHf2, PosHf2) : ((AvrPlc <= 3583) ? DecodeNum(num, 4u, DecHf0, PosHf0) : DecodeNum(num, 5u, DecHf1, PosHf1)))));
		num2 &= 0xFF;
		if (StMode != 0)
		{
			if (num2 == 0 && num > 4095)
			{
				num2 = 256;
			}
			if (--num2 == -1)
			{
				num = Inp.fgetbits();
				Inp.faddbits(1u);
				if ((num & 0x8000u) != 0)
				{
					NumHuf = (StMode = 0);
					return;
				}
				uint length = (((num & 0x4000u) != 0) ? 4u : 3u);
				Inp.faddbits(1u);
				uint num3 = DecodeNum(Inp.fgetbits(), 5u, DecHf2, PosHf2);
				num3 = (num3 << 5) | (Inp.fgetbits() >> 11);
				Inp.faddbits(5u);
				CopyString15(num3, length);
				return;
			}
		}
		else if (NumHuf++ >= 16 && FlagsCnt == 0)
		{
			StMode = 1;
		}
		AvrPlc += (uint)num2;
		AvrPlc -= AvrPlc >> 8;
		Nhfb += 16u;
		if (Nhfb > 255)
		{
			Nhfb = 144u;
			Nlzb >>= 1;
		}
		Window[UnpPtr++] = (byte)(ChSet[num2] >> 8);
		DestUnpSize--;
		uint num4;
		uint num5;
		while (true)
		{
			num4 = ChSet[num2];
			num5 = NToPl[num4++ & 0xFF]++;
			if ((num4 & 0xFF) <= 161)
			{
				break;
			}
			CorrHuff(ChSet, NToPl);
		}
		ChSet[num2] = ChSet[num5];
		ChSet[num5] = (ushort)num4;
	}

	private void GetFlagsBuf()
	{
		uint num = DecodeNum(Inp.fgetbits(), 5u, DecHf2, PosHf2);
		if (num >= ChSetC.Length)
		{
			return;
		}
		uint num2;
		uint num3;
		while (true)
		{
			num2 = ChSetC[num];
			FlagBuf = num2 >> 8;
			num3 = NToPlC[num2++ & 0xFF]++;
			if ((num2 & 0xFFu) != 0)
			{
				break;
			}
			CorrHuff(ChSetC, NToPlC);
		}
		ChSetC[num] = ChSetC[num3];
		ChSetC[num3] = (ushort)num2;
	}

	private void UnpInitData15(bool Solid)
	{
		if (!Solid)
		{
			AvrPlcB = (AvrLn1 = (AvrLn2 = (AvrLn3 = 0u)));
			NumHuf = (Buf60 = 0);
			AvrPlc = 13568u;
			MaxDist3 = 8193u;
			Nhfb = (Nlzb = 128u);
		}
		FlagsCnt = 0;
		FlagBuf = 0u;
		StMode = 0;
		LCount = 0;
		ReadTop = 0;
	}

	private void InitHuff()
	{
		for (uint num = 0u; num < 256; num++)
		{
			ChSet[num] = (ChSetB[num] = (ushort)(num << 8));
			ChSetA[num] = (ushort)num;
			ChSetC[num] = (ushort)(((~num + 1) & 0xFF) << 8);
		}
		new Span<byte>(NToPl).Clear();
		new Span<byte>(NToPlB).Clear();
		new Span<byte>(NToPlC).Clear();
		CorrHuff(ChSetB, NToPlB);
	}

	private void CorrHuff(ushort[] CharSet, byte[] NumToPlace)
	{
		for (int num = 7; num >= 0; num--)
		{
			for (int i = 0; i < 32; i++)
			{
				CharSet[i] = (ushort)((CharSet[i] & 0xFFFFFF00u) | (uint)num);
			}
		}
		new Span<byte>(NumToPlace, 0, NToPl.Length).Clear();
		for (int num = 6; num >= 0; num--)
		{
			NumToPlace[num] = (byte)((7 - num) * 32);
		}
	}

	private void CopyString15(uint Distance, uint Length)
	{
		DestUnpSize -= Length;
		while (Length-- != 0)
		{
			Window[UnpPtr] = Window[(UnpPtr - Distance) & MaxWinMask];
			UnpPtr = (UnpPtr + 1) & MaxWinMask;
		}
	}

	private uint DecodeNum(uint Num, uint StartPos, uint[] DecTab, uint[] PosTab)
	{
		Num &= 0xFFF0u;
		int i;
		for (i = 0; DecTab[i] <= Num; i++)
		{
			StartPos++;
		}
		Inp.faddbits(StartPos);
		return (Num - ((i != 0) ? DecTab[i - 1] : 0) >> (int)(16 - StartPos)) + PosTab[StartPos];
	}

	private void CopyString20(uint Length, uint Distance)
	{
		LastDist = (OldDist[OldDistPtr++ & 3] = Distance);
		LastLength = Length;
		DestUnpSize -= Length;
		CopyString(Length, Distance);
	}

	private void Unpack20(bool Solid)
	{
		if (Suspended)
		{
			UnpPtr = WrPtr;
		}
		else
		{
			UnpInitData(Solid);
			if (!UnpReadBuf() || ((!Solid || !TablesRead2) && !ReadTables20()))
			{
				return;
			}
			DestUnpSize--;
		}
		while (DestUnpSize >= 0)
		{
			UnpPtr &= MaxWinMask;
			if (Inp.InAddr > ReadTop - 30 && !UnpReadBuf())
			{
				break;
			}
			if (((WrPtr - UnpPtr) & MaxWinMask) < 270 && WrPtr != UnpPtr)
			{
				UnpWriteBuf20();
				if (Suspended)
				{
					return;
				}
			}
			if (UnpAudioBlock)
			{
				uint num = DecodeNumber(Inp, MD[UnpCurChannel]);
				if (num == 256)
				{
					if (!ReadTables20())
					{
						break;
					}
					continue;
				}
				Window[UnpPtr++] = DecodeAudio((int)num);
				if (++UnpCurChannel == UnpChannels)
				{
					UnpCurChannel = 0u;
				}
				DestUnpSize--;
				continue;
			}
			uint num2 = DecodeNumber(Inp, BlockTables.LD);
			if (num2 < 256)
			{
				Window[UnpPtr++] = (byte)num2;
				DestUnpSize--;
			}
			else if (num2 > 269)
			{
				uint num3 = (uint)(Unpack20Local.LDecode[num2 -= 270] + 3);
				uint num4;
				if ((num4 = Unpack20Local.LBits[num2]) != 0)
				{
					num3 += Inp.getbits() >> (int)(16 - num4);
					Inp.addbits(num4);
				}
				uint num5 = DecodeNumber(Inp, BlockTables.DD);
				uint num6 = Unpack20Local.DDecode[num5] + 1;
				if ((num4 = Unpack20Local.DBits[num5]) != 0)
				{
					num6 += Inp.getbits() >> (int)(16 - num4);
					Inp.addbits(num4);
				}
				if (num6 >= 8192)
				{
					num3++;
					if ((long)num6 >= 262144L)
					{
						num3++;
					}
				}
				CopyString20(num3, num6);
			}
			else if (num2 == 269)
			{
				if (!ReadTables20())
				{
					break;
				}
			}
			else if (num2 == 256)
			{
				CopyString20(LastLength, LastDist);
			}
			else if (num2 < 261)
			{
				uint num7 = OldDist[(OldDistPtr - (num2 - 256)) & 3];
				uint num8 = DecodeNumber(Inp, BlockTables.RD);
				uint num9 = (uint)(Unpack20Local.LDecode[num8] + 2);
				uint num4;
				if ((num4 = Unpack20Local.LBits[num8]) != 0)
				{
					num9 += Inp.getbits() >> (int)(16 - num4);
					Inp.addbits(num4);
				}
				if (num7 >= 257)
				{
					num9++;
					if (num7 >= 8192)
					{
						num9++;
						if (num7 >= 262144)
						{
							num9++;
						}
					}
				}
				CopyString20(num9, num7);
			}
			else if (num2 < 270)
			{
				uint num10 = (uint)(Unpack20Local.SDDecode[num2 -= 261] + 1);
				uint num4;
				if ((num4 = Unpack20Local.SDBits[num2]) != 0)
				{
					num10 += Inp.getbits() >> (int)(16 - num4);
					Inp.addbits(num4);
				}
				CopyString20(2u, num10);
			}
		}
		ReadLastTables();
		UnpWriteBuf20();
	}

	private void UnpWriteBuf20()
	{
		if (UnpPtr != WrPtr)
		{
			UnpSomeRead = true;
		}
		if (UnpPtr < WrPtr)
		{
			UnpIO_UnpWrite(Window, WrPtr, (uint)((int)(0 - WrPtr) & MaxWinMask));
			UnpIO_UnpWrite(Window, 0u, UnpPtr);
			UnpAllBuf = true;
		}
		else
		{
			UnpIO_UnpWrite(Window, WrPtr, UnpPtr - WrPtr);
		}
		WrPtr = UnpPtr;
	}

	private bool ReadTables20()
	{
		byte[] array = new byte[19];
		byte[] array2 = new byte[1028];
		if (Inp.InAddr > ReadTop - 25 && !UnpReadBuf())
		{
			return false;
		}
		uint num = Inp.getbits();
		UnpAudioBlock = (num & 0x8000) != 0;
		if ((num & 0x4000u) != 0)
		{
			new Span<byte>(UnpOldTable20).Clear();
		}
		Inp.addbits(2u);
		uint num2;
		if (UnpAudioBlock)
		{
			UnpChannels = ((num >> 12) & 3) + 1;
			if (UnpCurChannel >= UnpChannels)
			{
				UnpCurChannel = 0u;
			}
			Inp.addbits(2u);
			num2 = 257 * UnpChannels;
		}
		else
		{
			num2 = 374u;
		}
		for (uint num3 = 0u; num3 < 19; num3++)
		{
			array[num3] = (byte)(Inp.getbits() >> 12);
			Inp.addbits(4u);
		}
		MakeDecodeTables(array, 0, BlockTables.BD, 19u);
		uint num4 = 0u;
		while (num4 < num2)
		{
			if (Inp.InAddr > ReadTop - 5 && !UnpReadBuf())
			{
				return false;
			}
			uint num5 = DecodeNumber(Inp, BlockTables.BD);
			uint num6;
			switch (num5)
			{
			case 0u:
			case 1u:
			case 2u:
			case 3u:
			case 4u:
			case 5u:
			case 6u:
			case 7u:
			case 8u:
			case 9u:
			case 10u:
			case 11u:
			case 12u:
			case 13u:
			case 14u:
			case 15u:
				array2[num4] = (byte)((num5 + UnpOldTable20[num4]) & 0xFu);
				num4++;
				continue;
			case 16u:
			{
				uint num7 = (Inp.getbits() >> 14) + 3;
				Inp.addbits(2u);
				if (num4 == 0)
				{
					return false;
				}
				while (num7-- != 0 && num4 < num2)
				{
					array2[num4] = array2[num4 - 1];
					num4++;
				}
				continue;
			}
			case 17u:
				num6 = (Inp.getbits() >> 13) + 3;
				Inp.addbits(3u);
				break;
			default:
				num6 = (Inp.getbits() >> 9) + 11;
				Inp.addbits(7u);
				break;
			}
			while (num6-- != 0 && num4 < num2)
			{
				array2[num4++] = 0;
			}
		}
		TablesRead2 = true;
		if (Inp.InAddr > ReadTop)
		{
			return true;
		}
		if (UnpAudioBlock)
		{
			for (uint num8 = 0u; num8 < UnpChannels; num8++)
			{
				MakeDecodeTables(array2, (int)(num8 * 257), MD[num8], 257u);
			}
		}
		else
		{
			MakeDecodeTables(array2, 0, BlockTables.LD, 298u);
			MakeDecodeTables(array2, 298, BlockTables.DD, 48u);
			MakeDecodeTables(array2, 346, BlockTables.RD, 28u);
		}
		Array.Copy(array2, UnpOldTable20, UnpOldTable20.Length);
		return true;
	}

	private void ReadLastTables()
	{
		if (ReadTop < Inp.InAddr + 5)
		{
			return;
		}
		if (UnpAudioBlock)
		{
			if (DecodeNumber(Inp, MD[UnpCurChannel]) == 256)
			{
				ReadTables20();
			}
		}
		else if (DecodeNumber(Inp, BlockTables.LD) == 269)
		{
			ReadTables20();
		}
	}

	private void UnpInitData20(bool Solid)
	{
		if (!Solid)
		{
			TablesRead2 = false;
			UnpAudioBlock = false;
			UnpChannelDelta = 0;
			UnpCurChannel = 0u;
			UnpChannels = 1u;
			AudV = new AudioVariables[4];
			new Span<byte>(UnpOldTable20).Clear();
			MD = new DecodeTable[4];
		}
	}

	private byte DecodeAudio(int Delta)
	{
		AudioVariables audioVariables = AudV[UnpCurChannel];
		audioVariables.ByteCount++;
		audioVariables.D4 = audioVariables.D3;
		audioVariables.D3 = audioVariables.D2;
		audioVariables.D2 = audioVariables.LastDelta - audioVariables.D1;
		audioVariables.D1 = audioVariables.LastDelta;
		uint num = (uint)(((8 * audioVariables.LastChar + audioVariables.K1 * audioVariables.D1 + audioVariables.K2 * audioVariables.D2 + audioVariables.K3 * audioVariables.D3 + audioVariables.K4 * audioVariables.D4 + audioVariables.K5 * UnpChannelDelta >> 3) & 0xFF) - Delta);
		int num2 = (sbyte)Delta;
		num2 <<= 3;
		audioVariables.Dif[0] += (uint)Math.Abs(num2);
		audioVariables.Dif[1] += (uint)Math.Abs(num2 - audioVariables.D1);
		audioVariables.Dif[2] += (uint)Math.Abs(num2 + audioVariables.D1);
		audioVariables.Dif[3] += (uint)Math.Abs(num2 - audioVariables.D2);
		audioVariables.Dif[4] += (uint)Math.Abs(num2 + audioVariables.D2);
		audioVariables.Dif[5] += (uint)Math.Abs(num2 - audioVariables.D3);
		audioVariables.Dif[6] += (uint)Math.Abs(num2 + audioVariables.D3);
		audioVariables.Dif[7] += (uint)Math.Abs(num2 - audioVariables.D4);
		audioVariables.Dif[8] += (uint)Math.Abs(num2 + audioVariables.D4);
		audioVariables.Dif[9] += (uint)Math.Abs(num2 - UnpChannelDelta);
		audioVariables.Dif[10] += (uint)Math.Abs(num2 + UnpChannelDelta);
		UnpChannelDelta = (audioVariables.LastDelta = (sbyte)(num - audioVariables.LastChar));
		audioVariables.LastChar = (int)num;
		if ((audioVariables.ByteCount & 0x1F) == 0)
		{
			uint num3 = audioVariables.Dif[0];
			uint num4 = 0u;
			audioVariables.Dif[0] = 0u;
			for (uint num5 = 1u; num5 < audioVariables.Dif.Length; num5++)
			{
				if (audioVariables.Dif[num5] < num3)
				{
					num3 = audioVariables.Dif[num5];
					num4 = num5;
				}
				audioVariables.Dif[num5] = 0u;
			}
			switch (num4)
			{
			case 1u:
				if (audioVariables.K1 >= -16)
				{
					audioVariables.K1--;
				}
				break;
			case 2u:
				if (audioVariables.K1 < 16)
				{
					audioVariables.K1++;
				}
				break;
			case 3u:
				if (audioVariables.K2 >= -16)
				{
					audioVariables.K2--;
				}
				break;
			case 4u:
				if (audioVariables.K2 < 16)
				{
					audioVariables.K2++;
				}
				break;
			case 5u:
				if (audioVariables.K3 >= -16)
				{
					audioVariables.K3--;
				}
				break;
			case 6u:
				if (audioVariables.K3 < 16)
				{
					audioVariables.K3++;
				}
				break;
			case 7u:
				if (audioVariables.K4 >= -16)
				{
					audioVariables.K4--;
				}
				break;
			case 8u:
				if (audioVariables.K4 < 16)
				{
					audioVariables.K4++;
				}
				break;
			case 9u:
				if (audioVariables.K5 >= -16)
				{
					audioVariables.K5--;
				}
				break;
			case 10u:
				if (audioVariables.K5 < 16)
				{
					audioVariables.K5++;
				}
				break;
			}
		}
		return (byte)num;
	}

	private void Unpack5(bool Solid)
	{
		FileExtracted = true;
		if (!Suspended)
		{
			UnpInitData(Solid);
			if (!UnpReadBuf() || !ReadBlockHeader(Inp, ref BlockHeader) || !ReadTables(Inp, ref BlockHeader, ref BlockTables) || !TablesRead5)
			{
				return;
			}
		}
		while (true)
		{
			UnpPtr &= MaxWinMask;
			if (Inp.InAddr >= ReadBorder)
			{
				bool flag = false;
				while (Inp.InAddr > BlockHeader.BlockStart + BlockHeader.BlockSize - 1 || (Inp.InAddr == BlockHeader.BlockStart + BlockHeader.BlockSize - 1 && Inp.InBit >= BlockHeader.BlockBitSize))
				{
					if (BlockHeader.LastBlockInFile)
					{
						flag = true;
						break;
					}
					if (!ReadBlockHeader(Inp, ref BlockHeader) || !ReadTables(Inp, ref BlockHeader, ref BlockTables))
					{
						return;
					}
				}
				if (flag || !UnpReadBuf())
				{
					break;
				}
			}
			if (((WriteBorder - UnpPtr) & MaxWinMask) < 4100 && WriteBorder != UnpPtr)
			{
				UnpWriteBuf();
				if (WrittenFileSize > DestUnpSize)
				{
					return;
				}
				if (Suspended)
				{
					FileExtracted = false;
					return;
				}
			}
			uint num = DecodeNumber(Inp, BlockTables.LD);
			if (num < 256)
			{
				if (Fragmented)
				{
					FragWindow[UnpPtr++] = (byte)num;
				}
				else
				{
					Window[UnpPtr++] = (byte)num;
				}
			}
			else if (num >= 262)
			{
				uint num2 = SlotToLength(Inp, num - 262);
				uint num3 = 1u;
				uint num4 = DecodeNumber(Inp, BlockTables.DD);
				uint num5;
				if (num4 < 4)
				{
					num5 = 0u;
					num3 += num4;
				}
				else
				{
					num5 = num4 / 2 - 1;
					num3 += (2 | (num4 & 1)) << (int)num5;
				}
				if (num5 != 0)
				{
					if (num5 >= 4)
					{
						if (num5 > 4)
						{
							num3 += Inp.getbits32() >> (int)(36 - num5) << 4;
							Inp.addbits(num5 - 4);
						}
						uint num6 = DecodeNumber(Inp, BlockTables.LDD);
						num3 += num6;
					}
					else
					{
						num3 += Inp.getbits32() >> (int)(32 - num5);
						Inp.addbits(num5);
					}
				}
				if (num3 > 256)
				{
					num2++;
					if (num3 > 8192)
					{
						num2++;
						if (num3 > 262144)
						{
							num2++;
						}
					}
				}
				InsertOldDist(num3);
				LastLength = num2;
				if (Fragmented)
				{
					FragWindow.CopyString(num2, num3, ref UnpPtr, MaxWinMask);
				}
				else
				{
					CopyString(num2, num3);
				}
			}
			else if (num == 256)
			{
				UnpackFilter filter = new UnpackFilter();
				if (!ReadFilter(Inp, filter) || !AddFilter(filter))
				{
					break;
				}
			}
			else if (num == 257)
			{
				if (LastLength != 0)
				{
					if (Fragmented)
					{
						FragWindow.CopyString(LastLength, OldDist[0], ref UnpPtr, MaxWinMask);
					}
					else
					{
						CopyString(LastLength, OldDist[0]);
					}
				}
			}
			else if (num < 262)
			{
				uint num7 = num - 258;
				uint num8 = OldDist[num7];
				for (uint num9 = num7; num9 != 0; num9--)
				{
					OldDist[num9] = OldDist[num9 - 1];
				}
				OldDist[0] = num8;
				uint slot = DecodeNumber(Inp, BlockTables.RD);
				uint length = (LastLength = SlotToLength(Inp, slot));
				if (Fragmented)
				{
					FragWindow.CopyString(length, num8, ref UnpPtr, MaxWinMask);
				}
				else
				{
					CopyString(length, num8);
				}
			}
		}
		UnpWriteBuf();
	}

	private uint ReadFilterData(BitInput Inp)
	{
		uint num = (Inp.fgetbits() >> 14) + 1;
		Inp.addbits(2u);
		uint num2 = 0u;
		for (uint num3 = 0u; num3 < num; num3++)
		{
			num2 += Inp.fgetbits() >> 8 << (int)(num3 * 8);
			Inp.addbits(8u);
		}
		return num2;
	}

	private bool ReadFilter(BitInput Inp, UnpackFilter Filter)
	{
		if (!Inp.ExternalBuffer && Inp.InAddr > ReadTop - 16 && !UnpReadBuf())
		{
			return false;
		}
		Filter.BlockStart = ReadFilterData(Inp);
		Filter.BlockLength = ReadFilterData(Inp);
		if (Filter.BlockLength > 4194304)
		{
			Filter.BlockLength = 0u;
		}
		Filter.Type = (byte)(Inp.fgetbits() >> 13);
		Inp.faddbits(3u);
		if (Filter.Type == 0)
		{
			Filter.Channels = (byte)((Inp.fgetbits() >> 11) + 1);
			Inp.faddbits(5u);
		}
		return true;
	}

	private bool AddFilter(UnpackFilter Filter)
	{
		if (Filters.Count >= 8192)
		{
			UnpWriteBuf();
			if (Filters.Count >= 8192)
			{
				InitFilters();
			}
		}
		Filter.NextWindow = WrPtr != UnpPtr && ((WrPtr - UnpPtr) & MaxWinMask) <= Filter.BlockStart;
		Filter.BlockStart = (Filter.BlockStart + UnpPtr) & MaxWinMask;
		Filters.Add(Filter);
		return true;
	}

	private bool UnpReadBuf()
	{
		int num = ReadTop - Inp.InAddr;
		if (num < 0)
		{
			return false;
		}
		BlockHeader.BlockSize -= Inp.InAddr - BlockHeader.BlockStart;
		if (Inp.InAddr > 16384)
		{
			if (num > 0)
			{
				Buffer.BlockCopy(Inp.InBuf, Inp.InAddr, Inp.InBuf, 0, num);
			}
			Inp.InAddr = 0;
			ReadTop = num;
		}
		else
		{
			num = ReadTop;
		}
		int num2 = 0;
		if (32768 != num)
		{
			num2 = UnpIO_UnpRead(Inp.InBuf, num, 32768 - num);
		}
		if (num2 > 0)
		{
			ReadTop += num2;
		}
		ReadBorder = ReadTop - 30;
		BlockHeader.BlockStart = Inp.InAddr;
		if (BlockHeader.BlockSize != -1)
		{
			ReadBorder = Math.Min(ReadBorder, BlockHeader.BlockStart + BlockHeader.BlockSize - 1);
		}
		return num2 != -1;
	}

	private void UnpWriteBuf()
	{
		uint num = WrPtr;
		uint num2 = (UnpPtr - num) & MaxWinMask;
		uint num3 = num2;
		bool flag = false;
		for (int i = 0; i < Filters.Count; i++)
		{
			UnpackFilter unpackFilter = Filters[i];
			if (unpackFilter.Type == 8)
			{
				continue;
			}
			if (unpackFilter.NextWindow)
			{
				if (((unpackFilter.BlockStart - WrPtr) & MaxWinMask) <= num2)
				{
					unpackFilter.NextWindow = false;
				}
				continue;
			}
			uint blockStart = unpackFilter.BlockStart;
			uint blockLength = unpackFilter.BlockLength;
			if (((blockStart - num) & MaxWinMask) >= num3)
			{
				continue;
			}
			if (num != blockStart)
			{
				UnpWriteArea(num, blockStart);
				num = blockStart;
				num3 = (UnpPtr - num) & MaxWinMask;
			}
			if (blockLength <= num3)
			{
				if (blockLength == 0)
				{
					continue;
				}
				uint num4 = (blockStart + blockLength) & MaxWinMask;
				FilterSrcMemory = EnsureCapacity(FilterSrcMemory, checked((int)blockLength));
				byte[] filterSrcMemory = FilterSrcMemory;
				if (blockStart < num4 || num4 == 0)
				{
					if (Fragmented)
					{
						FragWindow.CopyData(filterSrcMemory, 0u, blockStart, blockLength);
					}
					else
					{
						Utility.Copy(Window, blockStart, filterSrcMemory, 0L, blockLength);
					}
				}
				else
				{
					uint num5 = MaxWinSize - blockStart;
					if (Fragmented)
					{
						FragWindow.CopyData(filterSrcMemory, 0u, blockStart, num5);
						FragWindow.CopyData(filterSrcMemory, num5, 0u, num4);
					}
					else
					{
						Utility.Copy(Window, blockStart, filterSrcMemory, 0L, num5);
						Utility.Copy(Window, 0L, filterSrcMemory, num5, num4);
					}
				}
				byte[] array = ApplyFilter(filterSrcMemory, blockLength, unpackFilter);
				Filters[i].Type = 8;
				if (array != null)
				{
					UnpIO_UnpWrite(array, 0u, blockLength);
				}
				UnpSomeRead = true;
				WrittenFileSize += blockLength;
				num = num4;
				num3 = (UnpPtr - num) & MaxWinMask;
				continue;
			}
			WrPtr = num;
			for (int j = i; j < Filters.Count; j++)
			{
				UnpackFilter unpackFilter2 = Filters[j];
				if (unpackFilter2.Type != 8)
				{
					unpackFilter2.NextWindow = false;
				}
			}
			flag = true;
			break;
		}
		int num6 = 0;
		for (int k = 0; k < Filters.Count; k++)
		{
			if (num6 > 0)
			{
				Filters[k - num6] = Filters[k];
			}
			if (Filters[k].Type == 8)
			{
				num6++;
			}
		}
		if (num6 > 0)
		{
			Filters.RemoveRange(Filters.Count - num6, num6);
		}
		if (!flag)
		{
			UnpWriteArea(num, UnpPtr);
			WrPtr = UnpPtr;
		}
		WriteBorder = (UnpPtr + Math.Min(MaxWinSize, 4194304u)) & MaxWinMask;
		if (WriteBorder == UnpPtr || (WrPtr != UnpPtr && ((WrPtr - UnpPtr) & MaxWinMask) < ((WriteBorder - UnpPtr) & MaxWinMask)))
		{
			WriteBorder = WrPtr;
		}
	}

	private byte[] ApplyFilter(byte[] __d, uint DataSize, UnpackFilter Flt)
	{
		int num = 0;
		switch (Flt.Type)
		{
		case 1:
		case 2:
		{
			uint num9 = (uint)WrittenFileSize;
			byte b2 = (byte)((Flt.Type == 2) ? 233 : 232);
			uint num10 = 0u;
			while (num10 + 4 < DataSize)
			{
				byte b3 = __d[num++];
				num10++;
				if (b3 != 232 && b3 != b2)
				{
					continue;
				}
				uint num11 = (num10 + num9) % 16777216;
				uint num12 = RawGet4(__d, num);
				if ((num12 & 0x80000000u) != 0)
				{
					if (((num12 + num11) & 0x80000000u) == 0)
					{
						RawPut4(num12 + 16777216, __d, num);
					}
				}
				else if (((num12 - 16777216) & 0x80000000u) != 0)
				{
					RawPut4(num12 - num11, __d, num);
				}
				num += 4;
				num10 += 4;
			}
			return __d;
		}
		case 3:
		{
			uint num5 = (uint)WrittenFileSize;
			for (uint num6 = 0u; num6 + 3 < DataSize; num6 += 4)
			{
				long num7 = num + num6;
				if (__d[num7 + 3] == 235)
				{
					uint num8 = (uint)(__d[num7] + __d[num7 + 1] * 256 + __d[num7 + 2] * 65536);
					num8 -= (num5 + num6) / 4;
					__d[num7] = (byte)num8;
					__d[num7 + 1] = (byte)(num8 >> 8);
					__d[num7 + 2] = (byte)(num8 >> 16);
				}
			}
			return __d;
		}
		case 0:
		{
			uint channels = Flt.Channels;
			uint num2 = 0u;
			FilterDstMemory = EnsureCapacity(FilterDstMemory, checked((int)DataSize));
			byte[] filterDstMemory = FilterDstMemory;
			for (uint num3 = 0u; num3 < channels; num3++)
			{
				byte b = 0;
				for (uint num4 = num3; num4 < DataSize; num4 += channels)
				{
					b = (filterDstMemory[num4] = (byte)(b - __d[num + num2++]));
				}
			}
			return filterDstMemory;
		}
		default:
			return null;
		}
	}

	private void UnpWriteArea(uint StartPtr, uint EndPtr)
	{
		if (EndPtr != StartPtr)
		{
			UnpSomeRead = true;
		}
		if (EndPtr < StartPtr)
		{
			UnpAllBuf = true;
		}
		if (Fragmented)
		{
			uint num = (EndPtr - StartPtr) & MaxWinMask;
			while (num != 0)
			{
				uint blockSize = FragWindow.GetBlockSize(StartPtr, num);
				FragWindow.GetBuffer(StartPtr, out var buf, out var offset);
				UnpWriteData(buf, offset, blockSize);
				num -= blockSize;
				StartPtr = (StartPtr + blockSize) & MaxWinMask;
			}
		}
		else if (EndPtr < StartPtr)
		{
			UnpWriteData(Window, StartPtr, MaxWinSize - StartPtr);
			UnpWriteData(Window, 0u, EndPtr);
		}
		else
		{
			UnpWriteData(Window, StartPtr, EndPtr - StartPtr);
		}
	}

	private void UnpWriteData(byte[] Data, uint offset, uint Size)
	{
		if (WrittenFileSize < DestUnpSize)
		{
			uint num = Size;
			long num2 = DestUnpSize - WrittenFileSize;
			if (num > num2)
			{
				num = (uint)num2;
			}
			UnpIO_UnpWrite(Data, offset, num);
			WrittenFileSize += Size;
		}
	}

	private void UnpInitData50(bool Solid)
	{
		if (!Solid)
		{
			TablesRead5 = false;
		}
	}

	private bool ReadBlockHeader(BitInput Inp, ref UnpackBlockHeader Header)
	{
		Header.HeaderSize = 0;
		if (!Inp.ExternalBuffer && Inp.InAddr > ReadTop - 7 && !UnpReadBuf())
		{
			return false;
		}
		Inp.faddbits((uint)(8 - Inp.InBit) & 7u);
		byte b = (byte)(Inp.fgetbits() >> 8);
		Inp.faddbits(8u);
		uint num = (uint)(((b >> 3) & 3) + 1);
		if (num == 4)
		{
			return false;
		}
		Header.HeaderSize = (int)(2 + num);
		Header.BlockBitSize = (b & 7) + 1;
		byte b2 = (byte)(Inp.fgetbits() >> 8);
		Inp.faddbits(8u);
		int num2 = 0;
		for (uint num3 = 0u; num3 < num; num3++)
		{
			num2 += (int)(Inp.fgetbits() >> 8 << (int)(num3 * 8));
			Inp.addbits(8u);
		}
		Header.BlockSize = num2;
		if ((byte)(0x5A ^ b ^ num2 ^ (num2 >> 8) ^ (num2 >> 16)) != b2)
		{
			return false;
		}
		Header.BlockStart = Inp.InAddr;
		ReadBorder = Math.Min(ReadBorder, Header.BlockStart + Header.BlockSize - 1);
		Header.LastBlockInFile = (b & 0x40) != 0;
		Header.TablePresent = (b & 0x80) != 0;
		return true;
	}

	private bool ReadTables(BitInput Inp, ref UnpackBlockHeader Header, ref UnpackBlockTables Tables)
	{
		if (!Header.TablePresent)
		{
			return true;
		}
		if (!Inp.ExternalBuffer && Inp.InAddr > ReadTop - 25 && !UnpReadBuf())
		{
			return false;
		}
		byte[] array = new byte[20];
		for (uint num = 0u; num < 20; num++)
		{
			uint num2 = (byte)(Inp.fgetbits() >> 12);
			Inp.faddbits(4u);
			if (num2 == 15)
			{
				uint num3 = (byte)(Inp.fgetbits() >> 12);
				Inp.faddbits(4u);
				if (num3 == 0)
				{
					array[num] = 15;
					continue;
				}
				num3 += 2;
				while (num3-- != 0 && num < array.Length)
				{
					array[num++] = 0;
				}
				num--;
			}
			else
			{
				array[num] = (byte)num2;
			}
		}
		MakeDecodeTables(array, 0, Tables.BD, 20u);
		byte[] array2 = new byte[430];
		uint num4 = 0u;
		while (num4 < 430)
		{
			if (!Inp.ExternalBuffer && Inp.InAddr > ReadTop - 5 && !UnpReadBuf())
			{
				return false;
			}
			uint num5 = DecodeNumber(Inp, Tables.BD);
			if (num5 < 16)
			{
				array2[num4] = (byte)num5;
				num4++;
			}
			else if (num5 < 18)
			{
				uint num6;
				if (num5 == 16)
				{
					num6 = (Inp.fgetbits() >> 13) + 3;
					Inp.faddbits(3u);
				}
				else
				{
					num6 = (Inp.fgetbits() >> 9) + 11;
					Inp.faddbits(7u);
				}
				if (num4 == 0)
				{
					return false;
				}
				while (num6-- != 0 && num4 < 430)
				{
					array2[num4] = array2[num4 - 1];
					num4++;
				}
			}
			else
			{
				uint num7;
				if (num5 == 18)
				{
					num7 = (Inp.fgetbits() >> 13) + 3;
					Inp.faddbits(3u);
				}
				else
				{
					num7 = (Inp.fgetbits() >> 9) + 11;
					Inp.faddbits(7u);
				}
				while (num7-- != 0 && num4 < 430)
				{
					array2[num4++] = 0;
				}
			}
		}
		TablesRead5 = true;
		if (!Inp.ExternalBuffer && Inp.InAddr > ReadTop)
		{
			return false;
		}
		MakeDecodeTables(array2, 0, Tables.LD, 306u);
		MakeDecodeTables(array2, 306, Tables.DD, 64u);
		MakeDecodeTables(array2, 370, Tables.LDD, 16u);
		MakeDecodeTables(array2, 386, Tables.RD, 44u);
		return true;
	}

	private void InitFilters()
	{
		Filters.Clear();
	}

	private void InsertOldDist(uint Distance)
	{
		OldDist[3] = OldDist[2];
		OldDist[2] = OldDist[1];
		OldDist[1] = OldDist[0];
		OldDist[0] = Distance;
	}

	private void CopyString(uint Length, uint Distance)
	{
		uint num = UnpPtr - Distance;
		if (num < MaxWinSize - 4097 && UnpPtr < MaxWinSize - 4097)
		{
			byte[] window = Window;
			while (Length-- != 0)
			{
				window[UnpPtr++] = window[num++];
			}
		}
		else
		{
			while (Length-- != 0)
			{
				Window[UnpPtr] = Window[num++ & MaxWinMask];
				UnpPtr = (UnpPtr + 1) & MaxWinMask;
			}
		}
	}

	private uint DecodeNumber(BitInput Inp, DecodeTable Dec)
	{
		uint num = Inp.getbits() & 0xFFFEu;
		if (num < Dec.DecodeLen[Dec.QuickBits])
		{
			uint num2 = num >> (int)(16 - Dec.QuickBits);
			Inp.addbits(Dec.QuickLen[num2]);
			return Dec.QuickNum[num2];
		}
		uint num3 = 15u;
		for (uint num4 = Dec.QuickBits + 1; num4 < 15; num4++)
		{
			if (num < Dec.DecodeLen[num4])
			{
				num3 = num4;
				break;
			}
		}
		Inp.addbits(num3);
		uint num5 = num - Dec.DecodeLen[num3 - 1];
		num5 >>= (int)(16 - num3);
		uint num6 = Dec.DecodePos[num3] + num5;
		if (num6 >= Dec.MaxNum)
		{
			num6 = 0u;
		}
		return Dec.DecodeNum[num6];
	}

	private uint SlotToLength(BitInput Inp, uint Slot)
	{
		uint num = 2u;
		uint num2;
		if (Slot < 8)
		{
			num2 = 0u;
			num += Slot;
		}
		else
		{
			num2 = Slot / 4 - 1;
			num += (4 | (Slot & 3)) << (int)num2;
		}
		if (num2 != 0)
		{
			num += Inp.getbits() >> (int)(16 - num2);
			Inp.addbits(num2);
		}
		return num;
	}

	public Unpack()
		: base(AllocBuffer: true)
	{
		_UnpackCtor();
		Window = null;
		Fragmented = false;
		Suspended = false;
		UnpAllBuf = false;
		UnpSomeRead = false;
		MaxWinSize = 0u;
		MaxWinMask = 0u;
		UnpInitData(Solid: false);
		UnpInitData15(Solid: false);
		InitHuff();
	}

	private void Init(uint WinSize, bool Solid)
	{
		if (WinSize == 0)
		{
			throw new InvalidFormatException("invalid window size (possibly due to a rar file with a 4GB being unpacked on a 32-bit platform)");
		}
		if (WinSize < 262144)
		{
			WinSize = 262144u;
		}
		if (WinSize <= MaxWinSize || WinSize >> 16 > 65536)
		{
			return;
		}
		bool flag = Solid && (Window != null || Fragmented);
		if (flag && Fragmented)
		{
			throw new InvalidFormatException("Grow && Fragmented");
		}
		byte[] array = (Fragmented ? null : new byte[WinSize]);
		if (array == null)
		{
			if (flag || WinSize < 16777216)
			{
				throw new InvalidFormatException("Grow || WinSize<0x1000000");
			}
			if (Window != null)
			{
				Window = null;
			}
			FragWindow.Init(WinSize);
			Fragmented = true;
		}
		if (!Fragmented)
		{
			if (flag)
			{
				for (uint num = 1u; num <= MaxWinSize; num++)
				{
					array[(UnpPtr - num) & (WinSize - 1)] = Window[(UnpPtr - num) & (MaxWinSize - 1)];
				}
			}
			Window = array;
		}
		MaxWinSize = WinSize;
		MaxWinMask = MaxWinSize - 1;
	}

	private void DoUnpack(uint Method, bool Solid)
	{
		switch (Method)
		{
		case 15u:
			if (!Fragmented)
			{
				Unpack15(Solid);
			}
			break;
		case 20u:
		case 26u:
			if (!Fragmented)
			{
				Unpack20(Solid);
			}
			break;
		case 29u:
			if (!Fragmented)
			{
				throw new NotImplementedException();
			}
			break;
		case 50u:
			Unpack5(Solid);
			break;
		default:
			throw new InvalidFormatException("unknown compression method " + Method);
		}
	}

	private void UnpInitData(bool Solid)
	{
		if (!Solid)
		{
			new Span<uint>(OldDist).Clear();
			OldDistPtr = 0u;
			LastDist = (LastLength = 0u);
			BlockTables = default(UnpackBlockTables);
			BlockTables.Init();
			UnpPtr = (WrPtr = 0u);
			WriteBorder = Math.Min(MaxWinSize, 4194304u) & MaxWinMask;
		}
		InitFilters();
		Inp.InitBitInput();
		WrittenFileSize = 0L;
		ReadTop = 0;
		ReadBorder = 0;
		BlockHeader = default(UnpackBlockHeader);
		BlockHeader.BlockSize = -1;
		UnpInitData20(Solid);
		UnpInitData50(Solid);
	}

	private void MakeDecodeTables(byte[] LengthTable, int offset, DecodeTable Dec, uint Size)
	{
		Dec.MaxNum = Size;
		uint[] array = new uint[16];
		for (uint num = 0u; num < Size; num++)
		{
			array[LengthTable[offset + num] & 0xF]++;
		}
		array[0] = 0u;
		new Span<ushort>(Dec.DecodeNum).Clear();
		Dec.DecodePos[0] = 0u;
		Dec.DecodeLen[0] = 0u;
		uint num2 = 0u;
		for (int i = 1; i < 16; i++)
		{
			num2 += array[i];
			uint num3 = num2 << 16 - i;
			num2 *= 2;
			Dec.DecodeLen[i] = num3;
			Dec.DecodePos[i] = Dec.DecodePos[i - 1] + array[i - 1];
		}
		uint[] array2 = new uint[Dec.DecodePos.Length];
		Array.Copy(Dec.DecodePos, array2, array2.Length);
		for (uint num4 = 0u; num4 < Size; num4++)
		{
			byte b = (byte)(LengthTable[offset + num4] & 0xFu);
			if (b != 0)
			{
				uint num5 = array2[b];
				Dec.DecodeNum[num5] = (ushort)num4;
				array2[b]++;
			}
		}
		if (Size - 298 <= 1 || Size == 306)
		{
			Dec.QuickBits = 10u;
		}
		else
		{
			Dec.QuickBits = 7u;
		}
		uint num6 = (uint)(1 << (int)Dec.QuickBits);
		byte b2 = 1;
		for (uint num7 = 0u; num7 < num6; num7++)
		{
			uint num8 = num7 << (int)(16 - Dec.QuickBits);
			while (b2 < Dec.DecodeLen.Length && num8 >= Dec.DecodeLen[b2])
			{
				b2++;
			}
			Dec.QuickLen[num7] = b2;
			uint num9 = num8 - Dec.DecodeLen[b2 - 1];
			num9 >>= 16 - b2;
			uint num10;
			if (b2 < Dec.DecodePos.Length && (num10 = Dec.DecodePos[b2] + num9) < Size)
			{
				Dec.QuickNum[num7] = Dec.DecodeNum[num10];
			}
			else
			{
				Dec.QuickNum[num7] = 0;
			}
		}
	}

	private bool IsFileExtracted()
	{
		return FileExtracted;
	}

	private void SetDestSize(long DestSize)
	{
		DestUnpSize = DestSize;
		FileExtracted = false;
	}

	private void SetSuspended(bool Suspended)
	{
		this.Suspended = Suspended;
	}

	private uint GetChar()
	{
		if (Inp.InAddr > 32738)
		{
			UnpReadBuf();
		}
		return Inp.InBuf[Inp.InAddr++];
	}
}
