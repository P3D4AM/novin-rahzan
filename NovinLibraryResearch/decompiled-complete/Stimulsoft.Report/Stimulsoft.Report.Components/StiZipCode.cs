using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiZipCode), "Stimulsoft.Report.Images.Components.StiZipCode.png")]
[StiGdiPainter(typeof(StiZipCodeGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiZipCodeWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiContextTool(typeof(IStiCanShrink))]
public class StiZipCode : StiComponent, IStiBorder, IStiExportImageExtended, IStiExportImage, IStiBrush
{
	private static readonly object EventGetZipCode = new object();

	public override StiComponentId ComponentId => StiComponentId.StiZipCode;

	public override string HelpUrl => "User-Manual/report_internals_output_text_parameters_zip_code.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("Gets or sets border of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	public override int ToolboxPosition => 116;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiZipCode");

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the component zip code.")]
	public string CodeValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a code of zip code.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiZipCodeExpression Code
	{
		get
		{
			return new StiZipCodeExpression(this, "Code");
		}
		set
		{
			value?.Set(this, "Code", value.Value);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the code of zip code.")]
	public StiGetZipCodeEvent GetZipCodeEvent
	{
		get
		{
			return new StiGetZipCodeEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 200.0, 32.0);

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Get or sets value, which indicates width and height ratio.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Ratio { get; set; } = true;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a fore color.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public Color ForeColor { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[DefaultValue(4.0)]
	[Description("Gets or sets a space ratio.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public double SpaceRatio { get; set; } = 4.0;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether it is necessary to draw the upper marks.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool UpperMarks { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets a contour size.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public double Size { get; set; } = 1.0;


	public event StiValueEventHandler GetZipCode
	{
		add
		{
			base.Events.AddHandler(EventGetZipCode, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetZipCode, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "CodeValue", CodeValue);
		JsonExtensions.AddPropertyJObject(val, "Code", Code.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetZipCodeEvent", GetZipCodeEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "Ratio", Ratio, true, false);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, Color.Black);
		JsonExtensions.AddPropertyDouble(val, "SpaceRatio", SpaceRatio, 4.0);
		JsonExtensions.AddPropertyBool(val, "UpperMarks", UpperMarks, false, false);
		JsonExtensions.AddPropertyDouble(val, "Size", Size, 1.0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "CodeValue":
				CodeValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Code":
			{
				StiZipCodeExpression stiZipCodeExpression = new StiZipCodeExpression();
				stiZipCodeExpression.LoadFromJsonObject((JObject)item.Value);
				Code = stiZipCodeExpression;
				break;
			}
			case "GetZipCodeEvent":
			{
				StiGetZipCodeEvent stiGetZipCodeEvent = new StiGetZipCodeEvent();
				stiGetZipCodeEvent.LoadFromJsonObject((JObject)item.Value);
				GetZipCodeEvent = stiGetZipCodeEvent;
				break;
			}
			case "Ratio":
				Ratio = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SpaceRatio":
				SpaceRatio = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "UpperMarks":
				UpperMarks = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Invalid comparison between Unknown and I4
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.ZipCode, new StiPropertyObject[4]
			{
				propertiesHelper.Code(),
				propertiesHelper.Ratio(),
				propertiesHelper.Size(),
				propertiesHelper.UpperMarks()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.ZipCode, new StiPropertyObject[5]
			{
				propertiesHelper.Code(),
				propertiesHelper.Ratio(),
				propertiesHelper.Size(),
				propertiesHelper.SpaceRatio(),
				propertiesHelper.UpperMarks()
			});
		}
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
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.ForeColor()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[6]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.ForeColor(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetZipCodeEvent
		});
		return stiEventCollection;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return true;
	}

	public override object Clone(bool cloneProperties)
	{
		StiZipCode stiZipCode = (StiZipCode)base.Clone(cloneProperties);
		StiBorder border = Border;
		object obj = ((border != null) ? border.Clone() : null);
		stiZipCode.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		StiBrush brush = Brush;
		object obj2 = ((brush != null) ? brush.Clone() : null);
		stiZipCode.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		return stiZipCode;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	public RectangleD CalculateRect(RectangleD rect, int count, int index, out RectangleD markRect)
	{
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.0;
		double num2 = 0.0;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		double num6 = 0.0;
		if (StiOptions.Engine.UseOldZipCodePaintMode)
		{
			if (!Ratio)
			{
				num2 = 5.0 * ((RectangleD)(ref rect)).Width / (double)(6 * count - 1);
				num3 = num2 / 5.0;
				num4 = ((RectangleD)(ref rect)).Height;
				num5 = ((RectangleD)(ref rect)).X + num3 * (double)index + num2 * (double)index;
				num6 = ((RectangleD)(ref rect)).Y;
			}
			else
			{
				num3 = ((RectangleD)(ref rect)).Height / 10.0;
				num4 = ((RectangleD)(ref rect)).Height;
				num2 = num4 / 2.0;
				num5 = ((RectangleD)(ref rect)).X + num3 * (double)index + num2 * (double)index;
				num6 = ((RectangleD)(ref rect)).Y;
			}
			markRect = new RectangleD(0.0, 0.0, 0.0, 0.0);
		}
		else if (!Ratio)
		{
			num = ((RectangleD)(ref rect)).Height / (double)(12 + (UpperMarks ? 3 : 0));
			double num7 = ((RectangleD)(ref rect)).Width / ((5.0 + SpaceRatio) * (double)count - SpaceRatio + 2.0);
			num2 = num7 * 5.0;
			num3 = num7 * SpaceRatio;
			num4 = num * 10.0;
			num5 = ((RectangleD)(ref rect)).X + num7 + (num2 + num3) * (double)index;
			num6 = ((RectangleD)(ref rect)).Y + num * (double)((!UpperMarks) ? 1 : 4);
			markRect = new RectangleD(num5 - num7, num6 - num * 4.0, num7 * 7.0, num * 2.0);
		}
		else
		{
			num = ((RectangleD)(ref rect)).Height / (double)(12 + (UpperMarks ? 3 : 0));
			num4 = num * 10.0;
			num2 = num * 5.0;
			num3 = num * SpaceRatio;
			num5 = ((RectangleD)(ref rect)).X + num + (num2 + num3) * (double)index;
			num6 = ((RectangleD)(ref rect)).Y + num * (double)((!UpperMarks) ? 1 : 4);
			markRect = new RectangleD(num5 - num, num6 - num * 4.0, num * 7.0, num * 2.0);
		}
		RectangleD result = default(RectangleD);
		((RectangleD)(ref result))._002Ector(num5, num6, num2, num4);
		return result;
	}

	public override void InvokeEvents()
	{
		base.InvokeEvents();
		try
		{
			if ((object)base.Events[EventGetZipCode] != null)
			{
				StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
				InvokeGetZipCode(this, stiValueEventArgs);
				if (stiValueEventArgs.Value != null)
				{
					CodeValue = stiValueEventArgs.Value.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	protected virtual void OnGetZipCode(StiValueEventArgs e)
	{
	}

	public void InvokeGetZipCode(StiComponent sender, StiValueEventArgs e)
	{
		try
		{
			OnGetZipCode(e);
			if (base.Events[EventGetZipCode] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetZipCodeEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in ZipCode property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Black;
	}

	public override StiComponent CreateNew()
	{
		return new StiZipCode();
	}

	public StiZipCode()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiZipCode(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
		Code.Value = "1234567890";
		if (StiOptions.Engine.UseOldZipCodePaintMode)
		{
			SpaceRatio = 1.0;
		}
	}
}
