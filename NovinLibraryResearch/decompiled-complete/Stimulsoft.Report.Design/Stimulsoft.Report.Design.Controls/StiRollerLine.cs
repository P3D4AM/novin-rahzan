using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
internal abstract class StiRollerLine : Panel
{
	private StiPageViewControl pageView;

	private Rectangle lastRollerLocation = Rectangle.Empty;

	protected StiDesignerControl designer;

	public StiPage Page
	{
		get
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			if (((Control)this).Parent == null)
			{
				return null;
			}
			int num = -1;
			foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
			{
				Control val = item;
				if (val is StiPageViewControl)
				{
					num = ((StiPageViewControl)(object)val).PageIndex;
					break;
				}
			}
			if (num == -1)
			{
				return null;
			}
			if (designer == null || designer.Report == null)
			{
				return null;
			}
			if (num >= designer.Report.Pages.Count)
			{
				return null;
			}
			return designer.Report.Pages[num];
		}
	}

	public StiPageViewControl PageView
	{
		get
		{
			return pageView;
		}
		set
		{
			pageView = value;
		}
	}

	public Rectangle LastRollerLocation
	{
		get
		{
			return lastRollerLocation;
		}
		set
		{
			lastRollerLocation = value;
		}
	}

	public double RollerStep => Page.Unit.RollerStep;

	public double Factor => Page.Zoom * Page.Unit.Factor * StiScale.Factor;

	public void Draw(Graphics g, Rectangle clipRect)
	{
		Graphics val = g;
		if (val == null)
		{
			val = ((Control)this).CreateGraphics();
		}
		DrawLine(val, clipRect);
		Rectangle rollerRect = GetRollerRect();
		LastRollerLocation = rollerRect;
		if (g == null)
		{
			val.Dispose();
		}
	}

	public abstract void DrawLine(Graphics g, Rectangle clipRect);

	public void UpdateRoller()
	{
		((Control)this).Invalidate();
	}

	public abstract Rectangle GetRollerRect();

	public virtual void DisposeRes()
	{
		pageView = null;
		designer = null;
	}

	public void OnMouseMove(object sender, MouseEventArgs e)
	{
		UpdateRoller();
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (designer == null || designer.Report == null)
		{
			return;
		}
		try
		{
			Draw(e.Graphics, e.ClipRectangle);
		}
		catch
		{
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		UpdateRoller();
	}

	public StiRollerLine(StiDesignerControl designer, StiPage page, StiPageViewControl pageView)
	{
		this.designer = designer;
		this.pageView = pageView;
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
	}
}
