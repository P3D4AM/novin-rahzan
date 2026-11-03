using System;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Gauge;

namespace Stimulsoft.Report;

internal static class StiStylesHelper
{
	internal static StiBorderSides GetBorderSidesFromLocation(StiComponent component)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		StiBorderSides val = (StiBorderSides)0;
		decimal num = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Left) / 10m, 0);
		decimal num2 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Top) / 10m, 0);
		decimal num3 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Parent.Width) / 10m, 0);
		decimal num4 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Parent.Height) / 10m, 0);
		decimal num5 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Left + component.Width) / 10m, 0);
		decimal num6 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Top + component.Height) / 10m, 0);
		if (num <= 0m)
		{
			val = (StiBorderSides)(val | 2);
		}
		if (num2 <= 0m)
		{
			val = (StiBorderSides)(val | 1);
		}
		if (num5 >= num3)
		{
			val = (StiBorderSides)(val | 4);
		}
		if (num6 >= num4)
		{
			val = (StiBorderSides)(val | 8);
		}
		return val;
	}

	internal static void ChangeComponentStyleName(StiComponent comp, string oldName, string newName)
	{
		if (comp.ComponentStyle == oldName)
		{
			comp.ComponentStyle = newName;
		}
		if (comp.Conditions != null && comp.Conditions.Count > 0)
		{
			for (int i = 0; i < comp.Conditions.Count; i++)
			{
				if (comp.Conditions[i] is StiCondition stiCondition && stiCondition.Style == oldName)
				{
					stiCondition.Style = newName;
				}
			}
		}
		ChangeDataBandStyleName(comp, oldName, newName);
		ChangeElementStyleName(comp, oldName, newName);
		ChangeChartStyleName(comp, oldName, newName);
		ChangeGaugeStyleName(comp, oldName, newName);
	}

	private static void ChangeDataBandStyleName(StiComponent comp, string oldName, string newName)
	{
		if (comp is StiDataBand stiDataBand)
		{
			if (stiDataBand.EvenStyle == oldName)
			{
				stiDataBand.EvenStyle = newName;
			}
			if (stiDataBand.OddStyle == oldName)
			{
				stiDataBand.OddStyle = newName;
			}
		}
	}

	private static void ChangeElementStyleName(StiComponent comp, string oldName, string newName)
	{
		if (comp is IStiDashboardElementStyle stiDashboardElementStyle && !(stiDashboardElementStyle.CustomStyleName != oldName))
		{
			stiDashboardElementStyle.CustomStyleName = newName;
		}
	}

	private static void ChangeChartStyleName(StiComponent comp, string oldName, string newName)
	{
		if (!(comp is StiChart stiChart) || stiChart.CustomStyleName != oldName)
		{
			return;
		}
		stiChart.CustomStyleName = newName;
		if (stiChart.Style is StiCustomStyle stiCustomStyle)
		{
			stiCustomStyle.Name = newName;
			if (stiCustomStyle.Core is StiCustomStyleCoreXF stiCustomStyleCoreXF)
			{
				stiCustomStyleCoreXF.ReportStyleName = newName;
			}
		}
	}

	private static void ChangeGaugeStyleName(StiComponent comp, string oldName, string newName)
	{
		if (comp is StiGauge stiGauge && !(stiGauge.CustomStyleName != oldName))
		{
			stiGauge.CustomStyleName = newName;
		}
	}
}
