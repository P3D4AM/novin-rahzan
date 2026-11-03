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

[StiServiceBitmap(typeof(StiPageFooterBand), "Stimulsoft.Report.Images.Components.StiPageFooterBand.png")]
[StiToolbox(true)]
[StiV1Builder(typeof(StiPageFooterBandV1Builder))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiPageFooterBand : StiStaticBand, IStiPrintOnEvenOddPages
{
	public override StiComponentId ComponentId => StiComponentId.StiPageFooterBand;

	public override string HelpUrl => "user-manual/report_internals_page_bands_pagefooter_band.htm";

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

	[DefaultValue(StiPrintOnEvenOddPagesType.Ignore)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(380)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates that the component is printed on even-odd pages.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPrintOnEvenOddPagesType PrintOnEvenOddPages { get; set; }

	[Browsable(false)]
	public override Color HeaderStartColor => Color.FromArgb(206, 207, 206);

	[Browsable(false)]
	public override Color HeaderEndColor => Color.FromArgb(206, 207, 206);

	public override int ToolboxPosition => 4;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority
	{
		get
		{
			if (StiOptions.Engine.DockPageFooterToBottom)
			{
				return -300;
			}
			return 1000;
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
	public override string LocalizedName => StiLocalization.Get("Components", "StiPageFooterBand");

	public override RectangleD DisplayRectangle
	{
		get
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (!StiOptions.Engine.DockPageFooterToBottom)
			{
				return base.DisplayRectangle;
			}
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			double num2 = Page.Unit.ConvertFromHInches(FooterSize);
			return new RectangleD(Left, Top - num - num2, Width, Height + num + num2);
		}
		set
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			if (StiOptions.Engine.DockPageFooterToBottom)
			{
				double num = Page.Unit.ConvertFromHInches(HeaderSize);
				double num2 = Page.Unit.ConvertFromHInches(FooterSize);
				Left = ((RectangleD)(ref value)).Left;
				Top = ((RectangleD)(ref value)).Top + num + num2;
				Width = ((RectangleD)(ref value)).Width;
				Height = ((RectangleD)(ref value)).Height - num - num2;
			}
			else
			{
				base.DisplayRectangle = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
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
		return new StiPageFooterBand();
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

	public StiPageFooterBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiPageFooterBand(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if (StiOptions.Engine.DockPageFooterToBottom)
		{
			DockStyle = StiDockStyle.Bottom;
		}
		PlaceOnToolbox = true;
	}
}
