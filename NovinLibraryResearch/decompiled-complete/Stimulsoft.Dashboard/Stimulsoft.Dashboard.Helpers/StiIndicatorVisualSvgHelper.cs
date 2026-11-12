using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Indicator;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dashboard.Visuals;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Export.Services.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Helpers;

public class StiIndicatorVisualSvgHelper : IStiIndicatorVisualSvgHelper
{
	public void WriteIndicator(XmlTextWriter writer, StiSvgData svgData, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		RectangleF rect = new RectangleF(0f, 0f, (float)svgData.Width, (float)svgData.Height);
		StiIndicatorElement stiIndicatorElement = svgData.Component as StiIndicatorElement;
		StiDataTable dataTable = GetDataTable(stiIndicatorElement);
		if (dataTable == null)
		{
			return;
		}
		IEnumerable<StiIndicatorIteration> enumerable = new StiIndicatorElementBuilder().Render(stiIndicatorElement, dataTable);
		if (isStyleSample)
		{
			enumerable = GetStyleSampleIterations();
		}
		else if (IsEmptyData(stiIndicatorElement, dataTable))
		{
			enumerable = GetEmptyDataIterations();
			stiIndicatorElement = GetEmptyDataIndicatorElement(stiIndicatorElement);
		}
		if (enumerable == null || !enumerable.Any())
		{
			return;
		}
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				StiGdiContextPainter contextPainter = new StiGdiContextPainter(val2);
				StiContext context = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
				StiIndicatorElementStyle indicatorStyle = StiDashboardStyleHelper.GetIndicatorStyle(stiIndicatorElement);
				StiIndicatorVisual stiIndicatorVisual = new StiIndicatorVisual(stiIndicatorElement, enumerable.ToList());
				if (needToScroll)
				{
					needToScroll = stiIndicatorVisual.GetContentRectangle(rect).Height > rect.Height + 5f;
					if (needToScroll)
					{
						rect.Width -= 15f;
						RectangleF contentRectangle = stiIndicatorVisual.GetContentRectangle(rect);
						rect = contentRectangle;
						rect.Height += 10f;
						contentHeight = Convert.ToInt32(rect.Height);
					}
				}
				stiIndicatorVisual.Draw(context, rect);
				writer.WriteStartElement("g");
				double value = (needToScroll ? 0.0 : (svgData.X + 0.5));
				double value2 = (needToScroll ? 0.0 : (svgData.Y + 0.5));
				writer.WriteAttributeString("transform", "translate(" + StiContextSvgHelper.DoubleToString(value) + "," + StiContextSvgHelper.DoubleToString(value2) + ")");
				StiContextSvgHelper.WriteGeoms(writer, context, needAnimation: false, correctionText: false);
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
	}

	private bool IsEmptyData(IStiElement element, StiDataTable dataTable)
	{
		if (dataTable != null && dataTable.IsEmpty)
		{
			return element.IsQuerable;
		}
		return false;
	}

	private List<StiIndicatorIteration> GetStyleSampleIterations()
	{
		return new List<StiIndicatorIteration>
		{
			new StiIndicatorIteration
			{
				Value = 100m
			}
		};
	}

	private List<StiIndicatorIteration> GetEmptyDataIterations()
	{
		return new List<StiIndicatorIteration>
		{
			new StiIndicatorIteration
			{
				Value = 0m,
				Series = StiLocalization.Get("Dashboard", "Blank")
			}
		};
	}

	private StiIndicatorElement GetEmptyDataIndicatorElement(StiIndicatorElement indicatorElement)
	{
		StiIndicatorElement stiIndicatorElement = indicatorElement.Clone() as StiIndicatorElement;
		stiIndicatorElement.GlyphColor = Color.FromArgb(240, 240, 240);
		return stiIndicatorElement;
	}

	private static StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.TryToGetOrCreate(element);
	}

	public void WriteIndicator(XmlTextWriter writer, StiSvgData svgData, bool isStyleSample = false)
	{
		bool needToScroll = false;
		int contentHeight = 0;
		WriteIndicator(writer, svgData, ref needToScroll, ref contentHeight, isStyleSample);
	}
}
