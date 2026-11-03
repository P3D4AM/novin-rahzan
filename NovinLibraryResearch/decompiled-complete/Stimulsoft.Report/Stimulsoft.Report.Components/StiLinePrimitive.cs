using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
public abstract class StiLinePrimitive : StiPrimitive
{
	public override bool InvalidateOnMouseOver => true;

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 60.0, 60.0);

	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a pen style.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiPenStyle Style { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets line color.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public Color Color { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[DefaultValue(1f)]
	[Description("Gets or sets size of the line.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Size { get; set; } = 1f;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Style", (Enum)(object)Style, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyColor(val, "Color", Color, Color.Black);
		JsonExtensions.AddPropertyFloat(val, "Size", Size, 1f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Style":
				Style = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Color":
				Color = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Black;
	}

	public StiLinePrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiLinePrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
