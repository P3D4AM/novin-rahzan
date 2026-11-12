using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiGroupBox), "Toolbox.StiGroupBox.bmp")]
public class StiGroupBox : GroupBox
{
	private bool collapsed;

	private bool allowCollapse;

	private bool changeFormWhenCollapse = true;

	protected bool IsMouseOverCollapse { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual int HeaderSize => StiScaleUI.YYI(28.0);

	[Category("Behavior")]
	[Browsable(false)]
	public int ResHeight { get; set; }

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((GroupBox)this).Text;
		}
		set
		{
			((GroupBox)this).Text = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	public bool Collapsed
	{
		get
		{
			return collapsed;
		}
		set
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			if (collapsed == value)
			{
				return;
			}
			collapsed = value;
			if (value)
			{
				ResHeight = ((Control)this).Height;
				((Control)this).Height = HeaderSize;
				foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
				{
					Control val = item;
					val.Visible = false;
				}
				if (ChangeFormWhenCollapse)
				{
					Form val2 = ((Control)this).FindForm();
					if (val2 != null)
					{
						((Control)val2).Height = ((Control)val2).Height - (ResHeight - HeaderSize);
					}
				}
				InvokeCollapsedChange(EventArgs.Empty);
			}
			else
			{
				((Control)this).Height = ResHeight;
				foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
				{
					Control val3 = item2;
					val3.Visible = true;
				}
				if (ChangeFormWhenCollapse)
				{
					Form val4 = ((Control)this).FindForm();
					if (val4 != null)
					{
						((Control)val4).Height = ((Control)val4).Height + (ResHeight - HeaderSize);
					}
				}
				InvokeCollapsedChange(EventArgs.Empty);
			}
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(false)]
	public bool AllowCollapse
	{
		get
		{
			return allowCollapse;
		}
		set
		{
			allowCollapse = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ChangeFormWhenCollapse
	{
		get
		{
			return changeFormWhenCollapse;
		}
		set
		{
			changeFormWhenCollapse = value;
			((Control)this).Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new Padding(6);
		}
		set
		{
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Padding Margin
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return new Padding(6);
		}
		set
		{
		}
	}

	[Category("Behavior")]
	public event EventHandler CollapsedChange;

	protected virtual Rectangle GetCollapseRectangle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).RightToLeft != 1)
		{
			return StiScaleUI.I(new Rectangle(7, 1, 10, 10));
		}
		return new Rectangle(((Control)this).Width - StiScaleUI.XXI(17.0), StiScaleUI.I1, StiScaleUI.I10, StiScaleUI.I10);
	}

	private bool GetIsMouseOverCollapse()
	{
		if (((Component)this).DesignMode)
		{
			return false;
		}
		Point pt = ((Control)this).PointToClient(Cursor.Position);
		return GetCollapseRectangle().Contains(pt);
	}

	private void DrawCollapse()
	{
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			DrawCollapse(val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected void DrawCollapse(Graphics g)
	{
		Rectangle collapseRectangle = GetCollapseRectangle();
		Pen val;
		if (!IsMouseOverCollapse)
		{
			g.FillRectangle(StiBrushes.Content, collapseRectangle);
			g.DrawRectangle(SystemPens.ControlDark, collapseRectangle);
			val = SystemPens.ControlDark;
		}
		else
		{
			g.FillRectangle(StiBrushes.Selected, collapseRectangle);
			g.DrawRectangle(StiPens.SelectedText, collapseRectangle);
			val = StiPens.SelectedText;
		}
		int num = (int)Math.Ceiling(2.0 * StiScaleUI.X);
		int num2 = (int)Math.Ceiling(2.0 * StiScaleUI.Y);
		g.DrawLine(val, collapseRectangle.X + num, collapseRectangle.Y + collapseRectangle.Height / 2, collapseRectangle.Right - num, collapseRectangle.Y + collapseRectangle.Height / 2);
		if (Collapsed)
		{
			g.DrawLine(val, collapseRectangle.X + collapseRectangle.Width / 2, collapseRectangle.Y + num2, collapseRectangle.X + collapseRectangle.Width / 2, collapseRectangle.Bottom - num2);
		}
	}

	private void CheckMouseOver()
	{
		bool isMouseOverCollapse = IsMouseOverCollapse;
		IsMouseOverCollapse = GetIsMouseOverCollapse();
		if (isMouseOverCollapse != IsMouseOverCollapse && AllowCollapse)
		{
			DrawCollapse();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Invalid comparison between Unknown and I4
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		int num = (AllowCollapse ? StiScaleUI.XXI(24.0) : StiScaleUI.I8);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		Rectangle rectangle = clientRectangle;
		Font val = ((Control)this).Font;
		if (StiScaleUI.X > 1.0)
		{
			val = new Font(((Control)this).Font.FontFamily, ((Control)this).Font.Size * 1.04f, ((Control)this).Font.Style, (GraphicsUnit)3);
		}
		SizeF sizeF = graphics.MeasureString(((Control)this).Text, val);
		rectangle.Width = (int)sizeF.Width + StiScaleUI.I4;
		rectangle.Height = (int)sizeF.Height + StiScaleUI.I1;
		if ((int)((Control)this).RightToLeft == 1)
		{
			rectangle.X = clientRectangle.Right - rectangle.Width - num;
		}
		else
		{
			rectangle.X += num;
		}
		if (((Control)this).Text.Length == 0)
		{
			rectangle.Width = 0;
		}
		Color disabledColor = StiColorUtils.GetDisabledColor((Control)(object)this);
		int num2 = clientRectangle.Top + val.Height / 2;
		if (((Control)this).Text.Length == 0)
		{
			num2 = 0;
		}
		bool flag = false;
		try
		{
			if (VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS)
			{
				VisualStyleElement val2 = (((Control)this).Enabled ? GroupBox.Normal : GroupBox.Disabled);
				if (VisualStyleRenderer.IsElementDefined(val2))
				{
					VisualStyleRenderer val3 = new VisualStyleRenderer(val2);
					val3.DrawBackground((IDeviceContext)(object)graphics, new Rectangle(clientRectangle.X, clientRectangle.Y + num2, clientRectangle.Width, clientRectangle.Height - num2));
					flag = true;
				}
			}
		}
		catch
		{
			flag = false;
		}
		if (!flag)
		{
			Pen val4 = new Pen(ControlPaint.Light(disabledColor, 1f));
			try
			{
				Pen val5 = new Pen(ControlPaint.Dark(disabledColor, 0f));
				try
				{
					graphics.DrawLine(val4, clientRectangle.Left + 1, num2, clientRectangle.Left + 1, clientRectangle.Height - 1);
					graphics.DrawLine(val5, clientRectangle.Left, num2 - 1, clientRectangle.Left, clientRectangle.Height - 2);
					graphics.DrawLine(val4, clientRectangle.Left, clientRectangle.Height - 1, clientRectangle.Width, clientRectangle.Height - 1);
					graphics.DrawLine(val5, clientRectangle.Left, clientRectangle.Height - 2, clientRectangle.Width - 1, clientRectangle.Height - 2);
					graphics.DrawLine(val4, clientRectangle.Left + 1, num2, rectangle.X + 1, num2);
					graphics.DrawLine(val5, clientRectangle.Left, num2 - 1, rectangle.X, num2 - 1);
					graphics.DrawLine(val4, rectangle.X + rectangle.Width, num2, clientRectangle.Width - 1, num2);
					graphics.DrawLine(val5, rectangle.X + rectangle.Width + 1, num2 - 1, clientRectangle.Width - 2, num2 - 1);
					graphics.DrawLine(val4, clientRectangle.Width - 1, num2, clientRectangle.Width - 1, clientRectangle.Height - 1);
					graphics.DrawLine(val5, clientRectangle.Width - 2, num2 - 1, clientRectangle.Width - 2, clientRectangle.Height - 2);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		StringFormat val6 = new StringFormat();
		try
		{
			val6.FormatFlags = (StringFormatFlags)4096;
			val6.Trimming = (StringTrimming)3;
			val6.Alignment = (StringAlignment)1;
			val6.HotkeyPrefix = (HotkeyPrefix)1;
			if ((int)((Control)this).RightToLeft == 1)
			{
				val6.FormatFlags = (StringFormatFlags)(val6.FormatFlags | 1);
			}
			if (flag)
			{
				SolidBrush val7 = new SolidBrush(((Control)this).BackColor);
				try
				{
					graphics.FillRectangle((Brush)(object)val7, rectangle);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			if (((Control)this).Enabled)
			{
				SolidBrush val8 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					graphics.DrawString(((Control)this).Text, val, (Brush)(object)val8, (RectangleF)rectangle, val6);
				}
				finally
				{
					((IDisposable)val8)?.Dispose();
				}
			}
			else if (StiUX.IsDark)
			{
				SolidBrush val9 = new SolidBrush(StiUX.InputForegroundDisabled);
				try
				{
					graphics.DrawString(((Control)this).Text, val, (Brush)(object)val9, (RectangleF)rectangle, val6);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			else
			{
				ControlPaint.DrawStringDisabled(graphics, ((Control)this).Text, val, disabledColor, (RectangleF)rectangle, val6);
			}
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		if (AllowCollapse)
		{
			DrawCollapse(graphics);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		CheckMouseOver();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		CheckMouseOver();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		CheckMouseOver();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		Point point = ((Control)this).PointToClient(Cursor.Position);
		if (point.Y >= 0 && point.Y <= HeaderSize && AllowCollapse)
		{
			Collapsed = !Collapsed;
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		e.Control.Visible = !Collapsed;
	}

	protected virtual void OnCollapsedChange(EventArgs e)
	{
		this.CollapsedChange?.Invoke(this, e);
	}

	public void InvokeCollapsedChange(EventArgs e)
	{
		OnCollapsedChange(e);
	}

	public StiGroupBox()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)2048, true);
	}
}
