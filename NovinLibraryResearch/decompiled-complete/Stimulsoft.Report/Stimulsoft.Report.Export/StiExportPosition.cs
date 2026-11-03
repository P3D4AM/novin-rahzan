using System;
using System.ComponentModel;

namespace Stimulsoft.Report.Export;

public enum StiExportPosition
{
	Pdf = 0,
	Xps = 1,
	Ppt2007 = 2,
	Html = 10,
	Html5 = 11,
	Mht = 12,
	Txt = 20,
	Rtf = 21,
	Word2007 = 22,
	Odt = 23,
	Excel = 30,
	ExcelXml = 31,
	Excel2007 = 32,
	Ods = 33,
	Data = 40,
	[Obsolete("Please use Data value instead Dbf!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Dbf = 41,
	[Obsolete("Please use Data value instead Xml!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Xml = 42,
	[Obsolete("Please use Data value instead Dif!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Dif = 43,
	[Obsolete("Please use Data value instead Sylk!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Sylk = 44,
	Image = 50,
	[Obsolete("Please use Image value instead Bmp!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Bmp = 50,
	[Obsolete("Please use Image value instead Gif!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Gif = 51,
	[Obsolete("Please use Image value instead Jpeg!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Jpeg = 52,
	[Obsolete("Please use Image value instead Pcx!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Pcx = 53,
	[Obsolete("Please use Image value instead Png!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Png = 54,
	[Obsolete("Please use Image value instead Tiff!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Tiff = 55,
	[Obsolete("Please use Image value instead Emf!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Emf = 60,
	[Obsolete("Please use Image value instead Svg!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Svg = 61,
	[Obsolete("Please use Image value instead Svgz!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Svgz = 62
}
