using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiVScrollBar : VScrollBar
{
	public bool IsStart => ((ScrollBar)this).Value == 0;

	public bool IsEnd => ((ScrollBar)this).Value == ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange;

	private void SetValue(int value)
	{
		((ScrollBar)this).Value = ((value >= 0) ? value : 0);
	}

	public void DoUp()
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

	public void DoDown()
	{
		if (((ScrollBar)this).Value + ((ScrollBar)this).SmallChange > ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange + 1)
		{
			SetValue(((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange + 1);
		}
		else
		{
			SetValue(((ScrollBar)this).Value + ((ScrollBar)this).SmallChange);
		}
	}

	public void DoPageUp()
	{
		if (((ScrollBar)this).Value - ((ScrollBar)this).LargeChange < 0)
		{
			SetValue(0);
		}
		else
		{
			SetValue(((ScrollBar)this).Value - ((ScrollBar)this).LargeChange);
		}
	}

	public void DoPageDown()
	{
		if (((ScrollBar)this).Value + ((ScrollBar)this).LargeChange >= ((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange)
		{
			SetValue(((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange);
		}
		else
		{
			SetValue(((ScrollBar)this).Value + ((ScrollBar)this).LargeChange);
		}
	}

	public void DoTop()
	{
		SetValue(0);
		((Control)this).Invalidate();
	}

	public void DoBottom()
	{
		SetValue(((ScrollBar)this).Maximum - ((ScrollBar)this).LargeChange);
	}
}
