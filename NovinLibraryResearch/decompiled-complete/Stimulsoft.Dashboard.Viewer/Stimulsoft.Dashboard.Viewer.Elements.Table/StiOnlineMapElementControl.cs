using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Elements.Table;

[ToolboxItem(false)]
public class StiOnlineMapElementControl : StiElementControl
{
	private Hashtable mapData;

	private WebBrowser ContainerControl { get; set; }

	protected StiOnlineMapElement OnlineMapElement => base.Element as StiOnlineMapElement;

	internal Hashtable MapData
	{
		get
		{
			if (mapData == null)
			{
				mapData = StiOnlineMapHelper.CalculateMapData(base.DataTable, OnlineMapElement);
			}
			return mapData;
		}
	}

	public override StiDataTable GetOrCreateData()
	{
		return StiElementDataCache.GetOrCreate(base.Element);
	}

	public override void CreateChild()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		StiScrollableControl stiScrollableControl = new StiScrollableControl(base.Element);
		((Control)stiScrollableControl).Dock = (DockStyle)5;
		base.Child = (Control)(object)stiScrollableControl;
		ContainerControl = new WebBrowser
		{
			ScrollBarsEnabled = false,
			ScriptErrorsSuppressed = true,
			Location = new Point(0, 0),
			Dock = (DockStyle)5,
			Visible = false
		};
		base.Child.Controls.Add((Control)(object)ContainerControl);
		base.Child.MouseEnter += Child_RefreshActionButtons;
		base.Child.MouseLeave += Child_RefreshActionButtons;
		base.Child.MouseMove += new MouseEventHandler(Child_RefreshActionButtons);
		ContainerControl.DocumentCompleted += (WebBrowserDocumentCompletedEventHandler)delegate
		{
			((Control)ContainerControl).Visible = true;
		};
		((Control)this).Controls.Add(base.Child);
	}

	public override void RefreshChild(StiDataTable dataTable)
	{
		base.RefreshChild(dataTable);
		base.DataTable = dataTable;
		base.IsShowNoResult = dataTable == null;
		base.IsShowEmptyData = dataTable?.IsEmpty ?? false;
		if (!base.IsShowNoResult && !base.IsShowEmptyData)
		{
			mapData = null;
			string script = StiBingMapHelper.GetScript(MapData);
			ContainerControl.DocumentText = script;
		}
	}

	protected override void Dispose(bool disposing)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (disposing && base.Child != null)
		{
			base.Child.MouseEnter -= Child_RefreshActionButtons;
			base.Child.MouseLeave -= Child_RefreshActionButtons;
			base.Child.MouseMove -= new MouseEventHandler(Child_RefreshActionButtons);
		}
		base.Dispose(disposing);
	}

	private void Child_RefreshActionButtons(object sender, EventArgs e)
	{
		RefreshActionButtons();
	}

	public StiOnlineMapElementControl(IStiElement element)
		: base(element)
	{
	}
}
