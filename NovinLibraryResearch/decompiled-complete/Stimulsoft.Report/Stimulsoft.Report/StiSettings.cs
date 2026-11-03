using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;

namespace Stimulsoft.Report;

public sealed class StiSettings
{
	private static DateTime? lastAccessTime;

	private static readonly object sync = new object();

	private static Hashtable settings = new Hashtable();

	public static string DefaultReportSettingsPath
	{
		get
		{
			return StiOptions.Configuration.DefaultReportSettingsPath;
		}
		set
		{
			StiOptions.Configuration.DefaultReportSettingsPath = value;
		}
	}

	public static string GetStr(string key, string subkey)
	{
		return GetStr(key, subkey, string.Empty);
	}

	public static string GetStr(string key, string subkey, string nullValue)
	{
		object obj = Get(key, subkey);
		if (obj == null || !(obj is string))
		{
			return nullValue;
		}
		return obj as string;
	}

	public static bool GetBool(string key, string subkey)
	{
		return GetBool(key, subkey, nullValue: false);
	}

	public static bool GetBool(string key, string subkey, bool nullValue)
	{
		object obj = Get(key, subkey);
		if (obj == null || !(obj is bool))
		{
			return nullValue;
		}
		return (bool)obj;
	}

	public static int GetInt(string key, string subkey)
	{
		return GetInt(key, subkey, -1);
	}

	public static int GetInt(string key, string subkey, int nullValue, bool isZoom = false)
	{
		object obj = Get(key, subkey);
		if (obj == null || !(obj is int))
		{
			if (!isZoom)
			{
				return nullValue;
			}
			return (int)Math.Ceiling(StiScale.Factor * (double)nullValue);
		}
		if (!isZoom)
		{
			return (int)obj;
		}
		return (int)Math.Ceiling(StiScale.Factor * (double)(int)obj);
	}

	public static Color GetColor(string key, string subkey, Color nullValue)
	{
		object obj = Get(key, subkey + ".A");
		object obj2 = Get(key, subkey + ".R");
		object obj3 = Get(key, subkey + ".G");
		object obj4 = Get(key, subkey + ".B");
		if (obj == null || !(obj is byte))
		{
			return nullValue;
		}
		if (obj2 == null || !(obj2 is byte))
		{
			return nullValue;
		}
		if (obj3 == null || !(obj3 is byte))
		{
			return nullValue;
		}
		if (obj4 == null || !(obj4 is byte))
		{
			return nullValue;
		}
		return Color.FromArgb((byte)obj, (byte)obj2, (byte)obj3, (byte)obj4);
	}

	public static float GetFloat(string key, string subkey)
	{
		return GetFloat(key, subkey, 0f);
	}

	public static float GetFloat(string key, string subkey, float nullValue)
	{
		object obj = Get(key, subkey);
		if (obj == null || !(obj is float))
		{
			return nullValue;
		}
		return (float)obj;
	}

	public static double GetDouble(string key, string subkey)
	{
		return GetDouble(key, subkey, 0.0);
	}

	public static double GetDouble(string key, string subkey, double nullValue)
	{
		object obj = Get(key, subkey);
		if (obj == null || !(obj is double))
		{
			return nullValue;
		}
		return (double)obj;
	}

	public static decimal GetDecimal(string key, string subkey)
	{
		return GetDecimal(key, subkey, 0m);
	}

	public static decimal GetDecimal(string key, string subkey, decimal nullValue)
	{
		object obj = Get(key, subkey);
		if (obj == null || !(obj is decimal))
		{
			return nullValue;
		}
		return (decimal)obj;
	}

	public static object Get(string key, string subkey)
	{
		return Get(key, subkey, null);
	}

	public static Hashtable GetCategory(string key)
	{
		if (!(settings[Environment.UserName] is Hashtable hashtable))
		{
			return null;
		}
		return hashtable[key] as Hashtable;
	}

	public static object Get(string key, string subkey, object nullValue)
	{
		if (!(settings[Environment.UserName] is Hashtable hashtable))
		{
			return nullValue;
		}
		if (!(hashtable[key] is Hashtable hashtable2))
		{
			return nullValue;
		}
		object obj = hashtable2[subkey];
		if (obj == null)
		{
			return nullValue;
		}
		return obj;
	}

	public static void Set(string key, string subkey, int value, bool isZoom = false)
	{
		Set(key, subkey, (object)(isZoom ? ((int)((double)value / StiScale.Factor)) : value));
	}

	public static void Set(string key, string subkey, object value)
	{
		Hashtable hashtable = settings[Environment.UserName] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			settings[Environment.UserName] = hashtable;
		}
		Hashtable hashtable2 = hashtable[key] as Hashtable;
		if (hashtable2 == null)
		{
			hashtable2 = (Hashtable)(hashtable[key] = new Hashtable());
		}
		if (value is Color color)
		{
			hashtable2[subkey + ".A"] = color.A;
			hashtable2[subkey + ".R"] = color.R;
			hashtable2[subkey + ".G"] = color.G;
			hashtable2[subkey + ".B"] = color.B;
		}
		else
		{
			hashtable2[subkey] = value;
		}
	}

	public static void ClearKey(string key)
	{
		if (settings[Environment.UserName] is Hashtable hashtable && hashtable.ContainsKey(key))
		{
			hashtable.Remove(key);
		}
	}

	public static void ClearKey(string key, string subkey)
	{
		if (settings[Environment.UserName] is Hashtable hashtable && hashtable[key] is Hashtable hashtable2 && hashtable2.ContainsKey(subkey))
		{
			hashtable2.Remove(subkey);
		}
	}

	public static void Clear()
	{
		settings.Clear();
	}

	public static void Load()
	{
		string path = StiPath.GetPath(GetDefaultReportSettingsPath());
		bool flag = File.Exists(path);
		DateTime? dateTime = (flag ? new DateTime?(new FileInfo(path).LastWriteTimeUtc) : null);
		if (!lastAccessTime.HasValue || lastAccessTime != dateTime)
		{
			if (flag)
			{
				Load(path);
			}
			else
			{
				Clear();
			}
			lastAccessTime = dateTime;
		}
	}

	public static void ReLoad()
	{
		Clear();
		string path = StiPath.GetPath(GetDefaultReportSettingsPath());
		if (File.Exists(path))
		{
			Load(path);
		}
	}

	public static void Load(string file)
	{
		Monitor.Enter(sync);
		try
		{
			if (File.Exists(file))
			{
				byte[] buffer = File.ReadAllBytes(file);
				using MemoryStream stream = new MemoryStream(buffer);
				Load(stream);
				return;
			}
		}
		catch
		{
		}
		finally
		{
			Monitor.Exit(sync);
		}
	}

	public static void Load(Stream stream)
	{
		Clear();
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextReader xmlTextReader = new XmlTextReader(stream);
			xmlTextReader.DtdProcessing = DtdProcessing.Ignore;
			xmlTextReader.Read();
			xmlTextReader.Read();
			StiReportObjectStringConverter stiReportObjectStringConverter = new StiReportObjectStringConverter();
			if (!xmlTextReader.IsStartElement() || !(xmlTextReader.Name == "StiSerializer"))
			{
				return;
			}
			string attribute = xmlTextReader.GetAttribute("application");
			if (!(attribute == "StiReportSettings"))
			{
				return;
			}
			Hashtable hashtable = null;
			Hashtable hashtable2 = null;
			xmlTextReader.Read();
			while (xmlTextReader.Read())
			{
				if (!xmlTextReader.IsStartElement())
				{
					continue;
				}
				if (xmlTextReader.Name == "User")
				{
					string attribute2 = xmlTextReader.GetAttribute("Name");
					Hashtable hashtable3 = settings[attribute2] as Hashtable;
					if (hashtable3 == null)
					{
						hashtable3 = new Hashtable();
						settings[attribute2] = hashtable3;
					}
					hashtable = hashtable3;
				}
				else if (xmlTextReader.Name == "Category")
				{
					string attribute3 = xmlTextReader.GetAttribute("Name");
					Hashtable hashtable4 = hashtable[attribute3] as Hashtable;
					if (hashtable4 == null)
					{
						hashtable4 = (Hashtable)(hashtable[attribute3] = new Hashtable());
					}
					hashtable2 = hashtable4;
				}
				else
				{
					if (!(xmlTextReader.Name == "Item"))
					{
						continue;
					}
					string attribute4 = xmlTextReader.GetAttribute("Key");
					string attribute5 = xmlTextReader.GetAttribute("Value");
					string attribute6 = xmlTextReader.GetAttribute("Type");
					Type type = StiTypeFinder.GetType(attribute6);
					if (type != null)
					{
						object obj = ((StiObjectStringConverter)stiReportObjectStringConverter).StringToObject(attribute5, type);
						if (obj != null)
						{
							hashtable2[attribute4] = obj;
						}
					}
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Loading settings...ERROR");
			StiLogService.Write(typeof(StiReport), e);
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public static void Save()
	{
		Save(StiPath.GetPath(GetDefaultReportSettingsPath()));
	}

	public static void Save(string file)
	{
		int num = 0;
		while (true)
		{
			bool flag = false;
			Monitor.Enter(sync);
			try
			{
				StiFileUtils.ProcessReadOnly(file);
				string directoryName = Path.GetDirectoryName(file);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				using MemoryStream memoryStream = new MemoryStream();
				Save(memoryStream);
				memoryStream.Flush();
				byte[] bytes = memoryStream.ToArray();
				File.WriteAllBytes(file, bytes);
			}
			catch
			{
				flag = true;
			}
			finally
			{
				Monitor.Exit(sync);
			}
			if (flag && num <= 4)
			{
				num++;
				continue;
			}
			break;
		}
	}

	public static void Save(Stream stream)
	{
		CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
		try
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stream, Encoding.UTF8);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("StiSerializer");
			xmlTextWriter.WriteAttributeString("version", "1.0");
			xmlTextWriter.WriteAttributeString("application", "StiReportSettings");
			ICollection keys = settings.Keys;
			foreach (string item in keys)
			{
				if (!(settings[item] is Hashtable { Count: >0 } hashtable))
				{
					continue;
				}
				xmlTextWriter.WriteStartElement("User");
				xmlTextWriter.WriteAttributeString("Name", item);
				ICollection keys2 = hashtable.Keys;
				foreach (string item2 in keys2)
				{
					if (!(hashtable[item2] is Hashtable { Count: >0 } hashtable2))
					{
						continue;
					}
					xmlTextWriter.WriteStartElement("Category");
					xmlTextWriter.WriteAttributeString("Name", item2);
					ICollection keys3 = hashtable2.Keys;
					foreach (string item3 in keys3)
					{
						object obj = hashtable2[item3];
						if (obj != null)
						{
							xmlTextWriter.WriteStartElement("Item");
							xmlTextWriter.WriteAttributeString("Key", item3);
							xmlTextWriter.WriteAttributeString("Value", obj.ToString());
							xmlTextWriter.WriteAttributeString("Type", obj.GetType().ToString());
							xmlTextWriter.WriteEndElement();
						}
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
		}
		catch (Exception e)
		{
			StiLogService.Write(typeof(StiReport), "Saving settings...ERROR");
			StiLogService.Write(typeof(StiReport), e);
		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}
	}

	public static string GetDefaultReportSettingsPath()
	{
		string text = DefaultReportSettingsPath;
		if (string.IsNullOrEmpty(text))
		{
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			text = "Stimulsoft" + directorySeparatorChar + "Stimulsoft.Report.settings";
			try
			{
				string folderPath;
				if (StiOptions.Engine.FullTrust)
				{
					folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
					if (folderPath.Length != 0)
					{
						text = Path.Combine(folderPath, text);
					}
				}
				folderPath = Path.GetDirectoryName(text);
				if (StiOptions.Engine.FullTrust && !string.IsNullOrWhiteSpace(folderPath) && !Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
			}
			catch
			{
				try
				{
					directorySeparatorChar = Path.DirectorySeparatorChar;
					text = "Stimulsoft" + directorySeparatorChar + "Stimulsoft.Report.settings";
					string folderPath = "Stimulsoft";
					if (StiOptions.Engine.FullTrust && !Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
				}
				catch
				{
					text = "Stimulsoft.Report.settings";
				}
			}
		}
		return text;
	}
}
