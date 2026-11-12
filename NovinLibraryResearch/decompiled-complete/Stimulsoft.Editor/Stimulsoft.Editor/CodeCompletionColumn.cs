using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class CodeCompletionColumn : ICodeCompletionColumn
{
	private string name;

	private FontStyle fontStyle;

	private Color foreColor = Consts.DefaultControlForeColor;

	private bool visible = true;

	[Description("Gets or sets the name of the \"CodeCompletionColumn\".")]
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

	[Description("Gets or sets style information for \"CodeCompletionColumn\" font.")]
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			fontStyle = value;
		}
	}

	[Description("Gets or sets Color structure that represents foreground color of the \"CodeCompletionColumn\".")]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			foreColor = value;
		}
	}

	[Description("Gets or sets a value indicating whether a column is visible.")]
	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
		}
	}

	public virtual void ResetFontStyle()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		fontStyle = (FontStyle)0;
	}

	public virtual void ResetForeColor()
	{
		ForeColor = Consts.DefaultControlForeColor;
	}

	public virtual void ResetVisible()
	{
		Visible = true;
	}
}
