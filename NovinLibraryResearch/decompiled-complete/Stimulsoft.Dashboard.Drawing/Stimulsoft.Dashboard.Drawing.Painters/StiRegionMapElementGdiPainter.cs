using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Drawing.Atoms;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiRegionMapElementGdiPainter : StiAtomElementGdiPainter
{
	public override void PaintAtom(Graphics g, RectangleD rect, IStiElement element)
	{
		StiRegionMapElement stiRegionMapElement = element as StiRegionMapElement;
		if (stiRegionMapElement.DataFrom == StiMapSource.Manual)
		{
			StiElementAtom.PaintElement(g, rect, element, null);
		}
		else if (stiRegionMapElement.DataFrom == StiMapSource.DataColumns)
		{
			StiDataTable dataTable = (StiDbsThumbnaiModelHelper.AllowPage(element.Page) ? StiThumbnailDataHelper.Get(element) : StiElementDataCache.GetOrCreateWithProgress(stiRegionMapElement));
			if (IsNoResult(element, dataTable))
			{
				PaintNoResult(g, rect, element);
			}
			else
			{
				StiElementAtom.PaintElement(g, rect, element, dataTable);
			}
		}
	}
}
