using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossColumn : StiCrossHeader
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossColumn;

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossColumn");

	[DefaultValue(StiEnumeratorType.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets enumerator type.")]
	public StiEnumeratorType EnumeratorType { get; set; }

	[DefaultValue(".")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets enumerator separator.")]
	public string EnumeratorSeparator { get; set; } = ".";


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "EnumeratorType", (Enum)EnumeratorType, (Enum)StiEnumeratorType.None);
		JsonExtensions.AddPropertyString(val, "EnumeratorSeparator", EnumeratorSeparator, ".");
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "EnumeratorType"))
			{
				if (text == "EnumeratorSeparator")
				{
					EnumeratorSeparator = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				EnumeratorType = StiJsonReportObjectHelper.DeserializeEnum<StiEnumeratorType>(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Invalid comparison between Unknown and I4
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		stiPropertyCollection.Add(StiPropertyCategories.Data, new StiPropertyObject[4]
		{
			propertiesHelper.DisplayValue(),
			propertiesHelper.SortDirection(),
			propertiesHelper.SortType(),
			propertiesHelper.Value()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[5]
			{
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.TextFormat(),
				propertiesHelper.WordWrap()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[7]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
				propertiesHelper.TextFormat(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[8]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
				propertiesHelper.TextFormat(),
				propertiesHelper.TextOptions(),
				propertiesHelper.WordWrap()
			});
		}
		if ((int)level != 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[2]
			{
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.EnumeratorSeparator(),
				propertiesHelper.EnumeratorType(),
				propertiesHelper.MergeHeaders(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.ShowTotal()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.EnumeratorSeparator(),
				propertiesHelper.EnumeratorType(),
				propertiesHelper.MergeHeaders(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.ShowTotal()
			});
		}
		return stiPropertyCollection;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossColumn();
	}
}
