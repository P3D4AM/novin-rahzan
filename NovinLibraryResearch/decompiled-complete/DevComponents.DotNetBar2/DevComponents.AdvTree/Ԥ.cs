using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using DevComponents.AdvTree.Display;
using DevComponents.AdvTree.Interop;
using DevComponents.DotNetBar;

namespace DevComponents.AdvTree;

internal class Ԥ : Control
{
	private int _ס = -1;

	private ColumnHeaderCollection _រ;

	private Cursor _נ;

	private ColumnHeader __0601;

	private ColumnHeader _ល;

	private Point _ᐨ = Point.Empty;

	internal DateTime វ = DateTime.MinValue;

	internal int Ԩ
	{
		get
		{
			return _ס;
		}
		set
		{
			_ס = value;
			((Control)this).Invalidate();
		}
	}

	public ColumnHeaderCollection យ
	{
		get
		{
			return _រ;
		}
		set
		{
			_រ = value;
		}
	}

	public Ԥ()
	{
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)4352, true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		DisplayHelp.FillRectangle(graphics, ((Control)this).ClientRectangle, ((Control)this).BackColor);
		if (_រ == null)
		{
			return;
		}
		AdvTree advTree = _173A();
		if (advTree != null)
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
			if (advTree.AntiAlias)
			{
				graphics.SmoothingMode = (SmoothingMode)4;
				graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
			}
			if (advTree.Zoom != 1f)
			{
				Matrix transform = گ(advTree.Zoom);
				graphics.Transform = transform;
			}
			advTree.NodeDisplay._070D(_រ, graphics, _070E: true);
			int ס = _ס;
			if (ס >= 0)
			{
				ស.អ(graphics, advTree, ס, _រ);
			}
			if (advTree.AntiAlias)
			{
				graphics.SmoothingMode = smoothingMode;
				graphics.TextRenderingHint = textRenderingHint;
			}
		}
		((Control)this).OnPaint(e);
	}

	private Point ឝ(float ڰ, int و, int ى)
	{
		if (ڰ == 1f)
		{
			return new Point(و, ى);
		}
		Point[] array = new Point[1]
		{
			new Point(و, ى)
		};
		Matrix val = گ(ڰ);
		try
		{
			val.Invert();
			val.TransformPoints(array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return array[0];
	}

	private Matrix گ(float ڰ)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		return new Matrix(ڰ, 0f, 0f, ڰ, 0f, 0f);
	}

	private AdvTree _173A()
	{
		return ((Control)this).Parent as AdvTree;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		AdvTree advTree = _173A();
		Point point = ឝ(advTree.Zoom, e.X, e.Y);
		ColumnHeader columnHeader = advTree.ي(point.X, point.Y, _រ);
		if (__0601 != columnHeader)
		{
			if (__0601 != null)
			{
				__0601.InternalMouseLeave((EventArgs)(object)e);
			}
			__0601 = columnHeader;
			if (__0601 != null)
			{
				__0601.InternalMouseEnter((EventArgs)(object)e);
				DevComponents.AdvTree.Interop._0EFE.ఘ((Control)(object)this);
			}
		}
		columnHeader?.InternalMouseMove(e);
		if ((int)e.Button == 1048576 && _ល != null && advTree.AllowUserToReorderColumns && Math.Abs(_ᐨ.X - e.X) > 1)
		{
			advTree._064F(point.X, point.Y);
		}
		else
		{
			if (advTree == null || (int)e.Button != 0 || !advTree.AllowUserToResizeColumns)
			{
				return;
			}
			if (advTree.ه(point.X, point.Y))
			{
				if (_נ == (Cursor)null)
				{
					_נ = ((Control)this).Cursor;
					((Control)this).Cursor = Cursors.VSplit;
				}
			}
			else
			{
				ឞ();
			}
			((Control)this).OnMouseMove(e);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Invalid comparison between Unknown and I4
		AdvTree advTree = _173A();
		Point point = Point.Empty;
		bool flag = false;
		_ᐨ = e.Location;
		if (advTree != null)
		{
			point = ឝ(advTree.Zoom, e.X, e.Y);
		}
		if (advTree != null && advTree.AllowUserToResizeColumns && (int)e.Button == 1048576)
		{
			flag = advTree.ه(point.X, point.Y);
		}
		if (advTree != null)
		{
			point = ឝ(advTree.Zoom, e.X, e.Y);
			ColumnHeader columnHeader = advTree.ي(point.X, point.Y, _រ);
			if (columnHeader != null)
			{
				_ល = columnHeader;
				if (!flag)
				{
					columnHeader.OnMouseDown(e);
				}
			}
		}
		if (advTree != null && (int)e.Button == 1048576 && advTree.AllowUserToResizeColumns)
		{
			if (flag)
			{
				advTree._0650(point.X, point.Y);
			}
			((Control)this).Invalidate();
			((Control)this).OnMouseDown(e);
		}
	}

	protected override void OnMouseHover(EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		if ((int)Control.MouseButtons == 0)
		{
			AdvTree advTree = _173A();
			Point point = ((Control)this).PointToClient(Control.MousePosition);
			point = ឝ(advTree.Zoom, point.X, point.Y);
			advTree.ي(point.X, point.Y, _រ)?.InternalMouseHover(e);
		}
		((Control)this).OnMouseHover(e);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		ឞ();
		_ᐨ = Point.Empty;
		if (_ល != null)
		{
			_ល.OnMouseUp(e);
			_ល = null;
		}
		((Control)this).Invalidate();
		((Control)this).OnMouseUp(e);
	}

	protected override void OnClick(EventArgs e)
	{
		AdvTree advTree = _173A();
		Point empty = Point.Empty;
		if (advTree != null)
		{
			Point point = ((Control)advTree).PointToClient(Control.MousePosition);
			empty = ឝ(advTree.Zoom, point.X, point.Y);
			advTree.ي(empty.X, empty.Y, _រ)?.OnClick(e);
		}
		((Control)this).Invalidate();
		((Control)this).OnClick(e);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		AdvTree advTree = _173A();
		Point empty = Point.Empty;
		if (advTree != null)
		{
			Point point = ((Control)advTree).PointToClient(Control.MousePosition);
			empty = ឝ(advTree.Zoom, point.X, point.Y);
			ColumnHeader columnHeader = advTree.ي(empty.X, empty.Y, _រ);
			if (columnHeader != null)
			{
				DateTime now = DateTime.Now;
				if (វ != DateTime.MinValue && now.Subtract(វ).TotalMilliseconds <= (double)SystemInformation.DoubleClickTime)
				{
					វ = DateTime.MinValue;
					return;
				}
				វ = DateTime.MinValue;
				columnHeader.OnDoubleClick(e);
			}
		}
		((Control)this).Invalidate();
		((Control)this).OnDoubleClick(e);
	}

	private void ឞ()
	{
		if (_נ != (Cursor)null)
		{
			((Control)this).Cursor = _נ;
			_נ = null;
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		if (__0601 != null)
		{
			__0601.InternalMouseLeave(e);
			__0601 = null;
		}
		if ((int)Control.MouseButtons != 1048576)
		{
			ឞ();
		}
		((Control)this).OnMouseLeave(e);
	}
}
