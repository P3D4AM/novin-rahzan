using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
public class PageSliderPage : Panel
{
	private int _PageNumber = 1;

	[DefaultValue(1)]
	[Category("Behavior")]
	[Description("Indicates page number. Page number determines the order in which pages are displayed inside of the PageSlider control.")]
	public int PageNumber
	{
		get
		{
			return _PageNumber;
		}
		set
		{
			if (value != _PageNumber)
			{
				int pageNumber = _PageNumber;
				_PageNumber = value;
				OnPageNumberChanged(pageNumber, value);
			}
		}
	}

	protected virtual void OnPageNumberChanged(int oldValue, int newValue)
	{
	}
}
