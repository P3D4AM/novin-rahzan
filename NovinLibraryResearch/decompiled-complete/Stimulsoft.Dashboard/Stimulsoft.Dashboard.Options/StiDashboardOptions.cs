using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Components.Image;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Components.Panel;
using Stimulsoft.Dashboard.Components.PivotTable;
using Stimulsoft.Dashboard.Components.Progress;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Components.Shape;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Components.TreeView;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Options;

public sealed class StiDashboardOptions
{
	public sealed class Data
	{
		public static bool AllowNulls = false;

		public static StiDashboardProgressRefreshType ProgressRefreshType = StiDashboardProgressRefreshType.Invalidate;
	}

	public sealed class Services
	{
		private static List<StiComponent> elements;

		public static List<StiComponent> Elements
		{
			get
			{
				lock (lockObject)
				{
					return elements ?? (elements = new List<StiComponent>
					{
						new StiDashboard(),
						new StiChartElement(),
						new StiGaugeElement(),
						new StiIndicatorElement(),
						new StiPivotTableElement(),
						new StiTableElement(),
						new StiRegionMapElement(),
						new StiOnlineMapElement(),
						new StiProgressElement(),
						new StiImageElement(),
						new StiShapeElement(),
						new StiTextElement(),
						new StiPanelElement(),
						new StiDatePickerElement(),
						new StiComboBoxElement(),
						new StiListBoxElement(),
						new StiTreeViewElement(),
						new StiTreeViewBoxElement()
					}.OrderBy((StiComponent f) => f.ToolboxPosition).ToList());
				}
			}
		}
	}

	private static object lockObject = new object();
}
