using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.ShapeTypes.Design;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.ShapeTypes;

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Arrow.png")]
[TypeConverter(typeof(StiArrowShapeTypeConverter))]
[StiGdiShapeTypePainter(typeof(StiArrowGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiArrowWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiArrowShapeType : StiShapeTypeService
{
	private float arrowWidth = 0.3f;

	private float arrowHeight = 0.4f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiArrowShapeType;

	public override string ServiceName => StiLocalization.Get("Shapes", "Arrow");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the arrow direction.")]
	public virtual StiShapeDirection Direction { get; set; }

	[DefaultValue(0.3f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[Description("Gets or sets the arrow width factor.")]
	public virtual float ArrowWidth
	{
		get
		{
			return arrowWidth;
		}
		set
		{
			if (value >= 0f && value <= 0.5f)
			{
				arrowWidth = value;
			}
		}
	}

	[DefaultValue(0.4f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("Gets or sets the arrow height factor.")]
	public virtual float ArrowHeight
	{
		get
		{
			return arrowHeight;
		}
		set
		{
			if (value >= 0f && value <= 1f)
			{
				arrowHeight = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiShapeDirection.Up);
		JsonExtensions.AddPropertyFloat(val, "ArrowWidth", ArrowWidth, 0.3f);
		JsonExtensions.AddPropertyFloat(val, "ArrowHeight", ArrowHeight, 0.4f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Direction":
				Direction = StiJsonReportObjectHelper.DeserializeEnum<StiShapeDirection>(item);
				break;
			case "ArrowWidth":
				arrowWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "ArrowHeight":
				arrowHeight = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.Direction(),
			propertiesHelper.ArrowWidth(),
			propertiesHelper.ArrowHeight()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiShapeTypeService CreateNew()
	{
		return new StiArrowShapeType();
	}

	public StiArrowShapeType()
		: this(StiShapeDirection.Up, 0.3f, 0.4f)
	{
	}

	public StiArrowShapeType(StiShapeDirection direction, float arrowWidth, float arrowHeight)
	{
		Direction = direction;
		this.arrowWidth = arrowWidth;
		this.arrowHeight = arrowHeight;
	}
}
