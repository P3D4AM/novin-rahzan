using System.Drawing;

namespace Stimulsoft.Editor;

internal struct LexStyleItem
{
	public string Name;

	public string InternalName;

	public Color ForeColor;

	public Color BackColor;

	public FontStyle FontStyle;

	public bool PlainText;

	public LexStyleItem(string name, string internalName, Color foreColor, Color backColor, FontStyle fontStyle, bool plainText)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Name = name;
		InternalName = internalName;
		ForeColor = foreColor;
		BackColor = backColor;
		FontStyle = fontStyle;
		PlainText = plainText;
	}

	public LexStyleItem(string name, string internalName, Color foreColor)
		: this(name, internalName, foreColor, Color.Empty, (FontStyle)0, plainText: false)
	{
	}

	public LexStyleItem(string name, string internalName)
		: this(name, internalName, Consts.DefaultControlForeColor, Color.Empty, (FontStyle)0, plainText: false)
	{
	}

	public LexStyleItem(string name, string internalName, Color foreColor, Color backColor)
		: this(name, internalName, foreColor, backColor, (FontStyle)0, plainText: false)
	{
	}
}
