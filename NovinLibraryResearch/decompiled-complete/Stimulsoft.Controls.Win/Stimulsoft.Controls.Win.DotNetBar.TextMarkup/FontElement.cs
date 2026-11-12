using System.Drawing;
using System.Xml;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

internal class FontElement : FontChangeElement
{
	private Color m_ForeColor = Color.Empty;

	private Color m_OldForeColor = Color.Empty;

	private int m_Size;

	private bool m_RelativeSize;

	private string m_Face = "";

	private string m_SystemColorName = "";

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

	public int Size
	{
		get
		{
			return m_Size;
		}
		set
		{
			m_Size = value;
		}
	}

	public string Face
	{
		get
		{
			return m_Face;
		}
		set
		{
			m_Face = value;
		}
	}

	protected override void SetFont(MarkupDrawContext d)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		Font val = d.CurrentFont;
		try
		{
			if (m_Face != "" || (m_Size != 0 && m_RelativeSize) || (m_Size > 4 && !m_RelativeSize))
			{
				if (m_Face != "")
				{
					d.CurrentFont = new Font(m_Face, (m_RelativeSize || m_Size == 0) ? (val.SizeInPoints + (float)m_Size) : ((float)m_Size), val.Style);
				}
				else
				{
					d.CurrentFont = new Font(val.FontFamily, (m_RelativeSize || m_Size == 0) ? (val.SizeInPoints + (float)m_Size) : ((float)m_Size), val.Style);
				}
			}
			else
			{
				val = null;
			}
		}
		catch
		{
			val = null;
		}
		if (val != null)
		{
			m_OldFont = val;
		}
		if (!d.IgnoreFormattingColors)
		{
			if (!m_ForeColor.IsEmpty)
			{
				m_OldForeColor = d.CurrentForeColor;
				d.CurrentForeColor = m_ForeColor;
			}
			else if (m_SystemColorName != "" && GlobalManager.Renderer is Office2007Renderer)
			{
				m_OldForeColor = d.CurrentForeColor;
				d.CurrentForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Form.Active.CaptionTextExtra;
			}
		}
	}

	public override void RenderEnd(MarkupDrawContext d)
	{
		RestoreForeColor(d);
		base.RenderEnd(d);
	}

	public override void MeasureEnd(Size availableSize, MarkupDrawContext d)
	{
		RestoreForeColor(d);
		base.MeasureEnd(availableSize, d);
	}

	protected virtual void RestoreForeColor(MarkupDrawContext d)
	{
		if (!m_OldForeColor.IsEmpty)
		{
			d.CurrentForeColor = m_OldForeColor;
		}
		m_OldForeColor = Color.Empty;
	}

	private Color GetColorFromName(string name)
	{
		string text = name.ToLower();
		m_SystemColorName = "";
		if (text == "syscaptiontextextra")
		{
			m_SystemColorName = text;
			return Color.Empty;
		}
		return Color.FromName(name);
	}

	public override void ReadAttributes(XmlTextReader reader)
	{
		m_RelativeSize = false;
		for (int i = 0; i < reader.AttributeCount; i++)
		{
			reader.MoveToAttribute(i);
			if (reader.Name.ToLower() == "color")
			{
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
			else if (reader.Name.ToLower() == "size")
			{
				string value2 = reader.Value;
				if (value2.StartsWith("+"))
				{
					try
					{
						m_Size = int.Parse(value2.Substring(1));
						m_RelativeSize = true;
					}
					catch
					{
						m_Size = 0;
					}
					continue;
				}
				if (value2.StartsWith("-"))
				{
					m_RelativeSize = true;
				}
				try
				{
					m_Size = int.Parse(value2);
				}
				catch
				{
					m_Size = 0;
				}
			}
			else if (reader.Name.ToLower() == "face")
			{
				m_Face = reader.Value;
			}
		}
	}
}
