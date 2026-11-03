using System.ComponentModel;
using System.Globalization;
using System.Text;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiDataExportSettings : StiPageRangeExportSettings
{
	private StiDataType dataType;

	[DefaultValue(StiDataType.Csv)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiDataType DataType
	{
		get
		{
			return dataType;
		}
		set
		{
			dataType = value;
		}
	}

	[DefaultValue(StiDataExportMode.Data)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiDataExportMode DataExportMode { get; set; } = StiDataExportMode.Data;


	[JsonConverter(typeof(StiEncodingJsonConverter))]
	public Encoding Encoding { get; set; }

	[DefaultValue(false)]
	public bool ExportDataOnly { get; set; }

	[DefaultValue(StiDbfCodePages.Default)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiDbfCodePages CodePage { get; set; }

	public string Separator { get; set; }

	public string TableName { get; set; }

	[DefaultValue(false)]
	public bool SkipColumnHeaders { get; set; }

	[DefaultValue(true)]
	public bool UseDefaultSystemEncoding { get; set; } = true;


	public override StiExportFormat GetExportFormat()
	{
		return DataType switch
		{
			StiDataType.Dbf => StiExportFormat.Dbf, 
			StiDataType.Dif => StiExportFormat.Dif, 
			StiDataType.Sylk => StiExportFormat.Sylk, 
			StiDataType.Xml => StiExportFormat.Xml, 
			StiDataType.Json => StiExportFormat.Json, 
			_ => StiExportFormat.Csv, 
		};
	}

	public StiDataExportSettings()
		: this(StiDataType.Csv)
	{
	}

	public StiDataExportSettings(StiDataType dataType)
	{
		this.dataType = dataType;
		Encoding = Encoding.UTF8;
		Separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
	}
}
