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

[StiServiceBitmap(typeof(StiChildBand), "Stimulsoft.Report.Images.Components.StiChildBand.png")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiKeepChildTogether))]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiSelectAllQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfSelectAllQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiChildBand : StiDynamicBand, IStiKeepChildTogether
{
	private bool keepChildTogether = true;

	private bool printIfParentDisabled;

	public override StiComponentId ComponentId => StiComponentId.StiChildBand;

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
	[Description("Gets or sets value indicates that childs are to be kept together.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool KeepChildTogether
	{
		get
		{
			return keepChildTogether;
		}
		set
		{
			if (keepChildTogether != value)
			{
				CheckBlockedException("KeepChildTogether");
				keepChildTogether = value;
			}
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public override StiPointerExpression Pointer
	{
		get
		{
			return base.Pointer;
		}
		set
		{
			base.Pointer = value;
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public override StiBookmarkExpression Bookmark
	{
		get
		{
			return base.Bookmark;
		}
		set
		{
			base.Bookmark = value;
		}
	}

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public override StiHyperlinkExpression Hyperlink
	{
		get
		{
			return base.Hyperlink;
		}
		set
		{
			base.Hyperlink = value;
		}
	}

	[Browsable(false)]
	public override Color HeaderStartColor
	{
		get
		{
			StiBand master = GetMaster();
			if (master == null)
			{
				return Color.White;
			}
			return StiColorUtils.Light(master.HeaderStartColor, (byte)50);
		}
	}

	[Browsable(false)]
	public override Color HeaderEndColor
	{
		get
		{
			StiBand master = GetMaster();
			if (master == null)
			{
				return Color.White;
			}
			return StiColorUtils.Light(master.HeaderEndColor, (byte)50);
		}
	}

	public override string HelpUrl => "user-manual/report_internals_child_band.htm";

	public override StiComponentType ComponentType => StiComponentType.Detail;

	public override int ToolboxPosition => 14;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Bands;

	public override int Priority => 300;

	public override string LocalizedName => StiLocalization.Get("Components", "StiChildBand");

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(350)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that if the parent band is disabled then the child band will be printed anyway.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool PrintIfParentDisabled
	{
		get
		{
			return printIfParentDisabled;
		}
		set
		{
			if (printIfParentDisabled != value)
			{
				CheckBlockedException("PrintIfParentDisabled");
				printIfParentDisabled = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "KeepChildTogether", KeepChildTogether, true, false);
		JsonExtensions.AddPropertyBool(val, "PrintIfParentDisabled", PrintIfParentDisabled, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "KeepChildTogether"))
			{
				if (text == "PrintIfParentDisabled")
				{
					printIfParentDisabled = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				keepChildTogether = StiJsonReportObjectHelper.DeserializeBool(item);
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
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Invalid comparison between Unknown and I4
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.KeepChildTogether(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.CanBreak(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintAtBottom(),
				propertiesHelper.PrintIfParentDisabled(),
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

	public override StiComponent CreateNew()
	{
		return new StiChildBand();
	}

	public StiBand GetMaster()
	{
		for (int num = Parent.Components.IndexOf(this) - 1; num >= 0; num--)
		{
			if (Parent.Components[num] is StiBand && !(Parent.Components[num] is StiChildBand))
			{
				return Parent.Components[num] as StiBand;
			}
		}
		return null;
	}

	public StiChildBand()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiChildBand(RectangleD rect)
		: base(rect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
