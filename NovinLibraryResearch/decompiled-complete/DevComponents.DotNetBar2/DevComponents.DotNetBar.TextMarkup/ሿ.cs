using System.Drawing;

namespace DevComponents.DotNetBar.TextMarkup;

internal class ሿ : _0CBC
{
	private int ቁ = 1;

	private Font ሢ;

	public int ቀ
	{
		get
		{
			return ቁ;
		}
		set
		{
			ቁ = value;
		}
	}

	public ሿ()
	{
	}

	public ሿ(int level)
	{
		ቁ = level;
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		SetFont(d);
		base.Measure(availableSize, d);
		if (ሢ != null)
		{
			d.CurrentFont = ሢ;
		}
	}

	public override void Render(MarkupDrawContext d)
	{
		SetFont(d);
		base.Render(d);
		if (ሢ != null)
		{
			d.CurrentFont = ሢ;
		}
	}

	protected virtual void SetFont(MarkupDrawContext d)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		Font val = d.CurrentFont;
		try
		{
			float num = d.CurrentFont.SizeInPoints;
			if (ቁ == 1)
			{
				num += 12f;
			}
			else if (ቁ == 2)
			{
				num += 8f;
			}
			else if (ቁ == 3)
			{
				num += 6f;
			}
			else if (ቁ == 4)
			{
				num += 4f;
			}
			else if (ቁ == 5)
			{
				num += 2f;
			}
			else if (ቁ == 6)
			{
				num += 1f;
			}
			d.CurrentFont = new Font(d.CurrentFont.FontFamily, num, (FontStyle)1);
		}
		catch
		{
			val = null;
		}
		if (val != null)
		{
			ሢ = val;
		}
	}
}
