using System;
using System.Collections.Generic;

namespace Stimulsoft.Base.Wizards;

public static class StiWizardSettings
{
	public static List<IStiWizardService> CurrentWizards { get; set; }

	public static IStiWizardData CurrentData { get; private set; }

	public static void Init()
	{
		try
		{
			Type type = Type.GetType("Stimulsoft.Report.Design.Wizards.StiWinFormsWizardData, Stimulsoft.Report.Design");
			if (type != null)
			{
				CurrentData = StiActivator.CreateObject(type) as IStiWizardData;
				if (CurrentData != null)
				{
					return;
				}
			}
		}
		catch
		{
		}
		try
		{
			Type type2 = Type.GetType("Stimulsoft.Client.Designer.Wizards.StiCloudWizardData, Stimulsoft.Client.Designer");
			if (type2 != null)
			{
				CurrentData = StiActivator.CreateObject(type2) as IStiWizardData;
				if (CurrentData != null)
				{
					return;
				}
			}
		}
		catch
		{
		}
		try
		{
			Type type3 = Type.GetType("Stimulsoft.Report.WpfDesign.Wizards.StiWpfWizardData, Stimulsoft.Report.WpfDesign");
			if (type3 != null)
			{
				CurrentData = StiActivator.CreateObject(type3) as IStiWizardData;
				_ = CurrentData;
			}
		}
		catch
		{
		}
	}
}
