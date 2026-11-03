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

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.RoundedRectangle.png")]
[TypeConverter(typeof(StiRoundedRectangleShapeTypeConverter))]
[StiGdiShapeTypePainter(typeof(StiRoundedRectangleGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiRoundedRectangleWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRoundedRectangleShapeType : StiShapeTypeService
{
	private float round = 0.2f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRoundedRectangleShapeType;

	public override string ServiceName => StiLocalization.Get("Shapes", "RoundedRectangle");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable]
	[Description("Gets or sets the factor of rounding.")]
	public virtual float Round
	{
		get
		{
			return round;
		}
		set
		{
			if (value > 0f && (double)value <= 0.5)
			{
				round = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyFloat(val, "Round", Round, 0.2f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Round")
			{
				round = StiJsonReportObjectHelper.DeserializeFloat(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.ShapeRound() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiShapeTypeService CreateNew()
	{
		return new StiRoundedRectangleShapeType();
	}

	public StiRoundedRectangleShapeType()
		: this(0.2f)
	{
	}

	public StiRoundedRectangleShapeType(float round)
	{
		this.round = round;
	}
}
