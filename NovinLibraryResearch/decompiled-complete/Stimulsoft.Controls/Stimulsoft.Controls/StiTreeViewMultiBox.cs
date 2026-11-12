using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiTreeViewMultiBox), "Toolbox.StiTreeViewBox.bmp")]
public class StiTreeViewMultiBox : StiTreeViewBox
{
	internal bool IsPressed2 { get; set; }

	protected bool IsMouseOverButton2
	{
		get
		{
			if (!ShowButton2)
			{
				return false;
			}
			return GetButtonRect2().Contains(((Control)this).PointToClient(Cursor.Position));
		}
	}

	public Image ButtonBitmap2 { get; set; }

	public bool ShowButton2 { get; set; } = true;


	[Category("Behavior")]
	public event EventHandler ButtonClick2;

	public Rectangle GetButtonRect2()
	{
		Rectangle buttonRect = base.GetButtonRect();
		buttonRect.X -= buttonRect.Width + 1;
		return buttonRect;
	}

	protected override Rectangle GetContentRect()
	{
		Rectangle contentRect = base.GetContentRect();
		Rectangle buttonRect = GetButtonRect2();
		contentRect.Width -= buttonRect.Width + 1;
		return contentRect;
	}

	protected virtual void OnButtonClick2(EventArgs e)
	{
		if (this.ButtonClick2 != null)
		{
			this.ButtonClick2(this, e);
		}
	}

	public void InvokeButtonClick2(EventArgs e)
	{
		OnButtonClick2(e);
	}

	protected override void OnClick(EventArgs e)
	{
		base.OnClick(e);
		if (IsMouseOverButton2)
		{
			OnButtonClick2(e);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((e.Button & 0x100000) > 0)
		{
			if (IsMouseOverButton2)
			{
				IsPressed2 = IsMouseOverButton2;
				((Control)this).Focus();
				((Control)this).Invalidate();
			}
			else
			{
				base.OnMouseDown(e);
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((e.Button & 0x100000) > 0)
		{
			base.IsPressed = false;
			IsPressed2 = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		base.OnPaint(p);
		Graphics graphics = p.Graphics;
		Rectangle buttonRect = GetButtonRect2();
		if (ShowButton && ShowButton2)
		{
			Image buttonBitmap = ButtonBitmap2;
			PaintButtons(graphics, buttonRect, (buttonBitmap is Bitmap) ? buttonBitmap : null, IsPressed2, IsMouseOverButton2);
		}
	}
}
