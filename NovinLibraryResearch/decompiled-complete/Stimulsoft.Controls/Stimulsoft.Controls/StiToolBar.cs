using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiToolBarDesigner))]
[ToolboxBitmap(typeof(StiToolBar), "Toolbox.StiToolBar.bmp")]
public class StiToolBar : Panel
{
	public enum StiToolBarLineStyle
	{
		All,
		Top,
		Bottom,
		TopBottom
	}

	protected const int DotWidth = 12;

	protected const int DockPaddingTitle = 12;

	protected const int DockPaddingNormal = 3;

	protected bool IsDragging;

	protected bool ChangePos = true;

	private bool allowMove = true;

	private StiToolBarLineStyle lineStyle = StiToolBarLineStyle.Bottom;

	private Point lastPos;

	private Point movePos;

	private StiControlStyle controlStyle;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool AllowMove
	{
		get
		{
			return allowMove;
		}
		set
		{
			allowMove = value;
		}
	}

	public StiToolBarLineStyle LineStyle
	{
		get
		{
			return lineStyle;
		}
		set
		{
			lineStyle = value;
			((Control)this).Invalidate();
		}
	}

	protected virtual bool IsOverTitle
	{
		get
		{
			if (!AllowMove)
			{
				return false;
			}
			Point point = ((Control)this).PointToClient(Cursor.Position);
			if (point.X >= 0 && point.X <= 6 && point.Y >= 0 && point.Y <= ((Control)this).Height)
			{
				return true;
			}
			return false;
		}
	}

	protected Point LastPos
	{
		get
		{
			return lastPos;
		}
		set
		{
			lastPos = value;
		}
	}

	protected Point MovePos
	{
		get
		{
			return movePos;
		}
		set
		{
			movePos = value;
		}
	}

	[DefaultValue(StiControlStyle.Flat)]
	public StiControlStyle ControlStyle
	{
		get
		{
			return controlStyle;
		}
		set
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			controlStyle = value;
			if (controlStyle == StiControlStyle.Office2013Blue)
			{
				((Control)this).Padding = new Padding(6, 3, 3, 4);
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
				return ((Control)this).BackColor;
			}
			return StiUX.Background;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	public void CheckSeparators()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		try
		{
			List<Control> list = new List<Control>();
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is StiToolButton && ((StiToolButton)(object)val).Style == StiToolBarButtonStyle.Separator)
				{
					val.Visible = true;
				}
				if (val.Visible)
				{
					list.Add(val);
				}
			}
			foreach (Control item2 in list)
			{
				if (!(item2 is StiToolButton) || ((StiToolButton)(object)item2).Style != StiToolBarButtonStyle.Separator || !item2.Visible)
				{
					continue;
				}
				int num = list.IndexOf(item2) + 1;
				for (int i = num; i < list.Count; i++)
				{
					StiToolButton stiToolButton = list[i] as StiToolButton;
					Control val2 = list[i];
					if ((stiToolButton != null && stiToolButton.Style != StiToolBarButtonStyle.Separator) || stiToolButton == null)
					{
						break;
					}
					val2.Visible = false;
				}
			}
			for (int j = 0; j < ((ArrangedElementCollection)((Control)this).Controls).Count; j++)
			{
				Control val3 = ((Control)this).Controls[j];
				if (val3.Visible)
				{
					if (val3 is StiToolButton && ((StiToolButton)(object)val3).Style == StiToolBarButtonStyle.Separator)
					{
						val3.Visible = false;
					}
					break;
				}
			}
			for (int num2 = ((ArrangedElementCollection)((Control)this).Controls).Count - 1; num2 >= 0; num2--)
			{
				Control val4 = ((Control)this).Controls[num2];
				if (val4.Visible)
				{
					if (val4 is StiToolButton && ((StiToolButton)(object)val4).Style == StiToolBarButtonStyle.Separator)
					{
						val4.Visible = false;
					}
					break;
				}
			}
			if (((ArrangedElementCollection)((Control)this).Controls).Count > 0)
			{
				for (int k = 0; k < ((ArrangedElementCollection)((Control)this).Controls).Count; k++)
				{
					Control val5 = ((Control)this).Controls[k];
					if (!(val5 is StiToolButton) || ((StiToolButton)(object)val5).Style != StiToolBarButtonStyle.Separator)
					{
						break;
					}
					val5.Visible = false;
				}
				int num3 = ((ArrangedElementCollection)((Control)this).Controls).Count - 1;
				while (num3 >= 0)
				{
					Control val6 = ((Control)this).Controls[num3];
					if (val6 is StiToolButton && ((StiToolButton)(object)val6).Style == StiToolBarButtonStyle.Separator)
					{
						val6.Visible = false;
						num3--;
						continue;
					}
					break;
				}
			}
		}
		catch
		{
		}
		((Control)this).ResumeLayout();
	}

	private void DrawDot(Graphics graphics, int x, int y)
	{
		if (!StiUX.IsDark)
		{
			int num = 2;
			graphics.FillRectangle(SystemBrushes.ControlLightLight, x + 1, y + 1, num, num);
			graphics.FillRectangle(SystemBrushes.ControlDarkDark, x, y, num, num);
		}
	}

	private int GetMaxVerticalPos()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		int num = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
		{
			Control val = item;
			if (val != this)
			{
				num = Math.Max(val.Bottom, num);
			}
		}
		return num;
	}

	private Point CheckLocation(Point location)
	{
		location.X = Math.Max(0, location.X);
		location.Y = Math.Max(0, location.Y);
		int freePos = GetFreePos(location.X, location.Y);
		location.X = Math.Max(freePos, location.X);
		location.Y = Math.Min(CorrectY(location.Y), GetMaxVerticalPos());
		if (location.X + ((Control)this).Width > ((Control)this).Parent.Width)
		{
			location.X = ((Control)this).Parent.Width - ((Control)this).Width;
		}
		return location;
	}

	private void SetLayoutTitle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).RightToLeft == 1)
		{
			((ScrollableControl)this).DockPadding.Right = 12;
			((ScrollableControl)this).DockPadding.Left = 3;
		}
		else
		{
			((ScrollableControl)this).DockPadding.Left = 12;
			((ScrollableControl)this).DockPadding.Right = 3;
		}
		((ScrollableControl)this).DockPadding.Top = 3;
		((ScrollableControl)this).DockPadding.Bottom = 3;
	}

	private void SetLayoutNormal()
	{
		((ScrollableControl)this).DockPadding.Left = 3;
		((ScrollableControl)this).DockPadding.Top = 3;
		((ScrollableControl)this).DockPadding.Bottom = 3;
		((ScrollableControl)this).DockPadding.Right = 3;
	}

	private int CorrectY(int y)
	{
		int val = (int)Math.Round((double)y / (double)((Control)this).Height) * ((Control)this).Height;
		return Math.Max(0, val);
	}

	private int GetFreePos(int x, int y)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		int num = x;
		if (((Control)this).Parent != null)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
			{
				Control val = item;
				if (val != this && val is StiToolBar stiToolBar && ((Control)stiToolBar).Visible && ((Control)stiToolBar).Top == y && x >= ((Control)stiToolBar).Left && num < ((Control)stiToolBar).Right)
				{
					num = ((Control)stiToolBar).Right;
				}
			}
		}
		return num;
	}

	private int GetHeight()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		int num = 0;
		if (((Control)this).Parent != null)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Parent.Controls)
			{
				Control val = item;
				if (val is StiToolBar stiToolBar && ((Control)stiToolBar).Visible && ((Control)stiToolBar).Bottom > num)
				{
					num = ((Control)stiToolBar).Bottom;
				}
			}
		}
		return num;
	}

	public void DoAutoSize()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		int num = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			num = Math.Max(val.Right, num);
		}
		((Control)this).Width = num + 4;
	}

	public void CheckState()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if ((int)((Control)this).Dock == 0 && ((Control)this).Parent != null && !(((Control)this).Parent is Form) && !((Component)this).DesignMode)
		{
			((Control)this).Parent.ClientSize = new Size(((Control)this).Parent.ClientSize.Width, GetHeight());
		}
	}

	public void PlaceOnControl()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		((Control)this).Top = 0;
		Control parent = ((Control)this).Parent;
		int num = 0;
		int num2;
		while (true)
		{
			num2 = 0;
			foreach (Control item in (ArrangedElementCollection)parent.Controls)
			{
				Control val = item;
				if (val.Top == num && val != this)
				{
					num2 = Math.Max(val.Right, num2);
				}
			}
			if (parent.Width > num2 + ((Control)this).Width || num2 == 0)
			{
				break;
			}
			num += ((Control)this).Height;
		}
		((Control)this).Left = num2;
		((Control)this).Top = num;
	}

	protected override void OnRightToLeftChanged(EventArgs e)
	{
		SetLayoutTitle();
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((e.Button & 0x100000) > 0 && IsOverTitle)
		{
			lastPos = Cursor.Position;
			IsDragging = true;
			movePos = ((Control)this).Location;
		}
		((Control)this).OnMouseDown(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		IsDragging = false;
		((Control)this).OnMouseUp(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (IsDragging)
		{
			if ((int)((Control)this).Dock == 0)
			{
				movePos.Offset(Cursor.Position.X - lastPos.X, Cursor.Position.Y - lastPos.Y);
				if (ChangePos)
				{
					((Control)this).Location = CheckLocation(new Point(movePos.X, CorrectY(movePos.Y)));
				}
				lastPos = Cursor.Position;
			}
			CheckState();
		}
		if ((int)((Control)this).Dock == 0)
		{
			if (IsOverTitle)
			{
				Cursor.Current = Cursors.SizeAll;
			}
			else
			{
				Cursor.Current = Cursors.Default;
			}
		}
	}

	protected override void OnPaintBackground(PaintEventArgs p)
	{
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Invalid comparison between Unknown and I4
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (((Control)this).BackgroundImage == null)
		{
			if (StiUX.IsDark)
			{
				StiDrawing.FillRectangle(graphics, StiUX.Background, rectangle);
			}
			else if (ControlStyle == StiControlStyle.Office2013Blue)
			{
				SolidBrush val = new SolidBrush(Color.White);
				try
				{
					graphics.FillRectangle((Brush)(object)val, rectangle);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else if (ControlStyle == StiControlStyle.Office2010)
			{
				SolidBrush val2 = new SolidBrush(Color.FromArgb(245, 245, 245));
				try
				{
					graphics.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				Brush controlBrush = StiBrushes.GetControlBrush(rectangle, 90f);
				try
				{
					graphics.FillRectangle(controlBrush, rectangle);
				}
				finally
				{
					((IDisposable)controlBrush)?.Dispose();
				}
			}
		}
		else
		{
			StiControlPaint.DrawImageBackground(graphics, ((Control)this).BackgroundImage, rectangle);
		}
		Pen val3 = new Pen(StiColorUtils.Dark(SystemColors.Control, 30));
		try
		{
			if (ControlStyle == StiControlStyle.Office2013Blue || ControlStyle == StiControlStyle.Office2010)
			{
				val3.Color = StiUX.Separator;
				if (!StiUX.IsDark)
				{
					val3.DashPattern = new float[2] { 1f, 2f };
				}
				switch (lineStyle)
				{
				case StiToolBarLineStyle.Top:
					graphics.DrawLine(val3, rectangle.X, rectangle.Y, rectangle.Right - 1, rectangle.Y);
					break;
				case StiToolBarLineStyle.Bottom:
					graphics.DrawLine(val3, rectangle.X, rectangle.Bottom - 1, rectangle.Right - 1, rectangle.Bottom - 1);
					break;
				case StiToolBarLineStyle.All:
					graphics.DrawRectangle(val3, rectangle.X, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1);
					break;
				default:
					graphics.DrawLine(val3, rectangle.X, rectangle.Top, rectangle.Right - 1, rectangle.Top);
					graphics.DrawLine(val3, rectangle.X, rectangle.Bottom - 1, rectangle.Right - 1, rectangle.Bottom - 1);
					break;
				}
			}
			else
			{
				graphics.DrawLine(val3, rectangle.X, rectangle.Bottom - 1, rectangle.Right - 1, rectangle.Bottom - 1);
				graphics.DrawLine(val3, rectangle.Right - 1, rectangle.Bottom - 1, rectangle.Right - 1, rectangle.Y);
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		if (!StiUX.IsDark && ControlStyle == StiControlStyle.Flat)
		{
			int num = (((Control)this).Height - 16) / 2;
			int x = 4;
			if ((int)((Control)this).RightToLeft == 1)
			{
				x = ((Control)this).Width - 6;
			}
			DrawDot(graphics, x, num);
			DrawDot(graphics, x, num + 4);
			DrawDot(graphics, x, num + 8);
			DrawDot(graphics, x, num + 12);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((ScrollableControl)this).OnVisibleChanged(e);
		CheckState();
	}

	public StiToolBar()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		SetLayoutTitle();
		((Control)this).Height = 28;
	}
}
