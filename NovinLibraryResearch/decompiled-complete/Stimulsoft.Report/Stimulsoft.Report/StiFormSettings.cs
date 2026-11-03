using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report;

public class StiFormSettings
{
	public static void Save(Form form)
	{
		Save(((Control)form).Name, form);
	}

	public static void Save(string name, Form form)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		if (!StiOptions.Designer.DontSaveFormsSettings)
		{
			StiWindowState val = new StiWindowState
			{
				X = (int)((double)((Control)form).Left / StiScale.Factor),
				Y = (int)((double)((Control)form).Top / StiScale.Factor),
				Width = (int)((double)((Control)form).Width / StiScale.Factor),
				Height = (int)((double)((Control)form).Height / StiScale.Factor)
			};
			StiWindowsSettings.Set(name, val);
			StiWindowsSettings.Save();
		}
	}

	public static void Load(Form form, bool skipLocation = false, bool skipSize = false)
	{
		Load(((Control)form).Name, form, skipLocation, skipSize);
	}

	public static void Load(string name, Form form, bool skipLocation = false, bool skipSize = false)
	{
		if (StiOptions.Designer.DontSaveFormsSettings)
		{
			return;
		}
		StiWindowState val = StiWindowsSettings.Get(name);
		if (val == null)
		{
			return;
		}
		int val2 = StiScale.XXI((double)val.Width);
		int val3 = StiScale.YYI((double)val.Height);
		val2 = Math.Min(val2, Screen.FromControl((Control)(object)form).Bounds.Width);
		val3 = Math.Min(val3, Screen.FromControl((Control)(object)form).Bounds.Height);
		((ContainerControl)form).AutoScaleMode = (AutoScaleMode)0;
		if (!skipSize)
		{
			((Control)form).Width = val2;
			((Control)form).Height = val3;
		}
		if (!skipLocation && Screen.AllScreens.Length == 1)
		{
			int left = StiScale.XXI((double)val.X);
			int num = StiScale.YYI((double)val.Y);
			Screen primaryScreen = Screen.PrimaryScreen;
			if (primaryScreen.Bounds.IntersectsWith(new Rectangle(val.X, num, val2, val3)))
			{
				form.StartPosition = (FormStartPosition)0;
				((Control)form).Left = left;
				((Control)form).Top = num;
			}
		}
	}

	public static Size LoadSize(Form form)
	{
		if (StiOptions.Designer.DontSaveFormsSettings)
		{
			return form.Size;
		}
		StiWindowState val = StiWindowsSettings.Get(((Control)form).Name);
		if (val == null)
		{
			return form.Size;
		}
		int val2 = StiScale.XXI((double)val.Width);
		int val3 = StiScale.YYI((double)val.Height);
		val2 = Math.Min(val2, Screen.FromControl((Control)(object)form).Bounds.Width);
		val3 = Math.Min(val3, Screen.FromControl((Control)(object)form).Bounds.Height);
		return new Size(val2, val3);
	}
}
