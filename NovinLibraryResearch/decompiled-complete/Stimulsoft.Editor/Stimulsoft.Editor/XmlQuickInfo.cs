using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[XmlRoot("QuickInfoCollection")]
public class XmlQuickInfo
{
	private QuickInfo owner;

	private string text;

	public string Text
	{
		get
		{
			if (owner == null)
			{
				return text;
			}
			return owner.Text;
		}
		set
		{
			text = value;
			if (owner != null)
			{
				owner.Text = value;
			}
		}
	}

	public XmlQuickInfo()
	{
	}

	public XmlQuickInfo(QuickInfo owner)
		: this()
	{
		this.owner = owner;
	}

	public virtual void FixupReferences(QuickInfo owner)
	{
		this.owner = owner;
		Text = text;
	}
}
