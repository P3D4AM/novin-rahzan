using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Stimulsoft.Report.Export;

public class StiMhtImageHost : StiHtmlImageHost
{
	public override string GetImageString(Bitmap bmp)
	{
		int num = base.ImageCache.AddImageInt((Image)(object)bmp, base.ForcePng ? ImageFormat.Png : base.HtmlExport.imageFormat);
		ImageFormat val = base.ImageCache.ImageFormatStore[num];
		string text = string.Empty;
		if (!string.IsNullOrEmpty(base.HtmlExport.fileName))
		{
			text = Path.GetDirectoryName(base.HtmlExport.fileName);
		}
		if (text.Length > 0 && text[text.Length - 1] != '\\')
		{
			text += "\\";
		}
		string text2 = Path.GetFileNameWithoutExtension(base.HtmlExport.fileName) + ".files\\";
		string text3 = Path.GetFileNameWithoutExtension(base.HtmlExport.fileName) + (num + 1) + "." + ((object)val).ToString();
		string text4 = "file:///" + text + text2 + text3;
		return StiExportUtils.StringToUrl(text4.Replace("\\", "/").Replace(" ", "_"));
	}

	public StiMhtImageHost(StiHtmlExportService htmlExport)
		: base(htmlExport)
	{
		base.IsMhtExport = true;
		base.ForcePng = false;
	}
}
