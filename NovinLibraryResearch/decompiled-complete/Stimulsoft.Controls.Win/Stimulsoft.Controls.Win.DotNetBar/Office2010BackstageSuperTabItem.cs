using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class Office2010BackstageSuperTabItem : SuperTabItemBaseDisplay
{
	private const int MarkerHeight = 7;

	private const int MarkerWidth = 13;

	public Office2010BackstageSuperTabItem(SuperTabItem tabItem)
		: base(tabItem)
	{
	}

	protected override void DrawTabBorder(Graphics g, GraphicsPath path, SuperTabItemStateColorTable ct)
	{
		base.DrawTabBorder(g, path, ct);
		if (!ct.SelectionMarker.IsEmpty)
		{
			DrawSelMarker(g, ct);
		}
	}

	private void DrawSelMarker(Graphics g, SuperTabItemStateColorTable ct)
	{
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		Point[] array;
		switch (base.TabStripItem.TabAlignment)
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
		Brush val = (Brush)new SolidBrush(ct.SelectionMarker);
		try
		{
			g.FillPolygon(val, array);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void DrawTabItemBackground(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors)
	{
		Rectangle r = Rectangle.Round(path.GetBounds());
		if (r.Width <= 0 || r.Height <= 0 || tabColors.Background.Colors == null)
		{
			return;
		}
		if (tabColors.Background.Colors.Length == 1)
		{
			DrawTabItemSolidBackground(g, path, tabColors, r);
			if (base.TabItem.IsSelected && base.TabItem.EffectiveStyle != eDotNetBarStyle.Metro)
			{
				DrawTabItemHighLight(g, path, tabColors, r);
			}
		}
		else
		{
			DrawTabItemGradientBackground(g, path, tabColors, r);
		}
	}

	private void DrawTabItemHighLight(Graphics g, GraphicsPath path, SuperTabItemStateColorTable tabColors, Rectangle r)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Region clip = g.Clip;
		g.SetClip(path, (CombineMode)1);
		GraphicsPath val = new GraphicsPath();
		try
		{
			Rectangle rectangle = r;
			rectangle.Inflate(r.Width / 3, r.Height / 3);
			val.AddEllipse(rectangle);
			PathGradientBrush val2 = new PathGradientBrush(val);
			try
			{
				val2.CenterPoint = new PointF(r.X + r.Width / 2, r.Bottom - r.Height / 4);
				val2.CenterColor = ControlPaint.LightLight(tabColors.Background.Colors[0]);
				val2.SurroundColors = new Color[1] { Color.Empty };
				g.FillPath((Brush)(object)val2, val);
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
		g.Clip = clip;
	}

	internal override GraphicsPath TabItemPath()
	{
		GraphicsPath val = base.TabItemPath();
		if (val != null)
		{
			return val;
		}
		return (GraphicsPath)(base.TabItem.TabAlignment switch
		{
			eTabStripAlignment.Top => TopTabPath(), 
			eTabStripAlignment.Bottom => BottomTabPath(), 
			eTabStripAlignment.Left => LeftTabPath(), 
			_ => RightTabPath(), 
		});
	}

	private GraphicsPath TopTabPath()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	private GraphicsPath BottomTabPath()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	private GraphicsPath LeftTabPath()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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

	private GraphicsPath RightTabPath()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		Rectangle displayRectangle = base.TabItem.DisplayRectangle;
		displayRectangle.Width--;
		displayRectangle.Height--;
		if (base.TabItem.IsSelected)
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
