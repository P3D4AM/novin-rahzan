using System;
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
public class StiGlareBrush : StiBrush
{
	private float focus;

	private float scale;

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

	[StiSerializable]
	[DefaultValue(0.5f)]
	[Description("Gets or sets value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).")]
	[RefreshProperties(RefreshProperties.All)]
	[StiOrder(400)]
	public float Focus
	{
		get
		{
			return focus;
		}
		set
		{
			if (focus != value)
			{
				if (value > 1f || value < 0f)
				{
					throw new ArgumentException("Focus must be in range between 0 and 1!");
				}
				focus = value;
			}
		}
	}

	[StiSerializable]
	[Description("Gets or sets value from 0 through 1 that specifies how fast the colors falloff from the focus.")]
	[RefreshProperties(RefreshProperties.All)]
	[DefaultValue(1f)]
	[StiOrder(500)]
	[StiGuiMode(StiGuiMode.Gdi)]
	public float Scale
	{
		get
		{
			return scale;
		}
		set
		{
			if (scale != value)
			{
				if (value > 1f || value < 0f)
				{
					throw new ArgumentException("Scale must be in range between 0 and 1!");
				}
				scale = value;
			}
		}
	}

	public override StiBrushIdent Ident => StiBrushIdent.Glare;

	private bool ShouldSerializeStartColor()
	{
		return StartColor != Color.Black;
	}

	private bool ShouldSerializeEndColor()
	{
		return EndColor != Color.White;
	}

	protected bool Equals(StiGlareBrush other)
	{
		if (StartColor.Equals((object?)other.StartColor) && EndColor.Equals((object?)other.EndColor) && Angle.Equals(other.Angle) && focus.Equals(other.focus))
		{
			return scale.Equals(other.scale);
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
					return Equals((StiGlareBrush)obj);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int hashCode = "StiGlareBrush".GetHashCode();
		hashCode = (hashCode * 397) ^ StartColor.GetHashCode();
		hashCode = (hashCode * 397) ^ EndColor.GetHashCode();
		hashCode = (hashCode * 397) ^ Angle.GetHashCode();
		hashCode = (hashCode * 397) ^ focus.GetHashCode();
		return (hashCode * 397) ^ scale.GetHashCode();
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
			case "Focus":
				Focus = item.DeserializeFloat();
				break;
			case "Scale":
				Scale = item.DeserializeFloat();
				break;
			}
		}
	}

	public StiGlareBrush()
		: this(Color.Black, Color.White, 0.0)
	{
	}

	public StiGlareBrush(Color startColor, Color endColor, double angle)
		: this(startColor, endColor, angle, 0.5f, 1f)
	{
	}

	public StiGlareBrush(Color startColor, Color endColor, double angle, float focus, float scale)
	{
		StartColor = startColor;
		EndColor = endColor;
		Angle = angle;
		this.focus = focus;
		this.scale = scale;
	}
}
