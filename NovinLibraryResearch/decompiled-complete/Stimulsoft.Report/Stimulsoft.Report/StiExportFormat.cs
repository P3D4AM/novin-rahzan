using System;
using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiExportFormat
{
	None = 0,
	Pdf = 1,
	Xps = 2,
	HtmlTable = 3,
	HtmlSpan = 4,
	HtmlDiv = 5,
	Rtf = 6,
	RtfTable = 7,
	RtfFrame = 8,
	RtfWinWord = 9,
	RtfTabbedText = 10,
	[Obsolete("Please use StiExportFormat.RtfFrame")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	RtfMode1 = 8,
	[Obsolete("Please use StiExportFormat.RtfWinWord")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	RtfMode2 = 9,
	[Obsolete("Please use StiExportFormat.RtfTabbedText")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	RtfMode3 = 10,
	Text = 11,
	Excel = 12,
	ExcelXml = 13,
	Excel2007 = 14,
	Word2007 = 15,
	Xml = 16,
	Csv = 17,
	Dif = 18,
	Sylk = 19,
	Image = 20,
	ImageGif = 21,
	ImageBmp = 22,
	ImagePng = 23,
	ImageTiff = 24,
	ImageJpeg = 25,
	ImagePcx = 26,
	ImageEmf = 27,
	ImageSvg = 28,
	ImageSvgz = 29,
	Mht = 30,
	Dbf = 31,
	Html = 32,
	Ods = 33,
	Odt = 34,
	Ppt2007 = 35,
	Html5 = 36,
	Data = 37,
	Json = 38,
	Document = 1000
}
