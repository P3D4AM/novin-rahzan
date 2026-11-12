using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Dashboard.Viewer.Controls.Table.Menu;

public class StiTableElementSortFilterMenu
{
	private StiContextMenuItem itemSortAsc;

	private StiContextMenuItem itemSortDesc;

	private StiContextMenuItem itemSortNone;

	private StiContextMenuItem itemFilterRemove;

	private StiContextMenuItem itemFilterSelectAll;

	private StiContextMenuItem itemCustomFilter;

	private StiCheckedListContextMenuItem itemFiltersList;

	private string LocSortAsc
	{
		get
		{
			if (ColumnType == null || ColumnType.IsNumericType())
			{
				return Loc.Get("Dashboard", "SortSmallestToLargest");
			}
			if (ColumnType.IsDateType())
			{
				return Loc.Get("Dashboard", "SortOldestToNewest");
			}
			return Loc.Get("Dashboard", "SortAZ");
		}
	}

	private string LocSortDesc
	{
		get
		{
			if (ColumnType == null || ColumnType.IsNumericType())
			{
				return Loc.Get("Dashboard", "SortLargestToSmallest");
			}
			if (ColumnType.IsDateType())
			{
				return Loc.Get("Dashboard", "SortNewestToOldest");
			}
			return Loc.Get("Dashboard", "SortZA");
		}
	}

	protected virtual IStiDesignerBase Designer => TableElement.Report.Designer;

	protected virtual IStiQueryObject QueryObject => TableElement.Page as IStiDashboard;

	protected virtual List<StiDataSortRule> Sorts
	{
		get
		{
			return TableElement.UserSorts;
		}
		set
		{
			TableElement.UserSorts = value;
		}
	}

	protected virtual List<string> ColumnKeys => (from c in TableElement?.FetchAllMeters()?.Select((IStiMeter k) => k?.Key)
		where c != null
		select c).ToList();

	protected virtual List<StiDataFilterRule> Filters => TableElement?.UserFilters;

	protected virtual List<StiDataActionRule> Actions => null;

	protected virtual List<StiDataFilterRule> UserFilters => (TableElement?.Page as IStiDashboard)?.GetUserFilters(TableElement);

	protected ContextMenuStrip Menu { get; set; }

	protected Form ParentForm { get; set; }

	protected string ColumnKey { get; set; }

	protected string ColumnPath { get; set; }

	protected Type ColumnType { get; set; }

	protected bool PrimaryKey { get; set; }

	protected List<StiTableElementSortFilterMenuItem> FilterItems { get; set; }

	protected StiDataTransformationRestrictions Restrictions { get; set; }

	public IStiTableElement TableElement { get; set; }

	public event EventHandler SortChanged;

	public event EventHandler FiltersChanged;

	public void CreateItems(List<IStiMeter> meters, int columnIndex, IStiTableElement element)
	{
		IStiTableDashboardInteraction tableInteraction = (element as IStiElementInteraction)?.DashboardInteraction as IStiTableDashboardInteraction;
		IStiMeter stiMeter = ((columnIndex != -1) ? meters[columnIndex] : null);
		bool allowInteractive = !(stiMeter is IStiSparklinesColumn);
		if (tableInteraction == null || tableInteraction.AllowUserFiltering || tableInteraction.AllowUserSorting)
		{
			RetrieveDataColumnItems(meters, columnIndex);
		}
		((ToolStrip)Menu).Items.Clear();
		CreateMenuItems(element, allowInteractive);
		((ToolStripDropDown)Menu).Opening += delegate
		{
			if (itemFiltersList != null)
			{
				itemFiltersList.CalculateSize();
			}
			if ((tableInteraction == null || tableInteraction.AllowUserSorting) && allowInteractive)
			{
				StiDataSortDirection sortDirection = StiDataSortRuleHelper.GetSortDirection(Sorts, ColumnKey);
				((ToolStripMenuItem)itemSortAsc).Checked = sortDirection == StiDataSortDirection.Ascending;
				((ToolStripMenuItem)itemSortDesc).Checked = sortDirection == StiDataSortDirection.Descending;
				((ToolStripMenuItem)itemSortNone).Checked = sortDirection == StiDataSortDirection.None;
			}
			if ((tableInteraction == null || tableInteraction.AllowUserFiltering) && allowInteractive && itemFilterSelectAll != null)
			{
				if (Filters.Any((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath)))
				{
					((ToolStripMenuItem)itemFilterSelectAll).Checked = FilterItems.Where((StiTableElementSortFilterMenuItem i) => i.MenuItem != null).All((StiTableElementSortFilterMenuItem i) => i.MenuItem.Checked);
				}
				else
				{
					SelectAll(value: true);
				}
			}
		};
	}

	protected virtual void CreateMenuItems(IStiTableElement element, bool allowInteractive)
	{
		if (allowInteractive)
		{
			IStiTableDashboardInteraction stiTableDashboardInteraction = ((element is IStiElementInteraction stiElementInteraction) ? stiElementInteraction.DashboardInteraction : null) as IStiTableDashboardInteraction;
			if (stiTableDashboardInteraction == null || stiTableDashboardInteraction.AllowUserSorting)
			{
				CreateSortMenuItems();
			}
			if (stiTableDashboardInteraction == null || stiTableDashboardInteraction.AllowUserFiltering)
			{
				CreateFilterMenuItems();
			}
		}
	}

	private void InvokeInplaceEvents()
	{
	}

	protected void CreateSortMenuItems()
	{
		CreateSortAscMenuItem();
		CreateSortDescMenuItem();
		CreateNoSortMenuItem();
	}

	private void CreateSortAscMenuItem()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = LocSortAsc;
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Sorts);
		itemSortAsc = stiContextMenuItem;
		((ToolStripItem)itemSortAsc).Click += delegate
		{
			((ToolStripMenuItem)itemSortAsc).Checked = true;
			((ToolStripMenuItem)itemSortDesc).Checked = false;
			((ToolStripMenuItem)itemSortNone).Checked = false;
			Sorts = StiDataSortRuleHelper.SetSortDirection(Sorts, ColumnKeys, ColumnKey, StiDataSortDirection.Ascending);
			this.SortChanged?.Invoke(this, EventArgs.Empty);
			InvokeInplaceEvents();
		};
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemSortAsc);
	}

	private void CreateSortDescMenuItem()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = LocSortDesc;
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Sorts);
		itemSortDesc = stiContextMenuItem;
		((ToolStripItem)itemSortDesc).Click += delegate
		{
			((ToolStripMenuItem)itemSortAsc).Checked = false;
			((ToolStripMenuItem)itemSortDesc).Checked = true;
			((ToolStripMenuItem)itemSortNone).Checked = false;
			Sorts = StiDataSortRuleHelper.SetSortDirection(Sorts, ColumnKeys, ColumnKey, StiDataSortDirection.Descending);
			this.SortChanged?.Invoke(this, EventArgs.Empty);
			InvokeInplaceEvents();
		};
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemSortDesc);
	}

	private void CreateNoSortMenuItem()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("FormBand", "NoSort");
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Sorts);
		itemSortNone = stiContextMenuItem;
		((ToolStripItem)itemSortNone).Click += delegate
		{
			((ToolStripMenuItem)itemSortAsc).Checked = false;
			((ToolStripMenuItem)itemSortDesc).Checked = false;
			((ToolStripMenuItem)itemSortNone).Checked = true;
			Sorts = StiDataSortRuleHelper.SetSortDirection(Sorts, ColumnKeys, ColumnKey, StiDataSortDirection.None);
			this.SortChanged?.Invoke(this, EventArgs.Empty);
			InvokeInplaceEvents();
		};
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemSortNone);
	}

	protected void CreateFilterMenuItems()
	{
		if (FilterItems == null || !FilterItems.Any())
		{
			return;
		}
		if (((ArrangedElementCollection)((ToolStrip)Menu).Items).Count > 0)
		{
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		}
		bool isScrollable = FilterItems.Count() > 10;
		StiCheckedListContextMenuItem stiCheckedListContextMenuItem = new StiCheckedListContextMenuItem();
		((ToolStripItem)stiCheckedListContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		itemFiltersList = stiCheckedListContextMenuItem;
		if (ColumnType != null)
		{
			if (ColumnType.IsNumericType())
			{
				CreateNumberFilterMenuItems();
			}
			else if (ColumnType.IsDateType())
			{
				CreateDateFilterMenuItems();
			}
			else if (ColumnType.IsBooleanType())
			{
				CreateBooleanFilterMenuItems();
			}
			else if (ColumnType.IsStringType())
			{
				CreateStringFilterMenuItems();
			}
		}
		CreateCustomFilterMenuItem();
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("FormBand", "RemoveFilter");
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		itemFilterRemove = stiContextMenuItem;
		((ToolStripItem)itemFilterRemove).Click += ItemFilterRemove_Click;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemFilterRemove);
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		StiContextMenuItem stiContextMenuItem2 = new StiContextMenuItem();
		((ToolStripMenuItem)stiContextMenuItem2).CheckOnClick = true;
		((ToolStripItem)stiContextMenuItem2).Text = Loc.Get("Dashboard", "SelectAll");
		((ToolStripItem)stiContextMenuItem2).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		itemFilterSelectAll = stiContextMenuItem2;
		((ToolStripItem)itemFilterSelectAll).Click += ItemFilterSelectAll_Click;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemFilterSelectAll);
		StiDataFilterConditionGroupType conditionGroupType = GetDataFilterConditionGroupType();
		FilterItems.ForEach(delegate(StiTableElementSortFilterMenuItem i)
		{
			bool flag = CheckCondition(conditionGroupType, i.FilterString);
			StiContextMenuItem stiContextMenuItem3 = new StiContextMenuItem();
			((ToolStripMenuItem)stiContextMenuItem3).CheckOnClick = true;
			((ToolStripItem)stiContextMenuItem3).Text = i.DisplayString;
			((ToolStripItem)stiContextMenuItem3).Tag = i;
			((ToolStripMenuItem)stiContextMenuItem3).Checked = flag;
			((ToolStripItem)stiContextMenuItem3).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
			StiContextMenuItem itemFilterItem = stiContextMenuItem3;
			((ToolStripItem)itemFilterItem).Click += ItemFilterItem_Click;
			i.MenuItem = (ToolStripMenuItem)(object)itemFilterItem;
			if (isScrollable)
			{
				((ToolStripMenuItem)itemFilterItem).CheckedChanged += delegate(object sender2, EventArgs e2)
				{
					int num = ((ObjectCollection)itemFiltersList.Items).IndexOf((object)itemFilterItem);
					if (num != -1)
					{
						((CheckedListBox)itemFiltersList.ListBox).SetItemChecked(num, ((ToolStripMenuItem)((sender2 is ToolStripMenuItem) ? sender2 : null)).Checked);
					}
				};
				itemFiltersList.Items.Add((object)itemFilterItem, flag);
			}
			else
			{
				((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemFilterItem);
			}
		});
		if (isScrollable)
		{
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemFiltersList);
		}
	}

	private void CreateCustomConditionFilterMenuItem(StiContextMenuItem menu, StiDataFilterCondition condition)
	{
		int num = ((IEnumerable)((ToolStripDropDownItem)menu).DropDownItems).Cast<ToolStripItem>().Count((ToolStripItem i) => !(i is StiSeparatorContextMenuItem));
		if (num % 2 == 0 && num > 0)
		{
			((ToolStripDropDownItem)menu).DropDownItems.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("PropertyEnum", $"StiFilterCondition{condition}") + ((condition != StiDataFilterCondition.IsBlank && condition != StiDataFilterCondition.IsNotBlank && condition != StiDataFilterCondition.IsNull && condition != StiDataFilterCondition.IsNotNull) ? "..." : string.Empty);
		((ToolStripItem)stiContextMenuItem).Tag = condition;
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStripItem)stiContextMenuItem2).Click += ItemCustomConditionFilter_Click;
		((ToolStripDropDownItem)menu).DropDownItems.Add((ToolStripItem)(object)stiContextMenuItem2);
	}

	private void CreateNumberFilterMenuItems()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("Dashboard", "NumberFilters") + "...";
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		StiDataFilterCondition[] array = new StiDataFilterCondition[10]
		{
			StiDataFilterCondition.EqualTo,
			StiDataFilterCondition.NotEqualTo,
			StiDataFilterCondition.Between,
			StiDataFilterCondition.NotBetween,
			StiDataFilterCondition.GreaterThan,
			StiDataFilterCondition.GreaterThanOrEqualTo,
			StiDataFilterCondition.LessThan,
			StiDataFilterCondition.LessThanOrEqualTo,
			StiDataFilterCondition.IsNull,
			StiDataFilterCondition.IsNotNull
		};
		StiDataFilterCondition[] array2 = array;
		foreach (StiDataFilterCondition condition in array2)
		{
			CreateCustomConditionFilterMenuItem(stiContextMenuItem2, condition);
		}
	}

	private void CreateDateFilterMenuItems()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("Dashboard", "DateFilters") + "...";
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		StiDataFilterCondition[] array = new StiDataFilterCondition[10]
		{
			StiDataFilterCondition.EqualTo,
			StiDataFilterCondition.NotEqualTo,
			StiDataFilterCondition.Between,
			StiDataFilterCondition.NotBetween,
			StiDataFilterCondition.GreaterThan,
			StiDataFilterCondition.GreaterThanOrEqualTo,
			StiDataFilterCondition.LessThan,
			StiDataFilterCondition.LessThanOrEqualTo,
			StiDataFilterCondition.IsNull,
			StiDataFilterCondition.IsNotNull
		};
		StiDataFilterCondition[] array2 = array;
		foreach (StiDataFilterCondition condition in array2)
		{
			CreateCustomConditionFilterMenuItem(stiContextMenuItem2, condition);
		}
	}

	private void CreateBooleanFilterMenuItems()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("Dashboard", "BooleanFilters") + "...";
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		StiDataFilterCondition[] array = new StiDataFilterCondition[4]
		{
			StiDataFilterCondition.EqualTo,
			StiDataFilterCondition.NotEqualTo,
			StiDataFilterCondition.IsNull,
			StiDataFilterCondition.IsNotNull
		};
		StiDataFilterCondition[] array2 = array;
		foreach (StiDataFilterCondition condition in array2)
		{
			CreateCustomConditionFilterMenuItem(stiContextMenuItem2, condition);
		}
	}

	private void CreateStringFilterMenuItems()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("Dashboard", "StringFilters");
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		StiDataFilterCondition[] array = new StiDataFilterCondition[10]
		{
			StiDataFilterCondition.EqualTo,
			StiDataFilterCondition.NotEqualTo,
			StiDataFilterCondition.Containing,
			StiDataFilterCondition.NotContaining,
			StiDataFilterCondition.BeginningWith,
			StiDataFilterCondition.EndingWith,
			StiDataFilterCondition.IsBlank,
			StiDataFilterCondition.IsNotBlank,
			StiDataFilterCondition.IsNull,
			StiDataFilterCondition.IsNotNull
		};
		StiDataFilterCondition[] array2 = array;
		foreach (StiDataFilterCondition condition in array2)
		{
			CreateCustomConditionFilterMenuItem(stiContextMenuItem2, condition);
		}
	}

	private void CreateCustomFilterMenuItem()
	{
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("Dashboard", "CustomFilter");
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Filters);
		itemCustomFilter = stiContextMenuItem;
		((ToolStripItem)itemCustomFilter).Click += ItemCustomFilter_Click;
		((ToolStrip)Menu).Items.Add((ToolStripItem)(object)itemCustomFilter);
	}

	private void RetrieveDataColumnItems(List<IStiMeter> meters, int columnIndex)
	{
		if (columnIndex >= meters.Count || meters[columnIndex] == null)
		{
			return;
		}
		ColumnKey = meters[columnIndex].Key;
		if (!(StiDataExpressionHelper.GetDataColumnFromExpression(QueryObject, meters[columnIndex].Expression) is StiDataColumn stiDataColumn))
		{
			FilterItems = null;
			return;
		}
		DataTable dataColumnTable = StiDataPicker.GetFromCache(stiDataColumn.DataSource);
		if (dataColumnTable == null)
		{
			FilterItems = null;
			return;
		}
		ColumnPath = stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
		int dataColumnIndex = dataColumnTable.Columns.IndexOf(ColumnPath);
		if (dataColumnIndex != -1)
		{
			ColumnType = dataColumnTable.Columns[dataColumnIndex].DataType;
			PrimaryKey = dataColumnTable.PrimaryKey.Any((DataColumn c) => c == dataColumnTable.Columns[dataColumnIndex]);
			StiDataFilterComparer comparer = new StiDataFilterComparer();
			List<object> source = Distinct(from r in dataColumnTable.AsEnumerable()
				select r[dataColumnIndex]).Where(IsValueCanBeFiltered).OrderBy((object c) => c, comparer).ToList();
			List<StiTableElementSortFilterMenuItem> list = (from i in source
				where i != null && i != DBNull.Value && !string.IsNullOrWhiteSpace(i.ToString())
				select new StiTableElementSortFilterMenuItem(i, ColumnType)).ToList();
			if (source.Any((object i) => i != null && i != DBNull.Value && string.IsNullOrWhiteSpace(i.ToString())))
			{
				list.Add(new StiTableElementSortFilterMenuItem(string.Empty, ColumnType));
			}
			if (source.Any((object i) => i == null || i == DBNull.Value))
			{
				list.Add(new StiTableElementSortFilterMenuItem(null, ColumnType));
			}
			FilterItems = list;
		}
	}

	private bool IsValueCanBeFiltered(object value)
	{
		if (value == null || value == DBNull.Value)
		{
			return true;
		}
		Type type = value.GetType();
		if (!type.IsNumericType() && !type.IsDateType() && !(type == typeof(string)))
		{
			return type == typeof(bool);
		}
		return true;
	}

	private bool CheckCondition(StiDataFilterConditionGroupType groupType, string value)
	{
		IEnumerable<StiDataFilterRule> source = Filters.Where((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath));
		switch (groupType)
		{
		case StiDataFilterConditionGroupType.Equal:
			if (value == null)
			{
				return source.Any((StiDataFilterRule r) => r.Condition == StiDataFilterCondition.IsNull);
			}
			if (value == string.Empty)
			{
				return source.Any((StiDataFilterRule r) => r.Condition == StiDataFilterCondition.IsBlank);
			}
			return source.Any((StiDataFilterRule r) => r.Value == value);
		default:
			return false;
		case StiDataFilterConditionGroupType.NotEqual:
			if (value == null)
			{
				return source.All((StiDataFilterRule r) => r.Condition != StiDataFilterCondition.IsNotNull);
			}
			if (value == string.Empty)
			{
				return source.All((StiDataFilterRule r) => r.Condition != StiDataFilterCondition.IsNotBlank);
			}
			return source.All((StiDataFilterRule r) => r.Value != value);
		}
	}

	private IEnumerable<object> Distinct(IEnumerable<object> items)
	{
		if (!(items.FirstOrDefault() is DateTime))
		{
			return items.Distinct();
		}
		return (from DateTime? x in items.Select(StiValueHelper.TryToNullableDateTime)
			group x by (!x.HasValue) ? null : new DateTime?(x.Value) into x
			select x.Key).Cast<object>().Distinct();
	}

	private StiDataFilterConditionGroupType GetDataFilterConditionGroupType()
	{
		IEnumerable<StiDataFilterRule> source = Filters.Where((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath));
		if (!source.Any())
		{
			return StiDataFilterConditionGroupType.Empty;
		}
		if (source.All((StiDataFilterRule r) => r.Condition == StiDataFilterCondition.EqualTo || r.Condition == StiDataFilterCondition.IsBlank || r.Condition == StiDataFilterCondition.IsNull))
		{
			return StiDataFilterConditionGroupType.Equal;
		}
		if (source.All((StiDataFilterRule r) => r.Condition == StiDataFilterCondition.NotEqualTo || r.Condition == StiDataFilterCondition.IsNotBlank || r.Condition == StiDataFilterCondition.IsNotNull))
		{
			return StiDataFilterConditionGroupType.NotEqual;
		}
		return StiDataFilterConditionGroupType.Custom;
	}

	private void ApplyFiltersFromList()
	{
		Filters.RemoveAll((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath));
		if (((ToolStripMenuItem)itemFilterSelectAll).Checked)
		{
			return;
		}
		int num = FilterItems.Count((StiTableElementSortFilterMenuItem i) => i.MenuItem != null && i.MenuItem.Checked);
		if (num == 0)
		{
			Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.IsFalse));
			return;
		}
		StiDataFilterConditionGroupType conditionGroupType = ((num > FilterItems.Count / 2) ? StiDataFilterConditionGroupType.NotEqual : StiDataFilterConditionGroupType.Equal);
		FilterItems.ForEach(delegate(StiTableElementSortFilterMenuItem i)
		{
			if (conditionGroupType == StiDataFilterConditionGroupType.Equal && i.MenuItem != null && i.MenuItem.Checked)
			{
				if (i.FilterString == null)
				{
					Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.IsNull));
				}
				else if (i.FilterString == string.Empty)
				{
					Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.IsBlank));
				}
				else
				{
					Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.EqualTo, i.FilterString));
				}
			}
			else if (conditionGroupType == StiDataFilterConditionGroupType.NotEqual && (i.MenuItem == null || !i.MenuItem.Checked))
			{
				if (i.FilterString == null)
				{
					Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.IsNotNull));
				}
				else if (i.FilterString == string.Empty)
				{
					Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.IsNotBlank));
				}
				else
				{
					Filters.Add(new StiDataFilterRule(ColumnKey, ColumnPath, StiDataFilterCondition.NotEqualTo, i.FilterString));
				}
			}
		});
	}

	private void SelectAll(bool value)
	{
		if (itemFilterRemove != null)
		{
			((ToolStripItem)itemFilterRemove).Enabled = !value;
		}
		if (itemFilterSelectAll != null)
		{
			((ToolStripMenuItem)itemFilterSelectAll).Checked = value;
		}
		if (FilterItems != null)
		{
			FilterItems.Where((StiTableElementSortFilterMenuItem i) => i.MenuItem != null).ToList().ForEach(delegate(StiTableElementSortFilterMenuItem i)
			{
				i.MenuItem.Checked = value;
			});
		}
	}

	protected List<string> GetFilterStringItems()
	{
		return (from i in FilterItems
			select i.FilterString into i
			where !string.IsNullOrWhiteSpace(i)
			select i).ToList();
	}

	protected bool IsAllowed(StiDataTransformationRestrictions restriction)
	{
		return (Restrictions & restriction) != restriction;
	}

	private void ItemFilterRemove_Click(object sender, EventArgs e)
	{
		SelectAll(value: true);
		ApplyFiltersFromList();
		this.FiltersChanged?.Invoke(this, EventArgs.Empty);
	}

	private void ItemFilterSelectAll_Click(object sender, EventArgs e)
	{
		SelectAll(((ToolStripMenuItem)itemFilterSelectAll).Checked);
		ApplyFiltersFromList();
		this.FiltersChanged?.Invoke(this, EventArgs.Empty);
	}

	private void ItemFilterItem_Click(object sender, EventArgs e)
	{
		((ToolStripMenuItem)itemFilterSelectAll).Checked = FilterItems.Where((StiTableElementSortFilterMenuItem i) => i.MenuItem != null).All((StiTableElementSortFilterMenuItem i) => i.MenuItem.Checked);
		ApplyFiltersFromList();
		((ToolStripItem)itemFilterRemove).Enabled = Filters.Any((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath));
		this.FiltersChanged?.Invoke(this, EventArgs.Empty);
	}

	private void ItemCustomFilter_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		List<string> filterStringItems = GetFilterStringItems();
		StiTableElementFiltersForm stiTableElementFiltersForm = new StiTableElementFiltersForm(Designer, Filters, ColumnKey, ColumnPath, ColumnType, filterStringItems);
		try
		{
			if ((int)((Form)stiTableElementFiltersForm).ShowDialog((IWin32Window)(object)ParentForm) == 1)
			{
				((ToolStripItem)itemFilterRemove).Enabled = Filters.Any((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath));
				this.FiltersChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		finally
		{
			((IDisposable)stiTableElementFiltersForm)?.Dispose();
		}
	}

	private void ItemCustomConditionFilter_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		List<string> filterStringItems = GetFilterStringItems();
		StiDataFilterCondition stiDataFilterCondition = (StiDataFilterCondition)((ToolStripItem)(ToolStripMenuItem)sender).Tag;
		if (stiDataFilterCondition == StiDataFilterCondition.IsBlank || stiDataFilterCondition == StiDataFilterCondition.IsNotBlank || stiDataFilterCondition == StiDataFilterCondition.IsNull || stiDataFilterCondition == StiDataFilterCondition.IsNotNull)
		{
			StiDataFilterRule item = new StiDataFilterRule(ColumnKey, ColumnPath, stiDataFilterCondition);
			Filters.RemoveAll((StiDataFilterRule r) => r.Key == ColumnKey || (r.Key == null && r.Path == ColumnPath));
			Filters.Insert(0, item);
			this.FiltersChanged?.Invoke(this, EventArgs.Empty);
			return;
		}
		StiTableElementFiltersForm stiTableElementFiltersForm = new StiTableElementFiltersForm(Designer, Filters, ColumnKey, ColumnPath, ColumnType, filterStringItems, stiDataFilterCondition);
		try
		{
			if ((int)((Form)stiTableElementFiltersForm).ShowDialog((IWin32Window)(object)ParentForm) == 1)
			{
				this.FiltersChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		finally
		{
			((IDisposable)stiTableElementFiltersForm)?.Dispose();
		}
	}

	public StiTableElementSortFilterMenu(ContextMenuStrip menu, IStiTableElement tableElement, Form parentForm = null, StiDataTransformationRestrictions restrictions = StiDataTransformationRestrictions.None)
	{
		Menu = menu;
		TableElement = tableElement;
		ParentForm = parentForm;
		Restrictions = restrictions;
	}
}
