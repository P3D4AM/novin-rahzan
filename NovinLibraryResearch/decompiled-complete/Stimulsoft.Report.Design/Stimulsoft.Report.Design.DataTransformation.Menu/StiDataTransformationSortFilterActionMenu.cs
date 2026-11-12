using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Dashboard.Table;
using Stimulsoft.Report.Design.DataTransformation.Editors;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.DataTransformation.Menu;

public class StiDataTransformationSortFilterActionMenu : StiTableElementSortFilterMenu
{
	private const bool CheckPrimaryKey = false;

	private StiDataTransformation transformation;

	protected override IStiDesignerBase Designer => transformation?.Dictionary?.Report?.Designer;

	protected override IStiQueryObject QueryObject => transformation;

	protected override List<StiDataFilterRule> Filters => transformation.Filters;

	protected override List<StiDataActionRule> Actions => transformation.Actions;

	protected override List<StiDataFilterRule> UserFilters => transformation.Filters;

	protected override List<StiDataSortRule> Sorts
	{
		get
		{
			return transformation.Sorts;
		}
		set
		{
			transformation.Sorts = value;
		}
	}

	protected override List<string> ColumnKeys => (from StiDataColumn c in transformation.Columns
		select c?.Key into c
		where c != null
		select c).ToList();

	public event EventHandler ActionsChanged;

	protected override void CreateMenuItems(IStiTableElement element, bool allowInteractive)
	{
		CreateSortMenuItems();
		CreateActionsMenuItems();
		CreateFilterMenuItems();
	}

	private void CreateActionsMenuItems()
	{
		bool flag = Actions != null && Actions.Any((StiDataActionRule a) => a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath));
		if (((ArrangedElementCollection)((ToolStrip)base.Menu).Items).Count > 0)
		{
			((ToolStrip)base.Menu).Items.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		}
		StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
		((ToolStripMenuItem)stiContextMenuItem).Checked = flag;
		((ToolStripItem)stiContextMenuItem).Text = Loc.Get("FormDictionaryDesigner", "Actions");
		((ToolStripItem)stiContextMenuItem).Enabled = IsAllowed(StiDataTransformationRestrictions.Actions);
		StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
		((ToolStrip)base.Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		StiContextMenuItem stiContextMenuItem3 = new StiContextMenuItem();
		((ToolStripMenuItem)stiContextMenuItem3).Checked = Actions.Any((StiDataActionRule a) => a.Type == StiDataActionType.Limit && (a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath)));
		((ToolStripItem)stiContextMenuItem3).Text = Loc.Get("Dashboard", "LimitRows") + "...";
		StiContextMenuItem stiContextMenuItem4 = stiContextMenuItem3;
		((ToolStripItem)stiContextMenuItem4).Click += ItemActionLimit_Click;
		((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems.Add((ToolStripItem)(object)stiContextMenuItem4);
		if (base.ColumnType == typeof(string))
		{
			StiContextMenuItem stiContextMenuItem5 = new StiContextMenuItem();
			((ToolStripMenuItem)stiContextMenuItem5).Checked = Actions.Any((StiDataActionRule a) => a.Type == StiDataActionType.Replace && (a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath)));
			((ToolStripItem)stiContextMenuItem5).Text = Loc.Get("Dashboard", "ReplaceValues") + "...";
			StiContextMenuItem stiContextMenuItem6 = stiContextMenuItem5;
			((ToolStripItem)stiContextMenuItem6).Click += ItemActionReplace_Click;
			((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems.Add((ToolStripItem)(object)stiContextMenuItem6);
		}
		if (base.ColumnType != null && base.ColumnType.IsNumericType())
		{
			StiContextMenuItem stiContextMenuItem7 = new StiContextMenuItem();
			((ToolStripMenuItem)stiContextMenuItem7).Checked = Actions.Any((StiDataActionRule a) => a.Type == StiDataActionType.RunningTotal && (a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath)));
			((ToolStripItem)stiContextMenuItem7).Text = Loc.Get("FormSystemTextEditor", "RunningTotal") + "...";
			StiContextMenuItem stiContextMenuItem8 = stiContextMenuItem7;
			((ToolStripItem)stiContextMenuItem8).Click += ItemActionRunningTotal_Click;
			((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems.Add((ToolStripItem)(object)stiContextMenuItem8);
		}
		if (base.ColumnType != null && base.ColumnType.IsNumericType())
		{
			StiContextMenuItem stiContextMenuItem9 = new StiContextMenuItem();
			((ToolStripMenuItem)stiContextMenuItem9).Checked = Actions.Any((StiDataActionRule a) => a.Type == StiDataActionType.Percentage && (a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath)));
			((ToolStripItem)stiContextMenuItem9).Text = Loc.Get("Dashboard", "ShowAsPercentages");
			StiContextMenuItem stiContextMenuItem10 = stiContextMenuItem9;
			((ToolStripItem)stiContextMenuItem10).Click += ItemActionPercentages_Click;
			((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems.Add((ToolStripItem)(object)stiContextMenuItem10);
		}
		if (((ArrangedElementCollection)((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems).Count > 0)
		{
			((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems.Add((ToolStripItem)(object)new StiSeparatorContextMenuItem());
		}
		StiContextMenuItem stiContextMenuItem11 = new StiContextMenuItem();
		((ToolStripItem)stiContextMenuItem11).Text = Loc.Get("Dashboard", "RemoveActions");
		((ToolStripItem)stiContextMenuItem11).Enabled = flag;
		StiContextMenuItem stiContextMenuItem12 = stiContextMenuItem11;
		((ToolStripItem)stiContextMenuItem12).Click += ItemRemoveActions_Click;
		((ToolStripDropDownItem)stiContextMenuItem2).DropDownItems.Add((ToolStripItem)(object)stiContextMenuItem12);
	}

	private void ItemActionLimit_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		StiDataTransformLimitForm stiDataTransformLimitForm = new StiDataTransformLimitForm(Actions, base.ColumnKey, base.ColumnPath, base.Restrictions);
		try
		{
			if ((int)((Form)stiDataTransformLimitForm).ShowDialog((IWin32Window)(object)base.ParentForm) == 1)
			{
				this.ActionsChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		finally
		{
			((IDisposable)stiDataTransformLimitForm)?.Dispose();
		}
	}

	private void ItemActionReplace_Click(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		List<string> filterStringItems = GetFilterStringItems();
		StiDataTransformReplaceForm stiDataTransformReplaceForm = new StiDataTransformReplaceForm(Actions, base.ColumnKey, base.ColumnPath, primaryKey: false, filterStringItems);
		try
		{
			if ((int)((Form)stiDataTransformReplaceForm).ShowDialog((IWin32Window)(object)base.ParentForm) == 1)
			{
				this.ActionsChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		finally
		{
			((IDisposable)stiDataTransformReplaceForm)?.Dispose();
		}
	}

	private void ItemActionRunningTotal_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		StiDataTransformRunningTotalForm stiDataTransformRunningTotalForm = new StiDataTransformRunningTotalForm(Actions, base.ColumnKey, base.ColumnPath);
		try
		{
			if ((int)((Form)stiDataTransformRunningTotalForm).ShowDialog((IWin32Window)(object)base.ParentForm) == 1)
			{
				this.ActionsChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		finally
		{
			((IDisposable)stiDataTransformRunningTotalForm)?.Dispose();
		}
	}

	private void ItemActionPercentages_Click(object sender, EventArgs e)
	{
		StiDataActionRule stiDataActionRule = Actions.FirstOrDefault((StiDataActionRule a) => a.Type == StiDataActionType.Percentage && (a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath)));
		if (stiDataActionRule != null)
		{
			Actions.Remove(stiDataActionRule);
		}
		else
		{
			stiDataActionRule = new StiDataActionRule(base.ColumnKey, base.ColumnPath);
			Actions.Add(stiDataActionRule);
		}
		this.ActionsChanged?.Invoke(this, EventArgs.Empty);
	}

	private void ItemRemoveActions_Click(object sender, EventArgs e)
	{
		if (Actions.Any((StiDataActionRule a) => a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath)))
		{
			Actions.RemoveAll((StiDataActionRule a) => a.Key == base.ColumnKey || (a.Key == null && a.Path == base.ColumnPath));
			this.ActionsChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public StiDataTransformationSortFilterActionMenu(StiContextMenu menu, StiDataTransformation transformation, Form parentForm = null, StiDataTransformationRestrictions restrictions = StiDataTransformationRestrictions.None)
		: base((ContextMenuStrip)(object)menu, null, parentForm, restrictions)
	{
		this.transformation = transformation;
	}
}
