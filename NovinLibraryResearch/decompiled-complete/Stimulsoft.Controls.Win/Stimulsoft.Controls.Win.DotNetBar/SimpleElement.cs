using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class SimpleElement : ISimpleElement
{
	private Rectangle m_Bounds = Rectangle.Empty;

	private Rectangle m_ImageBounds = Rectangle.Empty;

	private Rectangle m_TextBounds = Rectangle.Empty;

	private Image m_Image;

	private eSimplePartAlignment m_ImageAlignment;

	private string m_Text = "";

	private int m_FixedWidth;

	public int FixedWidth
	{
		get
		{
			return m_FixedWidth;
		}
		set
		{
			m_FixedWidth = value;
		}
	}

	public bool ImageVisible => m_Image != null;

	public Size ImageLayoutSize
	{
		get
		{
			if (m_Image == null)
			{
				return Size.Empty;
			}
			return m_Image.Size;
		}
	}

	public eSimplePartAlignment ImageAlignment
	{
		get
		{
			return m_ImageAlignment;
		}
		set
		{
			m_ImageAlignment = value;
		}
	}

	public int ImageTextSpacing => 1;

	public bool TextVisible => true;

	public string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			m_Text = value;
		}
	}

	public Rectangle Bounds
	{
		get
		{
			return m_Bounds;
		}
		set
		{
			m_Bounds = value;
		}
	}

	public Rectangle TextBounds
	{
		get
		{
			return m_TextBounds;
		}
		set
		{
			m_TextBounds = value;
		}
	}

	public Rectangle ImageBounds
	{
		get
		{
			return m_ImageBounds;
		}
		set
		{
			m_ImageBounds = value;
		}
	}

	public Image Image
	{
		get
		{
			return m_Image;
		}
		set
		{
			m_Image = value;
		}
	}
}
