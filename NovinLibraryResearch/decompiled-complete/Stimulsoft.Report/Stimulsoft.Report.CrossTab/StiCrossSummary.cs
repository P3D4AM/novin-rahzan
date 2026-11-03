using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossSummary : StiCrossCell
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossSummary;

	protected override StiTextHorAlignment DefaultHorAlignment => (StiTextHorAlignment)2;

	[StiOrder(130)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override StiTextHorAlignment HorAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.HorAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.HorAlignment = value;
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiOrder(140)]
	public override StiVertAlignment VertAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.VertAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.VertAlignment = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that the image will save its aspect ratio.")]
	public bool AspectRatio { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the horizontal alignment of an object.")]
	public StiHorAlignment ImageHorAlignment { get; set; } = (StiHorAlignment)1;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the vertical alignment of an object.")]
	public StiVertAlignment ImageVertAlignment { get; set; }

	[Browsable(true)]
	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that this component will stretch the image till the image will get size equal in its size on the page.")]
	public bool Stretch { get; set; } = true;


	[StiNonSerialized]
	[Browsable(false)]
	public string CrossColumnValue { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	public string CrossRowValue { get; set; }

	[Browsable(false)]
	public int IndexOfSelectValue { get; set; } = -1;


	public override string CellText => "0";

	[Browsable(true)]
	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that no need show zeroes.")]
	public override bool HideZeros
	{
		get
		{
			return base.HideZeros;
		}
		set
		{
			base.HideZeros = value;
		}
	}

	[Description("Gets or sets the type of values summation.")]
	[DefaultValue(StiSummaryType.Sum)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiSummaryType Summary { get; set; } = StiSummaryType.Sum;


	[Description("Gets or sets the type of values summation.")]
	[DefaultValue(StiSummaryValues.AllValues)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiSummaryValues SummaryValues { get; set; }

	[Description("Gets or sets value which indicates that style of summary cell will be used in row total.")]
	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(320)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool UseStyleOfSummaryInRowTotal { get; set; }

	[Description("Gets or sets value which indicates that style of summary cell will be used in column total.")]
	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool UseStyleOfSummaryInColumnTotal { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that value in cell must be shown as percents.")]
	public virtual bool ShowPercents { get; set; }

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossSummary");

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiTextHorAlignment)2);
		JsonExtensions.AddPropertyBool(val, "HideZeros", HideZeros, true, false);
		JsonExtensions.AddPropertyBool(val, "AspectRatio", AspectRatio, false, false);
		JsonExtensions.AddPropertyBool(val, "Stretch", Stretch, true, false);
		JsonExtensions.AddPropertyEnum(val, "Summary", (Enum)Summary, (Enum)StiSummaryType.Sum);
		JsonExtensions.AddPropertyEnum(val, "SummaryValues", (Enum)SummaryValues, (Enum)StiSummaryValues.AllValues);
		JsonExtensions.AddPropertyBool(val, "UseStyleOfSummaryInRowTotal", UseStyleOfSummaryInRowTotal, false, false);
		JsonExtensions.AddPropertyBool(val, "UseStyleOfSummaryInColumnTotal", UseStyleOfSummaryInColumnTotal, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowPercents", ShowPercents, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AspectRatio":
				AspectRatio = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Stretch":
				Stretch = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Summary":
				Summary = StiJsonReportObjectHelper.DeserializeEnum<StiSummaryType>(item);
				break;
			case "SummaryValues":
				SummaryValues = StiJsonReportObjectHelper.DeserializeEnum<StiSummaryValues>(item);
				break;
			case "UseStyleOfSummaryInRowTotal":
				UseStyleOfSummaryInRowTotal = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "UseStyleOfSummaryInColumnTotal":
				UseStyleOfSummaryInColumnTotal = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowPercents":
				ShowPercents = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Invalid comparison between Unknown and I4
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		bool flag = Summary == StiSummaryType.Image;
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.ShowPercents(),
			propertiesHelper.Summary(),
			propertiesHelper.SummaryValues(),
			propertiesHelper.Value()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		if (flag)
		{
			value = new StiPropertyObject[4]
			{
				propertiesHelper.ImageHorAlignment(),
				propertiesHelper.ImageVertAlignment(),
				propertiesHelper.AspectRatio(),
				propertiesHelper.Stretch()
			};
			stiPropertyCollection.Add(StiPropertyCategories.ImageAdditional, value);
		}
		if (!flag)
		{
			value = (((int)level == 0) ? new StiPropertyObject[8]
			{
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.HideZeros(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.TextFormat(),
				propertiesHelper.WordWrap()
			} : (((int)level != 1) ? new StiPropertyObject[11]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.HideZeros(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.Margins(),
				propertiesHelper.TextFormat(),
				propertiesHelper.TextOptions(),
				propertiesHelper.WordWrap()
			} : new StiPropertyObject[10]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.HideZeros(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.Margins(),
				propertiesHelper.TextFormat(),
				propertiesHelper.WordWrap()
			}));
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		}
		if ((int)level != 0)
		{
			value = new StiPropertyObject[2]
			{
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			};
			stiPropertyCollection.Add(StiPropertyCategories.Position, value);
		}
		value = (((int)level != 0) ? new StiPropertyObject[7]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions(),
			propertiesHelper.ComponentStyle(),
			propertiesHelper.UseParentStyles(),
			propertiesHelper.UseStyleOfSummaryInRowTotal(),
			propertiesHelper.UseStyleOfSummaryInColumnTotal()
		} : new StiPropertyObject[6]
		{
			propertiesHelper.Brush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions(),
			propertiesHelper.ComponentStyle(),
			propertiesHelper.UseStyleOfSummaryInRowTotal(),
			propertiesHelper.UseStyleOfSummaryInColumnTotal()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = (((int)level != 0) ? new StiPropertyObject[2]
		{
			propertiesHelper.InteractionEditor(),
			propertiesHelper.MergeHeaders()
		} : new StiPropertyObject[1] { propertiesHelper.MergeHeaders() });
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		StiCrossSummary stiCrossSummary = (StiCrossSummary)base.Clone(cloneProperties);
		stiCrossSummary.AspectRatio = AspectRatio;
		stiCrossSummary.Stretch = Stretch;
		stiCrossSummary.ImageHorAlignment = ImageHorAlignment;
		stiCrossSummary.ImageVertAlignment = ImageVertAlignment;
		stiCrossSummary.Summary = Summary;
		stiCrossSummary.SummaryValues = SummaryValues;
		stiCrossSummary.UseStyleOfSummaryInRowTotal = UseStyleOfSummaryInRowTotal;
		stiCrossSummary.UseStyleOfSummaryInColumnTotal = UseStyleOfSummaryInColumnTotal;
		return stiCrossSummary;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossSummary();
	}

	public StiCrossSummary()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		base.HorAlignment = (StiTextHorAlignment)2;
		base.HideZeros = true;
	}
}
