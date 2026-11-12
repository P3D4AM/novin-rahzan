using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class ColorTheme : IColorTheme, ICloneable
{
	private string name = string.Empty;

	private bool readOnly;

	private Font font = new Font(FontFamily.GenericMonospace.Name, 10f, (FontStyle)0);

	public string xmlFontName = FontFamily.GenericMonospace.Name;

	public float xmlFontSize = 10f;

	public FontStyle xmlFontStyle;

	private LexStyle[] lexStyles = new LexStyle[0];

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

	public bool ReadOnly
	{
		get
		{
			return readOnly;
		}
		set
		{
			readOnly = value;
		}
	}

	[XmlIgnore]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			font = value;
			xmlFontName = font.Name;
			xmlFontSize = font.Size;
			xmlFontStyle = font.Style;
		}
	}

	public string FontName
	{
		get
		{
			return xmlFontName;
		}
		set
		{
			xmlFontName = value;
		}
	}

	public float FontSize
	{
		get
		{
			return xmlFontSize;
		}
		set
		{
			xmlFontSize = value;
		}
	}

	public FontStyle FontStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return xmlFontStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			xmlFontStyle = value;
		}
	}

	public LexStyle[] LexStyles
	{
		get
		{
			return lexStyles;
		}
		set
		{
			lexStyles = value;
		}
	}

	[XmlIgnore]
	public ILexStyle this[string lexStyleName]
	{
		get
		{
			GetLexStyle(lexStyleName, out var lexStyleObject);
			return lexStyleObject;
		}
		set
		{
			GetLexStyle(lexStyleName, out var lexStyleObject);
			lexStyleObject = value;
		}
	}

	public ColorTheme()
	{
	}//IL_001c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0026: Expected O, but got Unknown


	public ColorTheme(string name, bool readOnly, string fontName, float fontSize, FontStyle fontStyle, LexStyle[] lexStyles)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		this.name = name;
		this.readOnly = readOnly;
		font = new Font(fontName, fontSize, (FontStyle)0);
		xmlFontName = fontName;
		xmlFontSize = fontSize;
		xmlFontStyle = fontStyle;
		this.lexStyles = lexStyles;
	}

	private bool GetLexStyle(string lexStyleName, out ILexStyle lexStyleObject)
	{
		LexStyle[] array = lexStyles;
		foreach (LexStyle lexStyle in array)
		{
			if (lexStyle.Name == lexStyleName)
			{
				lexStyleObject = lexStyle;
				return true;
			}
		}
		lexStyleObject = null;
		return false;
	}

	public bool Contains(string lexStyleName)
	{
		ILexStyle lexStyleObject;
		return GetLexStyle(lexStyleName, out lexStyleObject);
	}

	public object Clone()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		LexStyle[] array = new LexStyle[lexStyles.Length];
		for (int i = 0; i < lexStyles.Length; i++)
		{
			LexStyle lexStyle = new LexStyle();
			lexStyle.Assign(lexStyles[i]);
			array[i] = lexStyle;
		}
		ColorTheme colorTheme = new ColorTheme();
		colorTheme.name = name;
		colorTheme.readOnly = readOnly;
		colorTheme.lexStyles = array;
		colorTheme.Font = (Font)Font.Clone();
		return colorTheme;
	}
}
