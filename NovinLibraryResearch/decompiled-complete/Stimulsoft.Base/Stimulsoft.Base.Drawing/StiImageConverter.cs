using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Base.Drawing;

public sealed class StiImageConverter
{
	public static string ImageToPackedString(Image image)
	{
		if (image == null)
		{
			return string.Empty;
		}
		byte[] inArray = ImageToBytes(image);
		return StiGZipHelper.Pack(Convert.ToBase64String(inArray, StiBaseOptions.AllowInsertLineBreaksWhenSavingByteArray ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None));
	}

	public static Image PackedStringToImage(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		byte[] bytes = StringToByteArray(StiGZipHelper.Unpack(str));
		return BytesToImage(bytes);
	}

	public static string ImageToString(Image image, bool allowNulls = false)
	{
		byte[] array = ImageToBytes(image, allowNulls);
		if (array == null)
		{
			return null;
		}
		return Convert.ToBase64String(array, StiBaseOptions.AllowInsertLineBreaksWhenSavingByteArray ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None);
	}

	public static byte[] ImageToBytes(Image image, bool allowNulls = false)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		if (image == null)
		{
			if (!allowNulls)
			{
				return new byte[0];
			}
			return null;
		}
		if (image is Metafile)
		{
			return StiMetafileConverter.MetafileToBytes((Metafile)(object)((image is Metafile) ? image : null));
		}
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(Image));
		try
		{
			return (byte[])converter.ConvertTo(image, typeof(byte[]));
		}
		catch
		{
			try
			{
				Bitmap val = new Bitmap(image.Width, image.Height, image.PixelFormat);
				try
				{
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						val2.DrawImage(image, 0, 0, image.Width, image.Height);
						return (byte[])converter.ConvertTo(val, typeof(byte[]));
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
			catch
			{
			}
		}
		if (!allowNulls)
		{
			return new byte[0];
		}
		return null;
	}

	public static Image TryBytesToImage(byte[] bytes, int width = 200, int height = 200, bool stretch = true, bool aspectRatio = false)
	{
		try
		{
			return BytesToImage(bytes, width, height, stretch, aspectRatio);
		}
		catch
		{
			return null;
		}
	}

	public static Image BytesToImage(byte[] bytes, int width = 200, int height = 200, bool stretch = true, bool aspectRatio = false)
	{
		if (bytes == null || bytes.Length == 0)
		{
			return null;
		}
		if (StiSvgHelper.IsSvg(bytes))
		{
			return StiSvgHelper.ConvertSvgToImage(bytes, width, height, stretch, aspectRatio);
		}
		try
		{
			MemoryStream memoryStream = new MemoryStream(bytes);
			return Image.FromStream((Stream)memoryStream);
		}
		catch
		{
			return null;
		}
	}

	public static Image StringToImage(string str)
	{
		byte[] array = StringToByteArray(str);
		if (array == null)
		{
			return null;
		}
		return BytesToImage(array);
	}

	public static byte[] StringToByteArray(string str)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}
		if (str.StartsWith("data:image"))
		{
			int num = str.LastIndexOf(',');
			if (num > 0)
			{
				str = str.Substring(num + 1);
			}
		}
		if (!str.IsBase64String())
		{
			return null;
		}
		return Convert.FromBase64String(str);
	}
}
