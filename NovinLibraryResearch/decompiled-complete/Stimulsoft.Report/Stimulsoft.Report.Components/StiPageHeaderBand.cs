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
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiPageHeaderBand), "Stimulsoft.Report.Images.Components.StiPageHeaderBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiPageHeaderBandV1Builder))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiPageHeaderBand : StiStaticBand, IStiPrintOnEvenOddPages
{
	public override StiComponentId ComponentId => StiComponentId.StiPageHeaderBand;

	public override string HelpUrl => "user-manual/report_internals_page_bands_pageheader_band.htm";

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

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(206, 207, 206);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(206, 207, 206);

	public override int ToolboxPosition => 3;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority
	{
		get
		{
			if (Page.TitleBeforeHeader)
			{
				return -200;
			}
			return -400;
		}
	}

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
			return StiComponentType.Simple;
		}
	}

	[Browsable(false)]
	public override string LocalizedName => StiLocalization.Get("Components", "StiPageHeaderBand");

	[DefaultValue(StiPrintOnEvenOddPagesType.Ignore)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(380)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates that the component is printed on even-odd pages.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPrintOnEvenOddPagesType PrintOnEvenOddPages { get; set; }

	[DefaultValue(true)]
	[StiNonSerialized]
	[Browsable(false)]
	public bool PrintOnFirstPage
	{
		get
		{
			return (PrintOn & StiPrintOnType.ExceptFirstPage) == 0;
		}
		set
		{
			if (value)
			{
				if ((PrintOn & StiPrintOnType.ExceptFirstPage) > StiPrintOnType.AllPages)
				{
					PrintOn--;
				}
			}
			else
			{
				PrintOn |= StiPrintOnType.ExceptFirstPage;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanBreak");
		JsonExtensions.AddPropertyEnum(val, "PrintOnEvenOddPages", (Enum)PrintOnEvenOddPages, (Enum)StiPrintOnEvenOddPagesType.Ignore);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "PrintOnEvenOddPages")
			{
				PrintOnEvenOddPages = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnEvenOddPagesType>(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Invalid comparison between Unknown and I4
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
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
		return new StiPageHeaderBand();
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

	public StiPageHeaderBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiPageHeaderBand(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
