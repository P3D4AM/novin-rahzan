using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.Presentation;

internal class ᙸ : ᙝ
{
	private int ன;

	private ᙔ ᖾ = new ᙔ();

	private ᙙ ᙶ = new ᙙ();

	private eCornerType ᙾ = eCornerType.Square;

	private eCornerType ᙿ = eCornerType.Square;

	private eCornerType _1680 = eCornerType.Square;

	private eCornerType ᚁ = eCornerType.Square;

	public int ଖ
	{
		get
		{
			return ன;
		}
		set
		{
			ன = value;
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

	public eCornerType ᙹ
	{
		get
		{
			return ᙾ;
		}
		set
		{
			ᙾ = value;
			ᙿ = value;
			_1680 = value;
			ᚁ = value;
		}
	}

	public eCornerType ᙺ
	{
		get
		{
			return ᙾ;
		}
		set
		{
			ᙾ = value;
		}
	}

	public eCornerType ᙻ
	{
		get
		{
			return ᙿ;
		}
		set
		{
			ᙿ = value;
		}
	}

	public eCornerType ᙼ
	{
		get
		{
			return _1680;
		}
		set
		{
			_1680 = value;
		}
	}

	public eCornerType ᙽ
	{
		get
		{
			return ᚁ;
		}
		set
		{
			ᚁ = value;
		}
	}

	public ᙸ()
	{
	}

	public ᙸ(ᙔ border)
	{
		ᖾ = border;
	}

	public ᙸ(ᙔ border, int cornerSize, eCornerType cornerType)
	{
		ᖾ = border;
		ன = cornerSize;
		ᙹ = cornerType;
	}

	public ᙸ(ᙔ border, ᙙ fill)
	{
		ᖾ = border;
		ᙶ = fill;
	}

	public ᙸ(ᙔ border, ᙙ fill, int cornerSize, eCornerType cornerType)
	{
		ᖾ = border;
		ᙶ = fill;
		ன = cornerSize;
		ᙹ = cornerType;
	}

	public ᙸ(ᙔ border, ᙙ fill, int cornerSize, eCornerType cornerType, ᙯ padding)
	{
		ᖾ = border;
		ᙶ = fill;
		ன = cornerSize;
		base.ᙟ = padding;
		ᙹ = cornerType;
	}

	public ᙸ(ᙙ fill)
	{
		ᙶ = fill;
	}

	public override void Paint(ᙰ p)
	{
		Rectangle bounds = GetBounds(p.ঋ);
		Graphics ܓ = p.ܓ;
		PaintFill(ܓ, bounds);
		PaintBorder(ܓ, bounds);
		base.Paint(p);
	}

	protected virtual void PaintBorder(Graphics g, Rectangle r)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		if (ᖾ.ܐ == 0 || ᖾ.ᙕ.IsEmpty || r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		ᙔ ᙔ2 = ᖾ;
		r.Width--;
		r.Height--;
		GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, ன, eStyleBackgroundPathPart.Complete, ᙾ, ᙿ, _1680, ᚁ);
		try
		{
			Pen val = new Pen(ᙔ2.ᙕ, (float)ᙔ2.ܐ);
			try
			{
				roundedRectanglePath.Widen(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (ᙔ2.ᙖ.IsEmpty)
			{
				SolidBrush val2 = new SolidBrush(ᙔ2.ᙕ);
				try
				{
					g.FillPath((Brush)(object)val2, roundedRectanglePath);
					return;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			LinearGradientBrush val3 = DisplayHelp.CreateLinearGradientBrush(r, ᙔ2.ᙕ, ᙔ2.ᙖ, ᙔ2.ᙗ);
			try
			{
				g.FillPath((Brush)(object)val3, roundedRectanglePath);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)roundedRectanglePath)?.Dispose();
		}
	}

	protected virtual void PaintFill(Graphics g, Rectangle r)
	{
		if (r.Width <= 0 || r.Height <= 0)
		{
			return;
		}
		int num = ன;
		if (num > 0)
		{
			GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, ன, eStyleBackgroundPathPart.Complete, ᙾ, ᙿ, _1680, ᚁ);
			try
			{
				Brush val = ᙶ.ᙜ(Rectangle.Ceiling(roundedRectanglePath.GetBounds()));
				if (val != null)
				{
					DisplayHelp.FillPath(g, roundedRectanglePath, ᙶ.ᙕ, ᙶ.ᙖ, ᙶ.ᙗ);
					val.Dispose();
				}
				return;
			}
			finally
			{
				((IDisposable)roundedRectanglePath)?.Dispose();
			}
		}
		Brush val2 = ᙶ.ᙜ(r);
		if (val2 != null)
		{
			g.FillRectangle(val2, r);
			val2.Dispose();
		}
	}

	protected override Region ClipChildren(Graphics g, Rectangle childBounds)
	{
		if (ன > 0)
		{
			Region clip = g.Clip;
			GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(childBounds, ன, eStyleBackgroundPathPart.Complete, ᙾ, ᙿ, _1680, ᚁ);
			try
			{
				g.SetClip(roundedRectanglePath);
				return clip;
			}
			finally
			{
				((IDisposable)roundedRectanglePath)?.Dispose();
			}
		}
		return base.ClipChildren(g, childBounds);
	}
}
