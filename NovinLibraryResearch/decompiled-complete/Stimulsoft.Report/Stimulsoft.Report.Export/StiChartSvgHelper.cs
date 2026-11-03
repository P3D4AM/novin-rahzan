using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export.Services.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export;

public static class StiChartSvgHelper
{
	public static void WriteChart(XmlTextWriter writer, StiSvgData svgData, bool needAnimation)
	{
		WriteChart(writer, svgData, 1f, needAnimation);
	}

	public static void WriteChart(XmlTextWriter writer, StiSvgData svgData, float zoom, bool needAnimation)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		StiChart stiChart = svgData.Component as StiChart;
		stiChart.IsAnimation = needAnimation;
		StiContext stiContext = null;
		if (stiChart.Series.Count == 0)
		{
			WriteEmptyDataMessage(writer, svgData);
			stiChart.IsAnimation = false;
			return;
		}
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, zoom);
				bool isAnimation = stiChart.IsAnimation;
				stiChart.IsAnimationChangingValues = needAnimation;
				StiReport report = stiChart.Report;
				float num = (float)svgData.Width;
				float num2 = (float)svgData.Height;
				if (stiChart.Rotation == StiImageRotation.Rotate90CCW || stiChart.Rotation == StiImageRotation.Rotate90CW)
				{
					num = (float)svgData.Height;
					num2 = (float)svgData.Width;
				}
				StiCellGeom stiCellGeom = stiChart.Core.Render(stiContext, new RectangleF(0f, 0f, num, num2), useMargins: true);
				stiCellGeom.DrawGeom(stiContext);
				stiChart.PreviousAnimations = stiContext.Animations;
				stiChart.IsAnimation = isAnimation;
				writer.WriteStartElement("g");
				double num3 = svgData.X + 0.5;
				double num4 = svgData.Y + 0.5;
				string arg = "";
				switch (stiChart.Rotation)
				{
				case StiImageRotation.Rotate90CW:
					arg = " rotate(90)";
					num3 += (double)num2;
					break;
				case StiImageRotation.Rotate90CCW:
					arg = " rotate(-90)";
					num4 += (double)num;
					break;
				case StiImageRotation.Rotate180:
					arg = " rotate(180)";
					num4 += (double)num2;
					num3 += (double)num;
					break;
				case StiImageRotation.FlipHorizontal:
					arg = " scale(-1, 1)";
					num3 += (double)num;
					break;
				case StiImageRotation.FlipVertical:
					arg = " scale(1, -1)";
					num4 += (double)num2;
					break;
				}
				writer.WriteAttributeString("transform", $"translate({StiContextSvgHelper.DoubleToString(num3)},{StiContextSvgHelper.DoubleToString(num4)}){arg}");
				StiContextSvgHelper.WriteGeoms(writer, stiContext, needAnimation);
				writer.WriteEndElement();
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		stiChart.IsAnimation = false;
	}

	private static void WriteEmptyDataMessage(XmlTextWriter writer, StiSvgData svgData)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		if (svgData.Width > 50.0)
		{
			writer.WriteStartElement("image");
			writer.WriteAttributeString("x", StiContextSvgHelper.DoubleToString(svgData.Width / 2.0 - 16.0));
			writer.WriteAttributeString("y", StiContextSvgHelper.DoubleToString(svgData.Height / 2.0 - 16.0));
			writer.WriteAttributeString("width", "32");
			writer.WriteAttributeString("height", "32");
			writer.WriteStartAttribute("href");
			writer.WriteString("data:image/png;base64," + Convert.ToBase64String(GetEmptyDataImage()));
			writer.WriteRaw("\r\n");
			writer.WriteEndAttribute();
			writer.WriteEndElement();
		}
		Font baseFont = new Font("Arial", 8f);
		string text = Loc.Get("PropertyMain", "NoElements");
		double num = MeasureTextWidth(text, baseFont);
		if (svgData.Width > num + 30.0)
		{
			writer.WriteStartElement("text");
			writer.WriteAttributeString("x", StiContextSvgHelper.DoubleToString(svgData.Width / 2.0 - num / 2.0));
			writer.WriteAttributeString("y", StiContextSvgHelper.DoubleToString(svgData.Height / 2.0 + 30.0));
			writer.WriteAttributeString("style", "font-size:12px; font-family:'Arial'; fill: #a9a9a9;");
			writer.WriteRaw(text);
			writer.WriteEndElement();
		}
	}

	private static byte[] GetEmptyDataImage()
	{
		Assembly assembly = typeof(StiChartSvgHelper).Assembly;
		using Stream stream = assembly.GetManifestResourceStream("Stimulsoft.Report.Images.NotDefined.StiChart_x2.png");
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		return array;
	}

	private static double MeasureTextWidth(string text, Font baseFont)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				return val2.MeasureString(text, baseFont).Width;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
