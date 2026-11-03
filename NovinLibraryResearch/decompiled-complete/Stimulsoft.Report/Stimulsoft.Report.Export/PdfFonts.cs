using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Export;

[SuppressUnmanagedCodeSecurity]
internal class PdfFonts : PdfFontInfo
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct ABC
	{
		public int abcA;

		public uint abcB;

		public int abcC;

		internal ABC(int abcA, uint abcB, int abcC)
		{
			this.abcA = abcA;
			this.abcB = abcB;
			this.abcC = abcC;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct ABCFLOAT
	{
		public float abcfA;

		public float abcfB;

		public float abcfC;

		internal ABCFLOAT(float abcfA, float abcfB, float abcfC)
		{
			this.abcfA = abcfA;
			this.abcfB = abcfB;
			this.abcfC = abcfC;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct POINT
	{
		public int x;

		public int y;

		internal POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		internal RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct PANOSE
	{
		public byte bFamilyType;

		public byte bSerifStyle;

		public byte bWeight;

		public byte bProportion;

		public byte bContrast;

		public byte bStrokeVariation;

		public byte ArmStyle;

		public byte bLetterform;

		public byte bMidline;

		public byte bXHeight;

		internal PANOSE(byte bFamilyType, byte bSerifStyle, byte bWeight, byte bProportion, byte bContrast, byte bStrokeVariation, byte ArmStyle, byte bLetterform, byte bMidline, byte bXHeight)
		{
			this.bFamilyType = bFamilyType;
			this.bSerifStyle = bSerifStyle;
			this.bWeight = bWeight;
			this.bProportion = bProportion;
			this.bContrast = bContrast;
			this.bStrokeVariation = bStrokeVariation;
			this.ArmStyle = ArmStyle;
			this.bLetterform = bLetterform;
			this.bMidline = bMidline;
			this.bXHeight = bXHeight;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct TEXTMETRIC
	{
		public int tmHeight;

		public int tmAscent;

		public int tmDescent;

		public int tmInternalLeading;

		public int tmExternalLeading;

		public int tmAveCharWidth;

		public int tmMaxCharWidth;

		public int tmWeight;

		public int tmOverhang;

		public int tmDigitizedAspectX;

		public int tmDigitizedAspectY;

		public char tmFirstChar;

		public char tmLastChar;

		public char tmDefaultChar;

		public char tmBreakChar;

		public byte tmItalic;

		public byte tmUnderlined;

		public byte tmStruckOut;

		public byte tmPitchAndFamily;

		public byte tmCharSet;

		internal TEXTMETRIC(int tmHeight, int tmAscent, int tmDescent, int tmInternalLeading, int tmExternalLeading, int tmAveCharWidth, int tmMaxCharWidth, int tmWeight, int tmOverhang, int tmDigitizedAspectX, int tmDigitizedAspectY, char tmFirstChar, char tmLastChar, char tmDefaultChar, char tmBreakChar, byte tmItalic, byte tmUnderlined, byte tmStruckOut, byte tmPitchAndFamily, byte tmCharSet)
		{
			this.tmHeight = tmHeight;
			this.tmAscent = tmAscent;
			this.tmDescent = tmDescent;
			this.tmInternalLeading = tmInternalLeading;
			this.tmExternalLeading = tmExternalLeading;
			this.tmAveCharWidth = tmAveCharWidth;
			this.tmMaxCharWidth = tmMaxCharWidth;
			this.tmWeight = tmWeight;
			this.tmOverhang = tmOverhang;
			this.tmDigitizedAspectX = tmDigitizedAspectX;
			this.tmDigitizedAspectY = tmDigitizedAspectY;
			this.tmFirstChar = tmFirstChar;
			this.tmLastChar = tmLastChar;
			this.tmDefaultChar = tmDefaultChar;
			this.tmBreakChar = tmBreakChar;
			this.tmItalic = tmItalic;
			this.tmUnderlined = tmUnderlined;
			this.tmStruckOut = tmStruckOut;
			this.tmPitchAndFamily = tmPitchAndFamily;
			this.tmCharSet = tmCharSet;
		}
	}

	internal struct OUTLINETEXTMETRIC
	{
		public uint otmSize;

		public TEXTMETRIC otmTextMetrics;

		public byte otmFiller;

		public PANOSE otmPanoseNumber;

		public uint otmfsSelection;

		public uint otmfsType;

		public int otmsCharSlopeRise;

		public int otmsCharSlopeRun;

		public int otmItalicAngle;

		public uint otmEMSquare;

		public int otmAscent;

		public int otmDescent;

		public uint otmLineGap;

		public uint otmsCapEmHeight;

		public uint otmsXHeight;

		public RECT otmrcFontBox;

		public int otmMacAscent;

		public int otmMacDescent;

		public uint otmMacLineGap;

		public uint otmusMinimumPPEM;

		public POINT otmptSubscriptSize;

		public POINT otmptSubscriptOffset;

		public POINT otmptSuperscriptSize;

		public POINT otmptSuperscriptOffset;

		public uint otmsStrikeoutSize;

		public int otmsStrikeoutPosition;

		public int otmsUnderscoreSize;

		public int otmsUnderscorePosition;

		public IntPtr otmpFamilyName;

		public IntPtr otmpFaceName;

		public IntPtr otmpStyleName;

		public IntPtr otmpFullName;
	}

	internal struct pfontInfo
	{
		public string Name;

		public string PdfName;

		public bool Bold;

		public bool Italic;

		public int Number;

		public Font Font;

		public int ParentFontNumber;

		public byte[] ChildFontsMap;

		public bool UseUnicode;
	}

	internal class StiOpenTypeHelper
	{
		public struct NewTableItem
		{
			public uint Tag;

			public uint CheckSum;

			public uint NewOffset;

			public uint NewLength;

			public byte[] NewTable;
		}

		private const int TtfHeaderSize = 12;

		private const ushort ARG_1_AND_2_ARE_WORDS = 1;

		private const ushort ARGS_ARE_XY_VALUES = 2;

		private const ushort ROUND_XY_TO_GRID = 4;

		private const ushort WE_HAVE_A_SCALE = 8;

		private const ushort MORE_COMPONENTS = 32;

		private const ushort WE_HAVE_AN_X_AND_Y_SCALE = 64;

		private const ushort WE_HAVE_A_TWO_BY_TWO = 128;

		private const ushort WE_HAVE_INSTRUCTIONS = 256;

		private const ushort USE_MY_METRICS = 512;

		private const ushort OVERLAP_COMPOUND = 1024;

		private const ushort SCALED_COMPONENT_OFFSET = 2048;

		private const ushort UNSCALED_COMPONENT_OFFSET = 4096;

		private byte[] bufGetData = new byte[4];

		private static List<string> RequiredTablesNames = new List<string>
		{
			"head", "hhea", "hmtx", "maxp", "cmap", "OS/2", "post", "cvt ", "fpgm", "glyf",
			"loca", "prep", "name"
		};

		public void ReduceFontSize(ref byte[] buff, string fontName, bool remakeGlyphTable, ushort[] GlyphList, ushort[] GlyphRtfList)
		{
			try
			{
				TtfInfo val = StiFontReader.ScanFontFile(buff, fontName);
				if (val == null)
				{
					return;
				}
				List<string> list = new List<string>(RequiredTablesNames);
				if (!val.Tables.ContainsKey("glyf"))
				{
					remakeGlyphTable = false;
					list.Add("CFF ");
				}
				int num = 0;
				foreach (KeyValuePair<string, Table> table in val.Tables)
				{
					if (list.Contains(table.Key))
					{
						num++;
					}
				}
				Dictionary<string, NewTableItem> dictionary = new Dictionary<string, NewTableItem>();
				if (remakeGlyphTable)
				{
					bool[] array = new bool[val.Maxp.NumGlyphs];
					bool[] array2 = new bool[val.Maxp.NumGlyphs];
					foreach (ushort num2 in GlyphList)
					{
						if (num2 < val.Maxp.NumGlyphs)
						{
							array[num2] = true;
							array2[num2] = true;
						}
					}
					if (GlyphRtfList != null)
					{
						foreach (ushort num3 in GlyphRtfList)
						{
							if (num3 < val.Maxp.NumGlyphs)
							{
								array[num3] = true;
								array2[num3] = true;
							}
						}
					}
					array[0] = true;
					array[1] = true;
					array[2] = true;
					array[3] = true;
					array2[0] = true;
					array2[1] = true;
					array2[2] = true;
					array2[3] = true;
					Table val2 = val.Tables["loca"];
					Table val3 = val.Tables["glyf"];
					bool flag = true;
					while (flag)
					{
						flag = false;
						for (int k = 0; k < val.Maxp.NumGlyphs; k++)
						{
							if (!array2[k])
							{
								continue;
							}
							array2[k] = false;
							int pos = (int)(val2.Offset + k * ((val.Head.IndexToLocFormat == 1) ? 4 : 2));
							int num4 = 0;
							num4 = ((val.Head.IndexToLocFormat != 1) ? (2 * GetUInt16(buff, pos)) : ((int)GetUInt32(buff, pos)));
							num4 += (int)val3.Offset;
							if (GetInt16(buff, num4) != -1)
							{
								continue;
							}
							flag = true;
							num4 += 10;
							ushort num5 = 0;
							do
							{
								num5 = GetUInt16(buff, num4);
								ushort uInt = GetUInt16(buff, num4 + 2);
								if (uInt < val.Maxp.NumGlyphs)
								{
									array[uInt] = true;
									array2[uInt] = true;
								}
								num4 += 4;
								if ((num5 & 1) > 0)
								{
									num4 += 2;
									num4 += 2;
								}
								else
								{
									num4 += 2;
								}
								if ((num5 & 8) > 0)
								{
									num4 += 2;
								}
								else if ((num5 & 0x40) > 0)
								{
									num4 += 2;
									num4 += 2;
								}
								else if ((num5 & 0x80) > 0)
								{
									num4 += 2;
									num4 += 2;
									num4 += 2;
									num4 += 2;
								}
							}
							while ((num5 & 0x20) > 0);
						}
					}
					int num6 = 0;
					int num7 = (int)val2.Offset;
					for (int l = 0; l < val.Maxp.NumGlyphs; l++)
					{
						int num8 = 0;
						int num9 = 0;
						if (val.Head.IndexToLocFormat == 1)
						{
							num8 = (int)GetUInt32(buff, num7);
							num9 = (int)GetUInt32(buff, num7 + 4);
							num7 += 4;
						}
						else
						{
							num8 = 2 * GetUInt16(buff, num7);
							num9 = 2 * GetUInt16(buff, num7 + 2);
							num7 += 2;
						}
						if (array[l])
						{
							num6 += num9 - num8;
						}
					}
					byte[] array3 = new byte[num6 + 4];
					if (val3.Offset + val3.Length + 4 < buff.Length)
					{
						Array.Copy(buff, val3.Offset + val3.Length, array3, num6, 4L);
					}
					byte[] array4 = new byte[val2.Length + 4];
					if (val2.Offset + val2.Length + 4 < buff.Length)
					{
						Array.Copy(buff, val2.Offset + val2.Length, array4, val2.Length, 4L);
					}
					uint num10 = 0u;
					num7 = (int)val2.Offset;
					int num11 = 0;
					for (int m = 0; m < val.Maxp.NumGlyphs; m++)
					{
						int num12 = 0;
						int num13 = 0;
						if (val.Head.IndexToLocFormat == 1)
						{
							num12 = (int)GetUInt32(buff, num7);
							num13 = (int)GetUInt32(buff, num7 + 4);
							num7 += 4;
						}
						else
						{
							num12 = 2 * GetUInt16(buff, num7);
							num13 = 2 * GetUInt16(buff, num7 + 2);
							num7 += 2;
						}
						int num14 = num13 - num12;
						if (array[m])
						{
							for (int n = 0; n < num14; n++)
							{
								array3[num10 + n] = buff[val3.Offset + num12 + n];
							}
						}
						else
						{
							num14 = 0;
						}
						if (val.Head.IndexToLocFormat == 0)
						{
							SetUInt16(array4, num11, (ushort)(num10 / 2));
						}
						else
						{
							SetUInt32(array4, num11, num10);
						}
						num10 += (uint)num14;
						num11 += ((val.Head.IndexToLocFormat == 1) ? 4 : 2);
					}
					if (val.Head.IndexToLocFormat == 0)
					{
						SetUInt16(array4, num11, (ushort)(num10 / 2));
					}
					else
					{
						SetUInt32(array4, num11, num10);
					}
					NewTableItem newTableItem = default(NewTableItem);
					newTableItem.Tag = val3.Tag;
					newTableItem.NewLength = num10;
					newTableItem.NewTable = array3;
					NewTableItem value = newTableItem;
					newTableItem = default(NewTableItem);
					newTableItem.Tag = val2.Tag;
					newTableItem.NewLength = val2.Length;
					newTableItem.NewTable = array4;
					NewTableItem value2 = newTableItem;
					dictionary["glyf"] = value;
					dictionary["loca"] = value2;
				}
				byte[] array5 = new byte[12 + num * 16];
				Array.Copy(buff, val.HeaderOffset, array5, 0L, array5.Length);
				int num15;
				for (num15 = 1; 2 << num15 - 1 <= num; num15++)
				{
				}
				num15--;
				int num16 = (2 << num15 - 1) * 16;
				int num17 = num15;
				int num18 = num * 16 - num16;
				SetUInt16(array5, 4, (ushort)num);
				SetUInt16(array5, 6, (ushort)num16);
				SetUInt16(array5, 8, (ushort)num17);
				SetUInt16(array5, 10, (ushort)num18);
				uint num19 = 0u;
				uint num20 = (uint)(12 + num * 16);
				foreach (Table value3 in val.Tables.Values)
				{
					if (list.Contains(value3.TagString))
					{
						uint num21 = value3.Length;
						if (dictionary.ContainsKey(value3.TagString))
						{
							num21 = dictionary[value3.TagString].NewLength;
						}
						int num22 = (int)(12 + num19 * 16);
						SetUInt32(array5, num22, value3.Tag);
						SetUInt32(array5, num22 + 4, value3.Checksum);
						SetUInt32(array5, num22 + 8, num20);
						SetUInt32(array5, num22 + 12, num21);
						num20 += num21;
						uint num23 = num20 % 4;
						if (num23 != 0)
						{
							num20 += 4 - num23;
						}
						num19++;
					}
				}
				using MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(array5, 0, array5.Length);
				foreach (Table value4 in val.Tables.Values)
				{
					if (list.Contains(value4.TagString))
					{
						uint num24 = value4.Length;
						if (dictionary.ContainsKey(value4.TagString))
						{
							num24 = dictionary[value4.TagString].NewLength;
							memoryStream.Write(dictionary[value4.TagString].NewTable, 0, (int)num24);
						}
						else
						{
							memoryStream.Write(buff, (int)value4.Offset, (int)value4.Length);
						}
						int num25 = (int)num24 % 4;
						if (num25 > 0)
						{
							memoryStream.Write(new byte[4 - num25], 0, 4 - num25);
						}
					}
				}
				memoryStream.Flush();
				buff = memoryStream.ToArray();
				memoryStream.Close();
			}
			catch
			{
			}
		}

		public Dictionary<uint, uint> GetCharToGlyphTable(byte[] buff, string fontName)
		{
			TtfInfo val = StiFontReader.ScanFontFile(buff, fontName);
			if (val != null)
			{
				return val.GetCmapDictionary();
			}
			return null;
		}

		private ushort GetUInt16(byte[] buff, int pos)
		{
			bufGetData[0] = buff[pos + 1];
			bufGetData[1] = buff[pos];
			return BitConverter.ToUInt16(bufGetData, 0);
		}

		private uint GetUInt32(byte[] buff, int pos)
		{
			bufGetData[0] = buff[pos + 3];
			bufGetData[1] = buff[pos + 2];
			bufGetData[2] = buff[pos + 1];
			bufGetData[3] = buff[pos];
			return BitConverter.ToUInt32(bufGetData, 0);
		}

		private short GetInt16(byte[] buff, int pos)
		{
			return (short)GetUInt16(buff, pos);
		}

		private void SetUInt16(byte[] buff, int pos, ushort value)
		{
			BitConverter.GetBytes(value).CopyTo(bufGetData, 0);
			buff[pos] = bufGetData[1];
			buff[pos + 1] = bufGetData[0];
		}

		private void SetUInt32(byte[] buff, int pos, uint value)
		{
			BitConverter.GetBytes(value).CopyTo(bufGetData, 0);
			buff[pos] = bufGetData[3];
			buff[pos + 1] = bufGetData[2];
			buff[pos + 2] = bufGetData[1];
			buff[pos + 3] = bufGetData[0];
		}
	}

	internal const uint GDI_ERROR = uint.MaxValue;

	internal const ushort FirstMappedSymbol = 64;

	internal const float Factor = 0.366f;

	internal ushort MaxSymbols = 32767;

	internal bool forceUseUnicode = true;

	private OUTLINETEXTMETRIC otm;

	private int[] WIDTHS;

	internal PdfFontInfo[] fonts;

	public ArrayList UnicodeMapsList;

	public ArrayList GlyphMapsList;

	public bool standardPdfFonts = true;

	public ArrayList fontList;

	public Hashtable fontHash = new Hashtable();

	private FontsInfoStore fontsInfoStore;

	internal StiOpenTypeHelper OpenTypeHelper = new StiOpenTypeHelper();

	private static ushort[,] StandardFontWidths = null;

	private static string[] StandardFontCharsNames = null;

	private static int[,] StandardFontInfo = null;

	private static int[] uniMap = null;

	private static Hashtable FontsDataCache = new Hashtable();

	private Hashtable localFontsDataCache = new Hashtable();

	private int currentFont = -1;

	private const int StandardFontQuantity = 12;

	private const int StandardFontNumWidths = 310;

	private static string[] family_Helvetica = new string[9] { "Arial", "Arial Black", "Arial Narrow", "Century Gothic", "Comic Sans MS", "Impact", "Microsoft Sans Serif", "Tahoma", "Verdana" };

	private static string[] family_Courier = new string[2] { "Courier New", "Lucida Console" };

	private static string[] family_Times_Roman = new string[5] { "Book Antiqua", "Bookman Old Style", "Garamond", "Georgia", "Times New Roman" };

	private static string[] family_Symbol = new string[1] { "Symbol" };

	private static string[] family_ZapfDingbats = new string[1] { "Wingdings 2" };

	private static string[] PdfFontName = new string[14]
	{
		"Helvetica", "Helvetica-Bold", "Helvetica-Oblique", "Helvetica-BoldOblique", "Times-Roman", "Times-Bold", "Times-Italic", "Times-BoldItalic", "Courier", "Courier-Bold",
		"Courier-Oblique", "Courier-BoldOblique", "Symbol", "ZapfDingbats"
	};

	public int CurrentFont
	{
		get
		{
			return currentFont;
		}
		set
		{
			if (currentFont != value)
			{
				currentFont = value;
				if (fonts != null && fonts.Length != 0)
				{
					Widths = fonts[currentFont].Widths;
					CH = fonts[currentFont].CH;
					XH = fonts[currentFont].XH;
					ASC = fonts[currentFont].ASC;
					DESC = fonts[currentFont].DESC;
					tmASC = fonts[currentFont].tmASC;
					tmDESC = fonts[currentFont].tmDESC;
					tmExternal = fonts[currentFont].tmExternal;
					MacAscend = fonts[currentFont].MacAscend;
					MacDescend = fonts[currentFont].MacDescend;
					MacLineGap = fonts[currentFont].MacLineGap;
					LLX = fonts[currentFont].LLX;
					LLY = fonts[currentFont].LLY;
					URX = fonts[currentFont].URX;
					URY = fonts[currentFont].URY;
					StemV = fonts[currentFont].StemV;
					ItalicAngle = fonts[currentFont].ItalicAngle;
					LineGap = fonts[currentFont].LineGap;
					fsSelection = fonts[currentFont].fsSelection;
					UnderscoreSize = fonts[currentFont].UnderscoreSize;
					UnderscorePosition = fonts[currentFont].UnderscorePosition;
					StrikeoutSize = fonts[currentFont].StrikeoutSize;
					StrikeoutPosition = fonts[currentFont].StrikeoutPosition;
					UnicodeMap = fonts[currentFont].UnicodeMap;
					UnicodeMapBack = fonts[currentFont].UnicodeMapBack;
					GlyphList = fonts[currentFont].GlyphList;
					GlyphBackList = fonts[currentFont].GlyphBackList;
					GlyphRtfList = fonts[currentFont].GlyphRtfList;
					SymsToPDF = fonts[currentFont].SymsToPDF;
					CharPdfNames = fonts[currentFont].CharPdfNames;
					MappedSymbolsCount = fonts[currentFont].MappedSymbolsCount;
					NeedSyntBold = fonts[currentFont].NeedSyntBold;
					NeedSyntItalic = fonts[currentFont].NeedSyntItalic;
					GlyphWidths = fonts[currentFont].GlyphWidths;
					ChildFontsMap = fonts[currentFont].ChildFontsMap;
					UseUnicode = fonts[currentFont].UseUnicode;
				}
			}
		}
	}

	[DllImport("GDI32.dll", SetLastError = true)]
	internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern uint GetOutlineTextMetrics(IntPtr hdc, uint cbData, IntPtr lpOTM);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern uint GetGlyphIndices(IntPtr hdc, string lpstr, int c, [In][Out] ushort[] pgi, uint fl);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool GetCharABCWidthsI(IntPtr hdc, uint giFirst, uint cgi, [In][Out] ushort[] pgi, [In][Out] ABC[] lpabc);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern uint GetFontData(IntPtr hdc, uint dwTable, uint dwOffset, [In][Out] byte[] lpvBuffer, uint cbData);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern uint GetFontData(IntPtr hdc, uint dwTable, uint dwOffset, [In][Out] IntPtr lpvBuffer, uint cbData);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool DeleteObject(IntPtr objectHandle);

	public ushort[] MakeGlyphBackMap(Font font, bool useFontInfoStore)
	{
		if (font == null)
		{
			return null;
		}
		ushort[] array = null;
		FontsInfoStore.FontInfo fontInfo = null;
		if (useFontInfoStore || StiOptions.Export.Pdf.ForceUseFontsInfoStoreForWysiwyg)
		{
			fontInfo = fontsInfoStore.GetFontInfo(font);
		}
		if (fontInfo == null)
		{
			if (StiOptions.Engine.FullTrust)
			{
				object fontGlyphs = StiTextRenderer.GetFontGlyphs(font);
				ushort[] array2 = fontGlyphs as ushort[];
				if (array2 == null || array2.Length == 0)
				{
					ThrowError(1);
				}
				ushort[] array3 = new ushort[65536];
				for (int num = 65535; num >= 32; num--)
				{
					int num2 = array2[num];
					array3[num2] = (ushort)num;
				}
				return array3;
			}
			byte[] buff = null;
			GetFontDataBuf(font, out buff, useFontInfoStore: false);
			Dictionary<uint, uint> charToGlyphTable = OpenTypeHelper.GetCharToGlyphTable(buff, font.Name);
			ushort[] array4 = new ushort[65536];
			for (uint num3 = 65535u; num3 >= 32; num3--)
			{
				uint num4 = (charToGlyphTable.ContainsKey(num3) ? charToGlyphTable[num3] : 65535u);
				array4[num4] = (ushort)num3;
			}
			return array4;
		}
		ushort[] glyphs = fontInfo.Glyphs;
		ushort[] array5 = new ushort[65536];
		for (int num5 = 65534; num5 >= 32; num5--)
		{
			int num6 = glyphs[num5];
			array5[num6] = (ushort)num5;
		}
		return array5;
	}

	public void GetFontMetrics(Font font, PdfFontInfo currentFontInfo, ushort[] glyphMap, bool isWpf)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		otm = default(OUTLINETEXTMETRIC);
		WIDTHS = new int[MaxSymbols + 1];
		if (font == null)
		{
			return;
		}
		if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries && !IsFontStimulsoft(font.Name))
		{
			GetFontMetricsFullTrust(font, currentFontInfo, glyphMap, isWpf);
			return;
		}
		int num = 0;
		try
		{
			Font val = new Font(font.FontFamily, 2048f, font.Style);
			try
			{
				Image val2 = (Image)new Bitmap(1, 1);
				try
				{
					Graphics val3 = Graphics.FromImage(val2);
					try
					{
						StringFormat val4 = new StringFormat(StringFormat.GenericDefault);
						try
						{
							val4.FormatFlags = (StringFormatFlags)(val4.FormatFlags | 0x1000);
							string text = "0\n1";
							val4.SetMeasurableCharacterRanges((CharacterRange[])(object)new CharacterRange[1]
							{
								new CharacterRange(2, 1)
							});
							num = 10;
							Region[] array = val3.MeasureCharacterRanges(text, val, new RectangleF(0f, 0f, 10000000f, 10000000f), val4);
							RectangleF bounds = array[0].GetBounds(val3);
							num = 20;
							double num2 = bounds.Height;
							double num3 = bounds.Y;
							double num4 = 0.0;
							if (!StiOptions.Export.Pdf.AllowImportSystemLibraries)
							{
								num2 = (double)(val3.MeasureString("D\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD", val, 10000000, val4).Height / 10f) * 0.987;
								num3 = num2;
								num4 = val3.MeasureString("DD", val, 10000000, val4).Width;
							}
							if (StiOptions.Engine.TextLineSpacingScale != 1f)
							{
								num2 *= (double)StiOptions.Engine.TextLineSpacingScale;
								num3 *= (double)StiOptions.Engine.TextLineSpacingScale;
							}
							otm.otmTextMetrics.tmAscent = (int)(num2 * 0.8);
							otm.otmTextMetrics.tmDescent = (int)(num2 * 0.2);
							otm.otmTextMetrics.tmExternalLeading = (int)(num3 - num2);
							otm.otmsStrikeoutPosition = (int)(num2 * 0.25);
							otm.otmsStrikeoutSize = (uint)(num2 * 0.05);
							otm.otmsUnderscorePosition = -(int)(num2 * 0.1);
							otm.otmsUnderscoreSize = (int)(num2 * 0.07);
							num = 30;
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < currentFontInfo.MappedSymbolsCount; i++)
							{
								stringBuilder.Append((char)currentFontInfo.UnicodeMapBack[i]);
							}
							text = stringBuilder.ToString();
							array = (Region[])(object)new Region[text.Length];
							num = 40;
							int num5;
							for (int j = 0; j < text.Length; j += num5)
							{
								num5 = text.Length - j;
								if (num5 > 32)
								{
									num5 = 32;
								}
								CharacterRange[] array2 = (CharacterRange[])(object)new CharacterRange[num5];
								for (int k = 0; k < num5; k++)
								{
									((CharacterRange)(ref array2[k])).First = j + k;
									((CharacterRange)(ref array2[k])).Length = 1;
								}
								val4.SetMeasurableCharacterRanges(array2);
								val3.MeasureCharacterRanges(text, val, new RectangleF(0f, 0f, 10000000f, 10000000f), val4).CopyTo(array, j);
							}
							num = 50;
							for (int l = 0; l < currentFontInfo.MappedSymbolsCount; l++)
							{
								double num6 = 0.0;
								if (StiOptions.Export.Pdf.AllowImportSystemLibraries)
								{
									num6 = (double)(array[l].GetBounds(val3).Width * 0.366f) * StiDpiHelper.GraphicsScale;
								}
								else
								{
									double num7 = (double)val3.MeasureString("D" + text.Substring(l, 1) + "D", val, 10000000, val4).Width - num4;
									num6 = num7 * 0.971 * 0.3659999966621399 * StiDpiHelper.GraphicsScale;
								}
								if (char.GetUnicodeCategory(text[l]) == UnicodeCategory.NonSpacingMark)
								{
									num6 = 0.0;
								}
								WIDTHS[l] = (int)Math.Round(num6);
							}
							num = 60;
							if (!currentFontInfo.UseUnicode)
							{
								return;
							}
							byte[] buff = null;
							GetFontDataBuf(font, out buff, glyphMap[65535] == 0);
							if (buff == null)
							{
								throw new Exception("Font data not found.");
							}
							num = 70;
							Dictionary<uint, uint> charToGlyphTable = OpenTypeHelper.GetCharToGlyphTable(buff, font.Name);
							if (charToGlyphTable == null || charToGlyphTable.Count == 0)
							{
								throw new Exception("CMAP table not found.");
							}
							num = 80;
							uint num8 = 0u;
							if (font.Name == "Wingdings" || font.Name == "Wingdings 2" || font.Name == "Wingdings 3")
							{
								num8 = 61440u;
							}
							for (int m = 0; m < currentFontInfo.MappedSymbolsCount; m++)
							{
								uint num9 = currentFontInfo.UnicodeMapBack[m];
								if (num9 >= 128 && num9 <= 255)
								{
									num9 += num8;
								}
								currentFontInfo.GlyphList[m] = (ushort)(charToGlyphTable.ContainsKey(num9) ? charToGlyphTable[num9] : 65535u);
							}
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			string message = $"Exception in PdfFonts.GetFontMetrics, font '{font.Name}' step #{num}: '{ex.Message}'";
			StiLogService.Write(GetType(), message);
			StiLogService.Write(GetType(), ex.Message);
			throw new Exception(message);
		}
	}

	public void GetFontMetricsFullTrust(Font font, PdfFontInfo currentFontInfo, ushort[] glyphMap, bool isWpf)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		FontsInfoStore.FontInfo fontInfo = null;
		if (glyphMap[65535] == 0 || StiOptions.Export.Pdf.ForceUseFontsInfoStoreForWysiwyg)
		{
			fontInfo = fontsInfoStore.GetFontInfo(font);
		}
		if (fontInfo == null)
		{
			Graphics val = (StiOptions.Export.Pdf.AllowImportSystemLibraries ? Graphics.FromHwnd(IntPtr.Zero) : Graphics.FromImage((Image)new Bitmap(1, 1)));
			try
			{
				IntPtr hdc = val.GetHdc();
				try
				{
					Font val2 = new Font(font.FontFamily, 2048f, font.Style);
					try
					{
						IntPtr intPtr = val2.ToHfont();
						try
						{
							IntPtr hgdiobj = SelectObject(hdc, intPtr);
							uint outlineTextMetrics = GetOutlineTextMetrics(hdc, 0u, IntPtr.Zero);
							if (outlineTextMetrics == 0)
							{
								ThrowError(3);
							}
							IntPtr intPtr2 = Marshal.AllocHGlobal((int)outlineTextMetrics);
							try
							{
								if (GetOutlineTextMetrics(hdc, outlineTextMetrics, intPtr2) != 0)
								{
									otm = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(intPtr2, typeof(OUTLINETEXTMETRIC));
									long num = intPtr2.ToInt64();
									string text = Marshal.PtrToStringUni(new IntPtr(num + (long)otm.otmpFullName));
									string text2 = Marshal.PtrToStringUni(new IntPtr(num + (long)otm.otmpFamilyName));
									string text3 = Marshal.PtrToStringUni(new IntPtr(num + (long)otm.otmpStyleName));
									bool flag = text.ToLowerInvariant().IndexOf("bold", StringComparison.InvariantCulture) != -1 || text3.ToLowerInvariant().IndexOf("bold", StringComparison.InvariantCulture) != -1 || text3.ToLowerInvariant().IndexOf("полужирный", StringComparison.InvariantCulture) != -1;
									bool flag2 = text.ToLowerInvariant().IndexOf("italic", StringComparison.InvariantCulture) != -1 || text3.ToLowerInvariant().IndexOf("italic", StringComparison.InvariantCulture) != -1 || text3.ToLowerInvariant().IndexOf("курсив", StringComparison.InvariantCulture) != -1;
									bool needSyntBold = false;
									if (font.Bold)
									{
										if (!flag)
										{
											needSyntBold = true;
										}
										if (text2.ToLowerInvariant().IndexOf("bold", StringComparison.InvariantCulture) != -1)
										{
											needSyntBold = true;
										}
									}
									bool needSyntItalic = false;
									if (font.Italic && !flag2)
									{
										needSyntItalic = true;
									}
									NeedSyntBold = needSyntBold;
									NeedSyntItalic = needSyntItalic;
									if (font.Name == "Cambria Math")
									{
										otm.otmTextMetrics.tmAscent = (int)((double)otm.otmTextMetrics.tmAscent / 4.7);
										otm.otmTextMetrics.tmDescent = (int)((double)otm.otmTextMetrics.tmDescent / 4.7);
									}
								}
							}
							finally
							{
								Marshal.FreeHGlobal(intPtr2);
							}
							int num2 = 0;
							if (font.Name == "Wingdings" || font.Name == "Wingdings 2" || font.Name == "Wingdings 3")
							{
								num2 = 61440;
							}
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < currentFontInfo.MappedSymbolsCount; i++)
							{
								int num3 = currentFontInfo.UnicodeMapBack[i];
								if (num3 >= 128 && num3 <= 255)
								{
									stringBuilder.Append((char)(num2 + num3));
								}
								else
								{
									stringBuilder.Append((char)num3);
								}
							}
							string text4 = stringBuilder.ToString();
							int num4 = text4.Length;
							ushort[] array = new ushort[num4];
							uint glyphIndices = GetGlyphIndices(hdc, text4, num4, array, 1u);
							if (glyphIndices == uint.MaxValue)
							{
								ThrowError(4);
							}
							for (int j = 0; j < currentFontInfo.MappedSymbolsCount; j++)
							{
								currentFontInfo.GlyphList[j] = array[j];
							}
							if (glyphMap != null && glyphMap[65535] != 0)
							{
								int num5 = 0;
								for (int k = 0; k < 65535; k++)
								{
									if (glyphMap[k] != 0 && currentFontInfo.GlyphBackList[k] == 0)
									{
										num5++;
									}
								}
								int num6 = array.Length;
								ushort[] array2 = new ushort[num6 + num5];
								Array.Copy(array, 0, array2, 0, num6);
								for (int l = 0; l < 65535; l++)
								{
									if (glyphMap[l] != 0 && currentFontInfo.GlyphBackList[l] == 0)
									{
										array2[num6] = (ushort)l;
										num6++;
									}
								}
								array = array2;
								num4 = array2.Length;
							}
							ABC[] array3 = new ABC[num4];
							if (!GetCharABCWidthsI(hdc, 0u, (uint)num4, array, array3))
							{
								ThrowError(5);
							}
							double num7 = ((isWpf || StiDpiHelper.NeedFontScaling) ? StiDpiHelper.GraphicsScale : 1.0);
							if (glyphMap != null && glyphMap[65535] != 0)
							{
								int num8 = currentFontInfo.MappedSymbolsCount;
								for (int m = 0; m < 65536; m++)
								{
									if (glyphMap[m] != 0 && currentFontInfo.GlyphBackList[m] == 0)
									{
										double a = (double)((float)(array3[num8].abcA + array3[num8].abcB + array3[num8].abcC) * 0.366f) * num7;
										glyphMap[m] = (ushort)Math.Round(a);
										num8++;
									}
								}
								currentFontInfo.GlyphWidths = glyphMap;
							}
							for (int n = 0; n < currentFontInfo.MappedSymbolsCount; n++)
							{
								double a2 = (double)((float)(array3[n].abcA + array3[n].abcB + array3[n].abcC) * 0.366f) * num7;
								WIDTHS[n] = (int)Math.Round(a2);
							}
							SelectObject(hdc, hgdiobj);
							return;
						}
						finally
						{
							bool flag3 = DeleteObject(intPtr);
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					val.ReleaseHdc(hdc);
				}
			}
			finally
			{
				val.Dispose();
			}
		}
		int num9 = fontInfo.OTM.Length;
		IntPtr intPtr3 = Marshal.AllocHGlobal(num9);
		try
		{
			Marshal.Copy(fontInfo.OTM, 0, intPtr3, num9);
			otm = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(intPtr3, typeof(OUTLINETEXTMETRIC));
			NeedSyntBold = fontInfo.NeedSyntBold;
			NeedSyntItalic = fontInfo.NeedSyntItalic;
		}
		finally
		{
			Marshal.FreeHGlobal(intPtr3);
		}
		ushort[] glyphs = fontInfo.Glyphs;
		for (int num10 = 0; num10 < currentFontInfo.MappedSymbolsCount; num10++)
		{
			int num11 = currentFontInfo.UnicodeMapBack[num10];
			currentFontInfo.GlyphList[num10] = glyphs[num11];
			WIDTHS[num10] = fontInfo.Widths[num11];
		}
	}

	private void GetFontDataFromGdi(Font font, out byte[] buff, bool useFontInfoStore)
	{
		buff = new byte[0];
		if (font == null)
		{
			return;
		}
		if (IsFontStimulsoft(font.Name))
		{
			using Stream stream = typeof(StiFontIconsHelper).Assembly.GetManifestResourceStream("Stimulsoft.Base.FontIcons.Stimulsoft.ttf");
			if (stream != null)
			{
				buff = new byte[stream.Length];
				stream.Read(buff, 0, buff.Length);
				return;
			}
		}
		FontsInfoStore.FontInfo fontInfo = null;
		if (useFontInfoStore || StiOptions.Export.Pdf.ForceUseFontsInfoStoreForWysiwyg)
		{
			fontInfo = fontsInfoStore.GetFontInfo(font);
		}
		if (fontInfo == null)
		{
			byte[] customFontData = StiFontCollection.GetCustomFontData(font.Name);
			if (customFontData != null)
			{
				buff = customFontData;
			}
			else if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
			{
				GetFontDataFromGdiFullTrust(font, out buff);
			}
		}
		else
		{
			buff = fontInfo.Data;
		}
	}

	private static void GetFontDataFromGdiFullTrust(Font font, out byte[] buff)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		Graphics val = (StiOptions.Export.Pdf.AllowImportSystemLibraries ? Graphics.FromHwnd(IntPtr.Zero) : Graphics.FromImage((Image)new Bitmap(1, 1)));
		try
		{
			IntPtr hdc = val.GetHdc();
			try
			{
				Font val2 = new Font(font.FontFamily, 2048f, font.Style);
				try
				{
					IntPtr intPtr = val2.ToHfont();
					try
					{
						IntPtr hgdiobj = SelectObject(hdc, intPtr);
						uint fontData = GetFontData(hdc, 1717793908u, 0u, IntPtr.Zero, 0u);
						if (fontData == uint.MaxValue)
						{
							fontData = GetFontData(hdc, 0u, 0u, IntPtr.Zero, 0u);
							if (fontData == uint.MaxValue)
							{
								ThrowError(8);
							}
							buff = new byte[fontData];
							uint fontData2 = GetFontData(hdc, 0u, 0u, buff, fontData);
							if (fontData2 == uint.MaxValue)
							{
								ThrowError(9);
							}
							if (fontData2 != fontData)
							{
								ThrowError(10);
							}
						}
						else
						{
							buff = new byte[fontData];
							uint fontData3 = GetFontData(hdc, 1717793908u, 0u, buff, fontData);
						}
						SelectObject(hdc, hgdiobj);
					}
					finally
					{
						bool flag = DeleteObject(intPtr);
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				val.ReleaseHdc(hdc);
			}
		}
		finally
		{
			val.Dispose();
		}
	}

	public void InitFontsData(bool isWpf)
	{
		int count = fontList.Count;
		for (int i = 0; i < count; i++)
		{
			pfontInfo pfontInfo = (pfontInfo)fontList[i];
			bool flag = forceUseUnicode;
			if (flag)
			{
				byte[] buff = null;
				GetFontDataBuf(pfontInfo.Font, out buff, useFontInfoStore: false, onlyCheck: true);
				if (buff == null || buff.Length == 0)
				{
					flag = false;
				}
			}
			pfontInfo.UseUnicode = flag;
			fontList[i] = pfontInfo;
			if (flag)
			{
				continue;
			}
			ushort[] array = (ushort[])UnicodeMapsList[i];
			int num = 64;
			for (int j = 64; j < 65536; j++)
			{
				if (array[j] > 0)
				{
					num++;
				}
			}
			if (num <= 255)
			{
				continue;
			}
			pfontInfo = (pfontInfo)fontList[i];
			ushort[] array2 = null;
			byte[] array3 = new byte[65536];
			byte b = (byte)fontList.Count;
			for (int k = 0; k < 64; k++)
			{
				array3[k] = b;
			}
			bool flag2 = true;
			num = 64;
			for (int l = 64; l < 65536; l++)
			{
				if (flag2)
				{
					pfontInfo pfontInfo2 = default(pfontInfo);
					pfontInfo2.Name = pfontInfo.Name;
					pfontInfo2.PdfName = pfontInfo.PdfName;
					pfontInfo2.Bold = pfontInfo.Bold;
					pfontInfo2.Italic = pfontInfo.Italic;
					pfontInfo2.Number = pfontInfo.Number;
					pfontInfo2.Font = pfontInfo.Font;
					pfontInfo2.ParentFontNumber = i;
					pfontInfo2.UseUnicode = flag;
					fontList.Add(pfontInfo2);
					array2 = new ushort[65536];
					Array.Copy(array, 0, array2, 0, 64);
					UnicodeMapsList.Add(array2);
					GlyphMapsList.Add(new ushort[65536]);
					b = (byte)(fontList.Count - 1);
					num = 64;
					flag2 = false;
				}
				if (array[l] > 0)
				{
					array2[l] = 1;
					array3[l] = b;
					num++;
					if (num == 254)
					{
						flag2 = true;
					}
				}
			}
			pfontInfo.ChildFontsMap = array3;
			fontList[i] = pfontInfo;
		}
		fonts = new PdfFontInfo[fontList.Count];
		for (int m = 0; m < fontList.Count; m++)
		{
			fonts[m] = new PdfFontInfo();
			PdfFontInfo pdfFontInfo = fonts[m];
			pdfFontInfo.UnicodeMap = (ushort[])UnicodeMapsList[m];
			pdfFontInfo.UnicodeMapBack = new ushort[MaxSymbols + 1];
			pdfFontInfo.UseUnicode = ((pfontInfo)fontList[m]).UseUnicode;
			ushort[] array4 = (ushort[])GlyphMapsList[m];
			ushort[] array5 = new ushort[65536];
			if (array4[65535] != 0)
			{
				pdfFontInfo.GlyphBackList = MakeGlyphBackMap(((pfontInfo)fontList[m]).Font, useFontInfoStore: false);
				int num2 = 0;
				for (int n = 0; n < 65535; n++)
				{
					if (array4[n] != 0)
					{
						pdfFontInfo.UnicodeMap[pdfFontInfo.GlyphBackList[n]] = 1;
						array5[num2] = (ushort)n;
						num2++;
					}
				}
				pdfFontInfo.GlyphRtfList = new ushort[num2];
				Array.Copy(array5, 0, pdfFontInfo.GlyphRtfList, 0, num2);
			}
			ushort num3 = 64;
			for (int num4 = 64; num4 < 65536; num4++)
			{
				if (pdfFontInfo.UnicodeMap[num4] != 0)
				{
					pdfFontInfo.UnicodeMap[num4] = num3;
					pdfFontInfo.UnicodeMapBack[num3] = (ushort)num4;
					num3++;
					if (num3 > MaxSymbols)
					{
						num3 = MaxSymbols;
					}
					if (!pdfFontInfo.UseUnicode && num3 == 92)
					{
						pdfFontInfo.UnicodeMapBack[num3] = (ushort)num4;
						num3++;
					}
				}
			}
			ushort num5 = 8226;
			pdfFontInfo.UnicodeMap[num5] = num3;
			pdfFontInfo.UnicodeMapBack[num3] = num5;
			num3 = (ushort)(pdfFontInfo.MappedSymbolsCount = (ushort)(num3 + 1));
			for (ushort num6 = 0; num6 < 64; num6++)
			{
				pdfFontInfo.UnicodeMap[num6] = num6;
				pdfFontInfo.UnicodeMapBack[num6] = num6;
			}
			pdfFontInfo.CharPdfNames = new string[num3];
			pdfFontInfo.SymsToPDF = new int[num3];
			for (int num7 = 32; num7 < pdfFontInfo.MappedSymbolsCount; num7++)
			{
				int num8 = -1;
				for (int num9 = 0; num9 < 310; num9++)
				{
					if (pdfFontInfo.UnicodeMapBack[num7] == StandardFontWidths[num9, 0])
					{
						num8 = num9;
						break;
					}
				}
				pdfFontInfo.SymsToPDF[num7] = num8;
				string text = "uni" + pdfFontInfo.UnicodeMapBack[num7].ToString("X4");
				if (uniMap[pdfFontInfo.UnicodeMapBack[num7]] != -1)
				{
					text = StandardFontCharsNames[uniMap[pdfFontInfo.UnicodeMapBack[num7]]].Substring(4);
				}
				pdfFontInfo.CharPdfNames[num7] = text;
			}
			pdfFontInfo.GlyphList = new ushort[num3];
			pfontInfo pfontInfo3 = (pfontInfo)fontList[m];
			fonts[m].Widths = new int[num3];
			GetFontMetrics(pfontInfo3.Font, pdfFontInfo, array4, isWpf);
			for (int num10 = 0; num10 < num3 - 32; num10++)
			{
				fonts[m].Widths[num10] = WIDTHS[32 + num10];
			}
			fonts[m].CH = (int)Math.Round((float)otm.otmsCapEmHeight * 0.366f);
			fonts[m].XH = 0;
			fonts[m].ASC = (int)Math.Round((float)otm.otmAscent * 0.366f);
			fonts[m].DESC = (int)Math.Round((float)otm.otmDescent * 0.366f);
			fonts[m].tmASC = (int)Math.Round((float)otm.otmTextMetrics.tmAscent * 0.366f);
			fonts[m].tmDESC = (int)Math.Round((float)otm.otmTextMetrics.tmDescent * 0.366f);
			fonts[m].tmExternal = (int)Math.Round((float)otm.otmTextMetrics.tmExternalLeading * 0.366f);
			fonts[m].LLX = (int)Math.Round((float)otm.otmrcFontBox.left * 0.366f);
			fonts[m].LLY = (int)Math.Round((float)otm.otmrcFontBox.bottom * 0.366f);
			fonts[m].URX = (int)Math.Round((float)otm.otmrcFontBox.right * 0.366f);
			fonts[m].URY = (int)Math.Round((float)otm.otmrcFontBox.top * 0.366f);
			fonts[m].StemV = (int)Math.Round(0.0);
			fonts[m].ItalicAngle = (int)Math.Round((float)otm.otmItalicAngle / 10f);
			fonts[m].LineGap = (int)Math.Round((float)otm.otmLineGap * 0.366f);
			fonts[m].MacAscend = (int)Math.Round((float)otm.otmMacAscent * 0.366f);
			fonts[m].MacDescend = (int)Math.Round((float)otm.otmMacDescent * 0.366f);
			fonts[m].MacLineGap = (int)Math.Round((float)otm.otmMacLineGap * 0.366f);
			fonts[m].fsSelection = otm.otmfsSelection;
			fonts[m].UnderscoreSize = (int)Math.Round((float)otm.otmsUnderscoreSize * 0.366f);
			fonts[m].UnderscorePosition = (int)Math.Round((float)otm.otmsUnderscorePosition * 0.366f);
			fonts[m].StrikeoutSize = (int)Math.Round((float)otm.otmsStrikeoutSize * 0.366f);
			fonts[m].StrikeoutPosition = (int)Math.Round((float)otm.otmsStrikeoutPosition * 0.366f);
			fonts[m].NeedSyntBold = NeedSyntBold;
			fonts[m].NeedSyntItalic = NeedSyntItalic;
			if (standardPdfFonts)
			{
				for (int num11 = 32; num11 < pdfFontInfo.MappedSymbolsCount; num11++)
				{
					if (fonts[m].SymsToPDF[num11] != -1)
					{
						fonts[m].Widths[num11 - 32] = StandardFontWidths[fonts[m].SymsToPDF[num11], pfontInfo3.Number + 1];
					}
				}
				fonts[m].ItalicAngle = StandardFontInfo[0, pfontInfo3.Number] / 10;
				fonts[m].LLX = StandardFontInfo[1, pfontInfo3.Number];
				fonts[m].LLY = StandardFontInfo[2, pfontInfo3.Number];
				fonts[m].URX = StandardFontInfo[3, pfontInfo3.Number];
				fonts[m].URY = StandardFontInfo[4, pfontInfo3.Number];
				fonts[m].CH = StandardFontInfo[5, pfontInfo3.Number];
				fonts[m].XH = StandardFontInfo[6, pfontInfo3.Number];
				fonts[m].ASC = StandardFontInfo[7, pfontInfo3.Number];
				fonts[m].DESC = StandardFontInfo[8, pfontInfo3.Number];
				fonts[m].StemV = 0;
			}
			if (((pfontInfo)fontList[m]).ChildFontsMap != null)
			{
				pdfFontInfo.MappedSymbolsCount = 255;
				pdfFontInfo.ChildFontsMap = ((pfontInfo)fontList[m]).ChildFontsMap;
			}
		}
		currentFont = -1;
		CurrentFont = 0;
	}

	public int GetFontNumber(Font incomingFont)
	{
		int num = -1;
		string text = null;
		object obj = fontHash[incomingFont];
		if (obj == null)
		{
			text = incomingFont.Name;
			fontHash[incomingFont] = text;
		}
		else
		{
			text = (string)obj;
		}
		if (fontList.Count > 0)
		{
			for (int i = 0; i < fontList.Count; i++)
			{
				pfontInfo pfontInfo = (pfontInfo)fontList[i];
				if (pfontInfo.Name == text && pfontInfo.Bold == incomingFont.Bold && pfontInfo.Italic == incomingFont.Italic)
				{
					num = i;
					break;
				}
			}
		}
		if (num == -1)
		{
			pfontInfo pfontInfo2 = default(pfontInfo);
			pfontInfo2.Name = incomingFont.Name;
			pfontInfo2.Bold = incomingFont.Bold;
			pfontInfo2.Italic = incomingFont.Italic;
			pfontInfo2.Font = incomingFont;
			pfontInfo2.ParentFontNumber = -1;
			num = 0;
			string[] array = family_Helvetica;
			foreach (string text2 in array)
			{
				if (pfontInfo2.Name == text2)
				{
					num = 0;
				}
			}
			string[] array2 = family_Times_Roman;
			foreach (string text3 in array2)
			{
				if (pfontInfo2.Name == text3)
				{
					num = 4;
				}
			}
			string[] array3 = family_Courier;
			foreach (string text4 in array3)
			{
				if (pfontInfo2.Name == text4)
				{
					num = 8;
				}
			}
			if (num < 12)
			{
				if (pfontInfo2.Bold)
				{
					num++;
				}
				if (pfontInfo2.Italic)
				{
					num += 2;
				}
			}
			pfontInfo2.Number = num;
			pfontInfo2.PdfName = PdfFontName[num];
			fontList.Add(pfontInfo2);
			num = fontList.Count - 1;
			UnicodeMapsList.Add(new ushort[65536]);
			GlyphMapsList.Add(new ushort[65536]);
		}
		CurrentFont = num;
		return num;
	}

	public void StoreUnicodeSymbolsInMap(StringBuilder sb)
	{
		if (sb.Length > 0 && currentFont != -1)
		{
			ushort[] array = (ushort[])UnicodeMapsList[currentFont];
			string text = sb.ToString();
			for (int i = 0; i < text.Length; i++)
			{
				array[(uint)text[i]] = 1;
			}
		}
	}

	public void StoreGlyphsInMap(StringBuilder sb)
	{
		if (sb.Length > 0 && currentFont != -1)
		{
			ushort[] array = (ushort[])GlyphMapsList[currentFont];
			string text = sb.ToString();
			for (int i = 0; i < text.Length; i++)
			{
				array[(uint)text[i]] = 1;
			}
			array[65535] = 1;
		}
	}

	public static bool IsFontStimulsoft(string name)
	{
		return name == "Stimulsoft";
	}

	public static bool IsFontSymbolic(string name)
	{
		return name == "Symbol";
	}

	private static void ThrowError(int step)
	{
		Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
		throw new Exception($"PdfFonts error at point {step}, code #{ex.ErrorCode:X8}: {ex.Message}");
	}

	private static void ThrowError(int step, int error)
	{
		Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
		throw new Exception($"PdfFonts error at point {step}, code #{ex.ErrorCode:X8}(#{error:X8}): {ex.Message}");
	}

	public void Clear()
	{
		fonts = null;
		fontList = null;
		UnicodeMapsList = null;
		GlyphMapsList = null;
		Widths = null;
		CharPdfNames = null;
		UnicodeMap = null;
		UnicodeMapBack = null;
		GlyphList = null;
		GlyphBackList = null;
		GlyphRtfList = null;
		SymsToPDF = null;
		GlyphWidths = null;
		fontsInfoStore.Clear();
		localFontsDataCache.Clear();
	}

	public void GetFontDataBuf(Font font, out byte[] buff, bool useFontInfoStore, bool onlyCheck = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		string name = font.Name;
		FontStyle style = font.Style;
		string key = $"{name}_{((object)(FontStyle)(ref style)).ToString()}";
		byte[] buff2 = null;
		if (StiOptions.Export.Pdf.AllowFontsCache)
		{
			lock (FontsDataCache)
			{
				if (FontsDataCache.ContainsKey(key))
				{
					buff2 = (byte[])FontsDataCache[key];
				}
				else
				{
					GetFontDataFromGdi(font, out buff2, useFontInfoStore);
					FontsDataCache[key] = buff2;
				}
			}
			if (onlyCheck)
			{
				buff = buff2;
				return;
			}
			buff = new byte[buff2.Length];
			Array.Copy(buff2, buff, buff2.Length);
		}
		else
		{
			if (localFontsDataCache.ContainsKey(key))
			{
				buff2 = (byte[])localFontsDataCache[key];
			}
			else
			{
				GetFontDataFromGdi(font, out buff2, useFontInfoStore);
				localFontsDataCache[key] = buff2;
			}
			if (onlyCheck)
			{
				buff = buff2;
				return;
			}
			buff = new byte[buff2.Length];
			Array.Copy(buff2, buff, buff2.Length);
		}
	}

	public static void ClearFontsCache()
	{
		lock (FontsDataCache)
		{
			FontsDataCache.Clear();
		}
	}

	public PdfFonts()
	{
		fontList = new ArrayList();
		UnicodeMapsList = new ArrayList();
		GlyphMapsList = new ArrayList();
		InitStandardFontWidths();
		InitStandardFontCharsNames();
		InitStandardFontInfo();
		InitCharNamesUniMap();
		fontsInfoStore = new FontsInfoStore();
	}

	private static void InitStandardFontWidths()
	{
		if (StandardFontWidths == null)
		{
			StandardFontWidths = new ushort[310, 13]
			{
				{
					32, 278, 278, 278, 278, 250, 250, 250, 250, 600,
					600, 600, 600
				},
				{
					33, 278, 333, 278, 333, 333, 333, 333, 389, 600,
					600, 600, 600
				},
				{
					34, 355, 474, 355, 474, 408, 555, 420, 555, 600,
					600, 600, 600
				},
				{
					35, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					36, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					37, 889, 889, 889, 889, 833, 1000, 833, 833, 600,
					600, 600, 600
				},
				{
					38, 667, 722, 667, 722, 778, 833, 778, 778, 600,
					600, 600, 600
				},
				{
					39, 191, 238, 191, 238, 180, 278, 214, 278, 600,
					600, 600, 600
				},
				{
					40, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					41, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					42, 389, 389, 389, 389, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					43, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					44, 278, 278, 278, 278, 250, 250, 250, 250, 600,
					600, 600, 600
				},
				{
					45, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					46, 278, 278, 278, 278, 250, 250, 250, 250, 600,
					600, 600, 600
				},
				{
					47, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					48, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					49, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					50, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					51, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					52, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					53, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					54, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					55, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					56, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					57, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					58, 278, 333, 278, 333, 278, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					59, 278, 333, 278, 333, 278, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					60, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					61, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					62, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					63, 556, 611, 556, 611, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					64, 1015, 975, 1015, 975, 921, 930, 920, 832, 600,
					600, 600, 600
				},
				{
					65, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					66, 667, 722, 667, 722, 667, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					67, 722, 722, 722, 722, 667, 722, 667, 667, 600,
					600, 600, 600
				},
				{
					68, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					69, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					70, 611, 611, 611, 611, 556, 611, 611, 667, 600,
					600, 600, 600
				},
				{
					71, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					72, 722, 722, 722, 722, 722, 778, 722, 778, 600,
					600, 600, 600
				},
				{
					73, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					74, 500, 556, 500, 556, 389, 500, 444, 500, 600,
					600, 600, 600
				},
				{
					75, 667, 722, 667, 722, 722, 778, 667, 667, 600,
					600, 600, 600
				},
				{
					76, 556, 611, 556, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					77, 833, 833, 833, 833, 889, 944, 833, 889, 600,
					600, 600, 600
				},
				{
					78, 722, 722, 722, 722, 722, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					79, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					80, 667, 667, 667, 667, 556, 611, 611, 611, 600,
					600, 600, 600
				},
				{
					81, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					82, 722, 722, 722, 722, 667, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					83, 667, 667, 667, 667, 556, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					84, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					85, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					86, 667, 667, 667, 667, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					87, 944, 944, 944, 944, 944, 1000, 833, 889, 600,
					600, 600, 600
				},
				{
					88, 667, 667, 667, 667, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					89, 667, 667, 667, 667, 722, 722, 556, 611, 600,
					600, 600, 600
				},
				{
					90, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					91, 278, 333, 278, 333, 333, 333, 389, 333, 600,
					600, 600, 600
				},
				{
					92, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					93, 278, 333, 278, 333, 333, 333, 389, 333, 600,
					600, 600, 600
				},
				{
					94, 469, 584, 469, 584, 469, 581, 422, 570, 600,
					600, 600, 600
				},
				{
					95, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					96, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					97, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					98, 556, 611, 556, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					99, 500, 556, 500, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					100, 556, 611, 556, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					101, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					102, 278, 333, 278, 333, 333, 333, 278, 333, 600,
					600, 600, 600
				},
				{
					103, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					104, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					105, 222, 278, 222, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					106, 222, 278, 222, 278, 278, 333, 278, 278, 600,
					600, 600, 600
				},
				{
					107, 500, 556, 500, 556, 500, 556, 444, 500, 600,
					600, 600, 600
				},
				{
					108, 222, 278, 222, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					109, 833, 889, 833, 889, 778, 833, 722, 778, 600,
					600, 600, 600
				},
				{
					110, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					111, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					112, 556, 611, 556, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					113, 556, 611, 556, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					114, 333, 389, 333, 389, 333, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					115, 500, 556, 500, 556, 389, 389, 389, 389, 600,
					600, 600, 600
				},
				{
					116, 278, 333, 278, 333, 278, 333, 278, 278, 600,
					600, 600, 600
				},
				{
					117, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					118, 500, 556, 500, 556, 500, 500, 444, 444, 600,
					600, 600, 600
				},
				{
					119, 722, 778, 722, 778, 722, 722, 667, 667, 600,
					600, 600, 600
				},
				{
					120, 500, 556, 500, 556, 500, 500, 444, 500, 600,
					600, 600, 600
				},
				{
					121, 500, 556, 500, 556, 500, 500, 444, 444, 600,
					600, 600, 600
				},
				{
					122, 500, 500, 500, 500, 444, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					123, 334, 389, 334, 389, 480, 394, 400, 348, 600,
					600, 600, 600
				},
				{
					124, 260, 280, 260, 280, 200, 220, 275, 220, 600,
					600, 600, 600
				},
				{
					125, 334, 389, 334, 389, 480, 394, 400, 348, 600,
					600, 600, 600
				},
				{
					126, 584, 584, 584, 584, 541, 520, 541, 570, 600,
					600, 600, 600
				},
				{
					161, 333, 333, 333, 333, 333, 333, 389, 389, 600,
					600, 600, 600
				},
				{
					162, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					163, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					164, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					165, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					166, 260, 280, 260, 280, 200, 220, 275, 220, 600,
					600, 600, 600
				},
				{
					167, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					168, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					169, 737, 737, 737, 737, 760, 747, 760, 747, 600,
					600, 600, 600
				},
				{
					170, 370, 370, 370, 370, 276, 300, 276, 266, 600,
					600, 600, 600
				},
				{
					171, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					172, 584, 584, 584, 584, 564, 570, 675, 606, 600,
					600, 600, 600
				},
				{
					174, 737, 737, 737, 737, 760, 747, 760, 747, 600,
					600, 600, 600
				},
				{
					175, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					176, 400, 400, 400, 400, 400, 400, 400, 400, 600,
					600, 600, 600
				},
				{
					177, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					180, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					181, 556, 611, 556, 611, 500, 556, 500, 576, 600,
					600, 600, 600
				},
				{
					182, 537, 556, 537, 556, 453, 540, 523, 500, 600,
					600, 600, 600
				},
				{
					183, 278, 278, 278, 278, 250, 250, 250, 250, 600,
					600, 600, 600
				},
				{
					184, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					186, 365, 365, 365, 365, 310, 330, 310, 300, 600,
					600, 600, 600
				},
				{
					187, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					188, 834, 834, 834, 834, 750, 750, 750, 750, 600,
					600, 600, 600
				},
				{
					189, 834, 834, 834, 834, 750, 750, 750, 750, 600,
					600, 600, 600
				},
				{
					190, 834, 834, 834, 834, 750, 750, 750, 750, 600,
					600, 600, 600
				},
				{
					191, 611, 611, 611, 611, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					192, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					193, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					194, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					195, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					196, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					197, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					198, 1000, 1000, 1000, 1000, 889, 1000, 889, 944, 600,
					600, 600, 600
				},
				{
					199, 722, 722, 722, 722, 667, 722, 667, 667, 600,
					600, 600, 600
				},
				{
					200, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					201, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					202, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					203, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					204, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					205, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					206, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					207, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					208, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					209, 722, 722, 722, 722, 722, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					210, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					211, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					212, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					213, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					214, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					215, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					216, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					217, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					218, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					219, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					220, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					221, 667, 667, 667, 667, 722, 722, 556, 611, 600,
					600, 600, 600
				},
				{
					222, 667, 667, 667, 667, 556, 611, 611, 611, 600,
					600, 600, 600
				},
				{
					223, 611, 611, 611, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					224, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					225, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					226, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					227, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					228, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					229, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					230, 889, 889, 889, 889, 667, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					231, 500, 556, 500, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					232, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					233, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					234, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					235, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					236, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					237, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					238, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					239, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					240, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					241, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					242, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					243, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					244, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					245, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					246, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					247, 584, 584, 584, 584, 564, 570, 675, 570, 600,
					600, 600, 600
				},
				{
					248, 611, 611, 611, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					249, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					250, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					251, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					252, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					253, 500, 556, 500, 556, 500, 500, 444, 444, 600,
					600, 600, 600
				},
				{
					254, 556, 611, 556, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					255, 500, 556, 500, 556, 500, 500, 444, 444, 600,
					600, 600, 600
				},
				{
					256, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					257, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					258, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					259, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					260, 667, 722, 667, 722, 722, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					261, 556, 556, 556, 556, 444, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					262, 722, 722, 722, 722, 667, 722, 667, 667, 600,
					600, 600, 600
				},
				{
					263, 500, 556, 500, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					268, 722, 722, 722, 722, 667, 722, 667, 667, 600,
					600, 600, 600
				},
				{
					269, 500, 556, 500, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					270, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					271, 643, 743, 643, 743, 588, 672, 544, 608, 600,
					600, 600, 600
				},
				{
					272, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					273, 556, 611, 556, 611, 500, 556, 500, 500, 600,
					600, 600, 600
				},
				{
					274, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					275, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					278, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					279, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					280, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					281, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					282, 667, 667, 667, 667, 611, 667, 611, 667, 600,
					600, 600, 600
				},
				{
					283, 556, 556, 556, 556, 444, 444, 444, 444, 600,
					600, 600, 600
				},
				{
					286, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					287, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					290, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					291, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					298, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					299, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					302, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					303, 222, 278, 222, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					304, 278, 278, 278, 278, 333, 389, 333, 389, 600,
					600, 600, 600
				},
				{
					305, 278, 278, 278, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					310, 667, 722, 667, 722, 722, 778, 667, 667, 600,
					600, 600, 600
				},
				{
					311, 500, 556, 500, 556, 500, 556, 444, 500, 600,
					600, 600, 600
				},
				{
					313, 556, 611, 556, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					314, 222, 278, 222, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					315, 556, 611, 556, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					316, 222, 278, 222, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					317, 556, 611, 556, 611, 611, 667, 611, 611, 600,
					600, 600, 600
				},
				{
					318, 299, 400, 299, 400, 344, 394, 300, 382, 600,
					600, 600, 600
				},
				{
					321, 556, 611, 556, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					322, 222, 278, 222, 278, 278, 278, 278, 278, 600,
					600, 600, 600
				},
				{
					323, 722, 722, 722, 722, 722, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					324, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					325, 722, 722, 722, 722, 722, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					326, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					327, 722, 722, 722, 722, 722, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					328, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					332, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					333, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					336, 778, 778, 778, 778, 722, 778, 722, 722, 600,
					600, 600, 600
				},
				{
					337, 556, 611, 556, 611, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					338, 1000, 1000, 1000, 1000, 889, 1000, 944, 944, 600,
					600, 600, 600
				},
				{
					339, 944, 944, 944, 944, 722, 722, 667, 722, 600,
					600, 600, 600
				},
				{
					340, 722, 722, 722, 722, 667, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					341, 333, 389, 333, 389, 333, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					342, 722, 722, 722, 722, 667, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					343, 333, 389, 333, 389, 333, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					344, 722, 722, 722, 722, 667, 722, 611, 667, 600,
					600, 600, 600
				},
				{
					345, 333, 389, 333, 389, 333, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					346, 667, 667, 667, 667, 556, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					347, 500, 556, 500, 556, 389, 389, 389, 389, 600,
					600, 600, 600
				},
				{
					350, 667, 667, 667, 667, 556, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					351, 500, 556, 500, 556, 389, 389, 389, 389, 600,
					600, 600, 600
				},
				{
					352, 667, 667, 667, 667, 556, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					353, 500, 556, 500, 556, 389, 389, 389, 389, 600,
					600, 600, 600
				},
				{
					354, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					355, 278, 333, 278, 333, 278, 333, 278, 278, 600,
					600, 600, 600
				},
				{
					356, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					357, 317, 389, 317, 389, 326, 416, 300, 366, 600,
					600, 600, 600
				},
				{
					362, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					363, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					366, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					367, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					368, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					369, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					370, 722, 722, 722, 722, 722, 722, 722, 722, 600,
					600, 600, 600
				},
				{
					371, 556, 611, 556, 611, 500, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					376, 667, 667, 667, 667, 722, 722, 556, 611, 600,
					600, 600, 600
				},
				{
					377, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					378, 500, 500, 500, 500, 444, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					379, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					380, 500, 500, 500, 500, 444, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					381, 611, 611, 611, 611, 611, 667, 556, 611, 600,
					600, 600, 600
				},
				{
					382, 500, 500, 500, 500, 444, 444, 389, 389, 600,
					600, 600, 600
				},
				{
					402, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					536, 667, 667, 667, 667, 556, 556, 500, 556, 600,
					600, 600, 600
				},
				{
					537, 500, 556, 500, 556, 389, 389, 389, 389, 600,
					600, 600, 600
				},
				{
					710, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					711, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					728, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					729, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					730, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					731, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					732, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					733, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					8211, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					8212, 1000, 1000, 1000, 1000, 1000, 1000, 889, 1000, 600,
					600, 600, 600
				},
				{
					8216, 222, 278, 222, 278, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					8217, 222, 278, 222, 278, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					8218, 222, 278, 222, 278, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					8220, 333, 500, 333, 500, 444, 500, 556, 500, 600,
					600, 600, 600
				},
				{
					8221, 333, 500, 333, 500, 444, 500, 556, 500, 600,
					600, 600, 600
				},
				{
					8222, 333, 500, 333, 500, 444, 500, 556, 500, 600,
					600, 600, 600
				},
				{
					8224, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					8225, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					8226, 350, 350, 350, 350, 350, 350, 350, 350, 600,
					600, 600, 600
				},
				{
					8230, 1000, 1000, 1000, 1000, 1000, 1000, 889, 1000, 600,
					600, 600, 600
				},
				{
					8240, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 1000, 600,
					600, 600, 600
				},
				{
					8249, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					8250, 333, 333, 333, 333, 333, 333, 333, 333, 600,
					600, 600, 600
				},
				{
					8260, 167, 167, 167, 167, 167, 167, 167, 167, 600,
					600, 600, 600
				},
				{
					8364, 556, 556, 556, 556, 500, 500, 500, 500, 600,
					600, 600, 600
				},
				{
					8482, 1000, 1000, 1000, 1000, 980, 1000, 980, 1000, 600,
					600, 600, 600
				},
				{
					8706, 476, 494, 476, 494, 476, 494, 476, 494, 600,
					600, 600, 600
				},
				{
					8710, 612, 612, 612, 612, 612, 612, 612, 612, 600,
					600, 600, 600
				},
				{
					8721, 600, 600, 600, 600, 600, 600, 600, 600, 600,
					600, 600, 600
				},
				{
					8722, 584, 584, 584, 584, 564, 570, 675, 606, 600,
					600, 600, 600
				},
				{
					8730, 453, 549, 453, 549, 453, 549, 453, 549, 600,
					600, 600, 600
				},
				{
					8773, 250, 250, 250, 250, 250, 250, 250, 250, 600,
					600, 600, 600
				},
				{
					8800, 549, 549, 549, 549, 549, 549, 549, 549, 600,
					600, 600, 600
				},
				{
					8804, 549, 549, 549, 549, 549, 549, 549, 549, 600,
					600, 600, 600
				},
				{
					8805, 549, 549, 549, 549, 549, 549, 549, 549, 600,
					600, 600, 600
				},
				{
					9674, 471, 494, 471, 494, 471, 494, 471, 494, 600,
					600, 600, 600
				}
			};
		}
	}

	private static void InitStandardFontCharsNames()
	{
		if (StandardFontCharsNames == null)
		{
			StandardFontCharsNames = new string[3680]
			{
				"0001controlSTX", "0002controlSOT", "0003controlETX", "0004controlEOT", "0005controlENQ", "0006controlACK", "0007controlBEL", "0008controlBS", "0009controlHT", "000AcontrolLF",
				"000BcontrolVT", "000CcontrolFF", "000DcontrolCR", "000EcontrolSO", "000FcontrolSI", "0010controlDLE", "0011controlDC1", "0012controlDC2", "0013controlDC3", "0014controlDC4",
				"0015controlNAK", "0016controlSYN", "0017controlETB", "0018controlCAN", "0019controlEM", "001AcontrolSUB", "001BcontrolESC", "001CcontrolFS", "001DcontrolGS", "001EcontrolRS",
				"001FcontrolUS", "0020space", "0021exclam", "0022quotedbl", "0023numbersign", "0024dollar", "0025percent", "0026ampersand", "0027quotesingle", "0028parenleft",
				"0029parenright", "002Aasterisk", "002Bplus", "002Ccomma", "002Dhyphen", "002Eperiod", "002Fslash", "0030zero", "0031one", "0032two",
				"0033three", "0034four", "0035five", "0036six", "0037seven", "0038eight", "0039nine", "003Acolon", "003Bsemicolon", "003Cless",
				"003Dequal", "003Egreater", "003Fquestion", "0040at", "0041A", "0042B", "0043C", "0044D", "0045E", "0046F",
				"0047G", "0048H", "0049I", "004AJ", "004BK", "004CL", "004DM", "004EN", "004FO", "0050P",
				"0051Q", "0052R", "0053S", "0054T", "0055U", "0056V", "0057W", "0058X", "0059Y", "005AZ",
				"005Bbracketleft", "005Cbackslash", "005Dbracketright", "005Easciicircum", "005Funderscore", "0060grave", "0061a", "0062b", "0063c", "0064d",
				"0065e", "0066f", "0067g", "0068h", "0069i", "006Aj", "006Bk", "006Cl", "006Dm", "006En",
				"006Fo", "0070p", "0071q", "0072r", "0073s", "0074t", "0075u", "0076v", "0077w", "0078x",
				"0079y", "007Az", "007Bbraceleft", "007Cbar", "007Dbraceright", "007Easciitilde", "007FcontrolDEL", "00A0nbspace", "00A1exclamdown", "00A2cent",
				"00A3sterling", "00A4currency", "00A5yen", "00A6brokenbar", "00A7section", "00A8dieresis", "00A9copyright", "00AAordfeminine", "00ABguillemotleft", "00AClogicalnot",
				"00ADsofthyphen", "00AEregistered", "00AFmacron", "00B0degree", "00B1plusminus", "00B2twosuperior", "00B3threesuperior", "00B4acute", "00B5mu", "00B6paragraph",
				"00B7periodcentered", "00B8cedilla", "00B9onesuperior", "00BAordmasculine", "00BBguillemotright", "00BConequarter", "00BDonehalf", "00BEthreequarters", "00BFquestiondown", "00C0Agrave",
				"00C1Aacute", "00C2Acircumflex", "00C3Atilde", "00C4Adieresis", "00C5Aring", "00C6AE", "00C7Ccedilla", "00C8Egrave", "00C9Eacute", "00CAEcircumflex",
				"00CBEdieresis", "00CCIgrave", "00CDIacute", "00CEIcircumflex", "00CFIdieresis", "00D0Eth", "00D1Ntilde", "00D2Ograve", "00D3Oacute", "00D4Ocircumflex",
				"00D5Otilde", "00D6Odieresis", "00D7multiply", "00D8Oslash", "00D9Ugrave", "00DAUacute", "00DBUcircumflex", "00DCUdieresis", "00DDYacute", "00DEThorn",
				"00DFgermandbls", "00E0agrave", "00E1aacute", "00E2acircumflex", "00E3atilde", "00E4adieresis", "00E5aring", "00E6ae", "00E7ccedilla", "00E8egrave",
				"00E9eacute", "00EAecircumflex", "00EBedieresis", "00ECigrave", "00EDiacute", "00EEicircumflex", "00EFidieresis", "00F0eth", "00F1ntilde", "00F2ograve",
				"00F3oacute", "00F4ocircumflex", "00F5otilde", "00F6odieresis", "00F7divide", "00F8oslash", "00F9ugrave", "00FAuacute", "00FBucircumflex", "00FCudieresis",
				"00FDyacute", "00FEthorn", "00FFydieresis", "0100Amacron", "0101amacron", "0102Abreve", "0103abreve", "0104Aogonek", "0105aogonek", "0106Cacute",
				"0107cacute", "0108Ccircumflex", "0109ccircumflex", "010ACdotaccent", "010Bcdotaccent", "010CCcaron", "010Dccaron", "010EDcaron", "010Fdcaron", "0110Dcroat",
				"0111dcroat", "0112Emacron", "0113emacron", "0114Ebreve", "0115ebreve", "0116Edotaccent", "0117edotaccent", "0118Eogonek", "0119eogonek", "011AEcaron",
				"011Becaron", "011CGcircumflex", "011Dgcircumflex", "011EGbreve", "011Fgbreve", "0120Gdotaccent", "0121gdotaccent", "0122Gcommaaccent", "0123gcommaaccent", "0124Hcircumflex",
				"0125hcircumflex", "0126Hbar", "0127hbar", "0128Itilde", "0129itilde", "012AImacron", "012Bimacron", "012CIbreve", "012Dibreve", "012EIogonek",
				"012Fiogonek", "0130Idotaccent", "0131dotlessi", "0132IJ", "0133ij", "0134Jcircumflex", "0135jcircumflex", "0136Kcommaaccent", "0137kcommaaccent", "0138kgreenlandic",
				"0139Lacute", "013Alacute", "013BLcommaaccent", "013Clcommaaccent", "013DLcaron", "013Elcaron", "013FLdot", "0140ldot", "0141Lslash", "0142lslash",
				"0143Nacute", "0144nacute", "0145Ncommaaccent", "0146ncommaaccent", "0147Ncaron", "0148ncaron", "0149napostrophe", "014AEng", "014Beng", "014COmacron",
				"014Domacron", "014EObreve", "014Fobreve", "0150Ohungarumlaut", "0151ohungarumlaut", "0152OE", "0153oe", "0154Racute", "0155racute", "0156Rcommaaccent",
				"0157rcommaaccent", "0158Rcaron", "0159rcaron", "015ASacute", "015Bsacute", "015CScircumflex", "015Dscircumflex", "015EScedilla", "015Fscedilla", "0160Scaron",
				"0161scaron", "0162Tcommaaccent", "0163tcommaaccent", "0164Tcaron", "0165tcaron", "0166Tbar", "0167tbar", "0168Utilde", "0169utilde", "016AUmacron",
				"016Bumacron", "016CUbreve", "016Dubreve", "016EUring", "016Furing", "0170Uhungarumlaut", "0171uhungarumlaut", "0172Uogonek", "0173uogonek", "0174Wcircumflex",
				"0175wcircumflex", "0176Ycircumflex", "0177ycircumflex", "0178Ydieresis", "0179Zacute", "017Azacute", "017BZdotaccent", "017Czdotaccent", "017DZcaron", "017Ezcaron",
				"017Flongs", "0180bstroke", "0181Bhook", "0182Btopbar", "0183btopbar", "0184Tonesix", "0185tonesix", "0186Oopen", "0187Chook", "0188chook",
				"0189Dafrican", "018ADhook", "018BDtopbar", "018Cdtopbar", "018Ddeltaturned", "018EEreversed", "018FSchwa", "0190Eopen", "0191Fhook", "0192florin",
				"0193Ghook", "0194Gammaafrican", "0195hv", "0196Iotaafrican", "0197Istroke", "0198Khook", "0199khook", "019Albar", "019Blambdastroke", "019CMturned",
				"019DNhookleft", "019Enlegrightlong", "019FOcenteredtilde", "01A0Ohorn", "01A1ohorn", "01A2Oi", "01A3oi", "01A4Phook", "01A5phook", "01A6yr",
				"01A7Tonetwo", "01A8tonetwo", "01A9Esh", "01AAeshreversedloop", "01ABtpalatalhook", "01ACThook", "01ADthook", "01AETretroflexhook", "01AFUhorn", "01B0uhorn",
				"01B1Upsilonafrican", "01B2Vhook", "01B3Yhook", "01B4yhook", "01B5Zstroke", "01B6zstroke", "01B7Ezh", "01B8Ezhreversed", "01B9ezhreversed", "01BAezhtail",
				"01BBtwostroke", "01BCTonefive", "01BDtonefive", "01BEglottalinvertedstroke", "01BFwynn", "01C0clickdental", "01C1clicklateral", "01C2clickalveolar", "01C3clickretroflex", "01C4DZcaron",
				"01C5Dzcaron", "01C6dzcaron", "01C7LJ", "01C8Lj", "01C9lj", "01CANJ", "01CBNj", "01CCnj", "01CDAcaron", "01CEacaron",
				"01CFIcaron", "01D0icaron", "01D1Ocaron", "01D2ocaron", "01D3Ucaron", "01D4ucaron", "01D5Udieresismacron", "01D6udieresismacron", "01D7Udieresisacute", "01D8udieresisacute",
				"01D9Udieresiscaron", "01DAudieresiscaron", "01DBUdieresisgrave", "01DCudieresisgrave", "01DDeturned", "01DEAdieresismacron", "01DFadieresismacron", "01E0Adotmacron", "01E1adotmacron", "01E2AEmacron",
				"01E3aemacron", "01E4Gstroke", "01E5gstroke", "01E6Gcaron", "01E7gcaron", "01E8Kcaron", "01E9kcaron", "01EAOogonek", "01EBoogonek", "01ECOogonekmacron",
				"01EDoogonekmacron", "01EEEzhcaron", "01EFezhcaron", "01F0jcaron", "01F1DZ", "01F2Dz", "01F3dz", "01F4Gacute", "01F5gacute", "01FAAringacute",
				"01FBaringacute", "01FCAEacute", "01FDaeacute", "01FEOslashacute", "01FFoslashacute", "0200Adblgrave", "0201adblgrave", "0202Ainvertedbreve", "0203ainvertedbreve", "0204Edblgrave",
				"0205edblgrave", "0206Einvertedbreve", "0207einvertedbreve", "0208Idblgrave", "0209idblgrave", "020AIinvertedbreve", "020Biinvertedbreve", "020COdblgrave", "020Dodblgrave", "020EOinvertedbreve",
				"020Foinvertedbreve", "0210Rdblgrave", "0211rdblgrave", "0212Rinvertedbreve", "0213rinvertedbreve", "0214Udblgrave", "0215udblgrave", "0216Uinvertedbreve", "0217uinvertedbreve", "0218Scommaaccent",
				"0219scommaaccent", "0250aturned", "0251ascript", "0252ascriptturned", "0253bhook", "0254oopen", "0255ccurl", "0256dtail", "0257dhook", "0258ereversed",
				"0259schwa", "025Aschwahook", "025Beopen", "025Ceopenreversed", "025Deopenreversedhook", "025Eeopenreversedclosed", "025Fjdotlessstroke", "0260ghook", "0261gscript", "0263gammalatinsmall",
				"0264ramshorn", "0265hturned", "0266hhook", "0267henghook", "0268istroke", "0269iotalatin", "026Blmiddletilde", "026Clbelt", "026Dlhookretroflex", "026Elezh",
				"026Fmturned", "0270mlonglegturned", "0271mhook", "0272nhookleft", "0273nhookretroflex", "0275obarred", "0277omegalatinclosed", "0278philatin", "0279rturned", "027Arlonglegturned",
				"027Brhookturned", "027Crlongleg", "027Drhook", "027Erfishhook", "027Frfishhookreversed", "0281Rsmallinverted", "0282shook", "0283esh", "0284dotlessjstrokehook", "0285eshsquatreversed",
				"0286eshcurl", "0287tturned", "0288tretroflexhook", "0289ubar", "028Aupsilonlatin", "028Bvhook", "028Cvturned", "028Dwturned", "028Eyturned", "0290zretroflexhook",
				"0291zcurl", "0292ezh", "0293ezhcurl", "0294glottalstop", "0295glottalstopreversed", "0296glottalstopinverted", "0297cstretched", "0298bilabialclick", "029Aeopenclosed", "029BGsmallhook",
				"029Djcrossedtail", "029Ekturned", "02A0qhook", "02A1glottalstopstroke", "02A2glottalstopstrokereversed", "02A3dzaltone", "02A4dezh", "02A5dzcurl", "02A6ts", "02A7tesh",
				"02A8tccurl", "02B0hsuperior", "02B1hhooksuperior", "02B2jsuperior", "02B4rturnedsuperior", "02B5rhookturnedsuperior", "02B6Rsmallinvertedsuperior", "02B7wsuperior", "02B8ysuperior", "02B9primemod",
				"02BAdblprimemod", "02BBcommaturnedmod", "02BCafii57929", "02BDafii64937", "02BEringhalfright", "02BFringhalfleft", "02C0glottalstopmod", "02C1glottalstopreversedmod", "02C2arrowheadleftmod", "02C3arrowheadrightmod",
				"02C4arrowheadupmod", "02C5arrowheaddownmod", "02C6circumflex", "02C7caron", "02C8verticallinemod", "02C9firsttonechinese", "02CAsecondtonechinese", "02CBfourthtonechinese", "02CCverticallinelowmod", "02CDmacronlowmod",
				"02CEgravelowmod", "02CFacutelowmod", "02D0colontriangularmod", "02D1colontriangularhalfmod", "02D2ringhalfrightcentered", "02D3ringhalfleftcentered", "02D4uptackmod", "02D5downtackmod", "02D6plusmod", "02D7minusmod",
				"02D8breve", "02D9dotaccent", "02DAring", "02DBogonek", "02DCtilde", "02DDhungarumlaut", "02DErhotichookmod", "02E0gammasuperior", "02E3xsuperior", "02E4glottalstopreversedsuperior",
				"02E5tonebarextrahighmod", "02E6tonebarhighmod", "02E7tonebarmidmod", "02E8tonebarlowmod", "02E9tonebarextralowmod", "0300gravecomb", "0301acutecomb", "0302circumflexcmb", "0303tildecomb", "0304macroncmb",
				"0305overlinecmb", "0306brevecmb", "0307dotaccentcmb", "0308dieresiscmb", "0309hookabovecomb", "030Aringcmb", "030Bhungarumlautcmb", "030Ccaroncmb", "030Dverticallineabovecmb", "030Edblverticallineabovecmb",
				"030Fdblgravecmb", "0310candrabinducmb", "0311breveinvertedcmb", "0312commaturnedabovecmb", "0313commaabovecmb", "0314commareversedabovecmb", "0315commaaboverightcmb", "0316gravebelowcmb", "0317acutebelowcmb", "0318lefttackbelowcmb",
				"0319righttackbelowcmb", "031Aleftangleabovecmb", "031Bhorncmb", "031Cringhalfleftbelowcmb", "031Duptackbelowcmb", "031Edowntackbelowcmb", "031Fplusbelowcmb", "0320minusbelowcmb", "0321hookpalatalizedbelowcmb", "0322hookretroflexbelowcmb",
				"0323dotbelowcomb", "0324dieresisbelowcmb", "0325ringbelowcmb", "0327cedillacmb", "0328ogonekcmb", "0329verticallinebelowcmb", "032Abridgebelowcmb", "032Bdblarchinvertedbelowcmb", "032Ccaronbelowcmb", "032Dcircumflexbelowcmb",
				"032Ebrevebelowcmb", "032Fbreveinvertedbelowcmb", "0330tildebelowcmb", "0331macronbelowcmb", "0332lowlinecmb", "0333dbllowlinecmb", "0334tildeoverlaycmb", "0335strokeshortoverlaycmb", "0336strokelongoverlaycmb", "0337solidusshortoverlaycmb",
				"0338soliduslongoverlaycmb", "0339ringhalfrightbelowcmb", "033Abridgeinvertedbelowcmb", "033Bsquarebelowcmb", "033Cseagullbelowcmb", "033Dxabovecmb", "033Etildeverticalcmb", "033Fdbloverlinecmb", "0340gravetonecmb", "0341acutetonecmb",
				"0342perispomenigreekcmb", "0343koroniscmb", "0344dialytikatonoscmb", "0345ypogegrammenigreekcmb", "0360tildedoublecmb", "0361breveinverteddoublecmb", "0374numeralsigngreek", "0375numeralsignlowergreek", "037Aypogegrammeni", "037Equestiongreek",
				"0384tonos", "0385dieresistonos", "0386Alphatonos", "0387anoteleia", "0388Epsilontonos", "0389Etatonos", "038AIotatonos", "038COmicrontonos", "038EUpsilontonos", "038FOmegatonos",
				"0390iotadieresistonos", "0391Alpha", "0392Beta", "0393Gamma", "0394Deltagreek", "0395Epsilon", "0396Zeta", "0397Eta", "0398Theta", "0399Iota",
				"039AKappa", "039BLambda", "039CMu", "039DNu", "039EXi", "039FOmicron", "03A0Pi", "03A1Rho", "03A3Sigma", "03A4Tau",
				"03A5Upsilon", "03A6Phi", "03A7Chi", "03A8Psi", "03A9Omegagreek", "03AAIotadieresis", "03ABUpsilondieresis", "03ACalphatonos", "03ADepsilontonos", "03AEetatonos",
				"03AFiotatonos", "03B0upsilondieresistonos", "03B1alpha", "03B2beta", "03B3gamma", "03B4delta", "03B5epsilon", "03B6zeta", "03B7eta", "03B8theta",
				"03B9iota", "03BAkappa", "03BBlambda", "03BCmu", "03BDnu", "03BExi", "03BFomicron", "03C0pi", "03C1rho", "03C2sigma1",
				"03C3sigma", "03C4tau", "03C5upsilon", "03C6phi", "03C7chi", "03C8psi", "03C9omega", "03CAiotadieresis", "03CBupsilondieresis", "03CComicrontonos",
				"03CDupsilontonos", "03CEomegatonos", "03D0betasymbolgreek", "03D1theta1", "03D2Upsilon1", "03D3Upsilonacutehooksymbolgreek", "03D4Upsilondieresishooksymbolgreek", "03D5phi1", "03D6omega1", "03DAStigmagreek",
				"03DCDigammagreek", "03DEKoppagreek", "03E0Sampigreek", "03E2Sheicoptic", "03E3sheicoptic", "03E4Feicoptic", "03E5feicoptic", "03E6Kheicoptic", "03E7kheicoptic", "03E8Horicoptic",
				"03E9horicoptic", "03EAGangiacoptic", "03EBgangiacoptic", "03ECShimacoptic", "03EDshimacoptic", "03EEDeicoptic", "03EFdeicoptic", "03F0kappasymbolgreek", "03F1rhosymbolgreek", "03F2sigmalunatesymbolgreek",
				"03F3yotgreek", "0401afii10023", "0402afii10051", "0403afii10052", "0404afii10053", "0405afii10054", "0406afii10055", "0407afii10056", "0408afii10057", "0409afii10058",
				"040Aafii10059", "040Bafii10060", "040Cafii10061", "040Eafii10062", "040Fafii10145", "0410afii10017", "0411afii10018", "0412afii10019", "0413afii10020", "0414afii10021",
				"0415afii10022", "0416afii10024", "0417afii10025", "0418afii10026", "0419afii10027", "041Aafii10028", "041Bafii10029", "041Cafii10030", "041Dafii10031", "041Eafii10032",
				"041Fafii10033", "0420afii10034", "0421afii10035", "0422afii10036", "0423afii10037", "0424afii10038", "0425afii10039", "0426afii10040", "0427afii10041", "0428afii10042",
				"0429afii10043", "042Aafii10044", "042Bafii10045", "042Cafii10046", "042Dafii10047", "042Eafii10048", "042Fafii10049", "0430afii10065", "0431afii10066", "0432afii10067",
				"0433afii10068", "0434afii10069", "0435afii10070", "0436afii10072", "0437afii10073", "0438afii10074", "0439afii10075", "043Aafii10076", "043Bafii10077", "043Cafii10078",
				"043Dafii10079", "043Eafii10080", "043Fafii10081", "0440afii10082", "0441afii10083", "0442afii10084", "0443afii10085", "0444afii10086", "0445afii10087", "0446afii10088",
				"0447afii10089", "0448afii10090", "0449afii10091", "044Aafii10092", "044Bafii10093", "044Cafii10094", "044Dafii10095", "044Eafii10096", "044Fafii10097", "0451afii10071",
				"0452afii10099", "0453afii10100", "0454afii10101", "0455afii10102", "0456afii10103", "0457afii10104", "0458afii10105", "0459afii10106", "045Aafii10107", "045Bafii10108",
				"045Cafii10109", "045Eafii10110", "045Fafii10193", "0460Omegacyrillic", "0461omegacyrillic", "0462afii10146", "0463afii10194", "0464Eiotifiedcyrillic", "0465eiotifiedcyrillic", "0466Yuslittlecyrillic",
				"0467yuslittlecyrillic", "0468Yuslittleiotifiedcyrillic", "0469yuslittleiotifiedcyrillic", "046AYusbigcyrillic", "046Byusbigcyrillic", "046CYusbigiotifiedcyrillic", "046Dyusbigiotifiedcyrillic", "046EKsicyrillic", "046Fksicyrillic", "0470Psicyrillic",
				"0471psicyrillic", "0472afii10147", "0473afii10195", "0474afii10148", "0475afii10196", "0476Izhitsadblgravecyrillic", "0477izhitsadblgravecyrillic", "0478Ukcyrillic", "0479ukcyrillic", "047AOmegaroundcyrillic",
				"047Bomegaroundcyrillic", "047COmegatitlocyrillic", "047Domegatitlocyrillic", "047EOtcyrillic", "047Fotcyrillic", "0480Koppacyrillic", "0481koppacyrillic", "0482thousandcyrillic", "0483titlocyrilliccmb", "0484palatalizationcyrilliccmb",
				"0485dasiapneumatacyrilliccmb", "0486psilipneumatacyrilliccmb", "0490afii10050", "0491afii10098", "0492Ghestrokecyrillic", "0493ghestrokecyrillic", "0494Ghemiddlehookcyrillic", "0495ghemiddlehookcyrillic", "0496Zhedescendercyrillic", "0497zhedescendercyrillic",
				"0498Zedescendercyrillic", "0499zedescendercyrillic", "049AKadescendercyrillic", "049Bkadescendercyrillic", "049CKaverticalstrokecyrillic", "049Dkaverticalstrokecyrillic", "049EKastrokecyrillic", "049Fkastrokecyrillic", "04A0Kabashkircyrillic", "04A1kabashkircyrillic",
				"04A2Endescendercyrillic", "04A3endescendercyrillic", "04A4Enghecyrillic", "04A5enghecyrillic", "04A6Pemiddlehookcyrillic", "04A7pemiddlehookcyrillic", "04A8Haabkhasiancyrillic", "04A9haabkhasiancyrillic", "04AAEsdescendercyrillic", "04ABesdescendercyrillic",
				"04ACTedescendercyrillic", "04ADtedescendercyrillic", "04AEUstraightcyrillic", "04AFustraightcyrillic", "04B0Ustraightstrokecyrillic", "04B1ustraightstrokecyrillic", "04B2Hadescendercyrillic", "04B3hadescendercyrillic", "04B4Tetsecyrillic", "04B5tetsecyrillic",
				"04B6Chedescendercyrillic", "04B7chedescendercyrillic", "04B8Cheverticalstrokecyrillic", "04B9cheverticalstrokecyrillic", "04BAShhacyrillic", "04BBshhacyrillic", "04BCCheabkhasiancyrillic", "04BDcheabkhasiancyrillic", "04BEChedescenderabkhasiancyrillic", "04BFchedescenderabkhasiancyrillic",
				"04C0palochkacyrillic", "04C1Zhebrevecyrillic", "04C2zhebrevecyrillic", "04C3Kahookcyrillic", "04C4kahookcyrillic", "04C7Enhookcyrillic", "04C8enhookcyrillic", "04CBChekhakassiancyrillic", "04CCchekhakassiancyrillic", "04D0Abrevecyrillic",
				"04D1abrevecyrillic", "04D2Adieresiscyrillic", "04D3adieresiscyrillic", "04D4Aiecyrillic", "04D5aiecyrillic", "04D6Iebrevecyrillic", "04D7iebrevecyrillic", "04D8Schwacyrillic", "04D9afii10846", "04DASchwadieresiscyrillic",
				"04DBschwadieresiscyrillic", "04DCZhedieresiscyrillic", "04DDzhedieresiscyrillic", "04DEZedieresiscyrillic", "04DFzedieresiscyrillic", "04E0Dzeabkhasiancyrillic", "04E1dzeabkhasiancyrillic", "04E2Imacroncyrillic", "04E3imacroncyrillic", "04E4Idieresiscyrillic",
				"04E5idieresiscyrillic", "04E6Odieresiscyrillic", "04E7odieresiscyrillic", "04E8Obarredcyrillic", "04E9obarredcyrillic", "04EAObarreddieresiscyrillic", "04EBobarreddieresiscyrillic", "04EEUmacroncyrillic", "04EFumacroncyrillic", "04F0Udieresiscyrillic",
				"04F1udieresiscyrillic", "04F2Uhungarumlautcyrillic", "04F3uhungarumlautcyrillic", "04F4Chedieresiscyrillic", "04F5chedieresiscyrillic", "04F8Yerudieresiscyrillic", "04F9yerudieresiscyrillic", "0531Aybarmenian", "0532Benarmenian", "0533Gimarmenian",
				"0534Daarmenian", "0535Echarmenian", "0536Zaarmenian", "0537Eharmenian", "0538Etarmenian", "0539Toarmenian", "053AZhearmenian", "053BIniarmenian", "053CLiwnarmenian", "053DXeharmenian",
				"053ECaarmenian", "053FKenarmenian", "0540Hoarmenian", "0541Jaarmenian", "0542Ghadarmenian", "0543Cheharmenian", "0544Menarmenian", "0545Yiarmenian", "0546Nowarmenian", "0547Shaarmenian",
				"0548Voarmenian", "0549Chaarmenian", "054APeharmenian", "054BJheharmenian", "054CRaarmenian", "054DSeharmenian", "054EVewarmenian", "054FTiwnarmenian", "0550Reharmenian", "0551Coarmenian",
				"0552Yiwnarmenian", "0553Piwrarmenian", "0554Keharmenian", "0555Oharmenian", "0556Feharmenian", "0559ringhalfleftarmenian", "055Aapostrophearmenian", "055Bemphasismarkarmenian", "055Cexclamarmenian", "055Dcommaarmenian",
				"055Equestionarmenian", "055Fabbreviationmarkarmenian", "0561aybarmenian", "0562benarmenian", "0563gimarmenian", "0564daarmenian", "0565echarmenian", "0566zaarmenian", "0567eharmenian", "0568etarmenian",
				"0569toarmenian", "056Azhearmenian", "056Biniarmenian", "056Cliwnarmenian", "056Dxeharmenian", "056Ecaarmenian", "056Fkenarmenian", "0570hoarmenian", "0571jaarmenian", "0572ghadarmenian",
				"0573cheharmenian", "0574menarmenian", "0575yiarmenian", "0576nowarmenian", "0577shaarmenian", "0578voarmenian", "0579chaarmenian", "057Apeharmenian", "057Bjheharmenian", "057Craarmenian",
				"057Dseharmenian", "057Evewarmenian", "057Ftiwnarmenian", "0580reharmenian", "0581coarmenian", "0582yiwnarmenian", "0583piwrarmenian", "0584keharmenian", "0585oharmenian", "0586feharmenian",
				"0587echyiwnarmenian", "0589periodarmenian", "0591etnahtahebrew", "0592segoltahebrew", "0593shalshelethebrew", "0594zaqefqatanhebrew", "0595zaqefgadolhebrew", "0596tipehahebrew", "0597reviahebrew", "0598zarqahebrew",
				"0599pashtahebrew", "059Ayetivhebrew", "059Btevirhebrew", "059Cgereshaccenthebrew", "059Dgereshmuqdamhebrew", "059Egershayimaccenthebrew", "059Fqarneyparahebrew", "05A0telishagedolahebrew", "05A1pazerhebrew", "05A3munahhebrew",
				"05A4mahapakhhebrew", "05A5merkhahebrew", "05A6merkhakefulahebrew", "05A7dargahebrew", "05A8qadmahebrew", "05A9telishaqetanahebrew", "05AAyerahbenyomohebrew", "05ABolehebrew", "05ACiluyhebrew", "05ADdehihebrew",
				"05AEzinorhebrew", "05AFmasoracirclehebrew", "05B0afii57799", "05B1afii57801", "05B2afii57800", "05B3afii57802", "05B4afii57793", "05B5afii57794", "05B6afii57795", "05B7afii57798",
				"05B8afii57797", "05B9afii57806", "05BBafii57796", "05BCafii57807", "05BDafii57839", "05BEafii57645", "05BFafii57841", "05C0afii57842", "05C1afii57804", "05C2afii57803",
				"05C3afii57658", "05C4upperdothebrew", "05D0afii57664", "05D1afii57665", "05D2afii57666", "05D3afii57667", "05D4afii57668", "05D5afii57669", "05D6afii57670", "05D7afii57671",
				"05D8afii57672", "05D9afii57673", "05DAafii57674", "05DBafii57675", "05DCafii57676", "05DDafii57677", "05DEafii57678", "05DFafii57679", "05E0afii57680", "05E1afii57681",
				"05E2afii57682", "05E3afii57683", "05E4afii57684", "05E5afii57685", "05E6afii57686", "05E7afii57687", "05E8afii57688", "05E9afii57689", "05EAafii57690", "05F0afii57716",
				"05F1afii57717", "05F2afii57718", "05F3gereshhebrew", "05F4gershayimhebrew", "060Cafii57388", "061Bafii57403", "061Fafii57407", "0621afii57409", "0622afii57410", "0623afii57411",
				"0624afii57412", "0625afii57413", "0626afii57414", "0627afii57415", "0628afii57416", "0629afii57417", "062Aafii57418", "062Bafii57419", "062Cafii57420", "062Dafii57421",
				"062Eafii57422", "062Fafii57423", "0630afii57424", "0631afii57425", "0632afii57426", "0633afii57427", "0634afii57428", "0635afii57429", "0636afii57430", "0637afii57431",
				"0638afii57432", "0639afii57433", "063Aafii57434", "0640afii57440", "0641afii57441", "0642afii57442", "0643afii57443", "0644afii57444", "0645afii57445", "0646afii57446",
				"0647afii57470", "0648afii57448", "0649afii57449", "064Aafii57450", "064Bafii57451", "064Cafii57452", "064Dafii57453", "064Eafii57454", "064Fafii57455", "0650afii57456",
				"0651afii57457", "0652afii57458", "0660afii57392", "0661afii57393", "0662afii57394", "0663afii57395", "0664afii57396", "0665afii57397", "0666afii57398", "0667afii57399",
				"0668afii57400", "0669afii57401", "066Aafii57381", "066Bdecimalseparatorarabic", "066Cthousandsseparatorarabic", "066Dafii63167", "0679afii57511", "067Eafii57506", "0686afii57507", "0688afii57512",
				"0691afii57513", "0698afii57508", "06A4afii57505", "06AFafii57509", "06BAafii57514", "06C1hehaltonearabic", "06D1yehthreedotsbelowarabic", "06D2afii57519", "06D5afii57534", "06F0zeropersian",
				"06F1onepersian", "06F2twopersian", "06F3threepersian", "06F4fourpersian", "06F5fivepersian", "06F6sixpersian", "06F7sevenpersian", "06F8eightpersian", "06F9ninepersian", "0901candrabindudeva",
				"0902anusvaradeva", "0903visargadeva", "0905adeva", "0906aadeva", "0907ideva", "0908iideva", "0909udeva", "090Auudeva", "090Brvocalicdeva", "090Clvocalicdeva",
				"090Decandradeva", "090Eeshortdeva", "090Fedeva", "0910aideva", "0911ocandradeva", "0912oshortdeva", "0913odeva", "0914audeva", "0915kadeva", "0916khadeva",
				"0917gadeva", "0918ghadeva", "0919ngadeva", "091Acadeva", "091Bchadeva", "091Cjadeva", "091Djhadeva", "091Enyadeva", "091Fttadeva", "0920tthadeva",
				"0921ddadeva", "0922ddhadeva", "0923nnadeva", "0924tadeva", "0925thadeva", "0926dadeva", "0927dhadeva", "0928nadeva", "0929nnnadeva", "092Apadeva",
				"092Bphadeva", "092Cbadeva", "092Dbhadeva", "092Emadeva", "092Fyadeva", "0930radeva", "0931rradeva", "0932ladeva", "0933lladeva", "0934llladeva",
				"0935vadeva", "0936shadeva", "0937ssadeva", "0938sadeva", "0939hadeva", "093Cnuktadeva", "093Davagrahadeva", "093Eaavowelsigndeva", "093Fivowelsigndeva", "0940iivowelsigndeva",
				"0941uvowelsigndeva", "0942uuvowelsigndeva", "0943rvocalicvowelsigndeva", "0944rrvocalicvowelsigndeva", "0945ecandravowelsigndeva", "0946eshortvowelsigndeva", "0947evowelsigndeva", "0948aivowelsigndeva", "0949ocandravowelsigndeva", "094Aoshortvowelsigndeva",
				"094Bovowelsigndeva", "094Cauvowelsigndeva", "094Dviramadeva", "0950omdeva", "0951udattadeva", "0952anudattadeva", "0953gravedeva", "0954acutedeva", "0958qadeva", "0959khhadeva",
				"095Aghhadeva", "095Bzadeva", "095Cdddhadeva", "095Drhadeva", "095Efadeva", "095Fyyadeva", "0960rrvocalicdeva", "0961llvocalicdeva", "0962lvocalicvowelsigndeva", "0963llvocalicvowelsigndeva",
				"0964danda", "0965dbldanda", "0966zerodeva", "0967onedeva", "0968twodeva", "0969threedeva", "096Afourdeva", "096Bfivedeva", "096Csixdeva", "096Dsevendeva",
				"096Eeightdeva", "096Fninedeva", "0970abbreviationsigndeva", "0981candrabindubengali", "0982anusvarabengali", "0983visargabengali", "0985abengali", "0986aabengali", "0987ibengali", "0988iibengali",
				"0989ubengali", "098Auubengali", "098Brvocalicbengali", "098Clvocalicbengali", "098Febengali", "0990aibengali", "0993obengali", "0994aubengali", "0995kabengali", "0996khabengali",
				"0997gabengali", "0998ghabengali", "0999ngabengali", "099Acabengali", "099Bchabengali", "099Cjabengali", "099Djhabengali", "099Enyabengali", "099Fttabengali", "09A0tthabengali",
				"09A1ddabengali", "09A2ddhabengali", "09A3nnabengali", "09A4tabengali", "09A5thabengali", "09A6dabengali", "09A7dhabengali", "09A8nabengali", "09AApabengali", "09ABphabengali",
				"09ACbabengali", "09ADbhabengali", "09AEmabengali", "09AFyabengali", "09B0rabengali", "09B2labengali", "09B6shabengali", "09B7ssabengali", "09B8sabengali", "09B9habengali",
				"09BCnuktabengali", "09BEaavowelsignbengali", "09BFivowelsignbengali", "09C0iivowelsignbengali", "09C1uvowelsignbengali", "09C2uuvowelsignbengali", "09C3rvocalicvowelsignbengali", "09C4rrvocalicvowelsignbengali", "09C7evowelsignbengali", "09C8aivowelsignbengali",
				"09CBovowelsignbengali", "09CCauvowelsignbengali", "09CDviramabengali", "09D7aulengthmarkbengali", "09DCrrabengali", "09DDrhabengali", "09DFyyabengali", "09E0rrvocalicbengali", "09E1llvocalicbengali", "09E2lvocalicvowelsignbengali",
				"09E3llvocalicvowelsignbengali", "09E6zerobengali", "09E7onebengali", "09E8twobengali", "09E9threebengali", "09EAfourbengali", "09EBfivebengali", "09ECsixbengali", "09EDsevenbengali", "09EEeightbengali",
				"09EFninebengali", "09F0ramiddlediagonalbengali", "09F1ralowerdiagonalbengali", "09F2rupeemarkbengali", "09F3rupeesignbengali", "09F4onenumeratorbengali", "09F5twonumeratorbengali", "09F6threenumeratorbengali", "09F7fournumeratorbengali", "09F8denominatorminusonenumeratorbengali",
				"09F9sixteencurrencydenominatorbengali", "09FAissharbengali", "0A02bindigurmukhi", "0A05agurmukhi", "0A06aagurmukhi", "0A07igurmukhi", "0A08iigurmukhi", "0A09ugurmukhi", "0A0Auugurmukhi", "0A0Feegurmukhi",
				"0A10aigurmukhi", "0A13oogurmukhi", "0A14augurmukhi", "0A15kagurmukhi", "0A16khagurmukhi", "0A17gagurmukhi", "0A18ghagurmukhi", "0A19ngagurmukhi", "0A1Acagurmukhi", "0A1Bchagurmukhi",
				"0A1Cjagurmukhi", "0A1Djhagurmukhi", "0A1Enyagurmukhi", "0A1Fttagurmukhi", "0A20tthagurmukhi", "0A21ddagurmukhi", "0A22ddhagurmukhi", "0A23nnagurmukhi", "0A24tagurmukhi", "0A25thagurmukhi",
				"0A26dagurmukhi", "0A27dhagurmukhi", "0A28nagurmukhi", "0A2Apagurmukhi", "0A2Bphagurmukhi", "0A2Cbagurmukhi", "0A2Dbhagurmukhi", "0A2Emagurmukhi", "0A2Fyagurmukhi", "0A30ragurmukhi",
				"0A32lagurmukhi", "0A35vagurmukhi", "0A36shagurmukhi", "0A38sagurmukhi", "0A39hagurmukhi", "0A3Cnuktagurmukhi", "0A3Eaamatragurmukhi", "0A3Fimatragurmukhi", "0A40iimatragurmukhi", "0A41umatragurmukhi",
				"0A42uumatragurmukhi", "0A47eematragurmukhi", "0A48aimatragurmukhi", "0A4Boomatragurmukhi", "0A4Caumatragurmukhi", "0A4Dhalantgurmukhi", "0A59khhagurmukhi", "0A5Aghhagurmukhi", "0A5Bzagurmukhi", "0A5Crragurmukhi",
				"0A5Efagurmukhi", "0A66zerogurmukhi", "0A67onegurmukhi", "0A68twogurmukhi", "0A69threegurmukhi", "0A6Afourgurmukhi", "0A6Bfivegurmukhi", "0A6Csixgurmukhi", "0A6Dsevengurmukhi", "0A6Eeightgurmukhi",
				"0A6Fninegurmukhi", "0A70tippigurmukhi", "0A71addakgurmukhi", "0A72irigurmukhi", "0A73uragurmukhi", "0A74ekonkargurmukhi", "0A81candrabindugujarati", "0A82anusvaragujarati", "0A83visargagujarati", "0A85agujarati",
				"0A86aagujarati", "0A87igujarati", "0A88iigujarati", "0A89ugujarati", "0A8Auugujarati", "0A8Brvocalicgujarati", "0A8Decandragujarati", "0A8Fegujarati", "0A90aigujarati", "0A91ocandragujarati",
				"0A93ogujarati", "0A94augujarati", "0A95kagujarati", "0A96khagujarati", "0A97gagujarati", "0A98ghagujarati", "0A99ngagujarati", "0A9Acagujarati", "0A9Bchagujarati", "0A9Cjagujarati",
				"0A9Djhagujarati", "0A9Enyagujarati", "0A9Fttagujarati", "0AA0tthagujarati", "0AA1ddagujarati", "0AA2ddhagujarati", "0AA3nnagujarati", "0AA4tagujarati", "0AA5thagujarati", "0AA6dagujarati",
				"0AA7dhagujarati", "0AA8nagujarati", "0AAApagujarati", "0AABphagujarati", "0AACbagujarati", "0AADbhagujarati", "0AAEmagujarati", "0AAFyagujarati", "0AB0ragujarati", "0AB2lagujarati",
				"0AB3llagujarati", "0AB5vagujarati", "0AB6shagujarati", "0AB7ssagujarati", "0AB8sagujarati", "0AB9hagujarati", "0ABCnuktagujarati", "0ABEaavowelsigngujarati", "0ABFivowelsigngujarati", "0AC0iivowelsigngujarati",
				"0AC1uvowelsigngujarati", "0AC2uuvowelsigngujarati", "0AC3rvocalicvowelsigngujarati", "0AC4rrvocalicvowelsigngujarati", "0AC5ecandravowelsigngujarati", "0AC7evowelsigngujarati", "0AC8aivowelsigngujarati", "0AC9ocandravowelsigngujarati", "0ACBovowelsigngujarati", "0ACCauvowelsigngujarati",
				"0ACDviramagujarati", "0AD0omgujarati", "0AE0rrvocalicgujarati", "0AE6zerogujarati", "0AE7onegujarati", "0AE8twogujarati", "0AE9threegujarati", "0AEAfourgujarati", "0AEBfivegujarati", "0AECsixgujarati",
				"0AEDsevengujarati", "0AEEeightgujarati", "0AEFninegujarati", "0E01kokaithai", "0E02khokhaithai", "0E03khokhuatthai", "0E04khokhwaithai", "0E05khokhonthai", "0E06khorakhangthai", "0E07ngonguthai",
				"0E08chochanthai", "0E09chochingthai", "0E0Achochangthai", "0E0Bsosothai", "0E0Cchochoethai", "0E0Dyoyingthai", "0E0Edochadathai", "0E0Ftopatakthai", "0E10thothanthai", "0E11thonangmonthothai",
				"0E12thophuthaothai", "0E13nonenthai", "0E14dodekthai", "0E15totaothai", "0E16thothungthai", "0E17thothahanthai", "0E18thothongthai", "0E19nonuthai", "0E1Abobaimaithai", "0E1Bpoplathai",
				"0E1Cphophungthai", "0E1Dfofathai", "0E1Ephophanthai", "0E1Ffofanthai", "0E20phosamphaothai", "0E21momathai", "0E22yoyakthai", "0E23roruathai", "0E24ruthai", "0E25lolingthai",
				"0E26luthai", "0E27wowaenthai", "0E28sosalathai", "0E29sorusithai", "0E2Asosuathai", "0E2Bhohipthai", "0E2Clochulathai", "0E2Doangthai", "0E2Ehonokhukthai", "0E2Fpaiyannoithai",
				"0E30saraathai", "0E31maihanakatthai", "0E32saraaathai", "0E33saraamthai", "0E34saraithai", "0E35saraiithai", "0E36sarauethai", "0E37saraueethai", "0E38sarauthai", "0E39sarauuthai",
				"0E3Aphinthuthai", "0E3Fbahtthai", "0E40saraethai", "0E41saraaethai", "0E42saraothai", "0E43saraaimaimuanthai", "0E44saraaimaimalaithai", "0E45lakkhangyaothai", "0E46maiyamokthai", "0E47maitaikhuthai",
				"0E48maiekthai", "0E49maithothai", "0E4Amaitrithai", "0E4Bmaichattawathai", "0E4Cthanthakhatthai", "0E4Dnikhahitthai", "0E4Eyamakkanthai", "0E4Ffongmanthai", "0E50zerothai", "0E51onethai",
				"0E52twothai", "0E53threethai", "0E54fourthai", "0E55fivethai", "0E56sixthai", "0E57seventhai", "0E58eightthai", "0E59ninethai", "0E5Aangkhankhuthai", "0E5Bkhomutthai",
				"1E00Aringbelow", "1E01aringbelow", "1E02Bdotaccent", "1E03bdotaccent", "1E04Bdotbelow", "1E05bdotbelow", "1E06Blinebelow", "1E07blinebelow", "1E08Ccedillaacute", "1E09ccedillaacute",
				"1E0ADdotaccent", "1E0Bddotaccent", "1E0CDdotbelow", "1E0Dddotbelow", "1E0EDlinebelow", "1E0Fdlinebelow", "1E10Dcedilla", "1E11dcedilla", "1E12Dcircumflexbelow", "1E13dcircumflexbelow",
				"1E14Emacrongrave", "1E15emacrongrave", "1E16Emacronacute", "1E17emacronacute", "1E18Ecircumflexbelow", "1E19ecircumflexbelow", "1E1AEtildebelow", "1E1Betildebelow", "1E1CEcedillabreve", "1E1Decedillabreve",
				"1E1EFdotaccent", "1E1Ffdotaccent", "1E20Gmacron", "1E21gmacron", "1E22Hdotaccent", "1E23hdotaccent", "1E24Hdotbelow", "1E25hdotbelow", "1E26Hdieresis", "1E27hdieresis",
				"1E28Hcedilla", "1E29hcedilla", "1E2AHbrevebelow", "1E2Bhbrevebelow", "1E2CItildebelow", "1E2Ditildebelow", "1E2EIdieresisacute", "1E2Fidieresisacute", "1E30Kacute", "1E31kacute",
				"1E32Kdotbelow", "1E33kdotbelow", "1E34Klinebelow", "1E35klinebelow", "1E36Ldotbelow", "1E37ldotbelow", "1E38Ldotbelowmacron", "1E39ldotbelowmacron", "1E3ALlinebelow", "1E3Bllinebelow",
				"1E3CLcircumflexbelow", "1E3Dlcircumflexbelow", "1E3EMacute", "1E3Fmacute", "1E40Mdotaccent", "1E41mdotaccent", "1E42Mdotbelow", "1E43mdotbelow", "1E44Ndotaccent", "1E45ndotaccent",
				"1E46Ndotbelow", "1E47ndotbelow", "1E48Nlinebelow", "1E49nlinebelow", "1E4ANcircumflexbelow", "1E4Bncircumflexbelow", "1E4COtildeacute", "1E4Dotildeacute", "1E4EOtildedieresis", "1E4Fotildedieresis",
				"1E50Omacrongrave", "1E51omacrongrave", "1E52Omacronacute", "1E53omacronacute", "1E54Pacute", "1E55pacute", "1E56Pdotaccent", "1E57pdotaccent", "1E58Rdotaccent", "1E59rdotaccent",
				"1E5ARdotbelow", "1E5Brdotbelow", "1E5CRdotbelowmacron", "1E5Drdotbelowmacron", "1E5ERlinebelow", "1E5Frlinebelow", "1E60Sdotaccent", "1E61sdotaccent", "1E62Sdotbelow", "1E63sdotbelow",
				"1E64Sacutedotaccent", "1E65sacutedotaccent", "1E66Scarondotaccent", "1E67scarondotaccent", "1E68Sdotbelowdotaccent", "1E69sdotbelowdotaccent", "1E6ATdotaccent", "1E6Btdotaccent", "1E6CTdotbelow", "1E6Dtdotbelow",
				"1E6ETlinebelow", "1E6Ftlinebelow", "1E70Tcircumflexbelow", "1E71tcircumflexbelow", "1E72Udieresisbelow", "1E73udieresisbelow", "1E74Utildebelow", "1E75utildebelow", "1E76Ucircumflexbelow", "1E77ucircumflexbelow",
				"1E78Utildeacute", "1E79utildeacute", "1E7AUmacrondieresis", "1E7Bumacrondieresis", "1E7CVtilde", "1E7Dvtilde", "1E7EVdotbelow", "1E7Fvdotbelow", "1E80Wgrave", "1E81wgrave",
				"1E82Wacute", "1E83wacute", "1E84Wdieresis", "1E85wdieresis", "1E86Wdotaccent", "1E87wdotaccent", "1E88Wdotbelow", "1E89wdotbelow", "1E8AXdotaccent", "1E8Bxdotaccent",
				"1E8CXdieresis", "1E8Dxdieresis", "1E8EYdotaccent", "1E8Fydotaccent", "1E90Zcircumflex", "1E91zcircumflex", "1E92Zdotbelow", "1E93zdotbelow", "1E94Zlinebelow", "1E95zlinebelow",
				"1E96hlinebelow", "1E97tdieresis", "1E98wring", "1E99yring", "1E9Aarighthalfring", "1E9Bslongdotaccent", "1EA0Adotbelow", "1EA1adotbelow", "1EA2Ahookabove", "1EA3ahookabove",
				"1EA4Acircumflexacute", "1EA5acircumflexacute", "1EA6Acircumflexgrave", "1EA7acircumflexgrave", "1EA8Acircumflexhookabove", "1EA9acircumflexhookabove", "1EAAAcircumflextilde", "1EABacircumflextilde", "1EACAcircumflexdotbelow", "1EADacircumflexdotbelow",
				"1EAEAbreveacute", "1EAFabreveacute", "1EB0Abrevegrave", "1EB1abrevegrave", "1EB2Abrevehookabove", "1EB3abrevehookabove", "1EB4Abrevetilde", "1EB5abrevetilde", "1EB6Abrevedotbelow", "1EB7abrevedotbelow",
				"1EB8Edotbelow", "1EB9edotbelow", "1EBAEhookabove", "1EBBehookabove", "1EBCEtilde", "1EBDetilde", "1EBEEcircumflexacute", "1EBFecircumflexacute", "1EC0Ecircumflexgrave", "1EC1ecircumflexgrave",
				"1EC2Ecircumflexhookabove", "1EC3ecircumflexhookabove", "1EC4Ecircumflextilde", "1EC5ecircumflextilde", "1EC6Ecircumflexdotbelow", "1EC7ecircumflexdotbelow", "1EC8Ihookabove", "1EC9ihookabove", "1ECAIdotbelow", "1ECBidotbelow",
				"1ECCOdotbelow", "1ECDodotbelow", "1ECEOhookabove", "1ECFohookabove", "1ED0Ocircumflexacute", "1ED1ocircumflexacute", "1ED2Ocircumflexgrave", "1ED3ocircumflexgrave", "1ED4Ocircumflexhookabove", "1ED5ocircumflexhookabove",
				"1ED6Ocircumflextilde", "1ED7ocircumflextilde", "1ED8Ocircumflexdotbelow", "1ED9ocircumflexdotbelow", "1EDAOhornacute", "1EDBohornacute", "1EDCOhorngrave", "1EDDohorngrave", "1EDEOhornhookabove", "1EDFohornhookabove",
				"1EE0Ohorntilde", "1EE1ohorntilde", "1EE2Ohorndotbelow", "1EE3ohorndotbelow", "1EE4Udotbelow", "1EE5udotbelow", "1EE6Uhookabove", "1EE7uhookabove", "1EE8Uhornacute", "1EE9uhornacute",
				"1EEAUhorngrave", "1EEBuhorngrave", "1EECUhornhookabove", "1EEDuhornhookabove", "1EEEUhorntilde", "1EEFuhorntilde", "1EF0Uhorndotbelow", "1EF1uhorndotbelow", "1EF2Ygrave", "1EF3ygrave",
				"1EF4Ydotbelow", "1EF5ydotbelow", "1EF6Yhookabove", "1EF7yhookabove", "1EF8Ytilde", "1EF9ytilde", "2002enspace", "200Bzerowidthspace", "200Cafii61664", "200Dafii301",
				"200Eafii299", "200Fafii300", "2010hyphentwo", "2012figuredash", "2013endash", "2014emdash", "2015afii00208", "2016dblverticalbar", "2017underscoredbl", "2018quoteleft",
				"2019quoteright", "201Aquotesinglbase", "201Bquotereversed", "201Cquotedblleft", "201Dquotedblright", "201Equotedblbase", "2020dagger", "2021daggerdbl", "2022bullet", "2024onedotenleader",
				"2025twodotenleader", "2026ellipsis", "202Cafii61573", "202Dafii61574", "202Eafii61575", "2030perthousand", "2032minute", "2033second", "2035primereversed", "2039guilsinglleft",
				"203Aguilsinglright", "203Breferencemark", "203Cexclamdbl", "203Eoverline", "2042asterism", "2044fraction", "2070zerosuperior", "2074foursuperior", "2075fivesuperior", "2076sixsuperior",
				"2077sevensuperior", "2078eightsuperior", "2079ninesuperior", "207Aplussuperior", "207Cequalsuperior", "207Dparenleftsuperior", "207Eparenrightsuperior", "207Fnsuperior", "2080zeroinferior", "2081oneinferior",
				"2082twoinferior", "2083threeinferior", "2084fourinferior", "2085fiveinferior", "2086sixinferior", "2087seveninferior", "2088eightinferior", "2089nineinferior", "208Dparenleftinferior", "208Eparenrightinferior",
				"20A1colonmonetary", "20A2cruzeiro", "20A3franc", "20A4afii08941", "20A7peseta", "20A9won", "20AAafii57636", "20ABdong", "20ACEuro", "2103centigrade",
				"2105afii61248", "2109fahrenheit", "2111Ifraktur", "2113afii61289", "2116afii61352", "2118weierstrass", "211CRfraktur", "211Eprescription", "2121telephone", "2122trademark",
				"2126Omega", "212Bangstrom", "212Eestimated", "2135aleph", "2153onethird", "2154twothirds", "215Boneeighth", "215Cthreeeighths", "215Dfiveeighths", "215Eseveneighths",
				"2160Oneroman", "2161Tworoman", "2162Threeroman", "2163Fourroman", "2164Fiveroman", "2165Sixroman", "2166Sevenroman", "2167Eightroman", "2168Nineroman", "2169Tenroman",
				"216AElevenroman", "216BTwelveroman", "2170oneroman", "2171tworoman", "2172threeroman", "2173fourroman", "2174fiveroman", "2175sixroman", "2176sevenroman", "2177eightroman",
				"2178nineroman", "2179tenroman", "217Aelevenroman", "217Btwelveroman", "2190arrowleft", "2191arrowup", "2192arrowright", "2193arrowdown", "2194arrowboth", "2195arrowupdn",
				"2196arrowupleft", "2197arrowupright", "2198arrowdownright", "2199arrowdownleft", "21A8arrowupdnbse", "21B5carriagereturn", "21BCharpoonleftbarbup", "21C0harpoonrightbarbup", "21C4arrowrightoverleft", "21C5arrowupleftofdown",
				"21C6arrowleftoverright", "21CDarrowleftdblstroke", "21CFarrowrightdblstroke", "21D0arrowdblleft", "21D1arrowdblup", "21D2arrowdblright", "21D3arrowdbldown", "21D4arrowdblboth", "21DEpageup", "21DFpagedown",
				"21E0arrowdashleft", "21E1arrowdashup", "21E2arrowdashright", "21E3arrowdashdown", "21E4arrowtableft", "21E5arrowtabright", "21E6arrowleftwhite", "21E7arrowupwhite", "21E8arrowrightwhite", "21E9arrowdownwhite",
				"21EAcapslock", "2200universal", "2202partialdiff", "2203existential", "2205emptyset", "2206Delta", "2207gradient", "2208element", "2209notelement", "220Bsuchthat",
				"220Cnotcontains", "220Fproduct", "2211summation", "2212minus", "2213minusplus", "2215divisionslash", "2217asteriskmath", "2219bulletoperator", "221Aradical", "221Dproportional",
				"221Einfinity", "221Forthogonal", "2220angle", "2223divides", "2225parallel", "2226notparallel", "2227logicaland", "2228logicalor", "2229intersection", "222Aunion",
				"222Bintegral", "222Cdblintegral", "222Econtourintegral", "2234therefore", "2235because", "2236ratio", "2237proportion", "223Csimilar", "223Dreversedtilde", "2243asymptoticallyequal",
				"2245congruent", "2248approxequal", "224Callequal", "2250approaches", "2251geometricallyequal", "2252approxequalorimage", "2253imageorapproximatelyequal", "2260notequal", "2261equivalence", "2262notidentical",
				"2264lessequal", "2265greaterequal", "2266lessoverequal", "2267greateroverequal", "226Amuchless", "226Bmuchgreater", "226Enotless", "226Fnotgreater", "2270notlessnorequal", "2271notgreaternorequal",
				"2272lessorequivalent", "2273greaterorequivalent", "2276lessorgreater", "2277greaterorless", "2279notgreaternorless", "227Aprecedes", "227Bsucceeds", "2280notprecedes", "2281notsucceeds", "2282propersubset",
				"2283propersuperset", "2284notsubset", "2285notsuperset", "2286reflexsubset", "2287reflexsuperset", "228Asubsetnotequal", "228Bsupersetnotequal", "2295circleplus", "2296minuscircle", "2297circlemultiply",
				"2299circleot", "22A3tackleft", "22A4tackdown", "22A5perpendicular", "22BFrighttriangle", "22C5dotmath", "22CEcurlyor", "22CFcurlyand", "22DAlessequalorgreater", "22DBgreaterequalorless",
				"22EEellipsisvertical", "2302house", "2303control", "2305projective", "2310revlogicalnot", "2312arc", "2318propellor", "2320integraltp", "2321integralbt", "2325option",
				"2326deleteright", "2327clear", "2329angleleft", "232Aangleright", "232Bdeleteleft", "2423blank", "2460onecircle", "2461twocircle", "2462threecircle", "2463fourcircle",
				"2464fivecircle", "2465sixcircle", "2466sevencircle", "2467eightcircle", "2468ninecircle", "2469tencircle", "246Aelevencircle", "246Btwelvecircle", "246Cthirteencircle", "246Dfourteencircle",
				"246Efifteencircle", "246Fsixteencircle", "2470seventeencircle", "2471eighteencircle", "2472nineteencircle", "2473twentycircle", "2474oneparen", "2475twoparen", "2476threeparen", "2477fourparen",
				"2478fiveparen", "2479sixparen", "247Asevenparen", "247Beightparen", "247Cnineparen", "247Dtenparen", "247Eelevenparen", "247Ftwelveparen", "2480thirteenparen", "2481fourteenparen",
				"2482fifteenparen", "2483sixteenparen", "2484seventeenparen", "2485eighteenparen", "2486nineteenparen", "2487twentyparen", "2488oneperiod", "2489twoperiod", "248Athreeperiod", "248Bfourperiod",
				"248Cfiveperiod", "248Dsixperiod", "248Esevenperiod", "248Feightperiod", "2490nineperiod", "2491tenperiod", "2492elevenperiod", "2493twelveperiod", "2494thirteenperiod", "2495fourteenperiod",
				"2496fifteenperiod", "2497sixteenperiod", "2498seventeenperiod", "2499eighteenperiod", "249Anineteenperiod", "249Btwentyperiod", "249Caparen", "249Dbparen", "249Ecparen", "249Fdparen",
				"24A0eparen", "24A1fparen", "24A2gparen", "24A3hparen", "24A4iparen", "24A5jparen", "24A6kparen", "24A7lparen", "24A8mparen", "24A9nparen",
				"24AAoparen", "24ABpparen", "24ACqparen", "24ADrparen", "24AEsparen", "24AFtparen", "24B0uparen", "24B1vparen", "24B2wparen", "24B3xparen",
				"24B4yparen", "24B5zparen", "24B6Acircle", "24B7Bcircle", "24B8Ccircle", "24B9Dcircle", "24BAEcircle", "24BBFcircle", "24BCGcircle", "24BDHcircle",
				"24BEIcircle", "24BFJcircle", "24C0Kcircle", "24C1Lcircle", "24C2Mcircle", "24C3Ncircle", "24C4Ocircle", "24C5Pcircle", "24C6Qcircle", "24C7Rcircle",
				"24C8Scircle", "24C9Tcircle", "24CAUcircle", "24CBVcircle", "24CCWcircle", "24CDXcircle", "24CEYcircle", "24CFZcircle", "24D0acircle", "24D1bcircle",
				"24D2ccircle", "24D3dcircle", "24D4ecircle", "24D5fcircle", "24D6gcircle", "24D7hcircle", "24D8icircle", "24D9jcircle", "24DAkcircle", "24DBlcircle",
				"24DCmcircle", "24DDncircle", "24DEocircle", "24DFpcircle", "24E0qcircle", "24E1rcircle", "24E2scircle", "24E3tcircle", "24E4ucircle", "24E5vcircle",
				"24E6wcircle", "24E7xcircle", "24E8ycircle", "24E9zcircle", "2500SF100000", "2502SF110000", "250CSF010000", "2510SF030000", "2514SF020000", "2518SF040000",
				"251CSF080000", "2524SF090000", "252CSF060000", "2534SF070000", "253CSF050000", "2550SF430000", "2551SF240000", "2552SF510000", "2553SF520000", "2554SF390000",
				"2555SF220000", "2556SF210000", "2557SF250000", "2558SF500000", "2559SF490000", "255ASF380000", "255BSF280000", "255CSF270000", "255DSF260000", "255ESF360000",
				"255FSF370000", "2560SF420000", "2561SF190000", "2562SF200000", "2563SF230000", "2564SF470000", "2565SF480000", "2566SF410000", "2567SF450000", "2568SF460000",
				"2569SF400000", "256ASF540000", "256BSF530000", "256CSF440000", "2580upblock", "2584dnblock", "2588block", "258Clfblock", "2590rtblock", "2591ltshade",
				"2592shade", "2593dkshade", "25A0filledbox", "25A1H22073", "25A3squarewhitewithsmallblack", "25A4squarehorizontalfill", "25A5squareverticalfill", "25A6squareorthogonalcrosshatchfill", "25A7squareupperlefttolowerrightfill", "25A8squareupperrighttolowerleftfill",
				"25A9squarediagonalcrosshatchfill", "25AAH18543", "25ABH18551", "25ACfilledrect", "25B2triagup", "25B3whiteuppointingtriangle", "25B4blackuppointingsmalltriangle", "25B5whiteuppointingsmalltriangle", "25B6blackrightpointingtriangle", "25B7whiterightpointingtriangle",
				"25B9whiterightpointingsmalltriangle", "25BAtriagrt", "25BCtriagdn", "25BDwhitedownpointingtriangle", "25BFwhitedownpointingsmalltriangle", "25C0blackleftpointingtriangle", "25C1whiteleftpointingtriangle", "25C3whiteleftpointingsmalltriangle", "25C4triaglf", "25C6blackdiamond",
				"25C7whitediamond", "25C8whitediamondcontainingblacksmalldiamond", "25C9fisheye", "25CAlozenge", "25CBcircle", "25CCdottedcircle", "25CEbullseye", "25CFH18533", "25D0circlewithlefthalfblack", "25D1circlewithrighthalfblack",
				"25D8invbullet", "25D9invcircle", "25E2blacklowerrighttriangle", "25E3blacklowerlefttriangle", "25E4blackupperlefttriangle", "25E5blackupperrighttriangle", "25E6openbullet", "25EFlargecircle", "2605blackstar", "2606whitestar",
				"260Etelephoneblack", "260Fwhitetelephone", "261Cpointingindexleftwhite", "261Dpointingindexupwhite", "261Epointingindexrightwhite", "261Fpointingindexdownwhite", "262Fyinyang", "263Asmileface", "263Binvsmileface", "263Csun",
				"2640female", "2641earth", "2642male", "2660spade", "2661heartsuitwhite", "2662diamondsuitwhite", "2663club", "2664spadesuitwhite", "2665heart", "2666diamond",
				"2667clubsuitwhite", "2668hotsprings", "2669quarternote", "266Amusicalnote", "266Bmusicalnotedbl", "266Cbeamedsixteenthnotes", "266Dmusicflatsign", "266Fmusicsharpsign", "2713checkmark", "278Aonecircleinversesansserif",
				"278Btwocircleinversesansserif", "278Cthreecircleinversesansserif", "278Dfourcircleinversesansserif", "278Efivecircleinversesansserif", "278Fsixcircleinversesansserif", "2790sevencircleinversesansserif", "2791eightcircleinversesansserif", "2792ninecircleinversesansserif", "279Earrowrightheavy", "3000ideographicspace",
				"3001ideographiccomma", "3002ideographicperiod", "3003dittomark", "3004jis", "3005ideographiciterationmark", "3006ideographicclose", "3007ideographiczero", "3008anglebracketleft", "3009anglebracketright", "300Adblanglebracketleft",
				"300Bdblanglebracketright", "300Ccornerbracketleft", "300Dcornerbracketright", "300Ewhitecornerbracketleft", "300Fwhitecornerbracketright", "3010blacklenticularbracketleft", "3011blacklenticularbracketright", "3012postalmark", "3013getamark", "3014tortoiseshellbracketleft",
				"3015tortoiseshellbracketright", "3016whitelenticularbracketleft", "3017whitelenticularbracketright", "3018whitetortoiseshellbracketleft", "3019whitetortoiseshellbracketright", "301Cwavedash", "301Dquotedblprimereversed", "301Equotedblprime", "3020postalmarkface", "3021onehangzhou",
				"3022twohangzhou", "3023threehangzhou", "3024fourhangzhou", "3025fivehangzhou", "3026sixhangzhou", "3027sevenhangzhou", "3028eighthangzhou", "3029ninehangzhou", "3036circlepostalmark", "3041asmallhiragana",
				"3042ahiragana", "3043ismallhiragana", "3044ihiragana", "3045usmallhiragana", "3046uhiragana", "3047esmallhiragana", "3048ehiragana", "3049osmallhiragana", "304Aohiragana", "304Bkahiragana",
				"304Cgahiragana", "304Dkihiragana", "304Egihiragana", "304Fkuhiragana", "3050guhiragana", "3051kehiragana", "3052gehiragana", "3053kohiragana", "3054gohiragana", "3055sahiragana",
				"3056zahiragana", "3057sihiragana", "3058zihiragana", "3059suhiragana", "305Azuhiragana", "305Bsehiragana", "305Czehiragana", "305Dsohiragana", "305Ezohiragana", "305Ftahiragana",
				"3060dahiragana", "3061tihiragana", "3062dihiragana", "3063tusmallhiragana", "3064tuhiragana", "3065duhiragana", "3066tehiragana", "3067dehiragana", "3068tohiragana", "3069dohiragana",
				"306Anahiragana", "306Bnihiragana", "306Cnuhiragana", "306Dnehiragana", "306Enohiragana", "306Fhahiragana", "3070bahiragana", "3071pahiragana", "3072hihiragana", "3073bihiragana",
				"3074pihiragana", "3075huhiragana", "3076buhiragana", "3077puhiragana", "3078hehiragana", "3079behiragana", "307Apehiragana", "307Bhohiragana", "307Cbohiragana", "307Dpohiragana",
				"307Emahiragana", "307Fmihiragana", "3080muhiragana", "3081mehiragana", "3082mohiragana", "3083yasmallhiragana", "3084yahiragana", "3085yusmallhiragana", "3086yuhiragana", "3087yosmallhiragana",
				"3088yohiragana", "3089rahiragana", "308Arihiragana", "308Bruhiragana", "308Crehiragana", "308Drohiragana", "308Ewasmallhiragana", "308Fwahiragana", "3090wihiragana", "3091wehiragana",
				"3092wohiragana", "3093nhiragana", "3094vuhiragana", "309Bvoicedmarkkana", "309Csemivoicedmarkkana", "309Diterationhiragana", "309Evoicediterationhiragana", "30A1asmallkatakana", "30A2akatakana", "30A3ismallkatakana",
				"30A4ikatakana", "30A5usmallkatakana", "30A6ukatakana", "30A7esmallkatakana", "30A8ekatakana", "30A9osmallkatakana", "30AAokatakana", "30ABkakatakana", "30ACgakatakana", "30ADkikatakana",
				"30AEgikatakana", "30AFkukatakana", "30B0gukatakana", "30B1kekatakana", "30B2gekatakana", "30B3kokatakana", "30B4gokatakana", "30B5sakatakana", "30B6zakatakana", "30B7sikatakana",
				"30B8zikatakana", "30B9sukatakana", "30BAzukatakana", "30BBsekatakana", "30BCzekatakana", "30BDsokatakana", "30BEzokatakana", "30BFtakatakana", "30C0dakatakana", "30C1tikatakana",
				"30C2dikatakana", "30C3tusmallkatakana", "30C4tukatakana", "30C5dukatakana", "30C6tekatakana", "30C7dekatakana", "30C8tokatakana", "30C9dokatakana", "30CAnakatakana", "30CBnikatakana",
				"30CCnukatakana", "30CDnekatakana", "30CEnokatakana", "30CFhakatakana", "30D0bakatakana", "30D1pakatakana", "30D2hikatakana", "30D3bikatakana", "30D4pikatakana", "30D5hukatakana",
				"30D6bukatakana", "30D7pukatakana", "30D8hekatakana", "30D9bekatakana", "30DApekatakana", "30DBhokatakana", "30DCbokatakana", "30DDpokatakana", "30DEmakatakana", "30DFmikatakana",
				"30E0mukatakana", "30E1mekatakana", "30E2mokatakana", "30E3yasmallkatakana", "30E4yakatakana", "30E5yusmallkatakana", "30E6yukatakana", "30E7yosmallkatakana", "30E8yokatakana", "30E9rakatakana",
				"30EArikatakana", "30EBrukatakana", "30ECrekatakana", "30EDrokatakana", "30EEwasmallkatakana", "30EFwakatakana", "30F0wikatakana", "30F1wekatakana", "30F2wokatakana", "30F3nkatakana",
				"30F4vukatakana", "30F5kasmallkatakana", "30F6kesmallkatakana", "30F7vakatakana", "30F8vikatakana", "30F9vekatakana", "30FAvokatakana", "30FBdotkatakana", "30FCprolongedkana", "30FDiterationkatakana",
				"30FEvoicediterationkatakana", "3105bbopomofo", "3106pbopomofo", "3107mbopomofo", "3108fbopomofo", "3109dbopomofo", "310Atbopomofo", "310Bnbopomofo", "310Clbopomofo", "310Dgbopomofo",
				"310Ekbopomofo", "310Fhbopomofo", "3110jbopomofo", "3111qbopomofo", "3112xbopomofo", "3113zhbopomofo", "3114chbopomofo", "3115shbopomofo", "3116rbopomofo", "3117zbopomofo",
				"3118cbopomofo", "3119sbopomofo", "311Aabopomofo", "311Bobopomofo", "311Cebopomofo", "311Dehbopomofo", "311Eaibopomofo", "311Feibopomofo", "3120aubopomofo", "3121oubopomofo",
				"3122anbopomofo", "3123enbopomofo", "3124angbopomofo", "3125engbopomofo", "3126erbopomofo", "3127ibopomofo", "3128ubopomofo", "3129iubopomofo", "3131kiyeokkorean", "3132ssangkiyeokkorean",
				"3133kiyeoksioskorean", "3134nieunkorean", "3135nieuncieuckorean", "3136nieunhieuhkorean", "3137tikeutkorean", "3138ssangtikeutkorean", "3139rieulkorean", "313Arieulkiyeokkorean", "313Brieulmieumkorean", "313Crieulpieupkorean",
				"313Drieulsioskorean", "313Erieulthieuthkorean", "313Frieulphieuphkorean", "3140rieulhieuhkorean", "3141mieumkorean", "3142pieupkorean", "3143ssangpieupkorean", "3144pieupsioskorean", "3145sioskorean", "3146ssangsioskorean",
				"3147ieungkorean", "3148cieuckorean", "3149ssangcieuckorean", "314Achieuchkorean", "314Bkhieukhkorean", "314Cthieuthkorean", "314Dphieuphkorean", "314Ehieuhkorean", "314Fakorean", "3150aekorean",
				"3151yakorean", "3152yaekorean", "3153eokorean", "3154ekorean", "3155yeokorean", "3156yekorean", "3157okorean", "3158wakorean", "3159waekorean", "315Aoekorean",
				"315Byokorean", "315Cukorean", "315Dweokorean", "315Ewekorean", "315Fwikorean", "3160yukorean", "3161eukorean", "3162yikorean", "3163ikorean", "3164hangulfiller",
				"3165ssangnieunkorean", "3166nieuntikeutkorean", "3167nieunsioskorean", "3168nieunpansioskorean", "3169rieulkiyeoksioskorean", "316Arieultikeutkorean", "316Brieulpieupsioskorean", "316Crieulpansioskorean", "316Drieulyeorinhieuhkorean", "316Emieumpieupkorean",
				"316Fmieumsioskorean", "3170mieumpansioskorean", "3171kapyeounmieumkorean", "3172pieupkiyeokkorean", "3173pieuptikeutkorean", "3174pieupsioskiyeokkorean", "3175pieupsiostikeutkorean", "3176pieupcieuckorean", "3177pieupthieuthkorean", "3178kapyeounpieupkorean",
				"3179kapyeounssangpieupkorean", "317Asioskiyeokkorean", "317Bsiosnieunkorean", "317Csiostikeutkorean", "317Dsiospieupkorean", "317Esioscieuckorean", "317Fpansioskorean", "3180ssangieungkorean", "3181yesieungkorean", "3182yesieungsioskorean",
				"3183yesieungpansioskorean", "3184kapyeounphieuphkorean", "3185ssanghieuhkorean", "3186yeorinhieuhkorean", "3187yoyakorean", "3188yoyaekorean", "3189yoikorean", "318Ayuyeokorean", "318Byuyekorean", "318Cyuikorean",
				"318Daraeakorean", "318Earaeaekorean", "3200kiyeokparenkorean", "3201nieunparenkorean", "3202tikeutparenkorean", "3203rieulparenkorean", "3204mieumparenkorean", "3205pieupparenkorean", "3206siosparenkorean", "3207ieungparenkorean",
				"3208cieucparenkorean", "3209chieuchparenkorean", "320Akhieukhparenkorean", "320Bthieuthparenkorean", "320Cphieuphparenkorean", "320Dhieuhparenkorean", "320Ekiyeokaparenkorean", "320Fnieunaparenkorean", "3210tikeutaparenkorean", "3211rieulaparenkorean",
				"3212mieumaparenkorean", "3213pieupaparenkorean", "3214siosaparenkorean", "3215ieungaparenkorean", "3216cieucaparenkorean", "3217chieuchaparenkorean", "3218khieukhaparenkorean", "3219thieuthaparenkorean", "321Aphieuphaparenkorean", "321Bhieuhaparenkorean",
				"321Ccieucuparenkorean", "3220oneideographicparen", "3221twoideographicparen", "3222threeideographicparen", "3223fourideographicparen", "3224fiveideographicparen", "3225sixideographicparen", "3226sevenideographicparen", "3227eightideographicparen", "3228nineideographicparen",
				"3229tenideographicparen", "322Aideographicmoonparen", "322Bideographicfireparen", "322Cideographicwaterparen", "322Dideographicwoodparen", "322Eideographicmetalparen", "322Fideographicearthparen", "3230ideographicsunparen", "3231ideographicstockparen", "3232ideographichaveparen",
				"3233ideographicsocietyparen", "3234ideographicnameparen", "3235ideographicspecialparen", "3236ideographicfinancialparen", "3237ideographiccongratulationparen", "3238ideographiclaborparen", "3239ideographicrepresentparen", "323Aideographiccallparen", "323Bideographicstudyparen", "323Cideographicsuperviseparen",
				"323Dideographicenterpriseparen", "323Eideographicresourceparen", "323Fideographicallianceparen", "3240ideographicfestivalparen", "3242ideographicselfparen", "3243ideographicreachparen", "3260kiyeokcirclekorean", "3261nieuncirclekorean", "3262tikeutcirclekorean", "3263rieulcirclekorean",
				"3264mieumcirclekorean", "3265pieupcirclekorean", "3266sioscirclekorean", "3267ieungcirclekorean", "3268cieuccirclekorean", "3269chieuchcirclekorean", "326Akhieukhcirclekorean", "326Bthieuthcirclekorean", "326Cphieuphcirclekorean", "326Dhieuhcirclekorean",
				"326Ekiyeokacirclekorean", "326Fnieunacirclekorean", "3270tikeutacirclekorean", "3271rieulacirclekorean", "3272mieumacirclekorean", "3273pieupacirclekorean", "3274siosacirclekorean", "3275ieungacirclekorean", "3276cieucacirclekorean", "3277chieuchacirclekorean",
				"3278khieukhacirclekorean", "3279thieuthacirclekorean", "327Aphieuphacirclekorean", "327Bhieuhacirclekorean", "327Fkoreanstandardsymbol", "328Aideographmooncircle", "328Bideographfirecircle", "328Cideographwatercircle", "328Dideographwoodcircle", "328Eideographmetalcircle",
				"328Fideographearthcircle", "3290ideographsuncircle", "3294ideographnamecircle", "3296ideographicfinancialcircle", "3298ideographiclaborcircle", "3299ideographicsecretcircle", "329Dideographicexcellentcircle", "329Eideographicprintcircle", "32A3ideographiccorrectcircle", "32A4ideographichighcircle",
				"32A5ideographiccentrecircle", "32A6ideographiclowcircle", "32A7ideographicleftcircle", "32A8ideographicrightcircle", "32A9ideographicmedicinecircle", "3300apaatosquare", "3303aarusquare", "3305intisquare", "330Dkaroriisquare", "3314kirosquare",
				"3315kiroguramusquare", "3316kiromeetorusquare", "3318guramusquare", "331Ekooposquare", "3322sentisquare", "3323sentosquare", "3326dorusquare", "3327tonsquare", "332Ahaitusquare", "332Bpaasentosquare",
				"3331birusquare", "3333huiitosquare", "3336hekutaarusquare", "3339herutusquare", "333Bpeezisquare", "3342hoonsquare", "3347mansyonsquare", "3349mirisquare", "334Amiribaarusquare", "334Dmeetorusquare",
				"334Eyaadosquare", "3351rittorusquare", "3357wattosquare", "337Bheiseierasquare", "337Csyouwaerasquare", "337Dtaisyouerasquare", "337Emeizierasquare", "337Fcorporationsquare", "3380paampssquare", "3381nasquare",
				"3382muasquare", "3383masquare", "3384kasquare", "3385KBsquare", "3386MBsquare", "3387GBsquare", "3388calsquare", "3389kcalsquare", "338Apfsquare", "338Bnfsquare",
				"338Cmufsquare", "338Dmugsquare", "338Esquaremg", "338Fsquarekg", "3390Hzsquare", "3391khzsquare", "3392mhzsquare", "3393ghzsquare", "3394thzsquare", "3395mulsquare",
				"3396mlsquare", "3397dlsquare", "3398klsquare", "3399fmsquare", "339Anmsquare", "339Bmumsquare", "339Csquaremm", "339Dsquarecm", "339Esquarekm", "339Fmmsquaredsquare",
				"33A0cmsquaredsquare", "33A1squaremsquared", "33A2kmsquaredsquare", "33A3mmcubedsquare", "33A4cmcubedsquare", "33A5mcubedsquare", "33A6kmcubedsquare", "33A7moverssquare", "33A8moverssquaredsquare", "33A9pasquare",
				"33AAkpasquare", "33ABmpasquare", "33ACgpasquare", "33ADradsquare", "33AEradoverssquare", "33AFradoverssquaredsquare", "33B0pssquare", "33B1nssquare", "33B2mussquare", "33B3mssquare",
				"33B4pvsquare", "33B5nvsquare", "33B6muvsquare", "33B7mvsquare", "33B8kvsquare", "33B9mvmegasquare", "33BApwsquare", "33BBnwsquare", "33BCmuwsquare", "33BDmwsquare",
				"33BEkwsquare", "33BFmwmegasquare", "33C0kohmsquare", "33C1mohmsquare", "33C2amsquare", "33C3bqsquare", "33C4squarecc", "33C5cdsquare", "33C6coverkgsquare", "33C7cosquare",
				"33C8dbsquare", "33C9gysquare", "33CAhasquare", "33CBHPsquare", "33CDKKsquare", "33CEsquarekmcapital", "33CFktsquare", "33D0lmsquare", "33D1squareln", "33D2squarelog",
				"33D3lxsquare", "33D4mbsquare", "33D5squaremil", "33D6molsquare", "33D8pmsquare", "33DBsrsquare", "33DCsvsquare", "33DDwbsquare", "5344twentyhangzhou", "F6BEdotlessj",
				"F6BFLL", "F6C0ll", "F6C3commaaccent", "F6C4afii10063", "F6C5afii10064", "F6C6afii10192", "F6C7afii10831", "F6C8afii10832", "F6C9Acute", "F6CACaron",
				"F6CBDieresis", "F6CCDieresisAcute", "F6CDDieresisGrave", "F6CEGrave", "F6CFHungarumlaut", "F6D0Macron", "F6D1cyrBreve", "F6D2cyrFlex", "F6D3dblGrave", "F6D4cyrbreve",
				"F6D5cyrflex", "F6D6dblgrave", "F6D7dieresisacute", "F6D8dieresisgrave", "F6D9copyrightserif", "F6DAregisterserif", "F6DBtrademarkserif", "F6DConefitted", "F6DDrupiah", "F6DEthreequartersemdash",
				"F6DFcentinferior", "F6E0centsuperior", "F6E1commainferior", "F6E2commasuperior", "F6E3dollarinferior", "F6E4dollarsuperior", "F6E5hypheninferior", "F6E6hyphensuperior", "F6E7periodinferior", "F6E8periodsuperior",
				"F6E9asuperior", "F6EAbsuperior", "F6EBdsuperior", "F6ECesuperior", "F6EDisuperior", "F6EElsuperior", "F6EFmsuperior", "F6F0osuperior", "F6F1rsuperior", "F6F2ssuperior",
				"F6F3tsuperior", "F6F4Brevesmall", "F6F5Caronsmall", "F6F6Circumflexsmall", "F6F7Dotaccentsmall", "F6F8Hungarumlautsmall", "F6F9Lslashsmall", "F6FAOEsmall", "F6FBOgoneksmall", "F6FCRingsmall",
				"F6FDScaronsmall", "F6FETildesmall", "F6FFZcaronsmall", "F721exclamsmall", "F724dollaroldstyle", "F726ampersandsmall", "F730zerooldstyle", "F731oneoldstyle", "F732twooldstyle", "F733threeoldstyle",
				"F734fouroldstyle", "F735fiveoldstyle", "F736sixoldstyle", "F737sevenoldstyle", "F738eightoldstyle", "F739nineoldstyle", "F73Fquestionsmall", "F760Gravesmall", "F761Asmall", "F762Bsmall",
				"F763Csmall", "F764Dsmall", "F765Esmall", "F766Fsmall", "F767Gsmall", "F768Hsmall", "F769Ismall", "F76AJsmall", "F76BKsmall", "F76CLsmall",
				"F76DMsmall", "F76ENsmall", "F76FOsmall", "F770Psmall", "F771Qsmall", "F772Rsmall", "F773Ssmall", "F774Tsmall", "F775Usmall", "F776Vsmall",
				"F777Wsmall", "F778Xsmall", "F779Ysmall", "F77AZsmall", "F7A1exclamdownsmall", "F7A2centoldstyle", "F7A8Dieresissmall", "F7AFMacronsmall", "F7B4Acutesmall", "F7B8Cedillasmall",
				"F7BFquestiondownsmall", "F7E0Agravesmall", "F7E1Aacutesmall", "F7E2Acircumflexsmall", "F7E3Atildesmall", "F7E4Adieresissmall", "F7E5Aringsmall", "F7E6AEsmall", "F7E7Ccedillasmall", "F7E8Egravesmall",
				"F7E9Eacutesmall", "F7EAEcircumflexsmall", "F7EBEdieresissmall", "F7ECIgravesmall", "F7EDIacutesmall", "F7EEIcircumflexsmall", "F7EFIdieresissmall", "F7F0Ethsmall", "F7F1Ntildesmall", "F7F2Ogravesmall",
				"F7F3Oacutesmall", "F7F4Ocircumflexsmall", "F7F5Otildesmall", "F7F6Odieresissmall", "F7F8Oslashsmall", "F7F9Ugravesmall", "F7FAUacutesmall", "F7FBUcircumflexsmall", "F7FCUdieresissmall", "F7FDYacutesmall",
				"F7FEThornsmall", "F7FFYdieresissmall", "F884maihanakatleftthai", "F885saraileftthai", "F886saraiileftthai", "F887saraueleftthai", "F888saraueeleftthai", "F889maitaikhuleftthai", "F88Amaiekupperleftthai", "F88Bmaieklowrightthai",
				"F88Cmaieklowleftthai", "F88Dmaithoupperleftthai", "F88Emaitholowrightthai", "F88Fmaitholowleftthai", "F890maitriupperleftthai", "F891maitrilowrightthai", "F892maitrilowleftthai", "F893maichattawaupperleftthai", "F894maichattawalowrightthai", "F895maichattawalowleftthai",
				"F896thanthakhatupperleftthai", "F897thanthakhatlowrightthai", "F898thanthakhatlowleftthai", "F899nikhahitleftthai", "F8E5radicalex", "F8E6arrowvertex", "F8E7arrowhorizex", "F8E8registersans", "F8E9copyrightsans", "F8EAtrademarksans",
				"F8EBparenlefttp", "F8ECparenleftex", "F8EDparenleftbt", "F8EEbracketlefttp", "F8EFbracketleftex", "F8F0bracketleftbt", "F8F1bracelefttp", "F8F2braceleftmid", "F8F3braceleftbt", "F8F4braceex",
				"F8F5integralex", "F8F6parenrighttp", "F8F7parenrightex", "F8F8parenrightbt", "F8F9bracketrighttp", "F8FAbracketrightex", "F8FBbracketrightbt", "F8FCbracerighttp", "F8FDbracerightmid", "F8FEbracerightbt",
				"F8FFapple", "FB00ff", "FB01fi", "FB02fl", "FB03ffi", "FB04ffl", "FB1Fafii57705", "FB20ayinaltonehebrew", "FB2Aafii57694", "FB2Bafii57695",
				"FB2Cshindageshshindot", "FB2Dshindageshsindot", "FB2Ealefpatahhebrew", "FB2Falefqamatshebrew", "FB30alefdageshhebrew", "FB31betdagesh", "FB32gimeldagesh", "FB33daletdagesh", "FB34hedagesh", "FB35afii57723",
				"FB36zayindagesh", "FB38tetdagesh", "FB39yoddagesh", "FB3Afinalkafdagesh", "FB3Bkafdagesh", "FB3Clameddagesh", "FB3Ememdagesh", "FB40nundagesh", "FB41samekhdagesh", "FB43pefinaldageshhebrew",
				"FB44pedagesh", "FB46tsadidagesh", "FB47qofdagesh", "FB48reshdageshhebrew", "FB49shindagesh", "FB4Atavdagesh", "FB4Bafii57700", "FB4Cbetrafehebrew", "FB4Dkafrafehebrew", "FB4Eperafehebrew",
				"FB4Faleflamedhebrew", "FB57pehfinalarabic", "FB58pehinitialarabic", "FB59pehmedialarabic", "FB67ttehfinalarabic", "FB68ttehinitialarabic", "FB69ttehmedialarabic", "FB6Bvehfinalarabic", "FB6Cvehinitialarabic", "FB6Dvehmedialarabic",
				"FB7Btchehfinalarabic", "FB7Ctchehinitialarabic", "FB7Dtchehmedialarabic", "FB89ddalfinalarabic", "FB8Bjehfinalarabic", "FB8Drrehfinalarabic", "FB93gaffinalarabic", "FB94gafinitialarabic", "FB95gafmedialarabic", "FB9Fnoonghunnafinalarabic",
				"FBA4hehhamzaaboveisolatedarabic", "FBA5hehhamzaabovefinalarabic", "FBA7hehfinalaltonearabic", "FBA8hehinitialaltonearabic", "FBA9hehmedialaltonearabic", "FBAFyehbarreefinalarabic", "FC08behmeemisolatedarabic", "FC0Btehjeemisolatedarabic", "FC0Ctehhahisolatedarabic", "FC0Etehmeemisolatedarabic",
				"FC48meemmeemisolatedarabic", "FC4Bnoonjeemisolatedarabic", "FC4Enoonmeemisolatedarabic", "FC58yehmeemisolatedarabic", "FC5Eshaddadammatanarabic", "FC5Fshaddakasratanarabic", "FC60shaddafathaarabic", "FC61shaddadammaarabic", "FC62shaddakasraarabic", "FC6Dbehnoonfinalarabic",
				"FC73tehnoonfinalarabic", "FC8Dnoonnoonfinalarabic", "FC94yehnoonfinalarabic", "FC9Fbehmeeminitialarabic", "FCA1tehjeeminitialarabic", "FCA2tehhahinitialarabic", "FCA4tehmeeminitialarabic", "FCC9lamjeeminitialarabic", "FCCAlamhahinitialarabic", "FCCBlamkhahinitialarabic",
				"FCCClammeeminitialarabic", "FCD1meemmeeminitialarabic", "FCD2noonjeeminitialarabic", "FCD5noonmeeminitialarabic", "FCDDyehmeeminitialarabic", "FD3Eparenleftaltonearabic", "FD3Fparenrightaltonearabic", "FD88lammeemhahinitialarabic", "FDF2lamlamhehisolatedarabic", "FDFAsallallahoualayhewasallamarabic",
				"FE30twodotleadervertical", "FE31emdashvertical", "FE32endashvertical", "FE33underscorevertical", "FE34wavyunderscorevertical", "FE35parenleftvertical", "FE36parenrightvertical", "FE37braceleftvertical", "FE38bracerightvertical", "FE39tortoiseshellbracketleftvertical",
				"FE3Atortoiseshellbracketrightvertical", "FE3Bblacklenticularbracketleftvertical", "FE3Cblacklenticularbracketrightvertical", "FE3Ddblanglebracketleftvertical", "FE3Edblanglebracketrightvertical", "FE3Fanglebracketleftvertical", "FE40anglebracketrightvertical", "FE41cornerbracketleftvertical", "FE42cornerbracketrightvertical", "FE43whitecornerbracketleftvertical",
				"FE44whitecornerbracketrightvertical", "FE49overlinedashed", "FE4Aoverlinecenterline", "FE4Boverlinewavy", "FE4Coverlinedblwavy", "FE4Dlowlinedashed", "FE4Elowlinecenterline", "FE4Funderscorewavy", "FE50commasmall", "FE52periodsmall",
				"FE54semicolonsmall", "FE55colonsmall", "FE59parenleftsmall", "FE5Aparenrightsmall", "FE5Bbraceleftsmall", "FE5Cbracerightsmall", "FE5Dtortoiseshellbracketleftsmall", "FE5Etortoiseshellbracketrightsmall", "FE5Fnumbersignsmall", "FE61asterisksmall",
				"FE62plussmall", "FE63hyphensmall", "FE64lesssmall", "FE65greatersmall", "FE66equalsmall", "FE69dollarsmall", "FE6Apercentsmall", "FE6Batsmall", "FE82alefmaddaabovefinalarabic", "FE84alefhamzaabovefinalarabic",
				"FE86wawhamzaabovefinalarabic", "FE88alefhamzabelowfinalarabic", "FE8Ayehhamzaabovefinalarabic", "FE8Byehhamzaaboveinitialarabic", "FE8Cyehhamzaabovemedialarabic", "FE8Ealeffinalarabic", "FE90behfinalarabic", "FE91behinitialarabic", "FE92behmedialarabic", "FE94tehmarbutafinalarabic",
				"FE96tehfinalarabic", "FE97tehinitialarabic", "FE98tehmedialarabic", "FE9Athehfinalarabic", "FE9Bthehinitialarabic", "FE9Cthehmedialarabic", "FE9Ejeemfinalarabic", "FE9Fjeeminitialarabic", "FEA0jeemmedialarabic", "FEA2hahfinalarabic",
				"FEA3hahinitialarabic", "FEA4hahmedialarabic", "FEA6khahfinalarabic", "FEA7khahinitialarabic", "FEA8khahmedialarabic", "FEAAdalfinalarabic", "FEACthalfinalarabic", "FEAErehfinalarabic", "FEB0zainfinalarabic", "FEB2seenfinalarabic",
				"FEB3seeninitialarabic", "FEB4seenmedialarabic", "FEB6sheenfinalarabic", "FEB7sheeninitialarabic", "FEB8sheenmedialarabic", "FEBAsadfinalarabic", "FEBBsadinitialarabic", "FEBCsadmedialarabic", "FEBEdadfinalarabic", "FEBFdadinitialarabic",
				"FEC0dadmedialarabic", "FEC2tahfinalarabic", "FEC3tahinitialarabic", "FEC4tahmedialarabic", "FEC6zahfinalarabic", "FEC7zahinitialarabic", "FEC8zahmedialarabic", "FECAainfinalarabic", "FECBaininitialarabic", "FECCainmedialarabic",
				"FECEghainfinalarabic", "FECFghaininitialarabic", "FED0ghainmedialarabic", "FED2fehfinalarabic", "FED3fehinitialarabic", "FED4fehmedialarabic", "FED6qaffinalarabic", "FED7qafinitialarabic", "FED8qafmedialarabic", "FEDAkaffinalarabic",
				"FEDBkafinitialarabic", "FEDCkafmedialarabic", "FEDElamfinalarabic", "FEDFlaminitialarabic", "FEE0lammedialarabic", "FEE2meemfinalarabic", "FEE3meeminitialarabic", "FEE4meemmedialarabic", "FEE6noonfinalarabic", "FEE7nooninitialarabic",
				"FEE8noonmedialarabic", "FEEAhehfinalarabic", "FEEBhehinitialarabic", "FEEChehmedialarabic", "FEEEwawfinalarabic", "FEF0alefmaksurafinalarabic", "FEF2yehfinalarabic", "FEF3yehinitialarabic", "FEF4yehmedialarabic", "FEF5lamalefmaddaaboveisolatedarabic",
				"FEF6lamalefmaddaabovefinalarabic", "FEF7lamalefhamzaaboveisolatedarabic", "FEF8lamalefhamzaabovefinalarabic", "FEF9lamalefhamzabelowisolatedarabic", "FEFAlamalefhamzabelowfinalarabic", "FEFBlamalefisolatedarabic", "FEFClamaleffinalarabic", "FEFFzerowidthjoiner", "FF01exclammonospace", "FF02quotedblmonospace",
				"FF03numbersignmonospace", "FF04dollarmonospace", "FF05percentmonospace", "FF06ampersandmonospace", "FF07quotesinglemonospace", "FF08parenleftmonospace", "FF09parenrightmonospace", "FF0Aasteriskmonospace", "FF0Bplusmonospace", "FF0Ccommamonospace",
				"FF0Dhyphenmonospace", "FF0Eperiodmonospace", "FF0Fslashmonospace", "FF10zeromonospace", "FF11onemonospace", "FF12twomonospace", "FF13threemonospace", "FF14fourmonospace", "FF15fivemonospace", "FF16sixmonospace",
				"FF17sevenmonospace", "FF18eightmonospace", "FF19ninemonospace", "FF1Acolonmonospace", "FF1Bsemicolonmonospace", "FF1Clessmonospace", "FF1Dequalmonospace", "FF1Egreatermonospace", "FF1Fquestionmonospace", "FF20atmonospace",
				"FF21Amonospace", "FF22Bmonospace", "FF23Cmonospace", "FF24Dmonospace", "FF25Emonospace", "FF26Fmonospace", "FF27Gmonospace", "FF28Hmonospace", "FF29Imonospace", "FF2AJmonospace",
				"FF2BKmonospace", "FF2CLmonospace", "FF2DMmonospace", "FF2ENmonospace", "FF2FOmonospace", "FF30Pmonospace", "FF31Qmonospace", "FF32Rmonospace", "FF33Smonospace", "FF34Tmonospace",
				"FF35Umonospace", "FF36Vmonospace", "FF37Wmonospace", "FF38Xmonospace", "FF39Ymonospace", "FF3AZmonospace", "FF3Bbracketleftmonospace", "FF3Cbackslashmonospace", "FF3Dbracketrightmonospace", "FF3Easciicircummonospace",
				"FF3Funderscoremonospace", "FF40gravemonospace", "FF41amonospace", "FF42bmonospace", "FF43cmonospace", "FF44dmonospace", "FF45emonospace", "FF46fmonospace", "FF47gmonospace", "FF48hmonospace",
				"FF49imonospace", "FF4Ajmonospace", "FF4Bkmonospace", "FF4Clmonospace", "FF4Dmmonospace", "FF4Enmonospace", "FF4Fomonospace", "FF50pmonospace", "FF51qmonospace", "FF52rmonospace",
				"FF53smonospace", "FF54tmonospace", "FF55umonospace", "FF56vmonospace", "FF57wmonospace", "FF58xmonospace", "FF59ymonospace", "FF5Azmonospace", "FF5Bbraceleftmonospace", "FF5Cbarmonospace",
				"FF5Dbracerightmonospace", "FF5Easciitildemonospace", "FF61periodhalfwidth", "FF62cornerbracketlefthalfwidth", "FF63cornerbracketrighthalfwidth", "FF64ideographiccommaleft", "FF65middledotkatakanahalfwidth", "FF66wokatakanahalfwidth", "FF67asmallkatakanahalfwidth", "FF68ismallkatakanahalfwidth",
				"FF69usmallkatakanahalfwidth", "FF6Aesmallkatakanahalfwidth", "FF6Bosmallkatakanahalfwidth", "FF6Cyasmallkatakanahalfwidth", "FF6Dyusmallkatakanahalfwidth", "FF6Eyosmallkatakanahalfwidth", "FF6Ftusmallkatakanahalfwidth", "FF70katahiraprolongmarkhalfwidth", "FF71akatakanahalfwidth", "FF72ikatakanahalfwidth",
				"FF73ukatakanahalfwidth", "FF74ekatakanahalfwidth", "FF75okatakanahalfwidth", "FF76kakatakanahalfwidth", "FF77kikatakanahalfwidth", "FF78kukatakanahalfwidth", "FF79kekatakanahalfwidth", "FF7Akokatakanahalfwidth", "FF7Bsakatakanahalfwidth", "FF7Csikatakanahalfwidth",
				"FF7Dsukatakanahalfwidth", "FF7Esekatakanahalfwidth", "FF7Fsokatakanahalfwidth", "FF80takatakanahalfwidth", "FF81tikatakanahalfwidth", "FF82tukatakanahalfwidth", "FF83tekatakanahalfwidth", "FF84tokatakanahalfwidth", "FF85nakatakanahalfwidth", "FF86nikatakanahalfwidth",
				"FF87nukatakanahalfwidth", "FF88nekatakanahalfwidth", "FF89nokatakanahalfwidth", "FF8Ahakatakanahalfwidth", "FF8Bhikatakanahalfwidth", "FF8Chukatakanahalfwidth", "FF8Dhekatakanahalfwidth", "FF8Ehokatakanahalfwidth", "FF8Fmakatakanahalfwidth", "FF90mikatakanahalfwidth",
				"FF91mukatakanahalfwidth", "FF92mekatakanahalfwidth", "FF93mokatakanahalfwidth", "FF94yakatakanahalfwidth", "FF95yukatakanahalfwidth", "FF96yokatakanahalfwidth", "FF97rakatakanahalfwidth", "FF98rikatakanahalfwidth", "FF99rukatakanahalfwidth", "FF9Arekatakanahalfwidth",
				"FF9Brokatakanahalfwidth", "FF9Cwakatakanahalfwidth", "FF9Dnkatakanahalfwidth", "FF9Evoicedmarkkanahalfwidth", "FF9Fsemivoicedmarkkanahalfwidth", "FFE0centmonospace", "FFE1sterlingmonospace", "FFE3macronmonospace", "FFE5yenmonospace", "FFE6wonmonospace"
			};
		}
	}

	private static void InitStandardFontInfo()
	{
		if (StandardFontInfo == null)
		{
			StandardFontInfo = new int[11, 12]
			{
				{
					0, 0, -120, -120, 0, 0, -155, -150, 0, 0,
					-120, -120
				},
				{
					-166, -170, -170, -174, -168, -168, -169, -200, -23, -113,
					-27, -57
				},
				{
					-225, -228, -225, -228, -218, -218, -217, -218, -250, -250,
					-250, -250
				},
				{
					1000, 1003, 1116, 1114, 1000, 1000, 1010, 996, 715, 749,
					849, 869
				},
				{
					931, 962, 931, 962, 898, 935, 883, 921, 805, 801,
					805, 801
				},
				{
					718, 718, 718, 718, 662, 676, 653, 669, 562, 562,
					562, 562
				},
				{
					523, 532, 523, 532, 450, 461, 441, 462, 426, 439,
					426, 439
				},
				{
					718, 718, 718, 718, 683, 683, 683, 683, 629, 629,
					629, 629
				},
				{
					-207, -207, -207, -207, -217, -217, -217, -217, -157, -157,
					-157, -157
				},
				{
					76, 118, 76, 118, 28, 44, 32, 42, 51, 84,
					51, 84
				},
				{
					88, 140, 88, 140, 84, 139, 76, 121, 51, 106,
					51, 106
				}
			};
		}
	}

	private static void InitCharNamesUniMap()
	{
		if (uniMap == null)
		{
			uniMap = new int[65536];
			for (int i = 0; i < 65536; i++)
			{
				uniMap[i] = -1;
			}
			for (int j = 0; j < StandardFontCharsNames.Length; j++)
			{
				string s = StandardFontCharsNames[j].Substring(0, 4);
				uniMap[int.Parse(s, NumberStyles.HexNumber)] = j;
			}
		}
	}
}
