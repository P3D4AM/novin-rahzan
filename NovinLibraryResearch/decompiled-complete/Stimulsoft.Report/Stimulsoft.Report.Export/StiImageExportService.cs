using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Zip;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Export;

[StiServiceBitmap(typeof(StiExportService), "Stimulsoft.Report.Images.Exports.Bmp.png")]
public class StiImageExportService : StiExportService
{
	internal StiImageExportSettings imageSettings;

	private StiReport report;

	private string fileName;

	private bool sendEMail;

	private StiGuiMode guiMode;

	internal bool compressToArchive;

	public override string DefaultExtension
	{
		get
		{
			if (!compressToArchive)
			{
				return GetExtensionFromSettings();
			}
			return "zip";
		}
	}

	public override StiExportFormat ExportFormat
	{
		get
		{
			if (imageSettings != null && imageSettings.GetType() == typeof(StiImageExportSettings))
			{
				return StiExportFormat.Image;
			}
			if (imageSettings is StiBmpExportSettings || imageSettings.ImageType == StiImageType.Bmp)
			{
				return StiExportFormat.ImageBmp;
			}
			if (imageSettings is StiEmfExportSettings || imageSettings.ImageType == StiImageType.Emf)
			{
				return StiExportFormat.ImageEmf;
			}
			if (imageSettings is StiGifExportSettings || imageSettings.ImageType == StiImageType.Gif)
			{
				return StiExportFormat.ImageGif;
			}
			if (imageSettings is StiJpegExportSettings || imageSettings.ImageType == StiImageType.Jpeg)
			{
				return StiExportFormat.ImageJpeg;
			}
			if (imageSettings is StiPcxExportSettings || imageSettings.ImageType == StiImageType.Pcx)
			{
				return StiExportFormat.ImagePcx;
			}
			if (imageSettings is StiPngExportSettings || imageSettings.ImageType == StiImageType.Png)
			{
				return StiExportFormat.ImagePng;
			}
			if (imageSettings is StiSvgExportSettings || imageSettings.ImageType == StiImageType.Svg)
			{
				return StiExportFormat.ImageSvg;
			}
			if (imageSettings is StiSvgzExportSettings || imageSettings.ImageType == StiImageType.Svgz)
			{
				return StiExportFormat.ImageSvgz;
			}
			if (imageSettings is StiTiffExportSettings || imageSettings.ImageType == StiImageType.Tiff)
			{
				return StiExportFormat.ImageTiff;
			}
			return StiExportFormat.Image;
		}
	}

	public override string GroupCategory
	{
		get
		{
			if (this is StiEmfExportService || this is StiSvgExportService || this is StiSvgzExportService)
			{
				return "Meta";
			}
			return "Image";
		}
	}

	public override string ExportNameInMenu => StiLocalization.Get("Export", "ExportTypeImageFile");

	public override int Position => 50;

	private string GetExtensionFromSettings()
	{
		if (imageSettings is StiBmpExportSettings || imageSettings.ImageType == StiImageType.Bmp)
		{
			return "bmp";
		}
		if (imageSettings is StiEmfExportSettings || imageSettings.ImageType == StiImageType.Emf)
		{
			return "emf";
		}
		if (imageSettings is StiGifExportSettings || imageSettings.ImageType == StiImageType.Gif)
		{
			return "gif";
		}
		if (imageSettings is StiJpegExportSettings || imageSettings.ImageType == StiImageType.Jpeg)
		{
			return "jpg";
		}
		if (imageSettings is StiPcxExportSettings || imageSettings.ImageType == StiImageType.Pcx)
		{
			return "pcx";
		}
		if (imageSettings is StiPngExportSettings || imageSettings.ImageType == StiImageType.Png)
		{
			return "png";
		}
		if (imageSettings is StiSvgExportSettings || imageSettings.ImageType == StiImageType.Svg)
		{
			return "svg";
		}
		if (imageSettings is StiSvgzExportSettings || imageSettings.ImageType == StiImageType.Svgz)
		{
			return "svgz";
		}
		if (imageSettings is StiTiffExportSettings || imageSettings.ImageType == StiImageType.Tiff)
		{
			return "tiff";
		}
		return "jpg";
	}

	public override string GetFilter()
	{
		if (compressToArchive)
		{
			return StiLocalization.Get("FileFilters", "ZipArchives");
		}
		if (imageSettings is StiBmpExportSettings || imageSettings.ImageType == StiImageType.Bmp)
		{
			return StiLocalization.Get("FileFilters", "BmpFiles");
		}
		if (imageSettings is StiEmfExportSettings || imageSettings.ImageType == StiImageType.Emf)
		{
			return StiLocalization.Get("FileFilters", "EmfFiles");
		}
		if (imageSettings is StiGifExportSettings || imageSettings.ImageType == StiImageType.Gif)
		{
			return StiLocalization.Get("FileFilters", "GifFiles");
		}
		if (imageSettings is StiJpegExportSettings || imageSettings.ImageType == StiImageType.Jpeg)
		{
			return StiLocalization.Get("FileFilters", "JpegFiles");
		}
		if (imageSettings is StiPcxExportSettings || imageSettings.ImageType == StiImageType.Pcx)
		{
			return StiLocalization.Get("FileFilters", "PcxFiles");
		}
		if (imageSettings is StiPngExportSettings || imageSettings.ImageType == StiImageType.Png)
		{
			return StiLocalization.Get("FileFilters", "PngFiles");
		}
		if (imageSettings is StiSvgExportSettings || imageSettings.ImageType == StiImageType.Svg)
		{
			return StiLocalization.Get("FileFilters", "SvgFiles");
		}
		if (imageSettings is StiSvgzExportSettings || imageSettings.ImageType == StiImageType.Svgz)
		{
			return StiLocalization.Get("FileFilters", "SvgzFiles");
		}
		if (imageSettings is StiTiffExportSettings || imageSettings.ImageType == StiImageType.Tiff)
		{
			return StiLocalization.Get("FileFilters", "TiffFiles");
		}
		return StiLocalization.Get("FileFilters", "JpegFiles");
	}

	public override void ExportTo(StiReport report, Stream stream, StiExportSettings settings)
	{
		ExportImage(report, stream, settings as StiImageExportSettings);
		InvokeExporting(100, 100, 1, 1);
	}

	public override void Export(StiReport report, string fileName, bool sendEMail, StiGuiMode guiMode)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		IStiFormRunner exportFormRunner = StiGuiOptions.GetExportFormRunner("StiImageSetupForm", guiMode, base.OwnerWindow);
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

	private void Form_Complete(IStiFormRunner form, StiShowDialogCompleteEvetArgs e)
	{
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		if (e.DialogResult)
		{
			MultipleFiles = (bool)form["MultipleFiles"];
			compressToArchive = (bool)form["CompressToArchive"];
			imageSettings = new StiImageExportSettings
			{
				ImageType = (StiImageType)form["ImageType"],
				ImageZoom = (double)form["ImageZoom"],
				ImageResolution = (int)form["ImageResolution"],
				CutEdges = (bool)form["CutEdges"],
				ImageFormat = (StiImageFormat)form["ImageFormat"],
				PageRange = (form["PagesRange"] as StiPagesRange),
				MultipleFiles = (bool)form["MultipleFiles"],
				TiffCompressionScheme = (StiTiffCompressionScheme)form["TiffCompressionScheme"],
				DitheringType = (StiMonochromeDitheringType)form["MonochromeDitheringType"],
				CompressToArchive = (bool)form["CompressToArchive"]
			};
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = GetFileName(report, sendEMail);
			}
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}
			StiFileUtils.ProcessReadOnly(fileName);
			try
			{
				using FileStream stream = new FileStream(fileName, FileMode.Create);
				StartProgress(guiMode);
				StartExport(report, stream, imageSettings, sendEMail, (bool)form["OpenAfterExport"], fileName, guiMode);
				return;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		base.IsStopped = true;
	}

	private Image Draw(StiPage page, double imageZoomBase, int imageResolution, bool cutEdges, StiImageFormat imageFormat, Stream stream, bool emf)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		double num = imageZoomBase * (double)((float)imageResolution / 100f);
		double zoom = page.Report.Info.Zoom;
		page.Report.Info.Zoom = num;
		StiUnit unit = page.Unit;
		StiScale.Lock();
		RectangleD displayRectangle = page.DisplayRectangle;
		double num2 = unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width) * num;
		displayRectangle = page.DisplayRectangle;
		double num3 = unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height) * num;
		int num4 = (int)num2;
		int num5 = (int)num3;
		if ((double)num4 < num2)
		{
			num4++;
		}
		if ((double)num5 < num3)
		{
			num5++;
		}
		double left = page.Margins.Left;
		double right = page.Margins.Right;
		double top = page.Margins.Top;
		double bottom = page.Margins.Bottom;
		if (cutEdges)
		{
			double num6 = ((StiOptions.Export.ImageCutEdgesMode == StiImageCutEdgesMode.Full) ? 1.0 : 0.8);
			displayRectangle = page.DisplayRectangle;
			num4 = (int)(unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width - left * num6 - right * num6) * num);
			displayRectangle = page.DisplayRectangle;
			num5 = (int)(unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height - top * num6 - bottom * num6) * num);
		}
		Image val = (Image)((emf && StiOptions.Engine.FullTrust) ? ((object)CreateMetafileInFullTrust(stream)) : ((object)new Bitmap(num4, num5)));
		if (page.Report.RenderedWith == StiRenderedWith.Wpf)
		{
			try
			{
				page.DenyDrawSegmentMode = true;
				StiPainter painter = StiPainter.GetPainter(typeof(StiPage), (StiGuiMode)1);
				float zoom2 = (float)num;
				Image image = painter.GetImage(page, ref zoom2, StiExportFormat.ImagePng);
				Graphics val2 = Graphics.FromImage(val);
				try
				{
					if (cutEdges)
					{
						double num7 = ((StiOptions.Export.ImageCutEdgesMode == StiImageCutEdgesMode.Full) ? 1.0 : 0.8);
						int x = (int)(unit.ConvertToHInches(left * num7) * num);
						int y = (int)(unit.ConvertToHInches(top * num7) * num);
						val2.DrawImage(image, new Rectangle(0, 0, num4, num5), new Rectangle(x, y, num4, num5), (GraphicsUnit)2);
					}
					else
					{
						val2.DrawImage(image, new Rectangle(0, 0, num4, num5));
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				page.DenyDrawSegmentMode = false;
			}
		}
		else
		{
			Graphics val3 = Graphics.FromImage(val);
			try
			{
				if (cutEdges)
				{
					double num8 = ((StiOptions.Export.ImageCutEdgesMode == StiImageCutEdgesMode.Full) ? 1.0 : 0.8);
					val3.TranslateTransform((float)(unit.ConvertToHInches((0.0 - left) * num8) * num), (float)(unit.ConvertToHInches((0.0 - top) * num8) * num));
				}
				try
				{
					page.DenyDrawSegmentMode = true;
					StiPageGdiPainter stiPageGdiPainter = StiPainter.GetPainter(typeof(StiPage), (StiGuiMode)0) as StiPageGdiPainter;
					stiPageGdiPainter.Paint(page, new StiPaintEventArgs(val3, RectangleD.Empty));
				}
				finally
				{
					page.DenyDrawSegmentMode = false;
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		page.Report.Info.Zoom = zoom;
		StiScale.Unlock();
		if (imageFormat != StiImageFormat.Color)
		{
			Bitmap val4 = new Bitmap(val.Width, val.Height);
			Graphics val5 = Graphics.FromImage((Image)(object)val4);
			try
			{
				ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
				{
					new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
					new float[5] { 0.55f, 0.55f, 0.55f, 0f, 0f },
					new float[5] { 0.15f, 0.15f, 0.15f, 0f, 0f },
					new float[6] { 0f, 0f, 0f, 1f, 0f, 0f },
					new float[6] { 0f, 0f, 0f, 0f, 1f, 0f },
					new float[6] { 0f, 0f, 0f, 0f, 0f, 1f }
				});
				ImageAttributes val6 = new ImageAttributes();
				val6.SetColorMatrix(colorMatrix);
				val5.DrawImage(val, new Rectangle(0, 0, val.Width, val.Height), 0, 0, val.Width, val.Height, (GraphicsUnit)2, val6);
				val = (Image)(object)val4;
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		return val;
	}

	private Image CreateMetafileInFullTrust(Stream stream)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		Image val = null;
		Bitmap val2 = new Bitmap(1, 1);
		try
		{
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			try
			{
				IntPtr hdc = val3.GetHdc();
				val = (Image)new Metafile(stream, hdc);
				val3.ReleaseHdc(hdc);
				return val;
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public void ExportImage(StiReport report, string fileName, StiImageFormat imageFormat)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		using FileStream fileStream = new FileStream(fileName, FileMode.Create);
		ExportImage(report, fileStream, imageFormat);
		fileStream.Flush();
		fileStream.Close();
	}

	public virtual void ExportImage(StiReport report, Stream stream, StiImageFormat imageFormat)
	{
		ExportImage(report, stream, imageFormat, StiPagesRange.All);
	}

	public virtual void ExportImage(StiReport report, Stream stream, StiImageFormat imageFormat, StiPagesRange pageRange)
	{
		ExportImage(report, stream, 1.0, cutEdges: false, imageFormat, pageRange);
	}

	public virtual void ExportImage(StiReport report, Stream stream, double imageZoom, bool cutEdges, StiImageFormat imageFormat, StiPagesRange pageRange)
	{
		if (this is StiEmfExportService)
		{
			StiEmfExportSettings settings = new StiEmfExportSettings
			{
				PageRange = pageRange
			};
			ExportImage(report, stream, settings);
			return;
		}
		StiImageExportSettings settings2 = new StiImageExportSettings
		{
			PageRange = pageRange,
			ImageZoom = imageZoom,
			CutEdges = cutEdges,
			ImageFormat = imageFormat
		};
		ExportImage(report, stream, settings2);
	}

	public virtual void ExportImage(StiReport report, Stream stream, StiImageExportSettings settings)
	{
		if (settings == null)
		{
			settings = GetSettings();
		}
		if (!report.IsDocument && report.GetCurrentPage() is IStiDashboard)
		{
			StiDashboardExport.Export(report, stream, settings);
			return;
		}
		try
		{
			ExportImage1(report, stream, settings);
		}
		finally
		{
			StiExportUtils.EnableFontSmoothing(report);
		}
	}

	private StiImageExportSettings GetSettings()
	{
		if (this is StiBmpExportService)
		{
			return new StiImageExportSettings(StiImageType.Bmp);
		}
		if (this is StiEmfExportService)
		{
			return new StiImageExportSettings(StiImageType.Emf);
		}
		if (this is StiGifExportService)
		{
			return new StiImageExportSettings(StiImageType.Gif);
		}
		if (this is StiJpegExportService)
		{
			return new StiImageExportSettings(StiImageType.Jpeg);
		}
		if (this is StiPcxExportService)
		{
			return new StiImageExportSettings(StiImageType.Pcx);
		}
		if (this is StiPngExportService)
		{
			return new StiImageExportSettings(StiImageType.Png);
		}
		if (this is StiSvgExportService)
		{
			return new StiImageExportSettings(StiImageType.Svg);
		}
		if (this is StiSvgzExportService)
		{
			return new StiImageExportSettings(StiImageType.Svgz);
		}
		if (this is StiTiffExportService)
		{
			return new StiImageExportSettings(StiImageType.Tiff);
		}
		return new StiImageExportSettings(StiImageType.Jpeg);
	}

	private void ExportImage1(StiReport report, Stream baseStream, StiImageExportSettings settings)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		StiLogService.Write(((object)this).GetType(), $"Export report to {settings.ImageType} format ");
		if (settings == null)
		{
			throw new ArgumentNullException("The 'settings' argument cannot be equal in null.");
		}
		imageSettings = settings;
		StiPagesRange pageRange = settings.PageRange;
		double imageZoom = settings.ImageZoom;
		int imageResolution = settings.ImageResolution;
		bool cutEdges = settings.CutEdges;
		StiImageFormat imageFormat = settings.ImageFormat;
		StiMonochromeDitheringType ditheringType = settings.DitheringType;
		StiTiffCompressionScheme tiffCompressionScheme = settings.TiffCompressionScheme;
		MultipleFiles = settings.MultipleFiles;
		compressToArchive = settings.CompressToArchive;
		StiPagesCollection selectedPages = pageRange.GetSelectedPages(report.RenderedPages);
		Image val = null;
		Encoder saveFlag = Encoder.SaveFlag;
		EncoderParameters val2 = new EncoderParameters(1);
		ImageCodecInfo imageCodec = StiImageCodecInfo.GetImageCodec("image/tiff");
		if (tiffCompressionScheme != StiTiffCompressionScheme.Default)
		{
			if (MultipleFiles)
			{
				val2.Param[0] = new EncoderParameter(Encoder.Compression, (long)tiffCompressionScheme);
			}
			else
			{
				val2 = new EncoderParameters(2);
				val2.Param[1] = new EncoderParameter(Encoder.Compression, (long)tiffCompressionScheme);
			}
		}
		Stream stream = baseStream;
		StiZipWriter20 val3 = null;
		string text = string.Empty;
		if (compressToArchive)
		{
			if (stream is FileStream fileStream)
			{
				text = fileStream.Name;
			}
			try
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					text = Path.GetFileNameWithoutExtension(text);
				}
			}
			catch
			{
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = report.ReportName;
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "report";
			}
			if (selectedPages.Count > 1)
			{
				string orderFileName = StiExportService.GetOrderFileName(null, 1, report.RenderedPages.Count, null);
				if (text.EndsWith(orderFileName))
				{
					text = text.Substring(0, text.Length - orderFileName.Length);
				}
			}
			val3 = new StiZipWriter20();
			val3.Begin(baseStream, true);
			stream = new MemoryStream();
		}
		else if (baseStream is FileStream && selectedPages.Count > 1)
		{
			FileStream fileStream2 = baseStream as FileStream;
			text = Path.ChangeExtension(fileStream2.Name, "");
			text = text.Substring(0, text.Length - 1);
			string orderFileName2 = StiExportService.GetOrderFileName(null, 1, report.RenderedPages.Count, null);
			if (text.EndsWith(orderFileName2))
			{
				text = text.Substring(0, text.Length - orderFileName2.Length);
			}
		}
		base.StatusString = StiLocalization.Get("Export", "ExportingCreatingDocument");
		int num = 1;
		StiImageType stiImageType = settings.ImageType;
		if (stiImageType == StiImageType.Emf && !StiOptions.Engine.FullTrust)
		{
			stiImageType = StiImageType.Png;
		}
		foreach (StiPage item in selectedPages)
		{
			selectedPages.GetPage(item);
			InvokeExporting(item, selectedPages, 0, 1);
			if (base.IsStopped)
			{
				return;
			}
			Image val4 = null;
			if (settings.ImageType != StiImageType.Svg && settings.ImageType != StiImageType.Svgz)
			{
				val4 = Draw(item, imageZoom, imageResolution, cutEdges, imageFormat, stream, stiImageType == StiImageType.Emf);
				if (tiffCompressionScheme == StiTiffCompressionScheme.CCITT3 || tiffCompressionScheme == StiTiffCompressionScheme.CCITT4 || tiffCompressionScheme == StiTiffCompressionScheme.Rle || imageFormat == StiImageFormat.Monochrome)
				{
					val4 = StiTiffHelper.MakeMonochromeImage(val4, ditheringType, 128);
				}
				if (stiImageType != StiImageType.Emf)
				{
					((Bitmap)((val4 is Bitmap) ? val4 : null)).SetResolution((float)imageResolution, (float)imageResolution);
				}
			}
			if (stiImageType == StiImageType.Tiff && !MultipleFiles)
			{
				if (num == 1)
				{
					val = val4;
					val2.Param[0] = new EncoderParameter(saveFlag, 18L);
					val.Save(stream, imageCodec, val2);
				}
				else
				{
					val2.Param[0] = new EncoderParameter(saveFlag, 23L);
					val.SaveAdd(val4, val2);
				}
				if (num == selectedPages.Count)
				{
					val2.Param[0] = new EncoderParameter(saveFlag, 20L);
					val.SaveAdd(val2);
					if (compressToArchive)
					{
						stream.Flush();
						val3.AddFile(text + "." + GetExtensionFromSettings(), stream as MemoryStream);
						stream.Close();
					}
				}
				if (val4 != null && num != 1)
				{
					val4.Dispose();
				}
				num++;
				continue;
			}
			if (stiImageType != StiImageType.Emf)
			{
				if (stiImageType == StiImageType.Pcx)
				{
					StiPcxPaletteType paletteType = StiPcxPaletteType.Color;
					if (imageFormat == StiImageFormat.Monochrome)
					{
						paletteType = StiPcxPaletteType.Monochrome;
					}
					StiPcxHelper.SaveToStream(val4, paletteType, ditheringType, stream);
				}
				else if (stiImageType == StiImageType.Bmp && imageFormat == StiImageFormat.Monochrome)
				{
					StiBmpHelper.SaveToStreamMonochrome(val4, ditheringType, stream);
				}
				else if (stiImageType == StiImageType.Svg || stiImageType == StiImageType.Svgz)
				{
					StiSvgHelper.SaveToStream(report, item, stream, stiImageType == StiImageType.Svgz);
				}
				else if (stiImageType == StiImageType.Tiff && tiffCompressionScheme != StiTiffCompressionScheme.Default)
				{
					val4.Save(stream, imageCodec, val2);
				}
				else
				{
					ImageFormat val5 = ImageFormat.Bmp;
					switch (stiImageType)
					{
					case StiImageType.Bmp:
						val5 = ImageFormat.Bmp;
						break;
					case StiImageType.Emf:
						val5 = ImageFormat.Emf;
						break;
					case StiImageType.Gif:
						val5 = ImageFormat.Gif;
						break;
					case StiImageType.Jpeg:
						val5 = ImageFormat.Jpeg;
						break;
					case StiImageType.Png:
						val5 = ImageFormat.Png;
						break;
					case StiImageType.Tiff:
						val5 = ImageFormat.Tiff;
						break;
					}
					val4.Save(stream, val5);
				}
			}
			if (compressToArchive)
			{
				stream.Flush();
				string orderFileName3 = StiExportService.GetOrderFileName(text, num, report.RenderedPages.Count, GetExtensionFromSettings());
				val3.AddFile(orderFileName3, stream as MemoryStream);
				stream.Close();
				if (num < selectedPages.Count)
				{
					stream = new MemoryStream();
				}
				num++;
			}
			else if (stream is FileStream && selectedPages.Count > 1)
			{
				if (num > 1)
				{
					stream.Flush();
					stream.Close();
				}
				if (num < selectedPages.Count)
				{
					string orderFileName4 = StiExportService.GetOrderFileName(text, num + 1, report.RenderedPages.Count, GetExtensionFromSettings());
					StiFileUtils.ProcessReadOnly(orderFileName4);
					stream = new FileStream(orderFileName4, FileMode.Create);
				}
				num++;
			}
			if (val4 != null)
			{
				val4.Dispose();
			}
		}
		if (compressToArchive)
		{
			val3.End();
			val3 = null;
		}
		if (val != null)
		{
			val.Dispose();
		}
	}

	public StiImageExportService()
	{
		MultipleFiles = true;
	}
}
