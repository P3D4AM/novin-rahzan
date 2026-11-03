using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Surface.Editor;

namespace Stimulsoft.Report.App;

internal static class StiAppDesignAssembly
{
	private static object lockObject = new object();

	private static bool isInited;

	private static Assembly assembly;

	internal static bool IsAssemblyLoaded => Assembly != null;

	internal static Assembly Assembly
	{
		get
		{
			LoadAssembly();
			return assembly;
		}
		set
		{
			assembly = value;
		}
	}

	internal static void LoadAssembly()
	{
		if (isInited)
		{
			return;
		}
		lock (lockObject)
		{
			try
			{
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.App.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static IStiUnplacedElementsForm GetUnplacedComponentsUIFormFactory(List<IStiComponentUI> elements)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.App.Design.Editors.StiUnplacedComponentsForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new List<IStiComponentUI>[1] { elements };
			return StiActivator.CreateObject(type, array) as IStiUnplacedElementsForm;
		}
		catch
		{
		}
		return null;
	}

	internal static Form GetWatermarkFormFactory(IStiWatermarkUI watermark)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.App.Design.Editors.StiWatermarkEditorForm");
			if (type == null)
			{
				return null;
			}
			object obj = StiActivator.CreateObject(type, new object[2] { watermark.WatermarkUI, watermark });
			return (Form)((obj is Form) ? obj : null);
		}
		catch
		{
		}
		return null;
	}
}
