using System;
using Stimulsoft.Base;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Dashboard;

public class StiDataTransformationDesigner : StiComponentDesigner
{
	public static StiComponentDesigner GetTransformationDesigner(IStiDesignerBase designer, Type componentType)
	{
		if (!(componentType.GetCustomAttributes(typeof(StiDataTransformationDesignerAttribute), inherit: true) is StiDataTransformationDesignerAttribute[] array) || array.Length == 0)
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

	public StiDataTransformationDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
