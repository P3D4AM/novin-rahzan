using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base;

public static class StiFontCollection
{
	private static object lockObject = new object();

	private static Hashtable fontFamilyHash = new Hashtable();

	private static Hashtable fontPathHash = new Hashtable();

	private static Hashtable fontCustomFamilyHash = new Hashtable();

	private static Hashtable fontVHash = new Hashtable();

	private static Hashtable fontVCountHash = new Hashtable();

	private static Hashtable fontNameToHash = new Hashtable();

	private static Hashtable fontHashToData = new Hashtable();

	private static PrivateFontCollection instance = new PrivateFontCollection();

	public static PrivateFontCollection Instance
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			object obj = instance;
			if (obj == null)
			{
				PrivateFontCollection val = new PrivateFontCollection();
				instance = val;
				obj = (object)val;
			}
			return (PrivateFontCollection)obj;
		}
	}

	public static string DefaultCachePath { get; set; }

	public static bool AllowFileCache { get; set; } = true;


	public static void AddFontFile(string fileName, string alias = null)
	{
		lock (lockObject)
		{
			FontFamily[] families = ((FontCollection)Instance).Families;
			Instance.AddFontFile(fileName);
			FontFamily[] families2 = ((FontCollection)Instance).Families;
			if (families2.Length == families.Length)
			{
				return;
			}
			FontFamily val = families2.LastOrDefault();
			FontFamily[] array = families2;
			foreach (FontFamily val2 in array)
			{
				if (!families.Contains(val2))
				{
					val = val2;
					break;
				}
			}
			string key = val.Name;
			if (!string.IsNullOrWhiteSpace(alias))
			{
				key = alias;
			}
			fontPathHash[key] = fileName;
			fontCustomFamilyHash[key] = val;
		}
	}

	public static void AddMemoryFont(IntPtr memory, int length)
	{
		lock (lockObject)
		{
			Instance.AddMemoryFont(memory, length);
		}
	}

	public static List<FontFamily> GetFontFamilies()
	{
		lock (lockObject)
		{
			List<FontFamily> list = FontFamily.Families.ToList();
			list.AddRange(((FontCollection)instance).Families);
			foreach (DictionaryEntry item in fontVHash)
			{
				if (item.Value is FontV { Font: not null } fontV)
				{
					list.Add(fontV.Font.FontFamily);
				}
			}
			return list.OrderBy((FontFamily f) => f.Name).ToList();
		}
	}

	public static List<FontFamily> GetFontFamiliesFirstCustom()
	{
		lock (lockObject)
		{
			List<FontFamily> list = new List<FontFamily>();
			foreach (DictionaryEntry item in fontVHash)
			{
				if (item.Value is FontV { Font: not null } fontV)
				{
					list.Add(fontV.Font.FontFamily);
				}
			}
			list = list.OrderBy((FontFamily x) => x.Name).ToList();
			list.AddRange(FontFamily.Families.ToList());
			list.AddRange(((FontCollection)instance).Families);
			return list;
		}
	}

	public static FontFamily GetFontFamily(string fontName, bool allowNullResult = false)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		lock (lockObject)
		{
			object? obj = fontFamilyHash[fontName];
			FontFamily val = (FontFamily)((obj is FontFamily) ? obj : null);
			if (val != null)
			{
				return val;
			}
			object? obj2 = fontCustomFamilyHash[fontName];
			val = (FontFamily)((obj2 is FontFamily) ? obj2 : null);
			if (val == null)
			{
				FontV fontV = fontVHash.Values.Cast<FontV>().FirstOrDefault((FontV f) => f != null && f.Font != null && f.Name == fontName);
				if (fontV == null)
				{
					fontV = fontVHash.Values.Cast<FontV>().FirstOrDefault((FontV f) => f != null && f.Font != null && f.Font.FontFamily.Name == fontName);
				}
				if (fontV != null)
				{
					val = fontV.Font.FontFamily;
				}
			}
			if (val == null)
			{
				Font val2 = new Font(fontName, 1f, (FontStyle)0);
				try
				{
					if (val2.Name != fontName && allowNullResult)
					{
						return null;
					}
					val = val2.FontFamily;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			fontFamilyHash[fontName] = val;
			return val;
		}
	}

	public static bool IsCustomFont(string fontName)
	{
		if (((FontCollection)Instance).Families.Any((FontFamily f) => f.Name == fontName))
		{
			return true;
		}
		if (fontCustomFamilyHash.Keys.Cast<string>().Any((string f) => f == fontName))
		{
			return true;
		}
		return fontVHash.Values.Cast<FontV>().Any((FontV f) => (f != null && f.Font.FontFamily.Name == fontName) || f.Name == fontName);
	}

	public static string GetCustomFontPath(string fontName)
	{
		string text = fontPathHash[fontName] as string;
		if (text == null)
		{
			return null;
		}
		if (!text.StartsWith("file:///"))
		{
			text = "file:///" + text;
		}
		return text;
	}

	public static byte[] GetCustomFontData(string fontName)
	{
		string text = fontNameToHash[fontName] as string;
		if (!string.IsNullOrEmpty(text))
		{
			return fontHashToData[text] as byte[];
		}
		string text2 = fontPathHash[fontName] as string;
		if (string.IsNullOrWhiteSpace(text2))
		{
			return null;
		}
		byte[] array = null;
		try
		{
			array = File.ReadAllBytes(text2);
			string hashName = FontV.GetHashName(array);
			fontNameToHash[fontName] = hashName;
			fontHashToData[hashName] = array;
		}
		catch
		{
			fontPathHash[fontName] = null;
		}
		return array;
	}

	public static Font CreateFont(string fontName, float fontSize, FontStyle fontStyle)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		if (fontSize <= 0f)
		{
			fontSize = 0.1f;
		}
		return new Font(GetFontFamily(fontName), fontSize, fontStyle);
	}

	public static void AddResourceFont(string name, byte[] content, string extension, string alias)
	{
		if (content == null || content.Length == 0)
		{
			return;
		}
		string hashName = FontV.GetHashName(content);
		FontV fontV = fontVHash[hashName] as FontV;
		if (fontV == null)
		{
			lock (lockObject)
			{
				fontV = new FontV(alias, content, extension, AllowFileCache, hashName);
				fontVHash[hashName] = fontV;
				if (name == alias)
				{
					alias = fontV.Font.FontFamily.Name;
				}
				fontPathHash[alias] = fontV.CacheFile;
				fontFamilyHash.Remove(alias);
				if (!AllowFileCache)
				{
					fontNameToHash[alias] = hashName;
					fontHashToData[hashName] = content;
				}
				if (fontV.Name == fontV.Font.FontFamily.Name)
				{
					fontV.Name = name;
				}
			}
		}
		IncreaseCount(fontV);
	}

	public static FontFamily GetFontFamilyByContent(string name, byte[] content, string extension, string alias)
	{
		if (content == null || content.Length == 0)
		{
			return null;
		}
		string hashName = FontV.GetHashName(content);
		FontV fontV = fontVHash[hashName] as FontV;
		if (fontV == null)
		{
			lock (lockObject)
			{
				fontV = new FontV(alias, content, extension, AllowFileCache, hashName);
				fontVHash[hashName] = fontV;
				if (name == alias)
				{
					alias = fontV.Font.FontFamily.Name;
				}
				fontPathHash[alias] = fontV.CacheFile;
				fontFamilyHash.Remove(alias);
				if (!AllowFileCache)
				{
					fontNameToHash[alias] = hashName;
					fontHashToData[hashName] = content;
				}
			}
		}
		return fontV.Font.FontFamily;
	}

	private static void IncreaseCount(FontV fontV)
	{
		lock (lockObject)
		{
			int num = 1;
			object obj = fontVCountHash[fontV];
			if (obj != null)
			{
				num = Convert.ToInt32(obj);
				num++;
			}
			fontVCountHash[fontV] = num;
		}
	}

	public static void RemoveResourceFont(string name, byte[] content)
	{
		if (content == null || content.Length == 0)
		{
			return;
		}
		string hashName = FontV.GetHashName(content);
		object obj = fontVHash[hashName];
		if (obj == null)
		{
			return;
		}
		lock (lockObject)
		{
			object obj2 = fontVCountHash[obj];
			if (obj2 != null)
			{
				int num = Convert.ToInt32(obj2);
				if (num > 0)
				{
					num--;
				}
				fontVCountHash[obj] = num;
			}
		}
	}

	public static void RemoveUnusedResourceFonts()
	{
		lock (lockObject)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			foreach (DictionaryEntry item in fontVCountHash)
			{
				if (Convert.ToInt32(item.Value) != 0)
				{
					continue;
				}
				arrayList2.Add(item.Key);
				if (!(item.Key is FontV fontV))
				{
					continue;
				}
				foreach (DictionaryEntry item2 in fontVHash)
				{
					if (item2.Value == fontV)
					{
						arrayList.Add(item2.Key);
					}
				}
			}
			foreach (object item3 in arrayList)
			{
				fontVHash.Remove(item3);
			}
			foreach (object item4 in arrayList2)
			{
				fontVCountHash.Remove(item4);
			}
		}
	}

	public static void RemoveAllResourceFonts()
	{
		lock (lockObject)
		{
			fontVCountHash.Clear();
			foreach (DictionaryEntry item in fontVHash)
			{
				if (item.Value is FontV fontV)
				{
					fontV.Dispose();
				}
			}
			fontVHash.Clear();
		}
	}
}
