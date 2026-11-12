using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.ScrollBar;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(VScrollBarAdv), "ScrollBar.VScrollBarAdv.ico")]
public class VScrollBarAdv : ScrollBarAdv
{
	public bool IsStart => base.Value == 0;

	public bool IsEnd => base.Value == base.Maximum - base.LargeChange;

	protected override bool IsVertical()
	{
		return true;
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

	public void DoUp()
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

	public void DoDown()
	{
		if (base.Value + base.SmallChange > base.Maximum - base.LargeChange + 1)
		{
			SetValue(base.Maximum - base.LargeChange + 1);
		}
		else
		{
			SetValue(base.Value + base.SmallChange);
		}
	}

	public void DoPageUp()
	{
		if (base.Value - base.LargeChange < 0)
		{
			SetValue(0);
		}
		else
		{
			SetValue(base.Value - base.LargeChange);
		}
	}

	public void DoPageDown()
	{
		if (base.Value + base.LargeChange >= base.Maximum - base.LargeChange)
		{
			SetValue(base.Maximum - base.LargeChange);
		}
		else
		{
			SetValue(base.Value + base.LargeChange);
		}
	}

	public void DoTop()
	{
		SetValue(0);
		((Control)this).Invalidate();
	}

	public void DoBottom()
	{
		SetValue(base.Maximum - base.LargeChange);
	}
}
