using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
[ComVisible(false)]
public class PopupContainer : Control
{
	private const int ग = 33;

	private const int घ = 3;

	private const int ङ = 4;

	private const uint च = 2147483648u;

	private const uint छ = 67108864u;

	private const uint ज = 33554432u;

	private const int झ = 8;

	private const int ञ = 128;

	private const int ᒹ = 5;

	private ഓ _0B84;

	private bool ध;

	private MenuPanel ᒷ
	{
		get
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is MenuPanel)
			{
				return ((Control)this).Controls[0] as MenuPanel;
			}
			return null;
		}
	}

	private Bar ᒸ
	{
		get
		{
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0 && ((Control)this).Controls[0] is Bar)
			{
				return ((Control)this).Controls[0] as Bar;
			}
			return null;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 128;
			createParams.Caption = "";
			return createParams;
		}
	}

	public bool ShowDropShadow
	{
		get
		{
			return ध;
		}
		set
		{
			ध = value;
		}
	}

	public PopupContainer()
	{
		((Control)this).IsAccessible = false;
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void Dispose(bool disposing)
	{
		if (_0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((Control)this).OnVisibleChanged(e);
		if (!((Control)this).Visible && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}

	public void ShowShadow()
	{
		_089F.SetWindowPos(((Control)this).Handle, new IntPtr(-1), 0, 0, 0, 0, 19);
		if (ध)
		{
			if (_0B84 != null)
			{
				((Control)_0B84).Hide();
				((Component)(object)_0B84).Dispose();
				_0B84 = null;
			}
			if (((Control)this).Width > 5 && ((Control)this).Height > 5)
			{
				_0B84 = new ഓ(bAlphaShadow: true);
				((Control)_0B84).CreateControl();
				_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
				_0B84.ഖ();
			}
		}
	}

	protected override void OnResize(EventArgs e)
	{
		if (_0B84 != null)
		{
			_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
			_0B84.ഖ();
		}
		((Control)this).OnResize(e);
	}
}
