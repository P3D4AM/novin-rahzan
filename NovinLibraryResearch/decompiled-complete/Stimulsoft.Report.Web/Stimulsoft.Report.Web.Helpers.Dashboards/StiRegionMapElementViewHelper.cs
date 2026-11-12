using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Web.Helpers.Dashboards;

internal class StiRegionMapElementViewHelper
{
	public static string GetColumnPath(IStiRegionMapElement regionMapElement)
	{
		IStiMeter keyMeter = regionMapElement.GetKeyMeter();
		if (keyMeter == null)
		{
			return null;
		}
		StiDataColumn stiDataColumn = StiDataExpressionHelper.GetDataColumnFromExpression(regionMapElement.Page as IStiDashboard, keyMeter.Expression) as StiDataColumn;
		if (stiDataColumn?.DataSource == null)
		{
			return null;
		}
		return stiDataColumn.DataSource.Name + "." + stiDataColumn.Name;
	}
}
