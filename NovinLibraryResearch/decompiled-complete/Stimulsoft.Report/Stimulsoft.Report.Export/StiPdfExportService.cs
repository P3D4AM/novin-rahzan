using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Maps.Geoms;
using Stimulsoft.Base.Services;
using Stimulsoft.Gauge.Painters;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.MathFormula;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export.Tools;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export;

[SuppressUnmanagedCodeSecurity]
[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Pdf.png")]
public class StiPdfExportService : StiExportService
{
	internal struct StiPdfData
	{
		public double X;

		public double Y;

		public double Width;

		public double Height;

		public StiComponent Component;

		public double Right => X + Width;

		public double Top => Y + Height;

		public StiPdfData Clone()
		{
			StiPdfData result = default(StiPdfData);
			result.X = X;
			result.Y = Y;
			result.Width = Width;
			result.Height = Height;
			result.Component = Component;
			return result;
		}
	}

	private struct StiImageData
	{
		public int Width;

		public int Height;

		public string Name;

		public StiImageFormat ImageFormat;
	}

	private struct StiShadingData
	{
		public double X;

		public double Y;

		public double Width;

		public double Height;

		public int Page;

		public double Angle;

		public int FunctionIndex;
	}

	private struct StiShadingFunctionData
	{
		public Color Color1;

		public Color Color2;

		public bool IsGlare;
	}

	private class StiBookmarkTreeNode
	{
		public int Parent;

		public int First;

		public int Last;

		public int Prev;

		public int Next;

		public int Count;

		public int Page;

		public double Y;

		public string Title;

		public string Guid;

		public bool Used;
	}

	private struct StiLinkObject
	{
		public double X;

		public double Y;

		public double Width;

		public double Height;

		public int Page;

		public int DestPage;

		public double DestY;

		public string Link;
	}

	private class StiEditableObject
	{
		public double X;

		public double Y;

		public double Width;

		public double Height;

		public int Page;

		public string Text;

		public byte[] Content;

		public byte[] Content2;

		public bool Multiline;

		public StiTextHorAlignment Alignment;

		public int FontNumber;

		public double FontSize;

		public Color FontColor;

		public StiComponent Component;
	}

	internal StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private float imageQuality = 0.75f;

	private float imageResolutionMain = 1f;

	private StiImageResolutionMode imageResolutionMode = StiImageResolutionMode.Exactly;

	private StreamWriter sw;

	private Stream stream2;

	internal MemoryStream memoryPageStream;

	internal StreamWriter pageStream;

	private Encoder enc;

	private ArrayList imageList;

	private StiImageCache imageCache;

	private Hashtable imageInterpolationTable;

	private Hashtable imageCacheIndexToList;

	private Hashtable imageInfoList;

	private int imageInfoCounter;

	private int imagesCounter;

	private int imagesCurrent;

	private int fontsCounter;

	private int bookmarksCounter;

	private int linksCounter;

	private int annotsCounter;

	private int annotsCurrent;

	private int annots2Counter;

	private int annots2Current;

	private int unsignedSignaturesCounter;

	private int shadingCounter;

	private int shadingFunctionCounter;

	private int shadingCurrent;

	private int hatchCounter;

	private int tooltipsCounter;

	private NumberFormatInfo currentNumberFormat;

	private string[] colorTable = new string[256];

	private bool[] alphaTable = new bool[256];

	internal PdfFonts pdfFont;

	private int precision_digits;

	private bool clipLongTextLines;

	private bool standardPdfFonts = true;

	private bool embeddedFonts;

	private bool useUnicodeMode;

	private bool reduceFontSize = true;

	private bool compressed;

	private bool compressedFonts;

	private bool encrypted;

	private bool usePdfA;

	private StiPdfComplianceMode pdfComplianceMode;

	private bool exportRtfTextAsImage;

	private StiPdfAutoPrintMode autoPrint = StiPdfAutoPrintMode.None;

	private StiPdfImageCompressionMethod imageCompressionMethod = StiPdfImageCompressionMethod.Jpeg;

	private StiImageFormat imageFormat = StiImageFormat.Color;

	private StiMonochromeDitheringType monochromeDitheringType = StiMonochromeDitheringType.FloydSteinberg;

	private StiPdfAllowEditable allowEditable = StiPdfAllowEditable.No;

	private List<StiPdfEmbeddedFileData> embeddedFiles;

	private bool useTransparency = true;

	private StiPdfZUGFeRDComplianceMode zugferdComplianceMode;

	private string zugferdConformanceLevel = "BASIC";

	private bool[] fontGlyphsReduceNotNeed;

	private SortedList xref;

	private ArrayList bookmarksTree;

	private ArrayList bookmarksTreeTemp;

	private ArrayList linksArray;

	private List<StiLinkObject> tagsArray;

	private List<StiLinkObject> tooltipsArray;

	private ArrayList annotsArray;

	private List<StiEditableObject> annots2Array;

	private List<StiEditableObject> unsignedSignaturesArray;

	private ArrayList shadingArray;

	private ArrayList hatchArray;

	private List<StiShadingFunctionData> shadingFunctionArray;

	private bool haveBookmarks;

	private bool haveLinks;

	private bool haveAnnots;

	private bool haveDigitalSignature;

	private bool haveTooltips;

	private int offsetSignatureData;

	private int offsetSignatureFilter;

	private int offsetSignatureName;

	private int offsetSignatureLen2;

	private static int[] CodePage1252part80AF = new int[32]
	{
		8364, 8226, 8218, 402, 8222, 8230, 8224, 8225, 710, 8240,
		352, 8249, 338, 8226, 381, 8226, 8226, 8216, 8217, 8220,
		8221, 8226, 8211, 8212, 732, 8482, 353, 8250, 339, 8226,
		382, 376
	};

	private int[] CodePage1252 = new int[256];

	private const double hiToTwips = 0.72;

	private int precision_digits_font = 3;

	private const float pdfCKT = 0.55228f;

	private const float italicAngleTanValue = 0.325f;

	private const float boldFontStrokeWidthValue = 0.031f;

	private byte[] IDValue;

	private string IDValueString = string.Empty;

	private string IDValueStringMeta = string.Empty;

	private string currentDateTime = string.Empty;

	private string currentDateTimeMeta = string.Empty;

	private string producerName = "Stimulsoft Reports";

	private string creatorName = string.Empty;

	private string keywords = string.Empty;

	private int currentObjectNumber;

	private int currentGenerationNumber;

	private StiPdfEncryptionKeyLength keyLength = StiPdfEncryptionKeyLength.Bit40;

	private byte lastColorStrokeA = byte.MaxValue;

	private byte lastColorNonStrokeA = byte.MaxValue;

	private Stack colorStack;

	private const int signatureDataLen = 16384;

	private RectangleD signaturePlacement;

	private int signaturePageNumber;

	private string digitalSignatureReason;

	private string digitalSignatureLocation;

	private string digitalSignatureContactInfo;

	private string digitalSignatureSignedBy;

	private Graphics graphicsForTextRenderer;

	private Image imageForGraphicsForTextRenderer;

	private Hashtable watermarkImageExist;

	private StiPdfStructure info;

	private StiPdfMetafileRender mfRender;

	private StiPdfSecurity pdfSecurity;

	private bool isWpf;

	private static bool isWpfException = false;

	private static string[] hatchData = new string[54]
	{
		"000000FF00000000", "1010101010101010", "8040201008040201", "0102040810204080", "101010FF10101010", "8142241818244281", "8000000008000000", "0010000100100001", "2200880022008800", "2288228822882288",
		"2255885522558855", "AA558A55AA55A855", "AA55AA55AA55AA55", "BB55EE55BB55EE55", "DD77DD77DD77DD77", "FFDDFF77FFDDFF77", "FF7FFFF7FF7FFFF7", "FF7FFFFFFFF7FFFF", "8844221188442211", "1122448811224488",
		"CC663399CC663399", "993366CC993366CC", "E070381C0E0783C1", "C183070E1C3870E0", "4040404040404040", "00FF000000FF0000", "AAAAAAAAAAAAAAAA", "FF00FF00FF00FF00", "CCCCCCCCCCCCCCCC", "FFFF0000FFFF0000",
		"8844221100000000", "1122448800000000", "F00000000F000000", "8080808008080808", "0240088004200110", "0C8DB130031BD8C0", "8403304884033048", "00304A8100304A81", "0102040818244281", "202020FF020202FF",
		"1422518854224588", "F0F0F0F0AA55AA55", "0100201020000102", "AA00800080008000", "0020008800020088", "8448300C02010103", "33FFCCFF33FFCCFF", "98F8F877898F8F77", "111111FF111111FF", "3333CCCC3333CCCC",
		"0F0F0F0FF0F0F0F0", "0502058850205088", "10387CFE7C381000", "0000000000000000"
	};

	private static List<string> reservedKeywords = new List<string> { "Title", "Author", "Subject", "Keywords", "Creator", "Producer", "CreationDate", "ModDate", "Trapped" };

	private static byte[] sRGBprofile = new byte[3144]
	{
		0, 0, 12, 72, 76, 105, 110, 111, 2, 16,
		0, 0, 109, 110, 116, 114, 82, 71, 66, 32,
		88, 89, 90, 32, 7, 206, 0, 2, 0, 9,
		0, 6, 0, 49, 0, 0, 97, 99, 115, 112,
		77, 83, 70, 84, 0, 0, 0, 0, 73, 69,
		67, 32, 115, 82, 71, 66, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		246, 214, 0, 1, 0, 0, 0, 0, 211, 45,
		72, 80, 32, 32, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 17, 99, 112, 114, 116, 0, 0, 1, 80,
		0, 0, 0, 51, 100, 101, 115, 99, 0, 0,
		1, 132, 0, 0, 0, 108, 119, 116, 112, 116,
		0, 0, 1, 240, 0, 0, 0, 20, 98, 107,
		112, 116, 0, 0, 2, 4, 0, 0, 0, 20,
		114, 88, 89, 90, 0, 0, 2, 24, 0, 0,
		0, 20, 103, 88, 89, 90, 0, 0, 2, 44,
		0, 0, 0, 20, 98, 88, 89, 90, 0, 0,
		2, 64, 0, 0, 0, 20, 100, 109, 110, 100,
		0, 0, 2, 84, 0, 0, 0, 112, 100, 109,
		100, 100, 0, 0, 2, 196, 0, 0, 0, 136,
		118, 117, 101, 100, 0, 0, 3, 76, 0, 0,
		0, 134, 118, 105, 101, 119, 0, 0, 3, 212,
		0, 0, 0, 36, 108, 117, 109, 105, 0, 0,
		3, 248, 0, 0, 0, 20, 109, 101, 97, 115,
		0, 0, 4, 12, 0, 0, 0, 36, 116, 101,
		99, 104, 0, 0, 4, 48, 0, 0, 0, 12,
		114, 84, 82, 67, 0, 0, 4, 60, 0, 0,
		8, 12, 103, 84, 82, 67, 0, 0, 4, 60,
		0, 0, 8, 12, 98, 84, 82, 67, 0, 0,
		4, 60, 0, 0, 8, 12, 116, 101, 120, 116,
		0, 0, 0, 0, 67, 111, 112, 121, 114, 105,
		103, 104, 116, 32, 40, 99, 41, 32, 49, 57,
		57, 56, 32, 72, 101, 119, 108, 101, 116, 116,
		45, 80, 97, 99, 107, 97, 114, 100, 32, 67,
		111, 109, 112, 97, 110, 121, 0, 0, 100, 101,
		115, 99, 0, 0, 0, 0, 0, 0, 0, 18,
		115, 82, 71, 66, 32, 73, 69, 67, 54, 49,
		57, 54, 54, 45, 50, 46, 49, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 18, 115,
		82, 71, 66, 32, 73, 69, 67, 54, 49, 57,
		54, 54, 45, 50, 46, 49, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 88, 89, 90, 32,
		0, 0, 0, 0, 0, 0, 243, 81, 0, 1,
		0, 0, 0, 1, 22, 204, 88, 89, 90, 32,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 88, 89, 90, 32,
		0, 0, 0, 0, 0, 0, 111, 162, 0, 0,
		56, 245, 0, 0, 3, 144, 88, 89, 90, 32,
		0, 0, 0, 0, 0, 0, 98, 153, 0, 0,
		183, 133, 0, 0, 24, 218, 88, 89, 90, 32,
		0, 0, 0, 0, 0, 0, 36, 160, 0, 0,
		15, 132, 0, 0, 182, 207, 100, 101, 115, 99,
		0, 0, 0, 0, 0, 0, 0, 22, 73, 69,
		67, 32, 104, 116, 116, 112, 58, 47, 47, 119,
		119, 119, 46, 105, 101, 99, 46, 99, 104, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		22, 73, 69, 67, 32, 104, 116, 116, 112, 58,
		47, 47, 119, 119, 119, 46, 105, 101, 99, 46,
		99, 104, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 100, 101,
		115, 99, 0, 0, 0, 0, 0, 0, 0, 46,
		73, 69, 67, 32, 54, 49, 57, 54, 54, 45,
		50, 46, 49, 32, 68, 101, 102, 97, 117, 108,
		116, 32, 82, 71, 66, 32, 99, 111, 108, 111,
		117, 114, 32, 115, 112, 97, 99, 101, 32, 45,
		32, 115, 82, 71, 66, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 46, 73, 69, 67,
		32, 54, 49, 57, 54, 54, 45, 50, 46, 49,
		32, 68, 101, 102, 97, 117, 108, 116, 32, 82,
		71, 66, 32, 99, 111, 108, 111, 117, 114, 32,
		115, 112, 97, 99, 101, 32, 45, 32, 115, 82,
		71, 66, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 100, 101, 115, 99, 0, 0,
		0, 0, 0, 0, 0, 44, 82, 101, 102, 101,
		114, 101, 110, 99, 101, 32, 86, 105, 101, 119,
		105, 110, 103, 32, 67, 111, 110, 100, 105, 116,
		105, 111, 110, 32, 105, 110, 32, 73, 69, 67,
		54, 49, 57, 54, 54, 45, 50, 46, 49, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		44, 82, 101, 102, 101, 114, 101, 110, 99, 101,
		32, 86, 105, 101, 119, 105, 110, 103, 32, 67,
		111, 110, 100, 105, 116, 105, 111, 110, 32, 105,
		110, 32, 73, 69, 67, 54, 49, 57, 54, 54,
		45, 50, 46, 49, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		118, 105, 101, 119, 0, 0, 0, 0, 0, 19,
		164, 254, 0, 20, 95, 46, 0, 16, 207, 20,
		0, 3, 237, 204, 0, 4, 19, 11, 0, 3,
		92, 158, 0, 0, 0, 1, 88, 89, 90, 32,
		0, 0, 0, 0, 0, 76, 9, 86, 0, 80,
		0, 0, 0, 87, 31, 231, 109, 101, 97, 115,
		0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 2, 143, 0, 0,
		0, 2, 115, 105, 103, 32, 0, 0, 0, 0,
		67, 82, 84, 32, 99, 117, 114, 118, 0, 0,
		0, 0, 0, 0, 4, 0, 0, 0, 0, 5,
		0, 10, 0, 15, 0, 20, 0, 25, 0, 30,
		0, 35, 0, 40, 0, 45, 0, 50, 0, 55,
		0, 59, 0, 64, 0, 69, 0, 74, 0, 79,
		0, 84, 0, 89, 0, 94, 0, 99, 0, 104,
		0, 109, 0, 114, 0, 119, 0, 124, 0, 129,
		0, 134, 0, 139, 0, 144, 0, 149, 0, 154,
		0, 159, 0, 164, 0, 169, 0, 174, 0, 178,
		0, 183, 0, 188, 0, 193, 0, 198, 0, 203,
		0, 208, 0, 213, 0, 219, 0, 224, 0, 229,
		0, 235, 0, 240, 0, 246, 0, 251, 1, 1,
		1, 7, 1, 13, 1, 19, 1, 25, 1, 31,
		1, 37, 1, 43, 1, 50, 1, 56, 1, 62,
		1, 69, 1, 76, 1, 82, 1, 89, 1, 96,
		1, 103, 1, 110, 1, 117, 1, 124, 1, 131,
		1, 139, 1, 146, 1, 154, 1, 161, 1, 169,
		1, 177, 1, 185, 1, 193, 1, 201, 1, 209,
		1, 217, 1, 225, 1, 233, 1, 242, 1, 250,
		2, 3, 2, 12, 2, 20, 2, 29, 2, 38,
		2, 47, 2, 56, 2, 65, 2, 75, 2, 84,
		2, 93, 2, 103, 2, 113, 2, 122, 2, 132,
		2, 142, 2, 152, 2, 162, 2, 172, 2, 182,
		2, 193, 2, 203, 2, 213, 2, 224, 2, 235,
		2, 245, 3, 0, 3, 11, 3, 22, 3, 33,
		3, 45, 3, 56, 3, 67, 3, 79, 3, 90,
		3, 102, 3, 114, 3, 126, 3, 138, 3, 150,
		3, 162, 3, 174, 3, 186, 3, 199, 3, 211,
		3, 224, 3, 236, 3, 249, 4, 6, 4, 19,
		4, 32, 4, 45, 4, 59, 4, 72, 4, 85,
		4, 99, 4, 113, 4, 126, 4, 140, 4, 154,
		4, 168, 4, 182, 4, 196, 4, 211, 4, 225,
		4, 240, 4, 254, 5, 13, 5, 28, 5, 43,
		5, 58, 5, 73, 5, 88, 5, 103, 5, 119,
		5, 134, 5, 150, 5, 166, 5, 181, 5, 197,
		5, 213, 5, 229, 5, 246, 6, 6, 6, 22,
		6, 39, 6, 55, 6, 72, 6, 89, 6, 106,
		6, 123, 6, 140, 6, 157, 6, 175, 6, 192,
		6, 209, 6, 227, 6, 245, 7, 7, 7, 25,
		7, 43, 7, 61, 7, 79, 7, 97, 7, 116,
		7, 134, 7, 153, 7, 172, 7, 191, 7, 210,
		7, 229, 7, 248, 8, 11, 8, 31, 8, 50,
		8, 70, 8, 90, 8, 110, 8, 130, 8, 150,
		8, 170, 8, 190, 8, 210, 8, 231, 8, 251,
		9, 16, 9, 37, 9, 58, 9, 79, 9, 100,
		9, 121, 9, 143, 9, 164, 9, 186, 9, 207,
		9, 229, 9, 251, 10, 17, 10, 39, 10, 61,
		10, 84, 10, 106, 10, 129, 10, 152, 10, 174,
		10, 197, 10, 220, 10, 243, 11, 11, 11, 34,
		11, 57, 11, 81, 11, 105, 11, 128, 11, 152,
		11, 176, 11, 200, 11, 225, 11, 249, 12, 18,
		12, 42, 12, 67, 12, 92, 12, 117, 12, 142,
		12, 167, 12, 192, 12, 217, 12, 243, 13, 13,
		13, 38, 13, 64, 13, 90, 13, 116, 13, 142,
		13, 169, 13, 195, 13, 222, 13, 248, 14, 19,
		14, 46, 14, 73, 14, 100, 14, 127, 14, 155,
		14, 182, 14, 210, 14, 238, 15, 9, 15, 37,
		15, 65, 15, 94, 15, 122, 15, 150, 15, 179,
		15, 207, 15, 236, 16, 9, 16, 38, 16, 67,
		16, 97, 16, 126, 16, 155, 16, 185, 16, 215,
		16, 245, 17, 19, 17, 49, 17, 79, 17, 109,
		17, 140, 17, 170, 17, 201, 17, 232, 18, 7,
		18, 38, 18, 69, 18, 100, 18, 132, 18, 163,
		18, 195, 18, 227, 19, 3, 19, 35, 19, 67,
		19, 99, 19, 131, 19, 164, 19, 197, 19, 229,
		20, 6, 20, 39, 20, 73, 20, 106, 20, 139,
		20, 173, 20, 206, 20, 240, 21, 18, 21, 52,
		21, 86, 21, 120, 21, 155, 21, 189, 21, 224,
		22, 3, 22, 38, 22, 73, 22, 108, 22, 143,
		22, 178, 22, 214, 22, 250, 23, 29, 23, 65,
		23, 101, 23, 137, 23, 174, 23, 210, 23, 247,
		24, 27, 24, 64, 24, 101, 24, 138, 24, 175,
		24, 213, 24, 250, 25, 32, 25, 69, 25, 107,
		25, 145, 25, 183, 25, 221, 26, 4, 26, 42,
		26, 81, 26, 119, 26, 158, 26, 197, 26, 236,
		27, 20, 27, 59, 27, 99, 27, 138, 27, 178,
		27, 218, 28, 2, 28, 42, 28, 82, 28, 123,
		28, 163, 28, 204, 28, 245, 29, 30, 29, 71,
		29, 112, 29, 153, 29, 195, 29, 236, 30, 22,
		30, 64, 30, 106, 30, 148, 30, 190, 30, 233,
		31, 19, 31, 62, 31, 105, 31, 148, 31, 191,
		31, 234, 32, 21, 32, 65, 32, 108, 32, 152,
		32, 196, 32, 240, 33, 28, 33, 72, 33, 117,
		33, 161, 33, 206, 33, 251, 34, 39, 34, 85,
		34, 130, 34, 175, 34, 221, 35, 10, 35, 56,
		35, 102, 35, 148, 35, 194, 35, 240, 36, 31,
		36, 77, 36, 124, 36, 171, 36, 218, 37, 9,
		37, 56, 37, 104, 37, 151, 37, 199, 37, 247,
		38, 39, 38, 87, 38, 135, 38, 183, 38, 232,
		39, 24, 39, 73, 39, 122, 39, 171, 39, 220,
		40, 13, 40, 63, 40, 113, 40, 162, 40, 212,
		41, 6, 41, 56, 41, 107, 41, 157, 41, 208,
		42, 2, 42, 53, 42, 104, 42, 155, 42, 207,
		43, 2, 43, 54, 43, 105, 43, 157, 43, 209,
		44, 5, 44, 57, 44, 110, 44, 162, 44, 215,
		45, 12, 45, 65, 45, 118, 45, 171, 45, 225,
		46, 22, 46, 76, 46, 130, 46, 183, 46, 238,
		47, 36, 47, 90, 47, 145, 47, 199, 47, 254,
		48, 53, 48, 108, 48, 164, 48, 219, 49, 18,
		49, 74, 49, 130, 49, 186, 49, 242, 50, 42,
		50, 99, 50, 155, 50, 212, 51, 13, 51, 70,
		51, 127, 51, 184, 51, 241, 52, 43, 52, 101,
		52, 158, 52, 216, 53, 19, 53, 77, 53, 135,
		53, 194, 53, 253, 54, 55, 54, 114, 54, 174,
		54, 233, 55, 36, 55, 96, 55, 156, 55, 215,
		56, 20, 56, 80, 56, 140, 56, 200, 57, 5,
		57, 66, 57, 127, 57, 188, 57, 249, 58, 54,
		58, 116, 58, 178, 58, 239, 59, 45, 59, 107,
		59, 170, 59, 232, 60, 39, 60, 101, 60, 164,
		60, 227, 61, 34, 61, 97, 61, 161, 61, 224,
		62, 32, 62, 96, 62, 160, 62, 224, 63, 33,
		63, 97, 63, 162, 63, 226, 64, 35, 64, 100,
		64, 166, 64, 231, 65, 41, 65, 106, 65, 172,
		65, 238, 66, 48, 66, 114, 66, 181, 66, 247,
		67, 58, 67, 125, 67, 192, 68, 3, 68, 71,
		68, 138, 68, 206, 69, 18, 69, 85, 69, 154,
		69, 222, 70, 34, 70, 103, 70, 171, 70, 240,
		71, 53, 71, 123, 71, 192, 72, 5, 72, 75,
		72, 145, 72, 215, 73, 29, 73, 99, 73, 169,
		73, 240, 74, 55, 74, 125, 74, 196, 75, 12,
		75, 83, 75, 154, 75, 226, 76, 42, 76, 114,
		76, 186, 77, 2, 77, 74, 77, 147, 77, 220,
		78, 37, 78, 110, 78, 183, 79, 0, 79, 73,
		79, 147, 79, 221, 80, 39, 80, 113, 80, 187,
		81, 6, 81, 80, 81, 155, 81, 230, 82, 49,
		82, 124, 82, 199, 83, 19, 83, 95, 83, 170,
		83, 246, 84, 66, 84, 143, 84, 219, 85, 40,
		85, 117, 85, 194, 86, 15, 86, 92, 86, 169,
		86, 247, 87, 68, 87, 146, 87, 224, 88, 47,
		88, 125, 88, 203, 89, 26, 89, 105, 89, 184,
		90, 7, 90, 86, 90, 166, 90, 245, 91, 69,
		91, 149, 91, 229, 92, 53, 92, 134, 92, 214,
		93, 39, 93, 120, 93, 201, 94, 26, 94, 108,
		94, 189, 95, 15, 95, 97, 95, 179, 96, 5,
		96, 87, 96, 170, 96, 252, 97, 79, 97, 162,
		97, 245, 98, 73, 98, 156, 98, 240, 99, 67,
		99, 151, 99, 235, 100, 64, 100, 148, 100, 233,
		101, 61, 101, 146, 101, 231, 102, 61, 102, 146,
		102, 232, 103, 61, 103, 147, 103, 233, 104, 63,
		104, 150, 104, 236, 105, 67, 105, 154, 105, 241,
		106, 72, 106, 159, 106, 247, 107, 79, 107, 167,
		107, 255, 108, 87, 108, 175, 109, 8, 109, 96,
		109, 185, 110, 18, 110, 107, 110, 196, 111, 30,
		111, 120, 111, 209, 112, 43, 112, 134, 112, 224,
		113, 58, 113, 149, 113, 240, 114, 75, 114, 166,
		115, 1, 115, 93, 115, 184, 116, 20, 116, 112,
		116, 204, 117, 40, 117, 133, 117, 225, 118, 62,
		118, 155, 118, 248, 119, 86, 119, 179, 120, 17,
		120, 110, 120, 204, 121, 42, 121, 137, 121, 231,
		122, 70, 122, 165, 123, 4, 123, 99, 123, 194,
		124, 33, 124, 129, 124, 225, 125, 65, 125, 161,
		126, 1, 126, 98, 126, 194, 127, 35, 127, 132,
		127, 229, 128, 71, 128, 168, 129, 10, 129, 107,
		129, 205, 130, 48, 130, 146, 130, 244, 131, 87,
		131, 186, 132, 29, 132, 128, 132, 227, 133, 71,
		133, 171, 134, 14, 134, 114, 134, 215, 135, 59,
		135, 159, 136, 4, 136, 105, 136, 206, 137, 51,
		137, 153, 137, 254, 138, 100, 138, 202, 139, 48,
		139, 150, 139, 252, 140, 99, 140, 202, 141, 49,
		141, 152, 141, 255, 142, 102, 142, 206, 143, 54,
		143, 158, 144, 6, 144, 110, 144, 214, 145, 63,
		145, 168, 146, 17, 146, 122, 146, 227, 147, 77,
		147, 182, 148, 32, 148, 138, 148, 244, 149, 95,
		149, 201, 150, 52, 150, 159, 151, 10, 151, 117,
		151, 224, 152, 76, 152, 184, 153, 36, 153, 144,
		153, 252, 154, 104, 154, 213, 155, 66, 155, 175,
		156, 28, 156, 137, 156, 247, 157, 100, 157, 210,
		158, 64, 158, 174, 159, 29, 159, 139, 159, 250,
		160, 105, 160, 216, 161, 71, 161, 182, 162, 38,
		162, 150, 163, 6, 163, 118, 163, 230, 164, 86,
		164, 199, 165, 56, 165, 169, 166, 26, 166, 139,
		166, 253, 167, 110, 167, 224, 168, 82, 168, 196,
		169, 55, 169, 169, 170, 28, 170, 143, 171, 2,
		171, 117, 171, 233, 172, 92, 172, 208, 173, 68,
		173, 184, 174, 45, 174, 161, 175, 22, 175, 139,
		176, 0, 176, 117, 176, 234, 177, 96, 177, 214,
		178, 75, 178, 194, 179, 56, 179, 174, 180, 37,
		180, 156, 181, 19, 181, 138, 182, 1, 182, 121,
		182, 240, 183, 104, 183, 224, 184, 89, 184, 209,
		185, 74, 185, 194, 186, 59, 186, 181, 187, 46,
		187, 167, 188, 33, 188, 155, 189, 21, 189, 143,
		190, 10, 190, 132, 190, 255, 191, 122, 191, 245,
		192, 112, 192, 236, 193, 103, 193, 227, 194, 95,
		194, 219, 195, 88, 195, 212, 196, 81, 196, 206,
		197, 75, 197, 200, 198, 70, 198, 195, 199, 65,
		199, 191, 200, 61, 200, 188, 201, 58, 201, 185,
		202, 56, 202, 183, 203, 54, 203, 182, 204, 53,
		204, 181, 205, 53, 205, 181, 206, 54, 206, 182,
		207, 55, 207, 184, 208, 57, 208, 186, 209, 60,
		209, 190, 210, 63, 210, 193, 211, 68, 211, 198,
		212, 73, 212, 203, 213, 78, 213, 209, 214, 85,
		214, 216, 215, 92, 215, 224, 216, 100, 216, 232,
		217, 108, 217, 241, 218, 118, 218, 251, 219, 128,
		220, 5, 220, 138, 221, 16, 221, 150, 222, 28,
		222, 162, 223, 41, 223, 175, 224, 54, 224, 189,
		225, 68, 225, 204, 226, 83, 226, 219, 227, 99,
		227, 235, 228, 115, 228, 252, 229, 132, 230, 13,
		230, 150, 231, 31, 231, 169, 232, 50, 232, 188,
		233, 70, 233, 208, 234, 91, 234, 229, 235, 112,
		235, 251, 236, 134, 237, 17, 237, 156, 238, 40,
		238, 180, 239, 64, 239, 204, 240, 88, 240, 229,
		241, 114, 241, 255, 242, 140, 243, 25, 243, 167,
		244, 52, 244, 194, 245, 80, 245, 222, 246, 109,
		246, 251, 247, 138, 248, 25, 248, 168, 249, 56,
		249, 199, 250, 87, 250, 231, 251, 119, 252, 7,
		252, 152, 253, 41, 253, 186, 254, 75, 254, 220,
		255, 109, 255, 255
	};

	private static bool compatibleMode160 = false;

	private static bool printScaling = true;

	private Hashtable colorHash1 = new Hashtable();

	private Hashtable colorHash2 = new Hashtable();

	private static string[,] gsTable = null;

	private static object lockSplitTextIntoLinesEventFlag = new object();

	public override string DefaultExtension => "pdf";

	public override StiExportFormat ExportFormat => StiExportFormat.Pdf;

	public override string GroupCategory => "Document";

	public override int Position => 0;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypePdfFile");

	public override bool MultipleFiles => false;

	private static double fontCorrectValue
	{
		get
		{
			if (CompatibleMode160)
			{
				return 1.0;
			}
			return 0.957;
		}
	}

	public static bool CompatibleMode160
	{
		get
		{
			return compatibleMode160;
		}
		set
		{
			compatibleMode160 = value;
		}
	}

	public static bool PrintScaling
	{
		get
		{
			return printScaling;
		}
		set
		{
			printScaling = value;
		}
	}

	private static string[,] GsTable
	{
		get
		{
			if (gsTable == null)
			{
				gsTable = new string[256, 2];
				for (int i = 0; i < 256; i++)
				{
					gsTable[i, 0] = $"/GS{$"{i:X2}"}S gs";
					gsTable[i, 1] = $"/GS{$"{i:X2}"}N gs";
				}
			}
			return gsTable;
		}
	}

	public static event StiSplitTextIntoLinesEventHandler SplitTextIntoLinesEvent;

	internal StiPdfData RenderIndicators(StiPdfData pp)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Invalid comparison between Unknown and I4
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected I4, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Invalid comparison between Unknown and I4
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Invalid comparison between Unknown and I4
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Invalid comparison between Unknown and I4
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Invalid comparison between Unknown and I4
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Invalid comparison between Unknown and I4
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		if (!(pp.Component is StiText { Indicator: not null } stiText))
		{
			return pp;
		}
		RectangleD val = default(RectangleD);
		((RectangleD)(ref val))._002Ector(pp.X, pp.Y, pp.Width, pp.Height);
		if (stiText.Indicator is StiIconSetIndicator stiIconSetIndicator && (stiIconSetIndicator.Icon != 0 || stiIconSetIndicator.CustomIcon != null))
		{
			StiImageData stiImageData = (StiImageData)imageList[imagesCurrent];
			float num = (float)((double)stiImageData.Width * 0.72);
			float num2 = (float)((double)stiImageData.Height * 0.72);
			RectangleF rectangleF = ((RectangleD)(ref val)).ToRectangleF();
			float num3 = 0.72f;
			rectangleF.Inflate(0f - num3, 0f - num3);
			ContentAlignment alignment = stiIconSetIndicator.Alignment;
			if ((int)alignment <= 32)
			{
				switch (alignment - 1)
				{
				default:
					if ((int)alignment != 16)
					{
						if ((int)alignment == 32)
						{
							rectangleF = new RectangleF(rectangleF.X + (rectangleF.Width - num) / 2f, rectangleF.Y + (rectangleF.Height - num2) / 2f, num, num2);
						}
					}
					else
					{
						rectangleF = new RectangleF(rectangleF.X, rectangleF.Y + (rectangleF.Height - num2) / 2f, num, num2);
						((RectangleD)(ref val)).X = ((RectangleD)(ref val)).X + (double)(rectangleF.Width + 1f);
						((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - (double)(rectangleF.Width + 1f);
					}
					break;
				case 0:
					rectangleF = new RectangleF(rectangleF.X, rectangleF.Bottom - num2, num, num2);
					((RectangleD)(ref val)).X = ((RectangleD)(ref val)).X + (double)(rectangleF.Width + 1f);
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - (double)(rectangleF.Width + 1f);
					break;
				case 1:
					rectangleF = new RectangleF(rectangleF.X + (rectangleF.Width - num) / 2f, rectangleF.Bottom - num2, num, num2);
					break;
				case 3:
					rectangleF = new RectangleF(rectangleF.Right - num, rectangleF.Bottom - num2, num, num2);
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - (double)(rectangleF.Width + 1f);
					break;
				case 2:
					break;
				}
			}
			else if ((int)alignment <= 256)
			{
				if ((int)alignment != 64)
				{
					if ((int)alignment == 256)
					{
						rectangleF = new RectangleF(rectangleF.X, rectangleF.Y, num, num2);
						((RectangleD)(ref val)).X = ((RectangleD)(ref val)).X + (double)(rectangleF.Width + 1f);
						((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - (double)(rectangleF.Width + 1f);
					}
				}
				else
				{
					rectangleF = new RectangleF(rectangleF.Right - num, rectangleF.Y + (rectangleF.Height - num2) / 2f, num, num2);
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - (double)(rectangleF.Width + 1f);
				}
			}
			else if ((int)alignment != 512)
			{
				if ((int)alignment == 1024)
				{
					rectangleF = new RectangleF(rectangleF.Right - num, rectangleF.Y, num, num2);
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - (double)(rectangleF.Width + 1f);
				}
			}
			else
			{
				rectangleF = new RectangleF(rectangleF.X + (rectangleF.Width - num) / 2f, rectangleF.Y, num, num2);
			}
			StiPdfData pp2 = default(StiPdfData);
			pp2.X = rectangleF.X;
			pp2.Y = rectangleF.Y;
			pp2.Width = rectangleF.Width;
			pp2.Height = rectangleF.Height;
			WriteImageInfo2(pp2, 1.0, 1.0);
		}
		if (stiText.Indicator is StiDataBarIndicator { Value: not 0f } stiDataBarIndicator)
		{
			RectangleD rect = val;
			float num4 = 0.72f;
			((RectangleD)(ref rect)).Inflate((double)(-2f * num4), (double)(-2f * num4));
			float num5 = stiDataBarIndicator.Maximum + Math.Abs(stiDataBarIndicator.Minimum);
			double num6 = ((RectangleD)(ref rect)).Width * (double)Math.Abs(stiDataBarIndicator.Minimum) / (double)num5;
			double num7 = ((RectangleD)(ref rect)).Width * (double)stiDataBarIndicator.Maximum / (double)num5;
			double num8 = ((RectangleD)(ref rect)).Width * (double)Math.Abs(stiDataBarIndicator.Value) / (double)num5;
			StiDataBarDirection stiDataBarDirection = stiDataBarIndicator.Direction;
			if (stiDataBarDirection == StiDataBarDirection.Default)
			{
				stiDataBarDirection = ((stiText.TextOptions == null || !stiText.TextOptions.RightToLeft) ? StiDataBarDirection.LeftToRight : StiDataBarDirection.RighToLeft);
			}
			float num9 = 0f;
			if (stiDataBarDirection == StiDataBarDirection.LeftToRight)
			{
				if (stiDataBarIndicator.Value < 0f)
				{
					((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + (num6 - num8);
				}
				else
				{
					((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + num6;
				}
				((RectangleD)(ref rect)).Width = num8;
			}
			else
			{
				num9 = 180f;
				if (stiDataBarIndicator.Value < 0f)
				{
					((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).Right - num6;
				}
				else
				{
					((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).Right - num6 - num8;
				}
				((RectangleD)(ref rect)).Width = num8;
			}
			if (stiDataBarIndicator.Value < 0f)
			{
				num9 += 180f;
			}
			bool flag = false;
			if (((RectangleD)(ref rect)).Width > 0.0 && ((RectangleD)(ref rect)).Height > 0.0)
			{
				if (stiDataBarIndicator.BrushType == Stimulsoft.Report.Components.StiBrushType.Gradient)
				{
					RectangleD val2 = default(RectangleD);
					if (stiDataBarDirection == StiDataBarDirection.LeftToRight)
					{
						if (stiDataBarIndicator.Value > 0f)
						{
							((RectangleD)(ref val2))._002Ector(((RectangleD)(ref val)).Left + num6, ((RectangleD)(ref val)).Top, num7, ((RectangleD)(ref val)).Height);
						}
						else
						{
							((RectangleD)(ref val2))._002Ector(((RectangleD)(ref val)).Left, ((RectangleD)(ref val)).Top, num6, ((RectangleD)(ref val)).Height);
						}
					}
					else if (stiDataBarIndicator.Value < 0f)
					{
						((RectangleD)(ref val2))._002Ector(((RectangleD)(ref val)).Left + num7, ((RectangleD)(ref val)).Top, num6, ((RectangleD)(ref val)).Height);
					}
					else
					{
						((RectangleD)(ref val2))._002Ector(((RectangleD)(ref val)).Left, ((RectangleD)(ref val)).Top, num7, ((RectangleD)(ref val)).Height);
					}
					if (((RectangleD)(ref rect)).Width > 0.0 && ((RectangleD)(ref rect)).Width < 1.0)
					{
						((RectangleD)(ref rect)).Width = 1.0;
					}
					if (((RectangleD)(ref val2)).Width > 0.0 && ((RectangleD)(ref val2)).Width < 1.0)
					{
						((RectangleD)(ref val2)).Width = 1.0;
					}
					if (((RectangleD)(ref val2)).Width > 0.0 && ((RectangleD)(ref rect)).Width > 0.0)
					{
						Color color = ((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeColor : stiDataBarIndicator.PositiveColor);
						Color color2 = StiColorUtils.Light(color, (byte)200);
						((RectangleD)(ref val2)).X = ((RectangleD)(ref val2)).X - ((RectangleD)(ref val2)).Width * 0.1;
						((RectangleD)(ref val2)).Width = ((RectangleD)(ref val2)).Width + ((RectangleD)(ref val2)).Width * 0.2;
						StiGradientBrush brush = new StiGradientBrush(color, color2, (double)num9);
						FillRectBrush((StiBrush)(object)brush, rect);
						StiShadingData stiShadingData = (StiShadingData)shadingArray[shadingCurrent - 1];
						stiShadingData.Angle = num9;
						stiShadingData.X = ((RectangleD)(ref val2)).X;
						stiShadingData.Y = ((RectangleD)(ref val2)).Y;
						stiShadingData.Width = ((RectangleD)(ref val2)).Width;
						stiShadingData.Height = ((RectangleD)(ref val2)).Height;
						stiShadingData.FunctionIndex = GetShadingFunctionNumber(color, color2, isGlare: false);
						shadingArray[shadingCurrent - 1] = stiShadingData;
						flag = true;
					}
				}
				else
				{
					StiSolidBrush brush2 = new StiSolidBrush((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeColor : stiDataBarIndicator.PositiveColor);
					FillRectBrush((StiBrush)(object)brush2, rect);
				}
				if (stiDataBarIndicator.ShowBorder)
				{
					pageStream.WriteLine("q");
					PushColorToStack();
					SetStrokeColor((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeBorderColor : stiDataBarIndicator.PositiveBorderColor);
					pageStream.WriteLine("{0} {1} {2} {3} re S", ConvertToString(((RectangleD)(ref rect)).X), ConvertToString(((RectangleD)(ref rect)).Y), ConvertToString(((RectangleD)(ref rect)).Width), ConvertToString(((RectangleD)(ref rect)).Height));
					pageStream.WriteLine("Q");
					PopColorFromStack();
				}
			}
			if (!flag && stiDataBarIndicator.BrushType == Stimulsoft.Report.Components.StiBrushType.Gradient)
			{
				shadingCurrent++;
			}
		}
		pp.X = ((RectangleD)(ref val)).X;
		pp.Width = ((RectangleD)(ref val)).Width;
		return pp;
	}

	private bool CheckShape(StiShape shape)
	{
		if (shape.ShapeType is StiVerticalLineShapeType || shape.ShapeType is StiHorizontalLineShapeType || shape.ShapeType is StiTopAndBottomLineShapeType || shape.ShapeType is StiLeftAndRightLineShapeType || shape.ShapeType is StiRectangleShapeType || shape.ShapeType is StiRoundedRectangleShapeType || shape.ShapeType is StiDiagonalDownLineShapeType || shape.ShapeType is StiDiagonalUpLineShapeType || shape.ShapeType is StiTriangleShapeType || shape.ShapeType is StiOvalShapeType || shape.ShapeType is StiArrowShapeType || shape.ShapeType is StiOctagonShapeType || shape.ShapeType is StiComplexArrowShapeType || shape.ShapeType is StiBentArrowShapeType || shape.ShapeType is StiChevronShapeType || shape.ShapeType is StiDivisionShapeType || shape.ShapeType is StiEqualShapeType || shape.ShapeType is StiFlowchartCardShapeType || shape.ShapeType is StiFlowchartCollateShapeType || shape.ShapeType is StiFlowchartDecisionShapeType || shape.ShapeType is StiFlowchartManualInputShapeType || shape.ShapeType is StiFlowchartOffPageConnectorShapeType || shape.ShapeType is StiFlowchartPreparationShapeType || shape.ShapeType is StiFlowchartSortShapeType || shape.ShapeType is StiFrameShapeType || shape.ShapeType is StiMinusShapeType || shape.ShapeType is StiMultiplyShapeType || shape.ShapeType is StiParallelogramShapeType || shape.ShapeType is StiPlusShapeType || shape.ShapeType is StiRegularPentagonShapeType || shape.ShapeType is StiTrapezoidShapeType || shape.ShapeType is StiSnipSameSideCornerRectangleShapeType || shape.ShapeType is StiSnipDiagonalSideCornerRectangleShapeType)
		{
			return true;
		}
		return false;
	}

	private void RenderShape(StiPdfData pp, float imageResolution)
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Invalid comparison between Unknown and I4
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected I4, but got Unknown
		//IL_49cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_49eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f5: Expected O, but got Unknown
		//IL_49f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a03: Unknown result type (might be due to invalid IL or missing references)
		if (!(pp.Component is StiShape stiShape))
		{
			return;
		}
		if (CheckShape(stiShape))
		{
			IStiBrush stiBrush = pp.Component as IStiBrush;
			StiPdfGeomWriter stiPdfGeomWriter = new StiPdfGeomWriter(pageStream, this);
			Color nonStrokeColor = Color.Transparent;
			if (stiBrush != null)
			{
				nonStrokeColor = StiBrush.ToColor(stiBrush.Brush);
			}
			if (nonStrokeColor.A != 0)
			{
				SetNonStrokeColor(nonStrokeColor);
			}
			if (stiBrush != null)
			{
				if (stiBrush.Brush is StiGradientBrush || stiBrush.Brush is StiGlareBrush)
				{
					StoreShadingData2(pp.X, pp.Y, pp.Width, pp.Height, stiBrush.Brush);
					pageStream.WriteLine("/Pattern cs /P{0} scn", 1 + shadingCurrent);
				}
				if (stiBrush.Brush is StiHatchBrush)
				{
					StiBrush brush = stiBrush.Brush;
					StiHatchBrush brush2 = (StiHatchBrush)(object)((brush is StiHatchBrush) ? brush : null);
					pageStream.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber(brush2) + 1);
				}
			}
			Color borderColor = stiShape.BorderColor;
			SetStrokeColor(borderColor);
			bool flag = nonStrokeColor.A != 0;
			bool flag2 = (int)stiShape.Style != 6;
			if (!flag && !flag2)
			{
				return;
			}
			string text = ((!flag) ? (flag2 ? "S" : "n") : (flag2 ? "B" : "f"));
			pageStream.WriteLine("{0} w", ConvertToString((double)stiShape.Size * 0.72));
			pageStream.WriteLine("q");
			double num = (double)stiShape.Size * 0.72 * 0.04;
			StiPenStyle style = stiShape.Style;
			switch (style - 1)
			{
			case 3:
				pageStream.WriteLine("[{0} {1}] 0 d", ConvertToString(num), ConvertToString(num * 55.0));
				break;
			case 0:
				pageStream.WriteLine("[{0} {1}] 0 d", ConvertToString(num * 50.0), ConvertToString(num * 55.0));
				break;
			case 1:
				pageStream.WriteLine("[{0} {1} {2} {1}] 0 d", ConvertToString(num * 50.0), ConvertToString(num * 55.0), ConvertToString(num));
				break;
			case 2:
				pageStream.WriteLine("[{0} {1} {2} {1} {2} {1}] 0 d", ConvertToString(num * 50.0), ConvertToString(num * 55.0), ConvertToString(num));
				break;
			}
			if (stiShape.ShapeType is StiVerticalLineShapeType)
			{
				if (flag)
				{
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				}
				if (flag2)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height));
				}
			}
			if (stiShape.ShapeType is StiHorizontalLineShapeType)
			{
				if (flag)
				{
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				}
				if (flag2)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height / 2.0));
				}
			}
			if (stiShape.ShapeType is StiTopAndBottomLineShapeType)
			{
				if (flag)
				{
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				}
				if (flag2)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
				}
			}
			if (stiShape.ShapeType is StiLeftAndRightLineShapeType)
			{
				if (flag)
				{
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				}
				if (flag2)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height));
				}
			}
			if (stiShape.ShapeType is StiRectangleShapeType)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X), ConvertToString(pp.Y));
			}
			if (stiShape.ShapeType is StiDiagonalDownLineShapeType)
			{
				if (flag)
				{
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				}
				if (flag2)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
				}
			}
			if (stiShape.ShapeType is StiDiagonalUpLineShapeType)
			{
				if (flag)
				{
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
				}
				if (flag2)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height));
				}
			}
			if (stiShape.ShapeType is StiTriangleShapeType)
			{
				StiShapeDirection direction = (stiShape.ShapeType as StiTriangleShapeType).Direction;
				if (direction == StiShapeDirection.Up)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X), ConvertToString(pp.Y));
				}
				if (direction == StiShapeDirection.Down)
				{
					text = ((!flag) ? (flag2 ? "S" : "n") : (flag2 ? "B*" : "f*"));
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
				}
				if (direction == StiShapeDirection.Left)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y));
				}
				if (direction == StiShapeDirection.Right)
				{
					text = ((!flag) ? (flag2 ? "S" : "n") : (flag2 ? "B*" : "f*"));
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X), ConvertToString(pp.Y));
				}
			}
			if (stiShape.ShapeType is StiOvalShapeType)
			{
				pageStream.WriteLine(stiPdfGeomWriter.GetEllipseString(new RectangleF((float)pp.X, (float)pp.Y, (float)pp.Width, (float)pp.Height)) + text);
			}
			if (stiShape.ShapeType is StiRoundedRectangleShapeType)
			{
				float round = (stiShape.ShapeType as StiRoundedRectangleShapeType).Round;
				double num2 = pp.Width;
				if (num2 > pp.Height)
				{
					num2 = pp.Height;
				}
				double num3 = Math.Min(num2, 70.0) * (double)round;
				double num4 = num3 * 0.4477199912071228;
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + num3));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height - num3));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height - num4), ConvertToString(pp.X + num4), ConvertToString(pp.Y + pp.Height), ConvertToString(pp.X + num3), ConvertToString(pp.Y + pp.Height));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num3), ConvertToString(pp.Y + pp.Height));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(pp.X + pp.Width - num4), ConvertToString(pp.Y + pp.Height), ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height - num4), ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height - num3));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + num3));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + num4), ConvertToString(pp.X + pp.Width - num4), ConvertToString(pp.Y), ConvertToString(pp.X + pp.Width - num3), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num3), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c " + text, ConvertToString(pp.X + num4), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y + num4), ConvertToString(pp.X), ConvertToString(pp.Y + num3));
			}
			if (stiShape.ShapeType is StiArrowShapeType)
			{
				StiShapeDirection direction2 = (stiShape.ShapeType as StiArrowShapeType).Direction;
				float arrowWidth = (stiShape.ShapeType as StiArrowShapeType).ArrowWidth;
				float arrowHeight = (stiShape.ShapeType as StiArrowShapeType).ArrowHeight;
				double num5 = pp.Width * (double)arrowWidth;
				double num6 = pp.Height * (double)arrowHeight;
				if (direction2 == StiShapeDirection.Left || direction2 == StiShapeDirection.Right)
				{
					num5 = pp.Height * (double)arrowWidth;
					num6 = pp.Width * (double)arrowHeight;
				}
				if (arrowHeight == 0f)
				{
					num6 = Math.Min(pp.Width / 2.0, pp.Height / 2.0);
				}
				if (direction2 == StiShapeDirection.Up)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + num5), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num5), ConvertToString(pp.Y + pp.Height - num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height - num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height - num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num5), ConvertToString(pp.Y + pp.Height - num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num5), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X + num5), ConvertToString(pp.Y));
				}
				if (direction2 == StiShapeDirection.Down)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width - num5), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num5), ConvertToString(pp.Y + num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num5), ConvertToString(pp.Y + num6));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num5), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X + pp.Width - num5), ConvertToString(pp.Y + pp.Height));
				}
				if (direction2 == StiShapeDirection.Left)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + num5));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num6), ConvertToString(pp.Y + num5));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num6), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num6), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num6), ConvertToString(pp.Y + pp.Height - num5));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height - num5));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + num5));
				}
				if (direction2 == StiShapeDirection.Right)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height - num5));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num6), ConvertToString(pp.Y + pp.Height - num5));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num6), ConvertToString(pp.Y + pp.Height));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num6), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num6), ConvertToString(pp.Y + num5));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num5));
					pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height - num5));
				}
			}
			if (stiShape.ShapeType is StiOctagonShapeType)
			{
				StiOctagonShapeType stiOctagonShapeType = (StiOctagonShapeType)stiShape.ShapeType;
				double num7 = (double)((stiShape.Report != null) ? ((float)stiShape.Report.Unit.ConvertToHInches((double)stiOctagonShapeType.Bevel)) : stiOctagonShapeType.Bevel) * 0.72;
				double num8 = num7;
				if (stiOctagonShapeType.AutoSize)
				{
					num7 = pp.Width / 3.413396120071411;
					num8 = pp.Height / 3.413396120071411;
				}
				if (num7 > pp.Width / 2.0)
				{
					num7 = pp.Width / 2.0;
				}
				if (num8 > pp.Height / 2.0)
				{
					num8 = pp.Height / 2.0;
				}
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + num7), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num7), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + num8));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y + pp.Height - num8));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width - num7), ConvertToString(pp.Y + pp.Height));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num7), ConvertToString(pp.Y + pp.Height));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height - num8));
				pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num8));
				pageStream.WriteLine("{0} {1} l " + text, ConvertToString(pp.X + num7), ConvertToString(pp.Y));
			}
			if (stiShape.ShapeType is StiComplexArrowShapeType)
			{
				double num9 = ((pp.Width < pp.Height) ? (pp.Width / 2.0) : (pp.Height / 2.0));
				double num10 = pp.Height / 3.799999952316284;
				double num11 = pp.Width / 3.799999952316284;
				double num12 = ((pp.Height < pp.Width) ? (pp.Height / 2.0) : (pp.Width / 2.0));
				switch ((stiShape.ShapeType as StiComplexArrowShapeType).Direction)
				{
				case StiShapeDirection.Left:
				case StiShapeDirection.Right:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num9), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num9), ConvertToString(pp.Y + num10));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num9), ConvertToString(pp.Y + num10));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num9), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num9), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num9), ConvertToString(pp.Top - num10));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num9), ConvertToString(pp.Top - num10));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num9), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y + pp.Height / 2.0));
					break;
				case StiShapeDirection.Up:
				case StiShapeDirection.Down:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y + num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Y + num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num11), ConvertToString(pp.Y + num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num11), ConvertToString(pp.Top - num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top - num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Top - num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num11), ConvertToString(pp.Top - num12));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num11), ConvertToString(pp.Y + num12));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y + num12));
					break;
				}
			}
			if (stiShape.ShapeType is StiBentArrowShapeType)
			{
				double num13 = 0.0;
				double num14 = 0.0;
				double num15 = 0.0;
				if (pp.Height > pp.Width)
				{
					num14 = pp.Width / 4.0;
					num13 = num14;
					num15 = num14 / 2.0;
				}
				else
				{
					num13 = (int)(pp.Height / 4.0);
					num14 = num13;
					num15 = num14 / 2.0;
				}
				switch ((stiShape.ShapeType as StiBentArrowShapeType).Direction)
				{
				case StiShapeDirection.Up:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num13));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - (num15 + num13)), ConvertToString(pp.Y + num13));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - (num15 + num13)), ConvertToString(pp.Top - num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num14 * 2.0), ConvertToString(pp.Top - num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num14), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top - num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num15), ConvertToString(pp.Top - num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num15), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y));
					break;
				case StiShapeDirection.Left:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.Right), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top - num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14), ConvertToString(pp.Top - num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Top - num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14), ConvertToString(pp.Top - num14 * 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14), ConvertToString(pp.Top - num14 - num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num13), ConvertToString(pp.Top - num14 - num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num13), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Y));
					break;
				case StiShapeDirection.Down:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.Right), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num15), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num15), ConvertToString(pp.Y + num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14 * 2.0), ConvertToString(pp.Y + num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14 + num15), ConvertToString(pp.Y + num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num14 + num15), ConvertToString(pp.Top - num13));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top - num13));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top));
					break;
				case StiShapeDirection.Right:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num14), ConvertToString(pp.Y + num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num14), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Y + num14));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num14), ConvertToString(pp.Y + num14 * 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num14), ConvertToString(pp.Y + num14 + num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num13), ConvertToString(pp.Y + num14 + num15));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num13), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Top));
					break;
				}
			}
			if (stiShape.ShapeType is StiChevronShapeType)
			{
				double num16 = ((pp.Width > pp.Height) ? (pp.Height / 2.0) : (pp.Width / 2.0));
				switch ((stiShape.ShapeType as StiChevronShapeType).Direction)
				{
				case StiShapeDirection.Right:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num16), ConvertToString(pp.Top - pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num16), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top - pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num16), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Top));
					break;
				case StiShapeDirection.Left:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.Right), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num16), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + num16), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right - num16), ConvertToString(pp.Top - pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top));
					break;
				case StiShapeDirection.Up:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top - num16));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top - num16));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + num16));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Top - num16));
					break;
				case StiShapeDirection.Down:
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top - num16));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.Right), ConvertToString(pp.Y + num16));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y));
					pageStream.WriteLine("{0} {1} l", ConvertToString(pp.X), ConvertToString(pp.Y + num16));
					pageStream.WriteLine("{0} {1} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Top));
					break;
				}
			}
			if (stiShape.ShapeType is StiDivisionShapeType)
			{
				double num17 = pp.Height / 3.0;
				float num18 = 2.88f;
				pageStream.WriteLine(stiPdfGeomWriter.GetRectString(pp.X, pp.Y + num17 + (double)num18, pp.Width, num17 - (double)(num18 * 2f)) + text);
				pageStream.WriteLine(stiPdfGeomWriter.GetEllipseString(pp.X + pp.Width / 2.0 - num17 / 2.0, pp.Top - 0.72 - num17, num17, num17) + text);
				pageStream.WriteLine(stiPdfGeomWriter.GetEllipseString(pp.X + pp.Width / 2.0 - num17 / 2.0, pp.Y + 1.44, num17, num17) + text);
			}
			if (stiShape.ShapeType is StiEqualShapeType)
			{
				double num19 = (pp.Height - pp.Height / 6.0) / 2.0;
				pageStream.WriteLine(stiPdfGeomWriter.GetRectString(pp.X, pp.Top - num19, pp.Width, num19) + text);
				pageStream.WriteLine(stiPdfGeomWriter.GetRectString(pp.X, pp.Y, pp.Width, num19) + text);
			}
			if (stiShape.ShapeType is StiFlowchartCardShapeType)
			{
				pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.Right), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 5.0), ConvertToString(pp.X + pp.Width / 5.0), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top));
			}
			if (stiShape.ShapeType is StiFlowchartCollateShapeType)
			{
				switch ((stiShape.ShapeType as StiFlowchartCollateShapeType).Direction)
				{
				case StiShapeDirection.Up:
				case StiShapeDirection.Down:
					pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height / 2.0), ConvertToString(pp.Right), ConvertToString(pp.Y));
					break;
				case StiShapeDirection.Left:
				case StiShapeDirection.Right:
					pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height / 2.0));
					pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height / 2.0), ConvertToString(pp.Right), ConvertToString(pp.Top));
					break;
				}
			}
			if (stiShape.ShapeType is StiFlowchartDecisionShapeType)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 2.0));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 2.0));
			}
			if (stiShape.ShapeType is StiFlowchartManualInputShapeType)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 5.0));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 5.0));
			}
			if (stiShape.ShapeType is StiFlowchartOffPageConnectorShapeType)
			{
				double num20 = pp.Height / 5.0;
				double num21 = pp.Width / 5.0;
				switch ((stiShape.ShapeType as StiFlowchartOffPageConnectorShapeType).Direction)
				{
				case StiShapeDirection.Down:
					pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.X), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Y + num20), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y + num20), ConvertToString(pp.X), ConvertToString(pp.Top));
					break;
				case StiShapeDirection.Up:
					pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - num20));
					pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top - num20), ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y));
					break;
				case StiShapeDirection.Left:
					pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.X + num21), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X + num21), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.X + num21), ConvertToString(pp.Top));
					break;
				case StiShapeDirection.Right:
					pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.X), ConvertToString(pp.Top), ConvertToString(pp.Right - num21), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.Right - num21), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top));
					break;
				}
			}
			if (stiShape.ShapeType is StiFlowchartPreparationShapeType)
			{
				double num22 = pp.Width / 5.0;
				double num23 = pp.Height / 5.0;
				double num24 = pp.Width / 2.0;
				double num25 = pp.Height / 2.0;
				switch ((stiShape.ShapeType as StiFlowchartPreparationShapeType).Direction)
				{
				case StiShapeDirection.Left:
				case StiShapeDirection.Right:
					pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(pp.X), ConvertToString(pp.Top - num25), ConvertToString(pp.X + num22), ConvertToString(pp.Top), ConvertToString(pp.Right - num22), ConvertToString(pp.Top));
					pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top - num25), ConvertToString(pp.Right - num22), ConvertToString(pp.Y), ConvertToString(pp.X + num22), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - num25));
					break;
				case StiShapeDirection.Up:
				case StiShapeDirection.Down:
					pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(pp.X + num24), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top - num23), ConvertToString(pp.Right), ConvertToString(pp.Y + num23));
					pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X + num24), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y + num23), ConvertToString(pp.X), ConvertToString(pp.Top - num23), ConvertToString(pp.X + num24), ConvertToString(pp.Top));
					break;
				}
			}
			if (stiShape.ShapeType is StiFlowchartSortShapeType)
			{
				pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.Right), ConvertToString(pp.Top - pp.Height / 2.0));
			}
			if (stiShape.ShapeType is StiFrameShapeType)
			{
				double num26 = pp.Width / 7.0;
				double num27 = pp.Height / 7.0;
				pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h", ConvertToString(pp.X), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y));
				pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X + num26), ConvertToString(pp.Top - num27), ConvertToString(pp.X + num26), ConvertToString(pp.Y + num27), ConvertToString(pp.Right - num26), ConvertToString(pp.Y + num27), ConvertToString(pp.Right - num26), ConvertToString(pp.Top - num27));
			}
			if (stiShape.ShapeType is StiMinusShapeType)
			{
				double num28 = pp.Height / 3.0;
				pageStream.WriteLine(stiPdfGeomWriter.GetRectString(pp.X, pp.Y + num28, pp.Width, num28) + text);
			}
			if (stiShape.ShapeType is StiMultiplyShapeType)
			{
				double num29 = pp.Width / 4.0;
				double num30 = pp.Height / 4.0;
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X), ConvertToString(pp.Top - num30));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l", ConvertToString(pp.X + num29), ConvertToString(pp.Top), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top - num30), ConvertToString(pp.Right - num29), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top - num30));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l", ConvertToString(pp.Right - num29), ConvertToString(pp.Top - pp.Height / 2.0), ConvertToString(pp.Right), ConvertToString(pp.Y + num30), ConvertToString(pp.Right - num29), ConvertToString(pp.Y), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + num30));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l h " + text, ConvertToString(pp.X + num29), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y + num30), ConvertToString(pp.X + num29), ConvertToString(pp.Top - pp.Height / 2.0));
			}
			if (stiShape.ShapeType is StiParallelogramShapeType)
			{
				double num31 = pp.Width / 7.0;
				double num32 = pp.Height / 7.0;
				pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X + pp.Width / 5.0), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top), ConvertToString(pp.Right - pp.Width / 5.0), ConvertToString(pp.Y));
			}
			if (stiShape.ShapeType is StiPlusShapeType)
			{
				double num33 = pp.Width / 3.0;
				double num34 = pp.Height / 3.0;
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + num33), ConvertToString(pp.Top));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l", ConvertToString(pp.Right - num33), ConvertToString(pp.Top), ConvertToString(pp.Right - num33), ConvertToString(pp.Top - num34), ConvertToString(pp.Right), ConvertToString(pp.Top - num34), ConvertToString(pp.Right), ConvertToString(pp.Y + num34));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l", ConvertToString(pp.Right - num33), ConvertToString(pp.Y + num34), ConvertToString(pp.Right - num33), ConvertToString(pp.Y), ConvertToString(pp.X + num33), ConvertToString(pp.Y), ConvertToString(pp.X + num33), ConvertToString(pp.Y + num34));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y + num34), ConvertToString(pp.X), ConvertToString(pp.Top - num34), ConvertToString(pp.X + num33), ConvertToString(pp.Top - num34), ConvertToString(pp.X + num33), ConvertToString(pp.Top));
			}
			if (stiShape.ShapeType is StiRegularPentagonShapeType)
			{
				double num35 = pp.Height / 2.5999999046325684;
				double num36 = pp.Width / 5.5;
				pageStream.WriteLine("{0} {1} m {2} {3} l", ConvertToString(pp.X), ConvertToString(pp.Top - num35), ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Top));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top - num35), ConvertToString(pp.Right - num36), ConvertToString(pp.Y), ConvertToString(pp.X + num36), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Top - num35));
			}
			if (stiShape.ShapeType is StiTrapezoidShapeType)
			{
				double num37 = pp.Width / 4.75;
				pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h " + text, ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.X + num37), ConvertToString(pp.Top), ConvertToString(pp.Right - num37), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Y));
			}
			if (stiShape.ShapeType is StiSnipSameSideCornerRectangleShapeType)
			{
				double num38 = pp.Width / 7.199999809265137;
				double num39 = pp.Height / 4.599999904632568;
				pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(pp.X), ConvertToString(pp.Top - num39), ConvertToString(pp.X + num38), ConvertToString(pp.Top), ConvertToString(pp.Right - num38), ConvertToString(pp.Top));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Top - num39), ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y));
			}
			if (stiShape.ShapeType is StiSnipDiagonalSideCornerRectangleShapeType)
			{
				double num40 = pp.Width / 7.199999809265137;
				double num41 = pp.Height / 4.599999904632568;
				pageStream.WriteLine("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(pp.X), ConvertToString(pp.Top), ConvertToString(pp.Right - num40), ConvertToString(pp.Top), ConvertToString(pp.Right), ConvertToString(pp.Top - num41));
				pageStream.WriteLine("{0} {1} l {2} {3} l {4} {5} l h " + text, ConvertToString(pp.Right), ConvertToString(pp.Y), ConvertToString(pp.X + num40), ConvertToString(pp.Y), ConvertToString(pp.X), ConvertToString(pp.Y + num41));
			}
			pageStream.WriteLine("Q");
			string parsedText = stiShape.GetParsedText();
			if (!string.IsNullOrWhiteSpace(parsedText))
			{
				StiText component = new StiText(stiShape.ClientRectangle, parsedText)
				{
					Font = stiShape.Font,
					TextBrush = (StiBrush)new StiSolidBrush(stiShape.ForeColor),
					HorAlignment = stiShape.HorAlignment,
					VertAlignment = stiShape.VertAlignment,
					Margins = stiShape.Margins,
					WordWrap = true,
					TextQuality = StiTextQuality.Typographic,
					Page = stiShape.Page
				};
				StiPdfData pp2 = pp.Clone();
				pp2.Component = component;
				RenderTextFont(pp2);
				RenderText(pp2);
			}
			return;
		}
		float imageResolution2 = imageResolution;
		Image image = stiShape.GetImage(ref imageResolution);
		try
		{
			WriteImageInfo(pp, imageResolution2);
		}
		finally
		{
			((IDisposable)image)?.Dispose();
		}
	}

	private void RenderPrimitives(StiPdfData pp)
	{
		if (pp.Component is StiRoundedRectanglePrimitive)
		{
			RenderRoundedRectanglePrimitive(pp);
		}
		else
		{
			RenderLinePrimitive(pp);
		}
	}

	private void RenderRoundedRectanglePrimitive(StiPdfData pp)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Invalid comparison between Unknown and I4
		if (!(pp.Component is IStiBorder stiBorder))
		{
			return;
		}
		StiRoundedRectanglePrimitive stiRoundedRectanglePrimitive = pp.Component as StiRoundedRectanglePrimitive;
		if ((int)stiRoundedRectanglePrimitive.Style == 6)
		{
			return;
		}
		StiBorderSide val = new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style);
		bool flag = (int)stiBorder.Border.Style != 6 && (int)stiBorder.Border.Style > 0;
		if (flag)
		{
			pageStream.WriteLine("q");
			PushColorToStack();
		}
		double num = StoreBorderSideData(val);
		double num2 = Math.Min((pp.Width < pp.Height) ? pp.Width : pp.Height, 100.0 * pp.Component.Page.Zoom) * (double)stiRoundedRectanglePrimitive.Round;
		double num3 = num2 * 0.4477199912071228;
		double num4 = pp.X - num;
		double num5 = pp.X + pp.Width + num;
		double num6 = pp.X + pp.Width / 2.0;
		double num7 = pp.Y - num;
		double num8 = pp.Y + pp.Height + num;
		if (stiRoundedRectanglePrimitive.LeftSide)
		{
			if (stiRoundedRectanglePrimitive.BottomSide)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(stiRoundedRectanglePrimitive.RightSide ? num6 : num5), ConvertToString(num7));
				pageStream.WriteLine("{0} {1} l", ConvertToString(num4 + num2), ConvertToString(num7));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num4 + num3), ConvertToString(num7), ConvertToString(num4), ConvertToString(num7 + num3), ConvertToString(num4), ConvertToString(num7 + num2));
			}
			else
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(num4), ConvertToString(num7));
			}
			if (stiRoundedRectanglePrimitive.TopSide)
			{
				pageStream.WriteLine("{0} {1} l", ConvertToString(num4), ConvertToString(num8 - num2));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num4), ConvertToString(num8 - num3), ConvertToString(num4 + num3), ConvertToString(num8), ConvertToString(num4 + num2), ConvertToString(num8));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(stiRoundedRectanglePrimitive.RightSide ? num6 : num5), ConvertToString(num8));
			}
			else
			{
				pageStream.WriteLine("{0} {1} l S", ConvertToString(num4), ConvertToString(num8));
			}
		}
		if (stiRoundedRectanglePrimitive.RightSide)
		{
			if (stiRoundedRectanglePrimitive.BottomSide)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(stiRoundedRectanglePrimitive.LeftSide ? num6 : num4), ConvertToString(num7));
				pageStream.WriteLine("{0} {1} l", ConvertToString(num5 - num2), ConvertToString(num7));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num5 - num3), ConvertToString(num7), ConvertToString(num5), ConvertToString(num7 + num3), ConvertToString(num5), ConvertToString(num7 + num2));
			}
			else
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(num5), ConvertToString(num7));
			}
			if (stiRoundedRectanglePrimitive.TopSide)
			{
				pageStream.WriteLine("{0} {1} l", ConvertToString(num5), ConvertToString(num8 - num2));
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num5), ConvertToString(num8 - num3), ConvertToString(num5 - num3), ConvertToString(num8), ConvertToString(num5 - num2), ConvertToString(num8));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(stiRoundedRectanglePrimitive.LeftSide ? num6 : num4), ConvertToString(num8));
			}
			else
			{
				pageStream.WriteLine("{0} {1} l S", ConvertToString(num5), ConvertToString(num8));
			}
		}
		if (stiRoundedRectanglePrimitive.TopSide && !stiRoundedRectanglePrimitive.LeftSide && !stiRoundedRectanglePrimitive.RightSide)
		{
			pageStream.WriteLine("{0} {1} m", ConvertToString(num4), ConvertToString(num8));
			pageStream.WriteLine("{0} {1} l S", ConvertToString(num5), ConvertToString(num8));
		}
		if (stiRoundedRectanglePrimitive.BottomSide && !stiRoundedRectanglePrimitive.LeftSide && !stiRoundedRectanglePrimitive.RightSide)
		{
			pageStream.WriteLine("{0} {1} m", ConvertToString(num4), ConvertToString(num7));
			pageStream.WriteLine("{0} {1} l S", ConvertToString(num5), ConvertToString(num7));
		}
		if ((int)val.Style == 5)
		{
			num2 -= num * 2.0;
			num3 = num2 * 0.4477199912071228;
			double num9 = pp.X + num;
			double num10 = pp.X + pp.Width - num;
			double num11 = pp.Y + num;
			double num12 = pp.Y + pp.Height - num;
			if (stiRoundedRectanglePrimitive.LeftSide)
			{
				if (stiRoundedRectanglePrimitive.BottomSide)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(stiRoundedRectanglePrimitive.RightSide ? num6 : num5), ConvertToString(num11));
					pageStream.WriteLine("{0} {1} l", ConvertToString(num9 + num2), ConvertToString(num11));
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num9 + num3), ConvertToString(num11), ConvertToString(num9), ConvertToString(num11 + num3), ConvertToString(num9), ConvertToString(num11 + num2));
				}
				else
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(num9), ConvertToString(num7));
				}
				if (stiRoundedRectanglePrimitive.TopSide)
				{
					pageStream.WriteLine("{0} {1} l", ConvertToString(num9), ConvertToString(num12 - num2));
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num9), ConvertToString(num12 - num3), ConvertToString(num9 + num3), ConvertToString(num12), ConvertToString(num9 + num2), ConvertToString(num12));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(stiRoundedRectanglePrimitive.RightSide ? num6 : num5), ConvertToString(num12));
				}
				else
				{
					pageStream.WriteLine("{0} {1} l S", ConvertToString(num9), ConvertToString(num8));
				}
			}
			if (stiRoundedRectanglePrimitive.RightSide)
			{
				if (stiRoundedRectanglePrimitive.BottomSide)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(stiRoundedRectanglePrimitive.LeftSide ? num6 : num4), ConvertToString(num11));
					pageStream.WriteLine("{0} {1} l", ConvertToString(num10 - num2), ConvertToString(num11));
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num10 - num3), ConvertToString(num11), ConvertToString(num10), ConvertToString(num11 + num3), ConvertToString(num10), ConvertToString(num11 + num2));
				}
				else
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(num10), ConvertToString(num7));
				}
				if (stiRoundedRectanglePrimitive.TopSide)
				{
					pageStream.WriteLine("{0} {1} l", ConvertToString(num10), ConvertToString(num12 - num2));
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} c", ConvertToString(num10), ConvertToString(num12 - num3), ConvertToString(num10 - num3), ConvertToString(num12), ConvertToString(num10 - num2), ConvertToString(num12));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(stiRoundedRectanglePrimitive.LeftSide ? num6 : num4), ConvertToString(num12));
				}
				else
				{
					pageStream.WriteLine("{0} {1} l S", ConvertToString(num10), ConvertToString(num8));
				}
			}
			if (stiRoundedRectanglePrimitive.TopSide && !stiRoundedRectanglePrimitive.LeftSide && !stiRoundedRectanglePrimitive.RightSide)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(num4), ConvertToString(num12));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(num5), ConvertToString(num12));
			}
			if (stiRoundedRectanglePrimitive.BottomSide && !stiRoundedRectanglePrimitive.LeftSide && !stiRoundedRectanglePrimitive.RightSide)
			{
				pageStream.WriteLine("{0} {1} m", ConvertToString(num4), ConvertToString(num11));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(num5), ConvertToString(num11));
			}
		}
		if (flag)
		{
			pageStream.WriteLine("Q");
			PopColorFromStack();
		}
	}

	private void RenderLinePrimitive(StiPdfData pp)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Invalid comparison between Unknown and I4
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Invalid comparison between Unknown and I4
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		StiLinePrimitive stiLinePrimitive = pp.Component as StiLinePrimitive;
		StiHorizontalLinePrimitive stiHorizontalLinePrimitive = pp.Component as StiHorizontalLinePrimitive;
		StiVerticalLinePrimitive stiVerticalLinePrimitive = pp.Component as StiVerticalLinePrimitive;
		double x = pp.X;
		double top = pp.Top;
		double num = pp.X + ((stiHorizontalLinePrimitive != null) ? pp.Width : 0.0);
		double num2 = pp.Top - ((stiVerticalLinePrimitive != null) ? pp.Height : 0.0);
		if ((int)stiLinePrimitive.Style != 6)
		{
			StiBorderSide val = new StiBorderSide(stiLinePrimitive.Color, (double)stiLinePrimitive.Size, stiLinePrimitive.Style);
			bool flag = (int)val.Style != 6 && (int)val.Style > 0;
			if (flag)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
			}
			double num3 = StoreBorderSideData(val);
			if (stiHorizontalLinePrimitive != null)
			{
				pageStream.WriteLine("{0} {1} m {2} {1} l S", ConvertToString(x), ConvertToString(top - num3), ConvertToString(num));
				if ((int)val.Style == 5)
				{
					pageStream.WriteLine("{0} {1} m {2} {1} l S", ConvertToString(x), ConvertToString(top + num3), ConvertToString(num));
				}
			}
			if (stiVerticalLinePrimitive != null)
			{
				pageStream.WriteLine("{0} {1} m {0} {2} l S", ConvertToString(x - num3), ConvertToString(top), ConvertToString(num2));
				if ((int)val.Style == 5)
				{
					pageStream.WriteLine("{0} {1} m {0} {2} l S", ConvertToString(x + num3), ConvertToString(top), ConvertToString(num2));
				}
			}
			if (flag)
			{
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
		}
		double num4 = (double)stiLinePrimitive.Size * 0.72 * 0.9;
		if (num4 < 0.32)
		{
			num4 = 0.32;
		}
		pageStream.WriteLine("{0} w", ConvertToString(num4));
		if (stiHorizontalLinePrimitive != null)
		{
			RenderCap(stiHorizontalLinePrimitive.StartCap, x, top, StiAngle.Angle0, num4);
			RenderCap(stiHorizontalLinePrimitive.EndCap, num, num2, StiAngle.Angle180, num4);
		}
		if (stiVerticalLinePrimitive != null)
		{
			RenderCap(stiVerticalLinePrimitive.StartCap, x, top, StiAngle.Angle270, num4);
			RenderCap(stiVerticalLinePrimitive.EndCap, num, num2, StiAngle.Angle90, num4);
		}
	}

	private void RenderCap(StiCap cap, double x, double y, StiAngle angle, double lineSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected I4, but got Unknown
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Invalid comparison between Unknown and I4
		if ((int)cap.Style == 0)
		{
			return;
		}
		SetStrokeColor(cap.Color);
		if (cap.Fill)
		{
			SetNonStrokeColor(cap.Color);
		}
		double num = (double)((float)cap.Height / 2f) * 0.72;
		double num2 = (double)((float)cap.Width / 2f) * 0.72;
		StiCapStyle style = cap.Style;
		switch (style - 1)
		{
		case 0:
			if (angle == StiAngle.Angle0)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l h", ConvertToString(x - lineSize), ConvertToString(y), ConvertToString(x + num2 * 2.0), ConvertToString(y + num), ConvertToString(x + num2 * 2.0), ConvertToString(y - num));
			}
			if (angle == StiAngle.Angle90)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l h", ConvertToString(x), ConvertToString(y - lineSize), ConvertToString(x - num2), ConvertToString(y + num * 2.0), ConvertToString(x + num2), ConvertToString(y + num * 2.0));
			}
			if (angle == StiAngle.Angle180)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l h", ConvertToString(x + lineSize), ConvertToString(y), ConvertToString(x - num2 * 2.0), ConvertToString(y - num), ConvertToString(x - num2 * 2.0), ConvertToString(y + num));
			}
			if (angle == StiAngle.Angle270)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l h", ConvertToString(x), ConvertToString(y + lineSize), ConvertToString(x + num2), ConvertToString(y - num * 2.0), ConvertToString(x - num2), ConvertToString(y - num * 2.0));
			}
			break;
		case 1:
			if (angle == StiAngle.Angle0)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(x + num2 * 2.0), ConvertToString(y + num), ConvertToString(x - lineSize), ConvertToString(y), ConvertToString(x + num2 * 2.0), ConvertToString(y - num));
			}
			if (angle == StiAngle.Angle90)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(x - num2), ConvertToString(y + num * 2.0), ConvertToString(x), ConvertToString(y - lineSize), ConvertToString(x + num2), ConvertToString(y + num * 2.0));
			}
			if (angle == StiAngle.Angle180)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(x - num2 * 2.0), ConvertToString(y - num), ConvertToString(x + lineSize), ConvertToString(y), ConvertToString(x - num2 * 2.0), ConvertToString(y + num));
			}
			if (angle == StiAngle.Angle270)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l", ConvertToString(x + num2), ConvertToString(y - num * 2.0), ConvertToString(x), ConvertToString(y + lineSize), ConvertToString(x - num2), ConvertToString(y - num * 2.0));
			}
			break;
		case 2:
			if (angle == StiAngle.Angle0)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h", ConvertToString(x + num2 * 2.0), ConvertToString(y + num), ConvertToString(x - lineSize), ConvertToString(y), ConvertToString(x + num2 * 2.0), ConvertToString(y - num), ConvertToString(x + num2 * 1.3), ConvertToString(y));
			}
			if (angle == StiAngle.Angle90)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h", ConvertToString(x - num2), ConvertToString(y + num * 2.0), ConvertToString(x), ConvertToString(y - lineSize), ConvertToString(x + num2), ConvertToString(y + num * 2.0), ConvertToString(x), ConvertToString(y + num * 1.3));
			}
			if (angle == StiAngle.Angle180)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h", ConvertToString(x - num2 * 2.0), ConvertToString(y - num), ConvertToString(x + lineSize), ConvertToString(y), ConvertToString(x - num2 * 2.0), ConvertToString(y + num), ConvertToString(x - num2 * 1.3), ConvertToString(y));
			}
			if (angle == StiAngle.Angle270)
			{
				pageStream.Write("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h", ConvertToString(x + num2), ConvertToString(y - num * 2.0), ConvertToString(x), ConvertToString(y + lineSize), ConvertToString(x - num2), ConvertToString(y - num * 2.0), ConvertToString(x), ConvertToString(y - num * 1.3));
			}
			break;
		case 4:
			pageStream.Write("{0} {1} {2} {3} re", ConvertToString(x - num2), ConvertToString(y - num), ConvertToString(num2 * 2.0), ConvertToString(num * 2.0));
			break;
		case 3:
			pageStream.Write("{0} {1} m {2} {3} l {4} {5} l {6} {7} l h", ConvertToString(x), ConvertToString(y + num), ConvertToString(x + num2), ConvertToString(y), ConvertToString(x), ConvertToString(y - num), ConvertToString(x - num2), ConvertToString(y));
			break;
		case 5:
		{
			StiPdfGeomWriter stiPdfGeomWriter = new StiPdfGeomWriter(null, this);
			pageStream.Write(stiPdfGeomWriter.GetEllipseString(x - num2, y - num, num2 * 2.0, num * 2.0));
			break;
		}
		}
		if (cap.Fill && (int)cap.Style != 2)
		{
			pageStream.WriteLine(" B");
		}
		else
		{
			pageStream.WriteLine(" S");
		}
	}

	private void RenderCheckbox(StiPdfData pp, bool checkBoxValue, bool storeShading = true)
	{
		if (!(pp.Component is StiCheckBox stiCheckBox))
		{
			return;
		}
		string value = string.Empty;
		switch (checkBoxValue ? stiCheckBox.CheckStyleForTrue : stiCheckBox.CheckStyleForFalse)
		{
		case StiCheckStyle.Cross:
			value = "62.568 52.024 m 62.018 52.166 60.405 52.537 58.984 52.848 c 55.336 53.645 49.313 58.685 44.741 64.767 c 40.839 69.958 l 45.919 71.092 l \r\n53.272 72.735 59.559 76.81 67.746 85.239 c 74.954 92.661 l 68.543 100.174 l 56.77 113.972 54.196 123.193 59.915 131.088 c 62.809 135.083 71.734 143.458 73.097 143.458 c \r\n73.509 143.458 74.16 141.77 74.546 139.708 c 75.526 134.457 81.002 122.942 85.482 116.708 c 87.557 113.82 89.473 111.458 89.74 111.458 c \r\n90.006 111.458 93.515 114.945 97.537 119.208 c 113.934 136.584 127.211 138.972 135.818 126.095 c 139.973 119.877 140.004 118.024 135.958 117.739 c \r\n130.11 117.329 118.795 109.205 110.443 99.42 c 105.812 93.994 l 110.69 89.679 l 117.241 83.884 129.589 77.786 136.531 76.919 c \r\n139.576 76.539 142.068 75.813 142.068 75.307 c 142.068 72.526 132.802 60.889 129.038 58.942 c 121.077 54.825 112.668 58.23 96.273 72.209 c \r\n91.287 76.46 l 84.2 67.488 l 80.303 62.554 75.379 57.368 73.259 55.965 c 69.353 53.38 64.393 51.552 62.568 52.024 c h";
			break;
		case StiCheckStyle.Check:
			value = "60.972 37.503 m 51.173 63.277 43.562 76.623 35.37 82.397 c 30.912 85.54 l 33.664 88.435 l 37.539 92.513 43.698 95.935 48.566 96.713 c \r\n52.426 97.33 53.024 97.093 57.102 93.334 c 59.763 90.882 63.368 85.726 66.269 80.223 c 68.899 75.234 71.18 71.153 71.337 71.153 c \r\n71.493 71.153 73.65 74.19 76.13 77.903 c 96.259 108.044 129.683 141.214 157.565 158.718 c 166.414 164.274 l 168.677 161.643 l \r\n170.941 159.012 l 163.178 152.717 l 139.859 133.81 108.017 94.486 89.043 61.164 c 82.362 49.432 81.87 48.851 73.952 43.345 c \r\n69.45 40.214 64.908 37.04 63.858 36.292 c 62.149 35.074 61.848 35.2 60.972 37.503 c h";
			break;
		case StiCheckStyle.CrossRectangle:
			value = "24.153 97.958 m 24.153 170.458 l 98.653 170.458 l 173.153 170.458 l 173.153 97.958 l 173.153 25.458 l 98.653 25.458 l 24.153 25.458 l 24.153 97.958 l h \r\n157.911 97.708 m 157.653 154.958 l 98.653 154.958 l 39.653 154.958 l 39.393 98.958 l 39.25 68.158 39.348 42.395 39.611 41.708 c \r\n39.987 40.727 52.819 40.458 99.129 40.458 c 158.169 40.458 l 157.911 97.708 l h \r\n67.337 54.521 m 65.513 54.912 62.41 56.378 60.442 57.778 c 57.123 60.14 48.153 70.186 48.153 71.541 c 48.153 71.87 50.57 72.68 53.525 73.342 c \r\n60.71 74.95 67.272 79.277 75.328 87.718 c 82.003 94.713 l 75.624 102.027 l 64.931 114.288 61.644 123.705 65.472 131.108 c \r\n67.054 134.168 78.562 145.458 80.098 145.458 c 80.556 145.458 81.245 143.77 81.63 141.708 c 82.611 136.457 88.086 124.942 92.567 118.708 c \r\n94.642 115.82 96.558 113.458 96.824 113.458 c 97.091 113.458 100.6 116.945 104.622 121.208 c 121.019 138.584 134.296 140.972 142.903 128.095 c \r\n147.058 121.877 147.089 120.024 143.043 119.739 c 137.213 119.33 124.806 110.39 117.127 101.066 c 113.226 96.33 113.155 96.112 114.876 94.198 c \r\n118.066 90.648 128.579 83.654 133.847 81.578 c 136.682 80.461 141.285 79.244 144.077 78.873 c 146.868 78.503 149.153 77.878 149.153 77.484 c \r\n149.153 75.37 140.777 64.275 137.501 62.048 c 129.107 56.344 120.869 59.278 103.358 74.209 c 98.372 78.46 l 91.285 69.488 l \r\n81.563 57.18 74.76 52.928 67.337 54.521 c h";
			break;
		case StiCheckStyle.CheckRectangle:
			value = "19.915 96.5 m 19.915 169 l 91.857 169 l 163.8 169 l 170.357 173.111 l 176.914 177.223 l 178.882 174.861 l 179.963 173.563 180.864 172.217 180.882 171.872 c \r\n180.9 171.526 178.44 169.334 175.415 167 c 169.915 162.757 l 169.915 93.378 l 169.915 24 l 94.915 24 l 19.915 24 l 19.915 96.5 l h \r\n153.915 92.622 m 153.915 141.962 153.786 146.137 152.294 144.899 c 149.513 142.592 136.609 126.998 127.965 115.5 c 117.473 101.544 104.486 81.963 98.451 71 c \r\n93.993 62.903 93.316 62.192 84.16 56 c 78.873 52.425 74.256 49.375 73.9 49.223 c 73.544 49.07 71.988 52.22 70.441 56.223 c \r\n68.895 60.225 65.183 68.635 62.192 74.911 c 57.906 83.903 55.515 87.56 50.914 92.161 c 47.703 95.372 44.364 98 43.495 98 c \r\n40.697 98 41.79 99.66 47.479 104.049 c 53.073 108.365 60.662 111.14 64.28 110.194 c 67.84 109.263 73.689 102.039 78.2 93.002 c \r\n82.663 84.062 l 87.207 90.895 l 95.518 103.394 108.214 118.311 125.807 136.25 c 143.215 154 l 89.565 154 l 35.915 154 l 35.915 96.5 l \r\n35.915 39 l 94.915 39 l 153.915 39 l 153.915 92.622 l h";
			break;
		case StiCheckStyle.CrossCircle:
			value = "83.347 26.864 m 61.07 31.95 42.193 47.128 32.202 67.986 c 23.401 86.36 23.68 110.034 32.919 128.958 c 41.882 147.315 60.868 162.86 80.847 168.201 c \r\n91.083 170.936 112.112 170.628 121.812 167.6 c 147.999 159.425 167.881 138.673 173.432 113.721 c 175.869 102.768 175 85.662 171.452 74.743 c \r\n164.795 54.256 145.804 35.792 124.126 28.729 c 117.735 26.647 113.94 26.133 102.847 25.845 c 93.814 25.61 87.363 25.947 83.347 26.864 c h \r\n112.414 41.542 m 129.545 44.672 146.131 57.503 153.827 73.579 c 168.725 104.698 152.719 141.239 119.425 152.119 c 112.712 154.313 109.49 154.763 100.347 154.781 c \r\n90.993 154.8 88.185 154.404 81.579 152.131 c 64.423 146.231 51.91 134.6 45.14 118.265 c 42.988 113.072 42.446 109.911 42.069 100.368 c \r\n41.551 87.229 42.811 81.166 48.181 70.958 c 52.288 63.15 63.613 51.864 71.549 47.67 c 83.611 41.295 98.688 39.034 112.414 41.542 c h \r\n69.097 66.583 m 66.21 69.342 63.847 71.942 63.847 72.361 c 63.847 72.78 69.506 78.671 76.422 85.451 c 88.996 97.78 l 76.198 110.607 l 63.4 123.434 l \r\n68.336 128.446 l 71.051 131.202 73.641 133.458 74.091 133.458 c 74.542 133.458 80.666 127.846 87.7 120.988 c 100.49 108.517 l 104.919 113.071 l \r\n107.354 115.575 113.31 121.259 118.154 125.701 c 126.961 133.777 l 132.308 128.496 l 137.656 123.215 l 124.694 110.658 l 111.733 98.1 l 124.866 84.939 l \r\n137.999 71.779 l 132.815 67.118 l 129.964 64.555 127.11 62.458 126.474 62.458 c 125.837 62.458 119.93 67.858 113.347 74.458 c \r\n106.765 81.058 100.96 86.458 100.449 86.458 c 99.938 86.458 93.856 80.857 86.933 74.013 c 74.347 61.567 l 69.097 66.583 l h";
			break;
		case StiCheckStyle.DotCircle:
			value = "81.653 29.406 m 59.375 34.493 40.499 49.67 30.507 70.529 c 21.706 88.903 21.985 112.576 31.224 131.5 c 40.187 149.857 59.173 165.402 79.153 170.743 c \r\n89.388 173.479 110.417 173.17 120.117 170.142 c 146.304 161.968 166.186 141.215 171.737 116.263 c 174.174 105.311 173.305 88.205 169.757 77.285 c \r\n163.1 56.798 144.109 38.334 122.431 31.271 c 116.04 29.189 112.245 28.675 101.153 28.387 c 92.119 28.152 85.668 28.49 81.653 29.406 c h \r\n111.653 44.504 m 132.341 48.848 149.671 64.959 155.751 85.5 c 158.113 93.481 158.113 107.519 155.751 115.5 c 150.089 134.629 134.635 149.703 114.653 155.588 c \r\n106.553 157.973 90.741 157.974 82.695 155.589 c 62.46 149.592 46.687 133.961 41.605 114.869 c 39.656 107.547 39.74 91.753 41.764 84.932 c \r\n50.494 55.507 80.736 38.013 111.653 44.504 c h \r\n90.005 77.33 m 76.55 82.362 69.825 98.176 75.898 110.5 c 78.035 114.836 83.045 119.856 87.653 122.277 c 93.231 125.208 104.066 125.204 109.705 122.27 c \r\n127.735 112.887 128.781 89.485 111.62 79.428 c 106.047 76.162 95.789 75.166 90.005 77.33 c h";
			break;
		case StiCheckStyle.DotRectangle:
			value = "23.847 98.805 m 23.847 171.305 l 98.347 171.305 l 172.847 171.305 l 172.847 98.805 l 172.847 26.305 l 98.347 26.305 l 23.847 26.305 l 23.847 98.805 l h \r\n157.847 98.813 m 157.847 156.321 l 98.597 156.063 l 39.347 155.805 l 39.089 98.555 l 38.831 41.305 l 98.339 41.305 l 157.847 41.305 l 157.847 98.813 l h \r\n63.527 64.959 m 63.153 65.333 62.847 80.638 62.847 98.972 c 62.847 132.305 l 98.361 132.305 l 133.874 132.305 l 133.611 98.555 l 133.347 64.805 l \r\n98.777 64.542 l 79.763 64.398 63.901 64.585 63.527 64.959 c h";
			break;
		case StiCheckStyle.NoneCircle:
			value = "83.5 29.406 m 61.222 34.493 42.346 49.67 32.355 70.529 c 23.554 88.903 23.832 112.576 33.071 131.5 c 42.034 149.857 61.02 165.402 81 170.743 c \r\n91.235 173.479 112.265 173.17 121.965 170.142 c 148.151 161.968 168.034 141.215 173.585 116.263 c 176.022 105.311 175.152 88.205 171.605 77.285 c \r\n164.948 56.798 145.957 38.334 124.278 31.271 c 117.887 29.189 114.092 28.675 103 28.387 c 93.966 28.152 87.515 28.49 83.5 29.406 c h \r\n113.5 44.504 m 134.189 48.848 151.519 64.959 157.598 85.5 c 159.961 93.481 159.961 107.519 157.598 115.5 c 151.937 134.629 136.483 149.703 116.5 155.588 c \r\n108.401 157.973 92.589 157.974 84.543 155.589 c 64.308 149.592 48.534 133.961 43.453 114.869 c 41.504 107.547 41.588 91.753 43.612 84.932 c \r\n52.342 55.507 82.583 38.013 113.5 44.504 c h";
			break;
		case StiCheckStyle.NoneRectangle:
			value = "24.153 97.958 m 24.153 170.458 l 98.653 170.458 l 173.153 170.458 l 173.153 97.958 l 173.153 25.458 l 98.653 25.458 l 24.153 25.458 l 24.153 97.958 l h \r\n157.911 97.708 m 157.653 154.958 l 98.653 154.958 l 39.653 154.958 l 39.393 98.958 l 39.25 68.158 39.348 42.395 39.611 41.708 c \r\n39.987 40.727 52.819 40.458 99.129 40.458 c 158.169 40.458 l 157.911 97.708 l h";
			break;
		}
		pageStream.WriteLine("q");
		PushColorToStack();
		Color nonStrokeColor = StiBrush.ToColor(stiCheckBox.TextBrush);
		if (nonStrokeColor.A != 0)
		{
			SetNonStrokeColor(nonStrokeColor);
		}
		if (stiCheckBox.TextBrush is StiGradientBrush || stiCheckBox.TextBrush is StiGlareBrush)
		{
			if (storeShading)
			{
				StoreShadingData2(pp.X, pp.Y, pp.Width, pp.Height, stiCheckBox.TextBrush);
			}
			pageStream.WriteLine("/Pattern cs /P{0} scn", 1 + shadingCurrent);
		}
		if (stiCheckBox.TextBrush is StiHatchBrush)
		{
			StiBrush textBrush = stiCheckBox.TextBrush;
			StiHatchBrush brush = (StiHatchBrush)(object)((textBrush is StiHatchBrush) ? textBrush : null);
			pageStream.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber(brush) + 1);
		}
		SetStrokeColor(stiCheckBox.ContourColor);
		pageStream.WriteLine("{0} w", ConvertToString(stiCheckBox.Size));
		pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(pp.X + pp.Width / 2.0), ConvertToString(pp.Y + pp.Height / 2.0));
		double num = Math.Min(pp.Width, pp.Height);
		double value2 = num / 200.0;
		pageStream.WriteLine("{0} 0 0 {0} 0 0 cm", ConvertToString(value2));
		pageStream.WriteLine("1 0 0 1 -98 -98 cm");
		pageStream.WriteLine(value);
		pageStream.WriteLine("B");
		pageStream.WriteLine("Q");
		PopColorFromStack();
	}

	private bool? GetCheckBoxValue(StiCheckBox checkbox)
	{
		bool? result = null;
		if (checkbox != null && checkbox.CheckedValue != null)
		{
			result = false;
			string text = checkbox.CheckedValue.ToString().Trim().ToLower(CultureInfo.InvariantCulture);
			string[] array = checkbox.Values.Split('/', ';', ',');
			if (array != null && array.Length != 0)
			{
				string text2 = array[0].Trim().ToLower(CultureInfo.InvariantCulture);
				result = text == text2;
			}
		}
		return result;
	}

	internal void RenderText(StiPdfData pp)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Invalid comparison between Unknown and I4
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Invalid comparison between Unknown and I4
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Unknown result type (might be due to invalid IL or missing references)
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f6: Invalid comparison between Unknown and I4
		//IL_21aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ed: Invalid comparison between Unknown and I4
		//IL_2228: Unknown result type (might be due to invalid IL or missing references)
		//IL_222d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_232c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2332: Invalid comparison between Unknown and I4
		//IL_2256: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_234e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2353: Unknown result type (might be due to invalid IL or missing references)
		//IL_2355: Unknown result type (might be due to invalid IL or missing references)
		//IL_2358: Unknown result type (might be due to invalid IL or missing references)
		//IL_236e: Expected I4, but got Unknown
		//IL_2267: Unknown result type (might be due to invalid IL or missing references)
		//IL_226a: Invalid comparison between Unknown and I4
		//IL_21dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e0: Invalid comparison between Unknown and I4
		//IL_21d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2286: Unknown result type (might be due to invalid IL or missing references)
		//IL_2289: Invalid comparison between Unknown and I4
		//IL_21e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2254: Unknown result type (might be due to invalid IL or missing references)
		//IL_1036: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dd: Expected O, but got Unknown
		//IL_1064: Unknown result type (might be due to invalid IL or missing references)
		//IL_108f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1092: Invalid comparison between Unknown and I4
		//IL_10fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fe: Invalid comparison between Unknown and I4
		//IL_1062: Unknown result type (might be due to invalid IL or missing references)
		//IL_1524: Unknown result type (might be due to invalid IL or missing references)
		//IL_1529: Unknown result type (might be due to invalid IL or missing references)
		//IL_1562: Unknown result type (might be due to invalid IL or missing references)
		//IL_1599: Unknown result type (might be due to invalid IL or missing references)
		//IL_159c: Invalid comparison between Unknown and I4
		//IL_15b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b5: Invalid comparison between Unknown and I4
		//IL_1597: Unknown result type (might be due to invalid IL or missing references)
		//IL_157c: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c4: Invalid comparison between Unknown and I4
		//IL_1585: Unknown result type (might be due to invalid IL or missing references)
		//IL_1588: Invalid comparison between Unknown and I4
		//IL_1581: Unknown result type (might be due to invalid IL or missing references)
		//IL_158b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5f: Expected O, but got Unknown
		StiText stiText = pp.Component as StiText;
		bool flag = stiText.TextQuality == StiTextQuality.Wysiwyg;
		bool flag2 = (annotsCounter > 0 || annots2Counter > 0) && stiText.Editable;
		IStiTextOptions stiTextOptions = pp.Component as IStiTextOptions;
		bool flag3 = stiTextOptions != null && stiTextOptions.TextOptions.WordWrap;
		IStiTextHorAlignment stiTextHorAlignment = pp.Component as IStiTextHorAlignment;
		bool flag4 = stiTextHorAlignment != null && (int)stiTextHorAlignment.HorAlignment == 3;
		bool flag5 = StiOptions.Export.Pdf.WinFormsHighAccuracyMode == StiPdfHighAccuracyMode.All || (StiOptions.Export.Pdf.WinFormsHighAccuracyMode == StiPdfHighAccuracyMode.WordwrapOnly && flag3);
		bool flag6 = StiOptions.Export.Pdf.WpfHighAccuracyMode == StiPdfHighAccuracyMode.All || (StiOptions.Export.Pdf.WpfHighAccuracyMode == StiPdfHighAccuracyMode.WordwrapOnly && flag3);
		if (!flag2 && ((!isWpf && flag5 && !flag && !flag4) || (isWpf && flag6)))
		{
			RenderTextHQ(pp);
			return;
		}
		IStiTextBrush stiTextBrush = pp.Component as IStiTextBrush;
		IStiText stiText2 = pp.Component as IStiText;
		IStiVertAlignment stiVertAlignment = pp.Component as IStiVertAlignment;
		bool rightToLeft = stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
		string text = null;
		int num = 0;
		float num2 = 0f;
		if (flag4)
		{
			flag3 = true;
		}
		bool useUnicode = pdfFont.UseUnicode;
		if (stiText2 == null)
		{
			return;
		}
		StiMargins margins = stiText.Margins;
		double num3 = 0.72 * margins.Left;
		double num4 = 0.72 * margins.Right;
		double num5 = 0.72 * margins.Top;
		double num6 = 0.72 * margins.Bottom;
		double num7 = pp.X + num3;
		double num8 = pp.Y + num6;
		double num9 = pp.Width - num3 - num4;
		double num10 = pp.Height - num5 - num6;
		double num11 = 0.0;
		double num12 = 0.0;
		double num13 = 0.0;
		double num14 = 0.0;
		if ((int)stiText.Border.Style != 6)
		{
			float sizeOffset = stiText.Border.GetSizeOffset();
			if (stiText.Border.IsLeftBorderSidePresent)
			{
				num11 = sizeOffset;
			}
			if (stiText.Border.IsRightBorderSidePresent)
			{
				num12 = sizeOffset;
			}
			if (stiText.Border.IsTopBorderSidePresent)
			{
				num13 = sizeOffset;
			}
			if (stiText.Border.IsBottomBorderSidePresent)
			{
				num14 = sizeOffset;
			}
		}
		StiBorder border = stiText.Border;
		StiAdvancedBorder val = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
		if (val != (StiAdvancedBorder)null)
		{
			num11 = val.LeftSide.GetSizeOffset();
			num12 = val.RightSide.GetSizeOffset();
			num13 = val.TopSide.GetSizeOffset();
			num14 = val.BottomSide.GetSizeOffset();
		}
		double num15 = 0.9399999976158142;
		num7 += 0.72 * num15 * num11;
		num8 += 0.72 * num15 * num14;
		num9 -= 0.72 * num15 * (num11 + num12);
		num10 -= 0.72 * num15 * (num13 + num14);
		if (clipLongTextLines && !flag2)
		{
			pageStream.WriteLine("q");
			PushColorToStack();
			pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(num7), ConvertToString(num8), ConvertToString(num9), ConvertToString(num10));
		}
		Color color = Color.Transparent;
		if (stiTextBrush != null)
		{
			color = StiBrush.ToColor(stiTextBrush.TextBrush);
			SetNonStrokeColor(color);
			StoreShadingData2(pp.X, pp.Y, pp.Width, pp.Height, stiTextBrush.TextBrush);
			if (stiTextBrush.TextBrush is StiGradientBrush || stiTextBrush.TextBrush is StiGlareBrush)
			{
				pageStream.WriteLine("/Pattern cs /P{0} scn", 1 + shadingCurrent);
			}
			if (stiTextBrush.TextBrush is StiHatchBrush)
			{
				StiBrush textBrush = stiTextBrush.TextBrush;
				StiHatchBrush brush = (StiHatchBrush)(object)((textBrush is StiHatchBrush) ? textBrush : null);
				pageStream.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber(brush) + 1);
			}
			if (flag2)
			{
				StiEditableObject stiEditableObject = (StiEditableObject)annotsArray[annotsCurrent];
				stiEditableObject.FontColor = color;
			}
		}
		double num16 = 1.0;
		bool flag7 = false;
		IStiFont stiFont = pp.Component as IStiFont;
		if (stiFont != null)
		{
			num16 = stiFont.Font.SizeInPoints;
			flag7 = PdfFonts.IsFontSymbolic(stiFont.Font.Name);
		}
		double num17 = ((isWpf || StiDpiHelper.NeedFontScaling) ? StiDpiHelper.GraphicsScale : 1.0);
		double num18 = num16;
		if (StiOptions.Engine.TextLineSpacingScale != 1f)
		{
			if (StiDpiHelper.IsLinux && num16 >= 4.5 && num16 <= 7.5)
			{
				num18 = 5.5;
			}
			if (StiDpiHelper.IsMacOsX && num16 >= 5.5 && num16 <= 7.5)
			{
				num18 = 6.5;
			}
		}
		double num19 = num18 * fontCorrectValue * (double)pdfFont.ASC / 1000.0 * num17;
		double num20 = num18 * fontCorrectValue * (double)pdfFont.DESC / 1000.0 * num17;
		double num21 = num18 * fontCorrectValue * (double)pdfFont.tmASC / 1000.0 * num17;
		double num22 = num18 * fontCorrectValue * (double)pdfFont.tmDESC / 1000.0 * -1.0 * num17;
		double num23 = num18 * fontCorrectValue * (double)pdfFont.tmExternal / 1000.0 * num17;
		double num24 = num18 * fontCorrectValue * (double)pdfFont.LineGap / 1000.0 * num17;
		double num25 = num18 * fontCorrectValue * (double)pdfFont.UnderscoreSize / 1000.0 * num17;
		double num26 = num18 * fontCorrectValue * (double)pdfFont.UnderscorePosition / 1000.0 * num17;
		double num27 = num18 * fontCorrectValue * (double)pdfFont.StrikeoutSize / 1000.0 * num17;
		double num28 = num18 * fontCorrectValue * (double)pdfFont.StrikeoutPosition / 1000.0 * num17;
		double num29 = num21 - num22 + num23;
		double num30 = num29 * (double)stiText.LineSpacing;
		double num31 = num21;
		double num32 = num22;
		if ((pdfFont.fsSelection & 0x80u) != 0)
		{
			num30 = (num19 - num20 + num24) * (double)stiText.LineSpacing;
			num31 = num19;
			num32 = num20;
		}
		if (CompatibleMode160)
		{
			num31 = (num21 - num19) / 2.0;
			num32 = ((num20 > 0.0) ? num20 : (Math.Abs(num20) * 0.4));
			double num33 = (num21 - num22 - (num19 - num20)) / 2.0;
			double num34 = num33;
			num31 = num19 + num33;
			num32 = num20 - num33;
			num29 = num31 - num32;
			num30 = num29 * (double)stiText.LineSpacing;
		}
		bool flag8 = false;
		if (stiTextOptions != null)
		{
			num2 = stiTextOptions.TextOptions.Angle;
			flag8 = (num2 > 45f && num2 < 135f) || (num2 > 225f && num2 < 315f);
		}
		if (flag2)
		{
			num7 -= pp.X;
			num8 -= pp.Y;
		}
		float num35 = 0.85f;
		if (CompatibleMode160)
		{
			num7 += 1.0;
			num9 -= 2.0;
			num8 += 2.0;
			num10 -= 4.0;
		}
		double num36 = 0.0;
		if (stiText.TextQuality == StiTextQuality.Standard)
		{
			float num37 = (float)(0.35 - (14.0 - num16) * 0.04);
			num36 = 137.61;
			double num38 = num36 * (num16 * fontCorrectValue) / 1000.0;
			if (flag8)
			{
				num8 += (double)num37 + num38;
				num10 -= ((double)num37 + num38) * 2.0;
			}
			else
			{
				num7 += (double)num37 + num38;
				num9 -= ((double)num37 + num38) * 2.0;
			}
		}
		if (stiText.TextQuality == StiTextQuality.Typographic)
		{
			float num39 = (float)(0.05 + (8.0 - num16) * 0.009);
			if (flag8)
			{
				num8 += (double)num39;
				num10 -= (double)(num39 * 2f);
			}
			else
			{
				num7 += (double)num39;
				num9 -= (double)(num39 * 2f);
			}
		}
		StringBuilder stringBuilder = new StringBuilder(stiText2.Text);
		if (!StiDpiHelper.IsWindows)
		{
			if (stiText.CheckAllowHtmlTags())
			{
				stringBuilder = new StringBuilder(StiTextRenderer.GetPlainTextFromHtmlTags((string)stiText2.Text));
			}
			flag = false;
		}
		if (isWpf && flag3)
		{
			if (stiText.Page == null)
			{
				stiText.Page = report.RenderedPages[0];
			}
			RectangleD rect = stiText.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + 1.0;
			if (((RectangleD)(ref rect)).Width >= 2.0)
			{
				((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - 2.0;
			}
			rect = stiText.ConvertTextMargins(rect, convert: false);
			rect = stiText.ConvertTextBorders(rect, convert: false);
			num9 = ((RectangleD)(ref rect)).Width * 0.72;
		}
		if (flag2)
		{
			text = stringBuilder.ToString().Replace("\n", "");
		}
		stringBuilder.Replace("\r", "");
		stringBuilder.Replace("\u001f", "");
		if (num9 > 0.0 && stringBuilder.Length > 0)
		{
			double num40 = num7;
			double num41 = num8;
			double num42 = num9;
			double num43 = num10;
			if (num2 != 0f)
			{
				if (flag8)
				{
					double num44 = num9;
					num9 = num10;
					num10 = num44;
				}
				num7 = (0.0 - num9) / 2.0;
				num8 = (0.0 - num10) / 2.0;
			}
			List<string> list = null;
			List<LineInfo> list2 = null;
			if (flag)
			{
				CheckGraphicsForTextRenderer();
				List<string> list3 = new List<string>();
				list2 = new List<LineInfo>();
				string text2 = stringBuilder.ToString();
				RectangleD rect2 = stiText.Page.Unit.ConvertToHInches(pp.Component.ComponentToPage(pp.Component.ClientRectangle));
				rect2 = stiText.ConvertTextMargins(rect2, convert: false);
				rect2 = stiText.ConvertTextBorders(rect2, convert: false);
				StiTextRenderer.GetTextLinesAndWidths(graphicsForTextRenderer, ref text2, stiText.Font, rect2, 1.0, flag3, stiTextOptions.TextOptions.RightToLeft, 1.0, (double)stiTextOptions.TextOptions.Angle, stiTextOptions.TextOptions.Trimming, stiText.CheckAllowHtmlTags(), ref list3, ref list2, stiTextOptions.TextOptions, (double?)1.0);
				if (flag4)
				{
					for (int i = 0; i < list2.Count; i++)
					{
						if (list2[i].NeedWidthAlign)
						{
							list3[i] += "\a";
						}
					}
				}
				list = list3;
			}
			else
			{
				stringBuilder.Replace("\u00ad", "");
				if (flag4)
				{
					RectangleD val2 = stiText.Page.Unit.ConvertToHInches(pp.Component.ComponentToPage(pp.Component.ClientRectangle));
					val2 = stiText.ConvertTextMargins(val2, convert: false);
					val2 = stiText.ConvertTextBorders(val2, convert: false);
					((RectangleD)(ref val2)).Width = ((RectangleD)(ref val2)).Width / StiDpiHelper.DeviceCapsScale;
					((RectangleD)(ref val2)).Height = ((RectangleD)(ref val2)).Height / StiDpiHelper.DeviceCapsScale;
					CheckGraphicsForTextRenderer();
					list = StiTextDrawing.SplitTextWordwrapWidth(stringBuilder.ToString(), graphicsForTextRenderer, stiText.Font, val2);
					flag3 = false;
				}
				else
				{
					list = StiExportUtils.SplitString(stringBuilder.ToString(), removeControl: false);
				}
				if (flag3)
				{
					for (int j = 0; j < list.Count; j++)
					{
						string text3 = list[j];
						StringBuilder stringBuilder2 = new StringBuilder();
						int k = 0;
						while (k < text3.Length)
						{
							if (char.IsWhiteSpace(text3[k]))
							{
								stringBuilder2.Append(text3[k]);
								k++;
								continue;
							}
							StringBuilder stringBuilder3 = new StringBuilder();
							for (; k < text3.Length && !char.IsWhiteSpace(text3[k]); k++)
							{
								stringBuilder3.Append(text3[k]);
							}
							StringBuilder stringBuilder4 = StiBidirectionalConvert2.ConvertStringBuilder(stringBuilder3, rightToLeft: false);
							stringBuilder2.Append((object?)stringBuilder4);
							int num45 = stringBuilder3.Length - stringBuilder4.Length;
							if (num45 > 0)
							{
								stringBuilder2.Append('\0', num45);
							}
						}
						int[] array = new int[text3.Length];
						for (int l = 0; l < text3.Length; l++)
						{
							array[l] = pdfFont.UnicodeMap[(uint)stringBuilder2[l]];
						}
						int[] array2 = new int[text3.Length];
						int[] array3 = new int[text3.Length];
						int num46 = 0;
						float num47 = 0f;
						for (int m = 0; m < array.Length; m++)
						{
							if (array[m] >= 32)
							{
								num47 += (float)pdfFont.Widths[array[m] - 32];
							}
							if (array[m] == 9)
							{
								num47 += GetTabsSize(stiTextOptions, num16, (double)num47 + num36) + (float)num36;
							}
							array2[m] = (int)num47;
							if (IsWordWrapSymbol(stringBuilder2, m) && m > 0)
							{
								num46++;
							}
							array3[m] = num46;
						}
						double num48 = (double)num47 * (num16 * fontCorrectValue) / 1000.0;
						if (!(num48 > num9))
						{
							continue;
						}
						int n = text3.Length - 1;
						int num49 = (int)(num9 * 1000.0 / (num16 * fontCorrectValue));
						while (array2[n] > num49 && n > 0)
						{
							n--;
						}
						int num50 = n;
						if (array3[n] > 0)
						{
							if (array3[n] != array3[n + 1])
							{
								num50 = n++;
								for (; char.IsWhiteSpace(stringBuilder2[n]) && stringBuilder2[n] != '\t'; n++)
								{
								}
							}
							else
							{
								while (!IsWordWrapSymbol(stringBuilder2, n))
								{
									n--;
								}
								num50 = n - 1;
								while (char.IsWhiteSpace(stringBuilder2[num50]) && num50 > 0)
								{
									num50--;
								}
								for (; char.IsWhiteSpace(stringBuilder2[n]) && stringBuilder2[n] != '\t'; n++)
								{
								}
							}
						}
						else
						{
							n++;
						}
						if (flag4)
						{
							list[j] = text3.Substring(0, num50 + 1) + "\a";
						}
						else
						{
							list[j] = text3.Substring(0, num50 + 1);
						}
						list.Insert(j + 1, text3.Substring(n, text3.Length - n).TrimStart(new char[1] { ' ' }));
					}
				}
			}
			num = list.Count;
			int num51 = -1;
			if (stiTextOptions != null && (int)stiTextOptions.TextOptions.Trimming != 0)
			{
				double num52 = num10 / num30;
				num51 = (int)num52;
				if (num52 > (double)num51)
				{
					num51++;
				}
				if (num51 < 1)
				{
					num51 = 1;
				}
				if (num51 < num)
				{
					num = num51;
				}
			}
			for (int num53 = 0; num53 < list.Count; num53++)
			{
				string value = list[num53];
				StringBuilder stringBuilder5 = StiBidirectionalConvert2.ConvertStringBuilder(new StringBuilder(value), rightToLeft);
				for (int num54 = 0; num54 < stringBuilder5.Length; num54++)
				{
					stringBuilder5[num54] = (char)pdfFont.UnicodeMap[(uint)stringBuilder5[num54]];
				}
				list[num53] = stringBuilder5.ToString();
			}
			if (stiVertAlignment != null)
			{
				double num55 = (double)(num - 1) * num30 + num29;
				StiVertAlignment val3 = stiVertAlignment.VertAlignment;
				if (num2 != 0f && num2 != 90f && num2 != 180f && num2 != 270f)
				{
					val3 = (StiVertAlignment)1;
				}
				if ((int)val3 == 0)
				{
					num8 = ((!CompatibleMode160) ? (num8 + (num10 - num55 + (num29 - num31))) : (num8 + (num10 - (num55 - (0.0 - num32)))));
				}
				if ((int)val3 == 1)
				{
					num8 = (CompatibleMode160 ? (num8 + ((num10 - num55) / 2.0 + (0.0 - num32))) : ((stiText.TextQuality != StiTextQuality.Typographic) ? (num8 + ((num10 - num55) / 2.0 + (num29 - num31) * 1.2)) : (num8 + ((num10 - num55) / 2.0 - num32))));
				}
				if ((int)val3 == 2)
				{
					num8 = (CompatibleMode160 ? (num8 + (0.0 - num32)) : ((stiText.TextQuality != StiTextQuality.Typographic) ? (num8 + (num29 - num31) * 1.4) : (num8 + (0.0 - num32))));
				}
			}
			pageStream.WriteLine("BT");
			double num56 = 0.0;
			double num57 = 0.0;
			double[,] array4 = new double[list.Count, 3];
			Matrix val4 = null;
			if (num2 != 0f)
			{
				double num58 = (double)num2 * Math.PI / 180.0;
				val4 = new Matrix((float)Math.Cos(num58), (float)Math.Sin(num58), (float)(0.0 - Math.Sin(num58)), (float)Math.Cos(num58), (float)(num40 + num42 / 2.0), (float)(num41 + num43 / 2.0));
				if (!pdfFont.NeedSyntItalic || stiFont == null || !stiFont.Font.Italic)
				{
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} Tm", ConvertToString(val4.Elements[0]), ConvertToString(val4.Elements[1]), ConvertToString(val4.Elements[2]), ConvertToString(val4.Elements[3]), ConvertToString(val4.Elements[4]), ConvertToString(val4.Elements[5]));
				}
			}
			if (pdfFont.NeedSyntBold && stiFont != null && stiFont.Font.Bold)
			{
				pageStream.WriteLine("{0} w 2 Tr", ConvertToString(0.03099999949336052 * num16, 3));
				SetStrokeColor(color);
			}
			for (int num59 = 0; num59 < num && (stiTextOptions == null || !stiTextOptions.TextOptions.LineLimit || !(num30 * (double)(num59 + 1) > num10)); num59++)
			{
				string text4 = list[num59];
				float[] array5 = new float[text4.Length];
				int num60 = 0;
				double num61 = 0.0;
				double num62 = 0.0;
				double num63 = num9 * 1000.0 / (num16 * fontCorrectValue);
				int num64 = pdfFont.Widths[pdfFont.UnicodeMap[8230]];
				int length = 0;
				for (int num65 = 0; num65 < text4.Length; num65++)
				{
					int num66 = text4[num65];
					if (num66 >= 32)
					{
						num61 += (double)pdfFont.Widths[num66 - 32];
					}
					if (num66 == 9)
					{
						float num67 = GetTabsSize(stiTextOptions, num16, num61 + num36) + (float)num36;
						num61 += (double)num67;
						array5[num60++] = num67;
					}
					if (num61 + (double)num64 < num63)
					{
						num62 = num61;
						length = num65;
					}
				}
				num61 = num61 * (num16 * fontCorrectValue) / 1000.0;
				if (num51 != -1 && ((num59 == num - 1 && num51 < list.Count) || num61 > num9))
				{
					string text5 = text4.Substring(0, length);
					char c = (char)pdfFont.UnicodeMap[8230];
					text4 = text5 + c;
					num61 = (num62 + (double)num64) * (num16 * fontCorrectValue) / 1000.0;
				}
				if (useUnicode)
				{
					StringBuilder stringBuilder6 = new StringBuilder();
					for (int num68 = 0; num68 < text4.Length; num68++)
					{
						if (num68 % 121 == 120)
						{
							stringBuilder6.Append('\r');
						}
						int num69 = text4[num68];
						if (num69 == 32 || num69 == 9 || num69 == 7)
						{
							stringBuilder6.Append((char)num69);
							continue;
						}
						int num70 = pdfFont.GlyphList[num69];
						if (num70 == 65535)
						{
							num70 = 0;
						}
						stringBuilder6.Append(num70.ToString("X4"));
					}
					text4 = stringBuilder6.ToString();
				}
				else
				{
					text4 = ConvertToEscapeSequence(text4);
				}
				double num71 = num7;
				double num72 = num8 + num30 * (double)(num - num59 - 1);
				bool flag9 = false;
				bool flag10 = false;
				if (stiTextHorAlignment != null)
				{
					StiTextHorAlignment val5 = stiTextHorAlignment.HorAlignment;
					if (flag4)
					{
						if (text4.Length > 0 && text4[text4.Length - 1] == '\a')
						{
							text4 = text4.Substring(0, text4.Length - 1);
						}
						else
						{
							val5 = (StiTextHorAlignment)0;
						}
					}
					if (stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft)
					{
						if ((int)val5 == 0)
						{
							val5 = (StiTextHorAlignment)2;
						}
						else if ((int)val5 == 2)
						{
							val5 = (StiTextHorAlignment)0;
						}
					}
					if (flag && num61 > num9)
					{
						val5 = (StiTextHorAlignment)3;
					}
					if ((int)val5 == 1)
					{
						num71 += (num9 - num61) / 2.0;
					}
					if ((int)val5 == 2)
					{
						num71 += num9 - num61;
					}
					if ((int)val5 == 3)
					{
						int num73 = 0;
						for (int num74 = 0; num74 < text4.Length; num74++)
						{
							if (text4[num74] == ' ')
							{
								num73++;
							}
						}
						if (num73 > 0 && (num59 != list.Count - 1 || (flag && num61 > num9) || flag4))
						{
							flag10 = true;
							double num75 = (num9 - num61) / (double)num73 * 1000.0 / (num16 * fontCorrectValue);
							if (num75 > 0.0 || flag)
							{
								flag9 = true;
								num75 += (double)pdfFont.Widths[0];
								if (num75 < (double)pdfFont.Widths[0] * 0.5)
								{
									num75 = (double)pdfFont.Widths[0] * 0.5;
								}
								StringBuilder stringBuilder7 = new StringBuilder(useUnicode ? "<" : "(");
								for (int num76 = 0; num76 < text4.Length; num76++)
								{
									if (text4[num76] == ' ')
									{
										if (useUnicode)
										{
											stringBuilder7.Append(">-" + ConvertToString(num75) + "<");
										}
										else
										{
											stringBuilder7.Append(")-" + ConvertToString(num75) + "(");
										}
									}
									else
									{
										stringBuilder7.Append(text4[num76]);
									}
								}
								stringBuilder7.Append(useUnicode ? ">" : ")");
								text4 = stringBuilder7.ToString();
							}
						}
					}
				}
				if (num60 > 0)
				{
					int num77 = 0;
					StringBuilder stringBuilder8 = new StringBuilder();
					if (!flag10)
					{
						stringBuilder8.Append(useUnicode ? "<" : "(");
					}
					for (int num78 = 0; num78 < text4.Length; num78++)
					{
						if (text4[num78] == '\t')
						{
							double value2 = array5[num77++];
							if (useUnicode)
							{
								stringBuilder8.Append(">-" + ConvertToString(value2) + "<");
							}
							else
							{
								stringBuilder8.Append(")-" + ConvertToString(value2) + "(");
							}
						}
						else
						{
							stringBuilder8.Append(text4[num78]);
						}
					}
					if (!flag10)
					{
						stringBuilder8.Append(useUnicode ? ">" : ")");
					}
					text4 = stringBuilder8.ToString();
				}
				if (useUnicode)
				{
					text4 = text4.Replace(" ", pdfFont.GlyphList[32].ToString("X4")).Replace("\a", "0000");
				}
				bool flag11 = pdfFont.ChildFontsMap != null;
				byte b = 0;
				byte[] array6 = new byte[text4.Length];
				ArrayList arrayList = new ArrayList();
				for (int num79 = 0; num79 < text4.Length; num79++)
				{
					char c2 = text4[num79];
					array6[num79] = (byte)c2;
					if (flag7 && !useUnicode)
					{
						uint num80 = pdfFont.UnicodeMapBack[(uint)c2];
						if (num80 >= 61440 && num80 <= 61695)
						{
							array6[num79] = (byte)(num80 - 61440);
						}
					}
					bool flag12 = false;
					if (c2 == '\r')
					{
						arrayList.Add(num79);
						flag12 = true;
					}
					if (!flag11)
					{
						continue;
					}
					uint num81 = pdfFont.UnicodeMapBack[(uint)c2];
					if (c2 == '\\')
					{
						num81 = pdfFont.UnicodeMapBack[(uint)text4[num79 + 1]];
					}
					byte b2 = pdfFont.ChildFontsMap[num81];
					if (num81 < 64)
					{
						if (num79 > 0)
						{
							b2 = b;
						}
						else if (text4.Length > 1)
						{
							b2 = pdfFont.ChildFontsMap[pdfFont.UnicodeMapBack[(uint)text4[1]]];
						}
					}
					if (c2 != '\\')
					{
						array6[num79] = (byte)pdfFont.fonts[b2].UnicodeMap[num81];
					}
					if (b2 != b && !flag12 && num79 != 0)
					{
						arrayList.Add(num79);
					}
					b = b2;
				}
				arrayList.Add(text4.Length);
				array4[num59, 0] = num71;
				array4[num59, 1] = num72;
				array4[num59, 2] = num61;
				if (pdfFont.NeedSyntItalic && stiFont != null && stiFont.Font.Italic)
				{
					Matrix val6 = new Matrix(1f, 0f, 0.325f, 1f, (float)num71, (float)num72);
					if (val4 != null)
					{
						val6.Multiply(val4, (MatrixOrder)1);
					}
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} Tm", ConvertToString(val6.Elements[0]), ConvertToString(val6.Elements[1]), ConvertToString(val6.Elements[2]), ConvertToString(val6.Elements[3]), ConvertToString(val6.Elements[4]), ConvertToString(val6.Elements[5]));
				}
				else
				{
					pageStream.WriteLine("{0} {1} Td", ConvertToString(num71 - num56), ConvertToString(num72 - num57));
					num56 = num71;
					num57 = num72;
				}
				int num82 = 0;
				for (int num83 = 0; num83 < arrayList.Count; num83++)
				{
					if (flag11 && text4.Length > 0)
					{
						int index = ((text4[num82] < '@' && text4.Length > num82 + 1) ? (num82 + 1) : num82);
						byte b3 = pdfFont.ChildFontsMap[pdfFont.UnicodeMapBack[(uint)text4[index]]];
						pageStream.WriteLine("/F{0} {1} Tf", b3, ConvertToString(num16 * fontCorrectValue, precision_digits_font));
					}
					int num84 = (int)arrayList[num83];
					int num85 = num84 - num82;
					if (num85 > 0)
					{
						if (flag9 || num60 > 0)
						{
							pageStream.Write("[");
							if (num83 > 0)
							{
								pageStream.Write(useUnicode ? "<" : "(");
							}
							pageStream.Flush();
							memoryPageStream.Write(array6, num82, num85);
							if (num83 < arrayList.Count - 1)
							{
								pageStream.Write(useUnicode ? ">" : ")");
							}
							pageStream.WriteLine("] TJ");
						}
						else
						{
							pageStream.Write(useUnicode ? "<" : "(");
							pageStream.Flush();
							memoryPageStream.Write(array6, num82, num85);
							pageStream.WriteLine("{0} Tj", useUnicode ? ">" : ")");
						}
					}
					num82 = num84;
					if (num84 < array6.Length - 1 && array6[num84] == 13)
					{
						num82++;
					}
				}
			}
			if (pdfFont.NeedSyntBold && stiFont != null && stiFont.Font.Bold)
			{
				pageStream.WriteLine("0 Tr");
			}
			pageStream.WriteLine("ET");
			if (stiFont != null && stiFont.Font.Underline)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				double num86 = num16 * 0.09;
				double num87 = (0.0 - num16) * 0.115;
				if (pdfFont.UnderscoreSize != 0)
				{
					num86 = num25;
					num87 = num26;
				}
				if (num86 < 0.10000000149011612)
				{
					num86 = 0.10000000149011612;
				}
				pageStream.WriteLine("{0} w", ConvertToString(num86));
				if (num2 != 0f)
				{
					double num88 = (double)num2 * Math.PI / 180.0;
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num88)), ConvertToString(Math.Sin(num88)), ConvertToString(0.0 - Math.Sin(num88)), ConvertToString(Math.Cos(num88)), ConvertToString(num40 + num42 / 2.0), ConvertToString(num41 + num43 / 2.0));
				}
				if (stiTextBrush != null)
				{
					SetStrokeColor(color);
				}
				for (int num89 = 0; num89 < list.Count; num89++)
				{
					if (array4[num89, 2] != 0.0)
					{
						pageStream.WriteLine("{0} {1} m", ConvertToString(array4[num89, 0]), ConvertToString(array4[num89, 1] + num87));
						pageStream.WriteLine("{0} {1} l S", ConvertToString(array4[num89, 0] + array4[num89, 2]), ConvertToString(array4[num89, 1] + num87));
					}
				}
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
			if (stiFont != null && stiFont.Font.Strikeout)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				double num90 = num16 * 0.09;
				double num91 = num16 * 0.4;
				if (pdfFont.StrikeoutSize != 0)
				{
					num90 = num27;
					num91 = num28;
				}
				if (num90 < 0.10000000149011612)
				{
					num90 = 0.10000000149011612;
				}
				pageStream.WriteLine("{0} w", ConvertToString(num90));
				if (num2 != 0f)
				{
					double num92 = (double)num2 * Math.PI / 180.0;
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num92)), ConvertToString(Math.Sin(num92)), ConvertToString(0.0 - Math.Sin(num92)), ConvertToString(Math.Cos(num92)), ConvertToString(num40 + num42 / 2.0), ConvertToString(num41 + num43 / 2.0));
				}
				if (stiTextBrush != null)
				{
					SetStrokeColor(color);
				}
				for (int num93 = 0; num93 < list.Count; num93++)
				{
					if (array4[num93, 2] != 0.0)
					{
						pageStream.WriteLine("{0} {1} m", ConvertToString(array4[num93, 0]), ConvertToString(array4[num93, 1] + num91));
						pageStream.WriteLine("{0} {1} l S", ConvertToString(array4[num93, 0] + array4[num93, 2]), ConvertToString(array4[num93, 1] + num91));
					}
				}
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
		}
		if (flag2)
		{
			StiEditableObject stiEditableObject2 = (StiEditableObject)annotsArray[annotsCurrent];
			stiEditableObject2.Multiline = flag3;
			stiEditableObject2.X = pp.X;
			stiEditableObject2.Y = pp.Y;
			stiEditableObject2.Width = pp.Width;
			stiEditableObject2.Height = pp.Height;
			stiEditableObject2.Text = text;
			stiEditableObject2.Component = pp.Component;
			stiEditableObject2.Alignment = (StiTextHorAlignment)0;
			if (stiTextHorAlignment != null)
			{
				StiTextHorAlignment val7 = stiTextHorAlignment.HorAlignment;
				if (stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft)
				{
					if ((int)val7 == 0)
					{
						val7 = (StiTextHorAlignment)2;
					}
					else if ((int)val7 == 2)
					{
						val7 = (StiTextHorAlignment)0;
					}
				}
				stiEditableObject2.Alignment = val7;
			}
		}
		if ((int)stiText2.LinesOfUnderline != 6)
		{
			double num94 = pp.Y + num6 + (double)num35;
			bool flag13 = true;
			bool flag14 = true;
			if (stiVertAlignment != null)
			{
				if (num == 0)
				{
					num = 1;
				}
				double num95 = num30 * (double)num;
				StiVertAlignment val8 = stiVertAlignment.VertAlignment;
				if (num2 != 0f && num2 != 90f && num2 != 180f && num2 != 270f)
				{
					val8 = (StiVertAlignment)1;
				}
				if ((int)val8 == 0)
				{
					num94 += num10 - num31;
					flag13 = false;
				}
				if ((int)val8 == 1)
				{
					num94 += (num10 - num95) / 2.0 + (num30 - num31);
				}
				if ((int)val8 == 2)
				{
					num94 += num30 - num31;
					flag14 = false;
				}
			}
			num94 += num32;
			bool flag15 = false;
			double num96 = 0.0;
			if (pp.Component is IStiBorder stiBorder)
			{
				Color color2 = stiBorder.Border.Color;
				SetStrokeColor(color2);
				double num97 = stiBorder.Border.Size;
				if ((int)stiText2.LinesOfUnderline == 5)
				{
					num97 = 1.0;
				}
				double num98 = num97 * 0.72 * 0.9;
				pageStream.WriteLine("{0} w", ConvertToString(num98));
				flag15 = (int)stiText2.LinesOfUnderline == 5;
				if (flag15)
				{
					num96 = num98;
				}
				double num99 = num98 * 0.04;
				StiPenStyle linesOfUnderline = stiText2.LinesOfUnderline;
				switch (linesOfUnderline - 1)
				{
				case 3:
					pageStream.WriteLine("[{0} {1}] 0 d", ConvertToString(num99), ConvertToString(num99 * 55.0));
					break;
				case 0:
					pageStream.WriteLine("[{0} {1}] 0 d", ConvertToString(num99 * 50.0), ConvertToString(num99 * 55.0));
					break;
				case 1:
					pageStream.WriteLine("[{0} {1} {2} {1}] 0 d", ConvertToString(num99 * 50.0), ConvertToString(num99 * 55.0), ConvertToString(num99));
					break;
				case 2:
					pageStream.WriteLine("[{0} {1} {2} {1} {2} {1}] 0 d", ConvertToString(num99 * 50.0), ConvertToString(num99 * 55.0), ConvertToString(num99));
					break;
				}
			}
			double num100 = num94;
			if (flag13)
			{
				for (; num100 + num30 < pp.Y + pp.Height - num5; num100 += num30)
				{
				}
			}
			while (num100 > pp.Y + num6 + (flag14 ? 0.0 : num30))
			{
				pageStream.WriteLine("{0} {2} m {1} {2} l S", ConvertToString(pp.X), ConvertToString(pp.X + pp.Width), ConvertToString(num100 + num96));
				if (flag15)
				{
					pageStream.WriteLine("{0} {2} m {1} {2} l S", ConvertToString(pp.X), ConvertToString(pp.X + pp.Width), ConvertToString(num100 - num96));
				}
				num100 -= num30;
			}
		}
		if (clipLongTextLines && !flag2)
		{
			pageStream.WriteLine("Q");
			PopColorFromStack();
		}
	}

	internal void RenderText2(StiPdfData pp, int pageNumber = -1)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0997: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bba: Expected O, but got Unknown
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Expected O, but got Unknown
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1130: Unknown result type (might be due to invalid IL or missing references)
		//IL_1141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Unknown result type (might be due to invalid IL or missing references)
		IStiText stiText = pp.Component as IStiText;
		IStiTextOptions stiTextOptions = pp.Component as IStiTextOptions;
		StiText stiText2 = pp.Component as StiText;
		if (stiText == null || pp.Width <= 0.0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder(stiText2.Text);
		if (stringBuilder.Length == 0)
		{
			return;
		}
		bool flag = pp.Component is IStiTextHorAlignment stiTextHorAlignment && (int)stiTextHorAlignment.HorAlignment == 3;
		bool flag2 = (stiTextOptions != null && stiTextOptions.TextOptions.WordWrap) || flag;
		bool flag3 = (annotsCounter > 0 || annots2Counter > 0) && stiText2.Editable;
		StiMargins margins = stiText2.Margins;
		double num = 0.0;
		double num2 = num * margins.Left;
		double num3 = num * margins.Right;
		double num4 = num * margins.Top;
		double num5 = num * margins.Bottom;
		double num6 = pp.X + num2;
		double num7 = pp.Y + num5;
		double num8 = pp.Width - num2 - num3;
		double num9 = pp.Height - num4 - num5;
		if (clipLongTextLines && !flag3)
		{
			pageStream.WriteLine("q");
			PushColorToStack();
			pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(num6), ConvertToString(num7), ConvertToString(num8), ConvertToString(num9));
		}
		if (flag3)
		{
			num6 = 0.0;
			num7 = 0.0;
		}
		double num10 = num6;
		double num11 = num7;
		double num12 = num8;
		double num13 = num9;
		float num14 = 0f;
		if (stiTextOptions != null)
		{
			num14 = stiTextOptions.TextOptions.Angle;
			if (num14 != 0f)
			{
				if ((num14 > 45f && num14 < 135f) || (num14 > 225f && num14 < 315f))
				{
					double num15 = num8;
					num8 = num9;
					num9 = num15;
				}
				num6 = (0.0 - num8) / 2.0;
				num7 = (0.0 - num9) / 2.0;
			}
		}
		CheckGraphicsForTextRenderer();
		List<RunInfo> outRunsList = new List<RunInfo>();
		List<StiFontState> outFontsList = new List<StiFontState>();
		string text = stringBuilder.ToString();
		Color color = StiBrush.ToColor(stiText2.Brush);
		bool flag4 = false;
		RectangleD rect = stiText2.Page.Unit.ConvertToHInches(pp.Component.ComponentToPage(pp.Component.ClientRectangle));
		RectangleD val = stiText2.ConvertTextMargins(rect, convert: false);
		val = stiText2.ConvertTextBorders(val, convert: false);
		bool flag5 = StiOptions.Engine.UseNewHtmlEngine && stiText2.AllowHtmlTags;
		if (flag5)
		{
			StiHtmlTextRender.DrawTextForOutput(stiText2, out outRunsList, out outFontsList);
		}
		else
		{
			StiTextRenderer.DrawTextForOutput(graphicsForTextRenderer, text, stiText2.Font, val, StiBrush.ToColor(stiText2.TextBrush), color, (double)(stiText2.LineSpacing * StiOptions.Engine.TextLineSpacingScale), stiText2.HorAlignment, stiText2.VertAlignment, flag2, stiTextOptions.TextOptions.RightToLeft, 1.0, (double)stiTextOptions.TextOptions.Angle, stiTextOptions.TextOptions.Trimming, stiTextOptions.TextOptions.LineLimit, stiText2.CheckAllowHtmlTags(), outRunsList, outFontsList, stiTextOptions.TextOptions, (double?)1.0);
		}
		foreach (RunInfo item in outRunsList)
		{
			double num16 = num6 + 0.72 * (((RectangleD)(ref val)).Left + item.XPos - ((RectangleD)(ref rect)).Left);
			double num17 = num7 + 0.72 * (((RectangleD)(ref rect)).Bottom - (((RectangleD)(ref val)).Top + item.YPos));
			if ((num14 > 45f && num14 < 135f) || (num14 > 225f && num14 < 315f))
			{
				num16 = num6 + 0.72 * (((RectangleD)(ref rect)).Bottom - ((RectangleD)(ref val)).Bottom + item.XPos);
				num17 = num7 + 0.72 * (((RectangleD)(ref rect)).Right - (((RectangleD)(ref val)).Left + item.YPos));
			}
			pageStream.WriteLine("q");
			PushColorToStack();
			Font fontBase = outFontsList[item.FontIndex].FontBase;
			int fontNumber = pdfFont.GetFontNumber(fontBase);
			double num18 = (double)fontBase.SizeInPoints * fontCorrectValue;
			if (StiDpiHelper.NeedGraphicsScale && StiDpiHelper.NeedFontScaling && !flag5)
			{
				num18 /= StiDpiHelper.GraphicsScale;
			}
			pageStream.WriteLine("/F{0} {1} Tf", fontNumber, ConvertToString(num18, precision_digits_font));
			pdfFont.CurrentFont = fontNumber;
			bool useUnicode = pdfFont.UseUnicode;
			double num19 = fontBase.SizeInPoints;
			if (StiDpiHelper.NeedGraphicsScale && StiDpiHelper.NeedFontScaling && !flag5)
			{
				num19 /= StiDpiHelper.GraphicsScale;
			}
			double num20 = (StiDpiHelper.NeedFontScaling ? StiDpiHelper.GraphicsScale : 1.0);
			double num21 = num19 * fontCorrectValue * (double)pdfFont.ASC / 1000.0 * num20;
			double num22 = num19 * fontCorrectValue * (double)pdfFont.DESC / 1000.0 * num20;
			double num23 = num19 * fontCorrectValue * (double)pdfFont.LineGap / 1000.0 * num20;
			double num24 = num19 * fontCorrectValue * (double)pdfFont.tmASC / 1000.0 * num20;
			double num25 = num19 * fontCorrectValue * (double)pdfFont.tmDESC / 1000.0 * -1.0 * num20;
			double num26 = num19 * fontCorrectValue * (double)pdfFont.tmExternal / 1000.0 * num20;
			double num27 = num19 * fontCorrectValue * (double)pdfFont.UnderscoreSize / 1000.0 * num20;
			double num28 = num19 * fontCorrectValue * (double)pdfFont.UnderscorePosition / 1000.0 * num20;
			double num29 = num19 * fontCorrectValue * (double)pdfFont.StrikeoutSize / 1000.0 * num20;
			double num30 = num19 * fontCorrectValue * (double)pdfFont.StrikeoutPosition / 1000.0 * num20;
			double num31 = num24 - num25 + num26;
			double num32 = num24;
			double num33 = num25;
			if ((pdfFont.fsSelection & 0x80u) != 0)
			{
				num31 = (num21 - num22 + num23) * (double)stiText2.LineSpacing;
				num32 = num21;
				num33 = num22;
			}
			bool flag6 = true;
			if (flag5 && useUnicode && item.GlyphIndexList[0] == -1)
			{
				flag6 = false;
				for (int i = 0; i < item.Text.Length; i++)
				{
					item.GlyphIndexList[i] = pdfFont.GlyphList[pdfFont.UnicodeMap[(uint)item.Text[i]]];
				}
			}
			num17 -= num32;
			int num34 = 0;
			int num35 = 0;
			for (int j = 0; j < item.GlyphIndexList.Length; j++)
			{
				int num36 = 0;
				if (j < item.Text.Length)
				{
					num36 = item.Text[j];
				}
				if (useUnicode && flag6)
				{
					num36 = item.GlyphIndexList[j];
					num36 = pdfFont.GlyphBackList[num36];
				}
				int num37 = pdfFont.UnicodeMap[num36];
				int num38 = 1000;
				if (num37 >= 32)
				{
					num38 = pdfFont.Widths[num37 - 32];
				}
				if (num36 == 0 && pdfFont.GlyphWidths != null)
				{
					num38 = pdfFont.GlyphWidths[item.GlyphIndexList[j]];
				}
				num38 = (int)((double)num38 * item.ScaleList[j]);
				if (useUnicode)
				{
					if (item.GlyphWidths[j] > 0)
					{
						num34 += item.GlyphWidths[j];
						num35 += num38;
					}
				}
				else
				{
					num34 += item.Widths[j];
					num35 += num38;
				}
			}
			double num39 = (double)num34 * 0.72;
			double num40 = num39 / (num19 * fontCorrectValue / 1000.0) / (double)num35;
			if (!flag4)
			{
				Color backColor = item.BackColor;
				if (!backColor.Equals((object?)color))
				{
					flag4 = true;
				}
			}
			if (flag4)
			{
				Color backColor = item.BackColor;
				if (backColor.A != 0)
				{
					SetNonStrokeColor(item.BackColor);
					pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(num16), ConvertToString(num17 - (0.0 - num25)), ConvertToString(num39), ConvertToString(num31));
				}
			}
			SetNonStrokeColor(item.TextColor);
			bool flag7 = false;
			StringBuilder stringBuilder2 = new StringBuilder();
			num34 = 0;
			num35 = 0;
			int num41 = 0;
			stringBuilder2.Append(useUnicode ? "<" : "(");
			for (int k = 0; k < item.GlyphIndexList.Length; k++)
			{
				int num42 = 0;
				if (k < item.Text.Length)
				{
					num42 = item.Text[k];
				}
				if (useUnicode && flag6)
				{
					num42 = item.GlyphIndexList[k];
					num42 = pdfFont.GlyphBackList[num42];
				}
				int num43 = pdfFont.UnicodeMap[num42];
				int num44 = 1000;
				if (num43 >= 32)
				{
					num44 = pdfFont.Widths[num43 - 32];
				}
				double num45 = 0.0;
				if (flag7)
				{
					num34 += item.GlyphWidths[k];
					num35 += num44;
					num45 = 0.0 - ((double)num34 * 0.72 / (num19 * fontCorrectValue / 1000.0) - (double)num35);
					num35 += -(int)num45;
				}
				else if ((useUnicode ? item.GlyphWidths[k] : item.Widths[k]) > 0)
				{
					num35 += (int)((double)num44 * item.ScaleList[k]);
					num41 += num44;
					num45 = 0.0 - ((double)num35 * num40 - (double)num41);
					num41 += -(int)num45;
				}
				else
				{
					num45 = num44;
				}
				if (useUnicode)
				{
					int num46 = item.GlyphIndexList[k];
					if (num46 == 65535)
					{
						num46 = 0;
					}
					stringBuilder2.Append(num46.ToString("X4"));
					if (k < item.GlyphIndexList.Length - 1)
					{
						stringBuilder2.Append(">" + ConvertToString(num45) + "<");
					}
				}
				else
				{
					stringBuilder2.Append(ConvertToEscapeSequencePlusTabs(((char)num43).ToString()));
					if (k < item.GlyphIndexList.Length - 1)
					{
						stringBuilder2.Append(")" + ConvertToString(num45) + "(");
					}
				}
			}
			stringBuilder2.Append(useUnicode ? ">" : ")");
			byte[] array = new byte[stringBuilder2.Length];
			for (int l = 0; l < stringBuilder2.Length; l++)
			{
				array[l] = (byte)stringBuilder2[l];
			}
			pageStream.WriteLine("BT");
			Matrix val2 = null;
			if (num14 != 0f)
			{
				double num47 = (double)num14 * Math.PI / 180.0;
				val2 = new Matrix((float)Math.Cos(num47), (float)Math.Sin(num47), (float)(0.0 - Math.Sin(num47)), (float)Math.Cos(num47), (float)(num10 + num12 / 2.0), (float)(num11 + num13 / 2.0));
				Matrix val3 = new Matrix(1f, 0f, 0f, 1f, (float)num16, (float)num17);
				val2.Multiply(val3, (MatrixOrder)0);
			}
			if (pdfFont.NeedSyntItalic && fontBase.Italic)
			{
				if (val2 != null)
				{
					Matrix val4 = new Matrix(1f, 0f, 0.325f, 1f, 0f, 0f);
					val2.Multiply(val4, (MatrixOrder)0);
				}
				else
				{
					Matrix val5 = new Matrix(1f, 0f, 0.325f, 1f, (float)num16, (float)num17);
					val2 = val5;
				}
			}
			if (val2 != null)
			{
				pageStream.WriteLine("{0} {1} {2} {3} {4} {5} Tm", ConvertToString(val2.Elements[0], 7), ConvertToString(val2.Elements[1], 7), ConvertToString(val2.Elements[2], 7), ConvertToString(val2.Elements[3], 7), ConvertToString(val2.Elements[4], 6), ConvertToString(val2.Elements[5], 6));
			}
			else
			{
				pageStream.WriteLine("{0} {1} Td", ConvertToString(num16), ConvertToString(num17));
			}
			if (pdfFont.NeedSyntBold && fontBase.Bold)
			{
				pageStream.WriteLine("{0} w 2 Tr", ConvertToString(0.03099999949336052 * num19 * fontCorrectValue, precision_digits_font));
				SetStrokeColor(item.TextColor);
			}
			pageStream.Write("[");
			pageStream.Flush();
			memoryPageStream.Write(array, 0, array.Length);
			pageStream.WriteLine("] TJ");
			pageStream.WriteLine("ET");
			if (pdfFont.NeedSyntBold && fontBase.Bold)
			{
				pageStream.WriteLine("0 Tr");
			}
			if (fontBase.Underline)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				double num48 = num19 * 0.09;
				double num49 = (0.0 - num19) * 0.115;
				if (pdfFont.UnderscoreSize != 0)
				{
					num48 = num27;
					num49 = num28;
				}
				if (num48 < 0.10000000149011612)
				{
					num48 = 0.10000000149011612;
				}
				pageStream.WriteLine("{0} w", ConvertToString(num48));
				if (num14 != 0f)
				{
					double num50 = (double)num14 * Math.PI / 180.0;
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num50), 7), ConvertToString(Math.Sin(num50), 7), ConvertToString(0.0 - Math.Sin(num50), 7), ConvertToString(Math.Cos(num50), 7), ConvertToString(num10 + num12 / 2.0, 6), ConvertToString(num11 + num13 / 2.0, 6));
				}
				SetStrokeColor(item.TextColor);
				pageStream.WriteLine("{0} {1} m", ConvertToString(num16), ConvertToString(num17 + num49));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(num16 + num39), ConvertToString(num17 + num49));
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
			if (fontBase.Strikeout)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				double num51 = num19 * 0.09;
				double num52 = num19 * 0.4;
				if (pdfFont.StrikeoutSize != 0)
				{
					num51 = num29;
					num52 = num30;
				}
				if (num51 < 0.10000000149011612)
				{
					num51 = 0.10000000149011612;
				}
				pageStream.WriteLine("{0} w", ConvertToString(num51));
				if (num14 != 0f)
				{
					double num53 = (double)num14 * Math.PI / 180.0;
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num53), 7), ConvertToString(Math.Sin(num53), 7), ConvertToString(0.0 - Math.Sin(num53), 7), ConvertToString(Math.Cos(num53), 7), ConvertToString(num10 + num12 / 2.0, 6), ConvertToString(num11 + num13 / 2.0, 6));
				}
				SetStrokeColor(item.TextColor);
				pageStream.WriteLine("{0} {1} m", ConvertToString(num16), ConvertToString(num17 + num52));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(num16 + num39), ConvertToString(num17 + num52));
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
			pageStream.WriteLine("Q");
			PopColorFromStack();
			if (pageNumber != -1 && !string.IsNullOrWhiteSpace(item.Href) && !item.Href.Trim().StartsWith("javascript:") && !usePdfA)
			{
				StiLinkObject stiLinkObject = default(StiLinkObject);
				stiLinkObject.Link = item.Href;
				stiLinkObject.X = num16;
				stiLinkObject.Y = num17 + num33;
				stiLinkObject.Width = num39;
				stiLinkObject.Height = num31;
				stiLinkObject.Page = pageNumber;
				stiLinkObject.DestPage = -1;
				stiLinkObject.DestY = -1.0;
				linksArray.Add(stiLinkObject);
			}
		}
		_ = stiText.LinesOfUnderline;
		_ = 6;
		if (clipLongTextLines && !flag3)
		{
			pageStream.WriteLine("Q");
			PopColorFromStack();
		}
	}

	internal void RenderTextHQ(StiPdfData pp)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_12be: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c4: Invalid comparison between Unknown and I4
		//IL_13b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ba: Invalid comparison between Unknown and I4
		//IL_12f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fe: Invalid comparison between Unknown and I4
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1419: Unknown result type (might be due to invalid IL or missing references)
		//IL_141e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1420: Unknown result type (might be due to invalid IL or missing references)
		//IL_1423: Unknown result type (might be due to invalid IL or missing references)
		//IL_1439: Expected I4, but got Unknown
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_1338: Invalid comparison between Unknown and I4
		//IL_0922: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Expected O, but got Unknown
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1354: Unknown result type (might be due to invalid IL or missing references)
		//IL_1357: Invalid comparison between Unknown and I4
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Invalid comparison between Unknown and I4
		//IL_1322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Invalid comparison between Unknown and I4
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Invalid comparison between Unknown and I4
		//IL_0c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Invalid comparison between Unknown and I4
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3c: Expected O, but got Unknown
		//IL_0c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c60: Invalid comparison between Unknown and I4
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c63: Unknown result type (might be due to invalid IL or missing references)
		IStiTextBrush stiTextBrush = pp.Component as IStiTextBrush;
		IStiText stiText = pp.Component as IStiText;
		IStiTextOptions stiTextOptions = pp.Component as IStiTextOptions;
		IStiTextHorAlignment stiTextHorAlignment = pp.Component as IStiTextHorAlignment;
		IStiVertAlignment stiVertAlignment = pp.Component as IStiVertAlignment;
		StiText stiText2 = pp.Component as StiText;
		bool wordWrap = stiTextOptions != null && stiTextOptions.TextOptions.WordWrap;
		bool flag = stiTextHorAlignment != null && (int)stiTextHorAlignment.HorAlignment == 3;
		bool rightToLeft = stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
		int num = 0;
		float num2 = 0f;
		if (flag)
		{
			wordWrap = true;
		}
		bool useUnicode = pdfFont.UseUnicode;
		if (stiText == null)
		{
			return;
		}
		StiMargins margins = stiText2.Margins;
		double num3 = 0.72 * margins.Left;
		double num4 = 0.72 * margins.Right;
		double num5 = 0.72 * margins.Top;
		double num6 = 0.72 * margins.Bottom;
		double num7 = pp.X + num3;
		double num8 = pp.Y + num6;
		double num9 = pp.Width - num3 - num4;
		double num10 = pp.Height - num5 - num6;
		if (clipLongTextLines)
		{
			pageStream.WriteLine("q");
			PushColorToStack();
			pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(num7), ConvertToString(num8), ConvertToString(num9), ConvertToString(num10));
		}
		Color color = Color.Transparent;
		if (stiTextBrush != null)
		{
			color = StiBrush.ToColor(stiTextBrush.TextBrush);
			SetNonStrokeColor(color);
			StoreShadingData2(pp.X, pp.Y, pp.Width, pp.Height, stiTextBrush.TextBrush);
			if (stiTextBrush.TextBrush is StiGradientBrush || stiTextBrush.TextBrush is StiGlareBrush)
			{
				pageStream.WriteLine("/Pattern cs /P{0} scn", 1 + shadingCurrent);
			}
			if (stiTextBrush.TextBrush is StiHatchBrush)
			{
				StiBrush textBrush = stiTextBrush.TextBrush;
				StiHatchBrush brush = (StiHatchBrush)(object)((textBrush is StiHatchBrush) ? textBrush : null);
				pageStream.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber(brush) + 1);
			}
		}
		double num11 = 1.0;
		IStiFont stiFont = pp.Component as IStiFont;
		if (stiFont != null)
		{
			num11 = stiFont.Font.SizeInPoints;
		}
		double num12 = (StiDpiHelper.NeedFontScaling ? StiDpiHelper.GraphicsScale : 1.0);
		double num13 = num11 * fontCorrectValue * (double)pdfFont.ASC / 1000.0 * num12;
		double num14 = num11 * fontCorrectValue * (double)pdfFont.DESC / 1000.0 * num12;
		double num15 = num11 * fontCorrectValue * (double)pdfFont.LineGap / 1000.0 * num12;
		double num16 = num11 * fontCorrectValue * (double)pdfFont.tmASC / 1000.0 * num12;
		double num17 = num11 * fontCorrectValue * (double)pdfFont.tmDESC / 1000.0 * -1.0 * num12;
		double num18 = num11 * fontCorrectValue * (double)pdfFont.tmExternal / 1000.0 * num12;
		double num19 = num11 * fontCorrectValue * (double)pdfFont.UnderscoreSize / 1000.0 * num12;
		double num20 = num11 * fontCorrectValue * (double)pdfFont.UnderscorePosition / 1000.0 * num12;
		double num21 = num11 * fontCorrectValue * (double)pdfFont.StrikeoutSize / 1000.0 * num12;
		double num22 = num11 * fontCorrectValue * (double)pdfFont.StrikeoutPosition / 1000.0 * num12;
		double num23 = (num16 - num17 + num18) * (double)stiText2.LineSpacing * (double)StiOptions.Engine.TextLineSpacingScale;
		double num24 = num16;
		double num25 = num17;
		if ((pdfFont.fsSelection & 0x80u) != 0)
		{
			num23 = (num13 - num14 + num15) * (double)stiText2.LineSpacing;
			num24 = num13;
			num25 = num14;
		}
		float num26 = 0.85f;
		float num27 = 0.85f;
		num7 += (double)num26;
		num9 -= (double)(num26 * 2f);
		num8 += (double)num27;
		num10 -= (double)(num27 * 2f);
		double num28 = 0.0;
		if (!isWpf && stiText2.TextQuality == StiTextQuality.Standard)
		{
			num28 = (double)pdfFont.Widths[0] / 2.0 * 0.99;
			double num29 = num28 * (num11 * fontCorrectValue) / 1000.0;
			num7 += num29;
			num9 -= num29 * 2.0;
		}
		StringBuilder stringBuilder = new StringBuilder(stiText.Text);
		stringBuilder.Replace("\r", "");
		stringBuilder.Replace("\u001f", "");
		List<LineInfo> list;
		if (isWpf)
		{
			list = new List<LineInfo>();
			List<string> text = StiExportUtils.SplitString(stringBuilder.ToString(), removeControl: false);
			if (stiText2.Page == null)
			{
				stiText2.Page = report.RenderedPages[0];
			}
			RectangleD val = stiText2.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			((RectangleD)(ref val)).X = ((RectangleD)(ref val)).X + 1.0;
			if (((RectangleD)(ref val)).Width >= 2.0)
			{
				((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - 2.0;
			}
			val = stiText2.ConvertTextMargins(val, convert: false);
			val = stiText2.ConvertTextBorders(val, convert: false);
			StiSplitTextIntoLinesEventArgs stiSplitTextIntoLinesEventArgs = new StiSplitTextIntoLinesEventArgs();
			stiSplitTextIntoLinesEventArgs.TextComp = stiText2;
			stiSplitTextIntoLinesEventArgs.RectD = val;
			stiSplitTextIntoLinesEventArgs.Text = text;
			stiSplitTextIntoLinesEventArgs.LinesInfo = list;
			stiSplitTextIntoLinesEventArgs.WordWrap = wordWrap;
			string text2 = OnSplitTextIntoLines(stiSplitTextIntoLinesEventArgs);
		}
		else
		{
			RectangleD paintRectangle = stiText2.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			paintRectangle = stiText2.ConvertTextMargins(paintRectangle, convert: false);
			paintRectangle = stiText2.ConvertTextBorders(paintRectangle, convert: false);
			Bitmap val2 = new Bitmap(1, 1);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			list = StiTextDrawing.SplitTextWordwrap(stringBuilder.ToString(), val3, StiFontUtils.ChangeFontSize(stiText2.Font, stiText2.Font.Size * (float)StiDpiHelper.GraphicsScale), paintRectangle, stiTextOptions?.TextOptions, (StiTextHorAlignment)((stiTextHorAlignment != null) ? ((int)stiTextHorAlignment.HorAlignment) : 0), stiText2.TextQuality == StiTextQuality.Typographic);
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Text = StiBidirectionalConvert2.ConvertString(list[i].Text, rightToLeft);
		}
		if (num9 > 0.0 && stringBuilder.Length > 0)
		{
			double num30 = num7;
			double num31 = num8;
			double num32 = num9;
			double num33 = num10;
			if (stiTextOptions != null)
			{
				num2 = stiTextOptions.TextOptions.Angle;
				if (num2 != 0f)
				{
					if ((num2 > 45f && num2 < 135f) || (num2 > 225f && num2 < 315f))
					{
						double num34 = num9;
						num9 = num10;
						num10 = num34;
					}
					num7 = (0.0 - num9) / 2.0;
					num8 = (0.0 - num10) / 2.0;
				}
			}
			num = list.Count;
			int num35 = -1;
			if (stiTextOptions != null && (int)stiTextOptions.TextOptions.Trimming != 0)
			{
				double num36 = num10 / num23;
				num35 = (int)num36;
				if (num36 > (double)num35)
				{
					num35++;
				}
				if (num35 < 1)
				{
					num35 = 1;
				}
				if (num35 < num)
				{
					num = num35;
				}
			}
			if (stiVertAlignment != null)
			{
				double num37 = num23;
				double num38 = (double)num * num37;
				StiVertAlignment val4 = stiVertAlignment.VertAlignment;
				if (num2 != 0f && num2 != 90f && num2 != 180f && num2 != 270f)
				{
					val4 = (StiVertAlignment)1;
				}
				if ((int)val4 == 0)
				{
					num8 = ((!CompatibleMode160) ? (num8 + (num10 - num38 + (num37 - num24))) : (num8 + (num10 - (num38 - (0.0 - num25)))));
				}
				if ((int)val4 == 1)
				{
					num8 = ((!CompatibleMode160) ? (num8 + ((num10 - num38) / 2.0 + (num37 - num24))) : (num8 + ((num10 - num38) / 2.0 + (0.0 - num25))));
				}
				if ((int)val4 == 2)
				{
					num8 = ((!CompatibleMode160) ? (num8 + (num37 - num24)) : (num8 + (0.0 - num25)));
				}
			}
			pageStream.WriteLine("BT");
			double num39 = 0.0;
			double num40 = 0.0;
			double[,] array = new double[list.Count, 3];
			Matrix val5 = null;
			if (num2 != 0f)
			{
				double num41 = (double)num2 * Math.PI / 180.0;
				val5 = new Matrix((float)Math.Cos(num41), (float)Math.Sin(num41), (float)(0.0 - Math.Sin(num41)), (float)Math.Cos(num41), (float)(num30 + num32 / 2.0), (float)(num31 + num33 / 2.0));
				if (!pdfFont.NeedSyntItalic || stiFont == null || !stiFont.Font.Italic)
				{
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} Tm", ConvertToString(val5.Elements[0]), ConvertToString(val5.Elements[1]), ConvertToString(val5.Elements[2]), ConvertToString(val5.Elements[3]), ConvertToString(val5.Elements[4]), ConvertToString(val5.Elements[5]));
				}
			}
			if (pdfFont.NeedSyntBold && stiFont != null && stiFont.Font.Bold)
			{
				pageStream.WriteLine("{0} w 2 Tr", ConvertToString(0.03099999949336052 * num11, 3));
				SetStrokeColor(color);
			}
			for (int j = 0; j < num; j++)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				double num42 = 0.0;
				int num43 = 0;
				stringBuilder2.Append(useUnicode ? "<" : "(");
				LineInfo val6 = list[j];
				for (int k = 0; k < val6.Text.Length; k++)
				{
					int num44 = val6.Text[k];
					if (num44 == 9)
					{
						num44 = 32;
					}
					int num45 = pdfFont.UnicodeMap[num44];
					int num46 = 1000;
					if (num45 >= 32)
					{
						num46 = pdfFont.Widths[num45 - 32];
					}
					double num47 = 0.0;
					num42 += val6.Widths[k] * 10.0;
					num43 += num46;
					num47 = 0.0 - (num42 * 0.72 / (num11 * fontCorrectValue / 100.0) - (double)num43);
					num43 += -(int)num47;
					if (useUnicode)
					{
						int num48 = pdfFont.GlyphList[num45];
						if (num48 == 65535)
						{
							num48 = 0;
						}
						stringBuilder2.Append(num48.ToString("X4"));
						if (k < val6.Text.Length - 1)
						{
							stringBuilder2.Append(">" + ConvertToString(num47) + "<");
						}
					}
					else
					{
						stringBuilder2.Append(ConvertToEscapeSequencePlusTabs(((char)num45).ToString()));
						if (k < val6.Text.Length - 1)
						{
							stringBuilder2.Append(")" + ConvertToString(num47) + "(");
						}
					}
				}
				stringBuilder2.Append(useUnicode ? ">" : ")");
				string text3 = stringBuilder2.ToString();
				double num49 = num42 * 0.72 / 10.0;
				double num50 = num7;
				double num51 = num8 + num23 * (double)(num - j - 1);
				if (stiTextHorAlignment != null)
				{
					StiTextHorAlignment val7 = stiTextHorAlignment.HorAlignment;
					if (flag && !val6.NeedWidthAlign)
					{
						val7 = (StiTextHorAlignment)0;
					}
					if (stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft)
					{
						if ((int)val7 == 0)
						{
							val7 = (StiTextHorAlignment)2;
						}
						else if ((int)val7 == 2)
						{
							val7 = (StiTextHorAlignment)0;
						}
					}
					if ((int)val7 == 1)
					{
						num50 += (num9 - num49) / 2.0;
					}
					if ((int)val7 == 2)
					{
						num50 += num9 - num49;
					}
					_ = 3;
				}
				if (useUnicode)
				{
					text3 = text3.Replace(" ", pdfFont.GlyphList[32].ToString("X4")).Replace("\a", "0000");
				}
				array[j, 0] = num50;
				array[j, 1] = num51;
				array[j, 2] = num49;
				if (pdfFont.NeedSyntItalic && stiFont != null && stiFont.Font.Italic)
				{
					Matrix val8 = new Matrix(1f, 0f, 0.325f, 1f, (float)num50, (float)num51);
					if (val5 != null)
					{
						val8.Multiply(val5, (MatrixOrder)1);
					}
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} Tm", ConvertToString(val8.Elements[0]), ConvertToString(val8.Elements[1]), ConvertToString(val8.Elements[2]), ConvertToString(val8.Elements[3]), ConvertToString(val8.Elements[4]), ConvertToString(val8.Elements[5]));
				}
				else
				{
					pageStream.WriteLine("{0} {1} Td", ConvertToString(num50 - num39), ConvertToString(num51 - num40));
					num39 = num50;
					num40 = num51;
				}
				byte[] array2 = new byte[text3.Length];
				for (int l = 0; l < text3.Length; l++)
				{
					array2[l] = (byte)text3[l];
				}
				pageStream.Write("[");
				pageStream.Flush();
				memoryPageStream.Write(array2, 0, array2.Length);
				pageStream.WriteLine("] TJ");
			}
			if (pdfFont.NeedSyntBold && stiFont != null && stiFont.Font.Bold)
			{
				pageStream.WriteLine("0 Tr");
			}
			pageStream.WriteLine("ET");
			if (stiFont != null && stiFont.Font.Underline)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				double num52 = num11 * 0.09;
				double num53 = (0.0 - num11) * 0.115;
				if (pdfFont.UnderscoreSize != 0)
				{
					num52 = num19;
					num53 = num20;
				}
				if (num52 < 0.10000000149011612)
				{
					num52 = 0.10000000149011612;
				}
				pageStream.WriteLine("{0} w", ConvertToString(num52));
				if (num2 != 0f)
				{
					double num54 = (double)num2 * Math.PI / 180.0;
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num54)), ConvertToString(Math.Sin(num54)), ConvertToString(0.0 - Math.Sin(num54)), ConvertToString(Math.Cos(num54)), ConvertToString(num30 + num32 / 2.0), ConvertToString(num31 + num33 / 2.0));
				}
				if (stiTextBrush != null)
				{
					SetStrokeColor(color);
				}
				for (int m = 0; m < list.Count; m++)
				{
					if (array[m, 2] != 0.0)
					{
						pageStream.WriteLine("{0} {1} m", ConvertToString(array[m, 0]), ConvertToString(array[m, 1] + num53));
						pageStream.WriteLine("{0} {1} l S", ConvertToString(array[m, 0] + array[m, 2]), ConvertToString(array[m, 1] + num53));
					}
				}
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
			if (stiFont != null && stiFont.Font.Strikeout)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				double num55 = num11 * 0.09;
				double num56 = num11 * 0.4;
				if (pdfFont.StrikeoutSize != 0)
				{
					num55 = num21;
					num56 = num22;
				}
				if (num55 < 0.10000000149011612)
				{
					num55 = 0.10000000149011612;
				}
				pageStream.WriteLine("{0} w", ConvertToString(num55));
				if (num2 != 0f)
				{
					double num57 = (double)num2 * Math.PI / 180.0;
					pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num57)), ConvertToString(Math.Sin(num57)), ConvertToString(0.0 - Math.Sin(num57)), ConvertToString(Math.Cos(num57)), ConvertToString(num30 + num32 / 2.0), ConvertToString(num31 + num33 / 2.0));
				}
				if (stiTextBrush != null)
				{
					SetStrokeColor(color);
				}
				for (int n = 0; n < list.Count; n++)
				{
					if (array[n, 2] != 0.0)
					{
						pageStream.WriteLine("{0} {1} m", ConvertToString(array[n, 0]), ConvertToString(array[n, 1] + num56));
						pageStream.WriteLine("{0} {1} l S", ConvertToString(array[n, 0] + array[n, 2]), ConvertToString(array[n, 1] + num56));
					}
				}
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
		}
		if ((int)stiText.LinesOfUnderline != 6)
		{
			double num58 = pp.Y + num6 + (double)num27;
			bool flag2 = true;
			bool flag3 = true;
			if (stiVertAlignment != null)
			{
				if (num == 0)
				{
					num = 1;
				}
				double num59 = num23 * (double)num;
				StiVertAlignment val9 = stiVertAlignment.VertAlignment;
				if (num2 != 0f && num2 != 90f && num2 != 180f && num2 != 270f)
				{
					val9 = (StiVertAlignment)1;
				}
				if ((int)val9 == 0)
				{
					num58 += num10 - num24;
					flag2 = false;
				}
				if ((int)val9 == 1)
				{
					num58 += (num10 - num59) / 2.0 + (num23 - num24);
				}
				if ((int)val9 == 2)
				{
					num58 += num23 - num24;
					flag3 = false;
				}
			}
			num58 += num25;
			bool flag4 = false;
			double num60 = 0.0;
			if (pp.Component is IStiBorder stiBorder)
			{
				Color color2 = stiBorder.Border.Color;
				SetStrokeColor(color2);
				double num61 = stiBorder.Border.Size;
				if ((int)stiText.LinesOfUnderline == 5)
				{
					num61 = 1.0;
				}
				double num62 = num61 * 0.72 * 0.9;
				pageStream.WriteLine("{0} w", ConvertToString(num62));
				flag4 = (int)stiText.LinesOfUnderline == 5;
				if (flag4)
				{
					num60 = num62;
				}
				double num63 = num62 * 0.04;
				StiPenStyle linesOfUnderline = stiText.LinesOfUnderline;
				switch (linesOfUnderline - 1)
				{
				case 3:
					pageStream.WriteLine("[{0} {1}] 0 d", ConvertToString(num63), ConvertToString(num63 * 55.0));
					break;
				case 0:
					pageStream.WriteLine("[{0} {1}] 0 d", ConvertToString(num63 * 50.0), ConvertToString(num63 * 55.0));
					break;
				case 1:
					pageStream.WriteLine("[{0} {1} {2} {1}] 0 d", ConvertToString(num63 * 50.0), ConvertToString(num63 * 55.0), ConvertToString(num63));
					break;
				case 2:
					pageStream.WriteLine("[{0} {1} {2} {1} {2} {1}] 0 d", ConvertToString(num63 * 50.0), ConvertToString(num63 * 55.0), ConvertToString(num63));
					break;
				}
			}
			double num64 = num58;
			if (flag2)
			{
				for (; num64 + num23 < pp.Y + pp.Height - num5; num64 += num23)
				{
				}
			}
			while (num64 > pp.Y + num6 + (flag3 ? 0.0 : num23))
			{
				pageStream.WriteLine("{0} {2} m {1} {2} l S", ConvertToString(pp.X), ConvertToString(pp.X + pp.Width), ConvertToString(num64 + num60));
				if (flag4)
				{
					pageStream.WriteLine("{0} {2} m {1} {2} l S", ConvertToString(pp.X), ConvertToString(pp.X + pp.Width), ConvertToString(num64 - num60));
				}
				num64 -= num23;
			}
		}
		if (clipLongTextLines)
		{
			pageStream.WriteLine("Q");
			PopColorFromStack();
		}
	}

	internal void RenderTextFont(StiPdfData pp)
	{
		if (pp.Component is IStiFont stiFont)
		{
			int fontNumber = pdfFont.GetFontNumber(stiFont.Font);
			double num = (double)stiFont.Font.SizeInPoints * fontCorrectValue;
			pageStream.WriteLine("/F{0} {1} Tf", fontNumber, ConvertToString(num, precision_digits_font));
			pdfFont.CurrentFont = fontNumber;
			if ((annotsCounter > 0 || annots2Counter > 0) && (pp.Component as StiText).Editable)
			{
				StiEditableObject stiEditableObject = (StiEditableObject)annotsArray[annotsCurrent];
				stiEditableObject.FontNumber = fontNumber;
				stiEditableObject.FontSize = num;
			}
		}
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "PdfFiles");
	}

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportPdf(report, stream, settings as StiPdfExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiPdfExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			this.report = report;
			this.fileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(form_Complete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	private void form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = GetFileName(report, sendEMail);
			}
			if (fileName == null)
			{
				return;
			}
			StiFileUtils.ProcessReadOnly(fileName);
			try
			{
				using FileStream stream = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
				stiPdfExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiPdfExportSettings.ImageQuality = (float)form["ImageQuality"];
				stiPdfExportSettings.ImageCompressionMethod = (StiPdfImageCompressionMethod)form["ImageCompressionMethod"];
				stiPdfExportSettings.ImageResolution = (float)form["Resolution"];
				stiPdfExportSettings.EmbeddedFonts = (bool)form["EmbeddedFonts"] & (bool)form["EmbeddedFontsEnabled"];
				stiPdfExportSettings.ExportRtfTextAsImage = (bool)form["ExportRtfTextAsImage"];
				stiPdfExportSettings.PasswordInputUser = form["UserPassword"] as string;
				stiPdfExportSettings.PasswordInputOwner = form["OwnerPassword"] as string;
				stiPdfExportSettings.UserAccessPrivileges = (StiUserAccessPrivileges)form["UserAccessPrivileges"];
				stiPdfExportSettings.KeyLength = (StiPdfEncryptionKeyLength)form["EncryptionKeyLength"];
				stiPdfExportSettings.GetCertificateFromCryptoUI = (bool)form["GetCertificateFromCryptoUI"];
				stiPdfExportSettings.UseDigitalSignature = (bool)form["UseDigitalSignature"];
				stiPdfExportSettings.SubjectNameString = form["SubjectNameString"] as string;
				stiPdfExportSettings.PdfComplianceMode = (StiPdfComplianceMode)form["PdfComplianceMode"];
				stiPdfExportSettings.ImageFormat = (StiImageFormat)form["ImageFormat"];
				stiPdfExportSettings.DitheringType = (StiMonochromeDitheringType)form["MonochromeDitheringType"];
				stiPdfExportSettings.AllowEditable = (StiPdfAllowEditable)form["AllowEditable"];
				stiPdfExportSettings.ImageResolutionMode = (StiImageResolutionMode)form["ImageResolutionMode"];
				stiPdfExportSettings.CertificateThumbprint = (string)form["CertificateThumbprint"];
				StiPdfExportSettings settings = stiPdfExportSettings;
				StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		base.IsStopped = true;
	}

	public int GetHatchNumber(StiHatchBrush brush)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (hatchArray.Count > 0)
		{
			for (int i = 0; i < hatchArray.Count; i++)
			{
				StiHatchBrush val = (StiHatchBrush)hatchArray[i];
				if (brush.Style == val.Style && brush.BackColor == val.BackColor && brush.ForeColor == val.ForeColor)
				{
					return i;
				}
			}
		}
		hatchArray.Add(brush);
		return hatchArray.Count - 1;
	}

	public int GetHatchNumber(HatchBrush brush)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		return this.GetHatchNumber(new StiHatchBrush(brush.HatchStyle, brush.ForegroundColor, brush.BackgroundColor));
	}

	public int GetShadingFunctionNumber(Color color1, Color color2, bool isGlare)
	{
		if (shadingFunctionArray.Count > 0)
		{
			for (int i = 0; i < shadingFunctionArray.Count; i++)
			{
				StiShadingFunctionData stiShadingFunctionData = shadingFunctionArray[i];
				if (stiShadingFunctionData.Color1.Equals((object?)color1) && stiShadingFunctionData.Color2.Equals((object?)color2) && stiShadingFunctionData.IsGlare == isGlare)
				{
					return i;
				}
			}
		}
		StiShadingFunctionData item = default(StiShadingFunctionData);
		item.Color1 = color1;
		item.Color2 = color2;
		item.IsGlare = isGlare;
		shadingFunctionArray.Add(item);
		return shadingFunctionArray.Count - 1;
	}

	private bool IsWordWrapSymbol(StringBuilder sb, int index)
	{
		char c = sb[index];
		if ((c >= '\u3000' && c <= '\ud7af') || (char.IsWhiteSpace(c) && c != '\u00a0') || c == '(' || c == '{')
		{
			return true;
		}
		if (index > 0)
		{
			char c2 = sb[index - 1];
			if (c2 == '!' || c2 == '%' || c2 == ')' || c2 == '}' || c2 == '-' || c2 == '?')
			{
				return true;
			}
		}
		return false;
	}

	private float GetTabsSize(IStiTextOptions textOp, double sizeInPt, double currentPosition)
	{
		if (textOp != null && textOp.TextOptions != null)
		{
			double num = currentPosition;
			float num2 = 754f / (float)sizeInPt;
			float num3 = num2 * textOp.TextOptions.DistanceBetweenTabs;
			float num4 = num2 * textOp.TextOptions.FirstTabOffset + num3;
			if (isWpf)
			{
				num2 = 751f / (float)sizeInPt;
				num4 = (num3 = num2 * 64f);
			}
			if (currentPosition < (double)num4)
			{
				num = num4;
			}
			else if (textOp.TextOptions.DistanceBetweenTabs > 0f)
			{
				int num5 = (int)((currentPosition - (double)num4) / (double)num3);
				num5++;
				num = num4 + (float)num5 * num3;
			}
			return (float)(num - currentPosition);
		}
		return 0f;
	}

	private void CheckGraphicsForTextRenderer()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (graphicsForTextRenderer == null)
		{
			if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
			{
				graphicsForTextRenderer = Graphics.FromHwnd(IntPtr.Zero);
				return;
			}
			imageForGraphicsForTextRenderer = (Image)new Bitmap(1, 1);
			graphicsForTextRenderer = Graphics.FromImage(imageForGraphicsForTextRenderer);
		}
	}

	private void AddXref(int num)
	{
		sw.Flush();
		long position = sw.BaseStream.Position;
		xref.Add(num, position.ToString("D10") + " 00000 n");
		currentObjectNumber = num;
		currentGenerationNumber = 0;
	}

	internal string ConvertToString(double Value)
	{
		int decimals = precision_digits;
		return Math.Round((decimal)Value, decimals).ToString("G", currentNumberFormat);
	}

	internal string ConvertToString(double Value, int precision)
	{
		return Math.Round((decimal)Value, precision).ToString("G", currentNumberFormat);
	}

	internal static string ConvertToEscapeSequence(string value)
	{
		string text = "()\\\r\n\b\f";
		string text2 = "()\\rnbf";
		if (value == null)
		{
			return "";
		}
		string text3 = string.Empty;
		for (int i = 0; i < value.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < text.Length; j++)
			{
				if (value[i] == text[j])
				{
					text3 = text3 + "\\" + text2[j];
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				text3 += value[i];
			}
		}
		return text3;
	}

	internal static string ConvertToEscapeSequencePlusTabs(string value)
	{
		string text = "()\\\r\n\t\b\f";
		string text2 = "()\\rntbf";
		if (value == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < value.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < text.Length; j++)
			{
				if (value[i] == text[j])
				{
					stringBuilder.Append("\\" + text2[j]);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				stringBuilder.Append(value[i]);
			}
		}
		return stringBuilder.ToString();
	}

	internal static string CorrectNameEnconding(string input)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = bytes;
		foreach (byte b in array)
		{
			if (b < 33 || b > 126 || b == 35 || b == 47)
			{
				stringBuilder.Append($"#{b:X2}");
			}
			else
			{
				stringBuilder.Append((char)b);
			}
		}
		return stringBuilder.ToString();
	}

	internal void StoreWysiwygSymbols(StiText text, int pageNumber = -1)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		CheckGraphicsForTextRenderer();
		List<RunInfo> outRunsList = new List<RunInfo>();
		List<StiFontState> outFontsList = new List<StiFontState>();
		string text2 = text.Text;
		RectangleD rect = text.Page.Unit.ConvertToHInches(text.ComponentToPage(text.ClientRectangle));
		RectangleD rect2 = text.ConvertTextMargins(rect, convert: false);
		rect2 = text.ConvertTextBorders(rect2, convert: false);
		bool flag = StiOptions.Engine.UseNewHtmlEngine && text.AllowHtmlTags;
		if (flag)
		{
			StiHtmlTextRender.DrawTextForOutput(text, out outRunsList, out outFontsList);
		}
		else
		{
			StiTextRenderer.DrawTextForOutput(graphicsForTextRenderer, text2, text.Font, rect2, StiBrush.ToColor(text.TextBrush), StiBrush.ToColor(text.Brush), 1.0, text.HorAlignment, text.VertAlignment, ((IStiTextOptions)text).TextOptions.WordWrap, ((IStiTextOptions)text).TextOptions.RightToLeft, 1.0, (double)((IStiTextOptions)text).TextOptions.Angle, ((IStiTextOptions)text).TextOptions.Trimming, ((IStiTextOptions)text).TextOptions.LineLimit, text.CheckAllowHtmlTags(), outRunsList, outFontsList, ((IStiTextOptions)text).TextOptions, (double?)1.0);
		}
		pdfFont.StoreUnicodeSymbolsInMap(new StringBuilder(text2));
		foreach (RunInfo item in outRunsList)
		{
			Font fontBase = outFontsList[item.FontIndex].FontBase;
			int fontNumber = pdfFont.GetFontNumber(fontBase);
			pdfFont.CurrentFont = fontNumber;
			StringBuilder stringBuilder = new StringBuilder();
			if (!flag)
			{
				for (int i = 0; i < item.GlyphIndexList.Length; i++)
				{
					stringBuilder.Append((char)item.GlyphIndexList[i]);
				}
			}
			pdfFont.StoreGlyphsInMap(stringBuilder);
			pdfFont.StoreUnicodeSymbolsInMap(new StringBuilder(item.Text));
			if (pageNumber != -1 && !string.IsNullOrWhiteSpace(item.Href) && !item.Href.Trim().StartsWith("javascript:") && !usePdfA)
			{
				StiLinkObject stiLinkObject = default(StiLinkObject);
				stiLinkObject.Link = item.Href;
				stiLinkObject.Page = pageNumber;
				linksArray.Add(stiLinkObject);
			}
		}
	}

	internal void SetStrokeColor(Color tempColor)
	{
		string text = (string)colorHash1[tempColor];
		if (text == null)
		{
			text = $"{colorTable[tempColor.R]} {colorTable[tempColor.G]} {colorTable[tempColor.B]} RG";
			colorHash1[tempColor] = text;
		}
		pageStream.WriteLine(text);
		byte a = tempColor.A;
		if (a != lastColorStrokeA && StiOptions.Export.Pdf.AllowExtGState && useTransparency)
		{
			pageStream.WriteLine(GsTable[a, 0]);
			lastColorStrokeA = a;
			alphaTable[a] = true;
		}
	}

	internal void SetNonStrokeColor(Color tempColor)
	{
		string text = (string)colorHash2[tempColor];
		if (text == null)
		{
			text = $"{colorTable[tempColor.R]} {colorTable[tempColor.G]} {colorTable[tempColor.B]} rg";
			colorHash2[tempColor] = text;
		}
		pageStream.WriteLine(text);
		byte a = tempColor.A;
		if (a != lastColorNonStrokeA && StiOptions.Export.Pdf.AllowExtGState && useTransparency)
		{
			pageStream.WriteLine(GsTable[a, 1]);
			lastColorNonStrokeA = a;
			alphaTable[a] = true;
		}
	}

	internal void PushColorToStack()
	{
		colorStack.Push(lastColorStrokeA);
		colorStack.Push(lastColorNonStrokeA);
	}

	internal void PopColorFromStack()
	{
		lastColorNonStrokeA = (byte)colorStack.Pop();
		lastColorStrokeA = (byte)colorStack.Pop();
	}

	private void FillRectBrush(StiBrush brush, RectangleD rect)
	{
		Color nonStrokeColor = StiBrush.ToColor(brush);
		SetNonStrokeColor(nonStrokeColor);
		StoreShadingData2(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height, brush);
		if (brush is StiGradientBrush || brush is StiGlareBrush)
		{
			pageStream.WriteLine("/Pattern cs /P{0} scn", 1 + shadingCurrent);
		}
		if (brush is StiHatchBrush)
		{
			StiHatchBrush brush2 = (StiHatchBrush)(object)((brush is StiHatchBrush) ? brush : null);
			pageStream.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber(brush2) + 1);
		}
		if (brush is StiGlassBrush)
		{
			StiGlassBrush val = (StiGlassBrush)(object)((brush is StiGlassBrush) ? brush : null);
			if (val.DrawHatch)
			{
				StreamWriter streamWriter = pageStream;
				Brush topBrush = val.GetTopBrush();
				streamWriter.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber((HatchBrush)(object)((topBrush is HatchBrush) ? topBrush : null)) + 1);
			}
			else
			{
				SetNonStrokeColor(val.GetTopColor());
			}
			pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(((RectangleD)(ref rect)).X), ConvertToString(((RectangleD)(ref rect)).Y + ((RectangleD)(ref rect)).Height / 2.0), ConvertToString(((RectangleD)(ref rect)).Width), ConvertToString(((RectangleD)(ref rect)).Height / 2.0));
			if (val.DrawHatch)
			{
				StreamWriter streamWriter2 = pageStream;
				Brush bottomBrush = val.GetBottomBrush();
				streamWriter2.WriteLine("/Cs1 cs /PH{0} scn", GetHatchNumber((HatchBrush)(object)((bottomBrush is HatchBrush) ? bottomBrush : null)) + 1);
			}
			else
			{
				SetNonStrokeColor(val.GetBottomColor());
			}
			pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(((RectangleD)(ref rect)).X), ConvertToString(((RectangleD)(ref rect)).Y), ConvertToString(((RectangleD)(ref rect)).Width), ConvertToString(((RectangleD)(ref rect)).Height / 2.0));
			nonStrokeColor = Color.Transparent;
		}
		if (nonStrokeColor.A != 0)
		{
			pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(((RectangleD)(ref rect)).X), ConvertToString(((RectangleD)(ref rect)).Y), ConvertToString(((RectangleD)(ref rect)).Width), ConvertToString(((RectangleD)(ref rect)).Height));
		}
	}

	private void StoreStringLine(string noCryptString, string cryptString)
	{
		StoreStringLine(noCryptString, cryptString, escaping: false);
	}

	private void StoreStringLine(string noCryptString, string cryptString, bool escaping)
	{
		string st = noCryptString + ConvertToHexString(cryptString, escaping);
		StoreString(st);
		sw.WriteLine();
	}

	internal void StoreString(string st)
	{
		byte[] array = new byte[st.Length];
		for (int i = 0; i < st.Length; i++)
		{
			array[i] = (byte)st[i];
		}
		sw.Flush();
		stream2.Write(array, 0, array.Length);
	}

	internal string ConvertToHexString(string inString, bool escaping, bool forceHex = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(inString))
		{
			bool flag = forceHex;
			for (int i = 0; i < inString.Length; i++)
			{
				if (inString[i] > '\u007f')
				{
					flag = true;
				}
			}
			if (!encrypted)
			{
				if (flag)
				{
					if (!escaping)
					{
						stringBuilder.Append("(þÿ");
					}
					foreach (int num in inString)
					{
						stringBuilder.Append((char)((uint)(num >> 8) & 0xFFu));
						stringBuilder.Append((char)((uint)num & 0xFFu));
					}
					if (escaping)
					{
						stringBuilder = new StringBuilder("(þÿ" + ConvertToEscapeSequencePlusTabs(stringBuilder.ToString()));
					}
					stringBuilder.Append(")");
				}
				else if (escaping)
				{
					stringBuilder.Append("(" + ConvertToEscapeSequencePlusTabs(inString) + ")");
				}
				else
				{
					stringBuilder.Append("(" + inString + ")");
				}
			}
			else
			{
				byte[] array = null;
				if (flag)
				{
					array = new byte[(inString.Length + 1) * 2];
					for (int k = 0; k < inString.Length; k++)
					{
						int num2 = inString[k];
						array[2 + k * 2] = (byte)((uint)(num2 >> 8) & 0xFFu);
						array[2 + k * 2 + 1] = (byte)((uint)num2 & 0xFFu);
					}
					array[0] = 254;
					array[1] = byte.MaxValue;
				}
				else
				{
					array = new byte[inString.Length];
					for (int l = 0; l < inString.Length; l++)
					{
						array[l] = (byte)inString[l];
					}
				}
				byte[] array2 = pdfSecurity.EncryptData(array, currentObjectNumber, currentGenerationNumber);
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int m = 0; m < array2.Length; m++)
				{
					stringBuilder2.Append((char)array2[m]);
				}
				stringBuilder.Append("(" + ConvertToEscapeSequencePlusTabs(stringBuilder2.ToString()) + ")");
			}
		}
		else if (encrypted)
		{
			byte[] array3 = pdfSecurity.EncryptData(new byte[0], currentObjectNumber, currentGenerationNumber);
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int n = 0; n < array3.Length; n++)
			{
				stringBuilder3.Append((char)array3[n]);
			}
			stringBuilder.Append("(" + ConvertToEscapeSequencePlusTabs(stringBuilder3.ToString()) + ")");
		}
		else
		{
			stringBuilder.Append("()");
		}
		return stringBuilder.ToString();
	}

	private void StoreMemoryStream2(MemoryStream mem, string dictionaryString)
	{
		if (encrypted)
		{
			byte[] data = mem.ToArray();
			byte[] array = pdfSecurity.EncryptData(data, currentObjectNumber, currentGenerationNumber);
			sw.WriteLine(string.Format(dictionaryString, array.Length));
			sw.WriteLine(">>");
			sw.WriteLine("stream");
			sw.Flush();
			stream2.Write(array, 0, array.Length);
		}
		else
		{
			sw.WriteLine(string.Format(dictionaryString, mem.Length));
			sw.WriteLine(">>");
			sw.WriteLine("stream");
			sw.Flush();
			mem.WriteTo(stream2);
		}
		mem.Close();
	}

	private void StoreMemoryStream2(byte[] data, string dictionaryString)
	{
		if (encrypted)
		{
			byte[] array = pdfSecurity.EncryptData(data, currentObjectNumber, currentGenerationNumber);
			sw.WriteLine(string.Format(dictionaryString, array.Length));
			sw.WriteLine(">>");
			sw.WriteLine("stream");
			sw.Flush();
			stream2.Write(array, 0, array.Length);
		}
		else
		{
			sw.WriteLine(string.Format(dictionaryString, data.Length));
			sw.WriteLine(">>");
			sw.WriteLine("stream");
			sw.Flush();
			stream2.Write(data, 0, data.Length);
		}
	}

	private void RenderStartDoc(StiReport report, StiPagesCollection pages)
	{
		sw.WriteLine("%PDF-1.7");
		byte[] array = new byte[5] { 37, 226, 227, 207, 211 };
		sw.Flush();
		stream2.Write(array, 0, array.Length);
		sw.WriteLine("");
		AddXref(1);
		sw.WriteLine("1 0 obj");
		sw.WriteLine("<<");
		if (keyLength == StiPdfEncryptionKeyLength.Bit256_r5)
		{
			sw.WriteLine("/Extensions<</ADBE<</BaseVersion/1.7/ExtensionLevel 3>>>>");
		}
		if (keyLength == StiPdfEncryptionKeyLength.Bit256_r6)
		{
			sw.WriteLine("/Extensions<</ADBE<</BaseVersion/1.7/ExtensionLevel 8>>>>");
		}
		sw.WriteLine("/Type /Catalog");
		sw.WriteLine("/Pages 4 0 R");
		sw.WriteLine("/MarkInfo<</Marked true>>");
		sw.WriteLine("/Metadata {0} 0 R", info.Metadata.Ref);
		sw.WriteLine("/OutputIntents {0} 0 R", info.OutputIntents.Ref);
		sw.WriteLine("/StructTreeRoot 5 0 R");
		if (haveBookmarks)
		{
			sw.WriteLine("/Outlines {0} 0 R", info.Outlines.Ref);
			sw.WriteLine("/PageMode /UseOutlines");
		}
		else
		{
			sw.WriteLine("/PageMode /UseNone");
		}
		if (!PrintScaling)
		{
			sw.WriteLine("/ViewerPreferences");
			sw.WriteLine("<<");
			sw.WriteLine("/PrintScaling /None");
			sw.WriteLine(">>");
		}
		if (haveAnnots || haveDigitalSignature)
		{
			sw.WriteLine("/AcroForm {0} 0 R", info.AcroForm.Ref);
		}
		bool flag = autoPrint != StiPdfAutoPrintMode.None;
		bool flag2 = info.EmbeddedFilesList.Count > 0;
		if (flag || flag2)
		{
			sw.WriteLine("/Names <<");
			if (flag)
			{
				sw.WriteLine("/JavaScript {0} 0 R", info.EmbeddedJS.Ref);
			}
			if (flag2)
			{
				sw.WriteLine("/EmbeddedFiles <<");
				sw.Write("/Names [ ");
				for (int i = 0; i < info.EmbeddedFilesList.Count; i++)
				{
					StiPdfEmbeddedFileData stiPdfEmbeddedFileData = embeddedFiles[i];
					sw.Write("{0} {1} 0 R ", ConvertToHexString(stiPdfEmbeddedFileData.Name, escaping: true, forceHex: true), info.EmbeddedFilesList[i].Ref);
				}
				sw.WriteLine("]");
				sw.WriteLine(">>");
			}
			sw.WriteLine(">>");
		}
		if (zugferdComplianceMode != 0 || pdfComplianceMode == StiPdfComplianceMode.A3)
		{
			sw.Write("/AF [ ");
			for (int j = 0; j < info.EmbeddedFilesList.Count; j++)
			{
				sw.Write("{0} 0 R ", info.EmbeddedFilesList[j].Ref);
			}
			sw.WriteLine("]");
		}
		if (!usePdfA)
		{
			sw.WriteLine("/OCProperties << /OCGs [{0} 0 R] /D << /ON [{0} 0 R] /AS [<</Event /Print /OCGs [{0} 0 R] /Category [/Print]>>] >> >>", info.OptionalContentGroup.Ref);
		}
		if (!string.IsNullOrWhiteSpace(report.Culture))
		{
			StoreStringLine("/Lang ", report.Culture, escaping: true);
		}
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(2);
		sw.WriteLine("2 0 obj");
		sw.WriteLine("<<");
		StoreStringLine("/Producer ", producerName, escaping: true);
		StoreStringLine("/Creator ", creatorName, escaping: true);
		if (!string.IsNullOrEmpty(report.ReportAuthor))
		{
			StoreStringLine("/Author ", report.ReportAuthor, escaping: true);
		}
		if (!string.IsNullOrEmpty(report.ReportAlias))
		{
			StoreStringLine("/Subject ", report.ReportAlias, escaping: true);
		}
		if (!string.IsNullOrEmpty(report.ReportName))
		{
			StoreStringLine("/Title ", report.ReportName, escaping: true);
		}
		if (!string.IsNullOrEmpty(keywords))
		{
			StoreStringLine("/Keywords ", keywords, escaping: true);
		}
		StoreStringLine("/CreationDate ", "D:" + currentDateTime);
		StoreStringLine("/ModDate ", "D:" + currentDateTime);
		if (pdfComplianceMode == StiPdfComplianceMode.None)
		{
			foreach (StiMetaTag metaTag in report.MetaTags)
			{
				if (metaTag.Name.StartsWith("pdf:"))
				{
					string text = StiNameValidator.CorrectName(metaTag.Name.Substring(4), checkKeywords: false, report);
					if (!reservedKeywords.Contains(text))
					{
						StoreStringLine($"/{text} ", metaTag.Tag);
					}
				}
			}
		}
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(3);
		sw.WriteLine("3 0 obj");
		sw.WriteLine("<<");
		sw.WriteLine("/Cs1 [/Pattern /DeviceRGB]");
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(4);
		sw.WriteLine("4 0 obj");
		sw.WriteLine("<<");
		sw.WriteLine("/Type /Pages");
		sw.Write("/Kids [ ");
		for (int k = 0; k < pages.Count; k++)
		{
			sw.Write("{0} 0 R ", info.PageList[k].Ref);
		}
		sw.WriteLine("]");
		sw.WriteLine("/Count {0}", pages.Count);
		if (StiOptions.Export.Pdf.AllowInheritedPageResources)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add("/Resources");
			arrayList.Add("<<");
			arrayList.Add("/ProcSet [/PDF /Text /ImageC]");
			arrayList.Add("/Font");
			arrayList.Add("<<");
			for (int l = 0; l < fontsCounter; l++)
			{
				arrayList.Add($"/F{l} {info.FontList[l].Ref} 0 R");
			}
			arrayList.Add(">>");
			if (imagesCounter > 0)
			{
				arrayList.Add("/XObject");
				arrayList.Add("<<");
				for (int m = 0; m < imagesCounter; m++)
				{
					arrayList.Add($"/Image{m} {info.XObjectList[m].Ref} 0 R");
				}
				arrayList.Add(">>");
			}
			arrayList.Add("/Pattern");
			arrayList.Add("<<");
			arrayList.Add($"/P1 {info.Patterns.First.Ref} 0 R");
			for (int n = 0; n < hatchCounter; n++)
			{
				arrayList.Add($"/PH{1 + n} {info.Patterns.HatchItems[n].Ref} 0 R");
			}
			for (int num = 0; num < shadingCounter; num++)
			{
				arrayList.Add($"/P{2 + num} {info.Patterns.ShadingItems[num].Ref} 0 R");
			}
			arrayList.Add(">>");
			arrayList.Add("/ColorSpace 3 0 R");
			if (StiOptions.Export.Pdf.AllowExtGState && useTransparency)
			{
				arrayList.Add($"/ExtGState {info.ExtGState.Ref} 0 R");
			}
			if (!usePdfA)
			{
				arrayList.Add($"/Properties << /oc1 {info.OptionalContentGroup.Ref} 0 R >>");
			}
			arrayList.Add(">>");
			for (int num2 = 0; num2 < arrayList.Count; num2++)
			{
				sw.WriteLine((string)arrayList[num2]);
			}
		}
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(5);
		sw.WriteLine("5 0 obj");
		sw.WriteLine("<<");
		sw.WriteLine("/Type /StructTreeRoot");
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(6);
		sw.WriteLine("6 0 obj");
		sw.WriteLine("<<");
		sw.WriteLine("/Type /OCG");
		sw.WriteLine("/Name (Printable off)");
		sw.WriteLine("/Usage <<");
		sw.WriteLine("/Print << /PrintState /OFF >>");
		sw.WriteLine("/View << /ViewState /ON >>");
		sw.WriteLine(">>");
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		for (int num3 = 0; num3 < pages.Count; num3++)
		{
			AddXref(info.PageList[num3].Ref);
			sw.WriteLine("{0} 0 obj", info.PageList[num3].Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /Page");
			sw.WriteLine("/Parent 4 0 R");
			StiPage pageWithoutCache = pages.GetPageWithoutCache(num3);
			double num4 = 0.72 * report.Unit.ConvertToHInches(pageWithoutCache.PageHeight * (double)pageWithoutCache.SegmentPerHeight);
			double num5 = 0.72 * report.Unit.ConvertToHInches(pageWithoutCache.PageWidth * (double)pageWithoutCache.SegmentPerWidth);
			if (num4 > 14400.0)
			{
				num4 = 14400.0;
			}
			if (num5 > 14400.0)
			{
				num5 = 14400.0;
			}
			sw.WriteLine("/MediaBox [ 0 0 {0} {1} ]", ConvertToString(num5), ConvertToString(num4));
			if (!StiOptions.Export.Pdf.AllowInheritedPageResources)
			{
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add("/Resources");
				arrayList2.Add("<<");
				arrayList2.Add("/ProcSet [/PDF /Text /ImageC]");
				arrayList2.Add("/Font");
				arrayList2.Add("<<");
				for (int num6 = 0; num6 < fontsCounter; num6++)
				{
					arrayList2.Add($"/F{num6} {info.FontList[num6].Ref} 0 R");
				}
				arrayList2.Add(">>");
				if (imagesCounter > 0)
				{
					arrayList2.Add("/XObject");
					arrayList2.Add("<<");
					for (int num7 = 0; num7 < imagesCounter; num7++)
					{
						arrayList2.Add($"/Image{num7} {info.XObjectList[num7].Ref} 0 R");
					}
					arrayList2.Add(">>");
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append($"/P1 {info.Patterns.First.Ref} 0 R ");
				for (int num8 = 0; num8 < hatchCounter; num8++)
				{
					stringBuilder.Append($"/PH{1 + num8} {info.Patterns.HatchItems[num8].Ref} 0 R ");
				}
				for (int num9 = 0; num9 < shadingCounter; num9++)
				{
					if (((StiShadingData)shadingArray[num9]).Page == num3)
					{
						stringBuilder.Append($"/P{2 + num9} {info.Patterns.ShadingItems[num9].Ref} 0 R ");
					}
				}
				arrayList2.Add("/Pattern << " + stringBuilder.ToString() + " >>");
				arrayList2.Add("/ColorSpace 3 0 R");
				if (StiOptions.Export.Pdf.AllowExtGState && useTransparency)
				{
					arrayList2.Add($"/ExtGState {info.ExtGState.Ref} 0 R");
				}
				if (!usePdfA)
				{
					arrayList2.Add($"/Properties << /oc1 {info.OptionalContentGroup.Ref} 0 R >>");
				}
				arrayList2.Add(">>");
				for (int num10 = 0; num10 < arrayList2.Count; num10++)
				{
					sw.WriteLine((string)arrayList2[num10]);
				}
			}
			sw.WriteLine("/Contents {0} 0 R", info.PageList[num3].Content.Ref);
			if (useTransparency)
			{
				sw.WriteLine("/Group");
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Group");
				sw.WriteLine("/S /Transparency");
				sw.WriteLine("/CS /DeviceRGB");
				sw.WriteLine(">>");
			}
			if (haveLinks || haveAnnots || haveDigitalSignature || haveTooltips)
			{
				sw.WriteLine("/Annots [");
				if (haveLinks)
				{
					for (int num11 = 0; num11 < linksCounter; num11++)
					{
						if (((StiLinkObject)linksArray[num11]).Page == num3)
						{
							sw.WriteLine("{0} 0 R ", info.LinkList[num11].Ref);
						}
					}
				}
				if (haveAnnots)
				{
					for (int num12 = 0; num12 < annotsCounter; num12++)
					{
						StiEditableObject stiEditableObject = (StiEditableObject)annotsArray[num12];
						if (stiEditableObject.Page == num3)
						{
							sw.WriteLine("{0} 0 R ", info.AcroForm.Annots[num12].Ref);
						}
					}
					for (int num13 = 0; num13 < annots2Counter; num13++)
					{
						StiEditableObject stiEditableObject2 = annots2Array[num13];
						if (stiEditableObject2.Page == num3)
						{
							for (int num14 = 0; num14 < info.AcroForm.CheckBoxes[num13].Count; num14++)
							{
								sw.WriteLine("{0} 0 R ", info.AcroForm.CheckBoxes[num13][num14].Ref);
							}
						}
					}
					for (int num15 = 0; num15 < unsignedSignaturesCounter; num15++)
					{
						StiEditableObject stiEditableObject3 = unsignedSignaturesArray[num15];
						if (stiEditableObject3.Page == num3)
						{
							sw.WriteLine("{0} 0 R ", info.AcroForm.UnsignedSignatures[num15].Ref);
						}
					}
				}
				if (haveDigitalSignature && num3 == signaturePageNumber)
				{
					sw.WriteLine("{0} 0 R ", info.AcroForm.Signatures[0].Ref);
				}
				if (haveTooltips)
				{
					for (int num16 = 0; num16 < tooltipsCounter; num16++)
					{
						if (tooltipsArray[num16].Page == num3)
						{
							sw.WriteLine("{0} 0 R ", info.AcroForm.Tooltips[num16].Ref);
						}
					}
				}
				sw.WriteLine("]");
			}
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderEndDoc()
	{
		sw.Flush();
		long position = sw.BaseStream.Position;
		sw.WriteLine("xref");
		sw.WriteLine("0 {0}", xref.Count + 1);
		sw.WriteLine("0000000000 65535 f");
		for (int i = 0; i < xref.Count; i++)
		{
			sw.WriteLine((string)xref.GetByIndex(i));
		}
		sw.WriteLine("trailer");
		sw.WriteLine("<<");
		sw.WriteLine("/Size {0}", xref.Count + 1);
		sw.WriteLine("/Root 1 0 R");
		sw.WriteLine("/Info 2 0 R");
		if (encrypted)
		{
			sw.WriteLine("/Encrypt {0} 0 R", info.Encode.Ref);
		}
		sw.WriteLine("/ID[<{0}><{0}>]", IDValueString);
		sw.WriteLine(">>");
		sw.WriteLine("%Stimulsoft" + (StiOptions.Configuration.IsWPF ? "Wpf" : "Net") + (StiOptions.Configuration.IsWeb ? "Web" : ""));
		sw.WriteLine("%2022.1.1.0");
		sw.WriteLine("startxref");
		sw.WriteLine("{0}", position);
		sw.WriteLine("%%EOF");
	}

	private void RenderPageHeader(int pageNumber)
	{
		AddXref(info.PageList[pageNumber].Content.Ref);
		sw.WriteLine("{0} 0 obj", info.PageList[pageNumber].Content.Ref);
		sw.WriteLine("<<");
		memoryPageStream = new MemoryStream();
		pageStream = new StreamWriter(memoryPageStream);
		pageStream.NewLine = "\r\n";
		pageStream.WriteLine("2 J");
		lastColorNonStrokeA = byte.MaxValue;
		lastColorStrokeA = byte.MaxValue;
	}

	private void RenderPageFooter(double pageH, double pageW)
	{
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag = true;
		}
		if (!flag)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag = true;
			}
		}
		if (flag)
		{
			double num = pageW / 596.0 * 1.4;
			double num2 = pageH / 840.0 * 1.4;
			if (num > num2)
			{
				num = num2;
			}
			else
			{
				num2 = num;
			}
			pageStream.WriteLine("q");
			PushColorToStack();
			pageStream.WriteLine("1 J 1 j 20 w");
			SetStrokeColor(Color.FromArgb(64, 100, 100, 100));
			pageStream.WriteLine("{0} 0 0 {1} {2} {3} cm 0.707 0.707 -0.707 0.707 0 0 cm 1 0 0 1 -155 -50 cm", ConvertToString(num), ConvertToString(num2), ConvertToString(pageW / 2.0), ConvertToString(pageH / 2.0));
			pageStream.WriteLine("40 0 m 40 100 l 0 100 m 80 100 l S");
			pageStream.WriteLine("100 0 m 100 70 l 100 45 m 120 65 l 130 72 l 140 68 l S");
			pageStream.WriteLine("170 0 m 170 70 l 169 100 m 171 100 l S");
			pageStream.WriteLine("215 60 m 222 69 l 232 71 l 255 70 l 265 60 l 265 5 l 270 0 l 265 44 m 220 31 l 212 20 l 212 10 l 225 0 l 235 0 l 250 5 l 265 18 l S");
			pageStream.WriteLine("310 0 m 310 100 l S");
			pageStream.WriteLine("Q");
			PopColorFromStack();
		}
		pageStream.Flush();
		if (memoryPageStream.Position < memoryPageStream.Length)
		{
			memoryPageStream.SetLength(memoryPageStream.Position);
		}
		if (compressed)
		{
			byte[] data = memoryPageStream.ToArray();
			MemoryStream mem = StiExportUtils.MakePdfDeflateStream(data);
			StoreMemoryStream2(mem, "/Filter [/FlateDecode] /Length {0}");
		}
		else
		{
			StoreMemoryStream2(memoryPageStream, "/Filter [] /Length {0}");
		}
		sw.WriteLine("");
		pageStream.Close();
		memoryPageStream.Close();
		sw.WriteLine("endstream");
		sw.WriteLine("endobj");
		sw.WriteLine("");
	}

	private void RenderFontTable()
	{
		for (int i = 0; i < fontsCounter; i++)
		{
			StringBuilder stringBuilder = new StringBuilder();
			pdfFont.CurrentFont = i;
			PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[i];
			string text = pfontInfo.Name;
			if (StiOptions.Export.UseAlternativeFontNames && StiOptions.Export.AlternativeFontNames.ContainsKey(text))
			{
				text = (string)StiOptions.Export.AlternativeFontNames[text];
			}
			bool flag = embeddedFonts || PdfFonts.IsFontStimulsoft(pfontInfo.Name);
			bool flag2 = PdfFonts.IsFontSymbolic(text);
			if (pdfFont.UseUnicode)
			{
				stringBuilder = new StringBuilder(CorrectNameEnconding(text));
				if (pfontInfo.Bold || pfontInfo.Italic)
				{
					stringBuilder.Append(",");
					if (pfontInfo.Bold)
					{
						stringBuilder.Append("Bold");
					}
					if (pfontInfo.Italic)
					{
						stringBuilder.Append("Italic");
					}
				}
				if (flag)
				{
					Random random = new Random();
					stringBuilder.Insert(0, $"SR{(char)random.Next(65, 90)}{(char)random.Next(65, 90)}{(char)random.Next(65, 90)}{(char)random.Next(65, 90)}+");
				}
				AddXref(info.FontList[i].Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Font");
				sw.WriteLine("/Subtype /Type0");
				sw.WriteLine("/BaseFont /{0}", stringBuilder);
				sw.WriteLine("/DescendantFonts [{0} 0 R]", info.FontList[i].DescendantFont.Ref);
				sw.WriteLine("/Encoding /Identity-H");
				sw.WriteLine("/ToUnicode {0} 0 R", info.FontList[i].ToUnicode.Ref);
				sw.WriteLine("/Name /F{0}", i);
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(info.FontList[i].DescendantFont.Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].DescendantFont.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Font");
				sw.WriteLine("/Subtype /CIDFontType2");
				sw.WriteLine("/BaseFont /{0}", stringBuilder);
				sw.WriteLine("/CIDSystemInfo");
				sw.WriteLine("<<");
				StoreStringLine("/Registry", "Adobe");
				StoreStringLine("/Ordering", "Identity");
				sw.WriteLine("/Supplement 0");
				sw.WriteLine(">>");
				sw.WriteLine("/FontDescriptor {0} 0 R", info.FontList[i].FontDescriptor.Ref);
				if (usePdfA)
				{
					sw.WriteLine("/CIDToGIDMap /Identity");
				}
				sw.WriteLine("/W [0 [1000]");
				ushort[] glyphList = pdfFont.GlyphList;
				int num = glyphList.Length;
				int[] array = new int[num];
				int[] array2 = new int[num];
				for (int j = 32; j < num; j++)
				{
					array[j] = glyphList[j];
					array2[j] = j;
				}
				for (int k = 32; k < array.Length - 1; k++)
				{
					for (int l = k + 1; l < array.Length; l++)
					{
						if (array[k] > array[l])
						{
							int num2 = array[k];
							array[k] = array[l];
							array[l] = num2;
							int num3 = array2[k];
							array2[k] = array2[l];
							array2[l] = num3;
						}
					}
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				int m = 32;
				while (m < pdfFont.MappedSymbolsCount)
				{
					stringBuilder2.Append(array[m] + " [");
					stringBuilder2.Append(pdfFont.Widths[array2[m] - 32].ToString());
					for (m++; m < pdfFont.MappedSymbolsCount && array[m] - 1 == array[m - 1]; m++)
					{
						stringBuilder2.Append(" " + pdfFont.Widths[array2[m] - 32]);
					}
					for (; m < pdfFont.MappedSymbolsCount && array[m] == array[m - 1]; m++)
					{
					}
					stringBuilder2.Append("]");
					sw.WriteLine("{0}", stringBuilder2);
					stringBuilder2 = new StringBuilder();
				}
				if (pdfFont.GlyphWidths != null && pdfFont.GlyphWidths[65535] != 0)
				{
					for (int n = 0; n < 65535; n++)
					{
						if (pdfFont.GlyphWidths[n] > 0 && pdfFont.GlyphBackList[n] == 0)
						{
							sw.WriteLine($"{n} [{pdfFont.GlyphWidths[n]}]");
						}
					}
				}
				sw.WriteLine("]");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(info.FontList[i].ToUnicode.Ref);
				string arg = "SR+F" + i;
				sw.WriteLine("{0} 0 obj", info.FontList[i].ToUnicode.Ref);
				sw.WriteLine("<<");
				MemoryStream memoryStream = new MemoryStream();
				StreamWriter streamWriter = new StreamWriter(memoryStream);
				streamWriter.NewLine = "\r\n";
				streamWriter.WriteLine("/CIDInit /ProcSet findresource begin");
				streamWriter.WriteLine("12 dict begin");
				streamWriter.WriteLine("begincmap");
				streamWriter.WriteLine("/CIDSystemInfo");
				streamWriter.WriteLine("<<");
				streamWriter.WriteLine("/Registry (Adobe)");
				streamWriter.WriteLine("/Ordering ({0})", arg);
				streamWriter.WriteLine("/Supplement 0");
				streamWriter.WriteLine(">> def");
				streamWriter.WriteLine("/CMapName /{0} def", arg);
				streamWriter.WriteLine("/CMapType 2 def");
				streamWriter.WriteLine("1 begincodespacerange");
				streamWriter.WriteLine("<0000> <FFFF>");
				streamWriter.WriteLine("endcodespacerange");
				int num4 = pdfFont.MappedSymbolsCount - 32;
				int num5 = 32;
				while (num4 > 0)
				{
					int num6 = num4;
					if (num6 > 100)
					{
						num6 = 100;
					}
					streamWriter.WriteLine("{0} beginbfchar", num6);
					for (int num7 = 0; num7 < num6; num7++)
					{
						streamWriter.WriteLine("<{0:X4}> <{1:X4}>", pdfFont.GlyphList[num5], pdfFont.UnicodeMapBack[num5]);
						num5++;
					}
					streamWriter.WriteLine("endbfchar");
					num4 -= num6;
				}
				streamWriter.WriteLine("endcmap");
				streamWriter.WriteLine("CMapName currentdict /CMap defineresource pop");
				streamWriter.WriteLine("end");
				streamWriter.WriteLine("end");
				streamWriter.Flush();
				byte[] array3 = memoryStream.ToArray();
				memoryStream.Close();
				if (compressed)
				{
					MemoryStream mem = StiExportUtils.MakePdfDeflateStream(array3);
					StoreMemoryStream2(mem, "/Length {0} /Filter [/FlateDecode] /Length1 " + array3.Length);
				}
				else
				{
					StoreMemoryStream2(array3, "/Length {0} /Filter [] /Length1 " + array3.Length);
				}
				sw.WriteLine("");
				sw.WriteLine("endstream");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(info.FontList[i].CIDSet.Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].CIDSet.Ref);
				sw.WriteLine("<<");
				BitArray bitArray = new BitArray(65568, defaultValue: false);
				int num8 = 0;
				for (int num9 = 0; num9 < pdfFont.GlyphList.Length; num9++)
				{
					ushort num10 = pdfFont.GlyphList[num9];
					if (num10 != ushort.MaxValue)
					{
						bitArray[num10] = true;
						if (num10 > num8)
						{
							num8 = num10;
						}
					}
				}
				if (pdfFont.GlyphWidths != null && pdfFont.GlyphWidths[65535] != 0)
				{
					for (int num11 = 0; num11 < 65535; num11++)
					{
						if (pdfFont.GlyphWidths[num11] > 0 && pdfFont.GlyphBackList[num11] == 0)
						{
							bitArray[num11] = true;
							if (num11 > num8)
							{
								num8 = num11;
							}
						}
					}
				}
				int num12 = num8 / 8 + 1;
				byte[] array4 = new byte[num12 + 1];
				for (int num13 = 0; num13 < num12; num13++)
				{
					int num14 = num13 * 8;
					byte b = 0;
					if (bitArray[num14])
					{
						b = (byte)(b | 0x80u);
					}
					if (bitArray[num14 + 1])
					{
						b = (byte)(b | 0x40u);
					}
					if (bitArray[num14 + 2])
					{
						b = (byte)(b | 0x20u);
					}
					if (bitArray[num14 + 3])
					{
						b = (byte)(b | 0x10u);
					}
					if (bitArray[num14 + 4])
					{
						b = (byte)(b | 8u);
					}
					if (bitArray[num14 + 5])
					{
						b = (byte)(b | 4u);
					}
					if (bitArray[num14 + 6])
					{
						b = (byte)(b | 2u);
					}
					if (bitArray[num14 + 7])
					{
						b = (byte)(b | 1u);
					}
					array4[num13] = b;
				}
				if (compressed)
				{
					MemoryStream mem2 = StiExportUtils.MakePdfDeflateStream(array4);
					StoreMemoryStream2(mem2, "/Length {0} /Filter [/FlateDecode] /Length1 " + array4.Length);
				}
				else
				{
					StoreMemoryStream2(array4, "/Length {0} /Filter [] /Length1 " + array4.Length);
				}
				sw.WriteLine("");
				sw.WriteLine("endstream");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
			else
			{
				AddXref(info.FontList[i].Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Font");
				if (standardPdfFonts)
				{
					sw.WriteLine("/Subtype /Type1");
					sw.WriteLine("/BaseFont /{0}", pfontInfo.PdfName);
				}
				else
				{
					sw.WriteLine("/Subtype /TrueType");
					stringBuilder = new StringBuilder(CorrectNameEnconding(text));
					if (pfontInfo.Bold || pfontInfo.Italic)
					{
						stringBuilder.Append(",");
						if (pfontInfo.Bold)
						{
							stringBuilder.Append("Bold");
						}
						if (pfontInfo.Italic)
						{
							stringBuilder.Append("Italic");
						}
					}
					sw.WriteLine("/BaseFont /{0}", stringBuilder);
					sw.WriteLine("/FontDescriptor {0} 0 R", info.FontList[i].FontDescriptor.Ref);
				}
				sw.WriteLine("/Encoding {0} 0 R", info.FontList[i].Encoding.Ref);
				StringBuilder stringBuilder3 = new StringBuilder(" ");
				if (flag2)
				{
					for (int num15 = 32; num15 < 255; num15++)
					{
						int num16 = pdfFont.UnicodeMap[num15 + 61440];
						stringBuilder3.Append(((num16 > 0) ? pdfFont.Widths[num16 - 32] : 1000) + " ");
					}
				}
				else
				{
					for (int num17 = 32; num17 < pdfFont.MappedSymbolsCount; num17++)
					{
						stringBuilder3.Append(pdfFont.Widths[num17 - 32] + " ");
					}
				}
				sw.WriteLine("/FirstChar {0}", 32);
				sw.WriteLine("/LastChar {0}", flag2 ? 254 : (pdfFont.MappedSymbolsCount - 1));
				sw.WriteLine("/Widths [{0}]", stringBuilder3);
				sw.WriteLine("/Name /F{0}", i);
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(info.FontList[i].Encoding.Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].Encoding.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Encoding");
				sw.WriteLine("/BaseEncoding /WinAnsiEncoding");
				if (pdfFont.MappedSymbolsCount > 64 && !flag2)
				{
					StringBuilder stringBuilder4 = new StringBuilder((ushort)64 + " ");
					for (int num18 = 64; num18 < pdfFont.MappedSymbolsCount; num18++)
					{
						stringBuilder4.Append("/" + pdfFont.CharPdfNames[num18]);
					}
					sw.WriteLine("/Differences [{0}]", stringBuilder4);
				}
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
			byte[] buff = null;
			if (flag)
			{
				pdfFont.GetFontDataBuf(pfontInfo.Font, out buff, pdfFont.GlyphBackList == null);
			}
			if (!standardPdfFonts)
			{
				AddXref(info.FontList[i].FontDescriptor.Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].FontDescriptor.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /FontDescriptor");
				sw.WriteLine("/FontName /{0}", stringBuilder);
				sw.WriteLine("/Flags {0}", flag2 ? 4 : 32);
				sw.WriteLine("/Ascent {0}", pdfFont.tmASC);
				sw.WriteLine("/CapHeight {0}", pdfFont.CH);
				sw.WriteLine("/Descent {0}", pdfFont.tmDESC);
				sw.WriteLine("/FontBBox [{0} {1} {2} {3}]", pdfFont.LLX, pdfFont.LLY, pdfFont.URX, pdfFont.URY);
				sw.WriteLine("/ItalicAngle {0}", pdfFont.ItalicAngle);
				sw.WriteLine("/StemV {0}", pdfFont.StemV);
				if (flag && buff != null && buff.Length != 0)
				{
					sw.WriteLine("/FontFile2 {0} 0 R", info.FontList[(pfontInfo.ParentFontNumber == -1) ? i : pfontInfo.ParentFontNumber].FontFile2.Ref);
				}
				bool flag3 = pdfFont.UseUnicode;
				if (pdfComplianceMode == StiPdfComplianceMode.A2 || pdfComplianceMode == StiPdfComplianceMode.A3)
				{
					flag3 = false;
				}
				if (flag3)
				{
					sw.WriteLine("/CIDSet {0} 0 R", info.FontList[i].CIDSet.Ref);
				}
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
			if (!flag)
			{
				continue;
			}
			if (buff != null && buff.Length != 0 && pfontInfo.ParentFontNumber == -1)
			{
				if (reduceFontSize)
				{
					pdfFont.OpenTypeHelper.ReduceFontSize(ref buff, pfontInfo.Name, pdfFont.UseUnicode && !fontGlyphsReduceNotNeed[i], pdfFont.GlyphList, pdfFont.GlyphRtfList);
				}
				AddXref(info.FontList[i].FontFile2.Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].FontFile2.Ref);
				sw.WriteLine("<<");
				if (compressedFonts)
				{
					MemoryStream mem3 = StiExportUtils.MakePdfDeflateStream(buff);
					StoreMemoryStream2(mem3, "/Length {0} /Filter [/FlateDecode] /Length1 " + buff.Length);
				}
				else
				{
					StoreMemoryStream2(buff, "/Length {0} /Filter [] /Length1 " + buff.Length);
				}
				sw.WriteLine("");
				sw.WriteLine("endstream");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
			else
			{
				AddXref(info.FontList[i].FontFile2.Ref);
				sw.WriteLine("{0} 0 obj", info.FontList[i].FontFile2.Ref);
				sw.WriteLine("<< >>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
		}
	}

	private void RenderImageTable()
	{
		if (imagesCounter <= 0)
		{
			return;
		}
		for (int i = 0; i < imagesCounter; i++)
		{
			StiImageData stiImageData = (StiImageData)imageCacheIndexToList[i];
			byte[] array = (byte[])imageCache.ImagePackedStore[i];
			byte[] array2 = (byte[])imageCache.ImageMaskStore[i];
			Color[] array3 = (Color[])imageCache.ImagePaletteStore[i];
			StiImageFormat stiImageFormat = stiImageData.ImageFormat;
			AddXref(info.XObjectList[i].Ref);
			sw.WriteLine("{0} 0 obj", info.XObjectList[i].Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /XObject");
			sw.WriteLine("/Subtype /Image");
			if (imageCompressionMethod == StiPdfImageCompressionMethod.Indexed && stiImageFormat != StiImageFormat.Monochrome)
			{
				if (encrypted)
				{
					byte[] array4 = new byte[array3.Length * 3];
					int num = 0;
					Color[] array5 = array3;
					for (int j = 0; j < array5.Length; j++)
					{
						Color color = array5[j];
						array4[num++] = color.R;
						array4[num++] = color.G;
						array4[num++] = color.B;
					}
					byte[] array6 = pdfSecurity.EncryptData(array4, currentObjectNumber, currentGenerationNumber);
					StringBuilder stringBuilder = new StringBuilder();
					for (int k = 0; k < array6.Length; k++)
					{
						stringBuilder.Append(array6[k].ToString("X2"));
					}
					sw.WriteLine("/ColorSpace [/Indexed /DeviceRGB {0} <{1}> ]", array3.Length - 1, stringBuilder.ToString());
				}
				else
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					Color[] array7 = array3;
					for (int l = 0; l < array7.Length; l++)
					{
						Color color2 = array7[l];
						stringBuilder2.AppendFormat("{0:X2}{1:X2}{2:X2} ", color2.R, color2.G, color2.B);
					}
					sw.WriteLine("/ColorSpace [/Indexed /DeviceRGB {0} <{1}> ]", array3.Length - 1, stringBuilder2);
				}
			}
			else
			{
				sw.WriteLine("/ColorSpace /{0}", (stiImageFormat == StiImageFormat.Monochrome) ? "DeviceGray" : "DeviceRGB");
			}
			sw.WriteLine("/Width {0}", stiImageData.Width);
			sw.WriteLine("/Height {0}", stiImageData.Height);
			sw.WriteLine("/BitsPerComponent {0}", (stiImageFormat == StiImageFormat.Monochrome) ? "1" : "8");
			if (!usePdfA && imageInterpolationTable.ContainsKey(i))
			{
				sw.WriteLine("/Interpolate true");
			}
			if (array2 != null)
			{
				sw.WriteLine("/SMask {0} 0 R", info.XObjectList[i].Mask.Ref);
			}
			sw.WriteLine("/Name /{0}", stiImageData.Name);
			string empty = string.Empty;
			StiPdfImageCompressionMethod stiPdfImageCompressionMethod = imageCompressionMethod;
			empty = (((uint)(stiPdfImageCompressionMethod - 2) > 1u) ? "DCTDecode" : "FlateDecode");
			string dictionaryString = "/Length {0} " + $"/Filter [/{empty}]";
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(array, 0, array.Length);
			StoreMemoryStream2(memoryStream, dictionaryString);
			sw.WriteLine("");
			sw.WriteLine("endstream");
			sw.WriteLine("endobj");
			sw.WriteLine("");
			if (array2 != null)
			{
				AddXref(info.XObjectList[i].Mask.Ref);
				sw.WriteLine("{0} 0 obj", info.XObjectList[i].Mask.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /XObject");
				sw.WriteLine("/Subtype /Image");
				sw.WriteLine("/ColorSpace /DeviceGray");
				sw.WriteLine("/Width {0}", stiImageData.Width);
				sw.WriteLine("/Height {0}", stiImageData.Height);
				sw.WriteLine("/BitsPerComponent 8");
				if (!usePdfA && imageInterpolationTable.ContainsKey(i))
				{
					sw.WriteLine("/Interpolate true");
				}
				string dictionaryString2 = "/Length {0} /Filter /FlateDecode";
				MemoryStream memoryStream2 = new MemoryStream();
				memoryStream2.Write(array2, 0, array2.Length);
				StoreMemoryStream2(memoryStream2, dictionaryString2);
				sw.WriteLine("");
				sw.WriteLine("endstream");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
		}
	}

	private void RenderBookmarksTable()
	{
		if (!haveBookmarks)
		{
			return;
		}
		AddXref(info.Outlines.Ref);
		sw.WriteLine("{0} 0 obj", info.Outlines.Ref);
		sw.WriteLine("<<");
		sw.WriteLine("/Type /Outlines");
		sw.WriteLine("/First {0} 0 R", info.Outlines.Items[0].Ref);
		sw.WriteLine("/Last {0} 0 R", info.Outlines.Items[0].Ref);
		sw.WriteLine("/Count {0}", 1);
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		for (int i = 0; i < bookmarksCounter; i++)
		{
			AddXref(info.Outlines.Items[i].Ref);
			StiBookmarkTreeNode stiBookmarkTreeNode = (StiBookmarkTreeNode)bookmarksTree[i];
			sw.WriteLine("{0} 0 obj", info.Outlines.Items[i].Ref);
			sw.WriteLine("<<");
			StoreStringLine("/Title ", stiBookmarkTreeNode.Title, escaping: true);
			if (stiBookmarkTreeNode.Parent != -1)
			{
				sw.WriteLine("/Parent {0} 0 R", info.Outlines.Items[stiBookmarkTreeNode.Parent].Ref);
			}
			if (stiBookmarkTreeNode.Prev != -1)
			{
				sw.WriteLine("/Prev {0} 0 R", info.Outlines.Items[stiBookmarkTreeNode.Prev].Ref);
			}
			if (stiBookmarkTreeNode.Next != -1)
			{
				sw.WriteLine("/Next {0} 0 R", info.Outlines.Items[stiBookmarkTreeNode.Next].Ref);
			}
			if (stiBookmarkTreeNode.First != -1)
			{
				sw.WriteLine("/First {0} 0 R", info.Outlines.Items[stiBookmarkTreeNode.First].Ref);
			}
			if (stiBookmarkTreeNode.Last != -1)
			{
				sw.WriteLine("/Last {0} 0 R", info.Outlines.Items[stiBookmarkTreeNode.Last].Ref);
			}
			if (stiBookmarkTreeNode.Count > 0)
			{
				if (i == 0)
				{
					sw.WriteLine("/Count {0}", stiBookmarkTreeNode.Count);
				}
				else
				{
					sw.WriteLine("/Count {0}", -stiBookmarkTreeNode.Count);
				}
			}
			if (stiBookmarkTreeNode.Y > -1.0)
			{
				sw.WriteLine("/Dest [{0} 0 R /XYZ null {1} null]", info.PageList[stiBookmarkTreeNode.Page].Ref, ConvertToString(stiBookmarkTreeNode.Y));
			}
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderPatternTable()
	{
		AddXref(info.Patterns.Resources.Ref);
		sw.WriteLine("{0} 0 obj", info.Patterns.Resources.Ref);
		sw.WriteLine("<< /ProcSet [/PDF] >>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(info.Patterns.First.Ref);
		sw.WriteLine("{0} 0 obj", info.Patterns.First.Ref);
		sw.WriteLine("<<");
		sw.WriteLine("/Type /Pattern");
		sw.WriteLine("/PatternType 1");
		sw.WriteLine("/PaintType 1");
		sw.WriteLine("/TilingType 1");
		sw.WriteLine("/BBox [0 0 2 2]");
		sw.WriteLine("/XStep 3");
		sw.WriteLine("/YStep 3");
		sw.WriteLine("/Resources {0} 0 R", info.Patterns.Resources.Ref);
		memoryPageStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryPageStream);
		streamWriter.NewLine = "\r\n";
		streamWriter.WriteLine("1 J 1 j 1 w");
		streamWriter.WriteLine("1 0 0 RG");
		streamWriter.Write("1 1 m 1.1 1.1 l S");
		streamWriter.Flush();
		StoreMemoryStream2(memoryPageStream, "/Length {0}");
		sw.WriteLine("");
		sw.WriteLine("endstream");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		if (hatchCounter > 0)
		{
			for (int i = 0; i < hatchCounter; i++)
			{
				WriteHatchPattern(i);
			}
		}
		if (shadingCounter > 0)
		{
			for (int j = 0; j < shadingCounter; j++)
			{
				WriteShadingPattern(j);
			}
			sw.WriteLine("");
		}
		if (shadingFunctionCounter > 0)
		{
			for (int k = 0; k < shadingFunctionCounter; k++)
			{
				WriteShadingFunction(k);
			}
			sw.WriteLine("");
		}
		memoryPageStream.Close();
	}

	private void WriteHatchPattern(int indexHatch)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected I4, but got Unknown
		StiHatchBrush val = (StiHatchBrush)hatchArray[indexHatch];
		int num = (int)val.Style;
		if (num > 53)
		{
			num = 53;
		}
		AddXref(info.Patterns.HatchItems[indexHatch].Ref);
		sw.WriteLine("{0} 0 obj", info.Patterns.HatchItems[indexHatch].Ref);
		sw.WriteLine("<<");
		sw.WriteLine("/Type /Pattern");
		sw.WriteLine("/PatternType 1");
		sw.WriteLine("/PaintType 1");
		sw.WriteLine("/TilingType 1");
		sw.WriteLine("/BBox [0 0 1 1]");
		sw.WriteLine("/XStep 1");
		sw.WriteLine("/YStep 1");
		sw.WriteLine("/Resources {0} 0 R", info.Patterns.Resources.Ref);
		sw.WriteLine("/Matrix [5.5 0 0 5.5 0 0]");
		memoryPageStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryPageStream);
		streamWriter.NewLine = "\r\n";
		if (val.BackColor.A != 0)
		{
			streamWriter.WriteLine("{0} {1} {2} rg", colorTable[val.BackColor.R], colorTable[val.BackColor.G], colorTable[val.BackColor.B]);
			streamWriter.WriteLine("0 0 1 1 re f");
		}
		streamWriter.WriteLine("{0} {1} {2} rg", colorTable[val.ForeColor.R], colorTable[val.ForeColor.G], colorTable[val.ForeColor.B]);
		streamWriter.WriteLine("BI");
		streamWriter.WriteLine("/W 8");
		streamWriter.WriteLine("/H 8");
		streamWriter.WriteLine("/BPC 1");
		streamWriter.WriteLine("/IM true");
		streamWriter.WriteLine("/D [1 0]");
		streamWriter.WriteLine("/F [/AHx]");
		streamWriter.WriteLine("ID");
		streamWriter.WriteLine(hatchData[num] + ">");
		streamWriter.WriteLine("EI");
		streamWriter.Flush();
		StoreMemoryStream2(memoryPageStream, "/Length {0}");
		sw.WriteLine("endstream");
		sw.WriteLine("endobj");
		sw.WriteLine("");
	}

	private void WriteShadingPattern(int indexShading)
	{
		StiShadingData stiShadingData = (StiShadingData)shadingArray[indexShading];
		double num = 1.0;
		double num2 = 1.0;
		double num3 = stiShadingData.Angle;
		if (num3 < 0.0)
		{
			num3 += 360.0;
		}
		if (num3 >= 270.0 && num3 < 360.0)
		{
			num3 = 360.0 - num3;
			num2 = -1.0;
		}
		if (num3 >= 180.0 && num3 < 270.0)
		{
			num3 -= 180.0;
			num2 = -1.0;
			num = -1.0;
		}
		if (num3 >= 90.0 && num3 < 180.0)
		{
			num3 = 180.0 - num3;
			num = -1.0;
		}
		num3 = num3 * Math.PI / 180.0;
		double num4 = stiShadingData.X + stiShadingData.Width / 2.0;
		double num5 = stiShadingData.Y + stiShadingData.Height / 2.0;
		double num6 = Math.Sqrt(stiShadingData.Width * stiShadingData.Width + stiShadingData.Height * stiShadingData.Height) / 2.0;
		double num7 = Math.Atan2(stiShadingData.Height, stiShadingData.Width);
		double a = Math.PI / 2.0 - num3 + num7;
		double num8 = num6 * Math.Sin(a);
		double num9 = num8 * Math.Cos(num3) * num;
		double num10 = num8 * Math.Sin(num3) * num2;
		double value = num4 - num9;
		double value2 = num4 + num9;
		double value3 = num5 + num10;
		double value4 = num5 - num10;
		AddXref(info.Patterns.ShadingItems[indexShading].Ref);
		sw.WriteLine("{0} 0 obj", info.Patterns.ShadingItems[indexShading].Ref);
		sw.Write("<<");
		sw.Write("/PatternType 2");
		sw.Write("/Shading <<");
		sw.Write("/ColorSpace /DeviceRGB");
		sw.Write("/ShadingType 2");
		sw.Write("/Coords [{0} {1} {2} {3}]", ConvertToString(value), ConvertToString(value3), ConvertToString(value2), ConvertToString(value4));
		sw.Write("/Extend [true true]");
		sw.Write("/Function {0} 0 R", info.Patterns.ShadingFunctionItems[stiShadingData.FunctionIndex].Ref);
		sw.Write(">> ");
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
	}

	private void WriteShadingFunction(int indexShadingFunction)
	{
		StiShadingFunctionData stiShadingFunctionData = shadingFunctionArray[indexShadingFunction];
		AddXref(info.Patterns.ShadingFunctionItems[indexShadingFunction].Ref);
		sw.WriteLine("{0} 0 obj", info.Patterns.ShadingFunctionItems[indexShadingFunction].Ref);
		sw.Write("<<");
		sw.Write("/FunctionType 0");
		sw.Write("/Size [3]");
		sw.Write("/Decode [0 1 0 1 0 1]");
		sw.Write("/Range [0 1 0 1 0 1]");
		sw.Write("/BitsPerSample 8");
		sw.Write("/Domain [0 1]");
		sw.Write("/Encode [0 {0}]", (!stiShadingFunctionData.IsGlare) ? 1 : 2);
		sw.Write("/Order 1");
		memoryPageStream = new MemoryStream();
		memoryPageStream.WriteByte(stiShadingFunctionData.Color1.R);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color1.G);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color1.B);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color2.R);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color2.G);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color2.B);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color1.R);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color1.G);
		memoryPageStream.WriteByte(stiShadingFunctionData.Color1.B);
		string dictionaryString = "/Length {0}";
		StoreMemoryStream2(memoryPageStream, dictionaryString);
		sw.WriteLine("");
		sw.WriteLine("endstream");
		sw.WriteLine("endobj");
	}

	private void RenderLinkTable()
	{
		if (!haveLinks)
		{
			return;
		}
		for (int i = 0; i < linksCounter; i++)
		{
			StiLinkObject stiLinkObject = (StiLinkObject)linksArray[i];
			if (stiLinkObject.Link.StartsWith("##") && tagsArray.Count > 0)
			{
				string text = stiLinkObject.Link.Substring(2);
				for (int j = 0; j < tagsArray.Count; j++)
				{
					StiLinkObject stiLinkObject2 = tagsArray[j];
					if (stiLinkObject2.Link == text)
					{
						stiLinkObject.DestPage = stiLinkObject2.Page;
						stiLinkObject.DestY = stiLinkObject2.Y + stiLinkObject2.Height;
						break;
					}
				}
			}
			else if (stiLinkObject.Link[0] == '#' && haveBookmarks)
			{
				string text2 = stiLinkObject.Link.Substring(1);
				if (text2.Length > 2 && text2[0] == '%')
				{
					int num = text2.LastIndexOf(text2[1]);
					if (num < text2.Length - 1)
					{
						text2 = text2.Substring(num + 1);
					}
				}
				for (int k = 0; k < bookmarksCounter; k++)
				{
					StiBookmarkTreeNode stiBookmarkTreeNode = (StiBookmarkTreeNode)bookmarksTree[k];
					if (stiBookmarkTreeNode.Title == text2)
					{
						stiLinkObject.DestPage = stiBookmarkTreeNode.Page;
						stiLinkObject.DestY = stiBookmarkTreeNode.Y;
						break;
					}
				}
			}
			linksArray[i] = stiLinkObject;
			AddXref(info.LinkList[i].Ref);
			sw.WriteLine("{0} 0 obj", info.LinkList[i].Ref);
			sw.WriteLine("<<");
			sw.Write("/Type /Annot ");
			sw.WriteLine("/Subtype /Link");
			sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiLinkObject.X), ConvertToString(stiLinkObject.Y), ConvertToString(stiLinkObject.X + stiLinkObject.Width), ConvertToString(stiLinkObject.Y + stiLinkObject.Height));
			sw.WriteLine("/Border [0 0 0]");
			if (stiLinkObject.Link[0] == '#')
			{
				if (stiLinkObject.DestY > -1.0)
				{
					sw.WriteLine("/Dest [{0} 0 R /XYZ null {1} null]", info.PageList[stiLinkObject.DestPage].Ref, ConvertToString(stiLinkObject.DestY));
				}
			}
			else
			{
				string text3 = stiLinkObject.Link.Replace('\\', '/');
				bool flag = text3.Length > 6 && ((char.IsLetter(text3, 0) && text3[1] == ':' && text3[2] == '/') || (text3[0] == '/' && char.IsLetter(text3, 1) && text3[2] == '/'));
				if (flag && text3[1] == ':')
				{
					text3 = "/" + text3[0] + text3.Substring(2);
				}
				sw.Write("/A << ");
				sw.Write("/Type /Action ");
				if (flag)
				{
					sw.Write("/S /Launch ");
					sw.Write("/F << ");
					sw.Write("/Type /Filespec ");
					StoreStringLine("/F ", text3);
					StoreStringLine("/UF ", text3);
					sw.WriteLine(">>");
				}
				else
				{
					sw.Write("/S /URI ");
					StoreStringLine("/URI ", StiExportUtils.StringToUrl(text3));
				}
				sw.WriteLine(">>");
			}
			sw.WriteLine("/F 4");
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderAnnotTable()
	{
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Invalid comparison between Unknown and I4
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Invalid comparison between Unknown and I4
		if (!haveAnnots && !haveDigitalSignature && !haveTooltips)
		{
			return;
		}
		AddXref(info.AcroForm.Ref);
		sw.WriteLine("{0} 0 obj", info.AcroForm.Ref);
		sw.WriteLine("<<");
		sw.WriteLine("/Fields [");
		for (int i = 0; i < annotsCounter; i++)
		{
			sw.WriteLine("{0} 0 R", info.AcroForm.Annots[i].Ref);
		}
		for (int j = 0; j < annots2Counter; j++)
		{
			for (int k = 0; k < info.AcroForm.CheckBoxes[j].Count; k++)
			{
				sw.WriteLine("{0} 0 R", info.AcroForm.CheckBoxes[j][k].Ref);
			}
		}
		for (int l = 0; l < unsignedSignaturesCounter; l++)
		{
			sw.WriteLine("{0} 0 R", info.AcroForm.UnsignedSignatures[l].Ref);
		}
		if (haveDigitalSignature)
		{
			sw.WriteLine("{0} 0 R", info.AcroForm.Signatures[0].Ref);
		}
		for (int m = 0; m < tooltipsCounter; m++)
		{
			sw.WriteLine("{0} 0 R", info.AcroForm.Tooltips[m].Ref);
		}
		sw.WriteLine("]");
		sw.WriteLine("/DR <<");
		if (annotsCounter > 0 || annots2Counter > 0)
		{
			sw.WriteLine("/Font <<");
			for (int n = 0; n < fontsCounter; n++)
			{
				sw.WriteLine("/FA{0} {1} 0 R", n, info.AcroForm.AnnotFontItems[n].Ref);
			}
			sw.WriteLine(">>");
		}
		sw.WriteLine(">>");
		if (haveAnnots && fontsCounter > 0)
		{
			StoreStringLine("/DA ", "/FA0 0 Tf 0 g");
		}
		if (haveDigitalSignature)
		{
			sw.WriteLine("/SigFlags 3");
		}
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		if (!haveAnnots)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		for (int num = 0; num < annotsCounter; num++)
		{
			StiEditableObject stiEditableObject = (StiEditableObject)annotsArray[num];
			string text = string.Empty;
			if (StiOptions.Export.Pdf.UseEditableFieldName)
			{
				text = stiEditableObject.Component.Name;
			}
			if (StiOptions.Export.Pdf.UseEditableFieldAlias)
			{
				text = stiEditableObject.Component.Alias;
			}
			if (StiOptions.Export.Pdf.UseEditableFieldTag)
			{
				text = stiEditableObject.Component.TagValue as string;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = $"Field{num}";
			}
			if (hashtable.ContainsKey(text))
			{
				int num2 = 2;
				string text2 = string.Empty;
				while (hashtable.ContainsKey(text + text2))
				{
					text2 = "_" + num2;
					num2++;
				}
				text += text2;
			}
			hashtable.Add(text, text);
			AddXref(info.AcroForm.Annots[num].Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.Annots[num].Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /Annot");
			sw.WriteLine("/Subtype /Widget");
			sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiEditableObject.X), ConvertToString(stiEditableObject.Y), ConvertToString(stiEditableObject.X + stiEditableObject.Width), ConvertToString(stiEditableObject.Y + stiEditableObject.Height));
			sw.WriteLine("/F 4");
			sw.WriteLine("/P {0} 0 R", info.PageList[stiEditableObject.Page].Ref);
			sw.WriteLine("/FT /Tx");
			sw.WriteLine("/BS << /W 0 >>");
			StoreStringLine("/T ", text);
			sw.WriteLine("/MK << /TP 2 >>");
			sw.WriteLine("/H /P");
			sw.WriteLine("/AP << /N {0} 0 R >>", info.AcroForm.Annots[num].AP.Ref);
			int num3 = (stiEditableObject.Multiline ? 4096 : 0);
			if (stiEditableObject.Component is StiText && (stiEditableObject.Component as StiText).TextOptions != null && (stiEditableObject.Component as StiText).TextOptions.LineLimit)
			{
				num3 |= 0x800000;
			}
			sw.WriteLine("/Ff {0}", num3);
			StoreStringLine("/DA ", $"/FA{stiEditableObject.FontNumber} {ConvertToString(stiEditableObject.FontSize, precision_digits_font)} Tf {colorTable[stiEditableObject.FontColor.R]} {colorTable[stiEditableObject.FontColor.G]} {colorTable[stiEditableObject.FontColor.B]} rg");
			StoreStringLine("/V ", stiEditableObject.Text, escaping: true);
			StoreStringLine("/DV ", stiEditableObject.Text, escaping: true);
			sw.WriteLine("/Q {0}", (((int)stiEditableObject.Alignment == 1) ? 1 : 0) + (((int)stiEditableObject.Alignment == 2) ? 2 : 0));
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
			AddXref(info.AcroForm.Annots[num].AP.Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.Annots[num].AP.Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Subtype /Form");
			sw.WriteLine("/BBox [{0} {1} {2} {3}]", ConvertToString(0.0), ConvertToString(0.0), ConvertToString(stiEditableObject.Width), ConvertToString(stiEditableObject.Height));
			sw.WriteLine("/Resources <<");
			sw.WriteLine("/ProcSet [ /PDF /Text ]");
			sw.Write("/Font << ");
			for (int num4 = 0; num4 < fontsCounter; num4++)
			{
				sw.Write($"/F{num4} {info.FontList[num4].Ref} 0 R ");
			}
			sw.WriteLine(">> >>");
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.GetEncoding(1252));
			streamWriter.NewLine = "\r\n";
			streamWriter.WriteLine("/Tx BMC");
			streamWriter.WriteLine("q");
			int num5 = 1;
			streamWriter.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(num5), ConvertToString(num5), ConvertToString(stiEditableObject.Width - (double)num5), ConvertToString(stiEditableObject.Height - (double)num5));
			streamWriter.Flush();
			streamWriter.BaseStream.Write(stiEditableObject.Content, 0, stiEditableObject.Content.Length);
			streamWriter.WriteLine("Q");
			streamWriter.WriteLine("EMC");
			streamWriter.Flush();
			StoreMemoryStream2(memoryStream, "/Filter [] /Length {0}");
			sw.WriteLine("");
			streamWriter.Close();
			memoryStream.Close();
			sw.WriteLine("endstream");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
		for (int num6 = 0; num6 < fontsCounter; num6++)
		{
			pdfFont.CurrentFont = num6;
			PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[num6];
			string text3 = pfontInfo.Name;
			if (StiOptions.Export.UseAlternativeFontNames && StiOptions.Export.AlternativeFontNames.ContainsKey(text3))
			{
				text3 = (string)StiOptions.Export.AlternativeFontNames[text3];
			}
			StringBuilder stringBuilder = new StringBuilder(CorrectNameEnconding(text3));
			if (pfontInfo.Bold || pfontInfo.Italic)
			{
				stringBuilder.Append(",");
				if (pfontInfo.Bold)
				{
					stringBuilder.Append("Bold");
				}
				if (pfontInfo.Italic)
				{
					stringBuilder.Append("Italic");
				}
			}
			AddXref(info.AcroForm.AnnotFontItems[num6].Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.AnnotFontItems[num6].Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /Font");
			sw.WriteLine("/Subtype /TrueType");
			sw.WriteLine("/Name /FA{0}", num6);
			sw.WriteLine("/BaseFont /{0}", stringBuilder);
			sw.WriteLine("/Encoding /WinAnsiEncoding");
			sw.WriteLine("/FontDescriptor {0} 0 R", info.AcroForm.AnnotFontItems[num6].FontDescriptor.Ref);
			if (fontGlyphsReduceNotNeed[num6])
			{
				StringBuilder stringBuilder2 = new StringBuilder(" ");
				for (int num7 = 32; num7 < 256; num7++)
				{
					stringBuilder2.Append(pdfFont.Widths[pdfFont.UnicodeMap[CodePage1252[num7]] - 32] + " ");
				}
				sw.WriteLine("/FirstChar 32");
				sw.WriteLine("/LastChar 255");
				sw.WriteLine("/Widths [{0}]", stringBuilder2);
			}
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
			byte[] buff = null;
			bool flag = embeddedFonts || PdfFonts.IsFontStimulsoft(pfontInfo.Name);
			if (flag)
			{
				pdfFont.GetFontDataBuf(pfontInfo.Font, out buff, pdfFont.GlyphBackList == null);
			}
			AddXref(info.AcroForm.AnnotFontItems[num6].FontDescriptor.Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.AnnotFontItems[num6].FontDescriptor.Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /FontDescriptor");
			sw.WriteLine("/FontName /{0}", stringBuilder);
			sw.WriteLine("/Flags 32");
			sw.WriteLine("/Ascent {0}", pdfFont.MacAscend);
			sw.WriteLine("/CapHeight {0}", pdfFont.CH);
			sw.WriteLine("/Descent {0}", pdfFont.MacDescend);
			sw.WriteLine("/FontBBox [{0} {1} {2} {3}]", pdfFont.LLX, pdfFont.LLY, pdfFont.URX, pdfFont.URY);
			sw.WriteLine("/ItalicAngle {0}", pdfFont.ItalicAngle);
			sw.WriteLine("/StemV {0}", pdfFont.StemV);
			if (flag && buff != null && buff.Length != 0)
			{
				sw.WriteLine("/FontFile2 {0} 0 R", info.FontList[num6].FontFile2.Ref);
			}
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
		for (int num8 = 0; num8 < annots2Counter; num8++)
		{
			StiEditableObject stiEditableObject2 = annots2Array[num8];
			string text4 = string.Empty;
			if (StiOptions.Export.Pdf.UseEditableFieldName)
			{
				text4 = stiEditableObject2.Component.Name;
			}
			if (StiOptions.Export.Pdf.UseEditableFieldAlias)
			{
				text4 = stiEditableObject2.Component.Alias;
			}
			if (StiOptions.Export.Pdf.UseEditableFieldTag)
			{
				text4 = stiEditableObject2.Component.TagValue as string;
			}
			if (string.IsNullOrEmpty(text4))
			{
				text4 = $"Checkbox{num8 + 1}";
			}
			if (hashtable.ContainsKey(text4))
			{
				int num9 = 2;
				string text5 = string.Empty;
				while (hashtable.ContainsKey(text4 + text5))
				{
					text5 = "_" + num9;
					num9++;
				}
				text4 += text5;
			}
			hashtable.Add(text4, text4);
			StiCheckBox stiCheckBox = stiEditableObject2.Component as StiCheckBox;
			bool valueOrDefault = GetCheckBoxValue(stiCheckBox).GetValueOrDefault(false);
			if (info.AcroForm.CheckBoxes[num8].Count > 1)
			{
				StiPdfStructure.StiPdfAnnotObjInfo stiPdfAnnotObjInfo = info.AcroForm.CheckBoxes[num8][0];
				AddXref(stiPdfAnnotObjInfo.Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Annot");
				sw.WriteLine("/Subtype /Widget");
				sw.WriteLine("/FT /Btn");
				sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiEditableObject2.X), ConvertToString(stiEditableObject2.Y), ConvertToString(stiEditableObject2.X + stiEditableObject2.Width - 0.01), ConvertToString(stiEditableObject2.Y + stiEditableObject2.Height - 0.01));
				sw.WriteLine("/F {0}", 4 + ((info.AcroForm.CheckBoxes[num8].Count == 3 || !valueOrDefault) ? 2 : 0));
				sw.WriteLine("/P {0} 0 R", info.PageList[stiEditableObject2.Page].Ref);
				StoreStringLine("/T ", text4 + "Yes");
				sw.WriteLine("/MK << >>");
				sw.WriteLine("/Ff 65536");
				sw.WriteLine("/A {0} 0 R", stiPdfAnnotObjInfo.AA[0].Ref);
				sw.WriteLine("/AP << /N {0} 0 R >>", stiPdfAnnotObjInfo.AP.Ref);
				sw.WriteLine("/H /O");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo.AP.Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.AP.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /XObject");
				sw.WriteLine("/Subtype /Form");
				sw.WriteLine("/FormType 1");
				sw.WriteLine("/BBox [{0} {1} {2} {3}]", ConvertToString(0.0), ConvertToString(0.0), ConvertToString(stiEditableObject2.Width), ConvertToString(stiEditableObject2.Height));
				sw.WriteLine("/Matrix [ 1 0 0 1 0 0 ]");
				sw.WriteLine("/Resources <<");
				sw.WriteLine("/ProcSet [ /PDF ]");
				sw.WriteLine("/Pattern");
				sw.WriteLine("<<");
				for (int num10 = 0; num10 < hatchCounter; num10++)
				{
					sw.WriteLine($"/PH{1 + num10} {info.Patterns.HatchItems[num10].Ref} 0 R");
				}
				for (int num11 = 0; num11 < shadingCounter; num11++)
				{
					sw.WriteLine($"/P{2 + num11} {info.Patterns.ShadingItems[num11].Ref} 0 R");
				}
				sw.WriteLine(">>");
				sw.WriteLine("/ColorSpace 3 0 R");
				sw.WriteLine(">>");
				MemoryStream memoryStream2 = new MemoryStream();
				StreamWriter streamWriter2 = new StreamWriter(memoryStream2, Encoding.GetEncoding(1252));
				streamWriter2.NewLine = "\r\n";
				streamWriter2.WriteLine("/Tx BMC");
				streamWriter2.WriteLine("q");
				int num12 = 1;
				streamWriter2.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(num12), ConvertToString(num12), ConvertToString(stiEditableObject2.Width - (double)num12), ConvertToString(stiEditableObject2.Height - (double)num12));
				streamWriter2.Flush();
				streamWriter2.BaseStream.Write(stiEditableObject2.Content, 0, stiEditableObject2.Content.Length);
				streamWriter2.WriteLine("Q");
				streamWriter2.WriteLine("EMC");
				streamWriter2.Flush();
				StoreMemoryStream2(memoryStream2, "/Filter [] /Length {0}");
				sw.WriteLine("");
				streamWriter2.Close();
				memoryStream2.Close();
				sw.WriteLine("endstream");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo.AA[0].Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.AA[0].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Next {0} 0 R", stiPdfAnnotObjInfo.AA[1].Ref);
				sw.WriteLine("/S /Hide");
				sw.WriteLine("/T ({0})", text4 + "Yes");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo.AA[1].Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.AA[1].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/H false");
				sw.WriteLine("/S /Hide");
				sw.WriteLine("/T ({0})", text4 + "No");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				stiPdfAnnotObjInfo = info.AcroForm.CheckBoxes[num8][1];
				AddXref(stiPdfAnnotObjInfo.Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Annot");
				sw.WriteLine("/Subtype /Widget");
				sw.WriteLine("/FT /Btn");
				sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiEditableObject2.X), ConvertToString(stiEditableObject2.Y), ConvertToString(stiEditableObject2.X + stiEditableObject2.Width - 0.01), ConvertToString(stiEditableObject2.Y + stiEditableObject2.Height - 0.01));
				sw.WriteLine("/F {0}", 4 + ((info.AcroForm.CheckBoxes[num8].Count == 3 || valueOrDefault) ? 2 : 0));
				sw.WriteLine("/P {0} 0 R", info.PageList[stiEditableObject2.Page].Ref);
				StoreStringLine("/T ", text4 + "No");
				sw.WriteLine("/MK << >>");
				sw.WriteLine("/Ff 65536");
				sw.WriteLine("/A {0} 0 R", stiPdfAnnotObjInfo.AA[0].Ref);
				sw.WriteLine("/AP << /N {0} 0 R >>", stiPdfAnnotObjInfo.AP.Ref);
				sw.WriteLine("/H /O");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo.AP.Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.AP.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /XObject");
				sw.WriteLine("/Subtype /Form");
				sw.WriteLine("/FormType 1");
				sw.WriteLine("/BBox [{0} {1} {2} {3}]", ConvertToString(0.0), ConvertToString(0.0), ConvertToString(stiEditableObject2.Width), ConvertToString(stiEditableObject2.Height));
				sw.WriteLine("/Matrix [ 1 0 0 1 0 0 ]");
				sw.WriteLine("/Resources <<");
				sw.WriteLine("/ProcSet [ /PDF ]");
				sw.WriteLine("/Pattern");
				sw.WriteLine("<<");
				for (int num13 = 0; num13 < hatchCounter; num13++)
				{
					sw.WriteLine($"/PH{1 + num13} {info.Patterns.HatchItems[num13].Ref} 0 R");
				}
				for (int num14 = 0; num14 < shadingCounter; num14++)
				{
					sw.WriteLine($"/P{2 + num14} {info.Patterns.ShadingItems[num14].Ref} 0 R");
				}
				sw.WriteLine(">>");
				sw.WriteLine("/ColorSpace 3 0 R");
				sw.WriteLine(">>");
				memoryStream2 = new MemoryStream();
				streamWriter2 = new StreamWriter(memoryStream2, Encoding.GetEncoding(1252));
				streamWriter2.NewLine = "\r\n";
				streamWriter2.WriteLine("/Tx BMC");
				streamWriter2.WriteLine("q");
				num12 = 1;
				streamWriter2.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(num12), ConvertToString(num12), ConvertToString(stiEditableObject2.Width - (double)num12), ConvertToString(stiEditableObject2.Height - (double)num12));
				streamWriter2.Flush();
				streamWriter2.BaseStream.Write(stiEditableObject2.Content2, 0, stiEditableObject2.Content2.Length);
				streamWriter2.WriteLine("Q");
				streamWriter2.WriteLine("EMC");
				streamWriter2.Flush();
				StoreMemoryStream2(memoryStream2, "/Filter [] /Length {0}");
				sw.WriteLine("");
				streamWriter2.Close();
				memoryStream2.Close();
				sw.WriteLine("endstream");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo.AA[0].Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.AA[0].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/H false");
				sw.WriteLine("/Next {0} 0 R", stiPdfAnnotObjInfo.AA[1].Ref);
				sw.WriteLine("/S /Hide");
				sw.WriteLine("/T ({0})", text4 + "Yes");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo.AA[1].Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo.AA[1].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/S /Hide");
				sw.WriteLine("/T ({0})", text4 + "No");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
			if (info.AcroForm.CheckBoxes[num8].Count == 3)
			{
				StiPdfStructure.StiPdfAnnotObjInfo stiPdfAnnotObjInfo2 = info.AcroForm.CheckBoxes[num8][2];
				AddXref(stiPdfAnnotObjInfo2.Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo2.Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Type /Annot");
				sw.WriteLine("/Subtype /Widget");
				sw.WriteLine("/FT /Btn");
				sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiEditableObject2.X), ConvertToString(stiEditableObject2.Y), ConvertToString(stiEditableObject2.X + stiEditableObject2.Width - 0.01), ConvertToString(stiEditableObject2.Y + stiEditableObject2.Height - 0.01));
				sw.WriteLine("/F 4");
				sw.WriteLine("/P {0} 0 R", info.PageList[stiEditableObject2.Page].Ref);
				StoreStringLine("/T ", text4 + "None");
				sw.WriteLine("/MK << >>");
				sw.WriteLine("/Ff 65536");
				sw.WriteLine("/A {0} 0 R", stiPdfAnnotObjInfo2.AA[0].Ref);
				sw.WriteLine("/H /O");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo2.AA[0].Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo2.AA[0].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/Next {0} 0 R", stiPdfAnnotObjInfo2.AA[1].Ref);
				sw.WriteLine("/S /Hide");
				sw.WriteLine("/T ({0})", text4 + "None");
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
				AddXref(stiPdfAnnotObjInfo2.AA[1].Ref);
				sw.WriteLine("{0} 0 obj", stiPdfAnnotObjInfo2.AA[1].Ref);
				sw.WriteLine("<<");
				sw.WriteLine("/H false");
				sw.WriteLine("/S /Hide");
				sw.WriteLine("/T ({0})", text4 + ((stiCheckBox.CheckStyleForTrue == StiCheckStyle.None) ? "No" : "Yes"));
				sw.WriteLine(">>");
				sw.WriteLine("endobj");
				sw.WriteLine("");
			}
		}
		for (int num15 = 0; num15 < unsignedSignaturesCounter; num15++)
		{
			StiEditableObject stiEditableObject3 = unsignedSignaturesArray[num15];
			string text6 = string.Empty;
			if (StiOptions.Export.Pdf.UseEditableFieldName)
			{
				text6 = stiEditableObject3.Component.Name;
			}
			if (StiOptions.Export.Pdf.UseEditableFieldAlias)
			{
				text6 = stiEditableObject3.Component.Alias;
			}
			if (StiOptions.Export.Pdf.UseEditableFieldTag)
			{
				text6 = stiEditableObject3.Component.TagValue as string;
			}
			if (string.IsNullOrEmpty(text6))
			{
				text6 = $"UnsignedSignature{num15}";
			}
			if (hashtable.ContainsKey(text6))
			{
				int num16 = 2;
				string text7 = string.Empty;
				while (hashtable.ContainsKey(text6 + text7))
				{
					text7 = "_" + num16;
					num16++;
				}
				text6 += text7;
			}
			hashtable.Add(text6, text6);
			int @ref = info.AcroForm.UnsignedSignatures[num15].Ref;
			AddXref(@ref);
			sw.WriteLine("{0} 0 obj", @ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /Annot");
			sw.WriteLine("/Subtype /Widget");
			sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiEditableObject3.X), ConvertToString(stiEditableObject3.Y), ConvertToString(stiEditableObject3.X + stiEditableObject3.Width), ConvertToString(stiEditableObject3.Y + stiEditableObject3.Height));
			sw.WriteLine("/F 4");
			sw.WriteLine("/P {0} 0 R", info.PageList[stiEditableObject3.Page].Ref);
			sw.WriteLine("/FT /Sig");
			StoreStringLine("/T ", text6);
			sw.WriteLine("/MK << >>");
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderSignatureTable(StiReport report)
	{
		if (haveDigitalSignature)
		{
			string input = "Stimulsoft Reports.Net";
			input = ((!string.IsNullOrEmpty(digitalSignatureSignedBy)) ? pdfSecurity.MakeSignedByString(digitalSignatureSignedBy, padding: false) : pdfSecurity.MakeSignedByString(input));
			AddXref(info.AcroForm.Signatures[0].Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.Signatures[0].Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(((RectangleD)(ref signaturePlacement)).Left), ConvertToString(((RectangleD)(ref signaturePlacement)).Bottom), ConvertToString(((RectangleD)(ref signaturePlacement)).Right), ConvertToString(((RectangleD)(ref signaturePlacement)).Top));
			sw.WriteLine("/Type /Annot");
			sw.WriteLine("/Subtype /Widget");
			sw.WriteLine("/F 132");
			sw.WriteLine("/P {0} 0 R", info.PageList[signaturePageNumber].Ref);
			StoreStringLine("/T ", "Signature1");
			sw.WriteLine("/V {0} 0 R", info.AcroForm.Signatures[0].AP.Ref);
			StoreStringLine("/DA ", "0 g");
			sw.WriteLine("/FT /Sig");
			sw.WriteLine("/MK<<>>");
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
			AddXref(info.AcroForm.Signatures[0].AP.Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.Signatures[0].AP.Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /Sig");
			sw.Write("/Filter /");
			sw.Flush();
			offsetSignatureFilter = (int)sw.BaseStream.Position;
			sw.WriteLine("Adobe.PPKLite          ");
			sw.WriteLine("/SubFilter /adbe.pkcs7.detached");
			sw.Write("/Name ");
			sw.Flush();
			offsetSignatureName = (int)sw.BaseStream.Position;
			sw.WriteLine(input);
			sw.Write("/Contents<{0}>", new string('0', 16384));
			sw.Flush();
			offsetSignatureData = (int)(sw.BaseStream.Position - 16384 - 2);
			sw.WriteLine("");
			StoreStringLine("/M ", currentDateTime);
			if (!string.IsNullOrEmpty(digitalSignatureReason))
			{
				StoreStringLine("/Reason ", digitalSignatureReason);
			}
			if (!string.IsNullOrEmpty(digitalSignatureLocation))
			{
				StoreStringLine("/Location ", digitalSignatureLocation);
			}
			if (!string.IsNullOrEmpty(digitalSignatureContactInfo))
			{
				StoreStringLine("/ContactInfo ", digitalSignatureContactInfo);
			}
			sw.Write("/ByteRange [0 {0} {1} ", offsetSignatureData, offsetSignatureData + 16384 + 2);
			sw.Flush();
			offsetSignatureLen2 = (int)sw.BaseStream.Position;
			sw.WriteLine("0     ]");
			sw.WriteLine("/Prop_Build");
			sw.WriteLine("<<");
			sw.WriteLine("/App<<");
			string text = GetCreatorString();
			int num = text.ToLowerInvariant().IndexOf(" from ");
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			sw.WriteLine("/Name/{0}", CorrectNameEnconding(text));
			sw.WriteLine(">>");
			sw.WriteLine(">>");
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderTooltipTable()
	{
		if (!haveTooltips)
		{
			return;
		}
		for (int i = 0; i < tooltipsCounter; i++)
		{
			StiLinkObject stiLinkObject = tooltipsArray[i];
			bool flag = false;
			StiLinkObject stiLinkObject2 = default(StiLinkObject);
			for (int j = 0; j < linksCounter; j++)
			{
				stiLinkObject2 = (StiLinkObject)linksArray[j];
				if (stiLinkObject2.X == stiLinkObject.X && stiLinkObject2.Y == stiLinkObject.Y && stiLinkObject2.Width == stiLinkObject.Width && stiLinkObject2.Height == stiLinkObject.Height && stiLinkObject2.Page - 1 == stiLinkObject.Page)
				{
					flag = true;
					break;
				}
			}
			AddXref(info.AcroForm.Tooltips[i].Ref);
			sw.WriteLine("{0} 0 obj", info.AcroForm.Tooltips[i].Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Type /Annot");
			sw.WriteLine("/Subtype /Widget");
			sw.WriteLine("/Rect [{0} {1} {2} {3}]", ConvertToString(stiLinkObject.X), ConvertToString(stiLinkObject.Y), ConvertToString(stiLinkObject.X + stiLinkObject.Width), ConvertToString(stiLinkObject.Y + stiLinkObject.Height));
			StoreStringLine("/TU ", stiLinkObject.Link, escaping: true);
			sw.WriteLine("/F 0");
			sw.WriteLine("/P {0} 0 R", info.PageList[stiLinkObject.Page].Ref);
			StoreStringLine("/T ", $"Tooltip{i}");
			sw.WriteLine("/FT /Btn");
			sw.WriteLine("/Ff 65536");
			sw.WriteLine("/H /N");
			sw.WriteLine("/AP <<");
			sw.WriteLine(">>");
			if (flag)
			{
				if (stiLinkObject2.Link[0] == '#')
				{
					if (stiLinkObject2.DestY > -1.0)
					{
						sw.WriteLine("/Dest [{0} 0 R /XYZ null {1} null]", info.PageList[stiLinkObject2.DestPage].Ref, ConvertToString(stiLinkObject2.DestY));
					}
					else
					{
						sw.WriteLine("/Dest [{0} 0 R /XYZ null null null]", info.PageList[0].Ref);
					}
				}
				else
				{
					sw.WriteLine("/A <<");
					sw.WriteLine("/Type /Action");
					sw.WriteLine("/S /URI");
					StoreStringLine("/URI ", StiExportUtils.StringToUrl(stiLinkObject2.Link.Replace('\\', '/')));
					sw.WriteLine(">>");
				}
			}
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderEncodeRecord()
	{
		if (encrypted)
		{
			AddXref(info.Encode.Ref);
			sw.WriteLine("{0} 0 obj", info.Encode.Ref);
			sw.WriteLine("<<");
			pdfSecurity.RenderEncodeRecord(sw);
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderExtGStateRecord()
	{
		AddXref(info.ExtGState.Ref);
		sw.WriteLine("{0} 0 obj", info.ExtGState.Ref);
		sw.WriteLine("<<");
		for (int i = 0; i < 256; i++)
		{
			if (alphaTable[i])
			{
				string arg = $"{i:X2}";
				sw.WriteLine("/GS{0}S <</Type /ExtGState /BM /Normal /CA {1}>>", arg, colorTable[i]);
				sw.WriteLine("/GS{0}N <</Type /ExtGState /BM /Normal /ca {1}>>", arg, colorTable[i]);
			}
		}
		sw.WriteLine(">>");
		sw.WriteLine("endobj");
		sw.WriteLine("");
	}

	private void RenderBorder1(StiPdfData pp)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		StiBrush val = null;
		if (pp.Component is IStiBrush { Brush: not null } stiBrush)
		{
			val = stiBrush.Brush;
		}
		if (pp.Component is StiRichText stiRichText)
		{
			val = (StiBrush)new StiSolidBrush(stiRichText.BackColor);
		}
		if (val != null)
		{
			FillRectBrush(val, new RectangleD(pp.X, pp.Y, pp.Width, pp.Height));
		}
	}

	private void RenderBorder2(StiPdfData pp)
	{
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Invalid comparison between Unknown and I4
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Invalid comparison between Unknown and I4
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Invalid comparison between Unknown and I4
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Invalid comparison between Unknown and I4
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Invalid comparison between Unknown and I4
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Invalid comparison between Unknown and I4
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Invalid comparison between Unknown and I4
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Invalid comparison between Unknown and I4
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Invalid comparison between Unknown and I4
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		if (!(pp.Component is IStiBorder stiBorder))
		{
			return;
		}
		Color transparent = Color.Transparent;
		if (stiBorder.Border.DropShadow && stiBorder.Border.ShadowBrush != null)
		{
			transparent = StiBrush.ToColor(stiBorder.Border.ShadowBrush);
			if (transparent.A != 0)
			{
				double num = stiBorder.Border.ShadowSize * 0.8;
				SetNonStrokeColor(transparent);
				pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X + num), ConvertToString(pp.Y - num), ConvertToString(pp.Width - num), ConvertToString(num));
				pageStream.WriteLine("{0} {1} {2} {3} re f", ConvertToString(pp.X + pp.Width), ConvertToString(pp.Y - num), ConvertToString(num), ConvertToString(pp.Height));
			}
		}
		StiBorderSide val = new StiBorderSide(stiBorder.Border.Color, stiBorder.Border.Size, stiBorder.Border.Style);
		StiBorder border = stiBorder.Border;
		StiAdvancedBorder val2 = (StiAdvancedBorder)(object)((border is StiAdvancedBorder) ? border : null);
		bool flag = val2 != (StiAdvancedBorder)null;
		bool isLeftBorderSidePresent = stiBorder.Border.IsLeftBorderSidePresent;
		bool isRightBorderSidePresent = stiBorder.Border.IsRightBorderSidePresent;
		bool isTopBorderSidePresent = stiBorder.Border.IsTopBorderSidePresent;
		bool isBottomBorderSidePresent = stiBorder.Border.IsBottomBorderSidePresent;
		bool flag2 = (int)stiBorder.Border.Style != 6 && (int)stiBorder.Border.Style > 0;
		bool flag3 = (int)stiBorder.Border.Side != 0 && (int)stiBorder.Border.Style != 6;
		if (flag)
		{
			flag3 = (int)((StiBorder)val2).Side > 0;
		}
		if (!flag3)
		{
			return;
		}
		double num2 = 0.0;
		if (!flag)
		{
			if (flag2)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
			}
			num2 = StoreBorderSideData(val);
		}
		if (!flag && (int)stiBorder.Border.Side == 15)
		{
			pageStream.WriteLine("{0} {1} {2} {3} re S", ConvertToString(pp.X - num2), ConvertToString(pp.Y - num2), ConvertToString(pp.Width + num2 * 2.0), ConvertToString(pp.Height + num2 * 2.0));
			if ((int)stiBorder.Border.Style == 5)
			{
				pageStream.WriteLine("{0} {1} {2} {3} re S", ConvertToString(pp.X + num2), ConvertToString(pp.Y + num2), ConvertToString(pp.Width - num2 * 2.0), ConvertToString(pp.Height - num2 * 2.0));
			}
		}
		else
		{
			if (isLeftBorderSidePresent)
			{
				if (flag)
				{
					val = val2.LeftSide;
					if ((int)val.Style != 0)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
					}
					num2 = StoreBorderSideData(val);
				}
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X - num2), ConvertToString(pp.Y - num2));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X - num2), ConvertToString(pp.Y + pp.Height + num2));
				if ((int)val.Style == 5)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + num2), ConvertToString(pp.Y + (isBottomBorderSidePresent ? num2 : (0.0 - num2))));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + num2), ConvertToString(pp.Y + pp.Height + (isTopBorderSidePresent ? (0.0 - num2) : num2)));
				}
				if (flag && (int)val.Style != 0)
				{
					pageStream.WriteLine("Q");
					PopColorFromStack();
				}
			}
			if (isRightBorderSidePresent)
			{
				if (flag)
				{
					val = val2.RightSide;
					if ((int)val.Style != 0)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
					}
					num2 = StoreBorderSideData(val);
				}
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width + num2), ConvertToString(pp.Y - num2));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width + num2), ConvertToString(pp.Y + pp.Height + num2));
				if ((int)val.Style == 5)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + pp.Width - num2), ConvertToString(pp.Y + (isBottomBorderSidePresent ? num2 : (0.0 - num2))));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width - num2), ConvertToString(pp.Y + pp.Height + (isTopBorderSidePresent ? (0.0 - num2) : num2)));
				}
				if (flag && (int)val.Style != 0)
				{
					pageStream.WriteLine("Q");
					PopColorFromStack();
				}
			}
			if (isTopBorderSidePresent)
			{
				if (flag)
				{
					val = val2.TopSide;
					if ((int)val.Style != 0)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
					}
					num2 = StoreBorderSideData(val);
				}
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X - num2), ConvertToString(pp.Y + pp.Height + num2));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width + num2), ConvertToString(pp.Y + pp.Height + num2));
				if ((int)val.Style == 5)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + (isLeftBorderSidePresent ? num2 : (0.0 - num2))), ConvertToString(pp.Y + pp.Height - num2));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width + (isRightBorderSidePresent ? (0.0 - num2) : num2)), ConvertToString(pp.Y + pp.Height - num2));
				}
				if (flag && (int)val.Style != 0)
				{
					pageStream.WriteLine("Q");
					PopColorFromStack();
				}
			}
			if (isBottomBorderSidePresent)
			{
				if (flag)
				{
					val = val2.BottomSide;
					if ((int)val.Style != 0)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
					}
					num2 = StoreBorderSideData(val);
				}
				pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X - num2), ConvertToString(pp.Y - num2));
				pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width + num2), ConvertToString(pp.Y - num2));
				if ((int)val.Style == 5)
				{
					pageStream.WriteLine("{0} {1} m", ConvertToString(pp.X + (isLeftBorderSidePresent ? num2 : (0.0 - num2))), ConvertToString(pp.Y + num2));
					pageStream.WriteLine("{0} {1} l S", ConvertToString(pp.X + pp.Width + (isRightBorderSidePresent ? (0.0 - num2) : num2)), ConvertToString(pp.Y + num2));
				}
				if (flag && (int)val.Style != 0)
				{
					pageStream.WriteLine("Q");
					PopColorFromStack();
				}
			}
		}
		if (!flag && flag2)
		{
			pageStream.WriteLine("Q");
			PopColorFromStack();
		}
	}

	private double StoreBorderSideData(StiBorderSide border)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Invalid comparison between Unknown and I4
		double num = border.Size;
		if (num < 0.5)
		{
			num = 0.5;
		}
		if ((int)border.Style == 5)
		{
			num = 1.0;
		}
		double num2 = num * 0.72 * 0.955;
		pageStream.WriteLine("{0} w", ConvertToString(num2));
		string penStyleDashString = GetPenStyleDashString(border.Style, num2 * 0.04);
		if (penStyleDashString != null)
		{
			pageStream.WriteLine(penStyleDashString);
		}
		Color color = border.Color;
		SetStrokeColor(color);
		if ((int)border.Style != 5)
		{
			return 0.0;
		}
		return num2;
	}

	private string GetPenStyleDashString(StiPenStyle style, double step)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected I4, but got Unknown
		return (style - 1) switch
		{
			3 => $"[{ConvertToString(step)} {ConvertToString(step * 58.0)}] 0 d", 
			0 => $"[{ConvertToString(step * 49.5)} {ConvertToString(step * 62.0)}] 0 d", 
			1 => string.Format("[{0} {1} {2} {1}] 0 d", ConvertToString(step * 50.0), ConvertToString(step * 55.0), ConvertToString(step)), 
			2 => string.Format("[{0} {1} {2} {1} {2} {1}] 0 d", ConvertToString(step * 50.0), ConvertToString(step * 55.0), ConvertToString(step)), 
			_ => null, 
		};
	}

	private float StoreWatermarkMetafileData(Image image, float imageResolution, StiPage page)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		double num = report.Unit.ConvertToHInches(page.PageWidth * (double)page.SegmentPerWidth);
		double num2 = report.Unit.ConvertToHInches(page.PageHeight * (double)page.SegmentPerHeight);
		int num3 = (int)Math.Round(num * (double)imageResolution);
		int num4 = (int)Math.Round(num2 * (double)imageResolution);
		Bitmap val = new Bitmap(num3, num4);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				if (useTransparency)
				{
					val2.Clear(Color.FromArgb(1, 255, 255, 255));
				}
				else
				{
					val2.Clear(Color.White);
				}
				RectangleF rectangleF = new RectangleF(0f, 0f, num3, num4);
				if (rectangleF.Width > 0f && rectangleF.Height > 0f)
				{
					val2.DrawImage(image, rectangleF);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			StoreImageData((Image)(object)val, imageResolution, isImageComponent: false, needSmoothing: false);
			return imageResolution;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	internal float StoreImageData(Image image, float imageResolution, bool isImageComponent, bool needSmoothing, bool maxQuality = false)
	{
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Invalid comparison between Unknown and I4
		if (isImageComponent && imageResolutionMode == StiImageResolutionMode.NoMoreThan && imageResolution > imageResolutionMain)
		{
			int num = (int)Math.Round((float)image.Width * imageResolutionMain / imageResolution);
			int num2 = (int)Math.Round((float)image.Height * imageResolutionMain / imageResolution);
			Bitmap val = new Bitmap(num, num2);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			if (!useTransparency)
			{
				val2.Clear(Color.White);
			}
			val2.DrawImage(image, new Rectangle(0, 0, num, num2), 0, 0, image.Width, image.Height, (GraphicsUnit)2);
			val2.Dispose();
			image = (Image)(object)val;
			imageResolution = imageResolutionMain;
			imageCache.UsedMemoryCounter += ((Image)val).Width * ((Image)val).Height * 4;
		}
		if (isImageComponent && !useTransparency && (imageResolutionMode == StiImageResolutionMode.Auto || (imageResolutionMode == StiImageResolutionMode.NoMoreThan && imageResolution <= imageResolutionMain)))
		{
			int width = image.Width;
			int height = image.Height;
			Bitmap val3 = new Bitmap(width, height);
			Graphics val4 = Graphics.FromImage((Image)(object)val3);
			val4.Clear(Color.White);
			val4.DrawImage(image, new Rectangle(0, 0, width, height), 0, 0, width, height, (GraphicsUnit)2);
			val4.Dispose();
			image = (Image)(object)val3;
			imageCache.UsedMemoryCounter += ((Image)val3).Width * ((Image)val3).Height * 4;
		}
		if (imageFormat != StiImageFormat.Color)
		{
			Bitmap val5 = new Bitmap(image.Width, image.Height);
			Graphics val6 = Graphics.FromImage((Image)(object)val5);
			ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
			{
				new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
				new float[5] { 0.55f, 0.55f, 0.55f, 0f, 0f },
				new float[5] { 0.15f, 0.15f, 0.15f, 0f, 0f },
				new float[6] { 0f, 0f, 0f, 1f, 0f, 0f },
				new float[6] { 0f, 0f, 0f, 0f, 1f, 0f },
				new float[6] { 0f, 0f, 0f, 0f, 0f, 1f }
			});
			ImageAttributes val7 = new ImageAttributes();
			val7.SetColorMatrix(colorMatrix);
			val6.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, (GraphicsUnit)2, val7);
			val6.Dispose();
			image = (Image)(object)val5;
			imageCache.UsedMemoryCounter += ((Image)val5).Width * ((Image)val5).Height * 4;
		}
		if (imageFormat == StiImageFormat.Monochrome)
		{
			image = StiTiffHelper.MakeMonochromeImage(image, monochromeDitheringType, 128);
			imageCache.UsedMemoryCounter += image.Width * image.Height * 4;
		}
		int num3 = (maxQuality ? imageCache.AddImageInt(image, 1f) : imageCache.AddImageInt(image));
		StiImageData stiImageData = default(StiImageData);
		stiImageData.Width = image.Width;
		stiImageData.Height = image.Height;
		stiImageData.Name = $"Image{num3}";
		stiImageData.ImageFormat = imageFormat;
		if ((int)image.PixelFormat == 196865 && (imageCompressionMethod == StiPdfImageCompressionMethod.Flate || imageCompressionMethod == StiPdfImageCompressionMethod.Indexed))
		{
			stiImageData.ImageFormat = StiImageFormat.Monochrome;
		}
		imageList.Add(stiImageData);
		imageCacheIndexToList[num3] = stiImageData;
		if (needSmoothing)
		{
			imageInterpolationTable[num3] = true;
		}
		return imageResolution;
	}

	internal void StoreImageDataForGeom(StiImage image)
	{
		Image val = image.TakeGdiImageToDraw(imageResolutionMain);
		try
		{
			if (val != null)
			{
				float imageResolution = val.HorizontalResolution / 100f;
				if (imageResolutionMode == StiImageResolutionMode.NoMoreThan)
				{
					imageResolution = ((!image.Stretch) ? ((float)(1.0 / image.MultipleFactor)) : ((float)((double)val.Width / report.Unit.ConvertToHInches(image.Width))));
				}
				imageResolution = StoreImageData(val, imageResolution, isImageComponent: true, image.Smoothing);
				imageInfoList[imageInfoCounter] = imageResolution;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void WriteImageInfo(StiPdfData pp, float imageResolution, bool forceResolutionModeAuto = false)
	{
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Invalid comparison between Unknown and I4
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Invalid comparison between Unknown and I4
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Invalid comparison between Unknown and I4
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Invalid comparison between Unknown and I4
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Invalid comparison between Unknown and I4
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Invalid comparison between Unknown and I4
		StiImageData stiImageData = (StiImageData)imageList[imagesCurrent];
		imagesCurrent++;
		StiImage stiImage = pp.Component as StiImage;
		StiImageResolutionMode stiImageResolutionMode = imageResolutionMode;
		if (forceResolutionModeAuto)
		{
			stiImageResolutionMode = StiImageResolutionMode.Auto;
		}
		if (stiImageResolutionMode != StiImageResolutionMode.Exactly && stiImage != null && !stiImage.ImageToDrawIsMetafile() && !stiImage.Margins.IsEmpty)
		{
			double num = 0.72 * stiImage.Margins.Left;
			double num2 = 0.72 * stiImage.Margins.Right;
			double num3 = 0.72 * stiImage.Margins.Top;
			double num4 = 0.72 * stiImage.Margins.Bottom;
			if (num != 0.0)
			{
				pp.X += num;
				pp.Width -= num;
			}
			if (num4 != 0.0)
			{
				pp.Y += num4;
				pp.Height -= num4;
			}
			if (num2 != 0.0)
			{
				pp.Width -= num2;
			}
			if (num3 != 0.0)
			{
				pp.Height -= num3;
			}
		}
		double num5 = pp.X;
		double num6 = pp.Y;
		double value = 0.72 * (double)(stiImageData.Width - 1) / (double)imageResolution;
		double value2 = 0.72 * (double)(stiImageData.Height - 1) / (double)imageResolution;
		bool flag = false;
		if (stiImageResolutionMode != StiImageResolutionMode.Exactly && stiImage != null && stiImage.ExistImageToDraw() && !stiImage.ImageToDrawIsMetafile())
		{
			Image val = stiImage.TakeGdiImageToDraw(imageResolutionMain);
			try
			{
				if (val != null)
				{
					int num7 = val.Width;
					int num8 = val.Height;
					if (stiImage.ImageRotation == StiImageRotation.Rotate90CCW || stiImage.ImageRotation == StiImageRotation.Rotate90CW)
					{
						num7 = val.Height;
						num8 = val.Width;
					}
					RectangleD rect = stiImage.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
					rect = stiImage.ConvertImageMargins(rect, convert: false);
					RectangleF rectangleF = new RectangleF(0f, 0f, (float)((RectangleD)(ref rect)).Width, (float)((RectangleD)(ref rect)).Height);
					if (!stiImage.Stretch && !stiImage.Icon.HasValue)
					{
						float num9 = (float)((double)num7 * stiImage.MultipleFactor);
						float num10 = (float)((double)num8 * stiImage.MultipleFactor);
						rectangleF.Width = num9;
						rectangleF.Height = num10;
						StiHorAlignment horAlignment = stiImage.HorAlignment;
						if ((int)horAlignment != 2)
						{
							if ((int)horAlignment == 3)
							{
								rectangleF.X = (float)(((RectangleD)(ref rect)).Width - (double)num9);
							}
						}
						else
						{
							rectangleF.X = (float)((((RectangleD)(ref rect)).Width - (double)num9) / 2.0);
						}
						StiVertAlignment vertAlignment = stiImage.VertAlignment;
						if ((int)vertAlignment != 0)
						{
							if ((int)vertAlignment == 1)
							{
								rectangleF.Y = (float)((((RectangleD)(ref rect)).Height - (double)num10) / 2.0);
							}
						}
						else
						{
							rectangleF.Y = (float)(((RectangleD)(ref rect)).Height - (double)num10);
						}
						num5 += (double)rectangleF.X * 0.72;
						num6 += (double)rectangleF.Y * 0.72;
						value = (double)rectangleF.Width * 0.72;
						value2 = (double)rectangleF.Height * 0.72;
						flag = true;
					}
					else
					{
						float num11 = num7;
						float num12 = num8;
						if (stiImage.AspectRatio)
						{
							float num13 = rectangleF.Width / num11;
							float num14 = rectangleF.Height / num12;
							if (num13 > num14)
							{
								rectangleF.Width = num11 * num14;
							}
							else
							{
								rectangleF.Height = num12 * num13;
							}
							StiVertAlignment vertAlignment2 = stiImage.VertAlignment;
							if ((int)vertAlignment2 != 0)
							{
								if ((int)vertAlignment2 == 1)
								{
									rectangleF.Y = (float)((((RectangleD)(ref rect)).Height - (double)rectangleF.Height) / 2.0);
								}
							}
							else
							{
								rectangleF.Y = (float)(((RectangleD)(ref rect)).Height - (double)rectangleF.Height);
							}
							StiHorAlignment horAlignment2 = stiImage.HorAlignment;
							if ((int)horAlignment2 != 2)
							{
								if ((int)horAlignment2 == 3)
								{
									rectangleF.X = (float)(((RectangleD)(ref rect)).Width - (double)rectangleF.Width);
								}
							}
							else
							{
								rectangleF.X = (float)((((RectangleD)(ref rect)).Width - (double)rectangleF.Width) / 2.0);
							}
							num5 += (double)rectangleF.X * 0.72;
							num6 += (double)rectangleF.Y * 0.72;
							value = (double)rectangleF.Width * 0.72;
							value2 = (double)rectangleF.Height * 0.72;
						}
						else
						{
							value = pp.Width;
							value2 = pp.Height;
						}
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		pageStream.WriteLine("q");
		PushColorToStack();
		SetNonStrokeColor(Color.Black);
		if (flag)
		{
			pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(pp.X), ConvertToString(pp.Y), ConvertToString(pp.Width), ConvertToString(pp.Height));
		}
		pageStream.WriteLine("{0} 0 0 {1} {2} {3} cm", ConvertToString(value), ConvertToString(value2), ConvertToString(num5), ConvertToString(num6));
		pageStream.WriteLine("/{0} Do", stiImageData.Name);
		pageStream.WriteLine("Q");
		PopColorFromStack();
	}

	internal void WriteImageInfo2(StiPdfData pp, double imageResolutionX, double imageResolutionY)
	{
		StiImageData stiImageData = (StiImageData)imageList[imagesCurrent];
		imagesCurrent++;
		pageStream.WriteLine("q");
		PushColorToStack();
		SetNonStrokeColor(Color.Black);
		pageStream.WriteLine("{0} 0 0 {1} {2} {3} cm", ConvertToString(0.72 * (double)stiImageData.Width / imageResolutionX), ConvertToString(0.72 * (double)stiImageData.Height / imageResolutionY), ConvertToString(pp.X), ConvertToString(pp.Y));
		pageStream.WriteLine("/{0} Do", stiImageData.Name);
		pageStream.WriteLine("Q");
		PopColorFromStack();
	}

	internal void RenderImage(StiPdfData pp, float imageResolution, bool forceResolutionModeAuto = false)
	{
		if (imageInfoList.ContainsKey(imageInfoCounter))
		{
			float imageResolution2 = (float)imageInfoList[imageInfoCounter];
			WriteImageInfo(pp, imageResolution2, forceResolutionModeAuto);
		}
		else
		{
			if (!(pp.Component is IStiExportImageExtended stiExportImageExtended) || !pp.Component.IsExportAsImage(StiExportFormat.Pdf))
			{
				return;
			}
			float zoom = imageResolution;
			Image image = stiExportImageExtended.GetImage(ref zoom, StiExportFormat.Pdf);
			try
			{
				if (image != null)
				{
					WriteImageInfo(pp, zoom, forceResolutionModeAuto);
				}
			}
			finally
			{
				((IDisposable)image)?.Dispose();
			}
		}
	}

	private void RenderChart(StiPdfData pp, bool assemble, int pageNumber)
	{
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Expected O, but got Unknown
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		StiChart stiChart = pp.Component as StiChart;
		StiContext context = null;
		float num = 0.96f;
		float num2 = (float)(report.Unit.ConvertToHInches(pp.Component.Width) * (double)num);
		float num3 = (float)(report.Unit.ConvertToHInches(pp.Component.Height) * (double)num);
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 1f;
		float num8 = 1f;
		switch (stiChart.Rotation)
		{
		case StiImageRotation.Rotate90CCW:
			num4 = -90f;
			num6 = num3;
			break;
		case StiImageRotation.Rotate90CW:
			num4 = 90f;
			num5 = num2;
			break;
		case StiImageRotation.Rotate180:
			num4 = -180f;
			num5 = num2;
			num6 = num3;
			break;
		case StiImageRotation.FlipVertical:
			num8 = -1f;
			num6 = num3;
			break;
		case StiImageRotation.FlipHorizontal:
			num7 = -1f;
			num5 = num2;
			break;
		}
		StiImageRotation rotation = stiChart.Rotation;
		if ((uint)(rotation - 1) <= 1u)
		{
			float num9 = num2;
			num2 = num3;
			num3 = num9;
		}
		if (!assemble && stiChart.Rotation != 0)
		{
			Matrix val = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
			val.Translate(num5, num6);
			val.Scale(num7, num8);
			val.Rotate(num4);
			pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(val.Elements[0]), ConvertToString(val.Elements[1]), ConvertToString(val.Elements[2]), ConvertToString(val.Elements[3]), ConvertToString(val.Elements[4]), ConvertToString(val.Elements[5]));
		}
		Bitmap val2 = new Bitmap(1, 1);
		try
		{
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val3);
				context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
				bool isAnimation = stiChart.IsAnimation;
				stiChart.IsAnimation = false;
				StiCellGeom stiCellGeom = stiChart.Core.Render(context, new RectangleF(0f, 0f, num2, num3), useMargins: true);
				stiCellGeom.DrawGeom(context);
				stiChart.IsAnimation = isAnimation;
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
		RenderGdiContext(pp, assemble, pageNumber, context);
	}

	private void RenderSparkline(StiPdfData pp, bool assemble, int pageNumber)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		StiSparkline sparkline = pp.Component as StiSparkline;
		StiContext context = null;
		float num = 0.96f;
		float num2 = (float)(report.Unit.ConvertToHInches(pp.Component.Width) * (double)num);
		float num3 = (float)(report.Unit.ConvertToHInches(pp.Component.Height) * (double)num);
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
				StiSparklineGdiPainter.RenderSparkline(context, new RectangleD(0.0, 0.0, (double)num2, (double)num3), sparkline, 1f);
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
		RenderGdiContext(pp, assemble, pageNumber, context);
	}

	private void RenderGdiContext(StiPdfData pp, bool assemble, int pageNumber, StiContext context)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Invalid comparison between Unknown and I4
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Invalid comparison between Unknown and I4
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Invalid comparison between Unknown and I4
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Expected O, but got Unknown
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Invalid comparison between Unknown and I4
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Invalid comparison between Unknown and I4
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		//IL_0838: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Invalid comparison between Unknown and I4
		//IL_093e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Invalid comparison between Unknown and I4
		Matrix val = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
		val.Translate((float)pp.X, (float)(pp.Y + pp.Height));
		val.Scale(1f, -1f);
		val.Scale(0.75f, 0.75f);
		StiPdfGeomWriter stiPdfGeomWriter = new StiPdfGeomWriter(pageStream, this, assemble);
		stiPdfGeomWriter.pageNumber = pageNumber;
		stiPdfGeomWriter.matrixCache.Push(val);
		foreach (StiGeom geom in context.Geoms)
		{
			if (geom is StiPushTranslateTransformGeom)
			{
				stiPdfGeomWriter.SaveState();
				stiPdfGeomWriter.TranslateTransform((geom as StiPushTranslateTransformGeom).X, (geom as StiPushTranslateTransformGeom).Y);
			}
			if (geom is StiPushRotateTransformGeom)
			{
				stiPdfGeomWriter.SaveState();
				stiPdfGeomWriter.RotateTransform((geom as StiPushRotateTransformGeom).Angle);
			}
			if (geom is StiPopTransformGeom)
			{
				stiPdfGeomWriter.RestoreState();
			}
			if (geom is StiPushClipGeom)
			{
				stiPdfGeomWriter.SaveState();
				stiPdfGeomWriter.SetClip((geom as StiPushClipGeom).ClipRectangle);
			}
			if (geom is StiPushClipPathGeom)
			{
				stiPdfGeomWriter.SaveState();
				StiPushClipPathGeom stiPushClipPathGeom = geom as StiPushClipPathGeom;
				stiPdfGeomWriter.BeginPath();
				foreach (StiSegmentGeom geom2 in stiPushClipPathGeom.Geoms)
				{
					if (geom2 is StiPieSegmentGeom)
					{
						StiPieSegmentGeom stiPieSegmentGeom = geom2 as StiPieSegmentGeom;
						stiPdfGeomWriter.DrawPie(stiPieSegmentGeom.Rect, stiPieSegmentGeom.StartAngle, stiPieSegmentGeom.SweepAngle);
					}
					if (geom2 is StiArcSegmentGeom)
					{
						StiArcSegmentGeom stiArcSegmentGeom = geom2 as StiArcSegmentGeom;
						stiPdfGeomWriter.DrawArc(stiArcSegmentGeom.Rect, stiArcSegmentGeom.StartAngle, stiArcSegmentGeom.SweepAngle);
					}
					if (geom2 is StiLineSegmentGeom)
					{
						StiLineSegmentGeom stiLineSegmentGeom = geom2 as StiLineSegmentGeom;
						stiPdfGeomWriter.DrawLine(new PointF(stiLineSegmentGeom.X1, stiLineSegmentGeom.Y1), new PointF(stiLineSegmentGeom.X2, stiLineSegmentGeom.Y2), null);
					}
					if (geom2 is StiLinesSegmentGeom)
					{
						StiLinesSegmentGeom stiLinesSegmentGeom = geom2 as StiLinesSegmentGeom;
						stiPdfGeomWriter.DrawPolyline(stiLinesSegmentGeom.Points, null);
					}
					if (geom2 is StiCurveSegmentGeom)
					{
						StiCurveSegmentGeom stiCurveSegmentGeom = geom2 as StiCurveSegmentGeom;
						stiPdfGeomWriter.DrawSpline(stiCurveSegmentGeom.Points, stiCurveSegmentGeom.Tension, null);
					}
					if (geom2 is StiCloseFigureSegmentGeom)
					{
						stiPdfGeomWriter.CloseFigure();
					}
				}
				stiPdfGeomWriter.CloseFigure();
				stiPdfGeomWriter.EndPath();
				stiPdfGeomWriter.SetClipPath();
			}
			if (geom is StiPopClipGeom)
			{
				stiPdfGeomWriter.RestoreState();
			}
			if (geom is StiPushSmothingModeToAntiAliasGeom)
			{
				stiPdfGeomWriter.SaveState();
			}
			if (geom is StiPopSmothingModeGeom)
			{
				stiPdfGeomWriter.RestoreState();
			}
			if (geom is StiPushTextRenderingHintToAntiAliasGeom)
			{
				stiPdfGeomWriter.SaveState();
			}
			if (geom is StiPopTextRenderingHintGeom)
			{
				stiPdfGeomWriter.RestoreState();
			}
			if (geom is StiBorderGeom)
			{
				StiBorderGeom stiBorderGeom = geom as StiBorderGeom;
				if (stiBorderGeom.Background != null)
				{
					stiPdfGeomWriter.FillRectangle(RectToRectangleF(stiBorderGeom.Rect), stiBorderGeom.Background);
				}
				if (CheckPenGeom(stiBorderGeom.BorderPen))
				{
					stiPdfGeomWriter.DrawRectangle(RectToRectangleF(stiBorderGeom.Rect), stiBorderGeom.BorderPen);
				}
			}
			if (geom is StiLineGeom)
			{
				StiLineGeom stiLineGeom = geom as StiLineGeom;
				if (CheckPenGeom(stiLineGeom.Pen))
				{
					stiPdfGeomWriter.DrawLine(new PointF(stiLineGeom.X1, stiLineGeom.Y1), new PointF(stiLineGeom.X2, stiLineGeom.Y2), stiLineGeom.Pen);
				}
			}
			if (geom is StiLinesGeom)
			{
				StiLinesGeom stiLinesGeom = geom as StiLinesGeom;
				if (CheckPenGeom(stiLinesGeom.Pen))
				{
					stiPdfGeomWriter.DrawPolyline(stiLinesGeom.Points, stiLinesGeom.Pen);
				}
			}
			if (geom is StiCurveGeom)
			{
				StiCurveGeom stiCurveGeom = geom as StiCurveGeom;
				if (CheckPenGeom(stiCurveGeom.Pen))
				{
					stiPdfGeomWriter.DrawSpline(stiCurveGeom.Points, stiCurveGeom.Tension, stiCurveGeom.Pen);
				}
			}
			if (geom is StiEllipseGeom)
			{
				StiEllipseGeom stiEllipseGeom = geom as StiEllipseGeom;
				if (stiEllipseGeom.Background != null)
				{
					stiPdfGeomWriter.FillEllipse(RectToRectangleF(stiEllipseGeom.Rect), stiEllipseGeom.Background);
				}
				if (CheckPenGeom(stiEllipseGeom.BorderPen))
				{
					stiPdfGeomWriter.DrawEllipse(RectToRectangleF(stiEllipseGeom.Rect), stiEllipseGeom.BorderPen);
				}
			}
			if (geom is StiCachedShadowGeom)
			{
				StiCachedShadowGeom stiCachedShadowGeom = geom as StiCachedShadowGeom;
				RectangleF rect = stiCachedShadowGeom.Rect;
				StiShadowSides sides = stiCachedShadowGeom.Sides;
				float y = rect.Y + 8f;
				float x = rect.X + 8f;
				float num = (((sides & 4) > 0) ? 4 : 0);
				if ((sides & 1) > 0)
				{
					y = rect.Y + 4f;
				}
				if ((sides & 0x10) > 0)
				{
					x = rect.X + 4f;
				}
				for (int i = 0; i < 3; i++)
				{
					List<PointF> list = new List<PointF>();
					if ((sides & 2) > 0)
					{
						list.Add(new PointF(rect.Right, y));
						if ((sides & 8) > 0)
						{
							list.Add(new PointF(rect.Right, rect.Bottom));
							list.Add(new PointF(x, rect.Bottom));
							list.Add(new PointF(x, rect.Bottom + (float)i + 1f));
							list.Add(new PointF(rect.Right + (float)i + 1f, rect.Bottom + (float)i + 1f));
						}
						else
						{
							list.Add(new PointF(rect.Right, rect.Bottom + num));
							list.Add(new PointF(rect.Right + (float)i + 1f, rect.Bottom + num));
						}
						list.Add(new PointF(rect.Right + (float)i + 1f, y));
					}
					else
					{
						list.Add(new PointF(x, rect.Bottom));
						list.Add(new PointF(rect.Right + num, rect.Bottom));
						list.Add(new PointF(rect.Right + num, rect.Bottom + (float)i + 1f));
						list.Add(new PointF(x, rect.Bottom + (float)i + 1f));
					}
					PointF[] array = new PointF[list.Count];
					list.CopyTo(array);
					StiSolidBrush brush = new StiSolidBrush(Color.FromArgb(40 - 10 * i, Color.Black));
					stiPdfGeomWriter.FillPolygon(array, brush);
				}
			}
			if (geom is StiShadowGeom)
			{
				StiShadowGeom stiShadowGeom = geom as StiShadowGeom;
			}
			if (geom is StiTextGeom)
			{
				StiTextGeom stiTextGeom = geom as StiTextGeom;
				FontFamily val2 = null;
				if (PdfFonts.IsFontStimulsoft(stiTextGeom.Font.FontName))
				{
					val2 = StiFontIconsHelper.GetFontFamilyIcons();
				}
				if (val2 == null)
				{
					val2 = StiFontCollection.GetFontFamily(stiTextGeom.Font.FontName, false);
				}
				Font val3 = new Font(val2, stiTextGeom.Font.FontSize, stiTextGeom.Font.FontStyle, stiTextGeom.Font.Unit, stiTextGeom.Font.GdiCharSet, stiTextGeom.Font.GdiVerticalFont);
				StringFormat val4 = (StringFormat)(stiTextGeom.StringFormat.IsGeneric ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)new StringFormat()));
				val4.Alignment = stiTextGeom.StringFormat.Alignment;
				val4.FormatFlags = stiTextGeom.StringFormat.FormatFlags;
				val4.HotkeyPrefix = stiTextGeom.StringFormat.HotkeyPrefix;
				val4.LineAlignment = stiTextGeom.StringFormat.LineAlignment;
				val4.Trimming = stiTextGeom.StringFormat.Trimming;
				StiBrush brush2 = BrushToStiBrush(stiTextGeom.Brush);
				bool allowHtmlTags = StiOptions.Engine.AllowHtmlTagsInChart && StiTextRenderer.CheckTextForHtmlTags(stiTextGeom.Text);
				if (stiTextGeom.IsRotatedText)
				{
					PointF pointF = default(PointF);
					if (stiTextGeom.Location is PointF)
					{
						pointF = (PointF)stiTextGeom.Location;
					}
					else
					{
						RectangleF rectangleF = RectToRectangleF(stiTextGeom.Location);
						pointF = new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
					}
					Bitmap val5 = new Bitmap(1, 1);
					SizeF sizeF;
					try
					{
						Graphics val6 = Graphics.FromImage((Image)(object)val5);
						try
						{
							sizeF = val6.MeasureString(stiTextGeom.Text, val3, (int)Math.Round((double)stiTextGeom.MaximalWidth.GetValueOrDefault() / StiDpiHelper.DeviceCapsScale), val4);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
						sizeF.Width *= (float)StiDpiHelper.DeviceCapsScale;
						sizeF.Height *= (float)StiDpiHelper.DeviceCapsScale;
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					RectangleF rectangleF2 = new RectangleF(0f, 0f, sizeF.Width, sizeF.Height);
					PointF startPoint = GetStartPoint(stiTextGeom.RotationMode.GetValueOrDefault(), rectangleF2);
					rectangleF2.X -= startPoint.X;
					rectangleF2.Y -= startPoint.Y;
					if ((int)val4.Alignment == 1)
					{
						rectangleF2.X -= 1f;
					}
					if ((int)val4.Alignment == 2)
					{
						rectangleF2.X -= 2f;
					}
					rectangleF2.Width += 2f;
					stiPdfGeomWriter.SaveState();
					stiPdfGeomWriter.TranslateTransform(pointF.X, pointF.Y);
					if (stiTextGeom.Angle != 0f)
					{
						stiPdfGeomWriter.RotateTransform(stiTextGeom.Angle);
					}
					stiPdfGeomWriter.DrawString(stiTextGeom.Text, val3, brush2, rectangleF2, val4, allowHtmlTags);
					stiPdfGeomWriter.RestoreState();
				}
				else
				{
					stiPdfGeomWriter.DrawString(stiTextGeom.Text, val3, brush2, RectToRectangleF(stiTextGeom.Location), val4, allowHtmlTags);
				}
			}
			if (!(geom is StiPathGeom))
			{
				continue;
			}
			StiPathGeom stiPathGeom = geom as StiPathGeom;
			if (stiPathGeom.Background == null && !CheckPenGeom(stiPathGeom.Pen))
			{
				continue;
			}
			stiPdfGeomWriter.BeginPath();
			foreach (StiSegmentGeom geom3 in stiPathGeom.Geoms)
			{
				if (geom3 is StiPieSegmentGeom)
				{
					StiPieSegmentGeom stiPieSegmentGeom2 = geom3 as StiPieSegmentGeom;
					float startAngle = stiPieSegmentGeom2.RealStartAngle ?? stiPieSegmentGeom2.StartAngle;
					float sweepAngle = stiPieSegmentGeom2.RealSweepAngle ?? stiPieSegmentGeom2.SweepAngle;
					stiPdfGeomWriter.DrawPie(stiPieSegmentGeom2.Rect, startAngle, sweepAngle);
				}
				if (geom3 is StiArcSegmentGeom)
				{
					StiArcSegmentGeom stiArcSegmentGeom2 = geom3 as StiArcSegmentGeom;
					float startAngle2 = stiArcSegmentGeom2.RealStartAngle ?? stiArcSegmentGeom2.StartAngle;
					float sweepAngle2 = stiArcSegmentGeom2.RealSweepAngle ?? stiArcSegmentGeom2.SweepAngle;
					stiPdfGeomWriter.DrawArc(stiArcSegmentGeom2.Rect, startAngle2, sweepAngle2);
				}
				if (geom3 is StiLineSegmentGeom)
				{
					StiLineSegmentGeom stiLineSegmentGeom2 = geom3 as StiLineSegmentGeom;
					stiPdfGeomWriter.DrawLine(new PointF(stiLineSegmentGeom2.X1, stiLineSegmentGeom2.Y1), new PointF(stiLineSegmentGeom2.X2, stiLineSegmentGeom2.Y2), null);
				}
				if (geom3 is StiLinesSegmentGeom)
				{
					StiLinesSegmentGeom stiLinesSegmentGeom2 = geom3 as StiLinesSegmentGeom;
					stiPdfGeomWriter.DrawPolyline(stiLinesSegmentGeom2.Points, null);
				}
				if (geom3 is StiCurveSegmentGeom)
				{
					StiCurveSegmentGeom stiCurveSegmentGeom2 = geom3 as StiCurveSegmentGeom;
					stiPdfGeomWriter.DrawSpline(stiCurveSegmentGeom2.Points, stiCurveSegmentGeom2.Tension, null);
				}
				if (geom3 is StiCloseFigureSegmentGeom)
				{
					stiPdfGeomWriter.CloseFigure();
				}
			}
			stiPdfGeomWriter.CloseFigure();
			stiPdfGeomWriter.EndPath();
			if (stiPathGeom.Background != null)
			{
				stiPdfGeomWriter.FillPath(stiPathGeom.Background);
			}
			if (CheckPenGeom(stiPathGeom.Pen))
			{
				stiPdfGeomWriter.StrokePath(stiPathGeom.Pen);
			}
		}
	}

	private static PointF GetStartPoint(StiRotationMode rotationMode, RectangleF textRect)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected I4, but got Unknown
		PointF pointF = new PointF(textRect.X + textRect.Width / 2f, textRect.Y + textRect.Height / 2f);
		return (rotationMode - 1) switch
		{
			0 => new PointF(textRect.X, pointF.Y), 
			1 => new PointF(textRect.X, textRect.Bottom), 
			2 => new PointF(pointF.X, textRect.Top), 
			3 => pointF, 
			4 => new PointF(pointF.X, textRect.Bottom), 
			5 => new PointF(textRect.Right, textRect.Top), 
			6 => new PointF(textRect.Right, pointF.Y), 
			7 => new PointF(textRect.Right, textRect.Bottom), 
			_ => textRect.Location, 
		};
	}

	private RectangleF RectToRectangleF(object rect)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (rect is RectangleF)
		{
			return (RectangleF)rect;
		}
		if (rect is Rectangle rectangle)
		{
			return new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}
		if (rect is RectangleD val)
		{
			return new RectangleF((float)((RectangleD)(ref val)).X, (float)((RectangleD)(ref val)).Y, (float)((RectangleD)(ref val)).Width, (float)((RectangleD)(ref val)).Height);
		}
		return default(RectangleF);
	}

	private StiBrush BrushToStiBrush(object brush)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (brush == null)
		{
			return null;
		}
		if (brush is Color)
		{
			return (StiBrush)new StiSolidBrush((Color)brush);
		}
		if (brush is StiBrush)
		{
			return (StiBrush)brush;
		}
		return null;
	}

	private bool CheckPenGeom(StiPenGeom penGeom)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		if (penGeom != null && penGeom.Brush != null)
		{
			return (int)penGeom.PenStyle != 6;
		}
		return false;
	}

	private void RenderGauge(StiPdfData pp, bool assemble, int pageNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0755: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		StiGauge stiGauge = pp.Component as StiGauge;
		Bitmap val = new Bitmap(1, 1);
		StiGdiGaugeContextPainter stiGdiGaugeContextPainter;
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				bool isAnimation = stiGauge.IsAnimation;
				stiGauge.IsAnimation = false;
				float num = 0.96f;
				stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(val2, stiGauge, new RectangleF(0f, 0f, (float)(report.Unit.ConvertToHInches(pp.Component.Width) * (double)num), (float)(report.Unit.ConvertToHInches(pp.Component.Height) * (double)num)), num);
				stiGdiGaugeContextPainter.Geoms.Clear();
				stiGauge.DrawGauge(stiGdiGaugeContextPainter);
				stiGauge.IsAnimation = isAnimation;
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
		Matrix val3 = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
		val3.Translate((float)pp.X, (float)(pp.Y + pp.Height));
		val3.Scale(1f, -1f);
		val3.Scale(0.75f, 0.75f);
		StiPdfGeomWriter stiPdfGeomWriter = new StiPdfGeomWriter(pageStream, this, assemble);
		stiPdfGeomWriter.pageNumber = pageNumber;
		stiPdfGeomWriter.matrixCache.Push(val3);
		foreach (StiGaugeGeom geom in stiGdiGaugeContextPainter.Geoms)
		{
			if (geom is StiPushMatrixGaugeGeom)
			{
				StiPushMatrixGaugeGeom stiPushMatrixGaugeGeom = geom as StiPushMatrixGaugeGeom;
				stiPdfGeomWriter.SaveState();
				stiPdfGeomWriter.RotateTransform(stiPushMatrixGaugeGeom.Angle, stiPushMatrixGaugeGeom.CenterPoint.X, stiPushMatrixGaugeGeom.CenterPoint.Y);
				stiPdfGeomWriter.SaveState();
				stiPdfGeomWriter.TranslateTransform(0f - stiPushMatrixGaugeGeom.CenterPoint.X, 0f - stiPushMatrixGaugeGeom.CenterPoint.Y);
			}
			else if (geom is StiPopTranformGaugeGeom)
			{
				stiPdfGeomWriter.RestoreState();
				stiPdfGeomWriter.RestoreState();
			}
			else if (geom.Type == StiGaugeGeomType.RoundedRectangle)
			{
				StiRoundedRectangleGaugeGeom stiRoundedRectangleGaugeGeom = geom as StiRoundedRectangleGaugeGeom;
				stiPdfGeomWriter.BeginPath();
				int num2 = 1;
				int rightTop = stiRoundedRectangleGaugeGeom.RightTop;
				int leftTop = stiRoundedRectangleGaugeGeom.LeftTop;
				int rightBottom = stiRoundedRectangleGaugeGeom.RightBottom;
				int leftBottom = stiRoundedRectangleGaugeGeom.LeftBottom;
				RectangleF rect = stiRoundedRectangleGaugeGeom.Rect;
				float num3 = rect.X + rect.Width - (float)num2;
				float num4 = rect.Y + rect.Height - (float)num2;
				stiPdfGeomWriter.MoveTo(new PointF(rect.X + (float)leftTop, rect.Y));
				if (rightTop != 0)
				{
					stiPdfGeomWriter.DrawLineTo(new PointF(num3 - (float)rightTop, rect.Y), null);
					stiPdfGeomWriter.DrawArc(new RectangleF(num3 - (float)rightTop, rect.Y, rightTop * 2, rightTop * 2), 270f, 90f);
				}
				else
				{
					stiPdfGeomWriter.DrawLineTo(new PointF(num3, rect.Y), null);
				}
				if (rightBottom != 0)
				{
					stiPdfGeomWriter.DrawLineTo(new PointF(num3, num4 - (float)rightBottom), null);
					stiPdfGeomWriter.DrawArc(new RectangleF(num3 - (float)rightBottom, num4 - (float)rightBottom, rightBottom * 2, rightBottom * 2), 0f, 90f);
				}
				else
				{
					stiPdfGeomWriter.DrawLineTo(new PointF(num3, num4), null);
				}
				if (leftBottom != 0)
				{
					stiPdfGeomWriter.DrawLineTo(new PointF(rect.X + (float)leftBottom, num4), null);
					stiPdfGeomWriter.DrawArc(new RectangleF(rect.X - (float)leftBottom, num4 - (float)leftBottom, leftBottom * 2, leftBottom * 2), 90f, 90f);
				}
				else
				{
					stiPdfGeomWriter.DrawLineTo(new PointF(rect.X, num4), null);
				}
				stiPdfGeomWriter.CloseFigure();
				stiPdfGeomWriter.EndPath();
				if (!StiBrush.IsTransparent(stiRoundedRectangleGaugeGeom.Background))
				{
					stiPdfGeomWriter.FillPath(stiRoundedRectangleGaugeGeom.Background);
				}
				StiPenGeom penGeom = GetPenGeom(stiRoundedRectangleGaugeGeom);
				if (CheckPenGeom(penGeom))
				{
					stiPdfGeomWriter.StrokePath(penGeom);
				}
			}
			else if (geom.Type == StiGaugeGeomType.Rectangle)
			{
				StiRectangleGaugeGeom stiRectangleGaugeGeom = geom as StiRectangleGaugeGeom;
				if (stiRectangleGaugeGeom.Background != null)
				{
					stiPdfGeomWriter.FillRectangle(RectToRectangleF(stiRectangleGaugeGeom.Rect), stiRectangleGaugeGeom.Background);
				}
				StiPenGeom penGeom2 = GetPenGeom(stiRectangleGaugeGeom);
				if (CheckPenGeom(penGeom2))
				{
					stiPdfGeomWriter.DrawRectangle(RectToRectangleF(stiRectangleGaugeGeom.Rect), penGeom2);
				}
			}
			else if (geom.Type == StiGaugeGeomType.GraphicsPath)
			{
				StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = geom as StiGraphicsPathGaugeGeom;
				stiPdfGeomWriter.BeginPath();
				bool flag = false;
				foreach (StiGaugeGeom geom2 in stiGraphicsPathGaugeGeom.Geoms)
				{
					if (geom2.Type == StiGaugeGeomType.GraphicsPathArc)
					{
						StiGraphicsPathArcGaugeGeom stiGraphicsPathArcGaugeGeom = geom2 as StiGraphicsPathArcGaugeGeom;
						stiPdfGeomWriter.DrawArc(new RectangleF(stiGraphicsPathArcGaugeGeom.X, stiGraphicsPathArcGaugeGeom.Y, stiGraphicsPathArcGaugeGeom.Width, stiGraphicsPathArcGaugeGeom.Height), stiGraphicsPathArcGaugeGeom.StartAngle, stiGraphicsPathArcGaugeGeom.SweepAngle);
					}
					else if (geom2.Type == StiGaugeGeomType.GraphicsPathLine)
					{
						StiGraphicsPathLineGaugeGeom stiGraphicsPathLineGaugeGeom = geom2 as StiGraphicsPathLineGaugeGeom;
						stiPdfGeomWriter.DrawLine(stiGraphicsPathLineGaugeGeom.P1, stiGraphicsPathLineGaugeGeom.P2, null);
					}
					else if (geom2.Type == StiGaugeGeomType.GraphicsPathLines)
					{
						StiGraphicsPathLinesGaugeGeom stiGraphicsPathLinesGaugeGeom = geom2 as StiGraphicsPathLinesGaugeGeom;
						stiPdfGeomWriter.DrawPolyline(stiGraphicsPathLinesGaugeGeom.Points, null);
						stiPdfGeomWriter.CloseFigure();
						flag = true;
					}
					else if (geom2.Type == StiGaugeGeomType.GraphicsPathCloseFigure)
					{
						stiPdfGeomWriter.CloseFigure();
						flag = true;
					}
				}
				stiPdfGeomWriter.EndPath();
				if (flag && stiGraphicsPathGaugeGeom.Background != null)
				{
					stiPdfGeomWriter.FillPath(stiGraphicsPathGaugeGeom.Background);
				}
				StiPenGeom penGeom3 = GetPenGeom(stiGraphicsPathGaugeGeom);
				if (CheckPenGeom(penGeom3))
				{
					stiPdfGeomWriter.StrokePath(penGeom3);
				}
			}
			else if (geom.Type == StiGaugeGeomType.Pie)
			{
				StiPieGaugeGeom stiPieGaugeGeom = geom as StiPieGaugeGeom;
				stiPdfGeomWriter.BeginPath();
				stiPdfGeomWriter.DrawPie(stiPieGaugeGeom.Rect, stiPieGaugeGeom.StartAngle, stiPieGaugeGeom.SweepAngle);
				stiPdfGeomWriter.CloseFigure();
				stiPdfGeomWriter.EndPath();
				StiPenGeom penGeom4 = GetPenGeom(stiPieGaugeGeom);
				if (CheckPenGeom(penGeom4))
				{
					stiPdfGeomWriter.StrokePath(penGeom4);
				}
			}
			else if (geom.Type == StiGaugeGeomType.Ellipse)
			{
				StiEllipseGaugeGeom stiEllipseGaugeGeom = geom as StiEllipseGaugeGeom;
				if (stiEllipseGaugeGeom.Background != null)
				{
					stiPdfGeomWriter.FillEllipse(RectToRectangleF(stiEllipseGaugeGeom.Rect), stiEllipseGaugeGeom.Background);
				}
				StiPenGeom penGeom5 = GetPenGeom(stiEllipseGaugeGeom);
				if (CheckPenGeom(penGeom5))
				{
					stiPdfGeomWriter.DrawEllipse(RectToRectangleF(stiEllipseGaugeGeom.Rect), penGeom5);
				}
			}
			else if (geom.Type == StiGaugeGeomType.GraphicsArcGeometry)
			{
				StiGraphicsArcGeometryGaugeGeom stiGraphicsArcGeometryGaugeGeom = geom as StiGraphicsArcGeometryGaugeGeom;
				stiPdfGeomWriter.BeginPath();
				stiPdfGeomWriter.DrawArc(stiGraphicsArcGeometryGaugeGeom.Rect, stiGraphicsArcGeometryGaugeGeom.StartAngle, stiGraphicsArcGeometryGaugeGeom.SweepAngle);
				stiPdfGeomWriter.EndPath();
				StiPenGeom penGeom6 = GetPenGeom(stiGraphicsArcGeometryGaugeGeom);
				if (CheckPenGeom(penGeom6))
				{
					stiPdfGeomWriter.StrokePath(penGeom6);
				}
			}
			else if (geom.Type == StiGaugeGeomType.Text)
			{
				StiTextGaugeGeom stiTextGaugeGeom = geom as StiTextGaugeGeom;
				Font font = new Font(StiFontCollection.GetFontFamily(stiTextGaugeGeom.Font.Name, false), stiTextGaugeGeom.Font.Size, stiTextGaugeGeom.Font.Style, stiTextGaugeGeom.Font.Unit, stiTextGaugeGeom.Font.GdiCharSet, stiTextGaugeGeom.Font.GdiVerticalFont);
				StringFormat val4 = new StringFormat();
				val4.Alignment = stiTextGaugeGeom.StringFormat.Alignment;
				val4.FormatFlags = stiTextGaugeGeom.StringFormat.FormatFlags;
				val4.HotkeyPrefix = stiTextGaugeGeom.StringFormat.HotkeyPrefix;
				val4.LineAlignment = stiTextGaugeGeom.StringFormat.LineAlignment;
				val4.Trimming = stiTextGaugeGeom.StringFormat.Trimming;
				bool allowHtmlTags = StiOptions.Engine.AllowHtmlTagsInChart && StiTextRenderer.CheckTextForHtmlTags(stiTextGaugeGeom.Text);
				stiPdfGeomWriter.DrawString(stiTextGaugeGeom.Text, font, stiTextGaugeGeom.Foreground, stiTextGaugeGeom.Rect, val4, allowHtmlTags);
			}
			else
			{
				if (geom.Type != StiGaugeGeomType.RadialRange)
				{
					continue;
				}
				StiRadialRangeGaugeGeom stiRadialRangeGaugeGeom = geom as StiRadialRangeGaugeGeom;
				PointF centerPoint = stiRadialRangeGaugeGeom.CenterPoint;
				float startAngle = stiRadialRangeGaugeGeom.StartAngle;
				float sweepAngle = stiRadialRangeGaugeGeom.SweepAngle;
				float radius = stiRadialRangeGaugeGeom.Radius1;
				float radius2 = stiRadialRangeGaugeGeom.Radius2;
				float radius3 = stiRadialRangeGaugeGeom.Radius3;
				float radius4 = stiRadialRangeGaugeGeom.Radius4;
				float num5 = startAngle * StiDrawingHelper.PiDiv180;
				float x = centerPoint.X + radius * (float)Math.Cos(num5);
				float y = centerPoint.Y + radius * (float)Math.Sin(num5);
				PointF point = new PointF(x, y);
				float num6 = Round(Math.Abs(sweepAngle / 90f));
				float num7 = sweepAngle / num6;
				num5 = startAngle;
				stiPdfGeomWriter.BeginPath();
				stiPdfGeomWriter.MoveTo(point);
				float num8 = radius - radius2;
				float num9 = 1f / num6;
				float num10 = 0f;
				num5 = startAngle;
				for (int i = 0; (float)i < num6; i++)
				{
					float radius5 = radius - num8 * num10;
					float radius6 = radius - num8 * (num10 + num9);
					List<PointF> list = StiDrawingHelper.ConvertArcToCubicBezier(centerPoint, radius5, radius6, num5, num7);
					stiPdfGeomWriter.DrawBezierTo(list[1], list[2], list[3], null);
					num5 += num7;
					num10 += num9;
				}
				num7 = sweepAngle / num6;
				num8 = radius3 - radius4;
				num9 = 1f / num6;
				num10 = 0f;
				bool flag2 = true;
				List<PointF> list2 = new List<PointF>();
				for (int j = 0; (float)j < num6; j++)
				{
					float radius7 = radius3 - num8 * num10;
					float radius8 = radius3 - num8 * (num10 + num9);
					List<PointF> list3 = StiDrawingHelper.ConvertArcToCubicBezier(centerPoint, radius8, radius7, num5, 0f - num7);
					if (flag2)
					{
						stiPdfGeomWriter.DrawLineTo(list3[0], null);
						flag2 = false;
					}
					stiPdfGeomWriter.DrawBezierTo(list3[1], list3[2], list3[3], null);
					num5 -= num7;
					num10 -= num9;
				}
				stiPdfGeomWriter.CloseFigure();
				stiPdfGeomWriter.EndPath();
				if (!StiBrush.IsTransparent(stiRadialRangeGaugeGeom.Background))
				{
					stiPdfGeomWriter.FillPath(stiRadialRangeGaugeGeom.Background);
				}
				StiPenGeom penGeom7 = GetPenGeom(stiRadialRangeGaugeGeom);
				if (CheckPenGeom(penGeom7))
				{
					stiPdfGeomWriter.StrokePath(penGeom7);
				}
			}
		}
	}

	private StiPenGeom GetPenGeom(StiBrush brush, float width)
	{
		return new StiPenGeom(brush, width);
	}

	private StiPenGeom GetPenGeom(StiGaugeGeom geom)
	{
		if (geom is StiRectangleGaugeGeom)
		{
			return GetPenGeom((geom as StiRectangleGaugeGeom).BorderBrush, (geom as StiRectangleGaugeGeom).BorderWidth);
		}
		if (geom is StiRoundedRectangleGaugeGeom)
		{
			return GetPenGeom((geom as StiRoundedRectangleGaugeGeom).BorderBrush, (geom as StiRoundedRectangleGaugeGeom).BorderWidth);
		}
		if (geom is StiEllipseGaugeGeom)
		{
			return GetPenGeom((geom as StiEllipseGaugeGeom).BorderBrush, (geom as StiEllipseGaugeGeom).BorderWidth);
		}
		if (geom is StiGraphicsPathGaugeGeom)
		{
			return GetPenGeom((geom as StiGraphicsPathGaugeGeom).BorderBrush, (geom as StiGraphicsPathGaugeGeom).BorderWidth);
		}
		if (geom is StiPieGaugeGeom)
		{
			return GetPenGeom((geom as StiPieGaugeGeom).BorderBrush, (geom as StiPieGaugeGeom).BorderWidth);
		}
		if (geom is StiGraphicsArcGeometryGaugeGeom)
		{
			return GetPenGeom((geom as StiGraphicsArcGeometryGaugeGeom).BorderBrush, (geom as StiGraphicsArcGeometryGaugeGeom).BorderWidth);
		}
		if (geom is StiRadialRangeGaugeGeom)
		{
			return GetPenGeom((geom as StiRadialRangeGaugeGeom).BorderBrush, (geom as StiRadialRangeGaugeGeom).BorderWidth);
		}
		return null;
	}

	private static float Round(float value)
	{
		int num = (int)value;
		float num2 = value - (float)num;
		if (!(num2 > 0f))
		{
			return num;
		}
		return num + 1;
	}

	private void RenderMap(StiPdfData pp, bool assemble, int pageNumber)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Expected O, but got Unknown
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		StiMap stiMap = pp.Component as StiMap;
		StiGdiMapContextPainter stiGdiMapContextPainter = new StiGdiMapContextPainter(stiMap)
		{
			MapStyle = StiMap.GetMapStyle(stiMap),
			DataTable = stiMap.DataTable
		};
		stiGdiMapContextPainter.PrepareDataColumns();
		stiGdiMapContextPainter.UpdateGroupedData();
		stiGdiMapContextPainter.UpdateHeatmapWithGroup();
		StiBrush val = (StiBrush)new StiSolidBrush(stiGdiMapContextPainter.MapStyle.DefaultColor);
		StiMapGeomsContainer geomsObject = StiMapLoader.GetGeomsObject(stiMap.Report, stiMap.MapIdent, stiMap.Language);
		bool flag = false;
		bool flag2 = true;
		float num = 1f;
		if (stiMap.Stretch)
		{
			RectangleD paintRectangle = stiMap.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			num = Math.Min((float)((RectangleD)(ref paintRectangle)).Width / (float)geomsObject.Width, (float)((RectangleD)(ref paintRectangle)).Height / (float)geomsObject.Height);
			if (num == 0f)
			{
				num = 1f;
			}
		}
		Matrix val2 = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
		val2.Translate((float)pp.X, (float)(pp.Y + pp.Height));
		val2.Scale(1f, -1f);
		val2.Scale(0.75f, 0.75f);
		StiPdfGeomWriter stiPdfGeomWriter = new StiPdfGeomWriter(pageStream, this, assemble);
		stiPdfGeomWriter.pageNumber = pageNumber;
		stiPdfGeomWriter.matrixCache.Push(val2);
		stiPdfGeomWriter.SaveState();
		stiPdfGeomWriter.ScaleTransform(num * 0.96f, num * 0.96f);
		if (stiMap.Stretch && flag2)
		{
			RectangleD paintRectangle2 = stiMap.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			double num2 = ((RectangleD)(ref paintRectangle2)).Width / (double)num;
			double num3 = ((RectangleD)(ref paintRectangle2)).Height / (double)num;
			stiPdfGeomWriter.TranslateTransform((float)((num2 - geomsObject.Width) / 2.0), (float)((num3 - geomsObject.Height) / 2.0));
		}
		List<string> list = new List<string>();
		float num5 = (stiGdiMapContextPainter.IndividualStep = 0.5f / (float)geomsObject.Geoms.Count);
		foreach (StiMapGeomsObject geoms in geomsObject.Geoms.OrderBy((StiMapGeomsObject x) => x.Key))
		{
			StiMapData data = stiGdiMapContextPainter.MapData.FirstOrDefault((StiMapData x) => x.Key == geoms.Key);
			int num6 = 0;
			StiBrush val3 = stiGdiMapContextPainter.GetGeomBrush1(data);
			if (val3 == null)
			{
				val3 = val;
			}
			stiGdiMapContextPainter.IndividualStep += num5;
			foreach (StiMapGeom geom in geoms.Geoms)
			{
				switch (geom.GeomType)
				{
				case StiMapGeomType.MoveTo:
				{
					if (num6 > 0)
					{
						FillMapPath(stiPdfGeomWriter, stiGdiMapContextPainter.IsBorderEmpty, val3, stiGdiMapContextPainter.MapStyle.BorderColor, stiGdiMapContextPainter.MapStyle.BorderSize);
						num6 = 0;
					}
					stiPdfGeomWriter.BeginPath();
					StiMoveToMapGeom stiMoveToMapGeom = (StiMoveToMapGeom)geom;
					stiPdfGeomWriter.MoveTo(new PointF((float)stiMoveToMapGeom.X, (float)stiMoveToMapGeom.Y));
					break;
				}
				case StiMapGeomType.Line:
				{
					StiLineMapGeom stiLineMapGeom = (StiLineMapGeom)geom;
					PointF pointTo = new PointF((float)stiLineMapGeom.X, (float)stiLineMapGeom.Y);
					stiPdfGeomWriter.DrawLineTo(pointTo, null);
					num6++;
					break;
				}
				case StiMapGeomType.Bezier:
				{
					StiBezierMapGeom stiBezierMapGeom = (StiBezierMapGeom)geom;
					PointF p = new PointF((float)stiBezierMapGeom.X1, (float)stiBezierMapGeom.Y1);
					PointF p2 = new PointF((float)stiBezierMapGeom.X2, (float)stiBezierMapGeom.Y2);
					PointF p3 = new PointF((float)stiBezierMapGeom.X3, (float)stiBezierMapGeom.Y3);
					stiPdfGeomWriter.DrawBezierTo(p, p2, p3, null);
					num6++;
					break;
				}
				case StiMapGeomType.Beziers:
				{
					StiBeziersMapGeom stiBeziersMapGeom = (StiBeziersMapGeom)geom;
					List<PointF> list2 = new List<PointF>();
					PointF item = PointF.Empty;
					for (int i = 0; i < stiBeziersMapGeom.Array.Length; i += 2)
					{
						item = new PointF((float)stiBeziersMapGeom.Array[i], (float)stiBeziersMapGeom.Array[i + 1]);
						list2.Add(item);
					}
					list2.Add(item);
					if (list2.Count % 3 == 2)
					{
						list2.RemoveAt(list2.Count - 1);
					}
					if (list2.Count % 3 == 1)
					{
						list2.RemoveAt(list2.Count - 1);
					}
					for (int j = 0; j < list2.Count; j += 3)
					{
						stiPdfGeomWriter.DrawBezierTo(list2[j], list2[j + 1], list2[j + 2], null);
					}
					break;
				}
				case StiMapGeomType.Close:
					num6 = 0;
					stiPdfGeomWriter.CloseFigure();
					FillMapPath(stiPdfGeomWriter, stiGdiMapContextPainter.IsBorderEmpty, val3, stiGdiMapContextPainter.MapStyle.BorderColor, stiGdiMapContextPainter.MapStyle.BorderSize);
					break;
				}
			}
			if (num6 > 0)
			{
				num6 = 0;
				FillMapPath(stiPdfGeomWriter, stiGdiMapContextPainter.IsBorderEmpty, val3, stiGdiMapContextPainter.MapStyle.BorderColor, stiGdiMapContextPainter.MapStyle.BorderSize);
			}
			if (stiMap.ShowValue || stiMap.DisplayNameType != StiDisplayNameType.None)
			{
				list.Add(geoms.Key);
			}
		}
		if (list.Count > 0 && !flag)
		{
			StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(stiMap.Report, stiMap.MapIdent, stiMap.Language);
			float num7 = 0.75f * ((num > 1f) ? 1f : num);
			Font val4 = new Font("Calibri", 18f * num7);
			SolidBrush val5 = new SolidBrush(Color.FromArgb(255, 37, 37, 37));
			SolidBrush val6 = new SolidBrush(Color.FromArgb(180, 251, 251, 251));
			StiSolidBrush textBrush = new StiSolidBrush(Color.FromArgb(255, 37, 37, 37));
			StiSolidBrush textBrush2 = new StiSolidBrush(Color.FromArgb(180, 251, 251, 251));
			foreach (string key in list)
			{
				StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key];
				if (stiMapSvg.SkipText)
				{
					continue;
				}
				string text = null;
				StiMapData stiMapData = stiGdiMapContextPainter.MapData.FirstOrDefault((StiMapData x) => x.Key == key);
				switch (stiMap.DisplayNameType)
				{
				case StiDisplayNameType.Full:
					text = ((stiMapData != null) ? stiMapData.Name : stiMapSvg.EnglishName);
					break;
				case StiDisplayNameType.Short:
					text = StiMapHelper.PrepareIsoCode(stiMapSvg.ISOCode);
					break;
				}
				if (stiMap.ShowValue && stiMapData != null && stiMapData.Value != null)
				{
					string text2 = null;
					if (stiMap.ShortValue)
					{
						double result = 0.0;
						if (double.TryParse(stiMapData.Value, out result))
						{
							text2 = StiAbbreviationNumberFormatHelper.Format(result);
						}
					}
					if (text2 == null)
					{
						text2 = stiMapData.Value;
					}
					if (text == null)
					{
						text = text2;
					}
					else
					{
						text += Environment.NewLine;
						text += text2;
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				if (assemble)
				{
					int fontNumber = pdfFont.GetFontNumber(val4);
					StringBuilder sb = new StringBuilder(StiBidirectionalConvert2.ConvertString(text, rightToLeft: false));
					pdfFont.StoreUnicodeSymbolsInMap(sb);
					continue;
				}
				Rectangle rect = stiMapSvg.Rect;
				StiText stiText = new StiText(new RectangleD(1.0, 0.0, (double)rect.Width, (double)rect.Height), text);
				stiText.Font = val4;
				stiText.HorAlignment = stiMapSvg.HorAlignment;
				stiText.VertAlignment = stiMapSvg.VertAlignment;
				if (stiMapSvg.SetMaxWidth)
				{
					stiText.WordWrap = true;
				}
				StiPdfData pp2 = default(StiPdfData);
				pp2.Component = stiText;
				pp2.X = 1.0;
				pp2.Y = 0.0;
				pp2.Width = (double)rect.Width * 0.72;
				pp2.Height = (double)rect.Height * 0.72;
				pageStream.WriteLine("q");
				PushColorToStack();
				pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(rect.X), ConvertToString(rect.Y + rect.Height));
				pageStream.WriteLine("1 0 0 -1 0 0 cm");
				pageStream.WriteLine("1.39 0 0 1.39 0 0 cm");
				RenderTextFont(pp2);
				stiText.TextBrush = (StiBrush)(object)textBrush2;
				RenderText(pp2);
				pp2.X -= 1.0;
				pp2.Y += 1.0;
				stiText.TextBrush = (StiBrush)(object)textBrush;
				RenderText(pp2);
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
		}
		stiPdfGeomWriter.RestoreState();
	}

	private void FillMapPath(StiPdfGeomWriter pdfGeomWriter, bool isBorderEmpty, StiBrush fillBrush, Color borderColor, double borderSize)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		pdfGeomWriter.FillPath(StiBrush.ToColor(fillBrush));
		if (!isBorderEmpty)
		{
			Pen val = new Pen(borderColor, (float)borderSize);
			try
			{
				pdfGeomWriter.StrokePath(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	private void RenderWatermark(StiPage page, bool behind, double pageWidth, double pageHeight, float imageResolution)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		StiWatermark watermark = page.Watermark;
		if (watermark != null && watermark.Enabled)
		{
			if (watermarkImageExist.ContainsKey(page) && watermark.ShowImageBehind == behind)
			{
				RenderWatermarkImage(new RectangleD(0.0, 0.0, pageWidth, pageHeight), watermark.ImageStretch, watermark.ImageTiling, watermark.AspectRatio, watermark.ImageMultipleFactor, watermark.ImageAlignment);
			}
			if (!string.IsNullOrEmpty(watermark.Text) && watermark.ShowBehind == behind)
			{
				StiPdfData pp = default(StiPdfData);
				pp.X = 0.0;
				pp.Y = 0.0;
				pp.Width = pageWidth;
				pp.Height = pageHeight;
				StiText stiText = new StiText(new RectangleD(pp.X, pp.Y, pp.Width, pp.Height));
				stiText.Text = watermark.Text;
				stiText.TextBrush = watermark.TextBrush;
				stiText.Font = watermark.Font;
				stiText.TextOptions = new StiTextOptions();
				stiText.TextOptions.Angle = watermark.Angle;
				stiText.HorAlignment = (StiTextHorAlignment)1;
				stiText.VertAlignment = (StiVertAlignment)1;
				stiText.Page = page;
				stiText.TextQuality = StiTextQuality.Standard;
				pp.Component = stiText;
				RenderTextFont(pp);
				RenderText(pp);
			}
			object tagValue = page.TagValue;
			StiAdvancedWatermark val = (StiAdvancedWatermark)((tagValue is StiAdvancedWatermark) ? tagValue : null);
			if (val != null && val.WeaveEnabled && behind)
			{
				RenderWatermarkWeave(val, new RectangleD(0.0, 0.0, pageWidth, pageHeight), page);
			}
		}
	}

	private void RenderWatermarkImage(RectangleD mainRect, bool imageStretch, bool isImageTiling, bool imageAspectRatio, double imageMultipleFactor, ContentAlignment imageAlignment)
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Invalid comparison between Unknown and I4
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Invalid comparison between Unknown and I4
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Invalid comparison between Unknown and I4
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Invalid comparison between Unknown and I4
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Invalid comparison between Unknown and I4
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Invalid comparison between Unknown and I4
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Invalid comparison between Unknown and I4
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Invalid comparison between Unknown and I4
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Invalid comparison between Unknown and I4
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Invalid comparison between Unknown and I4
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Invalid comparison between Unknown and I4
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Invalid comparison between Unknown and I4
		StiImageData stiImageData = (StiImageData)imageList[imagesCurrent];
		double num = (double)stiImageData.Width * 0.72 * imageMultipleFactor;
		double num2 = (double)stiImageData.Height * 0.72 * imageMultipleFactor;
		double num3 = ((RectangleD)(ref mainRect)).Left;
		double num4 = ((RectangleD)(ref mainRect)).Top;
		int num5 = 1;
		int num6 = 1;
		if (imageStretch)
		{
			double num7 = num2 / num;
			num = ((RectangleD)(ref mainRect)).Width;
			num2 = ((RectangleD)(ref mainRect)).Height;
			isImageTiling = false;
			if (imageAspectRatio)
			{
				if (((RectangleD)(ref mainRect)).Height / ((RectangleD)(ref mainRect)).Width > num7)
				{
					num2 = num * num7;
				}
				else
				{
					num = num2 / num7;
				}
			}
		}
		if (imageStretch || imageMultipleFactor > 1.0)
		{
			imageInterpolationTable[(int)imageCache.ImageIndex[imagesCurrent]] = true;
		}
		if (isImageTiling)
		{
			imageAlignment = (ContentAlignment)1;
			num5 = (int)(((RectangleD)(ref mainRect)).Width / num) + 1;
			num6 = (int)(((RectangleD)(ref mainRect)).Height / num2) + 1;
		}
		if ((int)imageAlignment <= 32)
		{
			if ((int)imageAlignment != 2)
			{
				if ((int)imageAlignment == 4)
				{
					goto IL_0143;
				}
				if ((int)imageAlignment != 32)
				{
					goto IL_014f;
				}
			}
			goto IL_012b;
		}
		if ((int)imageAlignment != 64)
		{
			if ((int)imageAlignment == 512)
			{
				goto IL_012b;
			}
			if ((int)imageAlignment != 1024)
			{
				goto IL_014f;
			}
		}
		goto IL_0143;
		IL_012b:
		num3 += (((RectangleD)(ref mainRect)).Width - num) / 2.0;
		goto IL_014f;
		IL_0143:
		num3 += ((RectangleD)(ref mainRect)).Width - num;
		goto IL_014f;
		IL_014f:
		if ((int)imageAlignment <= 4)
		{
			if (imageAlignment - 1 <= 1 || (int)imageAlignment == 4)
			{
				num4 += ((RectangleD)(ref mainRect)).Height - num2;
			}
		}
		else if ((int)imageAlignment == 16 || (int)imageAlignment == 32 || (int)imageAlignment == 64)
		{
			num4 += (((RectangleD)(ref mainRect)).Height - num2) / 2.0;
		}
		SetNonStrokeColor(Color.Black);
		for (int i = 0; i < num6; i++)
		{
			for (int j = 0; j < num5; j++)
			{
				pageStream.WriteLine("q");
				PushColorToStack();
				pageStream.WriteLine("{0} 0 0 {1} {2} {3} cm", ConvertToString(num), ConvertToString(num2), ConvertToString(num3 + num * (double)j), ConvertToString(num4 - num2 * (double)i));
				pageStream.WriteLine("/{0} Do", stiImageData.Name);
				pageStream.WriteLine("Q");
				PopColorFromStack();
			}
		}
		imagesCurrent++;
	}

	private void RenderWatermarkWeave(StiAdvancedWatermark advWatermark, RectangleD mainRect, StiPage page)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		if (advWatermark == null)
		{
			return;
		}
		StiText stiText = new StiText();
		stiText.HorAlignment = (StiTextHorAlignment)1;
		stiText.VertAlignment = (StiVertAlignment)1;
		stiText.TextQuality = StiTextQuality.Standard;
		stiText.Page = page;
		stiText.Border = new StiBorder((StiBorderSides)15, Color.Green, 3.0, (StiPenStyle)0);
		StiSolidBrush val = new StiSolidBrush(advWatermark.WeaveMajorColor);
		StiSolidBrush val2 = new StiSolidBrush(advWatermark.WeaveMinorColor);
		FontFamily val3 = StiFontIconsHelper.GetFontFamilyIcons();
		if (val3 == null)
		{
			val3 = StiFontCollection.GetFontFamily("Stimulsoft", false);
		}
		Font val4 = new Font(val3, (float)((double)(advWatermark.WeaveMajorSize * 5) * 0.72 * 0.85));
		Font val5 = new Font(val3, (float)((double)(advWatermark.WeaveMinorSize * 5) * 0.72 * 0.85));
		string content = StiFontIconsHelper.GetContent(advWatermark.WeaveMajorIcon);
		string content2 = StiFontIconsHelper.GetContent(advWatermark.WeaveMinorIcon);
		double num = (double)advWatermark.WeaveDistance * 0.72;
		StiPdfData pp = default(StiPdfData);
		pp.Component = stiText;
		pp.Width = num * 2.0;
		pp.Height = num * 2.0;
		int num2 = (int)(Math.Max(((RectangleD)(ref mainRect)).Width, ((RectangleD)(ref mainRect)).Height) / 2.0 * 1.4 / num) + 1;
		bool flag = clipLongTextLines;
		clipLongTextLines = false;
		pageStream.WriteLine("q");
		PushColorToStack();
		pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(((RectangleD)(ref mainRect)).X + ((RectangleD)(ref mainRect)).Width / 2.0), ConvertToString(((RectangleD)(ref mainRect)).Y + ((RectangleD)(ref mainRect)).Height / 2.0));
		if (advWatermark.WeaveAngle != 0)
		{
			double num3 = (double)(-advWatermark.WeaveAngle) * Math.PI / 180.0;
			pageStream.WriteLine("{0} {1} {2} {3} {4} {5} cm", ConvertToString(Math.Cos(num3)), ConvertToString(Math.Sin(num3)), ConvertToString(0.0 - Math.Sin(num3)), ConvertToString(Math.Cos(num3)), ConvertToString(0.0), ConvertToString(0.0));
		}
		for (int i = -num2; i <= num2; i++)
		{
			for (int j = -num2; j <= num2; j++)
			{
				int num4 = (int)((double)j * num);
				int num5 = (int)((double)i * num);
				bool flag2 = (Math.Abs(j + i) & 1) == 0;
				pageStream.WriteLine("q");
				pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(num4), ConvertToString(num5));
				stiText.ClientRectangle = new RectangleD(0.0 - num, 0.0 - num, num * 2.0, num * 2.0);
				stiText.TextBrush = (StiBrush)(object)(flag2 ? val : val2);
				stiText.Font = (flag2 ? val4 : val5);
				stiText.Text = (flag2 ? content : content2);
				pp.X = 0.0 - num;
				pp.Y = 0.0 - num;
				RenderTextFont(pp);
				RenderText(pp);
				pageStream.WriteLine("Q");
			}
		}
		pageStream.WriteLine("Q");
		PopColorFromStack();
		clipLongTextLines = flag;
	}

	internal void StoreShadingData1(StiBrush brush, int pageNumber)
	{
		if (brush != null)
		{
			if (brush is StiGradientBrush)
			{
				StiGradientBrush val = (StiGradientBrush)(object)((brush is StiGradientBrush) ? brush : null);
				StiShadingData stiShadingData = default(StiShadingData);
				stiShadingData.Angle = val.Angle;
				stiShadingData.Page = pageNumber;
				stiShadingData.FunctionIndex = GetShadingFunctionNumber(val.StartColor, val.EndColor, isGlare: false);
				shadingArray.Add(stiShadingData);
			}
			if (brush is StiGlareBrush)
			{
				StiGlareBrush val2 = (StiGlareBrush)(object)((brush is StiGlareBrush) ? brush : null);
				StiShadingData stiShadingData2 = default(StiShadingData);
				stiShadingData2.Angle = val2.Angle;
				stiShadingData2.FunctionIndex = GetShadingFunctionNumber(val2.StartColor, val2.EndColor, isGlare: true);
				stiShadingData2.Page = pageNumber;
				shadingArray.Add(stiShadingData2);
			}
		}
	}

	internal int StoreShadingData2(double x, double y, double width, double height, StiBrush brush)
	{
		if (brush != null && (brush is StiGradientBrush || brush is StiGlareBrush))
		{
			StiShadingData stiShadingData = (StiShadingData)shadingArray[shadingCurrent];
			stiShadingData.X = x;
			stiShadingData.Y = y;
			stiShadingData.Width = width;
			stiShadingData.Height = height;
			shadingArray[shadingCurrent] = stiShadingData;
			shadingCurrent++;
		}
		return shadingCurrent;
	}

	internal void StoreHatchData(StiBrush brush)
	{
		if (brush == null)
		{
			return;
		}
		if (brush is StiHatchBrush)
		{
			GetHatchNumber((StiHatchBrush)(object)((brush is StiHatchBrush) ? brush : null));
		}
		if (brush is StiGlassBrush)
		{
			StiGlassBrush val = (StiGlassBrush)(object)((brush is StiGlassBrush) ? brush : null);
			if (val.DrawHatch)
			{
				Brush topBrush = val.GetTopBrush();
				GetHatchNumber((HatchBrush)(object)((topBrush is HatchBrush) ? topBrush : null));
				Brush bottomBrush = val.GetBottomBrush();
				GetHatchNumber((HatchBrush)(object)((bottomBrush is HatchBrush) ? bottomBrush : null));
			}
		}
	}

	private void RenderMetadata(StiReport report)
	{
		MemoryStream memoryStream = new MemoryStream();
		string text = "<?xpacket begin=\"ï»¿\" id=\"W5M0MpCehiHzreSzNTczkc9d\"?>\r\n";
		byte[] array = new byte[text.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)text[i];
		}
		memoryStream.Write(array, 0, array.Length);
		MemoryStream memoryStream2 = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream2, Encoding.UTF8);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlTextWriter.WriteStartElement("x:xmpmeta");
		xmlTextWriter.WriteAttributeString("xmlns:x", "adobe:ns:meta/");
		xmlTextWriter.WriteAttributeString("x:xmptk", "Adobe XMP Core 4.0-c316 44.253921, Sun Oct 01 2006 17:14:39");
		xmlTextWriter.WriteStartElement("rdf:RDF");
		xmlTextWriter.WriteAttributeString("xmlns:rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
		xmlTextWriter.WriteStartElement("rdf:Description");
		xmlTextWriter.WriteAttributeString("rdf:about", "");
		xmlTextWriter.WriteAttributeString("xmlns:xap", "http://ns.adobe.com/xap/1.0/");
		xmlTextWriter.WriteElementString("xap:ModifyDate", currentDateTimeMeta);
		xmlTextWriter.WriteElementString("xap:CreateDate", currentDateTimeMeta);
		xmlTextWriter.WriteElementString("xap:MetadataDate", currentDateTimeMeta);
		xmlTextWriter.WriteElementString("xap:CreatorTool", GetCreatorString());
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("rdf:Description");
		xmlTextWriter.WriteAttributeString("rdf:about", "");
		xmlTextWriter.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
		xmlTextWriter.WriteElementString("dc:format", "application/pdf");
		xmlTextWriter.WriteStartElement("dc:title");
		xmlTextWriter.WriteStartElement("rdf:Alt");
		if (!string.IsNullOrEmpty(report.ReportName))
		{
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("xml:lang", "x-default");
			xmlTextWriter.WriteString(report.ReportName);
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("dc:description");
		xmlTextWriter.WriteStartElement("rdf:Alt");
		if (!string.IsNullOrEmpty(report.ReportAlias))
		{
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("xml:lang", "x-default");
			xmlTextWriter.WriteString(report.ReportAlias);
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("dc:creator");
		xmlTextWriter.WriteStartElement("rdf:Seq");
		if (!string.IsNullOrEmpty(report.ReportAuthor))
		{
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("xml:lang", "x-default");
			xmlTextWriter.WriteString(report.ReportAuthor);
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("dc:subject");
		xmlTextWriter.WriteStartElement("rdf:Bag");
		if (!string.IsNullOrEmpty(keywords))
		{
			string[] array2 = keywords.Split(new char[1] { ';' });
			string[] array3 = array2;
			foreach (string text2 in array3)
			{
				xmlTextWriter.WriteStartElement("rdf:li");
				xmlTextWriter.WriteString(text2.Trim());
				xmlTextWriter.WriteFullEndElement();
			}
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("rdf:Description");
		xmlTextWriter.WriteAttributeString("rdf:about", "");
		xmlTextWriter.WriteAttributeString("xmlns:xapMM", "http://ns.adobe.com/xap/1.0/mm/");
		xmlTextWriter.WriteElementString("xapMM:DocumentID", IDValueStringMeta);
		xmlTextWriter.WriteElementString("xapMM:InstanceID", IDValueStringMeta);
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteStartElement("rdf:Description");
		xmlTextWriter.WriteAttributeString("rdf:about", "");
		xmlTextWriter.WriteAttributeString("xmlns:pdf", "http://ns.adobe.com/pdf/1.3/");
		xmlTextWriter.WriteAttributeString("xmlns:pdfx", "http://ns.adobe.com/pdf/1.3/");
		xmlTextWriter.WriteElementString("pdf:Producer", producerName);
		xmlTextWriter.WriteElementString("pdf:Keywords", keywords);
		if (pdfComplianceMode == StiPdfComplianceMode.None)
		{
			foreach (StiMetaTag metaTag in report.MetaTags)
			{
				if (metaTag.Name.StartsWith("pdf:"))
				{
					string text3 = StiNameValidator.CorrectName(metaTag.Name.Substring(4), checkKeywords: false, report);
					if (!reservedKeywords.Contains(text3))
					{
						xmlTextWriter.WriteElementString("pdfx:" + text3, metaTag.Tag);
					}
				}
			}
		}
		xmlTextWriter.WriteFullEndElement();
		if (zugferdComplianceMode != 0)
		{
			xmlTextWriter.WriteStartElement("rdf:Description");
			xmlTextWriter.WriteAttributeString("xmlns:pdfaExtension", "http://www.aiim.org/pdfa/ns/extension/");
			xmlTextWriter.WriteAttributeString("xmlns:pdfaField", "http://www.aiim.org/pdfa/ns/field#");
			xmlTextWriter.WriteAttributeString("xmlns:pdfaProperty", "http://www.aiim.org/pdfa/ns/property#");
			xmlTextWriter.WriteAttributeString("xmlns:pdfaSchema", "http://www.aiim.org/pdfa/ns/schema#");
			xmlTextWriter.WriteAttributeString("xmlns:pdfaType", "http://www.aiim.org/pdfa/ns/type#");
			xmlTextWriter.WriteAttributeString("rdf:about", "");
			xmlTextWriter.WriteStartElement("pdfaExtension:schemas");
			xmlTextWriter.WriteStartElement("rdf:Bag");
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("rdf:parseType", "Resource");
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V1)
			{
				xmlTextWriter.WriteElementString("pdfaSchema:schema", "ZUGFeRD PDFA Extension Schema");
				xmlTextWriter.WriteElementString("pdfaSchema:namespaceURI", "urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#");
				xmlTextWriter.WriteElementString("pdfaSchema:prefix", "zf");
			}
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2)
			{
				xmlTextWriter.WriteElementString("pdfaSchema:schema", "ZUGFeRD PDFA Extension Schema");
				xmlTextWriter.WriteElementString("pdfaSchema:namespaceURI", "urn:zugferd:pdfa:CrossIndustryDocument:invoice:2p0#");
				xmlTextWriter.WriteElementString("pdfaSchema:prefix", "zf");
			}
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2_1)
			{
				xmlTextWriter.WriteElementString("pdfaSchema:schema", "Factur-X/ZUGFeRD PDF/A Extension Schema");
				xmlTextWriter.WriteElementString("pdfaSchema:namespaceURI", "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#");
				xmlTextWriter.WriteElementString("pdfaSchema:prefix", "fx");
			}
			xmlTextWriter.WriteStartElement("pdfaSchema:property");
			xmlTextWriter.WriteStartElement("rdf:Seq");
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("rdf:parseType", "Resource");
			xmlTextWriter.WriteElementString("pdfaProperty:name", "DocumentFileName");
			xmlTextWriter.WriteElementString("pdfaProperty:valueType", "Text");
			xmlTextWriter.WriteElementString("pdfaProperty:category", "external");
			xmlTextWriter.WriteElementString("pdfaProperty:description", "Name of the embedded XML invoice file");
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("rdf:parseType", "Resource");
			xmlTextWriter.WriteElementString("pdfaProperty:name", "DocumentType");
			xmlTextWriter.WriteElementString("pdfaProperty:valueType", "Text");
			xmlTextWriter.WriteElementString("pdfaProperty:category", "external");
			xmlTextWriter.WriteElementString("pdfaProperty:description", "INVOICE");
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("rdf:parseType", "Resource");
			xmlTextWriter.WriteElementString("pdfaProperty:name", "Version");
			xmlTextWriter.WriteElementString("pdfaProperty:valueType", "Text");
			xmlTextWriter.WriteElementString("pdfaProperty:category", "external");
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2_1)
			{
				xmlTextWriter.WriteElementString("pdfaProperty:description", "Version of the Factur-X/ZUGFeRD XML schema");
			}
			else
			{
				xmlTextWriter.WriteElementString("pdfaProperty:description", "The actual version of the ZUGFeRD data");
			}
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteStartElement("rdf:li");
			xmlTextWriter.WriteAttributeString("rdf:parseType", "Resource");
			xmlTextWriter.WriteElementString("pdfaProperty:name", "ConformanceLevel");
			xmlTextWriter.WriteElementString("pdfaProperty:valueType", "Text");
			xmlTextWriter.WriteElementString("pdfaProperty:category", "external");
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2_1)
			{
				xmlTextWriter.WriteElementString("pdfaProperty:description", "Conformance level of the embedded Factur-X/ZUGFeRD XML invoice data");
			}
			else
			{
				xmlTextWriter.WriteElementString("pdfaProperty:description", "The conformance level of the ZUGFeRD data");
			}
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteFullEndElement();
			xmlTextWriter.WriteStartElement("rdf:Description");
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V1)
			{
				xmlTextWriter.WriteAttributeString("xmlns:zf", "urn:ferd:pdfa:CrossIndustryDocument:invoice:1p0#");
				xmlTextWriter.WriteAttributeString("zf:DocumentFileName", "ZUGFeRD-invoice.xml");
				xmlTextWriter.WriteAttributeString("zf:ConformanceLevel", zugferdConformanceLevel);
				xmlTextWriter.WriteAttributeString("zf:DocumentType", "INVOICE");
				xmlTextWriter.WriteAttributeString("zf:Version", "1.0");
			}
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2)
			{
				xmlTextWriter.WriteAttributeString("xmlns:zf", "urn:zugferd:pdfa:CrossIndustryDocument:invoice:2p0#");
				xmlTextWriter.WriteAttributeString("zf:DocumentFileName", "zugferd-invoice.xml");
				xmlTextWriter.WriteAttributeString("zf:ConformanceLevel", zugferdConformanceLevel);
				xmlTextWriter.WriteAttributeString("zf:DocumentType", "INVOICE");
				xmlTextWriter.WriteAttributeString("zf:Version", "1.0");
			}
			if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2_1)
			{
				xmlTextWriter.WriteAttributeString("xmlns:fx", "urn:factur-x:pdfa:CrossIndustryDocument:invoice:1p0#");
				xmlTextWriter.WriteAttributeString("fx:DocumentFileName", "factur-x.xml");
				xmlTextWriter.WriteAttributeString("fx:ConformanceLevel", zugferdConformanceLevel);
				xmlTextWriter.WriteAttributeString("fx:DocumentType", "INVOICE");
				xmlTextWriter.WriteAttributeString("fx:Version", "1.0");
			}
			xmlTextWriter.WriteAttributeString("rdf:about", "");
			xmlTextWriter.WriteFullEndElement();
		}
		if (usePdfA)
		{
			xmlTextWriter.WriteStartElement("rdf:Description");
			xmlTextWriter.WriteAttributeString("rdf:about", "");
			xmlTextWriter.WriteAttributeString("xmlns:pdfaid", "http://www.aiim.org/pdfa/ns/id/");
			string value = "1";
			switch (pdfComplianceMode)
			{
			case StiPdfComplianceMode.A1:
				value = "1";
				break;
			case StiPdfComplianceMode.A2:
				value = "2";
				break;
			case StiPdfComplianceMode.A3:
				value = "3";
				break;
			}
			string value2 = "A";
			switch (zugferdComplianceMode)
			{
			case StiPdfZUGFeRDComplianceMode.V1:
				value2 = "A";
				break;
			case StiPdfZUGFeRDComplianceMode.V2:
				value2 = "B";
				break;
			case StiPdfZUGFeRDComplianceMode.V2_1:
				value2 = "B";
				break;
			}
			xmlTextWriter.WriteElementString("pdfaid:part", value);
			xmlTextWriter.WriteElementString("pdfaid:conformance", value2);
			xmlTextWriter.WriteFullEndElement();
		}
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.WriteRaw("                              \r\n");
		xmlTextWriter.WriteRaw("                              \r\n");
		xmlTextWriter.WriteRaw("                              \r\n");
		xmlTextWriter.WriteRaw("                              \r\n");
		xmlTextWriter.WriteRaw("                              \r\n");
		xmlTextWriter.WriteProcessingInstruction("xpacket", "end=\"w\"");
		xmlTextWriter.Flush();
		byte[] array4 = memoryStream2.ToArray();
		memoryStream2.Close();
		xmlTextWriter.Close();
		memoryStream.Write(array4, 3, array4.Length - 3);
		AddXref(info.Metadata.Ref);
		sw.WriteLine("{0} 0 obj", info.Metadata.Ref);
		sw.WriteLine("<<");
		sw.WriteLine("/Type /Metadata");
		sw.WriteLine("/Subtype /XML");
		StoreMemoryStream2(memoryStream, "/Length {0}");
		sw.WriteLine("");
		sw.WriteLine("endstream");
		sw.WriteLine("endobj");
		sw.WriteLine("");
	}

	private void RenderColorSpace()
	{
		AddXref(info.DestOutputProfile.Ref);
		sw.WriteLine("{0} 0 obj", info.DestOutputProfile.Ref);
		sw.WriteLine("<<");
		sw.WriteLine("/N 3");
		MemoryStream memoryStream = null;
		string text = null;
		if (compressed)
		{
			memoryStream = StiExportUtils.MakePdfDeflateStream(sRGBprofile);
			text = "/Length {0} /Filter [/FlateDecode] /Length1 " + sRGBprofile.Length;
		}
		else
		{
			memoryStream = new MemoryStream();
			memoryStream.Write(sRGBprofile, 0, sRGBprofile.Length);
			text = "/Length {0}";
		}
		StoreMemoryStream2(memoryStream, text);
		sw.WriteLine("");
		sw.WriteLine("endstream");
		sw.WriteLine("endobj");
		sw.WriteLine("");
		AddXref(info.OutputIntents.Ref);
		sw.WriteLine("{0} 0 obj", info.OutputIntents.Ref);
		sw.WriteLine("[<<");
		StoreStringLine("/Info", "sRGB IEC61966-2.1");
		StoreStringLine("/OutputConditionIdentifier", "Custom");
		StoreStringLine("/OutputCondition", string.Empty);
		StoreStringLine("/RegistryName", string.Empty);
		sw.WriteLine("/S /GTS_PDFA1");
		sw.WriteLine("/DestOutputProfile {0} 0 R", info.DestOutputProfile.Ref);
		sw.WriteLine("/Type /OutputIntent");
		sw.WriteLine(">>]");
		sw.WriteLine("endobj");
		sw.WriteLine("");
	}

	private void RenderAutoPrint()
	{
		if (autoPrint != StiPdfAutoPrintMode.None)
		{
			AddXref(info.EmbeddedJS.Ref);
			sw.WriteLine("{0} 0 obj", info.EmbeddedJS.Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Names [(EmbeddedJS) {0} 0 R]", info.EmbeddedJS.Content.Ref);
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
			AddXref(info.EmbeddedJS.Content.Ref);
			sw.WriteLine("{0} 0 obj", info.EmbeddedJS.Content.Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/S /JavaScript");
			sw.WriteLine("/JS (print\\({0}\\);)", (autoPrint == StiPdfAutoPrintMode.Dialog) ? "true" : "false");
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void RenderEmbeddedFiles()
	{
		for (int i = 0; i < info.EmbeddedFilesList.Count; i++)
		{
			StiPdfEmbeddedFileData stiPdfEmbeddedFileData = embeddedFiles[i];
			AddXref(info.EmbeddedFilesList[i].Ref);
			sw.WriteLine("{0} 0 obj", info.EmbeddedFilesList[i].Ref);
			sw.WriteLine("<<");
			StoreStringLine("/F", stiPdfEmbeddedFileData.Name, escaping: true);
			sw.WriteLine("/Type /Filespec");
			sw.WriteLine("/EF <<");
			sw.WriteLine("/F {0} 0 R", info.EmbeddedFilesList[i].Content.Ref);
			sw.WriteLine(">>");
			if (zugferdComplianceMode != 0 || pdfComplianceMode == StiPdfComplianceMode.A3)
			{
				sw.WriteLine("/AFRelationship /Alternative");
			}
			StoreStringLine("/UF", stiPdfEmbeddedFileData.Name, escaping: true);
			StoreStringLine("/Desc", stiPdfEmbeddedFileData.Description, escaping: true);
			sw.WriteLine(">>");
			sw.WriteLine("endobj");
			sw.WriteLine("");
			AddXref(info.EmbeddedFilesList[i].Content.Ref);
			sw.WriteLine("{0} 0 obj", info.EmbeddedFilesList[i].Content.Ref);
			sw.WriteLine("<<");
			sw.WriteLine("/Subtype /{0}", CorrectNameEnconding(stiPdfEmbeddedFileData.MIMEType));
			sw.WriteLine("/Type /EmbeddedFile");
			sw.WriteLine("/Params <<");
			StoreStringLine("/ModDate ", "D:" + currentDateTime);
			sw.WriteLine("/Size {0}", stiPdfEmbeddedFileData.Data.Length);
			sw.WriteLine(">>");
			if (compressed)
			{
				MemoryStream mem = StiExportUtils.MakePdfDeflateStream(stiPdfEmbeddedFileData.Data);
				StoreMemoryStream2(mem, "/Filter [/FlateDecode] /Length {0}");
			}
			else
			{
				StoreMemoryStream2(stiPdfEmbeddedFileData.Data, "/Length {0}");
			}
			sw.WriteLine("");
			sw.WriteLine("endstream");
			sw.WriteLine("endobj");
			sw.WriteLine("");
		}
	}

	private void AddBookmarkNode(StiBookmark bkm, int parentNode)
	{
		StiBookmarkTreeNode stiBookmarkTreeNode = new StiBookmarkTreeNode();
		stiBookmarkTreeNode.Parent = -1;
		stiBookmarkTreeNode.First = -1;
		stiBookmarkTreeNode.Last = -1;
		stiBookmarkTreeNode.Prev = -1;
		stiBookmarkTreeNode.Next = -1;
		stiBookmarkTreeNode.Count = -1;
		stiBookmarkTreeNode.Page = -1;
		stiBookmarkTreeNode.Y = -1.0;
		stiBookmarkTreeNode.Title = string.Empty;
		stiBookmarkTreeNode.Guid = string.Empty;
		stiBookmarkTreeNode.Used = parentNode == -1;
		bookmarksTree.Add(stiBookmarkTreeNode);
		int num = bookmarksTree.Count - 1;
		stiBookmarkTreeNode.Parent = parentNode;
		stiBookmarkTreeNode.Title = bkm.Text;
		stiBookmarkTreeNode.Guid = bkm.ComponentGuid;
		if (bkm.Bookmarks.Count == 0)
		{
			stiBookmarkTreeNode.Count = 0;
		}
		else
		{
			int prev = -1;
			for (int i = 0; i <= bkm.Bookmarks.Count - 1; i++)
			{
				int count = bookmarksTree.Count;
				StiBookmark bkm2 = bkm.Bookmarks[i];
				AddBookmarkNode(bkm2, num);
				StiBookmarkTreeNode stiBookmarkTreeNode2 = (StiBookmarkTreeNode)bookmarksTree[count];
				if (i < bkm.Bookmarks.Count - 1)
				{
					stiBookmarkTreeNode2.Next = bookmarksTree.Count;
				}
				if (i > 0)
				{
					stiBookmarkTreeNode2.Prev = prev;
				}
				bookmarksTree[count] = stiBookmarkTreeNode2;
				prev = count;
			}
			stiBookmarkTreeNode.First = num + 1;
			stiBookmarkTreeNode.Last = bookmarksTree.Count - 1;
			stiBookmarkTreeNode.Count = bookmarksTree.Count - num - 1;
		}
		bookmarksTree[num] = stiBookmarkTreeNode;
	}

	private void MakeBookmarkFromTree(StiBookmark bookmark, StiBookmarkTreeNode treeNode)
	{
		bookmark.Text = treeNode.Title;
		bookmark.Bookmarks = new StiBookmarksCollection();
		if (treeNode.Count <= 0)
		{
			return;
		}
		int num = treeNode.First;
		while (num != -1)
		{
			StiBookmarkTreeNode stiBookmarkTreeNode = (StiBookmarkTreeNode)bookmarksTree[num];
			if (stiBookmarkTreeNode.Used)
			{
				StiBookmark bookmark2 = new StiBookmark();
				bookmark.Bookmarks.Add(bookmark2);
				MakeBookmarkFromTree(bookmark2, stiBookmarkTreeNode);
			}
			num = stiBookmarkTreeNode.Next;
		}
	}

	protected virtual string OnSplitTextIntoLines(StiSplitTextIntoLinesEventArgs e)
	{
		if (StiPdfExportService.SplitTextIntoLinesEvent != null)
		{
			return StiPdfExportService.SplitTextIntoLinesEvent(this, e);
		}
		return null;
	}

	private void PreparePdfTextHelper()
	{
		isWpf = false;
		if (isWpfException)
		{
			return;
		}
		try
		{
			lock (lockSplitTextIntoLinesEventFlag)
			{
				if (StiPdfExportService.SplitTextIntoLinesEvent == null)
				{
					object obj = StiActivator.CreateObject("Stimulsoft.Report.Wpf.Helpers.StiPdfTextHelper, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", new object[0]);
				}
			}
			if (StiPdfExportService.SplitTextIntoLinesEvent != null)
			{
				isWpf = true;
			}
		}
		catch
		{
			isWpf = false;
			isWpfException = true;
		}
	}

	public string GetCreatorString()
	{
		if (!string.IsNullOrEmpty(StiOptions.Export.Pdf.CreatorString))
		{
			return StiOptions.Export.Pdf.CreatorString;
		}
		return StiExportUtils.GetReportVersion(report);
	}

	public void ClearFontsCache()
	{
		PdfFonts.ClearFontsCache();
	}

	public void InitPdfFonts()
	{
		pdfFont = new PdfFonts();
	}

	public void ExportPdf(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportPdf(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportPdf(StiReport report, Stream stream)
	{
		StiPdfExportSettings settings = new StiPdfExportSettings();
		ExportPdf(report, stream, settings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange, float imageQuality, float imageResolution)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool compressed)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		stiPdfExportSettings.EmbeddedFonts = embeddedFonts;
		stiPdfExportSettings.StandardPdfFonts = standardPdfFonts;
		stiPdfExportSettings.Compressed = compressed;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool compressed, string passwordInputUser, string passwordInputOwner, StiUserAccessPrivileges userAccessPrivileges)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		stiPdfExportSettings.EmbeddedFonts = embeddedFonts;
		stiPdfExportSettings.StandardPdfFonts = standardPdfFonts;
		stiPdfExportSettings.Compressed = compressed;
		stiPdfExportSettings.PasswordInputUser = passwordInputUser;
		stiPdfExportSettings.PasswordInputOwner = passwordInputOwner;
		stiPdfExportSettings.UserAccessPrivileges = userAccessPrivileges;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool compressed, string passwordInputUser, string passwordInputOwner, StiUserAccessPrivileges userAccessPrivileges, StiPdfEncryptionKeyLength keyLength)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		stiPdfExportSettings.EmbeddedFonts = embeddedFonts;
		stiPdfExportSettings.StandardPdfFonts = standardPdfFonts;
		stiPdfExportSettings.Compressed = compressed;
		stiPdfExportSettings.PasswordInputUser = passwordInputUser;
		stiPdfExportSettings.PasswordInputOwner = passwordInputOwner;
		stiPdfExportSettings.UserAccessPrivileges = userAccessPrivileges;
		stiPdfExportSettings.KeyLength = keyLength;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool compressed, bool exportRtfTextAsImage, string passwordInputUser, string passwordInputOwner, StiUserAccessPrivileges userAccessPrivileges, StiPdfEncryptionKeyLength keyLength)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		stiPdfExportSettings.EmbeddedFonts = embeddedFonts;
		stiPdfExportSettings.StandardPdfFonts = standardPdfFonts;
		stiPdfExportSettings.Compressed = compressed;
		stiPdfExportSettings.ExportRtfTextAsImage = exportRtfTextAsImage;
		stiPdfExportSettings.PasswordInputUser = passwordInputUser;
		stiPdfExportSettings.PasswordInputOwner = passwordInputOwner;
		stiPdfExportSettings.UserAccessPrivileges = userAccessPrivileges;
		stiPdfExportSettings.KeyLength = keyLength;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPagesRange pageRange, float imageQuality, float imageResolution, bool embeddedFonts, bool standardPdfFonts, bool compressed, bool exportRtfTextAsImage, string passwordInputUser, string passwordInputOwner, StiUserAccessPrivileges userAccessPrivileges, StiPdfEncryptionKeyLength keyLength, bool useUnicode)
	{
		StiPdfExportSettings stiPdfExportSettings = new StiPdfExportSettings();
		stiPdfExportSettings.PageRange = pageRange;
		stiPdfExportSettings.ImageQuality = imageQuality;
		stiPdfExportSettings.ImageResolution = imageResolution;
		stiPdfExportSettings.EmbeddedFonts = embeddedFonts;
		stiPdfExportSettings.StandardPdfFonts = standardPdfFonts;
		stiPdfExportSettings.Compressed = compressed;
		stiPdfExportSettings.ExportRtfTextAsImage = exportRtfTextAsImage;
		stiPdfExportSettings.PasswordInputUser = passwordInputUser;
		stiPdfExportSettings.PasswordInputOwner = passwordInputOwner;
		stiPdfExportSettings.UserAccessPrivileges = userAccessPrivileges;
		stiPdfExportSettings.KeyLength = keyLength;
		stiPdfExportSettings.UseUnicode = useUnicode;
		ExportPdf(report, stream, stiPdfExportSettings);
	}

	public void ExportPdf(StiReport report, Stream stream, StiPdfExportSettings settings)
	{
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		try
		{
			ExportPdf1(report, stream, settings);
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
		}
	}

	private void ExportPdf1(StiReport report, Stream stream, StiPdfExportSettings settings)
	{
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Expected O, but got Unknown
		//IL_1ba2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eeb: Expected O, but got Unknown
		//IL_14bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2317: Unknown result type (might be due to invalid IL or missing references)
		//IL_231d: Invalid comparison between Unknown and I4
		//IL_279b: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2809: Unknown result type (might be due to invalid IL or missing references)
		//IL_2835: Unknown result type (might be due to invalid IL or missing references)
		//IL_2861: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a91: Invalid comparison between Unknown and I4
		//IL_2f23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f52: Expected O, but got Unknown
		//IL_2edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4092: Unknown result type (might be due to invalid IL or missing references)
		//IL_4097: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3428: Unknown result type (might be due to invalid IL or missing references)
		//IL_342d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3432: Unknown result type (might be due to invalid IL or missing references)
		//IL_313c: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3155: Unknown result type (might be due to invalid IL or missing references)
		//IL_315a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3198: Unknown result type (might be due to invalid IL or missing references)
		//IL_319a: Unknown result type (might be due to invalid IL or missing references)
		//IL_37f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31da: Unknown result type (might be due to invalid IL or missing references)
		//IL_31de: Unknown result type (might be due to invalid IL or missing references)
		//IL_3185: Unknown result type (might be due to invalid IL or missing references)
		//IL_318e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3191: Invalid comparison between Unknown and I4
		//IL_318a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3194: Unknown result type (might be due to invalid IL or missing references)
		StiLogService.Write(((object)this).GetType(), "Export report to Pdf format");
		this.report = report;
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		float num = settings.ImageQuality;
		float num2 = settings.ImageResolution;
		imageResolutionMode = settings.ImageResolutionMode;
		bool flag = settings.EmbeddedFonts;
		bool flag2 = settings.StandardPdfFonts;
		bool flag3 = settings.Compressed;
		bool flag4 = settings.ExportRtfTextAsImage;
		string passwordInputUser = settings.PasswordInputUser;
		string passwordInputOwner = settings.PasswordInputOwner;
		StiUserAccessPrivileges userAccessPrivileges = settings.UserAccessPrivileges;
		keyLength = settings.KeyLength;
		useUnicodeMode = settings.UseUnicode;
		pdfComplianceMode = settings.PdfComplianceMode;
		bool getCertificateFromCryptoUI = settings.GetCertificateFromCryptoUI;
		bool flag5 = settings.UseDigitalSignature;
		string subjectNameString = settings.SubjectNameString;
		bool useLocalMachineCertificates = settings.UseLocalMachineCertificates;
		imageFormat = settings.ImageFormat;
		monochromeDitheringType = settings.DitheringType;
		autoPrint = settings.AutoPrintMode;
		StiPdfImageCompressionMethod stiPdfImageCompressionMethod = settings.ImageCompressionMethod;
		byte[] certificateData = settings.CertificateData;
		string certificatePassword = settings.CertificatePassword;
		string certificateThumbprint = settings.CertificateThumbprint;
		allowEditable = settings.AllowEditable;
		digitalSignatureReason = settings.DigitalSignatureReason;
		digitalSignatureLocation = settings.DigitalSignatureLocation;
		digitalSignatureContactInfo = settings.DigitalSignatureContactInfo;
		digitalSignatureSignedBy = settings.DigitalSignatureSignedBy;
		embeddedFiles = settings.EmbeddedFiles;
		zugferdComplianceMode = settings.ZUGFeRDComplianceMode;
		zugferdConformanceLevel = settings.ZUGFeRDConformanceLevel;
		byte[] zUGFeRDInvoiceData = settings.ZUGFeRDInvoiceData;
		int imageIndexedColorPaletteSize = settings.ImageIndexedColorPaletteSize;
		creatorName = GetCreatorString();
		keywords = StiOptions.Export.Pdf.KeywordsString;
		if (!string.IsNullOrEmpty(settings.CreatorString))
		{
			creatorName = settings.CreatorString;
		}
		if (!string.IsNullOrEmpty(settings.KeywordsString))
		{
			keywords = settings.KeywordsString;
		}
		if (string.IsNullOrEmpty(creatorName))
		{
			creatorName = producerName;
		}
		if (embeddedFiles == null)
		{
			embeddedFiles = new List<StiPdfEmbeddedFileData>();
		}
		if (report.Dictionary.Resources != null)
		{
			foreach (StiResource resource in report.Dictionary.Resources)
			{
				if (resource.AvailableInTheViewer)
				{
					string description = ((resource.Alias != resource.Name) ? resource.Alias : string.Empty);
					StiPdfEmbeddedFileData item = new StiPdfEmbeddedFileData(resource.Name + resource.GetFileExt(), description, resource.Content, resource.GetContentType());
					embeddedFiles.Add(item);
				}
			}
		}
		if (zugferdComplianceMode != 0)
		{
			pdfComplianceMode = StiPdfComplianceMode.A3;
			if (zUGFeRDInvoiceData != null)
			{
				bool flag6 = false;
				foreach (StiPdfEmbeddedFileData embeddedFile in embeddedFiles)
				{
					string text = embeddedFile.Name.ToLowerInvariant();
					if (text == "zugferd-invoice.xml" || text == "factur-x.xml")
					{
						flag6 = true;
					}
				}
				if (!flag6)
				{
					if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V1)
					{
						embeddedFiles.Add(new StiPdfEmbeddedFileData("ZUGFeRD-invoice.xml", "ZUGFeRD Rechnung", zUGFeRDInvoiceData));
					}
					if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2)
					{
						embeddedFiles.Add(new StiPdfEmbeddedFileData("zugferd-invoice.xml", "ZUGFeRD Rechnung", zUGFeRDInvoiceData));
					}
					if (zugferdComplianceMode == StiPdfZUGFeRDComplianceMode.V2_1)
					{
						embeddedFiles.Add(new StiPdfEmbeddedFileData("factur-x.xml", "Factur-X/ZUGFeRD", zUGFeRDInvoiceData));
					}
				}
			}
		}
		if (embeddedFiles.Count > 1)
		{
			embeddedFiles = embeddedFiles.OrderBy((StiPdfEmbeddedFileData ef) => ef.Name).ToList();
		}
		usePdfA = pdfComplianceMode != StiPdfComplianceMode.None;
		useTransparency = pdfComplianceMode != StiPdfComplianceMode.A1;
		if (!flag)
		{
			useUnicodeMode = false;
		}
		compressedFonts = true;
		if (usePdfA)
		{
			flag2 = false;
			flag = true;
			useUnicodeMode = true;
		}
		bool flag7 = pageRange.RangeType != StiRangeType.All;
		reduceFontSize = StiOptions.Export.Pdf.ReduceFontFileSize;
		bool allowImageComparer = StiOptions.Export.Pdf.AllowImageComparer;
		bool useImageTransparency = StiOptions.Export.Pdf.AllowImageTransparency && useTransparency;
		if (StiOptions.Export.Pdf.AllowEditablePdf)
		{
			allowEditable = StiPdfAllowEditable.Yes;
		}
		base.StatusString = StiLocalization.Get("Report", "PreparingReport");
		if (flag)
		{
			flag2 = false;
		}
		bool flag8 = true;
		if (num < 0f)
		{
			num = 0f;
		}
		if (num > 1f)
		{
			num = 1f;
		}
		if (num2 < 10f)
		{
			num2 = 10f;
		}
		if (useUnicodeMode)
		{
			flag2 = false;
		}
		bool flag9 = true;
		if (!StiOptions.Engine.FullTrust)
		{
			useUnicodeMode = false;
			flag5 = false;
			flag9 = false;
		}
		if (!StiOptions.Export.Pdf.AllowImportSystemLibraries)
		{
			flag2 = false;
			flag5 = false;
		}
		imageQuality = num;
		embeddedFonts = flag;
		standardPdfFonts = flag2;
		compressed = flag3;
		exportRtfTextAsImage = flag4;
		if (imageFormat == StiImageFormat.Monochrome)
		{
			stiPdfImageCompressionMethod = StiPdfImageCompressionMethod.Flate;
		}
		imageCompressionMethod = stiPdfImageCompressionMethod;
		num2 = (imageResolutionMain = num2 / 100f);
		haveDigitalSignature = false;
		isWpf = false;
		if (report.IsWpf || report.RenderedWith == StiRenderedWith.Wpf)
		{
			PreparePdfTextHelper();
		}
		pdfFont = new PdfFonts();
		pdfFont.standardPdfFonts = flag2;
		pdfFont.forceUseUnicode = useUnicodeMode;
		fontGlyphsReduceNotNeed = null;
		precision_digits = StiOptions.Export.Pdf.DefaultCoordinatesPrecision;
		clipLongTextLines = flag8;
		currentNumberFormat = new CultureInfo("en-US").NumberFormat;
		currentNumberFormat.NumberDecimalSeparator = ".";
		int digits = 3;
		for (int i = 0; i <= 255; i++)
		{
			string text2 = Math.Round((double)i / 255.0, digits).ToString("G", currentNumberFormat);
			colorTable[i] = text2;
			alphaTable[i] = false;
		}
		lastColorStrokeA = byte.MaxValue;
		lastColorNonStrokeA = byte.MaxValue;
		colorStack = new Stack(32);
		imageList = new ArrayList();
		switch (stiPdfImageCompressionMethod)
		{
		case StiPdfImageCompressionMethod.Flate:
			imageCache = new StiImageCache(allowImageComparer, ImageFormat.MemoryBmp, num, useImageTransparency, 0);
			break;
		case StiPdfImageCompressionMethod.Indexed:
			imageCache = new StiImageCache(allowImageComparer, ImageFormat.MemoryBmp, num, useImageTransparency, imageIndexedColorPaletteSize);
			break;
		default:
			imageCache = new StiImageCache(allowImageComparer, ImageFormat.Jpeg, num, useImageTransparency, 0);
			break;
		}
		imageInterpolationTable = new Hashtable();
		imageCacheIndexToList = new Hashtable();
		imageInfoList = new Hashtable();
		imageInfoCounter = 0;
		watermarkImageExist = new Hashtable();
		pdfFont.fontList = new ArrayList();
		xref = new SortedList();
		bookmarksTree = new ArrayList();
		haveBookmarks = false;
		linksArray = new ArrayList();
		annotsArray = new ArrayList();
		annots2Array = new List<StiEditableObject>();
		unsignedSignaturesArray = new List<StiEditableObject>();
		shadingArray = new ArrayList();
		shadingFunctionArray = new List<StiShadingFunctionData>();
		hatchArray = new ArrayList();
		tooltipsArray = new List<StiLinkObject>();
		sw = new StreamWriter(stream, Encoding.GetEncoding(1252));
		sw.NewLine = "\r\n";
		stream2 = stream;
		enc = Encoding.GetEncoding(1252).GetEncoder();
		imagesCounter = 0;
		fontsCounter = 0;
		bookmarksCounter = 0;
		linksCounter = 0;
		annotsCounter = 0;
		annots2Counter = 0;
		shadingCounter = 0;
		shadingFunctionCounter = 0;
		hatchCounter = 0;
		tooltipsCounter = 0;
		for (int j = 0; j < 256; j++)
		{
			CodePage1252[j] = j;
		}
		for (int k = 0; k < 32; k++)
		{
			CodePage1252[128 + k] = CodePage1252part80AF[k];
		}
		fontGlyphsReduceNotNeed = new bool[256];
		mfRender = new StiPdfMetafileRender(this, useUnicodeMode);
		DateTime now = DateTime.Now;
		string text3 = now.ToString("yyyyMMddHHmmsszzz");
		currentDateTime = text3.Substring(0, 17) + "'" + text3.Substring(18, 2) + "'";
		currentDateTimeMeta = now.ToString("yyyy-MM-ddTHH:mm:sszzz");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(now.ToString("yyyyMMddHHmmssffff"));
		stringBuilder.Append(producerName);
		stringBuilder.Append(creatorName);
		stringBuilder.Append(report.ReportAuthor);
		stringBuilder.Append(report.ReportAlias);
		stringBuilder.Append(report.ReportName);
		byte[] array = new byte[stringBuilder.Length];
		for (int l = 0; l < stringBuilder.Length; l++)
		{
			array[l] = (byte)stringBuilder[l];
		}
		IDValue = StiMD5Helper.ComputeHash(array);
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int m = 0; m < IDValue.Length; m++)
		{
			stringBuilder2.Append(IDValue[m].ToString("X2"));
		}
		IDValueString = stringBuilder2.ToString();
		IDValueStringMeta = $"uuid:{IDValueString.Substring(0, 8)}-{IDValueString.Substring(8, 4)}-{IDValueString.Substring(12, 4)}-{IDValueString.Substring(16, 4)}-{IDValueString.Substring(20, 12)}".ToLowerInvariant();
		pdfSecurity = new StiPdfSecurity(this);
		if (!usePdfA && flag9)
		{
			encrypted = pdfSecurity.ComputingCryptoValues(userAccessPrivileges, passwordInputOwner, passwordInputUser, keyLength, IDValue);
		}
		if (encrypted && useUnicodeMode)
		{
			flag = (embeddedFonts = true);
		}
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = (StiOptions.Export.Pdf.DivideSegmentPages ? 3 : 2);
		StiPagesCollection stiPagesCollection = pageRange.GetSelectedPages(report.RenderedPages);
		if (StiOptions.Export.Pdf.DivideSegmentPages)
		{
			stiPagesCollection = StiSegmentPagesDivider.Divide(stiPagesCollection, this);
			base.CurrentPassNumber++;
		}
		if (report.Bookmark != null && report.Bookmark.Bookmarks.Count != 0)
		{
			AddBookmarkNode(report.Bookmark, -1);
			haveBookmarks = true;
			bookmarksCounter = bookmarksTree.Count;
		}
		base.StatusString = StiLocalization.Get("Export", "ExportingFormatingObjects");
		int num3 = 0;
		RectangleD val10;
		foreach (StiPage item7 in stiPagesCollection)
		{
			stiPagesCollection.GetPage(item7);
			InvokeExporting(num3, stiPagesCollection.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			StoreShadingData1(item7.Brush, num3);
			StoreHatchData(item7.Brush);
			if (item7.HyperlinkValue != null && item7.HyperlinkValue.ToString().Trim().Length > 0 && !item7.HyperlinkValue.ToString().Trim().StartsWith("javascript:") && !usePdfA)
			{
				StiLinkObject stiLinkObject = default(StiLinkObject);
				stiLinkObject.Link = item7.HyperlinkValue.ToString();
				stiLinkObject.Page = num3;
				linksArray.Add(stiLinkObject);
			}
			if (item7.Watermark != null && item7.Watermark.Enabled)
			{
				if (!string.IsNullOrEmpty(item7.Watermark.Text))
				{
					if (item7.Watermark.Font != null)
					{
						int fontNumber = pdfFont.GetFontNumber(item7.Watermark.Font);
					}
					StringBuilder sb = new StringBuilder(StiBidirectionalConvert2.ConvertString(item7.Watermark.Text, rightToLeft: false));
					pdfFont.StoreUnicodeSymbolsInMap(sb);
					StoreShadingData1(item7.Watermark.TextBrush, num3);
					StoreHatchData(item7.Watermark.TextBrush);
				}
				object tagValue = item7.TagValue;
				StiAdvancedWatermark val = (StiAdvancedWatermark)((tagValue is StiAdvancedWatermark) ? tagValue : null);
				if (val != null && val.WeaveEnabled)
				{
					FontFamily val2 = StiFontIconsHelper.GetFontFamilyIcons();
					if (val2 == null)
					{
						val2 = StiFontCollection.GetFontFamily("Stimulsoft", false);
					}
					Font incomingFont = new Font(val2, 8f);
					pdfFont.GetFontNumber(incomingFont);
					StringBuilder sb2 = new StringBuilder(StiFontIconsHelper.GetContent(val.WeaveMajorIcon) + StiFontIconsHelper.GetContent(val.WeaveMinorIcon));
					pdfFont.StoreUnicodeSymbolsInMap(sb2);
				}
				if (item7.Watermark.ShowImageBehind)
				{
					byte[] image = item7.Watermark.GetImage(item7.Report);
					if (image != null)
					{
						if (StiSvgHelper.IsSvg(image))
						{
							Image val3 = StiSvgHelper.ConvertSvgToImageScaled(image, num2);
							try
							{
								StoreImageData(val3, num2, isImageComponent: false, needSmoothing: false);
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						else
						{
							Image val4 = StiImageConverter.BytesToImage(image, 200, 200, true, false);
							try
							{
								if (val4 is Metafile)
								{
									StoreWatermarkMetafileData(val4, num2, item7);
								}
								else
								{
									StoreImageData(val4, num2, isImageComponent: false, needSmoothing: false);
								}
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						watermarkImageExist[item7] = null;
					}
				}
			}
			foreach (StiComponent component in item7.Components)
			{
				if (!component.Enabled || (report.IsPrinting && !component.Printable))
				{
					continue;
				}
				imageInfoCounter++;
				if (component.Width > 0.0 && component.Height > 0.0)
				{
					if (component is IStiBrush stiBrush)
					{
						StoreShadingData1(stiBrush.Brush, num3);
						StoreHatchData(stiBrush.Brush);
					}
					if (component is StiCheckBox)
					{
						StiCheckBox stiCheckBox = component as StiCheckBox;
						StoreShadingData1(stiCheckBox.TextBrush, num3);
						StoreHatchData(stiCheckBox.TextBrush);
						if (allowEditable == StiPdfAllowEditable.Yes && ((IStiEditable)stiCheckBox).Editable)
						{
							StiEditableObject stiEditableObject = new StiEditableObject();
							stiEditableObject.Page = num3;
							stiEditableObject.Component = component;
							annots2Array.Add(stiEditableObject);
						}
						continue;
					}
					object tagValue2 = component.TagValue;
					StiAdvancedWatermark val5 = (StiAdvancedWatermark)((tagValue2 is StiAdvancedWatermark) ? tagValue2 : null);
					if (val5 != null)
					{
						if (val5.WeaveEnabled)
						{
							FontFamily val6 = StiFontIconsHelper.GetFontFamilyIcons();
							if (val6 == null)
							{
								val6 = StiFontCollection.GetFontFamily("Stimulsoft", false);
							}
							Font incomingFont2 = new Font(val6, 8f);
							pdfFont.GetFontNumber(incomingFont2);
							StringBuilder sb3 = new StringBuilder(StiFontIconsHelper.GetContent(val5.WeaveMajorIcon) + StiFontIconsHelper.GetContent(val5.WeaveMinorIcon));
							pdfFont.StoreUnicodeSymbolsInMap(sb3);
						}
						if (val5.ImageEnabled && val5.ImageBytes != null)
						{
							if (StiSvgHelper.IsSvg(val5.ImageBytes))
							{
								Image val7 = StiSvgHelper.ConvertSvgToImageScaled(val5.ImageBytes, num2);
								try
								{
									StoreImageData(val7, num2, isImageComponent: false, needSmoothing: false);
								}
								finally
								{
									((IDisposable)val7)?.Dispose();
								}
							}
							else
							{
								Image val8 = StiImageConverter.BytesToImage(val5.ImageBytes, 200, 200, true, false);
								try
								{
									if (val8 is Metafile)
									{
										StoreWatermarkMetafileData(val8, num2, item7);
									}
									else
									{
										StoreImageData(val8, num2, isImageComponent: false, needSmoothing: false);
									}
								}
								finally
								{
									((IDisposable)val8)?.Dispose();
								}
							}
						}
						if (val5.TextEnabled && !string.IsNullOrEmpty(val5.Text))
						{
							if (val5.TextFont != null)
							{
								int fontNumber2 = pdfFont.GetFontNumber(val5.TextFont);
							}
							StringBuilder sb4 = new StringBuilder(StiBidirectionalConvert2.ConvertString(val5.Text, rightToLeft: false));
							pdfFont.StoreUnicodeSymbolsInMap(sb4);
						}
					}
					StiMathFormulaCache.Clear();
					bool flag10 = component.IsExportAsImage(StiExportFormat.Pdf);
					StiShape stiShape = component as StiShape;
					if (stiShape != null && CheckShape(stiShape))
					{
						flag10 = false;
					}
					if ((flag4 || !(component is StiRichText)) && flag10)
					{
						bool flag11 = false;
						StiImage stiImage = component as StiImage;
						if (imageResolutionMode != StiImageResolutionMode.Exactly && stiImage != null && stiImage.ExistImageToDraw() && !stiImage.ImageToDrawIsMetafile())
						{
							Image val9 = stiImage.TakeGdiImageToDraw(imageResolutionMain);
							try
							{
								if (val9 != null)
								{
									flag11 = true;
									if (stiImage.ImageRotation != 0)
									{
										Stimulsoft.Report.Helpers.StiImageHelper.RotateImage(val9, stiImage.ImageRotation, disposeOriginal: true);
									}
									float imageResolution = val9.HorizontalResolution / 100f;
									if (imageResolutionMode == StiImageResolutionMode.NoMoreThan)
									{
										imageResolution = ((!stiImage.Stretch) ? ((float)(1.0 / stiImage.MultipleFactor)) : ((float)((double)val9.Width / report.Unit.ConvertToHInches(component.Width))));
									}
									imageResolution = StoreImageData(val9, imageResolution, isImageComponent: true, stiImage.Smoothing);
									imageInfoList[imageInfoCounter] = imageResolution;
								}
							}
							finally
							{
								((IDisposable)val9)?.Dispose();
							}
						}
						if (!flag11 && component is IStiExportImageExtended stiExportImageExtended)
						{
							float zoom = num2;
							Image image2 = stiExportImageExtended.GetImage(ref zoom, StiExportFormat.Pdf);
							try
							{
								if (image2 != null)
								{
									StoreImageData(image2, zoom, isImageComponent: false, stiImage?.Smoothing ?? false);
									imageInfoList[imageInfoCounter] = zoom;
								}
							}
							finally
							{
								((IDisposable)image2)?.Dispose();
							}
						}
					}
					if (component is IStiFont stiFont)
					{
						int fontNumber3 = pdfFont.GetFontNumber(stiFont.Font);
					}
					IStiTextOptions stiTextOptions = component as IStiTextOptions;
					if (component is StiText && !flag10)
					{
						StiText stiText = component as StiText;
						if ((StiOptions.Engine.UseNewHtmlEngine && stiText.CheckAllowHtmlTags()) || (StiDpiHelper.IsWindows && (stiText.CheckAllowHtmlTags() || (StiOptions.Export.Pdf.UseWysiwygRender && stiText.TextQuality == StiTextQuality.Wysiwyg))))
						{
							if (StiOptions.Export.Pdf.UseOldModeAllowHtmlTags)
							{
								mfRender.AssembleTextToEmf(component, 1f);
							}
							else
							{
								StoreWysiwygSymbols(stiText, num3);
							}
						}
						else
						{
							bool flag12 = stiTextOptions != null && stiTextOptions.TextOptions != null && stiTextOptions.TextOptions.RightToLeft;
							StringBuilder sb5 = new StringBuilder(StiBidirectionalConvert2.ConvertString(stiText.Text, rightToLeft: false));
							pdfFont.StoreUnicodeSymbolsInMap(sb5);
							IStiTextBrush stiTextBrush = component as IStiTextBrush;
							if (stiText != null && stiTextBrush != null)
							{
								StoreShadingData1(stiTextBrush.TextBrush, num3);
								StoreHatchData(stiTextBrush.TextBrush);
							}
						}
						if (allowEditable == StiPdfAllowEditable.Yes && ((IStiEditable)stiText).Editable)
						{
							StiEditableObject stiEditableObject2 = new StiEditableObject();
							stiEditableObject2.Page = num3;
							annotsArray.Add(stiEditableObject2);
							fontGlyphsReduceNotNeed[pdfFont.CurrentFont] = true;
						}
						if (stiText.Indicator is StiIconSetIndicator stiIconSetIndicator && (stiIconSetIndicator.Icon != 0 || stiIconSetIndicator.CustomIcon != null))
						{
							Image icon = StiIconSetHelper.GetIcon(stiIconSetIndicator);
							try
							{
								float imageResolution2 = 1f;
								imageResolution2 = StoreImageData(icon, imageResolution2, isImageComponent: false, needSmoothing: true, maxQuality: true);
								imageInfoList[imageInfoCounter] = imageResolution2;
							}
							finally
							{
								((IDisposable)icon)?.Dispose();
							}
						}
						if (stiText.Indicator is StiDataBarIndicator { Value: not 0f, BrushType: Stimulsoft.Report.Components.StiBrushType.Gradient } stiDataBarIndicator)
						{
							Color color = ((stiDataBarIndicator.Value < 0f) ? stiDataBarIndicator.NegativeColor : stiDataBarIndicator.PositiveColor);
							Color color2 = StiColorUtils.Light(color, (byte)200);
							StiShadingData stiShadingData = default(StiShadingData);
							stiShadingData.Page = num3;
							stiShadingData.FunctionIndex = GetShadingFunctionNumber(color, color2, isGlare: false);
							shadingArray.Add(stiShadingData);
						}
					}
					if (!flag4 && component is StiRichText)
					{
						mfRender.AssembleRtf(component);
					}
					if (stiShape != null && !flag10 && !string.IsNullOrWhiteSpace(stiShape.GetParsedText()))
					{
						StringBuilder sb6 = new StringBuilder(StiBidirectionalConvert2.ConvertString(stiShape.GetParsedText(), rightToLeft: false));
						pdfFont.StoreUnicodeSymbolsInMap(sb6);
					}
					if (component is StiBarCode && !flag10)
					{
						StiPdfGeomWriter geomWriter = new StiPdfGeomWriter(pageStream, this, assembleData: true);
						StiBarCodeExportPainter context = new StiBarCodeExportPainter(geomWriter);
						StiBarCode stiBarCode = component as StiBarCode;
						if (!string.IsNullOrEmpty(stiBarCode.CodeValue) && stiBarCode.Page != null)
						{
							val10 = report.Unit.ConvertToHInches(stiBarCode.ClientRectangle);
							RectangleF rect = ((RectangleD)(ref val10)).ToRectangleF();
							stiBarCode.BarCodeType.Draw(context, stiBarCode, rect, 1f);
						}
					}
					else if (component is StiChart && !flag10)
					{
						StiPdfData pp = default(StiPdfData);
						pp.Component = component;
						RenderChart(pp, assemble: true, num3);
					}
					else if (component is StiGauge && !flag10)
					{
						StiPdfData pp2 = default(StiPdfData);
						pp2.Component = component;
						RenderGauge(pp2, assemble: true, num3);
					}
					else if (component is StiMap && !flag10)
					{
						StiPdfData pp3 = default(StiPdfData);
						pp3.Component = component;
						RenderMap(pp3, assemble: true, num3);
					}
					else if (component is StiSparkline && !flag10)
					{
						StiPdfData pp4 = default(StiPdfData);
						pp4.Component = component;
						RenderSparkline(pp4, assemble: true, num3);
					}
				}
				if (component.HyperlinkValue != null && component.HyperlinkValue.ToString().Trim().Length > 0 && !component.HyperlinkValue.ToString().Trim().StartsWith("javascript:"))
				{
					StiLinkObject stiLinkObject2 = default(StiLinkObject);
					stiLinkObject2.Link = component.HyperlinkValue.ToString();
					stiLinkObject2.Page = num3;
					linksArray.Add(stiLinkObject2);
				}
				if (component.ToolTipValue != null && component.ToolTipValue.ToString().Trim().Length > 0 && !usePdfA)
				{
					StiLinkObject item2 = default(StiLinkObject);
					item2.Page = num3;
					tooltipsArray.Add(item2);
				}
				if (component.TagValue != null && component.TagValue.ToString().ToLower(CultureInfo.InvariantCulture) == "pdfunsignedsignaturefield" && !usePdfA)
				{
					StiEditableObject stiEditableObject3 = new StiEditableObject();
					stiEditableObject3.Page = num3;
					unsignedSignaturesArray.Add(stiEditableObject3);
				}
				if (flag5 && !haveDigitalSignature && component.TagValue != null && component.TagValue.ToString().ToLower(CultureInfo.InvariantCulture) == "pdfdigitalsignature")
				{
					signaturePageNumber = num3;
					haveDigitalSignature = true;
				}
			}
			num3++;
			if (item7.Watermark == null || !item7.Watermark.Enabled || item7.Watermark.ShowImageBehind)
			{
				continue;
			}
			byte[] image3 = item7.Watermark.GetImage(item7.Report);
			if (image3 == null)
			{
				continue;
			}
			if (StiSvgHelper.IsSvg(image3))
			{
				Image val11 = StiSvgHelper.ConvertSvgToImageScaled(image3, num2);
				try
				{
					StoreImageData(val11, num2, isImageComponent: false, needSmoothing: false);
				}
				finally
				{
					((IDisposable)val11)?.Dispose();
				}
			}
			else
			{
				Image val12 = StiImageConverter.BytesToImage(image3, 200, 200, true, false);
				try
				{
					if (val12 is Metafile)
					{
						StoreWatermarkMetafileData(val12, num2, item7);
					}
					else
					{
						StoreImageData(val12, num2, isImageComponent: false, needSmoothing: false);
					}
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			watermarkImageExist[item7] = null;
		}
		for (int n = 0; n < pdfFont.fontList.Count; n++)
		{
			pdfFont.CurrentFont = n;
			pdfFont.StoreUnicodeSymbolsInMap(new StringBuilder("…"));
		}
		if (unsignedSignaturesArray.Count > 0 && (annotsArray.Count > 0 || annots2Array.Count > 0))
		{
			for (int num4 = 0; num4 < pdfFont.fontList.Count; num4++)
			{
				if (((PdfFonts.pfontInfo)pdfFont.fontList[num4]).Name == "Arial")
				{
					fontGlyphsReduceNotNeed[num4] = true;
				}
			}
		}
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int num5 = 32; num5 < 256; num5++)
		{
			stringBuilder3.Append((char)CodePage1252[num5]);
		}
		for (int num6 = 0; num6 < pdfFont.fontList.Count; num6++)
		{
			if (fontGlyphsReduceNotNeed[num6])
			{
				pdfFont.CurrentFont = num6;
				pdfFont.StoreUnicodeSymbolsInMap(stringBuilder3);
			}
		}
		imagesCurrent = 0;
		annotsCurrent = 0;
		annots2Current = 0;
		shadingCurrent = 0;
		linksCounter = linksArray.Count;
		haveLinks = linksCounter > 0;
		annotsCounter = annotsArray.Count;
		annots2Counter = annots2Array.Count;
		unsignedSignaturesCounter = unsignedSignaturesArray.Count;
		haveAnnots = annotsCounter > 0 || annots2Counter > 0 || unsignedSignaturesCounter > 0;
		shadingCounter = shadingArray.Count;
		shadingFunctionCounter = shadingFunctionArray.Count;
		hatchCounter = hatchArray.Count;
		tooltipsCounter = tooltipsArray.Count;
		haveTooltips = tooltipsCounter > 0;
		imagesCounter = imageCache.ImageStore.Count;
		int num7 = 0;
		for (int num8 = 0; num8 < imageCache.ImageMaskStore.Count; num8++)
		{
			if (imageCache.ImageMaskStore[num8] != null)
			{
				num7++;
			}
		}
		if (flag5 && !haveDigitalSignature)
		{
			haveDigitalSignature = true;
			signaturePageNumber = 0;
			StiPage stiPage2 = stiPagesCollection[0];
			double num9 = 0.72 * report.Unit.ConvertToHInches(stiPage2.PageHeight * (double)stiPage2.SegmentPerHeight);
			double num10 = 0.72 * report.Unit.ConvertToHInches(stiPage2.PageWidth * (double)stiPage2.SegmentPerWidth);
			double num11 = 0.72 * report.Unit.ConvertToHInches(stiPage2.Margins.Left);
			double num12 = 0.72 * report.Unit.ConvertToHInches(stiPage2.Margins.Right);
			double num13 = 0.72 * report.Unit.ConvertToHInches(stiPage2.Margins.Top);
			double num14 = 0.72 * report.Unit.ConvertToHInches(stiPage2.Margins.Bottom);
			signaturePlacement = new RectangleD(0.0, num9 - num13, num11, num14);
		}
		pdfFont.InitFontsData(isWpf);
		fontsCounter = pdfFont.fontList.Count;
		info = new StiPdfStructure();
		for (int num15 = 0; num15 < stiPagesCollection.Count; num15++)
		{
			info.PageList.Add(info.CreateContentObject(addRef: true));
		}
		for (int num16 = 0; num16 < imageCache.ImagePackedStore.Count; num16++)
		{
			info.XObjectList.Add(info.CreateXObject(addRef: true, imageCache.ImageMaskStore[num16] != null));
		}
		for (int num17 = 0; num17 < pdfFont.fontList.Count; num17++)
		{
			PdfFonts.pfontInfo pfontInfo = (PdfFonts.pfontInfo)pdfFont.fontList[num17];
			info.FontList.Add(info.CreateFontObject(addRef: true, pfontInfo.UseUnicode, flag2, flag || PdfFonts.IsFontStimulsoft(pfontInfo.Name)));
		}
		info.Outlines = info.CreateOutlinesObject(haveBookmarks);
		if (haveBookmarks)
		{
			for (int num18 = 0; num18 < bookmarksCounter; num18++)
			{
				info.Outlines.Items.Add(info.CreateObject(addRef: true));
			}
		}
		info.Patterns = info.CreatePatternsObject(addRef: true);
		for (int num19 = 0; num19 < hatchCounter; num19++)
		{
			info.Patterns.HatchItems.Add(info.CreateObject(addRef: true));
		}
		for (int num20 = 0; num20 < shadingCounter; num20++)
		{
			info.Patterns.ShadingItems.Add(info.CreateObject(addRef: true));
		}
		for (int num21 = 0; num21 < shadingFunctionCounter; num21++)
		{
			info.Patterns.ShadingFunctionItems.Add(info.CreateObject(addRef: true));
		}
		for (int num22 = 0; num22 < linksCounter; num22++)
		{
			info.LinkList.Add(info.CreateObject(addRef: true));
		}
		info.Encode = info.CreateObject(encrypted);
		info.ExtGState = info.CreateObject(addRef: true);
		info.AcroForm = info.CreateAcroFormObject(haveAnnots || haveDigitalSignature || haveTooltips);
		if (haveAnnots)
		{
			for (int num23 = 0; num23 < annotsCounter; num23++)
			{
				info.AcroForm.Annots.Add(info.CreateAnnotObject(addRef: true, createAP: true));
			}
			for (int num24 = 0; num24 < fontsCounter; num24++)
			{
				info.AcroForm.AnnotFontItems.Add(info.CreateFontObject(addRef: true, useUnicodeMode: false, standardPdfFonts: false, embeddedFonts: false, annotFont: true));
			}
			for (int num25 = 0; num25 < annots2Counter; num25++)
			{
				info.AcroForm.CheckBoxes.Add(new List<StiPdfStructure.StiPdfAnnotObjInfo>());
				info.AcroForm.CheckBoxes[num25].Add(info.CreateAnnotObject(addRef: true, createAP: true, 2));
				info.AcroForm.CheckBoxes[num25].Add(info.CreateAnnotObject(addRef: true, createAP: true, 2));
				if ((annots2Array[num25].Component as StiCheckBox).CheckedValue == null)
				{
					info.AcroForm.CheckBoxes[num25].Add(info.CreateAnnotObject(addRef: true, createAP: false, 2));
				}
			}
			for (int num26 = 0; num26 < unsignedSignaturesCounter; num26++)
			{
				info.AcroForm.UnsignedSignatures.Add(info.CreateAnnotObject(addRef: true));
			}
		}
		info.AcroForm.Signatures.Add(info.CreateAnnotObject(haveDigitalSignature, createAP: true));
		for (int num27 = 0; num27 < tooltipsCounter; num27++)
		{
			info.AcroForm.Tooltips.Add(info.CreateAnnotObject(addRef: true));
		}
		info.Metadata = info.CreateObject(addRef: true);
		info.DestOutputProfile = info.CreateObject(addRef: true);
		info.OutputIntents = info.CreateObject(addRef: true);
		info.EmbeddedJS = info.CreateContentObject(autoPrint != StiPdfAutoPrintMode.None);
		if (embeddedFiles != null && embeddedFiles.Count > 0)
		{
			foreach (StiPdfEmbeddedFileData embeddedFile2 in embeddedFiles)
			{
				info.EmbeddedFilesList.Add(info.CreateContentObject(addRef: true));
			}
		}
		RenderStartDoc(report, stiPagesCollection);
		int num28 = 0;
		linksArray.Clear();
		tagsArray = new List<StiLinkObject>();
		tooltipsArray.Clear();
		unsignedSignaturesArray.Clear();
		imageInfoCounter = 0;
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		base.CurrentPassNumber++;
		RectangleD val14 = default(RectangleD);
		RectangleD val16 = default(RectangleD);
		StiHtmlState val18 = default(StiHtmlState);
		foreach (StiPage item8 in stiPagesCollection)
		{
			stiPagesCollection.GetPage(item8);
			InvokeExporting(num28, stiPagesCollection.Count, base.CurrentPassNumber, base.MaximumPassNumber);
			if (base.IsStopped)
			{
				return;
			}
			RenderPageHeader(num28++);
			double num29 = 0.72 * report.Unit.ConvertToHInches(item8.PageHeight * (double)item8.SegmentPerHeight);
			double num30 = 0.72 * report.Unit.ConvertToHInches(item8.PageWidth * (double)item8.SegmentPerWidth);
			double num31 = 0.72 * report.Unit.ConvertToHInches(item8.Margins.Left);
			double num32 = 0.72 * report.Unit.ConvertToHInches(item8.Margins.Right);
			double num33 = 0.72 * report.Unit.ConvertToHInches(item8.Margins.Top);
			double num34 = 0.72 * report.Unit.ConvertToHInches(item8.Margins.Bottom);
			if (num29 > 14400.0)
			{
				num29 = 14400.0;
			}
			if ((int)report.Bookmark.Engine == 1)
			{
				if (haveBookmarks && item8.BookmarkValue != null && (string)item8.BookmarkValue != "")
				{
					bool flag13 = false;
					int num35 = 0;
					while (!flag13 && num35 < bookmarksCounter)
					{
						StiBookmarkTreeNode stiBookmarkTreeNode = (StiBookmarkTreeNode)bookmarksTree[num35];
						if (!stiBookmarkTreeNode.Used && stiBookmarkTreeNode.Page == -1 && stiBookmarkTreeNode.Title == (string)item8.BookmarkValue)
						{
							stiBookmarkTreeNode.Page = num28 - 1;
							stiBookmarkTreeNode.Y = num29;
							stiBookmarkTreeNode.Used = true;
							flag13 = true;
							bookmarksTree[num35] = stiBookmarkTreeNode;
							break;
						}
						num35++;
					}
				}
			}
			else if (haveBookmarks && (!string.IsNullOrEmpty(item8.Guid) || (item8.BookmarkValue != null && (string)item8.BookmarkValue != "")))
			{
				for (int num36 = 0; num36 < bookmarksCounter; num36++)
				{
					StiBookmarkTreeNode stiBookmarkTreeNode2 = (StiBookmarkTreeNode)bookmarksTree[num36];
					if (stiBookmarkTreeNode2.Used || stiBookmarkTreeNode2.Page != -1)
					{
						continue;
					}
					bool flag14 = false;
					if (!string.IsNullOrEmpty(stiBookmarkTreeNode2.Guid))
					{
						if (!string.IsNullOrEmpty(item8.Guid) && stiBookmarkTreeNode2.Guid == item8.Guid)
						{
							flag14 = true;
						}
					}
					else if (!string.IsNullOrEmpty(stiBookmarkTreeNode2.Title) && item8.BookmarkValue != null && ((string)item8.BookmarkValue).Length > 0 && stiBookmarkTreeNode2.Title == (string)item8.BookmarkValue)
					{
						flag14 = true;
					}
					if (flag14)
					{
						stiBookmarkTreeNode2.Page = num28 - 1;
						stiBookmarkTreeNode2.Y = num29;
						stiBookmarkTreeNode2.Used = true;
						bookmarksTree[num36] = stiBookmarkTreeNode2;
						break;
					}
				}
			}
			if (item8.HyperlinkValue != null && item8.HyperlinkValue.ToString().Trim().Length > 0 && !item8.HyperlinkValue.ToString().Trim().StartsWith("javascript:") && !usePdfA)
			{
				StiLinkObject stiLinkObject3 = default(StiLinkObject);
				stiLinkObject3.Link = item8.HyperlinkValue.ToString();
				stiLinkObject3.X = 0.0;
				stiLinkObject3.Y = 0.0;
				stiLinkObject3.Width = num30;
				stiLinkObject3.Height = num29;
				stiLinkObject3.Page = num28 - 1;
				stiLinkObject3.DestPage = -1;
				stiLinkObject3.DestY = -1.0;
				linksArray.Add(stiLinkObject3);
			}
			if (item8.PointerValue != null && !string.IsNullOrWhiteSpace((string)item8.PointerValue) && !string.IsNullOrWhiteSpace(item8.Guid))
			{
				string link = $"#{item8.PointerValue}#GUID#{item8.Guid}";
				StiLinkObject item3 = default(StiLinkObject);
				item3.Link = link;
				item3.X = 0.0;
				item3.Y = 0.0;
				item3.Width = num30;
				item3.Height = num29;
				item3.Page = num28 - 1;
				item3.DestPage = -1;
				item3.DestY = -1.0;
				tagsArray.Add(item3);
			}
			if (item8.Brush != null)
			{
				StiPdfData pp5 = default(StiPdfData);
				pp5.X = 0.0;
				pp5.Y = 0.0;
				pp5.Width = num30;
				pp5.Height = num29;
				pp5.Component = new StiContainer();
				(pp5.Component as StiContainer).Brush = item8.Brush;
				(pp5.Component as StiContainer).Border = null;
				RenderBorder1(pp5);
			}
			RenderWatermark(item8, behind: true, num30, num29, num2);
			string text4 = item8.TagValue as string;
			if (!string.IsNullOrWhiteSpace(text4) && text4.StartsWith("Segments:"))
			{
				int result = 0;
				if (int.TryParse(text4.Substring(9), out result))
				{
					StiBorderSides val13 = (StiBorderSides)result;
					((RectangleD)(ref val14))._002Ector(0.0, 0.0, num30, num29);
					double num37 = 1.0;
					if (((Enum)val13).HasFlag((Enum)(object)(StiBorderSides)2))
					{
						ref RectangleD reference = ref val14;
						((RectangleD)(ref reference)).X = ((RectangleD)(ref reference)).X + (num31 - num37);
						reference = ref val14;
						((RectangleD)(ref reference)).Width = ((RectangleD)(ref reference)).Width - (num31 - num37);
					}
					if (((Enum)val13).HasFlag((Enum)(object)(StiBorderSides)4))
					{
						ref RectangleD reference = ref val14;
						((RectangleD)(ref reference)).Width = ((RectangleD)(ref reference)).Width - (num32 - num37);
					}
					if (((Enum)val13).HasFlag((Enum)(object)(StiBorderSides)1))
					{
						ref RectangleD reference = ref val14;
						((RectangleD)(ref reference)).Height = ((RectangleD)(ref reference)).Height - (num33 - num37);
					}
					if (((Enum)val13).HasFlag((Enum)(object)(StiBorderSides)8))
					{
						ref RectangleD reference = ref val14;
						((RectangleD)(ref reference)).Y = ((RectangleD)(ref reference)).Y + (num34 - num37);
						reference = ref val14;
						((RectangleD)(ref reference)).Height = ((RectangleD)(ref reference)).Height - (num34 - num37);
					}
					pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(((RectangleD)(ref val14)).X), ConvertToString(((RectangleD)(ref val14)).Y), ConvertToString(((RectangleD)(ref val14)).Width), ConvertToString(((RectangleD)(ref val14)).Height));
				}
			}
			foreach (StiComponent component2 in item8.Components)
			{
				if (component2.Enabled && (!report.IsPrinting || component2.Printable) && component2.Width > 0.0 && component2.Height > 0.0 && component2 is StiText { ExceedMargins: not StiExceedMargins.None } stiText2)
				{
					double num38 = 0.72 * report.Unit.ConvertToHInches(component2.Left);
					double num39 = 0.72 * report.Unit.ConvertToHInches(component2.Top);
					double num40 = 0.72 * report.Unit.ConvertToHInches(component2.Right);
					double num41 = 0.72 * report.Unit.ConvertToHInches(component2.Bottom);
					StiPdfData stiPdfData = default(StiPdfData);
					stiPdfData.X = num38 + num31;
					stiPdfData.Y = num39 + num33;
					stiPdfData.Width = num40 - num38;
					stiPdfData.Height = num41 - num39;
					stiPdfData.Y = num29 - (stiPdfData.Y + stiPdfData.Height);
					stiPdfData.Component = component2;
					StiPdfData pp6 = stiPdfData.Clone();
					if ((stiText2.ExceedMargins & StiExceedMargins.Left) > StiExceedMargins.None)
					{
						pp6.X = 0.0;
						pp6.Width = stiPdfData.Right;
					}
					if ((stiText2.ExceedMargins & StiExceedMargins.Right) > StiExceedMargins.None)
					{
						pp6.Width = num30 - pp6.X;
					}
					if ((stiText2.ExceedMargins & StiExceedMargins.Bottom) > StiExceedMargins.None)
					{
						pp6.Y = 0.0;
						pp6.Height = stiPdfData.Top;
					}
					if ((stiText2.ExceedMargins & StiExceedMargins.Top) > StiExceedMargins.None)
					{
						pp6.Height = num29 - pp6.Y;
					}
					RenderBorder1(pp6);
				}
			}
			List<StiPdfData> list = new List<StiPdfData>();
			List<StiComponent> list2 = new List<StiComponent>();
			List<StiComponent> list3 = new List<StiComponent>();
			foreach (StiComponent component3 in item8.Components)
			{
				if (component3.TagValue != null && component3.TagValue.ToString().ToLowerInvariant() == "#drawtopmost")
				{
					list3.Add(component3);
				}
				else
				{
					list2.Add(component3);
				}
			}
			if (StiOptions.Export.Pdf.OrderComponentsByPlacement)
			{
				for (int num42 = 0; num42 < list2.Count - 1; num42++)
				{
					StiComponent stiComponent4 = list2[num42];
					for (int num43 = num42 + 1; num43 < list2.Count; num43++)
					{
						StiComponent stiComponent5 = list2[num43];
						if (!(stiComponent4.Top < stiComponent5.Top) && (stiComponent4.Top != stiComponent5.Top || !(stiComponent4.Left <= stiComponent5.Left)))
						{
							list2[num43] = stiComponent4;
							list2[num42] = stiComponent5;
							stiComponent4 = stiComponent5;
						}
					}
				}
			}
			list2.AddRange(list3);
			list3.Clear();
			foreach (StiComponent item9 in list2)
			{
				if (!item9.Enabled || (report.IsPrinting && !item9.Printable))
				{
					continue;
				}
				imageInfoCounter++;
				double num44 = 0.72 * report.Unit.ConvertToHInches(item9.Left);
				double num45 = 0.72 * report.Unit.ConvertToHInches(item9.Top);
				double num46 = 0.72 * report.Unit.ConvertToHInches(item9.Right);
				double num47 = 0.72 * report.Unit.ConvertToHInches(item9.Bottom);
				StiPdfData stiPdfData2 = default(StiPdfData);
				stiPdfData2.X = num44;
				stiPdfData2.Y = num45;
				stiPdfData2.Width = num46 - num44;
				stiPdfData2.Height = num47 - num45;
				stiPdfData2.Y += num33;
				stiPdfData2.X += num31;
				stiPdfData2.Y = num29 - (stiPdfData2.Y + stiPdfData2.Height);
				stiPdfData2.Component = item9;
				if (item9.Width > 0.0 && item9.Height > 0.0)
				{
					bool flag15 = item9.IsExportAsImage(StiExportFormat.Pdf);
					StiText stiText3 = item9 as StiText;
					bool flag16 = stiText3 != null && stiText3.ExceedMargins != StiExceedMargins.None;
					if (!usePdfA && StiOptions.Export.Pdf.AllowPrintable && !item9.Printable)
					{
						pageStream.WriteLine("/OC /oc1 BDC");
					}
					if (!(item9 is StiShape) && !flag16)
					{
						RenderBorder1(stiPdfData2);
					}
					object tagValue3 = item9.TagValue;
					StiAdvancedWatermark val15 = (StiAdvancedWatermark)((tagValue3 is StiAdvancedWatermark) ? tagValue3 : null);
					if (val15 != null)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
						pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(stiPdfData2.X), ConvertToString(stiPdfData2.Y), ConvertToString(stiPdfData2.Width), ConvertToString(stiPdfData2.Height));
						((RectangleD)(ref val16))._002Ector(stiPdfData2.X, stiPdfData2.Y, stiPdfData2.Width, stiPdfData2.Height);
						if (val15.ImageEnabled && val15.ImageBytes != null)
						{
							RenderWatermarkImage(val16, val15.ImageStretch, val15.ImageTiling, val15.ImageAspectRatio, val15.ImageMultipleFactor, val15.ImageAlignment);
						}
						if (!string.IsNullOrEmpty(val15.Text))
						{
							StiPdfData pp7 = stiPdfData2.Clone();
							StiText stiText4 = new StiText(val16);
							stiText4.Text = val15.Text;
							stiText4.TextBrush = (StiBrush)new StiSolidBrush(val15.TextColor);
							stiText4.Font = val15.TextFont;
							stiText4.Angle = val15.TextAngle;
							stiText4.HorAlignment = (StiTextHorAlignment)1;
							stiText4.VertAlignment = (StiVertAlignment)1;
							stiText4.Page = item8;
							stiText4.TextQuality = StiTextQuality.Standard;
							pp7.Component = stiText4;
							RenderTextFont(pp7);
							RenderText(pp7);
						}
						if (val15.WeaveEnabled)
						{
							RenderWatermarkWeave(val15, val16, item8);
						}
						pageStream.WriteLine("Q");
						PopColorFromStack();
					}
					if (stiText3 != null && !flag15)
					{
						int num48 = 0;
						if (haveAnnots && stiText3.Editable)
						{
							pageStream.Flush();
							num48 = (int)pageStream.BaseStream.Position;
							PushColorToStack();
							lastColorStrokeA = byte.MaxValue;
							lastColorNonStrokeA = byte.MaxValue;
						}
						StiPdfData pp8 = RenderIndicators(stiPdfData2);
						if ((StiOptions.Engine.UseNewHtmlEngine && stiText3.CheckAllowHtmlTags()) || (StiDpiHelper.IsWindows && (stiText3.CheckAllowHtmlTags() || (StiOptions.Export.Pdf.UseWysiwygRender && stiText3.TextQuality == StiTextQuality.Wysiwyg && !isWpf))))
						{
							RenderTextFont(stiPdfData2);
							if (StiOptions.Export.Pdf.UseOldModeAllowHtmlTags)
							{
								mfRender.RenderTextToEmf(pp8, 1f);
							}
							else
							{
								RenderText2(pp8, num28);
							}
							if (haveAnnots && (stiPdfData2.Component as StiText).Editable)
							{
								StiEditableObject stiEditableObject4 = (StiEditableObject)annotsArray[annotsCurrent];
								stiEditableObject4.Multiline = stiText3.WordWrap;
								stiEditableObject4.X = stiPdfData2.X;
								stiEditableObject4.Y = stiPdfData2.Y;
								stiEditableObject4.Width = stiPdfData2.Width;
								stiEditableObject4.Height = stiPdfData2.Height;
								stiEditableObject4.Component = stiPdfData2.Component;
								stiEditableObject4.Alignment = (StiTextHorAlignment)0;
								if (stiPdfData2.Component is IStiTextHorAlignment { HorAlignment: var val17 })
								{
									if (stiPdfData2.Component is IStiTextOptions { TextOptions: not null } stiTextOptions2 && stiTextOptions2.TextOptions.RightToLeft)
									{
										if ((int)val17 == 0)
										{
											val17 = (StiTextHorAlignment)2;
										}
										else if ((int)val17 == 2)
										{
											val17 = (StiTextHorAlignment)0;
										}
									}
									stiEditableObject4.Alignment = val17;
								}
								((StiHtmlState)(ref val18))._002Ector(string.Empty);
								List<StiHtmlState> list4 = StiTextRenderer.ParseHtmlToStates(stiText3.Text.Value, val18, false);
								StringBuilder stringBuilder4 = new StringBuilder();
								foreach (StiHtmlState item10 in list4)
								{
									stringBuilder4.Append((object?)item10.Text);
								}
								stiEditableObject4.Text = stringBuilder4.ToString().Replace("\n", "");
							}
						}
						else
						{
							RenderTextFont(stiPdfData2);
							RenderText(pp8);
						}
						if ((annotsCounter > 0 || annots2Counter > 0) && stiText3.Editable)
						{
							pageStream.Flush();
							int num49 = (int)pageStream.BaseStream.Position;
							pageStream.BaseStream.Seek(num48, SeekOrigin.Begin);
							byte[] array2 = new byte[num49 - num48];
							pageStream.BaseStream.Read(array2, 0, num49 - num48);
							pageStream.BaseStream.Seek(num48, SeekOrigin.Begin);
							StiEditableObject stiEditableObject5 = (StiEditableObject)annotsArray[annotsCurrent];
							stiEditableObject5.Content = array2;
							annotsCurrent++;
							PopColorFromStack();
						}
					}
					if (item9 is StiShape)
					{
						RenderShape(stiPdfData2, num2);
					}
					else if (item9 is StiBarCode && !flag15)
					{
						StiPdfGeomWriter geomWriter2 = new StiPdfGeomWriter(pageStream, this);
						StiBarCodeExportPainter context2 = new StiBarCodeExportPainter(geomWriter2);
						StiBarCode stiBarCode2 = item9 as StiBarCode;
						if (!string.IsNullOrEmpty(stiBarCode2.CodeValue) && stiBarCode2.Page != null)
						{
							pageStream.WriteLine("q");
							PushColorToStack();
							pageStream.WriteLine("{0} {1} {2} {3} re W n", ConvertToString(stiPdfData2.X), ConvertToString(stiPdfData2.Y), ConvertToString(stiPdfData2.Width), ConvertToString(stiPdfData2.Height));
							pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(num31), ConvertToString(num29 - num33));
							pageStream.WriteLine("1 0 0 -1 0 0 cm");
							pageStream.WriteLine("{0} 0 0 {0} 0 0 cm", ConvertToString(0.72));
							val10 = report.Unit.ConvertToHInches(stiBarCode2.ClientRectangle);
							RectangleF rect2 = ((RectangleD)(ref val10)).ToRectangleF();
							stiBarCode2.BarCodeType.Draw(context2, stiBarCode2, rect2, 1f);
							pageStream.WriteLine("Q");
							PopColorFromStack();
						}
					}
					else if (item9 is StiChart && !flag15)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
						pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(stiPdfData2.X), ConvertToString(stiPdfData2.Y + stiPdfData2.Height));
						pageStream.WriteLine("1 0 0 -1 0 0 cm");
						pageStream.WriteLine("{0} 0 0 {0} 0 0 cm", ConvertToString(0.75));
						RenderChart(stiPdfData2, assemble: false, num28 - 1);
						pageStream.WriteLine("Q");
						PopColorFromStack();
					}
					else if (item9 is StiGauge && !flag15)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
						pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(stiPdfData2.X), ConvertToString(stiPdfData2.Y + stiPdfData2.Height));
						pageStream.WriteLine("1 0 0 -1 0 0 cm");
						pageStream.WriteLine("{0} 0 0 {0} 0 0 cm", ConvertToString(0.75));
						RenderGauge(stiPdfData2, assemble: false, num28 - 1);
						pageStream.WriteLine("Q");
						PopColorFromStack();
					}
					else if (item9 is StiMap && !flag15)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
						pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(stiPdfData2.X), ConvertToString(stiPdfData2.Y + stiPdfData2.Height));
						pageStream.WriteLine("1 0 0 -1 0 0 cm");
						pageStream.WriteLine("{0} 0 0 {0} 0 0 cm", ConvertToString(0.75));
						RenderMap(stiPdfData2, assemble: false, num28 - 1);
						pageStream.WriteLine("Q");
						PopColorFromStack();
					}
					else if (item9 is StiSparkline && !flag15)
					{
						pageStream.WriteLine("q");
						PushColorToStack();
						pageStream.WriteLine("1 0 0 1 {0} {1} cm", ConvertToString(stiPdfData2.X), ConvertToString(stiPdfData2.Y + stiPdfData2.Height));
						pageStream.WriteLine("1 0 0 -1 0 0 cm");
						pageStream.WriteLine("{0} 0 0 {0} 0 0 cm", ConvertToString(0.75));
						RenderSparkline(stiPdfData2, assemble: false, num28 - 1);
						pageStream.WriteLine("Q");
						PopColorFromStack();
					}
					else if (!flag4 && item9 is StiRichText)
					{
						mfRender.RenderRtf(stiPdfData2);
					}
					else if (item9 is StiCheckBox)
					{
						StiCheckBox stiCheckBox2 = item9 as StiCheckBox;
						if (haveAnnots && allowEditable == StiPdfAllowEditable.Yes && stiCheckBox2.Editable)
						{
							StiEditableObject stiEditableObject6 = annots2Array[annots2Current];
							stiEditableObject6.Multiline = false;
							stiEditableObject6.X = stiPdfData2.X;
							stiEditableObject6.Y = stiPdfData2.Y;
							stiEditableObject6.Width = stiPdfData2.Width;
							stiEditableObject6.Height = stiPdfData2.Height;
							stiEditableObject6.Component = stiPdfData2.Component;
							stiEditableObject6.Alignment = (StiTextHorAlignment)1;
							stiEditableObject6.Text = "";
							double x = stiPdfData2.X;
							double y = stiPdfData2.Y;
							stiPdfData2.X = 0.0;
							stiPdfData2.Y = 0.0;
							pageStream.Flush();
							int num50 = (int)pageStream.BaseStream.Position;
							PushColorToStack();
							lastColorStrokeA = byte.MaxValue;
							lastColorNonStrokeA = byte.MaxValue;
							RenderCheckbox(stiPdfData2, checkBoxValue: true);
							pageStream.Flush();
							int num51 = (int)pageStream.BaseStream.Position;
							pageStream.BaseStream.Seek(num50, SeekOrigin.Begin);
							byte[] array3 = new byte[num51 - num50];
							pageStream.BaseStream.Read(array3, 0, num51 - num50);
							pageStream.BaseStream.Seek(num50, SeekOrigin.Begin);
							stiEditableObject6.Content = array3;
							PopColorFromStack();
							pageStream.Flush();
							num50 = (int)pageStream.BaseStream.Position;
							PushColorToStack();
							lastColorStrokeA = byte.MaxValue;
							lastColorNonStrokeA = byte.MaxValue;
							RenderCheckbox(stiPdfData2, checkBoxValue: false, storeShading: false);
							pageStream.Flush();
							num51 = (int)pageStream.BaseStream.Position;
							pageStream.BaseStream.Seek(num50, SeekOrigin.Begin);
							array3 = new byte[num51 - num50];
							pageStream.BaseStream.Read(array3, 0, num51 - num50);
							pageStream.BaseStream.Seek(num50, SeekOrigin.Begin);
							stiEditableObject6.Content2 = array3;
							PopColorFromStack();
							annots2Current++;
							stiPdfData2.X = x;
							stiPdfData2.Y = y;
						}
						else
						{
							bool? checkBoxValue = GetCheckBoxValue(stiCheckBox2);
							if (checkBoxValue.HasValue)
							{
								RenderCheckbox(stiPdfData2, checkBoxValue.Value);
							}
						}
					}
					else if (flag15)
					{
						RenderImage(stiPdfData2, num2);
					}
					if (item9 is StiLinePrimitive)
					{
						RenderPrimitives(stiPdfData2);
					}
					else if (stiPdfData2.Component is IStiBorder { Border: not null } stiBorder && stiBorder.Border.Topmost)
					{
						list.Add(stiPdfData2);
					}
					else
					{
						RenderBorder2(stiPdfData2);
					}
					if (!usePdfA && StiOptions.Export.Pdf.AllowPrintable && !item9.Printable)
					{
						pageStream.WriteLine("EMC");
					}
				}
				if ((int)report.Bookmark.Engine == 1)
				{
					if (haveBookmarks && item9.BookmarkValue != null && (string)item9.BookmarkValue != "")
					{
						bool flag17 = false;
						int num52 = 0;
						while (!flag17 && num52 < bookmarksCounter)
						{
							StiBookmarkTreeNode stiBookmarkTreeNode3 = (StiBookmarkTreeNode)bookmarksTree[num52];
							if (!stiBookmarkTreeNode3.Used && stiBookmarkTreeNode3.Page == -1 && stiBookmarkTreeNode3.Title == (string)item9.BookmarkValue)
							{
								stiBookmarkTreeNode3.Page = num28 - 1;
								stiBookmarkTreeNode3.Y = stiPdfData2.Y + stiPdfData2.Height;
								stiBookmarkTreeNode3.Used = true;
								flag17 = true;
								bookmarksTree[num52] = stiBookmarkTreeNode3;
								break;
							}
							num52++;
						}
					}
				}
				else if (haveBookmarks && (!string.IsNullOrEmpty(item9.Guid) || (item9.BookmarkValue != null && (string)item9.BookmarkValue != "")))
				{
					for (int num53 = 0; num53 < bookmarksCounter; num53++)
					{
						StiBookmarkTreeNode stiBookmarkTreeNode4 = (StiBookmarkTreeNode)bookmarksTree[num53];
						if (stiBookmarkTreeNode4.Used || stiBookmarkTreeNode4.Page != -1)
						{
							continue;
						}
						bool flag18 = false;
						if (!string.IsNullOrEmpty(stiBookmarkTreeNode4.Guid))
						{
							if (!string.IsNullOrEmpty(item9.Guid) && stiBookmarkTreeNode4.Guid == item9.Guid)
							{
								flag18 = true;
							}
						}
						else if (!string.IsNullOrEmpty(stiBookmarkTreeNode4.Title) && item9.BookmarkValue != null && ((string)item9.BookmarkValue).Length > 0 && stiBookmarkTreeNode4.Title == (string)item9.BookmarkValue)
						{
							flag18 = true;
						}
						if (flag18)
						{
							stiBookmarkTreeNode4.Page = num28 - 1;
							stiBookmarkTreeNode4.Y = stiPdfData2.Y + stiPdfData2.Height;
							stiBookmarkTreeNode4.Used = true;
							bookmarksTree[num53] = stiBookmarkTreeNode4;
							break;
						}
					}
				}
				if (item9.HyperlinkValue != null && item9.HyperlinkValue.ToString().Trim().Length > 0 && !item9.HyperlinkValue.ToString().Trim().StartsWith("javascript:"))
				{
					StiLinkObject stiLinkObject4 = default(StiLinkObject);
					stiLinkObject4.Link = item9.HyperlinkValue.ToString();
					stiLinkObject4.X = stiPdfData2.X;
					stiLinkObject4.Y = stiPdfData2.Y;
					stiLinkObject4.Width = stiPdfData2.Width;
					stiLinkObject4.Height = stiPdfData2.Height;
					stiLinkObject4.Page = num28;
					stiLinkObject4.DestPage = -1;
					stiLinkObject4.DestY = -1.0;
					linksArray.Add(stiLinkObject4);
				}
				if (item9.TagValue != null && item9.TagValue.ToString().Trim().Length > 0)
				{
					StiLinkObject item4 = default(StiLinkObject);
					item4.Link = item9.TagValue.ToString();
					item4.X = stiPdfData2.X;
					item4.Y = stiPdfData2.Y;
					item4.Width = stiPdfData2.Width;
					item4.Height = stiPdfData2.Height;
					item4.Page = num28 - 1;
					item4.DestPage = -1;
					item4.DestY = -1.0;
					tagsArray.Add(item4);
					if (item4.Link.Trim().ToLower(CultureInfo.InvariantCulture) == "pdfunsignedsignaturefield" && !usePdfA)
					{
						StiEditableObject stiEditableObject7 = new StiEditableObject();
						stiEditableObject7.X = item4.X;
						stiEditableObject7.Y = item4.Y;
						stiEditableObject7.Width = item4.Width;
						stiEditableObject7.Height = item4.Height;
						stiEditableObject7.Page = item4.Page;
						stiEditableObject7.Component = item9;
						unsignedSignaturesArray.Add(stiEditableObject7);
					}
				}
				if (item9.PointerValue != null && !string.IsNullOrWhiteSpace((string)item9.PointerValue) && !string.IsNullOrWhiteSpace(item9.Guid))
				{
					string link2 = $"#{item9.PointerValue}#GUID#{item9.Guid}";
					StiLinkObject item5 = default(StiLinkObject);
					item5.Link = link2;
					item5.X = stiPdfData2.X;
					item5.Y = stiPdfData2.Y;
					item5.Width = stiPdfData2.Width;
					item5.Height = stiPdfData2.Height;
					item5.Page = num28 - 1;
					item5.DestPage = -1;
					item5.DestY = -1.0;
					tagsArray.Add(item5);
				}
				if (item9.ToolTipValue != null && item9.ToolTipValue.ToString().Trim().Length > 0 && !usePdfA)
				{
					StiLinkObject item6 = default(StiLinkObject);
					item6.Link = item9.ToolTipValue.ToString().Trim();
					item6.X = stiPdfData2.X;
					item6.Y = stiPdfData2.Y;
					item6.Width = stiPdfData2.Width;
					item6.Height = stiPdfData2.Height;
					item6.Page = num28 - 1;
					item6.DestPage = -1;
					item6.DestY = -1.0;
					tooltipsArray.Add(item6);
				}
				if (item9.TagValue != null && item9.TagValue.ToString().ToLower(CultureInfo.InvariantCulture) == "pdfdigitalsignature")
				{
					signaturePlacement = new RectangleD(stiPdfData2.X, stiPdfData2.Y, stiPdfData2.Width, stiPdfData2.Height);
				}
			}
			foreach (StiPdfData item11 in list)
			{
				RenderBorder2(item11);
			}
			list.Clear();
			if (item8.Border != null)
			{
				StiPdfData pp9 = default(StiPdfData);
				pp9.X = num31;
				pp9.Y = num34;
				pp9.Width = num30 - num31 - num32;
				pp9.Height = num29 - num33 - num34;
				pp9.Component = new StiContainer();
				(pp9.Component as StiContainer).Border = item8.Border;
				RenderBorder2(pp9);
			}
			RenderWatermark(item8, behind: false, num30, num29, num2);
			RenderPageFooter(num29, num30);
		}
		bookmarksTreeTemp = null;
		if (haveBookmarks && flag7)
		{
			for (int num54 = 0; num54 < bookmarksTree.Count; num54++)
			{
				StiBookmarkTreeNode stiBookmarkTreeNode5 = (StiBookmarkTreeNode)bookmarksTree[num54];
				if (stiBookmarkTreeNode5.Used)
				{
					while (stiBookmarkTreeNode5.Parent != -1)
					{
						stiBookmarkTreeNode5 = (StiBookmarkTreeNode)bookmarksTree[stiBookmarkTreeNode5.Parent];
						stiBookmarkTreeNode5.Used = true;
					}
				}
			}
			StiBookmark stiBookmark = new StiBookmark();
			MakeBookmarkFromTree(stiBookmark, (StiBookmarkTreeNode)bookmarksTree[0]);
			bookmarksTreeTemp = bookmarksTree;
			bookmarksTree = new ArrayList();
			AddBookmarkNode(stiBookmark, -1);
			int num55 = bookmarksCounter - bookmarksTree.Count;
			if (num55 > 0)
			{
				StiBookmarkTreeNode stiBookmarkTreeNode6 = new StiBookmarkTreeNode();
				stiBookmarkTreeNode6.Parent = -1;
				stiBookmarkTreeNode6.First = -1;
				stiBookmarkTreeNode6.Last = -1;
				stiBookmarkTreeNode6.Prev = -1;
				stiBookmarkTreeNode6.Next = -1;
				stiBookmarkTreeNode6.Count = -1;
				stiBookmarkTreeNode6.Page = -1;
				stiBookmarkTreeNode6.Y = -1.0;
				stiBookmarkTreeNode6.Title = "";
				for (int num56 = 0; num56 < num55; num56++)
				{
					bookmarksTree.Add(stiBookmarkTreeNode6);
				}
			}
			Hashtable hashtable = new Hashtable();
			for (int num57 = 0; num57 < bookmarksTreeTemp.Count; num57++)
			{
				StiBookmarkTreeNode stiBookmarkTreeNode7 = (StiBookmarkTreeNode)bookmarksTreeTemp[num57];
				hashtable[stiBookmarkTreeNode7.Title] = stiBookmarkTreeNode7;
			}
			for (int num58 = 0; num58 < bookmarksTree.Count; num58++)
			{
				StiBookmarkTreeNode stiBookmarkTreeNode8 = (StiBookmarkTreeNode)bookmarksTree[num58];
				if (!string.IsNullOrEmpty(stiBookmarkTreeNode8.Title))
				{
					StiBookmarkTreeNode stiBookmarkTreeNode9 = (StiBookmarkTreeNode)hashtable[stiBookmarkTreeNode8.Title];
					stiBookmarkTreeNode8.Page = stiBookmarkTreeNode9.Page;
					stiBookmarkTreeNode8.Y = stiBookmarkTreeNode9.Y;
				}
			}
			bookmarksTreeTemp = null;
		}
		RenderImageTable();
		RenderFontTable();
		RenderBookmarksTable();
		RenderPatternTable();
		RenderLinkTable();
		RenderEncodeRecord();
		RenderExtGStateRecord();
		RenderAnnotTable();
		RenderSignatureTable(report);
		RenderTooltipTable();
		RenderMetadata(report);
		RenderColorSpace();
		RenderAutoPrint();
		RenderEmbeddedFiles();
		RenderEndDoc();
		imageList.Clear();
		imageList = null;
		imageInterpolationTable.Clear();
		imageInterpolationTable = null;
		imageCacheIndexToList.Clear();
		imageCacheIndexToList = null;
		imageInfoList.Clear();
		imageInfoList = null;
		xref = null;
		bookmarksTree = null;
		linksArray = null;
		tagsArray = null;
		tooltipsArray = null;
		annotsArray = null;
		annots2Array = null;
		unsignedSignaturesArray = null;
		shadingArray = null;
		hatchArray = null;
		colorStack = null;
		enc = null;
		if (graphicsForTextRenderer != null)
		{
			graphicsForTextRenderer.Dispose();
			graphicsForTextRenderer = null;
		}
		if (imageForGraphicsForTextRenderer != null)
		{
			imageForGraphicsForTextRenderer.Dispose();
			imageForGraphicsForTextRenderer = null;
		}
		pdfFont.Clear();
		pdfFont = null;
		imageCache.Clear();
		watermarkImageExist.Clear();
		mfRender.Clear();
		mfRender = null;
		sw.Flush();
		if (haveDigitalSignature)
		{
			int num59 = (int)stream.Length;
			int num60 = num59 - (offsetSignatureData + 16384 + 2);
			byte[] bytes = Encoding.ASCII.GetBytes(num60.ToString());
			stream.Seek(offsetSignatureLen2, SeekOrigin.Begin);
			stream.Write(bytes, 0, bytes.Length);
			stream.Flush();
			byte[] array4 = new byte[offsetSignatureData + num60];
			stream.Seek(0L, SeekOrigin.Begin);
			stream.Read(array4, 0, offsetSignatureData);
			stream.Seek(offsetSignatureData + 16384 + 2, SeekOrigin.Begin);
			stream.Read(array4, offsetSignatureData, num60);
			bool isGost = false;
			string signedBy = digitalSignatureSignedBy;
			byte[] array5 = pdfSecurity.CreateSignature(array4, getCertificateFromCryptoUI, subjectNameString, useLocalMachineCertificates, certificateData, certificatePassword, certificateThumbprint, out isGost, ref signedBy, offsetSignatureFilter, offsetSignatureName);
			byte[] bytes2 = Encoding.ASCII.GetBytes(BitConverter.ToString(array5).Replace("-", "").ToLower(CultureInfo.InvariantCulture));
			stream.Seek(offsetSignatureData + 1, SeekOrigin.Begin);
			stream.Write(bytes2, 0, bytes2.Length);
			if (isGost)
			{
				byte[] bytes3 = Encoding.ASCII.GetBytes("CryptoPro#20PDF");
				stream.Seek(offsetSignatureFilter, SeekOrigin.Begin);
				stream.Write(bytes3, 0, bytes3.Length);
			}
			if (string.IsNullOrEmpty(digitalSignatureSignedBy) && !string.IsNullOrEmpty(signedBy))
			{
				stream.Seek(offsetSignatureName, SeekOrigin.Begin);
				StoreString(signedBy);
			}
			stream.Seek(num59, SeekOrigin.Begin);
			stream.Flush();
		}
	}
}
