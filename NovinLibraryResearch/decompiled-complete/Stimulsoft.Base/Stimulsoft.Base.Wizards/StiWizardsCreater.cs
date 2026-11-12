using System;

namespace Stimulsoft.Base.Wizards;

public class StiWizardsCreater
{
	internal static IStiWizardStartScreenControl GetWizardStartScreenControl(bool isWinFormsMode, object designer)
	{
		try
		{
			Type type = Type.GetType("Stimulsoft.Wizard.Wpf.Wizards.StiWizardStartScreenControl, Stimulsoft.Wizard.Wpf");
			if (type != null)
			{
				return StiActivator.CreateObject(type, new object[2] { isWinFormsMode, designer }) as IStiWizardStartScreenControl;
			}
		}
		catch
		{
			return null;
		}
		return null;
	}

	internal static IStiNewReportWizardControl GetNewReportWizardControl(bool isWinFormsMode, object designer, IStiReport baseReport)
	{
		try
		{
			Type type = Type.GetType("Stimulsoft.Wizard.Wpf.Wizards.StiNewReportWizardControl, Stimulsoft.Wizard.Wpf");
			if (type != null)
			{
				return StiActivator.CreateObject(type, new object[3] { isWinFormsMode, designer, baseReport }) as IStiNewReportWizardControl;
			}
		}
		catch
		{
			return null;
		}
		return null;
	}
}
