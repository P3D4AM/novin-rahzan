using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiPanel), "Stimulsoft.Report.Images.Components.StiPanel.png")]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiEngine(/*Could not decode attribute arguments.*/)]
public class StiPanel : StiContainer
{
	private bool rightToLeft;

	private double columnGaps;

	private double columnWidth;

	private int columns;

	public override StiComponentId ComponentId => StiComponentId.StiPanel;

	public override string HelpUrl => "user-manual/report_internals_panels.htm";

	[Browsable(true)]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
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

	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets horizontal column direction.")]
	[StiOrder(150)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual bool RightToLeft
	{
		get
		{
			return rightToLeft;
		}
		set
		{
			if (rightToLeft != value)
			{
				CheckBlockedException("RightToLeft");
				rightToLeft = value;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0.0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets or sets distance between two columns.")]
	public virtual double ColumnGaps
	{
		get
		{
			return columnGaps;
		}
		set
		{
			if (columnGaps != value)
			{
				CheckBlockedException("ColumnGaps");
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException("ColumnGaps", $"Value of '{value}' is not valid for 'ColumnGaps'. " + "'ColumnGaps' must be greater than or equal to 0.");
				}
				columnGaps = Math.Round(value, 2);
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0.0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets width of column.")]
	public virtual double ColumnWidth
	{
		get
		{
			return columnWidth;
		}
		set
		{
			if (columnWidth != value)
			{
				CheckBlockedException("ColumnWidth");
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException("ColumnWidth", $"Value of '{value}' is not valid for 'ColumnWidth'. " + "'ColumnWidth' must be greater than or equal to 0.");
				}
				columnWidth = Math.Round(value, 2);
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets columns count.")]
	public virtual int Columns
	{
		get
		{
			return columns;
		}
		set
		{
			if (columns != value)
			{
				CheckBlockedException("Columns");
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("Columns", $"Value of '{value}' is not valid for 'Columns'. " + "'Columns' must be greater than or equal to 0.");
				}
				columns = value;
			}
		}
	}

	public override int Priority => 0;

	public override int ToolboxPosition => 110;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiPanel");

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "RightToLeft", RightToLeft, false, false);
		JsonExtensions.AddPropertyDouble(val, "ColumnGaps", ColumnGaps, 0.0);
		JsonExtensions.AddPropertyDouble(val, "ColumnWidth", ColumnWidth, 0.0);
		JsonExtensions.AddPropertyInt(val, "Columns", Columns, 0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "RightToLeft":
				rightToLeft = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ColumnGaps":
				columnGaps = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "ColumnWidth":
				columnWidth = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Columns":
				columns = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Invalid comparison between Unknown and I4
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Columns, new StiPropertyObject[4]
		{
			propertiesHelper.Columns(),
			propertiesHelper.ColumnWidth(),
			propertiesHelper.ColumnGaps(),
			propertiesHelper.RightToLeft()
		});
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
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
				propertiesHelper.GrowToHeight(),
				propertiesHelper.Enabled()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.CanBreak(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.CanBreak(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
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

	public virtual double GetColumnWidth()
	{
		double num = ColumnWidth;
		if (num == 0.0)
		{
			if (Columns == 0)
			{
				return Width;
			}
			num = Width / (double)Columns - ColumnGaps;
		}
		return num;
	}

	public override StiComponent CreateNew()
	{
		return new StiPanel();
	}

	public StiPanel()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiPanel(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
