using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Html5.png")]
public class StiHtml5ExportService : StiExportService
{
	private StiReport reporTmp;

	private string documentFileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	internal StiReport report;

	internal string fileName = string.Empty;

	internal ImageFormat imageFormat;

	private NumberFormatInfo numberFormat = new NumberFormatInfo();

	public StiHtmlTextWriter HtmlWriter;

	private Stream baseStream;

	private StreamWriter streamWriter;

	internal float imageQuality = 0.75f;

	internal float imageResolution = 96f;

	internal bool compressToArchive;

	private CultureInfo currentCulture;

	public override string DefaultExtension
	{
		get
		{
			if (!compressToArchive)
			{
				return "html";
			}
			return "zip";
		}
	}

	public override StiExportFormat ExportFormat => StiExportFormat.Html5;

	public override string GroupCategory => "Web";

	public override int Position => 11;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeHtml5File");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportHtml(report, stream, settings as StiHtmlExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiHtml5ExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			reporTmp = report;
			documentFileName = fileName;
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
		if (compressToArchive)
		{
			return StiLocalization.Get("FileFilters", "ZipArchives");
		}
		return StiLocalization.Get("FileFilters", "HtmlFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			if (string.IsNullOrEmpty(documentFileName))
			{
				documentFileName = GetFileName(reporTmp, sendEMail);
			}
			if (documentFileName == null)
			{
				return;
			}
			StiFileUtils.ProcessReadOnly(documentFileName);
			try
			{
				using FileStream stream = new FileStream(documentFileName, FileMode.Create);
				StartProgress(guiMode);
				StiHtmlExportSettings stiHtmlExportSettings = new StiHtmlExportSettings();
				stiHtmlExportSettings.ImageFormat = (ImageFormat)form["ImageFormat"];
				stiHtmlExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiHtmlExportSettings.ImageResolution = (float)form["Resolution"];
				stiHtmlExportSettings.ImageQuality = (float)form["ImageQuality"];
				stiHtmlExportSettings.ContinuousPages = (bool)form["ContinuousPages"];
				StiHtmlExportSettings settings = stiHtmlExportSettings;
				StartExport(reporTmp, stream, settings, sendEMail, (bool)form["OpenAfterExport"], documentFileName, guiMode);
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		base.IsStopped = true;
	}

	private void RenderPage(StiReport report, StiPage page, ref int clipCounter)
	{
		MemoryStream memoryStream = new MemoryStream();
		StiSvgHelper.SaveToStream(report, page, memoryStream, compressed: false, standalone: false, ref clipCounter, imageFormat, imageQuality, imageResolution);
		HtmlWriter.Flush();
		streamWriter.Flush();
		baseStream.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		byte[] array = new byte[memoryStream.Length];
		memoryStream.Read(array, 0, array.Length);
		baseStream.Write(array, 3, array.Length - 3);
		baseStream.Flush();
	}

	private void RenderStartDoc(StiPagesCollection pages, Encoding encoding)
	{
		HtmlWriter.Write("<!DOCTYPE html>");
		HtmlWriter.WriteLine();
		HtmlWriter.Indent++;
		HtmlWriter.WriteFullBeginTag("head");
		HtmlWriter.WriteLine();
		HtmlWriter.Indent++;
		HtmlWriter.WriteFullBeginTag("title");
		HtmlWriter.Write(report.ReportAlias);
		HtmlWriter.WriteEndTag("title");
		HtmlWriter.WriteLine();
		HtmlWriter.WriteBeginTag("meta");
		HtmlWriter.WriteAttribute("charset", encoding.WebName);
		HtmlWriter.WriteEndTag("meta");
		HtmlWriter.WriteLine();
		HtmlWriter.Indent--;
		HtmlWriter.WriteEndTag("head");
		HtmlWriter.WriteLine();
		HtmlWriter.WriteBeginTag("body");
		HtmlWriter.WriteAttribute("bgcolor", ColorTranslator.ToHtml(Color.LightBlue));
		HtmlWriter.WriteAttribute("style", "text-align:center;");
		HtmlWriter.Write(">");
		HtmlWriter.Indent++;
		HtmlWriter.WriteLine();
	}

	private void RenderEndDoc()
	{
		HtmlWriter.Indent--;
		HtmlWriter.WriteEndTag("body");
		HtmlWriter.Indent--;
		HtmlWriter.WriteLine();
		HtmlWriter.WriteEndTag("html");
	}

	public void ExportHtml(StiReport report, Stream stream, StiHtmlExportSettings settings)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Html format");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		imageFormat = settings.ImageFormat;
		imageResolution = settings.ImageResolution;
		imageQuality = settings.ImageQuality;
		compressToArchive = settings.CompressToArchive;
		bool continuousPages = settings.ContinuousPages;
		StiZipWriter20 val = null;
		fileName = string.Empty;
		MemoryStream memoryStream = null;
		if (settings.CompressToArchive)
		{
			if (stream is FileStream fileStream)
			{
				fileName = fileStream.Name;
			}
			try
			{
				if (!string.IsNullOrEmpty(fileName))
				{
					fileName = Path.GetFileNameWithoutExtension(fileName);
				}
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = report.ReportName;
			}
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = "report";
			}
			val = new StiZipWriter20();
			val.Begin(stream, true);
			memoryStream = new MemoryStream();
			stream = memoryStream;
		}
		baseStream = stream;
		streamWriter = new StreamWriter(stream, settings.Encoding);
		HtmlWriter = new StiHtmlTextWriter(streamWriter);
		StiPagesCollection selectedPages = settings.PageRange.GetSelectedPages(report.RenderedPages);
		currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			this.report = report;
			RenderStartDoc(selectedPages, settings.Encoding);
			StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
			bool flag = (!StiLicenseKeyValidator.IsValidInReportsDesignerOrOnPlatform((StiProductIdent)2, licenseKey) || !StiDesignerAppStatus.IsRunning) && !StiLicenseKeyValidator.IsValidOnNetFramework(licenseKey);
			if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
			{
				flag = true;
			}
			if (!flag)
			{
				try
				{
					using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
					using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
					rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
				}
				catch (Exception)
				{
					flag = true;
				}
			}
			if (flag)
			{
				HtmlWriter.Write("<div style=\"FONT-SIZE: 10px; COLOR: red; FONT-FAMILY: Arial\">Stimulsoft Reports - Trial</div>");
			}
			base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
			HtmlWriter.Indent = 0;
			int clipCounter = 1;
			foreach (StiPage item in selectedPages)
			{
				selectedPages.GetPage(item);
				InvokeExporting(item, selectedPages, 0, 1);
				if (base.IsStopped)
				{
					return;
				}
				RenderPage(report, item, ref clipCounter);
				if (continuousPages)
				{
					HtmlWriter.Write("<br/>");
				}
				HtmlWriter.WriteLine();
			}
			RenderEndDoc();
			HtmlWriter.Flush();
			streamWriter.Flush();
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			report = null;
		}
		if (settings.CompressToArchive)
		{
			val.AddFile(fileName + ".html", memoryStream);
			val.End();
			val = null;
		}
	}

	public StiHtml5ExportService()
	{
		numberFormat.NumberDecimalSeparator = ".";
	}
}
