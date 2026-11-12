using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Stimulsoft.Base.Drawing;

public sealed class StiMetafileSaver
{
	public static void Save(Stream stream, Metafile metafile)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		Metafile val4;
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				IntPtr hdc = val2.GetHdc();
				GraphicsUnit val3 = (GraphicsUnit)2;
				val4 = new Metafile(stream, hdc, ((Image)metafile).GetBounds(ref val3), (MetafileFrameUnit)2);
				val2.ReleaseHdc(hdc);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Graphics val5 = Graphics.FromImage((Image)(object)val4);
		try
		{
			val5.DrawImage((Image)(object)metafile, metafile.GetMetafileHeader().Bounds.Left, metafile.GetMetafileHeader().Bounds.Top, ((Image)metafile).Width, ((Image)metafile).Height);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}

	public static void Save(string fileName, Metafile metafile)
	{
		StiFileUtils.ProcessReadOnly(fileName);
		FileStream fileStream = new FileStream(fileName, FileMode.Create);
		Save(fileStream, metafile);
		fileStream.Flush();
		fileStream.Close();
	}
}
