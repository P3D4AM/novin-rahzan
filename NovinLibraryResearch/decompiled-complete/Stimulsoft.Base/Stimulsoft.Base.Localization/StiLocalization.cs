using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base.Localization;

public class StiLocalization
{
	private static Hashtable categories;

	private static bool isLocalizationLoaded;

	private static string prevLocalization = string.Empty;

	private static object lockObject = new object();

	public static bool SearchLocalizationFromRegistry { get; set; } = true;


	public static string DirectoryLocalization { get; set; } = "Localization";


	public static string Localization { get; set; } = string.Empty;


	public static string Language { get; set; }

	public static string Description { get; set; }

	public static string CultureName { get; set; }

	public static bool IsEn
	{
		get
		{
			if (!(CultureName.ToLowerInvariant() == "en"))
			{
				return CultureName.ToLowerInvariant() == "en-gb";
			}
			return true;
		}
	}

	public static bool IsDe => CultureName.ToLowerInvariant() == "de";

	public static bool IsRu => CultureName.ToLowerInvariant() == "ru";

	public static bool IsRussian
	{
		get
		{
			if (!(CultureName.ToLowerInvariant() == "ru") && !(CultureName.ToLowerInvariant() == "be"))
			{
				return CultureName.ToLowerInvariant() == "uk";
			}
			return true;
		}
	}

	public static bool IsCyrillic
	{
		get
		{
			if (!IsRussian)
			{
				return CultureName?.ToLowerInvariant() == "bg";
			}
			return true;
		}
	}

	public static bool BlockLocalizationExceptions { get; set; }

	public static bool BlockLocalizationLoading { get; set; }

	public static string GetDirectoryLocalizationFromRegistry()
	{
		if (!SearchLocalizationFromRegistry)
		{
			return null;
		}
		try
		{
			string value = Registry.GetValue("Localization");
			return (value != null && Directory.Exists(value)) ? value : null;
		}
		catch
		{
		}
		return null;
	}

	public static string GetDirectoryLocales()
	{
		try
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			if (assembly == null)
			{
				assembly = Assembly.GetExecutingAssembly();
			}
			string directoryName = Path.GetDirectoryName(assembly.Location);
			string text = Path.Combine(directoryName, "locales");
			if (Directory.Exists(text))
			{
				string[] array = Directory.EnumerateFiles(text, "*.xml").ToArray();
				if (array.Length != 0)
				{
					return text;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public static string GetEnumValue(string key)
	{
		if (categories == null)
		{
			LoadDefaultLocalization();
		}
		if (categories == null)
		{
			return null;
		}
		if (!(categories["PropertyEnum"] is Hashtable hashtable))
		{
			return null;
		}
		if (!(hashtable[key] is string result))
		{
			if (!key.EndsWith("Category"))
			{
				return null;
			}
			return key.Substring(0, key.Length - 8);
		}
		return result;
	}

	public static void LoadDefaultLocalization()
	{
		Stream stream = null;
		try
		{
			stream = typeof(StiLocalization).Assembly.GetManifestResourceStream("Stimulsoft.Base.Localization.en.xml");
			if (stream == null)
			{
				throw new Exception("Resourse 'Stimulsoft.Base.Localization.en.xml' is not found!");
			}
			LoadInternal(stream);
		}
		finally
		{
			if (stream != null)
			{
				stream.Close();
				stream.Dispose();
			}
		}
	}

	private static DirectoryInfo GetDirectoryInfoFromRegister()
	{
		if (!SearchLocalizationFromRegistry)
		{
			return null;
		}
		string value = Registry.GetValue("Localization");
		if (value == null || !Directory.Exists(value))
		{
			return null;
		}
		return new DirectoryInfo(value);
	}

	private static string GetDirectoryLocalization()
	{
		if (Directory.Exists(DirectoryLocalization))
		{
			return DirectoryLocalization;
		}
		if (Directory.Exists("locales"))
		{
			return "locales";
		}
		DirectoryInfo directoryInfoFromRegister = GetDirectoryInfoFromRegister();
		if (directoryInfoFromRegister == null || !directoryInfoFromRegister.Exists)
		{
			return null;
		}
		return directoryInfoFromRegister.Name;
	}

	private static void Check()
	{
		if (!isLocalizationLoaded || !(prevLocalization == Localization))
		{
			isLocalizationLoaded = true;
			prevLocalization = Localization;
			LoadCurrentLocalization();
		}
	}

	public static void LoadCurrentLocalization()
	{
		if (BlockLocalizationLoading)
		{
			return;
		}
		string directoryLocalization = GetDirectoryLocalization();
		DirectoryInfo directoryInfo = (Directory.Exists(directoryLocalization) ? new DirectoryInfo(directoryLocalization) : null);
		if (directoryInfo != null && directoryInfo.Exists)
		{
			FileInfo[] array = (from f in directoryInfo.GetFiles()
				where IsXmlExtension(f.FullName)
				select f).ToArray();
			if (!string.IsNullOrWhiteSpace(Localization))
			{
				FileInfo fileInfo = array.FirstOrDefault((FileInfo f) => f.Name.ToLowerInvariant() == Localization.ToLowerInvariant());
				if (fileInfo != null)
				{
					Load(fileInfo.FullName);
					return;
				}
			}
			else
			{
				string name = CultureInfo.CurrentCulture.Name;
				string twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
				FileInfo[] array2 = array;
				foreach (FileInfo fileInfo2 in array2)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo2.Name);
					if (fileNameWithoutExtension == name || fileNameWithoutExtension == twoLetterISOLanguageName)
					{
						Load(fileInfo2.FullName);
						return;
					}
				}
			}
		}
		LoadDefaultLocalization();
	}

	public static void Load(string file)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
			Load(fileStream);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
	}

	public static void Load(Stream stream)
	{
		if (!BlockLocalizationLoading)
		{
			if (categories != null)
			{
				categories.Clear();
				categories = null;
			}
			LoadDefaultLocalization();
			LoadInternal(stream);
		}
	}

	private static void LoadInternal(Stream stream)
	{
		if (BlockLocalizationLoading)
		{
			return;
		}
		XmlTextReader xmlTextReader = new XmlTextReader(stream);
		xmlTextReader.Read();
		xmlTextReader.Read();
		xmlTextReader.Read();
		if (!(xmlTextReader.Name == "Localization"))
		{
			return;
		}
		Language = xmlTextReader.GetAttribute("language");
		Description = xmlTextReader.GetAttribute("description");
		CultureName = xmlTextReader.GetAttribute("cultureName");
		string category = string.Empty;
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.IsStartElement())
			{
				string name = xmlTextReader.Name;
				if (xmlTextReader.Depth == 1)
				{
					category = name;
				}
				else
				{
					Add(category, name, xmlTextReader.ReadString());
				}
			}
			xmlTextReader.Read();
		}
	}

	public static void GetParam(string file, out string language, out string description, out string cultureName, string folder = null)
	{
		if (folder != null && Directory.Exists(folder))
		{
			file = Path.Combine(folder, file);
		}
		if (File.Exists(file) && IsXmlExtension(file))
		{
			FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
			GetParam(fileStream, out language, out description, out cultureName);
			fileStream.Close();
			fileStream.Dispose();
		}
		else
		{
			language = null;
			description = null;
			cultureName = null;
		}
	}

	public static void GetParam(Stream stream, out string language, out string description, out string cultureName)
	{
		XmlTextReader xmlTextReader = new XmlTextReader(stream);
		xmlTextReader.Read();
		xmlTextReader.Read();
		xmlTextReader.Read();
		language = xmlTextReader.GetAttribute("language");
		description = xmlTextReader.GetAttribute("description");
		cultureName = xmlTextReader.GetAttribute("cultureName");
	}

	private static bool IsXmlExtension(string file)
	{
		string extension = Path.GetExtension(file);
		if (!string.IsNullOrEmpty(extension))
		{
			return extension.ToLowerInvariant() == ".xml";
		}
		return false;
	}

	public static void Add(string category, string key, string value)
	{
		Check();
		lock (lockObject)
		{
			if (categories == null)
			{
				categories = new Hashtable();
			}
			Hashtable hashtable = categories[category] as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				categories[category] = hashtable;
			}
			hashtable[key] = value;
		}
	}

	public static string Get(string category, string key)
	{
		return Get(category, key, throwError: true);
	}

	public static string GetCleaned(string category, string key)
	{
		return Loc.GetCleaned(category, key);
	}

	public static string GetValue(string category, string key)
	{
		return Get(category, key);
	}

	public static string Get(string category, string key, bool throwError)
	{
		Check();
		bool flag = throwError;
		if (BlockLocalizationExceptions)
		{
			throwError = false;
		}
		if (categories == null)
		{
			LoadDefaultLocalization();
		}
		if (categories == null)
		{
			if (throwError)
			{
				throw new Exception("Localization file is not loaded!");
			}
			return null;
		}
		Hashtable hashtable = categories[category] as Hashtable;
		if (hashtable == null)
		{
			if (flag)
			{
				LoadDefaultLocalization();
				hashtable = categories[category] as Hashtable;
			}
			if (hashtable == null)
			{
				if (throwError)
				{
					throw new Exception("Category '" + category + "' is not found!");
				}
				return null;
			}
		}
		string text = hashtable[key] as string;
		if (text == null)
		{
			if (key.EndsWith("Category"))
			{
				return key.Substring(0, key.Length - 8);
			}
			if (flag)
			{
				LoadDefaultLocalization();
				text = hashtable[key] as string;
			}
			if (text == null)
			{
				if (throwError)
				{
					throw new Exception("Key '" + key + "' is not found!");
				}
				return null;
			}
		}
		return text;
	}

	public static void Set(string category, string key, string value)
	{
		Check();
		if (categories == null)
		{
			LoadDefaultLocalization();
		}
		if (categories == null)
		{
			throw new Exception("Localization file is not loaded!");
		}
		if (!(categories[category] is Hashtable hashtable))
		{
			throw new Exception("Category '" + category + "' is not found!");
		}
		lock (lockObject)
		{
			hashtable[key] = value;
		}
	}

	public static string[] GetKeys(string category)
	{
		Check();
		if (categories == null)
		{
			throw new Exception("Localization file is not loaded!");
		}
		if (!(categories[category] is Hashtable hashtable))
		{
			throw new Exception("Category '" + category + "' is not found!");
		}
		lock (lockObject)
		{
			string[] array = new string[hashtable.Keys.Count];
			hashtable.Keys.CopyTo(array, 0);
			Array.Sort(array);
			return array;
		}
	}

	public static string[] GetValues(string category)
	{
		Check();
		if (categories == null)
		{
			throw new Exception("Localization file is not loaded!");
		}
		if (!(categories[category] is Hashtable hashtable))
		{
			throw new Exception("Category '" + category + "' is not found!");
		}
		lock (lockObject)
		{
			string[] array = new string[hashtable.Values.Count];
			hashtable.Values.CopyTo(array, 0);
			Array.Sort(array);
			return array;
		}
	}

	public static string[] GetCategories()
	{
		Check();
		if (categories == null)
		{
			throw new Exception("Localization file is not loaded!");
		}
		string[] array = new string[categories.Count];
		categories.Keys.CopyTo(array, 0);
		return array;
	}

	public static string GetLocalization(bool format)
	{
		if (categories == null)
		{
			LoadDefaultLocalization();
		}
		return JsonConvert.SerializeObject(categories, format ? Stimulsoft.Base.Json.Formatting.Indented : Stimulsoft.Base.Json.Formatting.None);
	}
}
