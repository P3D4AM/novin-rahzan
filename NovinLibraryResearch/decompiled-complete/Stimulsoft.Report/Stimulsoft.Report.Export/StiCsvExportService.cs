using System;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Csv.png")]
public class StiCsvExportService : StiExportService
{
	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StreamWriter writer;

	public override string DefaultExtension => "csv";

	public override StiExportFormat ExportFormat => StiExportFormat.Csv;

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeCsvFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportCsv(report, stream, settings as StiDataExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiDataSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			this.report = report;
			this.fileName = fileName;
			this.sendEMail = sendEMail;
			this.guiMode = guiMode;
			exportFormRunner.Complete += new StiShowDialogCompleteEventHandler(Form_Complete);
			exportFormRunner.ShowDialog();
		}
		finally
		{
			((IDisposable)exportFormRunner)?.Dispose();
		}
	}

	public override string GetFilter()
	{
		return StiLocalization.Get("FileFilters", "CsvFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			switch ((StiExportFormat)form["ExportFormat"])
			{
			case StiExportFormat.Dbf:
			{
				StiDbfExportService stiDbfExportService = new StiDbfExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiDbfExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiDbfCodePages codePage = (StiDbfCodePages)Enum.Parse(typeof(StiDbfCodePages), StiDbfExportService.codePageCodes[(int)form["EncodingSelectedIndex"], 1].ToString());
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream = new FileStream(fileName, FileMode.Create);
					stiDbfExportService.StartProgress(guiMode);
					StiDbfExportSettings stiDbfExportSettings = new StiDbfExportSettings();
					stiDbfExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiDbfExportSettings.CodePage = codePage;
					StiDbfExportSettings settings = stiDbfExportSettings;
					stiDbfExportService.StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
					return;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			case StiExportFormat.Xml:
			{
				StiXmlExportService stiXmlExportService = new StiXmlExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiXmlExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream2 = new FileStream(fileName, FileMode.Create);
					stiXmlExportService.StartProgress(guiMode);
					StiXmlExportSettings stiXmlExportSettings = new StiXmlExportSettings();
					int num = (int)form["DataExportMode"];
					if (num == 1)
					{
						stiXmlExportSettings.DataExportMode = StiDataExportMode.DataAndHeadersFooters;
					}
					if (num == 2)
					{
						stiXmlExportSettings.DataExportMode = StiDataExportMode.AllBands;
					}
					stiXmlExportService.StartExport(report, stream2, stiXmlExportSettings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
					return;
				}
				catch (Exception ex2)
				{
					throw ex2;
				}
			}
			case StiExportFormat.Json:
			{
				StiJsonExportService stiJsonExportService = new StiJsonExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiJsonExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream5 = new FileStream(fileName, FileMode.Create);
					stiJsonExportService.StartProgress(guiMode);
					StiJsonExportSettings stiJsonExportSettings = new StiJsonExportSettings();
					int num2 = (int)form["DataExportMode"];
					if (num2 == 1)
					{
						stiJsonExportSettings.DataExportMode = StiDataExportMode.DataAndHeadersFooters;
					}
					if (num2 == 2)
					{
						stiJsonExportSettings.DataExportMode = StiDataExportMode.AllBands;
					}
					stiJsonExportService.StartExport(report, stream5, stiJsonExportSettings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
					return;
				}
				catch (Exception ex5)
				{
					throw ex5;
				}
			}
			case StiExportFormat.Dif:
			{
				StiDifExportService stiDifExportService = new StiDifExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiDifExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream3 = new FileStream(fileName, FileMode.Create);
					stiDifExportService.StartProgress(guiMode);
					StiDifExportSettings stiDifExportSettings = new StiDifExportSettings();
					stiDifExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiDifExportSettings.ExportDataOnly = (bool)form["ExportDataOnly"];
					stiDifExportSettings.Encoding = form["Encoding"] as Encoding;
					stiDifExportSettings.UseDefaultSystemEncoding = (bool)form["UseDefaultSystemEncoding"];
					StiDifExportSettings settings2 = stiDifExportSettings;
					stiDifExportService.StartExport(report, stream3, settings2, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
					return;
				}
				catch (Exception ex3)
				{
					throw ex3;
				}
			}
			case StiExportFormat.Sylk:
			{
				StiSylkExportService stiSylkExportService = new StiSylkExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiSylkExportService.GetFileName(report, sendEMail);
				}
				if (fileName == null)
				{
					return;
				}
				StiFileUtils.ProcessReadOnly(fileName);
				try
				{
					using FileStream stream4 = new FileStream(fileName, FileMode.Create);
					stiSylkExportService.StartProgress(guiMode);
					StiSylkExportSettings stiSylkExportSettings = new StiSylkExportSettings();
					stiSylkExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiSylkExportSettings.ExportDataOnly = (bool)form["ExportDataOnly"];
					stiSylkExportSettings.Encoding = form["Encoding"] as Encoding;
					stiSylkExportSettings.UseDefaultSystemEncoding = (bool)form["UseDefaultSystemEncoding"];
					StiSylkExportSettings settings3 = stiSylkExportSettings;
					stiSylkExportService.StartExport(report, stream4, settings3, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
					return;
				}
				catch (Exception ex4)
				{
					throw ex4;
				}
			}
			}
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
				using FileStream stream6 = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StiDataExportSettings stiDataExportSettings = new StiDataExportSettings();
				stiDataExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiDataExportSettings.Separator = form["Separator"] as string;
				stiDataExportSettings.Encoding = form["Encoding"] as Encoding;
				stiDataExportSettings.SkipColumnHeaders = (bool)form["SkipColumnHeaders"];
				StiDataExportSettings stiDataExportSettings2 = stiDataExportSettings;
				int num3 = (int)form["DataExportMode"];
				if (num3 == 1)
				{
					stiDataExportSettings2.DataExportMode = StiDataExportMode.DataAndHeadersFooters;
				}
				if (num3 == 2)
				{
					stiDataExportSettings2.DataExportMode = StiDataExportMode.AllBands;
				}
				StartExport(report, stream6, stiDataExportSettings2, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				return;
			}
			catch (Exception ex6)
			{
				throw ex6;
			}
		}
		base.IsStopped = true;
	}

	public void ExportCsv(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportCsv(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportCsv(StiReport report, Stream stream)
	{
		StiDataExportSettings settings = new StiDataExportSettings();
		ExportCsv(report, stream, settings);
	}

	public void ExportCsv(StiReport report, Stream stream, StiPagesRange pageRange, string separator, Encoding encoding)
	{
		StiDataExportSettings settings = new StiDataExportSettings
		{
			PageRange = pageRange,
			Separator = separator,
			Encoding = encoding
		};
		ExportCsv(report, stream, settings);
	}

	public void ExportCsv(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		StiLogService.Write(((object)this).GetType(), "Export report to Csv format");
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		string text = settings.Separator;
		Encoding encoding = settings.Encoding;
		bool skipColumnHeaders = settings.SkipColumnHeaders;
		StiDataExportMode dataExportMode = settings.DataExportMode;
		if (text == "\\t")
		{
			text = "\t";
		}
		if (StiOptions.Export.Csv.ForcedSeparator != null && StiOptions.Export.Csv.ForcedSeparator.Trim().Length > 0)
		{
			text = StiOptions.Export.Csv.ForcedSeparator.Trim();
		}
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		StiMatrix stiMatrix = new StiMatrix(selectedPages, checkForExcel: false, this, null, dataExportMode);
		stiMatrix.ScanComponentsPlacement(optimize: false);
		if (base.IsStopped)
		{
			return;
		}
		stiMatrix.PrepareDocument(this, dataExportMode);
		writer = new StreamWriter(stream, encoding);
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		if (!skipColumnHeaders)
		{
			for (int i = 0; i < stiMatrix.Fields.Length; i++)
			{
				string text2 = stiMatrix.Fields[i].Name.Replace("\"", "\"\"");
				if (text2.IndexOf(text, StringComparison.InvariantCulture) != -1 || text2.IndexOf('"') != -1 || text2.IndexOf('\n') != -1)
				{
					text2 = "\"" + text2 + "\"";
				}
				writer.Write(text2);
				if (i < stiMatrix.Fields.Length - 1)
				{
					writer.Write(text);
				}
			}
			writer.WriteLine();
		}
		base.CurrentPassNumber = 2;
		for (int j = 0; j < stiMatrix.DataArrayLength; j++)
		{
			InvokeExporting(j, stiMatrix.DataArrayLength - 1, base.CurrentPassNumber, base.MaximumPassNumber);
			if (base.IsStopped)
			{
				return;
			}
			for (int k = 0; k < stiMatrix.Fields.Length; k++)
			{
				string text3 = stiMatrix.Fields[k].DataArray[j];
				if (text3 == null)
				{
					text3 = string.Empty;
				}
				if (text3 != string.Empty)
				{
					if (text3.IndexOf(text, StringComparison.InvariantCulture) != -1 || text3.IndexOf('"') != -1 || text3.IndexOf('\n') != -1)
					{
						text3 = text3.Replace("\"", "\"\"");
						text3 = "\"" + text3 + "\"";
					}
					writer.Write(text3);
				}
				if (k < stiMatrix.Fields.Length - 1)
				{
					writer.Write(text);
				}
			}
			writer.WriteLine();
		}
		writer.Flush();
		if (stiMatrix != null)
		{
			stiMatrix.Clear();
			stiMatrix = null;
		}
	}
}
