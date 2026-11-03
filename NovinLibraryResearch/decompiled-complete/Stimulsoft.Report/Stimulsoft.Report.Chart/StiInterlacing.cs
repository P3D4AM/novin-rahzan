using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public abstract class StiInterlacing : IStiInterlacing, ICloneable, IStiJsonReportObject, IStiDefault, IStiPropertyGridObject
{
	internal bool needSetAreaJsonPropertyInternal;

	private bool allowApplyStyle = true;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiInterlacing;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public StiInterlacingCoreXF Core { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle
	{
		get
		{
			return allowApplyStyle;
		}
		set
		{
			if (allowApplyStyle != value)
			{
				allowApplyStyle = value;
				if (value && Area?.Chart != null)
				{
					Core.ApplyStyle(Area.Chart.Style);
				}
			}
		}
	}

	[StiSerializable]
	[Description("Gets or sets brush which used for drawing interlacing bar.")]
	public StiBrush InterlacedBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of interlaced bars.")]
	public bool Visible { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiArea Area { get; set; }

	[Browsable(false)]
	public bool IsDefault => AllowApplyStyle;

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", allowApplyStyle, true, false);
		JsonExtensions.AddPropertyBrush(val, "InterlacedBrush", InterlacedBrush);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		if (Area != null)
		{
			JsonExtensions.AddPropertyBool(val, "Area", true, false, false);
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				allowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "InterlacedBrush":
				InterlacedBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Area":
				needSetAreaJsonPropertyInternal = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.Interlacing() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		IStiInterlacing stiInterlacing = MemberwiseClone() as IStiInterlacing;
		object obj = InterlacedBrush.Clone();
		stiInterlacing.InterlacedBrush = (StiBrush)((obj is StiBrush) ? obj : null);
		if (Core != null)
		{
			stiInterlacing.Core = Core.Clone() as StiInterlacingCoreXF;
			stiInterlacing.Core.Interlacing = stiInterlacing;
		}
		return stiInterlacing;
	}

	private bool ShouldSerializeInterlacedBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (InterlacedBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)InterlacedBrush).Color == Color.Transparent);
		}
		return true;
	}

	public StiInterlacing()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		Core = new StiInterlacingCoreXF(this);
	}

	public StiInterlacing(StiBrush interlacedBrush, bool visible, bool allowApplyStyle)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		InterlacedBrush = interlacedBrush;
		Visible = visible;
		this.allowApplyStyle = allowApplyStyle;
		Core = new StiInterlacingCoreXF(this);
	}
}
