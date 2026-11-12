using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public abstract class StiElementAtom : IDisposable
{
	public IStiElement Element { get; set; }

	public RectangleD Location { get; set; }

	public virtual StiDataTable DataTable { get; set; }

	internal StiAnimationEngine AnimationEngine { get; private set; }

	public virtual void Dispose()
	{
		Element = null;
		DataTable = null;
	}

	public abstract void Paint(Graphics g);

	public abstract void Render();

	public static void PaintElement(Graphics g, RectangleD rect, IStiElement element, StiDataTable dataTable, bool isExporting = false)
	{
		using StiElementAtom stiElementAtom = Get(rect, element, dataTable, null, isExporting);
		stiElementAtom.Paint(g);
	}

	public static StiElementAtom Get(RectangleD location, IStiElement element, StiDataTable dataTable, StiAnimationEngine animationEngine = null, bool isExporting = false)
	{
		if (element is IStiChartElement)
		{
			return new StiChartAtom(element, location, dataTable, animationEngine);
		}
		if (element is IStiCardsElement)
		{
			return new StiCardsAtom(element, location, dataTable);
		}
		if (element is IStiGaugeElement)
		{
			return new StiGaugeAtom(element, location, dataTable, animationEngine);
		}
		if (element is IStiImageElement)
		{
			return new StiImageAtom(element, location, dataTable);
		}
		if (element is IStiIndicatorElement)
		{
			return new StiIndicatorAtom(element, location, dataTable);
		}
		if (element is IStiOnlineMapElement)
		{
			IStiOnlineMapElement stiOnlineMapElement = (IStiOnlineMapElement)element;
			if (stiOnlineMapElement.GetGisMeter() != null)
			{
				return new StiGisMapAtom(element, location, dataTable, isExporting);
			}
			return new StiOnlineMapAtom(element, location, dataTable, isExporting);
		}
		if (element is IStiProgressElement)
		{
			return new StiProgressAtom(element, location, dataTable);
		}
		if (element is IStiRegionMapElement)
		{
			return new StiRegionMapAtom(element, location, dataTable, animationEngine);
		}
		if (element is IStiShapeElement)
		{
			return new StiShapeAtom(element, location, dataTable);
		}
		if (element is IStiTextElement)
		{
			return new StiTextAtom(element, location, dataTable);
		}
		throw new StiTypeNotRecognizedException(element.GetType());
	}

	public StiElementAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: this(element, location, dataTable, null)
	{
	}

	public StiElementAtom(IStiElement element, RectangleD location, StiDataTable dataTable, StiAnimationEngine animation)
	{
		Element = element;
		Location = location;
		DataTable = dataTable;
		AnimationEngine = animation;
		Render();
	}
}
