using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiCloudTabulatorPageDesigner))]
public sealed class StiCloudTabulatorPage : ContainerControl
{
	internal Size TitleSize;

	private Rectangle titleRect = Rectangle.Empty;

	private bool showMarker;

	private bool showSeparator;

	private Image image;

	private bool invisible;

	internal Rectangle TitleRect
	{
		get
		{
			return titleRect;
		}
		set
		{
			titleRect = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowMarker
	{
		get
		{
			return showMarker;
		}
		set
		{
			if (showMarker != value)
			{
				showMarker = value;
				Control parent = ((Control)this).Parent;
				if (parent != null)
				{
					parent.Invalidate();
				}
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowSeparator
	{
		get
		{
			return showSeparator;
		}
		set
		{
			if (showSeparator != value)
			{
				showSeparator = value;
				Control parent = ((Control)this).Parent;
				if (parent != null)
				{
					parent.Invalidate();
				}
			}
		}
	}

	[DefaultValue(true)]
	public bool ShowSelected { get; set; } = true;


	public int PageIndex
	{
		get
		{
			if (((Control)this).Parent != null)
			{
				return ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
			}
			return -1;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	public Image Image
	{
		get
		{
			return image;
		}
		set
		{
			image = value;
			if (((Control)this).Parent != null)
			{
				((StiCloudTabulator)(object)((Control)this).Parent).UpdateTitle();
			}
			((Control)this).Invalidate();
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
			if (((Control)this).Parent != null)
			{
				((StiCloudTabulator)(object)((Control)this).Parent).UpdateTitle();
			}
			((Control)this).Invalidate();
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
			StiCloudTabulator stiCloudTabulator = ((Control)this).Parent as StiCloudTabulator;
			if (!((Component)this).DesignMode && stiCloudTabulator != null)
			{
				if (stiCloudTabulator.SelectedTab == null && !invisible)
				{
					stiCloudTabulator.SelectedTab = this;
				}
				else if (stiCloudTabulator.SelectedTab == this)
				{
					for (int i = 0; i < ((ArrangedElementCollection)((Control)stiCloudTabulator).Controls).Count; i++)
					{
						if (((Control)stiCloudTabulator).Controls[i] is StiCloudTabulatorPage { Invisible: false } stiCloudTabulatorPage)
						{
							stiCloudTabulator.SelectedTab = stiCloudTabulatorPage;
							break;
						}
					}
				}
				if (stiCloudTabulator.SelectedTab == this && invisible)
				{
					stiCloudTabulator.SelectedTab = null;
				}
			}
			((Control)this).Invalidate();
			stiCloudTabulator?.CheckScrollBar();
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
			if (((Control)this).Parent != null && this == ((StiCloudTabulator)(object)((Control)this).Parent).SelectedTab)
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

	public event EventHandler TabClick;

	internal void MeasureTitle()
	{
		if (((Control)this).Parent != null)
		{
			Graphics val = Graphics.FromHwnd(((Control)this).Handle);
			try
			{
				TitleSize = ((StiCloudTabulator)(object)((Control)this).Parent).Mode.MeasureTitle(val, this);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	internal void InvokeMouseClick(MouseEventArgs e)
	{
		if (this.TabClick != null)
		{
			this.TabClick(this, (EventArgs)(object)e);
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
		Graphics graphics = p.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (((Control)this).Parent != null)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Background, rect);
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		SolidBrush val = new SolidBrush(Color.FromArgb(198, 198, 198));
		try
		{
			graphics.FillRectangle((Brush)(object)val, 0, 0, 1, 1);
			graphics.FillRectangle((Brush)(object)val, rectangle.Right, 0, 1, 1);
			graphics.FillRectangle((Brush)(object)val, rectangle.Right, rectangle.Bottom, 1, 1);
			graphics.FillRectangle((Brush)(object)val, 0, rectangle.Bottom, 1, 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (!ShowSeparator)
		{
			return;
		}
		Pen val2 = new Pen(StiUX.Separator);
		try
		{
			if (!StiUX.IsDark)
			{
				val2.DashPattern = new float[2] { 1f, 2f };
			}
			if (((Control)this).Parent is StiCloudTabulator { TitlePosition: StiTabTitlePosition.LeftHorizontal })
			{
				graphics.DrawLine(val2, rectangle.Left, rectangle.Top, rectangle.Left, rectangle.Bottom);
			}
			else
			{
				graphics.DrawLine(val2, rectangle.Right, rectangle.Top, rectangle.Right, rectangle.Bottom);
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public StiCloudTabulatorPage()
		: this("StiCloudTabulatorPage")
	{
	}

	public StiCloudTabulatorPage(string text)
	{
		((Control)this).Text = text;
		DockPaddingEdges dockPadding = DockPadding;
		DockPaddingEdges dockPadding2 = DockPadding;
		int num2 = (DockPadding.Left = 0);
		int top = (dockPadding2.Bottom = num2);
		dockPadding.Top = top;
		DockPadding.Right = 1;
		((Control)this).Visible = false;
		ShowSeparator = true;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
	}
}
