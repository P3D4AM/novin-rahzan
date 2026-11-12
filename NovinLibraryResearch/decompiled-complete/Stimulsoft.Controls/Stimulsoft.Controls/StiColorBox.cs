using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiTabControl), "Toolbox.StiColorBox.bmp")]
public class StiColorBox : StiButtonEditBase, IStiPopupParentControl
{
	private bool showColorBox = true;

	private bool showColorName = true;

	private Color selectedColor = Color.Transparent;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	internal StiColorBoxPopupForm ColorBoxPopupForm { get; set; }

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowColorBox
	{
		get
		{
			return showColorBox;
		}
		set
		{
			showColorBox = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	public bool ShowColorName
	{
		get
		{
			return showColorName;
		}
		set
		{
			showColorName = value;
			((Control)this).Invalidate();
		}
	}

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

	protected override void OnEnabledChanged(EventArgs e)
	{
		base.OnEnabledChanged(e);
		((Control)base.TextBox).Visible = false;
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		base.OnPaint(p);
		Graphics graphics = p.Graphics;
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rectangle2 = Rectangle.Empty;
		Rectangle buttonRect = StiControlPaint.GetButtonRect(((Control)this).ClientRectangle, flat: true, (RightToLeft)0);
		Rectangle rectangle3 = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		if (showColorBox)
		{
			rectangle = ((!showColorName) ? new Rectangle(rectangle3.X + StiScaleUI.I2, rectangle3.Y + StiScaleUI.I2, ((Control)this).Width - buttonRect.Width - StiScaleUI.I10, ((Control)this).Height - StiScaleUI.I5) : new Rectangle(rectangle3.X + StiScaleUI.I2, rectangle3.Y + StiScaleUI.I2, StiScaleUI.XXI(22.0), ((Control)this).Height - StiScaleUI.I5));
			if (StiScaleUI.Id == SystemScaleID.x1)
			{
				rectangle.Height--;
			}
		}
		if (showColorName)
		{
			rectangle2 = ((!showColorBox) ? new Rectangle(StiScaleUI.I2, StiScaleUI.I1, ((Control)this).Width - buttonRect.Width - StiScaleUI.I2, ((Control)this).Height - StiScaleUI.I2) : new Rectangle(rectangle.Right + StiScaleUI.I2, StiScaleUI.I1, ((Control)this).Width - rectangle.Width - buttonRect.Width - StiScaleUI.I4, ((Control)this).Height - StiScaleUI.I2));
		}
		if (showColorBox)
		{
			SolidBrush val = new SolidBrush(SelectedColor);
			try
			{
				graphics.FillRectangle((Brush)(object)val, rectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (((Control)this).Enabled)
			{
				graphics.DrawRectangle(Pens.Gray, rectangle);
			}
			else
			{
				graphics.DrawRectangle(SystemPens.ControlDark, rectangle);
			}
		}
		if (!showColorName)
		{
			return;
		}
		StringFormat val2 = new StringFormat();
		try
		{
			val2.LineAlignment = (StringAlignment)1;
			val2.FormatFlags = (StringFormatFlags)4096;
			string text = null;
			text = (SelectedColor.IsSystemColor ? StiLocalization.Get("PropertySystemColors", SelectedColor.Name) : ((!SelectedColor.IsKnownColor) ? (StiLocalization.Get("FormColorBoxPopup", "Color") + "[A=" + SelectedColor.A + ", R=" + SelectedColor.R + ", G=" + SelectedColor.G + ", B=" + SelectedColor.B + "]") : StiLocalization.Get("PropertyColor", SelectedColor.Name)));
			if (((Control)this).Enabled)
			{
				SolidBrush val3 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					graphics.DrawString(text, ((Control)this).Font, (Brush)(object)val3, (RectangleF)rectangle2, val2);
					return;
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			graphics.DrawString(text, ((Control)this).Font, SystemBrushes.ControlDark, (RectangleF)rectangle2, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		base.OnMouseDown(e);
		if ((e.Button & 0x100000) > 0 && !ReadOnly)
		{
			if (!LockPopupInvoke && ColorBoxPopupForm == null)
			{
				InvokeDropDown(EventArgs.Empty);
				ColorBoxPopupForm = new StiColorBoxPopupForm((Control)(object)this, SelectedColor);
				ColorBoxPopupForm.ClosePopup += OnClosePopup;
				Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
				ColorBoxPopupForm.SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + StiScaleUI.I1, StiScaleUI.XXI(210.0), StiScaleUI.YYI(276.0));
				((Control)ColorBoxPopupForm).Font = ((Control)this).Font;
				ColorBoxPopupForm.ShowPopupForm();
				ColorBoxPopupForm.TabControl.SelectedIndex = ColorBoxPopupForm.TabControl.SelectedIndex;
				InvokeDropDown(EventArgs.Empty);
			}
			else
			{
				LockPopupInvoke = false;
			}
		}
	}

	private void OnClosePopup(object sender, EventArgs e)
	{
		if (ColorBoxPopupForm != null)
		{
			InvokeDropUp(e);
			Color color = ColorBoxPopupForm.SelectedColor;
			if (SelectedColor != color)
			{
				SelectedColor = color;
				InvokeSelectedIndexChanged(EventArgs.Empty);
			}
			((Component)(object)ColorBoxPopupForm).Dispose();
			ColorBoxPopupForm = null;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && base.ButtonBitmap != null)
		{
			base.ButtonBitmap.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	public StiColorBox()
	{
		((Control)base.TextBox).Visible = false;
	}
}
