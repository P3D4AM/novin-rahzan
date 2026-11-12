using System;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

internal class YearSelectorControl : BaseItemControl
{
	private YearSelectorContainer _YearSelector;

	public YearSelectorContainer YearSelector => _YearSelector;

	public YearSelectorControl()
	{
		_YearSelector = new YearSelectorContainer();
		HostItem = _YearSelector;
		FocusCuesEnabled = false;
	}

	protected override void OnResize(EventArgs e)
	{
		_YearSelector.FixedSize = ((Control)this).Size;
		base.OnResize(e);
	}
}
