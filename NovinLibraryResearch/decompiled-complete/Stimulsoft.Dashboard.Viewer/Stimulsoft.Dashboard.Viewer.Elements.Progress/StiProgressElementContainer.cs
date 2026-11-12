using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Progress;

[ToolboxItem(false)]
public class StiProgressElementContainer : StiElementControl, IStiRefreshControl
{
	private StiProgressControl ProgressControl { get; set; }

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
		((Control)this).Controls.Add((Control)(object)ProgressControl);
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
		StiProgressControl stiProgressControl = new StiProgressControl(base.Element);
		((Control)stiProgressControl).Dock = (DockStyle)5;
		ProgressControl = stiProgressControl;
		((Control)ProgressControl).MouseEnter += Child_RefreshActionButtons;
		((Control)ProgressControl).MouseLeave += Child_RefreshActionButtons;
		((Control)ProgressControl).MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		ProgressControl?.RefreshChild(dataTable);
		if (ProgressControl.Atom.Iterations.Count > 1)
		{
			SwitchToSingleControl();
		}
		else if (ProgressControl.Atom.Iterations.Count > 0)
		{
			SwitchToScrollableContainer();
		}
		RecalculateSize();
	}

	private void SwitchToSingleControl()
	{
		if (((Control)ProgressControl).Parent != Scrollable)
		{
			((Control)this).Controls.Clear();
			((Control)ProgressControl).Dock = (DockStyle)0;
			((Control)ProgressControl).Location = new Point(0, 0);
			((Control)Scrollable).Controls.Add((Control)(object)ProgressControl);
			((Control)this).Controls.Add((Control)(object)Scrollable);
		}
	}

	private void SwitchToScrollableContainer()
	{
		if (((Control)ProgressControl).Parent == Scrollable)
		{
			((Control)this).Controls.Clear();
			((Control)Scrollable).Controls.Clear();
			((Control)ProgressControl).Dock = (DockStyle)5;
			((Control)this).Controls.Add((Control)(object)ProgressControl);
		}
	}

	private void RecalculateSize()
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)Scrollable).Width + ((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Value - 18 - (((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Visible ? SystemInformation.VerticalScrollBarWidth : 0), ((Control)Scrollable).Height + ((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Value - (((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Visible ? SystemInformation.HorizontalScrollBarHeight : 0));
		((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Value = 0;
		((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Value = 0;
		ProgressControl?.RecalculateSize(rect);
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
			if (ProgressControl != null)
			{
				((Control)ProgressControl).MouseEnter -= Child_RefreshActionButtons;
				((Control)ProgressControl).MouseLeave -= Child_RefreshActionButtons;
				((Control)ProgressControl).MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
				((Component)(object)ProgressControl).Dispose();
				ProgressControl = null;
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

	public StiProgressElementContainer(IStiElement element)
		: base(element)
	{
	}
}
