using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiDialogStyle : StiBaseStyle
{
	private Font font = new Font("Arial", 8f);

	public override StiComponentId ComponentId => StiComponentId.StiDialogStyle;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("Gets or sets the font for drawing this style.")]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if (font != value)
			{
				if (value == null)
				{
					value = new Font("Arial", 8f);
				}
				font = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color ForeColor { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BackColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color GlyphColor { get; set; } = Color.DimGray;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SeparatorColor { get; set; } = Color.LightGray;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedBackColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedForeColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedGlyphColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotBackColor { get; set; } = StiColorUtils.Light(Color.LightGray, (byte)15);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotForeColor { get; set; } = StiElementConsts.ForegroundColor;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotGlyphColor { get; set; } = StiElementConsts.ForegroundColor;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotSelectedBackColor { get; set; } = StiColorUtils.Light(ColorTranslator.FromHtml("#3498db"), (byte)30);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotSelectedForeColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotSelectedGlyphColor { get; set; } = Color.White;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use Font for dialog controls.")]
	public bool AllowUseFont { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use BackColor for dialog controls.")]
	public bool AllowUseBackColor { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates whether a report engine can use ForeColor for dialog controls.")]
	public bool AllowUseForeColor { get; set; } = true;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, Color.Black);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "GlyphColor", GlyphColor, Color.DimGray);
		JsonExtensions.AddPropertyColor(val, "SeparatorColor", SeparatorColor, Color.LightGray);
		JsonExtensions.AddPropertyColor(val, "SelectedBackColor", SelectedBackColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "SelectedForeColor", SelectedForeColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "SelectedGlyphColor", SelectedGlyphColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "HotBackColor", HotBackColor, StiColorUtils.Light(Color.LightGray, (byte)15));
		JsonExtensions.AddPropertyColor(val, "HotForeColor", HotForeColor, StiElementConsts.ForegroundColor);
		JsonExtensions.AddPropertyColor(val, "HotGlyphColor", HotGlyphColor, StiElementConsts.ForegroundColor);
		JsonExtensions.AddPropertyColor(val, "HotSelectedBackColor", HotSelectedBackColor, StiColorUtils.Light(ColorTranslator.FromHtml("#3498db"), (byte)30));
		JsonExtensions.AddPropertyColor(val, "HotSelectedForeColor", HotSelectedForeColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "HotSelectedGlyphColor", HotSelectedGlyphColor, Color.White);
		JsonExtensions.AddPropertyBool(val, "AllowUseFont", AllowUseFont, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseBackColor", AllowUseBackColor, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowUseForeColor", AllowUseForeColor, true, false);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "GlyphColor":
				GlyphColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SeparatorColor":
				SeparatorColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedBackColor":
				SelectedBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedForeColor":
				SelectedForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedGlyphColor":
				SelectedGlyphColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotBackColor":
				HotBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotForeColor":
				HotForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotGlyphColor":
				HotGlyphColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotSelectedBackColor":
				HotSelectedBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotSelectedForeColor":
				HotSelectedForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotSelectedGlyphColor":
				HotSelectedGlyphColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AllowUseFont":
				AllowUseFont = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseBackColor":
				AllowUseBackColor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowUseForeColor":
				AllowUseForeColor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.StyleName(),
			propertiesHelper.Description(),
			propertiesHelper.StylesCollectionName(),
			propertiesHelper.StyleConditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		value = new StiPropertyObject[1] { propertiesHelper.Font() };
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.BackColor(),
			propertiesHelper.ForeColor(),
			propertiesHelper.AllowUseBackColor(),
			propertiesHelper.AllowUseFont(),
			propertiesHelper.AllowUseForeColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Parameters, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiDialogStyle stiDialogStyle = base.Clone() as StiDialogStyle;
		object obj = Font.Clone();
		stiDialogStyle.Font = (Font)((obj is Font) ? obj : null);
		return stiDialogStyle;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.Size == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Black;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeGlyphColor()
	{
		return GlyphColor != Color.DimGray;
	}

	private bool ShouldSerializeSeparatorColor()
	{
		return SeparatorColor != Color.LightGray;
	}

	private bool ShouldSerializeSelectedBackColor()
	{
		return SelectedBackColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeSelectedForeColor()
	{
		return SelectedForeColor != Color.White;
	}

	private bool ShouldSerializeSelectedGlyphColor()
	{
		return SelectedGlyphColor != Color.White;
	}

	private bool ShouldSerializeHotBackColor()
	{
		return HotBackColor != StiColorUtils.Light(Color.LightGray, (byte)15);
	}

	private bool ShouldSerializeHotForeColor()
	{
		return HotForeColor != StiElementConsts.ForegroundColor;
	}

	private bool ShouldSerializeHotGlyphColor()
	{
		return HotGlyphColor != StiElementConsts.ForegroundColor;
	}

	private bool ShouldSerializeHotSelectedBackColor()
	{
		return HotSelectedBackColor != StiColorUtils.Light(ColorTranslator.FromHtml("#3498db"), (byte)30);
	}

	private bool ShouldSerializeHotSelectedForeColor()
	{
		return HotSelectedForeColor != Color.White;
	}

	private bool ShouldSerializeHotSelectedGlyphColor()
	{
		return HotSelectedGlyphColor != Color.White;
	}

	private bool ShouldSerializeAllowUseFont()
	{
		return !AllowUseFont;
	}

	private bool ShouldSerializeAllowUseBackColor()
	{
		return !AllowUseBackColor;
	}

	private bool ShouldSerializeAllowUseForeColor()
	{
		return !AllowUseForeColor;
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + StiScaleUI.I11, rect.Y + StiScaleUI.I5, StiScaleUI.I(29), StiScaleUI.I(30));
		Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height / 4, rectangle.Width, rectangle.Height / 2);
		if (AllowUseBackColor && SelectedBackColor != Color.Transparent)
		{
			SolidBrush val = new SolidBrush(SelectedBackColor);
			try
			{
				g.FillRectangle((Brush)(object)val, rectangle2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		g.DrawRectangle(Pens.LightGray, rectangle2);
		SolidBrush val2 = new SolidBrush(ForeColor);
		try
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.Alignment = (StringAlignment)1;
					val4.LineAlignment = (StringAlignment)1;
					g.DrawString("OK", val3, (Brush)(object)val2, (RectangleF)rectangle2, val4);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (rect.Height < StiScaleUI.I(32))
		{
			rect.Width -= StiScaleUI.I(36);
			rect.X += StiScaleUI.I(36);
		}
		else
		{
			rect.Width -= StiScaleUI.I(56);
			rect.X += StiScaleUI.I(56);
		}
		SolidBrush val5 = new SolidBrush(Color.Black);
		try
		{
			Font val6 = new Font("Arial", 8f);
			try
			{
				StringFormat val7 = new StringFormat();
				try
				{
					val7.Alignment = (StringAlignment)0;
					val7.LineAlignment = (StringAlignment)1;
					g.DrawString(Name, val6, (Brush)(object)val5, (RectangleF)rect, val7);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width -= 2;
		rect.Height -= 3;
		DrawStyle(g, rect, paintValue, paintImage);
		Pen val = new Pen(Color.FromArgb(150, Color.Gray));
		try
		{
			val.DashStyle = (DashStyle)1;
			g.DrawRectangle(val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (rect.Width < 20)
		{
			rect.X += 4;
			rect.Y += 3;
			rect.Width -= 7;
			rect.Height -= 5;
			Brush val2 = (Brush)new SolidBrush(ForeColor);
			try
			{
				g.FillRectangle(val2, rect);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
		GetStyleFromComponent(component, styleElements, null);
	}

	internal void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements, StiBaseStyle componentStyle)
	{
		if (!(component is StiReportControl) && !(component is StiForm))
		{
			return;
		}
		StiDialogStyle stiDialogStyle = componentStyle as StiDialogStyle;
		if (stiDialogStyle != null)
		{
			return;
		}
		StiForm stiForm = component as StiForm;
		StiReportControl stiReportControl = component as StiReportControl;
		if ((styleElements & StiStyleElements.Font) > (StiStyleElements)0)
		{
			if (stiForm != null)
			{
				object obj = stiForm.Font.Clone();
				Font = (Font)((obj is Font) ? obj : null);
			}
			else
			{
				object obj2 = stiReportControl.Font.Clone();
				Font = (Font)((obj2 is Font) ? obj2 : null);
			}
			AllowUseFont = true;
		}
		if ((styleElements & StiStyleElements.Brush) > (StiStyleElements)0)
		{
			BackColor = stiForm?.BackColor ?? stiReportControl.BackColor;
			AllowUseBackColor = true;
		}
		if ((styleElements & StiStyleElements.TextBrush) > (StiStyleElements)0)
		{
			AllowUseForeColor = true;
		}
	}

	public override void SetStyleToComponent(StiComponent component)
	{
		if (component is StiReportControl stiReportControl && StiStyleConditionHelper.IsAllowStyle(component, this))
		{
			if (component is IStiFont && AllowUseFont)
			{
				object obj = Font.Clone();
				stiReportControl.Font = (Font)((obj is Font) ? obj : null);
			}
			if (AllowUseBackColor)
			{
				stiReportControl.BackColor = BackColor;
			}
			if (AllowUseForeColor)
			{
				stiReportControl.ForeColor = ForeColor;
			}
		}
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiDialogStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiDialogStyle");
	}

	internal StiDialogStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
	}//IL_000b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Expected O, but got Unknown


	public StiDialogStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiDialogStyle(string name)
		: this(name, "")
	{
	}

	public StiDialogStyle()
		: this("")
	{
	}
}
