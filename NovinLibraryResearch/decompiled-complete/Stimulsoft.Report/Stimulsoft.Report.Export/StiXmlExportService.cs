using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Xml.png")]
public class StiXmlExportService : StiExportService
{
	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	public override string DefaultExtension => "xml";

	public override StiExportFormat ExportFormat => StiExportFormat.Xml;

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeXmlFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportXml(report, stream, settings as StiDataExportSettings);
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
		return StiLocalization.Get("FileFilters", "XmlFiles");
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

	public virtual void ExportXml(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportXml(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public virtual void ExportXml(StiReport report, Stream stream)
	{
		ExportXml(report, stream, new StiDataExportSettings());
	}

	public virtual void ExportXml(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		StiLogService.Write(((object)this).GetType(), "Export report to Xml format");
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
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
			string text = stiMatrix.Fields[i].Name;
			if (StiOptions.Export.Xml.ConvertTagsToUpperCase)
			{
				text = text.ToUpper(CultureInfo.InvariantCulture);
			}
			string text2 = string.Empty;
			int num = 0;
			while (hashtable.Contains(text + text2))
			{
				num++;
				text2 = num.ToString();
			}
			text += text2;
			stiMatrix.Fields[i].Name = text;
			hashtable.Add(text, text);
		}
		string text3 = StiNameValidator.CorrectName(string.IsNullOrWhiteSpace(report.ReportAlias) ? report.ReportName : report.ReportAlias);
		if (string.IsNullOrWhiteSpace(text3))
		{
			text3 = "Report";
		}
		DataSet dataSet = new DataSet(text3)
		{
			EnforceConstraints = false
		};
		DataTable dataTable = new DataTable();
		if (!string.IsNullOrWhiteSpace(settings.TableName))
		{
			dataTable.TableName = settings.TableName;
		}
		for (int j = 0; j < stiMatrix.Fields.Length; j++)
		{
			switch (stiMatrix.Fields[j].Info[0])
			{
			case 0:
				dataTable.Columns.Add(stiMatrix.Fields[j].Name, typeof(string));
				break;
			case 1:
				dataTable.Columns.Add(stiMatrix.Fields[j].Name, typeof(int));
				break;
			case 2:
				dataTable.Columns.Add(stiMatrix.Fields[j].Name, typeof(long));
				break;
			case 3:
				dataTable.Columns.Add(stiMatrix.Fields[j].Name, typeof(float));
				break;
			case 4:
				dataTable.Columns.Add(stiMatrix.Fields[j].Name, typeof(double));
				break;
			case 5:
				dataTable.Columns.Add(stiMatrix.Fields[j].Name, typeof(DateTime));
				break;
			}
		}
		dataSet.Tables.Add(dataTable);
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		base.CurrentPassNumber = 2;
		for (int k = 0; k < stiMatrix.DataArrayLength; k++)
		{
			InvokeExporting(k, stiMatrix.DataArrayLength, base.CurrentPassNumber, base.MaximumPassNumber);
			if (base.IsStopped)
			{
				return;
			}
			DataRow dataRow = dataTable.NewRow();
			for (int l = 0; l < stiMatrix.Fields.Length; l++)
			{
				string text4 = stiMatrix.Fields[l].DataArray[k];
				switch (stiMatrix.Fields[l].Info[0])
				{
				case 0:
					dataRow[l] = text4;
					break;
				case 1:
				{
					if (int.TryParse(text4, out var result5))
					{
						dataRow[l] = result5;
					}
					break;
				}
				case 2:
				{
					if (long.TryParse(text4, out var result2))
					{
						dataRow[l] = result2;
					}
					break;
				}
				case 3:
				{
					if (float.TryParse(text4.Replace(".", ",").Replace(",", numberDecimalSeparator), out var result4))
					{
						dataRow[l] = result4;
					}
					break;
				}
				case 4:
				{
					if (double.TryParse(text4.Replace(".", ",").Replace(",", numberDecimalSeparator), out var result3))
					{
						dataRow[l] = result3;
					}
					break;
				}
				case 5:
				{
					if (DateTime.TryParse(text4, out var result))
					{
						dataRow[l] = result;
					}
					break;
				}
				}
			}
			dataTable.Rows.Add(dataRow);
		}
		WriteDataSetToStream(dataSet, stream, settings);
		if (stiMatrix != null)
		{
			stiMatrix.Clear();
			stiMatrix = null;
		}
	}

	protected virtual void WriteDataSetToStream(DataSet dataSet, Stream stream, StiDataExportSettings settings)
	{
		XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8)
		{
			Indentation = 1,
			Formatting = Formatting.Indented
		};
		if (StiOptions.Export.Xml.WriteXmlDocumentDeclaration)
		{
			xmlTextWriter.WriteStartDocument(standalone: true);
		}
		dataSet.WriteXml(xmlTextWriter);
		dataSet.Dispose();
		xmlTextWriter.Flush();
	}
}
