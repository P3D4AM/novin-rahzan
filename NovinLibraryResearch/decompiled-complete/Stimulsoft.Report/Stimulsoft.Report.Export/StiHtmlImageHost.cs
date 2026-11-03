using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Stimulsoft.Report.Export;

public class StiHtmlImageHost
{
	protected StiHtmlExportService HtmlExport { get; set; }

	public StiImageCache ImageCache { get; set; }

	internal bool IsMhtExport { get; set; }

	internal bool ForcePng { get; set; }

	public virtual string GetImageString(Bitmap bmp)
	{
		ImageFormat val = (ForcePng ? ImageFormat.Png : HtmlExport.imageFormat);
		int count = ImageCache.ImagePackedStore.Count;
		int num = ImageCache.AddImageInt((Image)(object)bmp, val);
		if (val == null)
		{
			val = ImageCache.ImageFormatStore[num];
		}
		if (HtmlExport.zip != null && !HtmlExport.useEmbeddedImages)
		{
			string fileName = HtmlExport.fileName;
			string text = $"{fileName}.files/{fileName}{num + 1}.{((object)val).ToString()}";
			if (ImageCache.ImagePackedStore.Count <= count)
			{
				return text;
			}
			byte[] buffer = (byte[])ImageCache.ImagePackedStore[num];
			MemoryStream memoryStream = new MemoryStream(buffer);
			HtmlExport.zip.AddFile(text, memoryStream);
			return text;
		}
		string text2 = null;
		if (HtmlExport.isFileStreamMode)
		{
			try
			{
				text2 = Path.GetDirectoryName(HtmlExport.fileName).Replace("\\", "/");
			}
			catch
			{
				HtmlExport.isFileStreamMode = false;
			}
		}
		if (HtmlExport.isFileStreamMode)
		{
			if (text2.Length > 0 && text2[text2.Length - 1] != '/')
			{
				text2 += "/";
			}
			string text3 = Path.GetFileNameWithoutExtension(HtmlExport.fileName) + ".files/";
			if (!Directory.Exists(text2 + text3))
			{
				Directory.CreateDirectory(text2 + text3);
			}
			string text4 = Path.GetFileNameWithoutExtension(HtmlExport.fileName) + (num + 1) + "." + ((object)val).ToString();
			if (ImageCache.ImagePackedStore.Count > count)
			{
				try
				{
					using FileStream fileStream = File.OpenWrite(text2 + text3 + text4);
					byte[] array = (byte[])ImageCache.ImagePackedStore[num];
					fileStream.Write(array, 0, array.Length);
				}
				catch
				{
					HtmlExport.isFileStreamMode = false;
				}
			}
			if (HtmlExport.isFileStreamMode)
			{
				return text3 + text4;
			}
		}
		byte[] inArray = (byte[])ImageCache.ImagePackedStore[num];
		return "data:image/" + ((object)val).ToString().ToLower() + ";base64," + Convert.ToBase64String(inArray);
	}

	public StiHtmlImageHost(StiHtmlExportService htmlExport)
	{
		HtmlExport = htmlExport;
		IsMhtExport = false;
		ForcePng = false;
	}
}
