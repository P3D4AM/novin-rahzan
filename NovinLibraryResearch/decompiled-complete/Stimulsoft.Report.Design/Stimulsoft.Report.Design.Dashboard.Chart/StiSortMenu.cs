using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Report.Design.Dashboard.Chart;

public class StiSortMenu
{
	protected IStiElement Element { get; }

	private StiDataTable ManualDataTable { get; set; }

	private IStiChartElement ChartElement => Element as IStiChartElement;

	private IStiIndicatorElement IndicatorElement => Element as IStiIndicatorElement;

	private IStiProgressElement ProgressElement => Element as IStiProgressElement;

	private IStiGaugeElement GaugeElement => Element as IStiGaugeElement;

	protected List<StiDataSortRule> Sort
	{
		get
		{
			return (Element as IStiUserSorts).UserSorts;
		}
		set
		{
			(Element as IStiUserSorts).UserSorts = value;
		}
	}

	protected Control Control { get; }

	protected StiContextMenu Menu { get; }

	private void Invalidate()
	{
		StiComponent stiComponent = Element as StiComponent;
		StiDesignerControl stiDesignerControl = stiComponent.Report.Designer.DesignerControl as StiDesignerControl;
		Rectangle clipRect = StiConvert.RectangleToScreen(stiDesignerControl, stiComponent.ClientRectangle);
		clipRect.Inflate(2, 2);
		stiDesignerControl.Builder.Redraw(clipRect);
	}

	public virtual void CreateItems(IStiElement element)
	{
		FetchManualDataTable();
		Menu.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(element);
		((ToolStrip)Menu).Items.Clear();
		CreateSortByArguments();
		CreateSortByValues();
		CreateSortBySeries();
		CreateSortByVariation();
		CreateSortByNone();
		if (Sort != null && Sort.Count > 0)
		{
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		}
		CreateSortAscending();
		CreateSortDescending();
	}

	private void FetchManualDataTable()
	{
		if (Element is IStiChartElement && Element is IStiManuallyEnteredData { DataMode: StiDataMode.ManuallyEnteringData } stiManuallyEnteredData)
		{
			ManualDataTable = stiManuallyEnteredData.GetManuallyEnteredDataTable();
		}
	}

	private List<IStiMeter> FetchAllValues()
	{
		if (ChartElement != null)
		{
			if (ManualDataTable != null)
			{
				if (!IsValuePresentedInManualData())
				{
					return null;
				}
				return ManualDataTable?.Meters?.Where((IStiMeter m) => m.Key == "Value")?.ToList();
			}
			return ChartElement.FetchAllValues();
		}
		if (IndicatorElement?.GetSeries() != null && IndicatorElement?.GetValue() != null)
		{
			return new List<IStiMeter> { IndicatorElement?.GetValue() };
		}
		if (ProgressElement?.GetSeries() != null && ProgressElement?.GetValue() != null)
		{
			return new List<IStiMeter> { ProgressElement?.GetValue() };
		}
		if (GaugeElement?.GetSeries() != null && GaugeElement?.GetValue() != null)
		{
			return new List<IStiMeter> { GaugeElement?.GetValue() };
		}
		return null;
	}

	private List<IStiMeter> FetchAllArguments()
	{
		if (ChartElement != null)
		{
			if (ManualDataTable != null)
			{
				if (!IsArgumentPresentedInManualData())
				{
					return null;
				}
				return ManualDataTable?.Meters?.Where((IStiMeter m) => m.Key == "Argument")?.ToList();
			}
			return ChartElement.FetchAllArguments();
		}
		if (IndicatorElement?.GetSeries() != null && IndicatorElement?.GetTarget() != null)
		{
			return new List<IStiMeter> { IndicatorElement?.GetTarget() };
		}
		if (ProgressElement?.GetSeries() != null && ProgressElement?.GetTarget() != null)
		{
			return new List<IStiMeter> { ProgressElement?.GetTarget() };
		}
		if (GaugeElement?.GetSeries() != null && GaugeElement?.GetTarget() != null)
		{
			return new List<IStiMeter> { GaugeElement?.GetTarget() };
		}
		return null;
	}

	private IStiMeter GetSeries()
	{
		if (ChartElement != null)
		{
			if (ManualDataTable != null)
			{
				if (!IsSeriesPresentedInManualData())
				{
					return null;
				}
				return ManualDataTable?.Meters?.Where((IStiMeter m) => m.Key == "Series")?.FirstOrDefault();
			}
			return ChartElement.GetSeries();
		}
		if (IndicatorElement?.GetSeries() != null)
		{
			return IndicatorElement?.GetSeries();
		}
		if (ProgressElement?.GetSeries() != null)
		{
			return ProgressElement?.GetSeries();
		}
		if (GaugeElement?.GetSeries() != null)
		{
			return GaugeElement?.GetSeries();
		}
		return null;
	}

	private bool IsValuePresentedInManualData()
	{
		IStiMeter stiMeter = ManualDataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == "Value");
		if (stiMeter == null)
		{
			return false;
		}
		int meterIndex = ManualDataTable.Meters.IndexOf(stiMeter);
		return ManualDataTable.Rows.Select((object[] r) => r[meterIndex]).Distinct().Count() > 1;
	}

	private bool IsArgumentPresentedInManualData()
	{
		IStiMeter stiMeter = ManualDataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == "Argument");
		if (stiMeter == null)
		{
			return false;
		}
		int meterIndex = ManualDataTable.Meters.IndexOf(stiMeter);
		return ManualDataTable.Rows.Select((object[] r) => r[meterIndex]).Distinct().Count() > 1;
	}

	private bool IsSeriesPresentedInManualData()
	{
		IStiMeter stiMeter = ManualDataTable.Meters.FirstOrDefault((IStiMeter m) => m.Key == "Series");
		if (stiMeter == null)
		{
			return false;
		}
		int meterIndex = ManualDataTable.Meters.IndexOf(stiMeter);
		return ManualDataTable.Rows.Select((object[] r) => r[meterIndex]).Distinct().Count() > 1;
	}

	protected virtual void CreateSortByArguments()
	{
		if (FetchAllArguments() == null)
		{
			return;
		}
		IEnumerable<string> enumerable = from a in FetchAllArguments()
			select StiLabelHelper.GetLabel(a) into a
			where !string.IsNullOrWhiteSpace(a)
			select a;
		if (enumerable.Any())
		{
			string text = string.Join(", ", enumerable);
			IStiMeter argument = FetchAllArguments().FirstOrDefault();
			StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
			((ToolStripItem)stiContextMenuItem).Text = Loc.GetMain("SortBy") + " " + text;
			((ToolStripMenuItem)stiContextMenuItem).Checked = Sort != null && Sort.Any((StiDataSortRule r) => r.Key == argument.Key);
			((ToolStripItem)stiContextMenuItem).Tag = argument;
			StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
			((ToolStripItem)stiContextMenuItem2).Click += delegate(object sender, EventArgs args)
			{
				((ToolStripDropDown)Menu).Close();
				Sort = new StiDataSortRule(GetKeyFromMenuItem(sender), GetSortDirection()).ToList();
				Invalidate();
			};
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		}
	}

	protected virtual void CreateSortByValues()
	{
		if (FetchAllValues() == null)
		{
			return;
		}
		foreach (IStiMeter value in FetchAllValues())
		{
			string label = StiLabelHelper.GetLabel(value);
			if (string.IsNullOrWhiteSpace(label))
			{
				break;
			}
			StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
			((ToolStripItem)stiContextMenuItem).Text = Loc.GetMain("SortBy") + " " + label;
			((ToolStripMenuItem)stiContextMenuItem).Checked = Sort != null && Sort.Any((StiDataSortRule r) => r.Key == value.Key);
			((ToolStripItem)stiContextMenuItem).Tag = value;
			StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
			((ToolStripItem)stiContextMenuItem2).Click += delegate(object sender, EventArgs args)
			{
				((ToolStripDropDown)Menu).Close();
				Sort = new StiDataSortRule(GetKeyFromMenuItem(sender), GetSortDirection()).ToList();
				Invalidate();
			};
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		}
	}

	protected virtual void CreateSortBySeries()
	{
		if (GetSeries() == null)
		{
			return;
		}
		string label = StiLabelHelper.GetLabel(GetSeries());
		if (!string.IsNullOrWhiteSpace(label))
		{
			StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
			((ToolStripItem)stiContextMenuItem).Text = Loc.GetMain("SortBy") + " " + label;
			((ToolStripMenuItem)stiContextMenuItem).Checked = Sort != null && Sort.Any((StiDataSortRule r) => r.Key == GetSeries().Key);
			((ToolStripItem)stiContextMenuItem).Tag = GetSeries();
			StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
			((ToolStripItem)stiContextMenuItem2).Click += delegate(object sender, EventArgs args)
			{
				((ToolStripDropDown)Menu).Close();
				Sort = new StiDataSortRule(GetKeyFromMenuItem(sender), GetSortDirection()).ToList();
				Invalidate();
			};
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		}
	}

	protected virtual void CreateSortByVariation()
	{
		if (!(Element is IStiAllowSortByVariation))
		{
			return;
		}
		List<IStiMeter> list = FetchAllValues();
		if (list == null)
		{
			return;
		}
		List<IStiMeter> list2 = FetchAllArguments();
		if (list2 != null)
		{
			StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
			((ToolStripItem)stiContextMenuItem).Text = Loc.GetMain("SortBy") + " " + Loc.GetEnum("StiTargetModeVariation");
			((ToolStripMenuItem)stiContextMenuItem).Checked = StiDataSortVariation.IsVariationSort(Sort);
			((ToolStripItem)stiContextMenuItem).Tag = StiDataSortVariation.Ident;
			StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
			((ToolStripItem)stiContextMenuItem2).Click += delegate
			{
				((ToolStripDropDown)Menu).Close();
				Sort = StiDataSortVariation.CreateVariationSort(GetSortDirection());
				Invalidate();
			};
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		}
	}

	protected virtual void CreateSortByNone()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.GetEnum("StiSeriesSortTypeNone");
		((ToolStripMenuItem)stiContextMenuItem).Checked = Sort == null || Sort.Count == 0;
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStripItem)stiContextMenuItem2).Click += delegate
		{
			((ToolStripDropDown)Menu).Close();
			Sort = new List<StiDataSortRule>();
			Invalidate();
		};
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
	}

	protected virtual void CreateSortAscending()
	{
		if (Sort == null || Sort.Count == 0)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.GetEnum("StiSeriesSortDirectionAscending");
		((ToolStripMenuItem)stiContextMenuItem).Checked = Sort.FirstOrDefault() != null && Sort.FirstOrDefault().Direction == StiDataSortDirection.Ascending;
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStripItem)stiContextMenuItem2).Click += delegate
		{
			((ToolStripDropDown)Menu).Close();
			if (Sort.FirstOrDefault() != null)
			{
				Sort.FirstOrDefault().Direction = StiDataSortDirection.Ascending;
			}
			Invalidate();
		};
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
	}

	protected virtual void CreateSortDescending()
	{
		if (Sort == null || Sort.Count == 0)
		{
			return;
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.GetEnum("StiSeriesSortDirectionDescending");
		((ToolStripMenuItem)stiContextMenuItem).Checked = Sort.FirstOrDefault() != null && Sort.FirstOrDefault().Direction == StiDataSortDirection.Descending;
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStripItem)stiContextMenuItem2).Click += delegate
		{
			((ToolStripDropDown)Menu).Close();
			if (Sort.FirstOrDefault() != null)
			{
				Sort.FirstOrDefault().Direction = StiDataSortDirection.Descending;
			}
			Invalidate();
		};
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
	}

	private string GetKeyFromMenuItem(object sender)
	{
		if (!(((sender is StiContextMenuItem stiContextMenuItem) ? ((ToolStripItem)stiContextMenuItem).Tag : null) is IStiMeter stiMeter))
		{
			return null;
		}
		return stiMeter.Key;
	}

	private StiDataSortDirection GetSortDirection()
	{
		if (Sort == null || Sort.Count == 0)
		{
			return StiDataSortDirection.Ascending;
		}
		return Sort.FirstOrDefault().Direction;
	}

	public StiSortMenu(StiContextMenu expressionMenu, Control control, IStiElement element)
	{
		Menu = expressionMenu;
		Element = element;
		Control = control;
	}
}
