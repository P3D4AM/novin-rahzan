using System;
using Stimulsoft.Base;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Dashboard;

public class StiDataSortDesigner : StiComponentDesigner
{
	public static StiComponentDesigner GetDataSortDesigner(IStiDesignerBase designer, Type componentType)
	{
		if (!(componentType.GetCustomAttributes(typeof(StiDataSortDesignerAttribute), inherit: true) is StiDataSortDesignerAttribute[] array) || array.Length == 0)
		{
			return null;
		}
		string designerTypeName = array[0].DesignerTypeName;
		Type typeFromName = StiComponentDesigner.GetTypeFromName(designerTypeName);
		if (typeFromName == null)
		{
			return null;
		}
		return StiActivator.CreateObject(typeFromName, new object[1] { designer }) as StiComponentDesigner;
	}

	public StiDataSortDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
