using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Helpers;

public class StiOnlineMapRepaintHelper
{
	private class StiOnlineMapObject
	{
		public IStiAppComponent Comp { get; set; }

		public int Key { get; set; }

		public string Script { get; set; }

		public WebBrowser Browser { get; set; }

		public long Tick { get; set; }

		public string ElementKey { get; set; } = "";

	}

	private class StiExportData
	{
		public Graphics G { get; set; }

		public string Script { get; set; }

		public Size Size { get; set; }

		public Size OrigialSize { get; set; }

		public bool IsReady { get; set; }

		public RectangleD Location { get; set; }
	}

	public const int TimerInterval = 1200;

	public const int BrowserLifetime = 300000;

	private static Timer timer = new Timer();

	private static ConcurrentDictionary<IStiAppComponent, StiOnlineMapObject> componentBrowserHash = new ConcurrentDictionary<IStiAppComponent, StiOnlineMapObject>();

	public static event EventHandler Tick;

	private static void Timer_Tick(object sender, EventArgs e)
	{
		StiOnlineMapRepaintHelper.Tick?.Invoke(null, EventArgs.Empty);
	}

	public static void Init()
	{
		timer.Interval = 1200;
		timer.Tick += Timer_Tick;
		timer.Start();
	}

	public static WebBrowser Get(IStiAppComponent comp, int key, bool ignoreKey = false)
	{
		lock (componentBrowserHash)
		{
			if (componentBrowserHash.ContainsKey(comp) && (ignoreKey || componentBrowserHash[comp].Key == key))
			{
				componentBrowserHash[comp].Tick = Environment.TickCount;
				return componentBrowserHash[comp].Browser;
			}
			return null;
		}
	}

	public static void ExportImage(Graphics g, string script, Size size, Size origialSize, RectangleD location)
	{
		StiExportData stiExportData = new StiExportData
		{
			Script = script,
			Size = size,
			OrigialSize = origialSize,
			G = g,
			Location = location
		};
		Thread thread = new Thread(RenderExportImage);
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start(stiExportData);
		while (!stiExportData.IsReady)
		{
			Thread.Sleep(1000);
		}
	}

	private static void RenderExportImage(object exportData)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		StiExportData stiExportData = (StiExportData)exportData;
		try
		{
			WebBrowser val = new WebBrowser
			{
				Size = stiExportData.Size,
				ScrollBarsEnabled = false,
				ScriptErrorsSuppressed = true,
				DocumentText = stiExportData.Script
			};
			long ticks = DateTime.Now.Ticks;
			while (ticks + 300000000 > DateTime.Now.Ticks)
			{
				Application.DoEvents();
			}
			Bitmap val2 = new Bitmap(stiExportData.Size.Width, stiExportData.Size.Height);
			try
			{
				((WebBrowserBase)val).DrawToBitmap(val2, new Rectangle(0, 0, stiExportData.Size.Width, stiExportData.Size.Height));
				if (stiExportData.Size.Equals((object?)stiExportData.OrigialSize))
				{
					stiExportData.G.DrawImageUnscaled((Image)(object)val2, stiExportData.Location.ToRectangle());
				}
				else
				{
					stiExportData.G.DrawImage((Image)(object)val2, stiExportData.Location.ToRectangle());
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		catch
		{
		}
		finally
		{
			stiExportData.IsReady = true;
		}
	}

	public static WebBrowser Add(IStiAppComponent comp, int key, string script, Size size, string elementKey)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		lock (componentBrowserHash)
		{
			if (!componentBrowserHash.ContainsKey(comp) || componentBrowserHash[comp].Key != key)
			{
				if (componentBrowserHash.ContainsKey(comp))
				{
					((Component)(object)componentBrowserHash[comp].Browser).Dispose();
					componentBrowserHash.TryRemove(comp, out var _);
				}
				componentBrowserHash[comp] = new StiOnlineMapObject
				{
					Comp = comp,
					Browser = new WebBrowser
					{
						Size = size,
						ScrollBarsEnabled = false,
						ScriptErrorsSuppressed = true,
						DocumentText = script
					},
					Script = script,
					Key = key,
					ElementKey = elementKey
				};
			}
			componentBrowserHash[comp].Tick = Environment.TickCount;
			return componentBrowserHash[comp].Browser;
		}
	}

	public static List<IStiAppComponent> FetchAllComponents()
	{
		lock (componentBrowserHash)
		{
			if (componentBrowserHash.Count > 0)
			{
				List<IStiAppComponent> list = new List<IStiAppComponent>();
				foreach (IStiAppComponent key in componentBrowserHash.Keys)
				{
					if (componentBrowserHash[key].Tick + 300000 > Environment.TickCount)
					{
						list.Add(key);
						continue;
					}
					((Component)(object)componentBrowserHash[key].Browser).Dispose();
					componentBrowserHash.TryRemove(key, out var _);
				}
				return list;
			}
		}
		return null;
	}

	public static void Clean(string reportKey)
	{
		lock (componentBrowserHash)
		{
			if (reportKey == null)
			{
				componentBrowserHash.Clear();
				return;
			}
			componentBrowserHash.Keys.Where((IStiAppComponent k) => componentBrowserHash[k].ElementKey.StartsWith(reportKey)).ToList().ForEach(delegate(IStiAppComponent k)
			{
				componentBrowserHash.TryRemove(k, out var _);
			});
		}
	}

	public static void Dispose()
	{
		timer.Tick -= Timer_Tick;
	}
}
