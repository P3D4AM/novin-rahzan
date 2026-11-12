using System;
using System.Collections;
using System.IO;
using System.Xml;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Helper;

public class StiLocalizationExt
{
	private static Hashtable categories;

	private static object lockObject = new object();

	public static string Localization { get; set; } = string.Empty;


	public static bool BlockLocalizationLoading { get; set; }

	public static void Load()
	{
		if (BlockLocalizationLoading)
		{
			return;
		}
		Stream stream = null;
		try
		{
			LoadDefaultLocalization();
			string localization = Localization;
			int num = localization.LastIndexOf(".", StringComparison.Ordinal);
			if (num > 0)
			{
				string text = localization.Insert(num, ".ext");
				if (text == "ua.ext.xml" || text == "be.ext.xml")
				{
					text = "ru.ext.xml";
				}
				if (text.ToLowerInvariant() == "en-gb.ext.xml")
				{
					text = "en.ext.xml";
				}
				if (text != "en.ext.xml")
				{
					stream = typeof(StiLocalizationExt).Assembly.GetManifestResourceStream("Stimulsoft.Report.Helper." + text);
					if (stream == null)
					{
						Localization = "en.xml";
					}
				}
			}
			if (stream != null)
			{
				Load(stream);
			}
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

	private static void LoadDefaultLocalization()
	{
		Stream stream = null;
		try
		{
			stream = typeof(StiLocalizationExt).Assembly.GetManifestResourceStream("Stimulsoft.Report.Helper.en.ext.xml");
			if (stream == null)
			{
				throw new Exception("The resourse 'Stimulsoft.Report.Helper.en.ext.xml' is not founded!");
			}
			if (categories != null)
			{
				categories.Clear();
			}
			Load(stream);
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

	public static void Load(Stream stream)
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

	private static void Add(string category, string key, string value)
	{
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

	public static string GetValue(string category, string key)
	{
		return Get(category, key);
	}

	public static string Get(string category, string key, bool throwError)
	{
		lock (lockObject)
		{
			if (StiLocalization.CultureName + ".xml" != Localization)
			{
				Localization = StiLocalization.CultureName + ".xml";
				Load();
			}
			if (categories == null)
			{
				Load();
			}
			if (categories == null)
			{
				if (throwError)
				{
					throw new Exception("The extended localization file is not loaded!");
				}
				return null;
			}
			if (!(categories[category] is Hashtable hashtable))
			{
				Hashtable hashtable2 = categories["Universal"] as Hashtable;
				if (hashtable2[key] is string result)
				{
					return result;
				}
				if (throwError)
				{
					throw new Exception("Category '" + category + "' is not found!");
				}
				return null;
			}
			if (!(hashtable[key] is string result2))
			{
				if (key.EndsWith("Category"))
				{
					return key.Substring(0, key.Length - 8);
				}
				Hashtable hashtable2 = categories["Universal"] as Hashtable;
				if (hashtable2[key] is string result3)
				{
					return result3;
				}
				if (throwError)
				{
					throw new Exception("Key '" + key + "' is not found!");
				}
				return null;
			}
			return result2;
		}
	}
}
