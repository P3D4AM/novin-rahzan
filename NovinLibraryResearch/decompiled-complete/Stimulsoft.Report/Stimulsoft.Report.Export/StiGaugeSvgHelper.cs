using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Gauge.Painters;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.GaugeGeoms;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Report.Export;

public static class StiGaugeSvgHelper
{
	private const double PiDiv180 = Math.PI / 180.0;

	private const double FourDivThree = 1.3333333333333333;

	public static void WriteGauge(XmlTextWriter writer, StiSvgData svgData, float zoom, bool needAnimation, bool isStyleSample = false)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcf: Expected O, but got Unknown
		StiGauge stiGauge = svgData.Component as StiGauge;
		bool isAnimation = stiGauge.IsAnimation;
		stiGauge.IsAnimation = needAnimation;
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				if (stiGauge.IsDesigning)
				{
					StiGaugeV2InitHelper.Prepare(stiGauge);
				}
				StiGdiGaugeContextPainter stiGdiGaugeContextPainter = new StiGdiGaugeContextPainter(val2, stiGauge, new RectangleF(0f, 0f, (float)svgData.Width, (float)svgData.Height), zoom)
				{
					Gauge = stiGauge,
					Rect = new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height),
					Zoom = zoom
				};
				stiGdiGaugeContextPainter.Geoms.Clear();
				stiGauge.DrawGauge(stiGdiGaugeContextPainter);
				stiGauge.PreviousAnimations = stiGdiGaugeContextPainter.Animations;
				if (isStyleSample)
				{
					stiGdiGaugeContextPainter.Geoms.RemoveAll((StiGaugeGeom geom) => geom is StiTextGaugeGeom || geom is StiRectangleGaugeGeom);
				}
				stiGdiGaugeContextPainter.Render();
				stiGauge.IsAnimation = isAnimation;
				double num = 0.0;
				foreach (StiGaugeGeom geom in stiGdiGaugeContextPainter.Geoms)
				{
					if (geom != null && geom.Animation != null && geom.Animation.Duration.TotalMilliseconds + geom.Animation.BeginTime.TotalMilliseconds > num)
					{
						num = geom.Animation.Duration.TotalMilliseconds + geom.Animation.BeginTime.TotalMilliseconds;
					}
				}
				writer.WriteStartElement("g");
				writer.WriteAttributeString("transform", "translate(" + (svgData.X + 0.5).ToString().Replace(",", ".") + "," + (svgData.Y + 0.5).ToString().Replace(",", ".") + ")");
				foreach (StiGaugeGeom geom2 in stiGdiGaugeContextPainter.Geoms)
				{
					if (geom2 is StiPushMatrixGaugeGeom)
					{
						StiPushMatrixGaugeGeom stiPushMatrixGaugeGeom = geom2 as StiPushMatrixGaugeGeom;
						writer.WriteStartElement("g");
						writer.WriteAttributeString("transform", string.Format("rotate({2}, {0}, {1})", stiPushMatrixGaugeGeom.CenterPoint.X.ToString().Replace(",", "."), stiPushMatrixGaugeGeom.CenterPoint.Y.ToString().Replace(",", "."), stiPushMatrixGaugeGeom.Angle.ToString().Replace(",", ".")));
					}
					else if (geom2 is StiPopTranformGaugeGeom)
					{
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.RoundedRectangle)
					{
						StiRoundedRectangleGaugeGeom stiRoundedRectangleGaugeGeom = geom2 as StiRoundedRectangleGaugeGeom;
						RectangleF rect = RectToRectangleF(stiRoundedRectangleGaugeGeom.Rect);
						StringBuilder stringBuilder = new StringBuilder();
						if (stiRoundedRectangleGaugeGeom.Background != null)
						{
							stringBuilder.Append(WriteFillBrush(writer, stiRoundedRectangleGaugeGeom.Background, rect));
						}
						else
						{
							stringBuilder.Append("fill-opacity:0;");
						}
						string arg = WriteBorderStroke(writer, stiRoundedRectangleGaugeGeom.BorderBrush, rect) ?? "";
						stringBuilder.Append($"{arg};stroke-width:{stiRoundedRectangleGaugeGeom.BorderWidth};");
						writer.WriteStartElement("rect");
						writer.WriteAttributeString("x", rect.X.ToString().Replace(",", "."));
						writer.WriteAttributeString("y", rect.Y.ToString().Replace(",", "."));
						writer.WriteAttributeString("ry", stiRoundedRectangleGaugeGeom.LeftTop.ToString().Replace(",", "."));
						writer.WriteAttributeString("rx", stiRoundedRectangleGaugeGeom.LeftTop.ToString().Replace(",", "."));
						writer.WriteAttributeString("height", rect.Height.ToString().Replace(",", "."));
						writer.WriteAttributeString("width", rect.Width.ToString().Replace(",", "."));
						writer.WriteAttributeString("shape-rendering", "crispEdges");
						writer.WriteAttributeString("style", stringBuilder.ToString());
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.Rectangle)
					{
						StiRectangleGaugeGeom stiRectangleGaugeGeom = geom2 as StiRectangleGaugeGeom;
						RectangleF rect2 = RectToRectangleF(stiRectangleGaugeGeom.Rect);
						StringBuilder stringBuilder2 = new StringBuilder();
						if (stiRectangleGaugeGeom.Background != null)
						{
							stringBuilder2.Append(WriteFillBrush(writer, stiRectangleGaugeGeom.Background, rect2));
						}
						else
						{
							stringBuilder2.Append("fill-opacity:0;");
						}
						string arg2 = WriteBorderStroke(writer, stiRectangleGaugeGeom.BorderBrush, rect2) ?? "";
						stringBuilder2.Append($"{arg2};stroke-width:{stiRectangleGaugeGeom.BorderWidth};");
						writer.WriteStartElement("rect");
						writer.WriteAttributeString("x", rect2.X.ToString().Replace(",", "."));
						writer.WriteAttributeString("y", rect2.Y.ToString().Replace(",", "."));
						writer.WriteAttributeString("height", (rect2.Height + 1f).ToString().Replace(",", "."));
						writer.WriteAttributeString("width", (rect2.Width + 1f).ToString().Replace(",", "."));
						writer.WriteAttributeString("shape-rendering", "crispEdges");
						writer.WriteAttributeString("style", stringBuilder2.ToString());
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.GraphicsPath)
					{
						StiGraphicsPathGaugeGeom stiGraphicsPathGaugeGeom = geom2 as StiGraphicsPathGaugeGeom;
						RectangleF rect3 = RectToRectangleF(stiGraphicsPathGaugeGeom.Rect);
						string text = ((stiGraphicsPathGaugeGeom.Background != null) ? WriteFillBrush(writer, stiGraphicsPathGaugeGeom.Background, rect3) : "fill-opacity:0;");
						string text2 = WriteBorderStroke(writer, stiGraphicsPathGaugeGeom.BorderBrush, rect3) ?? "";
						text = text + text2 + ";stroke-width:" + stiGraphicsPathGaugeGeom.BorderWidth.ToString().Replace(",", ".") + ";";
						string pathData = GetPathData(stiGraphicsPathGaugeGeom.Geoms, stiGraphicsPathGaugeGeom.StartPoint);
						writer.WriteStartElement("path");
						writer.WriteAttributeString("d", pathData);
						writer.WriteAttributeString("style", text);
						if (stiGraphicsPathGaugeGeom.Animation != null)
						{
							if (stiGraphicsPathGaugeGeom.Animation is StiTranslationAnimation)
							{
								StiTranslationAnimation stiTranslationAnimation = stiGraphicsPathGaugeGeom.Animation as StiTranslationAnimation;
								string text3 = (stiTranslationAnimation.StartPoint.X - stiTranslationAnimation.EndPoint.X).ToString().Replace(",", ".");
								string text4 = (stiTranslationAnimation.StartPoint.Y - stiTranslationAnimation.EndPoint.Y).ToString().Replace(",", ".");
								writer.WriteStartElement("animateTransform");
								writer.WriteAttributeString("attributeType", "xml");
								writer.WriteAttributeString("attributeName", "transform");
								writer.WriteAttributeString("type", "translate");
								writer.WriteAttributeString("from", text3 + " " + text4);
								writer.WriteAttributeString("to", "0 0");
								writer.WriteAttributeString("dur", $"{stiTranslationAnimation.Duration}");
								writer.WriteEndElement();
							}
							else if (stiGraphicsPathGaugeGeom.Animation is StiRotationAnimation)
							{
								StiRotationAnimation stiRotationAnimation = stiGraphicsPathGaugeGeom.Animation as StiRotationAnimation;
								writer.WriteStartElement("animateTransform");
								writer.WriteAttributeString("attributeType", "xml");
								writer.WriteAttributeString("attributeName", "transform");
								writer.WriteAttributeString("type", "rotate");
								writer.WriteAttributeString("from", (stiRotationAnimation.StartAngle - stiRotationAnimation.EndAngle).ToString().Replace(",", ".") + " " + stiRotationAnimation.CenterPoint.X.ToString().Replace(",", ".") + " " + stiRotationAnimation.CenterPoint.Y.ToString().Replace(",", "."));
								writer.WriteAttributeString("to", "0 " + stiRotationAnimation.CenterPoint.X.ToString().Replace(",", ".") + " " + stiRotationAnimation.CenterPoint.Y.ToString().Replace(",", "."));
								writer.WriteAttributeString("dur", $"{stiRotationAnimation.Duration}");
								writer.WriteEndElement();
							}
							else if (stiGraphicsPathGaugeGeom.Animation is StiScaleAnimation)
							{
								StiScaleAnimation stiScaleAnimation = stiGraphicsPathGaugeGeom.Animation as StiScaleAnimation;
								string text5 = ((0.0 - stiScaleAnimation.StartScaleX + stiScaleAnimation.EndScaleX) * stiScaleAnimation.CenterX).ToString().Replace(",", ".");
								string text6 = ((0.0 - stiScaleAnimation.StartScaleY + stiScaleAnimation.EndScaleY) * stiScaleAnimation.CenterY).ToString().Replace(",", ".");
								writer.WriteStartElement("animateTransform");
								writer.WriteAttributeString("attributeType", "xml");
								writer.WriteAttributeString("attributeName", "transform");
								writer.WriteAttributeString("type", "translate");
								writer.WriteAttributeString("from", text5 + " " + text6);
								writer.WriteAttributeString("to", "0 0");
								writer.WriteAttributeString("dur", $"{stiScaleAnimation.Duration}");
								writer.WriteEndElement();
								writer.WriteStartElement("animateTransform");
								writer.WriteAttributeString("attributeType", "xml");
								writer.WriteAttributeString("attributeName", "transform");
								writer.WriteAttributeString("type", "scale");
								writer.WriteAttributeString("from", $"{stiScaleAnimation.StartScaleX} {stiScaleAnimation.StartScaleY}");
								writer.WriteAttributeString("to", $"{stiScaleAnimation.EndScaleX} {stiScaleAnimation.EndScaleY}");
								writer.WriteAttributeString("dur", $"{stiScaleAnimation.Duration}");
								writer.WriteAttributeString("additive", "sum");
								writer.WriteEndElement();
							}
						}
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.Pie)
					{
						StiPieGaugeGeom stiPieGaugeGeom = geom2 as StiPieGaugeGeom;
						RectangleF rect4 = RectToRectangleF(stiPieGaugeGeom.Rect);
						string text7 = ((stiPieGaugeGeom.Background != null) ? WriteFillBrush(writer, stiPieGaugeGeom.Background, rect4) : "fill-opacity:0;");
						string text8 = WriteBorderStroke(writer, stiPieGaugeGeom.BorderBrush, rect4) ?? "";
						text7 = text7 + text8 + ";stroke-width:" + stiPieGaugeGeom.BorderWidth.ToString().Replace(",", ".") + ";";
						string value = AddPiePath(stiPieGaugeGeom, "");
						writer.WriteStartElement("path");
						writer.WriteAttributeString("d", value);
						writer.WriteAttributeString("style", text7);
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.Ellipse)
					{
						StiEllipseGaugeGeom stiEllipseGaugeGeom = geom2 as StiEllipseGaugeGeom;
						RectangleF rect5 = RectToRectangleF(stiEllipseGaugeGeom.Rect);
						string text9 = ((stiEllipseGaugeGeom.Background != null) ? WriteFillBrush(writer, stiEllipseGaugeGeom.Background, rect5) : "fill-opacity:0;");
						string text10 = WriteBorderStroke(writer, stiEllipseGaugeGeom.BorderBrush, rect5) ?? "";
						text9 = text9 + text10 + ";stroke-width:" + stiEllipseGaugeGeom.BorderWidth.ToString().Replace(",", ".") + ";";
						writer.WriteStartElement("ellipse");
						writer.WriteAttributeString("cx", (rect5.X + rect5.Width / 2f).ToString().Replace(",", "."));
						writer.WriteAttributeString("cy", (rect5.Y + rect5.Height / 2f).ToString().Replace(",", "."));
						writer.WriteAttributeString("rx", (rect5.Width / 2f).ToString().Replace(",", "."));
						writer.WriteAttributeString("ry", (rect5.Height / 2f).ToString().Replace(",", "."));
						writer.WriteAttributeString("style", text9);
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.GraphicsArcGeometry)
					{
						StiGraphicsArcGeometryGaugeGeom stiGraphicsArcGeometryGaugeGeom = geom2 as StiGraphicsArcGeometryGaugeGeom;
						RectangleF rect6 = RectToRectangleF(stiGraphicsArcGeometryGaugeGeom.Rect);
						string text11 = ((stiGraphicsArcGeometryGaugeGeom.Background != null) ? WriteFillBrush(writer, stiGraphicsArcGeometryGaugeGeom.Background, rect6) : "fill-opacity:0;");
						string text12 = WriteBorderStroke(writer, stiGraphicsArcGeometryGaugeGeom.BorderBrush, rect6) ?? "";
						text11 = text11 + text12 + ";stroke-width:" + stiGraphicsArcGeometryGaugeGeom.BorderWidth.ToString().Replace(",", ".") + ";";
						string value2 = AddArcPath(stiGraphicsArcGeometryGaugeGeom, "");
						writer.WriteStartElement("path");
						writer.WriteAttributeString("d", value2);
						writer.WriteAttributeString("style", text11);
						writer.WriteEndElement();
					}
					else if (geom2.Type == StiGaugeGeomType.Text)
					{
						StiTextGaugeGeom stiTextGaugeGeom = geom2 as StiTextGaugeGeom;
						Font val3 = new Font(StiFontCollection.GetFontFamily(stiTextGaugeGeom.Font.Name, false), stiTextGaugeGeom.Font.Size, stiTextGaugeGeom.Font.Style, stiTextGaugeGeom.Font.Unit, stiTextGaugeGeom.Font.GdiCharSet, stiTextGaugeGeom.Font.GdiVerticalFont);
						RectangleF rectangleF = RectToRectangleF(stiTextGaugeGeom.Rect);
						PointF pointF = new PointF(rectangleF.X, rectangleF.Y);
						float size = val3.Size * 4f / 3f;
						WriteText(writer, stiTextGaugeGeom.Text, val3, stiTextGaugeGeom.Foreground, pointF, size);
					}
					else
					{
						if (geom2.Type != StiGaugeGeomType.RadialRange)
						{
							continue;
						}
						StiRadialRangeGaugeGeom stiRadialRangeGaugeGeom = geom2 as StiRadialRangeGaugeGeom;
						double num2 = (double)stiRadialRangeGaugeGeom.StartAngle * (Math.PI / 180.0);
						StringBuilder stringBuilder3 = new StringBuilder();
						PointF centerPoint = stiRadialRangeGaugeGeom.CenterPoint;
						double num3 = (double)centerPoint.X + (double)stiRadialRangeGaugeGeom.Radius1 * Math.Cos(num2);
						double num4 = (double)centerPoint.Y + (double)stiRadialRangeGaugeGeom.Radius1 * Math.Sin(num2);
						double num5 = Math.Round(Math.Abs(stiRadialRangeGaugeGeom.SweepAngle / 10f));
						double num6 = (double)stiRadialRangeGaugeGeom.SweepAngle / num5;
						double num7 = stiRadialRangeGaugeGeom.Radius1 - stiRadialRangeGaugeGeom.Radius2;
						double num8 = 1.0 / num5;
						double num9 = 0.0;
						stringBuilder3.AppendFormat("M{0},{1}", num3.ToString().Replace(",", "."), num4.ToString().Replace(",", "."));
						num2 = stiRadialRangeGaugeGeom.StartAngle;
						int num10 = -1;
						while ((double)(++num10) < num5)
						{
							double radius = (double)stiRadialRangeGaugeGeom.Radius1 - num7 * num9;
							double radius2 = (double)stiRadialRangeGaugeGeom.Radius1 - num7 * (num9 + num8);
							PointF[] array = ConvertArcToCubicBezier(new PointF(centerPoint.X, centerPoint.Y), radius, radius2, num2, num6);
							if (num10 == 0)
							{
								stringBuilder3.AppendFormat(" C{0},{1},{2},{3},{4},{5}", array[1].X.ToString().Replace(",", "."), array[1].Y.ToString().Replace(",", "."), array[2].X.ToString().Replace(",", "."), array[2].Y.ToString().Replace(",", "."), array[3].X.ToString().Replace(",", "."), array[3].Y.ToString().Replace(",", "."));
							}
							else
							{
								stringBuilder3.AppendFormat(",{0},{1},{2},{3},{4},{5}", array[1].X.ToString().Replace(",", "."), array[1].Y.ToString().Replace(",", "."), array[2].X.ToString().Replace(",", "."), array[2].Y.ToString().Replace(",", "."), array[3].X.ToString().Replace(",", "."), array[3].Y.ToString().Replace(",", "."));
							}
							num2 += num6;
							num9 += num8;
						}
						num6 = (double)stiRadialRangeGaugeGeom.SweepAngle / num5;
						num7 = stiRadialRangeGaugeGeom.Radius3 - stiRadialRangeGaugeGeom.Radius4;
						num8 = 1.0 / num5;
						num9 = num5;
						num2 = stiRadialRangeGaugeGeom.StartAngle + stiRadialRangeGaugeGeom.SweepAngle;
						for (num10 = 0; (double)num10 < num5; num10++)
						{
							double radius3 = (double)stiRadialRangeGaugeGeom.Radius3 - num7 * num9;
							double radius4 = (double)stiRadialRangeGaugeGeom.Radius3 - num7 * (num9 + num8);
							PointF[] array2 = ConvertArcToCubicBezier(centerPoint, radius3, radius4, num2, 0.0 - num6);
							if (num10 == 0)
							{
								stringBuilder3.AppendFormat(" L{0},{1}", array2[0].X.ToString().Replace(",", "."), array2[0].Y.ToString().Replace(",", "."));
								stringBuilder3.AppendFormat(" C{0},{1},{2},{3},{4},{5}", array2[1].X.ToString().Replace(",", "."), array2[1].Y.ToString().Replace(",", "."), array2[2].X.ToString().Replace(",", "."), array2[2].Y.ToString().Replace(",", "."), array2[3].X.ToString().Replace(",", "."), array2[3].Y.ToString().Replace(",", "."));
							}
							else
							{
								stringBuilder3.AppendFormat(",{0},{1},{2},{3},{4},{5}", array2[1].X.ToString().Replace(",", "."), array2[1].Y.ToString().Replace(",", "."), array2[2].X.ToString().Replace(",", "."), array2[2].Y.ToString().Replace(",", "."), array2[3].X.ToString().Replace(",", "."), array2[3].Y.ToString().Replace(",", "."));
							}
							num2 -= num6;
							num9 -= num8;
						}
						stringBuilder3.AppendFormat("z");
						RectangleF rect7 = RectToRectangleF(stiRadialRangeGaugeGeom.Rect);
						string text13 = ((stiRadialRangeGaugeGeom.Background != null) ? WriteFillBrush(writer, stiRadialRangeGaugeGeom.Background, rect7) : "fill-opacity:0;");
						string text14 = WriteBorderStroke(writer, stiRadialRangeGaugeGeom.BorderBrush, rect7) ?? "";
						text13 = text13 + text14 + ";stroke-width:" + stiRadialRangeGaugeGeom.BorderWidth.ToString().Replace(",", ".") + ";";
						writer.WriteStartElement("path");
						writer.WriteAttributeString("d", stringBuilder3.ToString());
						writer.WriteAttributeString("style", text13);
						writer.WriteEndElement();
					}
				}
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
		writer.WriteEndElement();
	}

	public static void WriteGauge(XmlTextWriter writer, StiSvgData svgData, bool needAnimation, bool isStyleSample = false)
	{
		WriteGauge(writer, svgData, 1f, needAnimation, isStyleSample);
	}

	private static string GetPathData(List<StiGaugeGeom> geoms, PointF startPoint)
	{
		string text = "M" + startPoint.X.ToString().Replace(",", ".") + "," + startPoint.Y.ToString().Replace(",", ".");
		int num = 0;
		foreach (StiGaugeGeom geom in geoms)
		{
			if (geom.Type == StiGaugeGeomType.GraphicsPathArc)
			{
				StiGraphicsPathArcGaugeGeom stiGraphicsPathArcGaugeGeom = geom as StiGraphicsPathArcGaugeGeom;
				text += GetArcPath(new RectangleF(stiGraphicsPathArcGaugeGeom.X, stiGraphicsPathArcGaugeGeom.Y, stiGraphicsPathArcGaugeGeom.Width, stiGraphicsPathArcGaugeGeom.Height), text, stiGraphicsPathArcGaugeGeom.StartAngle, stiGraphicsPathArcGaugeGeom.SweepAngle, num == 0);
			}
			else if (geom.Type == StiGaugeGeomType.GraphicsPathLine)
			{
				StiGraphicsPathLineGaugeGeom stiGraphicsPathLineGaugeGeom = geom as StiGraphicsPathLineGaugeGeom;
				StringBuilder stringBuilder = new StringBuilder();
				if (!text.StartsWith("M"))
				{
					stringBuilder.AppendFormat("M{0},{1}", stiGraphicsPathLineGaugeGeom.P2.X.ToString().Replace(",", "."), stiGraphicsPathLineGaugeGeom.P2.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("L{0},{1}", stiGraphicsPathLineGaugeGeom.P2.X.ToString().Replace(",", "."), stiGraphicsPathLineGaugeGeom.P2.Y.ToString().Replace(",", "."));
				text += stringBuilder;
			}
			else if (geom.Type == StiGaugeGeomType.GraphicsPathLines)
			{
				StiGraphicsPathLinesGaugeGeom stiGraphicsPathLinesGaugeGeom = geom as StiGraphicsPathLinesGaugeGeom;
				StringBuilder stringBuilder2 = new StringBuilder();
				if (!text.StartsWith("M"))
				{
					stringBuilder2.AppendFormat("M{0},{1}", stiGraphicsPathLinesGaugeGeom.Points[0].X.ToString().Replace(",", "."), stiGraphicsPathLinesGaugeGeom.Points[0].Y.ToString().Replace(",", "."));
				}
				for (int i = 0; i < stiGraphicsPathLinesGaugeGeom.Points.Length; i++)
				{
					if (i == 0)
					{
						stringBuilder2.AppendFormat("L{0},{1}", stiGraphicsPathLinesGaugeGeom.Points[i].X.ToString().Replace(",", "."), stiGraphicsPathLinesGaugeGeom.Points[i].Y.ToString().Replace(",", "."));
					}
					else
					{
						stringBuilder2.AppendFormat(",{0},{1}", stiGraphicsPathLinesGaugeGeom.Points[i].X.ToString().Replace(",", "."), stiGraphicsPathLinesGaugeGeom.Points[i].Y.ToString().Replace(",", "."));
					}
				}
				text += stringBuilder2;
			}
			else if (geom.Type == StiGaugeGeomType.GraphicsPathCloseFigure)
			{
				text += "z";
			}
			num++;
		}
		return text;
	}

	public static string GetArcPath(RectangleF rect, string path_, float startAngle, double sweepAngle, bool isSetStartPoint)
	{
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder(path_);
		PointF centerPoint = new PointF(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
		PointF pointF = new PointF(centerPoint.X - rect.Width / 2f, centerPoint.Y - rect.Height / 2f);
		PointF pointF2 = new PointF(centerPoint.X, centerPoint.Y - rect.Height / 2f);
		PointF pointF3 = new PointF(centerPoint.X + rect.Width / 2f, centerPoint.Y - rect.Height / 2f);
		PointF pointF4 = new PointF(centerPoint.X + rect.Width / 2f, centerPoint.Y);
		PointF pointF5 = new PointF(centerPoint.X + rect.Width / 2f, centerPoint.Y + rect.Height / 2f);
		PointF pointF6 = new PointF(centerPoint.X, centerPoint.Y + rect.Height / 2f);
		PointF pointF7 = new PointF(centerPoint.X - rect.Width / 2f, centerPoint.Y + rect.Height / 2f);
		PointF pointF8 = new PointF(centerPoint.X - rect.Width / 2f, centerPoint.Y);
		if (startAngle == 0f)
		{
			if (sweepAngle == 90.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0},{1}", pointF4.X.ToString().Replace(",", "."), pointF4.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("Q{0},{1},{2},{3}", pointF5.X.ToString().Replace(",", "."), pointF5.Y.ToString().Replace(",", "."), pointF6.X.ToString().Replace(",", "."), pointF6.Y.ToString().Replace(",", "."));
				flag = true;
			}
			else if (sweepAngle == 180.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0},{1}", pointF4.X.ToString().Replace(",", "."), pointF4.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("A{0},{1},90,1,1,{2},{3}", (rect.Height / 2f).ToString().Replace(",", "."), (rect.Width / 2f).ToString().Replace(",", "."), pointF8.X.ToString().Replace(",", "."), pointF8.Y.ToString().Replace(",", "."));
				flag = true;
			}
		}
		else if (startAngle == 90f)
		{
			if (sweepAngle == 90.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0},{1}", pointF6.X.ToString().Replace(",", "."), pointF6.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("Q{0},{1},{2},{3}", pointF7.X.ToString().Replace(",", "."), pointF7.Y.ToString().Replace(",", "."), pointF8.X.ToString().Replace(",", "."), pointF8.Y.ToString().Replace(",", "."));
				flag = true;
			}
			else if (sweepAngle == 180.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0}, {1}", pointF6.X.ToString().Replace(",", "."), pointF6.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("A{0},{1},90,1,1,{2},{3}", (rect.Height / 2f).ToString().Replace(",", "."), (rect.Width / 2f).ToString().Replace(",", "."), pointF2.X.ToString().Replace(",", "."), pointF2.Y.ToString().Replace(",", "."));
				flag = true;
			}
		}
		else if (startAngle == 180f)
		{
			if (sweepAngle == 90.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0},{1}", pointF8.X.ToString().Replace(",", "."), pointF8.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("Q{0},{1},{2},{3}", pointF.X.ToString().Replace(",", "."), pointF.Y.ToString().Replace(",", "."), pointF2.X.ToString().Replace(",", "."), pointF2.Y.ToString().Replace(",", "."));
				flag = true;
			}
			else if (sweepAngle == 180.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0},{1}", pointF8.X.ToString().Replace(",", "."), pointF8.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("A{0},{1},90,1,1,{2},{3}", (rect.Height / 2f).ToString().Replace(",", "."), (rect.Width / 2f).ToString().Replace(",", "."), pointF4.X.ToString().Replace(",", "."), pointF4.Y.ToString().Replace(",", "."));
				flag = true;
			}
		}
		else if (startAngle == 270f)
		{
			if (sweepAngle == 90.0)
			{
				stringBuilder.AppendFormat("M{0},{1}", pointF2.X.ToString().Replace(",", "."), pointF2.Y.ToString().Replace(",", "."));
				stringBuilder.AppendFormat("Q{0},{1},{2},{3}", pointF3.X.ToString().Replace(",", "."), pointF3.Y.ToString().Replace(",", "."), pointF4.X.ToString().Replace(",", "."), pointF4.Y.ToString().Replace(",", "."));
				flag = true;
			}
			else if (sweepAngle == 180.0)
			{
				if (isSetStartPoint)
				{
					stringBuilder.AppendFormat("M{0},{1}", pointF2.X.ToString().Replace(",", "."), pointF2.Y.ToString().Replace(",", "."));
				}
				stringBuilder.AppendFormat("A{0},{1},90,1,1,{2},{3}", (rect.Height / 2f).ToString().Replace(",", "."), (rect.Width / 2f).ToString().Replace(",", "."), pointF6.X.ToString().Replace(",", "."), pointF6.Y.ToString().Replace(",", "."));
				flag = true;
			}
		}
		if (!flag)
		{
			float num = Math.Min(rect.Width / 2f, rect.Height / 2f);
			double num2 = (double)startAngle * (Math.PI / 180.0);
			float x = (float)((double)centerPoint.X + (double)num * Math.Cos(num2));
			float y = (float)((double)centerPoint.Y + (double)num * Math.Sin(num2));
			PointF pointF9 = new PointF(x, y);
			bool flag2 = false;
			if (stringBuilder.Length == 0)
			{
				stringBuilder.AppendFormat("M{0},{1}", pointF9.X.ToString().Replace(",", "."), pointF9.Y.ToString().Replace(",", "."));
			}
			else
			{
				flag2 = true;
			}
			double num3 = Round(Math.Abs(sweepAngle / 90.0));
			double num4 = sweepAngle / num3;
			num2 = startAngle;
			int num5 = -1;
			while ((double)(++num5) < num3)
			{
				PointF[] array = ConvertArcToCubicBezier(centerPoint, num, num2, num4);
				if (flag2)
				{
					stringBuilder.AppendFormat("M{0},{1}", array[0].X.ToString().Replace(",", "."), array[0].Y.ToString().Replace(",", "."));
					flag2 = false;
				}
				stringBuilder.AppendFormat(" C{0},{1},{2},{3},{4},{5}", array[1].X.ToString().Replace(",", "."), array[1].Y.ToString().Replace(",", "."), array[2].X.ToString().Replace(",", "."), array[2].Y.ToString().Replace(",", "."), array[3].X.ToString().Replace(",", "."), array[3].Y.ToString().Replace(",", "."));
				num2 += num4;
			}
		}
		return stringBuilder.ToString();
	}

	private static PointF[] ConvertArcToCubicBezier(PointF centerPoint, double radius, double startAngle, double sweepAngle)
	{
		double num = startAngle * (Math.PI / 180.0);
		double num2 = sweepAngle * (Math.PI / 180.0);
		double num3 = num + num2;
		double num4 = (double)centerPoint.X + radius * Math.Cos(num);
		double num5 = (double)centerPoint.Y + radius * Math.Sin(num);
		double num6 = (double)centerPoint.X + radius * Math.Cos(num3);
		double num7 = (double)centerPoint.Y + radius * Math.Sin(num3);
		double num8 = radius * 1.3333333333333333 * Math.Tan(0.25 * num2);
		double num9 = Math.Atan(num8 / radius);
		double num10 = radius / Math.Cos(num9);
		num9 += num;
		double num11 = (double)centerPoint.X + num10 * Math.Cos(num9);
		double num12 = (double)centerPoint.Y + num10 * Math.Sin(num9);
		num9 = Math.Atan((0.0 - num8) / radius);
		num9 += num3;
		double num13 = (double)centerPoint.X + num10 * Math.Cos(num9);
		double num14 = (double)centerPoint.Y + num10 * Math.Sin(num9);
		return new PointF[4]
		{
			new PointF((float)num4, (float)num5),
			new PointF((float)num11, (float)num12),
			new PointF((float)num13, (float)num14),
			new PointF((float)num6, (float)num7)
		};
	}

	private static string AddArcPath(object arcSegment, string path)
	{
		float num = ((arcSegment is StiGraphicsPathArcGaugeGeom) ? (arcSegment as StiGraphicsPathArcGaugeGeom).X : (arcSegment as StiGraphicsArcGeometryGaugeGeom).Rect.X);
		float num2 = ((arcSegment is StiGraphicsPathArcGaugeGeom) ? (arcSegment as StiGraphicsPathArcGaugeGeom).Y : (arcSegment as StiGraphicsArcGeometryGaugeGeom).Rect.Y);
		float num3 = ((arcSegment is StiGraphicsPathArcGaugeGeom) ? (arcSegment as StiGraphicsPathArcGaugeGeom).Width : (arcSegment as StiGraphicsArcGeometryGaugeGeom).Rect.Width);
		float num4 = ((arcSegment is StiGraphicsPathArcGaugeGeom) ? (arcSegment as StiGraphicsPathArcGaugeGeom).Height : (arcSegment as StiGraphicsArcGeometryGaugeGeom).Rect.Height);
		float num5 = ((arcSegment is StiGraphicsPathArcGaugeGeom) ? (arcSegment as StiGraphicsPathArcGaugeGeom).StartAngle : (arcSegment as StiGraphicsArcGeometryGaugeGeom).StartAngle);
		float num6 = ((arcSegment is StiGraphicsPathArcGaugeGeom) ? (arcSegment as StiGraphicsPathArcGaugeGeom).SweepAngle : (arcSegment as StiGraphicsArcGeometryGaugeGeom).SweepAngle);
		StringBuilder stringBuilder = new StringBuilder();
		double num7 = num + num3 / 2f;
		double num8 = num2 + num4 / 2f;
		double num9 = num3 / 2f;
		double num10 = (double)num5 * Math.PI / 180.0;
		double num11 = num7 + num9 * Math.Cos(num10);
		double num12 = num8 + num9 * Math.Sin(num10);
		if (!path.StartsWith("M"))
		{
			stringBuilder.AppendFormat("M{0},{1}", num11.ToString().Replace(",", "."), num12.ToString().Replace(",", "."));
		}
		double num13 = Round(Math.Abs(num6 / 10f));
		double num14 = (double)num6 / num13;
		num10 = num5;
		for (int i = 0; (double)i < num13; i++)
		{
			PointF[] array = ConvertArcToCubicBezier(new RectangleF(num, num2, num3, num4), num10, num14);
			if (i == 0)
			{
				stringBuilder.AppendFormat(" C{0},{1},{2},{3},{4},{5}", array[1].X.ToString().Replace(",", "."), array[1].Y.ToString().Replace(",", "."), array[2].X.ToString().Replace(",", "."), array[2].Y.ToString().Replace(",", "."), array[3].X.ToString().Replace(",", "."), array[3].Y.ToString().Replace(",", "."));
			}
			else
			{
				stringBuilder.AppendFormat(",{0},{1},{2},{3},{4},{5}", array[1].X.ToString().Replace(",", "."), array[1].Y.ToString().Replace(",", "."), array[2].X.ToString().Replace(",", "."), array[2].Y.ToString().Replace(",", "."), array[3].X.ToString().Replace(",", "."), array[3].Y.ToString().Replace(",", "."));
			}
			num10 += num14;
		}
		if (arcSegment is StiGraphicsArcGeometryGaugeGeom)
		{
			StiGraphicsArcGeometryGaugeGeom stiGraphicsArcGeometryGaugeGeom = arcSegment as StiGraphicsArcGeometryGaugeGeom;
			num13 = Round(Math.Abs(num6 / 10f));
			num14 = (double)num6 / num13;
			double num15 = num9 - (double)(num3 * stiGraphicsArcGeometryGaugeGeom.StartWidth);
			double num16 = num9 - (double)(num3 * stiGraphicsArcGeometryGaugeGeom.EndWidth);
			if (num15 <= 0.0 || num16 <= 0.0)
			{
				return null;
			}
			double num17 = num15 - num16;
			double num18 = 1.0 / num13;
			double num19 = num13;
			num10 = num5 + num6;
			for (int j = 0; (double)j < num13; j++)
			{
				double radius = num15 - num17 * num19;
				double radius2 = num15 - num17 * (num19 + num18);
				PointF[] array2 = ConvertArcToCubicBezier(new PointF((float)num7, (float)num8), radius, radius2, num10, 0.0 - num14);
				if (j == 0)
				{
					stringBuilder.AppendFormat(" L{0},{1}", array2[0].X.ToString().Replace(",", "."), array2[0].Y.ToString().Replace(",", "."));
					stringBuilder.AppendFormat(" C{0},{1},{2},{3},{4},{5}", array2[1].X.ToString().Replace(",", "."), array2[1].Y.ToString().Replace(",", "."), array2[2].X.ToString().Replace(",", "."), array2[2].Y.ToString().Replace(",", "."), array2[3].X.ToString().Replace(",", "."), array2[3].Y.ToString().Replace(",", "."));
				}
				else
				{
					stringBuilder.AppendFormat(",{0},{1},{2},{3},{4},{5}", array2[1].X.ToString().Replace(",", "."), array2[1].Y.ToString().Replace(",", "."), array2[2].X.ToString().Replace(",", "."), array2[2].Y.ToString().Replace(",", "."), array2[3].X.ToString().Replace(",", "."), array2[3].Y.ToString().Replace(",", "."));
				}
				num10 -= num14;
				num19 -= num18;
			}
			stringBuilder.AppendFormat("z");
		}
		return stringBuilder.ToString();
	}

	private static string AddPiePath(StiPieGaugeGeom pieSegment, string path)
	{
		StringBuilder stringBuilder = new StringBuilder();
		double num = pieSegment.Rect.X + pieSegment.Rect.Width / 2f;
		double num2 = pieSegment.Rect.Y + pieSegment.Rect.Height / 2f;
		double num3 = pieSegment.Rect.Width / 2f;
		double num4 = (double)pieSegment.StartAngle * Math.PI / 180.0;
		double num5 = num + num3 * Math.Cos(num4);
		double num6 = num2 + num3 * Math.Sin(num4);
		stringBuilder.AppendFormat("M{0},{1}", num.ToString().Replace(",", "."), num2.ToString().Replace(",", "."));
		stringBuilder.AppendFormat("L{0},{1}", num5.ToString().Replace(",", "."), num6.ToString().Replace(",", "."));
		double num7 = Round(Math.Abs(pieSegment.SweepAngle / 90f));
		double num8 = (double)pieSegment.SweepAngle / num7;
		num4 = pieSegment.StartAngle;
		for (int i = 0; (double)i < num7; i++)
		{
			PointF[] array = ConvertArcToCubicBezier(pieSegment.Rect, num4, num8);
			for (int j = 1; j < array.Length - 1; j += 3)
			{
				if (j == 1)
				{
					stringBuilder.AppendFormat("C{0},{1},{2},{3},{4},{5}", array[j].X.ToString().Replace(",", "."), array[j].Y.ToString().Replace(",", "."), array[j + 1].X.ToString().Replace(",", "."), array[j + 1].Y.ToString().Replace(",", "."), array[j + 2].X.ToString().Replace(",", "."), array[j + 2].Y.ToString().Replace(",", "."));
				}
				else
				{
					stringBuilder.AppendFormat(",{0},{1},{2},{3},{4},{5}", array[j].X.ToString().Replace(",", "."), array[j].Y.ToString().Replace(",", "."), array[j + 1].X.ToString().Replace(",", "."), array[j + 1].Y.ToString().Replace(",", "."), array[j + 2].X.ToString().Replace(",", "."), array[j + 2].Y.ToString().Replace(",", "."));
				}
			}
			num4 += num8;
		}
		stringBuilder.AppendFormat("L{0},{1}", num.ToString().Replace(",", "."), num2.ToString().Replace(",", "."));
		return stringBuilder.ToString();
	}

	private static PointF[] ConvertArcToCubicBezier(RectangleF rect, double startAngle1, double sweepAngle1)
	{
		float num = rect.X + rect.Width / 2f;
		float num2 = rect.Y + rect.Height / 2f;
		float num3 = Math.Min(rect.Width / 2f, rect.Height / 2f);
		double num4 = startAngle1 * Math.PI / 180.0;
		double num5 = sweepAngle1 * Math.PI / 180.0;
		double num6 = (startAngle1 + sweepAngle1) * Math.PI / 180.0;
		float x = (float)((double)num + (double)num3 * Math.Cos(num4));
		float y = (float)((double)num2 + (double)num3 * Math.Sin(num4));
		float x2 = (float)((double)num + (double)num3 * Math.Cos(num6));
		float y2 = (float)((double)num2 + (double)num3 * Math.Sin(num6));
		double num7 = (double)(num3 * 4f / 3f) * Math.Tan(0.25 * num5);
		double num8 = Math.Atan(num7 / (double)num3);
		double num9 = (double)num3 / Math.Cos(num8);
		num8 += num4;
		float x3 = (float)((double)num + num9 * Math.Cos(num8));
		float y3 = (float)((double)num2 + num9 * Math.Sin(num8));
		num8 = Math.Atan((0.0 - num7) / (double)num3);
		num8 += num6;
		float x4 = (float)((double)num + num9 * Math.Cos(num8));
		float y4 = (float)((double)num2 + num9 * Math.Sin(num8));
		return new PointF[4]
		{
			new PointF(x, y),
			new PointF(x3, y3),
			new PointF(x4, y4),
			new PointF(x2, y2)
		};
	}

	private static PointF[] ConvertArcToCubicBezier(PointF centerPoint, double radius1, double radius2, double startAngle, double sweepAngle)
	{
		double num = startAngle * (Math.PI / 180.0);
		double num2 = sweepAngle * (Math.PI / 180.0);
		double num3 = num + num2;
		double num4 = (double)centerPoint.X + radius1 * Math.Cos(num);
		double num5 = (double)centerPoint.Y + radius1 * Math.Sin(num);
		double num6 = (double)centerPoint.X + radius2 * Math.Cos(num3);
		double num7 = (double)centerPoint.Y + radius2 * Math.Sin(num3);
		double num8 = (radius1 - radius2) / 3.0;
		radius1 -= num8;
		double num9 = radius1 * 1.3333333333333333 * Math.Tan(0.25 * num2);
		double num10 = Math.Atan(num9 / radius1);
		double num11 = radius1 / Math.Cos(num10);
		num10 += num;
		double num12 = (double)centerPoint.X + num11 * Math.Cos(num10);
		double num13 = (double)centerPoint.Y + num11 * Math.Sin(num10);
		num10 = Math.Atan((0.0 - num9) / radius1);
		num10 += num3;
		double num14 = (double)centerPoint.X + num11 * Math.Cos(num10);
		double num15 = (double)centerPoint.Y + num11 * Math.Sin(num10);
		return new PointF[4]
		{
			new PointF((float)num4, (float)num5),
			new PointF((float)num12, (float)num13),
			new PointF((float)num14, (float)num15),
			new PointF((float)num6, (float)num7)
		};
	}

	private static double Round(double value)
	{
		int num = (int)value;
		double num2 = value - (double)num;
		return (num2 > 0.0) ? (num + 1) : num;
	}

	public static void WriteText(XmlTextWriter writer, string text, Font font, StiBrush foreground, PointF pointF, float size)
	{
		StringBuilder stringBuilder = new StringBuilder();
		writer.WriteStartElement("text");
		stringBuilder.AppendFormat("text-anchor:{0};", "middle");
		writer.WriteAttributeString("dy", "0.9em");
		writer.WriteAttributeString("dx", (0.3 * (double)text.Length).ToString().Replace(",", ".") + "em");
		writer.WriteAttributeString("transform", "translate(" + pointF.X.ToString().Replace(",", ".") + ", " + pointF.Y.ToString().Replace(",", ".") + ") ");
		writer.WriteAttributeString("font-size", size.ToString().Replace(",", "."));
		writer.WriteAttributeString("font-family", font.FontFamily.Name);
		Color color = StiBrush.ToColor(foreground);
		stringBuilder.Append($"fill:#{color.R:X2}{color.G:X2}{color.B:X2};");
		if (color.A != byte.MaxValue)
		{
			stringBuilder.Append("fill-opacity:" + Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", ".") + ";");
		}
		writer.WriteAttributeString("style", stringBuilder.ToString());
		writer.WriteValue(text);
		writer.WriteEndElement();
	}

	public static string WriteFillBrush(XmlTextWriter writer, object brush, RectangleF rect)
	{
		if (brush is Color color)
		{
			return string.Format("fill:rgb({0},{1},{2});fill-opacity:{3};", color.R, color.G, color.B, Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", "."));
		}
		if (brush is StiGradientBrush)
		{
			string text = StiBrushSvgHelper.WriteGradientBrush(writer, brush, rect);
			return "fill:url(#" + text + ");";
		}
		if (brush is StiGlareBrush)
		{
			string text2 = StiBrushSvgHelper.WriteGlareBrush(writer, brush, rect);
			return "fill:url(#" + text2 + ");";
		}
		if (brush is StiGlassBrush)
		{
			string text3 = StiBrushSvgHelper.WriteGlassBrush(writer, brush, rect);
			return "fill:url(#" + text3 + ");";
		}
		if (brush is StiHatchBrush)
		{
			string text4 = StiBrushSvgHelper.WriteHatchBrush(writer, brush);
			return "fill:url(#" + text4 + ");";
		}
		if (brush is StiBrush)
		{
			Color color2 = StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null));
			return string.Format("fill:rgb({0},{1},{2});fill-opacity:{3};", color2.R, color2.G, color2.B, Math.Round((float)(int)color2.A / 255f, 3).ToString().Replace(",", "."));
		}
		return "fill-opacity:0;";
	}

	private static string WriteBorderStroke(XmlTextWriter writer, object brush, RectangleF rect)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		if (brush is Color color)
		{
			string text = $"stroke:rgb({color.R},{color.G},{color.B});";
			double num = Math.Round((float)(int)color.A / 255f, 3);
			if (num != 1.0)
			{
				text = text + "stroke-opacity:" + num.ToString().Replace(",", ".") + ";";
			}
			return text;
		}
		if (brush is StiSolidBrush)
		{
			StiSolidBrush val = (StiSolidBrush)brush;
			string text2 = $"stroke:rgb({val.Color.R},{val.Color.G},{val.Color.B});";
			double num2 = Math.Round((float)(int)val.Color.A / 255f, 3);
			if (num2 != 1.0)
			{
				text2 = text2 + "stroke-opacity:" + num2.ToString().Replace(",", ".") + ";";
			}
			return text2;
		}
		if (brush is StiGradientBrush)
		{
			string text3 = StiBrushSvgHelper.WriteGradientBrush(writer, brush, rect);
			return "fill:url(#" + text3 + ");";
		}
		if (brush is StiGlareBrush)
		{
			string text4 = StiBrushSvgHelper.WriteGlareBrush(writer, brush, rect);
			return "fill:url(#" + text4 + ");";
		}
		if (brush is StiGlassBrush)
		{
			string text5 = StiBrushSvgHelper.WriteGlassBrush(writer, brush, rect);
			return "fill:url(#" + text5 + ");";
		}
		if (brush is StiHatchBrush)
		{
			string text6 = StiBrushSvgHelper.WriteHatchBrush(writer, brush);
			return "fill:url(#" + text6 + ");";
		}
		if (brush is StiBrush)
		{
			Color color2 = StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null));
			string text7 = $"stroke:rgb({color2.R},{color2.G},{color2.B})";
			double num3 = Math.Round((float)(int)color2.A / 255f, 3);
			if (num3 != 1.0)
			{
				text7 = text7 + ";stroke-opacity:" + num3.ToString().Replace(",", ".");
			}
		}
		return "stroke-opacity:0";
	}

	private static RectangleF RectToRectangleF(object rect)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (rect is RectangleF)
		{
			return (RectangleF)rect;
		}
		if (rect is Rectangle)
		{
			return (Rectangle)rect;
		}
		if (rect is RectangleD val)
		{
			return new RectangleF((float)((RectangleD)(ref val)).X, (float)((RectangleD)(ref val)).Y, (float)((RectangleD)(ref val)).Width, (float)((RectangleD)(ref val)).Height);
		}
		return default(RectangleF);
	}
}
