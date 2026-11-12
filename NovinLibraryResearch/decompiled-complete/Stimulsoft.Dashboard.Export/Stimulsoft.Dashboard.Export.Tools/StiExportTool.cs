using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Export.Tools;

public abstract class StiExportTool
{
	private static Hashtable typeToTool = new Hashtable();

	private static object lockObject = new object();

	public abstract void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings);

	public static StiExportTool GetTool(Type type)
	{
		lock (lockObject)
		{
			if (typeToTool[type] is StiExportTool result)
			{
				return result;
			}
			string text = ((type.GetCustomAttributes(typeof(StiExportToolAttribute), inherit: true).FirstOrDefault() is StiExportToolAttribute stiExportToolAttribute) ? stiExportToolAttribute.ExportToolTypeName : null);
			if (text == null)
			{
				return null;
			}
			StiExportTool stiExportTool = StiActivator.CreateObject(text, new object[0]) as StiExportTool;
			typeToTool[type] = stiExportTool;
			return stiExportTool;
		}
	}
}
