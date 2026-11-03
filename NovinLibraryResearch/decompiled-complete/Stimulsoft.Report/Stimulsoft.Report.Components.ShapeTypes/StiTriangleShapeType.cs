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

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Triangle.png")]
[TypeConverter(typeof(StiTriangleShapeTypeConverter))]
[StiGdiShapeTypePainter(typeof(StiTriangleGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiTriangleWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTriangleShapeType : StiShapeTypeService
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiTriangleShapeType;

	public override string ServiceName => StiLocalization.Get("Shapes", "Triangle");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiShapeDirection Direction { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Direction", (Enum)Direction, (Enum)StiShapeDirection.Up);
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

	public override object Clone()
	{
		StiTriangleShapeType stiTriangleShapeType = MemberwiseClone() as StiTriangleShapeType;
		stiTriangleShapeType.Direction = Direction;
		return stiTriangleShapeType;
	}

	public override StiShapeTypeService CreateNew()
	{
		return new StiTriangleShapeType();
	}

	public StiTriangleShapeType()
		: this(StiShapeDirection.Up)
	{
	}

	public StiTriangleShapeType(StiShapeDirection direction)
	{
		Direction = direction;
	}
}
