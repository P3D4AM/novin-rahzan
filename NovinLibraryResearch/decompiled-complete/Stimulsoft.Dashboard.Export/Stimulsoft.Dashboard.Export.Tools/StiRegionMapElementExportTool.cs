using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Dashboard.Export.Tools;

public class StiRegionMapElementExportTool : StiElementExportTool
{
	public override void Render(IStiElement element, StiPanel destination, RectangleD rect, StiDashboardExportSettings settings)
	{
		if (!RenderEmptyDataMessage(element, destination, rect, settings))
		{
			StiRegionMapElement stiRegionMapElement = (StiRegionMapElement)element;
			StiMap stiMap = new StiMap();
			stiMap.Language = stiRegionMapElement.Language;
			stiMap.Stretch = true;
			stiMap.Page = stiRegionMapElement.Page;
			stiMap.MapType = stiRegionMapElement.MapType;
			stiMap.DataFrom = stiRegionMapElement.DataFrom;
			stiMap.ShowLegend = true;
			stiMap.MapStyle = StiDashboardStyleHelper.GetMapStyleIdent(stiRegionMapElement);
			stiMap.ComponentStyle = stiRegionMapElement.CustomStyleName;
			stiMap.MapIdent = stiRegionMapElement.MapIdent;
			stiMap.MapMode = StiMapMode.Choropleth;
			stiMap.ShowValue = stiRegionMapElement.ShowValue;
			stiMap.ShortValue = stiRegionMapElement.ShortValue;
			stiMap.ShowBubble = stiRegionMapElement.ShowBubble;
			stiMap.ColorEach = stiRegionMapElement.ColorEach;
			stiMap.DisplayNameType = stiRegionMapElement.ShowName;
			stiMap.ColorDataColumn = ((stiRegionMapElement.ColorMeter == null) ? string.Empty : stiRegionMapElement.ColorMeter.Expression);
			stiMap.GroupDataColumn = ((stiRegionMapElement.GroupMeter == null) ? string.Empty : stiRegionMapElement.GroupMeter.Expression);
			stiMap.ValueDataColumn = ((stiRegionMapElement.ValueMeter == null) ? string.Empty : stiRegionMapElement.ValueMeter.Expression);
			stiMap.NameDataColumn = ((stiRegionMapElement.NameMeter == null) ? string.Empty : stiRegionMapElement.NameMeter.Expression);
			stiMap.KeyDataColumn = ((stiRegionMapElement.KeyMeter == null) ? string.Empty : stiRegionMapElement.KeyMeter.Expression);
			stiMap.MapData = stiRegionMapElement.MapData;
			stiMap.Brush = new StiSolidBrush(StiDashboardStyleHelper.GetBackColor(element, null, settings is StiSvgDashboardExportSettings || settings is StiHtmlDashboardExportSettings));
			stiMap.Left = rect.Left;
			stiMap.Top = rect.Top;
			stiMap.Width = rect.Width;
			stiMap.Height = rect.Height;
			StiMap stiMap2 = stiMap;
			if (stiRegionMapElement.DataFrom == StiMapSource.DataColumns)
			{
				stiMap2.DataTable = StiElementDataCache.TryToGetOrCreate(stiRegionMapElement);
				destination.Report.Dictionary = stiRegionMapElement.Report.Dictionary;
			}
			destination.Components.Add(stiMap2);
		}
	}
}
