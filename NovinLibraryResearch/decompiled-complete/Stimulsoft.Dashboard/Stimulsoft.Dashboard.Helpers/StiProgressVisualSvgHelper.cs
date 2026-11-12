using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Progress;
using Stimulsoft.Dashboard.Visuals.Progress.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Visuals;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Export.Services.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Helpers;

public class StiProgressVisualSvgHelper : IStiProgressVisualSvgHelper
{
	public void WriteProgress(XmlTextWriter writer, StiSvgData svgData, ref bool needToScroll, ref int contentHeight, bool isStyleSample = false)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		RectangleF rect = new RectangleF(0f, 0f, (float)svgData.Width, (float)svgData.Height);
		StiProgressElement element = svgData.Component as StiProgressElement;
		StiDataTable dataTable = GetDataTable(element);
		if (dataTable == null)
		{
			return;
		}
		IEnumerable<StiProgressIteration> enumerable = new StiProgressElementBuilder().Render(element, dataTable);
		if (isStyleSample)
		{
			enumerable = GetStyleSampleIterations();
		}
		else if (IsEmptyData(element, dataTable))
		{
			enumerable = GetEmptyDataIterations();
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
				StiProgressVisual stiProgressVisual = StiProgressVisualCreator.CreateProgressVisual(element, enumerable.ToList());
				if (needToScroll)
				{
					needToScroll = stiProgressVisual.GetContentRectangle(rect).Height > rect.Height + 5f;
					if (needToScroll)
					{
						rect.Width -= 15f;
						RectangleF contentRectangle = stiProgressVisual.GetContentRectangle(rect);
						rect = contentRectangle;
						contentHeight = Convert.ToInt32(rect.Height);
					}
				}
				stiProgressVisual.Draw(context, rect);
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

	private List<StiProgressIteration> GetStyleSampleIterations()
	{
		return new List<StiProgressIteration>
		{
			new StiProgressIteration
			{
				Value = 75m,
				Target = 100
			}
		};
	}

	private List<StiProgressIteration> GetEmptyDataIterations()
	{
		return new List<StiProgressIteration>
		{
			new StiProgressIteration
			{
				Value = 0m,
				Series = StiLocalization.Get("Dashboard", "Blank")
			}
		};
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

	public void WriteProgress(XmlTextWriter writer, StiSvgData svgData, bool isStyleSample = false)
	{
		bool needToScroll = false;
		int contentHeight = 0;
		WriteProgress(writer, svgData, ref needToScroll, ref contentHeight, isStyleSample);
	}
}
