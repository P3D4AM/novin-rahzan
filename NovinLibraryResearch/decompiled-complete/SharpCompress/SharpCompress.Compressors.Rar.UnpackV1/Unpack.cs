using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Common.Rar.Headers;
using SharpCompress.Compressors.PPMd.H;
using SharpCompress.Compressors.Rar.UnpackV1.Decode;
using SharpCompress.Compressors.Rar.UnpackV1.PPM;
using SharpCompress.Compressors.Rar.VM;

namespace SharpCompress.Compressors.Rar.UnpackV1;

internal sealed class Unpack : BitInput, IRarUnpack
{
	private readonly BitInput Inp;

	private readonly ModelPpm ppm = new ModelPpm();

	private readonly RarVM rarVM = new RarVM();

	private readonly List<UnpackFilter> filters = new List<UnpackFilter>();

	private readonly List<UnpackFilter> prgStack = new List<UnpackFilter>();

	private readonly List<int> oldFilterLengths = new List<int>();

	private int lastFilter;

	private bool tablesRead;

	private readonly byte[] unpOldTable = new byte[404];

	private BlockTypes unpBlockType;

	private long writtenFileSize;

	private bool ppmError;

	private int prevLowDist;

	private int lowDistRepCount;

	private static readonly int[] DBitLengthCounts = new int[19]
	{
		4, 2, 2, 2, 2, 2, 2, 2, 2, 2,
		2, 2, 2, 2, 2, 2, 14, 0, 12
	};

	private FileHeader fileHeader;

	private int readBorder;

	private bool suspended;

	internal bool unpAllBuf;

	private Stream readStream;

	private Stream writeStream;

	internal bool unpSomeRead;

	private int readTop;

	private long destUnpSize;

	private byte[] window;

	private readonly int[] oldDist = new int[4];

	private int unpPtr;

	private int wrPtr;

	private int oldDistPtr;

	private readonly int[] ChSet = new int[256];

	private readonly int[] ChSetA = new int[256];

	private readonly int[] ChSetB = new int[256];

	private readonly int[] ChSetC = new int[256];

	private readonly int[] Place = new int[256];

	private readonly int[] PlaceA = new int[256];

	private readonly int[] PlaceB = new int[256];

	private readonly int[] PlaceC = new int[256];

	private readonly int[] NToPl = new int[256];

	private readonly int[] NToPlB = new int[256];

	private readonly int[] NToPlC = new int[256];

	private int FlagBuf;

	private int AvrPlc;

	private int AvrPlcB;

	private int AvrLn1;

	private int AvrLn2;

	private int AvrLn3;

	private int Buf60;

	private int NumHuf;

	private int StMode;

	private int LCount;

	private int FlagsCnt;

	private int Nhfb;

	private int Nlzb;

	private int MaxDist3;

	private int lastDist;

	private int lastLength;

	private const int STARTL1 = 2;

	private static readonly int[] DecL1 = new int[11]
	{
		32768, 40960, 49152, 53248, 57344, 59904, 60928, 61440, 61952, 61952,
		65535
	};

	private static readonly int[] PosL1 = new int[13]
	{
		0, 0, 0, 2, 3, 5, 7, 11, 16, 20,
		24, 32, 32
	};

	private const int STARTL2 = 3;

	private static readonly int[] DecL2 = new int[10] { 40960, 49152, 53248, 57344, 59904, 60928, 61440, 61952, 62016, 65535 };

	private static readonly int[] PosL2 = new int[13]
	{
		0, 0, 0, 0, 5, 7, 9, 13, 18, 22,
		26, 34, 36
	};

	private const int STARTHF0 = 4;

	private static readonly int[] DecHf0 = new int[9] { 32768, 49152, 57344, 61952, 61952, 61952, 61952, 61952, 65535 };

	private static readonly int[] PosHf0 = new int[13]
	{
		0, 0, 0, 0, 0, 8, 16, 24, 33, 33,
		33, 33, 33
	};

	private const int STARTHF1 = 5;

	private static readonly int[] DecHf1 = new int[8] { 8192, 49152, 57344, 61440, 61952, 61952, 63456, 65535 };

	private static readonly int[] PosHf1 = new int[13]
	{
		0, 0, 0, 0, 0, 0, 4, 44, 60, 76,
		80, 80, 127
	};

	private const int STARTHF2 = 5;

	private static readonly int[] DecHf2 = new int[8] { 4096, 9216, 32768, 49152, 64000, 65535, 65535, 65535 };

	private static readonly int[] PosHf2 = new int[13]
	{
		0, 0, 0, 0, 0, 0, 2, 7, 53, 117,
		233, 0, 0
	};

	private const int STARTHF3 = 6;

	private static readonly int[] DecHf3 = new int[7] { 2048, 9216, 60928, 65152, 65535, 65535, 65535 };

	private static readonly int[] PosHf3 = new int[13]
	{
		0, 0, 0, 0, 0, 0, 0, 2, 16, 218,
		251, 0, 0
	};

	private const int STARTHF4 = 8;

	private static readonly int[] DecHf4 = new int[6] { 65280, 65535, 65535, 65535, 65535, 65535 };

	private static readonly int[] PosHf4 = new int[13]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 0, 255,
		0, 0, 0
	};

	private static readonly int[] ShortLen1 = new int[16]
	{
		1, 3, 4, 4, 5, 6, 7, 8, 8, 4,
		4, 5, 6, 6, 4, 0
	};

	private static readonly int[] ShortXor1 = new int[15]
	{
		0, 160, 208, 224, 240, 248, 252, 254, 255, 192,
		128, 144, 152, 156, 176
	};

	private static readonly int[] ShortLen2 = new int[16]
	{
		2, 3, 3, 3, 4, 4, 5, 6, 6, 4,
		4, 5, 6, 6, 4, 0
	};

	private static readonly int[] ShortXor2 = new int[15]
	{
		0, 64, 96, 160, 208, 224, 240, 248, 252, 192,
		128, 144, 152, 156, 176
	};

	private readonly MultDecode[] MD = new MultDecode[4];

	private readonly byte[] UnpOldTable20 = new byte[1028];

	private int UnpAudioBlock;

	private int UnpChannels;

	private int UnpCurChannel;

	private int UnpChannelDelta;

	private readonly AudioVariables[] AudV = new AudioVariables[4];

	private readonly LitDecode LD = new LitDecode();

	private readonly DistDecode DD = new DistDecode();

	private readonly LowDistDecode LDD = new LowDistDecode();

	private readonly RepDecode RD = new RepDecode();

	private readonly BitDecode BD = new BitDecode();

	private static readonly int[] LDecode = new int[28]
	{
		0, 1, 2, 3, 4, 5, 6, 7, 8, 10,
		12, 14, 16, 20, 24, 28, 32, 40, 48, 56,
		64, 80, 96, 112, 128, 160, 192, 224
	};

	private static readonly int[] DDecode = new int[48]
	{
		0, 1, 2, 3, 4, 6, 8, 12, 16, 24,
		32, 48, 64, 96, 128, 192, 256, 384, 512, 768,
		1024, 1536, 2048, 3072, 4096, 6144, 8192, 12288, 16384, 24576,
		32768, 49152, 65536, 98304, 131072, 196608, 262144, 327680, 393216, 458752,
		524288, 589824, 655360, 720896, 786432, 851968, 917504, 983040
	};

	private static readonly int[] DBits = new int[48]
	{
		0, 0, 0, 0, 1, 1, 2, 2, 3, 3,
		4, 4, 5, 5, 6, 6, 7, 7, 8, 8,
		9, 9, 10, 10, 11, 11, 12, 12, 13, 13,
		14, 14, 15, 15, 16, 16, 16, 16, 16, 16,
		16, 16, 16, 16, 16, 16, 16, 16
	};

	private static readonly int[] SDDecode = new int[8] { 0, 4, 8, 16, 32, 64, 128, 192 };

	private static readonly int[] SDBits = new int[8] { 2, 2, 3, 4, 5, 6, 6, 6 };

	private const int MAX_QUICK_DECODE_BITS = 10;

	private const int MAX_UNPACK_FILTERS = 8192;

	private const int MAX3_UNPACK_FILTERS = 8192;

	private const int MAX3_UNPACK_CHANNELS = 1024;

	private const int MAX_FILTER_BLOCK_SIZE = 4194304;

	private const int UNPACK_MAX_WRITE = 4194304;

	private bool TablesRead5;

	private int WriteBorder;

	private const int MaxWinSize = 4194304;

	private const int MaxWinMask = 4194303;

	public int BlockSize;

	public int BlockBitSize;

	public int BlockStart;

	public int HeaderSize;

	public bool LastBlockInFile;

	public bool TablePresent;

	public bool FileExtracted { get; private set; }

	public long DestSize
	{
		get
		{
			return destUnpSize;
		}
		set
		{
			destUnpSize = value;
			FileExtracted = false;
		}
	}

	public bool Suspended
	{
		get
		{
			return suspended;
		}
		set
		{
			suspended = value;
		}
	}

	public int Char
	{
		get
		{
			if (inAddr > 32738)
			{
				unpReadBuf();
			}
			return base.InBuf[inAddr++] & 0xFF;
		}
	}

	public int PpmEscChar { get; set; }

	private static ReadOnlySpan<byte> LBits => new byte[28]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		1, 1, 2, 2, 2, 2, 3, 3, 3, 3,
		4, 4, 4, 4, 5, 5, 5, 5
	};

	private int UnpPtr
	{
		get
		{
			return unpPtr;
		}
		set
		{
			unpPtr = value;
		}
	}

	private int ReadBorder
	{
		get
		{
			return readBorder;
		}
		set
		{
			readBorder = value;
		}
	}

	private long DestUnpSize
	{
		get
		{
			return destUnpSize;
		}
		set
		{
			destUnpSize = value;
		}
	}

	private long WrittenFileSize
	{
		get
		{
			return writtenFileSize;
		}
		set
		{
			writtenFileSize = value;
		}
	}

	private byte[] Window => window;

	private uint LastLength
	{
		get
		{
			return (uint)lastLength;
		}
		set
		{
			lastLength = (int)value;
		}
	}

	private int WrPtr
	{
		get
		{
			return wrPtr;
		}
		set
		{
			wrPtr = value;
		}
	}

	private Unpack BlockHeader => this;

	private Unpack Header => this;

	private int ReadTop
	{
		get
		{
			return readTop;
		}
		set
		{
			readTop = value;
		}
	}

	private List<UnpackFilter> Filters => filters;

	public Unpack()
	{
		Inp = this;
	}

	private void Init(byte[] window)
	{
		if (window == null)
		{
			this.window = new byte[4194304];
		}
		else
		{
			this.window = window;
		}
		inAddr = 0;
		UnpInitData(solid: false);
	}

	public void DoUnpack(FileHeader fileHeader, Stream readStream, Stream writeStream)
	{
		destUnpSize = fileHeader.UncompressedSize;
		this.fileHeader = fileHeader;
		this.readStream = readStream;
		this.writeStream = writeStream;
		if (!fileHeader.IsSolid)
		{
			Init(null);
		}
		suspended = false;
		DoUnpack();
	}

	public void DoUnpack()
	{
		if (fileHeader.CompressionMethod == 0)
		{
			UnstoreFile();
			return;
		}
		switch (fileHeader.CompressionAlgorithm)
		{
		case 15:
			unpack15(fileHeader.IsSolid);
			break;
		case 20:
		case 26:
			unpack20(fileHeader.IsSolid);
			break;
		case 29:
		case 36:
			Unpack29(fileHeader.IsSolid);
			break;
		case 50:
			Unpack5(fileHeader.IsSolid);
			break;
		default:
			throw new InvalidFormatException("unknown rar compression version " + fileHeader.CompressionAlgorithm);
		}
	}

	private void UnstoreFile()
	{
		byte[] array = new byte[65536];
		do
		{
			int num = readStream.Read(array, 0, (int)Math.Min(array.Length, destUnpSize));
			if (num != 0 && num != -1)
			{
				num = (int)((num < destUnpSize) ? num : destUnpSize);
				writeStream.Write(array, 0, num);
				if (destUnpSize >= 0)
				{
					destUnpSize -= num;
				}
				continue;
			}
			break;
		}
		while (!suspended);
	}

	private void Unpack29(bool solid)
	{
		int[] array = new int[60];
		byte[] array2 = new byte[60];
		if (array[1] == 0)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while (num4 < DBitLengthCounts.Length)
			{
				int num5 = DBitLengthCounts[num4];
				int num6 = 0;
				while (num6 < num5)
				{
					array[num3] = num;
					array2[num3] = (byte)num2;
					num6++;
					num3++;
					num += 1 << num2;
				}
				num4++;
				num2++;
			}
		}
		FileExtracted = true;
		if (!suspended)
		{
			UnpInitData(solid);
			if (!unpReadBuf() || ((!solid || !tablesRead) && !ReadTables()))
			{
				return;
			}
		}
		if (ppmError)
		{
			return;
		}
		while (true)
		{
			unpPtr &= 4194303;
			if (inAddr > readBorder && !unpReadBuf())
			{
				break;
			}
			if (((wrPtr - unpPtr) & 0x3FFFFF) < 260 && wrPtr != unpPtr)
			{
				UnpWriteBuf();
				if (destUnpSize <= 0)
				{
					return;
				}
				if (suspended)
				{
					FileExtracted = false;
					return;
				}
			}
			if (unpBlockType == BlockTypes.BLOCK_PPM)
			{
				int num7 = ppm.DecodeChar();
				if (num7 == -1)
				{
					ppmError = true;
					break;
				}
				if (num7 == PpmEscChar)
				{
					int num8 = ppm.DecodeChar();
					if (num8 == 0)
					{
						if (!ReadTables())
						{
							break;
						}
						continue;
					}
					if (num8 == 2 || num8 == -1)
					{
						break;
					}
					if (num8 == 3)
					{
						if (!ReadVMCodePPM())
						{
							break;
						}
						continue;
					}
					if (num8 == 4)
					{
						int num9 = 0;
						int num10 = 0;
						bool flag = false;
						for (int i = 0; i < 4; i++)
						{
							if (flag)
							{
								break;
							}
							int num11 = ppm.DecodeChar();
							if (num11 == -1)
							{
								flag = true;
							}
							else if (i == 3)
							{
								num10 = num11 & 0xFF;
							}
							else
							{
								num9 = (num9 << 8) + (num11 & 0xFF);
							}
						}
						if (flag)
						{
							break;
						}
						CopyString(num10 + 32, num9 + 2);
						continue;
					}
					if (num8 == 5)
					{
						int num12 = ppm.DecodeChar();
						if (num12 == -1)
						{
							break;
						}
						CopyString(num12 + 4, 1);
						continue;
					}
				}
				window[unpPtr++] = (byte)num7;
				continue;
			}
			int num13 = this.decodeNumber(LD);
			if (num13 < 256)
			{
				window[unpPtr++] = (byte)num13;
			}
			else if (num13 >= 271)
			{
				int num14 = LDecode[num13 -= 271] + 3;
				int num15;
				if ((num15 = LBits[num13]) > 0)
				{
					num14 += Utility.URShift(GetBits(), 16 - num15);
					AddBits(num15);
				}
				int num16 = this.decodeNumber(DD);
				int num17 = array[num16] + 1;
				if ((num15 = array2[num16]) > 0)
				{
					if (num16 > 9)
					{
						if (num15 > 4)
						{
							num17 += Utility.URShift(GetBits(), 20 - num15) << 4;
							AddBits(num15 - 4);
						}
						if (lowDistRepCount > 0)
						{
							lowDistRepCount--;
							num17 += prevLowDist;
						}
						else
						{
							int num18 = this.decodeNumber(LDD);
							if (num18 == 16)
							{
								lowDistRepCount = 15;
								num17 += prevLowDist;
							}
							else
							{
								num17 += num18;
								prevLowDist = num18;
							}
						}
					}
					else
					{
						num17 += Utility.URShift(GetBits(), 16 - num15);
						AddBits(num15);
					}
				}
				if (num17 >= 8192)
				{
					num14++;
					if ((long)num17 >= 262144L)
					{
						num14++;
					}
				}
				InsertOldDist(num17);
				InsertLastMatch(num14, num17);
				CopyString(num14, num17);
			}
			else if (num13 == 256)
			{
				if (!ReadEndOfBlock())
				{
					break;
				}
			}
			else if (num13 == 257)
			{
				if (!ReadVMCode())
				{
					break;
				}
			}
			else if (num13 == 258)
			{
				if (lastLength != 0)
				{
					CopyString(lastLength, lastDist);
				}
			}
			else if (num13 < 263)
			{
				int num19 = num13 - 259;
				int num20 = oldDist[num19];
				for (int num21 = num19; num21 > 0; num21--)
				{
					oldDist[num21] = oldDist[num21 - 1];
				}
				oldDist[0] = num20;
				int num22 = this.decodeNumber(RD);
				int num23 = LDecode[num22] + 2;
				int num15;
				if ((num15 = LBits[num22]) > 0)
				{
					num23 += Utility.URShift(GetBits(), 16 - num15);
					AddBits(num15);
				}
				InsertLastMatch(num23, num20);
				CopyString(num23, num20);
			}
			else if (num13 < 272)
			{
				int num24 = SDDecode[num13 -= 263] + 1;
				int num15;
				if ((num15 = SDBits[num13]) > 0)
				{
					num24 += Utility.URShift(GetBits(), 16 - num15);
					AddBits(num15);
				}
				InsertOldDist(num24);
				InsertLastMatch(2, num24);
				CopyString(2, num24);
			}
		}
		UnpWriteBuf();
	}

	private void UnpWriteBuf()
	{
		int num = wrPtr;
		int num2 = (unpPtr - num) & 0x3FFFFF;
		for (int i = 0; i < prgStack.Count; i++)
		{
			UnpackFilter unpackFilter = prgStack[i];
			if (unpackFilter == null)
			{
				continue;
			}
			if (unpackFilter.NextWindow)
			{
				unpackFilter.NextWindow = false;
				continue;
			}
			int blockStart = unpackFilter.BlockStart;
			int blockLength = unpackFilter.BlockLength;
			if (((blockStart - num) & 0x3FFFFF) >= num2)
			{
				continue;
			}
			if (num != blockStart)
			{
				UnpWriteArea(num, blockStart);
				num = blockStart;
				num2 = (unpPtr - num) & 0x3FFFFF;
			}
			if (blockLength <= num2)
			{
				int num3 = (blockStart + blockLength) & 0x3FFFFF;
				if (blockStart < num3 || num3 == 0)
				{
					rarVM.setMemory(0, window, blockStart, blockLength);
				}
				else
				{
					int num4 = 4194304 - blockStart;
					rarVM.setMemory(0, window, blockStart, num4);
					rarVM.setMemory(num4, window, 0, num3);
				}
				VMPreparedProgram program = filters[unpackFilter.ParentFilter].Program;
				VMPreparedProgram program2 = unpackFilter.Program;
				if (program.GlobalData.Count > 64)
				{
					program2.GlobalData.Clear();
					for (int j = 0; j < program.GlobalData.Count - 64; j++)
					{
						program2.GlobalData[64 + j] = program.GlobalData[64 + j];
					}
				}
				ExecuteCode(program2);
				if (program2.GlobalData.Count > 64)
				{
					if (program.GlobalData.Count < program2.GlobalData.Count)
					{
						program.GlobalData.SetSize(program2.GlobalData.Count);
					}
					for (int k = 0; k < program2.GlobalData.Count - 64; k++)
					{
						program.GlobalData[64 + k] = program2.GlobalData[64 + k];
					}
				}
				else
				{
					program.GlobalData.Clear();
				}
				int filteredDataOffset = program2.FilteredDataOffset;
				int filteredDataSize = program2.FilteredDataSize;
				byte[] array = new byte[filteredDataSize];
				for (int l = 0; l < filteredDataSize; l++)
				{
					array[l] = rarVM.Mem[filteredDataOffset + l];
				}
				prgStack[i] = null;
				while (i + 1 < prgStack.Count)
				{
					UnpackFilter unpackFilter2 = prgStack[i + 1];
					if (unpackFilter2 == null || unpackFilter2.BlockStart != blockStart || unpackFilter2.BlockLength != filteredDataSize || unpackFilter2.NextWindow)
					{
						break;
					}
					rarVM.setMemory(0, array, 0, filteredDataSize);
					VMPreparedProgram program3 = filters[unpackFilter2.ParentFilter].Program;
					VMPreparedProgram program4 = unpackFilter2.Program;
					if (program3.GlobalData.Count > 64)
					{
						program4.GlobalData.SetSize(program3.GlobalData.Count);
						for (int m = 0; m < program3.GlobalData.Count - 64; m++)
						{
							program4.GlobalData[64 + m] = program3.GlobalData[64 + m];
						}
					}
					ExecuteCode(program4);
					if (program4.GlobalData.Count > 64)
					{
						if (program3.GlobalData.Count < program4.GlobalData.Count)
						{
							program3.GlobalData.SetSize(program4.GlobalData.Count);
						}
						for (int n = 0; n < program4.GlobalData.Count - 64; n++)
						{
							program3.GlobalData[64 + n] = program4.GlobalData[64 + n];
						}
					}
					else
					{
						program3.GlobalData.Clear();
					}
					filteredDataOffset = program4.FilteredDataOffset;
					filteredDataSize = program4.FilteredDataSize;
					array = new byte[filteredDataSize];
					for (int num5 = 0; num5 < filteredDataSize; num5++)
					{
						array[num5] = program4.GlobalData[filteredDataOffset + num5];
					}
					i++;
					prgStack[i] = null;
				}
				writeStream.Write(array, 0, filteredDataSize);
				unpSomeRead = true;
				writtenFileSize += filteredDataSize;
				destUnpSize -= filteredDataSize;
				num = num3;
				num2 = (unpPtr - num) & 0x3FFFFF;
				continue;
			}
			for (int num6 = i; num6 < prgStack.Count; num6++)
			{
				UnpackFilter unpackFilter3 = prgStack[num6];
				if (unpackFilter3 != null && unpackFilter3.NextWindow)
				{
					unpackFilter3.NextWindow = false;
				}
			}
			wrPtr = num;
			return;
		}
		UnpWriteArea(num, unpPtr);
		wrPtr = unpPtr;
	}

	private void UnpWriteArea(int startPtr, int endPtr)
	{
		if (endPtr != startPtr)
		{
			unpSomeRead = true;
		}
		if (endPtr < startPtr)
		{
			UnpWriteData(window, startPtr, -startPtr & 0x3FFFFF);
			UnpWriteData(window, 0, endPtr);
			unpAllBuf = true;
		}
		else
		{
			UnpWriteData(window, startPtr, endPtr - startPtr);
		}
	}

	private void UnpWriteData(byte[] data, int offset, int size)
	{
		if (destUnpSize > 0)
		{
			int num = size;
			if (num > destUnpSize)
			{
				num = (int)destUnpSize;
			}
			writeStream.Write(data, offset, num);
			writtenFileSize += size;
			destUnpSize -= size;
		}
	}

	private void InsertOldDist(uint distance)
	{
		InsertOldDist((int)distance);
	}

	private void InsertOldDist(int distance)
	{
		oldDist[3] = oldDist[2];
		oldDist[2] = oldDist[1];
		oldDist[1] = oldDist[0];
		oldDist[0] = distance;
	}

	private void InsertLastMatch(int length, int distance)
	{
		lastDist = distance;
		lastLength = length;
	}

	private void CopyString(uint length, uint distance)
	{
		CopyString((int)length, (int)distance);
	}

	private void CopyString(int length, int distance)
	{
		int num = unpPtr - distance;
		if (num >= 0 && num < 4194044 && unpPtr < 4194044)
		{
			window[unpPtr++] = window[num++];
			while (--length > 0)
			{
				window[unpPtr++] = window[num++];
			}
		}
		else
		{
			while (length-- != 0)
			{
				window[unpPtr] = window[num++ & 0x3FFFFF];
				unpPtr = (unpPtr + 1) & 0x3FFFFF;
			}
		}
	}

	private void UnpInitData(bool solid)
	{
		if (!solid)
		{
			tablesRead = false;
			new Span<int>(oldDist).Clear();
			oldDistPtr = 0;
			lastDist = 0;
			lastLength = 0;
			new Span<byte>(unpOldTable).Clear();
			unpPtr = 0;
			wrPtr = 0;
			PpmEscChar = 2;
			WriteBorder = Math.Min(4194304, 4194304) & 0x3FFFFF;
			InitFilters();
		}
		InitBitInput();
		ppmError = false;
		writtenFileSize = 0L;
		readTop = 0;
		readBorder = 0;
		unpInitData20(solid);
	}

	private void InitFilters()
	{
		oldFilterLengths.Clear();
		lastFilter = 0;
		filters.Clear();
		prgStack.Clear();
	}

	private bool ReadEndOfBlock()
	{
		int bits = GetBits();
		bool flag = false;
		bool flag2;
		if (((uint)bits & 0x8000u) != 0)
		{
			flag2 = true;
			AddBits(1);
		}
		else
		{
			flag = true;
			flag2 = (bits & 0x4000) != 0;
			AddBits(2);
		}
		tablesRead = !flag2;
		if (!flag)
		{
			if (flag2)
			{
				return ReadTables();
			}
			return true;
		}
		return false;
	}

	private bool ReadTables()
	{
		byte[] array = new byte[20];
		byte[] array2 = new byte[404];
		if (inAddr > readTop - 25 && !unpReadBuf())
		{
			return false;
		}
		AddBits((8 - inBit) & 7);
		long num = GetBits() & -1;
		if ((num & 0x8000) != 0L)
		{
			unpBlockType = BlockTypes.BLOCK_PPM;
			return ppm.DecodeInit(this, PpmEscChar);
		}
		unpBlockType = BlockTypes.BLOCK_LZ;
		prevLowDist = 0;
		lowDistRepCount = 0;
		if ((num & 0x4000) == 0L)
		{
			new Span<byte>(unpOldTable).Clear();
		}
		AddBits(2);
		for (int i = 0; i < 20; i++)
		{
			int num2 = Utility.URShift(GetBits(), 12) & 0xFF;
			AddBits(4);
			if (num2 == 15)
			{
				int num3 = Utility.URShift(GetBits(), 12) & 0xFF;
				AddBits(4);
				if (num3 == 0)
				{
					array[i] = 15;
					continue;
				}
				num3 += 2;
				while (num3-- > 0 && i < array.Length)
				{
					array[i++] = 0;
				}
				i--;
			}
			else
			{
				array[i] = (byte)num2;
			}
		}
		UnpackUtility.makeDecodeTables(array, 0, BD, 20);
		int num4 = 404;
		int num5 = 0;
		while (num5 < num4)
		{
			if (inAddr > readTop - 5 && !unpReadBuf())
			{
				return false;
			}
			int num6 = this.decodeNumber(BD);
			if (num6 < 16)
			{
				array2[num5] = (byte)((uint)(num6 + unpOldTable[num5]) & 0xFu);
				num5++;
			}
			else if (num6 < 18)
			{
				int num7;
				if (num6 == 16)
				{
					num7 = Utility.URShift(GetBits(), 13) + 3;
					AddBits(3);
				}
				else
				{
					num7 = Utility.URShift(GetBits(), 9) + 11;
					AddBits(7);
				}
				while (num7-- > 0 && num5 < num4)
				{
					array2[num5] = array2[num5 - 1];
					num5++;
				}
			}
			else
			{
				int num8;
				if (num6 == 18)
				{
					num8 = Utility.URShift(GetBits(), 13) + 3;
					AddBits(3);
				}
				else
				{
					num8 = Utility.URShift(GetBits(), 9) + 11;
					AddBits(7);
				}
				while (num8-- > 0 && num5 < num4)
				{
					array2[num5++] = 0;
				}
			}
		}
		tablesRead = true;
		if (inAddr > readTop)
		{
			return false;
		}
		UnpackUtility.makeDecodeTables(array2, 0, LD, 299);
		UnpackUtility.makeDecodeTables(array2, 299, DD, 60);
		UnpackUtility.makeDecodeTables(array2, 359, LDD, 17);
		UnpackUtility.makeDecodeTables(array2, 376, RD, 28);
		Buffer.BlockCopy(array2, 0, unpOldTable, 0, unpOldTable.Length);
		return true;
	}

	private bool ReadVMCode()
	{
		int num = GetBits() >> 8;
		AddBits(8);
		int num2 = (num & 7) + 1;
		switch (num2)
		{
		case 7:
			num2 = (GetBits() >> 8) + 7;
			AddBits(8);
			break;
		case 8:
			num2 = GetBits();
			AddBits(16);
			break;
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < num2; i++)
		{
			if (inAddr >= readTop - 1 && !unpReadBuf() && i < num2 - 1)
			{
				return false;
			}
			list.Add((byte)(GetBits() >> 8));
			AddBits(8);
		}
		return AddVMCode(num, list, num2);
	}

	private bool ReadVMCodePPM()
	{
		int num = ppm.DecodeChar();
		if (num == -1)
		{
			return false;
		}
		int num2 = (num & 7) + 1;
		switch (num2)
		{
		case 7:
		{
			int num5 = ppm.DecodeChar();
			if (num5 == -1)
			{
				return false;
			}
			num2 = num5 + 7;
			break;
		}
		case 8:
		{
			int num3 = ppm.DecodeChar();
			if (num3 == -1)
			{
				return false;
			}
			int num4 = ppm.DecodeChar();
			if (num4 == -1)
			{
				return false;
			}
			num2 = num3 * 256 + num4;
			break;
		}
		}
		List<byte> list = new List<byte>();
		for (int i = 0; i < num2; i++)
		{
			int num6 = ppm.DecodeChar();
			if (num6 == -1)
			{
				return false;
			}
			list.Add((byte)num6);
		}
		return AddVMCode(num, list, num2);
	}

	private bool AddVMCode(int firstByte, List<byte> vmCode, int length)
	{
		BitInput bitInput = new BitInput();
		bitInput.InitBitInput();
		for (int i = 0; i < Math.Min(32768, vmCode.Count); i++)
		{
			bitInput.InBuf[i] = vmCode[i];
		}
		rarVM.init();
		int num;
		if (((uint)firstByte & 0x80u) != 0)
		{
			num = RarVM.ReadData(bitInput);
			if (num == 0)
			{
				InitFilters();
			}
			else
			{
				num--;
			}
		}
		else
		{
			num = lastFilter;
		}
		if (num > filters.Count || num > oldFilterLengths.Count)
		{
			return false;
		}
		lastFilter = num;
		bool flag = num == filters.Count;
		UnpackFilter unpackFilter = new UnpackFilter();
		UnpackFilter unpackFilter2;
		if (flag)
		{
			if (num > 1024)
			{
				return false;
			}
			unpackFilter2 = new UnpackFilter();
			filters.Add(unpackFilter2);
			unpackFilter.ParentFilter = filters.Count - 1;
			oldFilterLengths.Add(0);
			unpackFilter2.ExecCount = 0;
		}
		else
		{
			unpackFilter2 = filters[num];
			unpackFilter.ParentFilter = num;
			unpackFilter2.ExecCount++;
		}
		prgStack.Add(unpackFilter);
		unpackFilter.ExecCount = unpackFilter2.ExecCount;
		int num2 = RarVM.ReadData(bitInput);
		if (((uint)firstByte & 0x40u) != 0)
		{
			num2 += 258;
		}
		unpackFilter.BlockStart = (num2 + unpPtr) & 0x3FFFFF;
		if (((uint)firstByte & 0x20u) != 0)
		{
			unpackFilter.BlockLength = RarVM.ReadData(bitInput);
		}
		else
		{
			unpackFilter.BlockLength = ((num < oldFilterLengths.Count) ? oldFilterLengths[num] : 0);
		}
		unpackFilter.NextWindow = wrPtr != unpPtr && ((wrPtr - unpPtr) & 0x3FFFFF) <= num2;
		oldFilterLengths[num] = unpackFilter.BlockLength;
		new Span<int>(unpackFilter.Program.InitR).Clear();
		unpackFilter.Program.InitR[3] = 245760;
		unpackFilter.Program.InitR[4] = unpackFilter.BlockLength;
		unpackFilter.Program.InitR[5] = unpackFilter.ExecCount;
		if (((uint)firstByte & 0x10u) != 0)
		{
			int num3 = Utility.URShift(bitInput.GetBits(), 9);
			bitInput.AddBits(7);
			for (int j = 0; j < 7; j++)
			{
				if ((num3 & (1 << j)) != 0)
				{
					unpackFilter.Program.InitR[j] = RarVM.ReadData(bitInput);
				}
			}
		}
		if (flag)
		{
			int num4 = RarVM.ReadData(bitInput);
			if (num4 >= 65536 || num4 == 0)
			{
				return false;
			}
			byte[] array = new byte[num4];
			for (int k = 0; k < num4; k++)
			{
				if (bitInput.Overflow(3))
				{
					return false;
				}
				array[k] = (byte)(bitInput.GetBits() >> 8);
				bitInput.AddBits(8);
			}
			rarVM.prepare(array, num4, unpackFilter2.Program);
		}
		unpackFilter.Program.AltCommands = unpackFilter2.Program.Commands;
		unpackFilter.Program.CommandCount = unpackFilter2.Program.CommandCount;
		int count = unpackFilter2.Program.StaticData.Count;
		if (count > 0 && count < 8192)
		{
			unpackFilter.Program.StaticData = unpackFilter2.Program.StaticData;
		}
		if (unpackFilter.Program.GlobalData.Count < 64)
		{
			unpackFilter.Program.GlobalData.Clear();
			unpackFilter.Program.GlobalData.SetSize(64);
		}
		List<byte> globalData = unpackFilter.Program.GlobalData;
		for (int l = 0; l < 7; l++)
		{
			rarVM.SetLowEndianValue(globalData, l * 4, unpackFilter.Program.InitR[l]);
		}
		rarVM.SetLowEndianValue(globalData, 28, unpackFilter.BlockLength);
		rarVM.SetLowEndianValue(globalData, 32, 0);
		rarVM.SetLowEndianValue(globalData, 36, 0);
		rarVM.SetLowEndianValue(globalData, 40, 0);
		rarVM.SetLowEndianValue(globalData, 44, unpackFilter.ExecCount);
		for (int m = 0; m < 16; m++)
		{
			globalData[48 + m] = 0;
		}
		if (((uint)firstByte & 8u) != 0)
		{
			if (bitInput.Overflow(3))
			{
				return false;
			}
			int num5 = RarVM.ReadData(bitInput);
			if (num5 > 8128)
			{
				return false;
			}
			int count2 = unpackFilter.Program.GlobalData.Count;
			if (count2 < num5 + 64)
			{
				unpackFilter.Program.GlobalData.SetSize(num5 + 64 - count2);
			}
			int num6 = 64;
			globalData = unpackFilter.Program.GlobalData;
			for (int n = 0; n < num5; n++)
			{
				if (bitInput.Overflow(3))
				{
					return false;
				}
				globalData[num6 + n] = (byte)Utility.URShift(bitInput.GetBits(), 8);
				bitInput.AddBits(8);
			}
		}
		return true;
	}

	private void ExecuteCode(VMPreparedProgram Prg)
	{
		if (Prg.GlobalData.Count > 0)
		{
			Prg.InitR[6] = (int)writtenFileSize;
			rarVM.SetLowEndianValue(Prg.GlobalData, 36, (int)writtenFileSize);
			rarVM.SetLowEndianValue(Prg.GlobalData, 40, (int)Utility.URShift(writtenFileSize, 32));
			rarVM.execute(Prg);
		}
	}

	private void CleanUp()
	{
		if (ppm != null)
		{
			ppm.SubAlloc?.StopSubAllocator();
		}
	}

	private void unpack15(bool solid)
	{
		if (suspended)
		{
			unpPtr = wrPtr;
		}
		else
		{
			UnpInitData(solid);
			oldUnpInitData(solid);
			unpReadBuf();
			if (!solid)
			{
				initHuff();
				unpPtr = 0;
			}
			else
			{
				unpPtr = wrPtr;
			}
			destUnpSize--;
		}
		if (destUnpSize >= 0)
		{
			getFlagsBuf();
			FlagsCnt = 8;
		}
		while (destUnpSize >= 0)
		{
			unpPtr &= 4194303;
			if (inAddr > readTop - 30 && !unpReadBuf())
			{
				break;
			}
			if (((wrPtr - unpPtr) & 0x3FFFFF) < 270 && wrPtr != unpPtr)
			{
				oldUnpWriteBuf();
				if (suspended)
				{
					return;
				}
			}
			if (StMode != 0)
			{
				huffDecode();
				continue;
			}
			if (--FlagsCnt < 0)
			{
				getFlagsBuf();
				FlagsCnt = 7;
			}
			if (((uint)FlagBuf & 0x80u) != 0)
			{
				FlagBuf <<= 1;
				if (Nlzb > Nhfb)
				{
					longLZ();
				}
				else
				{
					huffDecode();
				}
				continue;
			}
			FlagBuf <<= 1;
			if (--FlagsCnt < 0)
			{
				getFlagsBuf();
				FlagsCnt = 7;
			}
			if (((uint)FlagBuf & 0x80u) != 0)
			{
				FlagBuf <<= 1;
				if (Nlzb > Nhfb)
				{
					huffDecode();
				}
				else
				{
					longLZ();
				}
			}
			else
			{
				FlagBuf <<= 1;
				shortLZ();
			}
		}
		oldUnpWriteBuf();
	}

	private bool unpReadBuf()
	{
		int num = readTop - inAddr;
		if (num < 0)
		{
			return false;
		}
		if (inAddr > 16384)
		{
			if (num > 0)
			{
				Array.Copy(base.InBuf, inAddr, base.InBuf, 0, num);
			}
			inAddr = 0;
			readTop = num;
		}
		else
		{
			num = readTop;
		}
		int num2 = readStream.Read(base.InBuf, num, (32768 - num) & -16);
		if (num2 > 0)
		{
			readTop += num2;
		}
		readBorder = readTop - 30;
		return num2 != -1;
	}

	private int getShortLen1(int pos)
	{
		if (pos != 1)
		{
			return ShortLen1[pos];
		}
		return Buf60 + 3;
	}

	private int getShortLen2(int pos)
	{
		if (pos != 3)
		{
			return ShortLen2[pos];
		}
		return Buf60 + 3;
	}

	private void shortLZ()
	{
		NumHuf = 0;
		int num = GetBits();
		if (LCount == 2)
		{
			AddBits(1);
			if (num >= 32768)
			{
				oldCopyString(lastDist, lastLength);
				return;
			}
			num <<= 1;
			LCount = 0;
		}
		num = Utility.URShift(num, 8);
		int i;
		if (AvrLn1 < 37)
		{
			for (i = 0; ((num ^ ShortXor1[i]) & ~Utility.URShift(255, getShortLen1(i))) != 0; i++)
			{
			}
			AddBits(getShortLen1(i));
		}
		else
		{
			for (i = 0; ((num ^ ShortXor2[i]) & ~(255 >> getShortLen2(i))) != 0; i++)
			{
			}
			AddBits(getShortLen2(i));
		}
		if (i >= 9)
		{
			int distance;
			switch (i)
			{
			case 9:
				LCount++;
				oldCopyString(lastDist, lastLength);
				return;
			case 14:
				LCount = 0;
				i = decodeNum(GetBits(), 3, DecL2, PosL2) + 5;
				distance = (GetBits() >> 1) | 0x8000;
				AddBits(15);
				lastLength = i;
				lastDist = distance;
				oldCopyString(distance, i);
				return;
			}
			LCount = 0;
			int num2 = i;
			distance = oldDist[(oldDistPtr - (i - 9)) & 3];
			i = decodeNum(GetBits(), 2, DecL1, PosL1) + 2;
			if (i == 257 && num2 == 10)
			{
				Buf60 ^= 1;
				return;
			}
			if (distance > 256)
			{
				i++;
			}
			if (distance >= MaxDist3)
			{
				i++;
			}
			oldDist[oldDistPtr++] = distance;
			oldDistPtr &= 3;
			lastLength = i;
			lastDist = distance;
			oldCopyString(distance, i);
		}
		else
		{
			LCount = 0;
			AvrLn1 += i;
			AvrLn1 -= AvrLn1 >> 4;
			int num3 = decodeNum(GetBits(), 5, DecHf2, PosHf2) & 0xFF;
			int distance = ChSetA[num3];
			if (--num3 != -1)
			{
				PlaceA[distance]--;
				int num4 = ChSetA[num3];
				PlaceA[num4]++;
				ChSetA[num3 + 1] = num4;
				ChSetA[num3] = distance;
			}
			i += 2;
			distance = (oldDist[oldDistPtr++] = distance + 1);
			oldDistPtr &= 3;
			lastLength = i;
			lastDist = distance;
			oldCopyString(distance, i);
		}
	}

	private void longLZ()
	{
		NumHuf = 0;
		Nlzb += 16;
		if (Nlzb > 255)
		{
			Nlzb = 144;
			Nhfb = Utility.URShift(Nhfb, 1);
		}
		int avrLn = AvrLn2;
		int bits = GetBits();
		int i;
		if (AvrLn2 >= 122)
		{
			i = decodeNum(bits, 3, DecL2, PosL2);
		}
		else if (AvrLn2 >= 64)
		{
			i = decodeNum(bits, 2, DecL1, PosL1);
		}
		else if (bits < 256)
		{
			i = bits;
			AddBits(16);
		}
		else
		{
			for (i = 0; ((bits << i) & 0x8000) == 0; i++)
			{
			}
			AddBits(i + 1);
		}
		AvrLn2 += i;
		AvrLn2 -= Utility.URShift(AvrLn2, 5);
		bits = GetBits();
		int num = ((AvrPlcB > 10495) ? decodeNum(bits, 5, DecHf2, PosHf2) : ((AvrPlcB <= 1791) ? decodeNum(bits, 4, DecHf0, PosHf0) : decodeNum(bits, 5, DecHf1, PosHf1)));
		AvrPlcB += num;
		AvrPlcB -= AvrPlcB >> 8;
		int num3;
		int num2;
		while (true)
		{
			num2 = ChSetB[num & 0xFF];
			num3 = NToPlB[num2++ & 0xFF]++;
			if (((uint)num2 & 0xFFu) != 0)
			{
				break;
			}
			corrHuff(ChSetB, NToPlB);
		}
		ChSetB[num] = ChSetB[num3];
		ChSetB[num3] = num2;
		num2 = Utility.URShift((num2 & 0xFF00) | Utility.URShift(GetBits(), 8), 1);
		AddBits(7);
		int avrLn2 = AvrLn3;
		if (i != 1 && i != 4)
		{
			if (i == 0 && num2 <= MaxDist3)
			{
				AvrLn3++;
				AvrLn3 -= AvrLn3 >> 8;
			}
			else if (AvrLn3 > 0)
			{
				AvrLn3--;
			}
		}
		i += 3;
		if (num2 >= MaxDist3)
		{
			i++;
		}
		if (num2 <= 256)
		{
			i += 8;
		}
		if (avrLn2 > 176 || (AvrPlc >= 10752 && avrLn < 64))
		{
			MaxDist3 = 32512;
		}
		else
		{
			MaxDist3 = 8193;
		}
		oldDist[oldDistPtr++] = num2;
		oldDistPtr &= 3;
		lastLength = i;
		lastDist = num2;
		oldCopyString(num2, i);
	}

	private void huffDecode()
	{
		int bits = GetBits();
		int num = ((AvrPlc > 30207) ? decodeNum(bits, 8, DecHf4, PosHf4) : ((AvrPlc > 24063) ? decodeNum(bits, 6, DecHf3, PosHf3) : ((AvrPlc > 13823) ? decodeNum(bits, 5, DecHf2, PosHf2) : ((AvrPlc <= 3583) ? decodeNum(bits, 4, DecHf0, PosHf0) : decodeNum(bits, 5, DecHf1, PosHf1)))));
		num &= 0xFF;
		if (StMode != 0)
		{
			if (num == 0 && bits > 4095)
			{
				num = 256;
			}
			if (--num == -1)
			{
				bits = GetBits();
				AddBits(1);
				if (((uint)bits & 0x8000u) != 0)
				{
					NumHuf = (StMode = 0);
					return;
				}
				int length = ((((uint)bits & 0x4000u) != 0) ? 4 : 3);
				AddBits(1);
				int num2 = decodeNum(GetBits(), 5, DecHf2, PosHf2);
				num2 = (num2 << 5) | Utility.URShift(GetBits(), 11);
				AddBits(5);
				oldCopyString(num2, length);
				return;
			}
		}
		else if (NumHuf++ >= 16 && FlagsCnt == 0)
		{
			StMode = 1;
		}
		AvrPlc += num;
		AvrPlc -= Utility.URShift(AvrPlc, 8);
		Nhfb += 16;
		if (Nhfb > 255)
		{
			Nhfb = 144;
			Nlzb = Utility.URShift(Nlzb, 1);
		}
		window[unpPtr++] = (byte)Utility.URShift(ChSet[num], 8);
		destUnpSize--;
		int num3;
		int num4;
		while (true)
		{
			num3 = ChSet[num];
			num4 = NToPl[num3++ & 0xFF]++;
			if ((num3 & 0xFF) <= 161)
			{
				break;
			}
			corrHuff(ChSet, NToPl);
		}
		ChSet[num] = ChSet[num4];
		ChSet[num4] = num3;
	}

	private void getFlagsBuf()
	{
		int num = decodeNum(GetBits(), 5, DecHf2, PosHf2);
		int num2;
		int num3;
		while (true)
		{
			num2 = ChSetC[num];
			FlagBuf = Utility.URShift(num2, 8);
			num3 = NToPlC[num2++ & 0xFF]++;
			if (((uint)num2 & 0xFFu) != 0)
			{
				break;
			}
			corrHuff(ChSetC, NToPlC);
		}
		ChSetC[num] = ChSetC[num3];
		ChSetC[num3] = num2;
	}

	private void oldUnpInitData(bool Solid)
	{
		if (!Solid)
		{
			AvrPlcB = (AvrLn1 = (AvrLn2 = (AvrLn3 = (NumHuf = (Buf60 = 0)))));
			AvrPlc = 13568;
			MaxDist3 = 8193;
			Nhfb = (Nlzb = 128);
		}
		FlagsCnt = 0;
		FlagBuf = 0;
		StMode = 0;
		LCount = 0;
		readTop = 0;
	}

	private void initHuff()
	{
		for (int i = 0; i < 256; i++)
		{
			Place[i] = (PlaceA[i] = (PlaceB[i] = i));
			PlaceC[i] = (~i + 1) & 0xFF;
			ChSet[i] = (ChSetB[i] = i << 8);
			ChSetA[i] = i;
			ChSetC[i] = ((~i + 1) & 0xFF) << 8;
		}
		new Span<int>(NToPl).Clear();
		new Span<int>(NToPlB).Clear();
		new Span<int>(NToPlC).Clear();
		corrHuff(ChSetB, NToPlB);
	}

	private void corrHuff(int[] CharSet, int[] NumToPlace)
	{
		int num = 0;
		for (int num2 = 7; num2 >= 0; num2--)
		{
			int num3 = 0;
			while (num3 < 32)
			{
				CharSet[num] = (CharSet[num] & -256) | num2;
				num3++;
				num++;
			}
		}
		new Span<int>(NumToPlace).Clear();
		for (int num2 = 6; num2 >= 0; num2--)
		{
			NumToPlace[num2] = (7 - num2) * 32;
		}
	}

	private void oldCopyString(int Distance, int Length)
	{
		destUnpSize -= Length;
		while (Length-- != 0)
		{
			window[unpPtr] = window[(unpPtr - Distance) & 0x3FFFFF];
			unpPtr = (unpPtr + 1) & 0x3FFFFF;
		}
	}

	private int decodeNum(int Num, int StartPos, int[] DecTab, int[] PosTab)
	{
		Num &= 0xFFF0;
		int i;
		for (i = 0; DecTab[i] <= Num; i++)
		{
			StartPos++;
		}
		AddBits(StartPos);
		return Utility.URShift(Num - ((i != 0) ? DecTab[i - 1] : 0), 16 - StartPos) + PosTab[StartPos];
	}

	private void oldUnpWriteBuf()
	{
		if (unpPtr != wrPtr)
		{
			unpSomeRead = true;
		}
		if (unpPtr < wrPtr)
		{
			writeStream.Write(window, wrPtr, -wrPtr & 0x3FFFFF);
			writeStream.Write(window, 0, unpPtr);
			unpAllBuf = true;
		}
		else
		{
			writeStream.Write(window, wrPtr, unpPtr - wrPtr);
		}
		wrPtr = unpPtr;
	}

	private void unpack20(bool solid)
	{
		if (suspended)
		{
			unpPtr = wrPtr;
		}
		else
		{
			UnpInitData(solid);
			if (!unpReadBuf() || (!solid && !ReadTables20()))
			{
				return;
			}
			destUnpSize--;
		}
		while (destUnpSize >= 0)
		{
			unpPtr &= 4194303;
			if (inAddr > readTop - 30 && !unpReadBuf())
			{
				break;
			}
			if (((wrPtr - unpPtr) & 0x3FFFFF) < 270 && wrPtr != unpPtr)
			{
				oldUnpWriteBuf();
				if (suspended)
				{
					return;
				}
			}
			if (UnpAudioBlock != 0)
			{
				int num = this.decodeNumber(MD[UnpCurChannel]);
				if (num == 256)
				{
					if (!ReadTables20())
					{
						break;
					}
					continue;
				}
				window[unpPtr++] = DecodeAudio(num);
				if (++UnpCurChannel == UnpChannels)
				{
					UnpCurChannel = 0;
				}
				destUnpSize--;
				continue;
			}
			int num2 = this.decodeNumber(LD);
			if (num2 < 256)
			{
				window[unpPtr++] = (byte)num2;
				destUnpSize--;
			}
			else if (num2 > 269)
			{
				int num3 = LDecode[num2 -= 270] + 3;
				int num4;
				if ((num4 = LBits[num2]) > 0)
				{
					num3 += Utility.URShift(GetBits(), 16 - num4);
					AddBits(num4);
				}
				int num5 = this.decodeNumber(DD);
				int num6 = DDecode[num5] + 1;
				if ((num4 = DBits[num5]) > 0)
				{
					num6 += Utility.URShift(GetBits(), 16 - num4);
					AddBits(num4);
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
				CopyString20(lastLength, lastDist);
			}
			else if (num2 < 261)
			{
				int num7 = oldDist[(oldDistPtr - (num2 - 256)) & 3];
				int num8 = this.decodeNumber(RD);
				int num9 = LDecode[num8] + 2;
				int num4;
				if ((num4 = LBits[num8]) > 0)
				{
					num9 += Utility.URShift(GetBits(), 16 - num4);
					AddBits(num4);
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
				int num10 = SDDecode[num2 -= 261] + 1;
				int num4;
				if ((num4 = SDBits[num2]) > 0)
				{
					num10 += Utility.URShift(GetBits(), 16 - num4);
					AddBits(num4);
				}
				CopyString20(2, num10);
			}
		}
		ReadLastTables();
		oldUnpWriteBuf();
	}

	private void CopyString20(int Length, int Distance)
	{
		lastDist = (oldDist[oldDistPtr++ & 3] = Distance);
		lastLength = Length;
		destUnpSize -= Length;
		int num = unpPtr - Distance;
		if (num < 4194004 && unpPtr < 4194004)
		{
			window[unpPtr++] = window[num++];
			window[unpPtr++] = window[num++];
			while (Length > 2)
			{
				Length--;
				window[unpPtr++] = window[num++];
			}
		}
		else
		{
			while (Length-- != 0)
			{
				window[unpPtr] = window[num++ & 0x3FFFFF];
				unpPtr = (unpPtr + 1) & 0x3FFFFF;
			}
		}
	}

	private bool ReadTables20()
	{
		byte[] array = new byte[19];
		byte[] array2 = new byte[1028];
		if (inAddr > readTop - 25 && !unpReadBuf())
		{
			return false;
		}
		int bits = GetBits();
		UnpAudioBlock = bits & 0x8000;
		if ((bits & 0x4000) == 0)
		{
			new Span<byte>(UnpOldTable20).Clear();
		}
		AddBits(2);
		int num;
		if (UnpAudioBlock != 0)
		{
			UnpChannels = (Utility.URShift(bits, 12) & 3) + 1;
			if (UnpCurChannel >= UnpChannels)
			{
				UnpCurChannel = 0;
			}
			AddBits(2);
			num = 257 * UnpChannels;
		}
		else
		{
			num = 374;
		}
		int i;
		for (i = 0; i < 19; i++)
		{
			array[i] = (byte)Utility.URShift(GetBits(), 12);
			AddBits(4);
		}
		UnpackUtility.makeDecodeTables(array, 0, BD, 19);
		i = 0;
		while (i < num)
		{
			if (inAddr > readTop - 5 && !unpReadBuf())
			{
				return false;
			}
			int num2 = this.decodeNumber(BD);
			if (num2 < 16)
			{
				array2[i] = (byte)((uint)(num2 + UnpOldTable20[i]) & 0xFu);
				i++;
				continue;
			}
			int num3;
			switch (num2)
			{
			case 16:
				num3 = Utility.URShift(GetBits(), 14) + 3;
				AddBits(2);
				while (num3-- > 0 && i < num)
				{
					array2[i] = array2[i - 1];
					i++;
				}
				continue;
			case 17:
				num3 = Utility.URShift(GetBits(), 13) + 3;
				AddBits(3);
				break;
			default:
				num3 = Utility.URShift(GetBits(), 9) + 11;
				AddBits(7);
				break;
			}
			while (num3-- > 0 && i < num)
			{
				array2[i++] = 0;
			}
		}
		if (inAddr > readTop)
		{
			return true;
		}
		if (UnpAudioBlock != 0)
		{
			for (i = 0; i < UnpChannels; i++)
			{
				UnpackUtility.makeDecodeTables(array2, i * 257, MD[i], 257);
			}
		}
		else
		{
			UnpackUtility.makeDecodeTables(array2, 0, LD, 298);
			UnpackUtility.makeDecodeTables(array2, 298, DD, 48);
			UnpackUtility.makeDecodeTables(array2, 346, RD, 28);
		}
		for (int j = 0; j < UnpOldTable20.Length; j++)
		{
			UnpOldTable20[j] = array2[j];
		}
		return true;
	}

	private void unpInitData20(bool Solid)
	{
		if (!Solid)
		{
			UnpChannelDelta = (UnpCurChannel = 0);
			UnpChannels = 1;
			AudV[0] = new AudioVariables();
			AudV[1] = new AudioVariables();
			AudV[2] = new AudioVariables();
			AudV[3] = new AudioVariables();
			new Span<byte>(UnpOldTable20).Clear();
		}
	}

	private void ReadLastTables()
	{
		if (readTop < inAddr + 5)
		{
			return;
		}
		if (UnpAudioBlock != 0)
		{
			if (this.decodeNumber(MD[UnpCurChannel]) == 256)
			{
				ReadTables20();
			}
		}
		else if (this.decodeNumber(LD) == 269)
		{
			ReadTables20();
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
		int num = (Utility.URShift(8 * audioVariables.LastChar + audioVariables.K1 * audioVariables.D1 + (audioVariables.K2 * audioVariables.D2 + audioVariables.K3 * audioVariables.D3) + (audioVariables.K4 * audioVariables.D4 + audioVariables.K5 * UnpChannelDelta), 3) & 0xFF) - Delta;
		int num2 = (byte)Delta << 3;
		audioVariables.Dif[0] += Math.Abs(num2);
		audioVariables.Dif[1] += Math.Abs(num2 - audioVariables.D1);
		audioVariables.Dif[2] += Math.Abs(num2 + audioVariables.D1);
		audioVariables.Dif[3] += Math.Abs(num2 - audioVariables.D2);
		audioVariables.Dif[4] += Math.Abs(num2 + audioVariables.D2);
		audioVariables.Dif[5] += Math.Abs(num2 - audioVariables.D3);
		audioVariables.Dif[6] += Math.Abs(num2 + audioVariables.D3);
		audioVariables.Dif[7] += Math.Abs(num2 - audioVariables.D4);
		audioVariables.Dif[8] += Math.Abs(num2 + audioVariables.D4);
		audioVariables.Dif[9] += Math.Abs(num2 - UnpChannelDelta);
		audioVariables.Dif[10] += Math.Abs(num2 + UnpChannelDelta);
		audioVariables.LastDelta = (byte)(num - audioVariables.LastChar);
		UnpChannelDelta = audioVariables.LastDelta;
		audioVariables.LastChar = num;
		if ((audioVariables.ByteCount & 0x1F) == 0)
		{
			int num3 = audioVariables.Dif[0];
			int num4 = 0;
			audioVariables.Dif[0] = 0;
			for (int i = 1; i < audioVariables.Dif.Length; i++)
			{
				if (audioVariables.Dif[i] < num3)
				{
					num3 = audioVariables.Dif[i];
					num4 = i;
				}
				audioVariables.Dif[i] = 0;
			}
			switch (num4)
			{
			case 1:
				if (audioVariables.K1 >= -16)
				{
					audioVariables.K1--;
				}
				break;
			case 2:
				if (audioVariables.K1 < 16)
				{
					audioVariables.K1++;
				}
				break;
			case 3:
				if (audioVariables.K2 >= -16)
				{
					audioVariables.K2--;
				}
				break;
			case 4:
				if (audioVariables.K2 < 16)
				{
					audioVariables.K2++;
				}
				break;
			case 5:
				if (audioVariables.K3 >= -16)
				{
					audioVariables.K3--;
				}
				break;
			case 6:
				if (audioVariables.K3 < 16)
				{
					audioVariables.K3++;
				}
				break;
			case 7:
				if (audioVariables.K4 >= -16)
				{
					audioVariables.K4--;
				}
				break;
			case 8:
				if (audioVariables.K4 < 16)
				{
					audioVariables.K4++;
				}
				break;
			case 9:
				if (audioVariables.K5 >= -16)
				{
					audioVariables.K5--;
				}
				break;
			case 10:
				if (audioVariables.K5 < 16)
				{
					audioVariables.K5++;
				}
				break;
			}
		}
		return (byte)num;
	}

	private uint OldDistN(int i)
	{
		return (uint)oldDist[i];
	}

	private void SetOldDistN(int i, uint value)
	{
		oldDist[i] = (int)value;
	}

	public void Unpack5(bool Solid)
	{
		FileExtracted = true;
		if (!Suspended)
		{
			UnpInitData(Solid);
			if (!UnpReadBuf() || !ReadBlockHeader() || !ReadTables() || !TablesRead5)
			{
				return;
			}
		}
		while (true)
		{
			UnpPtr &= 4194303;
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
					if (!ReadBlockHeader() || !ReadTables())
					{
						return;
					}
				}
				if (flag || !UnpReadBuf())
				{
					break;
				}
			}
			if ((long)((WriteBorder - UnpPtr) & 0x3FFFFF) < 4100L && WriteBorder != UnpPtr)
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
			uint num = this.DecodeNumber(LD);
			if (num < 256)
			{
				Window[UnpPtr++] = (byte)num;
			}
			else if (num >= 262)
			{
				uint num2 = SlotToLength(num - 262);
				uint num3 = 1u;
				uint num4 = this.DecodeNumber(DD);
				int num5;
				if (num4 < 4)
				{
					num5 = 0;
					num3 += num4;
				}
				else
				{
					num5 = (int)(num4 / 2 - 1);
					num3 += (2 | (num4 & 1)) << num5;
				}
				if (num5 > 0)
				{
					if (num5 >= 4)
					{
						if (num5 > 4)
						{
							num3 += Inp.getbits() >> 36 - num5 << 4;
							Inp.AddBits(num5 - 4);
						}
						uint num6 = this.DecodeNumber(LDD);
						num3 += num6;
					}
					else
					{
						num3 += Inp.getbits() >> 32 - num5;
						Inp.AddBits(num5);
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
				CopyString(num2, num3);
			}
			else if (num == 256)
			{
				UnpackFilter filter = new UnpackFilter();
				if (!ReadFilter(filter) || !AddFilter(filter))
				{
					break;
				}
			}
			else if (num == 257)
			{
				if (LastLength != 0)
				{
					CopyString(LastLength, OldDistN(0));
				}
			}
			else if (num < 262)
			{
				int num7 = (int)(num - 258);
				uint num8 = OldDistN(num7);
				for (int num9 = num7; num9 > 0; num9--)
				{
					SetOldDistN(num9, OldDistN(num9 - 1));
				}
				SetOldDistN(0, num8);
				uint slot = this.DecodeNumber(RD);
				uint length = (LastLength = SlotToLength(slot));
				CopyString(length, num8);
			}
		}
		UnpWriteBuf();
	}

	private uint ReadFilterData()
	{
		uint num = (Inp.fgetbits() >> 14) + 1;
		Inp.AddBits(2);
		uint num2 = 0u;
		for (int i = 0; i < num; i++)
		{
			num2 += Inp.fgetbits() >> 8 << i * 8;
			Inp.AddBits(8);
		}
		return num2;
	}

	private bool ReadFilter(UnpackFilter Filter)
	{
		if (!Inp.ExternalBuffer && Inp.InAddr > ReadTop - 16 && !UnpReadBuf())
		{
			return false;
		}
		Filter.uBlockStart = ReadFilterData();
		Filter.uBlockLength = ReadFilterData();
		if (Filter.BlockLength > 4194304)
		{
			Filter.BlockLength = 0;
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
		Filter.NextWindow = WrPtr != UnpPtr && ((WrPtr - UnpPtr) & 0x3FFFFF) <= Filter.BlockStart;
		Filter.uBlockStart = (uint)(Filter.BlockStart + UnpPtr) & 0x3FFFFFu;
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
				Array.Copy(base.InBuf, inAddr, base.InBuf, 0, num);
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
			num2 = readStream.Read(base.InBuf, num, 32768 - num);
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

	private void UnpInitData50(bool Solid)
	{
		if (!Solid)
		{
			TablesRead5 = false;
		}
	}

	private bool ReadBlockHeader()
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
		for (int i = 0; i < num; i++)
		{
			num2 += (int)(Inp.fgetbits() >> 8 << i * 8);
			Inp.AddBits(8);
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

	private uint SlotToLength(uint Slot)
	{
		uint num = 2u;
		int num2;
		if (Slot < 8)
		{
			num2 = 0;
			num += Slot;
		}
		else
		{
			num2 = (int)(Slot / 4 - 1);
			num += (4 | (Slot & 3)) << num2;
		}
		if (num2 > 0)
		{
			num += getbits() >> 16 - num2;
			AddBits(num2);
		}
		return num;
	}
}
