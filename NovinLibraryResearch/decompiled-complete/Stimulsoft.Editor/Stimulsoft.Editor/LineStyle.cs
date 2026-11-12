using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[TypeConverter(typeof(LineStyleConverter))]
public class LineStyle : ILineStyle
{
	private string name = string.Empty;

	private Color foreColor = EditConsts.DefaultLineStyleForeColor;

	private Color backColor = EditConsts.DefaultLineStyleBackColor;

	private Color penColor = Color.Empty;

	private int imageIndex = -1;

	private int updateCount;

	private LineStyleOptions options = EditConsts.DefaultLineStyleOptions;

	private SyntaxEdit owner;

	[Description("Gets or sets name of the \"LineStyle\".")]
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

	[XmlIgnore]
	[Description("Gets or sets foreground color of the \"LineStyle\".")]
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
	[Description("Gets or sets background color of the \"LineStyle\".")]
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

	[XmlIgnore]
	[Description("Gets or sets pen color of the \"LineStyle\".")]
	public Color PenColor
	{
		get
		{
			return penColor;
		}
		set
		{
			if (penColor != value)
			{
				penColor = value;
				Update();
			}
		}
	}

	[XmlElement("ForeColor")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ForeColorString
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
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string BackColorString
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

	[XmlElement("PenColor")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string PenColorString
	{
		get
		{
			return XmlHelper.SerializeColor(PenColor);
		}
		set
		{
			PenColor = XmlHelper.DeserializeColor(value);
		}
	}

	[DefaultValue(-1)]
	[Description("Gets or sets the index of the image displayed for the \"LineStyle\".")]
	public int ImageIndex
	{
		get
		{
			return imageIndex;
		}
		set
		{
			if (imageIndex != value)
			{
				imageIndex = value;
				Update();
			}
		}
	}

	[Editor("QWhale.Design.FlagEnumerationEditor, QWhale.Editor", typeof(UITypeEditor))]
	[Description("Gets or sets display options for the \"LineStyle\".")]
	public LineStyleOptions Options
	{
		get
		{
			return options;
		}
		set
		{
			if (options != value)
			{
				options = value;
				Update();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public SyntaxEdit Owner => owner;

	public LineStyle()
	{
	}

	public LineStyle(SyntaxEdit owner)
	{
		this.owner = owner;
	}

	public LineStyle(SyntaxEdit owner, string name, Color foreColor, Color backColor, Color penColor, int imageIndex, LineStyleOptions options)
	{
		this.owner = Owner;
		this.name = name;
		this.foreColor = foreColor;
		this.backColor = backColor;
		this.penColor = penColor;
		this.imageIndex = imageIndex;
		this.options = options;
	}

	protected void BeginUpdate()
	{
		updateCount++;
	}

	protected void EndUpdate()
	{
		updateCount--;
		if (updateCount == 0)
		{
			Update();
		}
	}

	protected void Update()
	{
		if (updateCount == 0 && owner != null)
		{
			((Control)owner).Invalidate();
		}
	}

	public void Assign(ILineStyle source)
	{
		BeginUpdate();
		try
		{
			Name = source.Name;
			ForeColor = source.ForeColor;
			BackColor = source.BackColor;
			PenColor = source.PenColor;
			ImageIndex = source.ImageIndex;
			Options = source.Options;
		}
		finally
		{
			EndUpdate();
		}
	}

	public Color GetBackColor(Color color)
	{
		Color color2 = (((options & LineStyleOptions.InvertColors) != 0) ? foreColor : backColor);
		if (!(color2 != Color.Empty))
		{
			return color;
		}
		return color2;
	}

	public Color GetForeColor(Color color)
	{
		Color color2 = (((options & LineStyleOptions.InvertColors) != 0) ? backColor : foreColor);
		if (!(color2 != Color.Empty))
		{
			return color;
		}
		return color2;
	}

	public virtual void ResetBackColor()
	{
		BackColor = EditConsts.DefaultLineStyleBackColor;
	}

	public virtual void ResetForeColor()
	{
		ForeColor = EditConsts.DefaultLineStyleForeColor;
	}

	public virtual void ResetPenColor()
	{
		PenColor = Color.Empty;
	}

	public virtual void ResetOptions()
	{
		Options = LineStyleOptions.InvertColors;
	}

	public virtual void ResetImageIndex()
	{
		ImageIndex = -1;
	}

	public bool ShouldSerializeForeColor()
	{
		return foreColor != EditConsts.DefaultLineStyleForeColor;
	}

	public bool ShouldSerializeBackColor()
	{
		return backColor != EditConsts.DefaultLineStyleBackColor;
	}

	public bool ShouldSerializePenColor()
	{
		return backColor != Color.Empty;
	}

	public bool ShouldSerializeOptions()
	{
		return options != EditConsts.DefaultLineStyleOptions;
	}
}
