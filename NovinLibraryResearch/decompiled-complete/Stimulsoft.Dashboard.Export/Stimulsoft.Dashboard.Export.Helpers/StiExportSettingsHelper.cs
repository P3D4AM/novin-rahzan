using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Export.Helpers;

public static class StiExportSettingsHelper
{
	public static StiExportSettings GetExportSettings(StiDashboardExportSettings settings)
	{
		switch (settings.Format)
		{
		case StiDashboardExportFormat.Pdf:
			return new StiPdfExportSettings
			{
				ImageQuality = 1f,
				ImageResolution = ((StiPdfDashboardExportSettings)settings).ImageQuality
			};
		case StiDashboardExportFormat.Excel:
			return new StiExcel2007ExportSettings
			{
				ImageQuality = 1f,
				ImageResolution = ((StiExcelDashboardExportSettings)settings).ImageQuality,
				DataExportMode = (((StiExcelDashboardExportSettings)settings).ExportDataOnly ? StiDataExportMode.DataAndHeadersFooters : StiDataExportMode.AllBands)
			};
		case StiDashboardExportFormat.Data:
			return new StiDataExportSettings
			{
				DataType = ((StiDataDashboardExportSettings)settings).DataType,
				DataExportMode = StiDataExportMode.DataAndHeadersFooters,
				SkipColumnHeaders = true
			};
		case StiDashboardExportFormat.Html:
			return new StiHtmlExportSettings
			{
				ImageResolution = ((StiHtmlDashboardExportSettings)settings).ImageQuality,
				UseEmbeddedImages = true,
				Zoom = (float)((StiHtmlDashboardExportSettings)settings).Scale / 100f,
				AddPageBreaks = false,
				ChartType = (((StiHtmlDashboardExportSettings)settings).EnableAnimation ? StiHtmlChartType.AnimatedVector : StiHtmlChartType.Vector)
			};
		case StiDashboardExportFormat.Image:
		{
			StiImageType imageType = ((StiImageDashboardExportSettings)settings).ImageType;
			if (imageType == StiImageType.Svg)
			{
				return new StiSvgExportSettings();
			}
			return new StiImageExportSettings
			{
				ImageType = imageType,
				ImageZoom = (float)((StiImageDashboardExportSettings)settings).Scale / 100f,
				ImageResolution = 100
			};
		}
		case StiDashboardExportFormat.Html5:
			return new StiSvgExportSettings();
		case StiDashboardExportFormat.Document:
			return null;
		default:
			throw new StiTypeNotRecognizedException(settings.Format);
		}
	}

	public static StiDashboardExportSettings GetDashboardExportSettings(StiExportSettings settings)
	{
		StiExportFormat stiExportFormat = settings?.GetExportFormat() ?? StiExportFormat.Document;
		StiDashboardExportSettings dashboardExportSettings = GetDashboardExportSettings(stiExportFormat);
		if (stiExportFormat == StiExportFormat.Html)
		{
			((StiHtmlDashboardExportSettings)dashboardExportSettings).EnableAnimation = ((StiHtmlExportSettings)settings).ChartType == StiHtmlChartType.AnimatedVector;
		}
		return dashboardExportSettings;
	}

	public static StiDashboardExportSettings GetDashboardExportSettings(StiExportFormat format)
	{
		switch (format)
		{
		case StiExportFormat.Pdf:
			return new StiPdfDashboardExportSettings();
		case StiExportFormat.Excel:
		case StiExportFormat.Excel2007:
			return new StiExcelDashboardExportSettings();
		case StiExportFormat.Data:
			return new StiDataDashboardExportSettings();
		case StiExportFormat.Csv:
			return new StiDataDashboardExportSettings(StiDataType.Csv);
		case StiExportFormat.Dbf:
			return new StiDataDashboardExportSettings(StiDataType.Dbf);
		case StiExportFormat.Xml:
			return new StiDataDashboardExportSettings(StiDataType.Xml);
		case StiExportFormat.Json:
			return new StiDataDashboardExportSettings(StiDataType.Json);
		case StiExportFormat.Dif:
			return new StiDataDashboardExportSettings(StiDataType.Dif);
		case StiExportFormat.Sylk:
			return new StiDataDashboardExportSettings(StiDataType.Sylk);
		case StiExportFormat.Image:
			return new StiImageDashboardExportSettings();
		case StiExportFormat.ImageBmp:
			return new StiImageDashboardExportSettings(StiImageType.Bmp);
		case StiExportFormat.ImageGif:
			return new StiImageDashboardExportSettings(StiImageType.Gif);
		case StiExportFormat.ImagePng:
			return new StiImageDashboardExportSettings(StiImageType.Png);
		case StiExportFormat.ImageTiff:
			return new StiImageDashboardExportSettings(StiImageType.Tiff);
		case StiExportFormat.ImageJpeg:
			return new StiImageDashboardExportSettings(StiImageType.Jpeg);
		case StiExportFormat.ImageEmf:
			return new StiImageDashboardExportSettings(StiImageType.Emf);
		case StiExportFormat.ImagePcx:
			return new StiImageDashboardExportSettings(StiImageType.Pcx);
		case StiExportFormat.ImageSvgz:
			return new StiImageDashboardExportSettings(StiImageType.Svgz);
		case StiExportFormat.ImageSvg:
			return new StiSvgDashboardExportSettings();
		case StiExportFormat.Html:
		case StiExportFormat.Html5:
			return new StiHtmlDashboardExportSettings();
		case StiExportFormat.Document:
			return null;
		default:
			throw new StiTypeNotRecognizedException(format);
		}
	}

	public static bool IsDataExport(StiDashboardExportSettings settings)
	{
		if (!(settings is StiDataDashboardExportSettings))
		{
			if (settings is StiExcelDashboardExportSettings)
			{
				return ((StiExcelDashboardExportSettings)settings).ExportDataOnly;
			}
			return false;
		}
		return true;
	}
}
