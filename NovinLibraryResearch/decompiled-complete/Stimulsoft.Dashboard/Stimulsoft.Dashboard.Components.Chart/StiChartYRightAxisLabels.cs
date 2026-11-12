using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Components.Chart;

public class StiChartYRightAxisLabels : StiChartAxisLabels
{
	[StiSerializable]
	[DefaultValue(StiHorAlignment.Left)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets label text alignment.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(7)]
	public override StiHorAlignment TextAlignment { get; set; } = StiHorAlignment.Left;


	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (!ShouldSerializeFont() && !ShouldSerializeColor() && base.Placement == StiLabelsPlacement.AutoRotation && TextAlignment == StiHorAlignment.Left && base.TextAfter == "" && base.TextBefore == "" && base.Angle == 0f)
			{
				return base.Step == 0f;
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyFloat("Angle", base.Angle);
		jObject.AddPropertyStringNullOrEmpty("TextBefore", base.TextBefore);
		jObject.AddPropertyStringNullOrEmpty("TextAfter", base.TextAfter);
		jObject.AddPropertyFontArial8("Font", base.Font);
		jObject.AddPropertyEnum("Placement", base.Placement, StiLabelsPlacement.AutoRotation);
		jObject.AddPropertyColor("Color", base.Color, Color.Transparent);
		jObject.AddPropertyEnum("TextAlignment", TextAlignment, StiHorAlignment.Left);
		jObject.AddPropertyFloat("Step", base.Step);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Angle":
				base.Angle = item.DeserializeFloat();
				break;
			case "TextBefore":
				base.TextBefore = item.DeserializeString();
				break;
			case "TextAfter":
				base.TextAfter = item.DeserializeString();
				break;
			case "Font":
				base.Font = item.DeserializeFont(base.Font);
				break;
			case "Placement":
				base.Placement = item.DeserializeEnum<StiLabelsPlacement>();
				break;
			case "Color":
				base.Color = item.DeserializeColor();
				break;
			case "TextAlignment":
				TextAlignment = item.DeserializeEnum<StiHorAlignment>();
				break;
			case "Step":
				base.Step = item.DeserializeFloat();
				break;
			}
		}
	}

	public StiChartYRightAxisLabels()
	{
	}

	[StiUniversalConstructor("Labels")]
	public StiChartYRightAxisLabels(string textBefore, string textAfter, float angle, Font font, StiLabelsPlacement placement, Color color, StiHorAlignment textAlignment)
	{
		base.TextBefore = textBefore;
		base.TextAfter = textAfter;
		base.Angle = angle;
		base.Font = font;
		base.Placement = placement;
		base.Color = color;
		TextAlignment = textAlignment;
	}
}
