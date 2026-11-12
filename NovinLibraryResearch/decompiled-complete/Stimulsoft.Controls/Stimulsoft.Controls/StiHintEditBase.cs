using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiHintEditBase : StiControlBase
{
	[ToolboxItem(false)]
	public class HintTextBox : TextBox
	{
		private string hint;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color ForeColor
		{
			get
			{
				if (!StiUX.IsDark)
				{
					return ((TextBoxBase)this).ForeColor;
				}
				return StiUX.InputForeground;
			}
			set
			{
				if (!StiUX.IsDark)
				{
					((TextBoxBase)this).ForeColor = value;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get
			{
				if (!StiUX.IsDark)
				{
					return ((TextBoxBase)this).BackColor;
				}
				return StiUX.InputBackground;
			}
			set
			{
				if (!StiUX.IsDark)
				{
					((TextBoxBase)this).BackColor = value;
				}
			}
		}

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

		public StiHintEditBase ParentControl { get; }

		public string Hint
		{
			get
			{
				return hint;
			}
			set
			{
				hint = value;
				((Control)this).Invalidate();
			}
		}

		protected override void WndProc(ref Message m)
		{
			((TextBox)this).WndProc(ref m);
			if (((Message)(ref m)).Msg == 15 && string.IsNullOrEmpty(((Control)this).Text) && !string.IsNullOrEmpty(Hint))
			{
				Graphics val = ((Control)this).CreateGraphics();
				try
				{
					Rectangle rectangle = new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width, ((Control)this).ClientRectangle.Height);
					TextRenderer.DrawText((IDeviceContext)(object)val, Hint, ((Control)this).Font, rectangle, Color.LightGray, ((Control)this).BackColor, (TextFormatFlags)4);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}

		private void This_TextChanged(object sender, EventArgs e)
		{
			((Control)this).Invalidate();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Invalid comparison between Unknown and I4
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			StiEditBase stiEditBase = ((Control)this).Parent as StiEditBase;
			if ((keyData & 0xFFFF) == 9 && (Control.ModifierKeys & 0x10000) > 0)
			{
				Form val = ((Control)stiEditBase).FindForm();
				if (val != null)
				{
					((Control)val).SelectNextControl((Control)(object)stiEditBase, false, true, true, true);
					return true;
				}
			}
			return ((TextBoxBase)this).ProcessDialogKey(keyData);
		}

		public HintTextBox(StiHintEditBase parentControl)
		{
			ParentControl = parentControl;
			((Control)this).TextChanged += This_TextChanged;
		}
	}

	protected HintTextBox TextBox { get; set; }

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
	public virtual Color BorderColor { get; set; } = Color.Transparent;


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
			return ((TextBoxBase)TextBox).ReadOnly;
		}
		set
		{
			((TextBoxBase)TextBox).ReadOnly = value;
			((Control)TextBox).BackColor = (value ? SystemColors.Control : ((Control)this).BackColor);
			((Control)this).Invalidate();
		}
	}

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		((Control)TextBox).BackColor = ((Control)this).BackColor;
	}

	protected override void OnForeColorChanged(EventArgs e)
	{
		((Control)this).OnForeColorChanged(e);
		((Control)TextBox).ForeColor = ((Control)this).ForeColor;
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

	protected override void OnFontChanged(EventArgs e)
	{
		((Control)this).OnFontChanged(e);
		((Control)TextBox).Font = ((Control)this).Font;
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
			StiDrawing.FillRectangle(graphics, ((Control)this).BackColor, rectangle);
		}
		if (AllowDrawBorder)
		{
			if (BorderColor != Color.Transparent)
			{
				StiDrawing.DrawRectangle(graphics, BorderColor, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
			}
			else
			{
				StiControlPaint.DrawBorder(graphics, rectangle, (IsMouseOver | ((Control)this).Focused) & base.HotFocus, ((Control)this).Enabled, flat: true);
			}
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

	public StiHintEditBase()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		TextBox = new HintTextBox(this);
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
