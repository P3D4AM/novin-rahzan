using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Gauge.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Gauge;

[TypeConverter(typeof(StiGaugeStyleConverter))]
public abstract class StiGaugeStyleXF : StiBaseStyle, IStiGaugeStyle, ICloneable, IStiJsonReportObject
{
	public override StiComponentId ComponentId => StiComponentId.StiGaugeStyle;

	public override string ServiceName => Core.LocalizedName;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Gauge";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiGaugeStyleXF);

	[Browsable(false)]
	public StiGaugeStyleCoreXF Core { get; set; }

	[Browsable(false)]
	[StiBrowsable(false)]
	public virtual bool AllowDashboard => false;

	[Browsable(false)]
	[StiBrowsable(false)]
	public abstract StiElementStyleIdent StyleIdent { get; }

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

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	internal static StiGaugeStyleXF CreateFromJsonObject(JObject jObject)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		if (ident == "StiGaugeStyleXF")
		{
			return new StiCustomGaugeStyle();
		}
		StiGaugeStyleXF stiGaugeStyleXF = StiOptions.Services.GaugeStyles.FirstOrDefault((StiGaugeStyleXF x) => ((object)x).GetType().Name == ident);
		if (stiGaugeStyleXF == null)
		{
			throw new Exception("Type " + ident + " is not found!");
		}
		return stiGaugeStyleXF.CreateNew();
	}

	public override string ToString()
	{
		if (this is StiCustomGaugeStyle && ((StiCustomGaugeStyleCoreXF)Core).ReportGaugeStyle != null)
		{
			return ((StiCustomGaugeStyleCoreXF)Core).ReportGaugeStyle.Name;
		}
		return ((StiService)this).ServiceName;
	}

	public bool CompareGaugeStyle(StiGaugeStyleXF style)
	{
		if (style == null)
		{
			return false;
		}
		StiCustomGaugeStyle stiCustomGaugeStyle = this as StiCustomGaugeStyle;
		StiCustomGaugeStyle stiCustomGaugeStyle2 = style as StiCustomGaugeStyle;
		if (stiCustomGaugeStyle != null && stiCustomGaugeStyle2 != null)
		{
			string reportStyleName = ((StiCustomGaugeStyleCoreXF)stiCustomGaugeStyle.Core).ReportStyleName;
			string reportStyleName2 = ((StiCustomGaugeStyleCoreXF)stiCustomGaugeStyle2.Core).ReportStyleName;
			return reportStyleName == reportStyleName2;
		}
		return ((object)this).GetType() == ((object)style).GetType();
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + 5, rect.Y + 5, 40, 30);
		Rectangle rectangle2 = new Rectangle(rectangle.X + rectangle.Width / 2 - rectangle.Height / 2, rectangle.Y, rectangle.Height, rectangle.Height);
		Brush brush = StiBrush.GetBrush(Core.Brush, rectangle);
		try
		{
			g.FillEllipse(brush, rectangle2);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
		g.DrawEllipse(Pens.LightGray, rectangle2);
		Rectangle rectangle3 = new Rectangle(rectangle.X + rectangle.Width / 2 - 2, rectangle.Y + rectangle.Height / 2 - 2, 4, 4);
		Brush brush2 = StiBrush.GetBrush(Core.NeedleCapBrush, rectangle3);
		try
		{
			Pen val = new Pen(brush2);
			try
			{
				g.DrawEllipse(val, rectangle3);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
		g.DrawLine(new Pen(StiBrush.ToColor(Core.NeedleBrush)), rectangle2.X + rectangle2.Width / 2 + 2, rectangle2.Y + rectangle2.Height / 2, rectangle2.X + rectangle2.Width + 2, rectangle2.Y + rectangle2.Height / 2);
		rect.X += rectangle.Right + 5;
		rect.Width -= rectangle.Width - 10;
		SolidBrush val2 = new SolidBrush(Color.Black);
		try
		{
			Font val3 = new Font("Arial", 10f);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.Alignment = (StringAlignment)0;
					val4.LineAlignment = (StringAlignment)1;
					Rectangle rectangle4 = rect;
					rectangle4.Inflate(-2, 0);
					SizeF sizeF = g.MeasureString(Name, val3);
					if ((float)rectangle4.Width < sizeF.Width)
					{
						val4.Alignment = (StringAlignment)0;
					}
					g.DrawString(Name, val3, (Brush)(object)val2, (RectangleF)rect, val4);
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

	public virtual StiGaugeStyleXF CreateNew()
	{
		throw new NotImplementedException();
	}
}
