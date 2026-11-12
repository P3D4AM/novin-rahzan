using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class SliderDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is Slider slider)
		{
			slider.Style = eDotNetBarStyle.StyleManagerControlled;
			((Control)slider).Width = 150;
		}
	}
}
