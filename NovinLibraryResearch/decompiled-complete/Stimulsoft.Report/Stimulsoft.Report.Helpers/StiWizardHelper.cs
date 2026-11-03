using System;
using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Report.Helpers;

public static class StiWizardHelper
{
	public static void InitServices()
	{
		try
		{
			Type type = StiTypeFinder.GetType("Stimulsoft.Wizard.Wpf.Services.StiWizardService, Stimulsoft.Wizard.Wpf");
			if (!(type == null))
			{
				MethodInfo method = type.GetMethod("Init", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}
		catch
		{
		}
	}
}
