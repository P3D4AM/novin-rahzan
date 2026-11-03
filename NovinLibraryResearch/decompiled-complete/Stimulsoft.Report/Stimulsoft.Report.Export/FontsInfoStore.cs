using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.IO.Compression;

namespace Stimulsoft.Report.Export;

public class FontsInfoStore
{
	internal class FontInfo
	{
		public FontStyleInfo[] StylesInfo;

		public bool NeedSyntBold;

		public bool NeedSyntItalic;

		public byte[] Data;

		public byte[] OTM;

		public ushort[] Glyphs;

		public ushort[] Widths;

		public void SetIndex(int index)
		{
			NeedSyntBold = false;
			NeedSyntItalic = false;
			int num = index;
			if (index == 1 && StylesInfo[index].NeedSyntBold)
			{
				num = 0;
				NeedSyntBold = true;
			}
			if (index == 2 && StylesInfo[index].NeedSyntItalic)
			{
				num = 0;
				NeedSyntItalic = true;
			}
			if (index == 3 && (StylesInfo[index].NeedSyntBold || StylesInfo[index].NeedSyntItalic))
			{
				if (!StylesInfo[1].NeedSyntBold)
				{
					num = 1;
					NeedSyntItalic = true;
				}
				else if (!StylesInfo[2].NeedSyntItalic)
				{
					num = 2;
					NeedSyntBold = true;
				}
				else
				{
					num = 0;
					NeedSyntBold = true;
					NeedSyntItalic = true;
				}
			}
			OTM = StylesInfo[num].OTM;
			Glyphs = StylesInfo[num].Glyphs;
			Widths = StylesInfo[num].Widths;
			Data = StylesInfo[num].Data;
		}

		public FontInfo()
		{
			StylesInfo = new FontStyleInfo[4];
			for (int i = 0; i < 4; i++)
			{
				StylesInfo[i] = new FontStyleInfo();
			}
		}
	}

	internal class FontStyleInfo
	{
		public bool NeedSyntBold;

		public bool NeedSyntItalic;

		public byte[] Data;

		public byte[] OTM;

		public ushort[] Glyphs;

		public ushort[] Widths;
	}

	private static Hashtable Store = new Hashtable();

	private Hashtable Cache;

	public static void LoadFontInfoToStore(string fontName, byte[] data)
	{
		Store[fontName] = data;
	}

	public static void LoadFontInfoToStore(string fontName, Stream stream)
	{
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		Store[fontName] = array;
	}

	public static void LoadFontInfoToStore(string fontName, string path)
	{
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, array.Length);
		fileStream.Close();
		Store[fontName] = array;
	}

	public static void ClearFontsInfoStore()
	{
		Store.Clear();
	}

	internal void Clear()
	{
		Cache.Clear();
	}

	internal FontInfo GetFontInfo(Font font)
	{
		FontInfo fontInfo = null;
		if (Cache.ContainsKey(font.Name))
		{
			fontInfo = (FontInfo)Cache[font.Name];
		}
		else if (Store.ContainsKey(font.Name))
		{
			fontInfo = new FontInfo();
			byte[] buffer = (byte[])Store[font.Name];
			MemoryStream memoryStream = new MemoryStream(buffer);
			GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress, leaveOpen: true);
			buffer = new byte[14];
			gZipStream.Read(buffer, 0, 14);
			fontInfo.StylesInfo[0].NeedSyntBold = buffer[6] == 49;
			fontInfo.StylesInfo[1].NeedSyntBold = buffer[7] == 49;
			fontInfo.StylesInfo[2].NeedSyntBold = buffer[8] == 49;
			fontInfo.StylesInfo[3].NeedSyntBold = buffer[9] == 49;
			fontInfo.StylesInfo[0].NeedSyntItalic = buffer[6] == 49;
			fontInfo.StylesInfo[1].NeedSyntItalic = buffer[7] == 49;
			fontInfo.StylesInfo[2].NeedSyntItalic = buffer[8] == 49;
			fontInfo.StylesInfo[3].NeedSyntItalic = buffer[9] == 49;
			ReadFontStyle(gZipStream, fontInfo, 0);
			ReadFontStyle(gZipStream, fontInfo, 1);
			ReadFontStyle(gZipStream, fontInfo, 2);
			ReadFontStyle(gZipStream, fontInfo, 3);
			gZipStream.Close();
			memoryStream.Close();
			Cache.Add(font.Name, fontInfo);
		}
		if (fontInfo != null)
		{
			int num = 0;
			if (font.Bold)
			{
				num++;
			}
			if (font.Italic)
			{
				num += 2;
			}
			fontInfo.SetIndex(num);
		}
		return fontInfo;
	}

	private static void ReadFontStyle(GZipStream gs, FontInfo fontInfo, int styleIndex)
	{
		if (!fontInfo.StylesInfo[styleIndex].NeedSyntBold || !fontInfo.StylesInfo[styleIndex].NeedSyntItalic)
		{
			byte[] array = new byte[9];
			gs.Read(array, 0, 9);
			int num = BitConverter.ToInt32(array, 5);
			byte[] array2 = new byte[num];
			gs.Read(array2, 0, num);
			fontInfo.StylesInfo[styleIndex].OTM = array2;
			array = new byte[9];
			gs.Read(array, 0, 9);
			num = BitConverter.ToInt32(array, 5);
			array2 = new byte[num * 2];
			gs.Read(array2, 0, array2.Length);
			ushort[] array3 = new ushort[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				array3[i] = BitConverter.ToUInt16(array2, num2);
				num2 += 2;
			}
			fontInfo.StylesInfo[styleIndex].Glyphs = array3;
			array = new byte[9];
			gs.Read(array, 0, 9);
			num = BitConverter.ToInt32(array, 5);
			array2 = new byte[num * 2];
			gs.Read(array2, 0, array2.Length);
			ushort[] array4 = new ushort[num];
			num2 = 0;
			for (int j = 0; j < num; j++)
			{
				array4[j] = BitConverter.ToUInt16(array2, num2);
				num2 += 2;
			}
			fontInfo.StylesInfo[styleIndex].Widths = array4;
			array = new byte[9];
			gs.Read(array, 0, 9);
			num = BitConverter.ToInt32(array, 5);
			array2 = new byte[num];
			gs.Read(array2, 0, num);
			fontInfo.StylesInfo[styleIndex].Data = array2;
		}
	}

	public FontsInfoStore()
	{
		Cache = new Hashtable();
	}
}
