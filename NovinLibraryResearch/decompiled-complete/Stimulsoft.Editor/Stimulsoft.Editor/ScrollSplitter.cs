using System.ComponentModel;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
public class ScrollSplitter : ScrollingButton
{
	public ScrollSplitter()
	{
		base.Button.BorderStyle = EditBorderStyle.Fixed3D;
	}
}
