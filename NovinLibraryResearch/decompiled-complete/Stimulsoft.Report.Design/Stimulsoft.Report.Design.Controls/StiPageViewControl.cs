using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public class StiPageViewControl : StiScrollableControl
{
	public int PageIndex { get; set; }

	public StiPage Page
	{
		get
		{
			if (Designer == null || Designer.Report == null || Designer.Report.Pages.Count <= PageIndex)
			{
				return null;
			}
			return Designer.Report.Pages[PageIndex];
		}
	}

	internal StiDesignerControl Designer { get; set; }

	public StiViewControl View { get; set; }

	public void DisposeRes()
	{
		if (Designer != null)
		{
			Designer.RefreshDesigner -= OnRefresh;
		}
		Designer = null;
		View.DisposeRes();
		View = null;
	}

	public void SetPageViewLocation()
	{
		Rectangle pageViewLocation = StiPageViewHelper.GetPageViewLocation(Page);
		if (pageViewLocation.X > StiScale.I10)
		{
			pageViewLocation.X++;
		}
		if (Page != null && (((Control)View).Width != pageViewLocation.Width || ((Control)View).Height != pageViewLocation.Height || ScrollWidth != pageViewLocation.Width + StiScale.I20 || ScrollHeight != pageViewLocation.Height + StiScale.I20))
		{
			((Control)this).SuspendLayout();
			if (((Control)View).Width != pageViewLocation.Width)
			{
				((Control)View).Width = pageViewLocation.Width;
			}
			if (((Control)View).Height != pageViewLocation.Height)
			{
				((Control)View).Height = pageViewLocation.Height;
			}
			int num = pageViewLocation.Width + StiPageViewHelper.PageViewHorDistance * 2;
			int num2 = pageViewLocation.Height + StiPageViewHelper.PageViewVerDistance * 2;
			if (ScrollWidth != num)
			{
				ScrollWidth = num;
			}
			if (ScrollHeight != num2)
			{
				ScrollHeight = num2;
			}
			((Control)this).ResumeLayout(false);
		}
	}

	public virtual void OnHorScroll(object sender, EventArgs args)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is StiHorRollerLine stiHorRollerLine)
			{
				stiHorRollerLine.OnScroll(sender, null);
			}
		}
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
	}

	public virtual void OnVerScroll(object sender, EventArgs args)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val is StiVerRollerLine stiVerRollerLine)
			{
				stiVerRollerLine.OnScroll(sender, null);
			}
		}
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
	}

	private void OnRefresh(object sender, EventArgs e)
	{
		SetPageViewLocation();
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)View).Focus();
		Designer.Report.ResetSelection();
		Designer.Report.Select();
		StiDockingPanelSelector.PropertiesPanelSelect(Designer);
		Designer.AddSelectedToPropertyGrid();
		((Control)Designer).Refresh();
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
		StiActions.MainMenu.InvokeReportSetup(Designer);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		if (Page != null && Page.IsDashboard)
		{
			SolidBrush val = new SolidBrush(Color.FromArgb(255, 242, 242, 242));
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, e.ClipRectangle);
				return;
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		((Control)this).OnPaint(e);
	}

	public StiPageViewControl(StiDesignerControl designer)
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).TabStop = true;
		Designer = designer;
		designer.RefreshDesigner += OnRefresh;
		if (StiOptions.Designer.BackgroundColor is Color)
		{
			((Control)this).BackColor = (Color)StiOptions.Designer.BackgroundColor;
		}
		else
		{
			((Control)this).BackColor = StiGuiHelper.GetColorPageViewBackground();
		}
		View = new StiViewControl();
		((Control)View).Location = new Point(StiPageViewHelper.PageViewHorDistance, StiPageViewHelper.PageViewVerDistance);
		((Control)this).Controls.Add((Control)(object)View);
		((ScrollBar)base.VScrollBar).ValueChanged += OnVerScroll;
		((ScrollBar)base.HScrollBar).ValueChanged += OnHorScroll;
	}
}
