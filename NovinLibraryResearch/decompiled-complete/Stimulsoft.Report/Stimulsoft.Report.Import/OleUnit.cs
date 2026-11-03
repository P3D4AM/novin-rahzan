using System;
using System.Collections;
using System.Text;

namespace Stimulsoft.Report.Import;

public class OleUnit
{
	public class OleContainer
	{
		public struct DirEntry
		{
			public string Name;

			public byte Type;

			public int DIDLeft;

			public int DIDRight;

			public int DIDRoot;

			public int SIDFirstSector;

			public int Size;
		}

		public byte[] Data;

		public DirEntry[] Dir;

		private uint BigSectorSize = 512u;

		private uint ShortSectorSize = 64u;

		private uint MinimumStreamSize = 4096u;

		private uint[] BSAT;

		private uint[] SSAT;

		private const int HeaderSize = 512;

		private const uint SIDFree = uint.MaxValue;

		private const uint SIDEoC = 4294967294u;

		private const uint SIDSAT = 4294967293u;

		private const uint SIDMSAT = 4294967292u;

		private ushort GetUInt16(uint pos)
		{
			return GetUInt16(pos);
		}

		private ushort GetUInt16(int pos)
		{
			return BitConverter.ToUInt16(Data, pos);
		}

		private uint GetUInt32(uint pos)
		{
			return GetUInt32(pos);
		}

		private uint GetUInt32(int pos)
		{
			return BitConverter.ToUInt32(Data, pos);
		}

		private int GetBigSectorOffset(uint number)
		{
			return (int)(512 + number * BigSectorSize);
		}

		private int GetShortSectorOffset(uint number)
		{
			uint[] bSatChain = GetBSatChain((uint)Dir[0].SIDFirstSector);
			uint num = BigSectorSize / ShortSectorSize;
			uint num2 = number / num;
			uint num3 = number - num2 * num;
			return (int)(GetBigSectorOffset(bSatChain[num2]) + num3 * ShortSectorSize);
		}

		private uint[] GetBSatChain(uint startSector)
		{
			ArrayList arrayList = new ArrayList();
			uint num = startSector;
			do
			{
				arrayList.Add(num);
				num = BSAT[num];
			}
			while (num != 4294967294u);
			uint[] array = new uint[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		private uint[] GetSSatChain(uint startSector)
		{
			ArrayList arrayList = new ArrayList();
			uint num = startSector;
			do
			{
				arrayList.Add(num);
				num = SSAT[num];
			}
			while (num != 4294967294u);
			uint[] array = new uint[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		private byte[] GetBigStreamData(uint[] chain)
		{
			byte[] array = new byte[chain.Length * BigSectorSize];
			for (int i = 0; i < chain.Length; i++)
			{
				int bigSectorOffset = GetBigSectorOffset(chain[i]);
				int destinationIndex = (int)(i * BigSectorSize);
				Array.Copy(Data, bigSectorOffset, array, destinationIndex, (int)BigSectorSize);
			}
			return array;
		}

		private byte[] GetShortStreamData(uint[] chain)
		{
			byte[] array = new byte[chain.Length * ShortSectorSize];
			for (int i = 0; i < chain.Length; i++)
			{
				int shortSectorOffset = GetShortSectorOffset(chain[i]);
				int destinationIndex = (int)(i * ShortSectorSize);
				Array.Copy(Data, shortSectorOffset, array, destinationIndex, (int)ShortSectorSize);
			}
			return array;
		}

		public byte[] GetStreamData(int streamNumber)
		{
			if (Dir[streamNumber].Type == 0 || Dir[streamNumber].Size == 0 || streamNumber == 0)
			{
				return null;
			}
			int size = Dir[streamNumber].Size;
			byte[] array = ((size < MinimumStreamSize) ? GetShortStreamData(GetSSatChain((uint)Dir[streamNumber].SIDFirstSector)) : GetBigStreamData(GetBSatChain((uint)Dir[streamNumber].SIDFirstSector)));
			if (array.Length > size)
			{
				byte[] array2 = new byte[size];
				Array.Copy(array, 0, array2, 0, size);
				return array2;
			}
			return array;
		}

		public void Clear()
		{
			Data = null;
			Dir = null;
			BSAT = null;
			SSAT = null;
		}

		public OleContainer(byte[] inData)
		{
			Data = inData;
			if (Data == null)
			{
				return;
			}
			BigSectorSize = (uint)(1 << (int)GetUInt16(30));
			ShortSectorSize = (uint)(1 << (int)GetUInt16(32));
			MinimumStreamSize = GetUInt32(56);
			uint uInt = GetUInt32(44);
			uint uInt2 = GetUInt32(68);
			uint uInt3 = GetUInt32(60);
			uint uInt4 = GetUInt32(48);
			uint[] array = null;
			array = new uint[uInt];
			uint num = 109u;
			int num2 = 76;
			uint num3 = uInt;
			while (num3 != 0)
			{
				array[uInt - num3] = GetUInt32(num2);
				num2 += 4;
				num--;
				num3--;
				if (num == 0)
				{
					num2 = GetBigSectorOffset((num2 == 512) ? uInt2 : GetUInt32(num2));
					num = (BigSectorSize >> 2) - 1;
				}
			}
			uint num4 = uInt * (BigSectorSize >> 2);
			BSAT = new uint[num4];
			for (int i = 0; i < uInt; i++)
			{
				int bigSectorOffset = GetBigSectorOffset(array[i]);
				for (int j = 0; j < BigSectorSize >> 2; j++)
				{
					BSAT[i * (BigSectorSize >> 2) + j] = GetUInt32(bigSectorOffset + j * 4);
				}
			}
			if (uInt3 != 4294967294u)
			{
				uint[] bSatChain = GetBSatChain(uInt3);
				uint num5 = (uint)(bSatChain.Length * (BigSectorSize >> 2));
				SSAT = new uint[num5];
				for (int k = 0; k < bSatChain.Length; k++)
				{
					int bigSectorOffset2 = GetBigSectorOffset(bSatChain[k]);
					for (int l = 0; l < BigSectorSize >> 2; l++)
					{
						SSAT[k * (BigSectorSize >> 2) + l] = GetUInt32(bigSectorOffset2 + l * 4);
					}
				}
			}
			uint[] bSatChain2 = GetBSatChain(uInt4);
			int num6 = (int)(BigSectorSize >> 7);
			uint num7 = (uint)(bSatChain2.Length * num6);
			Dir = new DirEntry[num7];
			for (int m = 0; m < num7; m++)
			{
				int num8 = m / num6;
				int num9 = m - num8 * num6;
				int num10 = GetBigSectorOffset(bSatChain2[num8]) + num9 * 128;
				Dir[m] = default(DirEntry);
				StringBuilder stringBuilder = new StringBuilder();
				for (int n = 0; n < 31; n++)
				{
					char c = BitConverter.ToChar(Data, num10 + n * 2);
					if (c == '\0')
					{
						break;
					}
					stringBuilder.Append(c);
				}
				Dir[m].Name = stringBuilder.ToString();
				Dir[m].Type = Data[num10 + 66];
				Dir[m].DIDLeft = (int)GetUInt32(num10 + 68);
				Dir[m].DIDRight = (int)GetUInt32(num10 + 72);
				Dir[m].DIDRoot = (int)GetUInt32(num10 + 76);
				Dir[m].SIDFirstSector = (int)GetUInt32(num10 + 116);
				Dir[m].Size = (int)GetUInt32(num10 + 120);
			}
		}
	}

	public class ObjectHeader
	{
		private short Signature;

		private short HeaderSize;

		private int ObjectType;

		private short NameLen;

		private short ClassLen;

		private short NameOffset;

		private short ClassOffset;

		private short ObjectSizeWidth;

		private short ObjectSizeHeight;

		private int OleVersion;

		private int Format;

		private int OleInfoLen;

		public string Name;

		public string Class;

		public string OleInfo;

		public int HeaderLen;

		private string GetString(byte[] data, int offset)
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (data[offset] != 0)
			{
				stringBuilder.Append((char)data[offset]);
				offset++;
			}
			return stringBuilder.ToString();
		}

		public ObjectHeader(byte[] data)
		{
			HeaderLen = 0;
			if (data != null && data.Length > 64 && BitConverter.ToUInt16(data, 0) == 7189)
			{
				Signature = BitConverter.ToInt16(data, 0);
				HeaderSize = BitConverter.ToInt16(data, 2);
				ObjectType = BitConverter.ToInt32(data, 4);
				NameLen = BitConverter.ToInt16(data, 8);
				ClassLen = BitConverter.ToInt16(data, 10);
				NameOffset = BitConverter.ToInt16(data, 12);
				ClassOffset = BitConverter.ToInt16(data, 14);
				ObjectSizeWidth = BitConverter.ToInt16(data, 16);
				ObjectSizeHeight = BitConverter.ToInt16(data, 18);
				OleVersion = BitConverter.ToInt32(data, HeaderSize);
				Format = BitConverter.ToInt32(data, HeaderSize + 4);
				OleInfoLen = BitConverter.ToInt32(data, HeaderSize + 8);
				Name = GetString(data, NameOffset);
				Class = GetString(data, ClassOffset);
				OleInfo = GetString(data, HeaderSize + 12);
				HeaderLen = HeaderSize + 12 + OleInfoLen + 12;
			}
		}
	}

	private const ulong OleContainerIdentifier = 16220472316735377360uL;

	private const ushort OleLinkIdentifier = 7189;

	public const int OlePresHeaderLength = 40;

	public static bool IsOleContainer(byte[] data)
	{
		if (data != null && data.Length > 64)
		{
			return BitConverter.ToUInt64(data, 0) == 16220472316735377360uL;
		}
		return false;
	}

	public static bool IsOleHeader(byte[] data)
	{
		if (data != null && data.Length > 64)
		{
			return BitConverter.ToUInt16(data, 0) == 7189;
		}
		return false;
	}

	public static bool CheckForOlePres(byte[] data)
	{
		if (data != null && data.Length > 64 && BitConverter.ToUInt32(data, 0) == uint.MaxValue && BitConverter.ToUInt32(data, 4) == 3)
		{
			return BitConverter.ToUInt32(data, 8) == 4;
		}
		return false;
	}
}
