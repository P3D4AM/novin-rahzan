using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(Line), "Controls.Line.ico")]
[Description("Horizontal or Vertical Line Control")]
[ToolboxItem(true)]
public class Line : Control
{
	private eItemAlignment __2BF9 = eItemAlignment.Center;

	private float __2BFA;

	private DashStyle __17DD;

	private int __2BFB = 1;

	private bool __2BFC;

	private eLineEndType __2BFD;

	private static readonly Size _2BFE = new Size(6, 6);

	private Size __2BFF = _2BFE;

	private eLineEndType _Ⰰ;

	private Size _Ⰱ = _2BFE;

	private Point _2BF7
	{
		get
		{
			if (!__2BFC)
			{
				if (__2BF9 == eItemAlignment.Center)
				{
					return new Point(0, ((Control)this).Height / 2);
				}
				if (__2BF9 == eItemAlignment.Near)
				{
					return new Point(0, __2BFB / 2 + ((__2BFD != 0) ? (__2BFF.Height / 2) : 0));
				}
				if (__2BF9 == eItemAlignment.Far)
				{
					return new Point(0, ((Control)this).Height - __2BFB / 2 - ((__2BFD != 0) ? (__2BFF.Height / 2) : 0));
				}
			}
			else
			{
				if (__2BF9 == eItemAlignment.Center)
				{
					return new Point(((Control)this).Width / 2, 0);
				}
				if (__2BF9 == eItemAlignment.Near)
				{
					return new Point(__2BFB / 2 + ((_Ⰰ != 0) ? (_Ⰱ.Width / 2) : 0), 0);
				}
				if (__2BF9 == eItemAlignment.Far)
				{
					return new Point(((Control)this).Width - __2BFB / 2 - ((_Ⰰ != 0) ? (_Ⰱ.Width / 2) : 0), 0);
				}
			}
			return Point.Empty;
		}
	}

	private Point _2BF8
	{
		get
		{
			if (!__2BFC)
			{
				return new Point(((Control)this).Width, _2BF7.Y);
			}
			return new Point(_2BF7.X, ((Control)this).Height);
		}
	}

	[Description("Specifies the line alignment within control bounds.")]
	[DefaultValue(eItemAlignment.Center)]
	[Category("Appearance")]
	public eItemAlignment LineAlignment
	{
		get
		{
			return __2BF9;
		}
		set
		{
			__2BF9 = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(0f)]
	[Description("Specifies distance from the start of a line to the beginning of a dash pattern.")]
	[Category("Appearance")]
	public float DashOffset
	{
		get
		{
			return __2BFA;
		}
		set
		{
			__2BFA = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[Description("Specifies the line dash style.")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public DashStyle DashStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return __17DD;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			__17DD = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(1)]
	[Description("Indicates line thickness in pixels.")]
	[Category("Appearance")]
	public int Thickness
	{
		get
		{
			return __2BFB;
		}
		set
		{
			__2BFB = value;
			((Control)this).Invalidate();
			if (((Control)this).AutoSize)
			{
				_21DF();
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates whether vertical line is drawn.")]
	[DefaultValue(false)]
	public bool VerticalLine
	{
		get
		{
			return __2BFC;
		}
		set
		{
			__2BFC = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
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
				_21DF();
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(eLineEndType.None)]
	[Description("Indicates the start of the line cap.")]
	public eLineEndType StartLineCap
	{
		get
		{
			return __2BFD;
		}
		set
		{
			if (value != __2BFD)
			{
				eLineEndType oldValue = __2BFD;
				__2BFD = value;
				OnStartLineCapChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the size of the start cap.")]
	[Category("Appearance")]
	public Size StartLineCapSize
	{
		get
		{
			return __2BFF;
		}
		set
		{
			if (value != __2BFF)
			{
				Size oldValue = __2BFF;
				__2BFF = value;
				OnStartLineCapSizeChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the start of the line cap.")]
	[Category("Appearance")]
	[DefaultValue(eLineEndType.None)]
	public eLineEndType EndLineCap
	{
		get
		{
			return _Ⰰ;
		}
		set
		{
			if (value != _Ⰰ)
			{
				eLineEndType ⰰ = _Ⰰ;
				_Ⰰ = value;
				OnEndLineCapChanged(ⰰ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates end line cap size.")]
	public Size EndLineCapSize
	{
		get
		{
			return _Ⰱ;
		}
		set
		{
			_Ⰱ = value;
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
		Pen val = new Pen(((Control)this).ForeColor, (float)__2BFB);
		try
		{
			val.DashStyle = __17DD;
			val.DashOffset = __2BFA;
			Point point = _2BF7;
			Point point2 = _2BF8;
			if (__2BFD != 0 && __2BFB > 1)
			{
				if (__2BFC)
				{
					point.Y += __2BFF.Height / 2;
				}
				else
				{
					point.X += __2BFF.Width / 2;
				}
			}
			if (_Ⰰ != 0 && __2BFB > 1)
			{
				if (__2BFC)
				{
					point2.Y -= _Ⰱ.Height / 2;
				}
				else
				{
					point2.X -= _Ⰱ.Width / 2;
				}
			}
			graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (__2BFD != 0 && __2BFF.Width > 0 && __2BFF.Height > 0)
		{
			Ⰲ(graphics, _2BF7, __2BFD, __2BFF, Ⰶ: true);
		}
		if (_Ⰰ != 0 && _Ⰱ.Width > 0 && _Ⰱ.Height > 0)
		{
			Ⰲ(graphics, _2BF8, _Ⰰ, _Ⰱ, Ⰶ: false);
		}
		graphics.SmoothingMode = smoothingMode;
		((Control)this).OnPaint(e);
	}

	private void Ⰲ(Graphics _٥, Point Ⰳ, eLineEndType Ⰴ, Size Ⰵ, bool Ⰶ)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Expected O, but got Unknown
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		switch (Ⰴ)
		{
		case eLineEndType.Arrow:
		{
			SmoothingMode smoothingMode4 = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)2;
			GraphicsPath val5 = new GraphicsPath();
			try
			{
				if (Ⰶ)
				{
					if (VerticalLine)
					{
						val5.AddLines(new Point[3]
						{
							new Point(Ⰳ.X, Ⰳ.Y),
							new Point(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y + Ⰵ.Height),
							new Point(Ⰳ.X + Ⰵ.Width / 2, Ⰳ.Y + Ⰵ.Height)
						});
					}
					else
					{
						val5.AddLines(new Point[3]
						{
							new Point(Ⰳ.X, Ⰳ.Y),
							new Point(Ⰳ.X + Ⰵ.Width, Ⰳ.Y - Ⰵ.Height / 2),
							new Point(Ⰳ.X + Ⰵ.Width, Ⰳ.Y + Ⰵ.Height / 2)
						});
					}
				}
				else if (VerticalLine)
				{
					val5.AddLines(new Point[3]
					{
						new Point(Ⰳ.X, Ⰳ.Y),
						new Point(Ⰳ.X + Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height),
						new Point(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height)
					});
				}
				else
				{
					val5.AddLines(new Point[3]
					{
						new Point(Ⰳ.X, Ⰳ.Y),
						new Point(Ⰳ.X - Ⰵ.Width, Ⰳ.Y + Ⰵ.Height / 2),
						new Point(Ⰳ.X - Ⰵ.Width, Ⰳ.Y - Ⰵ.Height / 2)
					});
				}
				val5.CloseAllFigures();
				SolidBrush val6 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					_٥.FillPath((Brush)(object)val6, val5);
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
			_٥.SmoothingMode = smoothingMode4;
			break;
		}
		case eLineEndType.Circle:
		{
			SmoothingMode smoothingMode2 = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)2;
			SolidBrush val2 = new SolidBrush(((Control)this).ForeColor);
			try
			{
				if (VerticalLine && Ⰶ)
				{
					_٥.FillEllipse((Brush)(object)val2, new Rectangle(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y, Ⰵ.Width, Ⰵ.Height));
				}
				else if (VerticalLine)
				{
					_٥.FillEllipse((Brush)(object)val2, new Rectangle(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height - 1, Ⰵ.Width, Ⰵ.Height));
				}
				else if (Ⰶ)
				{
					_٥.FillEllipse((Brush)(object)val2, new Rectangle(Ⰳ.X, Ⰳ.Y - Ⰵ.Height / 2, Ⰵ.Width, Ⰵ.Height));
				}
				else
				{
					_٥.FillEllipse((Brush)(object)val2, new Rectangle(Ⰳ.X - Ⰵ.Width - 1, Ⰳ.Y - Ⰵ.Height / 2, Ⰵ.Width, Ⰵ.Height));
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			_٥.SmoothingMode = smoothingMode2;
			break;
		}
		case eLineEndType.Diamond:
		{
			SmoothingMode smoothingMode3 = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)2;
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				if (Ⰶ)
				{
					if (VerticalLine)
					{
						val3.AddLines(new Point[4]
						{
							new Point(Ⰳ.X, Ⰳ.Y),
							new Point(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y + Ⰵ.Height / 2),
							new Point(Ⰳ.X, Ⰳ.Y + Ⰵ.Height),
							new Point(Ⰳ.X + Ⰵ.Width / 2, Ⰳ.Y + Ⰵ.Height / 2)
						});
					}
					else
					{
						val3.AddLines(new Point[4]
						{
							new Point(Ⰳ.X, Ⰳ.Y),
							new Point(Ⰳ.X + Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height / 2),
							new Point(Ⰳ.X + Ⰵ.Width, Ⰳ.Y),
							new Point(Ⰳ.X + Ⰵ.Width / 2, Ⰳ.Y + Ⰵ.Height / 2)
						});
					}
				}
				else if (VerticalLine)
				{
					Ⰳ.Y--;
					val3.AddLines(new Point[4]
					{
						new Point(Ⰳ.X, Ⰳ.Y),
						new Point(Ⰳ.X + Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height / 2),
						new Point(Ⰳ.X, Ⰳ.Y - Ⰵ.Height),
						new Point(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height / 2)
					});
				}
				else
				{
					Ⰳ.X--;
					val3.AddLines(new Point[4]
					{
						new Point(Ⰳ.X, Ⰳ.Y),
						new Point(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height / 2),
						new Point(Ⰳ.X - Ⰵ.Width, Ⰳ.Y),
						new Point(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y + Ⰵ.Height / 2)
					});
				}
				val3.CloseAllFigures();
				SolidBrush val4 = new SolidBrush(((Control)this).ForeColor);
				try
				{
					_٥.FillPath((Brush)(object)val4, val3);
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
			_٥.SmoothingMode = smoothingMode3;
			break;
		}
		case eLineEndType.Rectangle:
		{
			SmoothingMode smoothingMode = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)2;
			SolidBrush val = new SolidBrush(((Control)this).ForeColor);
			try
			{
				if (VerticalLine && Ⰶ)
				{
					_٥.FillRectangle((Brush)(object)val, new Rectangle(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y, Ⰵ.Width, Ⰵ.Height));
				}
				else if (VerticalLine)
				{
					_٥.FillRectangle((Brush)(object)val, new Rectangle(Ⰳ.X - Ⰵ.Width / 2, Ⰳ.Y - Ⰵ.Height - 1, Ⰵ.Width, Ⰵ.Height));
				}
				else if (Ⰶ)
				{
					_٥.FillRectangle((Brush)(object)val, new Rectangle(Ⰳ.X, Ⰳ.Y - Ⰵ.Height / 2, Ⰵ.Width, Ⰵ.Height));
				}
				else
				{
					_٥.FillRectangle((Brush)(object)val, new Rectangle(Ⰳ.X - Ⰵ.Width - 1, Ⰳ.Y - Ⰵ.Height / 2, Ⰵ.Width, Ⰵ.Height));
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			_٥.SmoothingMode = smoothingMode;
			break;
		}
		}
	}

	private void _21DF()
	{
		((Control)this).Size = ((Control)this).GetPreferredSize(((Control)this).Size);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (VerticalLine)
		{
			return new Size(__2BFB, ((Control)this).Height);
		}
		return new Size(((Control)this).Width, __2BFB);
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
		return __2BFF != _2BFE;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetStartLineCapSize()
	{
		StartLineCapSize = _2BFE;
	}

	protected virtual void OnEndLineCapChanged(eLineEndType oldValue, eLineEndType newValue)
	{
		((Control)this).Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeEndLineCapSize()
	{
		return _Ⰱ != _2BFE;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetEndLineCapSize()
	{
		EndLineCapSize = _2BFE;
	}
}
