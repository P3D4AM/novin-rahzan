using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;

namespace Stimulsoft.Controls.Win.Editors;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ComboItem : Component
{
	private string m_Text = "";

	private int m_ImageIndex = -1;

	private Image m_Image;

	private StringFormat m_TextFormat;

	private HorizontalAlignment m_ImagePosition;

	private string m_FontName = "";

	private FontStyle m_FontStyle;

	private float m_FontSize = 8f;

	private Color m_ForeColor = Color.Empty;

	private Color m_BackColor = Color.Empty;

	private object m_Tag;

	internal ComboBoxEx m_ComboBox;

	public bool IsFontItem;

	public bool IsResourceFontItem;

	private object _Value;

	[DefaultValue("")]
	[Browsable(true)]
	[Localizable(true)]
	public string Text
	{
		get
		{
			return m_Text;
		}
		set
		{
			m_Text = value;
		}
	}

	[DefaultValue(-1)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.ImageIndexEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(ImageIndexConverter))]
	[Localizable(true)]
	public int ImageIndex
	{
		get
		{
			return m_ImageIndex;
		}
		set
		{
			m_ImageIndex = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_TextFormat.Alignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_TextFormat.Alignment = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_TextFormat.LineAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			m_TextFormat.LineAlignment = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StringFormat TextFormat
	{
		get
		{
			return m_TextFormat;
		}
		set
		{
			m_TextFormat = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public HorizontalAlignment ImagePosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_ImagePosition;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_ImagePosition = value;
		}
	}

	[DefaultValue("")]
	public string FontName
	{
		get
		{
			return m_FontName;
		}
		set
		{
			m_FontName = value;
		}
	}

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

	public Color BackColor
	{
		get
		{
			return m_BackColor;
		}
		set
		{
			m_BackColor = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public FontStyle FontStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return m_FontStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_FontStyle = value;
		}
	}

	[DefaultValue(8f)]
	public float FontSize
	{
		get
		{
			return m_FontSize;
		}
		set
		{
			m_FontSize = value;
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	public Image Image
	{
		get
		{
			return m_Image;
		}
		set
		{
			m_Image = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object Tag
	{
		get
		{
			return m_Tag;
		}
		set
		{
			m_Tag = value;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ImageList ImageList
	{
		get
		{
			if (m_ComboBox != null)
			{
				return m_ComboBox.Images;
			}
			return null;
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ComboBoxEx Parent => m_ComboBox;

	[DefaultValue(null)]
	[Category("Data")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
		}
	}

	public ComboItem()
	{
		m_TextFormat = BarFunctions.CreateStringFormat();
		m_TextFormat.Alignment = (StringAlignment)0;
	}

	public ComboItem(string text)
		: this()
	{
		m_Text = text;
	}

	public ComboItem(string text, Color foreColor)
		: this()
	{
		m_Text = text;
		m_ForeColor = foreColor;
	}

	public ComboItem(string text, Color foreColor, Color backColor)
		: this()
	{
		m_Text = text;
		m_ForeColor = foreColor;
		m_BackColor = backColor;
	}

	public ComboItem(string text, Image image)
		: this()
	{
		m_Text = text;
		m_Image = image;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.DisposeImage(ref m_Image);
		}
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		return !m_ForeColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !m_BackColor.IsEmpty;
	}

	public override string ToString()
	{
		return m_Text;
	}
}
