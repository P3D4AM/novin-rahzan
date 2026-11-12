using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Dashboard.Viewer.Elements.Cards;

[ToolboxItem(false)]
public class StiCardsElementControl : StiElementControl, IStiRefreshControl
{
	private StiCardsControl CardsControl { get; set; }

	private ScrollableControl Scrollable
	{
		get
		{
			Control child = base.Child;
			return (ScrollableControl)(object)((child is ScrollableControl) ? child : null);
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
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		StiScrollableControl stiScrollableControl = new StiScrollableControl(base.Element);
		((Control)stiScrollableControl).Dock = (DockStyle)5;
		base.Child = (Control)(object)stiScrollableControl;
		base.Child.SizeChanged += Child_SizeChanged;
		base.Child.MouseEnter += Child_RefreshActionButtons;
		base.Child.MouseLeave += Child_RefreshActionButtons;
		base.Child.MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
		StiCardsControl stiCardsControl = new StiCardsControl(base.Element);
		((Control)stiCardsControl).Location = new Point(0, 0);
		CardsControl = stiCardsControl;
		base.Child.Controls.Add((Control)(object)CardsControl);
		((Control)CardsControl).MouseEnter += Child_RefreshActionButtons;
		((Control)CardsControl).MouseLeave += Child_RefreshActionButtons;
		((Control)CardsControl).MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
		((Control)this).Controls.Add(base.Child);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		CardsControl?.RefreshChild(dataTable);
		RecalculateSize();
	}

	private void RecalculateSize()
	{
		Rectangle rect = new Rectangle(0, 0, ((Control)Scrollable).Width + ((ScrollProperties)Scrollable.HorizontalScroll).Value - (((ScrollProperties)Scrollable.VerticalScroll).Visible ? SystemInformation.VerticalScrollBarWidth : 0), ((Control)Scrollable).Height + ((ScrollProperties)Scrollable.VerticalScroll).Value - (((ScrollProperties)Scrollable.HorizontalScroll).Visible ? SystemInformation.HorizontalScrollBarHeight : 0));
		((ScrollProperties)Scrollable.HorizontalScroll).Value = 0;
		((ScrollProperties)Scrollable.VerticalScroll).Value = 0;
		CardsControl?.RecalculateSize(rect);
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		if (disposing)
		{
			if (base.Child != null)
			{
				base.Child.SizeChanged -= Child_SizeChanged;
				base.Child.MouseEnter -= Child_RefreshActionButtons;
				base.Child.MouseLeave -= Child_RefreshActionButtons;
				base.Child.MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
			}
			if (CardsControl != null)
			{
				((Control)CardsControl).MouseEnter -= Child_RefreshActionButtons;
				((Control)CardsControl).MouseLeave -= Child_RefreshActionButtons;
				((Control)CardsControl).MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
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
		Scrollable.AutoScroll = false;
		Scrollable.AutoScroll = true;
	}

	public StiCardsElementControl(IStiElement element)
		: base(element)
	{
	}
}
