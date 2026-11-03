using System;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Dictionary;

internal static class StiDataEditorsHelper
{
	internal static IStiDataEditors Get()
	{
		Type type;
		if (StiOptions.Configuration.IsWPF)
		{
			type = Type.GetType("Stimulsoft.Client.Designer.Dictionary.StiDataEditors, Stimulsoft.Client.Designer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type != null)
			{
				return StiActivator.CreateObject(type, new object[0]) as IStiDataEditors;
			}
		}
		type = Type.GetType("Stimulsoft.Report.Dictionary.Design.StiDataEditors, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		return StiActivator.CreateObject(type, new object[0]) as IStiDataEditors;
	}
}
