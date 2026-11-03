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

[StiServiceBitmap(typeof(StiGroupFooterBand), "Stimulsoft.Report.Images.Components.StiGroupFooterBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiGroupFooterBandV1Builder))]
[StiV2Builder(typeof(StiGroupFooterBandV2Builder))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiGroupFooterBand : StiDynamicBand, IStiKeepGroupFooterTogether
{
	private bool keepGroupFooterTogether = true;

	private StiGroupFooterBandInfoV1 groupFooterBandInfoV1;

	private StiGroupFooterBandInfoV2 groupFooterBandInfoV2;

	public override StiComponentId ComponentId => StiComponentId.StiGroupFooterBand;

	public override string HelpUrl => "user-manual/report_internals_groups_groupfooterband.htm";

	[StiEngine(/*Could not decode attribute arguments.*/)]
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
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the group footer is printed with data.")]
	[StiShowInContextMenu]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepGroupFooterTogether
	{
		get
		{
			return keepGroupFooterTogether;
		}
		set
		{
			if (keepGroupFooterTogether != value)
			{
				CheckBlockedException("KeepGroupFooterTogether");
				keepGroupFooterTogether = value;
			}
		}
	}

	[Browsable(false)]
	public StiGroupFooterBandInfoV1 GroupFooterBandInfoV1 => groupFooterBandInfoV1 ?? (groupFooterBandInfoV1 = new StiGroupFooterBandInfoV1());

	[Browsable(false)]
	public StiGroupFooterBandInfoV2 GroupFooterBandInfoV2 => groupFooterBandInfoV2 ?? (groupFooterBandInfoV2 = new StiGroupFooterBandInfoV2());

	[Browsable(false)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public int Line
	{
		get
		{
			if (GroupFooterBandInfoV2.GroupHeader == null)
			{
				return 1;
			}
			return GroupFooterBandInfoV2.GroupHeader.Line;
		}
	}

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(239, 155, 52);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(239, 155, 52);

	public override int ToolboxPosition => 6;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override StiComponentType ComponentType => StiComponentType.Detail;

	public override string LocalizedName => StiLocalization.Get("Components", "StiGroupFooterBand");

	public override int Priority => 300;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "KeepGroupFooterTogether", KeepGroupFooterTogether, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "KeepGroupFooterTogether")
			{
				keepGroupFooterTogether = StiJsonReportObjectHelper.DeserializeBool(item);
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
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Invalid comparison between Unknown and I4
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[9]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepGroupFooterTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintOn(),
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
		StiGroupFooterBand stiGroupFooterBand = base.Clone(cloneProperties, cloneComponents) as StiGroupFooterBand;
		stiGroupFooterBand.groupFooterBandInfoV1 = GroupFooterBandInfoV1.Clone() as StiGroupFooterBandInfoV1;
		stiGroupFooterBand.groupFooterBandInfoV2 = GroupFooterBandInfoV2.Clone() as StiGroupFooterBandInfoV2;
		return stiGroupFooterBand;
	}

	public override StiComponent CreateNew()
	{
		return new StiGroupFooterBand();
	}

	public StiGroupFooterBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiGroupFooterBand(RectangleD rect)
		: base(rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
