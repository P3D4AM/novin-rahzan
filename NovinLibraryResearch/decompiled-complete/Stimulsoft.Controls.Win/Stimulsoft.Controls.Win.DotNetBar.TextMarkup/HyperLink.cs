using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class HyperLink : MarkupElement, IActiveMarkupElement
{
	private Color m_ForeColor = Color.Empty;

	private Color m_OldForeColor = Color.Empty;

	private string m_HRef = "";

	private string m_Name = "";

	private Cursor m_OldCursor;

	private bool m_IsMouseOver;

	private bool m_Visited;

	public Color ForeColor
	{
		get
		{
			return m_ForeColor;
		}
		set
		{
			m_ForeColor = value;
		}
	}

	public string HRef
	{
		get
		{
			return m_HRef;
		}
		set
		{
			m_HRef = value;
		}
	}

	public string Name
	{
		get
		{
			return m_Name;
		}
		set
		{
			m_Name = value;
		}
	}

	public override void Measure(Size availableSize, MarkupDrawContext d)
	{
		base.Bounds = Rectangle.Empty;
		SetForeColor(d);
	}

	public override void Render(MarkupDrawContext d)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		d.HyperLink = true;
		d.HyperlinkStyle = GetHyperlinkStyle();
		if (!d.HyperlinkStyle.BackColor.IsEmpty)
		{
			GraphicsPath val = new GraphicsPath();
			try
			{
				MarkupElementCollection elements = Parent.Elements;
				int num = elements.IndexOf(this) + 1;
				for (int i = num; i < elements.Count; i++)
				{
					MarkupElement markupElement = elements[i];
					if (markupElement.Visible)
					{
						if (markupElement is EndMarkupElement && ((EndMarkupElement)markupElement).StartElement == this)
						{
							break;
						}
						val.AddRectangle(markupElement.RenderBounds);
					}
				}
				SolidBrush val2 = new SolidBrush(d.HyperlinkStyle.BackColor);
				try
				{
					d.Graphics.FillPath((Brush)(object)val2, val);
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
		}
		SetForeColor(d);
	}

	private HyperlinkStyle GetHyperlinkStyle()
	{
		if (m_IsMouseOver && MarkupSettings.MouseOverHyperlink.IsChanged)
		{
			return MarkupSettings.MouseOverHyperlink;
		}
		if (m_Visited && MarkupSettings.VisitedHyperlink.IsChanged)
		{
			return MarkupSettings.VisitedHyperlink;
		}
		return MarkupSettings.NormalHyperlink;
	}

	protected virtual void SetForeColor(MarkupDrawContext d)
	{
		Color currentForeColor = Color.Empty;
		HyperlinkStyle hyperlinkStyle = GetHyperlinkStyle();
		if (hyperlinkStyle != null && !hyperlinkStyle.TextColor.IsEmpty)
		{
			currentForeColor = hyperlinkStyle.TextColor;
		}
		if (!m_ForeColor.IsEmpty)
		{
			currentForeColor = m_ForeColor;
		}
		if (!currentForeColor.IsEmpty)
		{
			m_OldForeColor = d.CurrentForeColor;
			d.CurrentForeColor = currentForeColor;
		}
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		RestoreForeColor(d);
		d.HyperLink = false;
		d.HyperlinkStyle = null;
		base.RenderEnd(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		RestoreForeColor(d);
		base.MeasureEnd(availableSize, d);
	}

	protected override void ArrangeCore(Rectangle finalRect, MarkupDrawContext d)
	{
	}

	protected virtual void RestoreForeColor(MarkupDrawContext d)
	{
		if (!m_OldForeColor.IsEmpty)
		{
			d.CurrentForeColor = m_OldForeColor;
		}
		m_OldForeColor = Color.Empty;
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "href")
			{
				m_HRef = reader.Value;
			}
			else if (reader.Name.ToLower() == "name")
			{
				m_Name = reader.Value;
			}
			else
			{
				if (!(reader.Name.ToLower() == "color"))
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
							m_ForeColor = ColorScheme.GetColor(value.Substring(1));
						}
					}
					else
					{
						m_ForeColor = GetColorFromName(value);
					}
				}
				catch
				{
					m_ForeColor = Color.Empty;
				}
			}
		}
	}

	private Color GetColorFromName(string name)
	{
		return Color.FromName(name);
	}

	public bool HitTest(int x, int y)
	{
		if (Parent == null)
		{
			return false;
		}
		MarkupElementCollection elements = Parent.Elements;
		int num = elements.IndexOf(this) + 1;
		for (int i = num; i < elements.Count; i++)
		{
			MarkupElement markupElement = elements[i];
			if (markupElement is EndMarkupElement && ((EndMarkupElement)markupElement).StartElement == this)
			{
				break;
			}
			if (elements[i].RenderBounds.Contains(x, y))
			{
				return true;
			}
		}
		return false;
	}

	public void MouseEnter(Control parent)
	{
		m_OldCursor = parent.Cursor;
		parent.Cursor = Cursors.Hand;
		m_IsMouseOver = true;
		if (MarkupSettings.MouseOverHyperlink.IsChanged)
		{
			InvalidateElements(parent);
		}
	}

	public void MouseLeave(Control parent)
	{
		if (m_OldCursor != (Cursor)null && parent != null)
		{
			parent.Cursor = m_OldCursor;
		}
		m_OldCursor = null;
		m_IsMouseOver = false;
		if (MarkupSettings.MouseOverHyperlink.IsChanged)
		{
			InvalidateElements(parent);
		}
	}

	public void MouseDown(Control parent, MouseEventArgs e)
	{
	}

	public void MouseUp(Control parent, MouseEventArgs e)
	{
	}

	public void Click(Control parent)
	{
		m_Visited = true;
		if (MarkupSettings.VisitedHyperlink.IsChanged)
		{
			InvalidateElements(parent);
		}
	}

	private void InvalidateElements(Control parent)
	{
		if (Parent == null)
		{
			return;
		}
		MarkupElementCollection elements = Parent.Elements;
		int num = elements.IndexOf(this) + 1;
		for (int i = num; i < elements.Count; i++)
		{
			MarkupElement markupElement = elements[i];
			if (markupElement.Visible)
			{
				if (markupElement is EndMarkupElement && ((EndMarkupElement)markupElement).StartElement == this)
				{
					break;
				}
				parent.Invalidate(markupElement.RenderBounds);
			}
		}
	}
}
