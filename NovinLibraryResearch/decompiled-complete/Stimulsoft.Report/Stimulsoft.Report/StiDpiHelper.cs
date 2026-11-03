using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report;

[SuppressUnmanagedCodeSecurity]
public sealed class StiDpiHelper
{
	private const short LOGPIXELSX = 88;

	private const short LOGPIXELSY = 90;

	private static int deviceCapsDpi;

	private static int graphicsDpi;

	private static int graphicsRichTextDpi;

	private static bool? fontScaling;

	private static bool? isWindows;

	private static bool? isLinux;

	private static bool? isMacOsX;

	public static int DeviceCapsDpi
	{
		get
		{
			if (deviceCapsDpi == 0)
			{
				if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
				{
					GetDpi();
				}
				else
				{
					deviceCapsDpi = 96;
					graphicsDpi = 96;
					graphicsRichTextDpi = 96;
					fontScaling = false;
				}
			}
			return deviceCapsDpi;
		}
	}

	public static int GraphicsDpi
	{
		get
		{
			if (graphicsDpi == 0)
			{
				if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
				{
					GetDpi();
				}
				else
				{
					deviceCapsDpi = 96;
					graphicsDpi = 96;
					graphicsRichTextDpi = 96;
					fontScaling = false;
				}
			}
			return graphicsDpi;
		}
	}

	public static int GraphicsRichTextDpi
	{
		get
		{
			if (graphicsRichTextDpi == 0)
			{
				if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
				{
					GetDpi();
				}
				else
				{
					deviceCapsDpi = 96;
					graphicsDpi = 96;
					graphicsRichTextDpi = 96;
					fontScaling = false;
				}
			}
			return graphicsRichTextDpi;
		}
	}

	public static double DeviceCapsScale
	{
		get
		{
			if (StiOptions.Engine.DpiAware)
			{
				int num = DeviceCapsDpi;
				if (num != 96)
				{
					return 96.0 / (double)num;
				}
			}
			return 1.0;
		}
	}

	public static double GraphicsScale
	{
		get
		{
			if (StiOptions.Engine.DpiAware)
			{
				int num = GraphicsDpi;
				if (num != 96)
				{
					return 96.0 / (double)num;
				}
			}
			return 1.0;
		}
	}

	public static double GraphicsRichTextScale
	{
		get
		{
			if (StiOptions.Engine.DpiAware)
			{
				int num = GraphicsRichTextDpi;
				if (num != 96)
				{
					return 96.0 / (double)num;
				}
			}
			return 1.0;
		}
	}

	public static bool NeedDeviceCapsScale => DeviceCapsDpi != 96;

	public static bool NeedGraphicsScale => GraphicsDpi != 96;

	public static bool NeedGraphicsRichTextScale => GraphicsRichTextDpi != 96;

	public static bool NeedFontScaling
	{
		get
		{
			if (!StiOptions.Engine.DpiAware)
			{
				return false;
			}
			if (!fontScaling.HasValue)
			{
				if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
				{
					GetDpi();
				}
				else
				{
					deviceCapsDpi = 96;
					graphicsDpi = 96;
					graphicsRichTextDpi = 96;
					fontScaling = false;
				}
			}
			return fontScaling.GetValueOrDefault(false);
		}
	}

	public static bool IsWindows
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			if (!isWindows.HasValue)
			{
				try
				{
					Graphics val = Graphics.FromImage((Image)new Bitmap(1, 1));
					try
					{
						IntPtr hdc = val.GetHdc();
						int deviceCaps = GetDeviceCaps(hdc, 88);
						val.ReleaseHdc();
						isWindows = true;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				catch
				{
					isWindows = false;
				}
			}
			return isWindows.GetValueOrDefault();
		}
	}

	public static bool IsLinux
	{
		get
		{
			if (!isLinux.HasValue)
			{
				try
				{
					if (File.Exists("/proc/sys/kernel/ostype"))
					{
						string text = File.ReadAllText("/proc/sys/kernel/ostype");
						if (text.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
						{
							isLinux = true;
						}
					}
				}
				catch
				{
					isLinux = false;
				}
			}
			return isLinux.GetValueOrDefault();
		}
	}

	public static bool IsMacOsX
	{
		get
		{
			if (!isMacOsX.HasValue)
			{
				try
				{
					if (File.Exists("/System/Library/CoreServices/SystemVersion.plist"))
					{
						isMacOsX = true;
					}
				}
				catch
				{
					isMacOsX = false;
				}
			}
			return isMacOsX.GetValueOrDefault();
		}
	}

	[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
	private static extern int GetDeviceCaps(IntPtr hDc, int nIndex);

	public static void Reset(int deviceCapsDpi = 0, int graphicsDpi = 0, int graphicsRichTextDpi = 0, bool? fontScaling = null)
	{
		StiDpiHelper.deviceCapsDpi = deviceCapsDpi;
		StiDpiHelper.graphicsDpi = graphicsDpi;
		StiDpiHelper.graphicsRichTextDpi = graphicsRichTextDpi;
		StiDpiHelper.fontScaling = fontScaling;
	}

	public static void CheckWysiwygScaling()
	{
		StiTextRenderer.StiDpiHelperGraphicsScale = (NeedFontScaling ? GraphicsScale : 1.0);
	}

	private static void GetDpi()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		try
		{
			Graphics val = Graphics.FromImage((Image)new Bitmap(1, 1));
			try
			{
				val.PageUnit = (GraphicsUnit)2;
				val.PageScale = 1f;
				IntPtr hdc = val.GetHdc();
				deviceCapsDpi = GetDeviceCaps(hdc, 88);
				val.ReleaseHdc();
				graphicsDpi = (int)val.DpiX;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
			deviceCapsDpi = 96;
			graphicsDpi = 96;
		}
		finally
		{
			if (deviceCapsDpi < 70 || deviceCapsDpi > 600)
			{
				deviceCapsDpi = 96;
			}
			if (graphicsDpi < 70 || graphicsDpi > 600)
			{
				graphicsDpi = 96;
			}
			graphicsRichTextDpi = graphicsDpi;
		}
		if (deviceCapsDpi == 96 && graphicsDpi == 96)
		{
			int registryValue = GetRegistryValue();
			if (registryValue != 96 && registryValue > 32 && registryValue < 600)
			{
				graphicsRichTextDpi = registryValue;
			}
		}
		if (graphicsRichTextDpi == 96)
		{
			int metafileDpi = GetMetafileDpi();
			if (metafileDpi != 96 && metafileDpi > 32 && metafileDpi < 600)
			{
				graphicsRichTextDpi = metafileDpi;
			}
		}
		fontScaling = false;
		if (graphicsDpi != 96)
		{
			Font val2 = new Font("Arial", 1000f, (GraphicsUnit)2);
			fontScaling = (double)Math.Abs((val2.SizeInPoints - 750f) / val2.SizeInPoints) > 0.01;
		}
	}

	private static int GetMetafileDpi()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			Graphics val = Graphics.FromImage((Image)new Bitmap(1, 1));
			try
			{
				IntPtr hdc = val.GetHdc();
				try
				{
					GraphicsUnit val2 = (GraphicsUnit)2;
					Metafile val3 = new Metafile(hdc, StiOptions.Engine.RichTextDrawingMetafileType);
					Graphics val4 = Graphics.FromImage((Image)(object)val3);
					try
					{
						val4.DrawLine(Pens.Black, 0, 0, 10, 10);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					RectangleF bounds = ((Image)val3).GetBounds(ref val2);
					MemoryStream memoryStream = new MemoryStream();
					StiMetafileSaver.Save((Stream)memoryStream, val3);
					((Image)val3).Dispose();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					Image obj = Image.FromStream((Stream)memoryStream);
					Metafile val5 = (Metafile)(object)((obj is Metafile) ? obj : null);
					RectangleF bounds2 = ((Image)val5).GetBounds(ref val2);
					((Image)val5).Dispose();
					return (int)Math.Round(bounds2.Width / bounds.Width * 4f, 0) * 24;
				}
				catch
				{
				}
				finally
				{
					val.ReleaseHdc(hdc);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch
		{
		}
		return 0;
	}

	private static int GetRegistryValue()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop");
			object obj = null;
			if (registryKey != null)
			{
				obj = registryKey.GetValue("Win8DpiScaling");
				if (obj != null)
				{
					int num = Convert.ToInt32(obj);
					if (num == 1)
					{
						obj = registryKey.GetValue("LogPixels");
						if (obj != null)
						{
							int num2 = Convert.ToInt32(obj);
							if (num2 != 96)
							{
								return num2;
							}
						}
					}
				}
			}
			RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop\\WindowMetrics");
			if (registryKey2 != null)
			{
				obj = registryKey2.GetValue("AppliedDPI");
				if (obj != null)
				{
					int num3 = Convert.ToInt32(obj);
					if (num3 != 96)
					{
						return num3;
					}
				}
			}
			registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\FontDPI");
			if (registryKey != null)
			{
				obj = registryKey.GetValue("LogPixels");
				if (obj != null)
				{
					int num4 = Convert.ToInt32(obj);
					if (num4 != 96)
					{
						return num4;
					}
				}
			}
		}
		catch
		{
		}
		return 0;
	}
}
