using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Presentation;

internal class ᙲ : ᙝ
{
	private ᙔ ᖾ;

	private ᙙ ᙶ;

	private GraphicsPath ᙷ;

	public GraphicsPath ᙳ
	{
		get
		{
			return ᙷ;
		}
		set
		{
			ᙷ = value;
		}
	}

	public ᙔ ᙴ
	{
		get
		{
			return ᖾ;
		}
		set
		{
			ᖾ = value;
		}
	}

	public ᙙ ᙵ
	{
		get
		{
			return ᙶ;
		}
		set
		{
			ᙶ = value;
		}
	}

	public ᙲ()
	{
	}

	public ᙲ(ᙔ border)
	{
		ᖾ = border;
	}

	public ᙲ(ᙙ fill)
	{
		ᙶ = fill;
	}

	public ᙲ(ᙔ border, ᙙ fill)
	{
		ᖾ = border;
		ᙶ = fill;
	}

	public override void Paint(ᙰ p)
	{
		if (ᙷ != null)
		{
			Rectangle bounds = GetBounds(p.ঋ);
			Graphics ܓ = p.ܓ;
			if (ᙶ != null)
			{
				PaintFill(ܓ, bounds);
			}
			if (ᖾ != null)
			{
				PaintBorder(ܓ, bounds);
			}
			base.Paint(p);
		}
	}

	protected virtual void PaintBorder(Graphics g, Rectangle r)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (ᖾ.ᙕ.IsEmpty)
		{
			return;
		}
		ᙔ ᙔ2 = ᖾ;
		object obj = ᙷ.Clone();
		GraphicsPath val = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
		try
		{
			val.Transform(new Matrix(1f, 0f, 0f, 1f, (float)r.X, (float)r.Y));
			DisplayHelp.DrawGradientPathBorder(g, val, ᙔ2.ᙕ, ᙔ2.ᙖ, ᙔ2.ᙗ, ᙔ2.ܐ);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected virtual void PaintFill(Graphics g, Rectangle r)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		if (r.Width <= 0 || r.Height <= 0 || ᙶ == null)
		{
			return;
		}
		object obj = ᙷ.Clone();
		GraphicsPath val = (GraphicsPath)((obj is GraphicsPath) ? obj : null);
		try
		{
			Rectangle rectangle = Rectangle.Ceiling(val.GetBounds());
			int num = r.X + (int)Math.Ceiling((double)(Math.Max(r.Width, rectangle.Width) - rectangle.Width) / 2.0);
			int num2 = r.Y + (r.Height - rectangle.Height) / 2;
			Matrix val2 = new Matrix(1f, 0f, 0f, 1f, (float)num, (float)num2);
			try
			{
				val.Transform(val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			rectangle = Rectangle.Ceiling(val.GetBounds());
			Brush val3 = ᙶ.ᙜ(rectangle);
			if (val3 != null)
			{
				g.FillPath(val3, val);
				val3.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
