using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Stimulsoft.Base.Drawing;

public sealed class StiMetafileConverter
{
	public static string MetafileToString(Metafile metafile)
	{
		byte[] inArray = MetafileToBytes(metafile);
		return Convert.ToBase64String(inArray, StiBaseOptions.AllowInsertLineBreaksWhenSavingByteArray ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None);
	}

	public static byte[] MetafileToBytes(Metafile metafile)
	{
		using MemoryStream memoryStream = new MemoryStream();
		StiMetafileSaver.Save(memoryStream, metafile);
		return memoryStream.ToArray();
	}

	public static Metafile BytesToMetafile(byte[] bytes)
	{
		MemoryStream memoryStream = new MemoryStream(bytes);
		Image obj = Image.FromStream((Stream)memoryStream);
		return (Metafile)(object)((obj is Metafile) ? obj : null);
	}

	public static Metafile StringToMetafile(string str)
	{
		byte[] bytes = Convert.FromBase64String(str);
		return BytesToMetafile(bytes);
	}

	public static byte[] MetafileToBitmapBytes(byte[] metafileBytes, int bitmapWidth, int bitmapHeight)
	{
		if (metafileBytes == null || metafileBytes.Length == 0)
		{
			return null;
		}
		Metafile val = BytesToMetafile(metafileBytes);
		try
		{
			Bitmap val2 = MetafileToBitmap(val, bitmapWidth, bitmapHeight);
			try
			{
				return StiImageConverter.ImageToBytes((Image)(object)val2, allowNulls: true);
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
	}

	public static Bitmap MetafileToBitmap(byte[] metafileBytes, int bitmapWidth, int bitmapHeight)
	{
		if (metafileBytes == null || metafileBytes.Length == 0)
		{
			return null;
		}
		Metafile val = BytesToMetafile(metafileBytes);
		try
		{
			return MetafileToBitmap(val, bitmapWidth, bitmapHeight);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static Bitmap MetafileToBitmap(Metafile metafile, int bitmapWidth, int bitmapHeight)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(bitmapWidth, bitmapHeight);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.Clear(Color.White);
			val2.SmoothingMode = (SmoothingMode)4;
			val2.DrawImage((Image)(object)metafile, 0, 0, bitmapWidth, bitmapHeight);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static byte[] MetafileToPngBytes(byte[] metafileBytes, int bitmapWidth, int bitmapHeight)
	{
		if (metafileBytes == null || metafileBytes.Length == 0)
		{
			return null;
		}
		Metafile val = BytesToMetafile(metafileBytes);
		try
		{
			return MetafileToPngBytes(val, bitmapWidth, bitmapHeight);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static byte[] MetafileToPngBytes(Metafile metafile, int bitmapWidth, int bitmapHeight)
	{
		Bitmap val = MetafileToBitmap(metafile, bitmapWidth, bitmapHeight);
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			((Image)val).Save((Stream)memoryStream, ImageFormat.Png);
			return memoryStream.ToArray();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
