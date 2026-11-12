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
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Shape;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiShape.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiShapeElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiShapeElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiShapeElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiShapeElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiShapeElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiShapeElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiShapeElement : StiElement, IStiShapeElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiTitleElement, IStiGlobalizationProvider
{
	private StiShapeTypeService shapeType = new StiRectangleShapeType();

	private float size = 1f;

	private StiBrush fill = new StiSolidBrush();

	private Color stroke = Color.Black;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiShapeElement;

	public override int ToolboxPosition => 314;

	public override string LocalizedName => StiLocalization.Get("Components", "StiShape");

	public override string HelpUrl => "user-manual/dashboards_shape.htm";

	[StiEventHide]
	[StiNonSerialized]
	public override StiClickEvent ClickEvent
	{
		get
		{
			return base.ClickEvent;
		}
		set
		{
			base.ClickEvent = value;
		}
	}

	[Browsable(false)]
	public override bool IsDefined => true;

	[Browsable(false)]
	public override bool IsQuerable => false;

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(300)]
	[Description("Specifies the interior spacing of the component.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public override StiPadding Padding
	{
		get
		{
			return base.Padding;
		}
		set
		{
			base.Padding = value;
		}
	}

	[StiCategory("Shape", StiCategorySource.PropertyCategory)]
	[StiOrder(150)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiShapeElementTypeServiceEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(StiLevel.Basic)]
	public StiShapeTypeService ShapeType
	{
		get
		{
			return shapeType;
		}
		set
		{
			if (shapeType != value)
			{
				shapeType = value;
				InvokeRefreshControl("ShapeType");
			}
		}
	}

	[StiCategory("Shape", StiCategorySource.PropertyCategory)]
	[StiOrder(160)]
	[StiSerializable]
	[DefaultValue(1f)]
	[Description("Gets or sets size of the border.")]
	[StiPropertyLevel(StiLevel.Basic)]
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
				InvokeRefreshControl("Size");
			}
		}
	}

	[StiCategory("Shape", StiCategorySource.PropertyCategory)]
	[StiOrder(140)]
	[StiSerializable]
	[Description("Gets or sets a brush to fill a component.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiBrush Fill
	{
		get
		{
			return fill;
		}
		set
		{
			if (fill != value)
			{
				fill = value;
				InvokeRefreshControl("Fill");
			}
		}
	}

	[StiCategory("Shape", StiCategorySource.PropertyCategory)]
	[StiOrder(170)]
	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets border color.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public Color Stroke
	{
		get
		{
			return stroke;
		}
		set
		{
			if (stroke != value)
			{
				stroke = value;
				InvokeRefreshControl("Stroke");
			}
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiShapeElement stiShapeElement = ((StiElement)base.Clone(cloneProperties)) as StiShapeElement;
		((IStiTitleElement)stiShapeElement).Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiShapeElement.ShapeType = ShapeType?.Clone() as StiShapeTypeService;
		return stiShapeElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		jObject.AddPropertyBrush("Fill", Fill);
		jObject.AddPropertyColor("Stroke", Stroke, Color.Black);
		jObject.AddPropertyFloat("Size", Size, 1f);
		jObject.AddPropertyJObject("ShapeType", ShapeType.SaveToJsonObject(mode));
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Fill":
				Fill = item.DeserializeBrush();
				break;
			case "Stroke":
				Stroke = item.DeserializeColor();
				break;
			case "Size":
				size = item.DeserializeFloat();
				break;
			case "ShapeType":
				ShapeType = StiShapeTypeService.CreateFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	private bool ShouldSerializePadding()
	{
		return !Padding.IsEmpty;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (propertyName == "Title.Text")
		{
			Title.Text = value;
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (propertyName == "Title.Text")
		{
			return Title.Text;
		}
		throw new ArgumentException("Property with name " + propertyName);
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		return new string[1] { "Title.Text" };
	}

	public override StiComponent CreateNew()
	{
		return new StiShapeElement();
	}

	private bool ShouldSerializeFill()
	{
		if (Fill != null && Fill is StiSolidBrush stiSolidBrush)
		{
			return !(stiSolidBrush.Color == Color.Transparent);
		}
		return true;
	}

	private bool ShouldSerializeStroke()
	{
		return Stroke != Color.Black;
	}

	public StiShapeElement()
		: this(RectangleD.Empty)
	{
	}

	public StiShapeElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		Title.Visible = false;
		Padding = new StiPadding(0.0);
	}
}
