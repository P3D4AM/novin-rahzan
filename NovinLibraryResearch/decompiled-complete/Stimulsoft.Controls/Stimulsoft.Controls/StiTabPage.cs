using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiTabPageDesigner))]
public class StiTabPage : Panel
{
	private Image image;

	private bool drawBorder = true;

	private bool invisible;

	[DefaultValue(null)]
	[Category("Appearance")]
	public virtual Image Image
	{
		get
		{
			return image;
		}
		set
		{
			image = value;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((Panel)this).Text;
		}
		set
		{
			((Panel)this).Text = value;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool DrawBorder
	{
		get
		{
			return drawBorder;
		}
		set
		{
			drawBorder = value;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	public bool Invisible
	{
		get
		{
			return invisible;
		}
		set
		{
			invisible = value;
			if (!((Component)this).DesignMode && ((Control)this).Parent is StiTabControl stiTabControl)
			{
				if (stiTabControl.SelectedTab == null && !invisible)
				{
					stiTabControl.SelectedTab = this;
				}
				else if (stiTabControl.SelectedTab == this)
				{
					foreach (StiTabPage item in (ArrangedElementCollection)((Control)stiTabControl).Controls)
					{
						if (!item.Invisible)
						{
							stiTabControl.SelectedTab = item;
							break;
						}
					}
				}
				if (stiTabControl.SelectedTab == this && invisible)
				{
					stiTabControl.SelectedTab = null;
				}
			}
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	[Browsable(false)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			if (((Control)this).Parent != null && this == ((StiTabControl)(object)((Control)this).Parent).SelectedTab)
			{
				((Control)this).Visible = true;
				Dock = (DockStyle)5;
			}
			else
			{
				((Control)this).Visible = value;
			}
		}
	}

	[Browsable(false)]
	public DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Dock = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	public BorderStyle BorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Panel)this).BorderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Panel)this).BorderStyle = value;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		if (!DrawBorder || Invisible)
		{
			return;
		}
		StiTabControl stiTabControl = ((Control)this).Parent as StiTabControl;
		if (stiTabControl.ControlStyle == StiControlStyle.Office2013Blue)
		{
			if (!stiTabControl.PositionAtBottom)
			{
				Pen val = new Pen(StiStyleOffice2013Blue.ColorLineGray);
				try
				{
					graphics.DrawLine(val, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
					graphics.DrawLine(val, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
					graphics.DrawLine(val, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			Pen val2 = new Pen(StiStyleOffice2013Blue.ColorLineGray);
			try
			{
				graphics.DrawLine(val2, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
				graphics.DrawLine(val2, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
				graphics.DrawLine(val2, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
				return;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (!stiTabControl.PositionAtBottom)
		{
			graphics.DrawLine(SystemPens.ControlLightLight, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
			graphics.DrawLine(SystemPens.ControlDark, rectangle.X, rectangle.Bottom, rectangle.Right, rectangle.Bottom);
			graphics.DrawLine(SystemPens.ControlDark, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
		}
		else
		{
			graphics.DrawLine(SystemPens.ControlLightLight, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom);
			graphics.DrawLine(SystemPens.ControlLightLight, rectangle.X, rectangle.Top, rectangle.Right, rectangle.Top);
			graphics.DrawLine(SystemPens.ControlDark, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
		}
	}

	public StiTabPage()
		: this("TabPage")
	{
	}

	public StiTabPage(string text)
	{
		((Control)this).Text = text;
		DockPaddingEdges dockPadding = DockPadding;
		DockPaddingEdges dockPadding2 = DockPadding;
		DockPaddingEdges dockPadding3 = DockPadding;
		int num2 = (DockPadding.Right = 4);
		int num4 = (dockPadding3.Bottom = num2);
		int top = (dockPadding2.Left = num4);
		dockPadding.Top = top;
		Visible = false;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
	}
}
