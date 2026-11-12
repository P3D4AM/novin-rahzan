using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Drawing.Painters.PivotTable;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.PivotTable;

[ToolboxItem(false)]
public class StiPivotTableElementControl : StiElementControl, IStiRefreshControl
{
	private StiPivotTableContainerControl ContainerControl { get; set; }

	public virtual void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element)
		{
			((Control)this).Refresh();
		}
	}

	public override StiDataTable GetOrCreateData()
	{
		IStiPivotGridContainer orCreatePivot = StiElementDataCache.GetOrCreatePivot(base.Element as IStiPivotTableElement, new StiPivotTableCreator());
		if (orCreatePivot == null)
		{
			return null;
		}
		return StiElementDataCache.GetOrCreate(base.Element);
	}

	public override void CreateChild()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		StiScrollableControl stiScrollableControl = new StiScrollableControl(base.Element);
		((ScrollableControl)stiScrollableControl).AutoScroll = true;
		((Control)stiScrollableControl).Dock = (DockStyle)5;
		base.Child = (Control)(object)stiScrollableControl;
		base.Child.MouseEnter += Child_RefreshActionButtons;
		base.Child.MouseLeave += Child_RefreshActionButtons;
		base.Child.MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
		StiPivotTableContainerControl stiPivotTableContainerControl = new StiPivotTableContainerControl((Control)(object)this, base.Element as StiPivotTableElement);
		((Control)stiPivotTableContainerControl).Location = new Point(0, 0);
		ContainerControl = stiPivotTableContainerControl;
		base.Child.Controls.Add((Control)(object)ContainerControl);
		((Control)ContainerControl).MouseEnter += Child_RefreshActionButtons;
		((Control)ContainerControl).MouseLeave += Child_RefreshActionButtons;
		((Control)ContainerControl).MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
		((Control)this).Controls.Add(base.Child);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (base.IsShowNoResult || base.IsShowEmptyData)
		{
			ContainerControl?.Clear();
			return;
		}
		StiPivotTableElement stiPivotTableElement = base.Element as StiPivotTableElement;
		StiCrossTab stiCrossTab = StiPivotTableToCrossTabCache.Get(stiPivotTableElement);
		if (stiCrossTab == null)
		{
			StiElementDataCache.GetOrCreatePivot(stiPivotTableElement, new StiPivotTableCreator());
			stiCrossTab = StiPivotTableToCrossTabCache.Get(stiPivotTableElement);
		}
		ContainerControl?.MakeGrid(stiCrossTab.CrossTabInfo.Cross, stiPivotTableElement);
		StiPivotTableToCrossTabCache.Put(stiPivotTableElement, stiCrossTab);
		StiPivotToConvertedStateCache.PutFalse(stiPivotTableElement);
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		if (disposing)
		{
			if (base.Child != null)
			{
				base.Child.MouseEnter -= Child_RefreshActionButtons;
				base.Child.MouseLeave -= Child_RefreshActionButtons;
				base.Child.MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
			}
			if (ContainerControl != null)
			{
				((Control)ContainerControl).MouseEnter -= Child_RefreshActionButtons;
				((Control)ContainerControl).MouseLeave -= Child_RefreshActionButtons;
				((Control)ContainerControl).MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
			}
		}
		base.Dispose(disposing);
	}

	private void Child_RefreshActionButtons(object sender, EventArgs e)
	{
		RefreshActionButtons();
	}

	public StiPivotTableElementControl(IStiElement element)
		: base(element)
	{
	}
}
