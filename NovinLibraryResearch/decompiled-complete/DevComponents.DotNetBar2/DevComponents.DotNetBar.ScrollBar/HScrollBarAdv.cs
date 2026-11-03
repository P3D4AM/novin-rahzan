using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.ScrollBar;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(HScrollBarAdv), "ScrollBar.HScrollBarAdv.ico")]
public class HScrollBarAdv : ScrollBarAdv
{
	protected override bool IsVertical()
	{
		return false;
	}
}
