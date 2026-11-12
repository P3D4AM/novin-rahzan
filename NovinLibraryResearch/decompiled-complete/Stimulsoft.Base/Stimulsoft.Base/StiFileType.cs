using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiFileType
{
	Unknown = 1,
	ReportSnapshot,
	Pdf,
	Xps,
	PowerPoint,
	Html,
	Text,
	RichText,
	Word,
	OpenDocumentWriter,
	Excel,
	OpenDocumentCalc,
	Data,
	Image,
	Xml,
	Xsd,
	Csv,
	Dbf,
	Sylk,
	Dif,
	Json,
	Gis
}
