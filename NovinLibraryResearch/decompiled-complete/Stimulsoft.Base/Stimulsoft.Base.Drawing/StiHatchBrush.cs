using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[RefreshProperties(RefreshProperties.All)]
[JsonObject]
public class StiHatchBrush : StiBrush
{
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets the color of spaces between the hatch lines drawn by this StiHatchBrush object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(200)]
	public Color BackColor { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets the color of hatch lines drawn by this StiHatchBrush object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(100)]
	public Color ForeColor { get; set; }

	[Editor("Stimulsoft.Base.Drawing.Design.StiHatchStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[TypeConverter(typeof(StiHatchStyleConverter))]
	[Description("Gets the hatch style of this StiHatchBrush object.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(300)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public HatchStyle Style { get; set; }

	public override StiBrushIdent Ident => StiBrushIdent.Hatch;

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Black;
	}

	protected bool Equals(StiHatchBrush other)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (BackColor.Equals((object?)other.BackColor) && ForeColor.Equals((object?)other.ForeColor))
		{
			return Style == other.Style;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj.GetType() == GetType())
				{
					return Equals((StiHatchBrush)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected I4, but got Unknown
		int hashCode = "StiHatchBrush".GetHashCode();
		hashCode = (hashCode * 397) ^ BackColor.GetHashCode();
		hashCode = (hashCode * 397) ^ ForeColor.GetHashCode();
		return (hashCode * 397) ^ Style;
	}

	public void LoadValuesFromJson(JObject jObject)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BackColor":
				BackColor = item.DeserializeColor();
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Style":
				Style = item.DeserializeEnum<HatchStyle>();
				break;
			}
		}
	}

	public StiHatchBrush()
	{
		Style = (HatchStyle)3;
		ForeColor = Color.Black;
		BackColor = Color.White;
	}

	public StiHatchBrush(HatchStyle style, Color foreColor, Color backColor)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Style = style;
		ForeColor = foreColor;
		BackColor = backColor;
	}
}
