using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Report.Export;

public class StiExcelExportSettings : StiPageRangeExportSettings
{
	private StiExcelType excelType;

	[DefaultValue(StiExcelType.ExcelBinary)]
	[JsonConverter(typeof(StringEnumConverter))]
	public virtual StiExcelType ExcelType
	{
		get
		{
			return excelType;
		}
		set
		{
			excelType = value;
		}
	}

	public bool UseOnePageHeaderAndFooter { get; set; }

	[Obsolete("Please use property 'DataExportMode'.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ExportDataOnly
	{
		get
		{
			return DataExportMode != StiDataExportMode.AllBands;
		}
		set
		{
			DataExportMode = (value ? StiDataExportMode.DataAndHeaders : StiDataExportMode.AllBands);
		}
	}

	[DefaultValue(StiDataExportMode.AllBands)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiDataExportMode DataExportMode { get; set; } = StiDataExportMode.AllBands;


	public bool ExportPageBreaks { get; set; }

	[DefaultValue(true)]
	public bool ExportObjectFormatting { get; set; } = true;


	public bool ExportEachPageToSheet { get; set; }

	[DefaultValue(0.75f)]
	public float ImageQuality { get; set; } = 0.75f;


	[DefaultValue(100f)]
	public float ImageResolution { get; set; } = 100f;


	public ImageFormat ImageFormat { get; set; } = StiOptions.Export.Excel.ImageFormat;


	[DefaultValue(null)]
	public string CompanyString { get; set; } = StiExportUtils.GetReportVersion();


	[DefaultValue(null)]
	public string LastModifiedString { get; set; } = StiExportUtils.GetReportVersion();


	[DefaultValue(StiExcel2007RestrictEditing.No)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiExcel2007RestrictEditing RestrictEditing { get; set; } = StiExcel2007RestrictEditing.No;


	public override StiExportFormat GetExportFormat()
	{
		return ExcelType switch
		{
			StiExcelType.ExcelXml => StiExportFormat.ExcelXml, 
			StiExcelType.Excel2007 => StiExportFormat.Excel2007, 
			_ => StiExportFormat.Excel, 
		};
	}

	public StiExcelExportSettings()
		: this(StiExcelType.ExcelBinary)
	{
	}

	public StiExcelExportSettings(StiExcelType excelType)
	{
		this.excelType = excelType;
	}
}
