using System;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Data.png")]
public class StiDataExportService : StiExportService
{
	private StiExportSettings exportSettings;

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	public override string DefaultExtension
	{
		get
		{
			if (exportSettings is StiDbfExportSettings)
			{
				return "dbf";
			}
			if (exportSettings is StiDifExportSettings)
			{
				return "dif";
			}
			if (exportSettings is StiSylkExportSettings)
			{
				return "slk";
			}
			if (exportSettings is StiXmlExportSettings)
			{
				return "xml";
			}
			if (exportSettings is StiJsonExportSettings)
			{
				return "json";
			}
			if (exportSettings is StiDataExportSettings)
			{
				switch ((exportSettings as StiDataExportSettings).DataType)
				{
				case StiDataType.Dbf:
					return "dbf";
				case StiDataType.Dif:
					return "dif";
				case StiDataType.Sylk:
					return "slk";
				case StiDataType.Xml:
					return "xml";
				case StiDataType.Json:
					return "json";
				}
			}
			return "csv";
		}
	}

	public override StiExportFormat ExportFormat
	{
		get
		{
			if (exportSettings is StiCsvExportSettings)
			{
				return StiExportFormat.Csv;
			}
			if (exportSettings is StiDbfExportSettings)
			{
				return StiExportFormat.Dbf;
			}
			if (exportSettings is StiDifExportSettings)
			{
				return StiExportFormat.Dif;
			}
			if (exportSettings is StiSylkExportSettings)
			{
				return StiExportFormat.Sylk;
			}
			if (exportSettings is StiXmlExportSettings)
			{
				return StiExportFormat.Xml;
			}
			if (exportSettings is StiJsonExportSettings)
			{
				return StiExportFormat.Json;
			}
			if (exportSettings is StiDataExportSettings)
			{
				switch ((exportSettings as StiDataExportSettings).DataType)
				{
				case StiDataType.Csv:
					return StiExportFormat.Csv;
				case StiDataType.Dbf:
					return StiExportFormat.Dbf;
				case StiDataType.Dif:
					return StiExportFormat.Dif;
				case StiDataType.Sylk:
					return StiExportFormat.Sylk;
				case StiDataType.Xml:
					return StiExportFormat.Xml;
				case StiDataType.Json:
					return StiExportFormat.Json;
				}
			}
			return StiExportFormat.Data;
		}
	}

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeDataFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportData(report, stream, settings as StiDataExportSettings);
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
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
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
				if (fileName != null)
				{
					StiDbfCodePages codePage = (StiDbfCodePages)Enum.Parse(typeof(StiDbfCodePages), StiDbfExportService.codePageCodes[(int)form["EncodingSelectedIndex"], 1].ToString());
					StiFileUtils.ProcessReadOnly(fileName);
					FileStream stream = new FileStream(fileName, FileMode.Create);
					stiDbfExportService.StartProgress(guiMode);
					StiDbfExportSettings stiDbfExportSettings = new StiDbfExportSettings();
					stiDbfExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiDbfExportSettings.CodePage = codePage;
					stiDbfExportService.StartExport(settings: exportSettings = stiDbfExportSettings, report: report, stream: stream, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: fileName, guiMode: guiMode);
				}
				return;
			}
			case StiExportFormat.Xml:
			{
				StiXmlExportService stiXmlExportService = new StiXmlExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiXmlExportService.GetFileName(report, sendEMail);
				}
				if (fileName != null)
				{
					StiFileUtils.ProcessReadOnly(fileName);
					FileStream stream4 = new FileStream(fileName, FileMode.Create);
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
					exportSettings = stiXmlExportSettings;
					stiXmlExportService.StartExport(report, stream4, stiXmlExportSettings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				}
				return;
			}
			case StiExportFormat.Json:
			{
				StiJsonExportService stiJsonExportService = new StiJsonExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiJsonExportService.GetFileName(report, sendEMail);
				}
				if (fileName != null)
				{
					StiFileUtils.ProcessReadOnly(fileName);
					FileStream stream5 = new FileStream(fileName, FileMode.Create);
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
					exportSettings = stiJsonExportSettings;
					stiJsonExportService.StartExport(report, stream5, stiJsonExportSettings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				}
				return;
			}
			case StiExportFormat.Dif:
			{
				StiDifExportService stiDifExportService = new StiDifExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiDifExportService.GetFileName(report, sendEMail);
				}
				if (fileName != null)
				{
					StiFileUtils.ProcessReadOnly(fileName);
					FileStream stream2 = new FileStream(fileName, FileMode.Create);
					stiDifExportService.StartProgress(guiMode);
					StiDifExportSettings stiDifExportSettings = new StiDifExportSettings();
					stiDifExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiDifExportSettings.ExportDataOnly = (bool)form["ExportDataOnly"];
					stiDifExportSettings.Encoding = form["Encoding"] as Encoding;
					stiDifExportSettings.UseDefaultSystemEncoding = (bool)form["UseDefaultSystemEncoding"];
					stiDifExportService.StartExport(settings: exportSettings = stiDifExportSettings, report: report, stream: stream2, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: fileName, guiMode: guiMode);
				}
				return;
			}
			case StiExportFormat.Sylk:
			{
				StiSylkExportService stiSylkExportService = new StiSylkExportService();
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = stiSylkExportService.GetFileName(report, sendEMail);
				}
				if (fileName != null)
				{
					StiFileUtils.ProcessReadOnly(fileName);
					FileStream stream3 = new FileStream(fileName, FileMode.Create);
					stiSylkExportService.StartProgress(guiMode);
					StiSylkExportSettings stiSylkExportSettings = new StiSylkExportSettings();
					stiSylkExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
					stiSylkExportSettings.ExportDataOnly = (bool)form["ExportDataOnly"];
					stiSylkExportSettings.Encoding = form["Encoding"] as Encoding;
					stiSylkExportSettings.UseDefaultSystemEncoding = (bool)form["UseDefaultSystemEncoding"];
					stiSylkExportService.StartExport(settings: exportSettings = stiSylkExportSettings, report: report, stream: stream3, sendEMail: sendEMail, openAfterExport: (bool)form["OpenAfterExport"], fileName: fileName, guiMode: guiMode);
				}
				return;
			}
			}
			StiCsvExportService stiCsvExportService = new StiCsvExportService();
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = stiCsvExportService.GetFileName(report, sendEMail);
			}
			if (fileName != null)
			{
				StiFileUtils.ProcessReadOnly(fileName);
				FileStream stream6 = new FileStream(fileName, FileMode.Create);
				stiCsvExportService.StartProgress(guiMode);
				StiCsvExportSettings stiCsvExportSettings = new StiCsvExportSettings();
				stiCsvExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiCsvExportSettings.Separator = form["Separator"] as string;
				stiCsvExportSettings.Encoding = form["Encoding"] as Encoding;
				stiCsvExportSettings.SkipColumnHeaders = (bool)form["SkipColumnHeaders"];
				StiCsvExportSettings stiCsvExportSettings2 = stiCsvExportSettings;
				int num3 = (int)form["DataExportMode"];
				if (num3 == 1)
				{
					stiCsvExportSettings2.DataExportMode = StiDataExportMode.DataAndHeadersFooters;
				}
				if (num3 == 2)
				{
					stiCsvExportSettings2.DataExportMode = StiDataExportMode.AllBands;
				}
				exportSettings = stiCsvExportSettings2;
				stiCsvExportService.StartExport(report, stream6, stiCsvExportSettings2, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
			}
		}
		else
		{
			base.IsStopped = true;
		}
	}

	public void ExportData(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		exportSettings = settings;
		StiDataType stiDataType = settings?.DataType ?? StiDataType.Csv;
		if (stiDataType == StiDataType.Dbf || settings is StiDbfExportSettings)
		{
			StiDbfExportService stiDbfExportService = new StiDbfExportService();
			stiDbfExportService.ExportDbf(report, stream, settings);
		}
		else if (stiDataType == StiDataType.Dif || settings is StiDifExportSettings)
		{
			StiDifExportService stiDifExportService = new StiDifExportService();
			stiDifExportService.ExportDif(report, stream, settings);
		}
		else if (stiDataType == StiDataType.Sylk || settings is StiSylkExportSettings)
		{
			StiSylkExportService stiSylkExportService = new StiSylkExportService();
			stiSylkExportService.ExportSylk(report, stream, settings);
		}
		else if (stiDataType == StiDataType.Xml || settings is StiXmlExportSettings)
		{
			StiXmlExportService stiXmlExportService = new StiXmlExportService();
			stiXmlExportService.ExportXml(report, stream, settings);
		}
		else if (stiDataType == StiDataType.Json)
		{
			StiJsonExportService stiJsonExportService = new StiJsonExportService();
			stiJsonExportService.ExportJson(report, stream, settings);
		}
		else
		{
			StiCsvExportService stiCsvExportService = new StiCsvExportService();
			stiCsvExportService.ExportCsv(report, stream, settings);
		}
	}
}
