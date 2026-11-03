using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.Editors;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class ComboItem : Component
{
	private string ө = "";

	private int դ = -1;

	private Image _0E66;

	private StringFormat Ṅ;

	private HorizontalAlignment _0E71;

	private string ṅ = "";

	private FontStyle Ṇ;

	private float ṇ = 8f;

	private Color _0D02 = Color.Empty;

	private Color ފ = Color.Empty;

	private object ӧ;

	internal ComboBoxEx ᚐ;

	internal bool Ṉ;

	private object _ᗸ;

	[Localizable(true)]
	[Browsable(true)]
	[DefaultValue("")]
	public string Text
	{
		get
		{
			return ө;
		}
		set
		{
			ө = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.ImageIndexEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	[TypeConverter(typeof(ImageIndexConverter))]
	[DefaultValue(-1)]
	public int ImageIndex
	{
		get
		{
			return դ;
		}
		set
		{
			դ = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Ṅ.Alignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Ṅ.Alignment = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StringAlignment TextLineAlignment
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Ṅ.LineAlignment;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Ṅ.LineAlignment = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StringFormat TextFormat
	{
		get
		{
			return Ṅ;
		}
		set
		{
			Ṅ = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public HorizontalAlignment ImagePosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _0E71;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_0E71 = value;
		}
	}

	[DefaultValue("")]
	public string FontName
	{
		get
		{
			return ṅ;
		}
		set
		{
			ṅ = value;
		}
	}

	public Color ForeColor
	{
		get
		{
			return _0D02;
		}
		set
		{
			_0D02 = value;
		}
	}

	public Color BackColor
	{
		get
		{
			return ފ;
		}
		set
		{
			ފ = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public FontStyle FontStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Ṇ;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			Ṇ = value;
		}
	}

	[DefaultValue(8f)]
	public float FontSize
	{
		get
		{
			return ṇ;
		}
		set
		{
			ṇ = value;
		}
	}

	[DefaultValue(null)]
	[Localizable(true)]
	public Image Image
	{
		get
		{
			return _0E66;
		}
		set
		{
			_0E66 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public object Tag
	{
		get
		{
			return ӧ;
		}
		set
		{
			ӧ = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ImageList ImageList
	{
		get
		{
			if (ᚐ != null)
			{
				return ᚐ.Images;
			}
			return null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public ComboBoxEx Parent => ᚐ;

	[TypeConverter(typeof(StringConverter))]
	[Category("Data")]
	[Localizable(true)]
	[DefaultValue(null)]
	public object Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			_ᗸ = value;
		}
	}

	public ComboItem()
	{
		Ṅ = BarFunctions.CreateStringFormat();
		Ṅ.Alignment = (StringAlignment)0;
	}

	public ComboItem(string text)
		: this()
	{
		ө = text;
	}

	public ComboItem(string text, Color foreColor)
		: this()
	{
		ө = text;
		_0D02 = foreColor;
	}

	public ComboItem(string text, Color foreColor, Color backColor)
		: this()
	{
		ө = text;
		_0D02 = foreColor;
		ފ = backColor;
	}

	public ComboItem(string text, Image image)
		: this()
	{
		ө = text;
		_0E66 = image;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !base.DesignMode)
		{
			BarUtilities.ᆟ(ref _0E66);
		}
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeForeColor()
	{
		return !_0D02.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !ފ.IsEmpty;
	}

	public override string ToString()
	{
		return ө;
	}
}
