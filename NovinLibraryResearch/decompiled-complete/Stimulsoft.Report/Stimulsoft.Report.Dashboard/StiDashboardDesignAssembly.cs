using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Dashboard.Editor;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Surface.Editor;

namespace Stimulsoft.Report.Dashboard;

internal static class StiDashboardDesignAssembly
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
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static IStiHtmlTextEditorControl GetHtmlTextEditor()
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Controls.StiHtmlTextEditorControl");
			if (type == null)
			{
				return null;
			}
			return StiActivator.CreateObject(type) as IStiHtmlTextEditorControl;
		}
		catch
		{
		}
		return null;
	}

	internal static IStiElementConditionsEditorForm GetElementConditionsEditorFormFactory(IStiElement element)
	{
		IStiElementConditionsEditorForm result = null;
		if (element is IStiChartElement)
		{
			result = GetChartElementConditionsEditorForm((IStiChartElement)element);
		}
		if (element is IStiIndicatorElement)
		{
			result = GetIndicatorElementConditionsEditorForm((IStiIndicatorElement)element);
		}
		if (element is IStiPivotTableElement)
		{
			result = GetPivotTableElementConditionsEditorForm((IStiPivotTableElement)element);
		}
		if (element is IStiProgressElement)
		{
			result = GetProgressElementConditionsEditorForm((IStiProgressElement)element);
		}
		if (element is IStiTableElement)
		{
			result = GetTableElementConditionsEditorForm((IStiTableElement)element);
		}
		return result;
	}

	internal static IStiUnplacedElementsForm GetUnplacedElementsFormFactory(List<IStiElement> elements)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.StiUnplacedElementsForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new List<IStiElement>[1] { elements };
			return StiActivator.CreateObject(type, array) as IStiUnplacedElementsForm;
		}
		catch
		{
		}
		return null;
	}

	internal static IStiElementConditionsEditorForm GetIndicatorElementConditionsEditorForm(IStiIndicatorElement indicatorElement)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.Indicator.StiIndicatorElementConditionsEditorForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new IStiIndicatorElement[1] { indicatorElement };
			return StiActivator.CreateObject(type, array) as IStiElementConditionsEditorForm;
		}
		catch
		{
		}
		return null;
	}

	internal static IStiElementConditionsEditorForm GetChartElementConditionsEditorForm(IStiChartElement chartElement)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.Chart.StiChartElementConditionsEditorForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new IStiChartElement[1] { chartElement };
			return StiActivator.CreateObject(type, array) as IStiElementConditionsEditorForm;
		}
		catch
		{
		}
		return null;
	}

	internal static IStiElementConditionsEditorForm GetPivotTableElementConditionsEditorForm(IStiPivotTableElement pivotTableElement)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.PivotTable.StiPivotTableElementConditionsEditorForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new IStiPivotTableElement[1] { pivotTableElement };
			return StiActivator.CreateObject(type, array) as IStiElementConditionsEditorForm;
		}
		catch
		{
		}
		return null;
	}

	internal static IStiElementConditionsEditorForm GetProgressElementConditionsEditorForm(IStiProgressElement progressElement)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.Progress.StiProgressElementConditionsEditorForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new IStiProgressElement[1] { progressElement };
			return StiActivator.CreateObject(type, array) as IStiElementConditionsEditorForm;
		}
		catch
		{
		}
		return null;
	}

	private static IStiElementConditionsEditorForm GetTableElementConditionsEditorForm(IStiTableElement element)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.Table.StiTableElementConditionsEditorForm");
			if (type == null)
			{
				return null;
			}
			object[] array = new IStiTableElement[1] { element };
			return StiActivator.CreateObject(type, array) as IStiElementConditionsEditorForm;
		}
		catch
		{
		}
		return null;
	}

	internal static Form GetWatermarkFormFactory(IStiDashboardWatermark watermark)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.Dashboard.Design.Editors.StiWatermarkEditorForm");
			if (type == null)
			{
				return null;
			}
			object obj = StiActivator.CreateObject(type, new object[2] { watermark.DashboardWatermark, watermark });
			return (Form)((obj is Form) ? obj : null);
		}
		catch
		{
		}
		return null;
	}
}
