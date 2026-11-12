using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing;

[SecuritySafeCritical]
public class StiTextRenderer
{
	public class StiFontState
	{
		public string FontName;

		public Font FontBase;

		public Font FontScaled;

		public int SuperOrSubscriptIndex;

		public int ParentFontIndex;

		public IntPtr hFont;

		public IntPtr hFontScaled;

		public IntPtr hScriptCache;

		public IntPtr hScriptCacheScaled;

		public double LineHeight;

		public double Ascend;

		public double Descend;

		public double ElipsisWidth;

		public double EmValue;

		public double TypoOffset;

		public bool HasNbspGlyf = true;

		private string fontNameReal;

		public string FontNameReal => fontNameReal ?? (fontNameReal = FontBase.Name);
	}

	public class LineInfo
	{
		public int Begin;

		public int Length;

		public bool NeedWidthAlign;

		public int Width;

		public double[] Widths;

		public double JustifyOffset;

		public string Text;

		public int IndexOfMaxFont;

		public double LineHeight;

		public StiTextHorAlignment TextAlignment;

		public int Indent;

		public int End
		{
			get
			{
				return Begin + Length;
			}
			set
			{
				Length = value - Begin;
			}
		}
	}

	public struct RunInfo
	{
		public string Text;

		public double XPos;

		public double YPos;

		public int[] Widths;

		public int[] GlyphWidths;

		public Color TextColor;

		public Color BackColor;

		public int FontIndex;

		public int[] GlyphIndexList;

		public double[] ScaleList;

		public string Href;

		public StiTextHorAlignment TextAlign;
	}

	[ToolboxItem(false)]
	[SuppressUnmanagedCodeSecurity]
	internal class StiMeasureRichTextBox : RichTextBox
	{
		private static IntPtr moduleHandle;

		private static bool failLoadModule;

		private static string lastCheckedClassName;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = ((RichTextBox)this).CreateParams;
				if (StiBaseOptions.ForceLoadExtendedRichTextLibrary.HasValue && !StiBaseOptions.ForceLoadExtendedRichTextLibrary.Value)
				{
					return createParams;
				}
				if (TryLoadLibrary())
				{
					createParams.ClassName = StiBaseOptions.ExtendedRichTextLibraryClassName;
				}
				return createParams;
			}
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		private static bool TryLoadLibrary()
		{
			if (!failLoadModule && moduleHandle == IntPtr.Zero)
			{
				try
				{
					moduleHandle = LoadLibrary("msftedit.dll");
				}
				catch
				{
					failLoadModule = true;
				}
			}
			return moduleHandle != IntPtr.Zero;
		}

		private void CheckClassName()
		{
			if (!(lastCheckedClassName != StiBaseOptions.ExtendedRichTextLibraryClassName))
			{
				return;
			}
			try
			{
				lastCheckedClassName = StiBaseOptions.ExtendedRichTextLibraryClassName;
				((RichTextBox)this).Rtf = string.Empty;
				((RichTextBox)this).Rtf = null;
			}
			catch (Win32Exception)
			{
				if (StiBaseOptions.ExtendedRichTextLibraryClassName.ToLowerInvariant() == "richedit20w")
				{
					StiBaseOptions.ForceLoadExtendedRichTextLibrary = false;
				}
				else if (StiBaseOptions.ExtendedRichTextLibraryClassName.ToLowerInvariant() == "richedit50w")
				{
					StiBaseOptions.ExtendedRichTextLibraryClassName = "RichEdit20W";
				}
				else
				{
					StiBaseOptions.ExtendedRichTextLibraryClassName = "RichEdit50W";
				}
			}
		}

		public StiMeasureRichTextBox()
		{
			CheckClassName();
		}
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
	private struct GOFFSET
	{
		[FieldOffset(0)]
		public int du;

		[FieldOffset(4)]
		public int dv;
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
	private struct SCRIPT_VISATTR
	{
		[FieldOffset(0)]
		private ushort packed;

		internal ushort uJustification => (ushort)(packed & 0xFu);

		internal ushort fClusterStart => (ushort)((packed & 0x10) >> 4);

		internal ushort fDiacritic => (ushort)((packed & 0x20) >> 5);

		internal ushort fZeroWidth => (ushort)((packed & 0x40) >> 6);

		internal ushort fReserved => (ushort)((packed & 0x80) >> 7);

		internal ushort fShapeReserved => (ushort)((packed & 0xFF00) >> 8);
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
	private struct SCRIPT_STATE
	{
		[FieldOffset(0)]
		internal ushort packed;

		internal ushort uBidiLevel
		{
			get
			{
				return (ushort)(packed & 0x1Fu);
			}
			set
			{
				packed = (ushort)((packed & 0xFFE0u) | (value & 0x1Fu));
			}
		}

		internal ushort fOverrideDirection => (ushort)((packed & 0x20) >> 5);

		internal ushort fInhibitSymSwap => (ushort)((packed & 0x40) >> 6);

		internal ushort fCharShape => (ushort)((packed & 0x80) >> 7);

		internal ushort fDigitSubstitute => (ushort)((packed & 0x100) >> 8);

		internal ushort fInhibitLigate => (ushort)((packed & 0x200) >> 9);

		internal ushort fDisplayZWG => (ushort)((packed & 0x400) >> 10);

		internal ushort fArabicNumContext => (ushort)((packed & 0x800) >> 11);

		internal ushort fGcpClusters => (ushort)((packed & 0x1000) >> 12);

		internal ushort fReserved => (ushort)((packed & 0x2000) >> 13);

		internal ushort fEngineReserved => (ushort)((packed & 0xC000) >> 14);
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
	private struct SCRIPT_ANALYSIS
	{
		[FieldOffset(0)]
		internal ushort packed;

		[FieldOffset(2)]
		public SCRIPT_STATE s;

		internal ushort eScript => (ushort)(packed & 0x3FFu);

		internal bool fRTL => (packed & 0x400) > 0;

		internal ushort fLayoutRTL => (ushort)((packed & 0x800) >> 11);

		internal ushort fLinkBefore => (ushort)((packed & 0x1000) >> 12);

		internal ushort fLinkAfter => (ushort)((packed & 0x2000) >> 13);

		internal ushort fLogicalOrder => (ushort)((packed & 0x4000) >> 14);

		internal ushort fNoGlyphIndex => (ushort)((packed & 0x8000) >> 15);
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto, Pack = 8)]
	private struct SCRIPT_ITEM
	{
		[FieldOffset(0)]
		internal int iCharPos;

		[FieldOffset(4)]
		internal SCRIPT_ANALYSIS a;
	}

	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
	private struct SCRIPT_CONTROL
	{
		[FieldOffset(0)]
		internal ushort uDefaultLanguage;

		[FieldOffset(2)]
		private byte packed;

		[FieldOffset(3)]
		internal byte fReserved;

		internal ushort fContextDigits => (ushort)(packed & 1u);

		internal ushort fInvertPreBoundDir => (ushort)((packed & 2) >> 1);

		internal ushort fInvertPostBoundDir => (ushort)((packed & 4) >> 2);

		internal ushort fLinkStringBefore => (ushort)((packed & 8) >> 3);

		internal ushort fLinkStringAfter => (ushort)((packed & 0x10) >> 4);

		internal ushort fNeutralOverride => (ushort)((packed & 0x20) >> 5);

		internal ushort fNumericOverride => (ushort)((packed & 0x40) >> 6);

		internal ushort fLegacyBidiClass => (ushort)((packed & 0x80) >> 7);

		private SCRIPT_CONTROL(uint data)
		{
			uDefaultLanguage = (ushort)(data & 0xFFFFu);
			packed = (byte)((data & 0xFF0000) >> 16);
			fReserved = (byte)((data & 0xFF000000u) >> 24);
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct SCRIPT_LOGATTR
	{
		internal byte packed;

		internal bool fSoftBreak
		{
			get
			{
				if ((packed & 1) <= 0)
				{
					return false;
				}
				return true;
			}
			set
			{
				packed |= 1;
				if (!value)
				{
					packed ^= 1;
				}
			}
		}

		internal bool fWhiteSpace
		{
			get
			{
				if ((packed & 2) >> 1 <= 0)
				{
					return false;
				}
				return true;
			}
		}

		internal bool fCharStop
		{
			get
			{
				if ((packed & 4) >> 2 <= 0)
				{
					return false;
				}
				return true;
			}
		}

		internal bool fWordStop
		{
			get
			{
				if ((packed & 8) >> 3 <= 0)
				{
					return false;
				}
				return true;
			}
		}

		internal bool fInvalid
		{
			get
			{
				if ((packed & 0x10) >> 4 <= 0)
				{
					return false;
				}
				return true;
			}
		}

		internal ushort fReserved => (ushort)((packed & 0xE0) >> 5);
	}

	private struct RECT
	{
		public int left;

		public int top;

		public int right;

		public int bottom;

		public Size Size => new Size(right - left, bottom - top);

		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}

		public RECT(Rectangle r)
		{
			left = r.Left;
			top = r.Top;
			right = r.Right;
			bottom = r.Bottom;
		}

		public static RECT FromXYWH(int x, int y, int width, int height)
		{
			return new RECT(x, y, x + width, y + height);
		}

		public Rectangle ToRectangle()
		{
			return new Rectangle(left, top, right - left, bottom - top);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class DRAWTEXTPARAMS
	{
		private int cbSize;

		public int iTabLength;

		public int iLeftMargin;

		public int iRightMargin;

		public int uiLengthDrawn;

		public DRAWTEXTPARAMS()
		{
			cbSize = Marshal.SizeOf(typeof(DRAWTEXTPARAMS));
		}
	}

	private struct ABC
	{
		internal int abcA;

		internal uint abcB;

		internal int abcC;

		internal ABC(int abcA, uint abcB, int abcC)
		{
			this.abcA = abcA;
			this.abcB = abcB;
			this.abcC = abcC;
		}
	}

	private struct XFORM
	{
		public float eM11;

		public float eM12;

		public float eM21;

		public float eM22;

		public float eDx;

		public float eDy;

		public XFORM(float eM11, float eM12, float eM21, float eM22, float eDx, float eDy)
		{
			this.eM11 = eM11;
			this.eM12 = eM12;
			this.eM21 = eM21;
			this.eM22 = eM22;
			this.eDx = eDx;
			this.eDy = eDy;
		}

		public XFORM(double eM11, double eM12, double eM21, double eM22, double eDx, double eDy)
		{
			this.eM11 = (float)eM11;
			this.eM12 = (float)eM12;
			this.eM21 = (float)eM21;
			this.eM22 = (float)eM22;
			this.eDx = (float)eDx;
			this.eDy = (float)eDy;
		}
	}

	private struct SIZE
	{
		public int cx;

		public int cy;

		public SIZE(int cx, int cy)
		{
			this.cx = cx;
			this.cy = cy;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct POINT
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
	private struct PANOSE
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
	private struct TEXTMETRIC
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

	private struct OUTLINETEXTMETRIC
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

		public uint otmpFamilyName;

		public uint otmpFaceName;

		public uint otmpStyleName;

		public uint otmpFullName;
	}

	[Flags]
	internal enum TextFormatFlags
	{
		Bottom = 8,
		CalculateRectangle = 0x400,
		Default = 0,
		EndEllipsis = 0x8000,
		ExpandTabs = 0x40,
		ExternalLeading = 0x200,
		HidePrefix = 0x100000,
		HorizontalCenter = 1,
		Internal = 0x1000,
		Left = 0,
		ModifyString = 0x10000,
		NoClipping = 0x100,
		NoFullWidthCharacterBreak = 0x80000,
		NoPrefix = 0x800,
		PathEllipsis = 0x4000,
		PrefixOnly = 0x200000,
		Right = 2,
		RightToLeft = 0x20000,
		SingleLine = 0x20,
		TabStop = 0x80,
		TextBoxControl = 0x2000,
		Top = 0,
		VerticalCenter = 4,
		WordBreak = 0x10,
		WordEllipsis = 0x40000
	}

	internal enum DeviceContextBackgroundMode
	{
		Opaque = 2,
		Transparent = 1
	}

	public enum StiHtmlTag
	{
		None,
		B,
		I,
		U,
		S,
		Sup,
		Sub,
		Font,
		FontName,
		FontSize,
		FontColor,
		Backcolor,
		LetterSpacing,
		WordSpacing,
		LineHeight,
		TextAlign,
		P,
		Br,
		OrderedList,
		UnorderedList,
		ListItem,
		A,
		Unknown
	}

	public enum StiHtmlTag2State
	{
		Start,
		End,
		Empty
	}

	public class StiHtmlTag2
	{
		public StiHtmlTag Tag;

		public string TagName;

		public List<TagPair> Attributes;

		internal StiHtmlTag2State State;

		public bool IsStart => State == StiHtmlTag2State.Start;

		public bool IsEnd => State == StiHtmlTag2State.End;

		public bool IsEmpty => State == StiHtmlTag2State.Empty;

		public bool IsStartTag(StiHtmlTag tag)
		{
			if (Tag == tag)
			{
				return State == StiHtmlTag2State.Start;
			}
			return false;
		}

		public bool IsEndTag(StiHtmlTag tag)
		{
			if (Tag == tag)
			{
				return State == StiHtmlTag2State.End;
			}
			return false;
		}

		public bool Equals(StiHtmlTag2 tag2)
		{
			if (Tag != tag2.Tag)
			{
				return false;
			}
			if (Tag == StiHtmlTag.Unknown)
			{
				return TagName == tag2.TagName;
			}
			return true;
		}

		public override string ToString()
		{
			if (Tag != StiHtmlTag.Unknown)
			{
				return Tag.ToString() + (IsStart ? "_Open" : "") + (IsEnd ? "_Close" : "");
			}
			return "'" + TagName + "'";
		}

		public StiHtmlTag2(StiHtmlTag tag = StiHtmlTag.None, StiHtmlTag2State state = StiHtmlTag2State.Start)
		{
			Tag = tag;
			State = state;
		}
	}

	public struct StiHtmlTagsState
	{
		public bool Bold;

		public bool Italic;

		public bool Underline;

		public bool Strikeout;

		public float FontSize;

		public string FontName;

		public Color FontColor;

		public Color BackColor;

		public bool Subscript;

		public bool Superscript;

		public double LetterSpacing;

		public double WordSpacing;

		public double LineHeight;

		public StiTextHorAlignment TextAlign;

		public bool IsColorChanged;

		public bool IsBackcolorChanged;

		public StiHtmlTag2 Tag;

		public int Indent;

		public string HtmlStyle;

		public string Href;

		public Hashtable StyleAttributes;

		public string GetStyleAttribute(string name)
		{
			if (StyleAttributes != null)
			{
				return (string)StyleAttributes[name];
			}
			return null;
		}

		public StiHtmlTagsState(bool bold, bool italic, bool underline, bool strikeout, float fontSize, string fontName, Color fontColor, Color backColor, bool superscript, bool subscript, double letterSpacing, double wordSpacing, double lineHeight, StiTextHorAlignment textAlign)
		{
			Bold = bold;
			Italic = italic;
			Underline = underline;
			Strikeout = strikeout;
			FontSize = fontSize;
			FontName = fontName;
			FontColor = fontColor;
			BackColor = backColor;
			Subscript = subscript;
			Superscript = superscript;
			LetterSpacing = letterSpacing;
			WordSpacing = wordSpacing;
			LineHeight = lineHeight;
			TextAlign = textAlign;
			IsColorChanged = false;
			IsBackcolorChanged = false;
			Tag = new StiHtmlTag2();
			Indent = 0;
			HtmlStyle = string.Empty;
			Href = null;
			StyleAttributes = null;
		}

		public StiHtmlTagsState(StiHtmlTagsState state)
		{
			Bold = state.Bold;
			Italic = state.Italic;
			Underline = state.Underline;
			Strikeout = state.Strikeout;
			FontSize = state.FontSize;
			FontName = state.FontName;
			FontColor = state.FontColor;
			BackColor = state.BackColor;
			Subscript = state.Subscript;
			Superscript = state.Superscript;
			LetterSpacing = state.LetterSpacing;
			WordSpacing = state.WordSpacing;
			LineHeight = state.LineHeight;
			TextAlign = state.TextAlign;
			IsColorChanged = state.IsColorChanged;
			IsBackcolorChanged = state.IsBackcolorChanged;
			Tag = state.Tag;
			Indent = state.Indent;
			HtmlStyle = string.Empty;
			Href = state.Href;
			StyleAttributes = state.StyleAttributes;
		}
	}

	public struct StiHtmlState
	{
		public StiHtmlTagsState TS;

		public StringBuilder Text;

		public int FontIndex;

		public int PosBegin;

		internal List<StiHtmlTagsState> TagsStack;

		internal List<int> ListLevels;

		public override string ToString()
		{
			return "\"" + Text.ToString().Replace("\n", "\\n") + "\" " + ((TS.Tag.Tag == StiHtmlTag.None) ? "" : TS.Tag.ToString()) + $" {TS.LineHeight}";
		}

		public StiHtmlState(string text)
		{
			TS = default(StiHtmlTagsState);
			TS.Tag = new StiHtmlTag2();
			Text = new StringBuilder(text);
			FontIndex = 0;
			PosBegin = 0;
			TagsStack = null;
			ListLevels = null;
		}

		public StiHtmlState(StiHtmlTagsState ts, int posBegin)
		{
			TS = ts;
			Text = new StringBuilder();
			FontIndex = 0;
			PosBegin = posBegin;
			TagsStack = null;
			ListLevels = null;
		}

		public StiHtmlState(StiHtmlState state)
		{
			TS = new StiHtmlTagsState(state.TS);
			Text = new StringBuilder();
			FontIndex = 0;
			PosBegin = state.PosBegin;
			TagsStack = ((state.TagsStack != null && state.TagsStack.Count > 0) ? state.TagsStack : null);
			ListLevels = state.ListLevels;
			if (TS.Indent < 0)
			{
				if (ListLevels != null)
				{
					TS.Indent = ListLevels.Count;
				}
				else
				{
					TS.Indent = 0;
				}
			}
		}
	}

	public class TagPair
	{
		public string Key;

		public string KeyBase;

		public string Value;
	}

	private static Hashtable htmlNameToColor;

	private static object lockHtmlNameToColor = new object();

	private static Hashtable outlineTextMetricsCache = new Hashtable();

	private static object lockOutlineTextMetricsCache = new object();

	private static object lockHashFonts = new object();

	private static Hashtable hashFontWidths = new Hashtable();

	private static Hashtable hashFontGlyphs = new Hashtable();

	private const int precisionDigits = 5;

	private const double defaultParagraphLineHeight = 0.7;

	private static List<string> fpc_fontName = new List<string> { "Arial", "Bookman Old Style" };

	private static int[,] fpc_fontData = new int[6, 42]
	{
		{
			0, 0, 0, 0, 0, 0, 2, 4, 2, -2,
			0, 0, 2, 2, 0, 0, 2, 2, 2, 1,
			0, 1, 0, 2, 2, 0, 0, 2, 2, 1,
			0, 0, 1, 2, 2, 2, 2, 2, 2, 2,
			2, 2
		},
		{
			0, 2, 2, 0, 0, 0, 2, 2, 0, -2,
			-1, 0, 1, 0, -1, 0, 0, 0, 1, 0,
			-1, 1, 0, 1, 0, -1, 0, 0, 0, 0,
			-1, -1, -1, 0, 0, -1, -1, 1, 0, 1,
			0, 0
		},
		{
			0, 0, 0, 0, 0, 0, -2, 2, 0, 2,
			1, 0, 0, 0, 1, 0, 0, 0, -1, 1,
			1, 0, 0, 1, 1, 1, 0, 0, -1, 0,
			1, 1, 1, 1, 1, 1, 1, 0, 1, 0,
			0, 1
		},
		{
			-1, -1, -1, 1, 1, 0, 1, 1, 1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, 1,
			1, 1, 0, 0, 0, 0, 0, 1, 2, 2,
			2, 1, 0, 0, 0, 1, 1, 2, 1, 0,
			0, 0
		},
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0
		},
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0
		}
	};

	private const int fpc_startIndex = 11;

	private static Hashtable CacheFontNameToNum = new Hashtable();

	private static Hashtable CacheNumToFontName = new Hashtable();

	private static Hashtable CacheFontSymbolsFallback = new Hashtable();

	private static int sizeofScriptItem = Marshal.SizeOf(typeof(SCRIPT_ITEM));

	private static int sizeofScriptVisattr = Marshal.SizeOf(typeof(SCRIPT_VISATTR));

	private static int sizeofGoffset = Marshal.SizeOf(typeof(GOFFSET));

	private const int E_SCRIPT_NOT_IN_FONT = -2147220992;

	private const int E_OUTOFMEMORY = -2147024882;

	private const int MWT_IDENTITY = 1;

	private const int MWT_LEFTMULTIPLY = 2;

	private const int MWT_RIGHTMULTIPLY = 3;

	private const int GM_COMPATIBLE = 1;

	private const int GM_ADVANCED = 2;

	private const int MM_TEXT = 1;

	private const int MM_LOMETRIC = 2;

	private const int MM_HIMETRIC = 3;

	private const int MM_LOENGLISH = 4;

	private const int MM_HIENGLISH = 5;

	private const int MM_TWIPS = 6;

	private const int MM_ISOTROPIC = 7;

	private const int MM_ANISOTROPIC = 8;

	private const uint GDI_ERROR = uint.MaxValue;

	private static Hashtable htmlEscapeSequence = null;

	private static string bulletBlack = new string('•', 1);

	private static string bulletWhite = new string('◦', 1);

	private static int[] Arabics = new int[6] { 1, 5, 10, 50, 100, 1000 };

	private static char[] Romans = new char[6] { 'I', 'V', 'X', 'L', 'C', 'M' };

	private static int[] Subs = new int[6] { 0, 0, 0, 2, 2, 4 };

	public static double PrecisionModeFactor { get; set; } = 4.0;


	public static bool PrecisionModeEnabled { get; set; } = false;


	public static bool PrecisionMode2Enabled { get; set; } = false;


	public static bool PrecisionMode3Enabled { get; set; } = true;


	public static bool CorrectionEnabled { get; set; } = true;


	public static float MaxFontSize { get; set; } = 1024f;


	public static bool Compatibility2009 { get; set; }

	public static bool OptimizeBottomMargin { get; set; } = true;


	public static bool InterpreteFontSizeInHtmlTagsAsInHtml { get; set; }

	public static double StiDpiHelperGraphicsScale { get; set; } = 1.0;


	public static string StiForceWidthAlignTag { get; set; } = "<forcewidth>";


	public static bool AllowCheckMissingGlyphs { get; set; } = true;


	private static Hashtable HtmlEscapeSequence
	{
		get
		{
			if (htmlEscapeSequence == null)
			{
				htmlEscapeSequence = new Hashtable();
				htmlEscapeSequence["&quot;"] = '"';
				htmlEscapeSequence["&amp;"] = '&';
				htmlEscapeSequence["&apos;"] = '\'';
				htmlEscapeSequence["&lt;"] = '<';
				htmlEscapeSequence["&gt;"] = '>';
				htmlEscapeSequence["&nbsp;"] = '\u00a0';
				htmlEscapeSequence["&iexcl;"] = '¡';
				htmlEscapeSequence["&cent;"] = '¢';
				htmlEscapeSequence["&pound;"] = '£';
				htmlEscapeSequence["&curren;"] = '¤';
				htmlEscapeSequence["&yen;"] = '¥';
				htmlEscapeSequence["&brvbar;"] = '¦';
				htmlEscapeSequence["&sect;"] = '§';
				htmlEscapeSequence["&uml;"] = '\u00a8';
				htmlEscapeSequence["&copy;"] = '©';
				htmlEscapeSequence["&ordf;"] = 'ª';
				htmlEscapeSequence["&laquo;"] = '«';
				htmlEscapeSequence["&not;"] = '¬';
				htmlEscapeSequence["&shy;"] = '\u00ad';
				htmlEscapeSequence["&reg;"] = '®';
				htmlEscapeSequence["&macr;"] = '\u00af';
				htmlEscapeSequence["&deg;"] = '°';
				htmlEscapeSequence["&plusmn;"] = '±';
				htmlEscapeSequence["&sup2;"] = '²';
				htmlEscapeSequence["&sup3;"] = '³';
				htmlEscapeSequence["&acute;"] = '\u00b4';
				htmlEscapeSequence["&micro;"] = 'µ';
				htmlEscapeSequence["&para;"] = '¶';
				htmlEscapeSequence["&middot;"] = '·';
				htmlEscapeSequence["&cedil;"] = '\u00b8';
				htmlEscapeSequence["&sup1;"] = '¹';
				htmlEscapeSequence["&ordm;"] = 'º';
				htmlEscapeSequence["&raquo;"] = '»';
				htmlEscapeSequence["&frac14;"] = '¼';
				htmlEscapeSequence["&frac12;"] = '½';
				htmlEscapeSequence["&frac34;"] = '¾';
				htmlEscapeSequence["&iquest;"] = '¿';
				htmlEscapeSequence["&Agrave;"] = 'À';
				htmlEscapeSequence["&Aacute;"] = 'Á';
				htmlEscapeSequence["&Acirc;"] = 'Â';
				htmlEscapeSequence["&Atilde;"] = 'Ã';
				htmlEscapeSequence["&Auml;"] = 'Ä';
				htmlEscapeSequence["&Aring;"] = 'Å';
				htmlEscapeSequence["&AElig;"] = 'Æ';
				htmlEscapeSequence["&Ccedil;"] = 'Ç';
				htmlEscapeSequence["&Egrave;"] = 'È';
				htmlEscapeSequence["&Eacute;"] = 'É';
				htmlEscapeSequence["&Ecirc;"] = 'Ê';
				htmlEscapeSequence["&Euml;"] = 'Ë';
				htmlEscapeSequence["&Igrave;"] = 'Ì';
				htmlEscapeSequence["&Iacute;"] = 'Í';
				htmlEscapeSequence["&Icirc;"] = 'Î';
				htmlEscapeSequence["&Iuml;"] = 'Ï';
				htmlEscapeSequence["&ETH;"] = 'Ð';
				htmlEscapeSequence["&Ntilde;"] = 'Ñ';
				htmlEscapeSequence["&Ograve;"] = 'Ò';
				htmlEscapeSequence["&Oacute;"] = 'Ó';
				htmlEscapeSequence["&Ocirc;"] = 'Ô';
				htmlEscapeSequence["&Otilde;"] = 'Õ';
				htmlEscapeSequence["&Ouml;"] = 'Ö';
				htmlEscapeSequence["&times;"] = '×';
				htmlEscapeSequence["&Oslash;"] = 'Ø';
				htmlEscapeSequence["&Ugrave;"] = 'Ù';
				htmlEscapeSequence["&Uacute;"] = 'Ú';
				htmlEscapeSequence["&Ucirc;"] = 'Û';
				htmlEscapeSequence["&Uuml;"] = 'Ü';
				htmlEscapeSequence["&Yacute;"] = 'Ý';
				htmlEscapeSequence["&THORN;"] = 'Þ';
				htmlEscapeSequence["&szlig;"] = 'ß';
				htmlEscapeSequence["&agrave;"] = 'à';
				htmlEscapeSequence["&aacute;"] = 'á';
				htmlEscapeSequence["&acirc;"] = 'â';
				htmlEscapeSequence["&atilde;"] = 'ã';
				htmlEscapeSequence["&auml;"] = 'ä';
				htmlEscapeSequence["&aring;"] = 'å';
				htmlEscapeSequence["&aelig;"] = 'æ';
				htmlEscapeSequence["&ccedil;"] = 'ç';
				htmlEscapeSequence["&egrave;"] = 'è';
				htmlEscapeSequence["&eacute;"] = 'é';
				htmlEscapeSequence["&ecirc;"] = 'ê';
				htmlEscapeSequence["&euml;"] = 'ë';
				htmlEscapeSequence["&igrave;"] = 'ì';
				htmlEscapeSequence["&iacute;"] = 'í';
				htmlEscapeSequence["&icirc;"] = 'î';
				htmlEscapeSequence["&iuml;"] = 'ï';
				htmlEscapeSequence["&eth;"] = 'ð';
				htmlEscapeSequence["&ntilde;"] = 'ñ';
				htmlEscapeSequence["&ograve;"] = 'ò';
				htmlEscapeSequence["&oacute;"] = 'ó';
				htmlEscapeSequence["&ocirc;"] = 'ô';
				htmlEscapeSequence["&otilde;"] = 'õ';
				htmlEscapeSequence["&ouml;"] = 'ö';
				htmlEscapeSequence["&divide;"] = '÷';
				htmlEscapeSequence["&oslash;"] = 'ø';
				htmlEscapeSequence["&ugrave;"] = 'ù';
				htmlEscapeSequence["&uacute;"] = 'ú';
				htmlEscapeSequence["&ucirc;"] = 'û';
				htmlEscapeSequence["&uuml;"] = 'ü';
				htmlEscapeSequence["&yacute;"] = 'ý';
				htmlEscapeSequence["&thorn;"] = 'þ';
				htmlEscapeSequence["&yuml;"] = 'ÿ';
				htmlEscapeSequence["&OElig;"] = 'Œ';
				htmlEscapeSequence["&oelig;"] = 'œ';
				htmlEscapeSequence["&Scaron;"] = 'Š';
				htmlEscapeSequence["&scaron;"] = 'š';
				htmlEscapeSequence["&Yuml;"] = 'Ÿ';
				htmlEscapeSequence["&fnof;"] = 'ƒ';
				htmlEscapeSequence["&circ;"] = 'ˆ';
				htmlEscapeSequence["&tilde;"] = '\u02dc';
				htmlEscapeSequence["&Alpha;"] = 'Α';
				htmlEscapeSequence["&Beta;"] = 'Β';
				htmlEscapeSequence["&Gamma;"] = 'Γ';
				htmlEscapeSequence["&Delta;"] = 'Δ';
				htmlEscapeSequence["&Epsilon;"] = 'Ε';
				htmlEscapeSequence["&Zeta;"] = 'Ζ';
				htmlEscapeSequence["&Eta;"] = 'Η';
				htmlEscapeSequence["&Theta;"] = 'Θ';
				htmlEscapeSequence["&Iota;"] = 'Ι';
				htmlEscapeSequence["&Kappa;"] = 'Κ';
				htmlEscapeSequence["&Lambda;"] = 'Λ';
				htmlEscapeSequence["&Mu;"] = 'Μ';
				htmlEscapeSequence["&Nu;"] = 'Ν';
				htmlEscapeSequence["&Xi;"] = 'Ξ';
				htmlEscapeSequence["&Omicron;"] = 'Ο';
				htmlEscapeSequence["&Pi;"] = 'Π';
				htmlEscapeSequence["&Rho;"] = 'Ρ';
				htmlEscapeSequence["&Sigma;"] = 'Σ';
				htmlEscapeSequence["&Tau;"] = 'Τ';
				htmlEscapeSequence["&Upsilon;"] = 'Υ';
				htmlEscapeSequence["&Phi;"] = 'Φ';
				htmlEscapeSequence["&Chi;"] = 'Χ';
				htmlEscapeSequence["&Psi;"] = 'Ψ';
				htmlEscapeSequence["&Omega;"] = 'Ω';
				htmlEscapeSequence["&alpha;"] = 'α';
				htmlEscapeSequence["&beta;"] = 'β';
				htmlEscapeSequence["&gamma;"] = 'γ';
				htmlEscapeSequence["&delta;"] = 'δ';
				htmlEscapeSequence["&epsilon;"] = 'ε';
				htmlEscapeSequence["&zeta;"] = 'ζ';
				htmlEscapeSequence["&eta;"] = 'η';
				htmlEscapeSequence["&theta;"] = 'θ';
				htmlEscapeSequence["&iota;"] = 'ι';
				htmlEscapeSequence["&kappa;"] = 'κ';
				htmlEscapeSequence["&lambda;"] = 'λ';
				htmlEscapeSequence["&mu;"] = 'μ';
				htmlEscapeSequence["&nu;"] = 'ν';
				htmlEscapeSequence["&xi;"] = 'ξ';
				htmlEscapeSequence["&omicron;"] = 'ο';
				htmlEscapeSequence["&pi;"] = 'π';
				htmlEscapeSequence["&rho;"] = 'ρ';
				htmlEscapeSequence["&sigmaf;"] = 'ς';
				htmlEscapeSequence["&sigma;"] = 'σ';
				htmlEscapeSequence["&tau;"] = 'τ';
				htmlEscapeSequence["&upsilon;"] = 'υ';
				htmlEscapeSequence["&phi;"] = 'φ';
				htmlEscapeSequence["&chi;"] = 'χ';
				htmlEscapeSequence["&psi;"] = 'ψ';
				htmlEscapeSequence["&omega;"] = 'ω';
				htmlEscapeSequence["&thetasym;"] = 'ϑ';
				htmlEscapeSequence["&upsih;"] = 'ϒ';
				htmlEscapeSequence["&piv;"] = 'ϖ';
				htmlEscapeSequence["&ensp;"] = '\u2002';
				htmlEscapeSequence["&emsp;"] = '\u2003';
				htmlEscapeSequence["&thinsp;"] = '\u2009';
				htmlEscapeSequence["&zwnj;"] = '\u200c';
				htmlEscapeSequence["&zwj;"] = '\u200d';
				htmlEscapeSequence["&lrm;"] = '\u200e';
				htmlEscapeSequence["&rlm;"] = '\u200f';
				htmlEscapeSequence["&ndash;"] = '–';
				htmlEscapeSequence["&mdash;"] = '—';
				htmlEscapeSequence["&lsquo;"] = '‘';
				htmlEscapeSequence["&rsquo;"] = '’';
				htmlEscapeSequence["&sbquo;"] = '‚';
				htmlEscapeSequence["&ldquo;"] = '“';
				htmlEscapeSequence["&rdquo;"] = '”';
				htmlEscapeSequence["&bdquo;"] = '„';
				htmlEscapeSequence["&dagger;"] = '†';
				htmlEscapeSequence["&Dagger;"] = '‡';
				htmlEscapeSequence["&bull;"] = '•';
				htmlEscapeSequence["&hellip;"] = '…';
				htmlEscapeSequence["&permil;"] = '‰';
				htmlEscapeSequence["&prime;"] = '′';
				htmlEscapeSequence["&Prime;"] = '″';
				htmlEscapeSequence["&lsaquo;"] = '‹';
				htmlEscapeSequence["&rsaquo;"] = '›';
				htmlEscapeSequence["&oline;"] = '‾';
				htmlEscapeSequence["&frasl;"] = '⁄';
				htmlEscapeSequence["&euro;"] = '€';
				htmlEscapeSequence["&image;"] = '₭';
				htmlEscapeSequence["&weierp;"] = '℘';
				htmlEscapeSequence["&real;"] = 'ℜ';
				htmlEscapeSequence["&trade;"] = '™';
				htmlEscapeSequence["&alefsym;"] = 'ℵ';
				htmlEscapeSequence["&larr;"] = '←';
				htmlEscapeSequence["&uarr;"] = '↑';
				htmlEscapeSequence["&rarr;"] = '→';
				htmlEscapeSequence["&darr;"] = '↓';
				htmlEscapeSequence["&harr;"] = '↔';
				htmlEscapeSequence["&crarr;"] = '↵';
				htmlEscapeSequence["&lArr;"] = '⇐';
				htmlEscapeSequence["&uArr;"] = '⇑';
				htmlEscapeSequence["&rArr;"] = '⇒';
				htmlEscapeSequence["&dArr;"] = '⇓';
				htmlEscapeSequence["&hArr;"] = '⇔';
				htmlEscapeSequence["&forall;"] = '∀';
				htmlEscapeSequence["&part;"] = '∂';
				htmlEscapeSequence["&exist;"] = '∃';
				htmlEscapeSequence["&empty;"] = '∅';
				htmlEscapeSequence["&nabla;"] = '∇';
				htmlEscapeSequence["&isin;"] = '∈';
				htmlEscapeSequence["&notin;"] = '∉';
				htmlEscapeSequence["&ni;"] = '∋';
				htmlEscapeSequence["&prod;"] = '∏';
				htmlEscapeSequence["&sum;"] = '∑';
				htmlEscapeSequence["&minus;"] = '−';
				htmlEscapeSequence["&lowast;"] = '∗';
				htmlEscapeSequence["&radic;"] = '√';
				htmlEscapeSequence["&prop;"] = '∝';
				htmlEscapeSequence["&infin;"] = '∞';
				htmlEscapeSequence["&ang;"] = '∠';
				htmlEscapeSequence["&and;"] = '∧';
				htmlEscapeSequence["&or;"] = '∨';
				htmlEscapeSequence["&cap;"] = '∩';
				htmlEscapeSequence["&cup;"] = '∪';
				htmlEscapeSequence["&int;"] = '∫';
				htmlEscapeSequence["&there4;"] = '∴';
				htmlEscapeSequence["&sim;"] = '∼';
				htmlEscapeSequence["&cong;"] = '≅';
				htmlEscapeSequence["&asymp;"] = '≈';
				htmlEscapeSequence["&ne;"] = '≠';
				htmlEscapeSequence["&equiv;"] = '≡';
				htmlEscapeSequence["&le;"] = '≤';
				htmlEscapeSequence["&ge;"] = '≥';
				htmlEscapeSequence["&sub;"] = '⊂';
				htmlEscapeSequence["&sup;"] = '⊃';
				htmlEscapeSequence["&nsub;"] = '⊄';
				htmlEscapeSequence["&sube;"] = '⊆';
				htmlEscapeSequence["&supe;"] = '⊇';
				htmlEscapeSequence["&oplus;"] = '⊕';
				htmlEscapeSequence["&otimes;"] = '⊗';
				htmlEscapeSequence["&perp;"] = '⊥';
				htmlEscapeSequence["&sdot;"] = '⋅';
				htmlEscapeSequence["&lceil;"] = '⌈';
				htmlEscapeSequence["&rceil;"] = '⌉';
				htmlEscapeSequence["&lfloor;"] = '⌊';
				htmlEscapeSequence["&rfloor;"] = '⌋';
				htmlEscapeSequence["&lang;"] = '〈';
				htmlEscapeSequence["&rang;"] = '〉';
				htmlEscapeSequence["&loz;"] = '◊';
				htmlEscapeSequence["&spades;"] = '♠';
				htmlEscapeSequence["&clubs;"] = '♣';
				htmlEscapeSequence["&hearts;"] = '♥';
				htmlEscapeSequence["&diams;"] = '♦';
			}
			return htmlEscapeSequence;
		}
	}

	public static object GetFontGlyphs(Font font)
	{
		string text = font.Name + (font.Bold ? ",bold" : "") + (font.Italic ? ",italic" : "");
		object obj = null;
		lock (lockHashFonts)
		{
			obj = hashFontGlyphs[text];
			if (obj == null)
			{
				GetGdiFontData(font, text);
				obj = hashFontGlyphs[text];
			}
		}
		return obj;
	}

	private static ushort[] GetFontWidth(StiFontState fontState)
	{
		string text = fontState.FontNameReal + (fontState.FontBase.Bold ? ",bold" : "") + (fontState.FontBase.Italic ? ",italic" : "");
		object obj = null;
		lock (lockHashFonts)
		{
			obj = hashFontWidths[text];
			if (obj == null)
			{
				GetGdiFontData(fontState.FontBase, text);
				obj = hashFontWidths[text];
			}
		}
		return (ushort[])obj;
	}

	[SecuritySafeCritical]
	private static void GetGdiFontData(Font font, string fontHashName)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		ushort[] array = new ushort[0];
		ushort[] value = new ushort[0];
		try
		{
			Graphics val = Graphics.FromHwnd(IntPtr.Zero);
			try
			{
				IntPtr hdc = val.GetHdc();
				try
				{
					Font val2 = StiFontCollection.CreateFont(font.Name, MaxFontSize, font.Style);
					try
					{
						IntPtr intPtr = val2.ToHfont();
						try
						{
							IntPtr obj = SelectObject(hdc, intPtr);
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = 0; i < 65536; i++)
							{
								stringBuilder.Append((char)i);
							}
							string text = stringBuilder.ToString();
							int length = text.Length;
							ushort[] array2 = new ushort[length];
							uint glyphIndices = GetGlyphIndices(hdc, text, length, array2, 1u);
							if (glyphIndices == uint.MaxValue)
							{
								ThrowError(100);
							}
							value = array2;
							int num = 0;
							for (int j = 0; j < length; j++)
							{
								if (array2[j] != ushort.MaxValue && array2[j] > num)
								{
									num = array2[j];
								}
							}
							ushort[] array3 = new ushort[num];
							for (ushort num2 = 0; num2 < num; num2++)
							{
								array3[num2] = num2;
							}
							ABC[] array4 = new ABC[num];
							if (!GetCharABCWidthsI(hdc, 0u, (uint)num, array3, array4))
							{
								ThrowError(101);
							}
							ushort[] array5 = new ushort[num];
							for (int k = 0; k < num; k++)
							{
								double a = array4[k].abcA + array4[k].abcB + array4[k].abcC;
								array5[k] = (ushort)Math.Round(a);
							}
							int num3 = num - 1;
							while (num3 > 0 && array5[num3 - 1] == array5[num - 1])
							{
								num3--;
							}
							num3++;
							array = new ushort[num3];
							for (int l = 0; l < num3; l++)
							{
								array[l] = array5[l];
							}
							SelectObject(hdc, obj);
						}
						finally
						{
							if (!DeleteObject(intPtr))
							{
								ThrowError(102);
							}
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
		catch
		{
		}
		hashFontGlyphs[fontHashName] = value;
		hashFontWidths[fontHashName] = array;
	}

	private static void ThrowError(int step)
	{
		Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
		throw new Exception($"TextRender error at step {step}, code #{ex.ErrorCode:X8}: {ex.Message}");
	}

	private static void ThrowError(int step, int error)
	{
		Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
		throw new Exception($"TextRender error at step {step}, code #{ex.ErrorCode:X8}(#{error:X8}): {ex.Message}");
	}

	internal static int ColorToWin32(Color c)
	{
		return c.R | (c.G << 8) | (c.B << 16);
	}

	private static int GetTabsWidth(StiTextOptions textOptions, double tabSpaceWidth, double currentPosition)
	{
		float num = 20f;
		float num2 = 40f;
		if (textOptions != null)
		{
			num = textOptions.DistanceBetweenTabs;
			num2 = textOptions.FirstTabOffset;
		}
		double num3 = currentPosition;
		double num4 = tabSpaceWidth * (double)num;
		double num5 = tabSpaceWidth * (double)num2 + num4;
		if (currentPosition < num5)
		{
			num3 = num5;
		}
		else if (num > 0f)
		{
			int num6 = (int)((currentPosition - num5) / num4);
			num6++;
			num3 = num5 + (double)num6 * num4;
		}
		return (int)Math.Round((decimal)(num3 - currentPosition));
	}

	public static int GetFontIndex(string fontName, float fontSize, bool bold, bool italic, bool underlined, bool strikeout, bool superOrSubscript, List<StiFontState> tempFontList)
	{
		int num = GetFontIndex2(fontName, fontSize, bold, italic, underlined, strikeout, tempFontList);
		if (superOrSubscript)
		{
			int num2 = tempFontList[num].SuperOrSubscriptIndex;
			if (num2 == -1)
			{
				num2 = GetFontIndex2(fontName, fontSize / 1.5f, bold, italic, underlined, strikeout, tempFontList);
				tempFontList[num].SuperOrSubscriptIndex = num2;
				tempFontList[num2].ParentFontIndex = num;
			}
			num = num2;
		}
		return num;
	}

	private static int GetFontIndex2(string fontName, float fontSize, bool bold, bool italic, bool underlined, bool strikeout, List<StiFontState> tempFontList)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if (tempFontList.Count > 0)
		{
			for (int i = 0; i < tempFontList.Count; i++)
			{
				StiFontState stiFontState = tempFontList[i];
				if (stiFontState.FontName == fontName && stiFontState.FontBase.Size == fontSize && stiFontState.FontBase.Bold == bold && stiFontState.FontBase.Italic == italic && stiFontState.FontBase.Underline == underlined && stiFontState.FontBase.Strikeout == strikeout)
				{
					return i;
				}
			}
		}
		FontStyle val = (FontStyle)0;
		if (bold)
		{
			val = (FontStyle)(val | 1);
		}
		if (italic)
		{
			val = (FontStyle)(val | 2);
		}
		if (underlined)
		{
			val = (FontStyle)(val | 4);
		}
		if (strikeout)
		{
			val = (FontStyle)(val | 8);
		}
		Font val2 = null;
		if (fontName.IndexOf(',') != -1)
		{
			string[] array = fontName.Split(new char[1] { ',' });
			string[] array2 = array;
			foreach (string text in array2)
			{
				val2 = StiFontCollection.CreateFont(text, fontSize, val);
				if (val2.Name.Equals(text, StringComparison.InvariantCultureIgnoreCase))
				{
					break;
				}
			}
		}
		else
		{
			val2 = StiFontCollection.CreateFont(fontName, fontSize, val);
		}
		StiFontState item = new StiFontState
		{
			FontName = fontName,
			FontBase = val2,
			ParentFontIndex = -1,
			SuperOrSubscriptIndex = -1
		};
		tempFontList.Add(item);
		return tempFontList.Count - 1;
	}

	private static double GetFontPositionCorrection(StiFontState fontState)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		int num = fpc_fontName.IndexOf(fontState.FontNameReal);
		if (num == -1)
		{
			return 0.0;
		}
		double result = 0.0;
		int num2 = 0;
		if ((fontState.FontBase.Style & 1) > 0)
		{
			num2++;
		}
		if ((fontState.FontBase.Style & 2) > 0)
		{
			num2 += 2;
		}
		if (num2 > 0)
		{
			int num3 = (int)Math.Round(fontState.FontBase.Size);
			if (num3 < 11)
			{
				num3 = 11;
			}
			if (num3 > 52)
			{
				num3 = 52;
			}
			int num4 = fpc_fontData[num * 3 + num2 - 1, num3 - 11];
			result = (double)((float)(-num4) * fontState.FontBase.Size) * 0.025;
		}
		return result;
	}

	[SecuritySafeCritical]
	public static SizeD MeasureText(Graphics g, string text, Font font, RectangleD bounds, double lineSpacing, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool lineLimit, bool allowHtmlTags, StiTextOptions textOptions, double? scaleCorrection = null)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		lock (g)
		{
			SizeD measureSize = new SizeD(0.0, 0.0);
			DrawTextBase(g, ref text, font, bounds, Color.Black, Color.Black, lineSpacing, StiTextHorAlignment.Left, StiVertAlignment.Top, wordWrap, rightToLeft, scale, angle, trimming, lineLimit, ref measureSize, needDraw: false, null, null, allowHtmlTags, null, null, textOptions, scaleCorrection);
			return measureSize;
		}
	}

	[SecuritySafeCritical]
	public static void DrawText(Graphics g, string text, Font font, RectangleD bounds, Color foreColor, Color backColor, double lineSpacing, StiTextHorAlignment horAlign, StiVertAlignment vertAlign, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool lineLimit, bool allowHtmlTags, StiTextOptions textOptions = null, double? scaleCorrection = null)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		SizeD measureSize = new SizeD(0.0, 0.0);
		DrawTextBase(g, ref text, font, bounds, foreColor, backColor, lineSpacing, horAlign, vertAlign, wordWrap, rightToLeft, scale, angle, trimming, lineLimit, ref measureSize, needDraw: true, null, null, allowHtmlTags, null, null, textOptions, scaleCorrection);
	}

	[SecuritySafeCritical]
	public static string BreakText(Graphics g, ref string text, Font font, RectangleD bounds, Color foreColor, Color backColor, double lineSpacing, StiTextHorAlignment horAlign, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool allowHtmlTags, StiTextOptions textOptions, double? scaleCorrection = null)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		SizeD measureSize = new SizeD(0.0, 0.0);
		return DrawTextBase(g, ref text, font, bounds, foreColor, backColor, lineSpacing, horAlign, StiVertAlignment.Top, wordWrap, rightToLeft, scale, angle, trimming, lineLimit: true, ref measureSize, needDraw: false, null, null, allowHtmlTags, null, null, textOptions, scaleCorrection);
	}

	[SecuritySafeCritical]
	public static List<string> GetTextLines(Graphics g, ref string text, Font font, RectangleD bounds, double lineSpacing, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool allowHtmlTags, StiTextOptions textOptions, double? scaleCorrection = null)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		SizeD measureSize = new SizeD(0.0, 0.0);
		List<string> list = new List<string>();
		DrawTextBase(g, ref text, font, bounds, Color.Black, Color.Black, lineSpacing, StiTextHorAlignment.Left, StiVertAlignment.Top, wordWrap, rightToLeft, scale, angle, trimming, lineLimit: true, ref measureSize, needDraw: false, list, null, allowHtmlTags, null, null, textOptions, scaleCorrection);
		return list;
	}

	[SecuritySafeCritical]
	public static List<string> GetTextLinesAndWidths(Graphics g, ref string text, Font font, RectangleD bounds, double lineSpacing, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool allowHtmlTags, ref List<string> textLines, ref List<LineInfo> linesInfo, StiTextOptions textOptions, double? scaleCorrection = null)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		SizeD measureSize = new SizeD(0.0, 0.0);
		DrawTextBase(g, ref text, font, bounds, Color.Black, Color.Black, lineSpacing, StiTextHorAlignment.Left, StiVertAlignment.Top, wordWrap, rightToLeft, scale, angle, trimming, lineLimit: true, ref measureSize, needDraw: false, textLines, linesInfo, allowHtmlTags, null, null, textOptions, scaleCorrection);
		return textLines;
	}

	[SecuritySafeCritical]
	public static void DrawTextForOutput(Graphics g, string text, Font font, RectangleD bounds, Color foreColor, Color backColor, double lineSpacing, StiTextHorAlignment horAlign, StiVertAlignment vertAlign, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool lineLimit, bool allowHtmlTags, List<RunInfo> outRunsList, List<StiFontState> outFontsList, StiTextOptions textOptions, double? scaleCorrection = null)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		SizeD measureSize = new SizeD(0.0, 0.0);
		DrawTextBase(g, ref text, font, bounds, foreColor, backColor, lineSpacing, horAlign, vertAlign, wordWrap, rightToLeft, scale, angle, trimming, lineLimit, ref measureSize, needDraw: true, null, null, allowHtmlTags, outRunsList, outFontsList, textOptions, scaleCorrection);
	}

	[SecuritySafeCritical]
	private static string DrawTextBase(Graphics g, ref string text, Font font, RectangleD bounds, Color foreColor, Color backColor, double lineSpacing, StiTextHorAlignment horAlign, StiVertAlignment vertAlign, bool wordWrap, bool rightToLeft, double scale, double angle, StringTrimming trimming, bool lineLimit, ref SizeD measureSize, bool needDraw, List<string> textLinesArray, List<LineInfo> textLinesInfo, bool allowHtmlTags, List<RunInfo> outRunsList, List<StiFontState> outFontsList, StiTextOptions textOptions, double? scaleCorrection = null)
	{
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Expected O, but got Unknown
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Invalid comparison between Unknown and I4
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Invalid comparison between Unknown and I4
		//IL_271f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca4: Invalid comparison between Unknown and I4
		//IL_2ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cac: Invalid comparison between Unknown and I4
		//IL_6691: Unknown result type (might be due to invalid IL or missing references)
		//IL_669a: Expected O, but got Unknown
		//IL_66e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_66e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_69e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_680a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6813: Expected O, but got Unknown
		//IL_68fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_690e: Expected O, but got Unknown
		//IL_258f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2592: Invalid comparison between Unknown and I4
		//IL_2594: Unknown result type (might be due to invalid IL or missing references)
		//IL_2597: Invalid comparison between Unknown and I4
		//IL_1f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f68: Invalid comparison between Unknown and I4
		//IL_1f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f6d: Invalid comparison between Unknown and I4
		//IL_2103: Unknown result type (might be due to invalid IL or missing references)
		//IL_2124: Unknown result type (might be due to invalid IL or missing references)
		//IL_2127: Invalid comparison between Unknown and I4
		//IL_2129: Unknown result type (might be due to invalid IL or missing references)
		//IL_212c: Invalid comparison between Unknown and I4
		//IL_224b: Unknown result type (might be due to invalid IL or missing references)
		//IL_224e: Invalid comparison between Unknown and I4
		//IL_2250: Unknown result type (might be due to invalid IL or missing references)
		//IL_2253: Invalid comparison between Unknown and I4
		//IL_226a: Unknown result type (might be due to invalid IL or missing references)
		//IL_226d: Invalid comparison between Unknown and I4
		//IL_226f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2272: Invalid comparison between Unknown and I4
		//IL_22c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c5: Invalid comparison between Unknown and I4
		//IL_22c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ca: Invalid comparison between Unknown and I4
		RectangleD rectangleD = new RectangleD(bounds.X + (double)g.Transform.OffsetX + 0.0, bounds.Y + (double)g.Transform.OffsetY, bounds.Width + 1.0, bounds.Height + 1.0);
		double num = (PrecisionMode2Enabled ? (1.5 + (((double)font.SizeInPoints * StiDpiHelperGraphicsScale * scaleCorrection.GetValueOrDefault(1.0) > 6.0) ? (((double)font.SizeInPoints * StiDpiHelperGraphicsScale * scaleCorrection.GetValueOrDefault(1.0) - 6.0) * 0.2) : 0.0)) : 1.5);
		RectangleD rectangleD2 = new RectangleD(rectangleD.X + num * scale, rectangleD.Y, (double)Math.Round((decimal)(bounds.Width - num * 2.0 * scale), 5), bounds.Height);
		SizeD sizeD = new SizeD(rectangleD2.Width, rectangleD2.Height);
		angle %= 360.0;
		if (angle < 0.0)
		{
			angle = 360.0 + angle;
		}
		Font val = font;
		double num2 = scale;
		if (PrecisionModeEnabled)
		{
			font = StiFontCollection.CreateFont(font.Name, (float)((double)font.Size * PrecisionModeFactor), font.Style);
			scale = num2 / PrecisionModeFactor;
		}
		GraphicsUnit pageUnit = g.PageUnit;
		float dpiX = g.DpiX;
		float dpiY = g.DpiY;
		RectangleF clipBounds = g.ClipBounds;
		float pageScale = g.PageScale;
		Image val2 = null;
		Type type = ((object)g).GetType();
		FieldInfo field = type.GetField("backingImage", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field != null)
		{
			object value = field.GetValue(g);
			if (value != null)
			{
				val2 = (Image)((value is Image) ? value : null);
			}
		}
		bool flag = (val2 != null && val2 is Metafile) || outRunsList != null;
		string result = text;
		string text2 = string.Empty;
		List<RunInfo> list = new List<RunInfo>();
		StiFontState[] array = null;
		StiHtmlState[] array2 = null;
		int[] array3 = null;
		int num3 = 0;
		if (!string.IsNullOrEmpty(text) && foreColor != Color.Transparent && font != null && scale > 1E-05)
		{
			if (horAlign == StiTextHorAlignment.Width)
			{
				wordWrap = true;
			}
			bool flag2 = text.EndsWith(StiForceWidthAlignTag, StringComparison.InvariantCulture);
			if (flag2)
			{
				text = text.Substring(0, text.Length - StiForceWidthAlignTag.Length);
				result = text;
			}
			string text3 = text;
			StiHtmlTagsState ts = new StiHtmlTagsState(val.Bold, val.Italic, val.Underline, val.Strikeout, val.SizeInPoints, val.Name, foreColor, backColor, superscript: false, subscript: false, 0.0, 0.0, lineSpacing, horAlign);
			StiHtmlState stiHtmlState = new StiHtmlState(ts, 0);
			if (!allowHtmlTags && AllowCheckMissingGlyphs)
			{
				try
				{
					StringBuilder text4 = stiHtmlState.Text;
					stiHtmlState.Text = new StringBuilder(text);
					List<StiHtmlState> list2 = CheckMissingGlyphs(stiHtmlState);
					stiHtmlState.Text = text4;
					if (list2 != null)
					{
						allowHtmlTags = true;
						text = ConvertSymbolsToTags(text);
						text3 = text;
					}
				}
				catch
				{
					AllowCheckMissingGlyphs = false;
				}
			}
			if (allowHtmlTags)
			{
				List<StiHtmlState> list3 = ParseHtmlToStates(text, stiHtmlState, !needDraw);
				array2 = new StiHtmlState[list3.Count];
				StringBuilder stringBuilder = new StringBuilder();
				List<int> list4 = new List<int>();
				for (int i = 0; i < list3.Count; i++)
				{
					StiHtmlState stiHtmlState2 = (array2[i] = list3[i]);
					StringBuilder stringBuilder2 = PrepareStateText(stiHtmlState2.Text);
					stringBuilder.Append((object?)stringBuilder2);
					for (int j = 0; j < stringBuilder2.Length; j++)
					{
						list4.Add(i);
					}
					if (stiHtmlState2.TS.TextAlign == StiTextHorAlignment.Width)
					{
						stiHtmlState2.TS.WordSpacing = 0.0;
					}
				}
				text = stringBuilder.ToString();
				array3 = new int[list4.Count];
				for (int k = 0; k < array3.Length; k++)
				{
					array3[k] = list4[k];
				}
			}
			else
			{
				array2 = new StiHtmlState[1] { stiHtmlState };
				array3 = new int[text.Length];
				for (int l = 0; l < array3.Length; l++)
				{
					array3[l] = 0;
				}
			}
			List<StiFontState> list5 = new List<StiFontState>();
			for (int m = 0; m < array2.Length; m++)
			{
				float num4 = (float)(PrecisionModeEnabled ? ((double)array2[m].TS.FontSize * PrecisionModeFactor) : ((double)array2[m].TS.FontSize));
				if (scaleCorrection.HasValue)
				{
					num4 = (float)((double)num4 * StiDpiHelperGraphicsScale * scaleCorrection.Value);
				}
				array2[m].FontIndex = GetFontIndex(array2[m].TS.FontName, num4, array2[m].TS.Bold, array2[m].TS.Italic, array2[m].TS.Underline, array2[m].TS.Strikeout, array2[m].TS.Superscript || array2[m].TS.Subscript, list5);
			}
			array = new StiFontState[list5.Count];
			for (int n = 0; n < array.Length; n++)
			{
				array[n] = list5[n];
				ushort[] array4 = (ushort[])GetFontGlyphs(list5[n].FontBase);
				if (array4[160] == ushort.MaxValue)
				{
					array[n].HasNbspGlyf = false;
				}
			}
			for (int num5 = 0; num5 < array.Length; num5++)
			{
				array[num5].FontScaled = null;
				array[num5].hFontScaled = IntPtr.Zero;
				array[num5].hScriptCache = IntPtr.Zero;
				array[num5].hScriptCacheScaled = IntPtr.Zero;
				if ((scale != 1.0 || PrecisionModeEnabled) && needDraw)
				{
					array[num5].FontScaled = new Font(StiFontCollection.GetFontFamily(array[num5].FontBase.FontFamily.Name), (float)((double)array[num5].FontBase.Size * scale), array[num5].FontBase.Style, array[num5].FontBase.Unit, array[num5].FontBase.GdiCharSet, array[num5].FontBase.GdiVerticalFont);
					array[num5].hFontScaled = array[num5].FontScaled.ToHfont();
				}
			}
			try
			{
				IntPtr hdc = g.GetHdc();
				try
				{
					for (int num6 = 0; num6 < array.Length; num6++)
					{
						OUTLINETEXTMETRIC outlineTextMetricsCached = GetOutlineTextMetricsCached(array[num6].FontName, array[num6].FontBase.Style, hdc);
						float num7 = (float)(outlineTextMetricsCached.otmTextMetrics.tmAscent + outlineTextMetricsCached.otmTextMetrics.tmDescent + outlineTextMetricsCached.otmTextMetrics.tmExternalLeading) / MaxFontSize * array[num6].FontBase.SizeInPoints;
						double num8 = (double)((float)outlineTextMetricsCached.otmTextMetrics.tmAscent / MaxFontSize * array[num6].FontBase.SizeInPoints) * scale;
						double descend = (double)((float)outlineTextMetricsCached.otmTextMetrics.tmDescent / MaxFontSize * array[num6].FontBase.SizeInPoints) * scale;
						double typoOffset = 0.0;
						if ((outlineTextMetricsCached.otmfsSelection & 0x80u) != 0)
						{
							typoOffset = num8;
							num7 = (float)(outlineTextMetricsCached.otmAscent - outlineTextMetricsCached.otmDescent + outlineTextMetricsCached.otmLineGap) / MaxFontSize * array[num6].FontBase.SizeInPoints;
							num8 = (double)((float)outlineTextMetricsCached.otmAscent / MaxFontSize * array[num6].FontBase.SizeInPoints) * scale;
							descend = (double)((float)(-outlineTextMetricsCached.otmDescent) / MaxFontSize * array[num6].FontBase.SizeInPoints) * scale;
							typoOffset = num8 - typoOffset;
						}
						double num9 = (double)num7 * scale;
						double num10 = num9 * lineSpacing;
						float sizeInPoints = array[num6].FontBase.SizeInPoints;
						array[num6].LineHeight = num9;
						array[num6].Ascend = num8;
						array[num6].Descend = descend;
						array[num6].EmValue = sizeInPoints;
						array[num6].TypoOffset = typoOffset;
					}
					for (int num11 = 0; num11 < array.Length; num11++)
					{
						array[num11].hFont = array[num11].FontBase.ToHfont();
					}
					int num12 = 0;
					int num13 = 0;
					XFORM lpXform = new XFORM(1f, 0f, 0f, 1f, 0f, 0f);
					try
					{
						if ((angle > 45.0 && angle < 135.0) || (angle > 225.0 && angle < 315.0))
						{
							double width = sizeD.Width;
							sizeD.Width = sizeD.Height;
							sizeD.Height = width;
						}
						double num14 = (0.0 - angle) * Math.PI / 180.0;
						IntPtr zero = IntPtr.Zero;
						IntPtr intPtr = IntPtr.Zero;
						int num15 = 0;
						if (needDraw)
						{
							num12 = SetGraphicsMode(hdc, 2);
							if (num12 == 0)
							{
								ThrowError(3);
							}
							if (Compatibility2009)
							{
								float num16 = 1f;
								float num17 = 1f;
								if ((int)pageUnit != 1 && (int)pageUnit != 2)
								{
									num13 = SetMapMode(hdc, 8);
									if (num13 == 0)
									{
										ThrowError(4);
									}
									float num18 = clipBounds.Width + ((clipBounds.X > 0f) ? clipBounds.X : 0f);
									float num19 = clipBounds.Height + ((clipBounds.Y > 0f) ? clipBounds.Y : 0f);
									num16 = dpiX / 100f;
									num17 = dpiY / 100f;
									float num20 = num18 * num16;
									float num21 = num19 * num17;
									SIZE lpSize = default(SIZE);
									if (!SetWindowExtEx(hdc, (int)num18, (int)num19, out lpSize))
									{
										ThrowError(5);
									}
									SIZE lpSize2 = default(SIZE);
									if (!SetViewportExtEx(hdc, (int)num20, (int)num21, out lpSize2))
									{
										ThrowError(6);
									}
								}
								if (!GetWorldTransform(hdc, out lpXform))
								{
									ThrowError(7);
								}
								XFORM lpXform2 = new XFORM(1.0, 0.0, 0.0, 1.0, rectangleD2.Left + rectangleD2.Width / 2.0, rectangleD2.Top + rectangleD2.Height / 2.0);
								if (!ModifyWorldTransform(hdc, ref lpXform2, 2u))
								{
									ThrowError(8);
								}
								XFORM lpXform3 = new XFORM(Math.Cos(num14), Math.Sin(num14), 0.0 - Math.Sin(num14), Math.Cos(num14), 0.0, 0.0);
								if (!ModifyWorldTransform(hdc, ref lpXform3, 2u))
								{
									ThrowError(9);
								}
								XFORM lpXform4 = new XFORM(1.0, 0.0, 0.0, 1.0, (0.0 - sizeD.Width) / 2.0, (0.0 - sizeD.Height) / 2.0);
								if (!ModifyWorldTransform(hdc, ref lpXform4, 2u))
								{
									ThrowError(10);
								}
								if (!foreColor.IsEmpty)
								{
									SetTextColor(hdc, ColorToWin32(foreColor));
								}
								DeviceContextBackgroundMode deviceContextBackgroundMode = ((backColor.IsEmpty || backColor == Color.Transparent) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque);
								SetBkMode(hdc, (int)deviceContextBackgroundMode);
								if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent)
								{
									SetBkColor(hdc, ColorToWin32(backColor));
								}
								num15 = GetClipRgn(hdc, zero);
								intPtr = CreateRectRgn((int)(rectangleD.Left * (double)num16), (int)(rectangleD.Top * (double)num17), (int)(rectangleD.Right * (double)num16), (int)(rectangleD.Bottom * (double)num17));
								SelectClipRgn(hdc, intPtr);
							}
						}
						SelectObject(hdc, array[0].hFont);
						RECT lprc = new RECT(rectangleD2.ToRectangle());
						List<LineInfo> list6 = new List<LineInfo>();
						int num22 = 0;
						while (num22 < text.Length)
						{
							LineInfo lineInfo = new LineInfo();
							lineInfo.Begin = num22;
							for (; num22 < text.Length && text[num22] != '\r' && text[num22] != '\n'; num22++)
							{
							}
							lineInfo.End = num22;
							while (lineInfo.End > lineInfo.Begin + 1 && char.IsWhiteSpace(text[lineInfo.End - 1]))
							{
								lineInfo.End--;
							}
							list6.Add(lineInfo);
							num22++;
							if (num22 < text.Length && (text[num22] == '\r' || text[num22] == '\n') && text[num22 - 1] != text[num22])
							{
								num22++;
							}
						}
						if (array2.Length != 0 && array2[0].TS.Indent < 0 && list6.Count > 0)
						{
							list6[0].Indent = -array2[0].TS.Indent;
							if (array2[0].ListLevels != null)
							{
								array2[0].TS.Indent = array2[0].ListLevels.Count;
							}
						}
						RECT lpRect = new RECT(0, 0, int.MaxValue, int.MaxValue);
						TextFormatFlags nFormat = TextFormatFlags.CalculateRectangle;
						DrawTextExW(hdc, "…", 1, ref lpRect, (int)nFormat, new DRAWTEXTPARAMS());
						int num23 = (int)((double)lpRect.Size.Width * scale);
						double num24 = 1.0;
						double num25 = (sizeD.Height - array[0].LineHeight) / (array[0].LineHeight * lineSpacing) + 1.0;
						int num26 = (int)num25;
						if (!lineLimit)
						{
							if (allowHtmlTags)
							{
								num26 = list6.Count;
							}
							else
							{
								num26++;
							}
						}
						int num27 = num26;
						List<LineInfo> list7 = new List<LineInfo>();
						int num28 = 0;
						for (int num29 = 0; num29 < list6.Count; num29++)
						{
							LineInfo lineInfo2 = list6[num29];
							string text5 = text.Substring(lineInfo2.Begin, lineInfo2.Length);
							int num30 = text5.Length;
							int indent = array2[array3[lineInfo2.Begin]].TS.Indent;
							int num31 = indent * 10;
							int num32 = 0;
							int num33 = 0;
							bool flag3 = num31 > 0;
							bool flag4 = false;
							if (lineInfo2.Indent > 0)
							{
								num33 = lineInfo2.Indent;
								num32 = lineInfo2.Indent;
								flag4 = true;
							}
							if (num30 == 0)
							{
								LineInfo lineInfo3 = new LineInfo();
								LineInfo lineInfo4 = lineInfo3;
								lineInfo4.Begin += lineInfo2.Begin;
								list7.Add(lineInfo3);
								num28 = list7.Count;
							}
							else
							{
								if (num30 <= 0)
								{
									continue;
								}
								int[] array5 = new int[num30];
								int num34 = 0;
								while (num34 < num30)
								{
									for (; num34 < num30; num34++)
									{
										UnicodeCategory unicodeCategory = char.GetUnicodeCategory(text5[num34]);
										if ((unicodeCategory == UnicodeCategory.OpenPunctuation || unicodeCategory == UnicodeCategory.ClosePunctuation || unicodeCategory == UnicodeCategory.InitialQuotePunctuation || unicodeCategory == UnicodeCategory.FinalQuotePunctuation) && text5[num34] != '’')
										{
											break;
										}
									}
									if (num34 >= num30)
									{
										continue;
									}
									int num35 = num34;
									int num36 = num34;
									if (char.GetUnicodeCategory(text5[num34]) == UnicodeCategory.OpenPunctuation || char.GetUnicodeCategory(text5[num34]) == UnicodeCategory.InitialQuotePunctuation)
									{
										for (; num34 < num30; num34++)
										{
											UnicodeCategory unicodeCategory2 = char.GetUnicodeCategory(text5[num34]);
											if (unicodeCategory2 != UnicodeCategory.OpenPunctuation && unicodeCategory2 != UnicodeCategory.InitialQuotePunctuation && unicodeCategory2 != UnicodeCategory.SpaceSeparator)
											{
												break;
											}
										}
										if (num34 < num30 && char.GetUnicodeCategory(text5[num34]) != UnicodeCategory.ClosePunctuation && char.GetUnicodeCategory(text5[num34]) != UnicodeCategory.FinalQuotePunctuation)
										{
											for (; num34 < num30; num34++)
											{
												UnicodeCategory unicodeCategory3 = char.GetUnicodeCategory(text5[num34]);
												if (unicodeCategory3 == UnicodeCategory.SpaceSeparator || isWordWrapSymbol2(text5, num34) || isCJKWordWrap(text5, num34))
												{
													break;
												}
											}
											for (; num34 < num30; num34++)
											{
												UnicodeCategory unicodeCategory4 = char.GetUnicodeCategory(text5[num34]);
												if (unicodeCategory4 != UnicodeCategory.SpaceSeparator)
												{
													break;
												}
											}
											if (num34 < num30 && char.GetUnicodeCategory(text5[num34]) == UnicodeCategory.DashPunctuation)
											{
												num34++;
											}
										}
										if (num34 < num30 && (char.GetUnicodeCategory(text5[num34]) == UnicodeCategory.ClosePunctuation || char.GetUnicodeCategory(text5[num34]) == UnicodeCategory.FinalQuotePunctuation))
										{
											for (; num34 < num30; num34++)
											{
												UnicodeCategory unicodeCategory5 = char.GetUnicodeCategory(text5[num34]);
												if (unicodeCategory5 != UnicodeCategory.ClosePunctuation && unicodeCategory5 != UnicodeCategory.FinalQuotePunctuation && unicodeCategory5 != UnicodeCategory.OtherPunctuation && unicodeCategory5 != UnicodeCategory.SpaceSeparator)
												{
													break;
												}
											}
										}
										num36 = num34;
									}
									else
									{
										num34--;
										for (; num34 >= 0; num34--)
										{
											UnicodeCategory unicodeCategory6 = char.GetUnicodeCategory(text5[num34]);
											if (unicodeCategory6 != UnicodeCategory.SpaceSeparator)
											{
												break;
											}
										}
										for (; num34 >= 0; num34--)
										{
											UnicodeCategory unicodeCategory7 = char.GetUnicodeCategory(text5[num34]);
											if (unicodeCategory7 == UnicodeCategory.SpaceSeparator || array5[num34] != 0 || isCJKWordWrap(text5, num34))
											{
												break;
											}
										}
										num35 = num34 + ((!isCJKWordWrap(text5, num34)) ? 1 : 0);
										for (num34 = num36; num34 < num30; num34++)
										{
											UnicodeCategory unicodeCategory8 = char.GetUnicodeCategory(text5[num34]);
											if (unicodeCategory8 != UnicodeCategory.ClosePunctuation && unicodeCategory8 != UnicodeCategory.FinalQuotePunctuation && unicodeCategory8 != UnicodeCategory.OtherPunctuation && unicodeCategory8 != UnicodeCategory.SpaceSeparator)
											{
												break;
											}
										}
										num36 = num34;
									}
									for (; num36 > num35 && char.GetUnicodeCategory(text5[num36 - 1]) == UnicodeCategory.SpaceSeparator; num36--)
									{
									}
									array5[num35] = 2;
									for (int num37 = num35 + 1; num37 < num36; num37++)
									{
										array5[num37] = 1;
									}
								}
								bool flag5 = false;
								for (int num38 = 0; num38 < num30; num38++)
								{
									if (text5[num38] != '‑' && text5[num38] != '\u00a0')
									{
										continue;
									}
									flag5 = true;
									array5[num38] = 1;
									int num39 = num38;
									bool flag6 = true;
									while (num39 > 0 && isNotWordWrapSymbol2(text5, num39 - 1))
									{
										num39--;
										if (array5[num39] == 0)
										{
											array5[num39] = 1;
											continue;
										}
										flag6 = false;
										break;
									}
									if (flag6)
									{
										array5[num39] = 2;
									}
									num39 = num38;
									while (num39 + 1 < num30 && (isNotWordWrapSymbol2(text5, num39 + 1) || text5[num39 + 1] == '‑' || text5[num39 + 1] == '\u00a0'))
									{
										num39++;
										array5[num39] = 1;
									}
									num38 = num39;
								}
								if (flag5)
								{
									text5 = text5.Replace('‑', '-');
								}
								for (int num40 = 0; num40 < num30; num40++)
								{
									if (!isNotWordWrapSymbol(text5, num40))
									{
										continue;
									}
									if (num40 > 0 && isNotWordWrapSymbol2(text5, num40 - 1))
									{
										array5[num40] = 1;
										int num41 = num40;
										if (!isWordWrapSymbol2(text5, num41) && !isCJKSymbol(text5, num41))
										{
											num41++;
											for (; num41 < text5.Length && char.IsLetterOrDigit(text5[num41]) && !isCJKSymbol(text5, num41) && array5[num41] == 0; num41++)
											{
												array5[num41] = 1;
											}
										}
										num41 = num40;
										bool flag7 = false;
										bool flag8 = true;
										while (num41 > 0 && isNotWordWrapSymbol2(text5, num41 - 1) && !isWordWrapSymbol2(text5, num41 - 1) && flag8)
										{
											flag7 = true;
											num41--;
											if (array5[num41] == 0)
											{
												array5[num41] = 1;
												flag8 = !isCJKSymbol(text5, num41);
												continue;
											}
											flag7 = false;
											break;
										}
										if (flag7)
										{
											array5[num41] = 2;
										}
									}
									else
									{
										if (num40 >= text5.Length - 1 || !isNotWordWrapSymbol2(text5, num40 + 1))
										{
											continue;
										}
										bool flag9 = array5[num40] == 0;
										int num42 = num40;
										if (!isWordWrapSymbol2(text5, num42))
										{
											num42++;
											for (; num42 < text5.Length && char.IsLetterOrDigit(text5[num42]) && array5[num42] == 0; num42++)
											{
												array5[num42] = 1;
											}
										}
										if (flag9)
										{
											array5[num40] = 2;
										}
									}
								}
								int[] array6 = new int[num30];
								SCRIPT_ITEM[] array7 = null;
								int scriptItemCount = 0;
								int num43 = 0;
								int num44 = 10;
								do
								{
									num44 *= 10;
									SCRIPT_CONTROL psControls = default(SCRIPT_CONTROL);
									SCRIPT_STATE psState = default(SCRIPT_STATE);
									psState.uBidiLevel = (ushort)(rightToLeft ? 1u : 0u);
									IntPtr intPtr2 = Marshal.AllocHGlobal(sizeofScriptItem * (num44 + 1) + 2);
									num43 = ScriptItemize(text5, text5.Length, num44, ref psControls, ref psState, intPtr2, out scriptItemCount);
									if (num43 != 0 && num43 != -2147024882)
									{
										Marshal.FreeHGlobal(intPtr2);
										ThrowError(11, num43);
									}
									if (num43 == 0)
									{
										array7 = new SCRIPT_ITEM[scriptItemCount + 1];
										IntPtr intPtr3 = intPtr2;
										for (int num45 = 0; num45 < scriptItemCount + 1; num45++)
										{
											array7[num45] = (SCRIPT_ITEM)Marshal.PtrToStructure(intPtr3, typeof(SCRIPT_ITEM));
											intPtr3 = (IntPtr)((long)intPtr3 + sizeofScriptItem);
										}
									}
									Marshal.FreeHGlobal(intPtr2);
								}
								while (num43 == -2147024882);
								List<SCRIPT_ITEM> list8 = new List<SCRIPT_ITEM>(array7);
								for (int num46 = array7.Length - 2; num46 > 0; num46--)
								{
									int iCharPos = array7[num46].iCharPos;
									if ((iCharPos == array7[num46 + 1].iCharPos - 1 && text5[iCharPos] == '-' && char.IsLetter(text5, iCharPos - 1)) || (char.IsDigit(text5, iCharPos) && char.IsLetter(text5, iCharPos - 1)))
									{
										list8.RemoveAt(num46);
									}
								}
								for (int num47 = array7.Length - 3; num47 > 0; num47--)
								{
									int iCharPos2 = array7[num47].iCharPos;
									if (iCharPos2 == array7[num47 + 1].iCharPos - 1 && text5[iCharPos2] == '’' && char.IsLetter(text5, iCharPos2 - 1) && char.IsLetter(text5, iCharPos2 + 1))
									{
										list8.RemoveAt(num47);
										list8.RemoveAt(num47);
									}
								}
								if (list8.Count < array7.Length)
								{
									array7 = new SCRIPT_ITEM[list8.Count];
									for (int num48 = 0; num48 < list8.Count; num48++)
									{
										array7[num48] = list8[num48];
									}
									scriptItemCount = array7.Length - 1;
								}
								list8.Clear();
								List<SCRIPT_ITEM> list9 = new List<SCRIPT_ITEM>();
								list9.Add(array7[0]);
								int num49 = 0;
								for (int num50 = 0; num50 < num30; num50++)
								{
									if (num50 == array7[num49 + 1].iCharPos)
									{
										num49++;
										list9.Add(array7[num49]);
									}
									else if (array5[num50] == 2 && num50 != 0)
									{
										SCRIPT_ITEM item = array7[num49];
										item.iCharPos = num50;
										list9.Add(item);
									}
									else if (num50 > 0 && array3[lineInfo2.Begin + num50] != array3[lineInfo2.Begin + num50 - 1])
									{
										SCRIPT_ITEM item2 = array7[num49];
										item2.iCharPos = num50;
										list9.Add(item2);
									}
								}
								list9.Add(array7[scriptItemCount]);
								array7 = new SCRIPT_ITEM[list9.Count];
								for (int num51 = 0; num51 < list9.Count; num51++)
								{
									array7[num51] = list9[num51];
								}
								scriptItemCount = list9.Count - 1;
								int num52 = num30 * 2;
								if (num52 < 20)
								{
									num52 = 20;
								}
								int num53 = 0;
								int num54 = 0;
								int num55 = 0;
								int num56 = 0;
								int num57 = -1;
								double num58 = 0.0;
								for (int num59 = 0; num59 < scriptItemCount; num59++)
								{
									ushort[] array8 = new ushort[num52];
									ushort[] array9 = new ushort[num30];
									IntPtr intPtr4 = Marshal.AllocHGlobal(sizeofScriptVisattr * num52);
									int[] array10 = new int[num52];
									int iCharPos3 = array7[num59].iCharPos;
									int num60 = array7[num59 + 1].iCharPos - iCharPos3;
									string text6 = text5.Substring(iCharPos3, num60);
									num3 = array3[lineInfo2.Begin + iCharPos3];
									StiFontState stiFontState = array[array2[num3].FontIndex];
									SelectObject(hdc, stiFontState.hFont);
									if (array5[iCharPos3] == 2 && num59 != num56)
									{
										num55 = num59;
									}
									num43 = ScriptShape(hdc, ref stiFontState.hScriptCache, text6, num60, num52, ref array7[num59].a, array8, array9, intPtr4, out var pcGlyphs);
									if (num43 == -2147220992)
									{
										num43 = 0;
										pcGlyphs = num60;
										for (int num61 = 0; num61 < pcGlyphs; num61++)
										{
											array9[num61] = (ushort)num61;
										}
										array7[num59].a.packed = 0;
									}
									if (num43 != 0 && num43 != -2147220992)
									{
										Marshal.FreeHGlobal(intPtr4);
										ThrowError(12, num43);
									}
									IntPtr intPtr5 = Marshal.AllocHGlobal(sizeofGoffset * pcGlyphs);
									num43 = ScriptPlace(hdc, ref stiFontState.hScriptCache, array8, pcGlyphs, intPtr4, ref array7[num59].a, array10, intPtr5, out var _);
									Marshal.FreeHGlobal(intPtr5);
									if (num43 != 0)
									{
										Marshal.FreeHGlobal(intPtr4);
										ThrowError(13, num43);
									}
									Marshal.FreeHGlobal(intPtr4);
									if (CorrectionEnabled && !Compatibility2009)
									{
										ushort[] fontWidth = GetFontWidth(stiFontState);
										if (fontWidth.Length != 0)
										{
											if (PrecisionMode3Enabled)
											{
												double num62 = (double)MaxFontSize / stiFontState.EmValue;
												double num63 = num58;
												int num64 = 0;
												for (int num65 = 0; num65 < pcGlyphs; num65++)
												{
													if (array10[num65] != 0)
													{
														int num66 = array8[num65];
														if (num66 >= fontWidth.Length)
														{
															num66 = fontWidth.Length - 1;
														}
														num63 += (double)(int)fontWidth[num66] / num62;
														int num67 = (int)(num63 - (double)num64);
														num64 += num67;
														array10[num65] = num67;
													}
												}
												if (num63 - (double)num64 > 0.5 && num59 < scriptItemCount - 1)
												{
													array10[pcGlyphs - 1]++;
													num64++;
												}
												num58 = num63 - (double)num64;
											}
											else if (PrecisionMode2Enabled)
											{
												double num68 = (double)MaxFontSize / stiFontState.EmValue;
												double num69 = num58;
												int num70 = 0;
												for (int num71 = 0; num71 < pcGlyphs; num71++)
												{
													if (array10[num71] != 0)
													{
														int num72 = array8[num71];
														if (num72 >= fontWidth.Length)
														{
															num72 = fontWidth.Length - 1;
														}
														num69 += (double)(int)fontWidth[num72] / num68;
														int num73 = (int)(num69 - (double)num70);
														num70 += num73;
														array10[num71] = num73;
													}
												}
												if (num69 - (double)num70 > 0.5)
												{
													array10[pcGlyphs - 1]++;
													num70++;
												}
												num58 = num69 - (double)num70;
											}
											else
											{
												double num74 = (double)MaxFontSize / stiFontState.EmValue;
												for (int num75 = 0; num75 < pcGlyphs; num75++)
												{
													if (array10[num75] == 0)
													{
														continue;
													}
													int num76 = array8[num75];
													if (num76 >= fontWidth.Length)
													{
														num76 = fontWidth.Length - 1;
													}
													double num77 = (double)(int)fontWidth[num76] / num74;
													if ((double)array10[num75] < num77 - 0.4)
													{
														int num78 = (int)Math.Round(num77);
														if (array10[num75] >= num78)
														{
															num78++;
														}
														array10[num75] = num78;
													}
													else
													{
														double num79 = (double)array10[num75] / num77;
														if ((double)array10[num75] - num77 > 1.0 && num79 > 1.1)
														{
															array10[num75] = (int)Math.Ceiling(num77);
														}
													}
												}
											}
										}
									}
									if (array2[num3].TS.LetterSpacing != 0.0)
									{
										double num80 = stiFontState.EmValue * 1.35 * array2[num3].TS.LetterSpacing;
										int num81 = 0;
										double num82 = 0.0;
										for (int num83 = 0; num83 < pcGlyphs; num83++)
										{
											num82 += (double)array10[num83] + num80;
											int num84 = (int)num82 - num81;
											num81 += num84;
											array10[num83] = num84;
										}
									}
									if (array2[num3].TS.WordSpacing != 0.0)
									{
										double num85 = stiFontState.EmValue * array2[num3].TS.WordSpacing;
										int num86 = 0;
										double num87 = 0.0;
										for (int num88 = 0; num88 < num60; num88++)
										{
											if (char.IsWhiteSpace(text6[num88]))
											{
												num87 += (double)array10[array9[num88]] + num85;
												int num89 = (int)num87 - num86;
												num86 += num89;
												array10[array9[num88]] += num89;
											}
										}
									}
									int num90 = 0;
									for (int num91 = 0; num91 < num60; num91++)
									{
										if (text6[num91] == '\u00ad')
										{
											num57 = array10[num91];
											array10[num91] = 0;
										}
										int num92 = 0;
										if (text6[num91] == '\t')
										{
											num92 = GetTabsWidth(textOptions, num24, num53 + num90);
										}
										else
										{
											int num93;
											for (num93 = array9[num91]; num91 < num60 - 1 && array9[num91 + 1] == num93; num91++)
											{
											}
											int num94 = num93;
											int num95 = num93;
											if (array7[num59].a.fRTL)
											{
												num94 = ((num91 + 1 < num60) ? (array9[num91 + 1] + 1) : 0);
												num95++;
											}
											else
											{
												num95 = ((num91 + 1 < num60) ? array9[num91 + 1] : pcGlyphs);
											}
											for (int num96 = num94; num96 < num95; num96++)
											{
												num92 += array10[num96];
											}
										}
										num90 += num92;
										array6[iCharPos3 + num91] = num92;
										if (flag3 && iCharPos3 + num91 + 1 == num31 && !flag4)
										{
											num32 = num53 + num90;
										}
									}
									double num97 = 2147483647.0;
									bool flag10 = false;
									if ((int)trimming != 0)
									{
										num97 = sizeD.Width;
										if (!wordWrap || list7.Count + 1 == num27)
										{
											flag10 = true;
										}
										if (((int)trimming == 3 || (int)trimming == 4) && flag10)
										{
											num97 = sizeD.Width - (double)num23;
										}
									}
									else if (wordWrap)
									{
										num97 = sizeD.Width - (double)num33;
									}
									if (Math.Round((decimal)((double)(num53 + num90) * scale), 5) <= (decimal)num97)
									{
										num53 += num90;
										continue;
									}
									IntPtr intPtr6 = Marshal.AllocHGlobal(num60 + 2);
									num43 = ScriptBreak(text6, num60, ref array7[num59].a, intPtr6);
									if (num43 != 0)
									{
										Marshal.FreeHGlobal(intPtr6);
										ThrowError(14, num43);
									}
									byte[] array11 = new byte[num60];
									Marshal.Copy(intPtr6, array11, 0, num60);
									Marshal.FreeHGlobal(intPtr6);
									SCRIPT_LOGATTR[] array12 = new SCRIPT_LOGATTR[num60];
									for (int num98 = 0; num98 < num60; num98++)
									{
										array12[num98].packed = array11[num98];
										char c = text6[num98];
										if (c == '(' || c == '{')
										{
											array12[num98].fSoftBreak = true;
										}
										if (num98 > 0)
										{
											c = text6[num98 - 1];
											if (c == '!' || c == '%' || c == ')' || c == '}' || c == '-' || c == '?')
											{
												array12[num98].fSoftBreak = true;
											}
											if (text6[num98] == '\u00b4' && char.IsLetterOrDigit(c))
											{
												array12[num98].fSoftBreak = false;
											}
										}
									}
									int num99 = 0;
									int num100 = 0;
									int num101 = 0;
									int num102 = 0;
									for (int num103 = 0; num103 < num60; num103++)
									{
										num97 = 2147483647.0;
										flag10 = false;
										if ((int)trimming != 0)
										{
											num97 = sizeD.Width;
											if (!wordWrap || list7.Count + 1 == num27)
											{
												flag10 = true;
											}
											if (((int)trimming == 3 || (int)trimming == 4) && flag10)
											{
												num97 = sizeD.Width - (double)num23;
											}
										}
										else if (wordWrap)
										{
											num97 = sizeD.Width - (double)num33;
										}
										if (array12[num103].fSoftBreak && (num103 <= 0 || text6[num103 - 1] != '\u00ad' || !(Math.Round((decimal)((double)(num53 + num100 + num57) * scale), 5) > (decimal)num97)))
										{
											num100 = num99;
											num101 = num103;
										}
										if (text6[num103] == '\t')
										{
											int tabsWidth = GetTabsWidth(textOptions, num24, num53 + num99);
											array6[iCharPos3 + num103] = tabsWidth;
										}
										for (int num104 = array9[num103]; num103 < num60 - 1 && array9[num103 + 1] == num104; num103++)
										{
										}
										num99 += array6[iCharPos3 + num103];
										if (!array12[num103].fWhiteSpace)
										{
											num100 = num99;
										}
										if (!(Math.Round((decimal)((double)(num53 + num100) * scale), 5) > (decimal)num97))
										{
											continue;
										}
										if (flag10)
										{
											if ((int)trimming == 1 || (int)trimming == 3)
											{
												num30 = iCharPos3 + num103;
												if (num30 == 0)
												{
													num30++;
												}
											}
											if ((int)trimming == 2 || (int)trimming == 4)
											{
												num30 = iCharPos3 + num101;
												if (num30 == 0)
												{
													num30 = iCharPos3 + num103;
												}
												if (num30 == 0)
												{
													num30++;
												}
											}
											num59 = scriptItemCount;
											if (num54 < num30)
											{
												LineInfo lineInfo5 = new LineInfo();
												lineInfo5.Begin = num54;
												lineInfo5.End = num30;
												lineInfo5.NeedWidthAlign = false;
												if ((int)trimming == 3 || (int)trimming == 4)
												{
													lineInfo5.Text = text5.Substring(lineInfo5.Begin, lineInfo5.Length) + "…";
												}
												list7.Add(lineInfo5);
												num54 = num30;
											}
											break;
										}
										bool flag11 = false;
										if (array5[iCharPos3 + num103] == 1)
										{
											int num105;
											for (num105 = iCharPos3 + num103 - 1; array5[num105] == 1; num105--)
											{
											}
											if (num105 == num54)
											{
												flag11 = true;
											}
										}
										if ((num101 == num102 && num53 == 0) || flag11)
										{
											num102 = num103;
											if (num102 == 0 && !flag11)
											{
												num102++;
											}
											if (iCharPos3 + num102 - num54 == 0)
											{
												num102++;
											}
											if (num102 > 0 && text6[num102 - 1] == '\u00ad')
											{
												num102--;
											}
										}
										else
										{
											num102 = num101;
										}
										LineInfo lineInfo6 = new LineInfo();
										lineInfo6.Begin = num54;
										lineInfo6.End = iCharPos3 + num102;
										lineInfo6.NeedWidthAlign = true;
										list7.Add(lineInfo6);
										if (flag4)
										{
											lineInfo6.Indent = num32;
										}
										if (lineInfo6.End - 1 > 0 && text[lineInfo6.End - 1] == '\u00ad' && num57 >= 0)
										{
											array6[lineInfo6.End - 1] = num57;
										}
										num33 = num32;
										flag4 = true;
										while (lineInfo6.End > lineInfo6.Begin + 1 && char.IsWhiteSpace(text5[lineInfo6.End - 1]))
										{
											lineInfo6.End--;
										}
										for (; num102 < num60 - 1 && (char.IsWhiteSpace(text5[iCharPos3 + num102]) || text5[iCharPos3 + num102] == '\u00ad'); num102++)
										{
										}
										num99 = 0;
										num100 = 0;
										num53 = 0;
										num101 = num102;
										num54 = iCharPos3 + num102;
										num103 = num102 - 1;
										if (num54 < num30 && array5[num54] == 1 && num55 != 0)
										{
											num54 = (lineInfo6.End = array7[num55].iCharPos);
											num59 = num55 - 1;
											num56 = num55;
											num55 = 0;
											while (lineInfo6.End > lineInfo6.Begin + 1 && char.IsWhiteSpace(text5[lineInfo6.End - 1]))
											{
												lineInfo6.End--;
											}
											break;
										}
										num55 = 0;
										num56 = 0;
									}
									num53 += num99;
									num58 = 0.0;
								}
								if (num54 < num30)
								{
									LineInfo lineInfo7 = new LineInfo();
									lineInfo7.Begin = num54;
									lineInfo7.End = num30;
									lineInfo7.NeedWidthAlign = false;
									if (((int)trimming == 3 || (int)trimming == 4) && list7.Count + 1 == num27 && num29 + 1 < list6.Count)
									{
										lineInfo7.Text = text5.Substring(lineInfo7.Begin, lineInfo7.Length) + "…";
									}
									if (flag4)
									{
										lineInfo7.Indent = num32;
									}
									list7.Add(lineInfo7);
								}
								for (int num106 = num28; num106 < list7.Count; num106++)
								{
									LineInfo lineInfo8 = list7[num106];
									int num107 = 0;
									int num108 = 0;
									for (int begin = lineInfo8.Begin; begin < lineInfo8.End; begin++)
									{
										num107 += array6[begin];
										char c2 = text[lineInfo2.Begin + begin];
										if (char.IsWhiteSpace(c2) && c2 != '\u00a0')
										{
											num108++;
										}
									}
									if (lineInfo8.Text != null)
									{
										num107 += num23;
									}
									lineInfo8.Width = (int)((double)num107 * scale);
									LineInfo lineInfo4 = lineInfo8;
									lineInfo4.Begin += lineInfo2.Begin;
									if (num108 > 0)
									{
										double num109 = (sizeD.Width - (double)lineInfo8.Width - (double)lineInfo8.Indent) / (double)num108;
										if (num109 > 0.0)
										{
											lineInfo8.JustifyOffset = num109;
										}
										else
										{
											lineInfo8.NeedWidthAlign = false;
										}
									}
								}
								num28 = list7.Count;
							}
						}
						list6 = list7;
						if (!lineLimit && (int)trimming == 0)
						{
							num27 = list6.Count;
						}
						int num110 = 0;
						double num111 = 0.0;
						num26 = 0;
						for (int num112 = 0; num112 < list6.Count; num112++)
						{
							LineInfo lineInfo9 = list6[num112];
							if (num110 < lineInfo9.Width)
							{
								num110 = lineInfo9.Width;
							}
							int begin2 = lineInfo9.Begin;
							if (begin2 > 0 && array3[begin2] - array3[begin2 - 1] == 2)
							{
								begin2--;
							}
							int num113 = array2[array3[lineInfo9.Begin]].FontIndex;
							if (lineInfo9.End - lineInfo9.Begin > 0)
							{
								int num114 = ((begin2 + 1 < array3.Length) ? array3[begin2 + 1] : array3[^1]);
								int num115 = ((lineInfo9.End < array3.Length) ? array3[lineInfo9.End] : array3[^1]);
								for (int num116 = num114; num116 <= num115; num116++)
								{
									int fontIndex = array2[num116].FontIndex;
									if (array[num113].FontBase.Size < array[fontIndex].FontBase.Size)
									{
										num113 = fontIndex;
									}
									else if (array[num113].LineHeight < array[fontIndex].LineHeight)
									{
										num113 = fontIndex;
									}
								}
							}
							lineInfo9.IndexOfMaxFont = num113;
							double num117 = 1.0;
							if (num112 != list6.Count - 1)
							{
								LineInfo lineInfo10 = list6[num112 + 1];
								int begin3 = lineInfo10.Begin;
								if (begin3 > 0)
								{
									begin3--;
								}
								num117 = array2[array3[begin3]].TS.LineHeight;
							}
							lineInfo9.LineHeight = array[num113].LineHeight * num117;
							num111 += lineInfo9.LineHeight;
							if (num111 < rectangleD2.Height || num111 - lineInfo9.LineHeight + array[num113].LineHeight < rectangleD2.Height)
							{
								num26++;
							}
							lineInfo9.TextAlignment = array2[array3[lineInfo9.End - ((lineInfo9.Length > 0) ? 1 : 0)]].TS.TextAlign;
						}
						if (lineLimit)
						{
							num27 = num26;
						}
						measureSize.Width = num110;
						measureSize.Height = (int)Math.Round((decimal)num111);
						if (list6.Count == 0)
						{
							measureSize.Height = 0.0;
						}
						if (OptimizeBottomMargin)
						{
							if (list6.Count == 1)
							{
								measureSize.Height += array[0].LineHeight * 0.07;
							}
							else if (list6.Count == 2)
							{
								measureSize.Height += array[0].LineHeight * 0.085;
							}
							else if (list6.Count > 2)
							{
								measureSize.Height += array[0].LineHeight * 0.1;
							}
						}
						else
						{
							if (list6.Count == 1)
							{
								measureSize.Height += array[0].LineHeight * 0.1;
							}
							if (list6.Count > 1)
							{
								measureSize.Height += array[0].LineHeight * 0.4;
							}
						}
						if ((angle > 45.0 && angle < 135.0) || (angle > 225.0 && angle < 315.0))
						{
							double width2 = measureSize.Width;
							measureSize.Width = measureSize.Height;
							measureSize.Height = width2;
							if (measureSize.Height > rectangleD2.Height && wordWrap)
							{
								measureSize.Height = rectangleD2.Height;
							}
							if (measureSize.Width > rectangleD2.Width)
							{
								measureSize.Width = rectangleD2.Width;
							}
						}
						else
						{
							if (measureSize.Width > rectangleD2.Width && wordWrap)
							{
								measureSize.Width = rectangleD2.Width;
							}
							if (measureSize.Height > rectangleD2.Height)
							{
								measureSize.Height = rectangleD2.Height;
							}
						}
						measureSize.Width += num * 2.0 * num2;
						if (textLinesArray != null)
						{
							for (int num118 = 0; num118 < list6.Count; num118++)
							{
								LineInfo lineInfo11 = list6[num118];
								string text7 = lineInfo11.Text;
								if (text7 == null)
								{
									text7 = text.Substring(lineInfo11.Begin, lineInfo11.Length);
								}
								bool flag12 = text7.Length > 0 && text7[text7.Length - 1] == '\u00ad';
								text7 = text7.Replace("\u00ad", "") + (flag12 ? "-" : "");
								if (lineInfo11.Indent > 0)
								{
									text7 = GetIndentString(lineInfo11.Indent) + text7;
								}
								textLinesArray.Add(text7);
								textLinesInfo?.Add(lineInfo11);
							}
						}
						if (needDraw)
						{
							if (!Compatibility2009)
							{
								float num119 = 1f;
								float num120 = 1f;
								if ((int)pageUnit != 1 && (int)pageUnit != 2)
								{
									num13 = SetMapMode(hdc, 8);
									if (num13 == 0)
									{
										ThrowError(4);
									}
									float num121 = clipBounds.Width + ((clipBounds.X > 0f) ? clipBounds.X : 0f);
									float num122 = clipBounds.Height + ((clipBounds.Y > 0f) ? clipBounds.Y : 0f);
									num119 = dpiX / 100f;
									num120 = dpiY / 100f;
									if ((double)pageScale != 0.01)
									{
										num119 *= pageScale / 0.01f;
										num120 *= pageScale / 0.01f;
									}
									float num123 = num121 * num119;
									float num124 = num122 * num120;
									SIZE lpSize3 = default(SIZE);
									if (!SetWindowExtEx(hdc, (int)num121, (int)num122, out lpSize3))
									{
										ThrowError(5);
									}
									SIZE lpSize4 = default(SIZE);
									if (!SetViewportExtEx(hdc, (int)num123, (int)num124, out lpSize4))
									{
										ThrowError(6);
									}
								}
								if (!GetWorldTransform(hdc, out lpXform))
								{
									ThrowError(7);
								}
								XFORM lpXform5 = new XFORM(1.0, 0.0, 0.0, 1.0, rectangleD2.Left + rectangleD2.Width / 2.0, rectangleD2.Top + rectangleD2.Height / 2.0);
								if (!ModifyWorldTransform(hdc, ref lpXform5, 2u))
								{
									ThrowError(8);
								}
								XFORM lpXform6 = new XFORM(Math.Cos(num14), Math.Sin(num14), 0.0 - Math.Sin(num14), Math.Cos(num14), 0.0, 0.0);
								if (!ModifyWorldTransform(hdc, ref lpXform6, 2u))
								{
									ThrowError(9);
								}
								XFORM lpXform7 = new XFORM(1.0, 0.0, 0.0, 1.0, (0.0 - sizeD.Width) / 2.0, (0.0 - sizeD.Height) / 2.0);
								if (!ModifyWorldTransform(hdc, ref lpXform7, 2u))
								{
									ThrowError(10);
								}
								if (!foreColor.IsEmpty)
								{
									SetTextColor(hdc, ColorToWin32(foreColor));
								}
								DeviceContextBackgroundMode deviceContextBackgroundMode2 = ((backColor.IsEmpty || backColor == Color.Transparent) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque);
								SetBkMode(hdc, (int)deviceContextBackgroundMode2);
								if (deviceContextBackgroundMode2 != DeviceContextBackgroundMode.Transparent)
								{
									SetBkColor(hdc, ColorToWin32(backColor));
								}
								num15 = GetClipRgn(hdc, zero);
								intPtr = CreateRectRgn((int)(rectangleD.Left * (double)num119), (int)(rectangleD.Top * (double)num120), (int)(rectangleD.Right * (double)num119), (int)(rectangleD.Bottom * (double)num120));
								SelectClipRgn(hdc, intPtr);
							}
							int num125 = list6.Count;
							if (num125 > num27)
							{
								num125 = num27;
							}
							double num126 = 0.0;
							double num127 = 0.0;
							for (int num128 = 0; num128 < num125; num128++)
							{
								num127 += list6[num128].LineHeight;
							}
							num127 = (int)num127;
							StiVertAlignment stiVertAlignment = vertAlign;
							if (angle != 0.0 && angle != 90.0 && angle != 180.0 && angle != 270.0)
							{
								stiVertAlignment = StiVertAlignment.Center;
							}
							switch (stiVertAlignment)
							{
							case StiVertAlignment.Center:
								num126 += (sizeD.Height - num127) / 2.0;
								break;
							case StiVertAlignment.Bottom:
								num126 += sizeD.Height - num127;
								break;
							}
							decimal num129 = (decimal)num126;
							for (int num130 = 0; num130 < num125; num130++)
							{
								LineInfo lineInfo12 = list6[num130];
								if (lineInfo12.Length > 0)
								{
									string text8 = text.Substring(lineInfo12.Begin, lineInfo12.Length);
									if (lineInfo12.Text != null)
									{
										text8 = lineInfo12.Text;
									}
									if (text8.IndexOf('‑') != -1)
									{
										text8 = text8.Replace('‑', '-');
									}
									text8 = text8.Substring(0, text8.Length - 1).Replace('\u00ad', '\u200b') + text8[text8.Length - 1];
									double num131 = (double)lineInfo12.Indent * scale;
									double num132 = lineInfo12.Width;
									StiTextHorAlignment stiTextHorAlignment = lineInfo12.TextAlignment;
									if (rightToLeft)
									{
										if (lineInfo12.TextAlignment == StiTextHorAlignment.Left)
										{
											stiTextHorAlignment = StiTextHorAlignment.Right;
										}
										if (lineInfo12.TextAlignment == StiTextHorAlignment.Right)
										{
											stiTextHorAlignment = StiTextHorAlignment.Left;
										}
									}
									if (flag2 && num130 == list6.Count - 1)
									{
										lineInfo12.NeedWidthAlign = true;
									}
									if (stiTextHorAlignment == StiTextHorAlignment.Width && !lineInfo12.NeedWidthAlign)
									{
										stiTextHorAlignment = (rightToLeft ? StiTextHorAlignment.Right : StiTextHorAlignment.Left);
									}
									switch (stiTextHorAlignment)
									{
									case StiTextHorAlignment.Center:
										num131 += (sizeD.Width - num132) / 2.0;
										break;
									case StiTextHorAlignment.Right:
										num131 += sizeD.Width - num132;
										break;
									}
									SCRIPT_ITEM[] array13 = null;
									int scriptItemCount2 = 0;
									int num133 = 0;
									int num134 = 10;
									do
									{
										num134 *= 10;
										SCRIPT_CONTROL psControls2 = default(SCRIPT_CONTROL);
										SCRIPT_STATE psState2 = default(SCRIPT_STATE);
										psState2.uBidiLevel = (ushort)(rightToLeft ? 1u : 0u);
										IntPtr intPtr7 = Marshal.AllocHGlobal(sizeofScriptItem * num134 + 1);
										num133 = ScriptItemize(text8, text8.Length, num134, ref psControls2, ref psState2, intPtr7, out scriptItemCount2);
										if (num133 != 0 && num133 != -2147024882)
										{
											Marshal.FreeHGlobal(intPtr7);
											ThrowError(15, num133);
										}
										array13 = new SCRIPT_ITEM[scriptItemCount2 + 1];
										IntPtr intPtr8 = intPtr7;
										for (int num135 = 0; num135 < scriptItemCount2 + 1; num135++)
										{
											array13[num135] = (SCRIPT_ITEM)Marshal.PtrToStructure(intPtr8, typeof(SCRIPT_ITEM));
											intPtr8 = (IntPtr)((long)intPtr8 + sizeofScriptItem);
										}
										Marshal.FreeHGlobal(intPtr7);
									}
									while (num133 == -2147024882);
									int num136 = scriptItemCount2;
									List<SCRIPT_ITEM> list10 = new List<SCRIPT_ITEM>();
									list10.Add(array13[0]);
									int num137 = 0;
									for (int num138 = 0; num138 < text8.Length; num138++)
									{
										if (num138 == array13[num137 + 1].iCharPos)
										{
											num137++;
											list10.Add(array13[num137]);
										}
										else if (num138 > 0 && array3[lineInfo12.Begin + num138] != array3[lineInfo12.Begin + num138 - 1])
										{
											SCRIPT_ITEM item3 = array13[num137];
											item3.iCharPos = num138;
											list10.Add(item3);
										}
									}
									list10.Add(array13[scriptItemCount2]);
									array13 = new SCRIPT_ITEM[list10.Count];
									for (int num139 = 0; num139 < list10.Count; num139++)
									{
										array13[num139] = list10[num139];
									}
									scriptItemCount2 = list10.Count - 1;
									byte[] array14 = new byte[scriptItemCount2];
									int[] array15 = new int[scriptItemCount2];
									int[] piLogicalToVisual = new int[scriptItemCount2];
									for (int num140 = 0; num140 < scriptItemCount2; num140++)
									{
										array14[num140] = (byte)array13[num140].a.s.uBidiLevel;
									}
									num133 = ScriptLayout(scriptItemCount2, array14, array15, piLogicalToVisual);
									if (num133 != 0)
									{
										ThrowError(16, num133);
									}
									int length = text8.Length;
									int num141 = length * 2;
									if (num141 < 20)
									{
										num141 = 20;
									}
									int num142 = 0;
									double num143 = num131;
									double num144 = 0.0;
									double num145 = 0.0;
									for (int num146 = 0; num146 < scriptItemCount2; num146++)
									{
										ushort[] array16 = new ushort[num141];
										ushort[] array17 = new ushort[length];
										IntPtr intPtr9 = Marshal.AllocHGlobal(sizeofScriptVisattr * num141);
										int[] array18 = new int[num141];
										int num147 = array15[num146];
										int iCharPos5 = array13[num147].iCharPos;
										int num148 = array13[num147 + 1].iCharPos - iCharPos5;
										string text9 = text8.Substring(iCharPos5, num148);
										num3 = array3[lineInfo12.Begin + iCharPos5];
										StiFontState stiFontState2 = array[array2[num3].FontIndex];
										bool flag13 = stiFontState2.hFontScaled != IntPtr.Zero;
										if (!flag)
										{
											if (flag13)
											{
												SelectObject(hdc, stiFontState2.hFontScaled);
											}
											else
											{
												SelectObject(hdc, stiFontState2.hFont);
											}
										}
										if (!stiFontState2.HasNbspGlyf && text9.IndexOf('\u00a0') != -1)
										{
											text9 = text9.Replace('\u00a0', ' ');
										}
										double num149 = (double)num129;
										if (array2[num3].TS.Superscript || array2[num3].TS.Subscript)
										{
											StiFontState stiFontState3 = array[stiFontState2.ParentFontIndex];
											num149 += array[lineInfo12.IndexOfMaxFont].Ascend - stiFontState3.Ascend;
											if (array2[num3].TS.Subscript)
											{
												num149 += stiFontState3.LineHeight - (stiFontState2.Ascend + stiFontState2.Descend);
											}
										}
										else
										{
											num149 += array[lineInfo12.IndexOfMaxFont].Ascend - stiFontState2.Ascend;
										}
										if (!flag)
										{
											if (!array2[num3].TS.FontColor.IsEmpty)
											{
												SetTextColor(hdc, ColorToWin32(array2[num3].TS.FontColor));
											}
											DeviceContextBackgroundMode deviceContextBackgroundMode3 = ((array2[num3].TS.BackColor.IsEmpty || array2[num3].TS.BackColor == Color.Transparent) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque);
											SetBkMode(hdc, (int)deviceContextBackgroundMode3);
											if (deviceContextBackgroundMode3 != DeviceContextBackgroundMode.Transparent)
											{
												SetBkColor(hdc, ColorToWin32(array2[num3].TS.BackColor));
											}
										}
										IntPtr psc = (flag13 ? stiFontState2.hScriptCacheScaled : stiFontState2.hScriptCache);
										num133 = ScriptShape(hdc, ref psc, text9, num148, length * 2, ref array13[num147].a, array16, array17, intPtr9, out var pcGlyphs2);
										if (num133 == -2147220992)
										{
											num133 = 0;
											pcGlyphs2 = num148;
											for (int num150 = 0; num150 < pcGlyphs2; num150++)
											{
												array17[num150] = (ushort)num150;
											}
											array13[num146].a.packed = 0;
										}
										if (num133 != 0 && num133 != -2147220992)
										{
											Marshal.FreeHGlobal(intPtr9);
											ThrowError(17, num133);
										}
										int num151 = num133;
										IntPtr intPtr10 = Marshal.AllocHGlobal(sizeofGoffset * num141);
										int num152 = 0;
										ABC pABC2;
										if (flag13)
										{
											SelectObject(hdc, stiFontState2.hFont);
											num133 = ScriptPlace(hdc, ref stiFontState2.hScriptCache, array16, pcGlyphs2, intPtr9, ref array13[num147].a, array18, intPtr10, out pABC2);
											if (num133 != 0)
											{
												Marshal.FreeHGlobal(intPtr10);
												Marshal.FreeHGlobal(intPtr9);
												ThrowError(1801, num133);
											}
											SelectObject(hdc, stiFontState2.hFontScaled);
											if (CorrectionEnabled && !Compatibility2009)
											{
												ushort[] fontWidth2 = GetFontWidth(stiFontState2);
												if (fontWidth2.Length != 0)
												{
													if (PrecisionMode3Enabled)
													{
														double num153 = (double)MaxFontSize / stiFontState2.EmValue;
														double num154 = num144;
														int num155 = 0;
														for (int num156 = 0; num156 < pcGlyphs2; num156++)
														{
															if (array18[num156] != 0)
															{
																int num157 = array16[num156];
																if (num157 >= fontWidth2.Length)
																{
																	num157 = fontWidth2.Length - 1;
																}
																num154 += (double)(int)fontWidth2[num157] / num153;
																int num158 = (int)(num154 - (double)num155);
																num155 += num158;
																array18[num156] = num158;
															}
														}
														if (num154 - (double)num155 > 0.5)
														{
															array18[pcGlyphs2 - 1]++;
															num155++;
														}
														num144 = num154 - (double)num155;
													}
													else if (PrecisionMode2Enabled)
													{
														double num159 = (double)MaxFontSize / stiFontState2.EmValue;
														double num160 = num144;
														int num161 = 0;
														for (int num162 = 0; num162 < pcGlyphs2; num162++)
														{
															if (array18[num162] != 0)
															{
																int num163 = array16[num162];
																if (num163 >= fontWidth2.Length)
																{
																	num163 = fontWidth2.Length - 1;
																}
																num160 += (double)(int)fontWidth2[num163] / num159;
																int num164 = (int)(num160 - (double)num161);
																num161 += num164;
																array18[num162] = num164;
															}
														}
														if (num160 - (double)num161 > 0.5)
														{
															array18[pcGlyphs2 - 1]++;
															num161++;
														}
														num144 = num160 - (double)num161;
													}
													else
													{
														double num165 = (double)MaxFontSize / stiFontState2.EmValue;
														for (int num166 = 0; num166 < pcGlyphs2; num166++)
														{
															if (array18[num166] == 0)
															{
																continue;
															}
															int num167 = array16[num166];
															if (num167 >= fontWidth2.Length)
															{
																num167 = fontWidth2.Length - 1;
															}
															double num168 = (double)(int)fontWidth2[num167] / num165;
															if ((double)array18[num166] < num168 - 0.4)
															{
																int num169 = (int)Math.Round(num168);
																if (array18[num166] >= num169)
																{
																	num169++;
																}
																array18[num166] = num169;
															}
															else
															{
																double num170 = (double)array18[num166] / num168;
																if ((double)array18[num166] - num168 > 1.0 && num170 > 1.1)
																{
																	array18[num166] = (int)Math.Ceiling(num168);
																}
															}
														}
													}
												}
											}
											for (int num171 = 0; num171 < pcGlyphs2; num171++)
											{
												num152 += array18[num171];
											}
										}
										num133 = ScriptPlace(hdc, ref psc, array16, pcGlyphs2, intPtr9, ref array13[num147].a, array18, intPtr10, out pABC2);
										if (num133 != 0)
										{
											Marshal.FreeHGlobal(intPtr10);
											Marshal.FreeHGlobal(intPtr9);
											ThrowError(18, num133);
										}
										Marshal.FreeHGlobal(intPtr9);
										if (CorrectionEnabled && !Compatibility2009)
										{
											ushort[] fontWidth3 = GetFontWidth(stiFontState2);
											if (fontWidth3.Length != 0)
											{
												if (PrecisionMode3Enabled)
												{
													double num172 = (double)MaxFontSize / stiFontState2.EmValue / scale;
													int num173 = 0;
													double num174 = num145 * num172;
													for (int num175 = 0; num175 < pcGlyphs2; num175++)
													{
														int num176 = array18[num175];
														if (num176 == 0)
														{
															continue;
														}
														int num177 = array16[num175];
														if (num177 >= fontWidth3.Length)
														{
															num177 = fontWidth3.Length - 1;
														}
														int num178 = fontWidth3[num177];
														num174 += (double)num178;
														if ((double)num176 < (double)num178 / num172 - 0.5)
														{
															int num179 = (int)Math.Round((double)num178 / num172);
															if (num176 >= num179)
															{
																num179++;
															}
															num176 = num179;
														}
														array18[num175] = num176;
														num173 += num176;
													}
													num174 /= num172;
													bool[] array19 = new bool[pcGlyphs2];
													int num180 = 0;
													int num181 = 0;
													for (int num182 = 0; num182 < num148; num182++)
													{
														if (char.IsWhiteSpace(text9[num182]))
														{
															int num183 = array17[num182];
															array19[num183] = true;
															num181 += array18[num183];
															num180++;
														}
													}
													if (num180 > 0 && (double)num173 - num174 < (double)num181 * 0.25)
													{
														double num184 = ((double)num181 - ((double)num173 - num174)) / (double)num181;
														double num185 = 0.0;
														int num186 = 0;
														for (int num187 = 0; num187 < pcGlyphs2; num187++)
														{
															if (array19[num187])
															{
																int num188 = array18[num187];
																if (num188 != 0)
																{
																	num185 += (double)num188 * num184;
																	int num189 = (int)Math.Round(num185 - (double)num186, 0);
																	num186 += num189;
																	array18[num187] = num189;
																}
															}
														}
														if (num185 - (double)num186 > 0.5)
														{
															array18[pcGlyphs2 - 1]++;
															num186++;
														}
														num145 = num185 - (double)num186;
													}
													else
													{
														double num190 = num174 / (double)num173;
														num174 = num145;
														int num191 = 0;
														for (int num192 = 0; num192 < pcGlyphs2; num192++)
														{
															int num193 = array18[num192];
															if (num193 != 0)
															{
																num174 += (double)num193 * num190;
																int num194 = (int)Math.Round(num174 - (double)num191, 0);
																num191 += num194;
																array18[num192] = num194;
															}
														}
														if (num174 - (double)num191 > 0.5)
														{
															array18[pcGlyphs2 - 1]++;
															num191++;
														}
														num145 = num174 - (double)num191;
													}
												}
												else if (PrecisionMode2Enabled)
												{
													double num195 = (double)MaxFontSize / stiFontState2.EmValue / scale;
													int num196 = 0;
													double num197 = num145 * num195;
													for (int num198 = 0; num198 < pcGlyphs2; num198++)
													{
														num196 += array18[num198];
														if (array18[num198] != 0)
														{
															int num199 = array16[num198];
															if (num199 >= fontWidth3.Length)
															{
																num199 = fontWidth3.Length - 1;
															}
															num197 += (double)(int)fontWidth3[num199];
														}
													}
													num197 /= num195;
													bool[] array20 = new bool[pcGlyphs2];
													int num200 = 0;
													int num201 = 0;
													for (int num202 = 0; num202 < num148; num202++)
													{
														if (char.IsWhiteSpace(text9[num202]))
														{
															int num203 = array17[num202];
															array20[num203] = true;
															num201 += array18[num203];
															num200++;
														}
													}
													if (num200 > 0 && (double)num196 - num197 < (double)num201 * 0.25)
													{
														double num204 = ((double)num201 - ((double)num196 - num197)) / (double)num201;
														double num205 = 0.0;
														int num206 = 0;
														for (int num207 = 0; num207 < pcGlyphs2; num207++)
														{
															if (array20[num207])
															{
																int num208 = array18[num207];
																if (num208 != 0)
																{
																	num205 += (double)num208 * num204;
																	int num209 = (int)Math.Round(num205 - (double)num206, 0);
																	num206 += num209;
																	array18[num207] = num209;
																}
															}
														}
														if (num205 - (double)num206 > 0.5)
														{
															array18[pcGlyphs2 - 1]++;
															num206++;
														}
														num145 = num205 - (double)num206;
													}
													else
													{
														double num210 = num197 / (double)num196;
														num197 = num145;
														int num211 = 0;
														for (int num212 = 0; num212 < pcGlyphs2; num212++)
														{
															int num213 = array18[num212];
															if (num213 != 0)
															{
																num197 += (double)num213 * num210;
																int num214 = (int)Math.Round(num197 - (double)num211, 0);
																num211 += num214;
																array18[num212] = num214;
															}
														}
														if (num197 - (double)num211 > 0.5)
														{
															array18[pcGlyphs2 - 1]++;
															num211++;
														}
														num145 = num197 - (double)num211;
													}
												}
												else
												{
													double num215 = (double)MaxFontSize / stiFontState2.EmValue / scale;
													for (int num216 = 0; num216 < pcGlyphs2; num216++)
													{
														if (array18[num216] == 0)
														{
															continue;
														}
														int num217 = array16[num216];
														if (num217 >= fontWidth3.Length)
														{
															num217 = fontWidth3.Length - 1;
														}
														double num218 = (double)(int)fontWidth3[num217] / num215;
														if ((double)array18[num216] < num218 - 0.4)
														{
															int num219 = (int)Math.Round(num218);
															if (array18[num216] >= num219)
															{
																num219++;
															}
															array18[num216] = num219;
														}
														else
														{
															double num220 = (double)array18[num216] / num218;
															if ((double)array18[num216] - num218 > 1.0 && num220 > 1.1)
															{
																array18[num216] = (int)Math.Ceiling(num218);
															}
														}
													}
												}
											}
										}
										if (flag13 && !PrecisionMode2Enabled)
										{
											int num221 = 0;
											for (int num222 = 0; num222 < pcGlyphs2; num222++)
											{
												num221 += array18[num222];
											}
											if (num221 > 0)
											{
												double num223 = (double)num152 * scale / (double)num221;
												int num224 = 0;
												double num225 = 0.0;
												for (int num226 = 0; num226 < pcGlyphs2; num226++)
												{
													num225 += (double)array18[num226] * num223;
													int num227 = (int)num225 - num224;
													num224 += num227;
													array18[num226] = num227;
												}
											}
										}
										int[] array21 = null;
										if (outRunsList != null)
										{
											array21 = new int[pcGlyphs2];
											for (int num228 = 0; num228 < pcGlyphs2; num228++)
											{
												array21[num228] = array18[num228];
											}
										}
										double num229 = 0.0;
										if (array2[num3].TS.LetterSpacing != 0.0)
										{
											double num230 = stiFontState2.EmValue * 1.35 * array2[num3].TS.LetterSpacing * scale;
											int num231 = 0;
											double num232 = 0.0;
											for (int num233 = 0; num233 < pcGlyphs2; num233++)
											{
												num232 += (double)array18[num233] + num230;
												int num234 = (int)num232 - num231;
												num231 += num234;
												array18[num233] = num234;
											}
											num229 += num230 * (double)pcGlyphs2;
										}
										if (array2[num3].TS.WordSpacing != 0.0)
										{
											double num235 = stiFontState2.EmValue * array2[num3].TS.WordSpacing * scale;
											int num236 = 0;
											double num237 = 0.0;
											for (int num238 = 0; num238 < num148; num238++)
											{
												if (char.IsWhiteSpace(text9[num238]))
												{
													num237 += (double)array18[array17[num238]] + num235;
													int num239 = (int)num237 - num236;
													num236 += num239;
													array18[array17[num238]] += num239;
													num229 += num235;
												}
											}
										}
										int num240 = 0;
										bool flag14 = !array13[num147].a.fRTL;
										for (int num241 = 0; num241 < num148; num241++)
										{
											int num242 = 0;
											if (text9[num241] == '\t')
											{
												int tabsWidth2 = GetTabsWidth(textOptions, num24 * scale, num142 + num240);
												num242 = tabsWidth2;
												num152 += (int)((double)tabsWidth2 / scale);
											}
											else
											{
												int num243 = array17[num241];
												while (num241 < num148 - 1 && array17[num241 + 1] == num243)
												{
													num241++;
													flag14 = false;
												}
												int num244 = num243;
												int num245 = num243;
												if (array13[num146].a.fRTL)
												{
													num244 = ((num241 + 1 < num148) ? (array17[num241 + 1] + 1) : 0);
													num245++;
												}
												else
												{
													num245 = ((num241 + 1 < num148) ? array17[num241 + 1] : pcGlyphs2);
												}
												for (int num246 = num244; num246 < num245; num246++)
												{
													num242 += array18[num246];
												}
												if (num245 - num244 > 1)
												{
													flag14 = false;
												}
											}
											num240 += num242;
										}
										int num247 = 0;
										double num248 = 0.0;
										if (stiTextHorAlignment == StiTextHorAlignment.Width)
										{
											for (int num249 = 0; num249 < num148; num249++)
											{
												if (char.IsWhiteSpace(text9[num249]) && text9[num249] != '\u00a0')
												{
													num248 += lineInfo12.JustifyOffset;
													int num250 = (int)(num248 - (double)num247);
													num247 += num250;
													array18[array17[num249]] += num250;
												}
											}
										}
										if ((flag14 && flag) || outRunsList != null)
										{
											RunInfo item4 = default(RunInfo);
											item4.Text = text9;
											item4.XPos = num143 + ((outRunsList != null) ? (rectangleD2.X - rectangleD.X) : 0.0);
											item4.YPos = num149;
											item4.Widths = new int[num148];
											for (int num251 = 0; num251 < num148; num251++)
											{
												int num252 = num251;
												int num253;
												for (num253 = array17[num251]; num251 < num148 - 1 && array17[num251 + 1] == num253; num251++)
												{
												}
												int num254 = num253;
												int num255 = num253;
												if (array13[num146].a.fRTL)
												{
													num254 = ((num251 + 1 < num148) ? (array17[num251 + 1] + 1) : 0);
													num255++;
												}
												else
												{
													num255 = ((num251 + 1 < num148) ? array17[num251 + 1] : pcGlyphs2);
												}
												if (num252 != num251 || num255 - num254 > 1)
												{
													int num256 = 0;
													for (int num257 = num254; num257 < num255; num257++)
													{
														num256 += array18[num257];
													}
													if (num251 != num252)
													{
														int num258 = num251 - num252 + 1;
														double num259 = (double)num256 / (double)num258;
														double num260 = 0.0;
														int num261 = 0;
														for (int num262 = 0; num262 < num258 - 1; num262++)
														{
															num260 += num259;
															int num263 = (int)Math.Round(num260) - num261;
															num261 += num263;
															item4.Widths[num252 + num262] = num263;
														}
														item4.Widths[num251] = num256 - num261;
													}
													else
													{
														item4.Widths[num251] = num256;
													}
												}
												else
												{
													item4.Widths[num251] = array18[num254];
												}
											}
											if (array21 == null)
											{
												array21 = new int[pcGlyphs2];
											}
											item4.GlyphIndexList = new int[pcGlyphs2];
											item4.GlyphWidths = new int[pcGlyphs2];
											item4.ScaleList = new double[pcGlyphs2];
											for (int num264 = 0; num264 < pcGlyphs2; num264++)
											{
												item4.GlyphIndexList[num264] = array16[num264];
												item4.GlyphWidths[num264] = array18[num264];
												item4.ScaleList[num264] = 1.0;
												if (array21[num264] != 0)
												{
													item4.ScaleList[num264] = (double)array18[num264] / (double)array21[num264];
												}
											}
											item4.TextColor = array2[num3].TS.FontColor;
											item4.BackColor = array2[num3].TS.BackColor;
											item4.FontIndex = array2[num3].FontIndex;
											item4.Href = array2[num3].TS.Href;
											item4.TextAlign = array2[num3].TS.TextAlign;
											list.Add(item4);
										}
										else
										{
											double fontPositionCorrection = GetFontPositionCorrection(stiFontState2);
											fontPositionCorrection += stiFontState2.TypoOffset;
											num133 = ScriptTextOut(hdc, ref psc, (int)Math.Round((decimal)num143), (int)Math.Round((decimal)(num149 + fontPositionCorrection)), 0u, ref lprc, ref array13[num147].a, IntPtr.Zero, 0, array16, pcGlyphs2, array18, IntPtr.Zero, intPtr10);
											if (num133 != 0)
											{
												Marshal.FreeHGlobal(intPtr10);
												StringBuilder stringBuilder3 = new StringBuilder();
												try
												{
													stringBuilder3.Append($" indexLine={num130}");
													stringBuilder3.Append($" scriptItemCount={num136}");
													stringBuilder3.Append($" indexRun={num146}");
													stringBuilder3.Append($" runCharLen={num148}");
													stringBuilder3.Append(string.Format(" runText={0}", (text9 == null) ? "null" : ("\"" + text9 + "\"")));
													stringBuilder3.Append($" err17={num151}");
													stringBuilder3.Append($" hdc={hdc.ToInt64():X8} hScriptCache={stiFontState2.hScriptCache.ToInt64():X8} x={(int)Math.Round((decimal)num143):X4} y={(int)Math.Round((decimal)num149):X4}");
													stringBuilder3.Append($" lpRect=({lprc.left:X4},{lprc.right:X4},{lprc.top:X4},{lprc.bottom:X4})");
													stringBuilder3.Append($" scriptItemList=({array13[num147].a.packed:X8},{array13[num147].a.s.packed:X8})");
													if (array16 != null)
													{
														stringBuilder3.Append($" glyphIndexList=[{array16.Length}]");
													}
													else
													{
														stringBuilder3.Append(" glyphIndexList=null");
													}
													stringBuilder3.Append($" glyphIndexCount={pcGlyphs2}");
													if (array18 != null)
													{
														stringBuilder3.Append($" advanceWidthList=[{array18.Length}]");
													}
													else
													{
														stringBuilder3.Append(" advanceWidthList=null");
													}
													stringBuilder3.Append(".\r Parameters: ");
													stringBuilder3.Append(string.Format(" text={0}", (text == null) ? "null" : ("\"" + text + "\"")));
													stringBuilder3.Append($" font={font.Name},{font.Size}");
													string text10 = (font.Bold ? "B" : "") + (font.Italic ? "I" : "") + (font.Underline ? "U" : "");
													if (text10.Length > 0)
													{
														stringBuilder3.Append("," + text10);
													}
													stringBuilder3.Append($" bounds=({bounds.Left.ToString().Replace(',', '.')},{bounds.Right.ToString().Replace(',', '.')},{bounds.Top.ToString().Replace(',', '.')},{bounds.Bottom.ToString().Replace(',', '.')})");
													stringBuilder3.Append($" lineSpacing={lineSpacing}");
													stringBuilder3.Append(string.Format(" wordwrap={0}", wordWrap ? "true" : "false"));
													stringBuilder3.Append(string.Format(" rightToLeft={0}", rightToLeft ? "true" : "false"));
													stringBuilder3.Append($" scale={scale}");
													stringBuilder3.Append($" angle={angle}");
													stringBuilder3.Append($" trim={((object)(StringTrimming)(ref trimming)).ToString()}");
													stringBuilder3.Append(string.Format(" lineLimit={0}", lineLimit ? "true" : "false"));
													stringBuilder3.Append(string.Format(" allowHtmlTags={0}", allowHtmlTags ? "true" : "false"));
												}
												catch
												{
													ThrowError(19, num133);
												}
												Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
												throw new Exception($"TextRender error at step {19}, code #{ex.ErrorCode:X8}(#{num133:X8}): {ex.Message} \r{stringBuilder3.ToString()}");
											}
										}
										Marshal.FreeHGlobal(intPtr10);
										if (flag13)
										{
											if (stiFontState2.hScriptCacheScaled != psc)
											{
												stiFontState2.hScriptCacheScaled = psc;
											}
										}
										else if (stiFontState2.hScriptCache != psc)
										{
											stiFontState2.hScriptCache = psc;
										}
										num143 = ((!flag13) ? (num143 + ((double)num240 + num248)) : (num143 + ((double)num152 * scale + num248 + num229)));
										num142 += num240;
									}
								}
								num129 += (decimal)lineInfo12.LineHeight;
							}
							if (num15 == 1)
							{
								SelectClipRgn(hdc, zero);
							}
							if (num15 == 0)
							{
								SelectClipRgn(hdc, IntPtr.Zero);
							}
							DeleteObject(intPtr);
						}
						if (!needDraw && list6.Count > num27)
						{
							flag2 = num27 > 0 && list6[num27 - 1].NeedWidthAlign;
							if (allowHtmlTags)
							{
								LineInfo lineInfo13 = list6[num27];
								int num265 = array3[lineInfo13.Begin];
								StiHtmlState stiHtmlState3 = array2[num265];
								int begin4;
								for (begin4 = lineInfo13.Begin; begin4 > 0 && array3[begin4 - 1] == array3[begin4]; begin4--)
								{
								}
								int num266 = lineInfo13.Begin - begin4;
								string text11 = PrepareStateText(stiHtmlState3.Text).ToString();
								if (num27 > 0)
								{
									string text12 = null;
									text12 = ((num265 <= 0 || array2[num265 - 1].TS.Tag.Tag != StiHtmlTag.ListItem || stiHtmlState3.TS.Tag.Tag != StiHtmlTag.ListItem) ? StateToHtml(stiHtmlState3, stiHtmlState3, text11.Substring(0, num266), lineInfo13.Indent) : ((array2[num265 - 1].ListLevels != null && stiHtmlState3.ListLevels != null && array2[num265 - 1].ListLevels.Count != stiHtmlState3.ListLevels.Count) ? (((stiHtmlState3.ListLevels[stiHtmlState3.ListLevels.Count - 1] > 0) ? "<ol>" : "<ul>") + text11.Substring(0, num266)) : ("<li>" + text11.Substring(0, num266))));
									result = text3.Substring(0, stiHtmlState3.PosBegin) + ((num266 > 0) ? text12 : "") + (flag2 ? StiForceWidthAlignTag : "");
								}
								else
								{
									result = string.Empty;
								}
								int num267 = array3[lineInfo13.Begin] + 1;
								text2 = StateToHtml(stiHtmlState3, ((stiHtmlState3.TS.Tag.Tag == StiHtmlTag.ListItem || stiHtmlState3.TS.Tag.Tag == StiHtmlTag.P) && num267 < array2.Length) ? array2[num267] : stiHtmlState3, text11.Substring(num266), lineInfo13.Indent);
								if (stiHtmlState3.TS.Tag.Tag == StiHtmlTag.ListItem && num267 < array2.Length)
								{
									text2 += array2[num267].Text;
									num267++;
								}
								for (; num267 < array2.Length && array2[num267].PosBegin == array2[num267 - 1].PosBegin && array2[num267].Text.ToString() != "\n" && array2[num267 - 1].Text.ToString() != "\n"; num267++)
								{
									text2 += array2[num267].Text;
								}
								if (num267 < array2.Length)
								{
									text2 += text3.Substring(array2[num267].PosBegin);
								}
							}
							else
							{
								LineInfo lineInfo14 = null;
								if (num27 > 0)
								{
									lineInfo14 = list6[num27 - 1];
									result = text.Substring(0, lineInfo14.End) + (flag2 ? StiForceWidthAlignTag : "");
								}
								else
								{
									result = string.Empty;
								}
								lineInfo14 = list6[num27];
								text2 = text.Substring(lineInfo14.Begin);
							}
						}
					}
					finally
					{
						if (needDraw)
						{
							if (!SetWorldTransform(hdc, ref lpXform))
							{
								ThrowError(20);
							}
							if (num13 != 0 && SetMapMode(hdc, num13) == 0)
							{
								ThrowError(21);
							}
							if (SetGraphicsMode(hdc, num12) == 0)
							{
								ThrowError(22);
							}
						}
						for (int num268 = 0; num268 < array.Length; num268++)
						{
							if (!DeleteObject(array[num268].hFont))
							{
								ThrowError(23);
							}
							ScriptFreeCache(ref array[num268].hScriptCache);
							if (array[num268].hScriptCacheScaled != IntPtr.Zero)
							{
								ScriptFreeCache(ref array[num268].hScriptCacheScaled);
							}
						}
					}
				}
				finally
				{
					g.ReleaseHdc(hdc);
				}
				if (list.Count > 0)
				{
					if (outRunsList != null)
					{
						outRunsList.Clear();
						outRunsList.AddRange(list);
						outFontsList.Clear();
						outFontsList.AddRange(array);
					}
					else
					{
						StringFormat val3 = new StringFormat(StringFormat.GenericTypographic);
						Region clip = g.Clip;
						RectangleF rectangleF = new RectangleF((float)bounds.X, (float)bounds.Y, (float)bounds.Width, (float)bounds.Height);
						g.SetClip(rectangleF, (CombineMode)1);
						TextRenderingHint textRenderingHint = g.TextRenderingHint;
						GraphicsState val4 = g.Save();
						g.TranslateTransform((float)(bounds.X + num * scale + rectangleD2.Width / 2.0), (float)(bounds.Y + rectangleD2.Height / 2.0));
						g.RotateTransform((float)(0.0 - angle));
						g.TranslateTransform((float)((0.0 - sizeD.Width) / 2.0), (float)((0.0 - sizeD.Height) / 2.0));
						for (int num269 = 0; num269 < list.Count; num269++)
						{
							RunInfo runInfo = list[num269];
							double num270 = runInfo.XPos;
							double yPos = runInfo.YPos;
							Font val5 = array[runInfo.FontIndex].FontBase;
							if (array[runInfo.FontIndex].hFontScaled != IntPtr.Zero)
							{
								val5 = array[runInfo.FontIndex].FontScaled;
							}
							SolidBrush val6 = new SolidBrush(runInfo.TextColor);
							if (runInfo.BackColor != Color.Transparent)
							{
								double num271 = 0.0;
								for (int num272 = 0; num272 < runInfo.Text.Length; num272++)
								{
									num271 += (double)runInfo.Widths[num272];
								}
								RectangleF rectangleF2 = default(RectangleF);
								rectangleF2.X = (float)num270;
								rectangleF2.Y = (float)yPos;
								rectangleF2.Width = (float)num271;
								rectangleF2.Height = (float)array[runInfo.FontIndex].LineHeight;
								g.FillRectangle((Brush)new SolidBrush(runInfo.BackColor), rectangleF2);
							}
							for (int num273 = 0; num273 < runInfo.Text.Length; num273++)
							{
								g.DrawString(runInfo.Text[num273].ToString(), val5, (Brush)(object)val6, (float)num270, (float)yPos, val3);
								num270 += (double)runInfo.Widths[num273];
							}
						}
						g.Restore(val4);
						g.TextRenderingHint = textRenderingHint;
						g.Clip = clip;
					}
				}
			}
			catch
			{
			}
			finally
			{
				for (int num274 = 0; num274 < array.Length; num274++)
				{
					if (array[num274].hFontScaled != IntPtr.Zero && !DeleteObject(array[num274].hFontScaled))
					{
						ThrowError(24);
					}
				}
			}
		}
		text = text2;
		return result;
	}

	private static OUTLINETEXTMETRIC GetOutlineTextMetricsCached(string fontName, FontStyle fontStyle, IntPtr hdc)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		string key = fontName + "*" + (char)(48 + fontStyle);
		object obj = outlineTextMetricsCache[key];
		if (obj == null)
		{
			OUTLINETEXTMETRIC oUTLINETEXTMETRIC = default(OUTLINETEXTMETRIC);
			lock (lockOutlineTextMetricsCache)
			{
				Font val = StiFontCollection.CreateFont(fontName, MaxFontSize, fontStyle);
				try
				{
					IntPtr intPtr = val.ToHfont();
					try
					{
						SelectObject(hdc, intPtr);
						uint outlineTextMetrics = GetOutlineTextMetrics(hdc, 0u, IntPtr.Zero);
						if (outlineTextMetrics == 0)
						{
							ThrowError(1);
						}
						IntPtr intPtr2 = Marshal.AllocHGlobal((int)outlineTextMetrics);
						try
						{
							if (GetOutlineTextMetrics(hdc, outlineTextMetrics, intPtr2) != 0)
							{
								oUTLINETEXTMETRIC = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(intPtr2, typeof(OUTLINETEXTMETRIC));
							}
						}
						finally
						{
							Marshal.FreeHGlobal(intPtr2);
						}
					}
					finally
					{
						if (!DeleteObject(intPtr))
						{
							ThrowError(2);
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				outlineTextMetricsCache[key] = oUTLINETEXTMETRIC;
				return oUTLINETEXTMETRIC;
			}
		}
		return (OUTLINETEXTMETRIC)obj;
	}

	private static bool isWordWrapSymbol2(string text, int pos)
	{
		char c = text[pos];
		if (c != '!' && c != '%' && c != ')' && c != '}' && c != '-' && c != '?' && c != '）' && c != '：' && c != '、' && c != '，')
		{
			return c == '。';
		}
		return true;
	}

	private static bool isNotWordWrapSymbol(string text, int pos)
	{
		UnicodeCategory unicodeCategory = char.GetUnicodeCategory(text[pos]);
		bool result = unicodeCategory == UnicodeCategory.OtherPunctuation || unicodeCategory == UnicodeCategory.MathSymbol || unicodeCategory == UnicodeCategory.CurrencySymbol || unicodeCategory == UnicodeCategory.OtherSymbol || unicodeCategory == UnicodeCategory.ConnectorPunctuation || unicodeCategory == UnicodeCategory.InitialQuotePunctuation || unicodeCategory == UnicodeCategory.FinalQuotePunctuation;
		if (pos > 0 && isWordWrapSymbol2(text, pos - 1))
		{
			result = false;
		}
		return result;
	}

	private static bool isNotWordWrapSymbol2(string text, int pos)
	{
		UnicodeCategory unicodeCategory = char.GetUnicodeCategory(text[pos]);
		if (unicodeCategory != UnicodeCategory.OtherPunctuation && unicodeCategory != UnicodeCategory.MathSymbol && unicodeCategory != UnicodeCategory.CurrencySymbol && unicodeCategory != UnicodeCategory.OtherSymbol && unicodeCategory != UnicodeCategory.ConnectorPunctuation && unicodeCategory != UnicodeCategory.InitialQuotePunctuation && unicodeCategory != UnicodeCategory.FinalQuotePunctuation)
		{
			return char.IsLetterOrDigit(text[pos]);
		}
		return true;
	}

	private static bool isCJKWordWrap(string text, int pos)
	{
		if (pos > 0 && isCJKSymbol(text, pos))
		{
			return isCJKSymbol(text, pos - 1);
		}
		return false;
	}

	private static bool isCJKSymbol(string text, int pos)
	{
		char c = text[pos];
		if (c < '一' || c > '鿌')
		{
			if (c >= '㐀')
			{
				return c <= '䶵';
			}
			return false;
		}
		return true;
	}

	internal static List<StiHtmlState> CheckMissingGlyphs(StiHtmlState state)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (StiFontCollection.IsCustomFont(state.TS.FontName))
		{
			return null;
		}
		string text = state.Text.ToString();
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		Font val = new Font(state.TS.FontName, 8f);
		ushort[] array = (ushort[])GetFontGlyphs(val);
		int fontNumber = GetFontNumber(val.Name);
		byte[] array2 = null;
		object obj = CacheFontSymbolsFallback[fontNumber];
		if (obj != null)
		{
			array2 = (byte[])obj;
		}
		else
		{
			array2 = new byte[65536];
			lock (CacheFontSymbolsFallback)
			{
				CacheFontSymbolsFallback[fontNumber] = array2;
			}
		}
		List<StiHtmlState> list = new List<StiHtmlState>();
		int num = fontNumber;
		StringBuilder stringBuilder = new StringBuilder();
		StiMeasureRichTextBox stiMeasureRichTextBox = null;
		for (int i = 0; i < text.Length; i++)
		{
			int num2 = text[i];
			string text2;
			if ((num2 & 0xD800) == 55296 && i + 1 < text.Length && (text[i + 1] & 0xDC00) == 56320)
			{
				text2 = text.Substring(i, 2);
				num2 = ((num2 & 0x3FF) << 10) | ((text[i + 1] & 0x3FF) + 65536);
				i++;
				if (array2.Length == 65536)
				{
					byte[] array3 = new byte[1114112];
					Array.Copy(array2, array3, 65536);
					array2 = array3;
					lock (CacheFontSymbolsFallback)
					{
						CacheFontSymbolsFallback[fontNumber] = array2;
					}
				}
			}
			else
			{
				text2 = text.Substring(i, 1);
			}
			int num3 = array2[num2];
			if (num3 == 0)
			{
				bool flag = false;
				if (array != null && num2 < array.Length)
				{
					int num4 = array[num2];
					if (num4 != 65535)
					{
						num3 = fontNumber;
						flag = true;
					}
				}
				if (!flag)
				{
					if (stiMeasureRichTextBox == null)
					{
						stiMeasureRichTextBox = new StiMeasureRichTextBox();
					}
					string fontNameOfSymbol = GetFontNameOfSymbol(text2, val, stiMeasureRichTextBox);
					num3 = ((fontNameOfSymbol != null) ? GetFontNumber(fontNameOfSymbol) : fontNumber);
				}
				array2[num2] = (byte)num3;
			}
			if (num != num3 && (num2 == 32 || num2 == 160))
			{
				num3 = num;
			}
			if (num != num3)
			{
				if (stringBuilder.Length > 0)
				{
					StiHtmlState item = new StiHtmlState(state);
					item.TS.FontName = (string)CacheNumToFontName[num];
					item.Text = new StringBuilder(stringBuilder.ToString());
					list.Add(item);
					stringBuilder.Clear();
				}
				num = num3;
			}
			stringBuilder.Append(text2);
		}
		((Component)(object)stiMeasureRichTextBox)?.Dispose();
		if (list.Count > 0 || num != fontNumber)
		{
			if (stringBuilder.Length > 0)
			{
				StiHtmlState item2 = new StiHtmlState(state);
				item2.TS.FontName = (string)CacheNumToFontName[num];
				item2.Text = stringBuilder;
				list.Add(item2);
			}
			return list;
		}
		return null;
	}

	private static int GetFontNumber(string fontName)
	{
		object obj = CacheFontNameToNum[fontName];
		if (obj != null)
		{
			return (int)obj;
		}
		lock (CacheFontNameToNum)
		{
			int num = CacheFontNameToNum.Count + 1;
			CacheFontNameToNum[fontName] = num;
			CacheNumToFontName[num] = fontName;
			return num;
		}
	}

	private static string GetFontNameOfSymbol(string st, Font baseFont, StiMeasureRichTextBox measureRichTextBox)
	{
		((Control)measureRichTextBox).Text = st;
		((TextBoxBase)measureRichTextBox).SelectAll();
		((RichTextBox)measureRichTextBox).SelectionFont = baseFont;
		((TextBoxBase)measureRichTextBox).Select(0, 1);
		Font selectionFont = ((RichTextBox)measureRichTextBox).SelectionFont;
		if (selectionFont.Name != baseFont.Name)
		{
			return selectionFont.Name;
		}
		return null;
	}

	private static string ConvertSymbolsToTags(string inputText)
	{
		if (string.IsNullOrWhiteSpace(inputText))
		{
			return inputText;
		}
		inputText = inputText.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
		StringBuilder stringBuilder = new StringBuilder();
		if (inputText.IndexOf('\r') == -1 && inputText.IndexOf('\n') == -1)
		{
			stringBuilder.Append(inputText);
		}
		else
		{
			for (int i = 0; i < inputText.Length; i++)
			{
				char c = inputText[i];
				if (c == '\r' || c == '\n')
				{
					if (i + 1 < inputText.Length)
					{
						char c2 = inputText[i + 1];
						if ((c2 == '\r' || c2 == '\n') && c2 != c)
						{
							i++;
						}
					}
					stringBuilder.Append("<br>");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
		}
		return stringBuilder.ToString();
	}

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	internal static extern uint GetOutlineTextMetrics(IntPtr hdc, uint cbData, IntPtr lpOTM);

	[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern int DrawTextExW(IntPtr hDC, string lpszString, int nCount, ref RECT lpRect, int nFormat, [In][Out] DRAWTEXTPARAMS param);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool DeleteObject(IntPtr objectHandle);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern int SetTextColor(IntPtr hDC, int crColor);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern int SetBkMode(IntPtr hDC, int nBkMode);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern int SetBkColor(IntPtr hDC, int clr);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern int GetClipRgn(IntPtr hDC, IntPtr hRgn);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
	private static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool GetWorldTransform(IntPtr hdc, out XFORM lpXform);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool SetWorldTransform(IntPtr hdc, ref XFORM lpXform);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool ModifyWorldTransform(IntPtr hdc, ref XFORM lpXform, uint iMode);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int SetGraphicsMode(IntPtr hdc, int iMode);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int SetMapMode(IntPtr hdc, int fnMapMode);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool GetWindowExtEx(IntPtr hdc, out SIZE lpSize);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool SetWindowExtEx(IntPtr hdc, int nXExtent, int nYExtent, out SIZE lpSize);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool GetViewportExtEx(IntPtr hdc, out SIZE lpSize);

	[DllImport("gdi32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern bool SetViewportExtEx(IntPtr hdc, int nXExtent, int nYExtent, out SIZE lpSize);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool GetCharABCWidthsI(IntPtr hdc, uint giFirst, uint cgi, [In][Out] ushort[] pgi, [In][Out] ABC[] lpabc);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern uint GetGlyphIndices(IntPtr hdc, string lpstr, int c, [In][Out] ushort[] pgi, uint fl);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptItemize(string pwcInChars, int cInChars, int cMaxItems, ref SCRIPT_CONTROL psControls, ref SCRIPT_STATE psState, IntPtr scriptItemList, out int scriptItemCount);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptLayout(int cRuns, byte[] pbLevel, int[] piVisualToLogical, int[] piLogicalToVisual);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptShape(IntPtr hdc, ref IntPtr psc, string pwcChars, int cChars, int cMaxGlyphs, ref SCRIPT_ANALYSIS psa, ushort[] pwOutGlyphs, ushort[] pwLogClust, IntPtr psva, out int pcGlyphs);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptPlace(IntPtr hdc, ref IntPtr psc, ushort[] pwGlyphs, int pcGlyphs, IntPtr psva, ref SCRIPT_ANALYSIS psa, int[] piAdvance, IntPtr pGoffset, out ABC pABC);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptTextOut(IntPtr hdc, ref IntPtr psc, int x, int y, uint fuOptions, ref RECT lprc, ref SCRIPT_ANALYSIS psa, IntPtr pwcReserved, int iReserved, ushort[] pwGlyphs, int cGlyphs, int[] piAdvance, IntPtr piJustify, IntPtr pGoffset);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptFreeCache(ref IntPtr psc);

	[DllImport("usp10.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern int ScriptBreak(string pwcInChars, int cInChars, ref SCRIPT_ANALYSIS psa, IntPtr psla);

	private static StiHtmlTag2 ConvertStringToTag(string st)
	{
		StiHtmlTag2 stiHtmlTag = new StiHtmlTag2();
		st = st.Trim();
		if (st.EndsWith("/"))
		{
			stiHtmlTag.State = StiHtmlTag2State.Empty;
			st = st.Substring(0, st.Length - 1).Trim();
		}
		stiHtmlTag.Attributes = ParseTagIntoPairs(st);
		if (stiHtmlTag.Attributes.Count == 0)
		{
			return stiHtmlTag;
		}
		string text = stiHtmlTag.Attributes[0].Key;
		if (text.StartsWith("/"))
		{
			stiHtmlTag.State = StiHtmlTag2State.End;
			text = text.Substring(1);
		}
		switch (text)
		{
		case "p":
			stiHtmlTag.Tag = StiHtmlTag.P;
			break;
		case "br":
			stiHtmlTag.Tag = StiHtmlTag.Br;
			break;
		case "ol":
			stiHtmlTag.Tag = StiHtmlTag.OrderedList;
			break;
		case "ul":
			stiHtmlTag.Tag = StiHtmlTag.UnorderedList;
			break;
		case "li":
			stiHtmlTag.Tag = StiHtmlTag.ListItem;
			break;
		case "a":
			stiHtmlTag.Tag = StiHtmlTag.A;
			break;
		case "strong":
		case "b":
			stiHtmlTag.Tag = StiHtmlTag.B;
			break;
		case "em":
		case "i":
			stiHtmlTag.Tag = StiHtmlTag.I;
			break;
		case "u":
			stiHtmlTag.Tag = StiHtmlTag.U;
			break;
		case "strike":
		case "s":
			stiHtmlTag.Tag = StiHtmlTag.S;
			break;
		case "sup":
			stiHtmlTag.Tag = StiHtmlTag.Sup;
			break;
		case "sub":
			stiHtmlTag.Tag = StiHtmlTag.Sub;
			break;
		case "letter-spacing":
			stiHtmlTag.Tag = StiHtmlTag.LetterSpacing;
			break;
		case "word-spacing":
			stiHtmlTag.Tag = StiHtmlTag.WordSpacing;
			break;
		case "line-height":
			stiHtmlTag.Tag = StiHtmlTag.LineHeight;
			break;
		case "text-align":
			stiHtmlTag.Tag = StiHtmlTag.TextAlign;
			break;
		case "font":
			stiHtmlTag.Tag = StiHtmlTag.Font;
			break;
		case "font-face":
		case "font-family":
		case "font-name":
			stiHtmlTag.Tag = StiHtmlTag.FontName;
			break;
		case "font-size":
			stiHtmlTag.Tag = StiHtmlTag.FontSize;
			break;
		case "font-color":
		case "color":
			stiHtmlTag.Tag = StiHtmlTag.FontColor;
			break;
		case "background-color":
			stiHtmlTag.Tag = StiHtmlTag.Backcolor;
			break;
		}
		if (stiHtmlTag.Tag == StiHtmlTag.None && !string.IsNullOrWhiteSpace(text))
		{
			stiHtmlTag.Tag = StiHtmlTag.Unknown;
			stiHtmlTag.TagName = text;
		}
		return stiHtmlTag;
	}

	public static List<StiHtmlState> ParseHtmlToStates(string inputHtml, StiHtmlState baseState, bool storeStack = false)
	{
		List<StiHtmlState> list = ParseHtmlToStates2(inputHtml, baseState, storeStack);
		if (AllowCheckMissingGlyphs)
		{
			try
			{
				List<StiHtmlState> list2 = new List<StiHtmlState>();
				foreach (StiHtmlState item in list)
				{
					List<StiHtmlState> list3 = CheckMissingGlyphs(item);
					if (list3 != null)
					{
						list2.AddRange(list3);
					}
					else
					{
						list2.Add(item);
					}
				}
				list = list2;
			}
			catch
			{
				AllowCheckMissingGlyphs = false;
			}
		}
		return list;
	}

	private static List<StiHtmlState> ParseHtmlToStates2(string inputHtml, StiHtmlState baseState, bool storeStack)
	{
		List<StiHtmlState> list = new List<StiHtmlState>();
		StiHtmlState state = baseState;
		List<StiHtmlTagsState> list2 = new List<StiHtmlTagsState>();
		int num = 0;
		bool flag = false;
		if (inputHtml == null)
		{
			inputHtml = string.Empty;
		}
		while (num < inputHtml.Length)
		{
			if (inputHtml[num] != '<')
			{
				char c = inputHtml[num];
				if (char.IsWhiteSpace(c) && c != '\u00a0')
				{
					if (!flag)
					{
						state.Text.Append(' ');
						flag = true;
					}
				}
				else if (char.GetUnicodeCategory(c) != UnicodeCategory.OtherNotAssigned)
				{
					state.Text.Append(c);
					flag = false;
				}
				num++;
				continue;
			}
			if (state.Text.Length > 0)
			{
				list.Add(state);
				state = new StiHtmlState(state);
				state.PosBegin = num;
				if (state.TS.Tag.Tag == StiHtmlTag.ListItem)
				{
					state.TS.Tag = new StiHtmlTag2();
				}
			}
			while (num < inputHtml.Length && inputHtml[num] == '<')
			{
				num++;
				int i;
				for (i = num; i < inputHtml.Length && inputHtml[i] != '>'; i++)
				{
				}
				string st = inputHtml.Substring(num, i - num);
				num = i;
				num++;
				StiHtmlTag2 stiHtmlTag = ConvertStringToTag(st);
				if (stiHtmlTag.Tag == StiHtmlTag.Unknown && stiHtmlTag.TagName == "div")
				{
					stiHtmlTag.Tag = StiHtmlTag.P;
					stiHtmlTag.Attributes.Add(new TagPair
					{
						Key = "style",
						KeyBase = "style",
						Value = "margin:0;"
					});
				}
				if (stiHtmlTag.IsEndTag(StiHtmlTag.P))
				{
					ParseStyleAttributes(stiHtmlTag, ref state, baseState.TS);
					state.Text.Append('\n');
					list.Add(state);
					state = new StiHtmlState(state);
					state.PosBegin = num;
					flag = true;
					state.TS.Tag = new StiHtmlTag2(StiHtmlTag.P, StiHtmlTag2State.End);
					if (num < inputHtml.Length && !string.IsNullOrWhiteSpace(inputHtml.Substring(num)))
					{
						double marginSize = GetMarginSize(state, "margin-bottom");
						state.Text.Append('\n');
						double lineHeight = state.TS.LineHeight;
						state.TS.LineHeight = marginSize;
						list.Add(state);
						state = new StiHtmlState(state);
						state.PosBegin = num;
						state.TS.LineHeight = lineHeight;
						state.TS.Tag = new StiHtmlTag2();
					}
					if (state.TS.Indent > 0)
					{
						state.Text.Append(GetIndentString(state.TS.Indent));
						list.Add(state);
						state = new StiHtmlState(state);
					}
					continue;
				}
				if (stiHtmlTag.IsStartTag(StiHtmlTag.P))
				{
					ParseStyleAttributes(stiHtmlTag, ref state, baseState.TS);
					double marginSize2 = GetMarginSize(state, "margin-top");
					if (list.Count <= 0 || !list[list.Count - 1].TS.Tag.IsEndTag(StiHtmlTag.P))
					{
						if (list.Count > 1 || (list.Count == 1 && list[0].Text.ToString().Trim().Length != 0))
						{
							state.Text.Append('\n');
							list.Add(state);
							state = new StiHtmlState(state);
						}
						state.PosBegin = num;
						flag = true;
						state.Text.Append('\n');
						double lineHeight2 = state.TS.LineHeight;
						state.TS.LineHeight = marginSize2;
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.P);
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.LineHeight = lineHeight2;
						state.PosBegin = num;
						if (state.TS.Indent > 0)
						{
							state.Text.Append(GetIndentString(state.TS.Indent));
							list.Add(state);
							state = new StiHtmlState(state);
						}
					}
					else
					{
						StiHtmlState value = list[list.Count - 1];
						if (value.TS.LineHeight < marginSize2)
						{
							value.TS.LineHeight = marginSize2;
							list[list.Count - 1] = value;
						}
					}
					continue;
				}
				if (stiHtmlTag.Tag == StiHtmlTag.Br)
				{
					flag = true;
					state.Text.Append('\n');
					list.Add(state);
					state = new StiHtmlState(state);
					state.PosBegin = num;
					if (state.TS.Indent > 0)
					{
						state.Text.Append(GetIndentString(state.TS.Indent));
					}
					continue;
				}
				if (stiHtmlTag.IsStartTag(StiHtmlTag.ListItem))
				{
					if (list.Count <= 0 || list[list.Count - 1].TS.Tag.Tag != StiHtmlTag.ListItem)
					{
						flag = true;
						if (list.Count != 0 && (list.Count <= 0 || !(list[list.Count - 1].Text.ToString() == "\n")))
						{
							state.Text.Append('\n');
							state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
							list.Add(state);
							state = new StiHtmlState(state);
						}
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						state.PosBegin = num;
						if (state.TS.Indent == 0 && state.ListLevels == null)
						{
							state.ListLevels = new List<int>();
							state.ListLevels.Add(0);
							state.TS.Indent++;
						}
						state.Text.Append(GetIndentString(state.TS.Indent));
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2();
						state.PosBegin = num;
					}
					if (state.TS.Indent == 0)
					{
						state.TS.Indent++;
					}
					if (state.ListLevels == null)
					{
						state.ListLevels = new List<int>();
					}
					while (state.TS.Indent > state.ListLevels.Count)
					{
						state.ListLevels.Add(0);
					}
					StiHtmlState value2 = list[list.Count - 1];
					ParseMarkerTypeAttribute(stiHtmlTag.Attributes, ref state);
					InsertMarker(value2.Text, state.ListLevels[state.TS.Indent - 1], state.TS.Indent);
					if (state.ListLevels[state.TS.Indent - 1] > 0)
					{
						state.ListLevels = new List<int>(state.ListLevels);
						state.ListLevels[state.TS.Indent - 1]++;
					}
					list[list.Count - 1] = value2;
					continue;
				}
				if (stiHtmlTag.IsEndTag(StiHtmlTag.ListItem))
				{
					if (list.Count <= 0 || list[list.Count - 1].TS.Tag.Tag != StiHtmlTag.ListItem)
					{
						flag = true;
						state.Text.Append('\n');
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						state.PosBegin = num;
						if (state.TS.Indent == 0)
						{
							state.TS.Indent++;
						}
						state.Text.Append(GetIndentString(state.TS.Indent));
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2();
						state.PosBegin = num;
					}
					continue;
				}
				if (stiHtmlTag.IsStartTag(StiHtmlTag.UnorderedList))
				{
					ParseStyleAttributes(stiHtmlTag, ref state, baseState.TS);
					if (list.Count <= 0 || list[list.Count - 1].TS.Tag.Tag != StiHtmlTag.ListItem)
					{
						flag = true;
						if (list.Count != 0 && (list.Count <= 0 || !(list[list.Count - 1].Text.ToString() == "\n")))
						{
							state.Text.Append('\n');
							state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
							list.Add(state);
							state = new StiHtmlState(state);
						}
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						state.PosBegin = num;
						state.Text.Append(GetIndentString(state.TS.Indent + 1));
						state.TS.Indent++;
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2();
						state.PosBegin = num;
					}
					else
					{
						state.TS.Indent++;
						StiHtmlState value3 = list[list.Count - 1];
						value3.Text.Append(GetIndentString(1));
						value3.TS.Indent++;
						list[list.Count - 1] = value3;
					}
					if (state.ListLevels == null)
					{
						state.ListLevels = new List<int>();
					}
					while (state.ListLevels.Count < state.TS.Indent)
					{
						state.ListLevels.Add(0);
					}
					state.ListLevels[state.TS.Indent - 1] = 1 - state.TS.Indent;
					ParseMarkerTypeAttribute(stiHtmlTag.Attributes, ref state);
					continue;
				}
				if (stiHtmlTag.IsEndTag(StiHtmlTag.UnorderedList))
				{
					if (list.Count <= 0 || list[list.Count - 1].TS.Tag.Tag != StiHtmlTag.ListItem)
					{
						flag = true;
						state.Text.Append('\n');
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						state.PosBegin = num;
						if (state.TS.Indent > 0)
						{
							state.TS.Indent--;
						}
						state.Text.Append(GetIndentString(state.TS.Indent));
						if (state.TS.Indent == 0)
						{
							state.ListLevels = null;
						}
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2();
						state.PosBegin = num;
					}
					else
					{
						if (state.TS.Indent > 0)
						{
							state.TS.Indent--;
						}
						StiHtmlState value4 = list[list.Count - 1];
						if (value4.TS.Indent > 0)
						{
							value4.TS.Indent--;
						}
						value4.Text = new StringBuilder(GetIndentString(value4.TS.Indent));
						if (state.TS.Indent == 0)
						{
							state.ListLevels = null;
							value4.ListLevels = null;
						}
						list[list.Count - 1] = value4;
						state.PosBegin = num;
					}
					continue;
				}
				if (stiHtmlTag.IsStartTag(StiHtmlTag.OrderedList))
				{
					ParseStyleAttributes(stiHtmlTag, ref state, baseState.TS);
					if (list.Count <= 0 || list[list.Count - 1].TS.Tag.Tag != StiHtmlTag.ListItem)
					{
						flag = true;
						if (list.Count != 0 && (list.Count <= 0 || !(list[list.Count - 1].Text.ToString() == "\n")))
						{
							state.Text.Append('\n');
							state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
							list.Add(state);
							state = new StiHtmlState(state);
						}
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						state.PosBegin = num;
						state.Text.Append(GetIndentString(state.TS.Indent + 1));
						state.TS.Indent++;
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2();
						state.PosBegin = num;
					}
					else
					{
						state.TS.Indent++;
						StiHtmlState value5 = list[list.Count - 1];
						value5.Text.Append(GetIndentString(1));
						value5.TS.Indent++;
						list[list.Count - 1] = value5;
					}
					if (state.ListLevels == null)
					{
						state.ListLevels = new List<int>();
					}
					while (state.ListLevels.Count < state.TS.Indent)
					{
						state.ListLevels.Add(1);
					}
					state.ListLevels[state.TS.Indent - 1] = 1;
					ParseMarkerTypeAttribute(stiHtmlTag.Attributes, ref state);
					StiHtmlState value6 = list[list.Count - 1];
					value6.ListLevels = state.ListLevels;
					list[list.Count - 1] = value6;
					continue;
				}
				if (stiHtmlTag.IsEndTag(StiHtmlTag.OrderedList))
				{
					if (list.Count <= 0 || list[list.Count - 1].TS.Tag.Tag != StiHtmlTag.ListItem)
					{
						flag = true;
						state.Text.Append('\n');
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.ListItem);
						state.PosBegin = num;
						if (state.TS.Indent > 0)
						{
							state.TS.Indent--;
						}
						state.Text.Append(GetIndentString(state.TS.Indent));
						if (state.TS.Indent == 0)
						{
							state.ListLevels = null;
						}
						list.Add(state);
						state = new StiHtmlState(state);
						state.TS.Tag = new StiHtmlTag2();
						state.PosBegin = num;
					}
					else
					{
						if (state.TS.Indent > 0)
						{
							state.TS.Indent--;
						}
						StiHtmlState value7 = list[list.Count - 1];
						if (value7.TS.Indent > 0)
						{
							value7.TS.Indent--;
						}
						value7.Text = new StringBuilder(GetIndentString(value7.TS.Indent));
						if (state.TS.Indent == 0)
						{
							state.ListLevels = null;
							value7.ListLevels = null;
						}
						list[list.Count - 1] = value7;
						state.PosBegin = num;
					}
					continue;
				}
				if (stiHtmlTag.Tag != 0)
				{
					StiHtmlTagsState tS = new StiHtmlTagsState(state.TS);
					ParseHtmlTag(stiHtmlTag, ref state, list2, baseState);
					if (tS.FontSize != state.TS.FontSize)
					{
						StiHtmlState stiHtmlState = new StiHtmlState(state);
						stiHtmlState.Text = state.Text;
						state.TS = tS;
						state.Text = new StringBuilder();
						list.Add(state);
						state = stiHtmlState;
						state.TS.Tag = new StiHtmlTag2(StiHtmlTag.Font);
					}
				}
				if (!storeStack || list2.Count <= 0)
				{
					continue;
				}
				state.TagsStack = new List<StiHtmlTagsState>();
				foreach (StiHtmlTagsState item in list2)
				{
					state.TagsStack.Add(new StiHtmlTagsState(item));
				}
			}
		}
		if (state.Text.Length > 0)
		{
			list.Add(state);
		}
		if (list.Count == 0)
		{
			list.Add(state);
		}
		return list;
	}

	private static double GetMarginSize(StiHtmlState state, string marginName)
	{
		double result = 0.7;
		try
		{
			string styleAttribute = state.TS.GetStyleAttribute(marginName);
			if (string.IsNullOrWhiteSpace(styleAttribute))
			{
				styleAttribute = state.TS.GetStyleAttribute("margin");
			}
			if (!string.IsNullOrWhiteSpace(styleAttribute))
			{
				result = ParseSizeToEm(styleAttribute, state.TS.FontSize, null, 0.7);
			}
		}
		catch
		{
		}
		return result;
	}

	private static void ParseHtmlTag(StiHtmlTag2 tag2, ref StiHtmlState state, List<StiHtmlTagsState> stack, StiHtmlState baseState)
	{
		string numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		StiHtmlTagsState item = new StiHtmlTagsState(state.TS);
		TagPair tagPair = tag2.Attributes[0];
		switch (tag2.Tag)
		{
		case StiHtmlTag.B:
			state.TS.Bold = tag2.IsStart;
			break;
		case StiHtmlTag.I:
			state.TS.Italic = tag2.IsStart;
			break;
		case StiHtmlTag.U:
			state.TS.Underline = tag2.IsStart;
			break;
		case StiHtmlTag.S:
			state.TS.Strikeout = tag2.IsStart;
			break;
		case StiHtmlTag.Sup:
			state.TS.Superscript = tag2.IsStart;
			state.TS.Subscript = false;
			break;
		case StiHtmlTag.Sub:
			state.TS.Subscript = tag2.IsStart;
			state.TS.Superscript = false;
			break;
		case StiHtmlTag.LetterSpacing:
			if (tag2.IsStart)
			{
				double letterSpacing = 0.0;
				if (tagPair.Value != "normal")
				{
					letterSpacing = ParseSizeToEm(tagPair.Value, state.TS.FontSize, numberDecimalSeparator);
				}
				state.TS.LetterSpacing = letterSpacing;
			}
			break;
		case StiHtmlTag.WordSpacing:
			if (tag2.IsStart)
			{
				double wordSpacing = 0.0;
				if (tagPair.Value != "normal")
				{
					wordSpacing = ParseSizeToEm(tagPair.Value, state.TS.FontSize, numberDecimalSeparator);
				}
				state.TS.WordSpacing = wordSpacing;
			}
			break;
		case StiHtmlTag.LineHeight:
			if (tag2.IsStart)
			{
				double num2 = 1.0;
				if (tagPair.Value != "normal")
				{
					num2 = ParseSizeToEm(tagPair.Value, state.TS.FontSize, numberDecimalSeparator, 1.0);
				}
				if (num2 < 0.0)
				{
					num2 = 1.0;
				}
				state.TS.LineHeight = num2;
			}
			break;
		case StiHtmlTag.TextAlign:
			if (!tag2.IsStart)
			{
				break;
			}
			try
			{
				string text = tagPair.Value.ToLowerInvariant();
				if (text == "left")
				{
					state.TS.TextAlign = StiTextHorAlignment.Left;
				}
				if (text == "right")
				{
					state.TS.TextAlign = StiTextHorAlignment.Right;
				}
				if (text == "center")
				{
					state.TS.TextAlign = StiTextHorAlignment.Center;
				}
				if (text == "justify")
				{
					state.TS.TextAlign = StiTextHorAlignment.Width;
				}
			}
			catch
			{
			}
			break;
		case StiHtmlTag.Font:
		{
			if (!tag2.IsStart || tag2.Attributes.Count <= 1)
			{
				break;
			}
			for (int i = 1; i < tag2.Attributes.Count; i++)
			{
				TagPair tagPair2 = tag2.Attributes[i];
				switch (tagPair2.Key)
				{
				case "color":
					try
					{
						state.TS.FontColor = ParseColor(tagPair2.Value, baseState.TS.FontColor);
						state.TS.IsColorChanged = true;
					}
					catch
					{
					}
					break;
				case "face":
				case "family":
				case "name":
					try
					{
						state.TS.FontName = tagPair2.Value;
					}
					catch
					{
					}
					break;
				case "size":
				{
					float fontSize2 = ParseFontSize(tagPair2.Value, numberDecimalSeparator);
					state.TS.FontSize = fontSize2;
					break;
				}
				case "style":
					state.TS.HtmlStyle = tagPair2.Value;
					break;
				}
			}
			break;
		}
		case StiHtmlTag.FontName:
			if (tag2.IsStart)
			{
				try
				{
					state.TS.FontName = tagPair.Value;
				}
				catch
				{
				}
			}
			break;
		case StiHtmlTag.FontSize:
			if (tag2.IsStart)
			{
				float fontSize = ParseFontSize(tagPair.Value, numberDecimalSeparator);
				state.TS.FontSize = fontSize;
			}
			break;
		case StiHtmlTag.FontColor:
			if (tag2.IsStart)
			{
				try
				{
					state.TS.FontColor = ParseColor(tagPair.Value, baseState.TS.FontColor);
					state.TS.IsColorChanged = true;
				}
				catch
				{
				}
			}
			break;
		case StiHtmlTag.Backcolor:
			if (tag2.IsStart)
			{
				try
				{
					state.TS.BackColor = ParseColor(tagPair.Value, baseState.TS.BackColor);
					state.TS.IsBackcolorChanged = true;
				}
				catch
				{
				}
			}
			break;
		case StiHtmlTag.A:
			if (!tag2.IsStart)
			{
				break;
			}
			try
			{
				if (tag2.Attributes.Count > 1 && tag2.Attributes[1].Key == "href")
				{
					string value = tag2.Attributes[1].Value;
					if (!string.IsNullOrWhiteSpace(value))
					{
						state.TS.Href = value.Trim();
						state.TS.FontColor = Color.Blue;
						state.TS.IsColorChanged = true;
					}
				}
			}
			catch
			{
			}
			break;
		case StiHtmlTag.Unknown:
			if (tag2.TagName == "stihtml")
			{
				stack.Clear();
				try
				{
					if (tag2.Attributes.Count > 1 && tag2.Attributes[1].KeyBase != null)
					{
						stack.AddRange(StringToStack(tag2.Attributes[1].KeyBase, baseState.TS));
					}
				}
				catch
				{
				}
			}
			if (!(tag2.TagName == "stihtml2"))
			{
				break;
			}
			try
			{
				if (tag2.Attributes.Count > 2 && tag2.Attributes[2].Key != null)
				{
					state.ListLevels = StringToListLevels(tag2.Attributes[2].Key);
					if (state.ListLevels != null)
					{
						state.TS.Indent = state.ListLevels.Count;
					}
					int num = int.Parse(tag2.Attributes[1].Key.ToString());
					if (num > 0)
					{
						state.TS.Indent = -num;
					}
				}
			}
			catch
			{
			}
			break;
		}
		if (tag2.IsEnd)
		{
			if (stack.Count > 0)
			{
				for (int num3 = stack.Count - 1; num3 >= 0; num3--)
				{
					StiHtmlTagsState tS = stack[num3];
					if (tS.Tag.Equals(tag2))
					{
						state.TS = tS;
						stack.RemoveRange(num3, stack.Count - num3);
						break;
					}
				}
			}
		}
		else if (tag2.Tag != 0)
		{
			item.Tag = tag2;
			stack.Add(item);
		}
		if (tag2.IsStart && tag2.Attributes.Count > 1)
		{
			ParseStyleAttributes(tag2, ref state, baseState.TS);
		}
	}

	private static List<TagPair> ParseTagIntoPairs(string tag)
	{
		List<TagPair> list = new List<TagPair>();
		int i;
		for (i = 0; i < tag.Length && tag[i] == ' '; i++)
		{
		}
		while (i < tag.Length)
		{
			int j = i;
			TagPair tagPair = new TagPair();
			for (; j < tag.Length && tag[j] != ' ' && tag[j] != '='; j++)
			{
			}
			tagPair.KeyBase = tag.Substring(i, j - i);
			tagPair.Key = tagPair.KeyBase.ToLowerInvariant();
			for (i = j; i < tag.Length && tag[i] == ' '; i++)
			{
			}
			if (i < tag.Length && tag[i] == '=')
			{
				for (i++; i < tag.Length && tag[i] == ' '; i++)
				{
				}
				if (i < tag.Length)
				{
					if (tag[i] == '"')
					{
						i++;
						for (j = i; j < tag.Length && tag[j] != '"'; j++)
						{
						}
						tagPair.Value = tag.Substring(i, j - i);
						i = j;
						i++;
					}
					else
					{
						for (j = i; j < tag.Length && tag[j] != ' '; j++)
						{
						}
						tagPair.Value = tag.Substring(i, j - i);
						i = j;
					}
				}
			}
			for (; i < tag.Length && tag[i] == ' '; i++)
			{
			}
			list.Add(tagPair);
		}
		return list;
	}

	private static void ParseMarkerTypeAttribute(List<TagPair> attr, ref StiHtmlState state)
	{
		foreach (TagPair item in attr)
		{
			if (item.KeyBase == "type" && !string.IsNullOrWhiteSpace(item.Value) && state.ListLevels != null)
			{
				char c = item.Value[0];
				if (item.Value == "disc")
				{
					c = '•';
				}
				if (item.Value == "circle")
				{
					c = '◦';
				}
				if (item.Value == "square")
				{
					c = '▪';
				}
				if (item.Value == "none")
				{
					c = ' ';
				}
				if (item.Value == "I")
				{
					state.ListLevels[state.ListLevels.Count - 1] = 32768;
				}
				else
				{
					state.ListLevels[state.ListLevels.Count - 1] = 0 - c;
				}
			}
		}
	}

	private static void ParseStyleAttributes(StiHtmlTag2 tag2, ref StiHtmlState state, StiHtmlTagsState baseTS)
	{
		for (int i = 1; i < tag2.Attributes.Count; i++)
		{
			TagPair tagPair = tag2.Attributes[i];
			if (tagPair.Key == "style")
			{
				ParseStyleAttribute(tagPair.Value, ref state, baseTS);
			}
		}
	}

	private static void ParseStyleAttribute(string style, ref StiHtmlState state, StiHtmlTagsState baseTS)
	{
		if (string.IsNullOrEmpty(style))
		{
			return;
		}
		string numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		string[] array = style.Trim().Split(new char[1] { ';' });
		string[] array2 = array;
		foreach (string text in array2)
		{
			string[] array3 = text.Split(new char[1] { ':' });
			bool flag = array3.Length > 1;
			string text2 = array3[0].Trim();
			string text3 = (flag ? array3[1].Trim() : null);
			switch (text2)
			{
			case "color":
				try
				{
					state.TS.FontColor = ParseColor(text3, baseTS.FontColor);
					state.TS.IsColorChanged = true;
				}
				catch
				{
				}
				continue;
			case "background-color":
				try
				{
					state.TS.BackColor = ParseColor(text3, baseTS.BackColor);
					state.TS.IsBackcolorChanged = true;
				}
				catch
				{
				}
				continue;
			case "text-decoration":
				if (text3 == "underline")
				{
					state.TS.Underline = true;
				}
				if (text3 == "line-through")
				{
					state.TS.Strikeout = true;
				}
				if (text3 == "none")
				{
					state.TS.Underline = false;
					state.TS.Strikeout = false;
				}
				continue;
			case "font-weight":
			{
				ref StiHtmlTagsState tS = ref state.TS;
				int bold;
				switch (text3)
				{
				default:
					bold = ((text3 == "900") ? 1 : 0);
					break;
				case "bold":
				case "bolder":
				case "600":
				case "700":
				case "800":
					bold = 1;
					break;
				}
				tS.Bold = (byte)bold != 0;
				continue;
			}
			case "font-style":
				if (text3 == "normal")
				{
					state.TS.Italic = false;
				}
				if (text3 == "italic" || text3 == "oblique")
				{
					state.TS.Italic = true;
				}
				continue;
			case "font-size":
			{
				float fontSize = ParseFontSize(text3, numberDecimalSeparator);
				state.TS.FontSize = fontSize;
				continue;
			}
			case "font-face":
			case "font-family":
			case "font-name":
				try
				{
					state.TS.FontName = text3.Replace("'", "").Replace("\"", "");
				}
				catch
				{
				}
				continue;
			case "vertical-align":
				if (text3 == "baseline")
				{
					state.TS.Subscript = false;
					state.TS.Superscript = false;
				}
				if (text3 == "sub")
				{
					state.TS.Subscript = true;
					state.TS.Superscript = false;
				}
				if (text3 == "super")
				{
					state.TS.Subscript = false;
					state.TS.Superscript = true;
				}
				continue;
			case "letter-spacing":
			{
				double letterSpacing = 0.0;
				if (text3 != "normal")
				{
					letterSpacing = ParseSizeToEm(text3, state.TS.FontSize, numberDecimalSeparator);
				}
				state.TS.LetterSpacing = letterSpacing;
				continue;
			}
			case "word-spacing":
			{
				double wordSpacing = 0.0;
				if (text3 != "normal")
				{
					wordSpacing = ParseSizeToEm(text3, state.TS.FontSize, numberDecimalSeparator);
				}
				state.TS.WordSpacing = wordSpacing;
				continue;
			}
			case "line-height":
			{
				double num = 1.0;
				if (text3 != "normal")
				{
					num = ParseSizeToEm(text3, state.TS.FontSize, numberDecimalSeparator, 1.0);
				}
				if (num < 0.0)
				{
					num = 1.0;
				}
				state.TS.LineHeight = num;
				continue;
			}
			case "text-align":
			{
				string text4 = text3.ToLowerInvariant();
				if (text4 == "left")
				{
					state.TS.TextAlign = StiTextHorAlignment.Left;
				}
				if (text4 == "right")
				{
					state.TS.TextAlign = StiTextHorAlignment.Right;
				}
				if (text4 == "center")
				{
					state.TS.TextAlign = StiTextHorAlignment.Center;
				}
				if (text4 == "justify")
				{
					state.TS.TextAlign = StiTextHorAlignment.Width;
				}
				continue;
			}
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				if (state.TS.StyleAttributes == null)
				{
					state.TS.StyleAttributes = new Hashtable();
				}
				state.TS.StyleAttributes[text2] = text3;
			}
		}
	}

	public static StringBuilder PrepareStateText(StringBuilder stateText)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		while (num < stateText.Length)
		{
			bool flag = false;
			if (stateText[num] == '&' && num + 3 < stateText.Length)
			{
				int i = num + 1;
				StringBuilder stringBuilder2 = new StringBuilder();
				if (stateText[i] == '#')
				{
					i++;
					if (stateText[i] == 'x')
					{
						for (i++; i < stateText.Length && char.IsLetterOrDigit(stateText[i]); i++)
						{
							stringBuilder2.Append(stateText[i]);
						}
						if (stringBuilder2.Length > 0 && i < stateText.Length && stateText[i] == ';')
						{
							i++;
							uint result = 0u;
							if (uint.TryParse(stringBuilder2.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result))
							{
								stringBuilder.Append((char)result);
								num = i;
								flag = true;
							}
						}
					}
					else
					{
						for (; i < stateText.Length && char.IsDigit(stateText[i]); i++)
						{
							stringBuilder2.Append(stateText[i]);
						}
						if (stringBuilder2.Length > 0 && i < stateText.Length && stateText[i] == ';')
						{
							i++;
							stringBuilder.Append((char)uint.Parse(stringBuilder2.ToString()));
							num = i;
							flag = true;
						}
					}
				}
				else
				{
					for (; i < stateText.Length && char.IsLetterOrDigit(stateText[i]); i++)
					{
						stringBuilder2.Append(stateText[i]);
					}
					if (stringBuilder2.Length > 0 && i < stateText.Length && stateText[i] == ';')
					{
						object obj = HtmlEscapeSequence["&" + stringBuilder2?.ToString() + ";"];
						if (obj != null)
						{
							i++;
							stringBuilder.Append((char)obj);
							num = i;
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				stringBuilder.Append(stateText[num]);
				num++;
			}
		}
		return stringBuilder;
	}

	private static Color ParseColor(string colorAttribute, Color inheritColor)
	{
		if (colorAttribute.ToLowerInvariant() == "inherit")
		{
			return inheritColor;
		}
		Color result = Color.Transparent;
		if (colorAttribute.Length > 1)
		{
			if (colorAttribute[0] == '#')
			{
				string text = colorAttribute.Substring(1).ToLowerInvariant();
				StringBuilder stringBuilder = new StringBuilder();
				string text2 = text;
				foreach (char c in text2)
				{
					if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f')
					{
						stringBuilder.Append(c);
					}
				}
				text = ((stringBuilder.Length != 3) ? stringBuilder.ToString() : string.Format("{0}{0}{1}{1}{2}{2}", stringBuilder[0], stringBuilder[1], stringBuilder[2]));
				if (text.Length == 6)
				{
					int num = Convert.ToInt32(text, 16);
					result = Color.FromArgb(255, (num >> 16) & 0xFF, (num >> 8) & 0xFF, num & 0xFF);
				}
			}
			else if (colorAttribute.StartsWith("rgb", StringComparison.InvariantCulture))
			{
				string[] array = colorAttribute.Trim().Substring(4, colorAttribute.Length - 5).Split(new char[1] { ',' });
				if (array.Length == 3)
				{
					int[] array2 = new int[3];
					if (array[0].EndsWith("%", StringComparison.InvariantCulture))
					{
						array2[0] = (int)Math.Round((double)Convert.ToInt32(array[0].Substring(0, array[0].Length - 1)) * 2.55);
					}
					else
					{
						array2[0] = Convert.ToInt32(array[0]);
					}
					if (array[1].EndsWith("%", StringComparison.InvariantCulture))
					{
						array2[1] = (int)Math.Round((double)Convert.ToInt32(array[1].Substring(0, array[1].Length - 1)) * 2.55);
					}
					else
					{
						array2[1] = Convert.ToInt32(array[1]);
					}
					if (array[2].EndsWith("%", StringComparison.InvariantCulture))
					{
						array2[2] = (int)Math.Round((double)Convert.ToInt32(array[2].Substring(0, array[2].Length - 1)) * 2.55);
					}
					else
					{
						array2[2] = Convert.ToInt32(array[2]);
					}
					result = Color.FromArgb(255, array2[0], array2[1], array2[2]);
				}
			}
			else
			{
				lock (lockHtmlNameToColor)
				{
					if (htmlNameToColor == null)
					{
						string[,] array3 = new string[178, 2]
						{
							{ "AliceBlue", "#F0F8FF" },
							{ "AntiqueWhite", "#FAEBD7" },
							{ "Aqua", "#00FFFF" },
							{ "Aquamarine", "#7FFFD4" },
							{ "Azure", "#F0FFFF" },
							{ "Beige", "#F5F5DC" },
							{ "Bisque", "#FFE4C4" },
							{ "Black", "#000000" },
							{ "BlanchedAlmond", "#FFEBCD" },
							{ "Blue", "#0000FF" },
							{ "BlueViolet", "#8A2BE2" },
							{ "Brown", "#A52A2A" },
							{ "BurlyWood", "#DEB887" },
							{ "CadetBlue", "#5F9EA0" },
							{ "Chartreuse", "#7FFF00" },
							{ "Chocolate", "#D2691E" },
							{ "Coral", "#FF7F50" },
							{ "CornflowerBlue", "#6495ED" },
							{ "Cornsilk", "#FFF8DC" },
							{ "Crimson", "#DC143C" },
							{ "Cyan", "#00FFFF" },
							{ "DarkBlue", "#00008B" },
							{ "DarkCyan", "#008B8B" },
							{ "DarkGoldenRod", "#B8860B" },
							{ "DarkGray", "#A9A9A9" },
							{ "DarkGrey", "#A9A9A9" },
							{ "DarkGreen", "#006400" },
							{ "DarkKhaki", "#BDB76B" },
							{ "DarkMagenta", "#8B008B" },
							{ "DarkOliveGreen", "#556B2F" },
							{ "Darkorange", "#FF8C00" },
							{ "DarkOrchid", "#9932CC" },
							{ "DarkRed", "#8B0000" },
							{ "DarkSalmon", "#E9967A" },
							{ "DarkSeaGreen", "#8FBC8F" },
							{ "DarkSlateBlue", "#483D8B" },
							{ "DarkSlateGray", "#2F4F4F" },
							{ "DarkSlateGrey", "#2F4F4F" },
							{ "DarkTurquoise", "#00CED1" },
							{ "DarkViolet", "#9400D3" },
							{ "DeepPink", "#FF1493" },
							{ "DeepSkyBlue", "#00BFFF" },
							{ "DimGray", "#696969" },
							{ "DimGrey", "#696969" },
							{ "DodgerBlue", "#1E90FF" },
							{ "FireBrick", "#B22222" },
							{ "FloralWhite", "#FFFAF0" },
							{ "ForestGreen", "#228B22" },
							{ "Fuchsia", "#FF00FF" },
							{ "Gainsboro", "#DCDCDC" },
							{ "GhostWhite", "#F8F8FF" },
							{ "Gold", "#FFD700" },
							{ "GoldenRod", "#DAA520" },
							{ "Gray", "#808080" },
							{ "Grey", "#808080" },
							{ "Green", "#008000" },
							{ "GreenYellow", "#ADFF2F" },
							{ "HoneyDew", "#F0FFF0" },
							{ "HotPink", "#FF69B4" },
							{ "IndianRed", "#CD5C5C" },
							{ "Indigo", "#4B0082" },
							{ "Ivory", "#FFFFF0" },
							{ "Khaki", "#F0E68C" },
							{ "Lavender", "#E6E6FA" },
							{ "LavenderBlush", "#FFF0F5" },
							{ "LawnGreen", "#7CFC00" },
							{ "LemonChiffon", "#FFFACD" },
							{ "LightBlue", "#ADD8E6" },
							{ "LightCoral", "#F08080" },
							{ "LightCyan", "#E0FFFF" },
							{ "LightGoldenRodYellow", "#FAFAD2" },
							{ "LightGray", "#D3D3D3" },
							{ "LightGrey", "#D3D3D3" },
							{ "LightGreen", "#90EE90" },
							{ "LightPink", "#FFB6C1" },
							{ "LightSalmon", "#FFA07A" },
							{ "LightSeaGreen", "#20B2AA" },
							{ "LightSkyBlue", "#87CEFA" },
							{ "LightSlateGray", "#778899" },
							{ "LightSlateGrey", "#778899" },
							{ "LightSteelBlue", "#B0C4DE" },
							{ "LightYellow", "#FFFFE0" },
							{ "Lime", "#00FF00" },
							{ "LimeGreen", "#32CD32" },
							{ "Linen", "#FAF0E6" },
							{ "Magenta", "#FF00FF" },
							{ "Maroon", "#800000" },
							{ "MediumAquaMarine", "#66CDAA" },
							{ "MediumBlue", "#0000CD" },
							{ "MediumOrchid", "#BA55D3" },
							{ "MediumPurple", "#9370D8" },
							{ "MediumSeaGreen", "#3CB371" },
							{ "MediumSlateBlue", "#7B68EE" },
							{ "MediumSpringGreen", "#00FA9A" },
							{ "MediumTurquoise", "#48D1CC" },
							{ "MediumVioletRed", "#C71585" },
							{ "MidnightBlue", "#191970" },
							{ "MintCream", "#F5FFFA" },
							{ "MistyRose", "#FFE4E1" },
							{ "Moccasin", "#FFE4B5" },
							{ "NavajoWhite", "#FFDEAD" },
							{ "Navy", "#000080" },
							{ "OldLace", "#FDF5E6" },
							{ "Olive", "#808000" },
							{ "OliveDrab", "#6B8E23" },
							{ "Orange", "#FFA500" },
							{ "OrangeRed", "#FF4500" },
							{ "Orchid", "#DA70D6" },
							{ "PaleGoldenRod", "#EEE8AA" },
							{ "PaleGreen", "#98FB98" },
							{ "PaleTurquoise", "#AFEEEE" },
							{ "PaleVioletRed", "#D87093" },
							{ "PapayaWhip", "#FFEFD5" },
							{ "PeachPuff", "#FFDAB9" },
							{ "Peru", "#CD853F" },
							{ "Pink", "#FFC0CB" },
							{ "Plum", "#DDA0DD" },
							{ "PowderBlue", "#B0E0E6" },
							{ "Purple", "#800080" },
							{ "Red", "#FF0000" },
							{ "RosyBrown", "#BC8F8F" },
							{ "RoyalBlue", "#4169E1" },
							{ "SaddleBrown", "#8B4513" },
							{ "Salmon", "#FA8072" },
							{ "SandyBrown", "#F4A460" },
							{ "SeaGreen", "#2E8B57" },
							{ "SeaShell", "#FFF5EE" },
							{ "Sienna", "#A0522D" },
							{ "Silver", "#C0C0C0" },
							{ "SkyBlue", "#87CEEB" },
							{ "SlateBlue", "#6A5ACD" },
							{ "SlateGray", "#708090" },
							{ "SlateGrey", "#708090" },
							{ "Snow", "#FFFAFA" },
							{ "SpringGreen", "#00FF7F" },
							{ "SteelBlue", "#4682B4" },
							{ "Tan", "#D2B48C" },
							{ "Teal", "#008080" },
							{ "Thistle", "#D8BFD8" },
							{ "Tomato", "#FF6347" },
							{ "Turquoise", "#40E0D0" },
							{ "Violet", "#EE82EE" },
							{ "Wheat", "#F5DEB3" },
							{ "White", "#FFFFFF" },
							{ "WhiteSmoke", "#F5F5F5" },
							{ "Yellow", "#FFFF00" },
							{ "YellowGreen", "#9ACD32" },
							{ "ActiveBorder", "#B4B4B4" },
							{ "ActiveCaption", "#99B4D1" },
							{ "AppWorkspace", "#ABABAB" },
							{ "Background", "#000000" },
							{ "ButtonAlternateFace", "#000000" },
							{ "ButtonDkShadow", "#696969" },
							{ "ButtonFace", "#F0F0F0" },
							{ "ButtonHighlight", "#FFFFFF" },
							{ "ButtonLight", "#E3E3E3" },
							{ "ButtonShadow", "#A0A0A0" },
							{ "ButtonText", "#000000" },
							{ "GradientActiveTitle", "#B9D1EA" },
							{ "GradientInactiveTitle", "#D7E4F2" },
							{ "GrayText", "#6D6D6D" },
							{ "Highlight", "#0078D7" },
							{ "HighlightText", "#FFFFFF" },
							{ "HotTrackingColor", "#0066CC" },
							{ "InactiveBorder", "#F4F7FC" },
							{ "InactiveCaption", "#BFCDDB" },
							{ "InactiveCaptionText", "#000000" },
							{ "InfoText", "#000000" },
							{ "InfoBackground", "#FFFFFF" },
							{ "Menu", "#F0F0F0" },
							{ "MenuBar", "#F0F0F0" },
							{ "MenuHighlight", "#0078D7" },
							{ "MenuText", "#000000" },
							{ "Scrollbar", "#C8C8C8" },
							{ "CaptionText", "#000000" },
							{ "Window", "#FFFFFF" },
							{ "WindowFrame", "#646464" },
							{ "WindowText", "#000000" }
						};
						htmlNameToColor = new Hashtable();
						for (int j = 0; j < array3.GetLength(0); j++)
						{
							string key = array3[j, 0].ToLowerInvariant();
							int num2 = Convert.ToInt32(array3[j, 1].Substring(1), 16);
							Color color = Color.FromArgb(255, (num2 >> 16) & 0xFF, (num2 >> 8) & 0xFF, num2 & 0xFF);
							htmlNameToColor[key] = color;
						}
					}
				}
				string key2 = colorAttribute.ToLowerInvariant();
				if (htmlNameToColor.ContainsKey(key2))
				{
					result = (Color)htmlNameToColor[key2];
				}
			}
		}
		return result;
	}

	private static string ColorToHtml(Color color)
	{
		string result = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
		if (color.A != byte.MaxValue)
		{
			result = "#ttt";
		}
		return result;
	}

	private static float ParseFontSize(string fontSizeAttribute, string delimiter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Invalid comparison between Unknown and I4
		GraphicsUnit val = (GraphicsUnit)3;
		if (fontSizeAttribute.EndsWith("px"))
		{
			val = (GraphicsUnit)2;
			fontSizeAttribute = fontSizeAttribute.Substring(0, fontSizeAttribute.Length - 2);
		}
		if (fontSizeAttribute.EndsWith("pt"))
		{
			fontSizeAttribute = fontSizeAttribute.Substring(0, fontSizeAttribute.Length - 2);
		}
		float result = 8f;
		if (!float.TryParse(fontSizeAttribute.Replace(',', '.').Replace(".", delimiter), out result))
		{
			result = 8f;
		}
		if ((double)result < 0.5)
		{
			result = 0.5f;
		}
		if (InterpreteFontSizeInHtmlTagsAsInHtml)
		{
			switch ((int)Math.Round(result))
			{
			case 1:
				result = 7f;
				break;
			case 2:
				result = 10f;
				break;
			case 3:
				result = 12f;
				break;
			case 4:
				result = 14f;
				break;
			case 5:
				result = 16f;
				break;
			case 6:
				result = 22f;
				break;
			case 7:
				result = 36f;
				break;
			}
		}
		if ((int)val == 2)
		{
			result *= 0.75f;
		}
		return result;
	}

	private static double ParseSizeToEm(string value, float emSize, string delimiter = null, double defaultValue = 0.0)
	{
		double num = 1.0;
		if (value.EndsWith("em"))
		{
			value = value.Substring(0, value.Length - 2);
		}
		else if (value.EndsWith("pt"))
		{
			value = value.Substring(0, value.Length - 2);
			num = 1f / emSize;
		}
		else if (value.EndsWith("px"))
		{
			value = value.Substring(0, value.Length - 2);
			num = 0.75 / (double)emSize;
		}
		else if (value.EndsWith("in"))
		{
			value = value.Substring(0, value.Length - 2);
			num = 72f / emSize;
		}
		else if (value.EndsWith("cm"))
		{
			value = value.Substring(0, value.Length - 2);
			num = 28.346456692913385 / (double)emSize;
		}
		else if (value.EndsWith("mm"))
		{
			value = value.Substring(0, value.Length - 2);
			num = 2.834645669291339 / (double)emSize;
		}
		else if (value.EndsWith("%"))
		{
			value = value.Substring(0, value.Length - 1);
			num = 0.01;
		}
		if (delimiter == null)
		{
			delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		}
		if (double.TryParse(value.Trim().Replace(',', '.').Replace(".", delimiter), out var result))
		{
			return result * num;
		}
		return defaultValue;
	}

	public static string StateToHtml(StiHtmlState state, StiHtmlState state2, string text, int lineInfoIndent)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"<font name=\"{state.TS.FontName}\" size=\"{state.TS.FontSize}\">");
		if (state2.TS.IsColorChanged)
		{
			stringBuilder.Append($"<font-color=\"{ColorToHtml(state2.TS.FontColor)}\">");
		}
		if (state2.TS.IsBackcolorChanged)
		{
			stringBuilder.Append($"<background-color=\"{ColorToHtml(state2.TS.BackColor)}\">");
		}
		stringBuilder.Append(string.Format("<{0}b>", state.TS.Bold ? "" : "/"));
		stringBuilder.Append(string.Format("<{0}i>", state.TS.Italic ? "" : "/"));
		stringBuilder.Append(string.Format("<{0}u>", state.TS.Underline ? "" : "/"));
		stringBuilder.Append(string.Format("<{0}s>", state.TS.Strikeout ? "" : "/"));
		stringBuilder.Append(string.Format("<{0}sup>", state.TS.Superscript ? "" : "/"));
		stringBuilder.Append(string.Format("<{0}sub>", state.TS.Subscript ? "" : "/"));
		stringBuilder.Append($"<letter-spacing=\"{state.TS.LetterSpacing}\">");
		stringBuilder.Append($"<word-spacing=\"{state.TS.WordSpacing}\">");
		stringBuilder.Append($"<line-height=\"{state2.TS.LineHeight}\">");
		string arg = "left";
		if (state.TS.TextAlign == StiTextHorAlignment.Center)
		{
			arg = "center";
		}
		if (state.TS.TextAlign == StiTextHorAlignment.Right)
		{
			arg = "right";
		}
		if (state.TS.TextAlign == StiTextHorAlignment.Width)
		{
			arg = "justify";
		}
		stringBuilder.Append($"<text-align=\"{arg}\">");
		stringBuilder.Append("<StiHtml " + StackToString(state.TagsStack) + ">");
		if (state2.TS.Indent > 0)
		{
			stringBuilder.Append($"<StiHtml2 {lineInfoIndent} {ListLevelsToString(state2.ListLevels, state2.TS.Indent)}>");
		}
		if (text != null)
		{
			stringBuilder.Append(text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"));
		}
		return stringBuilder.ToString();
	}

	private static string GetIndentString(int indent)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < indent; i++)
		{
			stringBuilder.Append("\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0");
		}
		return stringBuilder.ToString();
	}

	private static void InsertMarker(StringBuilder sb, int markerInt, int indent)
	{
		string text = bulletBlack;
		if (markerInt > 0)
		{
			text = ((markerInt < 32768) ? (markerInt + ".") : (ToRoman(markerInt - 32768 + 1) + "."));
		}
		else if (markerInt < -32)
		{
			text = ((char)(-markerInt)).ToString();
		}
		else
		{
			int num = -markerInt % 2;
			if (num == 1)
			{
				text = bulletWhite;
			}
		}
		int num2 = ((markerInt > 0) ? 2 : 3);
		if (sb.Length <= 3)
		{
			return;
		}
		if (text.Length >= sb.Length - num2)
		{
			sb.Remove(0, sb.Length - num2);
			sb.Insert(0, text);
			return;
		}
		int num3 = sb.Length - num2 - text.Length;
		for (int i = 0; i < text.Length; i++)
		{
			sb[num3 + i] = text[i];
		}
	}

	public static bool CheckTextForHtmlTags(string text)
	{
		if (string.IsNullOrWhiteSpace(text) || text.Length < 4)
		{
			return false;
		}
		int num = text.IndexOf("<");
		if (num != -1)
		{
			int num2 = text.IndexOf(">", num + 2);
			if (num2 != -1)
			{
				List<StiHtmlState> list = ParseHtmlToStates(text, new StiHtmlState
				{
					Text = new StringBuilder("a")
				});
				if (list.Count > 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static string GetPlainTextFromHtmlTags(string input)
	{
		StiHtmlState baseState = new StiHtmlState(new StiHtmlTagsState(bold: false, italic: false, underline: false, strikeout: false, 8f, "Arial", Color.Black, Color.White, superscript: false, subscript: false, 0.0, 0.0, 1.0, StiTextHorAlignment.Left), 0);
		List<StiHtmlState> list = ParseHtmlToStates(input, baseState);
		StringBuilder stringBuilder = new StringBuilder();
		foreach (StiHtmlState item in list)
		{
			if (item.Text.Length == 1 && item.Text[0] == '\n')
			{
				stringBuilder.Append("\r\n");
			}
			else
			{
				stringBuilder.Append((object?)PrepareStateText(item.Text));
			}
		}
		return stringBuilder.ToString();
	}

	private static string StackToString(List<StiHtmlTagsState> stack)
	{
		if (stack == null || stack.Count == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			for (int i = 0; i < stack.Count; i++)
			{
				StiHtmlTagsState stiHtmlTagsState = stack[i];
				StiHtmlTagsState stiHtmlTagsState2 = default(StiHtmlTagsState);
				bool flag = i == 0;
				if (!flag)
				{
					stiHtmlTagsState2 = stack[i - 1];
				}
				int length = stringBuilder.Length;
				if (stiHtmlTagsState.IsBackcolorChanged)
				{
					stringBuilder.AppendFormat("bc{0:X2}{1:X2}{2:X2}{3:X2}:", stiHtmlTagsState.BackColor.A, stiHtmlTagsState.BackColor.R, stiHtmlTagsState.BackColor.G, stiHtmlTagsState.BackColor.B);
				}
				if (stiHtmlTagsState.Bold && (flag || stiHtmlTagsState.Bold != stiHtmlTagsState2.Bold))
				{
					stringBuilder.Append("bd:");
				}
				if (stiHtmlTagsState.IsColorChanged)
				{
					stringBuilder.AppendFormat("fc{0:X2}{1:X2}{2:X2}{3:X2}:", stiHtmlTagsState.FontColor.A, stiHtmlTagsState.FontColor.R, stiHtmlTagsState.FontColor.G, stiHtmlTagsState.FontColor.B);
				}
				if (!string.IsNullOrEmpty(stiHtmlTagsState.FontName) && (flag || stiHtmlTagsState.FontName != stiHtmlTagsState2.FontName))
				{
					stringBuilder.AppendFormat("fn{0}:", stiHtmlTagsState.FontName.Replace(' ', '_'));
				}
				if (flag || stiHtmlTagsState.FontSize != stiHtmlTagsState2.FontSize)
				{
					stringBuilder.AppendFormat("fs{0}:", stiHtmlTagsState.FontSize);
				}
				if (stiHtmlTagsState.Italic && (flag || stiHtmlTagsState.Italic != stiHtmlTagsState2.Italic))
				{
					stringBuilder.Append("it:");
				}
				if (flag || stiHtmlTagsState.LetterSpacing != stiHtmlTagsState2.LetterSpacing)
				{
					stringBuilder.AppendFormat("ls{0}:", stiHtmlTagsState.LetterSpacing);
				}
				if (flag || stiHtmlTagsState.LineHeight != stiHtmlTagsState2.LineHeight)
				{
					stringBuilder.AppendFormat("lh{0}:", stiHtmlTagsState.LineHeight);
				}
				if (stiHtmlTagsState.Strikeout && (flag || stiHtmlTagsState.Strikeout != stiHtmlTagsState2.Strikeout))
				{
					stringBuilder.Append("st:");
				}
				if (stiHtmlTagsState.Subscript && (flag || stiHtmlTagsState.Subscript != stiHtmlTagsState2.Subscript))
				{
					stringBuilder.Append("sb:");
				}
				if (stiHtmlTagsState.Superscript && (flag || stiHtmlTagsState.Superscript != stiHtmlTagsState2.Superscript))
				{
					stringBuilder.Append("sp:");
				}
				if (flag || !stiHtmlTagsState.Tag.Equals(stiHtmlTagsState2.Tag))
				{
					string arg;
					if (stiHtmlTagsState.Tag.Tag == StiHtmlTag.Unknown)
					{
						arg = "'" + stiHtmlTagsState.Tag.TagName + "'";
					}
					else
					{
						int tag = (int)stiHtmlTagsState.Tag.Tag;
						arg = tag.ToString();
					}
					stringBuilder.AppendFormat("tg{0}:", arg);
				}
				if (flag || stiHtmlTagsState.TextAlign != stiHtmlTagsState2.TextAlign)
				{
					stringBuilder.AppendFormat("ta{0}:", (int)stiHtmlTagsState.TextAlign);
				}
				if (stiHtmlTagsState.Underline && (flag || stiHtmlTagsState.Underline != stiHtmlTagsState2.Underline))
				{
					stringBuilder.Append("un:");
				}
				if (flag || stiHtmlTagsState.WordSpacing != stiHtmlTagsState2.WordSpacing)
				{
					stringBuilder.AppendFormat("ws{0}:", stiHtmlTagsState.WordSpacing);
				}
				if (stringBuilder[stringBuilder.Length - 1] == ':')
				{
					stringBuilder.Length--;
				}
				if (i < stack.Count - 1 && stringBuilder.Length > length)
				{
					stringBuilder.Append(";");
				}
			}
			while (stringBuilder.Length > 1 && stringBuilder[stringBuilder.Length - 1] == ';')
			{
				stringBuilder.Length--;
			}
		}
		catch
		{
		}
		return stringBuilder.ToString();
	}

	private static List<StiHtmlTagsState> StringToStack(string inputString, StiHtmlTagsState baseState)
	{
		StiHtmlTagsState state = new StiHtmlTagsState(baseState);
		List<StiHtmlTagsState> list = new List<StiHtmlTagsState>();
		try
		{
			string[] array = inputString.Split(new char[1] { ';' });
			string[] array2 = array;
			foreach (string text in array2)
			{
				StiHtmlTagsState stiHtmlTagsState = new StiHtmlTagsState(state);
				string[] array3 = text.Split(new char[1] { ':' });
				string[] array4 = array3;
				foreach (string text2 in array4)
				{
					string text3 = text2.Substring(2);
					switch (text2.Substring(0, 2))
					{
					case "bc":
						stiHtmlTagsState.BackColor = Color.FromArgb(int.Parse(text3.Substring(0, 2), NumberStyles.HexNumber), int.Parse(text3.Substring(2, 2), NumberStyles.HexNumber), int.Parse(text3.Substring(4, 2), NumberStyles.HexNumber), int.Parse(text3.Substring(6, 2), NumberStyles.HexNumber));
						stiHtmlTagsState.IsBackcolorChanged = true;
						break;
					case "bd":
						stiHtmlTagsState.Bold = true;
						break;
					case "fc":
						stiHtmlTagsState.FontColor = Color.FromArgb(int.Parse(text3.Substring(0, 2), NumberStyles.HexNumber), int.Parse(text3.Substring(2, 2), NumberStyles.HexNumber), int.Parse(text3.Substring(4, 2), NumberStyles.HexNumber), int.Parse(text3.Substring(6, 2), NumberStyles.HexNumber));
						stiHtmlTagsState.IsColorChanged = true;
						break;
					case "fn":
						stiHtmlTagsState.FontName = text3.Replace('_', ' ');
						break;
					case "fs":
						stiHtmlTagsState.FontSize = float.Parse(text3);
						break;
					case "it":
						stiHtmlTagsState.Italic = true;
						break;
					case "ls":
						stiHtmlTagsState.LetterSpacing = double.Parse(text3);
						break;
					case "lh":
						if (!double.TryParse(text3.Replace(',', '.').Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out stiHtmlTagsState.LineHeight))
						{
							stiHtmlTagsState.LineHeight = 1.0;
						}
						break;
					case "st":
						stiHtmlTagsState.Strikeout = true;
						break;
					case "sb":
						stiHtmlTagsState.Subscript = true;
						break;
					case "sp":
						stiHtmlTagsState.Superscript = true;
						break;
					case "tg":
						if (text3.StartsWith("'"))
						{
							stiHtmlTagsState.Tag = ConvertStringToTag(text3.Substring(1, text3.Length - 2));
						}
						else
						{
							stiHtmlTagsState.Tag = new StiHtmlTag2((StiHtmlTag)int.Parse(text3));
						}
						break;
					case "ta":
						stiHtmlTagsState.TextAlign = (StiTextHorAlignment)int.Parse(text3);
						break;
					case "un":
						stiHtmlTagsState.Underline = true;
						break;
					case "ws":
						stiHtmlTagsState.WordSpacing = double.Parse(text3);
						break;
					}
				}
				list.Add(stiHtmlTagsState);
				state = stiHtmlTagsState;
			}
		}
		catch
		{
		}
		return list;
	}

	private static string ListLevelsToString(List<int> list, int indent)
	{
		if (list == null || list.Count == 0)
		{
			list = new List<int>();
		}
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			for (int i = 0; i < indent; i++)
			{
				if (i < list.Count)
				{
					stringBuilder.Append(list[i].ToString());
				}
				else
				{
					stringBuilder.Append("0");
				}
				if (i < indent - 1)
				{
					stringBuilder.Append(";");
				}
			}
		}
		catch
		{
		}
		return stringBuilder.ToString();
	}

	private static List<int> StringToListLevels(string inputString)
	{
		List<int> list = new List<int>();
		try
		{
			string[] array = inputString.Split(new char[1] { ';' });
			string[] array2 = array;
			foreach (string s in array2)
			{
				list.Add(int.Parse(s));
			}
		}
		catch
		{
		}
		return list;
	}

	private static string ToRoman(int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (value > 0)
		{
			for (int num = 5; num >= 0; num--)
			{
				if (value >= Arabics[num])
				{
					stringBuilder.Append(Romans[num]);
					value -= Arabics[num];
					break;
				}
				bool flag = false;
				for (int i = Subs[num]; i < num; i++)
				{
					if (Arabics[i] != Arabics[num] - Arabics[i] && value >= Arabics[num] - Arabics[i])
					{
						stringBuilder.Append(Romans[i]);
						stringBuilder.Append(Romans[num]);
						value -= Arabics[num] - Arabics[i];
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		return stringBuilder.ToString();
	}
}
