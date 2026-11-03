using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export.Tools;

[SuppressUnmanagedCodeSecurity]
internal class StiPdfMetafileRender
{
	internal struct SIZEL
	{
		public int cX;

		public int cY;
	}

	internal struct POINTL
	{
		public int X;

		public int Y;
	}

	internal struct RECTL
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct LOGFONT
	{
		public int lfHeight;

		public int lfWidth;

		public int lfEscapement;

		public int lfOrientation;

		public int lfWeight;

		public byte lfItalic;

		public byte lfUnderline;

		public byte lfStrikeOut;

		public byte lfCharSet;

		public byte lfOutPrecision;

		public byte lfClipPrecision;

		public byte lfQuality;

		public byte lfPitchAndFamily;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string lfFaceName;
	}

	internal struct tagENHMETAHEADER
	{
		public RECTL rclBounds;

		public RECTL rclFrame;

		public uint dSignature;

		public uint nVersion;

		public uint nBytes;

		public uint nRecords;

		public ushort nHandles;

		public ushort sReserved;

		public uint nDescription;

		public uint offDescription;

		public uint nPalEntries;

		public SIZEL szlDevice;

		public SIZEL szlMillimeters;

		public uint cbPixelFormat;

		public uint offPixelFormat;

		public uint bOpenGL;
	}

	internal class tagEMREXTTEXTOUT
	{
		public RECTL Bounds;

		public uint iGraphicsMode;

		public float exScale;

		public float eyScale;

		public POINTL Reference;

		public uint nChars;

		public uint offString;

		public uint fOptions;

		public RECTL rcl;

		public uint offDx;
	}

	internal struct XFORM
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
	}

	internal struct tagEMRBITBLT
	{
		public RECTL rclBounds;

		public int xDest;

		public int yDest;

		public int cxDest;

		public int cyDest;

		public uint dwRop;

		public int xSrc;

		public int ySrc;

		public XFORM xformSrc;

		public uint crBkColorSrc;

		public uint iUsageSrc;

		public uint offBmiSrc;

		public uint cbBmiSrc;

		public uint offBitsSrc;

		public uint cbBitsSrc;
	}

	internal struct tagEMRALPHABLEND
	{
		public RECTL rclBounds;

		public int xDest;

		public int yDest;

		public int cxDest;

		public int cyDest;

		public uint dwRop;

		public int xSrc;

		public int ySrc;

		public XFORM xformSrc;

		public uint crBkColorSrc;

		public uint iUsageSrc;

		public uint offBmiSrc;

		public uint cbBmiSrc;

		public uint offBitsSrc;

		public uint cbBitsSrc;

		public int cxSrc;

		public int cySrc;
	}

	internal struct tagEMRSTRETCHDIBITS
	{
		public RECTL rclBounds;

		public int xDest;

		public int yDest;

		public int xSrc;

		public int ySrc;

		public int cxSrc;

		public int cySrc;

		public uint offBmiSrc;

		public uint cbBmiSrc;

		public uint offBitsSrc;

		public uint cbBitsSrc;

		public uint iUsageSrc;

		public uint dwRop;

		public int cxDest;

		public int cyDest;
	}

	internal class EmfFontInfo
	{
		public int internalFontNumber;

		public LOGFONT elfLogFont;
	}

	internal class EmfBrushInfo
	{
		public uint lbStyle;

		public uint lbColor;

		public uint lbHatch;
	}

	internal class EmfPenInfo
	{
		public uint lopnStyle;

		public int lopnWidth;

		public uint lopnColor;
	}

	internal class GDIobjectInfo
	{
		public uint ihNumber;

		public EmfFontInfo Font;

		public EmfBrushInfo Brush;

		public EmfPenInfo Pen;
	}

	internal struct POINTD
	{
		public double X;

		public double Y;
	}

	private class DCData
	{
		public uint emfBackColor;

		public uint emfTextColor;

		public uint emfBkMode;

		public uint emfPolyFillMode;

		public uint emfMapMode;

		public uint emfTextAlign;
	}

	private StiPdfExportService pdfService;

	private PdfFonts pdfFont;

	private StreamWriter pageStream;

	private bool useUnicodeMode;

	private const double hiToTwips = 0.72;

	private const double fontCorrectValue = 0.955;

	private int precision_digits_font = 3;

	private const float italicAngleTanValue = 0.325f;

	private const float boldFontStrokeWidthValue = 0.031f;

	internal const uint ETO_OPAQUE = 2u;

	internal const uint ETO_CLIPPED = 4u;

	internal const uint ETO_GLYPH_INDEX = 16u;

	internal const uint ETO_PDY = 8192u;

	internal const int LF_FULLFACESIZE = 64;

	internal const int LF_FACESIZE = 32;

	internal const int GM_COMPATIBLE = 1;

	internal const int GM_ADVANCED = 2;

	internal const uint SRCCOPY = 13369376u;

	internal const uint SRCPAINT = 15597702u;

	internal const uint SRCAND = 8913094u;

	internal const uint SRCINVERT = 6684742u;

	internal const uint SRCERASE = 4457256u;

	internal const uint NOTSRCCOPY = 3342344u;

	internal const uint NOTSRCERASE = 1114278u;

	internal const uint MERGECOPY = 12583114u;

	internal const uint MERGEPAINT = 12255782u;

	internal const uint PATCOPY = 15728673u;

	internal const uint PATPAINT = 16452105u;

	internal const uint PATINVERT = 5898313u;

	internal const uint DSTINVERT = 5570569u;

	internal const uint BLACKNESS = 66u;

	internal const uint WHITENESS = 16711778u;

	internal const uint NOMIRRORBITMAP = 2147483648u;

	internal const uint CAPTUREBLT = 1073741824u;

	internal const uint TA_NOUPDATECP = 0u;

	internal const uint TA_UPDATECP = 1u;

	internal const uint TA_LEFT = 0u;

	internal const uint TA_RIGHT = 2u;

	internal const uint TA_CENTER = 6u;

	internal const uint TA_TOP = 0u;

	internal const uint TA_BOTTOM = 8u;

	internal const uint TA_BASELINE = 24u;

	internal const uint TA_RTLREADING = 256u;

	internal const uint TA_MASK = 287u;

	internal const uint BKMODE_TRANSPARENT = 1u;

	internal const uint BKMODE_OPAQUE = 2u;

	private byte[] dataArray;

	private int dataOffset;

	private uint emfSelectedObject;

	private uint gdiSelectedPen;

	private uint gdiSelectedBrush;

	private uint gdiSelectedFont;

	private POINTD emfCurrentPoint;

	private uint emfBackColor;

	private uint emfTextColor;

	private uint emfBkMode;

	private uint emfPolyFillMode;

	private uint emfMapMode;

	private uint emfTextAlign;

	private const double penWidthScale = 1.2;

	private const double penWidthDefault = 0.1;

	private PointD viewportOrg;

	private PointD viewportExt;

	private PointD windowOrg;

	private PointD windowExt;

	private uint viewportFlags;

	private double emfX;

	private double emfY;

	private int emfOffsetX;

	private int emfOffsetY;

	private double emfScale;

	private double emfScaleX;

	private double emfScaleY;

	private XFORM baseXForm;

	private StiPdfExportService.StiPdfData emfComponent;

	internal ArrayList GDIobjects = new ArrayList();

	private bool makePath;

	private ArrayList emfPath;

	private Stack dcStack;

	private DCData dcData;

	internal bool assembleData = true;

	internal bool centerEmf;

	private Bitmap emfBmp;

	private Graphics emfGr;

	private EnumerateMetafileProc m_delegate;

	private Point m_destPoint = new Point(0, 0);

	private static object lockMetafileFlag = new object();

	private string ConvertToString(double Value)
	{
		return pdfService.ConvertToString(Value);
	}

	private string ConvertToString(double Value, int precision)
	{
		return pdfService.ConvertToString(Value, precision);
	}

	private int ReadInt32()
	{
		int result = BitConverter.ToInt32(dataArray, dataOffset);
		dataOffset += 4;
		return result;
	}

	private short ReadInt16()
	{
		short result = BitConverter.ToInt16(dataArray, dataOffset);
		dataOffset += 2;
		return result;
	}

	private uint ReadUInt32()
	{
		uint result = BitConverter.ToUInt32(dataArray, dataOffset);
		dataOffset += 4;
		return result;
	}

	private ushort ReadUInt16()
	{
		ushort result = BitConverter.ToUInt16(dataArray, dataOffset);
		dataOffset += 2;
		return result;
	}

	private byte ReadUInt8()
	{
		byte result = dataArray[dataOffset];
		dataOffset++;
		return result;
	}

	private float ReadSingle()
	{
		float result = BitConverter.ToSingle(dataArray, dataOffset);
		dataOffset += 4;
		return result;
	}

	private string ReadStringTchar(int bufSize)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; BitConverter.ToUInt16(dataArray, dataOffset + i * 2) != 0 && i < bufSize; i++)
		{
			stringBuilder.Append(BitConverter.ToChar(dataArray, dataOffset + i * 2));
		}
		dataOffset += bufSize * 2;
		return stringBuilder.ToString();
	}

	private void RemoveObject(uint ihNumber)
	{
		for (int num = GDIobjects.Count - 1; num >= 0; num--)
		{
			GDIobjectInfo gDIobjectInfo = (GDIobjectInfo)GDIobjects[num];
			if (gDIobjectInfo.ihNumber == ihNumber)
			{
				GDIobjects.RemoveAt(num);
			}
		}
	}

	private void SelectObject(uint ihNumber)
	{
		switch (ihNumber)
		{
		case 2147483653u:
			gdiSelectedBrush = uint.MaxValue;
			return;
		case 2147483656u:
			gdiSelectedPen = uint.MaxValue;
			return;
		}
		for (int num = GDIobjects.Count - 1; num >= 0; num--)
		{
			GDIobjectInfo gDIobjectInfo = (GDIobjectInfo)GDIobjects[num];
			if (gDIobjectInfo.ihNumber == ihNumber)
			{
				if (gDIobjectInfo.Pen != null)
				{
					gdiSelectedPen = ihNumber;
				}
				if (gDIobjectInfo.Brush != null)
				{
					gdiSelectedBrush = ihNumber;
				}
				if (gDIobjectInfo.Font != null)
				{
					gdiSelectedFont = ihNumber;
				}
				break;
			}
		}
	}

	private int GetGdiObjectIndex(uint ihNumber)
	{
		for (int num = GDIobjects.Count - 1; num >= 0; num--)
		{
			GDIobjectInfo gDIobjectInfo = (GDIobjectInfo)GDIobjects[num];
			if (gDIobjectInfo.ihNumber == ihNumber)
			{
				return num;
			}
		}
		return 0;
	}

	private void SetPdfColor(uint Color)
	{
		Color strokeColor = ConvertColorFromGdi(Color);
		pdfService.SetStrokeColor(strokeColor);
	}

	private void SetPdfBackColor(uint Color)
	{
		Color nonStrokeColor = ConvertColorFromGdi(Color);
		pdfService.SetNonStrokeColor(nonStrokeColor);
	}

	private Color ConvertColorFromGdi(uint gdiColor)
	{
		return Color.FromArgb((int)(gdiColor & 0xFF), (int)((gdiColor & 0xFF00) >> 8), (int)((gdiColor & 0xFF0000) >> 16));
	}

	private void WriteTextToStream(tagEMREXTTEXTOUT extText, StringBuilder Text, int[] charsOffset)
	{
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		if (GDIobjects.Count <= 0 || extText.nChars == 0)
		{
			return;
		}
		float num = 0f;
		float num2 = 1f;
		float num3 = 0f;
		Font val = null;
		int num4 = 0;
		GDIobjectInfo gDIobjectInfo = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedFont)];
		if (gDIobjectInfo.Font != null)
		{
			int internalFontNumber = gDIobjectInfo.Font.internalFontNumber;
			pdfFont.CurrentFont = internalFontNumber;
			val = Font.FromLogFont((object)gDIobjectInfo.Font.elfLogFont);
			num2 = val.SizeInPoints;
			num = ((float)Math.Abs(gDIobjectInfo.Font.elfLogFont.lfHeight) + num2) / 2f;
			if (gDIobjectInfo.Font.elfLogFont.lfHeight < 0)
			{
				num = (float)Math.Abs(gDIobjectInfo.Font.elfLogFont.lfHeight) * ((float)pdfFont.tmASC + (float)pdfFont.tmExternal * 0.1f) / 1000f;
				num2 = (float)((double)Math.Abs(gDIobjectInfo.Font.elfLogFont.lfHeight) * 0.72 / 0.955);
			}
			num *= (float)StiDpiHelper.GraphicsScale;
			pageStream.WriteLine("/F{0} {1} Tf", internalFontNumber, ConvertToString((double)num2 * 0.955 * emfScaleX / 0.72, precision_digits_font));
			num3 = (float)pdfFont.tmDESC / (float)(pdfFont.tmASC + pdfFont.tmDESC + pdfFont.tmExternal) * (float)Math.Abs(gDIobjectInfo.Font.elfLogFont.lfHeight);
			num4 = gDIobjectInfo.Font.elfLogFont.lfEscapement;
		}
		if ((extText.fOptions & 2u) != 0)
		{
			SetPdfBackColor(emfBackColor);
			pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(emfX + (double)extText.rcl.Left * emfScaleX), ConvertToString(emfY - (double)extText.rcl.Bottom * emfScaleY), ConvertToString((double)(extText.rcl.Right - extText.rcl.Left) * emfScaleX), ConvertToString((double)(extText.rcl.Bottom - extText.rcl.Top) * emfScaleY));
		}
		SetPdfBackColor(emfTextColor);
		bool flag = (extText.fOptions & 0x10) != 0;
		StringBuilder stringBuilder = new StringBuilder();
		int num5 = 0;
		int num6 = 0;
		stringBuilder.Append(useUnicodeMode ? "<" : "(");
		for (int i = 0; i < Text.Length; i++)
		{
			int num7 = Text[i];
			if (flag)
			{
				num7 = pdfFont.GlyphBackList[num7];
			}
			int num8 = pdfFont.UnicodeMap[num7];
			int num9 = 1000;
			if (num8 >= 32)
			{
				num9 = pdfFont.Widths[num8 - 32];
			}
			int num10 = charsOffset[i];
			num6 += num9;
			num5 += num10;
			double num11 = 0.0 - ((double)num5 * 0.72 / ((double)num2 * 0.955 / 1000.0) - (double)num6);
			num6 += -(int)num11;
			if (useUnicodeMode)
			{
				int num12 = pdfFont.GlyphList[num8];
				if (flag)
				{
					num12 = Text[i];
				}
				if (num12 == 65535)
				{
					num12 = 0;
				}
				stringBuilder.Append(num12.ToString("X4"));
				if (i < Text.Length - 1)
				{
					stringBuilder.Append(">" + ConvertToString(num11) + "<");
				}
			}
			else
			{
				stringBuilder.Append(StiPdfExportService.ConvertToEscapeSequencePlusTabs(((char)num8).ToString()));
				if (i < Text.Length - 1)
				{
					stringBuilder.Append(")" + ConvertToString(num11) + "(");
				}
			}
		}
		stringBuilder.Append(useUnicodeMode ? ">" : ")");
		byte[] array = new byte[stringBuilder.Length];
		for (int j = 0; j < stringBuilder.Length; j++)
		{
			array[j] = (byte)stringBuilder[j];
		}
		pageStream.WriteLine("BT");
		double num13 = emfX + (double)extText.Reference.X * emfScaleX;
		double num14 = 0.02 + (double)num2 * emfScaleX * 0.008;
		double num15 = emfY + num14 - (double)((float)extText.Reference.Y + num) * emfScaleY;
		if (emfTextAlign == 8)
		{
			num15 = emfY + num14 - (double)((float)extText.Reference.Y - num3) * emfScaleY;
		}
		if (emfTextAlign == 24)
		{
			num15 = emfY + num14 - (double)extText.Reference.Y * emfScaleY;
		}
		Matrix val2 = null;
		if (num4 != 0)
		{
			double num16 = (double)((float)num4 / 10f) * Math.PI / 180.0;
			val2 = new Matrix((float)Math.Cos(num16), (float)Math.Sin(num16), (float)(0.0 - Math.Sin(num16)), (float)Math.Cos(num16), (float)num13, (float)num15);
		}
		if (pdfFont.NeedSyntItalic && val.Italic)
		{
			if (val2 != null)
			{
				Matrix val3 = new Matrix(1f, 0f, 0.325f, 1f, 0f, 0f);
				val3.Multiply(val2, (MatrixOrder)1);
			}
			else
			{
				Matrix val4 = new Matrix(1f, 0f, 0.325f, 1f, (float)num13, (float)num15);
				val2 = val4;
			}
		}
		if (val2 != null)
		{
			pageStream.WriteLine("{0} {1} {2} {3} {4} {5} Tm", ConvertToString(val2.Elements[0]), ConvertToString(val2.Elements[1]), ConvertToString(val2.Elements[2]), ConvertToString(val2.Elements[3]), ConvertToString(val2.Elements[4]), ConvertToString(val2.Elements[5]));
		}
		else
		{
			pageStream.WriteLine("{0} {1} Td", ConvertToString(num13), ConvertToString(num15));
		}
		if (pdfFont.NeedSyntBold && val.Bold)
		{
			pageStream.WriteLine("{0} w 2 Tr", ConvertToString((double)(0.031f * num2) * 0.955 * emfScale / 0.72, precision_digits_font));
			SetPdfColor(emfTextColor);
		}
		pageStream.Write("[");
		pageStream.Flush();
		pdfService.memoryPageStream.Write(array, 0, array.Length);
		pageStream.WriteLine("] TJ");
		pageStream.WriteLine("ET");
		if (pdfFont.NeedSyntBold && val.Bold)
		{
			pageStream.WriteLine("0 Tr");
		}
		if (val == null || !val.Underline)
		{
			return;
		}
		double num17 = 0.0;
		for (int k = 0; k < Text.Length; k++)
		{
			if ((byte)Text[k] >= 32)
			{
				num17 += (double)pdfFont.Widths[(byte)Text[k] - 32];
			}
		}
		num17 = num17 * ((double)num2 * 0.955) / 1000.0;
		pageStream.WriteLine("{0} w", ConvertToString((double)num2 * 0.09));
		SetPdfBackColor(emfTextColor);
		pageStream.WriteLine("{0} {1} m", ConvertToString(num13), ConvertToString(num15 - (double)num2 * 0.115));
		pageStream.WriteLine("{0} {1} l S", ConvertToString(num13 + num17), ConvertToString(num15 - (double)num2 * 0.115));
	}

	private double GetTransformedCX(int x)
	{
		return (float)x * baseXForm.eM11;
	}

	private double GetTransformedCY(int y)
	{
		return (float)y * baseXForm.eM22;
	}

	private double GetTransformedX(int x)
	{
		return (float)x * baseXForm.eM11 - (float)emfOffsetX;
	}

	private double GetTransformedY(int y)
	{
		return (float)y * baseXForm.eM22 - (float)emfOffsetY;
	}

	private POINTD GetTransformedXY(int x, int y)
	{
		POINTD result = default(POINTD);
		result.X = (float)x * baseXForm.eM11 - (float)emfOffsetX;
		result.Y = (float)y * baseXForm.eM22 - (float)emfOffsetY;
		return result;
	}

	private double GetTransformedW(int w)
	{
		return (float)w * ((baseXForm.eM11 + baseXForm.eM22) / 2f);
	}

	private void SetViewport()
	{
		if (viewportFlags == 15 && (emfMapMode == 7 || emfMapMode == 8))
		{
			float num = (float)(((PointD)(ref viewportExt)).X / ((PointD)(ref windowExt)).X);
			float num2 = (float)(((PointD)(ref viewportExt)).Y / ((PointD)(ref windowExt)).Y);
			float num3 = num;
			float num4 = num2;
			float num5 = (float)(emfX * (double)(1f - num) + (((PointD)(ref viewportOrg)).X - ((PointD)(ref windowOrg)).X * (double)num) * emfScaleX);
			float num6 = (float)(emfY * (double)(1f - num2) - (((PointD)(ref viewportOrg)).Y - ((PointD)(ref windowOrg)).Y * (double)num2) * emfScaleY);
			pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(num3, 7), ConvertToString(0.0), ConvertToString(0.0), ConvertToString(num4, 7), ConvertToString(num5, 6), ConvertToString(num6, 6));
			viewportFlags = 0u;
		}
	}

	private void AddStockGDIobjects()
	{
		GDIobjectInfo gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = uint.MaxValue;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483648u;
		EmfBrushInfo emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 16777215u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483649u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 12632256u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483650u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 8421504u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483651u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 4210752u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483652u;
		emfBrushInfo = new EmfBrushInfo();
		emfBrushInfo.lbStyle = 0u;
		emfBrushInfo.lbColor = 0u;
		emfBrushInfo.lbHatch = 0u;
		gDIobjectInfo.Brush = emfBrushInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483654u;
		EmfPenInfo emfPenInfo = new EmfPenInfo();
		emfPenInfo.lopnStyle = 0u;
		emfPenInfo.lopnWidth = 0;
		emfPenInfo.lopnColor = 16777215u;
		gDIobjectInfo.Pen = emfPenInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		gDIobjectInfo = new GDIobjectInfo();
		gDIobjectInfo.ihNumber = 2147483655u;
		emfPenInfo = new EmfPenInfo();
		emfPenInfo.lopnStyle = 0u;
		emfPenInfo.lopnWidth = 0;
		emfPenInfo.lopnColor = 0u;
		gDIobjectInfo.Pen = emfPenInfo;
		RemoveObject(gDIobjectInfo.ihNumber);
		GDIobjects.Add(gDIobjectInfo);
		SelectObject(2147483648u);
		SelectObject(2147483655u);
	}

	private bool MetafileCallback(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData)
	{
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_15d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e0: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Invalid comparison between Unknown and I4
		//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected I4, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Invalid comparison between Unknown and I4
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Invalid comparison between Unknown and I4
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Expected I4, but got Unknown
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Invalid comparison between Unknown and I4
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_0bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd5: Invalid comparison between Unknown and I4
		//IL_0bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Invalid comparison between Unknown and I4
		//IL_3548: Unknown result type (might be due to invalid IL or missing references)
		//IL_354d: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_348c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3491: Unknown result type (might be due to invalid IL or missing references)
		//IL_342e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Expected I4, but got Unknown
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Invalid comparison between Unknown and I4
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c7: Expected O, but got Unknown
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be0: Invalid comparison between Unknown and I4
		//IL_0bbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Invalid comparison between Unknown and I4
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba8: Expected I4, but got Unknown
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Invalid comparison between Unknown and I4
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_1568: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1578: Unknown result type (might be due to invalid IL or missing references)
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_157c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		//IL_0f7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1587: Unknown result type (might be due to invalid IL or missing references)
		//IL_158a: Unknown result type (might be due to invalid IL or missing references)
		//IL_158b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1596: Unknown result type (might be due to invalid IL or missing references)
		//IL_1599: Unknown result type (might be due to invalid IL or missing references)
		//IL_159a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e91: Invalid comparison between Unknown and I4
		//IL_21e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21eb: Invalid comparison between Unknown and I4
		//IL_15b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c2: Expected O, but got Unknown
		//IL_1f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f75: Invalid comparison between Unknown and I4
		//IL_22cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_22cf: Invalid comparison between Unknown and I4
		//IL_20e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e8: Invalid comparison between Unknown and I4
		//IL_2397: Unknown result type (might be due to invalid IL or missing references)
		//IL_239a: Invalid comparison between Unknown and I4
		dataArray = null;
		dataOffset = 0;
		if (data != IntPtr.Zero)
		{
			dataArray = new byte[dataSize];
			Marshal.Copy(data, dataArray, 0, dataSize);
		}
		if (assembleData)
		{
			if ((int)recordType <= 40)
			{
				if ((int)recordType != 37)
				{
					if ((int)recordType == 40)
					{
						RemoveObject(ReadUInt32());
					}
				}
				else
				{
					emfSelectedObject = ReadUInt32();
					SelectObject(emfSelectedObject);
				}
			}
			else
			{
				switch (recordType - 81)
				{
				default:
					if ((int)recordType == 114)
					{
						tagEMRALPHABLEND tagEMRALPHABLEND = default(tagEMRALPHABLEND);
						tagEMRALPHABLEND.rclBounds.Left = ReadInt32();
						tagEMRALPHABLEND.rclBounds.Top = ReadInt32();
						tagEMRALPHABLEND.rclBounds.Right = ReadInt32();
						tagEMRALPHABLEND.rclBounds.Bottom = ReadInt32();
						tagEMRALPHABLEND.xDest = (int)GetTransformedX(ReadInt32());
						tagEMRALPHABLEND.yDest = (int)GetTransformedY(ReadInt32());
						tagEMRALPHABLEND.cxDest = (int)GetTransformedCX(ReadInt32());
						tagEMRALPHABLEND.cyDest = (int)GetTransformedCY(ReadInt32());
						tagEMRALPHABLEND.dwRop = ReadUInt32();
						tagEMRALPHABLEND.xSrc = ReadInt32();
						tagEMRALPHABLEND.ySrc = ReadInt32();
						tagEMRALPHABLEND.xformSrc.eM11 = ReadSingle();
						tagEMRALPHABLEND.xformSrc.eM12 = ReadSingle();
						tagEMRALPHABLEND.xformSrc.eM21 = ReadSingle();
						tagEMRALPHABLEND.xformSrc.eM22 = ReadSingle();
						tagEMRALPHABLEND.xformSrc.eDx = ReadSingle();
						tagEMRALPHABLEND.xformSrc.eDy = ReadSingle();
						tagEMRALPHABLEND.crBkColorSrc = ReadUInt32();
						tagEMRALPHABLEND.iUsageSrc = ReadUInt32();
						tagEMRALPHABLEND.offBmiSrc = ReadUInt32();
						tagEMRALPHABLEND.cbBmiSrc = ReadUInt32();
						tagEMRALPHABLEND.offBitsSrc = ReadUInt32();
						tagEMRALPHABLEND.cbBitsSrc = ReadUInt32();
						tagEMRALPHABLEND.cxSrc = ReadInt32();
						tagEMRALPHABLEND.cySrc = ReadInt32();
						dataArray[tagEMRALPHABLEND.offBmiSrc - 8 - 14] = 66;
						dataArray[tagEMRALPHABLEND.offBmiSrc - 8 - 14 + 1] = 77;
						BitConverter.GetBytes(tagEMRALPHABLEND.cbBmiSrc + tagEMRALPHABLEND.cbBitsSrc + 14).CopyTo(dataArray, (int)(tagEMRALPHABLEND.offBmiSrc - 8 - 14 + 2));
						BitConverter.GetBytes(0u).CopyTo(dataArray, (int)(tagEMRALPHABLEND.offBmiSrc - 8 - 14 + 6));
						BitConverter.GetBytes(tagEMRALPHABLEND.cbBmiSrc + 14).CopyTo(dataArray, (int)(tagEMRALPHABLEND.offBmiSrc - 8 - 14 + 10));
						MemoryStream memoryStream2 = new MemoryStream(dataArray, (int)(tagEMRALPHABLEND.offBmiSrc - 8 - 14), (int)(tagEMRALPHABLEND.cbBmiSrc + tagEMRALPHABLEND.cbBitsSrc + 14));
						Bitmap val4 = new Bitmap((Stream)memoryStream2);
						if (val4 != null)
						{
							pdfService.StoreImageData((Image)(object)val4, 0f, isImageComponent: false, needSmoothing: false);
						}
						memoryStream2.Close();
						((Image)val4).Dispose();
					}
					break;
				case 2:
				{
					RECTL rECTL3 = default(RECTL);
					rECTL3.Left = ReadInt32();
					rECTL3.Top = ReadInt32();
					rECTL3.Right = ReadInt32();
					rECTL3.Bottom = ReadInt32();
					uint num8 = ReadUInt32();
					float num9 = ReadSingle();
					float num10 = ReadSingle();
					POINTL pOINTL2 = default(POINTL);
					pOINTL2.X = ReadInt32();
					pOINTL2.Y = ReadInt32();
					uint num11 = ReadUInt32();
					uint num12 = ReadUInt32();
					uint num13 = ReadUInt32();
					RECTL rECTL4 = default(RECTL);
					rECTL4.Left = ReadInt32();
					rECTL4.Top = ReadInt32();
					rECTL4.Right = ReadInt32();
					rECTL4.Bottom = ReadInt32();
					uint num14 = ReadUInt32();
					StringBuilder stringBuilder2 = new StringBuilder();
					int[] array2 = new int[num11];
					for (int j = 0; j < num11; j++)
					{
						stringBuilder2.Append((char)dataArray[(int)(num12 - 8 + j)]);
						array2[j] = BitConverter.ToChar(dataArray, (int)(num14 - 8 + j * 4));
					}
					if (GDIobjects.Count > 0)
					{
						GDIobjectInfo gDIobjectInfo2 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedFont)];
						if (gDIobjectInfo2.Font != null)
						{
							int internalFontNumber2 = gDIobjectInfo2.Font.internalFontNumber;
							pdfFont.CurrentFont = internalFontNumber2;
						}
					}
					if ((num13 & 0x10) == 0)
					{
						pdfFont.StoreUnicodeSymbolsInMap(stringBuilder2);
					}
					else
					{
						pdfFont.StoreGlyphsInMap(stringBuilder2);
					}
					break;
				}
				case 3:
				{
					RECTL rECTL = default(RECTL);
					rECTL.Left = ReadInt32();
					rECTL.Top = ReadInt32();
					rECTL.Right = ReadInt32();
					rECTL.Bottom = ReadInt32();
					uint num = ReadUInt32();
					float num2 = ReadSingle();
					float num3 = ReadSingle();
					POINTL pOINTL = default(POINTL);
					pOINTL.X = ReadInt32();
					pOINTL.Y = ReadInt32();
					uint num4 = ReadUInt32();
					uint num5 = ReadUInt32();
					uint num6 = ReadUInt32();
					RECTL rECTL2 = default(RECTL);
					rECTL2.Left = ReadInt32();
					rECTL2.Top = ReadInt32();
					rECTL2.Right = ReadInt32();
					rECTL2.Bottom = ReadInt32();
					uint num7 = ReadUInt32();
					StringBuilder stringBuilder = new StringBuilder();
					int[] array = new int[num4];
					for (int i = 0; i < num4; i++)
					{
						stringBuilder.Append(BitConverter.ToChar(dataArray, (int)(num5 - 8 + i * 2)));
						array[i] = BitConverter.ToChar(dataArray, (int)(num7 - 8 + i * 4));
					}
					if (GDIobjects.Count > 0)
					{
						GDIobjectInfo gDIobjectInfo = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedFont)];
						if (gDIobjectInfo.Font != null)
						{
							int internalFontNumber = gDIobjectInfo.Font.internalFontNumber;
							pdfFont.CurrentFont = internalFontNumber;
						}
					}
					if ((num6 & 0x10) == 0)
					{
						pdfFont.StoreUnicodeSymbolsInMap(stringBuilder);
					}
					else
					{
						pdfFont.StoreGlyphsInMap(stringBuilder);
					}
					break;
				}
				case 1:
				{
					LOGFONT elfLogFont = default(LOGFONT);
					uint ihNumber = ReadUInt32();
					elfLogFont.lfHeight = ReadInt32();
					elfLogFont.lfWidth = ReadInt32();
					elfLogFont.lfEscapement = ReadInt32();
					elfLogFont.lfOrientation = ReadInt32();
					elfLogFont.lfWeight = ReadInt32();
					elfLogFont.lfItalic = ReadUInt8();
					elfLogFont.lfUnderline = ReadUInt8();
					elfLogFont.lfStrikeOut = ReadUInt8();
					elfLogFont.lfCharSet = ReadUInt8();
					elfLogFont.lfOutPrecision = ReadUInt8();
					elfLogFont.lfClipPrecision = ReadUInt8();
					elfLogFont.lfQuality = ReadUInt8();
					elfLogFont.lfPitchAndFamily = ReadUInt8();
					elfLogFont.lfFaceName = ReadStringTchar(32);
					if (elfLogFont.lfHeight != 0)
					{
						FontStyle val2 = (FontStyle)0;
						if (elfLogFont.lfWeight > 500)
						{
							val2 = (FontStyle)(val2 | 1);
						}
						if (elfLogFont.lfItalic != 0)
						{
							val2 = (FontStyle)(val2 | 2);
						}
						if (elfLogFont.lfUnderline != 0)
						{
							val2 = (FontStyle)(val2 | 4);
						}
						Font val3 = null;
						try
						{
							val3 = new Font(StiFontCollection.GetFontFamily(elfLogFont.lfFaceName, false), (float)Math.Abs(elfLogFont.lfHeight), val2);
						}
						catch
						{
							val3 = new Font("Arial", (float)Math.Abs(elfLogFont.lfHeight), val2);
						}
						elfLogFont.lfFaceName = val3.Name;
						GDIobjectInfo gDIobjectInfo3 = new GDIobjectInfo();
						gDIobjectInfo3.ihNumber = ihNumber;
						EmfFontInfo emfFontInfo = new EmfFontInfo();
						emfFontInfo.internalFontNumber = pdfFont.GetFontNumber(val3);
						emfFontInfo.elfLogFont = elfLogFont;
						gDIobjectInfo3.Font = emfFontInfo;
						RemoveObject(gDIobjectInfo3.ihNumber);
						GDIobjects.Add(gDIobjectInfo3);
					}
					break;
				}
				case 0:
				{
					tagEMRSTRETCHDIBITS tagEMRSTRETCHDIBITS = default(tagEMRSTRETCHDIBITS);
					tagEMRSTRETCHDIBITS.rclBounds.Left = ReadInt32();
					tagEMRSTRETCHDIBITS.rclBounds.Top = ReadInt32();
					tagEMRSTRETCHDIBITS.rclBounds.Right = ReadInt32();
					tagEMRSTRETCHDIBITS.rclBounds.Bottom = ReadInt32();
					tagEMRSTRETCHDIBITS.xDest = ReadInt32();
					tagEMRSTRETCHDIBITS.yDest = ReadInt32();
					tagEMRSTRETCHDIBITS.xSrc = ReadInt32();
					tagEMRSTRETCHDIBITS.ySrc = ReadInt32();
					tagEMRSTRETCHDIBITS.cxSrc = ReadInt32();
					tagEMRSTRETCHDIBITS.cySrc = ReadInt32();
					tagEMRSTRETCHDIBITS.offBmiSrc = ReadUInt32();
					tagEMRSTRETCHDIBITS.cbBmiSrc = ReadUInt32();
					tagEMRSTRETCHDIBITS.offBitsSrc = ReadUInt32();
					tagEMRSTRETCHDIBITS.cbBitsSrc = ReadUInt32();
					tagEMRSTRETCHDIBITS.iUsageSrc = ReadUInt32();
					tagEMRSTRETCHDIBITS.dwRop = ReadUInt32();
					tagEMRSTRETCHDIBITS.cxDest = ReadInt32();
					tagEMRSTRETCHDIBITS.cyDest = ReadInt32();
					dataArray[tagEMRSTRETCHDIBITS.offBmiSrc - 8 - 14] = 66;
					dataArray[tagEMRSTRETCHDIBITS.offBmiSrc - 8 - 14 + 1] = 77;
					BitConverter.GetBytes(tagEMRSTRETCHDIBITS.cbBmiSrc + tagEMRSTRETCHDIBITS.cbBitsSrc + 14).CopyTo(dataArray, (int)(tagEMRSTRETCHDIBITS.offBmiSrc - 8 - 14 + 2));
					BitConverter.GetBytes(0u).CopyTo(dataArray, (int)(tagEMRSTRETCHDIBITS.offBmiSrc - 8 - 14 + 6));
					BitConverter.GetBytes(tagEMRSTRETCHDIBITS.cbBmiSrc + 14).CopyTo(dataArray, (int)(tagEMRSTRETCHDIBITS.offBmiSrc - 8 - 14 + 10));
					MemoryStream memoryStream = new MemoryStream(dataArray, (int)(tagEMRSTRETCHDIBITS.offBmiSrc - 8 - 14), (int)(tagEMRSTRETCHDIBITS.cbBmiSrc + tagEMRSTRETCHDIBITS.cbBitsSrc + 14));
					Bitmap val = new Bitmap((Stream)memoryStream);
					if (val != null)
					{
						pdfService.StoreImageData((Image)(object)val, 0f, isImageComponent: false, needSmoothing: false);
					}
					memoryStream.Close();
					((Image)val).Dispose();
					break;
				}
				}
			}
		}
		else
		{
			if (viewportFlags == 15 && (int)recordType != 12 && (int)recordType != 11 && (int)recordType != 10 && (int)recordType != 9)
			{
				SetViewport();
			}
			if ((int)recordType <= 95)
			{
				switch (recordType - 1)
				{
				default:
					switch (recordType - 54)
					{
					default:
						switch (recordType - 75)
						{
						case 8:
						{
							tagEMREXTTEXTOUT tagEMREXTTEXTOUT2 = new tagEMREXTTEXTOUT();
							tagEMREXTTEXTOUT2.Bounds.Left = ReadInt32();
							tagEMREXTTEXTOUT2.Bounds.Top = ReadInt32();
							tagEMREXTTEXTOUT2.Bounds.Right = ReadInt32();
							tagEMREXTTEXTOUT2.Bounds.Bottom = ReadInt32();
							tagEMREXTTEXTOUT2.iGraphicsMode = ReadUInt32();
							tagEMREXTTEXTOUT2.exScale = ReadSingle();
							tagEMREXTTEXTOUT2.eyScale = ReadSingle();
							tagEMREXTTEXTOUT2.Reference.X = (int)GetTransformedX(ReadInt32());
							tagEMREXTTEXTOUT2.Reference.Y = (int)GetTransformedY(ReadInt32());
							tagEMREXTTEXTOUT2.nChars = ReadUInt32();
							tagEMREXTTEXTOUT2.offString = ReadUInt32();
							tagEMREXTTEXTOUT2.fOptions = ReadUInt32();
							tagEMREXTTEXTOUT2.rcl.Left = (int)GetTransformedX(ReadInt32());
							tagEMREXTTEXTOUT2.rcl.Top = (int)GetTransformedY(ReadInt32());
							tagEMREXTTEXTOUT2.rcl.Right = (int)GetTransformedX(ReadInt32());
							tagEMREXTTEXTOUT2.rcl.Bottom = (int)GetTransformedY(ReadInt32());
							tagEMREXTTEXTOUT2.offDx = ReadUInt32();
							StringBuilder stringBuilder4 = new StringBuilder();
							int[] array7 = new int[tagEMREXTTEXTOUT2.nChars];
							for (int num41 = 0; num41 < tagEMREXTTEXTOUT2.nChars; num41++)
							{
								stringBuilder4.Append((char)dataArray[(int)(tagEMREXTTEXTOUT2.offString - 8 + num41)]);
								array7[num41] = BitConverter.ToChar(dataArray, (int)(tagEMREXTTEXTOUT2.offDx - 8 + num41 * 4));
							}
							WriteTextToStream(tagEMREXTTEXTOUT2, stringBuilder4, array7);
							break;
						}
						case 9:
						{
							tagEMREXTTEXTOUT tagEMREXTTEXTOUT = new tagEMREXTTEXTOUT();
							tagEMREXTTEXTOUT.Bounds.Left = ReadInt32();
							tagEMREXTTEXTOUT.Bounds.Top = ReadInt32();
							tagEMREXTTEXTOUT.Bounds.Right = ReadInt32();
							tagEMREXTTEXTOUT.Bounds.Bottom = ReadInt32();
							tagEMREXTTEXTOUT.iGraphicsMode = ReadUInt32();
							tagEMREXTTEXTOUT.exScale = ReadSingle();
							tagEMREXTTEXTOUT.eyScale = ReadSingle();
							tagEMREXTTEXTOUT.Reference.X = (int)GetTransformedX(ReadInt32());
							tagEMREXTTEXTOUT.Reference.Y = (int)GetTransformedY(ReadInt32());
							tagEMREXTTEXTOUT.nChars = ReadUInt32();
							tagEMREXTTEXTOUT.offString = ReadUInt32();
							tagEMREXTTEXTOUT.fOptions = ReadUInt32();
							tagEMREXTTEXTOUT.rcl.Left = (int)GetTransformedX(ReadInt32());
							tagEMREXTTEXTOUT.rcl.Top = (int)GetTransformedY(ReadInt32());
							tagEMREXTTEXTOUT.rcl.Right = (int)GetTransformedX(ReadInt32());
							tagEMREXTTEXTOUT.rcl.Bottom = (int)GetTransformedY(ReadInt32());
							tagEMREXTTEXTOUT.offDx = ReadUInt32();
							StringBuilder stringBuilder3 = new StringBuilder();
							int[] array6 = new int[tagEMREXTTEXTOUT.nChars];
							int num39 = (((tagEMREXTTEXTOUT.fOptions & 0x2000u) != 0) ? 8 : 4);
							for (int num40 = 0; num40 < tagEMREXTTEXTOUT.nChars; num40++)
							{
								stringBuilder3.Append(BitConverter.ToChar(dataArray, (int)(tagEMREXTTEXTOUT.offString - 8 + num40 * 2)));
								array6[num40] = BitConverter.ToInt32(dataArray, (int)(tagEMREXTTEXTOUT.offDx - 8 + num40 * num39));
							}
							WriteTextToStream(tagEMREXTTEXTOUT, stringBuilder3, array6);
							break;
						}
						case 7:
						{
							uint ihNumber5 = ReadUInt32();
							LOGFONT elfLogFont2 = default(LOGFONT);
							elfLogFont2.lfHeight = ReadInt32();
							elfLogFont2.lfWidth = ReadInt32();
							elfLogFont2.lfEscapement = ReadInt32();
							elfLogFont2.lfOrientation = ReadInt32();
							elfLogFont2.lfWeight = ReadInt32();
							elfLogFont2.lfItalic = ReadUInt8();
							elfLogFont2.lfUnderline = ReadUInt8();
							elfLogFont2.lfStrikeOut = ReadUInt8();
							elfLogFont2.lfCharSet = ReadUInt8();
							elfLogFont2.lfOutPrecision = ReadUInt8();
							elfLogFont2.lfClipPrecision = ReadUInt8();
							elfLogFont2.lfQuality = ReadUInt8();
							elfLogFont2.lfPitchAndFamily = ReadUInt8();
							elfLogFont2.lfFaceName = ReadStringTchar(32);
							if (elfLogFont2.lfHeight != 0)
							{
								elfLogFont2.lfCharSet = 0;
								FontStyle val7 = (FontStyle)0;
								if (elfLogFont2.lfWeight > 500)
								{
									val7 = (FontStyle)(val7 | 1);
								}
								if (elfLogFont2.lfItalic != 0)
								{
									val7 = (FontStyle)(val7 | 2);
								}
								if (elfLogFont2.lfUnderline != 0)
								{
									val7 = (FontStyle)(val7 | 4);
								}
								Font val8 = null;
								try
								{
									val8 = new Font(StiFontCollection.GetFontFamily(elfLogFont2.lfFaceName, false), (float)Math.Abs(elfLogFont2.lfHeight), val7);
								}
								catch
								{
									val8 = new Font("Arial", (float)Math.Abs(elfLogFont2.lfHeight), val7);
								}
								elfLogFont2.lfFaceName = val8.Name;
								GDIobjectInfo gDIobjectInfo14 = new GDIobjectInfo();
								gDIobjectInfo14.ihNumber = ihNumber5;
								EmfFontInfo emfFontInfo2 = new EmfFontInfo();
								emfFontInfo2.internalFontNumber = pdfFont.GetFontNumber(val8);
								emfFontInfo2.elfLogFont = elfLogFont2;
								gDIobjectInfo14.Font = emfFontInfo2;
								RemoveObject(gDIobjectInfo14.ihNumber);
								GDIobjects.Add(gDIobjectInfo14);
							}
							break;
						}
						case 20:
						{
							uint ihNumber4 = ReadUInt32();
							uint num28 = ReadUInt32();
							uint num29 = ReadUInt32();
							uint num30 = ReadUInt32();
							uint num31 = ReadUInt32();
							uint lopnStyle2 = ReadUInt32();
							uint lopnWidth = ReadUInt32();
							uint num32 = ReadUInt32();
							uint lopnColor2 = ReadUInt32();
							uint num33 = ReadUInt32();
							uint num34 = ReadUInt32();
							uint[] array4 = new uint[num34];
							for (int n = 0; n < num34; n++)
							{
								array4[n] = ReadUInt32();
							}
							GDIobjectInfo gDIobjectInfo11 = new GDIobjectInfo();
							gDIobjectInfo11.ihNumber = ihNumber4;
							EmfPenInfo emfPenInfo2 = new EmfPenInfo();
							emfPenInfo2.lopnStyle = lopnStyle2;
							emfPenInfo2.lopnWidth = (int)lopnWidth;
							emfPenInfo2.lopnColor = lopnColor2;
							gDIobjectInfo11.Pen = emfPenInfo2;
							RemoveObject(gDIobjectInfo11.ihNumber);
							GDIobjects.Add(gDIobjectInfo11);
							break;
						}
						case 10:
						case 13:
						{
							RECTL rECTL7 = default(RECTL);
							rECTL7.Left = ReadInt32();
							rECTL7.Top = ReadInt32();
							rECTL7.Right = ReadInt32();
							rECTL7.Bottom = ReadInt32();
							uint num26 = ReadUInt32();
							POINTD[] array3 = new POINTD[num26];
							for (int l = 0; l < num26; l++)
							{
								array3[l] = GetTransformedXY(ReadInt16(), ReadInt16());
							}
							if (GDIobjects.Count <= 0)
							{
								break;
							}
							bool flag3 = false;
							GDIobjectInfo gDIobjectInfo10 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
							if (gDIobjectInfo10.Pen != null)
							{
								flag3 = true;
							}
							POINTD pOINTD = emfCurrentPoint;
							int m = 0;
							if ((int)recordType == 85)
							{
								pOINTD = array3[0];
								m++;
							}
							string value5 = $"{ConvertToString(emfX + pOINTD.X * emfScaleX)} {ConvertToString(emfY - pOINTD.Y * emfScaleY)} m";
							if (!makePath && flag3)
							{
								SetPdfColor(gDIobjectInfo10.Pen.lopnColor);
								double num27 = GetTransformedW(gDIobjectInfo10.Pen.lopnWidth) * emfScale;
								if (num27 == 0.0)
								{
									num27 = 0.1;
								}
								pageStream.WriteLine("{0} w", ConvertToString(num27 * 1.2));
								pageStream.WriteLine(value5);
							}
							if (makePath && (int)recordType == 85)
							{
								emfPath.Add(value5);
							}
							for (; m < num26; m += 3)
							{
								string value6 = $"{ConvertToString(emfX + array3[m].X * emfScaleX)} {ConvertToString(emfY - array3[m].Y * emfScaleY)} {ConvertToString(emfX + array3[m + 1].X * emfScaleX)} {ConvertToString(emfY - array3[m + 1].Y * emfScaleY)} {ConvertToString(emfX + array3[m + 2].X * emfScaleX)} {ConvertToString(emfY - array3[m + 2].Y * emfScaleY)} c ";
								if (makePath)
								{
									emfPath.Add(value6);
								}
								if (!makePath && flag3)
								{
									pageStream.WriteLine(value6);
								}
							}
							if (!makePath && flag3)
							{
								pageStream.WriteLine("S");
							}
							if ((int)recordType == 88)
							{
								emfCurrentPoint.X = array3[m - 3 + 2].X;
								emfCurrentPoint.Y = array3[m - 3 + 2].Y;
							}
							break;
						}
						case 12:
						case 14:
						{
							RECTL rECTL8 = default(RECTL);
							rECTL8.Left = ReadInt32();
							rECTL8.Top = ReadInt32();
							rECTL8.Right = ReadInt32();
							rECTL8.Bottom = ReadInt32();
							uint num35 = ReadUInt32();
							POINTD[] array5 = new POINTD[num35];
							for (int num36 = 0; num36 < num35; num36++)
							{
								array5[num36] = GetTransformedXY(ReadInt16(), ReadInt16());
							}
							if (GDIobjects.Count <= 0)
							{
								break;
							}
							bool flag4 = false;
							GDIobjectInfo gDIobjectInfo12 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
							if (gDIobjectInfo12.Pen != null)
							{
								flag4 = true;
							}
							POINTD pOINTD2 = emfCurrentPoint;
							int num37 = 0;
							if ((int)recordType == 87)
							{
								pOINTD2 = array5[0];
								num37++;
							}
							string value7 = $"{ConvertToString(emfX + pOINTD2.X * emfScaleX)} {ConvertToString(emfY - pOINTD2.Y * emfScaleY)} m";
							if (!makePath && flag4)
							{
								SetPdfColor(gDIobjectInfo12.Pen.lopnColor);
								double num38 = GetTransformedW(gDIobjectInfo12.Pen.lopnWidth) * emfScale;
								if (num38 == 0.0)
								{
									num38 = 0.1;
								}
								pageStream.WriteLine("{0} w", ConvertToString(num38 * 1.2));
								pageStream.WriteLine(value7);
							}
							if (makePath && (int)recordType == 87)
							{
								emfPath.Add(value7);
							}
							for (; num37 < num35; num37++)
							{
								string value8 = $"{ConvertToString(emfX + array5[num37].X * emfScaleX)} {ConvertToString(emfY - array5[num37].Y * emfScaleY)} l";
								if (makePath)
								{
									emfPath.Add(value8);
								}
								if (!makePath && flag4)
								{
									pageStream.WriteLine(value8);
								}
							}
							if (!makePath && flag4)
							{
								pageStream.WriteLine("S");
							}
							if ((int)recordType == 89)
							{
								emfCurrentPoint.X = array5[num35 - 1].X;
								emfCurrentPoint.Y = array5[num35 - 1].Y;
							}
							break;
						}
						case 11:
						{
							RECTL rECTL10 = default(RECTL);
							rECTL10.Left = ReadInt32();
							rECTL10.Top = ReadInt32();
							rECTL10.Right = ReadInt32();
							rECTL10.Bottom = ReadInt32();
							uint num52 = ReadUInt32();
							POINTD[] array10 = new POINTD[num52];
							for (int num53 = 0; num53 < num52; num53++)
							{
								array10[num53] = GetTransformedXY(ReadInt16(), ReadInt16());
							}
							if (GDIobjects.Count <= 0)
							{
								break;
							}
							bool flag7 = false;
							bool flag8 = false;
							GDIobjectInfo gDIobjectInfo17 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
							if (gDIobjectInfo17.Pen != null)
							{
								flag7 = true;
							}
							GDIobjectInfo gDIobjectInfo18 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedBrush)];
							if (gDIobjectInfo18.Brush != null)
							{
								flag8 = true;
							}
							ArrayList arrayList3 = new ArrayList();
							POINTD pOINTD4 = array10[0];
							string value11 = $"{ConvertToString(emfX + pOINTD4.X * emfScaleX)} {ConvertToString(emfY - pOINTD4.Y * emfScaleY)} m";
							arrayList3.Add(value11);
							for (int num54 = 1; num54 < num52; num54++)
							{
								string value12 = $"{ConvertToString(emfX + array10[num54].X * emfScaleX)} {ConvertToString(emfY - array10[num54].Y * emfScaleY)} l";
								arrayList3.Add(value12);
							}
							if (flag8)
							{
								SetPdfBackColor(gDIobjectInfo18.Brush.lbColor);
								for (int num55 = 0; num55 < arrayList3.Count; num55++)
								{
									pageStream.WriteLine((string)arrayList3[num55]);
								}
								pageStream.WriteLine("h f");
							}
							if (flag7)
							{
								SetPdfColor(gDIobjectInfo17.Pen.lopnColor);
								double num56 = GetTransformedW(gDIobjectInfo17.Pen.lopnWidth) * emfScale;
								if (num56 == 0.0)
								{
									num56 = 0.1;
								}
								pageStream.WriteLine("{0} w", ConvertToString(num56 * 1.2));
								for (int num57 = 0; num57 < arrayList3.Count; num57++)
								{
									pageStream.WriteLine((string)arrayList3[num57]);
								}
								pageStream.WriteLine("h S");
							}
							arrayList3.Clear();
							break;
						}
						case 16:
						{
							RECTL rECTL9 = default(RECTL);
							rECTL9.Left = ReadInt32();
							rECTL9.Top = ReadInt32();
							rECTL9.Right = ReadInt32();
							rECTL9.Bottom = ReadInt32();
							uint num42 = ReadUInt32();
							uint num43 = ReadUInt32();
							uint[] array8 = new uint[num42];
							for (int num44 = 0; num44 < num42; num44++)
							{
								array8[num44] = ReadUInt32();
							}
							POINTD[] array9 = new POINTD[num43];
							for (int num45 = 0; num45 < num43; num45++)
							{
								array9[num45] = GetTransformedXY(ReadInt16(), ReadInt16());
							}
							if (GDIobjects.Count <= 0)
							{
								break;
							}
							bool flag5 = false;
							bool flag6 = false;
							GDIobjectInfo gDIobjectInfo15 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
							if (gDIobjectInfo15.Pen != null)
							{
								flag5 = true;
							}
							GDIobjectInfo gDIobjectInfo16 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedBrush)];
							if (gDIobjectInfo16.Brush != null)
							{
								flag6 = true;
							}
							ArrayList arrayList2 = new ArrayList();
							uint num46 = 0u;
							for (int num47 = 0; num47 < num42; num47++)
							{
								POINTD pOINTD3 = array9[num46];
								string value9 = $"{ConvertToString(emfX + pOINTD3.X * emfScaleX)} {ConvertToString(emfY - pOINTD3.Y * emfScaleY)} m";
								arrayList2.Add(value9);
								for (int num48 = 1; num48 < array8[num47]; num48++)
								{
									string value10 = $"{ConvertToString(emfX + array9[num46 + num48].X * emfScaleX)} {ConvertToString(emfY - array9[num46 + num48].Y * emfScaleY)} l";
									arrayList2.Add(value10);
								}
								arrayList2.Add("h");
								num46 += array8[num47];
							}
							if (flag6)
							{
								SetPdfBackColor(gDIobjectInfo16.Brush.lbColor);
								for (int num49 = 0; num49 < arrayList2.Count; num49++)
								{
									pageStream.WriteLine((string)arrayList2[num49]);
								}
								pageStream.WriteLine("f{0}", (emfPolyFillMode == 1) ? "*" : "");
							}
							if (flag5)
							{
								SetPdfColor(gDIobjectInfo15.Pen.lopnColor);
								double num50 = GetTransformedW(gDIobjectInfo15.Pen.lopnWidth) * emfScale;
								if (num50 == 0.0)
								{
									num50 = 0.1;
								}
								pageStream.WriteLine("{0} w", ConvertToString(num50 * 1.2));
								for (int num51 = 0; num51 < arrayList2.Count; num51++)
								{
									pageStream.WriteLine((string)arrayList2[num51]);
								}
								pageStream.WriteLine("S");
							}
							arrayList2.Clear();
							break;
						}
						case 1:
						{
							tagEMRBITBLT tagEMRBITBLT = default(tagEMRBITBLT);
							tagEMRBITBLT.rclBounds.Left = ReadInt32();
							tagEMRBITBLT.rclBounds.Top = ReadInt32();
							tagEMRBITBLT.rclBounds.Right = ReadInt32();
							tagEMRBITBLT.rclBounds.Bottom = ReadInt32();
							tagEMRBITBLT.xDest = (int)GetTransformedX(ReadInt32());
							tagEMRBITBLT.yDest = (int)GetTransformedY(ReadInt32());
							tagEMRBITBLT.cxDest = (int)GetTransformedCX(ReadInt32());
							tagEMRBITBLT.cyDest = (int)GetTransformedCY(ReadInt32());
							tagEMRBITBLT.dwRop = ReadUInt32();
							tagEMRBITBLT.xSrc = ReadInt32();
							tagEMRBITBLT.ySrc = ReadInt32();
							tagEMRBITBLT.xformSrc.eM11 = ReadSingle();
							tagEMRBITBLT.xformSrc.eM12 = ReadSingle();
							tagEMRBITBLT.xformSrc.eM21 = ReadSingle();
							tagEMRBITBLT.xformSrc.eM22 = ReadSingle();
							tagEMRBITBLT.xformSrc.eDx = ReadSingle();
							tagEMRBITBLT.xformSrc.eDy = ReadSingle();
							tagEMRBITBLT.crBkColorSrc = ReadUInt32();
							tagEMRBITBLT.iUsageSrc = ReadUInt32();
							tagEMRBITBLT.offBmiSrc = ReadUInt32();
							tagEMRBITBLT.cbBmiSrc = ReadUInt32();
							tagEMRBITBLT.offBitsSrc = ReadUInt32();
							tagEMRBITBLT.cbBitsSrc = ReadUInt32();
							uint dwRop = tagEMRBITBLT.dwRop;
							if (dwRop == 15728673 && GDIobjects.Count > 0)
							{
								GDIobjectInfo gDIobjectInfo13 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedBrush)];
								if (gDIobjectInfo13.Brush != null)
								{
									SetPdfBackColor(gDIobjectInfo13.Brush.lbColor);
									pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(emfX + (double)tagEMRBITBLT.xDest * emfScaleX), ConvertToString(emfY - (double)(tagEMRBITBLT.yDest + tagEMRBITBLT.cyDest) * emfScaleY), ConvertToString((double)tagEMRBITBLT.cxDest * emfScale), ConvertToString((double)tagEMRBITBLT.cyDest * emfScale));
								}
							}
							break;
						}
						case 6:
						{
							tagEMRSTRETCHDIBITS tagEMRSTRETCHDIBITS2 = default(tagEMRSTRETCHDIBITS);
							tagEMRSTRETCHDIBITS2.rclBounds.Left = (int)GetTransformedX(ReadInt32());
							tagEMRSTRETCHDIBITS2.rclBounds.Top = (int)GetTransformedY(ReadInt32());
							tagEMRSTRETCHDIBITS2.rclBounds.Right = (int)GetTransformedX(ReadInt32());
							tagEMRSTRETCHDIBITS2.rclBounds.Bottom = (int)GetTransformedY(ReadInt32());
							tagEMRSTRETCHDIBITS2.xDest = ReadInt32();
							tagEMRSTRETCHDIBITS2.yDest = ReadInt32();
							tagEMRSTRETCHDIBITS2.xSrc = ReadInt32();
							tagEMRSTRETCHDIBITS2.ySrc = ReadInt32();
							tagEMRSTRETCHDIBITS2.cxSrc = ReadInt32();
							tagEMRSTRETCHDIBITS2.cySrc = ReadInt32();
							tagEMRSTRETCHDIBITS2.offBmiSrc = ReadUInt32();
							tagEMRSTRETCHDIBITS2.cbBmiSrc = ReadUInt32();
							tagEMRSTRETCHDIBITS2.offBitsSrc = ReadUInt32();
							tagEMRSTRETCHDIBITS2.cbBitsSrc = ReadUInt32();
							tagEMRSTRETCHDIBITS2.iUsageSrc = ReadUInt32();
							tagEMRSTRETCHDIBITS2.dwRop = ReadUInt32();
							tagEMRSTRETCHDIBITS2.cxDest = (int)GetTransformedX(ReadInt32());
							tagEMRSTRETCHDIBITS2.cyDest = (int)GetTransformedY(ReadInt32());
							StiPdfExportService.StiPdfData pp = default(StiPdfExportService.StiPdfData);
							double num25 = 0.72 / emfScale;
							pp.X = emfX + (double)tagEMRSTRETCHDIBITS2.xDest * emfScaleX;
							pp.Y = emfY - (double)(tagEMRSTRETCHDIBITS2.yDest + tagEMRSTRETCHDIBITS2.cyDest) * emfScaleY;
							double imageResolutionX = (double)((float)tagEMRSTRETCHDIBITS2.cxSrc / (float)tagEMRSTRETCHDIBITS2.cxDest) * num25;
							double imageResolutionY = (double)((float)tagEMRSTRETCHDIBITS2.cySrc / (float)tagEMRSTRETCHDIBITS2.cyDest) * num25;
							if (tagEMRSTRETCHDIBITS2.cxSrc == tagEMRSTRETCHDIBITS2.cxDest && tagEMRSTRETCHDIBITS2.cySrc == tagEMRSTRETCHDIBITS2.cyDest)
							{
								imageResolutionX = 0.72 / emfScaleX;
								imageResolutionY = 0.72 / emfScaleY;
							}
							pdfService.WriteImageInfo2(pp, imageResolutionX, imageResolutionY);
							break;
						}
						}
						break;
					case 0:
					{
						POINTD transformedXY = GetTransformedXY(ReadInt32(), ReadInt32());
						if (GDIobjects.Count > 0)
						{
							string value4 = $"{ConvertToString(emfX + emfCurrentPoint.X * emfScaleX)} {ConvertToString(emfY - emfCurrentPoint.Y * emfScaleY)} m";
							string text2 = $"{ConvertToString(emfX + transformedXY.X * emfScaleX)} {ConvertToString(emfY - transformedXY.Y * emfScaleY)} l";
							if (makePath)
							{
								emfPath.Add(text2);
							}
							else
							{
								GDIobjectInfo gDIobjectInfo9 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
								if (gDIobjectInfo9.Pen != null)
								{
									SetPdfColor(gDIobjectInfo9.Pen.lopnColor);
									double num24 = GetTransformedW(gDIobjectInfo9.Pen.lopnWidth) * emfScale;
									if (num24 == 0.0)
									{
										num24 = 0.1;
									}
									pageStream.WriteLine("{0} w", ConvertToString(num24 * 1.2));
									pageStream.WriteLine(value4);
									pageStream.WriteLine(text2 + " S");
								}
							}
						}
						emfCurrentPoint.X = transformedXY.X;
						emfCurrentPoint.Y = transformedXY.Y;
						break;
					}
					case 5:
						makePath = true;
						emfPath = new ArrayList();
						break;
					case 6:
						makePath = false;
						break;
					case 7:
						emfPath.Add("h");
						break;
					case 10:
					{
						RECTL rECTL11 = default(RECTL);
						rECTL11.Left = ReadInt32();
						rECTL11.Top = ReadInt32();
						rECTL11.Right = ReadInt32();
						rECTL11.Bottom = ReadInt32();
						if (GDIobjects.Count <= 0)
						{
							break;
						}
						GDIobjectInfo gDIobjectInfo19 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
						if (gDIobjectInfo19.Pen == null)
						{
							break;
						}
						SetPdfColor(gDIobjectInfo19.Pen.lopnColor);
						double num58 = GetTransformedW(gDIobjectInfo19.Pen.lopnWidth) * emfScale;
						if (num58 == 0.0)
						{
							num58 = 0.1;
						}
						pageStream.WriteLine("{0} w", ConvertToString(num58 * 1.2));
						foreach (string item in emfPath)
						{
							pageStream.WriteLine(item);
						}
						pageStream.WriteLine("S");
						emfPath.Clear();
						break;
					}
					case 8:
					{
						RECTL rECTL6 = default(RECTL);
						rECTL6.Left = ReadInt32();
						rECTL6.Top = ReadInt32();
						rECTL6.Right = ReadInt32();
						rECTL6.Bottom = ReadInt32();
						if (GDIobjects.Count <= 0)
						{
							break;
						}
						GDIobjectInfo gDIobjectInfo8 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedBrush)];
						if (gDIobjectInfo8.Brush == null)
						{
							break;
						}
						SetPdfBackColor(gDIobjectInfo8.Brush.lbColor);
						foreach (string item2 in emfPath)
						{
							pageStream.WriteLine(item2);
						}
						pageStream.WriteLine("f");
						emfPath.Clear();
						break;
					}
					case 1:
					case 2:
					case 3:
					case 4:
					case 9:
						break;
					}
					break;
				case 0:
				{
					tagENHMETAHEADER tagENHMETAHEADER = default(tagENHMETAHEADER);
					tagENHMETAHEADER.rclBounds.Left = ReadInt32();
					tagENHMETAHEADER.rclBounds.Top = ReadInt32();
					tagENHMETAHEADER.rclBounds.Right = ReadInt32();
					tagENHMETAHEADER.rclBounds.Bottom = ReadInt32();
					tagENHMETAHEADER.rclFrame.Left = ReadInt32();
					tagENHMETAHEADER.rclFrame.Top = ReadInt32();
					tagENHMETAHEADER.rclFrame.Right = ReadInt32();
					tagENHMETAHEADER.rclFrame.Bottom = ReadInt32();
					tagENHMETAHEADER.dSignature = ReadUInt32();
					tagENHMETAHEADER.nVersion = ReadUInt32();
					tagENHMETAHEADER.nBytes = ReadUInt32();
					tagENHMETAHEADER.nRecords = ReadUInt32();
					tagENHMETAHEADER.nHandles = ReadUInt16();
					tagENHMETAHEADER.sReserved = ReadUInt16();
					tagENHMETAHEADER.nDescription = ReadUInt32();
					tagENHMETAHEADER.offDescription = ReadUInt32();
					tagENHMETAHEADER.nPalEntries = ReadUInt32();
					tagENHMETAHEADER.szlDevice.cX = ReadInt32();
					tagENHMETAHEADER.szlDevice.cY = ReadInt32();
					tagENHMETAHEADER.szlMillimeters.cX = ReadInt32();
					tagENHMETAHEADER.szlMillimeters.cY = ReadInt32();
					tagENHMETAHEADER.cbPixelFormat = ReadUInt32();
					tagENHMETAHEADER.offPixelFormat = ReadUInt32();
					tagENHMETAHEADER.bOpenGL = ReadUInt32();
					emfOffsetX = tagENHMETAHEADER.rclBounds.Left;
					emfOffsetY = tagENHMETAHEADER.rclBounds.Top;
					int num16 = tagENHMETAHEADER.rclBounds.Right - tagENHMETAHEADER.rclBounds.Left;
					int num17 = tagENHMETAHEADER.rclBounds.Bottom - tagENHMETAHEADER.rclBounds.Top;
					if (emfOffsetX != 0)
					{
						num16 += emfOffsetX * 2;
						emfOffsetX = 0;
					}
					if (emfOffsetY != 0)
					{
						num17 += emfOffsetY * 2;
						emfOffsetY = 0;
					}
					emfScale = 0.72;
					emfScaleX = 0.71712;
					emfScaleY = 0.72;
					if (pdfService.report.RenderedWith == StiRenderedWith.Wpf)
					{
						emfScaleY *= StiOptions.Export.Pdf.WpfRichTextVerticalScaling;
					}
					double num18 = (double)tagENHMETAHEADER.szlDevice.cX / (double)tagENHMETAHEADER.szlMillimeters.cX * 25.4;
					double num19 = (double)tagENHMETAHEADER.szlDevice.cY / (double)tagENHMETAHEADER.szlMillimeters.cY * 25.4;
					double num20 = (num18 + num19) / 2.0;
					double num21 = (double)tagENHMETAHEADER.szlDevice.cX / (double)tagENHMETAHEADER.szlDevice.cY;
					double num22 = (double)tagENHMETAHEADER.szlMillimeters.cX / (double)tagENHMETAHEADER.szlMillimeters.cY;
					double num23 = Math.Abs((num21 - num22) / num21);
					if (num20 > 110.0 && num23 < 0.04)
					{
						emfScale *= 96.0 / num20;
						emfScaleX *= 96.0 / num18;
						emfScaleY *= 96.0 / num19;
					}
					RectangleD rect = default(RectangleD);
					if (emfComponent.Component is StiRichText)
					{
						rect = (emfComponent.Component as StiRichText).ConvertTextMargins(rect, convert: false);
					}
					RectangleD val6 = default(RectangleD);
					((RectangleD)(ref val6))._002Ector(emfComponent.X + ((RectangleD)(ref rect)).X * 0.72, emfComponent.Y + (0.0 - ((RectangleD)(ref rect)).Bottom) * 0.72, emfComponent.Width + ((RectangleD)(ref rect)).Width * 0.72, emfComponent.Height + ((RectangleD)(ref rect)).Height * 0.72);
					emfX = ((RectangleD)(ref val6)).X + (centerEmf ? ((((RectangleD)(ref val6)).Width - (double)num16 * emfScale) / 2.0) : 0.0);
					emfY = ((RectangleD)(ref val6)).Y + ((RectangleD)(ref val6)).Height - (centerEmf ? ((((RectangleD)(ref val6)).Height - (double)num17 * emfScale) / 2.0) : 0.0);
					baseXForm = new XFORM(1f, 0f, 0f, 1f, 0f, 0f);
					viewportFlags = 0u;
					emfMapMode = 0u;
					dcStack = new Stack();
					break;
				}
				case 17:
					emfBkMode = ReadUInt32();
					break;
				case 16:
					emfMapMode = ReadUInt32();
					break;
				case 21:
					emfTextAlign = ReadUInt32();
					break;
				case 38:
				{
					uint ihNumber3 = ReadUInt32();
					uint lbStyle = ReadUInt32();
					uint lbColor = ReadUInt32();
					uint lbHatch = ReadUInt32();
					GDIobjectInfo gDIobjectInfo7 = new GDIobjectInfo();
					gDIobjectInfo7.ihNumber = ihNumber3;
					EmfBrushInfo emfBrushInfo = new EmfBrushInfo();
					emfBrushInfo.lbStyle = lbStyle;
					emfBrushInfo.lbColor = lbColor;
					emfBrushInfo.lbHatch = lbHatch;
					gDIobjectInfo7.Brush = emfBrushInfo;
					RemoveObject(gDIobjectInfo7.ihNumber);
					GDIobjects.Add(gDIobjectInfo7);
					break;
				}
				case 37:
				{
					uint ihNumber2 = ReadUInt32();
					uint lopnStyle = ReadUInt32();
					POINTL pOINTL3 = default(POINTL);
					pOINTL3.X = ReadInt32();
					pOINTL3.Y = ReadInt32();
					uint lopnColor = ReadUInt32();
					GDIobjectInfo gDIobjectInfo4 = new GDIobjectInfo();
					gDIobjectInfo4.ihNumber = ihNumber2;
					EmfPenInfo emfPenInfo = new EmfPenInfo();
					emfPenInfo.lopnStyle = lopnStyle;
					emfPenInfo.lopnWidth = pOINTL3.X;
					emfPenInfo.lopnColor = lopnColor;
					gDIobjectInfo4.Pen = emfPenInfo;
					RemoveObject(gDIobjectInfo4.ihNumber);
					GDIobjects.Add(gDIobjectInfo4);
					break;
				}
				case 36:
					emfSelectedObject = ReadUInt32();
					SelectObject(emfSelectedObject);
					break;
				case 39:
					RemoveObject(ReadUInt32());
					break;
				case 23:
					emfTextColor = ReadUInt32();
					break;
				case 24:
					emfBackColor = ReadUInt32();
					break;
				case 26:
					emfCurrentPoint = GetTransformedXY(ReadInt32(), ReadInt32());
					if (makePath)
					{
						string value14 = $"{ConvertToString(emfX + emfCurrentPoint.X * emfScaleX)} {ConvertToString(emfY - emfCurrentPoint.Y * emfScaleY)} m";
						emfPath.Add(value14);
					}
					break;
				case 42:
				{
					RECTL rECTL5 = default(RECTL);
					rECTL5.Left = ReadInt32();
					rECTL5.Top = ReadInt32();
					rECTL5.Right = ReadInt32();
					rECTL5.Bottom = ReadInt32();
					RectangleD val5 = default(RectangleD);
					((RectangleD)(ref val5))._002Ector(GetTransformedX(rECTL5.Left), GetTransformedY(rECTL5.Top), GetTransformedX(rECTL5.Right) - GetTransformedX(rECTL5.Left), GetTransformedY(rECTL5.Bottom) - GetTransformedY(rECTL5.Top));
					if (GDIobjects.Count <= 0)
					{
						break;
					}
					bool flag = false;
					bool flag2 = false;
					GDIobjectInfo gDIobjectInfo5 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedPen)];
					if (gDIobjectInfo5.Pen != null)
					{
						SetPdfColor(gDIobjectInfo5.Pen.lopnColor);
						double num15 = GetTransformedW(gDIobjectInfo5.Pen.lopnWidth) * emfScale;
						if (num15 == 0.0)
						{
							num15 = 0.1;
						}
						pageStream.WriteLine("{0} w", ConvertToString(num15 * 1.2));
						flag = true;
					}
					GDIobjectInfo gDIobjectInfo6 = (GDIobjectInfo)GDIobjects[GetGdiObjectIndex(gdiSelectedBrush)];
					if (gDIobjectInfo6.Brush != null)
					{
						SetPdfBackColor(gDIobjectInfo6.Brush.lbColor);
						flag2 = true;
					}
					ArrayList arrayList = new ArrayList();
					string value = $"{ConvertToString(emfX + ((RectangleD)(ref val5)).Left * emfScaleX)} {ConvertToString(emfY - ((RectangleD)(ref val5)).Bottom * emfScaleY)} m";
					arrayList.Add(value);
					string value2 = $"{ConvertToString(emfX + ((RectangleD)(ref val5)).Left * emfScaleX)} {ConvertToString(emfY - ((RectangleD)(ref val5)).Top * emfScaleY)} l";
					arrayList.Add(value2);
					value2 = $"{ConvertToString(emfX + ((RectangleD)(ref val5)).Right * emfScaleX)} {ConvertToString(emfY - ((RectangleD)(ref val5)).Top * emfScaleY)} l";
					arrayList.Add(value2);
					value2 = $"{ConvertToString(emfX + ((RectangleD)(ref val5)).Right * emfScaleX)} {ConvertToString(emfY - ((RectangleD)(ref val5)).Bottom * emfScaleY)} l";
					arrayList.Add(value2);
					string text = ((!flag2) ? (flag ? "h S" : string.Empty) : (flag ? "h B" : "h f"));
					if (text != string.Empty)
					{
						for (int k = 0; k < arrayList.Count; k++)
						{
							pageStream.WriteLine((string)arrayList[k]);
						}
						pageStream.WriteLine(text);
					}
					arrayList.Clear();
					break;
				}
				case 18:
					emfPolyFillMode = ReadUInt32();
					break;
				case 34:
				{
					XFORM xFORM2 = default(XFORM);
					xFORM2.eM11 = ReadSingle();
					xFORM2.eM12 = ReadSingle();
					xFORM2.eM21 = ReadSingle();
					xFORM2.eM22 = ReadSingle();
					xFORM2.eDx = ReadSingle();
					xFORM2.eDy = ReadSingle();
					baseXForm = xFORM2;
					break;
				}
				case 35:
				{
					XFORM xFORM = default(XFORM);
					xFORM.eM11 = ReadSingle();
					xFORM.eM12 = ReadSingle();
					xFORM.eM21 = ReadSingle();
					xFORM.eM22 = ReadSingle();
					xFORM.eDx = ReadSingle();
					xFORM.eDy = ReadSingle();
					baseXForm.eM11 *= xFORM.eM11;
					baseXForm.eM22 *= xFORM.eM22;
					break;
				}
				case 11:
					viewportOrg = new PointD(0.0, 0.0);
					((PointD)(ref viewportOrg)).X = GetTransformedX(ReadInt32());
					((PointD)(ref viewportOrg)).Y = GetTransformedY(ReadInt32());
					viewportFlags |= 1u;
					break;
				case 10:
					viewportExt = new PointD(0.0, 0.0);
					((PointD)(ref viewportExt)).X = GetTransformedCX(ReadInt32());
					((PointD)(ref viewportExt)).Y = GetTransformedCY(ReadInt32());
					viewportFlags |= 2u;
					break;
				case 9:
					windowOrg = new PointD(0.0, 0.0);
					((PointD)(ref windowOrg)).X = GetTransformedX(ReadInt32());
					((PointD)(ref windowOrg)).Y = GetTransformedY(ReadInt32());
					viewportFlags |= 4u;
					break;
				case 8:
					windowExt = new PointD(0.0, 0.0);
					((PointD)(ref windowExt)).X = GetTransformedCX(ReadInt32());
					((PointD)(ref windowExt)).Y = GetTransformedCY(ReadInt32());
					viewportFlags |= 8u;
					break;
				case 32:
					pageStream.WriteLine("q");
					dcData = new DCData();
					dcData.emfBackColor = emfBackColor;
					dcData.emfBkMode = emfBkMode;
					dcData.emfMapMode = emfMapMode;
					dcData.emfPolyFillMode = emfPolyFillMode;
					dcData.emfTextAlign = emfTextAlign;
					dcData.emfTextColor = emfTextColor;
					dcStack.Push(dcData);
					break;
				case 33:
					pageStream.WriteLine("Q");
					if (dcStack.Count > 0)
					{
						dcData = (DCData)dcStack.Pop();
						emfBackColor = dcData.emfBackColor;
						emfBkMode = dcData.emfBkMode;
						emfMapMode = dcData.emfMapMode;
						emfPolyFillMode = dcData.emfPolyFillMode;
						emfTextAlign = dcData.emfTextAlign;
						emfTextColor = dcData.emfTextColor;
					}
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 12:
				case 13:
				case 14:
				case 15:
				case 19:
				case 20:
				case 22:
				case 25:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
				case 40:
				case 41:
				case 43:
				case 44:
				case 45:
				case 46:
				case 47:
					break;
				}
			}
			else if ((int)recordType <= 16386)
			{
				if ((int)recordType != 114)
				{
					if (recordType - 16385 <= 1)
					{
					}
				}
				else
				{
					tagEMRALPHABLEND tagEMRALPHABLEND2 = default(tagEMRALPHABLEND);
					tagEMRALPHABLEND2.rclBounds.Left = ReadInt32();
					tagEMRALPHABLEND2.rclBounds.Top = ReadInt32();
					tagEMRALPHABLEND2.rclBounds.Right = ReadInt32();
					tagEMRALPHABLEND2.rclBounds.Bottom = ReadInt32();
					tagEMRALPHABLEND2.xDest = (int)GetTransformedX(ReadInt32());
					tagEMRALPHABLEND2.yDest = (int)GetTransformedY(ReadInt32());
					tagEMRALPHABLEND2.cxDest = (int)GetTransformedCX(ReadInt32());
					tagEMRALPHABLEND2.cyDest = (int)GetTransformedCY(ReadInt32());
					tagEMRALPHABLEND2.dwRop = ReadUInt32();
					tagEMRALPHABLEND2.xSrc = ReadInt32();
					tagEMRALPHABLEND2.ySrc = ReadInt32();
					tagEMRALPHABLEND2.xformSrc.eM11 = ReadSingle();
					tagEMRALPHABLEND2.xformSrc.eM12 = ReadSingle();
					tagEMRALPHABLEND2.xformSrc.eM21 = ReadSingle();
					tagEMRALPHABLEND2.xformSrc.eM22 = ReadSingle();
					tagEMRALPHABLEND2.xformSrc.eDx = ReadSingle();
					tagEMRALPHABLEND2.xformSrc.eDy = ReadSingle();
					tagEMRALPHABLEND2.crBkColorSrc = ReadUInt32();
					tagEMRALPHABLEND2.iUsageSrc = ReadUInt32();
					tagEMRALPHABLEND2.offBmiSrc = ReadUInt32();
					tagEMRALPHABLEND2.cbBmiSrc = ReadUInt32();
					tagEMRALPHABLEND2.offBitsSrc = ReadUInt32();
					tagEMRALPHABLEND2.cbBitsSrc = ReadUInt32();
					tagEMRALPHABLEND2.cxSrc = ReadInt32();
					tagEMRALPHABLEND2.cySrc = ReadInt32();
					StiPdfExportService.StiPdfData pp2 = default(StiPdfExportService.StiPdfData);
					double num59 = 0.72 / emfScale;
					pp2.X = emfX + (double)tagEMRALPHABLEND2.xDest * emfScaleX;
					pp2.Y = emfY - (double)(tagEMRALPHABLEND2.yDest + tagEMRALPHABLEND2.cyDest) * emfScaleY;
					double imageResolutionX2 = (double)((float)tagEMRALPHABLEND2.cxSrc / (float)tagEMRALPHABLEND2.cxDest) * num59;
					double imageResolutionY2 = (double)((float)tagEMRALPHABLEND2.cySrc / (float)tagEMRALPHABLEND2.cyDest) * num59;
					if (tagEMRALPHABLEND2.cxSrc == tagEMRALPHABLEND2.cxDest && tagEMRALPHABLEND2.cySrc == tagEMRALPHABLEND2.cyDest)
					{
						imageResolutionX2 = 0.72 / emfScaleX;
						imageResolutionY2 = 0.72 / emfScaleY;
					}
					pdfService.WriteImageInfo2(pp2, imageResolutionX2, imageResolutionY2);
				}
			}
			else if ((int)recordType != 16388 && (int)recordType == 16432)
			{
			}
		}
		return true;
	}

	public PdfFonts GetPdfFonts()
	{
		return pdfFont;
	}

	public void Clear()
	{
		m_delegate = null;
		if (emfGr != null)
		{
			emfGr.Dispose();
		}
		emfGr = null;
		if (emfBmp != null)
		{
			((Image)emfBmp).Dispose();
		}
		emfBmp = null;
		pdfFont = null;
		pageStream = null;
		pdfService = null;
	}

	public void RenderRtf(StiPdfExportService.StiPdfData pp)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		pageStream = pdfService.pageStream;
		emfComponent = pp;
		StiRichText stiRichText = pp.Component as StiRichText;
		lock (lockMetafileFlag)
		{
			if (pdfService.report.RenderedWith == StiRenderedWith.Wpf)
			{
				double height = stiRichText.Height;
				stiRichText.Height *= (double)(1f / StiOptions.Export.Pdf.WpfRichTextVerticalScaling) * 1.01;
				stiRichText.RenderMetafile();
				stiRichText.Height = height;
			}
			else
			{
				stiRichText.RenderMetafile();
			}
			Image image = stiRichText.Image;
			Metafile val = (Metafile)(object)((image is Metafile) ? image : null);
			if (emfBmp == null)
			{
				emfBmp = new Bitmap(25, 25);
			}
			if (emfGr == null)
			{
				emfGr = Graphics.FromImage((Image)(object)emfBmp);
			}
			if (m_delegate == null)
			{
				m_delegate = new EnumerateMetafileProc(MetafileCallback);
			}
			try
			{
				assembleData = false;
				centerEmf = false;
				GDIobjects.Clear();
				AddStockGDIobjects();
				pageStream.WriteLine("q");
				pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				pdfService.PushColorToStack();
				emfGr.EnumerateMetafile(val, m_destPoint, m_delegate);
			}
			catch
			{
			}
			finally
			{
				pageStream.WriteLine("Q");
				pdfService.PopColorFromStack();
				GDIobjects.Clear();
				val = null;
				if (stiRichText.Image != null)
				{
					stiRichText.Image.Dispose();
					stiRichText.Image = null;
				}
			}
		}
	}

	public void AssembleRtf(StiComponent component)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		StiRichText stiRichText = component as StiRichText;
		lock (lockMetafileFlag)
		{
			if (pdfService.report.RenderedWith == StiRenderedWith.Wpf)
			{
				double height = stiRichText.Height;
				stiRichText.Height *= (double)(1f / StiOptions.Export.Pdf.WpfRichTextVerticalScaling) * 1.01;
				stiRichText.RenderMetafile();
				stiRichText.Height = height;
			}
			else
			{
				stiRichText.RenderMetafile();
			}
			Image image = stiRichText.Image;
			Metafile val = (Metafile)(object)((image is Metafile) ? image : null);
			if (emfBmp == null)
			{
				emfBmp = new Bitmap(25, 25);
			}
			if (emfGr == null)
			{
				emfGr = Graphics.FromImage((Image)(object)emfBmp);
			}
			if (m_delegate == null)
			{
				m_delegate = new EnumerateMetafileProc(MetafileCallback);
			}
			try
			{
				assembleData = true;
				GDIobjects.Clear();
				emfGr.EnumerateMetafile(val, m_destPoint, m_delegate);
			}
			catch
			{
			}
			finally
			{
				GDIobjects.Clear();
				val = null;
				if (stiRichText.Image != null)
				{
					stiRichText.Image.Dispose();
					stiRichText.Image = null;
				}
			}
		}
	}

	public void RenderTextToEmf(StiPdfExportService.StiPdfData pp, float imageResolution)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		pageStream = pdfService.pageStream;
		emfComponent = pp;
		StiText stiText = pp.Component as StiText;
		float zoom = imageResolution;
		lock (lockMetafileFlag)
		{
			Image image = stiText.GetImage(ref zoom, StiExportFormat.Pdf);
			Metafile val = (Metafile)(object)((image is Metafile) ? image : null);
			if (emfBmp == null)
			{
				emfBmp = new Bitmap(25, 25);
			}
			if (emfGr == null)
			{
				emfGr = Graphics.FromImage((Image)(object)emfBmp);
			}
			if (m_delegate == null)
			{
				m_delegate = new EnumerateMetafileProc(MetafileCallback);
			}
			try
			{
				assembleData = false;
				centerEmf = false;
				GDIobjects.Clear();
				AddStockGDIobjects();
				pageStream.WriteLine("q");
				pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				pdfService.PushColorToStack();
				emfGr.EnumerateMetafile(val, m_destPoint, m_delegate);
			}
			catch
			{
			}
			finally
			{
				pageStream.WriteLine("Q");
				pdfService.PopColorFromStack();
				GDIobjects.Clear();
				((Image)val).Dispose();
			}
		}
	}

	public void AssembleTextToEmf(StiComponent component, float imageResolution)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		StiText stiText = component as StiText;
		float zoom = imageResolution;
		lock (lockMetafileFlag)
		{
			Image image = stiText.GetImage(ref zoom, StiExportFormat.Pdf);
			Metafile val = (Metafile)(object)((image is Metafile) ? image : null);
			if (emfBmp == null)
			{
				emfBmp = new Bitmap(25, 25);
			}
			if (emfGr == null)
			{
				emfGr = Graphics.FromImage((Image)(object)emfBmp);
			}
			if (m_delegate == null)
			{
				m_delegate = new EnumerateMetafileProc(MetafileCallback);
			}
			try
			{
				assembleData = true;
				GDIobjects.Clear();
				emfGr.EnumerateMetafile(val, m_destPoint, m_delegate);
			}
			catch
			{
			}
			finally
			{
				GDIobjects.Clear();
				((Image)val).Dispose();
			}
		}
	}

	public StiPdfMetafileRender(StiPdfExportService service, bool useUnicodeMode)
	{
		pdfService = service;
		pdfFont = service.pdfFont;
		this.useUnicodeMode = useUnicodeMode;
	}
}
