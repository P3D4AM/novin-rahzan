using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiReportSummaryBand), "Stimulsoft.Report.Images.Components.StiReportSummaryBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiReportSummaryBandV1Builder))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiPrintIfEmpty))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiReportSummaryBand : StiDynamicBand, IStiPrintIfEmpty, IStiKeepReportSummaryTogether
{
	private bool keepReportSummaryTogether = true;

	public override StiComponentId ComponentId => StiComponentId.StiReportSummaryBand;

	public override string HelpUrl => "user-manual/report_internals_report_bands_reportsummaryband.htm";

	[StiNonSerialized]
	[Browsable(false)]
	public override StiPrintOnType PrintOn
	{
		get
		{
			return base.PrintOn;
		}
		set
		{
			base.PrintOn = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the report summary is printed with data.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepReportSummaryTogether
	{
		get
		{
			return keepReportSummaryTogether;
		}
		set
		{
			if (keepReportSummaryTogether != value)
			{
				CheckBlockedException("KeepReportSummaryTogether");
				keepReportSummaryTogether = value;
			}
		}
	}

	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the footer is printed at bottom of page.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintIfEmpty { get; set; } = true;


	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(159, 213, 183);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(159, 213, 183);

	public override int ToolboxPosition => 2;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override string LocalizedName => StiLocalization.Get("Components", "StiReportSummaryBand");

	public override int Priority => 500;

	public override StiComponentType ComponentType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report != null && (int)Report.EngineVersion == 2)
			{
				return base.ComponentType;
			}
			return StiComponentType.Master;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.AddPropertyBool(val, "KeepReportSummaryTogether", KeepReportSummaryTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "PrintIfEmpty", PrintIfEmpty, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "KeepReportSummaryTogether"))
			{
				if (text == "PrintIfEmpty")
				{
					PrintIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				keepReportSummaryTogether = StiJsonReportObjectHelper.DeserializeBool(item);
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
				propertiesHelper.KeepReportSummaryTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintIfEmpty(),
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

	public override bool CanContainIn(StiComponent component)
	{
		if (component is IStiReportControl)
		{
			return false;
		}
		if (component is StiPage)
		{
			return true;
		}
		return false;
	}

	public override StiComponent CreateNew()
	{
		return new StiReportSummaryBand();
	}

	public StiComponent GetMaster()
	{
		foreach (StiComponent component in Parent.Components)
		{
			if (component is StiDataBand)
			{
				return component;
			}
		}
		return null;
	}

	public StiReportSummaryBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiReportSummaryBand(RectangleD rect)
		: base(rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		DockStyle = StiDockStyle.Top;
		PlaceOnToolbox = false;
	}
}
