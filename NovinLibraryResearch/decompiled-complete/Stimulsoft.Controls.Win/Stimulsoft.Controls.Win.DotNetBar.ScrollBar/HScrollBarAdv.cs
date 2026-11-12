using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(HScrollBarAdv), "ScrollBar.HScrollBarAdv.ico")]
public class HScrollBarAdv : ScrollBarAdv
{
	public bool IsStart => base.Value == 0;

	public bool IsEnd => base.Value == base.Maximum - base.LargeChange;

	protected override bool IsVertical()
	{
		return false;
	}

	private void SetValue(int value)
	{
		if (value < 0)
		{
			base.Value = 0;
		}
		else
		{
			base.Value = value;
		}
	}

	public void DoLeft()
	{
		if (base.Value - base.SmallChange < 0)
		{
			SetValue(0);
		}
		else
		{
			SetValue(base.Value - base.SmallChange);
		}
	}

	public void DoRight()
	{
		if (base.Value + base.SmallChange > base.Maximum - base.LargeChange + 1)
		{
			SetValue(Math.Max(base.Maximum - base.LargeChange + 1, base.Minimum));
		}
		else
		{
			SetValue(base.Value + base.SmallChange);
		}
	}

	public void DoPageLeft()
	{
		if (base.Value - base.SmallChange < 0)
		{
			SetValue(0);
		}
		else
		{
			SetValue(base.Value - base.SmallChange);
		}
	}

	public void DoPageRight()
	{
		if (base.Value + base.SmallChange > base.Maximum - base.LargeChange)
		{
			SetValue(base.Maximum - base.LargeChange);
		}
		else
		{
			SetValue(base.Value + base.SmallChange);
		}
	}

	public void DoLeftEdge()
	{
		SetValue(0);
		((Control)this).Invalidate();
	}

	public void DoRightEdge()
	{
		SetValue(base.Maximum - base.LargeChange);
	}
}
