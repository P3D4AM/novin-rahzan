using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons;

public sealed class ThumbButtonManager
{
	private bool _buttonsLoaded;

	private Dictionary<uint, ThumbButton> _thumbButtons = new Dictionary<uint, ThumbButton>();

	private IntPtr _hwnd;

	[CLSCompliant(false)]
	public ThumbButton this[uint id] => _thumbButtons[id];

	public ThumbButtonManager(IntPtr hwnd)
	{
		_hwnd = hwnd;
	}

	[CLSCompliant(false)]
	public ThumbButton CreateThumbButton(uint id, Icon icon, string tooltip)
	{
		return new ThumbButton(this, id, icon, tooltip);
	}

	public void DispatchMessage(ref Message message)
	{
		ulong num = (ulong)((Message)(ref message)).WParam.ToInt64();
		uint num2 = (uint)(num & 0xFFFFFFFFu);
		if (((Message)(ref message)).Msg == 273 && num2 >> 16 == 6144)
		{
			uint key = num2 & 0xFFFFu;
			_thumbButtons[key].FireClick();
		}
	}

	public void AddThumbButtons(params ThumbButton[] buttons)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			_thumbButtons.Add(buttons[i].Id, buttons[i]);
		}
		RefreshThumbButtons();
	}

	internal void RefreshThumbButtons()
	{
		int num = 0;
		THUMBBUTTON[] array = new THUMBBUTTON[_thumbButtons.Count];
		foreach (ThumbButton value in _thumbButtons.Values)
		{
			array[num] = value.Win32ThumbButton;
			num++;
		}
		if (_buttonsLoaded)
		{
			Windows7Taskbar.TaskbarList.ThumbBarUpdateButtons(_hwnd, (uint)array.Length, array);
			return;
		}
		Windows7Taskbar.TaskbarList.ThumbBarAddButtons(_hwnd, (uint)array.Length, array);
		_buttonsLoaded = true;
	}
}
