using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiTableStyle : StiBaseStyle
{
	public override StiComponentId ComponentId => StiComponentId.StiTableStyle;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BackColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color DataColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color DataForeground { get; set; } = ColorTranslator.FromHtml("#222");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedDataColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedDataForeground { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color AlternatingDataColor { get; set; } = ColorTranslator.FromHtml("#eee");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color AlternatingDataForeground { get; set; } = ColorTranslator.FromHtml("#222");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HeaderColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HeaderForeground { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotHeaderColor { get; set; } = Color.Transparent;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color FooterColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color FooterForeground { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color GridColor { get; set; } = Color.Gainsboro;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.Transparent);
		JsonExtensions.AddPropertyColor(val, "DataColor", DataColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "DataForeground", DataForeground, "#222");
		JsonExtensions.AddPropertyColor(val, "SelectedDataColor", SelectedDataColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "SelectedDataForeground", SelectedDataForeground, Color.White);
		JsonExtensions.AddPropertyColor(val, "AlternatingDataColor", AlternatingDataColor, "#eee");
		JsonExtensions.AddPropertyColor(val, "AlternatingDataForeground", AlternatingDataForeground, "#222");
		JsonExtensions.AddPropertyColor(val, "HeaderColor", HeaderColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "HeaderForeground", HeaderForeground, Color.White);
		JsonExtensions.AddPropertyColor(val, "HotHeaderColor", HotHeaderColor, Color.Transparent);
		JsonExtensions.AddPropertyColor(val, "FooterColor", FooterColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "FooterForeground", FooterForeground, Color.Black);
		JsonExtensions.AddPropertyColor(val, "GridColor", GridColor, Color.Black);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "DataColor":
				DataColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "DataForeground":
				DataForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedDataColor":
				SelectedDataColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedDataForeground":
				SelectedDataForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AlternatingDataColor":
				AlternatingDataColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AlternatingDataForeground":
				AlternatingDataForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HeaderColor":
				HeaderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HeaderForeground":
				HeaderForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotHeaderColor":
				HotHeaderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "FooterColor":
				FooterColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "FooterForeground":
				FooterForeground = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "GridColor":
				GridColor = StiJsonReportObjectHelper.DeserializeColor(item);
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
			propertiesHelper.StyleCollectionName(),
			propertiesHelper.StyleConditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.HeaderColor(),
			propertiesHelper.HeaderForeground(),
			propertiesHelper.FooterForeground(),
			propertiesHelper.DataColor(),
			propertiesHelper.DataForeground(),
			propertiesHelper.GridColor()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeDataColor()
	{
		return DataColor != Color.White;
	}

	private bool ShouldSerializeDataForeground()
	{
		return DataForeground != ColorTranslator.FromHtml("#222");
	}

	private bool ShouldSerializeSelectedDataColor()
	{
		return SelectedDataColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeSelectedDataForeground()
	{
		return SelectedDataForeground != Color.White;
	}

	private bool ShouldSerializeAlternatingDataColor()
	{
		return AlternatingDataColor != ColorTranslator.FromHtml("#eee");
	}

	private bool ShouldSerializeAlternatingDataForeground()
	{
		return AlternatingDataForeground != ColorTranslator.FromHtml("#222");
	}

	private bool ShouldSerializeHeaderColor()
	{
		return HeaderColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeHeaderForeground()
	{
		return HeaderForeground != Color.White;
	}

	private bool ShouldSerializeHotHeaderColor()
	{
		return HotHeaderColor != Color.Transparent;
	}

	private bool ShouldSerializeFooterColor()
	{
		return FooterColor != Color.White;
	}

	private bool ShouldSerializeFooterForeground()
	{
		return FooterForeground != Color.Black;
	}

	private bool ShouldSerializeGridColor()
	{
		return GridColor != Color.Gainsboro;
	}

	private Color GetColor(Color color)
	{
		if (!(color == Color.White))
		{
			return color;
		}
		return Color.FromArgb(180, 255, 255, 255);
	}

	public void DrawStyleForGallery(Graphics g, Rectangle rect)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + StiScale.XXI(5.0), rect.Y + StiScale.YYI(5.0), rect.Width - StiScale.XXI(10.0), rect.Height - StiScale.YYI(10.0));
		int num = rectangle.Height / 5;
		if (num * 5 > rectangle.Height)
		{
			num--;
		}
		rectangle.Height = num * 5;
		int num2 = rectangle.Width / num;
		rectangle.Width = num * num2;
		SolidBrush val = new SolidBrush(GetColor(DataColor));
		try
		{
			g.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(GetColor(GridColor), (float)(1.0 * StiScale.Factor));
		try
		{
			for (int i = 0; i < 6; i++)
			{
				g.DrawLine(val2, rectangle.X, rectangle.Y + num * i, rectangle.Right, rectangle.Y + num * i);
			}
			for (int j = 0; j <= num2; j++)
			{
				g.DrawLine(val2, rectangle.X + num * j, rectangle.Y, rectangle.X + num * j, rectangle.Bottom);
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		SolidBrush val3 = new SolidBrush(GetColor(HeaderColor));
		try
		{
			g.FillRectangle((Brush)(object)val3, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, num));
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		SolidBrush val4 = new SolidBrush(GetColor(FooterColor));
		try
		{
			g.FillRectangle((Brush)(object)val4, new Rectangle(rectangle.X, rectangle.Bottom - num, rectangle.Width, num));
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		Rectangle rectangle = rect;
		if (rect.Width == StiScaleUI.I(80) && rect.Height == StiScaleUI.I(80))
		{
			rectangle = new Rectangle(rect.X + StiScale.XXI(9.0), rect.Y + StiScale.YYI(10.0), StiScale.XXI(61.0), StiScale.YYI(60.0));
		}
		if (paintImage)
		{
			int num = rectangle.X;
			int num2 = rectangle.Y;
			int num3;
			int num4;
			int num5;
			if (rect.Width == StiScale.XXI(18.0) && rect.Height == StiScale.YYI(10.0))
			{
				num3 = StiScale.XXI(6.0);
				num4 = StiScale.YYI(3.0);
				num5 = num3 * 3;
			}
			else if (rectangle.Height == StiScaleUI.I(14) || (rect.Height >= StiScale.I(10) && rect.Height <= StiScale.I(18)))
			{
				num += StiScale.XXI(1.0);
				num2 += StiScale.YYI(1.0);
				num3 = StiScale.XXI(8.0);
				num4 = StiScale.YYI(3.0);
				num5 = num3 * 3;
			}
			else
			{
				num += StiScale.XXI(11.0);
				num2 += StiScale.YYI(5.0);
				num3 = StiScale.XXI(10.0);
				num4 = StiScale.YYI(6.0);
				num5 = num3 * 3 - StiScale.XXI(1.0);
			}
			SolidBrush val = new SolidBrush(HeaderColor);
			try
			{
				g.FillRectangle((Brush)(object)val, num, num2, num5, num4);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			SolidBrush val2 = new SolidBrush(FooterColor);
			try
			{
				g.FillRectangle((Brush)(object)val2, num, num2 + num4 * 3, num5, num4);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			SolidBrush val3 = new SolidBrush(DataColor);
			try
			{
				Pen val4 = new Pen(GridColor);
				try
				{
					for (int i = 0; i < 3; i++)
					{
						int num6 = num + num3 * i;
						int num7 = num3;
						if (i == 2)
						{
							num7--;
						}
						g.FillRectangle((Brush)(object)val3, num6, num2 + num4, num7, num4);
						g.DrawRectangle(val4, num6, num2 + num4, num7, num4);
						g.FillRectangle((Brush)(object)val3, num6, num2 + num4 * 2, num7, num4);
						g.DrawRectangle(val4, num6, num2 + num4 * 2, num7, num4);
					}
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
		if (paintValue)
		{
			return;
		}
		if (rect.Height < StiScale.YYI(32.0))
		{
			rect.Width -= StiScale.XXI(36.0);
			rect.X += StiScale.XXI(36.0);
		}
		else
		{
			rect.Width -= StiScale.XXI(56.0);
			rect.X += StiScale.XXI(56.0);
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
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
	}

	public override void SetStyleToComponent(StiComponent component)
	{
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiTableStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiTableStyle");
	}

	internal StiTableStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
	}

	public StiTableStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiTableStyle(string name)
		: this(name, "")
	{
	}

	public StiTableStyle()
		: this("")
	{
	}
}
