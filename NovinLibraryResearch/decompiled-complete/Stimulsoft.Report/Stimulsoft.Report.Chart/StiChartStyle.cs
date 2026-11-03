using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[StiServiceBitmap(typeof(StiChartStyle), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiChartStyle), "Stimulsoft.Report.Images.Components.StiChart.png")]
[TypeConverter(typeof(StiChartStyleConverter))]
public abstract class StiChartStyle : StiBaseStyle, IStiChartStyle, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiChartStyle;

	public override string ServiceName => Core.LocalizedName;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiChartStyle);

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual bool AllowDashboard => false;

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual StiElementStyleIdent StyleIdent => StiElementStyleIdent.Blue;

	[Browsable(false)]
	public virtual bool IsOffice2015Style => false;

	[Browsable(false)]
	public StiStyleCoreXF Core { get; set; }

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		throw new NotImplementedException();
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Ident", ((object)this).GetType().Name);
		return val;
	}

	internal static StiChartStyle CreateFromJsonObject(JObject jObject)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		if (ident == "StiCustomStyle")
		{
			return new StiCustomStyle();
		}
		StiChartStyle stiChartStyle = StiOptions.Services.ChartStyles.FirstOrDefault((StiChartStyle x) => ((object)x).GetType().Name == ident);
		if (stiChartStyle == null)
		{
			throw new Exception("Type " + ident + " is not found!");
		}
		return stiChartStyle.CreateNew();
	}

	public override string ToString()
	{
		if (this is StiCustomStyle && ((StiCustomStyleCoreXF)Core).ReportChartStyle != null)
		{
			return ((StiCustomStyleCoreXF)Core).ReportChartStyle.Name;
		}
		return ((StiService)this).ServiceName;
	}

	public bool CompareChartStyle(StiChartStyle style)
	{
		if (style == null)
		{
			return false;
		}
		StiCustomStyle stiCustomStyle = this as StiCustomStyle;
		StiCustomStyle stiCustomStyle2 = style as StiCustomStyle;
		if (stiCustomStyle != null && stiCustomStyle2 != null)
		{
			string text = ((StiCustomStyleCoreXF)stiCustomStyle.Core).ReportStyleName;
			if (((StiCustomStyleCoreXF)stiCustomStyle.Core).ReportStyle != null)
			{
				text = ((StiCustomStyleCoreXF)stiCustomStyle.Core).ReportStyle.Name;
			}
			string text2 = ((StiCustomStyleCoreXF)stiCustomStyle2.Core).ReportStyleName;
			if (((StiCustomStyleCoreXF)stiCustomStyle2.Core).ReportStyle != null)
			{
				text2 = ((StiCustomStyleCoreXF)stiCustomStyle2.Core).ReportStyle.Name;
			}
			return text == text2;
		}
		return ((object)this).GetType() == ((object)style).GetType();
	}

	public override void DrawStyleForEditor(Graphics g, Rectangle rect, DrawItemState state)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		base.DrawStyleForEditor(g, rect, state);
		g.SetClip(rect);
		int num = rect.Height - 6;
		Rectangle rectangle = new Rectangle(rect.X + 3, rect.Y + 3, num, num);
		Color[] styleColors = Core.StyleColors;
		foreach (Color color in styleColors)
		{
			SolidBrush val = new SolidBrush(color);
			try
			{
				g.FillRectangle((Brush)(object)val, rectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			rectangle.X += 6 + num;
		}
		g.ResetClip();
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected I4, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		Brush brush = StiBrush.GetBrush(Core.ChartBrush, rect);
		try
		{
			g.FillRectangle(brush, rect);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
		int num = rect.X;
		g.SetClip(rect);
		Color[] styleColors = Core.StyleColors;
		foreach (Color color in styleColors)
		{
			Rectangle rectangle = new Rectangle(num, rect.Y, 20, rect.Height);
			Brush val = (Brush)new SolidBrush(color);
			try
			{
				g.FillRectangle(val, rectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			num += 20;
		}
		g.ResetClip();
		if (!paintValue)
		{
			if (paintImage)
			{
				base.DrawStyleImage(g, rect);
				rect.Width -= 16;
				rect.X += 16;
			}
			GraphicsPath val2 = new GraphicsPath();
			try
			{
				Font val3 = new Font("Arial", (float)(10.0 * StiScale.Factor));
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val4.Alignment = (StringAlignment)0;
						val4.LineAlignment = (StringAlignment)1;
						Rectangle rectangle2 = rect;
						rectangle2.Inflate(-2, 0);
						SizeF sizeF = g.MeasureString(Core.LocalizedName, val3);
						if ((float)rectangle2.Width < sizeF.Width)
						{
							val4.Alignment = (StringAlignment)0;
						}
						val2.AddString(Core.LocalizedName, val3.FontFamily, (int)val3.Style, val3.Size, rectangle2, val4);
						Pen val5 = new Pen(Color.FromArgb(255, 251, 251, 251), 4f);
						try
						{
							val5.LineJoin = (LineJoin)2;
							g.DrawPath(val5, val2);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
						SolidBrush val6 = new SolidBrush(Color.FromArgb(255, 37, 37, 37));
						try
						{
							g.FillPath((Brush)(object)val6, val2);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
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
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			if (paintImage)
			{
				rect.Width += 16;
				rect.X -= 16;
			}
		}
		Pen val7 = new Pen(Core.ChartAreaBorderColor);
		try
		{
			g.DrawRectangle(val7, rect);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
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
		if (component is StiChart stiChart)
		{
			if (this is StiCustomStyle)
			{
				stiChart.ComponentStyle = ((StiCustomStyleCoreXF)Core).ReportStyleName;
				return;
			}
			stiChart.Style = (IStiChartStyle)((StiService)this).Clone();
			stiChart.Core.ApplyStyle(stiChart.Style);
		}
	}

	public virtual StiChartStyle CreateNew()
	{
		throw new NotImplementedException();
	}
}
