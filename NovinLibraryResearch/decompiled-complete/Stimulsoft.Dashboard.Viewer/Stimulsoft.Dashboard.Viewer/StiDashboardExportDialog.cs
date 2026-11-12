using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Dashboard.Viewer;

internal static class StiDashboardExportDialog
{
	public static void Save(IStiElement element, StiDashboardExportSettings settings)
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Invalid comparison between Unknown and I4
		try
		{
			StiSettings.Load();
			string text = (string.IsNullOrEmpty(StiDashboardViewerOptions.DefaultExportPath) ? StiSettings.GetStr("Dashboard", "ExportPath", string.Empty) : StiDashboardViewerOptions.DefaultExportPath);
			string fileName = GetFileName(element);
			SaveFileDialog val = new SaveFileDialog();
			try
			{
				if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
				{
					((FileDialog)val).InitialDirectory = text;
				}
				((FileDialog)val).Filter = GetFilter(settings);
				((FileDialog)val).RestoreDirectory = true;
				((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("Buttons", "Export"));
				((FileDialog)val).FileName = CorrectFileName(fileName);
				((FileDialog)val).DefaultExt = GetDefaultExtension(settings);
				if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
				{
					if (string.IsNullOrEmpty(StiDashboardViewerOptions.DefaultExportPath))
					{
						StiSettings.Load();
						StiSettings.Set("Dashboard", "ExportPath", Path.GetDirectoryName(((FileDialog)val).FileName));
						StiSettings.Save();
					}
					StiFileUtils.ProcessReadOnly(text);
					bool flag = StiDashboardExportTools.ExportToFile(element, ((FileDialog)val).FileName, settings, showProgress: true);
					if (settings.OpenAfterExport && flag)
					{
						OpenFile(((FileDialog)val).FileName);
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			if (ex is IOException)
			{
				MessageBox.Show(ex.Message, "IOException");
			}
			else
			{
				StiExceptionProvider.Show(ex);
			}
		}
	}

	private static void OpenFile(string path)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiProcess.Start("\"" + path + "\"");
		}
		catch (Exception ex)
		{
			if (ex is Win32Exception { NativeErrorCode: 1155 })
			{
				MessageBox.Show(ex.Message, StiLocalization.Get("Export", "OpenAfterExport"));
			}
			else
			{
				StiExceptionProvider.Show(ex);
			}
		}
	}

	private static string GetFilter(StiDashboardExportSettings settings)
	{
		return settings.Format switch
		{
			StiDashboardExportFormat.Pdf => Loc.Get("FileFilters", "PdfFiles"), 
			StiDashboardExportFormat.Excel => Loc.Get("FileFilters", "Excel2007Files"), 
			StiDashboardExportFormat.Html => Loc.Get("FileFilters", "HtmlFiles"), 
			StiDashboardExportFormat.Data => ((StiDataDashboardExportSettings)settings).DataType switch
			{
				StiDataType.Csv => Loc.Get("FileFilters", "CsvFiles"), 
				StiDataType.Dbf => Loc.Get("FileFilters", "DbfFiles"), 
				StiDataType.Xml => Loc.Get("FileFilters", "XmlFiles"), 
				StiDataType.Json => Loc.Get("FileFilters", "JsonFiles"), 
				StiDataType.Dif => Loc.Get("FileFilters", "DifFiles"), 
				StiDataType.Sylk => Loc.Get("FileFilters", "SylkFiles"), 
				_ => throw new StiTypeNotRecognizedException(settings.Format), 
			}, 
			StiDashboardExportFormat.Image => ((StiImageDashboardExportSettings)settings).ImageType switch
			{
				StiImageType.Bmp => Loc.Get("FileFilters", "BmpFiles"), 
				StiImageType.Gif => Loc.Get("FileFilters", "GifFiles"), 
				StiImageType.Png => Loc.Get("FileFilters", "PngFiles"), 
				StiImageType.Tiff => Loc.Get("FileFilters", "TiffFiles"), 
				StiImageType.Jpeg => Loc.Get("FileFilters", "JpegFiles"), 
				StiImageType.Pcx => Loc.Get("FileFilters", "PcxFiles"), 
				StiImageType.Emf => Loc.Get("FileFilters", "EmfFiles"), 
				StiImageType.Svg => Loc.Get("FileFilters", "SvgFiles"), 
				StiImageType.Svgz => Loc.Get("FileFilters", "SvgzFiles"), 
				_ => throw new StiTypeNotRecognizedException(settings.Format), 
			}, 
			_ => throw new StiTypeNotRecognizedException(settings.Format), 
		};
	}

	private static string GetDefaultExtension(StiDashboardExportSettings settings)
	{
		switch (settings.Format)
		{
		case StiDashboardExportFormat.Pdf:
			return "pdf";
		case StiDashboardExportFormat.Excel:
			return "xlsx";
		case StiDashboardExportFormat.Html:
			return "html";
		case StiDashboardExportFormat.Data:
		{
			StiDataType dataType = ((StiDataDashboardExportSettings)settings).DataType;
			if (dataType == StiDataType.Sylk)
			{
				return "slk";
			}
			return dataType.ToString().ToLower();
		}
		case StiDashboardExportFormat.Image:
		{
			StiImageType imageType = ((StiImageDashboardExportSettings)settings).ImageType;
			if (imageType == StiImageType.Jpeg)
			{
				return "jpg";
			}
			return imageType.ToString().ToLower();
		}
		default:
			throw new StiTypeNotRecognizedException(settings.Format);
		}
	}

	private static string GetFileName(IStiElement element)
	{
		IStiInteractionLayout stiInteractionLayout = (element as IStiElementInteraction)?.DashboardInteraction as IStiInteractionLayout;
		if (!string.IsNullOrWhiteSpace(stiInteractionLayout?.FileName))
		{
			string text = StiReportParser.Parse(stiInteractionLayout.FileName, element);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
		}
		IStiTitleElement stiTitleElement = element as IStiTitleElement;
		if (!string.IsNullOrWhiteSpace(stiTitleElement?.Title?.Text))
		{
			string text2 = CorrectFileName(stiTitleElement.Title?.Text);
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return text2;
			}
		}
		return element?.Report?.ReportAlias ?? Loc.Get("Components", "StiDashboard");
	}

	internal static string CorrectFileName(string str)
	{
		string text = "/\\:*?\"<>|";
		char[] array = str.ToCharArray();
		for (int i = 0; i < array.Length; i++)
		{
			int num = text.IndexOf(array[i]);
			if (num != -1)
			{
				array[i] = '_';
			}
		}
		return new string(array);
	}
}
