namespace Stimulsoft.Report.Export;

internal class PdfFontInfo
{
	public int[] Widths;

	public string[] CharPdfNames;

	public int CH;

	public int XH;

	public int ASC;

	public int DESC;

	public int tmASC;

	public int tmDESC;

	public int tmExternal;

	public int MacAscend;

	public int MacDescend;

	public int MacLineGap;

	public int LLX;

	public int LLY;

	public int URX;

	public int URY;

	public int StemV;

	public int ItalicAngle;

	public int LineGap;

	public uint fsSelection;

	public int UnderscoreSize;

	public int UnderscorePosition;

	public int StrikeoutSize;

	public int StrikeoutPosition;

	public ushort[] UnicodeMap;

	public ushort[] UnicodeMapBack;

	public ushort[] GlyphList;

	public ushort[] GlyphBackList;

	public ushort[] GlyphRtfList;

	public int[] SymsToPDF;

	public int MappedSymbolsCount;

	public bool NeedSyntBold;

	public bool NeedSyntItalic;

	public ushort[] GlyphWidths;

	public byte[] ChildFontsMap;

	public bool UseUnicode;
}
