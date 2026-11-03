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

[StiServiceBitmap(typeof(StiShapeTypeService), "Stimulsoft.Report.Bmp.ShapeTypes.Octagon.png")]
[TypeConverter(typeof(StiOctagonShapeTypeConverter))]
[StiGdiShapeTypePainter(typeof(StiOctagonGdiShapeTypePainter))]
[StiWpfShapeTypePainter("Stimulsoft.Report.Painters.StiOctagonWpfShapeTypePainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiOctagonShapeType : StiShapeTypeService
{
	private float bevel;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiOctagonShapeType;

	public override string ServiceName => StiLocalization.Get("Shapes", "Octagon");

	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[Description("Gets or sets the octagon autosize.")]
	public virtual bool AutoSize { get; set; } = true;


	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[Description("Gets or sets the bevel size.")]
	public virtual float Bevel
	{
		get
		{
			return bevel;
		}
		set
		{
			if (value >= 0f)
			{
				bevel = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "AutoSize", AutoSize, true, false);
		JsonExtensions.AddPropertyFloat(val, "Bevel", Bevel, 0f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "AutoSize"))
			{
				if (name == "Bevel")
				{
					bevel = StiJsonReportObjectHelper.DeserializeFloat(item);
				}
			}
			else
			{
				AutoSize = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[2]
		{
			propertiesHelper.AutoSize(),
			propertiesHelper.Bevel()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override StiShapeTypeService CreateNew()
	{
		return new StiOctagonShapeType();
	}

	public StiOctagonShapeType()
		: this(autoSize: true, 0f)
	{
	}

	public StiOctagonShapeType(bool autoSize, float bevel)
	{
		AutoSize = autoSize;
		this.bevel = bevel;
	}
}
