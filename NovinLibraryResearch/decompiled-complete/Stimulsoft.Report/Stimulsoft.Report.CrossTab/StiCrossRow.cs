using System;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossRow : StiCrossHeader
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossRow;

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossRow");

	[DefaultValue(StiEnumeratorType.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiEnumeratorType EnumeratorType { get; set; }

	[DefaultValue(".")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string EnumeratorSeparator { get; set; } = ".";


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public bool KeepMergedCellsTogether { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "EnumeratorType", (Enum)EnumeratorType, (Enum)StiEnumeratorType.None);
		JsonExtensions.AddPropertyString(val, "EnumeratorSeparator", EnumeratorSeparator, ".");
		JsonExtensions.AddPropertyBool(val, "KeepMergedCellsTogether", KeepMergedCellsTogether, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "EnumeratorType":
				EnumeratorType = StiJsonReportObjectHelper.DeserializeEnum<StiEnumeratorType>(item);
				break;
			case "EnumeratorSeparator":
				EnumeratorSeparator = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "KeepMergedCellsTogether":
				KeepMergedCellsTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.EnumeratorSeparator(),
				propertiesHelper.EnumeratorType(),
				propertiesHelper.MergeHeaders(),
				propertiesHelper.KeepMergedCellsTogether(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.ShowTotal()
			});
		}
		return stiPropertyCollection;
	}

	public StiCrossTitle GetCrossRowTitle()
	{
		if (!(Parent is StiCrossTab stiCrossTab))
		{
			return null;
		}
		string name = Name + "_Title";
		return stiCrossTab.Components.ToList().FirstOrDefault((StiComponent c) => c is StiCrossTitle && c.Name == name) as StiCrossTitle;
	}

	public StiCrossRowTotal GetCrossRowTotal()
	{
		if (!(Parent is StiCrossTab stiCrossTab))
		{
			return null;
		}
		string name = Name.Replace("Row", "RowTotal");
		return stiCrossTab.Components.ToList().FirstOrDefault((StiComponent c) => c is StiCrossRowTotal && c.Name == name) as StiCrossRowTotal;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossRow();
	}
}
