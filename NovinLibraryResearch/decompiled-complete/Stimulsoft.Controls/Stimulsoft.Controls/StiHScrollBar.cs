using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiHScrollBar : HScrollBar
{
	public bool IsStart => ((ScrollBar)this).Value == 0;

	public bool IsEnd => ((ScrollBar)this).Value == ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange;

	private void SetValue(int value)
	{
		if (value < 0)
		{
			((ScrollBar)this).Value = 0;
		}
		else
		{
			((ScrollBar)this).Value = value;
		}
	}

	public void DoLeft()
	{
		if (((ScrollBar)this).Value - ((ScrollBar)this).SmallChange < 0)
		{
			SetValue(0);
		}
		else
		{
			SetValue(((ScrollBar)this).Value - ((ScrollBar)this).SmallChange);
		}
	}

	public void DoRight()
	{
		if (((ScrollBar)this).Value + ((ScrollBar)this).SmallChange > ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange + 1)
		{
			SetValue(Math.Max(((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange + 1, ((ScrollBar)this).Minimum));
		}
		else
		{
			SetValue(((ScrollBar)this).Value + ((ScrollBar)this).SmallChange);
		}
	}

	public void DoPageLeft()
	{
		if (((ScrollBar)this).Value - ((ScrollBar)this).SmallChange < 0)
		{
			SetValue(0);
		}
		else
		{
			SetValue(((ScrollBar)this).Value - ((ScrollBar)this).SmallChange);
		}
	}

	public void DoPageRight()
	{
		if (((ScrollBar)this).Value + ((ScrollBar)this).SmallChange > ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange)
		{
			SetValue(((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange);
		}
		else
		{
			SetValue(((ScrollBar)this).Value + ((ScrollBar)this).SmallChange);
		}
	}

	public void DoLeftEdge()
	{
		SetValue(0);
		((Control)this).Invalidate();
	}

	public void DoRightEdge()
	{
		SetValue(((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange);
	}
}
