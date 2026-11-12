using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiTabulatorPageDesigner))]
public class StiTabulatorPage : ContainerControl
{
	private Image image;

	private bool invisible;

	internal Size TitleSize { get; set; }

	public bool IsMouseOver
	{
		get
		{
			if (((Control)this).Parent == null)
			{
				return false;
			}
			Graphics val = Graphics.FromHwnd(((Control)this).Handle);
			try
			{
				return ((StiTabulator)(object)((Control)this).Parent).GetTitlePageRectangle(val, this).Contains(((Control)this).Parent.PointToClient(Cursor.Position));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public int PageIndex
	{
		get
		{
			if (((Control)this).Parent == null)
			{
				return -1;
			}
			return ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
		}
	}

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
			MeasureTitle();
			((Control)this).Invalidate();
			(((Control)this).Parent as StiTabulator)?.UpdateTitle();
		}
	}

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
			MeasureTitle();
			((Control)this).Invalidate();
			(((Control)this).Parent as StiTabulator)?.UpdateTitle();
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
			if (invisible == value)
			{
				return;
			}
			invisible = value;
			if (!((Component)this).DesignMode && ((Control)this).Parent is StiTabulator stiTabulator)
			{
				if (stiTabulator.SelectedTab == null && !invisible)
				{
					stiTabulator.SelectedTab = this;
				}
				else if (stiTabulator.SelectedTab == this)
				{
					foreach (StiTabulatorPage item in (ArrangedElementCollection)((Control)stiTabulator).Controls)
					{
						if (!item.Invisible)
						{
							stiTabulator.SelectedTab = item;
							break;
						}
					}
				}
				if (stiTabulator.SelectedTab == this && invisible)
				{
					stiTabulator.SelectedTab = null;
				}
			}
			((Control)this).Invalidate();
			Control parent = ((Control)this).Parent;
			if (parent != null)
			{
				parent.Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	public bool InvisibleVisible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			if (((Control)this).Parent != null && this == ((StiTabulator)(object)((Control)this).Parent).SelectedTab)
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

	internal void MeasureTitle()
	{
		if (((Control)this).Parent == null)
		{
			return;
		}
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			TitleSize = ((StiTabulator)(object)((Control)this).Parent).Mode.MeasureTitle(val, this);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((ContainerControl)this).OnParentChanged(e);
		MeasureTitle();
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
		Graphics graphics = p.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (((Control)this).Parent != null)
		{
			StiDrawing.FillRectangle(graphics, StiUX.ButtonBackground, rect);
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		if (((Control)this).Parent != null)
		{
			((StiTabulator)(object)((Control)this).Parent).Mode.DrawPage(p.Graphics, this);
		}
	}

	public StiTabulatorPage()
		: this("TabulatorPage")
	{
	}

	public StiTabulatorPage(string text)
	{
		((Control)this).Text = text;
		DockPaddingEdges dockPadding = DockPadding;
		DockPaddingEdges dockPadding2 = DockPadding;
		int num2 = (DockPadding.Bottom = 0);
		int top = (dockPadding2.Left = num2);
		dockPadding.Top = top;
		DockPadding.Right = StiScaleUI.I1;
		((Control)this).Visible = false;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
	}
}
