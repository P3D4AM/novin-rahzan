using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiScrollableControl), "Toolbox.StiScrollableControl.bmp")]
public class StiScrollableControl : ScrollableControl
{
	private Panel edgePanel = new Panel();

	private int scrollWidth = 100;

	private int scrollHeight = 100;

	private int scrollTop;

	private int scrollLeft;

	[Browsable(false)]
	public Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	public StiVScrollBar VScrollBar { get; set; }

	[Browsable(false)]
	public StiHScrollBar HScrollBar { get; set; }

	[Category("Behavior")]
	public virtual int ScrollWidth
	{
		get
		{
			return scrollWidth;
		}
		set
		{
			scrollWidth = value;
			SetPos();
			Draw();
		}
	}

	[Category("Behavior")]
	public virtual int ScrollHeight
	{
		get
		{
			return scrollHeight;
		}
		set
		{
			scrollHeight = value;
			SetPos();
			Draw();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ScrollTop
	{
		get
		{
			return scrollTop;
		}
		set
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			int num = value - scrollTop;
			if (num == 0)
			{
				return;
			}
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (!IsScrollBar(val))
				{
					val.Top += num;
				}
			}
			scrollTop = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ScrollLeft
	{
		get
		{
			return scrollLeft;
		}
		set
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			int num = value - scrollLeft;
			if (num == 0)
			{
				return;
			}
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (!IsScrollBar(val))
				{
					val.Left += num;
				}
			}
			scrollLeft = value;
		}
	}

	[Browsable(false)]
	public int ClientWidth
	{
		get
		{
			if (((Control)VScrollBar).Visible)
			{
				return ((Control)this).Width - StiScaleUI.I16;
			}
			return ((Control)this).Width;
		}
	}

	[Browsable(false)]
	public int ClientHeight
	{
		get
		{
			if (((Control)HScrollBar).Visible)
			{
				return ((Control)this).Height - StiScaleUI.I16;
			}
			return ((Control)this).Height;
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		SetPos();
		((ScrollableControl)this).SetDisplayRectLocation(StiScaleUI.I10, StiScaleUI.I10);
	}

	private void OnValueChanged(object sender, EventArgs e)
	{
		SetPos();
		Draw();
	}

	private void Draw()
	{
		((Control)this).Invalidate(((Control)this).ClientRectangle);
	}

	private bool IsScrollBar(Control control)
	{
		if (control != VScrollBar && control != HScrollBar)
		{
			return control == edgePanel;
		}
		return true;
	}

	protected void SetPos()
	{
		try
		{
			if (ClientWidth >= ScrollWidth)
			{
				((Control)HScrollBar).Visible = false;
			}
			else
			{
				((Control)HScrollBar).Visible = true;
			}
			if (ClientHeight >= ScrollHeight)
			{
				((Control)VScrollBar).Visible = false;
			}
			else
			{
				((Control)VScrollBar).Visible = true;
			}
			((Control)VScrollBar).Width = StiScaleUI.I16;
			if (((Control)HScrollBar).Visible)
			{
				((Control)VScrollBar).Height = ((Control)this).Height - StiScaleUI.I16;
			}
			else
			{
				((Control)VScrollBar).Height = ((Control)this).Height;
			}
			((Control)VScrollBar).Top = 0;
			((Control)VScrollBar).Left = ((Control)this).Width - ((Control)VScrollBar).Width;
			((Control)HScrollBar).Width = ((Control)this).Width - StiScaleUI.I16;
			if (((Control)VScrollBar).Visible)
			{
				((Control)HScrollBar).Width = ((Control)this).Width - StiScaleUI.I16;
			}
			else
			{
				((Control)HScrollBar).Width = ((Control)this).Width;
			}
			((Control)HScrollBar).Height = StiScaleUI.I16;
			((Control)HScrollBar).Left = 0;
			((Control)HScrollBar).Top = ((Control)this).Height - ((Control)HScrollBar).Height;
			((Control)edgePanel).Left = ((Control)HScrollBar).Right;
			((Control)edgePanel).Top = ((Control)VScrollBar).Bottom;
			((Control)edgePanel).Width = ((Control)VScrollBar).Width;
			((Control)edgePanel).Height = ((Control)HScrollBar).Height;
			((Control)edgePanel).Visible = ((Control)HScrollBar).Visible & ((Control)VScrollBar).Visible;
			((Control)edgePanel).BackColor = SystemColors.Control;
			ScrollTop = -((ScrollBar)VScrollBar).Value;
			ScrollLeft = -((ScrollBar)HScrollBar).Value;
			((ScrollBar)VScrollBar).Minimum = 0;
			((ScrollBar)VScrollBar).Maximum = ScrollHeight;
			((ScrollBar)VScrollBar).LargeChange = Math.Max(ClientHeight, 0);
			((ScrollBar)VScrollBar).SmallChange = ((ScrollBar)VScrollBar).LargeChange / StiScaleUI.I10;
			((ScrollBar)HScrollBar).Minimum = 0;
			((ScrollBar)HScrollBar).Maximum = ScrollWidth;
			((ScrollBar)HScrollBar).LargeChange = Math.Max(ClientWidth, 0);
			((ScrollBar)HScrollBar).SmallChange = ((ScrollBar)HScrollBar).LargeChange / StiScaleUI.I10;
		}
		catch
		{
		}
	}

	public StiScrollableControl()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)1, false);
		VScrollBar = new StiVScrollBar();
		((ScrollBar)VScrollBar).ValueChanged += OnValueChanged;
		HScrollBar = new StiHScrollBar();
		((ScrollBar)HScrollBar).ValueChanged += OnValueChanged;
		((Control)this).Controls.Add((Control)(object)VScrollBar);
		((Control)this).Controls.Add((Control)(object)HScrollBar);
		((Control)this).Controls.Add((Control)(object)edgePanel);
		SetPos();
		((Control)this).Width = 100;
		((Control)this).Height = 100;
		((Control)this).ClientSize = new Size(40, 40);
	}
}
