using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
internal class ᆡ : Form
{
	private const int ग = 33;

	private const int घ = 3;

	private Bar Ⴭ;

	private Size ᆢ = Size.Empty;

	private bool ᆣ;

	public ᆡ(Bar objBar)
	{
		Ⴭ = objBar;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).ControlBox = false;
		((Form)this).ShowInTaskbar = false;
		if (objBar.Owner is IOwner owner)
		{
			((Form)this).Owner = owner.ParentForm;
		}
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).Font = ((Control)objBar).Font;
	}

	public void ᆤ()
	{
		ᆣ = true;
		((Form)this).Close();
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (((Control)this).Visible && !ᆣ)
		{
			e.Cancel = true;
			if (Ⴭ != null)
			{
				Ⴭ.ఈ();
			}
		}
		((Form)this).OnClosing(e);
	}

	public void ᆥ(Size ᆦ)
	{
		if (Ⴭ == null || ᆢ == ᆦ || !Ⴭ.IsThemed)
		{
			return;
		}
		ᆢ = ᆦ;
		_0A79 obj = Ⴭ._0A79;
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			IntPtr intPtr = obj._0EED(val, _༣._༦, ཉ.ཋ, new Rectangle(0, 0, ᆦ.Width, ᆦ.Height));
			if (intPtr != IntPtr.Zero)
			{
				_089F.SetWindowRgn(((Control)this).Handle, intPtr, bRedraw: true);
			}
		}
		finally
		{
			val.Dispose();
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33 && Ⴭ.LayoutType != eLayoutType.DockContainer)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Form)this).WndProc(ref m);
		}
	}

	protected override void OnActivated(EventArgs e)
	{
		((Form)this).OnActivated(e);
		if (Ⴭ != null && Ⴭ.LayoutType == eLayoutType.DockContainer)
		{
			Ⴭ._0BCE(_0BCF: true);
		}
	}

	protected override void OnDeactivate(EventArgs e)
	{
		((Form)this).OnDeactivate(e);
		if (Ⴭ != null && Ⴭ.LayoutType == eLayoutType.DockContainer)
		{
			Ⴭ._0BCE(_0BCF: false);
		}
	}
}
