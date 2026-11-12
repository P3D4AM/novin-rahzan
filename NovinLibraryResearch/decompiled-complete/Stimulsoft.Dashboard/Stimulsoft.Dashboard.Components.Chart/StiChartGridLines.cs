using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiChartGridLines : ICloneable, IStiJsonReportObject, IStiDefault
{
	public enum Order
	{
		Color = 1,
		Visible
	}

	[StiSerializable]
	[Description("Gets or sets color which will be used for drawing major grid lines.")]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(1)]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color Color { get; set; } = Color.Transparent;


	public abstract bool Visible { get; set; }

	[Browsable(false)]
	public abstract bool IsDefault { get; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyColor("Color", Color, Color.Transparent);
		jObject.AddPropertyBool("Visible", Visible);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Color"))
			{
				if (name == "Visible")
				{
					Visible = item.DeserializeBool();
				}
			}
			else
			{
				Color = item.DeserializeColor();
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone() as StiChartGridLines;
	}

	private bool ShouldSerializeColor()
	{
		return Color != Color.Transparent;
	}
}
