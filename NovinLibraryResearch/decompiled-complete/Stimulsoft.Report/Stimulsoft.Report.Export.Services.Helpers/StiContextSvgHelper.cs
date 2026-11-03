using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Export.Services.Helpers;

public static class StiContextSvgHelper
{
	public static void WriteGeoms(XmlTextWriter writer, StiContext context, bool needAnimation, bool correctionText = true)
	{
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected I4, but got Unknown
		//IL_1eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef4: Invalid comparison between Unknown and I4
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Expected I4, but got Unknown
		//IL_24ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b4: Invalid comparison between Unknown and I4
		//IL_251b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2218: Unknown result type (might be due to invalid IL or missing references)
		//IL_256b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2571: Invalid comparison between Unknown and I4
		//IL_2336: Unknown result type (might be due to invalid IL or missing references)
		//IL_233c: Invalid comparison between Unknown and I4
		//IL_2379: Unknown result type (might be due to invalid IL or missing references)
		//IL_33f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_238d: Unknown result type (might be due to invalid IL or missing references)
		//IL_340d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Expected I4, but got Unknown
		//IL_3981: Unknown result type (might be due to invalid IL or missing references)
		//IL_3987: Unknown result type (might be due to invalid IL or missing references)
		//IL_399e: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_357e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_39bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3735: Unknown result type (might be due to invalid IL or missing references)
		//IL_3592: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef8: Invalid comparison between Unknown and I4
		//IL_3ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_39d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_39df: Unknown result type (might be due to invalid IL or missing references)
		//IL_3749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f19: Invalid comparison between Unknown and I4
		//IL_3ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f41: Invalid comparison between Unknown and I4
		//IL_0f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Invalid comparison between Unknown and I4
		//IL_3f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fae: Expected I4, but got Unknown
		//IL_40cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_40fb: Expected I4, but got Unknown
		//IL_42e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_42e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ea: Invalid comparison between Unknown and I4
		//IL_4303: Unknown result type (might be due to invalid IL or missing references)
		//IL_4309: Unknown result type (might be due to invalid IL or missing references)
		//IL_430b: Invalid comparison between Unknown and I4
		//IL_432b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4331: Unknown result type (might be due to invalid IL or missing references)
		//IL_4333: Invalid comparison between Unknown and I4
		//IL_434a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4350: Unknown result type (might be due to invalid IL or missing references)
		//IL_4352: Invalid comparison between Unknown and I4
		//IL_4502: Unknown result type (might be due to invalid IL or missing references)
		//IL_450c: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_45eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_45ee: Invalid comparison between Unknown and I4
		//IL_45f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f3: Invalid comparison between Unknown and I4
		//IL_45f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f8: Invalid comparison between Unknown and I4
		string text = string.Empty;
		double num = 0.0;
		bool flag = true;
		foreach (StiGeom geom in context.Geoms)
		{
			if (geom is StiAnimationGeom { Animation: not null } stiAnimationGeom && stiAnimationGeom.Animation.Duration.TotalMilliseconds + stiAnimationGeom.Animation.BeginTime.TotalMilliseconds > num)
			{
				num = stiAnimationGeom.Animation.Duration.TotalMilliseconds + stiAnimationGeom.Animation.BeginTime.TotalMilliseconds;
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		List<StiGeom> list = new List<StiGeom>();
		foreach (StiGeom geom2 in context.Geoms)
		{
			if (geom2 is StiPushTranslateTransformGeom)
			{
				list.Add(geom2);
				writer.WriteStartElement("g");
				num2 += (geom2 as StiPushTranslateTransformGeom).X;
				num3 += (geom2 as StiPushTranslateTransformGeom).Y;
				continue;
			}
			if (geom2 is StiPushRotateTransformGeom)
			{
				list.Add(geom2);
				writer.WriteStartElement("g");
				writer.WriteAttributeString("transform", $"rotate({DoubleToString((geom2 as StiPushRotateTransformGeom).Angle)} {DoubleToString(num2)} {DoubleToString(num3)})");
				continue;
			}
			if (geom2 is StiPopTransformGeom)
			{
				StiGeom stiGeom = list[list.Count - 1];
				if (stiGeom is StiPushTranslateTransformGeom stiPushTranslateTransformGeom)
				{
					num2 -= stiPushTranslateTransformGeom.X;
					num3 -= stiPushTranslateTransformGeom.Y;
				}
				list.Remove(stiGeom);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiClusteredBarSeriesAnimationGeom)
			{
				StiClusteredBarSeriesAnimationGeom stiClusteredBarSeriesAnimationGeom = geom2 as StiClusteredBarSeriesAnimationGeom;
				RectangleF rect = RectToRectangleF(stiClusteredBarSeriesAnimationGeom.ColumnRect);
				RectangleF rectangleF = ((stiClusteredBarSeriesAnimationGeom.Animation is StiColumnAnimation stiColumnAnimation) ? stiColumnAnimation.RectFrom : RectangleF.Empty);
				StringBuilder stringBuilder = new StringBuilder();
				if (stiClusteredBarSeriesAnimationGeom.Background != null)
				{
					stringBuilder.Append(WriteFillBrush(writer, stiClusteredBarSeriesAnimationGeom.Background, rect, num2, num3));
				}
				else
				{
					stringBuilder.Append("fill:none;");
				}
				if (CheckPenGeom(stiClusteredBarSeriesAnimationGeom.BorderPen))
				{
					string arg = $"{WriteBorderStroke(writer, stiClusteredBarSeriesAnimationGeom.BorderPen.Brush, rect)}";
					stringBuilder.Append($"{arg};stroke-width:{stiClusteredBarSeriesAnimationGeom.BorderPen.Thickness};");
				}
				writer.WriteStartElement("rect");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("shape-rendering", "crispEdges");
				if (needAnimation)
				{
					writer.WriteAttributeString("x", DoubleToString(rectangleF.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rectangleF.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rectangleF.Width));
					writer.WriteAttributeString("height", DoubleToString(rectangleF.Height));
					string text2 = "[\"y\", " + DoubleToString(rectangleF.Y + num3) + ", " + DoubleToString(rect.Y + num3) + ", \"\"]";
					text2 = text2 + ", [\"height\", " + DoubleToString(rectangleF.Height) + ", " + DoubleToString(rect.Height) + ", \"\"]";
					text2 = text2 + ", [\"x\", " + DoubleToString(rectangleF.X + num2) + ", " + DoubleToString(rect.X + num2) + ", \"\"]";
					text2 = text2 + ", [\"width\", " + DoubleToString(rectangleF.Width) + ", " + DoubleToString(rect.Width) + ", \"\"]";
					AddAnimation(writer, "[" + text2 + "]", stiClusteredBarSeriesAnimationGeom.Animation.BeginTime + stiClusteredBarSeriesAnimationGeom.Animation.BeginTimeCorrect, stiClusteredBarSeriesAnimationGeom.Animation.Duration);
				}
				else
				{
					writer.WriteAttributeString("x", DoubleToString(rect.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rect.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rect.Width));
					writer.WriteAttributeString("height", DoubleToString(rect.Height));
				}
				WriteInteraction(writer, stiClusteredBarSeriesAnimationGeom.Interaction);
				writer.WriteAttributeString("style", stringBuilder.ToString());
				WriteTooltip(writer, stiClusteredBarSeriesAnimationGeom.ToolTip);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiLabelAnimationGeom)
			{
				StiLabelAnimationGeom stiLabelAnimationGeom = geom2 as StiLabelAnimationGeom;
				StiLabelAnimation stiLabelAnimation = stiLabelAnimationGeom.Animation as StiLabelAnimation;
				StiPieLabelAnimation stiPieLabelAnimation = stiLabelAnimationGeom.Animation as StiPieLabelAnimation;
				RectangleF rect2 = RectToRectangleF(stiLabelAnimationGeom.Rectangle);
				if (stiLabelAnimation != null)
				{
					rect2 = stiLabelAnimation.LabelRect;
				}
				else if (stiPieLabelAnimation != null)
				{
					rect2 = stiPieLabelAnimation.RectLabelFrom;
				}
				StringFormat val = (StringFormat)(stiLabelAnimationGeom.StringFormat.IsGeneric ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)new StringFormat()));
				val.Alignment = stiLabelAnimationGeom.StringFormat.Alignment;
				val.FormatFlags = stiLabelAnimationGeom.StringFormat.FormatFlags;
				val.HotkeyPrefix = stiLabelAnimationGeom.StringFormat.HotkeyPrefix;
				val.LineAlignment = stiLabelAnimationGeom.StringFormat.LineAlignment;
				val.Trimming = stiLabelAnimationGeom.StringFormat.Trimming;
				PointF pointF = default(PointF);
				if (stiLabelAnimationGeom.Angle == 0f)
				{
					pointF = new PointF(rect2.X, rect2.Y);
					StringAlignment lineAlignment = val.LineAlignment;
					switch ((int)lineAlignment)
					{
					case 0:
						pointF = new PointF(rect2.X, rect2.Y);
						break;
					case 1:
						pointF = new PointF(rect2.X, rect2.Y + rect2.Height / 2f);
						break;
					case 2:
						pointF = new PointF(rect2.X, rect2.Y + rect2.Height);
						break;
					}
					stiLabelAnimationGeom.RotationMode = (StiRotationMode)1;
				}
				else
				{
					pointF = new PointF(rect2.X + rect2.Width / 2f, rect2.Y + rect2.Height / 2f);
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				if (stiLabelAnimationGeom.LabelBrush != null)
				{
					stringBuilder2.Append(WriteFillBrush(writer, stiLabelAnimationGeom.LabelBrush, rect2, num2, num3));
				}
				else
				{
					stringBuilder2.Append("fill:none;");
				}
				if (stiLabelAnimationGeom.DrawBorder && CheckPenGeom(stiLabelAnimationGeom.PenBorder))
				{
					string arg2 = $"{WriteBorderStroke(writer, stiLabelAnimationGeom.PenBorder.Brush, rect2)}";
					stringBuilder2.Append($"{arg2};stroke-width:{stiLabelAnimationGeom.PenBorder.Thickness};");
				}
				writer.WriteStartElement("rect");
				float num4 = 0f;
				float num5 = 0f;
				switch (stiLabelAnimationGeom.RotationMode)
				{
				case 1L:
					num5 = rect2.Height / 2f;
					break;
				case 2L:
					num5 = rect2.Height;
					break;
				case 3L:
					num4 = rect2.Width / 2f;
					break;
				case 4L:
					num4 = rect2.Width / 2f;
					num5 = rect2.Height / 2f;
					break;
				case 5L:
					num4 = rect2.Width / 2f;
					num5 = rect2.Height;
					break;
				case 6L:
					num4 = rect2.Width;
					break;
				case 7L:
					num4 = rect2.Width;
					num5 = rect2.Height / 2f;
					break;
				case 8L:
					num4 = rect2.Width;
					num5 = rect2.Height;
					break;
				}
				writer.WriteAttributeString("transform", "translate(" + DoubleToString(pointF.X + num2 - num4) + ", " + DoubleToString(pointF.Y + num3 - num5) + ") rotate(" + DoubleToString(stiLabelAnimationGeom.Angle) + " " + DoubleToString(num4) + "," + DoubleToString(num5) + ")");
				writer.WriteAttributeString("width", DoubleToString(rect2.Width));
				writer.WriteAttributeString("height", DoubleToString(rect2.Height));
				writer.WriteAttributeString("style", stringBuilder2.ToString());
				writer.WriteAttributeString("shape-rendering", "crispEdges");
				writer.WriteAttributeString("opacity", (stiLabelAnimation != null || stiPieLabelAnimation != null) ? "1" : "0");
				writer.WriteAttributeString("fill", "rgba(0,0,0,0)");
				if (stiLabelAnimation != null)
				{
					float num6 = (float)stiLabelAnimationGeom.Rectangle.X - stiLabelAnimation.LabelRect.X;
					float num7 = (float)stiLabelAnimationGeom.Rectangle.Y - stiLabelAnimation.LabelRect.Y;
					AddAnimation(writer, "[[\"translate\",\"" + DoubleToString(pointF.X + num2) + ":" + DoubleToString(pointF.Y + num3) + "\",\"" + DoubleToString(pointF.X + num2 + num6) + ":" + DoubleToString(pointF.Y + num3 + num7) + "\",\" rotate(" + DoubleToString(stiLabelAnimationGeom.Angle) + " " + DoubleToString(rect2.Width / 2f) + "," + DoubleToString(rect2.Height / 2f) + ")\"]]", stiLabelAnimation.BeginTime, stiLabelAnimation.Duration);
				}
				else if (stiPieLabelAnimation != null)
				{
					float num8 = (float)stiLabelAnimationGeom.Rectangle.X - stiPieLabelAnimation.RectLabel.X;
					float num9 = (float)stiLabelAnimationGeom.Rectangle.Y - stiPieLabelAnimation.RectLabel.Y;
					AddAnimation(writer, "[[\"translate\",\"" + DoubleToString(pointF.X + num2) + ":" + DoubleToString(pointF.Y + num3) + "\",\"" + DoubleToString(pointF.X + num2 + num8) + ":" + DoubleToString(pointF.Y + num3 + num9) + "\",\" rotate(" + DoubleToString(stiLabelAnimationGeom.Angle) + " " + DoubleToString(rect2.Width / 2f) + "," + DoubleToString(rect2.Height / 2f) + ")\"]]", stiPieLabelAnimation.BeginTime, stiPieLabelAnimation.Duration);
				}
				else
				{
					AddAnimation(writer, "[[\"opacity\", 0, 1, \"\"]]", TimeSpan.FromMilliseconds(num), TimeSpan.FromMilliseconds(500.0));
				}
				writer.WriteEndElement();
				float num10 = (correctionText ? (stiLabelAnimationGeom.Font.FontSize * 4f / 3f) : stiLabelAnimationGeom.Font.FontSize);
				writer.WriteStartElement("text");
				switch (stiLabelAnimationGeom.RotationMode)
				{
				case 0L:
					writer.WriteAttributeString("dy", "0.5em");
					break;
				case 2L:
					stringBuilder2.Append("text-anchor:middle;");
					writer.WriteAttributeString("dy", "1em");
					break;
				case 3L:
					stringBuilder2.Append("text-anchor:middle;");
					if (correctionText)
					{
						writer.WriteAttributeString("dy", "0.5em");
					}
					else
					{
						writer.WriteAttributeString("dy", "0.3em");
					}
					break;
				case 4L:
					stringBuilder2.Append("text-anchor:middle;");
					break;
				case 5L:
					stringBuilder2.Append("text-anchor:end;");
					writer.WriteAttributeString("dy", "1em");
					break;
				case 6L:
					stringBuilder2.Append("text-anchor:end;");
					writer.WriteAttributeString("dy", "0.5em");
					break;
				case 7L:
					stringBuilder2.Append("text-anchor:end;");
					break;
				default:
					writer.WriteAttributeString("dy", "1em");
					break;
				case 1L:
					break;
				}
				if (stiLabelAnimationGeom.Angle == 0f && stiLabelAnimationGeom.Rectangle.Width == 0)
				{
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					writer.WriteAttributeString("x", DoubleToString(pointF.X + num2));
					writer.WriteAttributeString("y", DoubleToString(pointF.Y + num3));
				}
				else
				{
					writer.WriteAttributeString("transform", $"translate({DoubleToString(pointF.X + num2)}, {DoubleToString(pointF.Y + num3)}) rotate({DoubleToString(stiLabelAnimationGeom.Angle)} 0,0)");
				}
				writer.WriteAttributeString("font-size", DoubleToString(num10));
				writer.WriteAttributeString("font-family", stiLabelAnimationGeom.Font.FontName);
				if ((stiLabelAnimationGeom.Font.FontStyle & 1) > 0)
				{
					writer.WriteAttributeString("font-weight", "bold");
				}
				if ((stiLabelAnimationGeom.Font.FontStyle & 2) > 0)
				{
					writer.WriteAttributeString("font-style", "italic");
				}
				string text3 = "";
				if ((stiLabelAnimationGeom.Font.FontStyle & 4) > 0)
				{
					text3 += "underline";
				}
				if ((stiLabelAnimationGeom.Font.FontStyle & 8) > 0)
				{
					text3 += " line-through";
				}
				if (!string.IsNullOrEmpty(text3))
				{
					writer.WriteAttributeString("text-decoration", text3);
				}
				Color color = ((stiLabelAnimationGeom.TextBrush is Color) ? ((Color)stiLabelAnimationGeom.TextBrush) : StiBrush.ToColor((StiBrush)/*isinst with value type is only supported in some contexts*/));
				stringBuilder2.Append($"fill:#{color.R:X2}{color.G:X2}{color.B:X2};");
				if (color.A != byte.MaxValue)
				{
					stringBuilder2.Append($"fill-opacity:{DoubleToString(Math.Round((float)(int)color.A / 255f, 3))};");
				}
				stringBuilder2.Append("pointer-events: none");
				writer.WriteAttributeString("style", stringBuilder2.ToString());
				writer.WriteAttributeString("opacity", (stiLabelAnimation != null || stiPieLabelAnimation != null) ? "1" : "0");
				if (stiLabelAnimation != null)
				{
					AddAnimation(writer, string.Format("[[\"translate\",\"" + DoubleToString(stiLabelAnimation.LabelRect.X + num2) + ":" + DoubleToString(stiLabelAnimation.LabelRect.Y + num3) + "\",\"" + DoubleToString(rect2.X + num2) + ":" + DoubleToString(rect2.Y + num3) + "\",\" rotate({0} 0, 0)\"]]", DoubleToString(stiLabelAnimationGeom.Angle)), stiLabelAnimation.BeginTime, stiLabelAnimation.Duration);
					if (stiLabelAnimation.ValueFrom.HasValue && stiLabelAnimation.Value.HasValue)
					{
						decimal d = (decimal)stiLabelAnimation.ValueFrom.Value - (decimal)stiLabelAnimation.Value.Value;
						int num11 = BitConverter.GetBytes(decimal.GetBits(d)[3])[2];
						AddAnimation(writer, "[[\"value\", " + DoubleToString(stiLabelAnimation.ValueFrom.Value) + ", " + DoubleToString(stiLabelAnimation.Value.Value) + ", \"" + num11 + "\"]]", stiLabelAnimation.BeginTime, stiLabelAnimation.Duration, "a1");
					}
				}
				else if (stiPieLabelAnimation != null)
				{
					AddAnimation(writer, string.Format("[[\"translate\",\"" + DoubleToString(stiPieLabelAnimation.RectLabelFrom.X + num2) + ":" + DoubleToString(stiPieLabelAnimation.RectLabelFrom.Y + num3) + "\",\"" + DoubleToString(rect2.X + num2) + ":" + DoubleToString(rect2.Y + num3) + "\",\" rotate({0} 0, 0)\"]]", DoubleToString(stiLabelAnimationGeom.Angle)), stiPieLabelAnimation.BeginTime, stiPieLabelAnimation.Duration);
					if (stiPieLabelAnimation.ValueFrom.HasValue && stiPieLabelAnimation.Value.HasValue)
					{
						decimal d2 = (decimal)stiPieLabelAnimation.ValueFrom.Value - (decimal)stiPieLabelAnimation.Value.Value;
						int num12 = BitConverter.GetBytes(decimal.GetBits(d2)[3])[2];
						AddAnimation(writer, "[[\"value\", " + DoubleToString(stiPieLabelAnimation.ValueFrom.Value) + ", " + DoubleToString(stiPieLabelAnimation.Value.Value) + ", \"" + num12 + "\", \"" + stiLabelAnimationGeom.Text + "\" ]]", stiPieLabelAnimation.BeginTime, stiPieLabelAnimation.Duration, "a1");
					}
				}
				else
				{
					AddAnimation(writer, "[[\"opacity\", 0, 1, \"\"]]", TimeSpan.FromMilliseconds(num), TimeSpan.FromMilliseconds(500.0));
				}
				if (stiLabelAnimation != null && stiLabelAnimation.ValueFrom.HasValue)
				{
					writer.WriteValue(DoubleToString(stiLabelAnimation.ValueFrom.Value));
				}
				else if (stiPieLabelAnimation != null && stiPieLabelAnimation.ValueFrom.HasValue)
				{
					writer.WriteValue(DoubleToString(stiPieLabelAnimation.ValueFrom.Value));
				}
				else
				{
					writer.WriteValue(stiLabelAnimationGeom.Text);
				}
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiPushClipGeom)
			{
				StiPushClipGeom stiPushClipGeom = geom2 as StiPushClipGeom;
				RectangleF rectangleF2 = RectToRectangleF(stiPushClipGeom.ClipRectangle);
				if (rectangleF2.Width > 0f && rectangleF2.Height > 0f)
				{
					string text4 = "s" + StiGuidUtils.NewGuid();
					writer.WriteStartElement("defs");
					writer.WriteStartElement("clipPath");
					writer.WriteAttributeString("id", text4);
					writer.WriteStartElement("rect");
					writer.WriteAttributeString("x", DoubleToString(rectangleF2.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rectangleF2.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rectangleF2.Width));
					writer.WriteAttributeString("height", DoubleToString(rectangleF2.Height));
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					text = text4;
				}
				continue;
			}
			if (geom2 is StiPushClipPathGeom)
			{
				StiPushClipPathGeom stiPushClipPathGeom = geom2 as StiPushClipPathGeom;
				StringBuilder animatedPath;
				TimeSpan? duration;
				string pathData = GetPathData(stiPushClipPathGeom.Geoms, num2, num3, out animatedPath, out duration);
				string text5 = "s" + StiGuidUtils.NewGuid();
				writer.WriteStartElement("defs");
				writer.WriteStartElement("clipPath");
				writer.WriteAttributeString("id", text5);
				writer.WriteStartElement("path");
				writer.WriteAttributeString("d", pathData);
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				text = text5;
				continue;
			}
			if (geom2 is StiPopClipGeom)
			{
				text = string.Empty;
				continue;
			}
			if (geom2 is StiShadowAnimationGeom)
			{
				StiShadowAnimationGeom stiShadowAnimationGeom = geom2 as StiShadowAnimationGeom;
				RectangleF rectangleF3 = RectToRectangleF(stiShadowAnimationGeom.Rect);
				string text6 = "s" + StiGuidUtils.NewGuid();
				writer.WriteStartElement("defs");
				writer.WriteStartElement("filter");
				writer.WriteAttributeString("id", text6);
				writer.WriteAttributeString("x", "0");
				writer.WriteAttributeString("y", "0");
				writer.WriteAttributeString("width", "200%");
				writer.WriteAttributeString("height", "200%");
				writer.WriteStartElement("feOffset");
				writer.WriteAttributeString("result", "offOut");
				writer.WriteAttributeString("in", "SourceGraphic");
				writer.WriteAttributeString("dx", "1.111111111111111");
				writer.WriteAttributeString("dy", "1.111111111111111");
				writer.WriteEndElement();
				writer.WriteStartElement("feColorMatrix");
				writer.WriteAttributeString("result", "matrixOut");
				writer.WriteAttributeString("in", "offOut");
				writer.WriteAttributeString("type", "matrix");
				writer.WriteAttributeString("values", "0.58 0 0 0 0 0 0.58 0 0 0 0 0 0.58 0 0 0 0 0 1 0");
				writer.WriteEndElement();
				writer.WriteStartElement("feGaussianBlur");
				writer.WriteAttributeString("result", "blurOut");
				writer.WriteAttributeString("in", "matrixOut");
				writer.WriteAttributeString("stdDeviation", "1.111111111111111");
				writer.WriteEndElement();
				writer.WriteStartElement("feBlend");
				writer.WriteAttributeString("mode", "normal");
				writer.WriteAttributeString("in", "SourceGraphic");
				writer.WriteAttributeString("in2", "blurOut");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteStartElement("rect");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("x", DoubleToString(rectangleF3.X + num2));
				writer.WriteAttributeString("y", DoubleToString(rectangleF3.Y + num3));
				writer.WriteAttributeString("height", DoubleToString(rectangleF3.Height));
				writer.WriteAttributeString("width", DoubleToString(rectangleF3.Width));
				writer.WriteAttributeString("fill", "rgb(150,150,150)");
				writer.WriteAttributeString("filter", $"url(#{text6})");
				writer.WriteAttributeString("rx", DoubleToString(stiShadowAnimationGeom.RadiusX));
				writer.WriteAttributeString("ry", DoubleToString(stiShadowAnimationGeom.RadiusY));
				if (needAnimation)
				{
					writer.WriteAttributeString("opacity", "0");
					AddAnimation(writer, "[[\"opacity\", 0, 1, \"\"]]", stiShadowAnimationGeom.Animation.BeginTime, stiShadowAnimationGeom.Animation.Duration);
				}
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiBorderAnimationGeom)
			{
				StiBorderAnimationGeom stiBorderAnimationGeom = geom2 as StiBorderAnimationGeom;
				RectangleF rect3 = RectToRectangleF(stiBorderAnimationGeom.Rect);
				if (rect3.Width > 0f && rect3.Height > 0f)
				{
					StiOpacityAnimation stiOpacityAnimation = stiBorderAnimationGeom.Animation as StiOpacityAnimation;
					StringBuilder stringBuilder3 = new StringBuilder();
					if (stiBorderAnimationGeom.Background != null)
					{
						stringBuilder3.Append(WriteFillBrush(writer, stiBorderAnimationGeom.Background, rect3, num2, num3));
					}
					else
					{
						stringBuilder3.Append("fill:none;");
					}
					if (CheckPenGeom(stiBorderAnimationGeom.BorderPen))
					{
						string arg3 = $"{WriteBorderStroke(writer, stiBorderAnimationGeom.BorderPen.Brush, rect3)}";
						stringBuilder3.Append($"{arg3};stroke-width:{stiBorderAnimationGeom.BorderPen.Thickness};");
					}
					writer.WriteStartElement("rect");
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					writer.WriteAttributeString("x", DoubleToString(rect3.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rect3.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rect3.Width));
					writer.WriteAttributeString("height", DoubleToString(rect3.Height));
					writer.WriteAttributeString("style", stringBuilder3.ToString());
					writer.WriteAttributeString("shape-rendering", "crispEdges");
					if (stiOpacityAnimation != null)
					{
						writer.WriteAttributeString("opacity", "0");
						AddAnimation(writer, "[[\"opacity\", 0 , 1,\"\"]]", stiOpacityAnimation.BeginTime, stiOpacityAnimation.Duration);
					}
					WriteInteraction(writer, stiBorderAnimationGeom.Interaction);
					WriteTooltip(writer, stiBorderAnimationGeom.ToolTip);
					writer.WriteEndElement();
				}
				continue;
			}
			if (geom2 is StiClusteredColumnSeriesAnimationGeom)
			{
				StiClusteredColumnSeriesAnimationGeom stiClusteredColumnSeriesAnimationGeom = geom2 as StiClusteredColumnSeriesAnimationGeom;
				RectangleF rect4 = RectToRectangleF(stiClusteredColumnSeriesAnimationGeom.ColumnRect);
				RectangleF rectangleF4 = ((stiClusteredColumnSeriesAnimationGeom.Animation is StiColumnAnimation stiColumnAnimation2) ? stiColumnAnimation2.RectFrom : RectangleF.Empty);
				StringBuilder stringBuilder4 = new StringBuilder();
				if (stiClusteredColumnSeriesAnimationGeom.Background != null)
				{
					stringBuilder4.Append(WriteFillBrush(writer, stiClusteredColumnSeriesAnimationGeom.Background, rect4, num2, num3));
				}
				else
				{
					stringBuilder4.Append("fill:none;");
				}
				if (CheckPenGeom(stiClusteredColumnSeriesAnimationGeom.BorderPen))
				{
					string arg4 = $"{WriteBorderStroke(writer, stiClusteredColumnSeriesAnimationGeom.BorderPen.Brush, rect4)}";
					stringBuilder4.Append($"{arg4};stroke-width:{stiClusteredColumnSeriesAnimationGeom.BorderPen.Thickness};");
				}
				writer.WriteStartElement("rect");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("shape-rendering", "crispEdges");
				if (needAnimation)
				{
					writer.WriteAttributeString("x", DoubleToString(rectangleF4.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rectangleF4.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rectangleF4.Width));
					writer.WriteAttributeString("height", DoubleToString(rectangleF4.Height));
					string text7 = "[\"y\", " + DoubleToString(rectangleF4.Y + num3) + ", " + DoubleToString(rect4.Y + num3) + ", \"\"]";
					text7 = text7 + ", [\"height\", " + DoubleToString(rectangleF4.Height) + ", " + DoubleToString(rect4.Height) + ", \"\"]";
					text7 = text7 + ", [\"x\", " + DoubleToString(rectangleF4.X + num2) + ", " + DoubleToString(rect4.X + num2) + ", \"\"]";
					text7 = text7 + ", [\"width\", " + DoubleToString(rectangleF4.Width) + ", " + DoubleToString(rect4.Width) + ", \"\"]";
					AddAnimation(writer, "[" + text7 + "]", stiClusteredColumnSeriesAnimationGeom.Animation.BeginTime + stiClusteredColumnSeriesAnimationGeom.Animation.BeginTimeCorrect, stiClusteredColumnSeriesAnimationGeom.Animation.Duration);
				}
				else
				{
					writer.WriteAttributeString("x", DoubleToString(rect4.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rect4.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rect4.Width));
					writer.WriteAttributeString("height", DoubleToString(rect4.Height));
				}
				WriteInteraction(writer, stiClusteredColumnSeriesAnimationGeom.Interaction);
				writer.WriteAttributeString("style", stringBuilder4.ToString());
				WriteTooltip(writer, stiClusteredColumnSeriesAnimationGeom.ToolTip);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiLinesAnimationGeom)
			{
				StiLinesAnimationGeom stiLinesAnimationGeom = geom2 as StiLinesAnimationGeom;
				if (!CheckPenGeom(stiLinesAnimationGeom.Pen))
				{
					continue;
				}
				string text8 = "g" + StiGuidUtils.NewGuid();
				if ((int)stiLinesAnimationGeom.Animation.Type == 2)
				{
					Color color2 = (Color)stiLinesAnimationGeom.Pen.Brush;
					writer.WriteStartElement("g");
					writer.WriteStartElement("defs");
					writer.WriteStartElement("linearGradient");
					writer.WriteAttributeString("id", text8);
					writer.WriteAttributeString("x1", "0%");
					writer.WriteAttributeString("y1", "0%");
					writer.WriteAttributeString("x2", "100%");
					writer.WriteAttributeString("y2", "0%");
					writer.WriteStartElement("stop");
					writer.WriteAttributeString("offset", "0%");
					writer.WriteAttributeString("stop-color", $"rgba({color2.R},{color2.G},{color2.B},{DoubleToString(Math.Round((float)(int)color2.A / 255f, 3))})");
					writer.WriteAttributeString("stop-opacity", "1");
					writer.WriteAttributeString("style", "x: 0px;");
					AddAnimation(writer, "[[\"offset\", 0 , 100,\"%\"]]", stiLinesAnimationGeom.Animation.BeginTime, stiLinesAnimationGeom.Animation.Duration);
					writer.WriteEndElement();
					writer.WriteStartElement("stop");
					writer.WriteAttributeString("offset", "0%");
					writer.WriteAttributeString("stop-color", "transparent");
					writer.WriteAttributeString("stop-opacity", "0");
					writer.WriteAttributeString("style", "x: 00px;");
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				StiPointsAnimation stiPointsAnimation = stiLinesAnimationGeom.Animation as StiPointsAnimation;
				writer.WriteStartElement("polyline");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				StringBuilder stringBuilder5 = new StringBuilder();
				StringBuilder stringBuilder6 = new StringBuilder();
				for (int i = 0; stiLinesAnimationGeom.Points.Length > i; i++)
				{
					PointF pointF2 = stiLinesAnimationGeom.Points[i];
					float num13 = 0f;
					if (i == stiLinesAnimationGeom.Points.Length - 1)
					{
						num13 = 0.0001f * (float)i;
					}
					if (stiPointsAnimation == null)
					{
						stringBuilder5.AppendFormat("{0},{1} ", DoubleToString(pointF2.X + num2), DoubleToString(pointF2.Y + num3 + num13));
						continue;
					}
					stringBuilder5.AppendFormat("{0},{1} ", DoubleToString(stiPointsAnimation.PointsFrom[i].X + num2), DoubleToString(stiPointsAnimation.PointsFrom[i].Y + num3 + num13));
					stringBuilder6.AppendFormat("{0},{1} ", DoubleToString(pointF2.X + num2), DoubleToString(pointF2.Y + num3 + num13));
				}
				writer.WriteAttributeString("fill", "none");
				writer.WriteAttributeString("points", stringBuilder5.ToString());
				if ((int)stiLinesAnimationGeom.Animation.Type == 0 || stiPointsAnimation != null)
				{
					string arg5 = $"{WriteBorderStroke(writer, stiLinesAnimationGeom.Pen.Brush, default(RectangleF))}";
					StringBuilder stringBuilder7 = new StringBuilder();
					stringBuilder7.AppendFormat("{0};stroke-width:{1};", arg5, DoubleToString(stiLinesAnimationGeom.Pen.Thickness));
					writer.WriteAttributeString("style", stringBuilder7.ToString());
					if (stiPointsAnimation == null)
					{
						writer.WriteAttributeString("opacity", "0");
						AddAnimation(writer, "[[\"opacity\", 0 , 1,\"\"]]", stiLinesAnimationGeom.Animation.BeginTime, stiLinesAnimationGeom.Animation.Duration);
					}
					else
					{
						AddAnimation(writer, "[[\"points\", \"" + stringBuilder5.ToString() + "\", \"" + stringBuilder6.ToString() + "\",\"\"]]", stiLinesAnimationGeom.Animation.BeginTime, stiLinesAnimationGeom.Animation.Duration);
					}
				}
				else if ((int)stiLinesAnimationGeom.Animation.Type == 2)
				{
					writer.WriteAttributeString("stroke-width", DoubleToString(stiLinesAnimationGeom.Pen.Thickness));
					writer.WriteAttributeString("stroke", $"url(#{text8})");
				}
				if ((int)stiLinesAnimationGeom.Pen.PenStyle != 0)
				{
					writer.WriteAttributeString("stroke-dasharray", StiSvgHelper.GetLineStyleDash(stiLinesAnimationGeom.Pen.PenStyle, stiLinesAnimationGeom.Pen.Thickness));
				}
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiEllipseAnimationGeom)
			{
				StiEllipseAnimationGeom stiEllipseAnimationGeom = geom2 as StiEllipseAnimationGeom;
				RectangleF rect5 = RectToRectangleF(stiEllipseAnimationGeom.Rect);
				string empty = string.Empty;
				string empty2 = string.Empty;
				empty = ((stiEllipseAnimationGeom.Background == null) ? "fill:none;" : WriteFillBrush(writer, stiEllipseAnimationGeom.Background, rect5, num2, num3));
				if (CheckPenGeom(stiEllipseAnimationGeom.BorderPen))
				{
					string arg6 = $"{WriteBorderStroke(writer, stiEllipseAnimationGeom.BorderPen.Brush, rect5)}";
					empty += $"{arg6};stroke-width:{DoubleToString(stiEllipseAnimationGeom.BorderPen.Thickness)};";
				}
				writer.WriteStartElement("ellipse");
				writer.WriteAttributeString("rx", DoubleToString(rect5.Width / 2f));
				writer.WriteAttributeString("ry", DoubleToString(rect5.Height / 2f));
				if ((int)stiEllipseAnimationGeom.Animation.Type != 1)
				{
					writer.WriteAttributeString("cx", DoubleToString(rect5.X + num2 + rect5.Width / 2f));
					writer.WriteAttributeString("cy", DoubleToString(rect5.Y + num3 + rect5.Height / 2f));
				}
				writer.WriteAttributeString("style", empty);
				if ((int)stiEllipseAnimationGeom.Animation.Type == 0)
				{
					writer.WriteAttributeString("opacity", "0");
					AddAnimation(writer, "[[\"opacity\", 0 , 1,\"\"]]", stiEllipseAnimationGeom.Animation.BeginTime, stiEllipseAnimationGeom.Animation.Duration);
				}
				else if ((int)stiEllipseAnimationGeom.Animation.Type == 1)
				{
					writer.WriteAttributeString("transform", "scale(0)");
					AddAnimation(writer, $"[[\"transform\", 0 , 1,\")\",\"translate({DoubleToString(rect5.X + num2 + rect5.Width / 2f)},{DoubleToString(rect5.Y + num3 + rect5.Height / 2f)}) scale(\"]]", stiEllipseAnimationGeom.Animation.BeginTime, stiEllipseAnimationGeom.Animation.Duration);
				}
				WriteInteraction(writer, stiEllipseAnimationGeom.Interaction);
				WriteTooltip(writer, stiEllipseAnimationGeom.ToolTip);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiPathElementAnimationGeom stiPathElementAnimationGeom)
			{
				StiPathElementAnimationGeom stiPathElementAnimationGeom2 = geom2 as StiPathElementAnimationGeom;
				RectangleF rect6 = RectToRectangleF(stiPathElementAnimationGeom2.Rect);
				string empty3 = string.Empty;
				string empty4 = string.Empty;
				empty3 = ((stiPathElementAnimationGeom2.Background == null) ? "fill:none;" : WriteFillBrush(writer, stiPathElementAnimationGeom2.Background, rect6, num2, num3));
				if (CheckPenGeom(stiPathElementAnimationGeom2.BorderPen))
				{
					string arg7 = $"{WriteBorderStroke(writer, stiPathElementAnimationGeom2.BorderPen.Brush, rect6)}";
					empty3 += $"{arg7};stroke-width:{DoubleToString(stiPathElementAnimationGeom2.BorderPen.Thickness)};";
				}
				StringBuilder animatedPath2;
				TimeSpan? duration2;
				string pathData2 = GetPathData(stiPathElementAnimationGeom2.PathGeoms, num2, num3, out animatedPath2, out duration2, stiPathElementAnimationGeom.Animation);
				writer.WriteStartElement("path");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("d", pathData2);
				writer.WriteAttributeString("style", empty3);
				if (animatedPath2.Length == 0)
				{
					if (stiPathElementAnimationGeom2.Animation != null)
					{
						writer.WriteAttributeString("opacity", "0");
						AddAnimation(writer, "[[\"opacity\", 0 , 1,\"\"]]", stiPathElementAnimationGeom2.Animation.BeginTime, stiPathElementAnimationGeom2.Animation.Duration);
					}
				}
				else if (stiPathElementAnimationGeom2.PathGeoms.Count == 1 && stiPathElementAnimationGeom2.PathGeoms[0] is StiPieSegmentGeom)
				{
					AddAnimation(writer, "[[\"pie\", \"" + animatedPath2?.ToString() + "\", 1,\"\"]]", TimeSpan.FromMilliseconds(0.0), duration2.Value);
				}
				else if (stiPathElementAnimationGeom2.PathGeoms.Count == 4 && stiPathElementAnimationGeom2.Animation is StiPieSegmentAnimation)
				{
					AddAnimation(writer, "[[\"doughnut\", \"" + animatedPath2?.ToString() + "\", 1,\"\"]]", TimeSpan.FromMilliseconds(0.0), duration2.Value);
				}
				else
				{
					AddAnimation(writer, "[[\"path\", \"" + animatedPath2?.ToString() + "\", 1,\"\"]]", TimeSpan.FromMilliseconds(0.0), duration2.Value);
				}
				WriteInteraction(writer, stiPathElementAnimationGeom2.Interaction);
				WriteTooltip(writer, stiPathElementAnimationGeom2.ToolTip);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiPathAnimationGeom)
			{
				StiPathAnimationGeom stiPathAnimationGeom = geom2 as StiPathAnimationGeom;
				RectangleF rect7 = RectToRectangleF(stiPathAnimationGeom.Rect);
				string empty5 = string.Empty;
				string empty6 = string.Empty;
				empty5 = ((stiPathAnimationGeom.Background == null) ? "fill:none;" : WriteFillBrush(writer, stiPathAnimationGeom.Background, rect7, num2, num3));
				if (CheckPenGeom(stiPathAnimationGeom.Pen))
				{
					string arg8 = $"{WriteBorderStroke(writer, stiPathAnimationGeom.Pen.Brush, rect7)}";
					empty5 += $"{arg8};stroke-width:{DoubleToString(stiPathAnimationGeom.Pen.Thickness)};";
				}
				StringBuilder animatedPath3;
				TimeSpan? duration3;
				string pathData3 = GetPathData(stiPathAnimationGeom.Geoms, num2, num3, out animatedPath3, out duration3);
				writer.WriteStartElement("path");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("d", pathData3);
				writer.WriteAttributeString("style", empty5);
				if (stiPathAnimationGeom.Animation != null)
				{
					writer.WriteAttributeString("opacity", "0");
					AddAnimation(writer, "[[\"opacity\", 0 , 1,\"\"]]", stiPathAnimationGeom.Animation.BeginTime, stiPathAnimationGeom.Animation.Duration);
				}
				else if (animatedPath3.Length != 0)
				{
					AddAnimation(writer, "[[\"path\", \"" + animatedPath3?.ToString() + "\", 1,\"\"]]", TimeSpan.FromMilliseconds(0.0), duration3.Value);
				}
				WriteInteraction(writer, stiPathAnimationGeom.Interaction);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiCurveAnimationGeom)
			{
				StiCurveAnimationGeom stiCurveAnimationGeom = geom2 as StiCurveAnimationGeom;
				StiPointsAnimation stiPointsAnimation2 = stiCurveAnimationGeom.Animation as StiPointsAnimation;
				if (!CheckPenGeom(stiCurveAnimationGeom.Pen))
				{
					continue;
				}
				string text9 = "g" + StiGuidUtils.NewGuid();
				Color color3 = (Color)stiCurveAnimationGeom.Pen.Brush;
				writer.WriteStartElement("g");
				writer.WriteStartElement("defs");
				writer.WriteStartElement("linearGradient");
				writer.WriteAttributeString("id", text9);
				writer.WriteAttributeString("x1", "0%");
				writer.WriteAttributeString("y1", "0%");
				writer.WriteAttributeString("x2", "100%");
				writer.WriteAttributeString("y2", "0%");
				writer.WriteStartElement("stop");
				writer.WriteAttributeString("offset", "0%");
				writer.WriteAttributeString("stop-color", $"rgba({color3.R},{color3.G},{color3.B},{DoubleToString(Math.Round((float)(int)color3.A / 255f, 3))})");
				writer.WriteAttributeString("stop-opacity", "1");
				writer.WriteAttributeString("style", "x: 0px;");
				if (stiPointsAnimation2 == null)
				{
					AddAnimation(writer, "[[\"offset\", 0 , 100,\"%\"]]", stiCurveAnimationGeom.Animation.BeginTime, stiCurveAnimationGeom.Animation.Duration);
				}
				writer.WriteEndElement();
				if (stiPointsAnimation2 == null)
				{
					writer.WriteStartElement("stop");
					writer.WriteAttributeString("offset", "0%");
					writer.WriteAttributeString("stop-color", "transparent");
					writer.WriteAttributeString("stop-opacity", "0");
					writer.WriteAttributeString("style", "x: 0px;");
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteStartElement("path");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				PointF[] array = ConvertSplineToCubicBezier(stiCurveAnimationGeom.Points, stiCurveAnimationGeom.Tension);
				if (stiPointsAnimation2 != null)
				{
					StringBuilder stringBuilder8 = new StringBuilder();
					PointF[] array2 = ConvertSplineToCubicBezier(stiPointsAnimation2.PointsFrom, stiCurveAnimationGeom.Tension);
					stringBuilder8.AppendFormat("M{0}:{1},{2}:{3} C", DoubleToString(array2[0].X + num2), DoubleToString(array[0].X + num2), DoubleToString(array2[0].Y + num3), DoubleToString(array[0].Y + num3));
					for (int j = 1; j < array.Length; j++)
					{
						stringBuilder8.AppendFormat("{0}:{1},{2}:{3} ", DoubleToString(array2[j].X + num2), DoubleToString(array[j].X + num2), DoubleToString(array2[j].Y + num3), DoubleToString((double)(array[j].Y + num3) + (double)j * 0.0001));
					}
					AddAnimation(writer, "[[\"path\", \"" + stringBuilder8?.ToString() + "\", 1,\"\"]]", TimeSpan.FromMilliseconds(0.0), TimeSpan.FromSeconds(1.0));
					StringBuilder stringBuilder9 = new StringBuilder();
					stringBuilder9.AppendFormat("M{0},{1} C", DoubleToString(array2[0].X + num2), DoubleToString(array2[0].Y + num3));
					for (int k = 1; k < array2.Length; k++)
					{
						stringBuilder9.AppendFormat("{0},{1} ", DoubleToString(array2[k].X + num2), DoubleToString((double)(array2[k].Y + num3) + (double)k * 0.0001));
					}
					writer.WriteAttributeString("d", stringBuilder9.ToString());
				}
				else
				{
					StringBuilder stringBuilder10 = new StringBuilder();
					stringBuilder10.AppendFormat("M{0},{1} C", DoubleToString(array[0].X + num2), DoubleToString(array[0].Y + num3));
					for (int l = 1; l < array.Length; l++)
					{
						stringBuilder10.AppendFormat("{0},{1} ", DoubleToString(array[l].X + num2), DoubleToString((double)(array[l].Y + num3) + (double)l * 0.0001));
					}
					writer.WriteAttributeString("d", stringBuilder10.ToString());
				}
				writer.WriteAttributeString("fill", "none");
				writer.WriteAttributeString("stroke-width", DoubleToString(stiCurveAnimationGeom.Pen.Thickness));
				writer.WriteAttributeString("stroke", $"url(#{text9})");
				if ((int)stiCurveAnimationGeom.Pen.PenStyle != 0)
				{
					writer.WriteAttributeString("stroke-dasharray", StiSvgHelper.GetLineStyleDash(stiCurveAnimationGeom.Pen.PenStyle, stiCurveAnimationGeom.Pen.Thickness));
				}
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiImageGeom)
			{
				StiImageGeom stiImageGeom = geom2 as StiImageGeom;
				if (stiImageGeom.Image != null)
				{
					Image imageFromObject = Stimulsoft.Report.Helpers.StiImageHelper.GetImageFromObject(stiImageGeom.Image);
					try
					{
						MemoryStream memoryStream = new MemoryStream();
						imageFromObject.Save((Stream)memoryStream, ImageFormat.Jpeg);
						byte[] array3 = memoryStream.ToArray();
						writer.WriteStartElement("image");
						writer.WriteAttributeString("x", DoubleToString(stiImageGeom.Rect.X));
						writer.WriteAttributeString("y", DoubleToString(stiImageGeom.Rect.Y));
						writer.WriteAttributeString("width", DoubleToString(stiImageGeom.Rect.Width));
						writer.WriteAttributeString("height", DoubleToString(stiImageGeom.Rect.Height));
						writer.WriteStartAttribute("href");
						string arg9 = "jpg";
						writer.WriteString($"data:image/{arg9};base64,");
						writer.WriteRaw("\r\n");
						writer.WriteBase64(array3, 0, array3.Length);
						writer.WriteEndAttribute();
						writer.WriteEndElement();
					}
					finally
					{
						((IDisposable)imageFromObject)?.Dispose();
					}
				}
				continue;
			}
			if (geom2 is StiBorderGeom)
			{
				StiBorderGeom stiBorderGeom = geom2 as StiBorderGeom;
				RectangleF rect8 = RectToRectangleF(stiBorderGeom.Rect);
				if (rect8.Width > 0f && rect8.Height > 0f)
				{
					StringBuilder stringBuilder11 = new StringBuilder();
					stringBuilder11.Append(WriteFillBrush(writer, stiBorderGeom.Background, rect8, num2, num3));
					if (CheckPenGeom(stiBorderGeom.BorderPen))
					{
						string arg10 = $"{WriteBorderStroke(writer, stiBorderGeom.BorderPen.Brush, rect8)}";
						stringBuilder11.Append($"{arg10};stroke-width:{stiBorderGeom.BorderPen.Thickness};");
					}
					writer.WriteStartElement("rect");
					if (stiBorderGeom.BackgroundMouseOver != null)
					{
						WriteFillBrushMouserOver(writer, stiBorderGeom.BackgroundMouseOver, rect8, num2, num3);
					}
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					writer.WriteAttributeString("x", DoubleToString(rect8.X + num2));
					writer.WriteAttributeString("y", DoubleToString(rect8.Y + num3));
					writer.WriteAttributeString("width", DoubleToString(rect8.Width));
					writer.WriteAttributeString("height", DoubleToString(rect8.Height));
					writer.WriteAttributeString("style", stringBuilder11.ToString());
					writer.WriteAttributeString("shape-rendering", "crispEdges");
					WriteInteraction(writer, stiBorderGeom.Interaction);
					writer.WriteEndElement();
				}
				continue;
			}
			if (geom2 is StiLineGeom)
			{
				StiLineGeom stiLineGeom = geom2 as StiLineGeom;
				if (CheckPenGeom(stiLineGeom.Pen))
				{
					writer.WriteStartElement("line");
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					writer.WriteAttributeString("x1", DoubleToString(stiLineGeom.X1 + num2));
					writer.WriteAttributeString("y1", DoubleToString(stiLineGeom.Y1 + num3));
					writer.WriteAttributeString("x2", DoubleToString(stiLineGeom.X2 + num2));
					writer.WriteAttributeString("y2", DoubleToString(stiLineGeom.Y2 + num3));
					string arg11 = $"{WriteBorderStroke(writer, stiLineGeom.Pen.Brush, default(RectangleF))}";
					string value = $"{arg11};stroke-width:{DoubleToString(stiLineGeom.Pen.Thickness)};";
					writer.WriteAttributeString("style", value);
					if ((int)stiLineGeom.Pen.PenStyle != 0)
					{
						writer.WriteAttributeString("stroke-dasharray", StiSvgHelper.GetLineStyleDash(stiLineGeom.Pen.PenStyle, stiLineGeom.Pen.Thickness));
					}
					writer.WriteEndElement();
				}
				continue;
			}
			if (geom2 is StiLinesGeom)
			{
				StiLinesGeom stiLinesGeom = geom2 as StiLinesGeom;
				if (CheckPenGeom(stiLinesGeom.Pen))
				{
					writer.WriteStartElement("polyline");
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					string empty7 = string.Empty;
					StringBuilder stringBuilder12 = new StringBuilder();
					for (int m = 0; m < stiLinesGeom.Points.Length; m++)
					{
						PointF pointF3 = stiLinesGeom.Points[m];
						stringBuilder12.AppendFormat("{0},{1} ", DoubleToString(pointF3.X + num2), DoubleToString((double)(pointF3.Y + num3) + (double)m * 0.0001));
					}
					writer.WriteAttributeString("fill", "none");
					writer.WriteAttributeString("points", stringBuilder12.ToString());
					string arg12 = $"{WriteBorderStroke(writer, stiLinesGeom.Pen.Brush, default(RectangleF))}";
					StringBuilder stringBuilder13 = new StringBuilder();
					stringBuilder13.AppendFormat("{0};stroke-width:{1};", arg12, DoubleToString(stiLinesGeom.Pen.Thickness));
					writer.WriteAttributeString("style", stringBuilder13.ToString());
					if ((int)stiLinesGeom.Pen.PenStyle != 0)
					{
						writer.WriteAttributeString("stroke-dasharray", StiSvgHelper.GetLineStyleDash(stiLinesGeom.Pen.PenStyle, stiLinesGeom.Pen.Thickness));
					}
					writer.WriteEndElement();
				}
				continue;
			}
			if (geom2 is StiCurveGeom)
			{
				StiCurveGeom stiCurveGeom = geom2 as StiCurveGeom;
				if (CheckPenGeom(stiCurveGeom.Pen))
				{
					writer.WriteStartElement("path");
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					PointF[] array4 = ConvertSplineToCubicBezier(stiCurveGeom.Points, stiCurveGeom.Tension);
					StringBuilder stringBuilder14 = new StringBuilder();
					stringBuilder14.AppendFormat("M{0},{1} C", DoubleToString(array4[0].X + num2), DoubleToString(array4[0].Y + num3));
					for (int n = 1; n < array4.Length; n++)
					{
						stringBuilder14.AppendFormat("{0},{1} ", DoubleToString(array4[n].X + num2), DoubleToString((double)(array4[n].Y + num3) + (double)n * 0.0001));
					}
					writer.WriteAttributeString("d", stringBuilder14.ToString());
					writer.WriteAttributeString("fill", "none");
					string arg13 = $"{WriteBorderStroke(writer, stiCurveGeom.Pen.Brush, default(RectangleF))}";
					string value2 = $"{arg13};stroke-width:{DoubleToString(stiCurveGeom.Pen.Thickness)};";
					writer.WriteAttributeString("style", value2);
					if ((int)stiCurveGeom.Pen.PenStyle != 0)
					{
						writer.WriteAttributeString("stroke-dasharray", StiSvgHelper.GetLineStyleDash(stiCurveGeom.Pen.PenStyle, stiCurveGeom.Pen.Thickness));
					}
					writer.WriteEndElement();
				}
				continue;
			}
			if (geom2 is StiEllipseGeom)
			{
				StiEllipseGeom stiEllipseGeom = geom2 as StiEllipseGeom;
				RectangleF rect9 = RectToRectangleF(stiEllipseGeom.Rect);
				string empty8 = string.Empty;
				string empty9 = string.Empty;
				empty8 = ((stiEllipseGeom.Background == null) ? "fill:none;" : WriteFillBrush(writer, stiEllipseGeom.Background, rect9, num2, num3));
				if (CheckPenGeom(stiEllipseGeom.BorderPen))
				{
					string arg14 = $"{WriteBorderStroke(writer, stiEllipseGeom.BorderPen.Brush, rect9)}";
					empty8 += $"{arg14};stroke-width:{DoubleToString(stiEllipseGeom.BorderPen.Thickness)};";
				}
				writer.WriteStartElement("ellipse");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("cx", DoubleToString(rect9.X + num2 + rect9.Width / 2f));
				writer.WriteAttributeString("cy", DoubleToString(rect9.Y + num3 + rect9.Height / 2f));
				writer.WriteAttributeString("rx", DoubleToString(rect9.Width / 2f));
				writer.WriteAttributeString("ry", DoubleToString(rect9.Height / 2f));
				writer.WriteAttributeString("style", empty8);
				writer.WriteAttributeString("elementindex", stiEllipseGeom.ElementIndex.ToString());
				WriteInteraction(writer, stiEllipseGeom.Interaction, addElementIndex: false);
				WriteTooltip(writer, stiEllipseGeom.ToolTip);
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiCachedShadowGeom && flag)
			{
				StiCachedShadowGeom stiCachedShadowGeom = geom2 as StiCachedShadowGeom;
				RectangleF rect10 = stiCachedShadowGeom.Rect;
				float num14 = rect10.Top - 10f;
				float num15 = rect10.Right + 10f;
				float num16 = rect10.Bottom + 10f;
				float num17 = rect10.Left - 10f;
				if ((stiCachedShadowGeom.Sides & 1) == 0)
				{
					num14 = stiCachedShadowGeom.ClipRect.Top;
				}
				if ((stiCachedShadowGeom.Sides & 2) == 0)
				{
					num15 = stiCachedShadowGeom.ClipRect.Right;
				}
				if ((stiCachedShadowGeom.Sides & 8) == 0)
				{
					num16 = stiCachedShadowGeom.ClipRect.Bottom;
				}
				if ((stiCachedShadowGeom.Sides & 0x10) == 0)
				{
					num17 = stiCachedShadowGeom.ClipRect.Left;
				}
				rect10.X += 2f;
				rect10.Y += 2f;
				string text10 = "s" + StiGuidUtils.NewGuid();
				string text11 = "s" + StiGuidUtils.NewGuid();
				writer.WriteStartElement("defs");
				writer.WriteStartElement("filter");
				writer.WriteAttributeString("id", text10);
				writer.WriteStartElement("feGaussianBlur");
				writer.WriteAttributeString("in", "SourceGraphic");
				writer.WriteAttributeString("stdDeviation", "2");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteStartElement("clipPath");
				writer.WriteAttributeString("id", text11);
				writer.WriteStartElement("rect");
				writer.WriteAttributeString("x", DoubleToString(num17 + num2));
				writer.WriteAttributeString("y", DoubleToString(num14 + num3));
				writer.WriteAttributeString("width", DoubleToString(num15 - num17));
				writer.WriteAttributeString("height", DoubleToString(num16 - num14));
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteStartElement("rect");
				writer.WriteAttributeString("x", DoubleToString(rect10.X + num2));
				writer.WriteAttributeString("y", DoubleToString(rect10.Y + num3));
				writer.WriteAttributeString("width", DoubleToString(rect10.Width));
				writer.WriteAttributeString("height", DoubleToString(rect10.Height));
				writer.WriteAttributeString("fill", "rgba(190,190,190,10)");
				writer.WriteAttributeString("filter", "url(#" + text10 + ")");
				writer.WriteAttributeString("clip-path", $"url(#{text11})");
				writer.WriteEndElement();
				continue;
			}
			if (geom2 is StiShadowGeom)
			{
				StiShadowGeom stiShadowGeom = geom2 as StiShadowGeom;
				string text12 = "s" + StiGuidUtils.NewGuid();
				writer.WriteStartElement("defs");
				writer.WriteStartElement("filter");
				writer.WriteAttributeString("id", text12);
				writer.WriteAttributeString("x", "0");
				writer.WriteAttributeString("y", "0");
				writer.WriteAttributeString("width", "200%");
				writer.WriteAttributeString("height", "200%");
				writer.WriteStartElement("feOffset");
				writer.WriteAttributeString("result", "offOut");
				writer.WriteAttributeString("in", "SourceGraphic");
				writer.WriteAttributeString("dx", "1.111111111111111");
				writer.WriteAttributeString("dy", "1.111111111111111");
				writer.WriteEndElement();
				writer.WriteStartElement("feColorMatrix");
				writer.WriteAttributeString("result", "matrixOut");
				writer.WriteAttributeString("in", "offOut");
				writer.WriteAttributeString("type", "matrix");
				writer.WriteAttributeString("values", "0.58 0 0 0 0 0 0.58 0 0 0 0 0 0.58 0 0 0 0 0 1 0");
				writer.WriteEndElement();
				writer.WriteStartElement("feGaussianBlur");
				writer.WriteAttributeString("result", "blurOut");
				writer.WriteAttributeString("in", "matrixOut");
				writer.WriteAttributeString("stdDeviation", "1.111111111111111");
				writer.WriteEndElement();
				writer.WriteStartElement("feBlend");
				writer.WriteAttributeString("mode", "normal");
				writer.WriteAttributeString("in", "SourceGraphic");
				writer.WriteAttributeString("in2", "blurOut");
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.WriteEndElement();
				foreach (StiGeom geom3 in stiShadowGeom.ShadowContext.Geoms)
				{
					if (geom3 is StiPathGeom stiPathGeom)
					{
						StringBuilder animatedPath4;
						TimeSpan? duration4;
						string pathData4 = GetPathData(stiPathGeom.Geoms, num2 + stiShadowGeom.Rect.X, num3 + stiShadowGeom.Rect.Y, out animatedPath4, out duration4);
						writer.WriteStartElement("path");
						if (!string.IsNullOrEmpty(text))
						{
							writer.WriteAttributeString("clip-path", $"url(#{text})");
						}
						writer.WriteAttributeString("d", pathData4);
						writer.WriteAttributeString("fill", "rgb(150,150,150)");
						writer.WriteAttributeString("filter", $"url(#{text12})");
						writer.WriteAttributeString("elementindex", stiPathGeom.ElementIndex.ToString());
						writer.WriteEndElement();
					}
				}
				continue;
			}
			StiTextGeom stiTextGeom;
			int length;
			int num19;
			SizeF sizeF;
			double num22;
			if (geom2 is StiTextGeom)
			{
				stiTextGeom = geom2 as StiTextGeom;
				StringFormat val2 = (StringFormat)(stiTextGeom.StringFormat.IsGeneric ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)new StringFormat()));
				val2.Alignment = stiTextGeom.StringFormat.Alignment;
				val2.FormatFlags = stiTextGeom.StringFormat.FormatFlags;
				val2.HotkeyPrefix = stiTextGeom.StringFormat.HotkeyPrefix;
				val2.LineAlignment = stiTextGeom.StringFormat.LineAlignment;
				val2.Trimming = stiTextGeom.StringFormat.Trimming;
				PointF pointF4 = default(PointF);
				if (stiTextGeom.Angle == 0f && !(stiTextGeom.Location is PointF) && !stiTextGeom.IsRotatedText)
				{
					RectangleF rectangleF5 = RectToRectangleF(stiTextGeom.Location);
					pointF4 = new PointF(rectangleF5.X, rectangleF5.Y);
					StringAlignment lineAlignment2 = val2.LineAlignment;
					switch ((int)lineAlignment2)
					{
					case 0:
						pointF4 = new PointF(rectangleF5.X, rectangleF5.Y);
						break;
					case 1:
						pointF4 = new PointF(rectangleF5.X, rectangleF5.Y + rectangleF5.Height / 2f);
						break;
					case 2:
						pointF4 = new PointF(rectangleF5.X, rectangleF5.Y + rectangleF5.Height);
						break;
					}
					stiTextGeom.RotationMode = (StiRotationMode)1;
				}
				else if (!(stiTextGeom.Location is PointF))
				{
					RectangleF rectangleF6 = RectToRectangleF(stiTextGeom.Location);
					pointF4 = new PointF(rectangleF6.X + rectangleF6.Width / 2f, rectangleF6.Y + rectangleF6.Height / 2f);
				}
				else
				{
					pointF4 = (PointF)stiTextGeom.Location;
				}
				StringBuilder stringBuilder15 = new StringBuilder();
				float num18 = (correctionText ? (stiTextGeom.Font.FontSize * 4f / 3f) : stiTextGeom.Font.FontSize);
				writer.WriteStartElement("text");
				switch (stiTextGeom.RotationMode)
				{
				case 0L:
					writer.WriteAttributeString("dy", "0.5em");
					break;
				case 2L:
					stringBuilder15.Append("text-anchor:middle;");
					writer.WriteAttributeString("dy", "1em");
					break;
				case 3L:
					stringBuilder15.Append("text-anchor:middle;");
					if (correctionText)
					{
						writer.WriteAttributeString("dy", "0.4em");
					}
					else
					{
						writer.WriteAttributeString("dy", "0.3em");
					}
					break;
				case 4L:
					stringBuilder15.Append("text-anchor:middle;");
					break;
				case 5L:
					stringBuilder15.Append("text-anchor:end;");
					writer.WriteAttributeString("dy", "1em");
					break;
				case 6L:
					stringBuilder15.Append("text-anchor:end;");
					writer.WriteAttributeString("dy", "0.5em");
					break;
				case 7L:
					stringBuilder15.Append("text-anchor:end;");
					break;
				default:
					writer.WriteAttributeString("dy", "1em");
					break;
				case 1L:
					break;
				}
				if (stiTextGeom.Angle == 0f && (stiTextGeom.MaximalWidth == 0 || !stiTextGeom.MaximalWidth.HasValue))
				{
					if (!string.IsNullOrEmpty(text))
					{
						writer.WriteAttributeString("clip-path", $"url(#{text})");
					}
					writer.WriteAttributeString("x", DoubleToString(pointF4.X + num2));
					writer.WriteAttributeString("y", DoubleToString(pointF4.Y + num3));
				}
				else
				{
					writer.WriteAttributeString("transform", $"translate({DoubleToString(pointF4.X + num2)}, {DoubleToString(pointF4.Y + num3)}) rotate({DoubleToString(stiTextGeom.Angle)} 0,0)");
				}
				writer.WriteAttributeString("font-size", DoubleToString(num18));
				writer.WriteAttributeString("font-family", stiTextGeom.Font.FontName);
				if ((stiTextGeom.Font.FontStyle & 1) > 0)
				{
					writer.WriteAttributeString("font-weight", "bold");
				}
				if ((stiTextGeom.Font.FontStyle & 2) > 0)
				{
					writer.WriteAttributeString("font-style", "italic");
				}
				string text13 = "";
				if ((stiTextGeom.Font.FontStyle & 4) > 0)
				{
					text13 += "underline";
				}
				if ((stiTextGeom.Font.FontStyle & 8) > 0)
				{
					text13 += " line-through";
				}
				if (!string.IsNullOrEmpty(text13))
				{
					writer.WriteAttributeString("text-decoration", text13);
				}
				Color color4 = ((stiTextGeom.Brush is Color) ? ((Color)stiTextGeom.Brush) : StiBrush.ToColor((StiBrush)/*isinst with value type is only supported in some contexts*/));
				stringBuilder15.Append($"fill:#{color4.R:X2}{color4.G:X2}{color4.B:X2};");
				if (color4.A != byte.MaxValue)
				{
					stringBuilder15.Append($"fill-opacity:{DoubleToString(Math.Round((float)(int)color4.A / 255f, 3))};");
				}
				if (string.IsNullOrEmpty(stiTextGeom.ToolTip))
				{
					stringBuilder15.Append("pointer-events: none");
				}
				writer.WriteAttributeString("style", stringBuilder15.ToString());
				writer.WriteAttributeString("elementindex", stiTextGeom.ElementIndex.ToString());
				WriteTooltip(writer, stiTextGeom.ToolTip);
				writer.WriteAttributeString("opacity", (stiTextGeom.Animation == null) ? "1" : "0");
				if (stiTextGeom.Animation != null)
				{
					AddAnimation(writer, "[[\"opacity\", 0, 1, \"\"]]", stiTextGeom.Animation.BeginTime, stiTextGeom.Animation.Duration);
				}
				if (stiTextGeom.MaximalWidth != 0 && stiTextGeom.MaximalWidth.HasValue && (stiTextGeom.StringFormat.FormatFlags & 0x1000) == 0)
				{
					length = stiTextGeom.Text.Length;
					num19 = length;
					sizeF = new SizeF(0f, 0f);
					for (int num20 = 0; num20 < length; num20++)
					{
						sizeF = context.MeasureString(stiTextGeom.Text.Substring(0, num20), stiTextGeom.Font);
						if (sizeF.Width > (float?)stiTextGeom.MaximalWidth && num20 != 0)
						{
							num19 = num20 - 1;
							break;
						}
					}
					double num21 = Math.Ceiling((double)length / (double)num19);
					num22 = 0.0;
					StiRotationMode? rotationMode = stiTextGeom.RotationMode;
					if (rotationMode.HasValue)
					{
						StiRotationMode valueOrDefault = rotationMode.GetValueOrDefault();
						if ((int)valueOrDefault == 1 || (int)valueOrDefault == 4 || (int)valueOrDefault == 7)
						{
							num22 = (0.0 - num21) * (double)sizeF.Height / 2.0 + (double)(sizeF.Height / 2f);
							goto IL_462e;
						}
					}
					num22 = 0.0;
					goto IL_462e;
				}
				writer.WriteValue(stiTextGeom.Text);
				goto IL_46f0;
			}
			if (geom2 is StiPathGeom)
			{
				StiPathGeom stiPathGeom2 = geom2 as StiPathGeom;
				RectangleF rect11 = RectToRectangleF(stiPathGeom2.Rect);
				string empty10 = string.Empty;
				string empty11 = string.Empty;
				empty10 = ((stiPathGeom2.Background == null) ? "fill:none;" : WriteFillBrush(writer, stiPathGeom2.Background, rect11, num2, num3));
				if (CheckPenGeom(stiPathGeom2.Pen))
				{
					string arg15 = $"{WriteBorderStroke(writer, stiPathGeom2.Pen.Brush, rect11)}";
					empty10 += $"{arg15};stroke-width:{DoubleToString(stiPathGeom2.Pen.Thickness)};";
				}
				StringBuilder animatedPath5;
				TimeSpan? duration5;
				string pathData5 = GetPathData(stiPathGeom2.Geoms, num2, num3, out animatedPath5, out duration5);
				writer.WriteStartElement("path");
				if (!string.IsNullOrEmpty(text))
				{
					writer.WriteAttributeString("clip-path", $"url(#{text})");
				}
				writer.WriteAttributeString("d", pathData5);
				writer.WriteAttributeString("style", empty10);
				writer.WriteAttributeString("elementindex", stiPathGeom2.ElementIndex.ToString());
				WriteInteraction(writer, stiPathGeom2.Interaction, addElementIndex: false);
				WriteTooltip(writer, stiPathGeom2.ToolTip);
				writer.WriteEndElement();
			}
			continue;
			IL_46f0:
			writer.WriteEndElement();
			continue;
			IL_462e:
			if (num19 > 0)
			{
				int num23 = 0;
				int num24 = 0;
				while (num23 < length)
				{
					writer.WriteStartElement("tspan");
					writer.WriteAttributeString("x", "0");
					if (num24 == 0)
					{
						writer.WriteAttributeString("y", DoubleToString(num22));
					}
					else
					{
						writer.WriteAttributeString("dy", DoubleToString(sizeF.Height));
					}
					writer.WriteString((num23 + num19 < stiTextGeom.Text.Length) ? stiTextGeom.Text.Substring(num23, num19) : stiTextGeom.Text.Substring(num23));
					writer.WriteEndElement();
					num23 += num19;
					num24++;
				}
			}
			goto IL_46f0;
		}
	}

	private static void AddAnimation(XmlTextWriter writer, string actions, TimeSpan? begin, TimeSpan duration, string number = "")
	{
		string value = $"{{\"actions\":{actions}, \"begin\":{(begin.HasValue ? begin.Value.TotalMilliseconds : 0.0)}, \"duration\":{duration.TotalMilliseconds}}}";
		writer.WriteAttributeString("_animation" + number, value);
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

	private static void WriteInteraction(XmlTextWriter writer, StiInteractionDataGeom interaction, bool addElementIndex = true)
	{
		if (interaction == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(interaction.ComponentName))
		{
			writer.WriteAttributeString("interaction", interaction.ComponentName);
		}
		if (!string.IsNullOrEmpty(interaction.PageGuid))
		{
			writer.WriteAttributeString("pageguid", interaction.PageGuid);
		}
		if (!string.IsNullOrEmpty(interaction.ComponentIndex))
		{
			writer.WriteAttributeString("compindex", interaction.ComponentIndex);
		}
		if (!string.IsNullOrEmpty(interaction.PageIndex))
		{
			writer.WriteAttributeString("pageindex", interaction.PageIndex);
		}
		if (addElementIndex)
		{
			writer.WriteAttributeString("elementindex", interaction.ElementIndex);
		}
		if (!string.IsNullOrEmpty(interaction.InteractionHyperlink))
		{
			writer.WriteAttributeString("interactionhyperlink", interaction.InteractionHyperlink);
		}
		if (!string.IsNullOrEmpty(interaction.InteractionToolTip))
		{
			writer.WriteAttributeString("interactiontooltip", interaction.InteractionToolTip);
		}
		if (interaction.InteractionData is StiSeriesInteractionData { OriginalArgument: var obj } stiSeriesInteractionData)
		{
			if (obj == null)
			{
				obj = stiSeriesInteractionData.Argument;
			}
			object obj2 = obj;
			object obj3 = ((obj2 == null || !ListExt.IsList(obj2)) ? obj2 : ListExt.ToList(obj2)?.FirstOrDefault());
			writer.WriteAttributeString("elementargument", (obj3 == null || obj3 == DBNull.Value) ? "sti_IsNullValue" : obj3.ToString());
			writer.WriteAttributeString("elementvalue", stiSeriesInteractionData.Value?.ToString());
			writer.WriteAttributeString("elementendvalue", stiSeriesInteractionData.EndValue?.ToString());
			string value = stiSeriesInteractionData.Series?.CoreTitle;
			if (string.IsNullOrEmpty(value))
			{
				value = stiSeriesInteractionData.Series?.TitleValue;
			}
			if (!string.IsNullOrEmpty(value))
			{
				writer.WriteAttributeString("elementseries", value);
			}
			if (!string.IsNullOrEmpty(stiSeriesInteractionData.Hyperlink))
			{
				writer.WriteAttributeString("interactionhyperlink", stiSeriesInteractionData.Hyperlink);
			}
			if (!string.IsNullOrEmpty(stiSeriesInteractionData.Tooltip))
			{
				writer.WriteAttributeString("interactiontooltip", stiSeriesInteractionData.Tooltip);
			}
		}
		if (interaction.InteractionData is StiIndicatorInteractionData stiIndicatorInteractionData)
		{
			writer.WriteAttributeString("elementvalue", stiIndicatorInteractionData.Value?.ToString());
			writer.WriteAttributeString("elementseries", stiIndicatorInteractionData.SeriesText);
			writer.WriteAttributeString("elementtarget", stiIndicatorInteractionData.Target?.ToString());
		}
	}

	private static string GetPathData(List<StiSegmentGeom> geoms, float dx, float dy, out StringBuilder animatedPath, out TimeSpan? duration, StiAnimation animation = null)
	{
		string text = string.Empty;
		animatedPath = new StringBuilder();
		duration = TimeSpan.FromMilliseconds(0.0);
		if (geoms.Count == 4 && geoms[0] is StiArcSegmentGeom stiArcSegmentGeom && geoms[2] is StiArcSegmentGeom stiArcSegmentGeom2 && animation is StiPieSegmentAnimation animation2)
		{
			return text + AddDoughnutPath(stiArcSegmentGeom.Rect, stiArcSegmentGeom2.Rect, stiArcSegmentGeom.StartAngle, stiArcSegmentGeom.SweepAngle, animation2, text, dx, dy, animatedPath, out duration);
		}
		int num = 0;
		foreach (StiSegmentGeom geom in geoms)
		{
			if (geom is StiArcSegmentGeom)
			{
				StiArcSegmentGeom arcSegment = geom as StiArcSegmentGeom;
				text += AddArcPath(arcSegment, text, dx, dy);
			}
			else if (geom is StiCurveSegmentGeom)
			{
				StiCurveSegmentGeom stiCurveSegmentGeom = geom as StiCurveSegmentGeom;
				StiPointsAnimation stiPointsAnimation = stiCurveSegmentGeom.Animation as StiPointsAnimation;
				Point[] array = StiCurveHelper.CardinalSpline(stiCurveSegmentGeom.Points, closed: false);
				Point[] array2 = ((stiPointsAnimation != null) ? StiCurveHelper.CardinalSpline(stiPointsAnimation.PointsFrom, closed: false) : null);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 1; i < array.Length; i += 3)
				{
					if (stiPointsAnimation != null)
					{
						if (i == 1)
						{
							stringBuilder.AppendFormat("C{0},{1},{2},{3},{4},{5}", DoubleToString((float)array2[i].X + dx), DoubleToString((float)array2[i].Y + dy), DoubleToString((float)array2[i + 1].X + dx), DoubleToString((float)array2[i + 1].Y + dy), DoubleToString((float)array2[i + 2].X + dx), DoubleToString((float)array2[i + 2].Y + dy));
						}
						else
						{
							stringBuilder.AppendFormat(",{0},{1},{2},{3},{4},{5}", DoubleToString((float)array2[i].X + dx), (float)array2[i].Y + dy, DoubleToString((float)array2[i + 1].X + dx), (float)array2[i + 1].Y + dy, DoubleToString((float)array2[i + 2].X + dx), (double)((float)array2[i + 2].Y + dy) + (double)i * 0.0001);
						}
						if (i == 1)
						{
							animatedPath.AppendFormat("C{0}:{1},{2}:{3},{4}:{5},{6}:{7},{8}:{9},{10}:{11}", DoubleToString((float)array2[i].X + dx), DoubleToString((float)array[i].X + dx), DoubleToString((float)array2[i].Y + dy), DoubleToString((float)array[i].Y + dy), DoubleToString((float)array2[i + 1].X + dx), DoubleToString((float)array[i + 1].X + dx), DoubleToString((float)array2[i + 1].Y + dy), DoubleToString((float)array[i + 1].Y + dy), DoubleToString((float)array2[i + 2].X + dx), DoubleToString((float)array[i + 2].X + dx), DoubleToString((float)array2[i + 2].Y + dy), DoubleToString((float)array[i + 2].Y + dy));
						}
						else
						{
							animatedPath.AppendFormat(",{0}:{1},{2}:{3},{4}:{5},{6}:{7},{8}:{9},{10}:{11}", DoubleToString((float)array2[i].X + dx), DoubleToString((float)array[i].X + dx), DoubleToString((float)array2[i].Y + dy), DoubleToString((float)array[i].Y + dy), DoubleToString((float)array2[i + 1].X + dx), DoubleToString((float)array[i + 1].X + dx), DoubleToString((float)array2[i + 1].Y + dy), DoubleToString((float)array[i + 1].Y + dy), DoubleToString((float)array2[i + 2].X + dx), DoubleToString((float)array[i + 2].X + dx), DoubleToString((float)array2[i + 2].Y + dy), DoubleToString((double)((float)array[i + 2].Y + dy) + (double)i * 0.0001));
						}
						duration = TimeSpan.FromSeconds(1.0);
					}
					else if (i == 1)
					{
						stringBuilder.AppendFormat("C{0},{1},{2},{3},{4},{5}", DoubleToString((float)array[i].X + dx), DoubleToString((float)array[i].Y + dy), DoubleToString((float)array[i + 1].X + dx), DoubleToString((float)array[i + 1].Y + dy), DoubleToString((float)array[i + 2].X + dx), DoubleToString((float)array[i + 2].Y + dy));
					}
					else
					{
						stringBuilder.AppendFormat(",{0},{1},{2},{3},{4},{5}", DoubleToString((float)array[i].X + dx), DoubleToString((float)array[i].Y + dy), DoubleToString((float)array[i + 1].X + dx), DoubleToString((float)array[i + 1].Y + dy), DoubleToString((float)array[i + 2].X + dx), DoubleToString((double)((float)array[i + 2].Y + dy) + (double)i * 0.0001));
					}
				}
				text += stringBuilder;
			}
			else if (geom is StiLineSegmentGeom)
			{
				StiLineSegmentGeom stiLineSegmentGeom = geom as StiLineSegmentGeom;
				StiPointsAnimation stiPointsAnimation2 = stiLineSegmentGeom.Animation as StiPointsAnimation;
				StringBuilder stringBuilder2 = new StringBuilder();
				if (!text.StartsWith("M"))
				{
					if (stiPointsAnimation2 != null)
					{
						stringBuilder2.AppendFormat("M{0},{1}", DoubleToString(stiPointsAnimation2.PointsFrom[0].X + dx), DoubleToString(stiPointsAnimation2.PointsFrom[0].Y + dy));
						animatedPath.AppendFormat("M{0}:{1},{2}:{3}", DoubleToString(stiPointsAnimation2.PointsFrom[0].X + dx), DoubleToString(stiLineSegmentGeom.X1 + dx), DoubleToString(stiPointsAnimation2.PointsFrom[0].Y + dy), DoubleToString(stiLineSegmentGeom.Y1 + dy));
					}
					else
					{
						stringBuilder2.AppendFormat("M{0},{1}", DoubleToString(stiLineSegmentGeom.X1 + dx), DoubleToString(stiLineSegmentGeom.Y1 + dy));
					}
				}
				if (stiPointsAnimation2 != null)
				{
					stringBuilder2.AppendFormat("L{0},{1}", DoubleToString(stiPointsAnimation2.PointsFrom[1].X + dx), DoubleToString(stiPointsAnimation2.PointsFrom[1].Y + dy));
					animatedPath.AppendFormat("L{0}:{1},{2}:{3}", DoubleToString(stiPointsAnimation2.PointsFrom[1].X + dx), DoubleToString(stiLineSegmentGeom.X2 + dx), DoubleToString(stiPointsAnimation2.PointsFrom[1].Y + dy), DoubleToString((double)(stiLineSegmentGeom.Y2 + dy) + 0.0001));
					duration = stiPointsAnimation2.Duration;
				}
				else
				{
					stringBuilder2.AppendFormat("L{0},{1}", DoubleToString(stiLineSegmentGeom.X2 + dx), DoubleToString((double)(stiLineSegmentGeom.Y2 + dy) + 0.0001));
				}
				text += stringBuilder2;
			}
			else if (geom is StiLinesSegmentGeom)
			{
				StiLinesSegmentGeom stiLinesSegmentGeom = geom as StiLinesSegmentGeom;
				StiPointsAnimation stiPointsAnimation3 = stiLinesSegmentGeom.Animation as StiPointsAnimation;
				StringBuilder stringBuilder3 = new StringBuilder();
				if (!text.StartsWith("M"))
				{
					if (stiPointsAnimation3 != null)
					{
						stringBuilder3.AppendFormat("M{0},{1}", DoubleToString(stiPointsAnimation3.PointsFrom[0].X + dx), DoubleToString(stiPointsAnimation3.PointsFrom[0].Y + dy));
						animatedPath.AppendFormat("M{0}:{1},{2}:{3}", DoubleToString(stiPointsAnimation3.PointsFrom[0].X + dx), DoubleToString(stiPointsAnimation3.PointsFrom[1].X + dx), DoubleToString(stiPointsAnimation3.PointsFrom[0].Y + dy), DoubleToString((double)(stiPointsAnimation3.PointsFrom[1].Y + dy) + 0.0001));
					}
					else
					{
						stringBuilder3.AppendFormat("M{0},{1}", DoubleToString(stiLinesSegmentGeom.Points[0].X + dx), DoubleToString((double)(stiLinesSegmentGeom.Points[0].Y + dy) + 0.0001));
					}
				}
				for (int j = 0; j < stiLinesSegmentGeom.Points.Length; j++)
				{
					if (stiPointsAnimation3 != null)
					{
						stringBuilder3.AppendFormat("{0}{1},{2}", (j == 0) ? "L" : ",", DoubleToString(stiPointsAnimation3.PointsFrom[j].X + dx), DoubleToString(stiPointsAnimation3.PointsFrom[j].Y + dy));
						animatedPath.AppendFormat("{0}{1}:{2},{3}:{4}", (j == 0) ? "L" : ",", DoubleToString(stiPointsAnimation3.PointsFrom[j].X + dx), DoubleToString(stiLinesSegmentGeom.Points[j].X + dx), DoubleToString(stiPointsAnimation3.PointsFrom[j].Y + dy), DoubleToString(stiLinesSegmentGeom.Points[j].Y + dy));
						duration = stiPointsAnimation3.Duration;
					}
					else
					{
						stringBuilder3.AppendFormat("{0}{1},{2}", (j == 0) ? "L" : ",", DoubleToString(stiLinesSegmentGeom.Points[j].X + dx), DoubleToString((double)(stiLinesSegmentGeom.Points[j].Y + dy) + (double)j * 0.0001));
					}
				}
				text += stringBuilder3;
			}
			else if (geom is StiPieSegmentGeom)
			{
				StiPieSegmentGeom stiPieSegmentGeom = geom as StiPieSegmentGeom;
				stiPieSegmentGeom.Animation = animation;
				text += AddPiePath(stiPieSegmentGeom, text, dx, dy, animatedPath, out duration);
			}
			else
			{
				_ = geom is StiCloseFigureSegmentGeom;
			}
			num++;
		}
		return text;
	}

	private static string AddArcPath(float centerX, float centerY, float radius, float startAngle, float sweepAngle, float dx, float dy)
	{
		StringBuilder stringBuilder = new StringBuilder();
		float num = (float)Round(Math.Abs(sweepAngle / 90f));
		float num2 = sweepAngle / num;
		for (int i = 0; (float)i < num; i++)
		{
			PointF[] array = ConvertArcToCubicBezier(centerX - dx, centerY - dy, radius, startAngle, num2);
			for (int j = 1; j < array.Length - 1; j += 3)
			{
				if (j == 1)
				{
					stringBuilder.AppendFormat("C{0},{1},{2},{3},{4},{5}", DoubleToString(array[1].X + dx), DoubleToString(array[1].Y + dy), DoubleToString(array[2].X + dx), DoubleToString(array[2].Y + dy), DoubleToString(array[3].X + dx), DoubleToString(array[3].Y + dy));
				}
				else
				{
					stringBuilder.AppendFormat(",{0},{1},{2},{3},{4},{5}", DoubleToString(array[1].X + dx), DoubleToString(array[1].Y + dy), DoubleToString(array[2].X + dx), DoubleToString(array[2].Y + dy), DoubleToString(array[3].X + dx), DoubleToString(array[3].Y + dy));
				}
				startAngle += num2;
			}
		}
		return stringBuilder.ToString();
	}

	private static string AddArcPathCrossElipse(float centerX, float centerY, float radiusX, float radiusY, float startAngle, float sweepAngle, float dx, float dy)
	{
		return SvgEllipseArc(centerX, centerY, radiusX, radiusY, startAngle, sweepAngle);
	}

	private static string SvgEllipseArc(float cx, float cy, float rx, float ry, float t1, float s, float rot = 0f)
	{
		if (s == 360f)
		{
			s = 359.99f;
		}
		t1 = t1 * (float)Math.PI / 180f;
		s = s * (float)Math.PI / 180f;
		float[,] abcd = RotateMatrix(rot);
		float[] array = VecAdd(MatricTimes(abcd, new float[2]
		{
			rx * (float)Math.Cos(t1),
			ry * (float)Math.Sin(t1)
		}), new float[2] { cx, cy });
		float[] array2 = VecAdd(MatricTimes(abcd, new float[2]
		{
			rx * (float)Math.Cos(t1 + s),
			ry * (float)Math.Sin(t1 + s)
		}), new float[2] { cx, cy });
		int num = (((double)s > Math.PI) ? 1 : 0);
		int num2 = ((s > 0f) ? 1 : 0);
		return "M " + DoubleToString(array[0]) + " " + DoubleToString(array[1]) + " A " + DoubleToString(rx) + " " + DoubleToString(ry) + " " + DoubleToString((double)rot / (Math.PI * 2.0) * 360.0) + " " + DoubleToString(num) + " " + DoubleToString(num2) + " " + DoubleToString(array2[0]) + " " + DoubleToString(array2[1]);
	}

	private static float[] MatricTimes(float[,] abcd, float[] xy)
	{
		float num = abcd[0, 0];
		float num2 = abcd[0, 1];
		float num3 = abcd[1, 0];
		float num4 = abcd[1, 1];
		float num5 = xy[0];
		float num6 = xy[1];
		return new float[2]
		{
			num * num5 + num2 * num6,
			num3 * num5 + num4 * num6
		};
	}

	private static float[,] RotateMatrix(float x)
	{
		return new float[2, 2]
		{
			{
				(float)Math.Cos(x),
				0f - (float)Math.Sin(x)
			},
			{
				(float)Math.Sin(x),
				(float)Math.Cos(x)
			}
		};
	}

	private static float[] VecAdd(float[] a, float[] b)
	{
		return new float[2]
		{
			a[0] + b[0],
			a[1] + b[1]
		};
	}

	private static string AddArcPath(StiArcSegmentGeom arcSegment, string path, float dx, float dy)
	{
		StringBuilder stringBuilder = new StringBuilder();
		float num = arcSegment.Rect.X + dx + arcSegment.Rect.Width / 2f;
		float num2 = arcSegment.Rect.Y + dy + arcSegment.Rect.Height / 2f;
		float num3 = arcSegment.Rect.Width / 2f;
		float num4 = arcSegment.Rect.Height / 2f;
		float num5 = (arcSegment.RealStartAngle.HasValue ? arcSegment.RealStartAngle.GetValueOrDefault() : arcSegment.StartAngle);
		float num6 = (arcSegment.RealStartAngle.HasValue ? arcSegment.RealSweepAngle.GetValueOrDefault() : arcSegment.SweepAngle);
		double num7 = (double)num5 * Math.PI / 180.0;
		double value = (double)num + (double)num3 * Math.Cos(num7);
		double value2 = (double)num2 + (double)num4 * Math.Sin(num7);
		if (!path.StartsWith("M") || num6 % 360f == 0f)
		{
			stringBuilder.AppendFormat("M{0},{1}", DoubleToString(value), DoubleToString(value2));
		}
		if (arcSegment.CrossElipseDraw)
		{
			stringBuilder.Append(AddArcPathCrossElipse(num, num2, num3, num4, num5, num6, dx, dy));
		}
		else
		{
			stringBuilder.Append(AddArcPath(num, num2, num3, num5, num6, dx, dy));
		}
		return stringBuilder.ToString();
	}

	private static string AddPiePath(StiPieSegmentGeom pieSegment, string path, float dx, float dy, StringBuilder animatedPath, out TimeSpan? duration)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StiPieSegmentAnimation stiPieSegmentAnimation = pieSegment.Animation as StiPieSegmentAnimation;
		float num = pieSegment.Rect.X + dx + pieSegment.Rect.Width / 2f;
		float num2 = pieSegment.Rect.Y + dy + pieSegment.Rect.Height / 2f;
		float num3 = ((stiPieSegmentAnimation == null) ? (pieSegment.Rect.Width / 2f) : (stiPieSegmentAnimation.RectFrom.Width / 2f));
		float num4 = ((stiPieSegmentAnimation == null) ? (pieSegment.Rect.Height / 2f) : (stiPieSegmentAnimation.RectFrom.Height / 2f));
		float num5 = stiPieSegmentAnimation?.StartAngleFrom ?? pieSegment.StartAngle;
		float num6 = ((stiPieSegmentAnimation == null) ? pieSegment.SweepAngle : (stiPieSegmentAnimation.EndAngleFrom - stiPieSegmentAnimation.StartAngleFrom));
		if (pieSegment.RealStartAngle.HasValue)
		{
			num5 = pieSegment.RealStartAngle.GetValueOrDefault();
		}
		if (pieSegment.RealSweepAngle.HasValue)
		{
			num6 = pieSegment.RealSweepAngle.GetValueOrDefault();
		}
		double num7 = (double)num5 * Math.PI / 180.0;
		double value = (double)num + (double)num3 * Math.Cos(num7);
		double value2 = (double)num2 + (double)num4 * Math.Sin(num7);
		stringBuilder.AppendFormat("M{0},{1}", DoubleToString(num), DoubleToString(num2));
		stringBuilder.AppendFormat("L{0},{1}", DoubleToString(value), DoubleToString(value2));
		if (pieSegment.CrossElipseDraw)
		{
			stringBuilder.Append(AddArcPathCrossElipse(num, num2, num3, num4, num5, num6, dx, dy));
		}
		else
		{
			stringBuilder.Append(AddArcPath(num, num2, num3, num5, num6, dx, dy));
		}
		stringBuilder.AppendFormat("L{0},{1}", DoubleToString(num), DoubleToString(num2));
		if (stiPieSegmentAnimation != null)
		{
			duration = stiPieSegmentAnimation.Duration;
			animatedPath.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"startAngle\": " + DoubleToString(pieSegment.StartAngle) + ", \"startAngleFrom\": " + DoubleToString(stiPieSegmentAnimation.StartAngleFrom) + ", \"sweepAngle\": " + DoubleToString(pieSegment.SweepAngle) + ", \"sweepAngleFrom\": " + DoubleToString(num6) + ", \"radiusFrom\": " + DoubleToString(num3) + ", \"x\": " + DoubleToString(pieSegment.Rect.X) + ", \"y\": " + DoubleToString(pieSegment.Rect.Y) + ", \"width\": " + DoubleToString(pieSegment.Rect.Width) + ", \"height\": " + DoubleToString(pieSegment.Rect.Height) + ", \"dx\": " + DoubleToString(dx) + ", \"dy\": " + DoubleToString(dy) + "}")));
		}
		else
		{
			duration = TimeSpan.FromSeconds(1.0);
		}
		return stringBuilder.ToString();
	}

	private static string AddDoughnutPath(RectangleF rect, RectangleF rectDt, float startAngle, float sweepAngle, StiPieSegmentAnimation animation, string path, float dx, float dy, StringBuilder animatedPath, out TimeSpan? duration)
	{
		StringBuilder stringBuilder = new StringBuilder();
		float num = rect.X + dx + rect.Width / 2f;
		float num2 = rect.Y + dy + rect.Height / 2f;
		float num3 = ((animation == null) ? (rect.Width / 2f) : (animation.RectFrom.Width / 2f));
		float num4 = ((animation == null) ? (rectDt.Width / 2f) : (animation.RectDtFrom.Width / 2f));
		float num5 = animation?.StartAngleFrom ?? startAngle;
		float num6 = ((animation == null) ? sweepAngle : (animation.EndAngleFrom - animation.StartAngleFrom));
		float startAngle2 = animation?.EndAngleFrom ?? (startAngle + sweepAngle);
		double num7 = (double)num5 * Math.PI / 180.0;
		double num8 = (double)(num5 + num6) * Math.PI / 180.0;
		double value = (double)num + (double)num3 * Math.Cos(num7);
		double value2 = (double)num2 + (double)num3 * Math.Sin(num7);
		double value3 = (double)num + (double)num4 * Math.Cos(num7);
		double value4 = (double)num2 + (double)num4 * Math.Sin(num7);
		double value5 = (double)num + (double)num4 * Math.Cos(num8);
		double value6 = (double)num2 + (double)num4 * Math.Sin(num8);
		stringBuilder.AppendFormat("M{0},{1}", DoubleToString(value3), DoubleToString(value4));
		stringBuilder.AppendFormat("L{0},{1}", DoubleToString(value), DoubleToString(value2));
		stringBuilder.Append(AddArcPath(num, num2, num3, num5, num6, dx, dy));
		stringBuilder.AppendFormat("L{0},{1}", DoubleToString(value5), DoubleToString(value6));
		stringBuilder.Append(AddArcPath(num, num2, num4, startAngle2, 0f - num6, dx, dy));
		if (animation != null)
		{
			duration = animation.Duration;
			animatedPath.Append(Convert.ToBase64String(Encoding.UTF8.GetBytes("{\"startAngle\": " + DoubleToString(startAngle) + ", \"startAngleFrom\": " + DoubleToString(animation.StartAngleFrom) + ", \"sweepAngle\": " + DoubleToString(sweepAngle) + ", \"sweepAngleFrom\": " + DoubleToString(num6) + ", \"radiusFrom\": " + DoubleToString(num3) + ", \"radiusDtFrom\": " + DoubleToString(num4) + ", \"x\": " + DoubleToString(rect.X) + ", \"y\": " + DoubleToString(rect.Y) + ", \"width\": " + DoubleToString(rect.Width) + ", \"widthDt\": " + DoubleToString(rectDt.Width) + ", \"height\": " + DoubleToString(rect.Height) + ", \"dx\": " + DoubleToString(dx) + ", \"dy\": " + DoubleToString(dy) + "}")));
		}
		else
		{
			duration = TimeSpan.FromSeconds(1.0);
		}
		return stringBuilder.ToString();
	}

	public static RectangleF CorrectRectLabel(StiRotationMode rotationMode, RectangleF textRect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected I4, but got Unknown
		return (int)rotationMode switch
		{
			1 => new RectangleF(textRect.X + textRect.Width / 2f, textRect.Y, textRect.Width, textRect.Height), 
			2 => new RectangleF(textRect.X + textRect.Width / 2f, textRect.Y - textRect.Height / 2f, textRect.Width, textRect.Height), 
			0 => new RectangleF(textRect.X + textRect.Width / 2f, textRect.Y + textRect.Height / 2f, textRect.Width, textRect.Height), 
			3 => new RectangleF(textRect.X, textRect.Y + textRect.Height / 2f, textRect.Width, textRect.Height), 
			4 => textRect, 
			5 => new RectangleF(textRect.X, textRect.Y - textRect.Height / 2f, textRect.Width, textRect.Height), 
			6 => new RectangleF(textRect.X - textRect.Width / 2f, textRect.Y + textRect.Height / 2f, textRect.Width, textRect.Height), 
			7 => new RectangleF(textRect.X - textRect.Width / 2f, textRect.Y, textRect.Width, textRect.Height), 
			8 => new RectangleF(textRect.X - textRect.Width / 2f, textRect.Y - textRect.Height / 2f, textRect.Width, textRect.Height), 
			_ => textRect, 
		};
	}

	private static PointF[] ConvertArcToCubicBezier(float centerX, float centerY, float radius, float startAngle, float sweepAngle)
	{
		double num = (double)startAngle * Math.PI / 180.0;
		double num2 = (double)sweepAngle * Math.PI / 180.0;
		double num3 = (double)(startAngle + sweepAngle) * Math.PI / 180.0;
		double num4 = (double)centerX + (double)radius * Math.Cos(num);
		double num5 = (double)centerY + (double)radius * Math.Sin(num);
		double num6 = (double)centerX + (double)radius * Math.Cos(num3);
		double num7 = (double)centerY + (double)radius * Math.Sin(num3);
		double num8 = (double)(radius * 4f / 3f) * Math.Tan(0.25 * num2);
		double num9 = Math.Atan(num8 / (double)radius);
		double num10 = (double)radius / Math.Cos(num9);
		num9 += num;
		double num11 = (double)centerX + num10 * Math.Cos(num9);
		double num12 = (double)centerY + num10 * Math.Sin(num9);
		num9 = Math.Atan((0.0 - num8) / (double)radius);
		num9 += num3;
		double num13 = (double)centerX + num10 * Math.Cos(num9);
		double num14 = (double)centerY + num10 * Math.Sin(num9);
		return new PointF[4]
		{
			new PointF((float)num4, (float)num5),
			new PointF((float)num11, (float)num12),
			new PointF((float)num13, (float)num14),
			new PointF((float)num6, (float)num7)
		};
	}

	private static double Round(double value)
	{
		int num = (int)value;
		double num2 = value - (double)num;
		if (!(num2 > 0.0))
		{
			return num;
		}
		return num + 1;
	}

	private static PointF[] ConvertSplineToCubicBezier(PointF[] points, float tension)
	{
		int num = points.Length;
		int num2 = num * 3 - 2;
		PointF[] array = new PointF[num2];
		tension *= 0.3f;
		array[0] = points[0];
		array[1] = CalculateCurveBezierEndPoints(points[0], points[1], tension);
		for (int i = 0; i < num - 2; i++)
		{
			PointF[] array2 = CalculateCurveBezier(points, i, tension);
			array[3 * i + 2] = array2[0];
			array[3 * i + 3] = points[i + 1];
			array[3 * i + 4] = array2[1];
		}
		array[num2 - 2] = CalculateCurveBezierEndPoints(points[num - 1], points[num - 2], tension);
		array[num2 - 1] = points[num - 1];
		return array;
	}

	private static PointF[] CalculateCurveBezier(PointF[] points, int index, float tension)
	{
		float num = points[index + 2].X - points[index].X;
		float num2 = points[index + 2].Y - points[index].Y;
		return new PointF[2]
		{
			new PointF(points[index + 1].X - tension * num, points[index + 1].Y - tension * num2),
			new PointF(points[index + 1].X + tension * num, points[index + 1].Y + tension * num2)
		};
	}

	private static PointF CalculateCurveBezierEndPoints(PointF end, PointF adj, float tension)
	{
		return new PointF(tension * (adj.X - end.X) + end.X, tension * (adj.Y - end.Y) + end.Y);
	}

	private static bool CheckPenGeom(StiPenGeom penGeom)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		if (penGeom != null && penGeom.Brush != null)
		{
			return (int)penGeom.PenStyle != 6;
		}
		return false;
	}

	public static void WriteTooltip(XmlTextWriter writer, string tooltip)
	{
		if (string.IsNullOrEmpty(tooltip))
		{
			return;
		}
		if (StiJsonChecker.IsValidJson(tooltip))
		{
			JToken source = JToken.Parse(tooltip);
			JToken value = ((IEnumerable)source).Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiColor").Value;
			JToken value2 = ((IEnumerable)source).Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiValue").Value;
			JToken value3 = ((IEnumerable)source).Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiArgument").Value;
			JToken value4 = ((IEnumerable)source).Cast<KeyValuePair<string, JToken>>().FirstOrDefault((KeyValuePair<string, JToken> a) => a.Key == "StiWeight").Value;
			if (value4 != null)
			{
				writer.WriteAttributeString("_text1", ((object)value2).ToString());
				if (value3 != null)
				{
					writer.WriteAttributeString("_text2", StiLocalization.Get("PropertyMain", "Argument") + ": " + ((object)value3).ToString());
				}
				if (value2 != null)
				{
					writer.WriteAttributeString("_text3", StiLocalization.Get("PropertyMain", "Weight") + ": " + ((object)value4).ToString());
				}
			}
			else
			{
				if (value3 != null)
				{
					writer.WriteAttributeString("_text1", ((object)value3).ToString());
				}
				if (value2 != null)
				{
					writer.WriteAttributeString("_text2", ((object)value2).ToString());
				}
			}
			if (value != null)
			{
				Color color = ColorTranslator.FromHtml(((object)value).ToString());
				writer.WriteAttributeString("_color", $"#{color.R:X2}{color.G:X2}{color.B:X2}");
			}
			writer.WriteAttributeString("_ismap", "true");
		}
		else
		{
			writer.WriteStartElement("title");
			writer.WriteValue(tooltip);
			writer.WriteEndElement();
		}
	}

	public static void WriteFillBrushMouserOver(XmlTextWriter writer, object brushOver, RectangleF rect, float dx = 0f, float dy = 0f)
	{
		RectangleF sRect = rect;
		sRect.X += dx;
		sRect.Y += dy;
		writer.WriteAttributeString("_brushover", StiBrushSvgHelper.GetFillBrush(writer, brushOver, sRect) ?? "");
	}

	public static string WriteFillBrush(XmlTextWriter writer, object brush, RectangleF rect, float dx = 0f, float dy = 0f)
	{
		RectangleF sRect = rect;
		sRect.X += dx;
		sRect.Y += dy;
		return "fill:" + StiBrushSvgHelper.GetFillBrush(writer, brush, sRect) + ";";
	}

	private static string WriteBorderStroke(XmlTextWriter writer, object brush, RectangleF rect)
	{
		if (brush is Color color)
		{
			string text = $"stroke:rgb({color.R},{color.G},{color.B});";
			double num = Math.Round((float)(int)color.A / 255f, 3);
			if (num != 1.0)
			{
				text += $"stroke-opacity:{DoubleToString(num)};";
			}
			return text;
		}
		if (brush is StiGradientBrush)
		{
			string arg = StiBrushSvgHelper.WriteGradientBrush(writer, brush, rect);
			return $"fill:url(#{arg});";
		}
		if (brush is StiGlareBrush)
		{
			string arg2 = StiBrushSvgHelper.WriteGlareBrush(writer, brush, rect);
			return $"fill:url(#{arg2});";
		}
		if (brush is StiGlassBrush)
		{
			string arg3 = StiBrushSvgHelper.WriteGlassBrush(writer, brush, rect);
			return $"fill:url(#{arg3});";
		}
		if (brush is StiHatchBrush)
		{
			string arg4 = StiBrushSvgHelper.WriteHatchBrush(writer, brush);
			return $"fill:url(#{arg4});";
		}
		if (brush is StiBrush)
		{
			Color color2 = StiBrush.ToColor((StiBrush)((brush is StiBrush) ? brush : null));
			string text2 = $"stroke:rgb({color2.R},{color2.G},{color2.B})";
			double num2 = Math.Round((float)(int)color2.A / 255f, 3);
			if (num2 != 1.0)
			{
				text2 += $";stroke-opacity:{DoubleToString(num2)}";
			}
		}
		return "stroke-opacity:0";
	}

	internal static string DoubleToString(double value)
	{
		return value.ToString().Replace(",", ".");
	}
}
