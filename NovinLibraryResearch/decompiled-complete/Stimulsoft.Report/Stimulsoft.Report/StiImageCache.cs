using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Export;

namespace Stimulsoft.Report;

public class StiImageCache
{
	public ArrayList ImageStore;

	public ArrayList ImagePackedStore;

	public ArrayList ImageMaskStore;

	public ArrayList ImageIndex;

	public List<ImageFormat> ImageFormatStore;

	public ArrayList ImagePaletteStore;

	private List<uint> ImageChecksum;

	private Hashtable ImageHashTable;

	private bool useImageComparer = true;

	private bool useImageCompression;

	private bool useImageTransparency;

	private ImageCodecInfo jpegCodec;

	private ImageFormat imageSaveFormat = ImageFormat.Bmp;

	private float imageQuality = 1f;

	private int indexedColorPaletteSize;

	private long usedMemoryCounter;

	private long usedMemoryCounterLimit = 100000000L;

	private static readonly uint CrcSeed = uint.MaxValue;

	private static readonly uint[] CrcTable = new uint[256]
	{
		0u, 1996959894u, 3993919788u, 2567524794u, 124634137u, 1886057615u, 3915621685u, 2657392035u, 249268274u, 2044508324u,
		3772115230u, 2547177864u, 162941995u, 2125561021u, 3887607047u, 2428444049u, 498536548u, 1789927666u, 4089016648u, 2227061214u,
		450548861u, 1843258603u, 4107580753u, 2211677639u, 325883990u, 1684777152u, 4251122042u, 2321926636u, 335633487u, 1661365465u,
		4195302755u, 2366115317u, 997073096u, 1281953886u, 3579855332u, 2724688242u, 1006888145u, 1258607687u, 3524101629u, 2768942443u,
		901097722u, 1119000684u, 3686517206u, 2898065728u, 853044451u, 1172266101u, 3705015759u, 2882616665u, 651767980u, 1373503546u,
		3369554304u, 3218104598u, 565507253u, 1454621731u, 3485111705u, 3099436303u, 671266974u, 1594198024u, 3322730930u, 2970347812u,
		795835527u, 1483230225u, 3244367275u, 3060149565u, 1994146192u, 31158534u, 2563907772u, 4023717930u, 1907459465u, 112637215u,
		2680153253u, 3904427059u, 2013776290u, 251722036u, 2517215374u, 3775830040u, 2137656763u, 141376813u, 2439277719u, 3865271297u,
		1802195444u, 476864866u, 2238001368u, 4066508878u, 1812370925u, 453092731u, 2181625025u, 4111451223u, 1706088902u, 314042704u,
		2344532202u, 4240017532u, 1658658271u, 366619977u, 2362670323u, 4224994405u, 1303535960u, 984961486u, 2747007092u, 3569037538u,
		1256170817u, 1037604311u, 2765210733u, 3554079995u, 1131014506u, 879679996u, 2909243462u, 3663771856u, 1141124467u, 855842277u,
		2852801631u, 3708648649u, 1342533948u, 654459306u, 3188396048u, 3373015174u, 1466479909u, 544179635u, 3110523913u, 3462522015u,
		1591671054u, 702138776u, 2966460450u, 3352799412u, 1504918807u, 783551873u, 3082640443u, 3233442989u, 3988292384u, 2596254646u,
		62317068u, 1957810842u, 3939845945u, 2647816111u, 81470997u, 1943803523u, 3814918930u, 2489596804u, 225274430u, 2053790376u,
		3826175755u, 2466906013u, 167816743u, 2097651377u, 4027552580u, 2265490386u, 503444072u, 1762050814u, 4150417245u, 2154129355u,
		426522225u, 1852507879u, 4275313526u, 2312317920u, 282753626u, 1742555852u, 4189708143u, 2394877945u, 397917763u, 1622183637u,
		3604390888u, 2714866558u, 953729732u, 1340076626u, 3518719985u, 2797360999u, 1068828381u, 1219638859u, 3624741850u, 2936675148u,
		906185462u, 1090812512u, 3747672003u, 2825379669u, 829329135u, 1181335161u, 3412177804u, 3160834842u, 628085408u, 1382605366u,
		3423369109u, 3138078467u, 570562233u, 1426400815u, 3317316542u, 2998733608u, 733239954u, 1555261956u, 3268935591u, 3050360625u,
		752459403u, 1541320221u, 2607071920u, 3965973030u, 1969922972u, 40735498u, 2617837225u, 3943577151u, 1913087877u, 83908371u,
		2512341634u, 3803740692u, 2075208622u, 213261112u, 2463272603u, 3855990285u, 2094854071u, 198958881u, 2262029012u, 4057260610u,
		1759359992u, 534414190u, 2176718541u, 4139329115u, 1873836001u, 414664567u, 2282248934u, 4279200368u, 1711684554u, 285281116u,
		2405801727u, 4167216745u, 1634467795u, 376229701u, 2685067896u, 3608007406u, 1308918612u, 956543938u, 2808555105u, 3495958263u,
		1231636301u, 1047427035u, 2932959818u, 3654703836u, 1088359270u, 936918000u, 2847714899u, 3736837829u, 1202900863u, 817233897u,
		3183342108u, 3401237130u, 1404277552u, 615818150u, 3134207493u, 3453421203u, 1423857449u, 601450431u, 3009837614u, 3294710456u,
		1567103746u, 711928724u, 3020668471u, 3272380065u, 1510334235u, 755167117u
	};

	internal long UsedMemoryCounter
	{
		get
		{
			return usedMemoryCounter;
		}
		set
		{
			usedMemoryCounter = value;
		}
	}

	internal long UsedMemoryCounterLimit
	{
		get
		{
			return usedMemoryCounterLimit;
		}
		set
		{
			usedMemoryCounterLimit = value;
		}
	}

	private void Init(bool useImageComparer, bool useImageCompression, ImageFormat imageFormat)
	{
		ImageStore = new ArrayList();
		ImagePackedStore = new ArrayList();
		ImageMaskStore = new ArrayList();
		ImageIndex = new ArrayList();
		ImageFormatStore = new List<ImageFormat>();
		ImagePaletteStore = new ArrayList();
		ImageChecksum = new List<uint>();
		ImageHashTable = new Hashtable();
		this.useImageComparer = useImageComparer;
		this.useImageCompression = useImageCompression;
		imageSaveFormat = imageFormat;
	}

	public StiImageCache(bool useImageComparer)
	{
		Init(useImageComparer, useImageCompression: false, ImageFormat.Bmp);
	}

	public StiImageCache(bool useImageComparer, ImageFormat imageFormat, float imageQuality)
	{
		Init(useImageComparer, useImageCompression: true, imageFormat);
		this.imageQuality = imageQuality;
		jpegCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
	}

	public StiImageCache(bool useImageComparer, ImageFormat imageFormat, float imageQuality, bool useImageTransparency, int paletteSize)
	{
		Init(useImageComparer, useImageCompression: true, imageFormat);
		this.imageQuality = imageQuality;
		jpegCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
		this.useImageTransparency = useImageTransparency;
		indexedColorPaletteSize = paletteSize;
	}

	public void Clear()
	{
		ImageStore.Clear();
		ImageStore = null;
		ImagePackedStore.Clear();
		ImagePackedStore = null;
		ImageMaskStore.Clear();
		ImageMaskStore = null;
		ImageIndex.Clear();
		ImageIndex = null;
		ImageFormatStore.Clear();
		ImageFormatStore = null;
		ImagePaletteStore.Clear();
		ImagePaletteStore = null;
		ImageChecksum.Clear();
		ImageChecksum = null;
		ImageHashTable.Clear();
		ImageHashTable = null;
		jpegCodec = null;
	}

	public int AddImageInt(Image image)
	{
		return AddImageInt(image, imageSaveFormat);
	}

	public int AddImageInt(Image image, float imageQuality)
	{
		float num = this.imageQuality;
		this.imageQuality = imageQuality;
		int result = AddImageInt(image, imageSaveFormat);
		this.imageQuality = num;
		return result;
	}

	public int AddImageInt(Image image, ImageFormat imageFormat)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Invalid comparison between Unknown and I4
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Invalid comparison between Unknown and I4
		uint num = (uint)ImageStore.Count;
		byte[] array = null;
		byte[] array2 = null;
		Color[] palette = null;
		Image val = image;
		int width = image.Width;
		int height = image.Height;
		if (imageFormat == null)
		{
			imageFormat = ImageFormat.Jpeg;
			if (((object)ImageFormat.Jpeg).Equals((object?)image.RawFormat))
			{
				imageFormat = ImageFormat.Jpeg;
			}
			if (((object)ImageFormat.Png).Equals((object?)image.RawFormat))
			{
				imageFormat = ImageFormat.Png;
			}
			if (((object)ImageFormat.Gif).Equals((object?)image.RawFormat))
			{
				imageFormat = ImageFormat.Gif;
			}
		}
		if (useImageComparer || useImageCompression)
		{
			MemoryStream memoryStream = new MemoryStream();
			if (imageFormat == ImageFormat.MemoryBmp)
			{
				if ((int)image.PixelFormat != 2498570 && (int)image.PixelFormat != 139273 && (int)image.PixelFormat != 196865)
				{
					Image val2 = (Image)new Bitmap(width, height, (PixelFormat)2498570);
					Graphics val3 = Graphics.FromImage(val2);
					val3.DrawImage(image, 0, 0, width, height);
					val3.Dispose();
					image = val2;
				}
				image.Save((Stream)memoryStream, ImageFormat.Bmp);
				memoryStream.Seek(0L, SeekOrigin.End);
				memoryStream.WriteByte(0);
				byte[] array3 = memoryStream.ToArray();
				memoryStream.Close();
				memoryStream.Dispose();
				usedMemoryCounter += array3.Length;
				byte[] array4 = null;
				if ((int)image.PixelFormat == 196865)
				{
					array4 = new byte[height * ((width - 1) / 8 + 1) + 4];
					int num2 = 0;
					byte b = 128;
					byte b2 = 0;
					int num3 = BitConverter.ToInt32(array3, 10);
					int num4 = width / 32;
					if (width % 32 > 0)
					{
						num4++;
					}
					num4 *= 4;
					for (int i = 0; i < height; i++)
					{
						int num5 = num3 + (height - i - 1) * num4;
						byte b3 = 128;
						byte b4 = array3[num5];
						for (int j = 0; j < width; j++)
						{
							if ((b4 & b3) != 0)
							{
								b2 |= b;
							}
							b3 >>= 1;
							if (b3 == 0)
							{
								b3 = 128;
								num5++;
								b4 = array3[num5];
							}
							b >>= 1;
							if (b == 0)
							{
								array4[num2] = b2;
								b2 = 0;
								b = 128;
								num2++;
							}
						}
						if (b != 128)
						{
							array4[num2] = b2;
							b2 = 0;
							b = 128;
							num2++;
						}
					}
				}
				else
				{
					array4 = new byte[height * width * 3];
					int num6 = BitConverter.ToInt32(array3, 10);
					int num7 = 0;
					for (int k = 0; k < height; k++)
					{
						int num8 = num6 + (height - k - 1) * width * 4;
						for (int l = 0; l < width; l++)
						{
							array4[num7] = array3[num8 + 2];
							array4[num7 + 1] = array3[num8 + 1];
							array4[num7 + 2] = array3[num8];
							num7 += 3;
							num8 += 4;
						}
					}
				}
				memoryStream = ((indexedColorPaletteSize != 0 && (int)image.PixelFormat != 196865) ? StiExportUtils.MakePdfDeflateStream(StiColorQuantization.ConvertToIndexed(array4, indexedColorPaletteSize, out palette, StiMonochromeDitheringType.None, ref usedMemoryCounter)) : StiExportUtils.MakePdfDeflateStream(array4));
				usedMemoryCounter += array3.Length;
				usedMemoryCounter += array4.Length;
				array3 = null;
				array4 = null;
			}
			else
			{
				if (image.HorizontalResolution.Equals(1f) || (int)image.PixelFormat == 8207)
				{
					Image val4 = (Image)new Bitmap(width, height, (PixelFormat)2498570);
					Graphics val5 = Graphics.FromImage(val4);
					val5.DrawImage(image, 0, 0, width, height);
					val5.Dispose();
					image = val4;
				}
				if (((object)ImageFormat.Jpeg).Equals((object?)imageFormat) && jpegCodec != null)
				{
					EncoderParameters val6 = new EncoderParameters(1);
					val6.Param[0] = new EncoderParameter(Encoder.Quality, (long)(imageQuality * 100f));
					image.Save((Stream)memoryStream, jpegCodec, val6);
				}
				else
				{
					image.Save((Stream)memoryStream, imageFormat);
				}
			}
			array = memoryStream.ToArray();
			memoryStream.Close();
			memoryStream.Dispose();
			usedMemoryCounter += array.Length;
			if (imageFormat == ImageFormat.Bmp && array != null && BitConverter.ToInt16(array, 0) == 19778 && BitConverter.ToInt32(array, 14) == 40 && BitConverter.ToInt32(array, 28) == 24 && image.Width * 3 % 4 > 0)
			{
				int num9 = BitConverter.ToInt32(array, 10) + image.Width * 3;
				int num10 = 4 - image.Width * 3 % 4;
				int num11 = (image.Width * 3 / 4 + 1) * 4;
				for (int m = 0; m < image.Height; m++)
				{
					for (int n = 0; n < num10; n++)
					{
						array[num9 + n] = 0;
					}
					num9 += num11;
				}
			}
		}
		if (useImageComparer)
		{
			num = CrcSeed;
			int num12 = 0;
			int num13 = array.Length;
			while (--num13 >= 0)
			{
				num = CrcTable[(num ^ array[num12++]) & 0xFF] ^ (num >> 8);
			}
			num ^= CrcSeed;
		}
		if (useImageCompression)
		{
			image = null;
		}
		else
		{
			array = null;
		}
		int num14 = -1;
		if (ImageHashTable.ContainsKey(num))
		{
			num14 = (int)ImageHashTable[num];
		}
		else
		{
			if (useImageCompression && useImageTransparency && val is Bitmap)
			{
				Bitmap val7 = (Bitmap)(object)((val is Bitmap) ? val : null);
				int width2 = ((Image)val7).Width;
				int height2 = ((Image)val7).Height;
				byte[] array5 = null;
				if (StiOptions.Engine.FullTrust)
				{
					array5 = GetMaskInFullTrust(val7, ref usedMemoryCounter);
				}
				else
				{
					array5 = new byte[width2 * height2];
					bool flag = false;
					for (int num15 = 0; num15 < height2; num15++)
					{
						int num16 = num15 * width2;
						for (int num17 = 0; num17 < width2; num17++)
						{
							byte a = val7.GetPixel(num17, num15).A;
							array5[num16++] = a;
							if (a != byte.MaxValue)
							{
								flag = true;
							}
						}
					}
					usedMemoryCounter += array5.Length;
					if (!flag)
					{
						array5 = null;
					}
				}
				if (array5 != null)
				{
					MemoryStream memoryStream2 = StiExportUtils.MakePdfDeflateStream(array5);
					array2 = memoryStream2.ToArray();
					memoryStream2.Close();
					memoryStream2.Dispose();
					usedMemoryCounter += array2.Length;
				}
				array5 = null;
			}
			num14 = ImageStore.Count;
			ImageStore.Add(image);
			ImagePackedStore.Add(array);
			ImageMaskStore.Add(array2);
			ImagePaletteStore.Add(palette);
			ImageChecksum.Add(num);
			ImageHashTable.Add(num, num14);
			ImageFormatStore.Add(imageFormat);
		}
		ImageIndex.Add(num14);
		array = null;
		array2 = null;
		val = null;
		if (StiOptions.Engine.UseGarbageCollectorForImageCache)
		{
			if (usedMemoryCounter > UsedMemoryCounterLimit)
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
		return num14;
	}

	private byte[] GetMaskInFullTrust(Bitmap bitmap, ref long usedMemoryCounter)
	{
		int num = 0;
		int num2 = 0;
		byte[] array = null;
		byte[] array2 = null;
		int width = ((Image)bitmap).Width;
		int height = ((Image)bitmap).Height;
		Rectangle rectangle = new Rectangle(0, 0, width, height);
		BitmapData val = bitmap.LockBits(rectangle, (ImageLockMode)1, (PixelFormat)2498570);
		int num3 = Math.Abs(val.Stride);
		num = num3 * val.Height;
		num2 = val.Width * val.Height;
		array = new byte[num];
		array2 = new byte[num2];
		Marshal.Copy(val.Scan0, array, 0, num);
		bitmap.UnlockBits(val);
		bool flag = false;
		for (int i = 0; i < height; i++)
		{
			int num4 = i * num3 + 3;
			int num5 = i * width;
			for (int num6 = width; num6 > 0; num6--)
			{
				if ((array2[num5] = array[num4]) != byte.MaxValue)
				{
					flag = true;
				}
				num4 += 4;
				num5++;
			}
		}
		usedMemoryCounter += array.Length;
		usedMemoryCounter += array2.Length;
		array = null;
		if (!flag)
		{
			array2 = null;
		}
		return array2;
	}
}
