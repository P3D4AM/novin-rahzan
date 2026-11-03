using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Mht.png")]
public class StiMhtExportService : StiExportService
{
	internal string fileName = string.Empty;

	internal bool compressToArchive;

	private StiReport report;

	private string documentFileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	public override string DefaultExtension
	{
		get
		{
			if (!compressToArchive)
			{
				return "mht";
			}
			return "zip";
		}
	}

	public override StiExportFormat ExportFormat => StiExportFormat.Mht;

	public override string GroupCategory => "Web";

	public override int Position => 12;

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeMhtFile");

	public override bool MultipleFiles => false;

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportMht(report, stream, settings as StiHtmlExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiHtmlExportSetupForm", guiMode, base.OwnerWindow);
		try
		{
			exportFormRunner["CurrentPage"] = report.CurrentPrintPage;
			exportFormRunner["OpenAfterExportEnabled"] = !sendEMail;
			exportFormRunner["ExportFormat"] = StiExportFormat.Mht;
			this.report = report;
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
		return StiLocalization.Get("FileFilters", "MhtFiles");
	}

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			if (string.IsNullOrEmpty(documentFileName))
			{
				documentFileName = GetFileName(report, sendEMail);
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
				stiHtmlExportSettings.Zoom = (float)form["Zoom"];
				stiHtmlExportSettings.ImageFormat = (ImageFormat)form["ImageFormat"];
				stiHtmlExportSettings.PageRange = form["PagesRange"] as StiPagesRange;
				stiHtmlExportSettings.ExportMode = (StiHtmlExportMode)form["ExportMode"];
				stiHtmlExportSettings.ExportQuality = StiHtmlExportQuality.High;
				stiHtmlExportSettings.AddPageBreaks = (bool)form["AddPageBreaks"];
				stiHtmlExportSettings.Encoding = Encoding.UTF8;
				StiHtmlExportSettings settings = stiHtmlExportSettings;
				fileName = documentFileName;
				StartExport(report, stream, settings, sendEMail, (bool)form["OpenAfterExport"], documentFileName, guiMode);
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		base.IsStopped = true;
	}

	public void ExportMht(StiReport report, string fileName)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportMht(report, fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public void ExportMht(StiReport report, Stream stream)
	{
		ExportMht(report, stream, new StiHtmlExportSettings());
	}

	public void ExportMht(StiReport report, Stream stream, double zoom, ImageFormat imageFormat)
	{
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			ImageFormat = imageFormat
		};
		ExportMht(report, stream, settings);
	}

	public void ExportMht(StiReport report, Stream stream, StiPagesRange pageRange)
	{
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			PageRange = pageRange
		};
		ExportMht(report, stream, settings);
	}

	public void ExportMht(StiReport report, Stream stream, double zoom, ImageFormat imageFormat, StiPagesRange pageRange)
	{
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			PageRange = pageRange,
			Zoom = zoom,
			ImageFormat = imageFormat
		};
		ExportMht(report, stream, settings);
	}

	public void ExportMht(StiReport report, Stream stream, double zoom, ImageFormat imageFormat, StiPagesRange pageRange, StiHtmlExportMode exportMode, Encoding encoding)
	{
		StiHtmlExportSettings settings = new StiHtmlExportSettings
		{
			PageRange = pageRange,
			Zoom = zoom,
			ImageFormat = imageFormat,
			ExportMode = exportMode,
			Encoding = encoding
		};
		ExportMht(report, stream, settings);
	}

	public void ExportMht(StiReport report, Stream stream, StiHtmlExportSettings settings)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), "Export report to Mht format");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		StiPagesRange pageRange = settings.PageRange;
		Encoding encoding = settings.Encoding;
		compressToArchive = settings.CompressToArchive;
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
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			StiHtmlExportService stiHtmlExportService = new StiHtmlExportService();
			stiHtmlExportService.HtmlImageHost = new StiMhtImageHost(stiHtmlExportService);
			MemoryStream memoryStream2 = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream2, encoding);
			StiHtmlTextWriter writer = new StiHtmlTextWriter(streamWriter);
			StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
			stiHtmlExportService.fileName = fileName;
			StiMhtImageHost stiMhtImageHost = stiHtmlExportService.HtmlImageHost as StiMhtImageHost;
			stiHtmlExportService.Progress = base.Progress;
			StiHtmlExportSettings settings2 = new StiHtmlExportSettings
			{
				AddPageBreaks = settings.AddPageBreaks,
				Encoding = settings.Encoding,
				ExportMode = settings.ExportMode,
				ExportQuality = settings.ExportQuality,
				ImageFormat = settings.ImageFormat,
				PageRange = settings.PageRange,
				Zoom = settings.Zoom,
				ExportBookmarksMode = settings.ExportBookmarksMode,
				BookmarksTreeWidth = settings.BookmarksTreeWidth,
				ImageQuality = settings.ImageQuality,
				ImageResolution = settings.ImageResolution
			};
			stiHtmlExportService.ExportHtml(report, writer, settings2, settings.PageRange.GetSelectedPages(report.RenderedPages));
			streamWriter.Flush();
			memoryStream2.WriteByte(13);
			memoryStream2.WriteByte(10);
			DateTime now = DateTime.Now;
			string arg = "----=_NextPart_000_0000_" + now.Year.ToString("X4") + now.Month.ToString("X2") + now.Day.ToString("X2") + "." + now.Hour.ToString("X2") + now.Minute.ToString("X2") + now.Second.ToString("X2");
			string text = string.Empty;
			if (!string.IsNullOrEmpty(stiHtmlExportService.fileName))
			{
				text = Path.GetDirectoryName(stiHtmlExportService.fileName);
			}
			if (text.Length > 0 && text[text.Length - 1] != '\\')
			{
				text += "\\";
			}
			string text2 = Path.GetFileNameWithoutExtension(fileName) + ".files\\";
			string text3 = "file:///" + text + Path.GetFileNameWithoutExtension(fileName) + ".html";
			StreamWriter streamWriter2 = new StreamWriter(stream, Encoding.ASCII);
			streamWriter2.WriteLine("From: <Saved by Stimulsoft Reports>");
			streamWriter2.WriteLine("Subject: {0}", report.ReportName);
			streamWriter2.WriteLine("Date: {0}", now.ToString("r"));
			streamWriter2.WriteLine("MIME-Version: 1.0");
			streamWriter2.WriteLine("Content-Type: multipart/related;");
			streamWriter2.WriteLine("\ttype=\"text/html\";");
			streamWriter2.WriteLine("\tboundary=\"{0}\"", arg);
			streamWriter2.WriteLine("");
			streamWriter2.WriteLine("--{0}", arg);
			streamWriter2.WriteLine("Content-Type: text/html;");
			streamWriter2.WriteLine("\tcharset=\"{0}\"", encoding.WebName);
			streamWriter2.WriteLine("Content-Transfer-Encoding: quoted-printable");
			streamWriter2.WriteLine("Content-Location: {0}", StiExportUtils.StringToUrl(text3.Replace("\\", "/").Replace(" ", "_")));
			streamWriter2.WriteLine("");
			streamWriter2.Flush();
			memoryStream2.Seek(0L, SeekOrigin.Begin);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < memoryStream2.Length; i++)
			{
				int num = memoryStream2.ReadByte();
				if (num == 10)
				{
					continue;
				}
				if (num != 13 && stringBuilder.Length > 72)
				{
					stringBuilder.Append("=");
					stringBuilder.Append('\r');
					stringBuilder.Append('\n');
					for (int j = 0; j < stringBuilder.Length; j++)
					{
						stream.WriteByte((byte)stringBuilder[j]);
					}
					stringBuilder.Length = 0;
				}
				if (num >= 32 && num <= 126 && num != 61)
				{
					stringBuilder.Append((char)num);
				}
				else if (num == 13)
				{
					if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ' ')
					{
						stringBuilder.Length--;
						stringBuilder.Append("=20");
					}
					stringBuilder.Append('\r');
					stringBuilder.Append('\n');
					for (int k = 0; k < stringBuilder.Length; k++)
					{
						stream.WriteByte((byte)stringBuilder[k]);
					}
					stringBuilder.Length = 0;
				}
				else
				{
					stringBuilder.AppendFormat("=" + num.ToString("X2"));
				}
			}
			memoryStream2.Close();
			stream.Flush();
			for (int l = 0; l < stiMhtImageHost.ImageCache.ImagePackedStore.Count; l++)
			{
				ImageFormat val2 = stiMhtImageHost.ImageCache.ImageFormatStore[l];
				string arg2 = ((val2 == ImageFormat.Jpeg) ? "jpeg" : "gif");
				if (val2 == ImageFormat.Png)
				{
					arg2 = "png";
				}
				string text4 = "file:///" + text + text2 + Path.GetFileNameWithoutExtension(fileName) + (l + 1) + "." + ((object)val2).ToString();
				text4 = StiExportUtils.StringToUrl(text4.Replace("\\", "/").Replace(" ", "_"));
				streamWriter2.WriteLine("");
				streamWriter2.WriteLine("--{0}", arg);
				streamWriter2.WriteLine("Content-Type: image/{0}", arg2);
				streamWriter2.WriteLine("Content-Transfer-Encoding: base64");
				streamWriter2.WriteLine("Content-Location: {0}", text4);
				streamWriter2.WriteLine("");
				streamWriter2.Flush();
				byte[] array = (byte[])stiMhtImageHost.ImageCache.ImagePackedStore[l];
				string text5 = Convert.ToBase64String(array, 0, array.Length);
				int num2 = 0;
				for (int m = 0; m < text5.Length; m++)
				{
					stream.WriteByte((byte)text5[m]);
					num2++;
					if (num2 == 76)
					{
						stream.WriteByte(13);
						stream.WriteByte(10);
						num2 = 0;
					}
				}
				stream.WriteByte(13);
				stream.WriteByte(10);
				stream.Flush();
			}
			stiMhtImageHost.ImageCache.Clear();
			streamWriter2.WriteLine("");
			streamWriter2.WriteLine("--{0}--", arg);
			streamWriter2.Flush();
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
		if (settings.CompressToArchive)
		{
			val.AddFile(fileName + ".mht", memoryStream);
			val.End();
			val = null;
		}
	}
}
