using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class BodyElement : ContainerElement
{
	private MarkupElementCollection m_ActiveElements;

	private IActiveMarkupElement m_MouseOverElement;

	internal bool HasExpandElement;

	internal string PlainText = "";

	public MarkupElementCollection ActiveElements => m_ActiveElements;

	internal IActiveMarkupElement MouseOverElement => m_MouseOverElement;

	public event EventHandler HyperLinkClick;

	public BodyElement()
	{
		m_ActiveElements = new MarkupElementCollection(null);
	}

	public void MouseLeave(Control parent)
	{
		if (m_MouseOverElement != null)
		{
			m_MouseOverElement.MouseLeave(parent);
		}
		m_MouseOverElement = null;
	}

	public void MouseMove(Control parent, MouseEventArgs e)
	{
		if (m_MouseOverElement != null && m_MouseOverElement.HitTest(e.X, e.Y))
		{
			return;
		}
		if (m_MouseOverElement != null)
		{
			m_MouseOverElement.MouseLeave(parent);
		}
		m_MouseOverElement = null;
		foreach (IActiveMarkupElement activeElement in m_ActiveElements)
		{
			if (activeElement.HitTest(e.X, e.Y))
			{
				m_MouseOverElement = activeElement;
				m_MouseOverElement.MouseEnter(parent);
			}
		}
	}

	public void MouseDown(Control parent, MouseEventArgs e)
	{
		if (m_MouseOverElement != null)
		{
			m_MouseOverElement.MouseDown(parent, e);
		}
	}

	public void MouseUp(Control parent, MouseEventArgs e)
	{
		if (m_MouseOverElement != null)
		{
			m_MouseOverElement.MouseUp(parent, e);
		}
	}

	public void Click(Control parent)
	{
		if (m_MouseOverElement != null)
		{
			m_MouseOverElement.Click(parent);
			if (m_MouseOverElement is HyperLink && this.HyperLinkClick != null)
			{
				this.HyperLinkClick(m_MouseOverElement, new EventArgs());
			}
		}
	}
}
