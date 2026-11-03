using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiColumnFooterBand), "Stimulsoft.Report.Images.Components.StiColumnFooterBand.png")]
[StiV2Builder(typeof(StiColumnFooterBandV2Builder))]
[StiToolbox(true)]
[StiContextTool(typeof(IStiPrintIfEmpty))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiPrintOnAllPages))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiColumnFooterBand : StiFooterBand
{
	public override StiComponentId ComponentId => StiComponentId.StiColumnFooterBand;

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

	[StiEngine(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool PrintAtBottom
	{
		get
		{
			return base.PrintAtBottom;
		}
		set
		{
			base.PrintAtBottom = value;
		}
	}

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(239, 109, 73);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(239, 109, 73);

	public override string HelpUrl => "user-manual/report_internals_columns_columns_on_data_band_columnfooter_band.htm";

	public override int ToolboxPosition => 10;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority => 300;

	public override string LocalizedName => StiLocalization.Get("Components", "StiColumnFooterBand");

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "KeepFooterTogether", KeepFooterTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "StartNewPage", StartNewPage, false, false);
		JsonExtensions.AddPropertyFloat(val, "StartNewPageIfLessThan", StartNewPageIfLessThan, 100f);
		JsonExtensions.AddPropertyBool(val, "PrintIfEmpty", PrintIfEmpty, false, false);
		JsonExtensions.AddPropertyEnum(val, "PrintOnEvenOddPages", (Enum)PrintOnEvenOddPages, (Enum)StiPrintOnEvenOddPagesType.Ignore);
		JsonExtensions.AddPropertyBool(val, "PrintOnAllPages", PrintOnAllPages, false, false);
		return val;
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
				propertiesHelper.MaxHeight(),
				propertiesHelper.MinHeight()
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
				propertiesHelper.KeepFooterTogether(),
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

	public override StiComponent CreateNew()
	{
		return new StiColumnFooterBand();
	}

	public StiColumnFooterBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiColumnFooterBand(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
