using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(PageNavigator), "PageNavigator.PageNavigator.ico")]
[ToolboxItem(false)]
[DefaultEvent("ValueChanged")]
[ComVisible(false)]
public class PageNavigator : BaseItemControl
{
	private PageNavigatorItem _PageNavItem;

	protected override Size DefaultSize
	{
		get
		{
			int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
			int num = verticalScrollBarWidth * 3;
			if (_PageNavItem != null && _PageNavItem.Orientation == eOrientation.Horizontal)
			{
				return new Size(num, verticalScrollBarWidth);
			}
			return new Size(verticalScrollBarWidth, num);
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates control layout orientation.")]
	public eOrientation Orientation
	{
		get
		{
			return _PageNavItem.Orientation;
		}
		set
		{
			if (_PageNavItem.Orientation != value)
			{
				_PageNavItem.Orientation = value;
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the PreviousPage button of the control.")]
	public string PreviousPageTooltip
	{
		get
		{
			return _PageNavItem.PreviousPageTooltip;
		}
		set
		{
			_PageNavItem.PreviousPageTooltip = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the TodayPage button of the control.")]
	public string TodayTooltip
	{
		get
		{
			return _PageNavItem.TodayTooltip;
		}
		set
		{
			_PageNavItem.TodayTooltip = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the NextPage button of the control.")]
	public string NextPageTooltip
	{
		get
		{
			return _PageNavItem.NextPageTooltip;
		}
		set
		{
			_PageNavItem.NextPageTooltip = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(eDotNetBarStyle.StyleManagerControlled)]
	public override eDotNetBarStyle Style
	{
		get
		{
			return base.Style;
		}
		set
		{
			base.Style = value;
		}
	}

	[Description("Occurs when NavigateNextPage button is clicked.")]
	public event EventHandler NavigateNextPage;

	[Description("Occurs when NavigateToday button is clicked.")]
	public event EventHandler NavigateToday;

	[Description("Occurs when NavigatePreviousPage button is clicked.")]
	public event EventHandler NavigatePreviousPage;

	public PageNavigator()
	{
		_PageNavItem = new PageNavigatorItem();
		_PageNavItem.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)this).Size = _PageNavItem.Size;
		HostItem = _PageNavItem;
		HookEvents(hook: true);
	}

	private void HookEvents(bool hook)
	{
		if (hook)
		{
			_PageNavItem.NavigatePreviousPage += PageNavNavigatePreviousPage;
			_PageNavItem.NavigateNextPage += PageNavNavigateNextPage;
			_PageNavItem.NavigateToday += PageNavNavigateToday;
		}
		else
		{
			_PageNavItem.NavigatePreviousPage -= PageNavNavigatePreviousPage;
			_PageNavItem.NavigateNextPage -= PageNavNavigateNextPage;
			_PageNavItem.NavigateToday -= PageNavNavigateToday;
		}
	}

	private void PageNavNavigatePreviousPage(object sender, EventArgs e)
	{
		OnNavigatePreviousPage();
	}

	private void OnNavigatePreviousPage()
	{
		if (this.NavigatePreviousPage != null)
		{
			this.NavigatePreviousPage(this, EventArgs.Empty);
		}
	}

	private void PageNavNavigateToday(object sender, EventArgs e)
	{
		OnNavigateToday();
	}

	private void OnNavigateToday()
	{
		if (this.NavigateToday != null)
		{
			this.NavigateToday(this, EventArgs.Empty);
		}
	}

	private void PageNavNavigateNextPage(object sender, EventArgs e)
	{
		OnNavigateNextPage();
	}

	private void OnNavigateNextPage()
	{
		if (this.NavigateNextPage != null)
		{
			this.NavigateNextPage(this, EventArgs.Empty);
		}
	}

	public override void RecalcLayout()
	{
		base.RecalcLayout();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			HookEvents(hook: false);
		}
		((Control)this).Dispose(disposing);
	}
}
