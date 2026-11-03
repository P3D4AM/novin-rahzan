using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Helpers;

public static class StiFormHelper
{
	public static void LoadStateForm(Form form, string key, ref Point position, ref Size size, bool allowMaximize = false)
	{
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		int primaryScreenNumber = GetPrimaryScreenNumber();
		if (string.IsNullOrEmpty(key))
		{
			key = ((object)form).GetType().Name;
		}
		int @int = StiSettings.GetInt(key, "ScreenNumber", primaryScreenNumber);
		position.X = StiScale.I(StiSettings.GetInt(key, "WindowXPos", 0));
		position.Y = StiScale.I(StiSettings.GetInt(key, "WindowYPos", 0));
		size.Width = StiScale.I(StiSettings.GetInt(key, "WindowWidth", size.Width));
		size.Height = StiScale.I(StiSettings.GetInt(key, "WindowHeight", size.Height));
		if (Screen.AllScreens.Length == 1)
		{
			position.X = Math.Max(0, position.X);
			position.Y = Math.Max(0, position.Y);
			position.X = Math.Min(Screen.PrimaryScreen.WorkingArea.Width, position.X);
			position.Y = Math.Min(Screen.PrimaryScreen.WorkingArea.Height, position.Y);
		}
		Screen val;
		if (@int != primaryScreenNumber)
		{
			if (@int + 1 > Screen.AllScreens.Length)
			{
				val = Screen.PrimaryScreen;
				position = val.Bounds.Location;
			}
			else
			{
				val = Screen.AllScreens[@int];
				if (!val.Bounds.Contains(new Rectangle(position, size)))
				{
					position = val.Bounds.Location;
				}
			}
		}
		else
		{
			val = Screen.PrimaryScreen;
			if (StiSettings.GetInt(key, "ScreenNumber", -1) == -1)
			{
				position = val.Bounds.Location;
			}
		}
		if (allowMaximize)
		{
			if (StiSettings.GetInt(key, "WindowXPos", int.MaxValue) == int.MaxValue && StiSettings.GetInt(key, "WindowYPos", int.MaxValue) == int.MaxValue)
			{
				Rectangle workingArea = val.WorkingArea;
				position = new Point(workingArea.Left + (workingArea.Width - size.Width) / 2, workingArea.Top + (workingArea.Height - size.Height) / 2);
			}
			form.WindowState = (FormWindowState)(StiSettings.GetBool(key, "IsMaximize", nullValue: true) ? 2 : 0);
		}
		else if ((int)form.WindowState == 0 && StiSettings.GetInt(key, "WindowXPos", int.MaxValue) == int.MaxValue && StiSettings.GetInt(key, "WindowYPos", int.MaxValue) == int.MaxValue)
		{
			Rectangle workingArea2 = val.WorkingArea;
			position = new Point(workingArea2.Left + (workingArea2.Width - size.Width) / 2, workingArea2.Top + (workingArea2.Height - size.Height) / 2);
		}
	}

	public static void LoadStateForm(Form form, string key, ref Point position, ref Size size, ref FormWindowState wndState, bool allowMaximize = false)
	{
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		StiSettings.Load();
		int primaryScreenNumber = GetPrimaryScreenNumber();
		if (string.IsNullOrEmpty(key))
		{
			key = ((object)form).GetType().Name;
		}
		int @int = StiSettings.GetInt(key, "ScreenNumber", primaryScreenNumber);
		position.X = StiScale.I(StiSettings.GetInt(key, "WindowXPos", 0));
		position.Y = StiScale.I(StiSettings.GetInt(key, "WindowYPos", 0));
		size.Width = StiScale.I(StiSettings.GetInt(key, "WindowWidth", size.Width));
		size.Height = StiScale.I(StiSettings.GetInt(key, "WindowHeight", size.Height));
		if (Screen.AllScreens.Length == 1)
		{
			position.X = Math.Max(0, position.X);
			position.Y = Math.Max(0, position.Y);
			position.X = Math.Min(Screen.PrimaryScreen.WorkingArea.Width, position.X);
			position.Y = Math.Min(Screen.PrimaryScreen.WorkingArea.Height, position.Y);
		}
		Screen val;
		if (@int != primaryScreenNumber)
		{
			if (@int + 1 > Screen.AllScreens.Length)
			{
				val = Screen.PrimaryScreen;
				position = val.Bounds.Location;
			}
			else
			{
				val = Screen.AllScreens[@int];
				if (!val.Bounds.Contains(new Rectangle(position, size)))
				{
					position = val.Bounds.Location;
				}
			}
		}
		else
		{
			val = Screen.PrimaryScreen;
			if (StiSettings.GetInt(key, "ScreenNumber", -1) == -1)
			{
				position = val.Bounds.Location;
			}
		}
		if (allowMaximize)
		{
			if (StiSettings.GetInt(key, "WindowXPos", int.MaxValue) == int.MaxValue && StiSettings.GetInt(key, "WindowYPos", int.MaxValue) == int.MaxValue)
			{
				Rectangle workingArea = val.WorkingArea;
				position = new Point(workingArea.Left + (workingArea.Width - size.Width) / 2, workingArea.Top + (workingArea.Height - size.Height) / 2);
			}
			wndState = (FormWindowState)(StiSettings.GetBool(key, "IsMaximize", nullValue: true) ? 2 : 0);
		}
		else if ((int)form.WindowState == 0 && StiSettings.GetInt(key, "WindowXPos", int.MaxValue) == int.MaxValue && StiSettings.GetInt(key, "WindowYPos", int.MaxValue) == int.MaxValue)
		{
			Rectangle workingArea2 = val.WorkingArea;
			position = new Point(workingArea2.Left + (workingArea2.Width - size.Width) / 2, workingArea2.Top + (workingArea2.Height - size.Height) / 2);
		}
	}

	public static void SaveStateForm(Form form, string key, bool allowMaximize = false)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		Point p;
		Size formSize;
		int screenNumber = GetScreenNumber(form, out p, out formSize);
		if (string.IsNullOrEmpty(key))
		{
			key = ((object)form).GetType().Name;
		}
		StiSettings.Load();
		if (allowMaximize)
		{
			StiSettings.Set(key, "IsMaximize", (int)form.WindowState == 2);
		}
		StiSettings.Set(key, "ScreenNumber", screenNumber);
		if ((int)form.WindowState != 1)
		{
			StiSettings.Set(key, "WindowXPos", (int)((double)p.X / StiScale.Factor));
			StiSettings.Set(key, "WindowYPos", (int)((double)p.Y / StiScale.Factor));
			StiSettings.Set(key, "WindowWidth", (int)((double)formSize.Width / StiScale.Factor));
			StiSettings.Set(key, "WindowHeight", (int)((double)formSize.Height / StiScale.Factor));
		}
		StiSettings.Save();
	}

	private static int GetScreenNumber(Form form, out Point p, out Size formSize)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)form.WindowState == 2)
		{
			p = form.RestoreBounds.Location;
			formSize = form.RestoreBounds.Size;
		}
		else
		{
			p = form.Location;
			formSize = form.Size;
		}
		if (Screen.AllScreens.Length > 1)
		{
			for (int i = 0; i < Screen.AllScreens.Length; i++)
			{
				Screen val = Screen.AllScreens[i];
				if (val.Bounds.IntersectsWith(new Rectangle(p, formSize)))
				{
					return i;
				}
			}
		}
		return GetPrimaryScreenNumber();
	}

	public static int GetPrimaryScreenNumber()
	{
		int i = 0;
		int result = 0;
		for (; i < Screen.AllScreens.Length; i++)
		{
			if (Screen.AllScreens[i].Primary)
			{
				result = i;
				break;
			}
		}
		return result;
	}
}
