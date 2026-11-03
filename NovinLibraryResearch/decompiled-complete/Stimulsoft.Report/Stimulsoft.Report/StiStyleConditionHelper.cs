using System;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.CrossTab;

namespace Stimulsoft.Report;

internal static class StiStyleConditionHelper
{
	public static bool IsAllowStyle(StiComponent component, StiBaseStyle style)
	{
		foreach (StiStyleCondition condition in style.Conditions)
		{
			bool flag = true;
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			if ((condition.Type & StiStyleConditionType.ComponentName) > (StiStyleConditionType)0)
			{
				switch (condition.OperationComponentName)
				{
				case StiStyleConditionOperation.EqualTo:
					flag = component.Name == condition.ComponentName;
					break;
				case StiStyleConditionOperation.NotEqualTo:
					flag = component.Name != condition.ComponentName;
					break;
				case StiStyleConditionOperation.BeginningWith:
					flag = component.Name != null && component.Name.StartsWith(condition.ComponentName);
					break;
				case StiStyleConditionOperation.EndingWith:
					flag = component.Name != null && component.Name.EndsWith(condition.ComponentName);
					break;
				case StiStyleConditionOperation.Containing:
					flag = component.Name != null && component.Name.Contains(condition.ComponentName);
					break;
				case StiStyleConditionOperation.NotContaining:
					flag = component.Name != null && !component.Name.Contains(condition.ComponentName);
					break;
				}
			}
			if ((condition.Type & StiStyleConditionType.ComponentType) > (StiStyleConditionType)0)
			{
				flag2 = false;
				if ((condition.ComponentType & StiStyleComponentType.Chart) > (StiStyleComponentType)0 && component is StiChart)
				{
					flag2 = true;
				}
				if ((condition.ComponentType & StiStyleComponentType.CrossTab) > (StiStyleComponentType)0 && (component is StiCrossTab || component is IStiCrossTabField))
				{
					flag2 = true;
				}
				if ((condition.ComponentType & StiStyleComponentType.Image) > (StiStyleComponentType)0 && component is StiImage)
				{
					flag2 = true;
				}
				if ((condition.ComponentType & StiStyleComponentType.Text) > (StiStyleComponentType)0 && component is StiSimpleText)
				{
					flag2 = true;
				}
				if ((condition.ComponentType & StiStyleComponentType.Primitive) > (StiStyleComponentType)0 && (component is StiShape || component is StiPrimitive))
				{
					flag2 = true;
				}
				if ((condition.ComponentType & StiStyleComponentType.CheckBox) > (StiStyleComponentType)0 && component is StiCheckBox)
				{
					flag2 = true;
				}
				if (condition.OperationComponentType == StiStyleConditionOperation.NotEqualTo)
				{
					flag2 = !flag2;
				}
			}
			if ((condition.Type & StiStyleConditionType.Location) > (StiStyleConditionType)0 && component.Page != null && component.Parent != null)
			{
				decimal num = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Left) / 10m, 0);
				decimal num2 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Top) / 10m, 0);
				decimal num3 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Width) / 10m, 0);
				decimal num4 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Height) / 10m, 0);
				decimal num5 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Parent.Width) / 10m, 0);
				decimal num6 = Math.Round((decimal)component.Page.Unit.ConvertToHInches(component.Parent.Height) / 10m, 0);
				decimal num7 = num + num3;
				decimal num8 = num2 + num4;
				flag4 = false;
				if ((condition.Location & StiStyleLocation.Left) > StiStyleLocation.None && num2 <= 0m && num <= 0m && num4 == num6)
				{
					flag4 = true;
				}
				if ((condition.Location & StiStyleLocation.Right) > StiStyleLocation.None && num2 <= 0m && num7 >= num5 && num4 == num6)
				{
					flag4 = true;
				}
				if ((condition.Location & StiStyleLocation.Top) > StiStyleLocation.None && num <= 0m && num2 <= 0m && num3 == num5)
				{
					flag4 = true;
				}
				if ((condition.Location & StiStyleLocation.Bottom) > StiStyleLocation.None && num <= 0m && num8 >= num6 && num3 == num5)
				{
					flag4 = true;
				}
				if ((condition.Location & StiStyleLocation.CenterHorizontal) > StiStyleLocation.None && num2 <= 0m && num > 0m && num7 < num5 && num4 == num6)
				{
					flag4 = true;
				}
				if ((condition.Location & StiStyleLocation.CenterVertical) > StiStyleLocation.None && num <= 0m && num2 > 0m && num8 < num6 && num3 == num5)
				{
					flag4 = true;
				}
				if (!flag4)
				{
					if ((condition.Location & StiStyleLocation.TopLeft) > StiStyleLocation.None && num2 <= 0m && num <= 0m)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.TopCenter) > StiStyleLocation.None && num2 <= 0m && num > 0m && num7 < num5)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.TopRight) > StiStyleLocation.None && num2 <= 0m && num7 >= num5)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.MiddleLeft) > StiStyleLocation.None && num2 > 0m && num8 < num6 && num <= 0m)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.MiddleCenter) > StiStyleLocation.None && num2 > 0m && num8 < num6 && num > 0m && num7 < num5)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.MiddleRight) > StiStyleLocation.None && num2 > 0m && num8 < num6 && num7 >= num5)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.BottomLeft) > StiStyleLocation.None && num8 >= num6 && num <= 0m)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.BottomCenter) > StiStyleLocation.None && num8 >= num6 && num > 0m && num7 < num5)
					{
						flag4 = true;
					}
					if ((condition.Location & StiStyleLocation.BottomRight) > StiStyleLocation.None && num8 >= num6 && num7 >= num5)
					{
						flag4 = true;
					}
				}
				if (condition.OperationPlacement == StiStyleConditionOperation.NotEqualTo)
				{
					flag4 = !flag4;
				}
			}
			if ((condition.Type & StiStyleConditionType.Placement) > (StiStyleConditionType)0)
			{
				flag3 = false;
				if ((condition.Placement & StiStyleComponentPlacement.ReportTitle) > StiStyleComponentPlacement.None && component.Parent is StiReportTitleBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.ReportSummary) > StiStyleComponentPlacement.None && component.Parent is StiReportSummaryBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.PageHeader) > StiStyleComponentPlacement.None && component.Parent is StiPageHeaderBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.PageFooter) > StiStyleComponentPlacement.None && component.Parent is StiPageFooterBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.GroupHeader) > StiStyleComponentPlacement.None && component.Parent is StiGroupHeaderBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.GroupFooter) > StiStyleComponentPlacement.None && component.Parent is StiGroupFooterBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Header) > StiStyleComponentPlacement.None && component.Parent is StiHeaderBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Footer) > StiStyleComponentPlacement.None && component.Parent is StiFooterBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.ColumnHeader) > StiStyleComponentPlacement.None && component.Parent is StiColumnHeaderBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.ColumnFooter) > StiStyleComponentPlacement.None && component.Parent is StiColumnFooterBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Data) > StiStyleComponentPlacement.None && component.Parent is StiDataBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None && component is StiDataBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None && component is StiDataBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Table) > StiStyleComponentPlacement.None && component.Parent is StiTable)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Hierarchical) > StiStyleComponentPlacement.None && component.Parent is StiHierarchicalBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Child) > StiStyleComponentPlacement.None && component.Parent is StiChildBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Empty) > StiStyleComponentPlacement.None && component.Parent is StiEmptyBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Overlay) > StiStyleComponentPlacement.None && component.Parent is StiOverlayBand)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Panel) > StiStyleComponentPlacement.None && component.Parent is StiPanel)
				{
					flag3 = true;
				}
				if ((condition.Placement & StiStyleComponentPlacement.Page) > StiStyleComponentPlacement.None && component.Parent is StiPage)
				{
					flag3 = true;
				}
				if (flag3 && (condition.Type & StiStyleConditionType.PlacementNestedLevel) > (StiStyleConditionType)0)
				{
					int num9 = 1;
					StiComponent stiComponent = component.Parent;
					if ((condition.Placement & StiStyleComponentPlacement.DataEvenStyle) > StiStyleComponentPlacement.None || (condition.Placement & StiStyleComponentPlacement.DataOddStyle) > StiStyleComponentPlacement.None)
					{
						stiComponent = component;
					}
					if (stiComponent is StiBand stiBand)
					{
						num9 = stiBand.NestedLevel;
					}
					if (condition.OperationPlacementNestedLevel == StiStyleConditionOperation.EqualTo)
					{
						flag3 = num9 == condition.PlacementNestedLevel;
					}
					else if (condition.OperationPlacementNestedLevel == StiStyleConditionOperation.NotEqualTo)
					{
						flag3 = num9 != condition.PlacementNestedLevel;
					}
					else if (condition.OperationPlacementNestedLevel == StiStyleConditionOperation.GreaterThan)
					{
						flag3 = num9 > condition.PlacementNestedLevel;
					}
					else if (condition.OperationPlacementNestedLevel == StiStyleConditionOperation.GreaterThanOrEqualTo)
					{
						flag3 = num9 >= condition.PlacementNestedLevel;
					}
					else if (condition.OperationPlacementNestedLevel == StiStyleConditionOperation.LessThan)
					{
						flag3 = num9 < condition.PlacementNestedLevel;
					}
					else if (condition.OperationPlacementNestedLevel == StiStyleConditionOperation.LessThanOrEqualTo)
					{
						flag3 = num9 <= condition.PlacementNestedLevel;
					}
				}
				if (condition.OperationPlacement == StiStyleConditionOperation.NotEqualTo)
				{
					flag3 = !flag3;
				}
			}
			if (!flag || !flag2 || !flag3 || !flag4)
			{
				return false;
			}
		}
		return true;
	}
}
