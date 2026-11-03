using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

public class StiCrossTitle : StiCrossField
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossTitle;

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossTitle");

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(370)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the component is printed on all pages.")]
	public virtual bool PrintOnAllPages { get; set; } = true;


	[StiSerializable]
	[Browsable(false)]
	public string TypeOfComponent { get; set; } = string.Empty;


	[Browsable(true)]
	public sealed override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiFormatService TextFormat
	{
		get
		{
			return base.TextFormat;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool HideZeros
	{
		get
		{
			return base.HideZeros;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
		}
	}

	public override string CellText => GetTextInternal();

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TextFormat");
		JsonExtensions.RemoveProperty(val, "HideZeros");
		JsonExtensions.RemoveProperty(val, "Conditions");
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TypeOfComponent", TypeOfComponent);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "PrintOnAllPages"))
			{
				if (text == "TypeOfComponent")
				{
					TypeOfComponent = StiJsonReportObjectHelper.DeserializeString(item);
				}
			}
			else
			{
				PrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[1] { propertiesHelper.Text() });
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[4]
			{
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.WordWrap()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[6]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[7]
			{
				propertiesHelper.AllowHtmlTags(),
				propertiesHelper.TextBrush(),
				propertiesHelper.fAngle(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
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
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[3]
			{
				propertiesHelper.MergeHeaders(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOnAllPages()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[4]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.MergeHeaders(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOnAllPages()
			});
		}
		return stiPropertyCollection;
	}

	public override StiComponent CreateNew()
	{
		return new StiCrossTitle();
	}

	public StiCrossTitle()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		base.Brush = (StiBrush)new StiSolidBrush(Color.LightGray);
	}
}
