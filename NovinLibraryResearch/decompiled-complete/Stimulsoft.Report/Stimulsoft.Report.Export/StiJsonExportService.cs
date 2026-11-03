using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Json.png")]
public class StiJsonExportService : StiExportService
{
	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	public override string DefaultExtension => "json";

	public override StiExportFormat ExportFormat => StiExportFormat.Json;

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeJsonFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportJson(report, stream, settings as StiDataExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiXmlExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
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
		return StiLocalization.Get("FileFilters", "JsonFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
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
				StiDataExportSettings settings = new StiDataExportSettings();
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

	public void ExportJson(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportJson(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportJson(StiReport report, Stream stream)
	{
		ExportJson(report, stream, new StiDataExportSettings());
	}

	public void ExportJson(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Json format");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiDataExportMode dataExportMode = settings.DataExportMode;
		string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		StiPagesCollection selectedPages = StiPagesRange.All.GetSelectedPages(report.RenderedPages);
		StiMatrix stiMatrix = new StiMatrix(selectedPages, checkForExcel: false, this);
		stiMatrix.ScanComponentsPlacement(optimize: false);
		if (base.IsStopped)
		{
			return;
		}
		stiMatrix.PrepareDocument(this, dataExportMode);
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i < stiMatrix.Fields.Length; i++)
		{
			string name = stiMatrix.Fields[i].Name;
			string text = string.Empty;
			int num = 0;
			while (hashtable.Contains(name + text))
			{
				num++;
				text = num.ToString();
			}
			name += text;
			stiMatrix.Fields[i].Name = name;
			hashtable.Add(name, name);
		}
		JArray val = new JArray();
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		base.CurrentPassNumber = 2;
		for (int j = 0; j < stiMatrix.DataArrayLength; j++)
		{
			InvokeExporting(j, stiMatrix.DataArrayLength, base.CurrentPassNumber, base.MaximumPassNumber);
			if (base.IsStopped)
			{
				return;
			}
			JObject val2 = new JObject();
			for (int k = 0; k < stiMatrix.Fields.Length; k++)
			{
				string text2 = stiMatrix.Fields[k].DataArray[j];
				switch (stiMatrix.Fields[k].Info[0])
				{
				case 1:
				case 2:
				{
					if (int.TryParse(text2, out var result4))
					{
						JsonExtensions.AddPropertyInt(val2, stiMatrix.Fields[k].Name, result4, 0);
						continue;
					}
					break;
				}
				case 3:
				{
					if (float.TryParse(text2.Replace(".", ",").Replace(",", numberDecimalSeparator), out var result2))
					{
						JsonExtensions.AddPropertyFloat(val2, stiMatrix.Fields[k].Name, result2, 0f);
						continue;
					}
					break;
				}
				case 4:
				{
					if (double.TryParse(text2.Replace(".", ",").Replace(",", numberDecimalSeparator), out var result3))
					{
						JsonExtensions.AddPropertyDouble(val2, stiMatrix.Fields[k].Name, result3);
						continue;
					}
					break;
				}
				case 5:
				{
					if (DateTime.TryParse(text2, out var result))
					{
						JsonExtensions.AddPropertyDateTime(val2, stiMatrix.Fields[k].Name, result);
						continue;
					}
					break;
				}
				}
				JsonExtensions.AddPropertyString(val2, stiMatrix.Fields[k].Name, text2);
			}
			val.Add((JToken)(object)val2);
		}
		JObject val3 = new JObject();
		string text3 = StiNameValidator.CorrectName(string.IsNullOrWhiteSpace(report.ReportAlias) ? report.ReportName : report.ReportAlias);
		if (!string.IsNullOrWhiteSpace(settings.TableName))
		{
			text3 = settings.TableName;
		}
		if (string.IsNullOrWhiteSpace(text3))
		{
			text3 = "Table1";
		}
		val3.Add(text3, (JToken)(object)val);
		string s = ((object)val3).ToString();
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		stream.Write(bytes, 0, bytes.Length);
		stream.Flush();
		if (stiMatrix != null)
		{
			stiMatrix.Clear();
			stiMatrix = null;
		}
	}
}
