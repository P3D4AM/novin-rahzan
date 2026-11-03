using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Dif.png")]
public class StiDifExportService : StiExportService
{
	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	private StreamWriter writer;

	public override string DefaultExtension => "dif";

	public override StiExportFormat ExportFormat => StiExportFormat.Dif;

	public override string GroupCategory => "Data";

	public override int Position => 40;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeDifFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportDif(report, stream, settings as StiDataExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiDifExportSetupForm", guiMode, base.OwnerWindow);
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
		return StiLocalization.Get("FileFilters", "DifFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
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
				StiDataExportSettings stiDataExportSettings = new StiDataExportSettings();
				stiDataExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiDataExportSettings.ExportDataOnly = (bool)form["ExportDataOnly"];
				stiDataExportSettings.Encoding = form["Encoding"] as Encoding;
				stiDataExportSettings.UseDefaultSystemEncoding = (bool)form["UseDefaultSystemEncoding"];
				StiDataExportSettings settings = stiDataExportSettings;
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

	public void ExportDif(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportDif(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportDif(StiReport report, Stream stream)
	{
		ExportDif(report, stream, new StiDataExportSettings());
	}

	public void ExportDif(StiReport report, Stream stream, StiPagesRange pageRange, bool exportDataOnly, bool useDefaultSystemEncoding, Encoding encoding)
	{
		StiDataExportSettings settings = new StiDataExportSettings
		{
			PageRange = pageRange,
			ExportDataOnly = exportDataOnly,
			UseDefaultSystemEncoding = useDefaultSystemEncoding,
			Encoding = encoding
		};
		ExportDif(report, stream, settings);
	}

	public void ExportDif(StiReport report, Stream stream, StiDataExportSettings settings)
	{
		StiLogService.Write(((object)this).GetType(), "Export report to Dif format");
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
		bool exportDataOnly = settings.ExportDataOnly;
		bool useDefaultSystemEncoding = settings.UseDefaultSystemEncoding;
		Encoding encoding = settings.Encoding;
		if (useDefaultSystemEncoding)
		{
			encoding = Encoding.GetEncoding(CultureInfo.InstalledUICulture.TextInfo.OEMCodePage);
		}
		writer = new StreamWriter(stream, encoding);
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		base.CurrentPassNumber = 0;
		base.MaximumPassNumber = 3;
		StiDataExportMode stiDataExportMode = (exportDataOnly ? StiDataExportMode.DataAndHeaders : StiDataExportMode.AllBands);
		StiMatrix stiMatrix = new StiMatrix(selectedPages, checkForExcel: true, this, null, stiDataExportMode);
		stiMatrix.ScanComponentsPlacement(optimize: false);
		if (base.IsStopped)
		{
			return;
		}
		writer.WriteLine("TABLE");
		writer.WriteLine("0,1");
		writer.WriteLine("\"EXCEL\"");
		writer.WriteLine("VECTORS");
		writer.WriteLine($"0,{stiMatrix.CoordX.Count - 1}");
		writer.WriteLine("\"\"");
		writer.WriteLine("TUPLES");
		writer.WriteLine($"0,{stiMatrix.CoordY.Count - 1}");
		writer.WriteLine("\"\"");
		writer.WriteLine("DATA");
		writer.WriteLine("0,0");
		writer.WriteLine("\"\"");
		base.CurrentPassNumber = 2;
		if (exportDataOnly)
		{
			stiMatrix.PrepareDocument(this, stiDataExportMode);
			for (int i = 0; i < stiMatrix.DataArrayLength; i++)
			{
				InvokeExporting(i, stiMatrix.DataArrayLength - 1, base.CurrentPassNumber, base.MaximumPassNumber);
				if (base.IsStopped)
				{
					return;
				}
				writer.WriteLine("-1,0");
				writer.WriteLine("BOT");
				for (int j = 0; j < stiMatrix.Fields.Length; j++)
				{
					string text = stiMatrix.Fields[j].DataArray[i];
					if (text == null)
					{
						text = string.Empty;
					}
					if (stiMatrix.Fields[j].Info[0] != 0)
					{
						writer.WriteLine("0,{0}", text);
						writer.WriteLine("V");
					}
					else
					{
						writer.WriteLine("1,0");
						writer.WriteLine("\"{0}\"", text);
					}
				}
			}
		}
		else
		{
			RichTextBox val = null;
			for (int k = 0; k < stiMatrix.CoordY.Count - 1; k++)
			{
				InvokeExporting(k, stiMatrix.CoordY.Count - 1, base.CurrentPassNumber, base.MaximumPassNumber);
				if (base.IsStopped)
				{
					return;
				}
				writer.WriteLine("-1,0");
				writer.WriteLine("BOT");
				for (int l = 0; l < stiMatrix.CoordX.Count - 1; l++)
				{
					string text2 = null;
					string text3 = string.Empty;
					StiCell stiCell = stiMatrix.Cells[k, l];
					if (stiCell != null)
					{
						text2 = stiCell.Text;
						if (stiCell.Component is StiRichText stiRichText && stiRichText.RtfText != string.Empty)
						{
							if (val == null)
							{
								val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
							}
							stiRichText.GetPreparedText(val);
							text2 = ((Control)val).Text;
						}
						if (text2 == null)
						{
							text2 = string.Empty;
						}
						if (stiCell.Component is StiText stiText)
						{
							text3 = stiText.Format;
						}
					}
					if (text2 == null)
					{
						text2 = string.Empty;
					}
					bool flag = false;
					if (!string.IsNullOrEmpty(text3) && (text3[0] == 'C' || text3[0] == 'N' || text3[0] == 'P' || text3[0] == 'D' || text3[0] == 'T'))
					{
						flag = true;
					}
					if (flag)
					{
						writer.WriteLine("0,{0}", text2);
						writer.WriteLine("V");
					}
					else
					{
						writer.WriteLine("1,0");
						writer.WriteLine("\"{0}\"", text2);
					}
				}
			}
		}
		writer.WriteLine("-1,0");
		writer.WriteLine("EOD");
		writer.Flush();
		if (stiMatrix != null)
		{
			stiMatrix.Clear();
			stiMatrix = null;
		}
	}
}
