using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

public abstract class DocumentBaseContainer
{
	private Rectangle Ⴤ = Rectangle.Empty;

	private Rectangle Ⴥ = Rectangle.Empty;

	private DocumentBaseContainer ӭ;

	private static long _10C6;

	private long _Ⴧ;

	public long Id => _Ⴧ;

	[Browsable(false)]
	public virtual Rectangle DisplayBounds => Ⴥ;

	[Browsable(false)]
	public virtual Rectangle LayoutBounds => Ⴤ;

	public virtual DocumentBaseContainer Parent => ӭ;

	public abstract bool Visible { get; }

	protected internal abstract Size MinimumSize { get; }

	public DocumentBaseContainer()
	{
		_10C6++;
		_Ⴧ = _10C6;
	}

	public abstract void Layout(Rectangle bounds);

	internal void _10C8(Rectangle ल)
	{
		Ⴥ = ल;
	}

	internal void _10C9(Rectangle ल)
	{
		Ⴤ = ल;
	}

	public void ResetLayoutBounds()
	{
		Ⴤ = Rectangle.Empty;
	}

	public void ResetDisplayBounds()
	{
		Ⴥ = Rectangle.Empty;
	}

	internal void _0CC5(DocumentBaseContainer ԟ)
	{
		ӭ = ԟ;
	}

	public virtual void SetWidth(int width)
	{
		if (ӭ != null)
		{
			ӭ.OnSetWidth(this, width);
		}
		if (width >= MinimumSize.Width || MinimumSize.Width == 0)
		{
			ResetDisplayBounds();
			Ⴤ.Width = width;
		}
	}

	public virtual void SetHeight(int height)
	{
		if (ӭ != null)
		{
			ӭ.OnSetHeight(this, height);
		}
		if (height >= MinimumSize.Height || MinimumSize.Height == 0)
		{
			ResetDisplayBounds();
			Ⴤ.Height = height;
		}
	}

	protected internal virtual bool OnSetWidth(DocumentBaseContainer doc, int width)
	{
		return false;
	}

	protected internal virtual bool OnSetHeight(DocumentBaseContainer doc, int height)
	{
		return false;
	}

	internal virtual void Ⴠ(int Ⴡ, int Ⴢ, bool Ⴣ)
	{
		Ⴥ.Offset(Ⴡ, Ⴢ);
	}
}
