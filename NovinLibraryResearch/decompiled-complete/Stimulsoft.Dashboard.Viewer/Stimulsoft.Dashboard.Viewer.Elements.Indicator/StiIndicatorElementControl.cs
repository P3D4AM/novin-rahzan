using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Indicator;

[ToolboxItem(false)]
public class StiIndicatorElementControl : StiElementControl, IStiRefreshControl
{
	private StiIndicatorControl IndicatorControl { get; set; }

	private StiScrollableControl Scrollable
	{
		get
		{
			return base.Child as StiScrollableControl;
		}
		set
		{
			base.Child = (Control)(object)value;
		}
	}

	public virtual void RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		if (sender == base.Element)
		{
			((Control)this).Refresh();
		}
	}

	public override void CreateChild()
	{
		CreateScrollableContainer();
		CreateSingleControl();
		((Control)this).Controls.Add((Control)(object)IndicatorControl);
	}

	private void CreateScrollableContainer()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		StiScrollableControl stiScrollableControl = new StiScrollableControl(base.Element);
		((Control)stiScrollableControl).Dock = (DockStyle)5;
		Scrollable = stiScrollableControl;
		((Control)Scrollable).SizeChanged += Child_SizeChanged;
		((Control)Scrollable).MouseEnter += Child_RefreshActionButtons;
		((Control)Scrollable).MouseLeave += Child_RefreshActionButtons;
		((Control)Scrollable).MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
	}

	private void CreateSingleControl()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		StiIndicatorControl stiIndicatorControl = new StiIndicatorControl(base.Element);
		((Control)stiIndicatorControl).Dock = (DockStyle)5;
		IndicatorControl = stiIndicatorControl;
		((Control)IndicatorControl).MouseEnter += Child_RefreshActionButtons;
		((Control)IndicatorControl).MouseLeave += Child_RefreshActionButtons;
		((Control)IndicatorControl).MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		IndicatorControl?.RefreshChild(dataTable);
		if (IndicatorControl.IndicatorAtom.Iterations.Count > 1)
		{
			SwitchToSingleControl();
		}
		else if (IndicatorControl.IndicatorAtom.Iterations.Count > 0)
		{
			SwitchToScrollableContainer();
		}
		RecalculateSize();
	}

	private void SwitchToSingleControl()
	{
		if (((Control)IndicatorControl).Parent != Scrollable)
		{
			((Control)this).Controls.Clear();
			((Control)IndicatorControl).Dock = (DockStyle)0;
			((Control)IndicatorControl).Location = new Point(0, 0);
			((Control)Scrollable).Controls.Add((Control)(object)IndicatorControl);
			((Control)this).Controls.Add((Control)(object)Scrollable);
		}
	}

	private void SwitchToScrollableContainer()
	{
		if (((Control)IndicatorControl).Parent == Scrollable)
		{
			((Control)this).Controls.Clear();
			((Control)Scrollable).Controls.Clear();
			((Control)IndicatorControl).Dock = (DockStyle)5;
			((Control)this).Controls.Add((Control)(object)IndicatorControl);
		}
	}

	private void RecalculateSize()
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)Scrollable).Width + ((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Value - (((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Visible ? SystemInformation.VerticalScrollBarWidth : 0), ((Control)Scrollable).Height + ((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Value - (((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Visible ? SystemInformation.HorizontalScrollBarHeight : 0));
		((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Value = 0;
		((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Value = 0;
		if (IndicatorControl != null)
		{
			IndicatorControl.RecalculateSize(rect);
			if (((Control)IndicatorControl).Height > ((Control)Scrollable).Height)
			{
				((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Visible = true;
			}
			if (((Control)IndicatorControl).Width > ((Control)Scrollable).Width)
			{
				((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Visible = true;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		if (disposing)
		{
			if (Scrollable != null)
			{
				((Control)Scrollable).SizeChanged -= Child_SizeChanged;
				((Control)Scrollable).MouseEnter -= Child_RefreshActionButtons;
				((Control)Scrollable).MouseLeave -= Child_RefreshActionButtons;
				((Control)Scrollable).MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
				((Component)(object)Scrollable).Dispose();
				Scrollable = null;
			}
			if (IndicatorControl != null)
			{
				((Control)IndicatorControl).MouseEnter -= Child_RefreshActionButtons;
				((Control)IndicatorControl).MouseLeave -= Child_RefreshActionButtons;
				((Control)IndicatorControl).MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
				((Component)(object)IndicatorControl).Dispose();
				IndicatorControl = null;
			}
		}
		base.Dispose(disposing);
	}

	private void Child_RefreshActionButtons(object sender, EventArgs e)
	{
		RefreshActionButtons();
	}

	private void Child_SizeChanged(object sender, EventArgs e)
	{
		RecalculateSize();
		((ScrollableControl)Scrollable).AutoScroll = false;
		((ScrollableControl)Scrollable).AutoScroll = true;
	}

	public StiIndicatorElementControl(IStiElement element)
		: base(element)
	{
	}
}
