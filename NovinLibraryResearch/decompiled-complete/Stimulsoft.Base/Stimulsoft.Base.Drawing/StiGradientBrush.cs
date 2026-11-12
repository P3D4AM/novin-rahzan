using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[RefreshProperties(RefreshProperties.All)]
[JsonObject]
public class StiGradientBrush : StiBrush
{
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the starting color for the gradient.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(200)]
	public Color StartColor { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the ending color for the gradient.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(300)]
	public Color EndColor { get; set; }

	[StiSerializable]
	[Description("Gets or sets the angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(100)]
	[DefaultValue(0.0)]
	public double Angle { get; set; }

	public override StiBrushIdent Ident => StiBrushIdent.Gradient;

	private bool ShouldSerializeStartColor()
	{
		return StartColor != Color.White;
	}

	private bool ShouldSerializeEndColor()
	{
		return EndColor != Color.Black;
	}

	protected bool Equals(StiGradientBrush other)
	{
		if (StartColor.Equals((object?)other.StartColor) && EndColor.Equals((object?)other.EndColor))
		{
			return Angle.Equals(other.Angle);
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
					return Equals((StiGradientBrush)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = "StiGradientBrush".GetHashCode();
		hashCode = (hashCode * 397) ^ StartColor.GetHashCode();
		hashCode = (hashCode * 397) ^ EndColor.GetHashCode();
		return (hashCode * 397) ^ Angle.GetHashCode();
	}

	public void LoadValuesFromJson(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "StartColor":
				StartColor = item.DeserializeColor();
				break;
			case "EndColor":
				EndColor = item.DeserializeColor();
				break;
			case "Angle":
				Angle = item.DeserializeDouble();
				break;
			}
		}
	}

	public StiGradientBrush()
	{
		StartColor = Color.Black;
		EndColor = Color.White;
		Angle = 0.0;
	}

	public StiGradientBrush(Color startColor, Color endColor, double angle)
	{
		StartColor = startColor;
		EndColor = endColor;
		Angle = angle;
	}
}
