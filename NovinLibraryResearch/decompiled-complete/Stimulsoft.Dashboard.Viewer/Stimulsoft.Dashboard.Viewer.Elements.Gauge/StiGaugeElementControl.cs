using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Visuals.Gauge;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Gauge;

[ToolboxItem(false)]
public class StiGaugeElementControl : StiElementControl, IStiRefreshControl
{
	private StiGaugeControl GaugeControl { get; set; }

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
			GaugeControl?.AnimationEngine?.Finish();
			GaugeControl?.Atom.Render();
			StiGaugeControl gaugeControl = GaugeControl;
			if (gaugeControl != null)
			{
				((Control)gaugeControl).Invalidate();
			}
		}
	}

	public override void CreateChild()
	{
		CreateScrollableContainer();
		CreateSingleControl();
		((Control)this).Controls.Add((Control)(object)GaugeControl);
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
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		StiGaugeControl stiGaugeControl = new StiGaugeControl(base.Element);
		((Control)stiGaugeControl).Dock = (DockStyle)5;
		stiGaugeControl.AnimationEngine = base.AnimationEngine;
		GaugeControl = stiGaugeControl;
		((Control)GaugeControl).MouseEnter += Child_RefreshActionButtons;
		((Control)GaugeControl).MouseLeave += Child_RefreshActionButtons;
		((Control)GaugeControl).MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		StiGaugeControl gaugeControl = GaugeControl;
		if (gaugeControl != null && gaugeControl.Atom?.Iterations.Count > 0)
		{
			((IStiGaugeElement)base.Element).PreviousAnimations = GaugeControl.Atom.Iterations.FirstOrDefault().Gauge.PreviousAnimations;
		}
		GaugeControl?.RefreshChild(dataTable);
		if (GaugeControl.Atom.Iterations.Count > 1)
		{
			SwitchToSingleControl();
		}
		else if (GaugeControl.Atom.Iterations.Count > 0)
		{
			SwitchToScrollableContainer();
		}
		base.AnimationEngine.Start();
		RecalculateSize();
	}

	private void SwitchToSingleControl()
	{
		if (((Control)GaugeControl).Parent != Scrollable)
		{
			((Control)this).Controls.Clear();
			((Control)GaugeControl).Dock = (DockStyle)0;
			((Control)GaugeControl).Location = new Point(0, 0);
			((Control)Scrollable).Controls.Add((Control)(object)GaugeControl);
			((Control)this).Controls.Add((Control)(object)Scrollable);
		}
	}

	private void SwitchToScrollableContainer()
	{
		if (((Control)GaugeControl).Parent == Scrollable)
		{
			((Control)this).Controls.Clear();
			((Control)Scrollable).Controls.Clear();
			((Control)GaugeControl).Dock = (DockStyle)5;
			((Control)this).Controls.Add((Control)(object)GaugeControl);
		}
	}

	private void RecalculateSize()
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)Scrollable).Width + ((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Value - 18 - (((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Visible ? SystemInformation.VerticalScrollBarWidth : 0), ((Control)Scrollable).Height + ((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Value - (((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Visible ? SystemInformation.HorizontalScrollBarHeight : 0));
		((ScrollProperties)((ScrollableControl)Scrollable).HorizontalScroll).Value = 0;
		((ScrollProperties)((ScrollableControl)Scrollable).VerticalScroll).Value = 0;
		GaugeControl?.RecalculateSize(rect);
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
			if (GaugeControl != null)
			{
				((Control)GaugeControl).MouseEnter -= Child_RefreshActionButtons;
				((Control)GaugeControl).MouseLeave -= Child_RefreshActionButtons;
				((Control)GaugeControl).MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
				GaugeControl.AnimationEngine = null;
				((Component)(object)GaugeControl).Dispose();
				GaugeControl = null;
			}
			if (base.AnimationEngine != null)
			{
				base.AnimationEngine.Animation -= AnimationEngine_Animation;
				base.AnimationEngine.Dispose();
				base.AnimationEngine = null;
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

	private void AnimationEngine_Animation(StiComponent component)
	{
		if (GaugeControl.Atom.Iterations.Any((StiGaugeIteration iteration) => iteration.Gauge == component))
		{
			((Control)GaugeControl).Invalidate();
		}
	}

	public StiGaugeElementControl(IStiElement element)
		: base(element)
	{
		base.AnimationEngine.Animation += AnimationEngine_Animation;
	}
}
