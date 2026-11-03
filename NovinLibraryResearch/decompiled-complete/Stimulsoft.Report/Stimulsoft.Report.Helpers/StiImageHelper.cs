using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Import;

namespace Stimulsoft.Report.Helpers;

public static class StiImageHelper
{
	private static long usedMemoryCounter;

	public static Image TryFromFile(string fileName, int width = 200, int height = 200)
	{
		try
		{
			return FromFile(fileName, width, height);
		}
		catch
		{
			return null;
		}
	}

	public static byte[] TryBytesFromFile(string fileName)
	{
		try
		{
			return BytesFromFile(fileName);
		}
		catch
		{
			return null;
		}
	}

	public static Image FromFile(string fileName, int width = 100, int height = 100)
	{
		if (!File.Exists(fileName))
		{
			return null;
		}
		byte[] bytes = File.ReadAllBytes(fileName);
		return FromBytes(bytes, width, height);
	}

	public static byte[] BytesFromFile(string fileName)
	{
		if (!File.Exists(fileName))
		{
			return null;
		}
		return File.ReadAllBytes(fileName);
	}

	public static Image TryFromBytes(byte[] bytes, int width, int height)
	{
		try
		{
			return FromBytes(bytes, width, height);
		}
		catch
		{
			return null;
		}
	}

	public static Image FromBytes(byte[] bytes, int width, int height, bool stretch = true, bool aspectRatio = false)
	{
		if (bytes == null || bytes.Length == 0)
		{
			return null;
		}
		return StiImageConverter.BytesToImage(bytes, width, height, stretch, aspectRatio);
	}

	public static Image GetImageFromObject(object imageObject, int width = 200, int height = 200, bool stretch = true, bool aspectRatio = false)
	{
		return StiImageConverter.BytesToImage(GetImageBytesFromObject(imageObject), width, height, stretch, aspectRatio);
	}

	public static byte[] GetImageBytesFromObject(object imageObject)
	{
		if (imageObject is Image)
		{
			return StiImageConverter.ImageToBytes((Image)((imageObject is Image) ? imageObject : null), false);
		}
		if (imageObject is string && ((string)imageObject).Length > 0)
		{
			try
			{
				string text = imageObject as string;
				int num = StringExt.IndexOfInvariant(text, "<svg");
				if (num >= 0 && num < 1000)
				{
					return Encoding.UTF8.GetBytes(text);
				}
				return StiImageConverter.StringToByteArray(text);
			}
			catch
			{
			}
		}
		byte[] array = imageObject as byte[];
		ProcessGarbageCollector();
		if (array != null && array.Length != 0)
		{
			if (OleUnit.IsOleHeader(array))
			{
				OleUnit.ObjectHeader objectHeader = new OleUnit.ObjectHeader(array);
				byte[] array2 = new byte[array.Length - objectHeader.HeaderLen];
				Array.Copy(array, objectHeader.HeaderLen, array2, 0, array.Length - objectHeader.HeaderLen);
				usedMemoryCounter += array.Length;
				array = array2;
			}
			if (OleUnit.IsOleContainer(array))
			{
				OleUnit.OleContainer oleContainer = new OleUnit.OleContainer(array);
				int streamNumber = 0;
				for (int i = 1; i < oleContainer.Dir.Length; i++)
				{
					if (oleContainer.Dir[i].Name.IndexOf("OlePres", StringComparison.InvariantCulture) != -1)
					{
						streamNumber = i;
						break;
					}
				}
				usedMemoryCounter += array.Length;
				array = oleContainer.GetStreamData(streamNumber);
				oleContainer.Clear();
				if (array == null || array.Length == 0)
				{
					return null;
				}
				int num2 = 0;
				bool flag = false;
				int num3 = array.Length;
				if (OleUnit.CheckForOlePres(array))
				{
					num2 = 40;
					if (BitConverter.ToUInt16(array, num2) == 1 && BitConverter.ToUInt16(array, num2 + 2) == 9)
					{
						num2 += 18;
						int num4 = 0;
						int num5 = 0;
						do
						{
							num5 = BitConverter.ToInt32(array, num2);
							num4 = BitConverter.ToInt16(array, num2 + 4);
							switch (num4)
							{
							case 2881:
								num2 += 26;
								flag = true;
								num3 = num5 * 2 - 26;
								break;
							case 3907:
								num2 += 28;
								flag = true;
								num3 = num5 * 2 - 28;
								break;
							default:
								if (num5 < 1)
								{
									break;
								}
								num2 += num5 * 2;
								if (num4 == 0 && num5 == 3)
								{
									break;
								}
								continue;
							}
							break;
						}
						while (num2 < array.Length - 6);
					}
					if (!flag)
					{
						num2 = 40;
						num3 -= 40;
					}
				}
				int num6 = BitConverter.ToUInt16(array, num2 + 14);
				int num7 = 0;
				if (num6 == 8)
				{
					num7 = 1024;
				}
				if (num6 == 4)
				{
					num7 = 64;
				}
				byte[] array3 = new byte[14]
				{
					66, 77, 1, 53, 0, 0, 0, 0, 0, 0,
					54, 0, 0, 0
				};
				BitConverter.GetBytes(num3 + 14).CopyTo(array3, 2);
				BitConverter.GetBytes(54 + num7).CopyTo(array3, 10);
				MemoryStream memoryStream = new MemoryStream();
				memoryStream.Write(array3, 0, 14);
				memoryStream.Write(array, num2, num3);
				memoryStream.Position = 0L;
				usedMemoryCounter += array.Length;
				if (StiOptions.Engine.ConvertBmpDataToPng && memoryStream.Length > 200000)
				{
					Image val = Image.FromStream((Stream)memoryStream, true);
					MemoryStream memoryStream2 = new MemoryStream();
					val.Save((Stream)memoryStream2, ImageFormat.Png);
					memoryStream2.Seek(0L, SeekOrigin.Begin);
					memoryStream = memoryStream2;
					usedMemoryCounter += memoryStream.Length;
				}
				return memoryStream.ToArray();
			}
			return array;
		}
		return null;
	}

	private static void ProcessGarbageCollector()
	{
		if (StiOptions.Engine.UseGarbageCollectorForImageCache)
		{
			if (usedMemoryCounter > 100000000)
			{
				usedMemoryCounter = 0L;
				GC.Collect();
				GC.WaitForPendingFinalizers();
				GC.Collect();
			}
		}
		else
		{
			usedMemoryCounter = 0L;
		}
	}

	private static string GetImageName(byte[] data)
	{
		if (IsTiff(data))
		{
			return "Tiff";
		}
		if (IsPng(data))
		{
			return "Png";
		}
		if (IsGif(data))
		{
			return "Gif";
		}
		if (IsJpeg(data))
		{
			return "Jpeg";
		}
		if (IsBmp(data))
		{
			return "Bmp";
		}
		if (IsEmf(data))
		{
			return "Emf";
		}
		if (IsWmf(data))
		{
			return "Wmf";
		}
		if (IsIcon(data))
		{
			return "Icon";
		}
		if (IsSvg(data))
		{
			return "Svg";
		}
		if (IsXml(data))
		{
			for (int i = 5; i < data.Length - 4; i++)
			{
				if (data[i] == 60)
				{
					if (data[i + 1] != 115 || data[i + 2] != 118 || data[i + 3] != 103)
					{
						break;
					}
					return "Svg";
				}
			}
		}
		return null;
	}

	public static bool IsEqualImages(byte[] image1, byte[] image2, int bytesToCompare = 0)
	{
		if (image1 != null && image2 == null)
		{
			return false;
		}
		if (image1 == null && image2 != null)
		{
			return false;
		}
		if (image1.Length != image2.Length)
		{
			return false;
		}
		int num = ((bytesToCompare == 0) ? image1.Length : bytesToCompare);
		int num2 = num - num % 8;
		for (int i = 0; i < num2; i += 8)
		{
			if (BitConverter.ToInt64(image1, i) != BitConverter.ToInt64(image2, i))
			{
				return false;
			}
		}
		for (int j = num2; j < num; j++)
		{
			if (image1[j] != image2[j])
			{
				return false;
			}
		}
		return true;
	}

	public static Image RotateImage(Image image, StiImageRotation rotation, bool disposeOriginal = false)
	{
		if (image == null)
		{
			return null;
		}
		if (StiOptions.Engine.FullTrust && image is Metafile)
		{
			return RotateMetafile(image, rotation, disposeOriginal);
		}
		switch (rotation)
		{
		case StiImageRotation.Rotate180:
			image.RotateFlip((RotateFlipType)2);
			break;
		case StiImageRotation.Rotate90CW:
			image.RotateFlip((RotateFlipType)1);
			break;
		case StiImageRotation.Rotate90CCW:
			image.RotateFlip((RotateFlipType)3);
			break;
		case StiImageRotation.FlipHorizontal:
			image.RotateFlip((RotateFlipType)4);
			break;
		case StiImageRotation.FlipVertical:
			image.RotateFlip((RotateFlipType)6);
			break;
		}
		return image;
	}

	private static Image RotateMetafile(Image image, StiImageRotation rotation, bool disposeOriginal = false)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		if (image == null)
		{
			return null;
		}
		Metafile val = (Metafile)(object)((image is Metafile) ? image : null);
		GraphicsUnit val2 = (GraphicsUnit)2;
		RectangleF bounds = ((Image)val).GetBounds(ref val2);
		PointF pointF = new PointF(bounds.Width / 2f, bounds.Height / 2f);
		Metafile val7;
		switch (rotation)
		{
		case StiImageRotation.Rotate90CCW:
		{
			MemoryStream memoryStream = new MemoryStream();
			Bitmap val9 = new Bitmap(1, 1);
			try
			{
				Graphics val10 = Graphics.FromImage((Image)(object)val9);
				try
				{
					IntPtr hdc2 = val10.GetHdc();
					val7 = new Metafile((Stream)memoryStream, hdc2, new RectangleF(bounds.Y, bounds.X, bounds.Height, bounds.Width), (MetafileFrameUnit)2);
					val10.ReleaseHdc(hdc2);
				}
				finally
				{
					((IDisposable)val10)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val9)?.Dispose();
			}
			Graphics val11 = Graphics.FromImage((Image)(object)val7);
			try
			{
				val11.TranslateTransform(pointF.Y, pointF.X);
				val11.RotateTransform(-90f);
				val11.TranslateTransform(0f - pointF.X, 0f - pointF.Y);
				val11.DrawImage((Image)(object)val, val.GetMetafileHeader().Bounds.Left, val.GetMetafileHeader().Bounds.Top, ((Image)val).Width, ((Image)val).Height);
			}
			finally
			{
				((IDisposable)val11)?.Dispose();
			}
			if (disposeOriginal)
			{
				((Image)val).Dispose();
			}
			return (Image)(object)val7;
		}
		case StiImageRotation.Rotate90CW:
		{
			MemoryStream memoryStream = new MemoryStream();
			Bitmap val12 = new Bitmap(1, 1);
			try
			{
				Graphics val13 = Graphics.FromImage((Image)(object)val12);
				try
				{
					IntPtr hdc3 = val13.GetHdc();
					val7 = new Metafile((Stream)memoryStream, hdc3, new RectangleF(bounds.Y, bounds.X, bounds.Height, bounds.Width), (MetafileFrameUnit)2);
					val13.ReleaseHdc(hdc3);
				}
				finally
				{
					((IDisposable)val13)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val12)?.Dispose();
			}
			Graphics val14 = Graphics.FromImage((Image)(object)val7);
			try
			{
				val14.TranslateTransform(pointF.Y, pointF.X);
				val14.RotateTransform(90f);
				val14.TranslateTransform(0f - pointF.X, 0f - pointF.Y);
				val14.DrawImage((Image)(object)val, val.GetMetafileHeader().Bounds.Left, val.GetMetafileHeader().Bounds.Top, ((Image)val).Width, ((Image)val).Height);
			}
			finally
			{
				((IDisposable)val14)?.Dispose();
			}
			if (disposeOriginal)
			{
				((Image)val).Dispose();
			}
			return (Image)(object)val7;
		}
		case StiImageRotation.Rotate180:
		{
			MemoryStream memoryStream = new MemoryStream();
			Bitmap val5 = new Bitmap(1, 1);
			try
			{
				Graphics val6 = Graphics.FromImage((Image)(object)val5);
				try
				{
					IntPtr hdc = val6.GetHdc();
					val7 = new Metafile((Stream)memoryStream, hdc, bounds, (MetafileFrameUnit)2);
					val6.ReleaseHdc(hdc);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			Graphics val8 = Graphics.FromImage((Image)(object)val7);
			try
			{
				val8.TranslateTransform(pointF.X, pointF.Y);
				val8.RotateTransform(180f);
				val8.TranslateTransform(0f - pointF.X, 0f - pointF.Y);
				val8.DrawImage((Image)(object)val, val.GetMetafileHeader().Bounds.Left, val.GetMetafileHeader().Bounds.Top, ((Image)val).Width, ((Image)val).Height);
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
			if (disposeOriginal)
			{
				((Image)val).Dispose();
			}
			return (Image)(object)val7;
		}
		case StiImageRotation.FlipHorizontal:
		{
			Bitmap val15 = new Bitmap((int)bounds.Width, (int)bounds.Height);
			Graphics val16 = Graphics.FromImage((Image)(object)val15);
			try
			{
				val16.DrawImage((Image)(object)val, -val.GetMetafileHeader().Bounds.Left, -val.GetMetafileHeader().Bounds.Top, ((Image)val).Width, ((Image)val).Height);
			}
			finally
			{
				((IDisposable)val16)?.Dispose();
			}
			((Image)val15).RotateFlip((RotateFlipType)4);
			if (disposeOriginal)
			{
				((Image)val).Dispose();
			}
			return (Image)(object)val15;
		}
		case StiImageRotation.FlipVertical:
		{
			Bitmap val3 = new Bitmap((int)bounds.Width, (int)bounds.Height);
			Graphics val4 = Graphics.FromImage((Image)(object)val3);
			try
			{
				val4.DrawImage((Image)(object)val, -val.GetMetafileHeader().Bounds.Left, -val.GetMetafileHeader().Bounds.Top, ((Image)val).Width, ((Image)val).Height);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			((Image)val3).RotateFlip((RotateFlipType)6);
			if (disposeOriginal)
			{
				((Image)val).Dispose();
			}
			return (Image)(object)val3;
		}
		default:
			return (Image)(object)val;
		}
	}

	public static bool IsXml(byte[] data)
	{
		if (data != null && data.Length > 4 && data[0] == 60 && data[1] == 63 && data[2] == 120 && data[3] == 109)
		{
			return data[4] == 108;
		}
		return false;
	}

	public static bool IsSvg(byte[] data)
	{
		return StiSvgHelper.IsSvg(data);
	}

	public static bool IsIcon(byte[] data)
	{
		if (data != null && data.Length > 4 && data[0] == 0 && data[1] == 0 && (data[2] == 1 || data[2] == 2) && data[3] == 0)
		{
			return data[4] != 0;
		}
		return false;
	}

	public static bool IsWmf(byte[] data)
	{
		if (data != null && data.Length > 3)
		{
			if (data[0] != 215 || data[1] != 205 || data[2] != 198 || data[3] != 154)
			{
				if (data[0] == 1 && data[1] == 0 && data[2] == 9)
				{
					return data[3] == 0;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public static bool IsEmf(byte[] data)
	{
		if (data != null && data.Length > 45 && data[40] == 32 && data[41] == 69 && data[42] == 77)
		{
			return data[43] == 70;
		}
		return false;
	}

	public static bool IsBmp(byte[] data)
	{
		if (data != null && data.Length > 2 && data[0] == 66)
		{
			return data[1] == 77;
		}
		return false;
	}

	public static bool IsJpeg(byte[] data)
	{
		if (data != null && data.Length > 2 && data[0] == byte.MaxValue)
		{
			return data[1] == 216;
		}
		return false;
	}

	public static bool IsGif(byte[] data)
	{
		if (data != null && data.Length > 3 && data[0] == 71 && data[1] == 73)
		{
			return data[2] == 70;
		}
		return false;
	}

	public static bool IsPng(byte[] data)
	{
		if (data != null && data.Length > 3 && data[0] == 137 && data[1] == 80 && data[2] == 78)
		{
			return data[3] == 71;
		}
		return false;
	}

	public static bool IsTiff(byte[] data)
	{
		if (data != null && data.Length > 3)
		{
			if (data[0] != 73 || data[1] != 73 || data[2] != 42 || data[3] != 0)
			{
				if (data[0] == 77 && data[1] == 77 && data[2] == 0)
				{
					return data[3] == 42;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public static bool IsMetafile(byte[] data)
	{
		if (!IsEmf(data))
		{
			return IsWmf(data);
		}
		return true;
	}

	public static bool IsImage(object data)
	{
		if (data is byte[])
		{
			return IsImage(data as byte[]);
		}
		if (data is string)
		{
			return IsImage(data as string);
		}
		return data is Image;
	}

	public static bool IsImage(byte[] data)
	{
		if (data != null && data.Length < 128)
		{
			return false;
		}
		if (OleUnit.IsOleHeader(data))
		{
			OleUnit.ObjectHeader objectHeader = new OleUnit.ObjectHeader(data);
			byte[] array = new byte[data.Length - objectHeader.HeaderLen];
			Array.Copy(data, objectHeader.HeaderLen, array, 0, data.Length - objectHeader.HeaderLen);
			data = array;
		}
		return GetImageName(data) != null;
	}

	public static bool IsImage(string str)
	{
		if (str != null && str.Length < 128)
		{
			return false;
		}
		try
		{
			if (!StringExt.IsBase64String(str))
			{
				return false;
			}
			byte[] data = Convert.FromBase64String(str.Substring(0, 256));
			return GetImageName(data) != null;
		}
		catch
		{
		}
		return false;
	}
}
