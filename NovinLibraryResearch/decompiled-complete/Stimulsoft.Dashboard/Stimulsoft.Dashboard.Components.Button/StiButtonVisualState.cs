using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Button.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Button;

[TypeConverter(typeof(StiButtonVisualStateConverter))]
public class StiButtonVisualState : IStiButtonVisualState, IStiSerializeToCodeAsClass, ICloneable, IStiDefault
{
	private Font font;

	private StiBrush brush = new StiDefaultBrush();

	private StiBrush textBrush = new StiDefaultBrush();

	private StiSimpleBorder border;

	private StiButtonElementIconSet iconSet = new StiButtonElementIconSet();

	private StiBrush iconBrush = new StiDefaultBrush();

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (!ShouldSerializeBrush() && !ShouldSerializeTextBrush() && !ShouldSerializeIconBrush() && !ShouldSerializeBorder() && !ShouldSerializeIconSet())
			{
				return !ShouldSerializeFont();
			}
			return false;
		}
	}

	[StiSerializable]
	[DefaultValue(null)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStateFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The font used to display text in the component.")]
	[TypeConverter(typeof(StiExpressionFontConverter))]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowDefaultAndStyle)]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
				InvokeRefreshControl("Font");
			}
		}
	}

	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[TypeConverter(typeof(StiBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowDefaultAndStyle)]
	[StiDefaultStringValue("DefaultBrush")]
	public virtual StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			if (brush != value)
			{
				brush = value;
				InvokeRefreshControl("Brush");
			}
		}
	}

	[StiSerializable]
	[Description("The brush of the component, which is used to display text.")]
	[TypeConverter(typeof(StiBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowDefaultAndStyle)]
	[StiDefaultStringValue("DefaultBrush")]
	public virtual StiBrush TextBrush
	{
		get
		{
			return textBrush;
		}
		set
		{
			if (textBrush != value)
			{
				textBrush = value;
				InvokeRefreshControl("TextBrush");
			}
		}
	}

	[StiSerializable]
	[DefaultValue(null)]
	[Description("The appearance and behavior of the component border.")]
	[Editor("Stimulsoft.Base.Drawing.Design.StiStateSimpleBorderEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowDefaultAndStyle)]
	public StiSimpleBorder Border
	{
		get
		{
			return border;
		}
		set
		{
			if (border != value)
			{
				border = value;
				InvokeRefreshControl("Border");
			}
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[Description("The set of icons used in the component.")]
	public StiButtonElementIconSet IconSet
	{
		get
		{
			return iconSet;
		}
		set
		{
			if (iconSet != value)
			{
				iconSet = value;
				InvokeRefreshControl("IconSet");
			}
		}
	}

	[StiSerializable]
	[Description("The icon brush of the component.")]
	[TypeConverter(typeof(StiBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowDefaultAndStyle)]
	[StiDefaultStringValue("DefaultBrush")]
	public virtual StiBrush IconBrush
	{
		get
		{
			return iconBrush;
		}
		set
		{
			if (iconBrush != value)
			{
				iconBrush = value;
				InvokeRefreshControl("IconBrush");
			}
		}
	}

	[Browsable(false)]
	internal StiButtonElement Parent { get; set; }

	public object Clone()
	{
		StiButtonVisualState stiButtonVisualState = MemberwiseClone() as StiButtonVisualState;
		stiButtonVisualState.Border = Border?.Clone() as StiSimpleBorder;
		stiButtonVisualState.Brush = Brush?.Clone() as StiBrush;
		stiButtonVisualState.TextBrush = TextBrush?.Clone() as StiBrush;
		stiButtonVisualState.IconBrush = IconBrush?.Clone() as StiBrush;
		Font obj = Font;
		object obj2 = ((obj != null) ? obj.Clone() : null);
		stiButtonVisualState.Font = (Font)((obj2 is Font) ? obj2 : null);
		stiButtonVisualState.IconSet = IconSet?.Clone() as StiButtonElementIconSet;
		return stiButtonVisualState;
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		if (IsDefault)
		{
			return null;
		}
		JObject jObject = new JObject();
		jObject.AddPropertyBrush("Brush", Brush);
		jObject.AddPropertyBrush("TextBrush", TextBrush);
		jObject.AddPropertyBrush("IconBrush", IconBrush);
		jObject.AddPropertyFontArial10("Font", Font);
		jObject.AddPropertyBorder("Border", Border);
		if (IconSet != null && !IconSet.IsDefault)
		{
			jObject.AddPropertyJObject("IconSet", IconSet.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Brush":
				Brush = item.DeserializeBrush();
				break;
			case "TextBrush":
				TextBrush = item.DeserializeBrush();
				break;
			case "IconBrush":
				IconBrush = item.DeserializeBrush();
				break;
			case "Border":
				Border = item.DeserializeSimpleBorder();
				break;
			case "IconSet":
				IconSet.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			}
		}
	}

	private void InvokeRefreshControl(string name)
	{
		Parent?.InvokeRefreshControl(name);
	}

	private bool ShouldSerializeFont()
	{
		return Font != null;
	}

	private bool ShouldSerializeBrush()
	{
		return !(Brush is StiDefaultBrush);
	}

	private bool ShouldSerializeTextBrush()
	{
		return !(TextBrush is StiDefaultBrush);
	}

	private bool ShouldSerializeBorder()
	{
		return Border != null;
	}

	private bool ShouldSerializeIconSet()
	{
		if (IconSet != null)
		{
			return !IconSet.IsDefault;
		}
		return true;
	}

	public IStiButtonElementIconSet GetIconSet()
	{
		return IconSet;
	}

	private bool ShouldSerializeIconBrush()
	{
		return !(IconBrush is StiDefaultBrush);
	}
}
