using System;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using Stimulsoft.Report;

namespace Stimulsoft.BlocklyEditor.Wpf.Helper;

public class StiWindowHelper
{
	public static void LoadStateWindow(Window window)
	{
		LoadStateWindow(window, Size.Empty, setSize: true, loadSettings: true);
	}

	public static void LoadStateWindow(Window window, Size defaultSize, bool setSize, bool loadSettings)
	{
		LoadStateWindow(window, ((object)window).GetType().Name, defaultSize, setSize, loadSettings);
	}

	public static void LoadStateWindow(Window window, string key, Size defaultSize, bool setSize, bool loadSettings)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		if (loadSettings)
		{
			StiSettings.Load();
		}
		int @int = StiSettings.GetInt(key, "WindowState", -1);
		if (@int == -1)
		{
			return;
		}
		WindowState val = (WindowState)@int;
		if ((int)val == 2)
		{
			((FrameworkElement)window).Width = defaultSize.Width;
			((FrameworkElement)window).Height = defaultSize.Height;
			DispatcherTimer val2 = new DispatcherTimer
			{
				Tag = window,
				Interval = TimeSpan.FromMilliseconds(100.0)
			};
			val2.Tick += OnMaximizedTimerTick;
			val2.Start();
			((UIElement)window).Opacity = 0.0;
		}
		else
		{
			window.WindowState = (WindowState)0;
			if (setSize)
			{
				((FrameworkElement)window).Width = StiSettings.GetDouble(key, "WindowWidth", defaultSize.Width);
				((FrameworkElement)window).Height = StiSettings.GetDouble(key, "WindowHeight", defaultSize.Height);
			}
		}
	}

	private static void OnMaximizedTimerTick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		DispatcherTimer val = (DispatcherTimer)sender;
		val.Stop();
		Window val2 = (Window)val.Tag;
		val2.WindowState = (WindowState)2;
		((UIElement)val2).Opacity = 1.0;
	}

	public static void SaveStateWindow(Window window)
	{
		SaveStateWindow(window, Size.Empty, saveSize: true);
	}

	public static void SaveStateWindow(Window window, Size defaultSize, bool saveSize)
	{
		SaveStateWindow(window, ((object)window).GetType().Name, defaultSize, saveSize);
	}

	public static void SaveStateWindow(Window window, string key, Size defaultSize, bool saveSize)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected I4, but got Unknown
		StiSettings.Load();
		StiSettings.Set(key, "WindowState", (int)window.WindowState);
		double num = ((FrameworkElement)window).ActualWidth;
		if (double.IsNaN(num))
		{
			num = defaultSize.Width;
		}
		StiSettings.Set(key, "WindowWidth", num);
		num = ((FrameworkElement)window).ActualHeight;
		if (double.IsNaN(num))
		{
			num = defaultSize.Height;
		}
		StiSettings.Set(key, "WindowHeight", num);
		StiSettings.Save();
	}
}
