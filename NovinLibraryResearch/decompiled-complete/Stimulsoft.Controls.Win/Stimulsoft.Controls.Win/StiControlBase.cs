using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win;

[ToolboxItem(false)]
public class StiControlBase : Control
{
	private bool flat = true;

	[DefaultValue(true)]
	[Category("Appearance")]
	public virtual bool Flat
	{
		get
		{
			return flat;
		}
		set
		{
			flat = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsMouseOver { get; set; }

	[Category("Behavior")]
	[DefaultValue(true)]
	public bool HotFocus { get; set; } = true;


	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		if (Flat)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((Control)this).OnLostFocus(e);
		if (Flat)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		if (Flat)
		{
			IsMouseOver = true;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		if (Flat)
		{
			IsMouseOver = false;
			((Control)this).Invalidate();
		}
	}

	public StiControlBase()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).TabStop = true;
	}
}
