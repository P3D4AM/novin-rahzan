using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar.Design;

public class RangeSliderDesigner : ControlDesigner
{
	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is RangeSlider rangeSlider)
		{
			rangeSlider.Style = eDotNetBarStyle.StyleManagerControlled;
			((Control)rangeSlider).Width = 140;
			((Control)rangeSlider).Height = 24;
			rangeSlider.FocusCuesEnabled = false;
		}
	}
}
