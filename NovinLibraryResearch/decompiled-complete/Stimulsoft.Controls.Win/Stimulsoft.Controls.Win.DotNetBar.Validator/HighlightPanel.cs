using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

[ToolboxItem(false)]
internal class HighlightPanel : Control
{
	private struct HighlightRegion
	{
		public Rectangle Bounds;

		public Color BackColor;

		public eHighlightColor HighlightColor;

		public HighlightRegion(Rectangle bounds, Color backColor, eHighlightColor highlightColor)
		{
			Bounds = bounds;
			BackColor = backColor;
			HighlightColor = highlightColor;
		}
	}

	private Dictionary<Control, eHighlightColor> _Highlights;

	private List<HighlightRegion> _HighlightRegions = new List<HighlightRegion>();

	private bool _RegionInitialized;

	private bool _UpdatingRegion;

	private Control _FocusHighlightControl;

	private eHighlightColor _FocusHighlightColor = eHighlightColor.Blue;

	private Color[] _CustomHighlightColors;

	public Control FocusHighlightControl
	{
		get
		{
			return _FocusHighlightControl;
		}
		set
		{
			_FocusHighlightControl = value;
		}
	}

	public eHighlightColor FocusHighlightColor
	{
		get
		{
			return _FocusHighlightColor;
		}
		set
		{
			_FocusHighlightColor = value;
		}
	}

	public Color[] CustomHighlightColors
	{
		get
		{
			return _CustomHighlightColors;
		}
		set
		{
			_CustomHighlightColors = value;
		}
	}

	public HighlightPanel(Dictionary<Control, eHighlightColor> highlights)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		_Highlights = highlights;
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		graphics.SmoothingMode = (SmoothingMode)4;
		foreach (HighlightRegion highlightRegion in _HighlightRegions)
		{
			Color[] highlightColors = GetHighlightColors(highlightRegion.HighlightColor);
			Rectangle bounds = highlightRegion.Bounds;
			Color backColor = highlightRegion.BackColor;
			bounds.Inflate(1, 1);
			DisplayHelp.FillRectangle(graphics, bounds, backColor);
			bounds.Inflate(-1, -1);
			DisplayHelp.FillRoundedRectangle(graphics, bounds, 2, highlightColors[0]);
			bounds.Inflate(-2, -2);
			DisplayHelp.DrawRectangle(graphics, highlightColors[2], bounds);
			bounds.Inflate(1, 1);
			DisplayHelp.DrawRoundedRectangle(graphics, highlightColors[1], bounds, 2);
		}
		((Control)this).OnPaint(e);
	}

	private Color[] GetHighlightColors(eHighlightColor color)
	{
		Color[] array = new Color[3];
		switch (color)
		{
		case eHighlightColor.Blue:
			array[0] = ColorScheme.GetColor(172, 6986964);
			array[1] = ColorScheme.GetColor(6986964);
			array[2] = ColorScheme.GetColor(6127268);
			break;
		case eHighlightColor.Orange:
			array[0] = ColorScheme.GetColor(172, 16751616);
			array[1] = ColorScheme.GetColor(16751616);
			array[2] = ColorScheme.GetColor(13395456);
			break;
		case eHighlightColor.Green:
			array[0] = ColorScheme.GetColor(172, 7450993);
			array[1] = ColorScheme.GetColor(7450993);
			array[2] = ColorScheme.GetColor(3381555);
			break;
		case eHighlightColor.Custom:
			if (_CustomHighlightColors == null || _CustomHighlightColors.Length < 3)
			{
				array[0] = Color.Red;
				array[1] = Color.Red;
				array[2] = Color.Red;
			}
			else
			{
				array[0] = _CustomHighlightColors[0];
				array[1] = _CustomHighlightColors[1];
				array[2] = _CustomHighlightColors[2];
			}
			break;
		default:
			array[0] = ColorScheme.GetColor(172, 12988464);
			array[1] = ColorScheme.GetColor(12988464);
			array[2] = ColorScheme.GetColor(10027008);
			break;
		}
		return array;
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).Visible && !_UpdatingRegion)
		{
			UpdateRegion();
		}
		((Control)this).OnVisibleChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (!_RegionInitialized)
		{
			UpdateRegion();
		}
		((Control)this).OnHandleCreated(e);
	}

	internal void UpdateRegion()
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		if (_UpdatingRegion || !((Control)this).IsHandleCreated)
		{
			return;
		}
		try
		{
			_UpdatingRegion = true;
			((Control)this).Region = null;
			_HighlightRegions.Clear();
			if (_Highlights == null)
			{
				return;
			}
			if (_Highlights.Count == 0 && _FocusHighlightControl == null)
			{
				((Control)this).Visible = false;
				return;
			}
			bool flag = true;
			Region val = null;
			foreach (KeyValuePair<Control, eHighlightColor> highlight in _Highlights)
			{
				if (highlight.Value == eHighlightColor.None || !GetIsVisible(highlight.Key))
				{
					continue;
				}
				if (highlight.Key == _FocusHighlightControl)
				{
					flag = false;
				}
				Rectangle controlRect = GetControlRect(highlight.Key);
				if (!controlRect.IsEmpty)
				{
					controlRect.Inflate(2, 2);
					_HighlightRegions.Add(new HighlightRegion(controlRect, GetBackColor(highlight.Key.Parent), highlight.Value));
					if (val == null)
					{
						val = new Region(controlRect);
					}
					else
					{
						val.Union(controlRect);
					}
					controlRect.Inflate(-3, -3);
					val.Exclude(controlRect);
				}
			}
			if (flag && _FocusHighlightControl != null && _FocusHighlightControl.Visible)
			{
				Rectangle controlRect2 = GetControlRect(_FocusHighlightControl);
				if (!controlRect2.IsEmpty)
				{
					controlRect2.Inflate(2, 2);
					_HighlightRegions.Add(new HighlightRegion(controlRect2, GetBackColor(_FocusHighlightControl.Parent), _FocusHighlightColor));
					if (val == null)
					{
						val = new Region(controlRect2);
					}
					else
					{
						val.Union(controlRect2);
					}
					controlRect2.Inflate(-3, -3);
					val.Exclude(controlRect2);
				}
			}
			((Control)this).Region = val;
			if (val == null)
			{
				((Control)this).Visible = false;
			}
			else if (!((Control)this).Visible)
			{
				((Control)this).Visible = true;
				((Control)this).BringToFront();
			}
			((Control)this).Invalidate();
		}
		finally
		{
			_UpdatingRegion = false;
			_RegionInitialized = true;
		}
	}

	private bool GetIsVisible(Control control)
	{
		if (!control.Visible)
		{
			return false;
		}
		if (control.Parent == null || !control.IsHandleCreated)
		{
			return control.Visible;
		}
		WinApi.RECT r = default(WinApi.RECT);
		WinApi.GetWindowRect(control.Handle, ref r);
		Point point = control.Parent.PointToClient(new Point(r.Left + 3, r.Top + 3));
		IntPtr intPtr = NativeFunctions.ChildWindowFromPoint(control.Parent.Handle, new NativeFunctions.POINT(point.X, point.Y));
		if (intPtr == IntPtr.Zero)
		{
			return control.Visible;
		}
		Control val = Control.FromHandle(intPtr);
		if (val != null && val != control && val != this && val != control.Parent)
		{
			return false;
		}
		return control.Visible;
	}

	private Color GetBackColor(Control control)
	{
		Color color = control.BackColor;
		if (color.IsEmpty || color == Color.Transparent)
		{
			color = SystemColors.Control;
		}
		else if (color.A < byte.MaxValue)
		{
			color = Color.FromArgb(255, color);
		}
		return color;
	}

	protected override void OnResize(EventArgs e)
	{
		UpdateRegion();
		((Control)this).OnResize(e);
	}

	private Rectangle GetControlRect(Control c)
	{
		if (!c.IsHandleCreated)
		{
			return Rectangle.Empty;
		}
		WinApi.RECT r = default(WinApi.RECT);
		WinApi.GetWindowRect(c.Handle, ref r);
		Point location = ((Control)this).PointToClient(r.Location);
		return new Rectangle(location, r.Size);
	}
}
