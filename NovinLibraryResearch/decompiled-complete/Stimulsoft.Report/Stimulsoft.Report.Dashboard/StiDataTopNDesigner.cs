using System;
using Stimulsoft.Base;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Dashboard;

public class StiDataTopNDesigner : StiComponentDesigner
{
	public static StiComponentDesigner GetDesigner(IStiDesignerBase designer, Type componentType)
	{
		if (!(componentType.GetCustomAttributes(typeof(StiDataTopNDesignerAttribute), inherit: true) is StiDataTopNDesignerAttribute[] array) || array.Length == 0)
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

	public StiDataTopNDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
