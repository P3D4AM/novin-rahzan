using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class StiImageUtils2
{
	public static Bitmap GetImage(Type type, string imageName)
	{
		return GetImage(type, imageName, makeTransparent: true);
	}

	public static byte[] GetByteArray(Type type, string imageName)
	{
		return GetByteArray(type, imageName, makeTransparent: true);
	}

	public static Bitmap GetImage(Type type, string imageName, bool makeTransparent)
	{
		return GetImage(type.Module.Assembly, imageName, makeTransparent);
	}

	public static byte[] GetByteArray(Type type, string imageName, bool makeTransparent)
	{
		return GetByteArray(type.Module.Assembly, imageName, makeTransparent);
	}

	public static Bitmap GetImage(string assemblyName, string imageName)
	{
		return GetImage(assemblyName, imageName, makeTransparent: true);
	}

	public static byte[] GetByteArray(string assemblyName, string imageName)
	{
		return GetByteArray(assemblyName, imageName, makeTransparent: true);
	}

	public static Bitmap GetImage(string assemblyName, string imageName, bool makeTransparent)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			string text = assembly.FullName;
			int num = text.IndexOf(',');
			if (num > -1)
			{
				text = text.Substring(0, num);
			}
			if (text == assemblyName)
			{
				return GetImage(assembly, imageName, makeTransparent);
			}
		}
		throw new Exception("Can't find assembly '" + assemblyName + "'");
	}

	public static byte[] GetByteArray(string assemblyName, string imageName, bool makeTransparent)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly[] array = assemblies;
		foreach (Assembly assembly in array)
		{
			string text = assembly.FullName;
			int num = text.IndexOf(',');
			if (num > -1)
			{
				text = text.Substring(0, num);
			}
			if (text == assemblyName)
			{
				return GetByteArray(assembly, imageName, makeTransparent);
			}
		}
		throw new Exception("Can't find assembly '" + assemblyName + "'");
	}

	public static Bitmap GetImage(Assembly imageAssembly, string imageName)
	{
		return GetImage(imageAssembly, imageName, makeTransparent: true);
	}

	public static Bitmap GetImage(Assembly imageAssembly, string imageName, bool makeTransparent)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		Stream manifestResourceStream = imageAssembly.GetManifestResourceStream(imageName);
		if (manifestResourceStream != null)
		{
			Bitmap val = new Bitmap(manifestResourceStream);
			if (makeTransparent)
			{
				MakeImageBackgroundAlphaZero(val);
			}
			return val;
		}
		throw new Exception($"Can't find image '{imageName}' in resources of '{imageAssembly}' assembly");
	}

	public static bool ExistsImage(Assembly imageAssembly, string imageName)
	{
		return imageAssembly.GetManifestResourceInfo(imageName) != null;
	}

	public static byte[] GetByteArray(Assembly imageAssembly, string imageName, bool makeTransparent)
	{
		Stream manifestResourceStream = imageAssembly.GetManifestResourceStream(imageName);
		if (manifestResourceStream != null)
		{
			byte[] array = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(array, 0, array.Length);
			return array;
		}
		throw new Exception($"Can't find image '{imageName}' in resources of '{imageAssembly}' assembly");
	}

	public static void MakeImageBackgroundAlphaZero(Bitmap image)
	{
		Color pixel = image.GetPixel(0, ((Image)image).Height - 1);
		image.MakeTransparent();
		Color color = Color.FromArgb(0, pixel);
		image.SetPixel(0, ((Image)image).Height - 1, color);
	}

	public static Bitmap ReplaceImageColor(Bitmap bmp, Color colorForReplace, Color replacedColor)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		Bitmap val = new Bitmap(((Image)bmp).Width, ((Image)bmp).Height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		ImageAttributes val3 = new ImageAttributes();
		val3.SetRemapTable((ColorMap[])(object)new ColorMap[1]
		{
			new ColorMap
			{
				OldColor = replacedColor,
				NewColor = colorForReplace
			}
		});
		val2.DrawImage((Image)(object)bmp, new Rectangle(0, 0, ((Image)bmp).Width, ((Image)bmp).Height), 0, 0, ((Image)bmp).Width, ((Image)bmp).Height, (GraphicsUnit)2, val3);
		val2.Dispose();
		return val;
	}

	public static Bitmap ConvertToDisabled(Bitmap bmp)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		Bitmap val = new Bitmap(((Image)bmp).Width, ((Image)bmp).Height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		ImageAttributes val3 = new ImageAttributes();
		ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
		{
			new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
			new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
			new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0.4f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0.4f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0f, 0.4f }
		});
		val3.SetColorMatrix(colorMatrix);
		val2.DrawImage((Image)(object)bmp, new Rectangle(0, 0, ((Image)bmp).Width, ((Image)bmp).Height), 0, 0, ((Image)bmp).Width, ((Image)bmp).Height, (GraphicsUnit)2, val3);
		val2.Dispose();
		return val;
	}

	public static Bitmap ConvertToGrayscale(Bitmap bmp)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		Bitmap val = new Bitmap(((Image)bmp).Width, ((Image)bmp).Height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
		{
			new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
			new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
			new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 1f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 1f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0f, 1f }
		});
		ImageAttributes val3 = new ImageAttributes();
		val3.SetColorMatrix(colorMatrix);
		val2.DrawImage((Image)(object)bmp, new Rectangle(0, 0, ((Image)bmp).Width, ((Image)bmp).Height), 0, 0, ((Image)bmp).Width, ((Image)bmp).Height, (GraphicsUnit)2, val3);
		val2.Dispose();
		return val;
	}

	public static Bitmap ResizeImage(Image image, int width, int height)
	{
		int imageWidth = width;
		int imageHeight = height;
		if (image.Width == 16 && image.Height == 16)
		{
			decimal num = (decimal)StiScale2.Factor;
			if (!(num == 1.25m))
			{
				if (!(num == 2.25m))
				{
					if (num == 2.5m)
					{
						imageWidth = 32;
						imageHeight = 32;
					}
				}
				else
				{
					imageWidth = 32;
					imageHeight = 32;
				}
			}
			else
			{
				imageWidth = 16;
				imageHeight = 16;
			}
		}
		if (image.Width == 32 && image.Height == 32)
		{
			decimal num2 = (decimal)StiScale2.Factor;
			if (!(num2 == 1.25m))
			{
				if (!(num2 == 2.25m))
				{
					if (num2 == 2.5m)
					{
						imageWidth = 64;
						imageHeight = 64;
					}
				}
				else
				{
					imageWidth = 64;
					imageHeight = 64;
				}
			}
			else
			{
				imageWidth = 32;
				imageHeight = 32;
			}
		}
		return ResizeImage(image, width, height, imageWidth, imageHeight);
	}

	public static Bitmap ResizeImage(Image image, int canvasWidth, int canvasHeight, int imageWidth, int imageHeight)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle((canvasWidth - imageWidth) / 2, (canvasHeight - imageHeight) / 2, imageWidth, imageHeight);
		Bitmap val = new Bitmap(canvasWidth, canvasHeight);
		val.SetResolution(image.HorizontalResolution, image.VerticalResolution);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.CompositingMode = (CompositingMode)1;
			val2.CompositingQuality = (CompositingQuality)2;
			val2.InterpolationMode = (InterpolationMode)5;
			val2.PixelOffsetMode = (PixelOffsetMode)2;
			ImageAttributes val3 = new ImageAttributes();
			try
			{
				val3.SetWrapMode((WrapMode)3);
				val2.DrawImage(image, rectangle, 0, 0, image.Width, image.Height, (GraphicsUnit)2, val3);
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

	public static Icon IconFromBytes(byte[] bytes)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		using MemoryStream memoryStream = new MemoryStream(bytes);
		return new Icon((Stream)memoryStream);
	}
}
