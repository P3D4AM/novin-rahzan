using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Stimulsoft.Base.Drawing;

public class StiFontReader
{
	public class Table
	{
		public uint Tag { get; set; }

		public string TagString { get; set; }

		public uint Checksum { get; set; }

		public uint Offset { get; set; }

		public uint Length { get; set; }
	}

	public class Head
	{
		public ushort MajorVersion { get; set; }

		public ushort MinorVersion { get; set; }

		public int FontRevision { get; set; }

		public uint ChecksumAdjustment { get; set; }

		public uint MagicNumber { get; set; }

		public ushort Flags { get; set; }

		public ushort UnitsPerEm { get; set; }

		public DateTime Created { get; set; }

		public DateTime Modified { get; set; }

		public short YMin { get; set; }

		public short XMin { get; set; }

		public short XMax { get; set; }

		public short YMax { get; set; }

		public ushort MacStyle { get; set; }

		public ushort LowestRecPPEM { get; set; }

		public short FontDirectionHint { get; set; }

		public short IndexToLocFormat { get; set; }

		public short GlyphDataFormat { get; set; }
	}

	public class NameRecord
	{
		public ushort PlatformID { get; set; }

		public ushort EncodingID { get; set; }

		public ushort LanguageID { get; set; }

		public ushort NameID { get; set; }

		public ushort Length { get; set; }

		public ushort Offset { get; set; }

		public string String { get; set; }
	}

	public class LangTagRecord
	{
		public ushort Length { get; set; }

		public ushort Offset { get; set; }
	}

	public class Name
	{
		public ushort Format { get; set; } = 1;


		public ushort Count { get; set; }

		public ushort StringOffset { get; set; }

		public List<NameRecord> NameRecord { get; set; }

		public ushort LangTagCount { get; set; }

		public List<LangTagRecord> LangTagRecord { get; set; }

		public Hashtable FontNames { get; set; }
	}

	public class EncodingRecord
	{
		public ushort PlatformID { get; set; }

		public ushort EncodingID { get; set; }

		public uint Offset { get; set; }
	}

	public class Format4
	{
		public ushort Format { get; set; }

		public ushort Length { get; set; }

		public ushort Language { get; set; }

		public ushort SegCountX2 { get; set; }

		public ushort SearchRange { get; set; }

		public ushort EntrySelector { get; set; }

		public ushort RangeShift { get; set; }

		public List<ushort> EndCode { get; set; }

		public List<ushort> StartCode { get; set; }

		public List<short> IdDelta { get; set; }

		public List<ushort> IdRangeOffset { get; set; }

		public Dictionary<uint, uint> GlyphIndexMap { get; set; }
	}

	public class Cmap
	{
		public ushort Version { get; set; }

		public ushort NumTables { get; set; }

		public List<EncodingRecord> EncodingRecords { get; set; }

		public Dictionary<uint, uint> GlyphIndexMap { get; set; }
	}

	public class Maxp
	{
		public uint Version { get; set; }

		public ushort NumGlyphs { get; set; }

		public ushort MaxPoints { get; set; }

		public ushort MaxContours { get; set; }

		public ushort MaxCompositePoints { get; set; }

		public ushort MaxCompositeContours { get; set; }

		public ushort MaxZones { get; set; }

		public ushort MaxTwilightPoints { get; set; }

		public ushort MaxStorage { get; set; }

		public ushort MaxFunctionDefs { get; set; }

		public ushort MaxInstructionDefs { get; set; }

		public ushort MaxStackElements { get; set; }

		public ushort MaxSizeOfInstructions { get; set; }

		public ushort MaxComponentElements { get; set; }

		public ushort MaxComponentDepth { get; set; }
	}

	public class Hhea
	{
		public uint Version { get; set; }

		public short Ascent { get; set; }

		public short Descent { get; set; }

		public short LineGap { get; set; }

		public ushort AdvanceWidthMax { get; set; }

		public short MinLeftSideBearing { get; set; }

		public short MinRightSideBearing { get; set; }

		public short XMaxExtent { get; set; }

		public short CaretSlopeRise { get; set; }

		public short CaretSlopeRun { get; set; }

		public short CaretOffset { get; set; }

		public short MetricDataFormat { get; set; }

		public ushort NumOfLongHorMetrics { get; set; }
	}

	public class LongHorMetric
	{
		public ushort AdvanceWidth { get; set; }

		public short LeftSideBearing { get; set; }
	}

	public class Hmtx
	{
		public List<LongHorMetric> HMetrics { get; set; }

		public List<short> LeftSideBearing { get; set; }
	}

	public class GlyphHeader
	{
		public short NumberOfContours { get; set; }

		public short XMin { get; set; }

		public short YMin { get; set; }

		public short XMax { get; set; }

		public short YMax { get; set; }
	}

	public class Glyph
	{
		public int X { get; set; }

		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public int Lsb { get; set; }

		public int Rsb { get; set; }

		public int AdvanceWidth => Lsb + Width + Rsb;
	}

	public class TtfInfo
	{
		public uint HeaderOffset;

		public uint SfntVersion;

		public ushort NumTables;

		public StiFontReader FontReader;

		public Dictionary<string, Table> Tables { get; set; }

		public Head Head { get; set; }

		public Name Name { get; set; }

		public Maxp Maxp { get; set; }

		public Hhea Hhea { get; set; }

		public Hmtx Hmtx { get; set; }

		public Cmap Cmap { get; set; }

		public Dictionary<uint, uint> GetCmapDictionary()
		{
			return FontReader.GetCmapDictionary();
		}
	}

	private uint position;

	private byte[] data;

	private TtfInfo ttf;

	public TtfInfo Ttf => ttf;

	private Head ReadHeadTable()
	{
		SetPosition(ttf.Tables["head"].Offset);
		Head head = new Head
		{
			MajorVersion = getUint16(),
			MinorVersion = getUint16(),
			FontRevision = getFixed(),
			ChecksumAdjustment = getUint32(),
			MagicNumber = getUint32(),
			Flags = getUint16(),
			UnitsPerEm = getUint16(),
			Created = getDate(),
			Modified = getDate(),
			XMin = getInt16(),
			YMin = getInt16(),
			XMax = getInt16(),
			YMax = getInt16(),
			MacStyle = getUint16(),
			LowestRecPPEM = getUint16(),
			FontDirectionHint = getInt16(),
			IndexToLocFormat = getInt16(),
			GlyphDataFormat = getInt16()
		};
		if (head.MagicNumber != 1594834165)
		{
			throw new Exception("Magic number is incorrect");
		}
		return head;
	}

	private Name ReadNameTable()
	{
		uint offset = ttf.Tables["name"].Offset;
		SetPosition(offset);
		Name name;
		switch (getUint16())
		{
		case 0:
		{
			name = new Name
			{
				Format = 0,
				Count = getUint16(),
				StringOffset = getOffset16(),
				NameRecord = new List<NameRecord>()
			};
			for (int k = 0; k < name.Count; k++)
			{
				name.NameRecord.Add(new NameRecord
				{
					PlatformID = getUint16(),
					EncodingID = getUint16(),
					LanguageID = getUint16(),
					NameID = getUint16(),
					Length = getUint16(),
					Offset = getOffset16()
				});
			}
			break;
		}
		case 1:
		{
			name = new Name
			{
				Format = 1,
				Count = getUint16(),
				StringOffset = getUint16(),
				NameRecord = new List<NameRecord>()
			};
			for (int i = 0; i < name.Count; i++)
			{
				name.NameRecord.Add(new NameRecord
				{
					PlatformID = getUint16(),
					EncodingID = getUint16(),
					LanguageID = getUint16(),
					NameID = getUint16(),
					Length = getUint16(),
					Offset = getOffset16()
				});
			}
			name.LangTagCount = getUint16();
			name.LangTagRecord = new List<LangTagRecord>();
			for (int j = 0; j < name.LangTagCount; j++)
			{
				name.LangTagRecord.Add(new LangTagRecord
				{
					Length = getUint16(),
					Offset = getOffset16()
				});
			}
			name = name;
			break;
		}
		default:
			throw new Exception("Incorrect format");
		}
		name.FontNames = new Hashtable();
		foreach (NameRecord item in name.NameRecord)
		{
			SetPosition(offset + name.StringOffset + item.Offset);
			if (item.PlatformID == 3)
			{
				item.String = GetStringUtf16(item.Length);
			}
			else
			{
				item.String = GetString(item.Length);
			}
			if ((item.PlatformID == 3 && item.EncodingID == 1 && item.NameID == 1) || (item.PlatformID == 0 && item.EncodingID == 3 && item.NameID == 1))
			{
				name.FontNames[item.String] = item.String;
			}
		}
		return name;
	}

	private Maxp ReadMaxpTable()
	{
		SetPosition(ttf.Tables["maxp"].Offset);
		return new Maxp
		{
			Version = getUint32(),
			NumGlyphs = getUint16(),
			MaxPoints = getUint16(),
			MaxContours = getUint16(),
			MaxCompositePoints = getUint16(),
			MaxCompositeContours = getUint16(),
			MaxZones = getUint16(),
			MaxTwilightPoints = getUint16(),
			MaxStorage = getUint16(),
			MaxFunctionDefs = getUint16(),
			MaxInstructionDefs = getUint16(),
			MaxStackElements = getUint16(),
			MaxSizeOfInstructions = getUint16(),
			MaxComponentElements = getUint16(),
			MaxComponentDepth = getUint16()
		};
	}

	private Hhea ReadHheaTable()
	{
		SetPosition(ttf.Tables["hhea"].Offset);
		Hhea hhea = new Hhea
		{
			Version = getUint32(),
			Ascent = getFWord(),
			Descent = getFWord(),
			LineGap = getFWord(),
			AdvanceWidthMax = getUFWord(),
			MinLeftSideBearing = getFWord(),
			MinRightSideBearing = getFWord(),
			XMaxExtent = getFWord(),
			CaretSlopeRise = getInt16(),
			CaretSlopeRun = getInt16(),
			CaretOffset = getFWord()
		};
		getInt16();
		getInt16();
		getInt16();
		getInt16();
		hhea.MetricDataFormat = getInt16();
		hhea.NumOfLongHorMetrics = getUint16();
		return hhea;
	}

	private Hmtx ReadHmtxTable(ushort numOfLongHorMetrics, uint numGlyphs)
	{
		SetPosition(ttf.Tables["hmtx"].Offset);
		List<LongHorMetric> list = new List<LongHorMetric>();
		for (int i = 0; i < numOfLongHorMetrics; i++)
		{
			list.Add(new LongHorMetric
			{
				AdvanceWidth = getUint16(),
				LeftSideBearing = getInt16()
			});
		}
		List<short> list2 = new List<short>();
		for (int j = 0; j < numGlyphs - numOfLongHorMetrics; j++)
		{
			list2.Add(getFWord());
		}
		return new Hmtx
		{
			HMetrics = list,
			LeftSideBearing = list2
		};
	}

	private Format4 ParseFormat4()
	{
		Format4 format = new Format4
		{
			Format = 4,
			Length = getUint16(),
			Language = getUint16(),
			SegCountX2 = getUint16(),
			SearchRange = getUint16(),
			EntrySelector = getUint16(),
			RangeShift = getUint16(),
			EndCode = new List<ushort>(),
			StartCode = new List<ushort>(),
			IdDelta = new List<short>(),
			IdRangeOffset = new List<ushort>(),
			GlyphIndexMap = new Dictionary<uint, uint>()
		};
		int num = format.SegCountX2 >> 1;
		for (int i = 0; i < num; i++)
		{
			format.EndCode.Add(getUint16());
		}
		getUint16();
		for (int j = 0; j < num; j++)
		{
			format.StartCode.Add(getUint16());
		}
		for (int k = 0; k < num; k++)
		{
			format.IdDelta.Add(getInt16());
		}
		uint num2 = position;
		for (int l = 0; l < num; l++)
		{
			format.IdRangeOffset.Add(getUint16());
		}
		for (int m = 0; m < num - 1; m++)
		{
			uint num3 = 0u;
			ushort num4 = format.EndCode[m];
			ushort num5 = format.StartCode[m];
			short num6 = format.IdDelta[m];
			ushort num7 = format.IdRangeOffset[m];
			for (ushort num8 = num5; num8 <= num4; num8++)
			{
				if (num7 != 0)
				{
					int num9 = (num8 - num5) * 2;
					int num10 = m * 2;
					uint num11 = (uint)(num2 + num7 + num10 + num9);
					SetPosition(num11);
					num3 = getUint16();
					if (num3 != 0)
					{
						num3 = (uint)((num3 + num6) & 0xFFFF);
					}
				}
				else
				{
					num3 = (uint)(num8 + num6) & 0xFFFFu;
				}
				format.GlyphIndexMap[num8] = (ushort)num3;
			}
		}
		return format;
	}

	public Cmap ReadCmapTable()
	{
		uint offset = ttf.Tables["cmap"].Offset;
		SetPosition(offset);
		Cmap cmap = new Cmap
		{
			Version = getUint16(),
			NumTables = getUint16(),
			EncodingRecords = new List<EncodingRecord>()
		};
		if (cmap.Version != 0)
		{
			throw new Exception($"cmap version should be 0 but is {cmap.Version}");
		}
		for (int i = 0; i < cmap.NumTables; i++)
		{
			cmap.EncodingRecords.Add(new EncodingRecord
			{
				PlatformID = getUint16(),
				EncodingID = getUint16(),
				Offset = getOffset32()
			});
		}
		return cmap;
	}

	private Dictionary<uint, uint> GetCmapDictionary()
	{
		if (ttf.Cmap.GlyphIndexMap != null)
		{
			return ttf.Cmap.GlyphIndexMap;
		}
		foreach (EncodingRecord encodingRecord in ttf.Cmap.EncodingRecords)
		{
			if (encodingRecord.PlatformID == 3 && encodingRecord.EncodingID == 1)
			{
				SetPosition(ttf.Tables["cmap"].Offset + encodingRecord.Offset);
				ushort @uint = getUint16();
				if (@uint != 4)
				{
					throw new Exception($"Unsupported format: {@uint}.Required: 4.");
				}
				ttf.Cmap.GlyphIndexMap = ParseFormat4().GlyphIndexMap;
			}
		}
		if (ttf.Cmap.GlyphIndexMap == null)
		{
			ttf.Cmap.GlyphIndexMap = new Dictionary<uint, uint>();
		}
		return ttf.Cmap.GlyphIndexMap;
	}

	private ushort[] GetCmapTable()
	{
		ushort[] array = null;
		uint offset = ttf.Tables["cmap"].Offset;
		foreach (EncodingRecord encodingRecord in ttf.Cmap.EncodingRecords)
		{
			if (encodingRecord.PlatformID != 3 || encodingRecord.EncodingID != 1)
			{
				continue;
			}
			ushort uInt = GetUInt16(offset + encodingRecord.Offset);
			if (uInt != 4)
			{
				continue;
			}
			array = new ushort[65536];
			for (int i = 0; i < 65536; i++)
			{
				array[i] = ushort.MaxValue;
			}
			uint uInt2 = GetUInt16(offset + encodingRecord.Offset + 6);
			for (int j = 0; j < uInt2; j += 2)
			{
				uint num = (uint)(offset + encodingRecord.Offset + 14 + j);
				ushort uInt3 = GetUInt16(num);
				ushort uInt4 = GetUInt16(num + 2 + uInt2);
				short num2 = (short)GetUInt16(num + 2 + uInt2 * 2);
				ushort uInt5 = GetUInt16(num + 2 + uInt2 * 3);
				if (uInt4 == ushort.MaxValue)
				{
					continue;
				}
				for (int k = uInt4; k <= uInt3; k++)
				{
					int num3 = 65535;
					if (uInt5 == 0)
					{
						num3 = num2 + k;
					}
					else
					{
						uint pos = (uint)(uInt5 + 2 * (k - uInt4) + (num + 2 + uInt2 * 3));
						num3 = GetUInt16(pos);
						if (num3 != 0)
						{
							num3 += num2;
						}
					}
					array[k] = (ushort)num3;
				}
			}
		}
		return array;
	}

	private List<uint> ReadLocaTable()
	{
		SetPosition(ttf.Tables["loca"].Offset);
		List<uint> list = new List<uint>();
		for (int i = 0; i < ttf.Maxp.NumGlyphs + 1; i++)
		{
			if (ttf.Head.IndexToLocFormat == 0)
			{
				list.Add(getOffset16());
			}
			else
			{
				list.Add(getOffset32());
			}
		}
		return list;
	}

	private List<GlyphHeader> ReadGlyfTable()
	{
		List<uint> list = ReadLocaTable();
		uint offset = ttf.Tables["glyf"].Offset;
		SetPosition(offset);
		List<GlyphHeader> list2 = new List<GlyphHeader>();
		for (int i = 0; i < list.Count - 1; i++)
		{
			int num = ((ttf.Head.IndexToLocFormat != 0) ? 1 : 2);
			long num2 = list[i] * num;
			SetPosition((uint)(offset + num2));
			list2.Add(new GlyphHeader
			{
				NumberOfContours = getInt16(),
				XMin = getInt16(),
				YMin = getInt16(),
				XMax = getInt16(),
				YMax = getInt16()
			});
		}
		return list2;
	}

	private void GetOtmInfo(byte[] buff, int offset)
	{
	}

	private byte getUint8()
	{
		return data[position++];
	}

	private ushort getUint16()
	{
		return (ushort)((getUint8() << 8) | getUint8());
	}

	private uint getUint32()
	{
		return (uint)((getUint8() << 24) | (getUint8() << 16) | (getUint8() << 8) | getUint8());
	}

	private short getInt16()
	{
		ushort @uint = getUint16();
		if ((@uint & 0x8000u) != 0)
		{
			return (short)(@uint - 65536);
		}
		return (short)@uint;
	}

	private int getInt32()
	{
		return (getUint8() << 24) | (getUint8() << 16) | (getUint8() << 8) | getUint8();
	}

	private short getFWord()
	{
		return getInt16();
	}

	private ushort getUFWord()
	{
		return getUint16();
	}

	private ushort getOffset16()
	{
		return getUint16();
	}

	private uint getOffset32()
	{
		return getUint32();
	}

	private short getF2Dot14()
	{
		return getInt16();
	}

	private int getFixed()
	{
		return getInt32();
	}

	private string GetString(int length)
	{
		byte[] array = new byte[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = getUint8();
		}
		return Encoding.UTF8.GetString(array);
	}

	private string GetStringUtf16(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length / 2; i++)
		{
			stringBuilder.Append((char)getUint16());
		}
		return stringBuilder.ToString();
	}

	private DateTime getDate()
	{
		long num = (long)getUint32() * 4294967296L + getUint32();
		return default(DateTime);
	}

	private void SetPosition(uint pos)
	{
		position = pos;
	}

	private ushort GetUInt16(uint pos)
	{
		return (ushort)((data[pos] << 8) | data[pos + 1]);
	}

	private uint GetUInt32(uint pos)
	{
		return (uint)((data[pos] << 24) | (data[pos + 1] << 16) | (data[pos + 2] << 8) | data[pos + 3]);
	}

	public TtfInfo ReadFont(int position = 0)
	{
		this.position = (uint)position;
		ttf = new TtfInfo();
		ttf.HeaderOffset = (uint)position;
		ttf.SfntVersion = getUint32();
		ttf.NumTables = getUint16();
		getUint16();
		getUint16();
		getUint16();
		ttf.Tables = new Dictionary<string, Table>();
		for (int i = 0; i < ttf.NumTables; i++)
		{
			uint @uint = getUint32();
			this.position -= 4u;
			string @string = GetString(4);
			ttf.Tables[@string] = new Table
			{
				Tag = @uint,
				TagString = @string,
				Checksum = getUint32(),
				Offset = getUint32(),
				Length = getUint32()
			};
		}
		ttf.Head = ReadHeadTable();
		ttf.Name = ReadNameTable();
		ttf.Maxp = ReadMaxpTable();
		ttf.Hhea = ReadHheaTable();
		ttf.Hmtx = ReadHmtxTable(ttf.Hhea.NumOfLongHorMetrics, ttf.Maxp.NumGlyphs);
		ttf.Cmap = ReadCmapTable();
		return ttf;
	}

	public TtfInfo ScanFontFile(string fontName)
	{
		uint uInt = GetUInt32(0u);
		if (uInt == 65536 || uInt == 1330926671)
		{
			return ReadFont();
		}
		uint uInt2 = GetUInt32(4u);
		if (uInt == 1953784678 && (uInt2 == 65536 || uInt2 == 131072))
		{
			uint uInt3 = GetUInt32(8u);
			for (uint num = uInt3 - 1; num >= 0; num--)
			{
				uint uInt4 = GetUInt32(12 + num * 4);
				uint uInt5 = GetUInt32(uInt4);
				if (uInt5 == 65536 || uInt5 == 1330926671)
				{
					ReadFont((int)uInt4);
					if (ttf.Name.FontNames.ContainsKey(fontName))
					{
						return ttf;
					}
				}
			}
		}
		return null;
	}

	public static TtfInfo ScanFontFile(byte[] data, string fontName)
	{
		StiFontReader stiFontReader = new StiFontReader(data);
		TtfInfo ttfInfo = stiFontReader.ScanFontFile(fontName);
		if (ttfInfo != null)
		{
			ttfInfo.FontReader = stiFontReader;
		}
		return ttfInfo;
	}

	public StiFontReader(byte[] data)
	{
		this.data = data;
	}
}
