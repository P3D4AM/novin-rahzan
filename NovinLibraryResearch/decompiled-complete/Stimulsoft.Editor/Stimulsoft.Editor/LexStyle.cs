using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class LexStyle : ILexStyle
{
	private string name;

	private string desc;

	private Color foreColor;

	private Color backColor;

	private FontStyle fontStyle;

	private bool plainText;

	private ILexScheme scheme;

	private bool foreColorEnabled = true;

	private bool backColorEnabled = true;

	private bool boldEnabled = true;

	private bool italicEnabled = true;

	private bool underlineEnabled = true;

	[Description("Gets or sets name of the \"LexStyle\".")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	[Description("Gets or sets a description of the \"LexStyle\".")]
	public string Desc
	{
		get
		{
			return desc;
		}
		set
		{
			desc = value;
		}
	}

	[XmlIgnore]
	[Description("Gets or sets a foreground color of the \"LexStyle\".")]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				Update();
			}
		}
	}

	[XmlIgnore]
	[Description("Gets or sets a background color of the \"LexStyle\".")]
	public Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			if (backColor != value)
			{
				backColor = value;
				Update();
			}
		}
	}

	[XmlElement("ForeColor")]
	[Description("Gets or sets string representation of the \"ForeColor\" property.")]
	public string XmlForeColor
	{
		get
		{
			return XmlHelper.SerializeColor(ForeColor);
		}
		set
		{
			ForeColor = XmlHelper.DeserializeColor(value);
		}
	}

	[XmlElement("BackColor")]
	[Description("Gets or sets string representation of the \"BackColor\" property.")]
	public string XmlBackColor
	{
		get
		{
			return XmlHelper.SerializeColor(BackColor);
		}
		set
		{
			BackColor = XmlHelper.DeserializeColor(value);
		}
	}

	[Description("Gets or sets a font style of the \"LexStyle\".")]
	public FontStyle FontStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return fontStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (fontStyle != value)
			{
				fontStyle = value;
				Update();
			}
		}
	}

	[Description("Gets or sets index of this \"LexStyle\" within the lexical style collection.")]
	public int Index
	{
		get
		{
			if (scheme == null)
			{
				return -1;
			}
			return scheme.LexStyles.IndexOf(this);
		}
	}

	[Description("Gets or sets value indicating whether Edit control should use text formatting rules, like checking spelling.")]
	public bool PlainText
	{
		get
		{
			return plainText;
		}
		set
		{
			plainText = value;
		}
	}

	[XmlIgnore]
	[Description("Gets or sets \"ILexScheme\" that owns this \"LexStyle\".")]
	public ILexScheme Scheme
	{
		get
		{
			return scheme;
		}
		set
		{
			scheme = value;
		}
	}

	public bool ForeColorEnabled
	{
		get
		{
			return foreColorEnabled;
		}
		set
		{
			foreColorEnabled = value;
		}
	}

	public bool BackColorEnabled
	{
		get
		{
			return backColorEnabled;
		}
		set
		{
			backColorEnabled = value;
		}
	}

	public bool BoldEnabled
	{
		get
		{
			return boldEnabled;
		}
		set
		{
			boldEnabled = value;
		}
	}

	public bool ItalicEnabled
	{
		get
		{
			return italicEnabled;
		}
		set
		{
			italicEnabled = value;
		}
	}

	public bool UnderlineEnabled
	{
		get
		{
			return underlineEnabled;
		}
		set
		{
			underlineEnabled = value;
		}
	}

	public LexStyle()
	{
		foreColor = Consts.DefaultControlForeColor;
		backColor = Color.Empty;
	}

	public LexStyle(ILexScheme scheme)
		: this()
	{
		this.scheme = scheme;
	}

	public void Update()
	{
	}

	public void Assign(ILexStyle source)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		name = source.Name;
		desc = source.Desc;
		foreColor = source.ForeColor;
		backColor = source.BackColor;
		fontStyle = source.FontStyle;
		plainText = source.PlainText;
		foreColorEnabled = source.ForeColorEnabled;
		backColorEnabled = source.BackColorEnabled;
		boldEnabled = source.BoldEnabled;
		italicEnabled = source.ItalicEnabled;
		underlineEnabled = source.UnderlineEnabled;
	}

	public virtual void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	public virtual void ResetForeColor()
	{
		ForeColor = Consts.DefaultControlForeColor;
	}

	public virtual void ResetFontStyle()
	{
		FontStyle = (FontStyle)0;
	}

	public virtual void ResetPlainText()
	{
		PlainText = false;
	}

	public bool ShouldSerializeXmlForeColor()
	{
		return foreColor != Color.Empty;
	}

	public bool ShouldSerializeXmlBackColor()
	{
		return backColor != Color.Empty;
	}

	public bool ShouldSerializePlainText()
	{
		return plainText;
	}

	public bool ShouldSerializeFontStyle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		return (int)fontStyle > 0;
	}
}
