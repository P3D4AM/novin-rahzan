using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using Stimulsoft.Base.Crypto;

namespace Stimulsoft.Base.Drawing;

public class FontV : IDisposable
{
	private IntPtr fontData = IntPtr.Zero;

	private int fontDataLength;

	private PrivateFontCollection fontsCollection;

	public Font Font { get; }

	public string Name { get; set; }

	public string CacheFile { get; }

	public void Dispose()
	{
		if (Font != null)
		{
			Font.Dispose();
		}
		if (fontsCollection != null)
		{
			((FontCollection)fontsCollection).Dispose();
		}
		if (fontData != IntPtr.Zero)
		{
			Marshal.FreeCoTaskMem(fontData);
		}
		if (!string.IsNullOrWhiteSpace(CacheFile))
		{
			try
			{
				File.Delete(CacheFile);
			}
			catch
			{
			}
		}
	}

	private static string CreateTempFile(byte[] content, string extension, string dataHash)
	{
		try
		{
			string text = StiFontCollection.DefaultCachePath;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				text = Path.Combine(text, "Stimulsoft", "ReportsResources");
			}
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string path = dataHash + "." + extension.ToLowerInvariant();
			text = Path.Combine(text, path);
			if (!File.Exists(text))
			{
				File.WriteAllBytes(text, content);
			}
			return text;
		}
		catch
		{
			StiFontCollection.AllowFileCache = false;
		}
		return null;
	}

	internal static string GetHashName(byte[] content)
	{
		long num = Crc32.Calculate(content);
		return $"{num:X8}-{content.Length}";
	}

	public FontV(string name, byte[] content, string extension, bool allowFileCache, string hash = null)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		if (content == null)
		{
			return;
		}
		fontDataLength = content.Length;
		Name = name;
		if (hash == null)
		{
			hash = GetHashName(content);
		}
		fontsCollection = new PrivateFontCollection();
		if (allowFileCache)
		{
			CacheFile = CreateTempFile(content, extension, hash);
			if (!string.IsNullOrEmpty(CacheFile))
			{
				try
				{
					fontsCollection.AddFontFile(CacheFile);
				}
				catch
				{
					CacheFile = null;
				}
			}
		}
		if (string.IsNullOrEmpty(CacheFile))
		{
			fontData = Marshal.AllocCoTaskMem(content.Length);
			Marshal.Copy(content, 0, fontData, content.Length);
			fontsCollection.AddMemoryFont(fontData, content.Length);
		}
		if (((FontCollection)fontsCollection).Families.Length != 0)
		{
			Font = new Font(((FontCollection)fontsCollection).Families[0], 32f);
		}
		else
		{
			Font = SystemFonts.DefaultFont;
		}
	}
}
