using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Export.Services.Helpers;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Export;

public class StiSvgHelper
{
	private const double correctFontSize = 1.35;

	private const float pdfCKT = 0.56f;

	public static string GetLineStyleDash(StiPenStyle penStyle, double width)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected I4, but got Unknown
		string text = Math.Round(1.2 * width, 1).ToString().Replace(",", ".");
		string arg = Math.Round(3.0 * width, 1).ToString().Replace(",", ".");
		string result = string.Empty;
		switch (penStyle - 1)
		{
		case 3:
			result = string.Format("{0},{0}", text);
			break;
		case 0:
			result = $"{arg},{text}";
			break;
		case 1:
			result = string.Format("{0},{1},{1},{1}", arg, text);
			break;
		case 2:
			result = string.Format("{0},{1},{1},{1},{1},{1}", arg, text);
			break;
		}
		return result;
	}

	public static string ToUnits(double number)
	{
		return Math.Round(number, 2).ToString().Replace(",", ".");
	}

	private void GetTextLinesAndWidths()
	{
	}

	private static void WriteCoordinates(XmlTextWriter writer, StiSvgData svgData)
	{
		writer.WriteAttributeString("x", ToUnits(svgData.X));
		writer.WriteAttributeString("y", ToUnits(svgData.Y));
		writer.WriteAttributeString("width", ToUnits(svgData.Width));
		writer.WriteAttributeString("height", ToUnits(svgData.Height));
	}

	private static void WriteStrokeInfo(XmlTextWriter writer, Color color, double width, StiPenStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if ((int)style == 6)
		{
			writer.WriteAttributeString("stroke", "none");
			return;
		}
		writer.WriteAttributeString("stroke", $"#{color.R:X2}{color.G:X2}{color.B:X2}");
		if (color.A != byte.MaxValue)
		{
			writer.WriteAttributeString("stroke-opacity", Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", "."));
		}
		writer.WriteAttributeString("stroke-width", ToUnits(width));
		if ((int)style != 0)
		{
			writer.WriteAttributeString("stroke-dasharray", GetLineStyleDash(style, width));
		}
	}

	private static void WriteFillInfo(XmlTextWriter writer, Color color)
	{
		writer.WriteAttributeString("fill", $"#{color.R:X2}{color.G:X2}{color.B:X2}");
		if (color.A != byte.MaxValue)
		{
			writer.WriteAttributeString("fill-opacity", Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", "."));
		}
	}

	internal static bool CheckShape(StiComponent component)
	{
		if (!(component is StiShape))
		{
			return false;
		}
		if (!(component is StiShape stiShape))
		{
			return false;
		}
		if (component is IStiBrush { Brush: not null } stiBrush && !(stiBrush.Brush is StiSolidBrush) && !(stiBrush.Brush is StiEmptyBrush))
		{
			return false;
		}
		if (stiShape.ShapeType is StiVerticalLineShapeType || stiShape.ShapeType is StiHorizontalLineShapeType || stiShape.ShapeType is StiTopAndBottomLineShapeType || stiShape.ShapeType is StiLeftAndRightLineShapeType || stiShape.ShapeType is StiRectangleShapeType || stiShape.ShapeType is StiRoundedRectangleShapeType || stiShape.ShapeType is StiDiagonalDownLineShapeType || stiShape.ShapeType is StiDiagonalUpLineShapeType || stiShape.ShapeType is StiTriangleShapeType || stiShape.ShapeType is StiOvalShapeType || stiShape.ShapeType is StiArrowShapeType || stiShape.ShapeType is StiOctagonShapeType || stiShape.ShapeType is StiComplexArrowShapeType || stiShape.ShapeType is StiBentArrowShapeType || stiShape.ShapeType is StiChevronShapeType || stiShape.ShapeType is StiDivisionShapeType || stiShape.ShapeType is StiEqualShapeType || stiShape.ShapeType is StiFlowchartCardShapeType || stiShape.ShapeType is StiFlowchartCollateShapeType || stiShape.ShapeType is StiFlowchartDecisionShapeType || stiShape.ShapeType is StiFlowchartManualInputShapeType || stiShape.ShapeType is StiFlowchartOffPageConnectorShapeType || stiShape.ShapeType is StiFlowchartPreparationShapeType || stiShape.ShapeType is StiFlowchartSortShapeType || stiShape.ShapeType is StiFrameShapeType || stiShape.ShapeType is StiMinusShapeType || stiShape.ShapeType is StiMultiplyShapeType || stiShape.ShapeType is StiParallelogramShapeType || stiShape.ShapeType is StiPlusShapeType || stiShape.ShapeType is StiRegularPentagonShapeType || stiShape.ShapeType is StiTrapezoidShapeType || stiShape.ShapeType is StiSnipSameSideCornerRectangleShapeType || stiShape.ShapeType is StiSnipDiagonalSideCornerRectangleShapeType)
		{
			return true;
		}
		return false;
	}

	private static MemoryStream WriteDocument(StiReport report, StiPage page, bool standalone, ImageFormat imageFormat, float imageQuality, float imageResolution)
	{
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		int num = -1;
		xmlTextWriter.Indentation = ((num >= 0) ? num : 0);
		xmlTextWriter.Formatting = ((num >= 0) ? Formatting.Indented : Formatting.None);
		ImageCodecInfo imageCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
		imageResolution /= 100f;
		Hashtable guids = new Hashtable();
		if (standalone)
		{
			xmlTextWriter.WriteStartDocument();
		}
		xmlTextWriter.WriteStartElement("svg");
		xmlTextWriter.WriteAttributeString("version", "1.1");
		xmlTextWriter.WriteAttributeString("baseProfile", "full");
		if (!standalone)
		{
			xmlTextWriter.WriteAttributeString("style", "margin:5px; border:1px solid DarkGrey;");
		}
		xmlTextWriter.WriteAttributeString("xmlns", "http://www.w3.org/2000/svg");
		xmlTextWriter.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
		xmlTextWriter.WriteAttributeString("xmlns:ev", "http://www.w3.org/2001/xml-events");
		double num2 = 1.0;
		double num3 = num2 * report.Unit.ConvertToHInches(page.PageHeight * (double)page.SegmentPerHeight);
		double num4 = num2 * report.Unit.ConvertToHInches(page.PageWidth * (double)page.SegmentPerWidth);
		double num5 = num2 * report.Unit.ConvertToHInches(page.Margins.Left);
		double num6 = num2 * report.Unit.ConvertToHInches(page.Margins.Right);
		double num7 = num2 * report.Unit.ConvertToHInches(page.Margins.Top);
		double num8 = num2 * report.Unit.ConvertToHInches(page.Margins.Bottom);
		xmlTextWriter.WriteAttributeString("height", ToUnits(num3));
		xmlTextWriter.WriteAttributeString("width", ToUnits(num4));
		bool flag = false;
		xmlTextWriter.WriteStartElement("defs");
		foreach (StiComponent component in page.Components)
		{
			bool flag2 = component.Enabled;
			if (component is StiPointPrimitive)
			{
				flag2 = false;
			}
			if (component.Width == 0.0 || component.Height == 0.0)
			{
				flag2 = false;
			}
			if (!flag2)
			{
				continue;
			}
			double num9 = num2 * report.Unit.ConvertToHInches(component.Left);
			double num10 = num2 * report.Unit.ConvertToHInches(component.Top);
			double num11 = num2 * report.Unit.ConvertToHInches(component.Right);
			double num12 = num2 * report.Unit.ConvertToHInches(component.Bottom);
			StiSvgData svgData = default(StiSvgData);
			svgData.X = num9 + num5;
			svgData.Y = num10 + num7;
			svgData.Width = num11 - num9;
			svgData.Height = num12 - num10;
			svgData.Component = component;
			bool flag3 = component.IsExportAsImage(StiExportFormat.ImageSvg);
			if (component is StiText stiText && !flag3 && stiText.Text.ToString() != null && stiText.Text.ToString().Trim().Length > 0)
			{
				xmlTextWriter.WriteStartElement("clipPath");
				xmlTextWriter.WriteAttributeString("id", GetClipPathName(component, guids));
				xmlTextWriter.WriteStartElement("rect");
				WriteCoordinates(xmlTextWriter, svgData);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
			}
			object tagValue = component.TagValue;
			StiAdvancedWatermark val = (StiAdvancedWatermark)((tagValue is StiAdvancedWatermark) ? tagValue : null);
			if (val != null)
			{
				xmlTextWriter.WriteStartElement("clipPath");
				xmlTextWriter.WriteAttributeString("id", GetClipPathName(component, guids));
				xmlTextWriter.WriteStartElement("rect");
				WriteCoordinates(xmlTextWriter, svgData);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				if (val.WeaveEnabled)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			using Stream stream = typeof(StiFontIconsHelper).Assembly.GetManifestResourceStream("Stimulsoft.Base.FontIcons.Stimulsoft.ttf");
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<style>@font-face {font-family: 'Stimulsoft';src: url(data:font/ttf;base64,");
			stringBuilder.Append(Convert.ToBase64String(array));
			stringBuilder.Append(") format('truetype');font-weight: normal;font-style: normal;}</style>");
			xmlTextWriter.WriteRaw(stringBuilder.ToString());
		}
		xmlTextWriter.WriteEndElement();
		int gradientCounter = 1;
		StiContainer stiContainer = new StiContainer();
		stiContainer.Border = (StiBorder)page.Border.Clone();
		stiContainer.Brush = (StiBrush)page.Brush.Clone();
		if (stiContainer.Brush is StiEmptyBrush)
		{
			stiContainer.Brush = (StiBrush)new StiSolidBrush(Color.Transparent);
		}
		else if (StiBrush.ToColor(stiContainer.Brush).A == 0)
		{
			stiContainer.Brush = (StiBrush)new StiSolidBrush(Color.White);
		}
		StiSvgData svgData2 = default(StiSvgData);
		svgData2.X = 0.0;
		svgData2.Y = 0.0;
		svgData2.Width = num4;
		svgData2.Height = num3;
		svgData2.Component = stiContainer;
		WriteBorder1(xmlTextWriter, svgData2, ref gradientCounter);
		WriteWatermark(xmlTextWriter, num, page, behind: true, num4, num3, imageResolution);
		string value = page.BookmarkValue as string;
		if (!string.IsNullOrEmpty(value))
		{
			xmlTextWriter.WriteStartElement("view");
			xmlTextWriter.WriteAttributeString("id", value);
			xmlTextWriter.WriteAttributeString("viewBox", "0 0 1 1");
			xmlTextWriter.WriteEndElement();
		}
		foreach (StiComponent component2 in page.Components)
		{
			bool flag4 = component2.Enabled;
			if (component2 is StiPointPrimitive)
			{
				flag4 = false;
			}
			if (component2.Width == 0.0 || component2.Height == 0.0)
			{
				flag4 = false;
			}
			if (!flag4)
			{
				continue;
			}
			double num13 = num2 * report.Unit.ConvertToHInches(component2.Left);
			double num14 = num2 * report.Unit.ConvertToHInches(component2.Top);
			double num15 = num2 * report.Unit.ConvertToHInches(component2.Right);
			double num16 = num2 * report.Unit.ConvertToHInches(component2.Bottom);
			StiSvgData stiSvgData = default(StiSvgData);
			stiSvgData.X = num13 + num5;
			stiSvgData.Y = num14 + num7;
			stiSvgData.Width = num15 - num13;
			stiSvgData.Height = num16 - num14;
			stiSvgData.Component = component2;
			bool flag5 = CheckShape(component2);
			bool flag6 = component2.IsExportAsImage(StiExportFormat.ImageSvg);
			if (!flag5 && !flag6)
			{
				WriteBorder1(xmlTextWriter, stiSvgData, ref gradientCounter);
			}
			object tagValue2 = component2.TagValue;
			StiAdvancedWatermark val2 = (StiAdvancedWatermark)((tagValue2 is StiAdvancedWatermark) ? tagValue2 : null);
			if (val2 != null)
			{
				RenderAdvancedWatermark(xmlTextWriter, num, val2, stiSvgData, page, guids);
			}
			string value2 = component2.HyperlinkValue as string;
			if (!string.IsNullOrEmpty(value2))
			{
				xmlTextWriter.WriteStartElement("a");
				xmlTextWriter.WriteAttributeString("xlink:href", value2);
			}
			if (component2 is StiText && !flag6)
			{
				WriteText(xmlTextWriter, stiSvgData, num, useClip: true, guids);
			}
			else if (flag5)
			{
				WriteShape(xmlTextWriter, stiSvgData, num, useClip: true, guids);
			}
			else if (component2 is StiBarCode)
			{
				WriteBarCode(xmlTextWriter, stiSvgData);
			}
			else if (component2 is StiSparkline)
			{
				StiSparklineSvgHelper.WriteSparkline(xmlTextWriter, stiSvgData);
			}
			else if (component2 is StiCheckBox)
			{
				WriteCheckBox(xmlTextWriter, stiSvgData);
			}
			else if (component2 is StiChart)
			{
				StiChartSvgHelper.WriteChart(xmlTextWriter, stiSvgData, needAnimation: false);
			}
			else if (component2 is StiMap && ((StiMap)component2).MapMode != StiMapMode.Online)
			{
				StiMapSvgHelper.DrawMap(xmlTextWriter, (StiMap)component2, stiSvgData.X, stiSvgData.Y, stiSvgData.Width, stiSvgData.Height, animated: false);
			}
			else if (flag6)
			{
				WriteImage(xmlTextWriter, stiSvgData, imageResolution, imageFormat, imageQuality, imageCodec);
			}
			else
			{
				string value3 = stiSvgData.Component.BookmarkValue as string;
				if (!string.IsNullOrEmpty(value3))
				{
					xmlTextWriter.WriteStartElement("view");
					xmlTextWriter.WriteAttributeString("id", value3);
					xmlTextWriter.WriteAttributeString("viewBox", $"{ToUnits(stiSvgData.X)} {ToUnits(stiSvgData.Y)} {ToUnits(stiSvgData.Width)} {ToUnits(stiSvgData.Height)}");
					xmlTextWriter.WriteEndElement();
				}
			}
			if (!string.IsNullOrEmpty(value2))
			{
				xmlTextWriter.WriteEndElement();
			}
			if (!standalone)
			{
				stiSvgData.X = Math.Round(stiSvgData.X - 0.5) + 0.5;
				stiSvgData.Y = Math.Round(stiSvgData.Y - 0.5) + 0.5;
				stiSvgData.Width = Math.Round(num15 + num5 - 0.5) + 0.5 - stiSvgData.X;
				stiSvgData.Height = Math.Round(num16 + num7 - 0.5) + 0.5 - stiSvgData.Y;
			}
			if (component2 is StiRectanglePrimitive)
			{
				WriteRoundedRectanglePrimitive(xmlTextWriter, stiSvgData);
			}
			else
			{
				WriteBorder2(xmlTextWriter, stiSvgData);
			}
		}
		svgData2.X = num5;
		svgData2.Y = num7;
		svgData2.Width -= num5 + num6;
		svgData2.Height -= num7 + num8;
		WriteBorder2(xmlTextWriter, svgData2);
		WriteWatermark(xmlTextWriter, num, page, behind: false, num4, num3, imageResolution);
		xmlTextWriter.WriteFullEndElement();
		if (standalone)
		{
			xmlTextWriter.WriteEndDocument();
		}
		xmlTextWriter.Flush();
		return memoryStream;
	}

	private static void WriteWatermark(XmlTextWriter writer, int xmlIndentation, StiPage page, bool behind, double pageWidth, double pageHeight, float imageResolution)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		StiWatermark watermark = page.Watermark;
		if (watermark != null && watermark.Enabled)
		{
			if (watermark.ExistImage() && watermark.ShowImageBehind == behind)
			{
				WriteWatermarkImage(writer, new RectangleD(0.0, 0.0, pageWidth, pageHeight), watermark.ImageBytes, watermark.TryTakeGdiImage(), watermark.ImageStretch, watermark.ImageTiling, watermark.AspectRatio, watermark.ImageMultipleFactor, watermark.ImageAlignment);
			}
			if (!string.IsNullOrEmpty(watermark.Text) && watermark.ShowBehind == behind)
			{
				StiSvgData svgData = default(StiSvgData);
				svgData.X = 0.0;
				svgData.Y = 0.0;
				svgData.Width = pageWidth;
				svgData.Height = pageHeight;
				StiText stiText = new StiText(new RectangleD(svgData.X, svgData.Y, svgData.Width, svgData.Height));
				stiText.Text = watermark.Text;
				stiText.TextBrush = watermark.TextBrush;
				stiText.Font = watermark.Font;
				stiText.TextOptions = new StiTextOptions();
				stiText.TextOptions.Angle = watermark.Angle;
				stiText.HorAlignment = (StiTextHorAlignment)1;
				stiText.VertAlignment = (StiVertAlignment)1;
				stiText.Page = page;
				stiText.TextQuality = StiTextQuality.Standard;
				svgData.Component = stiText;
				WriteText(writer, svgData, xmlIndentation, useClip: false, null);
			}
			object tagValue = page.TagValue;
			StiAdvancedWatermark val = (StiAdvancedWatermark)((tagValue is StiAdvancedWatermark) ? tagValue : null);
			if (val != null && val.WeaveEnabled && behind)
			{
				WriteWatermarkWeave(writer, val, new RectangleD(0.0, 0.0, pageWidth, pageHeight));
			}
		}
	}

	private static void WriteWatermarkImage(XmlTextWriter writer, RectangleD mainRect, byte[] imageBytes, Image image, bool imageStretch, bool isImageTiling, bool imageAspectRatio, double imageMultipleFactor, ContentAlignment imageAlignment)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Invalid comparison between Unknown and I4
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Invalid comparison between Unknown and I4
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Invalid comparison between Unknown and I4
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Invalid comparison between Unknown and I4
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Invalid comparison between Unknown and I4
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Invalid comparison between Unknown and I4
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Invalid comparison between Unknown and I4
		double num = (double)image.Width * imageMultipleFactor;
		double num2 = (double)image.Height * imageMultipleFactor;
		double num3 = ((RectangleD)(ref mainRect)).Left;
		double num4 = ((RectangleD)(ref mainRect)).Top;
		int num5 = 1;
		int num6 = 1;
		if (imageStretch)
		{
			double num7 = num2 / num;
			num = ((RectangleD)(ref mainRect)).Width;
			num2 = ((RectangleD)(ref mainRect)).Height;
			isImageTiling = false;
			if (imageAspectRatio)
			{
				if (((RectangleD)(ref mainRect)).Height / ((RectangleD)(ref mainRect)).Width > num7)
				{
					num2 = num * num7;
				}
				else
				{
					num = num2 / num7;
				}
			}
		}
		if (isImageTiling)
		{
			imageAlignment = (ContentAlignment)1;
			num5 = (int)(((RectangleD)(ref mainRect)).Width / num) + 1;
			num6 = (int)(((RectangleD)(ref mainRect)).Height / num2) + 1;
		}
		if ((int)imageAlignment <= 32)
		{
			if ((int)imageAlignment != 2)
			{
				if ((int)imageAlignment == 4)
				{
					goto IL_00d8;
				}
				if ((int)imageAlignment != 32)
				{
					goto IL_00e4;
				}
			}
			goto IL_00c0;
		}
		if ((int)imageAlignment != 64)
		{
			if ((int)imageAlignment == 512)
			{
				goto IL_00c0;
			}
			if ((int)imageAlignment != 1024)
			{
				goto IL_00e4;
			}
		}
		goto IL_00d8;
		IL_00c0:
		num3 += (((RectangleD)(ref mainRect)).Width - num) / 2.0;
		goto IL_00e4;
		IL_00e4:
		if ((int)imageAlignment <= 64)
		{
			if ((int)imageAlignment == 16 || (int)imageAlignment == 32 || (int)imageAlignment == 64)
			{
				num4 += (((RectangleD)(ref mainRect)).Height - num2) / 2.0;
			}
		}
		else if ((int)imageAlignment == 256 || (int)imageAlignment == 512 || (int)imageAlignment == 1024)
		{
			num4 += ((RectangleD)(ref mainRect)).Height - num2;
		}
		StiSvgData svgData = default(StiSvgData);
		svgData.Width = num;
		svgData.Height = num2;
		for (int i = 0; i < num6; i++)
		{
			for (int j = 0; j < num5; j++)
			{
				svgData.X = num3 + num * (double)j;
				svgData.Y = num4 + num2 * (double)i;
				WriteImageBytes(writer, svgData, imageBytes);
			}
		}
		return;
		IL_00d8:
		num3 += ((RectangleD)(ref mainRect)).Width - num;
		goto IL_00e4;
	}

	internal static void WriteImageBytes(XmlTextWriter writer, StiSvgData svgData, byte[] imageBytes)
	{
		if (imageBytes != null && imageBytes.Length != 0)
		{
			string arg = "png";
			if (Stimulsoft.Report.Helpers.StiImageHelper.IsMetafile(imageBytes))
			{
				imageBytes = StiMetafileConverter.MetafileToPngBytes(imageBytes, (int)svgData.Width, (int)svgData.Height);
			}
			else if (Stimulsoft.Report.Helpers.StiImageHelper.IsBmp(imageBytes))
			{
				arg = "bmp";
			}
			else if (Stimulsoft.Report.Helpers.StiImageHelper.IsJpeg(imageBytes))
			{
				arg = "jpeg";
			}
			else if (Stimulsoft.Report.Helpers.StiImageHelper.IsGif(imageBytes))
			{
				arg = "gif";
			}
			else if (StiSvgHelper.IsSvg(imageBytes))
			{
				arg = "svg+xml";
			}
			writer.WriteStartElement("image");
			WriteCoordinates(writer, svgData);
			writer.WriteStartAttribute("href");
			writer.WriteString($"data:image/{arg};base64,");
			writer.WriteRaw("\r\n");
			writer.WriteBase64(imageBytes, 0, imageBytes.Length);
			writer.WriteEndAttribute();
			writer.WriteEndElement();
		}
	}

	private static void WriteWatermarkWeave(XmlTextWriter writer, StiAdvancedWatermark advWatermark, RectangleD mainRect)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		if (advWatermark == null)
		{
			return;
		}
		Color weaveMajorColor = advWatermark.WeaveMajorColor;
		Color weaveMinorColor = advWatermark.WeaveMinorColor;
		FontFamily val = StiFontIconsHelper.GetFontFamilyIcons();
		if (val == null)
		{
			val = StiFontCollection.GetFontFamily("Stimulsoft", false);
		}
		Font val2 = new Font(val, (float)((double)(advWatermark.WeaveMajorSize * 5) * 0.57));
		Font val3 = new Font(val, (float)((double)(advWatermark.WeaveMinorSize * 5) * 0.57));
		string content = StiFontIconsHelper.GetContent(advWatermark.WeaveMajorIcon);
		string content2 = StiFontIconsHelper.GetContent(advWatermark.WeaveMinorIcon);
		double num = advWatermark.WeaveDistance;
		int num2 = (int)(Math.Max(((RectangleD)(ref mainRect)).Width, ((RectangleD)(ref mainRect)).Height) / 2.0 * 1.4 / num) + 1;
		writer.WriteStartElement("g");
		writer.WriteAttributeString("transform", $"translate({ToUnits(((RectangleD)(ref mainRect)).X + ((RectangleD)(ref mainRect)).Width / 2.0)},{ToUnits(((RectangleD)(ref mainRect)).Y + ((RectangleD)(ref mainRect)).Height / 2.0)})");
		if (advWatermark.WeaveAngle != 0)
		{
			writer.WriteStartElement("g");
			writer.WriteAttributeString("transform", $"rotate({ToUnits(advWatermark.WeaveAngle)})");
		}
		for (int i = -num2; i <= num2; i++)
		{
			for (int j = -num2; j <= num2; j++)
			{
				int num3 = (int)((double)j * num);
				int num4 = (int)((double)i * num);
				bool flag = (Math.Abs(j + i) & 1) == 0;
				Color textColor = (flag ? weaveMajorColor : weaveMinorColor);
				Font font = (flag ? val2 : val3);
				string text = (flag ? content : content2);
				writer.WriteStartElement("text");
				writer.WriteAttributeString("x", ToUnits(num3));
				writer.WriteAttributeString("y", ToUnits(num4));
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(GetStyleString(font, textColor));
				stringBuilder.Append("text-anchor:middle;alignment-baseline:middle;");
				writer.WriteAttributeString("style", stringBuilder.ToString());
				writer.WriteString(text);
				writer.WriteEndElement();
			}
		}
		if (advWatermark.WeaveAngle != 0)
		{
			writer.WriteFullEndElement();
		}
		writer.WriteFullEndElement();
	}

	private static void RenderAdvancedWatermark(XmlTextWriter writer, int xmlIndentation, StiAdvancedWatermark advWatermark, StiSvgData pp, StiPage page, Hashtable guids)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		if (advWatermark != null)
		{
			writer.WriteStartElement("g");
			writer.WriteAttributeString("clip-path", $"url(#{GetClipPathName(pp.Component, guids)})");
			RectangleD val = default(RectangleD);
			((RectangleD)(ref val))._002Ector(pp.X, pp.Y, pp.Width, pp.Height);
			if (advWatermark.ImageEnabled && advWatermark.ImageBytes != null)
			{
				WriteWatermarkImage(writer, val, advWatermark.ImageBytes, advWatermark.Image, advWatermark.ImageStretch, advWatermark.ImageTiling, advWatermark.ImageAspectRatio, advWatermark.ImageMultipleFactor, advWatermark.ImageAlignment);
			}
			if (!string.IsNullOrEmpty(advWatermark.Text))
			{
				StiText stiText = new StiText(val);
				stiText.Text = advWatermark.Text;
				stiText.TextBrush = (StiBrush)new StiSolidBrush(advWatermark.TextColor);
				stiText.Font = advWatermark.TextFont;
				stiText.Angle = advWatermark.TextAngle;
				stiText.HorAlignment = (StiTextHorAlignment)1;
				stiText.VertAlignment = (StiVertAlignment)1;
				stiText.Page = page;
				stiText.TextQuality = StiTextQuality.Standard;
				StiSvgData svgData = default(StiSvgData);
				svgData.X = pp.X;
				svgData.Y = pp.Y;
				svgData.Width = pp.Width;
				svgData.Height = pp.Height;
				svgData.Component = stiText;
				WriteText(writer, svgData, xmlIndentation, useClip: false, null);
			}
			if (advWatermark.WeaveEnabled)
			{
				WriteWatermarkWeave(writer, advWatermark, val);
			}
			writer.WriteFullEndElement();
		}
	}

	private static void WriteBorder1(XmlTextWriter writer, StiSvgData svgData, ref int gradientCounter)
	{
		if (!(svgData.Component is IStiBrush { Brush: not null } stiBrush))
		{
			return;
		}
		if (stiBrush.Brush is StiGradientBrush)
		{
			string arg = StiBrushSvgHelper.WriteGradientBrush(writer, stiBrush.Brush, new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height));
			writer.WriteStartElement("rect");
			WriteCoordinates(writer, svgData);
			writer.WriteAttributeString("fill", $"url(#{arg})");
			writer.WriteEndElement();
			return;
		}
		if (stiBrush.Brush is StiGlareBrush)
		{
			string arg2 = StiBrushSvgHelper.WriteGlareBrush(writer, stiBrush.Brush, new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height));
			writer.WriteStartElement("rect");
			WriteCoordinates(writer, svgData);
			writer.WriteAttributeString("fill", $"url(#{arg2})");
			writer.WriteEndElement();
			return;
		}
		if (stiBrush.Brush is StiGlassBrush)
		{
			string arg3 = StiBrushSvgHelper.WriteGlassBrush(writer, stiBrush.Brush, new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height));
			writer.WriteStartElement("rect");
			WriteCoordinates(writer, svgData);
			writer.WriteAttributeString("fill", $"url(#{arg3})");
			writer.WriteEndElement();
			return;
		}
		if (stiBrush.Brush is StiHatchBrush)
		{
			string arg4 = StiBrushSvgHelper.WriteHatchBrush(writer, stiBrush.Brush);
			writer.WriteStartElement("rect");
			WriteCoordinates(writer, svgData);
			writer.WriteAttributeString("fill", $"url(#{arg4})");
			writer.WriteEndElement();
			return;
		}
		Color color = StiBrush.ToColor(stiBrush.Brush);
		if (color != Color.Transparent)
		{
			writer.WriteStartElement("rect");
			WriteCoordinates(writer, svgData);
			WriteFillInfo(writer, color);
			writer.WriteEndElement();
		}
	}

	private static void WriteBorder2(XmlTextWriter writer, StiSvgData svgData)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Invalid comparison between Unknown and I4
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		if (!(svgData.Component is IStiBorder { Border: not null } stiBorder) || (int)stiBorder.Border.Side == 0 || (int)stiBorder.Border.Style == 6)
		{
			return;
		}
		StiBorder border = stiBorder.Border;
		if ((int)border.Side == 15)
		{
			writer.WriteStartElement("rect");
			WriteCoordinates(writer, svgData);
			writer.WriteAttributeString("fill", "none");
			WriteStrokeInfo(writer, border.Color, border.Size, border.Style);
			writer.WriteEndElement();
			return;
		}
		writer.WriteStartElement("path");
		string arg = ToUnits(svgData.X);
		string arg2 = ToUnits(svgData.X + svgData.Width);
		string arg3 = ToUnits(svgData.Y + svgData.Height);
		string arg4 = ToUnits(svgData.Y);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		if ((border.Side & 2) != 0)
		{
			stringBuilder.Append($"M {arg} {arg3} ");
			stringBuilder.Append($"L {arg} {arg4} ");
			flag = true;
		}
		else
		{
			flag = false;
		}
		if ((border.Side & 1) != 0)
		{
			if (!flag)
			{
				stringBuilder.Append($"M {arg} {arg4} ");
			}
			stringBuilder.Append($"L {arg2} {arg4} ");
			flag = true;
		}
		else
		{
			flag = false;
		}
		if ((border.Side & 4) != 0)
		{
			if (!flag)
			{
				stringBuilder.Append($"M {arg2} {arg4} ");
			}
			stringBuilder.Append($"L {arg2} {arg3} ");
			flag = true;
		}
		else
		{
			flag = false;
		}
		if ((border.Side & 8) != 0)
		{
			if (!flag)
			{
				stringBuilder.Append($"M {arg2} {arg3} ");
			}
			stringBuilder.Append($"L {arg} {arg3} ");
		}
		writer.WriteAttributeString("d", stringBuilder.ToString());
		writer.WriteAttributeString("fill", "none");
		WriteStrokeInfo(writer, border.Color, border.Size, border.Style);
		writer.WriteEndElement();
	}

	private static void WriteText(XmlTextWriter writer, StiSvgData svgData, int xmlIndentation, bool useClip, Hashtable guids, Color? textBackColor = null)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Invalid comparison between Unknown and I4
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Invalid comparison between Unknown and I4
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Invalid comparison between Unknown and I4
		//IL_0c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Invalid comparison between Unknown and I4
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Invalid comparison between Unknown and I4
		//IL_0839: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6b: Invalid comparison between Unknown and I4
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Invalid comparison between Unknown and I4
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Invalid comparison between Unknown and I4
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0975: Unknown result type (might be due to invalid IL or missing references)
		StiText stiText = svgData.Component as StiText;
		bool flag = (int)stiText.HorAlignment == 3;
		StiTextOptions val = (StiTextOptions)(((object)stiText.TextOptions) ?? ((object)new StiTextOptions()));
		if (flag)
		{
			object obj = val.Clone();
			val = (StiTextOptions)((obj is StiTextOptions) ? obj : null);
			val.WordWrap = true;
		}
		float num;
		for (num = val.Angle; num >= 360f; num -= 360f)
		{
		}
		string text = ((stiText.Report != null && stiText.Report.Info.ForceDesigningMode && stiText.Report.Designer != null && stiText.Report.Designer.UseAliases) ? StiExpressionPacker.PackExpression(stiText.GetTextInternal(), stiText.Report, useBraces: true) : stiText.Text.ToString());
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		RectangleD paintRectangle = stiText.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
		RectangleD rect = stiText.ConvertTextMargins(paintRectangle, convert: false);
		rect = stiText.ConvertTextBorders(rect, convert: false);
		RectangleD val2 = default(RectangleD);
		((RectangleD)(ref val2))._002Ector(((RectangleD)(ref rect)).X, ((RectangleD)(ref rect)).Y, ((RectangleD)(ref rect)).Width, ((RectangleD)(ref rect)).Height);
		if ((num > 45f && num < 135f) || (num > 225f && num < 315f))
		{
			double width = ((RectangleD)(ref val2)).Width;
			((RectangleD)(ref val2)).Width = ((RectangleD)(ref val2)).Height;
			((RectangleD)(ref val2)).Height = width;
		}
		Bitmap val3 = new Bitmap(1, 1);
		Graphics val4 = Graphics.FromImage((Image)(object)val3);
		if (!StiDpiHelper.IsWindows && stiText.CheckAllowHtmlTags() && !StiOptions.Engine.UseNewHtmlEngine)
		{
			text = StiTextRenderer.GetPlainTextFromHtmlTags(text);
		}
		if ((StiDpiHelper.IsWindows || StiOptions.Engine.UseNewHtmlEngine) && stiText.CheckAllowHtmlTags())
		{
			double num2 = svgData.X;
			double num3 = svgData.Y;
			double num4 = svgData.Width;
			double num5 = svgData.Height;
			if (num != 0f)
			{
				if ((num > 45f && num < 135f) || (num > 225f && num < 315f))
				{
					double num6 = num4;
					num4 = num5;
					num5 = num6;
				}
				num2 = (0.0 - num4) / 2.0;
				num3 = (0.0 - num5) / 2.0;
				if (useClip)
				{
					writer.WriteStartElement("g");
					writer.WriteAttributeString("clip-path", $"url(#{GetClipPathName(stiText, guids)})");
				}
				writer.WriteStartElement("g");
				writer.WriteAttributeString("transform", $"translate({ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y + svgData.Height / 2.0)})");
				writer.WriteStartElement("g");
				writer.WriteAttributeString("transform", $"rotate({ToUnits(0f - num)})");
			}
			List<RunInfo> outRunsList = new List<RunInfo>();
			List<StiFontState> outFontsList = new List<StiFontState>();
			bool flag2 = val.WordWrap || flag;
			string name = stiText.Font.Name;
			Color color = StiBrush.ToColor(stiText.TextBrush);
			Color color2 = StiBrush.ToColor(stiText.Brush);
			if (StiOptions.Engine.UseNewHtmlEngine)
			{
				StiHtmlTextRender.DrawTextForOutput(stiText, out outRunsList, out outFontsList);
			}
			else
			{
				StiTextRenderer.DrawTextForOutput(val4, text, stiText.Font, val2, color, color2, (double)(stiText.LineSpacing * StiOptions.Engine.TextLineSpacingScale), stiText.HorAlignment, stiText.VertAlignment, flag2, val.RightToLeft, 1.0, (double)num, val.Trimming, val.LineLimit, stiText.CheckAllowHtmlTags(), outRunsList, outFontsList, val, (double?)1.0);
			}
			foreach (RunInfo item in outRunsList)
			{
				int num7 = color2.ToArgb();
				Color backColor = item.BackColor;
				if (num7 != backColor.ToArgb())
				{
					StiFontState val5 = outFontsList[item.FontIndex];
					double number = num2 + item.XPos + 0.25;
					double number2 = num3 + item.YPos;
					double num8 = 0.0;
					int[] widths = item.Widths;
					foreach (int num9 in widths)
					{
						num8 += (double)num9;
					}
					writer.WriteStartElement("rect");
					writer.WriteAttributeString("x", ToUnits(number));
					writer.WriteAttributeString("y", ToUnits(number2));
					writer.WriteAttributeString("width", ToUnits(num8));
					writer.WriteAttributeString("height", ToUnits(val5.FontBase.Height));
					WriteFillInfo(writer, item.BackColor);
					writer.WriteEndElement();
				}
			}
			writer.WriteStartElement("text");
			writer.WriteAttributeString("x", ToUnits(num2));
			writer.WriteAttributeString("y", ToUnits(num3));
			string value = stiText.BookmarkValue as string;
			if (!string.IsNullOrEmpty(value))
			{
				writer.WriteAttributeString("id", value);
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (useClip && num == 0f)
			{
				stringBuilder.Append($"clip-path: url(#{GetClipPathName(stiText, guids)});");
			}
			stringBuilder.Append(GetStyleString(stiText.Font, color));
			stringBuilder.Append("white-space:pre;");
			writer.WriteAttributeString("style", stringBuilder.ToString());
			foreach (RunInfo item2 in outRunsList)
			{
				StiFontState val6 = outFontsList[item2.FontIndex];
				double num10 = val6.FontBase.SizeInPoints;
				if (StiDpiHelper.NeedGraphicsScale && StiDpiHelper.NeedFontScaling)
				{
					num10 /= StiDpiHelper.GraphicsScale;
				}
				double num11 = num2 + (((RectangleD)(ref rect)).Left + item2.XPos - ((RectangleD)(ref paintRectangle)).Left) + 1.0;
				double number3 = num3 + item2.YPos + val6.Ascend + 0.5;
				StringBuilder stringBuilder2 = new StringBuilder();
				if (name != val6.FontNameReal)
				{
					stringBuilder2.Append($"font-family:'{val6.FontNameReal}';");
				}
				if ((double)stiText.Font.SizeInPoints != num10)
				{
					stringBuilder2.Append($"font-size:{Math.Round(num10, 2).ToString().Replace(',', '.')}pt;");
				}
				if (stiText.Font.Bold != val6.FontBase.Bold)
				{
					stringBuilder2.Append("font-weight:" + (val6.FontBase.Bold ? "bold" : "normal") + ";");
				}
				if (stiText.Font.Italic != val6.FontBase.Italic)
				{
					stringBuilder2.Append("font-style:" + (val6.FontBase.Italic ? "italic" : "normal") + ";");
				}
				if (stiText.Font.Underline != val6.FontBase.Underline || stiText.Font.Strikeout != val6.FontBase.Strikeout)
				{
					string arg = ((!val6.FontBase.Underline) ? (val6.FontBase.Strikeout ? "line-through" : "none") : (val6.FontBase.Strikeout ? "underline line-through" : "underline"));
					stringBuilder2.Append($"text-decoration:{arg};");
				}
				int num12 = color.ToArgb();
				Color backColor = item2.TextColor;
				if (num12 != backColor.ToArgb())
				{
					backColor = item2.TextColor;
					object arg2 = backColor.R;
					backColor = item2.TextColor;
					object arg3 = backColor.G;
					backColor = item2.TextColor;
					stringBuilder2.Append($"fill:#{arg2:X2}{arg3:X2}{backColor.B:X2};");
					backColor = item2.TextColor;
					if (backColor.A != byte.MaxValue)
					{
						backColor = item2.TextColor;
						stringBuilder2.Append(string.Format("fill-opacity:{0};", Math.Round((float)(int)backColor.A / 255f, 3).ToString().Replace(",", ".")));
					}
				}
				double num13 = 0.0;
				int[] widths2 = item2.Widths;
				foreach (int num14 in widths2)
				{
					num13 += (double)num14;
				}
				if ((int)item2.TextAlign != 0)
				{
					if ((int)item2.TextAlign == 1)
					{
						num11 += num13 / 2.0;
						stringBuilder2.Append("text-anchor:middle;");
					}
					if ((int)item2.TextAlign == 2)
					{
						num11 += num13;
						stringBuilder2.Append("text-anchor:end;");
					}
				}
				writer.WriteStartElement("tspan");
				writer.WriteAttributeString("x", ToUnits(num11));
				writer.WriteAttributeString("y", ToUnits(number3));
				if (stringBuilder2.Length > 0)
				{
					writer.WriteAttributeString("style", stringBuilder2.ToString());
				}
				writer.WriteAttributeString("textLength", ToUnits(num13));
				if (!string.IsNullOrWhiteSpace(item2.Href))
				{
					writer.WriteStartElement("a");
					writer.WriteAttributeString("href", item2.Href);
					writer.WriteAttributeString("target", "_blank");
				}
				string text2 = item2.Text;
				if (text2 != null && text2.Trim().Length == 0)
				{
					text2 = "\u00a0";
				}
				writer.WriteString(text2);
				if (!string.IsNullOrWhiteSpace(item2.Href))
				{
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
			writer.WriteFullEndElement();
			if (num != 0f)
			{
				writer.WriteFullEndElement();
				writer.WriteFullEndElement();
				if (useClip)
				{
					writer.WriteFullEndElement();
				}
			}
			return;
		}
		List<LineInfo> list = StiTextDrawing.SplitTextWordwrap(text, val4, StiFontUtils.ChangeFontSize(stiText.Font, stiText.Font.Size * (float)StiDpiHelper.GraphicsScale), val2, val, stiText.HorAlignment, stiText.TextQuality == StiTextQuality.Typographic);
		foreach (LineInfo item3 in list)
		{
			item3.LineHeight *= (double)stiText.LineSpacing;
		}
		bool flag3 = list.Count > 0;
		if (list.Count == 1 && list[0].Text.Length == 0)
		{
			flag3 = false;
		}
		if (!flag3)
		{
			return;
		}
		double num15 = 0.0;
		foreach (LineInfo item4 in list)
		{
			num15 += item4.LineHeight;
		}
		double num16 = 0.0;
		if (stiText.Border != null)
		{
			num16 = stiText.Border.Size / 2.0;
		}
		StringBuilder stringBuilder3 = new StringBuilder();
		stringBuilder3.Append("white-space:pre;");
		if (useClip && num == 0f)
		{
			stringBuilder3.Append($"clip-path: url(#{GetClipPathName(stiText, guids)});");
		}
		double num17 = svgData.X + stiText.Margins.Left;
		double num18 = svgData.Y + stiText.Margins.Top;
		double num19 = svgData.Width - stiText.Margins.Left - stiText.Margins.Right;
		double num20 = svgData.Height - stiText.Margins.Top - stiText.Margins.Bottom;
		double num21 = num17;
		double num22 = num18 + (double)stiText.Font.SizeInPoints * 0.9 * 1.35;
		if (num % 90f == 0f)
		{
			if ((!val.RightToLeft && (int)stiText.HorAlignment == 0) || (val.RightToLeft && (int)stiText.HorAlignment == 2))
			{
				if (val.RightToLeft)
				{
					stringBuilder3.Append(string.Format("text-anchor:{0};", "end"));
				}
				num21 += 2.5 + num16;
			}
			if ((int)stiText.HorAlignment == 1)
			{
				stringBuilder3.Append(string.Format("text-anchor:{0};", "middle"));
				num21 = ((num != 90f && num != 270f) ? (num21 + num19 / 2.0) : (num21 + num20 / 2.0));
			}
			if ((val.RightToLeft && (int)stiText.HorAlignment == 0) || (!val.RightToLeft && (int)stiText.HorAlignment == 2))
			{
				stringBuilder3.Append(string.Format("text-anchor:{0};", val.RightToLeft ? "start" : "end"));
				num21 = ((num != 90f && num != 270f) ? (num21 + (num19 - 2.0 - num16)) : (num21 + (num20 - 2.0 - num16)));
			}
			if ((int)stiText.HorAlignment == 3)
			{
				num21 += 2.0 + num16;
			}
			if ((int)stiText.VertAlignment == 0)
			{
				num22 += num16;
			}
			if ((int)stiText.VertAlignment == 1)
			{
				num22 = ((num != 90f && num != 270f) ? (num22 + (num20 - num15) / 2.0) : (num22 + (num19 - num15) / 2.0));
			}
			if ((int)stiText.VertAlignment == 2)
			{
				num22 = ((num != 90f && num != 270f) ? (num22 + (num20 - num15 - num16)) : (num22 + (num19 - num15 - num16)));
			}
		}
		else
		{
			stringBuilder3.Append(string.Format("text-anchor:{0};", "middle"));
			num21 += num19 / 2.0;
			num22 += (num20 - num15) / 2.0;
		}
		stringBuilder3.Append(GetStyleString(stiText.Font, StiBrush.ToColor(stiText.TextBrush)));
		if (num != 0f)
		{
			if (useClip)
			{
				writer.WriteStartElement("g");
				writer.WriteAttributeString("clip-path", $"url(#{GetClipPathName(stiText, guids)})");
			}
			writer.WriteStartElement("g");
			writer.WriteAttributeString("transform", $"translate({ToUnits(num17 + num19 / 2.0)},{ToUnits(num18 + num20 / 2.0)})");
			writer.WriteStartElement("g");
			writer.WriteAttributeString("transform", $"rotate({ToUnits(0f - num)})");
			if (num == 90f || num == 270f)
			{
				num21 -= num17 + num20 / 2.0;
				num22 -= num18 + num19 / 2.0;
			}
			else
			{
				num21 -= num17 + num19 / 2.0;
				num22 -= num18 + num20 / 2.0;
			}
		}
		bool flag4 = StiBidirectionalConvert.StringContainArabicOrHebrew(text);
		bool flag5 = textBackColor.HasValue && !textBackColor.Equals(Color.Transparent);
		if (flag5)
		{
			writer.WriteStartElement("filter");
			writer.WriteAttributeString("x", "0");
			writer.WriteAttributeString("y", "0");
			writer.WriteAttributeString("width", "1");
			writer.WriteAttributeString("height", "1");
			writer.WriteAttributeString("id", "solid");
			writer.WriteStartElement("feFlood");
			writer.WriteAttributeString("flood-color", $"rgb({textBackColor?.R},{textBackColor?.G},{textBackColor?.B})");
			writer.WriteAttributeString("result", "bg");
			writer.WriteEndElement();
			writer.WriteStartElement("feMerge");
			writer.WriteStartElement("feMergeNode");
			writer.WriteAttributeString("in", "bg");
			writer.WriteEndElement();
			writer.WriteStartElement("feMergeNode");
			writer.WriteAttributeString("in", "SourceGraphic");
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		writer.WriteStartElement("text");
		writer.WriteAttributeString("x", ToUnits(num21));
		writer.WriteAttributeString("y", ToUnits(num22));
		writer.WriteAttributeString("style", stringBuilder3.ToString());
		if (val.RightToLeft)
		{
			writer.WriteAttributeString("direction", "rtl");
			writer.WriteAttributeString("unicode-bidi", "embed");
		}
		if (flag5)
		{
			writer.WriteAttributeString("filter", "url(#solid)");
		}
		string value2 = stiText.BookmarkValue as string;
		if (!string.IsNullOrEmpty(value2))
		{
			writer.WriteAttributeString("id", value2);
		}
		writer.Indentation = 0;
		writer.Formatting = Formatting.None;
		for (int k = 0; k < list.Count; k++)
		{
			LineInfo val7 = list[k];
			if (list.Count > 1)
			{
				if (flag4)
				{
					if (k > 0)
					{
						writer.WriteEndElement();
						writer.WriteStartElement("text");
						writer.WriteAttributeString("x", ToUnits(num21));
						writer.WriteAttributeString("y", ToUnits(num22 + (double)k * val7.LineHeight));
						writer.WriteAttributeString("style", stringBuilder3.ToString());
						if (val.RightToLeft && StiBidirectionalConvert.StringContainArabicOrHebrew(list[k].Text))
						{
							writer.WriteAttributeString("direction", "rtl");
							writer.WriteAttributeString("unicode-bidi", "embed");
						}
					}
				}
				else
				{
					writer.WriteStartElement("tspan");
					if (k > 0)
					{
						writer.WriteAttributeString("x", ToUnits(num21));
						writer.WriteAttributeString("dy", ToUnits(val7.LineHeight));
					}
				}
			}
			if (val7.NeedWidthAlign)
			{
				float num23;
				for (num23 = num; num23 < 0f; num23 += 360f)
				{
				}
				double num24 = num19;
				if ((num23 > 45f && num23 < 135f) || (num23 > 225f && num23 < 315f))
				{
					num24 = num20;
				}
				double num25 = num24 - 4.0 - num16 * 2.0;
				writer.WriteAttributeString("textLength", ToUnits((num25 > 0.0) ? num25 : 1.0));
			}
			string text3 = list[k].Text;
			if (text3 != null && text3.Trim().Length == 0)
			{
				text3 = "\u00a0";
			}
			writer.WriteString(text3);
			if (!flag4 && list.Count > 1)
			{
				writer.WriteEndElement();
			}
		}
		writer.WriteFullEndElement();
		writer.Indentation = ((xmlIndentation >= 0) ? xmlIndentation : 0);
		writer.Formatting = ((xmlIndentation >= 0) ? Formatting.Indented : Formatting.None);
		if (num != 0f)
		{
			writer.WriteFullEndElement();
			writer.WriteFullEndElement();
			if (useClip)
			{
				writer.WriteFullEndElement();
			}
		}
	}

	public static string GetStyleString(Font font, Color textColor)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append($"font-size:{font.SizeInPoints.ToString().Replace(',', '.')}pt;");
		stringBuilder.Append($"font-family:'{font.Name}';");
		if (font.Bold)
		{
			stringBuilder.Append("font-weight:bold;");
		}
		if (font.Italic)
		{
			stringBuilder.Append("font-style:italic;");
		}
		if (font.Underline || font.Strikeout)
		{
			string arg = ((!font.Underline) ? (font.Strikeout ? "line-through" : null) : (font.Strikeout ? "underline line-through" : "underline"));
			stringBuilder.Append($"text-decoration:{arg};");
		}
		stringBuilder.Append($"fill:#{textColor.R:X2}{textColor.G:X2}{textColor.B:X2};");
		if (textColor.A != byte.MaxValue)
		{
			stringBuilder.Append(string.Format("fill-opacity:{0};", Math.Round((float)(int)textColor.A / 255f, 3).ToString().Replace(",", ".")));
		}
		return stringBuilder.ToString();
	}

	internal static void WriteImage(XmlTextWriter writer, StiSvgData svgData, float imageResolution, ImageFormat imageFormat, float imageQuality, ImageCodecInfo imageCodec)
	{
		if (svgData.Component is IStiExportImageExtended stiExportImageExtended && svgData.Component.IsExportAsImage(StiExportFormat.ImagePng))
		{
			float zoom = imageResolution;
			Image image = stiExportImageExtended.GetImage(ref zoom, StiExportFormat.ImagePng);
			try
			{
				WriteImage2(writer, svgData, image, imageResolution, imageFormat, imageQuality, imageCodec);
			}
			finally
			{
				((IDisposable)image)?.Dispose();
			}
		}
	}

	internal static void WriteImage2(XmlTextWriter writer, StiSvgData svgData, Image image, float imageResolution, ImageFormat imageFormat, float imageQuality, ImageCodecInfo imageCodec)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		if (image != null)
		{
			MemoryStream memoryStream = new MemoryStream();
			if (imageFormat == ImageFormat.Png || imageFormat == ImageFormat.Gif || imageFormat == ImageFormat.Bmp)
			{
				image.Save((Stream)memoryStream, imageFormat);
			}
			else if (imageCodec == null)
			{
				image.Save((Stream)memoryStream, ImageFormat.Jpeg);
			}
			else
			{
				EncoderParameters val = new EncoderParameters(1);
				val.Param[0] = new EncoderParameter(Encoder.Quality, (long)(imageQuality * 100f));
				image.Save((Stream)memoryStream, imageCodec, val);
			}
			byte[] array = memoryStream.ToArray();
			writer.WriteStartElement("image");
			string value = svgData.Component.BookmarkValue as string;
			if (!string.IsNullOrEmpty(value))
			{
				writer.WriteAttributeString("id", value);
			}
			if (svgData.Component is StiMap)
			{
				writer.WriteAttributeString("x", ToUnits(svgData.X));
				writer.WriteAttributeString("y", ToUnits(svgData.Y));
				writer.WriteAttributeString("width", ToUnits(image.Width));
				writer.WriteAttributeString("height", ToUnits(image.Height));
			}
			else
			{
				WriteCoordinates(writer, svgData);
			}
			writer.WriteStartAttribute("href");
			string arg = "jpg";
			if (imageFormat == ImageFormat.Png)
			{
				arg = "png";
			}
			if (imageFormat == ImageFormat.Gif)
			{
				arg = "gif";
			}
			if (imageFormat == ImageFormat.Bmp)
			{
				arg = "bmp";
			}
			writer.WriteString($"data:image/{arg};base64,");
			writer.WriteRaw("\r\n");
			writer.WriteBase64(array, 0, array.Length);
			writer.WriteEndAttribute();
			writer.WriteEndElement();
		}
	}

	private static void WriteBarCode(XmlTextWriter writer, StiSvgData svgData)
	{
		StiBarCode stiBarCode = svgData.Component as StiBarCode;
		StiSvgGeomWriter geomWriter = new StiSvgGeomWriter(writer);
		StiBarCodeExportPainter context = new StiBarCodeExportPainter(geomWriter);
		stiBarCode.BarCodeType.Draw(context, stiBarCode, new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height), 1f);
	}

	private static void WriteShape(XmlTextWriter writer, StiSvgData svgData, int xmlIndentation, bool useClip, Hashtable guids)
	{
		//IL_4640: Unknown result type (might be due to invalid IL or missing references)
		//IL_465f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4669: Expected O, but got Unknown
		//IL_466b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4677: Unknown result type (might be due to invalid IL or missing references)
		//IL_4609: Unknown result type (might be due to invalid IL or missing references)
		//IL_453c: Unknown result type (might be due to invalid IL or missing references)
		StiShape stiShape = svgData.Component as StiShape;
		IStiBrush stiBrush = svgData.Component as IStiBrush;
		Color color = Color.Transparent;
		if (stiBrush != null)
		{
			color = StiBrush.ToColor(stiBrush.Brush);
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (stiShape.ShapeType is StiVerticalLineShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2.Append($"M {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} V {ToUnits(svgData.Bottom)}");
		}
		if (stiShape.ShapeType is StiHorizontalLineShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} H {ToUnits(svgData.Right)}");
		}
		if (stiShape.ShapeType is StiTopAndBottomLineShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2.Append(string.Format("M {0},{1} H {2} M {0} {3} H {2}", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
		}
		if (stiShape.ShapeType is StiLeftAndRightLineShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2.Append(string.Format("M {0},{1} V {3} M {2},{1} V {3}", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
		}
		if (stiShape.ShapeType is StiRectangleShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiDiagonalDownLineShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right)} {ToUnits(svgData.Bottom)}");
		}
		if (stiShape.ShapeType is StiDiagonalUpLineShapeType)
		{
			stringBuilder.Append(string.Format("M {0},{1} V {3} H {2} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
			stringBuilder2.Append(string.Format("M {0},{3} L {2} {1}", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom)));
		}
		if (stiShape.ShapeType is StiTriangleShapeType)
		{
			StiShapeDirection direction = (stiShape.ShapeType as StiTriangleShapeType).Direction;
			if (direction == StiShapeDirection.Up)
			{
				stringBuilder.Append(string.Format("M {0},{3} L {4},{1} {2},{3} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom), ToUnits(svgData.X + svgData.Width / 2.0)));
			}
			if (direction == StiShapeDirection.Down)
			{
				stringBuilder.Append(string.Format("M {0},{1} L {4},{3} {2},{1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom), ToUnits(svgData.X + svgData.Width / 2.0)));
			}
			if (direction == StiShapeDirection.Left)
			{
				stringBuilder.Append(string.Format("M {2},{3} L {0},{4} {2},{1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom), ToUnits(svgData.Y + svgData.Height / 2.0)));
			}
			if (direction == StiShapeDirection.Right)
			{
				stringBuilder.Append(string.Format("M {0},{3} L {2},{4} {0},{1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Right), ToUnits(svgData.Bottom), ToUnits(svgData.Y + svgData.Height / 2.0)));
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiOvalShapeType)
		{
			double num = svgData.Width / 2.0 * 0.4399999976158142;
			double num2 = svgData.Height / 2.0 * 0.4399999976158142;
			double x = svgData.X;
			double y = svgData.Y;
			double right = svgData.Right;
			double bottom = svgData.Bottom;
			double number = svgData.X + svgData.Width / 2.0;
			double number2 = svgData.Y + svgData.Height / 2.0;
			stringBuilder.Append($"M {ToUnits(number)},{ToUnits(bottom)} ");
			stringBuilder.Append($"C {ToUnits(x + num)},{ToUnits(bottom)} {ToUnits(x)},{ToUnits(bottom - num2)} {ToUnits(x)},{ToUnits(number2)} ");
			stringBuilder.Append($"C {ToUnits(x)},{ToUnits(y + num2)} {ToUnits(x + num)},{ToUnits(y)} {ToUnits(number)},{ToUnits(y)} ");
			stringBuilder.Append($"C {ToUnits(right - num)},{ToUnits(y)} {ToUnits(right)},{ToUnits(y + num2)} {ToUnits(right)},{ToUnits(number2)} ");
			stringBuilder.Append($"C {ToUnits(right)},{ToUnits(bottom - num2)} {ToUnits(right - num)},{ToUnits(bottom)} {ToUnits(number)},{ToUnits(bottom)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiRoundedRectangleShapeType)
		{
			float round = (stiShape.ShapeType as StiRoundedRectangleShapeType).Round;
			double num3 = svgData.Width;
			if (num3 > svgData.Height)
			{
				num3 = svgData.Height;
			}
			double num4 = Math.Min(num3, 100.0 * stiShape.Page.Zoom) * (double)round;
			double num5 = num4 * 0.4399999976158142;
			double x2 = svgData.X;
			double y2 = svgData.Y;
			double right2 = svgData.Right;
			double bottom2 = svgData.Bottom;
			stringBuilder.Append($"M {ToUnits(x2 + num4)},{ToUnits(bottom2)} ");
			stringBuilder.Append($"C {ToUnits(x2 + num5)},{ToUnits(bottom2)} {ToUnits(x2)},{ToUnits(bottom2 - num5)} {ToUnits(x2)},{ToUnits(bottom2 - num4)} ");
			stringBuilder.Append($"V {ToUnits(y2 + num4)} ");
			stringBuilder.Append($"C {ToUnits(x2)},{ToUnits(y2 + num5)} {ToUnits(x2 + num5)},{ToUnits(y2)} {ToUnits(x2 + num4)},{ToUnits(y2)} ");
			stringBuilder.Append($"H {ToUnits(right2 - num4)} ");
			stringBuilder.Append($"C {ToUnits(right2 - num5)},{ToUnits(y2)} {ToUnits(right2)},{ToUnits(y2 + num5)} {ToUnits(right2)},{ToUnits(y2 + num4)} ");
			stringBuilder.Append($"V {ToUnits(bottom2 - num4)} ");
			stringBuilder.Append($"C {ToUnits(right2)},{ToUnits(bottom2 - num5)} {ToUnits(right2 - num5)},{ToUnits(bottom2)} {ToUnits(right2 - num4)},{ToUnits(bottom2)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiOctagonShapeType)
		{
			StiOctagonShapeType stiOctagonShapeType = stiShape.ShapeType as StiOctagonShapeType;
			double num6 = ((stiShape.Report != null) ? stiShape.Report.Unit.ConvertToHInches((double)stiOctagonShapeType.Bevel) : ((double)stiOctagonShapeType.Bevel)) * stiShape.Page.Zoom;
			double num7 = num6;
			if (stiOctagonShapeType.AutoSize)
			{
				num6 = svgData.Width / 3.413396120071411;
				num7 = svgData.Height / 3.413396120071411;
			}
			if (num6 > svgData.Width / 2.0)
			{
				num6 = svgData.Width / 2.0;
			}
			if (num7 > svgData.Height / 2.0)
			{
				num7 = svgData.Height / 2.0;
			}
			stringBuilder.Append($"M {ToUnits(svgData.X + num6)},{ToUnits(svgData.Y)} ");
			stringBuilder.Append($"L {ToUnits(svgData.Right - num6)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num7)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num7)} {ToUnits(svgData.Right - num6)},{ToUnits(svgData.Bottom)} ");
			stringBuilder.Append($"{ToUnits(svgData.X + num6)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num7)} ");
			stringBuilder.Append($"{ToUnits(svgData.X + num6)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num7)} ");
			stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + num7)} {ToUnits(svgData.X + num6)},{ToUnits(svgData.Y)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiArrowShapeType)
		{
			StiShapeDirection direction2 = (stiShape.ShapeType as StiArrowShapeType).Direction;
			float arrowWidth = (stiShape.ShapeType as StiArrowShapeType).ArrowWidth;
			float arrowHeight = (stiShape.ShapeType as StiArrowShapeType).ArrowHeight;
			double num8 = svgData.Width * (double)arrowWidth;
			double num9 = svgData.Height * (double)arrowHeight;
			if (direction2 == StiShapeDirection.Left || direction2 == StiShapeDirection.Right)
			{
				num8 = svgData.Height * (double)arrowWidth;
				num9 = svgData.Width * (double)arrowHeight;
			}
			if (direction2 == StiShapeDirection.Up)
			{
				stringBuilder.Append($"M {ToUnits(svgData.Right - num8)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"L {ToUnits(svgData.Right - num8)},{ToUnits(svgData.Y + num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num8)},{ToUnits(svgData.Y + num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num8)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num8)},{ToUnits(svgData.Bottom)} Z");
			}
			if (direction2 == StiShapeDirection.Down)
			{
				stringBuilder.Append($"M {ToUnits(svgData.X + num8)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + num8)},{ToUnits(svgData.Bottom - num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num8)},{ToUnits(svgData.Bottom - num9)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num8)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num8)},{ToUnits(svgData.Y)} Z");
			}
			if (direction2 == StiShapeDirection.Left)
			{
				stringBuilder.Append($"M {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num8)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + num9)},{ToUnits(svgData.Y + num8)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num9)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num9)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num9)},{ToUnits(svgData.Bottom - num8)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num8)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num8)} Z");
			}
			if (direction2 == StiShapeDirection.Right)
			{
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num8)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + svgData.Width - num9)},{ToUnits(svgData.Bottom - num8)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num9)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num9)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num9)},{ToUnits(svgData.Y + num8)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + num8)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num8)} Z");
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiComplexArrowShapeType)
		{
			double num10 = ((svgData.Width < svgData.Height) ? (svgData.Width / 2.0) : (svgData.Height / 2.0));
			double num11 = svgData.Height / 3.799999952316284;
			double num12 = svgData.Width / 3.799999952316284;
			double num13 = ((svgData.Height < svgData.Width) ? (svgData.Height / 2.0) : (svgData.Width / 2.0));
			switch ((stiShape.ShapeType as StiComplexArrowShapeType).Direction)
			{
			case StiShapeDirection.Left:
			case StiShapeDirection.Right:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + num10)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num10)},{ToUnits(svgData.Y + num11)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num10)},{ToUnits(svgData.Y + num11)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num10)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num10)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num10)},{ToUnits(svgData.Bottom - num11)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num10)},{ToUnits(svgData.Bottom - num11)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num10)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} Z");
				break;
			case StiShapeDirection.Up:
			case StiShapeDirection.Down:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num13)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num12)},{ToUnits(svgData.Y + num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num12)},{ToUnits(svgData.Bottom - num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num12)},{ToUnits(svgData.Bottom - num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num12)},{ToUnits(svgData.Y + num13)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + num13)} Z");
				break;
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiBentArrowShapeType)
		{
			double num14 = 0.0;
			double num15 = 0.0;
			double num16 = 0.0;
			if (svgData.Height > svgData.Width)
			{
				num15 = svgData.Width / 4.0;
				num14 = num15;
				num16 = num15 / 2.0;
			}
			else
			{
				num14 = (int)(svgData.Height / 4.0);
				num15 = num14;
				num16 = num15 / 2.0;
			}
			switch ((stiShape.ShapeType as StiBentArrowShapeType).Direction)
			{
			case StiShapeDirection.Up:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num14)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - (num16 + num14))},{ToUnits(svgData.Bottom - num14)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - (num16 + num14))},{ToUnits(svgData.Y + num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num15 * 2.0)},{ToUnits(svgData.Y + num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num15)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num16)},{ToUnits(svgData.Y + num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num16)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} Z");
				break;
			case StiShapeDirection.Left:
				stringBuilder.Append($"M {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"L {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15)},{ToUnits(svgData.Y + num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15)},{ToUnits(svgData.Y + num15 * 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15)},{ToUnits(svgData.Y + num15 + num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num14)},{ToUnits(svgData.Y + num15 + num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num14)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} Z");
				break;
			case StiShapeDirection.Down:
				stringBuilder.Append($"M {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + num16)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num16)},{ToUnits(svgData.Bottom - num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15 * 2.0)},{ToUnits(svgData.Bottom - num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15 + num16)},{ToUnits(svgData.Bottom - num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num15 + num16)},{ToUnits(svgData.Y + num14)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num14)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y)} Z");
				break;
			case StiShapeDirection.Right:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num15)},{ToUnits(svgData.Bottom - num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num15)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num15)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num15)},{ToUnits(svgData.Bottom - num15 * 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num15)},{ToUnits(svgData.Bottom - num15 - num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num14)},{ToUnits(svgData.Bottom - num15 - num16)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num14)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiChevronShapeType)
		{
			double num17 = ((svgData.Width > svgData.Height) ? (svgData.Height / 2.0) : (svgData.Width / 2.0));
			switch ((stiShape.ShapeType as StiChevronShapeType).Direction)
			{
			case StiShapeDirection.Right:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + num17)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num17)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num17)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			case StiShapeDirection.Left:
				stringBuilder.Append($"M {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + num17)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num17)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right - num17)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y)} Z");
				break;
			case StiShapeDirection.Up:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num17)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num17)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom - num17)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y + num17)} Z");
				break;
			case StiShapeDirection.Down:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y + num17)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num17)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num17)} ");
				stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiDivisionShapeType)
		{
			double num18 = svgData.Height / 3.0;
			num18 += 4.0;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num18)} ");
			stringBuilder.Append($"L {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num18)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num18)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num18)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num18)} ");
			num18 -= 4.0;
			stringBuilder.Append($"M {ToUnits(svgData.Width / 2.0 - num18 / 2.0 + svgData.X)},{ToUnits(svgData.Y + 1.0 + num18 / 2.0)} ");
			stringBuilder.Append(string.Format("a {0},{0} 0 1,0 {1},0 a {0},{0} 0 1,0 -{1},0 ", num18 / 2.0, num18));
			stringBuilder.Append($"M {ToUnits(svgData.Width / 2.0 - num18 / 2.0 + svgData.X)},{ToUnits(svgData.Bottom - 2.0 - num18 / 2.0)} ");
			stringBuilder.Append(string.Format("a {0},{0} 0 1,0 {1},0 a {0},{0} 0 1,0 -{1},0 Z", num18 / 2.0, num18));
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiEqualShapeType)
		{
			double num19 = (svgData.Height - svgData.Height / 6.0) / 2.0;
			stringBuilder.Append(string.Format("M {0},{1} h {2} v {3} H {0} V {1} ", ToUnits(svgData.X), ToUnits(svgData.Bottom - num19), ToUnits(svgData.Width), ToUnits(num19)));
			stringBuilder.Append(string.Format("M {0},{1} h {2} v {3} H {0} V {1} Z", ToUnits(svgData.X), ToUnits(svgData.Y), ToUnits(svgData.Width), ToUnits(num19)));
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartCardShapeType)
		{
			stringBuilder.Append($"M {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} ");
			stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 5.0)} {ToUnits(svgData.X + svgData.Width / 5.0)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartCollateShapeType)
		{
			switch ((stiShape.ShapeType as StiFlowchartCollateShapeType).Direction)
			{
			case StiShapeDirection.Up:
			case StiShapeDirection.Down:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} {ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			case StiShapeDirection.Left:
			case StiShapeDirection.Right:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"L {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartDecisionShapeType)
		{
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} ");
			stringBuilder.Append($"L {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartManualInputShapeType)
		{
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 5.0)} ");
			stringBuilder.Append($"L {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 5.0)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartOffPageConnectorShapeType)
		{
			double num20 = svgData.Height / 5.0;
			double num21 = svgData.Width / 5.0;
			switch ((stiShape.ShapeType as StiFlowchartOffPageConnectorShapeType).Direction)
			{
			case StiShapeDirection.Down:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num20)} {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num20)} {ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			case StiShapeDirection.Up:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} L {ToUnits(svgData.X)},{ToUnits(svgData.Y + num20)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num20)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} Z");
				break;
			case StiShapeDirection.Left:
				stringBuilder.Append($"M {ToUnits(svgData.X + num21)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + num21)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.X + num21)},{ToUnits(svgData.Y)} Z");
				break;
			case StiShapeDirection.Right:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right - num21)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.Right - num21)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
				break;
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartPreparationShapeType)
		{
			double num22 = svgData.Width / 5.0;
			double num23 = svgData.Height / 5.0;
			double num24 = svgData.Width / 2.0;
			double num25 = svgData.Height / 2.0;
			switch ((stiShape.ShapeType as StiFlowchartPreparationShapeType).Direction)
			{
			case StiShapeDirection.Left:
			case StiShapeDirection.Right:
				stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num25)} L {ToUnits(svgData.X + num22)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right - num22)},{ToUnits(svgData.Y)} ");
				stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num25)} {ToUnits(svgData.Right - num22)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + num22)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num25)} Z");
				break;
			case StiShapeDirection.Up:
			case StiShapeDirection.Down:
				stringBuilder.Append($"M {ToUnits(svgData.X + num24)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num23)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num23)} ");
				stringBuilder.Append($"{ToUnits(svgData.X + num24)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num23)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num23)} {ToUnits(svgData.X + num24)},{ToUnits(svgData.Y)} Z");
				break;
			}
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFlowchartSortShapeType)
		{
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} L {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + svgData.Height / 2.0)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiFrameShapeType)
		{
			double num26 = svgData.Width / 7.0;
			double num27 = svgData.Height / 7.0;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y)} ");
			stringBuilder.Append($"M {ToUnits(svgData.X + num26)},{ToUnits(svgData.Y + num27)} L {ToUnits(svgData.X + num26)},{ToUnits(svgData.Bottom - num27)} {ToUnits(svgData.Right - num26)},{ToUnits(svgData.Bottom - num27)} {ToUnits(svgData.Right - num26)},{ToUnits(svgData.Y + num27)} {ToUnits(svgData.X + num26)},{ToUnits(svgData.Y + num27)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiMinusShapeType)
		{
			double num28 = svgData.Height / 3.0;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num28)} ");
			stringBuilder.Append($"L {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num28)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num28)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num28)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num28)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiMultiplyShapeType)
		{
			double num29 = svgData.Width / 4.0;
			double num30 = svgData.Height / 4.0;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num30)} ");
			stringBuilder.Append($"L {ToUnits(svgData.X + num29)},{ToUnits(svgData.Y)} {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y + num30)} {ToUnits(svgData.Right - num29)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num30)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right - num29)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num30)} {ToUnits(svgData.Right - num29)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Bottom - num30)} ");
			stringBuilder.Append($"{ToUnits(svgData.X + num29)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num30)} {ToUnits(svgData.X + num29)},{ToUnits(svgData.Y + svgData.Height / 2.0)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num30)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiParallelogramShapeType)
		{
			double num31 = svgData.Width / 7.0;
			double num32 = svgData.Height / 7.0;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} L {ToUnits(svgData.X + svgData.Width / 5.0)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right - svgData.Width / 5.0)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiPlusShapeType)
		{
			double num33 = svgData.Width / 3.0;
			double num34 = svgData.Height / 3.0;
			stringBuilder.Append($"M {ToUnits(svgData.X + num33)},{ToUnits(svgData.Y)} ");
			stringBuilder.Append($"L {ToUnits(svgData.Right - num33)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right - num33)},{ToUnits(svgData.Y + num34)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num34)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom - num34)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right - num33)},{ToUnits(svgData.Bottom - num34)} {ToUnits(svgData.Right - num33)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + num33)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + num33)},{ToUnits(svgData.Bottom - num34)} ");
			stringBuilder.Append($"{ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num34)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num34)} {ToUnits(svgData.X + num33)},{ToUnits(svgData.Y + num34)} {ToUnits(svgData.X + num33)},{ToUnits(svgData.Y)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiRegularPentagonShapeType)
		{
			double num35 = svgData.Height / 2.5999999046325684;
			double num36 = svgData.Width / 5.5;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num35)} L {ToUnits(svgData.X + svgData.Width / 2.0)},{ToUnits(svgData.Y)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num35)} {ToUnits(svgData.Right - num36)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + num36)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num35)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiTrapezoidShapeType)
		{
			double num37 = svgData.Width / 4.75;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} L {ToUnits(svgData.X + num37)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right - num37)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiSnipSameSideCornerRectangleShapeType)
		{
			double num38 = svgData.Width / 7.199999809265137;
			double num39 = svgData.Height / 4.599999904632568;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y + num39)} L {ToUnits(svgData.X + num38)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right - num38)},{ToUnits(svgData.Y)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Y + num39)} {ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Y + num39)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stiShape.ShapeType is StiSnipDiagonalSideCornerRectangleShapeType)
		{
			double num40 = svgData.Width / 7.199999809265137;
			double num41 = svgData.Height / 4.599999904632568;
			stringBuilder.Append($"M {ToUnits(svgData.X)},{ToUnits(svgData.Y)} L {ToUnits(svgData.Right - num40)},{ToUnits(svgData.Y)} {ToUnits(svgData.Right)},{ToUnits(svgData.Y + num41)} ");
			stringBuilder.Append($"{ToUnits(svgData.Right)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X + num40)},{ToUnits(svgData.Bottom)} {ToUnits(svgData.X)},{ToUnits(svgData.Bottom - num41)} {ToUnits(svgData.X)},{ToUnits(svgData.Y)} Z");
			stringBuilder2 = stringBuilder;
		}
		if (stringBuilder.Length > 0 || stringBuilder2.Length > 0)
		{
			if (stringBuilder == stringBuilder2)
			{
				writer.WriteStartElement("path");
				if (color != Color.Transparent)
				{
					WriteFillInfo(writer, color);
				}
				else
				{
					writer.WriteAttributeString("fill", "none");
				}
				if (stiShape.BorderColor != Color.Transparent)
				{
					WriteStrokeInfo(writer, stiShape.BorderColor, stiShape.Size, stiShape.Style);
				}
				else
				{
					writer.WriteAttributeString("stroke", "none");
				}
				writer.WriteAttributeString("d", stringBuilder.ToString());
				writer.WriteEndElement();
			}
			else
			{
				if (stringBuilder.Length > 0 && color != Color.Transparent)
				{
					writer.WriteStartElement("path");
					WriteFillInfo(writer, color);
					writer.WriteAttributeString("stroke", "none");
					writer.WriteAttributeString("d", stringBuilder.ToString());
					writer.WriteEndElement();
				}
				if (stringBuilder2.Length > 0 && stiShape.BorderColor != Color.Transparent)
				{
					writer.WriteStartElement("path");
					writer.WriteAttributeString("fill", "none");
					WriteStrokeInfo(writer, stiShape.BorderColor, stiShape.Size, stiShape.Style);
					writer.WriteAttributeString("d", stringBuilder2.ToString());
					writer.WriteEndElement();
				}
			}
		}
		string parsedText = stiShape.GetParsedText();
		if (!string.IsNullOrWhiteSpace(parsedText))
		{
			StiText component = new StiText(stiShape.ClientRectangle, parsedText)
			{
				Font = stiShape.Font,
				TextBrush = (StiBrush)new StiSolidBrush(stiShape.ForeColor),
				HorAlignment = stiShape.HorAlignment,
				VertAlignment = stiShape.VertAlignment,
				Margins = stiShape.Margins,
				WordWrap = true,
				TextQuality = StiTextQuality.Typographic,
				Page = stiShape.Page
			};
			StiSvgData stiSvgData = default(StiSvgData);
			stiSvgData.X = svgData.X;
			stiSvgData.Y = svgData.Y;
			stiSvgData.Width = svgData.Width;
			stiSvgData.Height = svgData.Height;
			stiSvgData.Component = component;
			StiSvgData svgData2 = stiSvgData;
			WriteText(writer, svgData2, xmlIndentation, useClip: false, guids, stiShape.BackgroundColor);
		}
	}

	private static void WriteRoundedRectanglePrimitive(XmlTextWriter writer, StiSvgData svgData)
	{
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		StiRoundedRectanglePrimitive stiRoundedRectanglePrimitive = svgData.Component as StiRoundedRectanglePrimitive;
		if (stiRoundedRectanglePrimitive.Color != Color.Transparent)
		{
			StringBuilder stringBuilder = new StringBuilder();
			float round = stiRoundedRectanglePrimitive.Round;
			double num = svgData.Width;
			if (num > svgData.Height)
			{
				num = svgData.Height;
			}
			double num2 = Math.Min(num, 100.0 * stiRoundedRectanglePrimitive.Page.Zoom) * (double)round;
			double num3 = num2 * 0.4399999976158142;
			double x = svgData.X;
			double y = svgData.Y;
			double num4 = svgData.X + svgData.Width;
			double num5 = svgData.Y + svgData.Height;
			stringBuilder.Append($"M {ToUnits(x + num2)} {ToUnits(num5)} ");
			stringBuilder.Append($"C {ToUnits(x + num3)} {ToUnits(num5)} {ToUnits(x)} {ToUnits(num5 - num3)} {ToUnits(x)} {ToUnits(num5 - num2)} ");
			stringBuilder.Append($"V {ToUnits(y + num2)} ");
			stringBuilder.Append($"C {ToUnits(x)} {ToUnits(y + num3)} {ToUnits(x + num3)} {ToUnits(y)} {ToUnits(x + num2)} {ToUnits(y)} ");
			stringBuilder.Append($"H {ToUnits(num4 - num2)} ");
			stringBuilder.Append($"C {ToUnits(num4 - num3)} {ToUnits(y)} {ToUnits(num4)} {ToUnits(y + num3)} {ToUnits(num4)} {ToUnits(y + num2)} ");
			stringBuilder.Append($"V {ToUnits(num5 - num2)} ");
			stringBuilder.Append($"C {ToUnits(num4)} {ToUnits(num5 - num3)} {ToUnits(num4 - num3)} {ToUnits(num5)} {ToUnits(num4 - num2)} {ToUnits(num5)} Z");
			writer.WriteStartElement("path");
			writer.WriteAttributeString("fill", "none");
			WriteStrokeInfo(writer, stiRoundedRectanglePrimitive.Color, stiRoundedRectanglePrimitive.Size, stiRoundedRectanglePrimitive.Style);
			writer.WriteAttributeString("d", stringBuilder.ToString());
			writer.WriteEndElement();
		}
	}

	private static string GetClipPathName(StiComponent component, Hashtable guids)
	{
		if (guids != null)
		{
			string text = (string)guids[component];
			if (text == null)
			{
				text = (string)(guids[component] = Guid.NewGuid().ToString("N"));
			}
			return $"clipPath_{text}";
		}
		if (component.Guid == null)
		{
			component.NewGuid();
		}
		return $"clipPath_{component.Guid}";
	}

	public static string SaveComponentToString(StiComponent component)
	{
		return SaveComponentToString(component, 1.0);
	}

	public static string SaveComponentToString(StiComponent component, double zoom)
	{
		return SaveComponentToString(component, ImageFormat.Png, 0.75f, (float)zoom * 100f);
	}

	internal static string SaveComponentToString(StiComponent component, ImageFormat imageFormat, float imageQuality, float imageResolution)
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		int num = -1;
		xmlTextWriter.Indentation = ((num >= 0) ? num : 0);
		xmlTextWriter.Formatting = ((num >= 0) ? Formatting.Indented : Formatting.None);
		if (component.Report == null)
		{
			return string.Empty;
		}
		StiUnit unit = component.Report.Unit;
		if (component.Page != null)
		{
			unit = component.Page.Unit;
		}
		double num2 = 0.96;
		double num3 = 0.956;
		if (component.IsDesigning)
		{
			num2 = 1.0;
			num3 = 1.0;
		}
		double num4 = num2 * unit.ConvertToHInches(component.Left);
		double num5 = num3 * unit.ConvertToHInches(component.Top);
		double num6 = num2 * unit.ConvertToHInches(component.Right);
		double num7 = num3 * unit.ConvertToHInches(component.Bottom);
		StiSvgData svgData = default(StiSvgData);
		svgData.X = 0.0;
		svgData.Y = 0.0;
		svgData.Width = Math.Round(num6 - num4, 0);
		svgData.Height = Math.Round(num7 - num5, 0);
		svgData.Component = component;
		bool flag = component.IsExportAsImage(StiExportFormat.ImageSvg);
		bool flag2 = CheckShape(component);
		int gradientCounter = 1;
		Hashtable guids = new Hashtable();
		if (component is StiText stiText && !flag && stiText.Text.ToString() != null && stiText.Text.ToString().Trim().Length > 0)
		{
			xmlTextWriter.WriteStartElement("defs");
			xmlTextWriter.WriteStartElement("clipPath");
			xmlTextWriter.WriteStartElement("rect");
			WriteCoordinates(xmlTextWriter, svgData);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		if (!flag2 && !flag)
		{
			WriteBorder1(xmlTextWriter, svgData, ref gradientCounter);
		}
		if (component is StiText && !flag)
		{
			WriteText(xmlTextWriter, svgData, num, useClip: true, guids);
		}
		else if (component is StiBarCode)
		{
			WriteBarCode(xmlTextWriter, svgData);
		}
		else if (component is StiSparkline)
		{
			StiSparklineSvgHelper.WriteSparkline(xmlTextWriter, svgData);
		}
		else if (component is StiCheckBox)
		{
			WriteCheckBox(xmlTextWriter, svgData);
		}
		else if (component is StiChart)
		{
			StiChartSvgHelper.WriteChart(xmlTextWriter, svgData, needAnimation: false);
		}
		else if (component is StiGauge)
		{
			StiGaugeSvgHelper.WriteGauge(xmlTextWriter, svgData, needAnimation: false);
		}
		else if (component is StiMap && ((StiMap)component).MapMode == StiMapMode.Choropleth)
		{
			StiMapSvgHelper.DrawMap(xmlTextWriter, component as StiMap, 0.0, 0.0, svgData.Width, svgData.Height, animated: false);
		}
		else if (component is IStiProgressElement)
		{
			StiProgressElementSvgHelper.WriteProgress(xmlTextWriter, svgData, 1f);
		}
		else if (component is IStiIndicatorElement)
		{
			StiIndicatorElementSvgHelper.WriteIndicator(xmlTextWriter, svgData, 1f);
		}
		else if (component is IStiCardsElement)
		{
			StiCardsElementSvgHelper.WriteCards(xmlTextWriter, svgData, 1f);
		}
		else if (component is StiMathFormula stiMathFormula && string.IsNullOrEmpty(stiMathFormula.LaTexExpression))
		{
			StiMathFormulaSvgHelper.WriteEmptyDataMessage(xmlTextWriter, svgData);
		}
		else if (flag2)
		{
			WriteShape(xmlTextWriter, svgData, num, useClip: true, guids);
		}
		else if (flag)
		{
			ImageCodecInfo imageCodec = StiImageCodecInfo.GetImageCodec("image/jpeg");
			WriteImage(xmlTextWriter, svgData, imageResolution / 100f, imageFormat, imageQuality, imageCodec);
		}
		xmlTextWriter.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		StreamReader streamReader = new StreamReader(memoryStream);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		memoryStream.Close();
		StiExportUtils.EnableFontSmoothing(component.Report);
		return result;
	}

	public static void SaveToStream(StiReport report, StiPage page, Stream stream, bool compressed)
	{
		int clipCounter = 0;
		SaveToStream(report, page, stream, compressed, standalone: true, ref clipCounter, ImageFormat.Png, 0.75f, 100f);
	}

	internal static void SaveToStream(StiReport report, StiPage page, Stream stream, bool compressed, bool standalone, ref int clipCounter, ImageFormat imageFormat, float imageQuality, float imageResolution)
	{
		if (compressed)
		{
			GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
			WriteDocument(report, page, standalone, imageFormat, imageQuality, imageResolution).WriteTo(gZipStream);
			gZipStream.Close();
		}
		else
		{
			WriteDocument(report, page, standalone, imageFormat, imageQuality, imageResolution).WriteTo(stream);
		}
	}

	public static void WriteCheckBox(XmlTextWriter writer, StiSvgData svgData)
	{
		if (svgData.Component is StiCheckBox stiCheckBox)
		{
			string value = StiContextSvgHelper.WriteFillBrush(writer, stiCheckBox.TextBrush, new RectangleF((float)svgData.X, (float)svgData.Y, (float)svgData.Width, (float)svgData.Height));
			writer.WriteStartElement("g");
			int num = 200;
			double num2 = 1.0 / ((double)num / Math.Min(svgData.Width, svgData.Height));
			double num3 = 0.0;
			double num4 = 0.0;
			if (svgData.Width > svgData.Height)
			{
				num3 = Math.Abs(svgData.Width / 2.0 - (double)num * num2 / 2.0);
			}
			else if (svgData.Width < svgData.Height)
			{
				num4 = Math.Abs(svgData.Height / 2.0 - (double)num * num2 / 2.0);
			}
			writer.WriteAttributeString("transform", string.Format("translate({0},{1})", (svgData.X + num3).ToString().Replace(",", "."), (svgData.Y + num4).ToString().Replace(",", ".")));
			writer.WriteStartElement("path");
			writer.WriteAttributeString("d", GetCheckBoxData(stiCheckBox));
			writer.WriteAttributeString("stroke", $"#{stiCheckBox.ContourColor.R:X2}{stiCheckBox.ContourColor.G:X2}{stiCheckBox.ContourColor.B:X2}");
			writer.WriteAttributeString("stroke-width", stiCheckBox.Size.ToString());
			writer.WriteAttributeString("stroke-linecap", "round");
			writer.WriteAttributeString("stroke-linejoin", "round");
			writer.WriteAttributeString("transform", $"scale({num2})".Replace(",", "."));
			writer.WriteAttributeString("style", value);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}

	private static string GetCheckBoxData(StiCheckBox checkbox)
	{
		bool flag = false;
		object obj = checkbox.CheckedValue ?? checkbox.Checked.Value;
		string text = obj.ToString().Trim().ToLower(CultureInfo.InvariantCulture);
		string[] array = checkbox.Values.Split('/', ';', ',');
		if (array != null && array.Length != 0)
		{
			string text2 = array[0].Trim().ToLower(CultureInfo.InvariantCulture);
			flag = text == text2;
		}
		string result = string.Empty;
		switch (flag ? checkbox.CheckStyleForTrue : checkbox.CheckStyleForFalse)
		{
		case StiCheckStyle.Cross:
			result = "m 62.567796,147.97593 c -0.55,-0.14223 -2.162828,-0.5128 -3.584062,-0.82348 -3.647667,-0.79738 -9.670499,-5.83775 -14.242817,-11.91949 l -3.902341,-5.19058 5.080199,-1.13481 c 7.353071,-1.64253 13.640456,-5.71752 21.826811,-14.14646 l 7.208128,-7.42171 -6.410736,-7.513354 c -11.773129,-13.79803 -14.346726,-23.01954 -8.627769,-30.91434 2.894109,-3.9952 11.818482,-12.369333 13.182086,-12.369333 0.411356,0 1.063049,1.6875 1.448207,3.750003 0.980474,5.25038 6.456187,16.76587 10.936694,23 2.075266,2.8875 3.991125,5.25 4.257464,5.25 0.266339,0 3.775242,-3.4875 7.797566,-7.75 16.397034,-17.37615 29.674184,-19.76481 38.280564,-6.88699 4.15523,6.21753 4.18631,8.07093 0.14012,8.3552 -5.84833,0.41088 -17.16241,8.5342 -25.51465,18.319104 l -4.63153,5.42599 4.87803,4.31529 c 6.55108,5.79533 18.8991,11.89272 25.84076,12.76002 3.0455,0.38051 5.53727,1.10582 5.53727,1.6118 0,2.7809 -9.26611,14.41872 -13.03,16.36511 -7.96116,4.11687 -16.36991,0.71207 -32.764584,-13.26677 l -4.985957,-4.25125 -7.086791,8.97188 c -3.897736,4.93454 -8.82141,10.1198 -10.9415,11.52281 -3.906121,2.58495 -8.86588,4.41339 -10.691162,3.94136 z";
			break;
		case StiCheckStyle.Check:
			result = "M 60.972125,162.49704 C 51.172676,136.72254 43.561975,123.37669 35.370344,117.6027 l -4.45827,-3.14248 2.75159,-2.89559 c 3.875121,-4.07793 10.034743,-7.49924 14.902472,-8.27747 3.859874,-0.61709 4.458306,-0.38024 8.535897,3.37835 2.660692,2.45254 6.265525,7.60856 9.167226,13.11196 2.630218,4.98849 4.910542,9.06999 5.067388,9.06999 0.156846,0 2.31372,-3.0375 4.793052,-6.75 C 96.259164,91.956015 129.68299,58.786374 157.56485,41.281603 l 8.84913,-5.555656 2.2633,2.631238 2.26329,2.631237 -7.76266,6.294183 C 139.859,66.19023 108.01682,105.51363 89.042715,138.83563 c -6.680477,11.73214 -7.172359,12.31296 -15.090788,17.81963 -4.501873,3.13071 -9.044031,6.30443 -10.093684,7.05271 -1.708923,1.21826 -2.010678,1.09165 -2.886118,-1.21093 z";
			break;
		case StiCheckStyle.CrossRectangle:
			result = "m 24.152542,102.04237 0,-72.499996 74.5,0 74.499998,0 0,72.499996 0,72.5 -74.499998,0 -74.5,0 0,-72.5 z m 133.758188,0.25 -0.25819,-57.249996 -58.999998,0 -59,0 -0.259695,55.999996 c -0.142833,30.8 -0.04446,56.5625 0.218615,57.25 0.375181,0.98048 13.207991,1.25 59.517885,1.25 l 59.039573,0 -0.25819,-57.25 z m -90.574091,43.18692 c -1.823747,-0.3912 -4.926397,-1.85716 -6.894778,-3.25768 -3.319254,-2.36169 -12.289319,-12.40741 -12.289319,-13.76302 0,-0.32888 2.417494,-1.13897 5.372209,-1.80021 7.185193,-1.60797 13.747505,-5.93496 21.803114,-14.3763 l 6.675323,-6.99496 -6.379078,-7.31436 C 64.931387,85.71231 61.643682,76.29465 65.471903,68.89169 67.054097,65.83207 78.56175,54.542374 80.098251,54.542374 c 0.45744,0 1.146839,1.6875 1.531997,3.75 0.980474,5.250386 6.456187,16.765876 10.936694,22.999996 2.075266,2.8875 3.991125,5.25 4.257464,5.25 0.266339,0 3.775244,-3.4875 7.797564,-7.75 16.39704,-17.376139 29.67419,-19.764806 38.28057,-6.88698 4.15523,6.21752 4.18631,8.07092 0.14012,8.35519 -5.82996,0.40959 -18.23707,9.34942 -25.91566,18.67328 -3.90068,4.73647 -3.97203,4.95414 -2.2514,6.86861 3.19054,3.54997 13.7039,10.54321 18.97191,12.61967 2.83427,1.11716 7.43737,2.33421 10.22912,2.70455 2.79175,0.37034 5.07591,0.9956 5.07591,1.38947 0,2.11419 -8.37504,13.20895 -11.6517,15.4355 -8.39423,5.70403 -16.63203,2.77 -34.14289,-12.16054 l -4.985955,-4.25125 -7.086791,8.97188 c -9.722344,12.3085 -16.524852,16.55998 -23.948565,14.96754 z";
			break;
		case StiCheckStyle.CheckRectangle:
			result = "m 19.915254,103.5 0,-72.5 71.942245,0 71.942241,0 6.55727,-4.11139 6.55726,-4.11139 1.96722,2.36139 c 1.08197,1.298765 1.98219,2.644166 2.00049,2.98978 0.0183,0.345615 -2.44173,2.53784 -5.46673,4.87161 l -5.5,4.243219 0,69.378391 0,69.37839 -74.999991,0 -75.000005,0 0,-72.5 z m 133.999996,3.87756 c 0,-49.33933 -0.12953,-53.514947 -1.62169,-52.276568 -2.78014,2.307312 -15.68408,17.90053 -24.32871,29.399008 -10.4919,13.955575 -23.47926,33.53736 -29.514025,44.5 -4.457326,8.09707 -5.134776,8.80812 -14.291256,15 -5.28667,3.575 -9.903486,6.62471 -10.259592,6.77712 -0.356107,0.15242 -1.912439,-2.99758 -3.458515,-7 -1.546077,-4.00241 -5.258394,-12.41205 -8.249593,-18.68809 -4.285436,-8.99155 -6.676569,-12.64898 -11.27758,-17.25 C 47.70282,104.62757 44.364254,102 43.495254,102 c -2.798369,0 -1.704872,-1.66044 3.983717,-6.049158 5.593548,-4.31539 13.183139,-7.091307 16.801313,-6.145133 3.559412,0.930807 9.408491,8.154973 13.919775,17.192241 l 4.46286,8.94025 4.54378,-6.83321 C 95.518219,96.605618 108.21371,81.688517 125.80695,63.75 L 143.21531,46 l -53.650021,0 -53.650035,0 0,57.5 0,57.5 59.000005,0 58.999991,0 0,-53.62244 z";
			break;
		case StiCheckStyle.CrossCircle:
			result = "M 83.347458,173.13597 C 61.069754,168.04956 42.193415,152.8724 32.202285,132.01368 23.4014,113.63986 23.679644,89.965903 32.91889,71.042373 41.881579,52.685283 60.867647,37.139882 80.847458,31.799452 c 10.235111,-2.735756 31.264662,-2.427393 40.964762,0.600679 26.18668,8.174684 46.06876,28.926852 51.62012,53.879155 2.43666,10.952327 1.56754,28.058524 -1.98036,38.977594 -6.65679,20.48707 -25.64801,38.95163 -47.32647,46.01402 -6.3909,2.08202 -10.18566,2.59644 -21.27805,2.88446 -9.033911,0.23456 -15.484931,-0.10267 -19.500002,-1.01939 z M 112.4138,158.45825 c 17.13137,-3.13002 33.71724,-15.96081 41.41353,-32.03742 14.8975,-31.119027 -1.10807,-67.659584 -34.40232,-78.540141 -6.71328,-2.193899 -9.93541,-2.643501 -19.07755,-2.661999 -9.354252,-0.01893 -12.16228,0.37753 -18.768532,2.649866 -17.155451,5.900919 -29.669426,17.531424 -36.438658,33.866137 -2.152301,5.193678 -2.694658,8.35455 -3.070923,17.89744 -0.518057,13.139047 0.741843,19.201887 6.111644,29.410237 4.106815,7.80733 15.431893,19.09359 23.36818,23.28808 12.061362,6.37467 27.138828,8.6356 40.864629,6.1278 z M 69.097458,133.41654 c -2.8875,-2.75881 -5.25,-5.35869 -5.25,-5.77751 0,-0.41882 5.658529,-6.30954 12.57451,-13.0905 l 12.57451,-12.329 L 76.198053,89.392633 63.399628,76.565738 68.335951,71.554056 c 2.714978,-2.756426 5.304859,-5.011683 5.75529,-5.011683 0.450432,0 6.574351,5.611554 13.608709,12.470121 l 12.78974,12.470119 4.42889,-4.553471 c 2.43588,-2.50441 8.39186,-8.187924 13.23551,-12.630032 l 8.80663,-8.076559 5.34744,5.281006 5.34743,5.281007 -12.96155,12.557899 -12.96154,12.557897 13.13318,13.16027 13.13319,13.16027 -5.18386,4.66074 c -2.85112,2.5634 -5.70472,4.66073 -6.34134,4.66073 -0.63661,0 -6.5434,-5.4 -13.12621,-12 -6.58281,-6.6 -12.3871,-12 -12.89844,-12 -0.511329,0 -6.593363,5.60029 -13.515627,12.44509 l -12.585935,12.44508 -5.25,-5.016 z";
			break;
		case StiCheckStyle.DotCircle:
			result = "M 81.652542,170.5936 C 59.374838,165.50719 40.498499,150.33003 30.507369,129.47131 21.706484,111.09749 21.984728,87.42353 31.223974,68.5 40.186663,50.14291 59.172731,34.597509 79.152542,29.257079 89.387653,26.521323 110.4172,26.829686 120.1173,29.857758 c 26.18668,8.174684 46.06876,28.926852 51.62012,53.879152 2.43666,10.95233 1.56754,28.05853 -1.98036,38.9776 -6.65679,20.48707 -25.64801,38.95163 -47.32647,46.01402 -6.3909,2.08202 -10.18566,2.59644 -21.27805,2.88446 -9.033907,0.23456 -15.484927,-0.10267 -19.499998,-1.01939 z m 29.999998,-15.098 c 20.68862,-4.34363 38.01874,-20.45437 44.09844,-40.9956 2.36228,-7.9813 2.36228,-22.0187 0,-30 C 150.08927,65.371023 134.63549,50.297336 114.65254,44.412396 106.5531,42.027127 90.741304,42.026386 82.695253,44.4109 62.460276,50.407701 46.686742,66.039241 41.6053,85.13096 c -1.948821,7.32201 -1.86506,23.11641 0.158766,29.93754 8.730326,29.42481 38.97193,46.91812 69.888474,40.4271 z M 90.004747,122.6703 C 76.550209,117.63801 69.825047,101.82445 75.898143,89.5 c 2.136718,-4.33615 7.147144,-9.356192 11.754399,-11.776953 5.578622,-2.931141 16.413098,-2.927504 22.052908,0.0074 18.03,9.382663 19.07573,32.784373 1.91442,42.841563 -5.57282,3.26589 -15.830952,4.2617 -21.615123,2.09829 z";
			break;
		case StiCheckStyle.DotRectangle:
			result = "m 23.847458,101.19491 0,-72.499995 74.5,0 74.499992,0 0,72.499995 0,72.5 -74.499992,0 -74.5,0 0,-72.5 z m 133.999992,-0.008 0,-57.507925 -59.249992,0.25793 -59.25,0.25793 -0.25819,57.249995 -0.258189,57.25 59.508189,0 59.508182,0 0,-57.50793 z m -94.320573,33.85402 c -0.37368,-0.37368 -0.679419,-15.67942 -0.679419,-34.01275 l 0,-33.333335 35.513302,0 35.51329,0 -0.2633,33.749995 -0.2633,33.75 -34.570573,0.26275 c -19.013819,0.14452 -34.876319,-0.043 -35.25,-0.41666 z";
			break;
		case StiCheckStyle.NoneCircle:
			result = "M 83.5,170.5936 C 61.222296,165.50719 42.345957,150.33003 32.354827,129.47131 23.553942,111.09749 23.832186,87.423523 33.071432,68.5 42.034121,50.14291 61.020189,34.597509 81,29.257079 c 10.235111,-2.735756 31.26466,-2.427393 40.96476,0.600679 26.18668,8.174684 46.06876,28.926852 51.62012,53.879155 2.43666,10.95232 1.56754,28.058527 -1.98036,38.977597 -6.65679,20.48707 -25.64801,38.95163 -47.32647,46.01402 -6.3909,2.08202 -10.18566,2.59644 -21.27805,2.88446 -9.033909,0.23456 -15.484929,-0.10267 -19.5,-1.01939 z m 30,-15.098 c 20.68862,-4.34363 38.01874,-20.45437 44.09844,-40.9956 2.36228,-7.9813 2.36228,-22.018707 0,-29.999997 C 151.93673,65.371023 136.48295,50.297336 116.5,44.412396 108.40056,42.027127 92.588762,42.026386 84.542711,44.410896 64.307734,50.407697 48.5342,66.039237 43.452758,85.130959 c -1.948821,7.322 -1.86506,23.116411 0.158766,29.937541 8.730326,29.42481 38.97193,46.91812 69.888476,40.4271 z";
			break;
		case StiCheckStyle.NoneRectangle:
			result = "m 24.152542,102.04237 0,-72.499997 74.5,0 74.500008,0 0,72.499997 0,72.5 -74.500008,0 -74.5,0 0,-72.5 z m 133.758198,0.25 -0.25819,-57.249997 -59.000008,0 -59,0 -0.259695,55.999997 c -0.142833,30.8 -0.04446,56.5625 0.218615,57.25 0.375181,0.98048 13.207991,1.25 59.517885,1.25 l 59.039583,0 -0.25819,-57.25 z";
			break;
		}
		return result;
	}

	public static string RenderCheckBox(StiCheckStyle style, Color contourColor, double contourSize, StiBrush textBrush, double width)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.Transparent;
		if (textBrush is StiSolidBrush)
		{
			color = ((StiSolidBrush)textBrush).Color;
		}
		else if (textBrush is StiGradientBrush)
		{
			color = ((StiGradientBrush)textBrush).StartColor;
		}
		else if (textBrush is StiGlareBrush)
		{
			color = ((StiGlareBrush)textBrush).StartColor;
		}
		else if (textBrush is StiGlassBrush)
		{
			color = ((StiGlassBrush)textBrush).Color;
		}
		else if (textBrush is StiHatchBrush)
		{
			color = ((StiHatchBrush)textBrush).ForeColor;
		}
		string arg = string.Format("<svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0\" y=\"0\" width=\"{0}px\" height=\"{0}px\">", width);
		string arg2 = $"<path stroke=\"#{contourColor.R:X2}{contourColor.G:X2}{contourColor.B:X2}\" stroke-width=\"{contourSize}\" fill=\"#{color.R:X2}{color.G:X2}{color.B:X2}\" stroke-linecap=\"round\" stroke-linejoin=\"round\" transform=\"scale({1.0 / (200.0 / width)})\" d=\"";
		string arg3 = string.Empty;
		switch (style)
		{
		case StiCheckStyle.Cross:
			arg3 = "m 62.567796,147.97593 c -0.55,-0.14223 -2.162828,-0.5128 -3.584062,-0.82348 -3.647667,-0.79738 -9.670499,-5.83775 -14.242817,-11.91949 l -3.902341,-5.19058 5.080199,-1.13481 c 7.353071,-1.64253 13.640456,-5.71752 21.826811,-14.14646 l 7.208128,-7.42171 -6.410736,-7.513354 c -11.773129,-13.79803 -14.346726,-23.01954 -8.627769,-30.91434 2.894109,-3.9952 11.818482,-12.369333 13.182086,-12.369333 0.411356,0 1.063049,1.6875 1.448207,3.750003 0.980474,5.25038 6.456187,16.76587 10.936694,23 2.075266,2.8875 3.991125,5.25 4.257464,5.25 0.266339,0 3.775242,-3.4875 7.797566,-7.75 16.397034,-17.37615 29.674184,-19.76481 38.280564,-6.88699 4.15523,6.21753 4.18631,8.07093 0.14012,8.3552 -5.84833,0.41088 -17.16241,8.5342 -25.51465,18.319104 l -4.63153,5.42599 4.87803,4.31529 c 6.55108,5.79533 18.8991,11.89272 25.84076,12.76002 3.0455,0.38051 5.53727,1.10582 5.53727,1.6118 0,2.7809 -9.26611,14.41872 -13.03,16.36511 -7.96116,4.11687 -16.36991,0.71207 -32.764584,-13.26677 l -4.985957,-4.25125 -7.086791,8.97188 c -3.897736,4.93454 -8.82141,10.1198 -10.9415,11.52281 -3.906121,2.58495 -8.86588,4.41339 -10.691162,3.94136 z";
			break;
		case StiCheckStyle.Check:
			arg3 = "M 60.972125,162.49704 C 51.172676,136.72254 43.561975,123.37669 35.370344,117.6027 l -4.45827,-3.14248 2.75159,-2.89559 c 3.875121,-4.07793 10.034743,-7.49924 14.902472,-8.27747 3.859874,-0.61709 4.458306,-0.38024 8.535897,3.37835 2.660692,2.45254 6.265525,7.60856 9.167226,13.11196 2.630218,4.98849 4.910542,9.06999 5.067388,9.06999 0.156846,0 2.31372,-3.0375 4.793052,-6.75 C 96.259164,91.956015 129.68299,58.786374 157.56485,41.281603 l 8.84913,-5.555656 2.2633,2.631238 2.26329,2.631237 -7.76266,6.294183 C 139.859,66.19023 108.01682,105.51363 89.042715,138.83563 c -6.680477,11.73214 -7.172359,12.31296 -15.090788,17.81963 -4.501873,3.13071 -9.044031,6.30443 -10.093684,7.05271 -1.708923,1.21826 -2.010678,1.09165 -2.886118,-1.21093 z";
			break;
		case StiCheckStyle.CrossRectangle:
			arg3 = "m 24.152542,102.04237 0,-72.499996 74.5,0 74.499998,0 0,72.499996 0,72.5 -74.499998,0 -74.5,0 0,-72.5 z m 133.758188,0.25 -0.25819,-57.249996 -58.999998,0 -59,0 -0.259695,55.999996 c -0.142833,30.8 -0.04446,56.5625 0.218615,57.25 0.375181,0.98048 13.207991,1.25 59.517885,1.25 l 59.039573,0 -0.25819,-57.25 z m -90.574091,43.18692 c -1.823747,-0.3912 -4.926397,-1.85716 -6.894778,-3.25768 -3.319254,-2.36169 -12.289319,-12.40741 -12.289319,-13.76302 0,-0.32888 2.417494,-1.13897 5.372209,-1.80021 7.185193,-1.60797 13.747505,-5.93496 21.803114,-14.3763 l 6.675323,-6.99496 -6.379078,-7.31436 C 64.931387,85.71231 61.643682,76.29465 65.471903,68.89169 67.054097,65.83207 78.56175,54.542374 80.098251,54.542374 c 0.45744,0 1.146839,1.6875 1.531997,3.75 0.980474,5.250386 6.456187,16.765876 10.936694,22.999996 2.075266,2.8875 3.991125,5.25 4.257464,5.25 0.266339,0 3.775244,-3.4875 7.797564,-7.75 16.39704,-17.376139 29.67419,-19.764806 38.28057,-6.88698 4.15523,6.21752 4.18631,8.07092 0.14012,8.35519 -5.82996,0.40959 -18.23707,9.34942 -25.91566,18.67328 -3.90068,4.73647 -3.97203,4.95414 -2.2514,6.86861 3.19054,3.54997 13.7039,10.54321 18.97191,12.61967 2.83427,1.11716 7.43737,2.33421 10.22912,2.70455 2.79175,0.37034 5.07591,0.9956 5.07591,1.38947 0,2.11419 -8.37504,13.20895 -11.6517,15.4355 -8.39423,5.70403 -16.63203,2.77 -34.14289,-12.16054 l -4.985955,-4.25125 -7.086791,8.97188 c -9.722344,12.3085 -16.524852,16.55998 -23.948565,14.96754 z";
			break;
		case StiCheckStyle.CheckRectangle:
			arg3 = "m 19.915254,103.5 0,-72.5 71.942245,0 71.942241,0 6.55727,-4.11139 6.55726,-4.11139 1.96722,2.36139 c 1.08197,1.298765 1.98219,2.644166 2.00049,2.98978 0.0183,0.345615 -2.44173,2.53784 -5.46673,4.87161 l -5.5,4.243219 0,69.378391 0,69.37839 -74.999991,0 -75.000005,0 0,-72.5 z m 133.999996,3.87756 c 0,-49.33933 -0.12953,-53.514947 -1.62169,-52.276568 -2.78014,2.307312 -15.68408,17.90053 -24.32871,29.399008 -10.4919,13.955575 -23.47926,33.53736 -29.514025,44.5 -4.457326,8.09707 -5.134776,8.80812 -14.291256,15 -5.28667,3.575 -9.903486,6.62471 -10.259592,6.77712 -0.356107,0.15242 -1.912439,-2.99758 -3.458515,-7 -1.546077,-4.00241 -5.258394,-12.41205 -8.249593,-18.68809 -4.285436,-8.99155 -6.676569,-12.64898 -11.27758,-17.25 C 47.70282,104.62757 44.364254,102 43.495254,102 c -2.798369,0 -1.704872,-1.66044 3.983717,-6.049158 5.593548,-4.31539 13.183139,-7.091307 16.801313,-6.145133 3.559412,0.930807 9.408491,8.154973 13.919775,17.192241 l 4.46286,8.94025 4.54378,-6.83321 C 95.518219,96.605618 108.21371,81.688517 125.80695,63.75 L 143.21531,46 l -53.650021,0 -53.650035,0 0,57.5 0,57.5 59.000005,0 58.999991,0 0,-53.62244 z";
			break;
		case StiCheckStyle.CrossCircle:
			arg3 = "M 83.347458,173.13597 C 61.069754,168.04956 42.193415,152.8724 32.202285,132.01368 23.4014,113.63986 23.679644,89.965903 32.91889,71.042373 41.881579,52.685283 60.867647,37.139882 80.847458,31.799452 c 10.235111,-2.735756 31.264662,-2.427393 40.964762,0.600679 26.18668,8.174684 46.06876,28.926852 51.62012,53.879155 2.43666,10.952327 1.56754,28.058524 -1.98036,38.977594 -6.65679,20.48707 -25.64801,38.95163 -47.32647,46.01402 -6.3909,2.08202 -10.18566,2.59644 -21.27805,2.88446 -9.033911,0.23456 -15.484931,-0.10267 -19.500002,-1.01939 z M 112.4138,158.45825 c 17.13137,-3.13002 33.71724,-15.96081 41.41353,-32.03742 14.8975,-31.119027 -1.10807,-67.659584 -34.40232,-78.540141 -6.71328,-2.193899 -9.93541,-2.643501 -19.07755,-2.661999 -9.354252,-0.01893 -12.16228,0.37753 -18.768532,2.649866 -17.155451,5.900919 -29.669426,17.531424 -36.438658,33.866137 -2.152301,5.193678 -2.694658,8.35455 -3.070923,17.89744 -0.518057,13.139047 0.741843,19.201887 6.111644,29.410237 4.106815,7.80733 15.431893,19.09359 23.36818,23.28808 12.061362,6.37467 27.138828,8.6356 40.864629,6.1278 z M 69.097458,133.41654 c -2.8875,-2.75881 -5.25,-5.35869 -5.25,-5.77751 0,-0.41882 5.658529,-6.30954 12.57451,-13.0905 l 12.57451,-12.329 L 76.198053,89.392633 63.399628,76.565738 68.335951,71.554056 c 2.714978,-2.756426 5.304859,-5.011683 5.75529,-5.011683 0.450432,0 6.574351,5.611554 13.608709,12.470121 l 12.78974,12.470119 4.42889,-4.553471 c 2.43588,-2.50441 8.39186,-8.187924 13.23551,-12.630032 l 8.80663,-8.076559 5.34744,5.281006 5.34743,5.281007 -12.96155,12.557899 -12.96154,12.557897 13.13318,13.16027 13.13319,13.16027 -5.18386,4.66074 c -2.85112,2.5634 -5.70472,4.66073 -6.34134,4.66073 -0.63661,0 -6.5434,-5.4 -13.12621,-12 -6.58281,-6.6 -12.3871,-12 -12.89844,-12 -0.511329,0 -6.593363,5.60029 -13.515627,12.44509 l -12.585935,12.44508 -5.25,-5.016 z";
			break;
		case StiCheckStyle.DotCircle:
			arg3 = "M 81.652542,170.5936 C 59.374838,165.50719 40.498499,150.33003 30.507369,129.47131 21.706484,111.09749 21.984728,87.42353 31.223974,68.5 40.186663,50.14291 59.172731,34.597509 79.152542,29.257079 89.387653,26.521323 110.4172,26.829686 120.1173,29.857758 c 26.18668,8.174684 46.06876,28.926852 51.62012,53.879152 2.43666,10.95233 1.56754,28.05853 -1.98036,38.9776 -6.65679,20.48707 -25.64801,38.95163 -47.32647,46.01402 -6.3909,2.08202 -10.18566,2.59644 -21.27805,2.88446 -9.033907,0.23456 -15.484927,-0.10267 -19.499998,-1.01939 z m 29.999998,-15.098 c 20.68862,-4.34363 38.01874,-20.45437 44.09844,-40.9956 2.36228,-7.9813 2.36228,-22.0187 0,-30 C 150.08927,65.371023 134.63549,50.297336 114.65254,44.412396 106.5531,42.027127 90.741304,42.026386 82.695253,44.4109 62.460276,50.407701 46.686742,66.039241 41.6053,85.13096 c -1.948821,7.32201 -1.86506,23.11641 0.158766,29.93754 8.730326,29.42481 38.97193,46.91812 69.888474,40.4271 z M 90.004747,122.6703 C 76.550209,117.63801 69.825047,101.82445 75.898143,89.5 c 2.136718,-4.33615 7.147144,-9.356192 11.754399,-11.776953 5.578622,-2.931141 16.413098,-2.927504 22.052908,0.0074 18.03,9.382663 19.07573,32.784373 1.91442,42.841563 -5.57282,3.26589 -15.830952,4.2617 -21.615123,2.09829 z";
			break;
		case StiCheckStyle.DotRectangle:
			arg3 = "m 23.847458,101.19491 0,-72.499995 74.5,0 74.499992,0 0,72.499995 0,72.5 -74.499992,0 -74.5,0 0,-72.5 z m 133.999992,-0.008 0,-57.507925 -59.249992,0.25793 -59.25,0.25793 -0.25819,57.249995 -0.258189,57.25 59.508189,0 59.508182,0 0,-57.50793 z m -94.320573,33.85402 c -0.37368,-0.37368 -0.679419,-15.67942 -0.679419,-34.01275 l 0,-33.333335 35.513302,0 35.51329,0 -0.2633,33.749995 -0.2633,33.75 -34.570573,0.26275 c -19.013819,0.14452 -34.876319,-0.043 -35.25,-0.41666 z";
			break;
		case StiCheckStyle.NoneCircle:
			arg3 = "M 83.5,170.5936 C 61.222296,165.50719 42.345957,150.33003 32.354827,129.47131 23.553942,111.09749 23.832186,87.423523 33.071432,68.5 42.034121,50.14291 61.020189,34.597509 81,29.257079 c 10.235111,-2.735756 31.26466,-2.427393 40.96476,0.600679 26.18668,8.174684 46.06876,28.926852 51.62012,53.879155 2.43666,10.95232 1.56754,28.058527 -1.98036,38.977597 -6.65679,20.48707 -25.64801,38.95163 -47.32647,46.01402 -6.3909,2.08202 -10.18566,2.59644 -21.27805,2.88446 -9.033909,0.23456 -15.484929,-0.10267 -19.5,-1.01939 z m 30,-15.098 c 20.68862,-4.34363 38.01874,-20.45437 44.09844,-40.9956 2.36228,-7.9813 2.36228,-22.018707 0,-29.999997 C 151.93673,65.371023 136.48295,50.297336 116.5,44.412396 108.40056,42.027127 92.588762,42.026386 84.542711,44.410896 64.307734,50.407697 48.5342,66.039237 43.452758,85.130959 c -1.948821,7.322 -1.86506,23.116411 0.158766,29.937541 8.730326,29.42481 38.97193,46.91812 69.888476,40.4271 z";
			break;
		case StiCheckStyle.NoneRectangle:
			arg3 = "m 24.152542,102.04237 0,-72.499997 74.5,0 74.500008,0 0,72.499997 0,72.5 -74.500008,0 -74.5,0 0,-72.5 z m 133.758198,0.25 -0.25819,-57.249997 -59.000008,0 -59,0 -0.259695,55.999997 c -0.142833,30.8 -0.04446,56.5625 0.218615,57.25 0.375181,0.98048 13.207991,1.25 59.517885,1.25 l 59.039583,0 -0.25819,-57.25 z";
			break;
		}
		return $"{arg}{arg2}{arg3}\" /></svg>";
	}

	public static string RenderShapeAsBase64(StiComponent component)
	{
		byte[] inArray = RenderShape(component).ToArray();
		return "data:image/svg+xml;base64," + Convert.ToBase64String(inArray);
	}

	public static MemoryStream RenderShape(StiComponent component)
	{
		if (!(component is StiShape stiShape) || !CheckShape(component))
		{
			return null;
		}
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
		{
			Indentation = 0,
			Formatting = Formatting.None
		};
		xmlTextWriter.WriteStartDocument();
		xmlTextWriter.WriteStartElement("svg");
		xmlTextWriter.WriteAttributeString("xmlns", "http://www.w3.org/2000/svg");
		double num = 1.0;
		double num2 = num * component.Report.Unit.ConvertToHInches(component.Height);
		double num3 = num * component.Report.Unit.ConvertToHInches(component.Width);
		int num4 = (int)Math.Round((double)(stiShape.Size / 2f) + 0.5);
		xmlTextWriter.WriteAttributeString("height", ToUnits(num2));
		xmlTextWriter.WriteAttributeString("width", ToUnits(num3));
		xmlTextWriter.WriteAttributeString("viewBox", $"{ToUnits(-num4)} {ToUnits(-num4)} {ToUnits(num3 + (double)(num4 * 2))} {ToUnits(num2 + (double)(num4 * 2))}");
		StiSvgData svgData = default(StiSvgData);
		svgData.X = 0.0;
		svgData.Y = 0.0;
		svgData.Width = num3;
		svgData.Height = num2;
		svgData.Component = component;
		WriteShape(xmlTextWriter, svgData, -1, useClip: false, new Hashtable());
		xmlTextWriter.WriteFullEndElement();
		xmlTextWriter.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return memoryStream;
	}
}
