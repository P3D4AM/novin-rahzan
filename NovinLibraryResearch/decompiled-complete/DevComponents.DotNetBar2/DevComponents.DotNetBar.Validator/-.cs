using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

internal class _2FFB : Control
{
	private struct 〆
	{
		public Rectangle ঋ;

		public Color ᖳ;

		public eHighlightColor _3007;

		public 〆(Rectangle bounds, Color backColor, eHighlightColor highlightColor)
		{
			ঋ = bounds;
			ᖳ = backColor;
			_3007 = highlightColor;
		}
	}

	private Dictionary<Control, eHighlightColor> __2FF2;

	private List<〆> __2FFF = new List<〆>();

	private bool __3000;

	private bool __3001;

	private Control __3002;

	private eHighlightColor _ᚾ = eHighlightColor.Blue;

	private Color[] __2FFA;

	public Control _2FFC
	{
		get
		{
			return __3002;
		}
		set
		{
			__3002 = value;
		}
	}

	public eHighlightColor _2FFD
	{
		get
		{
			return _ᚾ;
		}
		set
		{
			_ᚾ = value;
		}
	}

	public Color[] _2FFE
	{
		get
		{
			return __2FFA;
		}
		set
		{
			__2FFA = value;
		}
	}

	public _2FFB(Dictionary<Control, eHighlightColor> highlights)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		__2FF2 = highlights;
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
		foreach (〆 item in __2FFF)
		{
			Color[] array = _3003(item._3007);
			Rectangle ঋ = item.ঋ;
			Color ᖳ = item.ᖳ;
			ঋ.Inflate(1, 1);
			DisplayHelp.FillRectangle(graphics, ঋ, ᖳ);
			ঋ.Inflate(-1, -1);
			DisplayHelp.FillRoundedRectangle(graphics, ঋ, 2, array[0]);
			ঋ.Inflate(-2, -2);
			DisplayHelp.DrawRectangle(graphics, array[2], ঋ);
			ঋ.Inflate(1, 1);
			DisplayHelp.DrawRoundedRectangle(graphics, array[1], ঋ, 2);
		}
		((Control)this).OnPaint(e);
	}

	private Color[] _3003(eHighlightColor _065B)
	{
		Color[] array = new Color[3];
		switch (_065B)
		{
		case eHighlightColor.Blue:
		{
			ref Color reference13 = ref array[0];
			reference13 = ColorScheme.GetColor(172, 6986964);
			ref Color reference14 = ref array[1];
			reference14 = ColorScheme.GetColor(6986964);
			ref Color reference15 = ref array[2];
			reference15 = ColorScheme.GetColor(6127268);
			break;
		}
		case eHighlightColor.Orange:
		{
			ref Color reference10 = ref array[0];
			reference10 = ColorScheme.GetColor(172, 16751616);
			ref Color reference11 = ref array[1];
			reference11 = ColorScheme.GetColor(16751616);
			ref Color reference12 = ref array[2];
			reference12 = ColorScheme.GetColor(13395456);
			break;
		}
		case eHighlightColor.Green:
		{
			ref Color reference16 = ref array[0];
			reference16 = ColorScheme.GetColor(172, 7450993);
			ref Color reference17 = ref array[1];
			reference17 = ColorScheme.GetColor(7450993);
			ref Color reference18 = ref array[2];
			reference18 = ColorScheme.GetColor(3381555);
			break;
		}
		case eHighlightColor.Custom:
			if (__2FFA == null || __2FFA.Length < 3)
			{
				ref Color reference4 = ref array[0];
				reference4 = Color.Red;
				ref Color reference5 = ref array[1];
				reference5 = Color.Red;
				ref Color reference6 = ref array[2];
				reference6 = Color.Red;
			}
			else
			{
				ref Color reference7 = ref array[0];
				reference7 = __2FFA[0];
				ref Color reference8 = ref array[1];
				reference8 = __2FFA[1];
				ref Color reference9 = ref array[2];
				reference9 = __2FFA[2];
			}
			break;
		default:
		{
			ref Color reference = ref array[0];
			reference = ColorScheme.GetColor(172, 12988464);
			ref Color reference2 = ref array[1];
			reference2 = ColorScheme.GetColor(12988464);
			ref Color reference3 = ref array[2];
			reference3 = ColorScheme.GetColor(10027008);
			break;
		}
		}
		return array;
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).Visible && !__3001)
		{
			ᱎ();
		}
		((Control)this).OnVisibleChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (!__3000)
		{
			ᱎ();
		}
		((Control)this).OnHandleCreated(e);
	}

	internal void ᱎ()
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Expected O, but got Unknown
		if (__3001 || !((Control)this).IsHandleCreated)
		{
			return;
		}
		try
		{
			__3001 = true;
			((Control)this).Region = null;
			__2FFF.Clear();
			if (__2FF2 == null)
			{
				return;
			}
			if (__2FF2.Count == 0 && __3002 == null)
			{
				((Control)this).Visible = false;
				return;
			}
			bool flag = true;
			Region val = null;
			foreach (KeyValuePair<Control, eHighlightColor> item in __2FF2)
			{
				if (item.Value == eHighlightColor.None || !_3004(item.Key))
				{
					continue;
				}
				if (item.Key == __3002)
				{
					flag = false;
				}
				Rectangle rectangle = 々(item.Key);
				if (!rectangle.IsEmpty)
				{
					rectangle.Inflate(2, 2);
					__2FFF.Add(new 〆(rectangle, _1DD4(item.Key.Parent), item.Value));
					if (val == null)
					{
						val = new Region(rectangle);
					}
					else
					{
						val.Union(rectangle);
					}
					rectangle.Inflate(-3, -3);
					val.Exclude(rectangle);
				}
			}
			if (flag && __3002 != null && __3002.Visible)
			{
				Rectangle rectangle2 = 々(__3002);
				if (!rectangle2.IsEmpty)
				{
					rectangle2.Inflate(2, 2);
					__2FFF.Add(new 〆(rectangle2, _1DD4(__3002.Parent), _ᚾ));
					if (val == null)
					{
						val = new Region(rectangle2);
					}
					else
					{
						val.Union(rectangle2);
					}
					rectangle2.Inflate(-3, -3);
					val.Exclude(rectangle2);
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
			__3001 = false;
			__3000 = true;
		}
	}

	private bool _3004(Control ॼ)
	{
		if (!ॼ.Visible)
		{
			return false;
		}
		if (ॼ.Parent == null || !ॼ.IsHandleCreated)
		{
			return ॼ.Visible;
		}
		_0EFE.RECT r = default(_0EFE.RECT);
		_0EFE.GetWindowRect(ॼ.Handle, ref r);
		Point point = ॼ.Parent.PointToClient(new Point(r.Left + 3, r.Top + 3));
		IntPtr intPtr = _089F.ChildWindowFromPointEx(ॼ.Parent.Handle, new _089F.POINT(point.X, point.Y), 1u);
		if (intPtr == IntPtr.Zero)
		{
			return ॼ.Visible;
		}
		Control val = Control.FromHandle(intPtr);
		if (val != null && val != ॼ && val != this && val != ॼ.Parent)
		{
			return false;
		}
		return ॼ.Visible;
	}

	private Color _1DD4(Control ॼ)
	{
		Color color = ॼ.BackColor;
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
		ᱎ();
		((Control)this).OnResize(e);
	}

	private Rectangle 々(Control _0BDE)
	{
		if (!_0BDE.IsHandleCreated)
		{
			return Rectangle.Empty;
		}
		_0EFE.RECT r = default(_0EFE.RECT);
		_0EFE.GetWindowRect(_0BDE.Handle, ref r);
		Point location = ((Control)this).PointToClient(r.Location);
		return new Rectangle(location, r.Size);
	}
}
internal class _3008 : IErrorProvider
{
	private ErrorProvider __2FEC;

	private int __300A;

	public int _3009
	{
		get
		{
			return __300A;
		}
		set
		{
			__300A = value;
		}
	}

	public _3008(ErrorProvider errorProvider)
	{
		__2FEC = errorProvider;
	}

	public _3008(ErrorProvider errorProvider, int iconPadding)
	{
		__2FEC = errorProvider;
		__300A = iconPadding;
	}

	public void SetError(Control control, string value)
	{
		__2FEC.SetError(control, value);
		if (__300A != 0)
		{
			__2FEC.SetIconPadding(control, __300A);
		}
	}

	public void ClearError(Control control)
	{
		__2FEC.SetError(control, (string)null);
		if (__300A != 0)
		{
			__2FEC.SetIconPadding(control, 0);
		}
	}
}
