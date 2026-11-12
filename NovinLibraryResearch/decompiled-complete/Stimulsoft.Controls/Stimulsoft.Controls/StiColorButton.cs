using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiColorButton), "Toolbox.StiColorButton.bmp")]
public class StiColorButton : StiToolButton, IStiPopupParentControl
{
	private Color selectedColor = Color.Transparent;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((ButtonBase)this).Text;
		}
		set
		{
			((ButtonBase)this).Text = value;
		}
	}

	internal StiColorBoxPopupForm ColorBoxPopupForm { get; set; }

	[Category("Behavior")]
	public Color SelectedColor
	{
		get
		{
			return selectedColor;
		}
		set
		{
			selectedColor = value;
			OnSelectedIndexChanged(EventArgs.Empty);
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	public event EventHandler DropDown;

	[Category("Behavior")]
	public event EventHandler DropUp;

	[Category("Behavior")]
	public event EventHandler SelectedIndexChanged;

	protected virtual void OnDropDown(EventArgs e)
	{
		this.DropDown?.Invoke(this, e);
	}

	public void InvokeDropDown(EventArgs e)
	{
		OnDropDown(e);
	}

	protected virtual void OnDropUp(EventArgs e)
	{
		this.DropUp?.Invoke(this, e);
	}

	public void InvokeDropUp(EventArgs e)
	{
		OnDropUp(e);
	}

	protected virtual void OnSelectedIndexChanged(EventArgs e)
	{
		this.SelectedIndexChanged?.Invoke(this, e);
	}

	public void InvokeSelectedIndexChanged(EventArgs e)
	{
		OnSelectedIndexChanged(e);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		base.OnPaint(p);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(clientRectangle.X + StiScaleUI.I2, clientRectangle.Bottom - StiScaleUI.I5, clientRectangle.Width - StiScaleUI.I14, StiScaleUI.I4);
		if (!((Control)this).Enabled)
		{
			return;
		}
		Brush val = (Brush)new SolidBrush(SelectedColor);
		try
		{
			graphics.FillRectangle(val, rectangle);
			if (SelectedColor == Color.Transparent)
			{
				graphics.DrawRectangle(SystemPens.ControlDark, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		base.OnMouseDown(e);
		if ((e.Button & 0x100000) > 0)
		{
			if (!LockPopupInvoke && ColorBoxPopupForm == null)
			{
				ColorBoxPopupForm = new StiColorBoxPopupForm((Control)(object)this, SelectedColor);
				ColorBoxPopupForm.ClosePopup += OnClosePopup;
				Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
				ColorBoxPopupForm.SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + StiScaleUI.I1, StiScaleUI.XXI(210.0), StiScaleUI.YYI(270.0));
				((Control)ColorBoxPopupForm).Font = ((Control)this).Font;
				ColorBoxPopupForm.ShowPopupForm();
				ColorBoxPopupForm.TabControl.SelectedIndex = ColorBoxPopupForm.TabControl.SelectedIndex;
				InvokeDropDown(EventArgs.Empty);
			}
			else
			{
				LockPopupInvoke = false;
				base.Pushed = true;
			}
		}
	}

	private void OnClosePopup(object sender, EventArgs e)
	{
		if (ColorBoxPopupForm != null)
		{
			base.Pushed = false;
			InvokeDropUp(e);
			Color color = ColorBoxPopupForm.SelectedColor;
			if (SelectedColor != color)
			{
				SelectedColor = color;
			}
			((Component)(object)ColorBoxPopupForm).Dispose();
			ColorBoxPopupForm = null;
		}
	}

	public StiColorButton()
	{
		base.IsDrawDropDownArrow = true;
		base.Style = StiToolBarButtonStyle.PushButton;
	}
}
