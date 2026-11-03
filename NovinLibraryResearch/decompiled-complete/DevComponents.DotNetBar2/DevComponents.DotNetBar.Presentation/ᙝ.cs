using System.Drawing;

namespace DevComponents.DotNetBar.Presentation;

internal class ᙝ
{
	private ᙞ ᙢ = new ᙞ();

	private ᙩ _߄ = new ᙩ();

	private _166D ᙣ;

	private ᙯ ሳ;

	private bool ᙤ;

	public ᙞ ᙞ => ᙢ;

	public ᙩ ᇐ => _߄;

	public ᙯ ᙟ
	{
		get
		{
			return ሳ;
		}
		set
		{
			ሳ = value;
		}
	}

	public _166D ᙠ
	{
		get
		{
			if (ᙣ == null)
			{
				ᙣ = new _166D();
			}
			return ᙣ;
		}
	}

	public bool ᙡ
	{
		get
		{
			return ᙤ;
		}
		set
		{
			ᙤ = value;
		}
	}

	public virtual void Paint(ᙰ p)
	{
		if (ᙣ == null || ᙣ.Count <= 0)
		{
			return;
		}
		Rectangle ঋ = p.ঋ;
		Rectangle rectangle = new Rectangle(GetLocation(p.ঋ), GetSize(p.ঋ));
		if (ሳ != null)
		{
			rectangle.Width -= ሳ._0E63;
			rectangle.Height -= ሳ._0E62;
			rectangle.X += ሳ._0749;
			rectangle.Y += ሳ._074A;
		}
		p.ঋ = rectangle;
		Region val = ClipChildren(p.ܓ, rectangle);
		if (ᙤ && p.ܓ.Clip != null)
		{
			p.ᙱ = p.ܓ.Clip;
		}
		foreach (ᙝ item in ᙣ)
		{
			item.Paint(p);
		}
		if (p.ᙱ != null)
		{
			p.ᙱ.Dispose();
			p.ᙱ = null;
		}
		if (val != null)
		{
			p.ܓ.Clip = val;
			val.Dispose();
		}
		else
		{
			p.ܓ.ResetClip();
		}
		p.ঋ = ঋ;
		if (val != null)
		{
			val.Dispose();
		}
	}

	protected virtual Region ClipChildren(Graphics g, Rectangle childBounds)
	{
		Region clip = g.Clip;
		g.SetClip(childBounds);
		return clip;
	}

	protected virtual Point GetLocation(Rectangle bounds)
	{
		return GetLocation(bounds, ᙢ);
	}

	protected virtual Point GetLocation(Rectangle bounds, ᙞ refLocation)
	{
		Point location = bounds.Location;
		if (refLocation.ᙥ == ᙧ.ሪ)
		{
			location.X = bounds.Right;
		}
		else if (refLocation.ᙥ == ᙧ._074A)
		{
			location.X = bounds.Y;
		}
		else if (refLocation.ᙥ == ᙧ.ራ)
		{
			location.X = bounds.Bottom;
		}
		if (refLocation.ᙦ == ᙧ.ራ)
		{
			location.Y = bounds.Bottom;
		}
		else if (refLocation.ᙦ == ᙧ.ሪ)
		{
			location.Y = bounds.Right;
		}
		else if (refLocation.ᙦ == ᙧ._0749)
		{
			location.Y = bounds.X;
		}
		location.Offset(refLocation.ᒈ, refLocation.ᒉ);
		return location;
	}

	protected virtual Size GetSize(Rectangle bounds)
	{
		Size size = bounds.Size;
		if (_߄.ᙪ == ᙬ.ܐ)
		{
			size.Width = bounds.Width + _߄.ܐ;
		}
		else if (_߄.ᙪ == ᙬ._0711)
		{
			size.Width = bounds.Height + _߄._0711;
		}
		else if (_߄.ܐ != 0)
		{
			size.Width = _߄.ܐ;
		}
		if (_߄.ᙫ == ᙬ._0711)
		{
			size.Height = bounds.Height + _߄._0711;
		}
		else if (_߄.ᙫ == ᙬ.ܐ)
		{
			size.Height = bounds.Width + _߄._0711;
		}
		else if (_߄._0711 != 0)
		{
			size.Height = _߄._0711;
		}
		return size;
	}

	protected virtual Rectangle GetBounds(Rectangle parentBounds)
	{
		return new Rectangle(GetLocation(parentBounds), GetSize(parentBounds));
	}
}
