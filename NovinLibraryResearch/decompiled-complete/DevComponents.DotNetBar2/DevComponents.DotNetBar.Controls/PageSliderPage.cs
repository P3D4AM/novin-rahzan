using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.PageSliderPageDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class PageSliderPage : Panel
{
	private int __2868 = 1;

	[DefaultValue(1)]
	[Category("Behavior")]
	[Description("Indicates page number. Page number determines the order in which pages are displayed inside of the PageSlider control.")]
	public int PageNumber
	{
		get
		{
			return __2868;
		}
		set
		{
			if (value != __2868)
			{
				int oldValue = __2868;
				__2868 = value;
				OnPageNumberChanged(oldValue, value);
			}
		}
	}

	protected virtual void OnPageNumberChanged(int oldValue, int newValue)
	{
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && ((Control)this).Parent is PageSlider pageSlider)
		{
			pageSlider._288E();
		}
		((Control)this).OnMouseDown(e);
	}
}
