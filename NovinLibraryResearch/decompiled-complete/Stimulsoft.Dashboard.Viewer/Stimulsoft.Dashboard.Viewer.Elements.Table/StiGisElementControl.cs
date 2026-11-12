using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.Table;

[ToolboxItem(false)]
public class StiGisElementControl : StiElementControl
{
	private IStiGisMapControl GisControl { get; set; }

	protected StiOnlineMapElement OnlineMapElement => base.Element as StiOnlineMapElement;

	public override StiDataTable GetOrCreateData()
	{
		return StiElementDataCache.GetOrCreate(base.Element);
	}

	public override void CreateChild()
	{
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		GisControl = StiGisMapRepaintHelper.CreateWinFormsControl();
		if (GisControl == null)
		{
			return;
		}
		GisControl.SetProviderType(OnlineMapElement.Provider, OnlineMapElement.GisSettings.Color, OnlineMapElement.GisSettings.Size, OnlineMapElement.GisSettings.ShowPlacemark, OnlineMapElement.GisSettings.Language, OnlineMapElement.GisSettings.Icon, OnlineMapElement.GisSettings.IconColor);
		GisControl.Zoom = 9.0;
		GisControl.AllowLocalCache = true;
		if (base.DataTable != null)
		{
			List<string> stringValues = GetStringValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapMeter));
			List<string> stringValues2 = GetStringValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapColorMeter));
			List<object> objectValues = GetObjectValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapLineSizeMeter));
			List<string> stringValues3 = GetStringValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapDescriptionMeter));
			GisControl.InitGeoCommands(stringValues, stringValues2, objectValues, stringValues3);
		}
		base.Child = (Control)GisControl;
		((Control)this).Controls.Add((Control)GisControl);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.DataTable = dataTable;
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (!base.IsShowNoResult && !base.IsShowEmptyData && GisControl != null)
		{
			List<string> stringValues = GetStringValues(base.DataTable?.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapMeter));
			List<string> stringValues2 = GetStringValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapColorMeter));
			List<object> objectValues = GetObjectValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapLineSizeMeter));
			List<string> stringValues3 = GetStringValues(base.DataTable.Meters?.FirstOrDefault((IStiMeter f) => f is StiGisMapDescriptionMeter));
			GisControl.InitGeoCommands(stringValues, stringValues2, objectValues, stringValues3);
		}
	}

	protected List<string> GetStringValues(IStiMeter meter)
	{
		if (base.DataTable == null || meter == null)
		{
			return null;
		}
		int num = base.DataTable.Meters.IndexOf(meter);
		if (num == -1)
		{
			return null;
		}
		return base.DataTable.Rows.GetStringArrayItem(num).ToList();
	}

	protected List<object> GetObjectValues(IStiMeter meter)
	{
		if (base.DataTable == null || meter == null)
		{
			return null;
		}
		int num = base.DataTable.Meters.IndexOf(meter);
		if (num == -1)
		{
			return null;
		}
		return base.DataTable.Rows.GetArrayItem(num).ToList();
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public StiGisElementControl(IStiElement element)
		: base(element)
	{
	}
}
