using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
internal class StiPenStyleButton : StiToolButton, IStiPopupParentControl
{
	internal StiPenStyleButtonForm PenStyleButtonForm;

	private StiPenStyle penStyle;

	private bool lockPopupInvoke;

	[Category("Behavior")]
	public StiPenStyle PenStyle
	{
		get
		{
			return penStyle;
		}
		set
		{
			penStyle = value;
			InvokeSelectedIndexChanged(this, new EventArgs());
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke
	{
		get
		{
			return lockPopupInvoke;
		}
		set
		{
			lockPopupInvoke = value;
		}
	}

	[Browsable(false)]
	public new StiToolBarButtonStyle Style
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

	[Browsable(false)]
	public new bool Pushed
	{
		get
		{
			return base.Pushed;
		}
		set
		{
			base.Pushed = value;
		}
	}

	public event EventHandler DropDown;

	public event EventHandler DropUp;

	public event EventHandler SelectedIndexChanged;

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		base.OnPaint(p);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(clientRectangle.X + 3, clientRectangle.Top + 3, clientRectangle.Width - 17, clientRectangle.Height - 7);
		if (((Control)this).Enabled)
		{
			graphics.FillRectangle(SystemBrushes.Window, rectangle);
			graphics.DrawRectangle(Pens.Black, rectangle);
		}
		else
		{
			graphics.FillRectangle(SystemBrushes.Control, rectangle);
			graphics.DrawRectangle(SystemPens.ControlDark, rectangle);
		}
		Pen val = new Pen(Color.Black, 2f);
		try
		{
			val.DashStyle = StiPenUtils.GetPenStyle(PenStyle);
			if (((Control)this).Enabled)
			{
				int num = rectangle.Y + rectangle.Height / 2 + 1;
				if (penStyle == StiPenStyle.Double)
				{
					val.Width = 1f;
					graphics.DrawLine(val, rectangle.X + 1, num - 1, rectangle.Right, num - 1);
					graphics.DrawLine(val, rectangle.X + 1, num + 1, rectangle.Right, num + 1);
				}
				else if (penStyle != StiPenStyle.None)
				{
					graphics.DrawLine(val, rectangle.X + 1, num, rectangle.Right, num);
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnClosePopup(object sender, EventArgs e)
	{
		if (PenStyleButtonForm != null)
		{
			((Component)(object)PenStyleButtonForm).Dispose();
			PenStyleButtonForm = null;
			Pushed = false;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		if (!LockPopupInvoke && PenStyleButtonForm == null)
		{
			InvokeDropDown(this, EventArgs.Empty);
			PenStyleButtonForm = new StiPenStyleButtonForm((Control)(object)this);
			Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
			PenStyleButtonForm.SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + 1, 240, 138);
			PenStyleButtonForm.ClosePopup += OnClosePopup;
			PenStyleButtonForm.ShowPopupForm();
		}
		else
		{
			LockPopupInvoke = false;
			Pushed = true;
		}
	}

	public virtual void InvokeDropDown(object sender, EventArgs e)
	{
		if (this.DropDown != null)
		{
			this.DropDown(sender, e);
		}
	}

	public virtual void InvokeDropUp(object sender, EventArgs e)
	{
		if (this.DropUp != null)
		{
			this.DropUp(sender, e);
		}
	}

	public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
	{
		if (this.SelectedIndexChanged != null)
		{
			this.SelectedIndexChanged(sender, e);
		}
	}

	public StiPenStyleButton()
	{
		base.IsDrawDropDownArrow = true;
		base.Style = StiToolBarButtonStyle.PushButton;
	}
}
