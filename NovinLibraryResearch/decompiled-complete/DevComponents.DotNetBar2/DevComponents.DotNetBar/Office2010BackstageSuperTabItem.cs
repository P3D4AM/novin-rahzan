using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

public class Office2010BackstageSuperTabItem : SuperTabItemBaseDisplay
{
	private const int _2087 = 7;

	private const int _2088 = 13;

	public Office2010BackstageSuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	protected override void DrawTabBorder(Graphics g, GraphicsPath path, SuperTabItemStateColorTable ct)
	{
		base.DrawTabBorder(g, path, ct);
		if (!ct.SelectionMarker.IsEmpty)
		{
			_2089(g, ct);
		}
	}

	private void _2089(Graphics _٥, SuperTabItemStateColorTable ዅ)
	{
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		Point[] array;
		switch (base._1FD4.TabAlignment)
		{
		case eTabStripAlignment.Top:
			displayRectangle.X += (displayRectangle.Width - 13) / 2;
			displayRectangle.Y = displayRectangle.Bottom - 7 + 2;
			displayRectangle.Width = 13;
			displayRectangle.Height = 7;
			array = new Point[3]
			{
				new Point(displayRectangle.X, displayRectangle.Bottom),
				new Point(displayRectangle.X + 6, displayRectangle.Y),
				new Point(displayRectangle.Right, displayRectangle.Bottom)
			};
			break;
		case eTabStripAlignment.Left:
			displayRectangle.X = displayRectangle.Right - 7 + 2;
			displayRectangle.Y += (displayRectangle.Height - 13) / 2;
			displayRectangle.Width = 7;
			displayRectangle.Height = 13;
			array = new Point[3]
			{
				new Point(displayRectangle.Right, displayRectangle.Bottom),
				new Point(displayRectangle.X, displayRectangle.Y + 6),
				new Point(displayRectangle.Right, displayRectangle.Y)
			};
			break;
		case eTabStripAlignment.Bottom:
			displayRectangle.X += (displayRectangle.Width - 13) / 2;
			displayRectangle.Y -= 3;
			displayRectangle.Width = 13;
			displayRectangle.Height = 7;
			array = new Point[3]
			{
				new Point(displayRectangle.X, displayRectangle.Y),
				new Point(displayRectangle.X + 6, displayRectangle.Bottom),
				new Point(displayRectangle.Right, displayRectangle.Y)
			};
			break;
		default:
			displayRectangle.X -= 3;
			displayRectangle.Y += (displayRectangle.Height - 13) / 2;
			displayRectangle.Width = 7;
			displayRectangle.Height = 13;
			array = new Point[3]
			{
				new Point(displayRectangle.X, displayRectangle.Y),
				new Point(displayRectangle.Right, displayRectangle.Y + 6),
				new Point(displayRectangle.X, displayRectangle.Bottom)
			};
			break;
		}
		Brush val = (Brush)new SolidBrush(ዅ.SelectionMarker);
		try
		{
			_٥.FillPolygon(val, array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void DrawTabItemBackground(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors)
	{
		Rectangle ल = Rectangle.Round(path.GetBounds());
		if (ल.Width <= 0 || ल.Height <= 0 || tabColors.Background.Colors == null)
		{
			return;
		}
		if (tabColors.Background.Colors.Length == 1)
		{
			ῧ(g, path, tabColors, ल);
			if (base.ᑸ.IsSelected && !StyleManager.IsMetro(base.ᑸ.EffectiveStyle))
			{
				_208A(g, path, tabColors, ल);
			}
		}
		else
		{
			Ῡ(g, path, tabColors, ल);
		}
	}

	private void _208A(Graphics _٥, GraphicsPath _1A6D, SuperTabItemStateColorTable Ῠ, Rectangle ल)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Region clip = _٥.Clip;
		_٥.SetClip(_1A6D, (CombineMode)1);
		GraphicsPath val = new GraphicsPath();
		try
		{
			Rectangle rectangle = ल;
			rectangle.Inflate(ल.Width / 3, ल.Height / 3);
			val.AddEllipse(rectangle);
			PathGradientBrush val2 = new PathGradientBrush(val);
			try
			{
				val2.CenterPoint = new PointF(ल.X + ल.Width / 2, ल.Bottom - ल.Height / 4);
				val2.CenterColor = ControlPaint.LightLight(Ῠ.Background.Colors[0]);
				val2.SurroundColors = new Color[1] { Color.Empty };
				_٥.FillPath((Brush)(object)val2, val);
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
		_٥.Clip = clip;
	}

	internal override GraphicsPath _1FDE()
	{
		GraphicsPath val = base._1FDE();
		if (val != null)
		{
			return val;
		}
		return (GraphicsPath)(base.ᑸ.TabAlignment switch
		{
			eTabStripAlignment.Top => _2083(), 
			eTabStripAlignment.Bottom => _2084(), 
			eTabStripAlignment.Left => _2085(), 
			_ => _2086(), 
		});
	}

	private GraphicsPath _2083()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Height += 2;
		}
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.X, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Bottom)
		});
		return val;
	}

	private GraphicsPath _2084()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Y -= 2;
			displayRectangle.Height += 2;
		}
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.Right, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Top)
		});
		return val;
	}

	private GraphicsPath _2085()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.Width += 2;
		}
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.Right, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Top)
		});
		return val;
	}

	private GraphicsPath _2086()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		Rectangle displayRectangle = base.ᑸ.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.ᑸ.IsSelected)
		{
			displayRectangle.X -= 2;
			displayRectangle.Width += 2;
		}
		GraphicsPath val = new GraphicsPath();
		val.AddLines(new Point[4]
		{
			new Point(displayRectangle.X, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Top),
			new Point(displayRectangle.Right, displayRectangle.Bottom),
			new Point(displayRectangle.X, displayRectangle.Bottom)
		});
		return val;
	}
}
