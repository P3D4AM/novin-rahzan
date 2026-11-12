using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[TypeConverter(typeof(ExpandableObjectConverter))]
public class TabColors
{
	private Color m_BackColor = Color.Empty;

	private Color m_BackColor2 = Color.Empty;

	private int m_BackColorGradientAngle = 90;

	private Color m_LightBorderColor = Color.Empty;

	private Color m_DarkBorderColor = Color.Empty;

	private Color m_BorderColor = Color.Empty;

	private Color m_TextColor = Color.Empty;

	private BackgroundColorBlendCollection m_BackgroundColorBlend = new BackgroundColorBlendCollection();

	[Browsable(true)]
	[Description("Indicates the inactive tab background color.")]
	[Category("Style")]
	public Color BackColor
	{
		get
		{
			return m_BackColor;
		}
		set
		{
			m_BackColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab target gradient background color.")]
	[Category("Style")]
	public Color BackColor2
	{
		get
		{
			return m_BackColor2;
		}
		set
		{
			m_BackColor2 = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the gradient angle.")]
	[Category("Style")]
	[DefaultValue(90)]
	public int BackColorGradientAngle
	{
		get
		{
			return m_BackColorGradientAngle;
		}
		set
		{
			m_BackColorGradientAngle = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Collection that defines the multicolor gradient background.")]
	public BackgroundColorBlendCollection BackgroundColorBlend => m_BackgroundColorBlend;

	[Browsable(true)]
	[Description("Indicates the inactive tab light border color.")]
	[Category("Style")]
	public Color LightBorderColor
	{
		get
		{
			return m_LightBorderColor;
		}
		set
		{
			m_LightBorderColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab dark border color.")]
	[Category("Style")]
	public Color DarkBorderColor
	{
		get
		{
			return m_DarkBorderColor;
		}
		set
		{
			m_DarkBorderColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab border color.")]
	[Category("Style")]
	public Color BorderColor
	{
		get
		{
			return m_BorderColor;
		}
		set
		{
			m_BorderColor = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Description("Indicates the inactive tab text color.")]
	[Category("Style")]
	public Color TextColor
	{
		get
		{
			return m_TextColor;
		}
		set
		{
			m_TextColor = value;
			Refresh();
		}
	}

	public event EventHandler ColorChanged;

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !m_BackColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor2()
	{
		return !m_BackColor2.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor2()
	{
		BackColor2 = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeLightBorderColor()
	{
		return !m_LightBorderColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetLightBorderColor()
	{
		LightBorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDarkBorderColor()
	{
		return !m_DarkBorderColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDarkBorderColor()
	{
		DarkBorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !m_BorderColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !m_TextColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	private void Refresh()
	{
		if (this.ColorChanged != null)
		{
			this.ColorChanged(this, new EventArgs());
		}
	}
}
