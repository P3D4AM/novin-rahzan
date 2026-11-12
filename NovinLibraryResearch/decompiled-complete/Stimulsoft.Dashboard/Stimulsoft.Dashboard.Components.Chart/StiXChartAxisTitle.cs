using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiXChartAxisTitle : StiChartAxisTitle
{
	[StiSerializable]
	[DefaultValue(StiDirection.LeftToRight)]
	[Description("Gets or set text direction for axis title drawing.")]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory("Common", StiCategorySource.PropertyCategory)]
	[StiOrder(3)]
	public virtual StiDirection Direction { get; set; }

	[Browsable(false)]
	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault)
			{
				return Direction == StiDirection.LeftToRight;
			}
			return false;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyEnum("Direction", Direction, StiDirection.LeftToRight);
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Direction")
			{
				Direction = item.DeserializeEnum<StiDirection>();
			}
		}
	}

	public StiXChartAxisTitle()
	{
	}

	[StiUniversalConstructor("Title")]
	public StiXChartAxisTitle(Font font, string text, Color color, StringAlignment alignment, StiDirection direction, StiTitlePosition position, bool visible)
		: base(font, text, color, alignment, position, visible)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		Direction = direction;
	}
}
