using System;
using System.Drawing;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

public sealed class ThumbButton
{
	private ThumbButtonManager _manager;

	private uint id;

	private Icon icon;

	private string tooltip;

	private THBFLAGS flags;

	[CLSCompliant(false)]
	public uint Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public Icon Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
		}
	}

	public string Tooltip
	{
		get
		{
			return tooltip;
		}
		set
		{
			tooltip = value;
		}
	}

	internal THBFLAGS Flags
	{
		get
		{
			return flags;
		}
		set
		{
			flags = value;
		}
	}

	internal THUMBBUTTON Win32ThumbButton
	{
		get
		{
			THUMBBUTTON result = default(THUMBBUTTON);
			result.iId = Id;
			result.szTip = Tooltip;
			result.hIcon = Icon.Handle;
			result.dwFlags = Flags;
			result.dwMask = THBMASK.THB_FLAGS;
			if (!string.IsNullOrEmpty(tooltip))
			{
				result.dwMask |= THBMASK.THB_TOOLTIP;
			}
			if (icon != null)
			{
				result.dwMask |= THBMASK.THB_ICON;
			}
			return result;
		}
	}

	public bool Visible
	{
		get
		{
			return (flags & THBFLAGS.THBF_HIDDEN) == 0;
		}
		set
		{
			if (value)
			{
				flags &= (THBFLAGS)(-9);
			}
			else
			{
				flags |= THBFLAGS.THBF_HIDDEN;
			}
			_manager.RefreshThumbButtons();
		}
	}

	public bool Enabled
	{
		get
		{
			return (flags & THBFLAGS.THBF_DISABLED) == 0;
		}
		set
		{
			if (value)
			{
				flags &= (THBFLAGS)(-2);
			}
			else
			{
				flags |= THBFLAGS.THBF_DISABLED;
			}
			_manager.RefreshThumbButtons();
		}
	}

	public event EventHandler Clicked;

	internal void FireClick()
	{
		if (this.Clicked != null)
		{
			this.Clicked(this, EventArgs.Empty);
		}
	}

	internal ThumbButton(ThumbButtonManager manager, uint id, Icon icon, string tooltip)
	{
		_manager = manager;
		this.id = id;
		this.icon = icon;
		this.tooltip = tooltip;
	}
}
