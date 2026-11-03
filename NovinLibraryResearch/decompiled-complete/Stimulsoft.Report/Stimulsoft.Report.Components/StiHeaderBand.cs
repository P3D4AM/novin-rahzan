using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiHeaderBand), "Stimulsoft.Report.Images.Components.StiHeaderBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiHeaderBandV1Builder))]
[StiV2Builder(typeof(StiHeaderBandV2Builder))]
[StiContextTool(typeof(IStiPrintIfEmpty))]
[StiContextTool(typeof(IStiPrintOnAllPages))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiStartNewPage))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiCanShrink))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiHeaderBand : StiDynamicBand, IStiPrintIfEmpty, IStiPrintOnAllPages, IStiPrintOnEvenOddPages, IStiStartNewPage, IStiKeepHeaderTogether
{
	private bool keepHeaderTogether = true;

	private StiHeaderBandInfoV1 headerBandInfoV1;

	private StiHeaderBandInfoV2 headerBandInfoV2;

	public override StiComponentId ComponentId => StiComponentId.StiHeaderBand;

	public override string HelpUrl => "user-manual/report_internals_creating_lists_list_with_header.htm";

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that header is printed with data together.")]
	[StiShowInContextMenu]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepHeaderTogether
	{
		get
		{
			return keepHeaderTogether;
		}
		set
		{
			if (keepHeaderTogether != value)
			{
				CheckBlockedException("KeepHeaderTogether");
				keepHeaderTogether = value;
			}
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool CanBreak
	{
		get
		{
			return base.CanBreak;
		}
		set
		{
			base.CanBreak = value;
			HeaderBandInfoV1.ForceCanBreak = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiOrder(460)]
	[Description("Gets or sets value indicates that it is necessary to print every new string on a new page.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool StartNewPage { get; set; }

	[DefaultValue(100f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(470)]
	[Description("Gets or sets value which indicates how much free space on a page (in percentage terms) should be reserved for formation of a new page. The value should be set in the range from 0 to 100. If the value is 100 then, in any case, a new page will be formed. This property is used together with the StartNewPage property.")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float StartNewPageIfLessThan
	{
		get
		{
			return BreakIfLessThan;
		}
		set
		{
			BreakIfLessThan = value;
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool ResetPageNumber
	{
		get
		{
			return base.ResetPageNumber;
		}
		set
		{
			base.ResetPageNumber = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the header if data not present.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintIfEmpty { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(370)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the component is printed on all pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintOnAllPages { get; set; } = true;


	[DefaultValue(StiPrintOnEvenOddPagesType.Ignore)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(380)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates that the component is printed on even-odd pages.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPrintOnEvenOddPagesType PrintOnEvenOddPages { get; set; }

	[Browsable(false)]
	public StiHeaderBandInfoV1 HeaderBandInfoV1 => headerBandInfoV1 ?? (headerBandInfoV1 = new StiHeaderBandInfoV1());

	[Browsable(false)]
	public StiHeaderBandInfoV2 HeaderBandInfoV2 => headerBandInfoV2 ?? (headerBandInfoV2 = new StiHeaderBandInfoV2());

	public override int ToolboxPosition => 7;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority => 300;

	public override string LocalizedName => StiLocalization.Get("Components", "StiHeaderBand");

	public override StiComponentType ComponentType => StiComponentType.Detail;

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(178, 197, 223);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(178, 197, 223);

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyBool(val, "KeepHeaderTogether", KeepHeaderTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "StartNewPage", StartNewPage, false, false);
		JsonExtensions.AddPropertyFloat(val, "StartNewPageIfLessThan", StartNewPageIfLessThan, 100f);
		JsonExtensions.AddPropertyBool(val, "PrintIfEmpty", PrintIfEmpty, false, false);
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, true, false);
		JsonExtensions.AddPropertyEnum(val, "PrintOnEvenOddPages", (Enum)PrintOnEvenOddPages, (Enum)StiPrintOnEvenOddPagesType.Ignore);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "KeepHeaderTogether":
				keepHeaderTogether = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPage":
				StartNewPage = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartNewPageIfLessThan":
				StartNewPageIfLessThan = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "PrintIfEmpty":
				PrintIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PrintOnAllPages":
				PrintOnAllPages = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PrintOnEvenOddPages":
				PrintOnEvenOddPages = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnEvenOddPagesType>(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[4]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[5]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.SkipFirst()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.PageColumnBreak, new StiPropertyObject[6]
			{
				propertiesHelper.NewPageBefore(),
				propertiesHelper.NewPageAfter(),
				propertiesHelper.NewColumnBefore(),
				propertiesHelper.NewColumnAfter(),
				propertiesHelper.BreakIfLessThan(),
				propertiesHelper.SkipFirst()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Height() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Height(),
				propertiesHelper.MinHeight(),
				propertiesHelper.MaxHeight()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[4]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[12]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepHeaderTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintIfEmpty(),
				propertiesHelper.PrintOn(),
				propertiesHelper.PrintOnAllPages(),
				propertiesHelper.PrintOnEvenOddPages(),
				propertiesHelper.ResetPageNumber()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	public override object Clone(bool cloneProperties, bool cloneComponents)
	{
		StiHeaderBand stiHeaderBand = base.Clone(cloneProperties, cloneComponents) as StiHeaderBand;
		stiHeaderBand.headerBandInfoV1 = HeaderBandInfoV1.Clone() as StiHeaderBandInfoV1;
		stiHeaderBand.headerBandInfoV2 = HeaderBandInfoV2.Clone() as StiHeaderBandInfoV2;
		return stiHeaderBand;
	}

	public override StiComponent CreateNew()
	{
		return new StiHeaderBand();
	}

	public StiHeaderBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiHeaderBand(RectangleD rect)
		: base(rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
