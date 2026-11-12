using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[Description("Horizontal or Vertical Line Control")]
public class Line : Control
{
	private eItemAlignment _LineAlignment = eItemAlignment.Center;

	private float _DashOffset;

	private DashStyle _DashStyle;

	private int _Thickness = 1;

	private bool _VerticalLine;

	private eLineEndType _StartLineCap;

	private static readonly Size DefaultCapSize = new Size(6, 6);

	private Size _StartLineCapSize = DefaultCapSize;

	private eLineEndType _EndLineCap;

	private Size _EndLineCapSize = DefaultCapSize;

	private Point LineStartPoint
	{
		get
		{
			if (!_VerticalLine)
			{
				if (_LineAlignment == eItemAlignment.Center)
				{
					return new Point(0, ((Control)this).Height / 2);
				}
				if (_LineAlignment == eItemAlignment.Near)
				{
					return new Point(0, _Thickness / 2 + ((_StartLineCap != 0) ? (_StartLineCapSize.Height / 2) : 0));
				}
				if (_LineAlignment == eItemAlignment.Far)
				{
					return new Point(0, ((Control)this).Height - _Thickness / 2 - ((_StartLineCap != 0) ? (_StartLineCapSize.Height / 2) : 0));
				}
			}
			else
			{
				if (_LineAlignment == eItemAlignment.Center)
				{
					return new Point(((Control)this).Width / 2, 0);
				}
				if (_LineAlignment == eItemAlignment.Near)
				{
					return new Point(_Thickness / 2 + ((_EndLineCap != 0) ? (_EndLineCapSize.Width / 2) : 0), 0);
				}
				if (_LineAlignment == eItemAlignment.Far)
				{
					return new Point(((Control)this).Width - _Thickness / 2 - ((_EndLineCap != 0) ? (_EndLineCapSize.Width / 2) : 0), 0);
				}
			}
			return Point.Empty;
		}
	}

	private Point LineEndPoint
	{
		get
		{
			if (!_VerticalLine)
			{
				return new Point(((Control)this).Width, LineStartPoint.Y);
			}
			return new Point(LineStartPoint.X, ((Control)this).Height);
		}
	}

	[DefaultValue(eItemAlignment.Center)]
	[Category("Appearance")]
	[Description("Specifies the line alignment within control bounds.")]
	public eItemAlignment LineAlignment
	{
		get
		{
			return _LineAlignment;
		}
		set
		{
			_LineAlignment = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Specifies distance from the start of a line to the beginning of a dash pattern.")]
	public float DashOffset
	{
		get
		{
			return _DashOffset;
		}
		set
		{
			_DashOffset = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Specifies the line dash style.")]
	public DashStyle DashStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _DashStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_DashStyle = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(1)]
	[Category("Appearance")]
	[Description("Indicates line thickness in pixels.")]
	public int Thickness
	{
		get
		{
			return _Thickness;
		}
		set
		{
			_Thickness = value;
			((Control)this).Invalidate();
			if (((Control)this).AutoSize)
			{
				AdjustSize();
			}
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether vertical line is drawn.")]
	public bool VerticalLine
	{
		get
		{
			return _VerticalLine;
		}
		set
		{
			_VerticalLine = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				((Control)this).AutoSize = value;
				AdjustSize();
			}
		}
	}

	[DefaultValue(eLineEndType.None)]
	[Category("Appearance")]
	[Description("Indicates the start of the line cap.")]
	public eLineEndType StartLineCap
	{
		get
		{
			return _StartLineCap;
		}
		set
		{
			if (value != _StartLineCap)
			{
				eLineEndType startLineCap = _StartLineCap;
				_StartLineCap = value;
				OnStartLineCapChanged(startLineCap, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates the size of the start cap.")]
	public Size StartLineCapSize
	{
		get
		{
			return _StartLineCapSize;
		}
		set
		{
			if (value != _StartLineCapSize)
			{
				Size startLineCapSize = _StartLineCapSize;
				_StartLineCapSize = value;
				OnStartLineCapSizeChanged(startLineCapSize, value);
			}
		}
	}

	[DefaultValue(eLineEndType.None)]
	[Category("Appearance")]
	[Description("Indicates the start of the line cap.")]
	public eLineEndType EndLineCap
	{
		get
		{
			return _EndLineCap;
		}
		set
		{
			if (value != _EndLineCap)
			{
				eLineEndType endLineCap = _EndLineCap;
				_EndLineCap = value;
				OnEndLineCapChanged(endLineCap, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates end line cap size.")]
	public Size EndLineCapSize
	{
		get
		{
			return _EndLineCapSize;
		}
		set
		{
			_EndLineCapSize = value;
			((Control)this).Refresh();
		}
	}

	public Line()
	{
		((Control)this).SetStyle((ControlStyles)141314, true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)3;
		Pen val = new Pen(((Control)this).ForeColor, (float)_Thickness);
		try
		{
			val.DashStyle = _DashStyle;
			val.DashOffset = _DashOffset;
			Point lineStartPoint = LineStartPoint;
			Point lineEndPoint = LineEndPoint;
			if (_StartLineCap != 0 && _Thickness > 1)
			{
				if (_VerticalLine)
				{
					lineStartPoint.Y += _StartLineCapSize.Height / 2;
				}
				else
				{
					lineStartPoint.X += _StartLineCapSize.Width / 2;
				}
			}
			if (_EndLineCap != 0 && _Thickness > 1)
			{
				if (_VerticalLine)
				{
					lineEndPoint.Y -= _EndLineCapSize.Height / 2;
				}
				else
				{
					lineEndPoint.X -= _EndLineCapSize.Width / 2;
				}
			}
			graphics.DrawLine(val, lineStartPoint, lineEndPoint);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (_StartLineCap != 0 && _StartLineCapSize.Width > 0 && _StartLineCapSize.Height > 0)
		{
			DrawLineCap(graphics, LineStartPoint, _StartLineCap, _StartLineCapSize, isStartCap: true);
		}
		if (_EndLineCap != 0 && _EndLineCapSize.Width > 0 && _EndLineCapSize.Height > 0)
		{
			DrawLineCap(graphics, LineEndPoint, _EndLineCap, _EndLineCapSize, isStartCap: false);
		}
		graphics.SmoothingMode = smoothingMode;
		((Control)this).OnPaint(e);
	}

	private void DrawLineCap(Graphics g, Point linePoint, eLineEndType lineCap, Size capSize, bool isStartCap)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		switch (lineCap)
		{
		case eLineEndType.Arrow:
		{
			SmoothingMode smoothingMode4 = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)2;
			GraphicsPath val5 = new GraphicsPath();
			try
			{
				if (isStartCap)
				{
					if (VerticalLine)
					{
						val5.AddLines(new Point[3]
						{
							new Point(linePoint.X, linePoint.Y),
							new Point(linePoint.X - capSize.Width / 2, linePoint.Y + capSize.Height),
							new Point(linePoint.X + capSize.Width / 2, linePoint.Y + capSize.Height)
						});
					}
					else
					{
						val5.AddLines(new Point[3]
						{
							new Point(linePoint.X, linePoint.Y),
							new Point(linePoint.X + capSize.Width, linePoint.Y - capSize.Height / 2),
							new Point(linePoint.X + capSize.Width, linePoint.Y + capSize.Height / 2)
						});
					}
				}
				else if (VerticalLine)
				{
					val5.AddLines(new Point[3]
					{
						new Point(linePoint.X, linePoint.Y),
						new Point(linePoint.X + capSize.Width / 2, linePoint.Y - capSize.Height),
						new Point(linePoint.X - capSize.Width / 2, linePoint.Y - capSize.Height)
					});
				}
				else
				{
					val5.AddLines(new Point[3]
					{
						new Point(linePoint.X, linePoint.Y),
						new Point(linePoint.X - capSize.Width, linePoint.Y + capSize.Height / 2),
						new Point(linePoint.X - capSize.Width, linePoint.Y - capSize.Height / 2)
					});
				}
				val5.CloseAllFigures();
				SolidBrush val6 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					g.FillPath((Brush)(object)val6, val5);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			g.SmoothingMode = smoothingMode4;
			break;
		}
		case eLineEndType.Circle:
		{
			SmoothingMode smoothingMode2 = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)2;
			SolidBrush val2 = new SolidBrush(((Control)this).ForeColor);
			try
			{
				if (VerticalLine && isStartCap)
				{
					g.FillEllipse((Brush)(object)val2, new Rectangle(linePoint.X - capSize.Width / 2, linePoint.Y, capSize.Width, capSize.Height));
				}
				else if (VerticalLine)
				{
					g.FillEllipse((Brush)(object)val2, new Rectangle(linePoint.X - capSize.Width / 2, linePoint.Y - capSize.Height - 1, capSize.Width, capSize.Height));
				}
				else if (isStartCap)
				{
					g.FillEllipse((Brush)(object)val2, new Rectangle(linePoint.X, linePoint.Y - capSize.Height / 2, capSize.Width, capSize.Height));
				}
				else
				{
					g.FillEllipse((Brush)(object)val2, new Rectangle(linePoint.X - capSize.Width - 1, linePoint.Y - capSize.Height / 2, capSize.Width, capSize.Height));
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			g.SmoothingMode = smoothingMode2;
			break;
		}
		case eLineEndType.Diamond:
		{
			SmoothingMode smoothingMode3 = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)2;
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				if (isStartCap)
				{
					if (VerticalLine)
					{
						val3.AddLines(new Point[4]
						{
							new Point(linePoint.X, linePoint.Y),
							new Point(linePoint.X - capSize.Width / 2, linePoint.Y + capSize.Height / 2),
							new Point(linePoint.X, linePoint.Y + capSize.Height),
							new Point(linePoint.X + capSize.Width / 2, linePoint.Y + capSize.Height / 2)
						});
					}
					else
					{
						val3.AddLines(new Point[4]
						{
							new Point(linePoint.X, linePoint.Y),
							new Point(linePoint.X + capSize.Width / 2, linePoint.Y - capSize.Height / 2),
							new Point(linePoint.X + capSize.Width, linePoint.Y),
							new Point(linePoint.X + capSize.Width / 2, linePoint.Y + capSize.Height / 2)
						});
					}
				}
				else if (VerticalLine)
				{
					linePoint.Y--;
					val3.AddLines(new Point[4]
					{
						new Point(linePoint.X, linePoint.Y),
						new Point(linePoint.X + capSize.Width / 2, linePoint.Y - capSize.Height / 2),
						new Point(linePoint.X, linePoint.Y - capSize.Height),
						new Point(linePoint.X - capSize.Width / 2, linePoint.Y - capSize.Height / 2)
					});
				}
				else
				{
					linePoint.X--;
					val3.AddLines(new Point[4]
					{
						new Point(linePoint.X, linePoint.Y),
						new Point(linePoint.X - capSize.Width / 2, linePoint.Y - capSize.Height / 2),
						new Point(linePoint.X - capSize.Width, linePoint.Y),
						new Point(linePoint.X - capSize.Width / 2, linePoint.Y + capSize.Height / 2)
					});
				}
				val3.CloseAllFigures();
				SolidBrush val4 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					g.FillPath((Brush)(object)val4, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			g.SmoothingMode = smoothingMode3;
			break;
		}
		case eLineEndType.Rectangle:
		{
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)2;
			SolidBrush val = new SolidBrush(((Control)this).ForeColor);
			try
			{
				if (VerticalLine && isStartCap)
				{
					g.FillRectangle((Brush)(object)val, new Rectangle(linePoint.X - capSize.Width / 2, linePoint.Y, capSize.Width, capSize.Height));
				}
				else if (VerticalLine)
				{
					g.FillRectangle((Brush)(object)val, new Rectangle(linePoint.X - capSize.Width / 2, linePoint.Y - capSize.Height - 1, capSize.Width, capSize.Height));
				}
				else if (isStartCap)
				{
					g.FillRectangle((Brush)(object)val, new Rectangle(linePoint.X, linePoint.Y - capSize.Height / 2, capSize.Width, capSize.Height));
				}
				else
				{
					g.FillRectangle((Brush)(object)val, new Rectangle(linePoint.X - capSize.Width - 1, linePoint.Y - capSize.Height / 2, capSize.Width, capSize.Height));
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			g.SmoothingMode = smoothingMode;
			break;
		}
		}
	}

	private void AdjustSize()
	{
		((Control)this).Size = ((Control)this).GetPreferredSize(((Control)this).Size);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (VerticalLine)
		{
			return new Size(_Thickness, ((Control)this).Height);
		}
		return new Size(((Control)this).Width, _Thickness);
	}

	protected virtual void OnStartLineCapChanged(eLineEndType oldValue, eLineEndType newValue)
	{
		((Control)this).Refresh();
	}

	protected virtual void OnStartLineCapSizeChanged(Size oldValue, Size newValue)
	{
		((Control)this).Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeStartLineCapSize()
	{
		return _StartLineCapSize != DefaultCapSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetStartLineCapSize()
	{
		StartLineCapSize = DefaultCapSize;
	}

	protected virtual void OnEndLineCapChanged(eLineEndType oldValue, eLineEndType newValue)
	{
		((Control)this).Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeEndLineCapSize()
	{
		return _EndLineCapSize != DefaultCapSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetEndLineCapSize()
	{
		EndLineCapSize = DefaultCapSize;
	}
}
