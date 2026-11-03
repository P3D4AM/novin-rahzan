using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[DefaultEvent("ValueChanged")]
[ComVisible(false)]
[ToolboxBitmap(typeof(PageNavigator), "PageNavigator.PageNavigator.ico")]
[ToolboxItem(true)]
public class PageNavigator : BaseItemControl
{
	private PageNavigatorItem _Ⱁ;

	private EventHandler Ⱂ;

	private EventHandler Ⱃ;

	private EventHandler Ⱄ;

	protected override Size DefaultSize
	{
		get
		{
			int verticalScrollBarWidth = SystemInformation.VerticalScrollBarWidth;
			int num = verticalScrollBarWidth * 3;
			if (_Ⱁ != null && _Ⱁ.Orientation == eOrientation.Horizontal)
			{
				return new Size(num, verticalScrollBarWidth);
			}
			return new Size(verticalScrollBarWidth, num);
		}
	}

	[Category("Appearance")]
	[DefaultValue(eOrientation.Horizontal)]
	[Description("Indicates control layout orientation.")]
	public eOrientation Orientation
	{
		get
		{
			return _Ⱁ.Orientation;
		}
		set
		{
			if (_Ⱁ.Orientation != value)
			{
				_Ⱁ.Orientation = value;
				RecalcLayout();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates tooltip for the PreviousPage button of the control.")]
	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	public string PreviousPageTooltip
	{
		get
		{
			return _Ⱁ.PreviousPageTooltip;
		}
		set
		{
			_Ⱁ.PreviousPageTooltip = value;
		}
	}

	[Localizable(true)]
	[Browsable(true)]
	[Description("Indicates tooltip for the TodayPage button of the control.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string TodayTooltip
	{
		get
		{
			return _Ⱁ.TodayTooltip;
		}
		set
		{
			_Ⱁ.TodayTooltip = value;
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
			return _Ⱁ.NextPageTooltip;
		}
		set
		{
			_Ⱁ.NextPageTooltip = value;
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
	public event EventHandler NavigateNextPage
	{
		add
		{
			EventHandler eventHandler = Ⱂ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱂ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⱂ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱂ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when NavigateToday button is clicked.")]
	public event EventHandler NavigateToday
	{
		add
		{
			EventHandler eventHandler = Ⱃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⱃ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱃ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when NavigatePreviousPage button is clicked.")]
	public event EventHandler NavigatePreviousPage
	{
		add
		{
			EventHandler eventHandler = Ⱄ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱄ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ⱄ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ⱄ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public PageNavigator()
	{
		_Ⱁ = new PageNavigatorItem();
		_Ⱁ.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)this).Size = _Ⱁ.Size;
		HostItem = _Ⱁ;
		ἧ(Ἠ: true);
	}

	private void ἧ(bool Ἠ)
	{
		if (Ἠ)
		{
			_Ⱁ.NavigatePreviousPage += Ⱅ;
			_Ⱁ.NavigateNextPage += Ⱉ;
			_Ⱁ.NavigateToday += Ⱇ;
		}
		else
		{
			_Ⱁ.NavigatePreviousPage -= Ⱅ;
			_Ⱁ.NavigateNextPage -= Ⱉ;
			_Ⱁ.NavigateToday -= Ⱇ;
		}
	}

	private void Ⱅ(object _٠, EventArgs ؿ)
	{
		Ⱆ();
	}

	private void Ⱆ()
	{
		if (Ⱄ != null)
		{
			Ⱄ(this, EventArgs.Empty);
		}
	}

	private void Ⱇ(object _٠, EventArgs ؿ)
	{
		Ⱈ();
	}

	private void Ⱈ()
	{
		if (Ⱃ != null)
		{
			Ⱃ(this, EventArgs.Empty);
		}
	}

	private void Ⱉ(object _٠, EventArgs ؿ)
	{
		Ⱊ();
	}

	private void Ⱊ()
	{
		if (Ⱂ != null)
		{
			Ⱂ(this, EventArgs.Empty);
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
			ἧ(Ἠ: false);
		}
		base.Dispose(disposing);
	}
}
