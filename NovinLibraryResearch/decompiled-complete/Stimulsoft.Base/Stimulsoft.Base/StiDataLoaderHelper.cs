using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base;

public static class StiDataLoaderHelper
{
	public class Data
	{
		public string Name { get; }

		public byte[] Array { get; }

		public List<Data> ToList()
		{
			return new List<Data> { this };
		}

		public Data(string name, byte[] array)
		{
			Name = name;
			Array = array;
		}
	}

	public static List<Data> LoadMultiple(string path, string filter)
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		if (Directory.Exists(path))
		{
			return (from f in Directory.EnumerateFiles(path, filter, SearchOption.TopDirectoryOnly)
				select new Data(Path.GetFileNameWithoutExtension(f), File.ReadAllBytes(f))).ToList();
		}
		return LoadSingle(path)?.ToList();
	}

	public static Data LoadSingle(string path, CookieContainer cookieContainer = null, NameValueCollection headers = null)
	{
		if (!string.IsNullOrEmpty(path) && !path.StartsWith("http://") && !path.StartsWith("https://") && File.Exists(path))
		{
			return new Data(Path.GetFileNameWithoutExtension(path), File.ReadAllBytes(path));
		}
		Uri uri = new Uri(path);
		return new Data(Path.GetFileNameWithoutExtension(uri.LocalPath), StiBytesFromURL.Load(path, cookieContainer, headers));
	}
}
