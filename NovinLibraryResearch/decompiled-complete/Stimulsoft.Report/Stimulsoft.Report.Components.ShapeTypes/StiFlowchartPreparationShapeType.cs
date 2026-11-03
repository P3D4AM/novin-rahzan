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

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.FlowchartPreparation.png")]
[TypeConverter(typeof(StiFlowchartPreparationShapeTypeConverter))]
[StiGdiShapeTypePainter(typeof(StiFlowchartPreparationGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiFlowchartPreparationWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiFlowchartPreparationShapeType : StiShapeTypeService
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiFlowchartPreparationShapeType;

	public override string ServiceName => StiLocalization.Get("Shapes", "FlowchartPreparation");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiShapeDirection Direction { get; set; } = StiShapeDirection.Left;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiShapeDirection.Left);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Direction")
			{
				Direction = StiJsonReportObjectHelper.DeserializeEnum<StiShapeDirection>(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Direction() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiShapeTypeService CreateNew()
	{
		return new StiFlowchartPreparationShapeType();
	}

	public StiFlowchartPreparationShapeType()
		: this(StiShapeDirection.Left)
	{
	}

	public StiFlowchartPreparationShapeType(StiShapeDirection direction)
	{
		Direction = direction;
	}
}
