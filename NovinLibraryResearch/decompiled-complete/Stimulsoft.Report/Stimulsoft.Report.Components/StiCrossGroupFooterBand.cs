using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiCrossGroupFooterBand), "Stimulsoft.Report.Images.Components.StiCrossGroupFooterBand.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catCrossBands.png")]
[StiGdiPainter(typeof(StiCrossBandGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCrossBandWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiCrossGroupFooterBand : StiGroupFooterBand
{
	public override StiComponentId ComponentId => StiComponentId.StiCrossGroupFooterBand;

	public override string HelpUrl => "user-manual/report_internals_bands_band_types_cross_bands.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public override bool NewPageBefore
	{
		get
		{
			return base.NewPageBefore;
		}
		set
		{
			base.NewPageBefore = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool NewPageAfter
	{
		get
		{
			return base.NewPageAfter;
		}
		set
		{
			base.NewPageAfter = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool NewColumnBefore
	{
		get
		{
			return base.NewColumnBefore;
		}
		set
		{
			base.NewColumnBefore = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool NewColumnAfter
	{
		get
		{
			return base.NewColumnAfter;
		}
		set
		{
			base.NewColumnAfter = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool SkipFirst
	{
		get
		{
			return base.SkipFirst;
		}
		set
		{
			base.SkipFirst = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override float BreakIfLessThan
	{
		get
		{
			return base.BreakIfLessThan;
		}
		set
		{
			base.BreakIfLessThan = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool GrowToHeight
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
	[StiNonSerialized]
	public override bool ResetPageNumber
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool PrintAtBottom
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
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

	public override int ToolboxPosition => 22;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Cross;

	public override string LocalizedCategory => StiLocalization.Get("Report", "CrossBands");

	public override string LocalizedName => StiLocalization.Get("Components", "StiCrossGroupFooterBand");

	public override bool IsCross => true;

	public override int Priority => 300;

	[StiNonSerialized]
	[Browsable(false)]
	public override StiDockStyle DockStyle
	{
		get
		{
			return StiDockStyle.Left;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override SizeD MinSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MinSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MinSize = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override SizeD MaxSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MaxSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MaxSize = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double MaxHeight
	{
		get
		{
			return base.MaxHeight;
		}
		set
		{
			base.MaxHeight = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double MinHeight
	{
		get
		{
			return base.MinHeight;
		}
		set
		{
			base.MinHeight = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(500)]
	[Description("Gets or sets minimal width of band.")]
	[DefaultValue(0.0)]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double MinWidth
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = base.MinSize;
			return ((SizeD)(ref minSize)).Width;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = base.MinSize;
			if (((SizeD)(ref minSize)).Width != value)
			{
				base.MinSize = new SizeD(value, 0.0);
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(510)]
	[Description("Gets or sets maximal width of band.")]
	[DefaultValue(0.0)]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double MaxWidth
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = base.MaxSize;
			return ((SizeD)(ref maxSize)).Width;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = base.MaxSize;
			if (((SizeD)(ref maxSize)).Width != value)
			{
				base.MaxSize = new SizeD(value, 0.0);
			}
		}
	}

	[Browsable(true)]
	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			base.Width = value;
		}
	}

	[Browsable(false)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override double Height
	{
		get
		{
			return base.Height;
		}
		set
		{
			base.Height = value;
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 30.0, 50.0);

	public override RectangleD SelectRectangle
	{
		get
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			return new RectangleD(Left, Top, Width, Height + num);
		}
		set
		{
			double num = Page.Unit.ConvertFromHInches(HeaderSize);
			Left = ((RectangleD)(ref value)).Left;
			Top = ((RectangleD)(ref value)).Top;
			Width = ((RectangleD)(ref value)).Width;
			Height = ((RectangleD)(ref value)).Height - num;
		}
	}

	[Browsable(false)]
	public override RectangleD DisplayRectangle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return SelectRectangle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			SelectRectangle = value;
		}
	}

	public override double HeaderSize
	{
		get
		{
			if (Report.Info.ShowHeaders)
			{
				return StiAlignValue.AlignToMaxGrid(15.0, Page.Unit.ConvertToHInches(Page.GridSize), true) - 4.0;
			}
			return 0.0;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "NewPageBefore");
		JsonExtensions.RemoveProperty(val, "NewPageAfter");
		JsonExtensions.RemoveProperty(val, "NewColumnBefore");
		JsonExtensions.RemoveProperty(val, "NewColumnAfter");
		JsonExtensions.RemoveProperty(val, "SkipFirst");
		JsonExtensions.RemoveProperty(val, "BreakIfLessThan");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "ResetPageNumber");
		JsonExtensions.RemoveProperty(val, "PrintAtBottom");
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.RemoveProperty(val, "DockStyle");
		JsonExtensions.RemoveProperty(val, "MinSize");
		JsonExtensions.RemoveProperty(val, "MaxSize");
		JsonExtensions.RemoveProperty(val, "MaxHeight");
		JsonExtensions.RemoveProperty(val, "MinHeight");
		JsonExtensions.AddPropertyDouble(val, "MinWidth", MinWidth, 0.0);
		JsonExtensions.AddPropertyDouble(val, "MaxWidth", MaxWidth, 0.0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "MinWidth"))
			{
				if (text == "MaxWidth")
				{
					MaxWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
				}
			}
			else
			{
				MinWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[1] { propertiesHelper.Width() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[3]
			{
				propertiesHelper.Width(),
				propertiesHelper.MinWidth(),
				propertiesHelper.MaxWidth()
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
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[6]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepGroupFooterTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepGroupFooterTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn()
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
		return new StiCrossGroupFooterBand();
	}

	public StiCrossGroupFooterBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCrossGroupFooterBand(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
