using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Controls.Win;

[ToolboxItem(false)]
public class StiNumericUpDownBase : StiControlBase
{
	[ToolboxItem(false)]
	public class InternalNumericUpDownBox : NumericUpDown
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

		public StiNumericUpDownBase ParentControl { get; }

		internal int SpinButtonsWidth { get; set; } = 20;


		protected override void WndProc(ref Message msg)
		{
			if (((Message)(ref msg)).Msg == 15)
			{
				((Control)this).DefWndProc(ref msg);
				Graphics val = Graphics.FromHwnd(((Control)this).Handle);
				try
				{
					Rectangle rectangle = new Rectangle(((Control)this).Width - SpinButtonsWidth, 0, SpinButtonsWidth, ((Control)this).Height);
					val.FillRectangle(Brushes.White, rectangle);
					return;
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			((UpDownBase)this).WndProc(ref msg);
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
			StiNumericUpDownBase stiNumericUpDownBase = ((Control)this).Parent as StiNumericUpDownBase;
			if ((keyData & 0xFFFF) == 9 && (Control.ModifierKeys & 0x10000) > 0)
			{
				Form val = ((Control)stiNumericUpDownBase).FindForm();
				if (val != null)
				{
					((Control)val).SelectNextControl((Control)(object)stiNumericUpDownBase, false, true, true, true);
					return true;
				}
			}
			return ((ContainerControl)this).ProcessDialogKey(keyData);
		}

		public InternalNumericUpDownBox(StiNumericUpDownBase parentControl)
		{
			ParentControl = parentControl;
			SpinButtonsWidth = ((Control)this).Controls[0].Width + StiScale2.I2;
			((Control)this).Controls.RemoveAt(0);
		}
	}

	private int buttonWidth = StiScale2.I6;

	private int buttonHeight = StiScale2.I3;

	private bool isPressed;

	private int DropDownButtonWidth => SystemInformation.VerticalScrollBarWidth - 1;

	protected InternalNumericUpDownBox NumericBox { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AllowDrawBorder { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Focused
	{
		get
		{
			if (!((Control)this).Focused)
			{
				return ((Control)NumericBox).Focused;
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
			return ((UpDownBase)NumericBox).ReadOnly;
		}
		set
		{
			((UpDownBase)NumericBox).ReadOnly = value;
			((Control)NumericBox).BackColor = (value ? SystemColors.Control : ((Control)this).BackColor);
			((Control)this).Invalidate();
		}
	}

	public int DecimalPlaces
	{
		get
		{
			return ((NumericUpDown)NumericBox).DecimalPlaces;
		}
		set
		{
			((NumericUpDown)NumericBox).DecimalPlaces = value;
		}
	}

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

	protected override void OnBackColorChanged(EventArgs e)
	{
		((Control)this).OnBackColorChanged(e);
		((Control)NumericBox).BackColor = ((Control)this).BackColor;
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Control)this).OnTextChanged(e);
		((Control)NumericBox).Text = ((Control)this).Text;
	}

	protected override void OnEnabledChanged(EventArgs e)
	{
		((Control)this).OnEnabledChanged(e);
		((Control)NumericBox).Visible = ((Control)this).Enabled;
		((Control)this).Invalidate();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)NumericBox).Focus();
		base.OnGotFocus(e);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		SetPosNumericBox();
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
		DrawButtons(graphics);
		if (AllowDrawBorder)
		{
			StiControlPaint.DrawBorder(graphics, rectangle, (IsMouseOver | ((Control)this).Focused) & base.HotFocus, ((Control)this).Enabled, Flat);
		}
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		((Component)(object)NumericBox).Dispose();
	}

	private void DrawButtons(Graphics g)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		Rectangle upButtonRect = GetUpButtonRect();
		Rectangle downButtonRect = GetDownButtonRect();
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		bool flag = upButtonRect.Contains(pt);
		bool flag2 = downButtonRect.Contains(pt);
		Color color = StiColors.Selected;
		Color color2 = StiColors.SelectedText;
		if (isPressed)
		{
			color = StiColorUtils.Dark(color, 30);
			color2 = StiColorUtils.Dark(color2, 30);
		}
		SolidBrush val = new SolidBrush(color);
		try
		{
			Pen val2 = new Pen(color2);
			try
			{
				if (flag)
				{
					g.FillRectangle((Brush)(object)val, upButtonRect);
					g.DrawRectangle(val2, upButtonRect);
				}
				if (flag2)
				{
					g.FillRectangle((Brush)(object)val, downButtonRect);
					g.DrawRectangle(val2, downButtonRect);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		DrawUpArrow(g);
		DrawDownArrow(g);
	}

	private Rectangle GetUpButtonRect()
	{
		Rectangle buttonsRect = GetButtonsRect();
		int height = buttonsRect.Height / 2;
		return new Rectangle(buttonsRect.X, buttonsRect.Y, buttonsRect.Width, height);
	}

	private Rectangle GetDownButtonRect()
	{
		Rectangle buttonsRect = GetButtonsRect();
		int num = buttonsRect.Height / 2;
		return new Rectangle(buttonsRect.X, buttonsRect.Bottom - num - 1, buttonsRect.Width, num);
	}

	private Rectangle GetButtonsRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.X = result.Right - DropDownButtonWidth - 1;
		result.Width = DropDownButtonWidth;
		return result;
	}

	private void DrawUpArrow(Graphics g)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		Rectangle upButtonRect = GetUpButtonRect();
		Rectangle rectangle = new Rectangle(upButtonRect.X + (upButtonRect.Width - buttonWidth) / 2, upButtonRect.Y + (upButtonRect.Height - buttonHeight) / 2, buttonWidth, buttonHeight);
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			Pen val = new Pen(Color.DimGray, 1.6f);
			try
			{
				Point[] array = new Point[3]
				{
					new Point(rectangle.X, rectangle.Bottom),
					new Point(rectangle.X + rectangle.Width / 2, rectangle.Y),
					new Point(rectangle.Right, rectangle.Bottom)
				};
				g.DrawLines(val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	private void DrawDownArrow(Graphics g)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		Rectangle downButtonRect = GetDownButtonRect();
		Rectangle rectangle = new Rectangle(downButtonRect.X + (downButtonRect.Width - buttonWidth) / 2, downButtonRect.Y + (downButtonRect.Height - buttonHeight) / 2, buttonWidth, buttonHeight);
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			Pen val = new Pen(Color.DimGray, 1.6f);
			try
			{
				Point[] array = new Point[3]
				{
					new Point(rectangle.X, rectangle.Y),
					new Point(rectangle.X + rectangle.Width / 2, rectangle.Bottom),
					new Point(rectangle.Right, rectangle.Y)
				};
				g.DrawLines(val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	protected virtual Rectangle GetContentRect()
	{
		return new Rectangle(2, 2, ((Control)this).Width - 4, ((Control)this).Height - 4);
	}

	protected virtual void SetPosNumericBox()
	{
		try
		{
			Rectangle contentRect = GetContentRect();
			((Control)NumericBox).Left = contentRect.Left + StiScale2.I1;
			((Control)NumericBox).Width = contentRect.Width - StiScale2.XXI(22.0);
			((Control)NumericBox).Top = contentRect.Y + (((Control)this).Height - ((Control)NumericBox).Height) / 2;
		}
		catch
		{
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		isPressed = false;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		isPressed = true;
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		if (GetUpButtonRect().Contains(pt))
		{
			((UpDownBase)NumericBox).UpButton();
		}
		if (GetDownButtonRect().Contains(pt))
		{
			((UpDownBase)NumericBox).DownButton();
		}
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
		((Control)this).Invalidate();
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

	private void NumericBox_TextChanged(object sender, EventArgs e)
	{
		((Control)this).Text = ((Control)NumericBox).Text;
	}

	private void NumericBox_GotFocus(object sender, EventArgs e)
	{
		base.OnGotFocus(e);
		((Control)NumericBox).Name = ((Control)this).Name;
		((Control)NumericBox).Tag = ((Control)this).Tag;
		((Control)this).Invalidate();
	}

	private void NumericBox_LostFocus(object sender, EventArgs e)
	{
		OnLostFocus(e);
		((Control)this).Invalidate();
	}

	private void NumericBox_SizeChanged(object sender, EventArgs e)
	{
		SetPosNumericBox();
		((Control)this).Invalidate();
	}

	private void NumericBox_MouseEnter(object sender, EventArgs e)
	{
		IsMouseOver = true;
		((Control)this).Invalidate();
	}

	private void NumericBox_MouseLeave(object sender, EventArgs e)
	{
		IsMouseOver = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
		((Control)this).Invalidate();
	}

	public StiNumericUpDownBase()
	{
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		NumericBox = new InternalNumericUpDownBox(this);
		((Control)this).Width = StiScale2.XXI(100.0);
		((Control)this).Height = StiScale2.YYI(21.0);
		((Control)this).BackColor = Color.White;
		((UpDownBase)NumericBox).BorderStyle = (BorderStyle)0;
		((UpDownBase)NumericBox).MouseEnter += NumericBox_MouseEnter;
		((UpDownBase)NumericBox).MouseLeave += NumericBox_MouseLeave;
		((Control)NumericBox).GotFocus += NumericBox_GotFocus;
		((Control)NumericBox).LostFocus += NumericBox_LostFocus;
		((Control)NumericBox).SizeChanged += NumericBox_SizeChanged;
		((NumericUpDown)NumericBox).TextChanged += NumericBox_TextChanged;
		((Control)NumericBox).MouseUp += new MouseEventHandler(InvokeMouseUp);
		((Control)NumericBox).MouseDown += new MouseEventHandler(InvokeMouseDown);
		((UpDownBase)NumericBox).MouseEnter += InvokeMouseEnter;
		((UpDownBase)NumericBox).MouseHover += InvokeMouseHover;
		((UpDownBase)NumericBox).MouseLeave += InvokeMouseLeave;
		((UpDownBase)NumericBox).MouseMove += new MouseEventHandler(InvokeMouseMove);
		((Control)NumericBox).KeyDown += new KeyEventHandler(InvokeKeyDown);
		((Control)NumericBox).KeyPress += new KeyPressEventHandler(InvokeKeyPress);
		((Control)NumericBox).KeyUp += new KeyEventHandler(InvokeKeyUp);
		((Control)NumericBox).Click += InvokeClick;
		((Control)NumericBox).DoubleClick += InvokeDoubleClick;
		((Control)this).Controls.Add((Control)(object)NumericBox);
		SetPosNumericBox();
	}
}
