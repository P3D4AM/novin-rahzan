using System;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win.Editors.DateTimeAdv;

internal class CenturySelectorControl : BaseItemControl
{
	private CenturySelectorContainer _CenturySelector;

	public CenturySelectorContainer CenturySelector => _CenturySelector;

	public CenturySelectorControl()
	{
		_CenturySelector = new CenturySelectorContainer();
		HostItem = _CenturySelector;
		FocusCuesEnabled = false;
	}

	protected override void OnResize(EventArgs e)
	{
		_CenturySelector.FixedSize = ((Control)this).Size;
		base.OnResize(e);
	}
}
