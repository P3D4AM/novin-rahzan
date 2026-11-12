using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class Heading : ContainerElement
{
	private int m_Level = 1;

	private Font m_OldFont;

	public int Level
	{
		get
		{
			return m_Level;
		}
		set
		{
			m_Level = value;
		}
	}

	public Heading()
	{
	}

	public Heading(int level)
	{
		m_Level = level;
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		SetFont(d);
		base.Measure(availableSize, d);
		if (m_OldFont != null)
		{
			d.CurrentFont = m_OldFont;
		}
	}

	public override void Render(MarkupDrawContext d)
	{
		SetFont(d);
		base.Render(d);
		if (m_OldFont != null)
		{
			d.CurrentFont = m_OldFont;
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
			if (m_Level == 1)
			{
				num += 12f;
			}
			else if (m_Level == 2)
			{
				num += 8f;
			}
			else if (m_Level == 3)
			{
				num += 6f;
			}
			else if (m_Level == 4)
			{
				num += 4f;
			}
			else if (m_Level == 5)
			{
				num += 2f;
			}
			else if (m_Level == 6)
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
			m_OldFont = val;
		}
	}
}
