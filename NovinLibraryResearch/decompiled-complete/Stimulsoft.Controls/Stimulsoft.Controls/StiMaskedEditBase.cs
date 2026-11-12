using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiMaskedEditBase : StiControlBase
{
	[ToolboxItem(false)]
	public class InternalMaskedTextBox : MaskedTextBox
	{
		public override ISite Site
		{
			get
			{
				if (((Component)this).DesignMode || ParentControl == null)
				{
					return ((Control)this).Site;
				}
				((Control)this).Name = ((Control)ParentControl).Name;
				((Control)this).Tag = ((Control)ParentControl).Tag;
				return ((Control)this).Site;
			}
		}

		public StiMaskedEditBase ParentControl { get; }

		protected override bool ProcessDialogKey(Keys keyData)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Invalid comparison between Unknown and I4
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			StiMaskedEditBase stiMaskedEditBase = ((Control)this).Parent as StiMaskedEditBase;
			if ((keyData & 0xFFFF) == 9 && (Control.ModifierKeys & 0x10000) > 0)
			{
				Form val = ((Control)stiMaskedEditBase).FindForm();
				if (val != null)
				{
					((Control)val).SelectNextControl((Control)(object)stiMaskedEditBase, false, true, true, true);
					return true;
				}
			}
			return ((TextBoxBase)this).ProcessDialogKey(keyData);
		}

		public InternalMaskedTextBox(StiMaskedEditBase parentControl)
		{
			ParentControl = parentControl;
		}
	}

	protected MaskedTextBox TextBox { get; set; }

	[Browsable(false)]
	public override Image BackgroundImage
	{
		get
		{
			return base.BackgroundImage;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AllowDrawBorder { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int SelectionStart
	{
		get
		{
			return ((TextBoxBase)TextBox).SelectionStart;
		}
		set
		{
			((TextBoxBase)TextBox).SelectionStart = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int SelectionLength
	{
		get
		{
			return ((TextBoxBase)TextBox).SelectionLength;
		}
		set
		{
			((TextBoxBase)TextBox).SelectionLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string SelectionText
	{
		get
		{
			return ((TextBoxBase)TextBox).SelectedText;
		}
		set
		{
			((TextBoxBase)TextBox).SelectedText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool HideSelection
	{
		get
		{
			return ((TextBoxBase)TextBox).HideSelection;
		}
		set
		{
			((TextBoxBase)TextBox).HideSelection = value;
		}
	}

	[DefaultValue(32767)]
	[Category("Behavior")]
	public virtual int MaxLength
	{
		get
		{
			return ((TextBoxBase)TextBox).MaxLength;
		}
		set
		{
			((TextBoxBase)TextBox).MaxLength = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Focused
	{
		get
		{
			if (!((Control)this).Focused)
			{
				return ((Control)TextBox).Focused;
			}
			return true;
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	public virtual bool ReadOnly
	{
		get
		{
			return TextBox.ReadOnly;
		}
		set
		{
			TextBox.ReadOnly = value;
			((Control)TextBox).BackColor = ((Control)this).BackColor;
			((Control)this).Invalidate();
		}
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		((Control)TextBox).BackColor = ((Control)this).BackColor;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Control)this).OnTextChanged(e);
		((Control)TextBox).Text = ((Control)this).Text;
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		((Control)this).OnEnabledChanged(e);
		((Control)TextBox).Visible = ((Control)this).Enabled;
		((Control)this).Invalidate();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)TextBox).Focus();
		((TextBoxBase)TextBox).Select(0, 0);
		base.OnGotFocus(e);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		SetPosTextBox();
		((Control)this).OnSizeChanged(e);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (ReadOnly)
		{
			IsMouseOver = false;
		}
		if (!((Control)this).Enabled)
		{
			graphics.FillRectangle(Brushes.WhiteSmoke, rectangle);
		}
		else
		{
			graphics.FillRectangle(Brushes.White, rectangle);
		}
		if (AllowDrawBorder)
		{
			StiControlPaint.DrawBorder(graphics, rectangle, (IsMouseOver | ((Control)this).Focused) & base.HotFocus, ((Control)this).Enabled, flat: true);
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		base.OnMouseEnter(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		((Control)this).OnKeyDown(e);
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		((Control)this).OnKeyUp(e);
	}

	protected override void OnKeyPress(KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
	}

	private void InvokeMouseUp(object sender, MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
	}

	private void InvokeMouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
	}

	private void InvokeMouseEnter(object sender, EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
	}

	private void InvokeMouseLeave(object sender, EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
	}

	private void InvokeMouseHover(object sender, EventArgs e)
	{
		((Control)this).OnMouseHover(e);
	}

	private void InvokeMouseMove(object sender, MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	private void InvokeKeyDown(object sender, KeyEventArgs e)
	{
		((Control)this).OnKeyDown(e);
	}

	private void InvokeKeyUp(object sender, KeyEventArgs e)
	{
		((Control)this).OnKeyUp(e);
	}

	private void InvokeKeyPress(object sender, KeyPressEventArgs e)
	{
		((Control)this).OnKeyPress(e);
	}

	private void InvokeClick(object sender, EventArgs e)
	{
		((Control)this).OnClick(e);
	}

	private void InvokeDoubleClick(object sender, EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
	}

	private void TextBox_TextChanged(object sender, EventArgs e)
	{
		((Control)this).Text = ((Control)TextBox).Text;
	}

	private void TextBox_GotFocus(object sender, EventArgs e)
	{
		base.OnGotFocus(e);
		((Control)TextBox).Name = ((Control)this).Name;
		((Control)TextBox).Tag = ((Control)this).Tag;
		((Control)this).Invalidate();
	}

	private void TextBox_LostFocus(object sender, EventArgs e)
	{
		OnLostFocus(e);
		((Control)this).Invalidate();
	}

	private void TextBox_SizeChanged(object sender, EventArgs e)
	{
		SetPosTextBox();
		((Control)this).Invalidate();
	}

	private void TextBox_MouseEnter(object sender, EventArgs e)
	{
		IsMouseOver = true;
		((Control)this).Invalidate();
	}

	private void TextBox_MouseLeave(object sender, EventArgs e)
	{
		IsMouseOver = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
		((Control)this).Invalidate();
	}

	public void Select(int start, int length)
	{
		((TextBoxBase)TextBox).Select(start, length);
	}

	protected virtual Rectangle GetContentRect()
	{
		return new Rectangle(2, 2, ((Control)this).Width - 4, ((Control)this).Height - 4);
	}

	protected virtual void SetPosTextBox()
	{
		try
		{
			Rectangle contentRect = GetContentRect();
			((Control)TextBox).Left = contentRect.Left + StiScaleUI.I1;
			((Control)TextBox).Width = contentRect.Width - StiScaleUI.I2;
			((Control)TextBox).Top = contentRect.Top + (contentRect.Height - ((Control)TextBox).Height) / 2;
		}
		catch
		{
		}
	}

	public StiMaskedEditBase()
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		TextBox = (MaskedTextBox)(object)new InternalMaskedTextBox(this);
		((Control)this).Width = StiScaleUI.I(100);
		((Control)this).Height = StiScaleUI.I(21);
		((Control)this).BackColor = Color.White;
		((TextBoxBase)TextBox).BorderStyle = (BorderStyle)0;
		((Control)TextBox).MouseEnter += TextBox_MouseEnter;
		((Control)TextBox).MouseLeave += TextBox_MouseLeave;
		((Control)TextBox).GotFocus += TextBox_GotFocus;
		((Control)TextBox).LostFocus += TextBox_LostFocus;
		((Control)TextBox).SizeChanged += TextBox_SizeChanged;
		((Control)TextBox).TextChanged += TextBox_TextChanged;
		((Control)TextBox).MouseUp += new MouseEventHandler(InvokeMouseUp);
		((Control)TextBox).MouseDown += new MouseEventHandler(InvokeMouseDown);
		((Control)TextBox).MouseEnter += InvokeMouseEnter;
		((Control)TextBox).MouseHover += InvokeMouseHover;
		((Control)TextBox).MouseLeave += InvokeMouseLeave;
		((Control)TextBox).MouseMove += new MouseEventHandler(InvokeMouseMove);
		((Control)TextBox).KeyDown += new KeyEventHandler(InvokeKeyDown);
		((Control)TextBox).KeyPress += new KeyPressEventHandler(InvokeKeyPress);
		((Control)TextBox).KeyUp += new KeyEventHandler(InvokeKeyUp);
		((TextBoxBase)TextBox).Click += InvokeClick;
		((Control)TextBox).DoubleClick += InvokeDoubleClick;
		((Control)this).Controls.Add((Control)(object)TextBox);
		SetPosTextBox();
	}
}
