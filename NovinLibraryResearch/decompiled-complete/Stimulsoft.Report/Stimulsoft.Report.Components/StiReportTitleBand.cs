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

[StiServiceBitmap(typeof(StiReportTitleBand), "Stimulsoft.Report.Images.Components.StiReportTitleBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiReportTitleBandV1Builder))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiPrintIfEmpty))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiReportTitleBand : StiStaticBand, IStiPrintIfEmpty
{
	public override StiComponentId ComponentId => StiComponentId.StiReportTitleBand;

	public override string HelpUrl => "user-manual/report_internals_report_bands_reporttitleband.htm";

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

	[StiNonSerialized]
	[Browsable(false)]
	public override bool CanBreak
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that the report title band is printed if data is not present.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintIfEmpty { get; set; } = true;


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

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(159, 213, 183);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(159, 213, 183);

	public override StiComponentType ComponentType
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			if (Report != null && (int)Report.EngineVersion == 2)
			{
				if (Page.TitleBeforeHeader)
				{
					return StiComponentType.Static;
				}
				return StiComponentType.Master;
			}
			return StiComponentType.Simple;
		}
	}

	public override int ToolboxPosition => 1;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority
	{
		get
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Invalid comparison between Unknown and I4
			if (Page.TitleBeforeHeader)
			{
				return -400;
			}
			if (Report != null && (int)Report.EngineVersion == 1)
			{
				return -200;
			}
			return 200;
		}
	}

	public override string LocalizedName => StiLocalization.Get("Components", "StiReportTitleBand");

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "CanBreak");
		JsonExtensions.AddPropertyBool(val, "PrintIfEmpty", PrintIfEmpty, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "PrintIfEmpty")
			{
				PrintIfEmpty = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[3]
			{
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
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

	public override bool Break(StiComponent dividedComponent, double devideFactor, ref double divLine)
	{
		divLine = 0.0;
		StiContainer stiContainer = StiComponentDivider.BreakContainer(Height, this);
		((StiContainer)dividedComponent).Components.Clear();
		((StiContainer)dividedComponent).Components.AddRange(stiContainer.Components);
		return true;
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
		return new StiReportTitleBand();
	}

	public StiComponent GetMaster()
	{
		foreach (StiComponent component in Parent.Components)
		{
			if (component is StiDataBand && !(component is StiEmptyBand))
			{
				return component;
			}
		}
		return null;
	}

	public StiReportTitleBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiReportTitleBand(RectangleD rect)
		: base(rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
