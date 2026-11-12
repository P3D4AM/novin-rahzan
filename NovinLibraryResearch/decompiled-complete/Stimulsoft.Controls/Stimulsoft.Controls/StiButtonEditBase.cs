using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiButtonEditBase : StiEditBase
{
	protected static Image dropDownButtonBitmap;

	private StiButtonIconKind buttonIconKind;

	private bool showButton = true;

	protected int DropDownButtonWidth => SystemInformation.VerticalScrollBarWidth - 1;

	internal bool IsPressed { get; set; }

	[Browsable(false)]
	[DefaultValue(null)]
	public Image ButtonBitmap { get; set; }

	[DefaultValue(StiButtonIconKind.DropDownArrow)]
	public virtual StiButtonIconKind ButtonIconKind
	{
		get
		{
			return buttonIconKind;
		}
		set
		{
			buttonIconKind = value;
			((Control)this).Invalidate();
		}
	}

	protected virtual bool IsMouseOverButton => GetButtonRect().Contains(((Control)this).PointToClient(Cursor.Position));

	[Category("Behavior")]
	[DefaultValue(true)]
	public virtual bool ShowButton
	{
		get
		{
			return showButton;
		}
		set
		{
			showButton = value;
			SetPosTextBox();
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	public event EventHandler ButtonClick;

	public virtual Rectangle GetButtonRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.X = result.Right - DropDownButtonWidth - 1;
		result.Width = DropDownButtonWidth;
		return result;
	}

	protected override Rectangle GetContentRect()
	{
		Rectangle result = new Rectangle(StiScaleUI.I2, StiScaleUI.I2, ((Control)this).Width - StiScaleUI.I4, ((Control)this).Height - StiScaleUI.I4);
		Rectangle buttonRect = GetButtonRect();
		buttonRect.Width += StiScaleUI.I2;
		result.Width -= buttonRect.Width;
		return result;
	}

	protected virtual void PaintButtons(Graphics g, Rectangle buttonRect, Image bmp, bool isPressed, bool isMouseOverButton)
	{
		Color color = (StiUX.IsDark ? StiUX.ButtonBackground : StiColors.Selected);
		Color color2 = (StiUX.IsDark ? StiUX.InputBorder : StiColors.SelectedText);
		if (isPressed)
		{
			color = (StiUX.IsDark ? StiUX.ButtonBackgroundPressed : StiColorUtils.Dark(color, 30));
			color2 = (StiUX.IsDark ? StiUX.ButtonBorderPressed : StiColorUtils.Dark(color2, 30));
		}
		if (isMouseOverButton)
		{
			StiDrawing.FillRectangle(g, color, buttonRect);
			Rectangle rect = buttonRect;
			rect.Height--;
			StiDrawing.DrawRectangle(g, color2, rect);
		}
		if (bmp == null)
		{
			DrawDownArrow(g);
			return;
		}
		Rectangle rectangle = new Rectangle(buttonRect.X + (buttonRect.Width - bmp.Width) / 2, buttonRect.Y + (buttonRect.Height - bmp.Height) / 2, bmp.Width, bmp.Height);
		if (((Control)this).Enabled)
		{
			g.DrawImage(bmp, rectangle);
		}
		else
		{
			StiControlPaint.DrawImageDisabled(g, bmp, rectangle);
		}
	}

	private Rectangle GetDownButtonRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.X = result.Right - DropDownButtonWidth - 1;
		result.Width = DropDownButtonWidth;
		return result;
	}

	private void DrawDownArrow(Graphics g)
	{
		Rectangle downButtonRect = GetDownButtonRect();
		if (ButtonIconKind == StiButtonIconKind.DropDownArrow)
		{
			StiButtonHelper.DrawDownArrow(g, downButtonRect, ((Control)this).Enabled);
		}
		else
		{
			StiButtonHelper.DrawDots(g, downButtonRect, ((Control)this).Enabled);
		}
	}

	protected virtual void OnButtonClick(EventArgs e)
	{
		this.ButtonClick?.Invoke(this, e);
	}

	public void InvokeButtonClick(EventArgs e)
	{
		OnButtonClick(e);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		base.OnPaint(p);
		Graphics graphics = p.Graphics;
		Rectangle buttonRect = GetButtonRect();
		if (ShowButton)
		{
			Image buttonBitmap = ButtonBitmap;
			PaintButtons(graphics, buttonRect, (buttonBitmap is Bitmap) ? buttonBitmap : null, IsPressed, IsMouseOverButton);
		}
	}

	protected override void OnClick(EventArgs e)
	{
		base.OnClick(e);
		if (IsMouseOverButton)
		{
			OnButtonClick(e);
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
			IsPressed = IsMouseOverButton;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		base.OnMouseUp(e);
		if ((e.Button & 0x100000) > 0)
		{
			IsPressed = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
		((Control)this).Invalidate();
	}

	public static Image GetImage(Image image)
	{
		if (image == null)
		{
			return null;
		}
		if (StiScaleUI.IsNoScaling)
		{
			return image;
		}
		Bitmap result = StiImageUtils.ResizeImage(image, StiScaleUI.XXI(image.Width), StiScaleUI.YYI(image.Height));
		image.Dispose();
		return (Image)(object)result;
	}

	static StiButtonEditBase()
	{
	}
}
