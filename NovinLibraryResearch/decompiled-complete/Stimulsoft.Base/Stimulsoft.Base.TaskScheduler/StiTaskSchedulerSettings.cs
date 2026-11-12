using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Base.TaskScheduler;

public static class StiTaskSchedulerSettings
{
	private static MethodInfo runMethod;

	private static Type winFormsDbsHtmlEditor;

	public static IStiTaskSchedulerBridge Current { get; set; }

	public static bool AllowHtmlEditor { get; private set; }

	public static bool TryToLoadTaskShedulerAssembly()
	{
		try
		{
			string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Stimulsoft.TaskScheduler.dll");
			if (!File.Exists(path))
			{
				return false;
			}
			Assembly assembly = Assembly.LoadFile(path);
			if (assembly == null)
			{
				return false;
			}
			Type type = assembly.GetType("Stimulsoft.TaskScheduler.StiSchedulerBridge");
			if (type == null)
			{
				return false;
			}
			type.GetMethod("Init", BindingFlags.Static | BindingFlags.Public)?.Invoke(null, null);
		}
		catch
		{
			return false;
		}
		try
		{
			winFormsDbsHtmlEditor = Type.GetType("Stimulsoft.Dashboard.Design.Editors.StiHtmlEditorForm, Stimulsoft.Dashboard.Design");
			if (winFormsDbsHtmlEditor != null)
			{
				AllowHtmlEditor = true;
				return true;
			}
		}
		catch
		{
		}
		return true;
	}

	public static void Run(string key)
	{
		try
		{
			if (runMethod == null)
			{
				Type type = Type.GetType("Stimulsoft.TaskScheduler.StiSchedulerRunner, Stimulsoft.TaskScheduler");
				if (type == null)
				{
					return;
				}
				MethodInfo method = type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					return;
				}
				runMethod = method;
			}
			runMethod.Invoke(null, new object[1] { key });
		}
		catch
		{
		}
	}

	public static void RunLogViewer(string logFileName)
	{
		try
		{
			if (runMethod == null)
			{
				Type type = Type.GetType("Stimulsoft.TaskScheduler.StiSchedulerRunner, Stimulsoft.TaskScheduler");
				if (type == null)
				{
					return;
				}
				MethodInfo method = type.GetMethod("RunLogViewer", BindingFlags.Static | BindingFlags.Public);
				if (method == null)
				{
					return;
				}
				runMethod = method;
			}
			runMethod.Invoke(null, new object[1] { logFileName });
		}
		catch
		{
		}
	}

	public static bool EditHtmlText(ref string text, IntPtr handle)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		if (winFormsDbsHtmlEditor != null)
		{
			try
			{
				object obj = StiActivator.CreateObject(winFormsDbsHtmlEditor, new object[2] { text, null });
				Form val = (Form)((obj is Form) ? obj : null);
				if (val != null)
				{
					NativeWindow val2 = new NativeWindow();
					val2.AssignHandle(handle);
					if ((int)val.ShowDialog((IWin32Window)(object)val2) == 1)
					{
						PropertyInfo property = winFormsDbsHtmlEditor.GetProperty("HtmlText", BindingFlags.Instance | BindingFlags.NonPublic);
						if (property != null)
						{
							text = property.GetValue(val) as string;
						}
						return true;
					}
				}
			}
			catch
			{
			}
		}
		return false;
	}
}
