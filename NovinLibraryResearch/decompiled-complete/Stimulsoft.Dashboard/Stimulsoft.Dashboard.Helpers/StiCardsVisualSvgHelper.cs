using System;
using System.Drawing;
using System.Xml;
using Stimulsoft.Base.Context;
using Stimulsoft.Dashboard.Components.Cards;
using Stimulsoft.Dashboard.Visuals.Cards;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Dashboard.Visuals;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Export.Services.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Helpers;

public class StiCardsVisualSvgHelper : IStiCardsVisualSvgHelper
{
	public void WriteCards(XmlTextWriter writer, StiSvgData svgData, bool isStyleSample = false)
	{
		bool needToScroll = false;
		int contentHeight = 0;
		WriteCards(writer, svgData, ref needToScroll, ref contentHeight, isStyleSample);
	}

	public void WriteCards(XmlTextWriter writer, StiSvgData svgData, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		RectangleF rect = new RectangleF(0f, 0f, (float)svgData.Width, (float)svgData.Height);
		StiCardsElement element = svgData.Component as StiCardsElement;
		StiDataTable dataTable = GetDataTable(element);
		if (dataTable == null)
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
				StiCardsElementStyle cardsStyle = StiDashboardStyleHelper.GetCardsStyle(element);
				StiCardsVisual stiCardsVisual = new StiCardsVisual(element, dataTable);
				if (needToScroll)
				{
					needToScroll = stiCardsVisual.GetContentRectangle(rect).Height > rect.Height + 5f;
					if (needToScroll)
					{
						rect.Width -= 15f;
						RectangleF contentRectangle = stiCardsVisual.GetContentRectangle(rect);
						rect = contentRectangle;
						rect.Height += 10f;
						contentHeight = Convert.ToInt32(rect.Height);
					}
				}
				stiCardsVisual.Draw(context, rect);
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

	private static StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.TryToGetOrCreate(element);
	}
}
