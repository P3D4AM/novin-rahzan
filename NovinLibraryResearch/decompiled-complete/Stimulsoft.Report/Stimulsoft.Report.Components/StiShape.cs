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
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiShape), "Stimulsoft.Report.Images.Components.StiShape.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiShape.png")]
[StiGdiPainter(typeof(StiShapeGdiPainter))]
[StiDesigner("Stimulsoft.Report.Components.Design.StiShapeDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiShapeWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfShapeDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiGrowToHeight))]
public class StiShape : StiComponent, IStiBrush, IStiBorderColor, IStiForeColor, IStiFont, IStiTextHorAlignment, IStiVertAlignment, IStiExportImageExtended, IStiExportImage, IStiShape
{
	private Font font = new Font("Arial", 8f);

	private float size = 1f;

	public override StiComponentId ComponentId => StiComponentId.StiShape;

	public override string HelpUrl => "user-manual/report_internals_primitives.htm?zoom_highlightsub=Shape";

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
	public virtual StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BorderColor { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[DefaultValue(null)]
	[StiSerializable]
	[Description("Gets or sets text.")]
	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string Text { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(210)]
	[StiSerializable]
	[Description("Gets or sets font of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (value != null || !base.IsDesigning)
			{
				font = value;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(250)]
	[Description("Gets or sets the text horizontal alignment.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiTextHorAlignment HorAlignment { get; set; } = (StiTextHorAlignment)1;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(260)]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiVertAlignment VertAlignment { get; set; } = (StiVertAlignment)1;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[StiSerializable]
	[Description("Gets or sets a color to draw text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual Color ForeColor { get; set; } = Color.Black;


	public override int ToolboxPosition => 108;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Shapes;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Shapes");

	public override string LocalizedName => StiLocalization.Get("Components", "StiShape");

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(240)]
	[StiSerializable]
	[Description("Gets or sets the text background.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual Color BackgroundColor { get; set; } = Color.Transparent;


	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 64.0, 64.0);

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	[Description("Gets or sets text margins.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMargins Margins { get; set; } = new StiMargins();


	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a pen style.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiPenStyle Style { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[StiSerializable]
	[DefaultValue(1f)]
	[Description("Gets or sets size of the border.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float Size
	{
		get
		{
			return size;
		}
		set
		{
			if (value != size)
			{
				if (value < 1f)
				{
					size = 1f;
				}
				else
				{
					size = value;
				}
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.ShapeTypes.Design.StiShapeTypeServiceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets type of the shape.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiShapeTypeService ShapeType { get; set; } = new StiRectangleShapeType();


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Black);
		JsonExtensions.AddPropertyEnum(val, "Style", (Enum)(object)Style, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyFloat(val, "Size", Size, 1f);
		JsonExtensions.AddPropertyJObject(val, "ShapeType", ShapeType.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyString(val, "Text", Text);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, Color.Black);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiTextHorAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)1);
		JsonExtensions.AddPropertyJObject(val, "Margins", Margins.SaveToJsonObject(0.0, 0.0, 0.0, 0.0));
		JsonExtensions.AddPropertyColor(val, "BackgroundColor", BackgroundColor, Color.Transparent);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackgroundColor":
				BackgroundColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, font);
				break;
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiTextHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiVertAlignment>(item);
				break;
			case "Margins":
			{
				StiMargins stiMargins = new StiMargins();
				stiMargins.LoadFromJsonObject((JObject)item.Value);
				Margins = stiMargins;
				break;
			}
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Style":
				Style = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Size":
				size = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "ShapeType":
				ShapeType = StiShapeTypeService.CreateFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Invalid comparison between Unknown and I4
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.ShapeEditor() });
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
			stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[1] { propertiesHelper.TextStr() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Text, new StiPropertyObject[7]
			{
				propertiesHelper.TextStr(),
				propertiesHelper.Font(),
				propertiesHelper.Margins(),
				propertiesHelper.ForeColor(),
				propertiesHelper.BackgroundColor(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[3]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[4]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.GrowToHeight(),
				propertiesHelper.Enabled()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.InteractionEditor(),
				propertiesHelper.AnchorMode(),
				propertiesHelper.GrowToHeight(),
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

	public override object Clone(bool cloneProperties)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		StiShape stiShape = (StiShape)base.Clone(cloneProperties);
		if (ShapeType != null)
		{
			stiShape.ShapeType = (StiShapeTypeService)((StiService)ShapeType).Clone();
		}
		else
		{
			stiShape.ShapeType = null;
		}
		if (Brush != null)
		{
			stiShape.Brush = (StiBrush)Brush.Clone();
		}
		else
		{
			stiShape.Brush = null;
		}
		return stiShape;
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		base.Convert(oldUnit, newUnit, isReportSnapshot);
		if (ShapeType is StiOctagonShapeType)
		{
			(ShapeType as StiOctagonShapeType).Bevel = (float)newUnit.ConvertFromHInches(oldUnit.ConvertToHInches((double)(ShapeType as StiOctagonShapeType).Bevel));
		}
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

	private bool ShouldSerializeFont()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Font != null && Font.Name == "Arial" && (int)font.Style == 0)
		{
			return font.Size != 8f;
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Black;
	}

	internal string GetParsedText()
	{
		if (string.IsNullOrEmpty(Text) || !Text.Contains("{"))
		{
			return Text;
		}
		return StiReportParser.Parse(Text, (IStiReportComponent)(object)this);
	}

	public override StiComponent CreateNew()
	{
		return new StiShape();
	}

	private bool ShouldSerializeBackgroundColor()
	{
		return BackgroundColor != Color.Transparent;
	}

	private bool ShouldSerializeMargins()
	{
		return !Margins.IsDefault;
	}

	public StiShape()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiShape(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
