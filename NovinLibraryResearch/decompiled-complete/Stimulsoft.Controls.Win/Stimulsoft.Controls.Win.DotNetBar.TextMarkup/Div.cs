using System.Drawing;
using System.Xml;
using Stimulsoft.Controls.Win.UI.ContentManager;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class Div : ContainerElement
{
	private eParagraphAlignment m_Align;

	private eParagraphVerticalAlignment m_VAlign;

	private int m_Width;

	private int m_Height;

	private Padding m_Padding = new Padding(0, 0, 0, 0);

	private Color m_BackColor = Color.Empty;

	public eParagraphAlignment Align
	{
		get
		{
			return m_Align;
		}
		set
		{
			m_Align = value;
		}
	}

	public override void Render(MarkupDrawContext d)
	{
		Rectangle bounds = base.Bounds;
		bounds.Offset(d.Offset);
		if (!m_BackColor.IsEmpty)
		{
			DisplayHelp.FillRectangle(d.Graphics, bounds, m_BackColor);
		}
		base.Render(d);
	}

	protected override SerialContentLayoutManager GetLayoutManager(bool multiLine)
	{
		SerialContentLayoutManager layoutManager = base.GetLayoutManager(multiLine);
		if (m_Align == eParagraphAlignment.Left)
		{
			layoutManager.ContentAlignment = eContentAlignment.Left;
		}
		else if (m_Align == eParagraphAlignment.Right)
		{
			layoutManager.ContentAlignment = eContentAlignment.Right;
		}
		else if (m_Align == eParagraphAlignment.Center)
		{
			layoutManager.ContentAlignment = eContentAlignment.Center;
		}
		if (m_VAlign != 0)
		{
			layoutManager.EvenHeight = false;
			layoutManager.BlockLineAlignment = ((m_VAlign == eParagraphVerticalAlignment.Bottom) ? eContentVerticalAlignment.Bottom : eContentVerticalAlignment.Middle);
		}
		return layoutManager;
	}

	protected override Point GetContainerOffset()
	{
		if (m_Padding.Left == 0 && m_Padding.Top == 0)
		{
			return base.GetContainerOffset();
		}
		Point containerOffset = base.GetContainerOffset();
		containerOffset.X += m_Padding.Left;
		containerOffset.Y += m_Padding.Top;
		return containerOffset;
	}

	protected override void ArrangeInternal(Rectangle bounds, MarkupDrawContext d)
	{
		Rectangle bounds2 = bounds;
		if (m_Width > 0)
		{
			bounds2.Width = m_Width;
		}
		if (m_Height > 0)
		{
			bounds2.Height = m_Height;
		}
		if (m_Padding.IsEmpty)
		{
			base.ArrangeInternal(bounds2, d);
			if (m_Width > 0)
			{
				base.Bounds = new Rectangle(base.Bounds.X, base.Bounds.Y, m_Width, (m_Height > 0) ? m_Height : (base.Bounds.Height + m_Padding.Bottom));
			}
			else if (m_Height > 0)
			{
				base.Bounds = new Rectangle(base.Bounds.X, base.Bounds.Y, base.Bounds.Width, m_Height);
			}
			return;
		}
		bounds2.X += m_Padding.Left;
		bounds2.Y += m_Padding.Top;
		bounds2.Width -= m_Padding.Horizontal;
		bounds2.Height -= m_Padding.Vertical;
		base.ArrangeInternal(bounds2, d);
		bounds2 = new Rectangle(bounds.X, bounds.Y, base.Bounds.Width + m_Padding.Horizontal, base.Bounds.Height + m_Padding.Vertical);
		if (m_Width > 0)
		{
			bounds2.Width = m_Width;
		}
		if (m_Height > 0)
		{
			bounds2.Height = m_Height;
		}
		base.Bounds = bounds2;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "align")
			{
				switch (reader.Value.ToLower())
				{
				case "left":
					m_Align = eParagraphAlignment.Left;
					break;
				case "right":
					m_Align = eParagraphAlignment.Right;
					break;
				case "center":
					m_Align = eParagraphAlignment.Center;
					break;
				}
			}
			else if (reader.Name.ToLower() == "valign")
			{
				switch (reader.Value.ToLower())
				{
				case "top":
					m_VAlign = eParagraphVerticalAlignment.Top;
					break;
				case "middle":
					m_VAlign = eParagraphVerticalAlignment.Middle;
					break;
				case "bottom":
					m_VAlign = eParagraphVerticalAlignment.Bottom;
					break;
				}
			}
			else if (reader.Name.ToLower() == "width")
			{
				try
				{
					m_Width = int.Parse(reader.Value);
				}
				catch
				{
					m_Width = 0;
				}
			}
			else if (reader.Name.ToLower() == "height")
			{
				try
				{
					m_Height = int.Parse(reader.Value);
				}
				catch
				{
					m_Height = 0;
				}
			}
			else if (reader.Name.ToLower() == "padding")
			{
				try
				{
					string[] array = reader.Value.Split(new char[1] { ',' });
					if (array.Length != 0)
					{
						m_Padding.Left = int.Parse(array[0]);
					}
					if (array.Length > 1)
					{
						m_Padding.Right = int.Parse(array[1]);
					}
					if (array.Length > 2)
					{
						m_Padding.Top = int.Parse(array[2]);
					}
					if (array.Length > 3)
					{
						m_Padding.Bottom = int.Parse(array[3]);
					}
				}
				catch
				{
					m_Padding = new Padding(0, 0, 0, 0);
				}
			}
			else
			{
				if (!(reader.Name.ToLower() == "bgcolor"))
				{
					continue;
				}
				try
				{
					string value = reader.Value;
					if (value.StartsWith("#"))
					{
						if (value.Length == 7)
						{
							m_BackColor = ColorScheme.GetColor(value.Substring(1));
						}
					}
					else
					{
						m_BackColor = Color.FromName(value);
					}
				}
				catch
				{
					m_BackColor = Color.Empty;
				}
			}
		}
	}
}
