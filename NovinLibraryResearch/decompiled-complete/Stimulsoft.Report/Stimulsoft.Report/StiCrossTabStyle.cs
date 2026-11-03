using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiCrossTabStyle : StiBaseStyle
{
	private Color totalCellColumnBackColor = Color.Empty;

	private Color totalCellColumnForeColor = Color.Empty;

	private Color totalCellRowBackColor = Color.Empty;

	private Color totalCellRowForeColor = Color.Empty;

	public override StiComponentId ComponentId => StiComponentId.StiCrossTabStyle;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BackColor { get; set; } = Color.White;


	[StiNonSerialized]
	[Browsable(false)]
	public Color Color
	{
		get
		{
			return ColumnHeaderBackColor;
		}
		set
		{
			ColumnHeaderBackColor = value;
			RowHeaderBackColor = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color CellBackColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color AlternatingCellBackColor { get; set; } = ColorTranslator.FromHtml("#eee");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color AlternatingCellForeColor { get; set; } = StiColor.Get("#323a45");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedCellBackColor { get; set; } = StiColorUtils.Light(ColorTranslator.FromHtml("#3498db"), (byte)30);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color SelectedCellForeColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color ColumnHeaderBackColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color ColumnHeaderForeColor { get; set; } = ColorTranslator.FromHtml("#fff");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color RowHeaderBackColor { get; set; } = ColorTranslator.FromHtml("#3498db");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color RowHeaderForeColor { get; set; } = ColorTranslator.FromHtml("#eee");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotColumnHeaderBackColor { get; set; } = StiColorUtils.Dark(ColorTranslator.FromHtml("#3498db"), (byte)30);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color HotRowHeaderBackColor { get; set; } = StiColorUtils.Dark(ColorTranslator.FromHtml("#3498db"), (byte)30);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color CellForeColor { get; set; } = ColorTranslator.FromHtml("#323a45");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color LineColor { get; set; } = Color.White;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color TotalCellColumnBackColor
	{
		get
		{
			if (totalCellColumnBackColor == Color.Empty)
			{
				totalCellColumnBackColor = ColumnHeaderBackColor;
			}
			return totalCellColumnBackColor;
		}
		set
		{
			totalCellColumnBackColor = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color TotalCellColumnForeColor
	{
		get
		{
			if (totalCellColumnForeColor == Color.Empty)
			{
				totalCellColumnForeColor = ColumnHeaderForeColor;
			}
			return totalCellColumnForeColor;
		}
		set
		{
			totalCellColumnForeColor = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color TotalCellRowBackColor
	{
		get
		{
			if (totalCellRowBackColor == Color.Empty)
			{
				totalCellRowBackColor = RowHeaderBackColor;
			}
			return totalCellRowBackColor;
		}
		set
		{
			totalCellRowBackColor = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Color TotalCellRowForeColor
	{
		get
		{
			if (totalCellRowForeColor == Color.Empty)
			{
				totalCellRowForeColor = RowHeaderForeColor;
			}
			return totalCellRowForeColor;
		}
		set
		{
			totalCellRowForeColor = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.Transparent);
		JsonExtensions.AddPropertyColor(val, "CellBackColor", CellBackColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "AlternatingCellBackColor", AlternatingCellBackColor, "#eee");
		JsonExtensions.AddPropertyColor(val, "AlternatingCellForeColor", AlternatingCellForeColor, "#323a45");
		JsonExtensions.AddPropertyColor(val, "SelectedCellBackColor", SelectedCellBackColor, StiColorUtils.Light(ColorTranslator.FromHtml("#3498db"), (byte)30));
		JsonExtensions.AddPropertyColor(val, "SelectedCellForeColor", SelectedCellForeColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "ColumnHeaderBackColor", ColumnHeaderBackColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "ColumnHeaderForeColor", ColumnHeaderForeColor, "#fff");
		JsonExtensions.AddPropertyColor(val, "RowHeaderBackColor", RowHeaderBackColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "RowHeaderForeColor", RowHeaderForeColor, "#eee");
		JsonExtensions.AddPropertyColor(val, "HotColumnHeaderBackColor", HotColumnHeaderBackColor, StiColorUtils.Dark(ColorTranslator.FromHtml("#3498db"), (byte)30));
		JsonExtensions.AddPropertyColor(val, "HotRowHeaderBackColor", HotRowHeaderBackColor, StiColorUtils.Dark(ColorTranslator.FromHtml("#3498db"), (byte)30));
		JsonExtensions.AddPropertyColor(val, "CellForeColor", CellForeColor, "#323a45");
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.White);
		JsonExtensions.AddPropertyColor(val, "TotalCellColumnBackColor", TotalCellColumnBackColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "TotalCellColumnForeColor", TotalCellColumnForeColor, "#fff");
		JsonExtensions.AddPropertyColor(val, "TotalCellRowBackColor", TotalCellRowBackColor, "#3498db");
		JsonExtensions.AddPropertyColor(val, "TotalCellRowForeColor", TotalCellRowForeColor, "#eee");
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
			case "CellBackColor":
				CellBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AlternatingCellForeColor":
				AlternatingCellForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AlternatingCellBackColor":
				AlternatingCellBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedCellBackColor":
				SelectedCellBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "SelectedCellForeColor":
				SelectedCellForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ColumnHeaderBackColor":
				ColumnHeaderBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ColumnHeaderForeColor":
				ColumnHeaderForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "RowHeaderBackColor":
				RowHeaderBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "RowHeaderForeColor":
				RowHeaderForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotColumnHeaderBackColor":
				HotColumnHeaderBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "HotRowHeaderBackColor":
				HotRowHeaderBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "CellForeColor":
				CellForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TotalCellColumnBackColor":
				TotalCellColumnBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TotalCellColumnForeColor":
				TotalCellColumnForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TotalCellRowBackColor":
				TotalCellRowBackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TotalCellRowForeColor":
				TotalCellRowForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
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
		value = new StiPropertyObject[1] { propertiesHelper.Color() };
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		return stiPropertyCollection;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	private bool ShouldSerializeCellBackColor()
	{
		return CellBackColor != Color.White;
	}

	private bool ShouldSerializeAlternatingCellBackColor()
	{
		return AlternatingCellBackColor != ColorTranslator.FromHtml("#eee");
	}

	private bool ShouldSerializeAlternatingCellForeColor()
	{
		return AlternatingCellForeColor != StiColor.Get("#323a45");
	}

	private bool ShouldSerializeSelectedCellBackColor()
	{
		return SelectedCellBackColor != StiColorUtils.Light(ColorTranslator.FromHtml("#3498db"), (byte)30);
	}

	private bool ShouldSerializeSelectedCellForeColor()
	{
		return SelectedCellForeColor != Color.White;
	}

	private bool ShouldSerializeColumnHeaderBackColor()
	{
		return ColumnHeaderBackColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeColumnHeaderForeColor()
	{
		return ColumnHeaderForeColor != ColorTranslator.FromHtml("#fff");
	}

	private bool ShouldSerializeRowHeaderBackColor()
	{
		return RowHeaderBackColor != ColorTranslator.FromHtml("#3498db");
	}

	private bool ShouldSerializeRowHeaderForeColor()
	{
		return RowHeaderForeColor != ColorTranslator.FromHtml("#eee");
	}

	private bool ShouldSerializeHotColumnHeaderBackColor()
	{
		return HotColumnHeaderBackColor != StiColorUtils.Dark(ColorTranslator.FromHtml("#3498db"), (byte)30);
	}

	private bool ShouldSerializeHotRowHeaderBackColor()
	{
		return HotRowHeaderBackColor != StiColorUtils.Dark(ColorTranslator.FromHtml("#3498db"), (byte)30);
	}

	private bool ShouldSerializeCellForeColor()
	{
		return CellForeColor != ColorTranslator.FromHtml("#323a45");
	}

	private bool ShouldSerializeLineColor()
	{
		return LineColor != Color.White;
	}

	private bool ShouldSerializeTotalCellColumnBackColor()
	{
		return TotalCellColumnBackColor != Color.Empty;
	}

	private bool ShouldSerializeTotalCellColumnForeColor()
	{
		return TotalCellColumnForeColor != Color.Empty;
	}

	private bool ShouldSerializeTotalCellRowBackColor()
	{
		return TotalCellRowBackColor != Color.Empty;
	}

	private bool ShouldSerializeTotalCellRowForeColor()
	{
		return TotalCellRowForeColor != Color.Empty;
	}

	public void DrawStyleForGallery(Graphics g, Rectangle rect)
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + StiScale.XXI(5.0), rect.Y + StiScale.YYI(5.0), rect.Width - StiScale.XXI(10.0), rect.Height - StiScale.YYI(10.0));
		int num = rectangle.Height / 5;
		if (num * 5 > rectangle.Height)
		{
			num -= StiScale.XXI(1.0);
		}
		rectangle.Height = num * 5;
		int num2 = rectangle.Width / num;
		rectangle.Width = num * num2;
		g.FillRectangle(Brushes.White, new Rectangle(rectangle.X + num, rectangle.Y + num, rectangle.Width - num, rectangle.Height - num));
		SolidBrush val = new SolidBrush((Color == Color.White) ? Color.FromArgb(255, 236, 236, 236) : Color);
		try
		{
			g.FillRectangle((Brush)(object)val, new Rectangle(rectangle.X + num, rectangle.Y, rectangle.Width - num, num));
			g.FillRectangle((Brush)(object)val, new Rectangle(rectangle.X, rectangle.Y + num, num, rectangle.Height - num));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		for (int i = 0; i < 6; i++)
		{
			if (i == 0)
			{
				g.DrawLine(Pens.LightGray, rectangle.X + num, rectangle.Y + num * i, rectangle.Right, rectangle.Y + num * i);
			}
			else
			{
				g.DrawLine(Pens.LightGray, rectangle.X, rectangle.Y + num * i, rectangle.Right, rectangle.Y + num * i);
			}
		}
		for (int j = 0; j <= num2; j++)
		{
			if (j == 0)
			{
				g.DrawLine(Pens.LightGray, rectangle.X + num * j, rectangle.Y + num, rectangle.X + num * j, rectangle.Bottom);
			}
			else
			{
				g.DrawLine(Pens.LightGray, rectangle.X + num * j, rectangle.Y, rectangle.X + num * j, rectangle.Bottom);
			}
		}
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		Rectangle rectangle = ((rect.Width == StiScale.I(80) && rect.Height == StiScale.I(80)) ? new Rectangle(rect.X + StiScale.I9, rect.Y + StiScale.I10, StiScale.I(61), StiScale.I(60)) : ((rect.Width == StiScale.I(18) && rect.Height == StiScale.I10) ? rect : ((rect.Height < StiScale.I(10) || rect.Height > StiScale.I(16)) ? new Rectangle(rect.X + StiScale.I11, rect.Y + StiScale.I5, StiScale.I(30), StiScale.I(30)) : rect)));
		if (paintImage)
		{
			int num = rectangle.Height / 5;
			int num2 = num;
			g.FillRectangle(Brushes.White, new Rectangle(rectangle.X + num2, rectangle.Y + num, num2 * 5, rectangle.Height - num - num));
			SolidBrush val = new SolidBrush(ColumnHeaderBackColor);
			try
			{
				g.FillRectangle((Brush)(object)val, new Rectangle(rectangle.X + num2, rectangle.Y, num2 * 5, num));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			SolidBrush val2 = new SolidBrush(RowHeaderBackColor);
			try
			{
				g.FillRectangle((Brush)(object)val2, new Rectangle(rectangle.X, rectangle.Y + num, num2 * 5, rectangle.Height - num - num));
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			for (int i = 0; i < 5; i++)
			{
				int num3 = rectangle.Y + num2;
				if (i > 0)
				{
					num3 -= num2;
				}
				g.DrawLine(Pens.LightGray, rectangle.X + num2 * i, num3, rectangle.X + num2 * i, rectangle.Y + rectangle.Height - num);
			}
			for (int j = 0; j < 5; j++)
			{
				if (j == 0)
				{
					g.DrawLine(Pens.LightGray, rectangle.X + num, rectangle.Y + num * j, rectangle.X + rectangle.Width - 1, rectangle.Y + num * j);
				}
				else
				{
					g.DrawLine(Pens.LightGray, rectangle.X, rectangle.Y + num * j, rectangle.X + num2 * 5, rectangle.Y + num * j);
				}
			}
			g.DrawLine(Pens.LightGray, rectangle.X, rectangle.Y, rectangle.X + num2 * 5, rectangle.Y + rectangle.Height - num);
		}
		if (paintValue)
		{
			return;
		}
		if (rect.Height < StiScale.I(32))
		{
			rect.Width -= StiScale.I(36);
			rect.X += StiScale.I(36);
		}
		else
		{
			rect.Width -= StiScale.I(56);
			rect.X += StiScale.I(56);
		}
		SolidBrush val3 = new SolidBrush(Color.Black);
		try
		{
			Font val4 = new Font("Arial", 8f);
			try
			{
				StringFormat val5 = new StringFormat();
				try
				{
					val5.Alignment = (StringAlignment)0;
					val5.LineAlignment = (StringAlignment)1;
					g.DrawString(Name, val4, (Brush)(object)val3, (RectangleF)rect, val5);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
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

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width -= StiScale.I2;
		rect.Height -= StiScale.I3;
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
		if (styleElements != StiStyleElements.All)
		{
			throw new Exception("StiCrossTabStyle support only StiStyleElements.All.");
		}
		if (component is StiCrossTab { CrossTabStyle: var crossTabStyle } stiCrossTab)
		{
			if (!string.IsNullOrEmpty(crossTabStyle) && stiCrossTab.Report != null && stiCrossTab.Report.Styles[crossTabStyle] is StiCrossTabStyle)
			{
				Color = ((StiCrossTabStyle)stiCrossTab.Report.Styles[crossTabStyle]).Color;
			}
			else if (stiCrossTab.CrossTabStyleIndex < StiOptions.Designer.CrossTab.StyleColors.Length && stiCrossTab.CrossTabStyleIndex >= 0 && stiCrossTab.CrossTabStyleIndex < StiOptions.Designer.CrossTab.StyleColors.Length - 1)
			{
				Color = StiOptions.Designer.CrossTab.StyleColors[stiCrossTab.CrossTabStyleIndex];
			}
		}
	}

	public override void SetStyleToComponent(StiComponent component)
	{
		if (component is StiCrossTab stiCrossTab && StiStyleConditionHelper.IsAllowStyle(component, this))
		{
			stiCrossTab.CrossTabStyleColor = Color;
			stiCrossTab.UpdateStyles();
		}
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiCrossTabStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiCrossTabStyle");
	}

	internal StiCrossTabStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
	}

	public StiCrossTabStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiCrossTabStyle(string name)
		: this(name, "")
	{
	}

	public StiCrossTabStyle()
		: this("")
	{
	}
}
