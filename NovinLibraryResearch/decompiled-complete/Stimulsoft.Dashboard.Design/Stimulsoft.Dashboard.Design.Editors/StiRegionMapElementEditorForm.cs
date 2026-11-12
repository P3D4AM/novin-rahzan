using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Design.Editors.RegionMap;
using Stimulsoft.Report;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiRegionMapElementEditorForm : StiElementEditorForm
{
	private IContainer components;

	public override bool AllowResize => false;

	internal StiRegionMapElement MapElement => base.Element as StiRegionMapElement;

	internal StiRegionMapDataControl RegionControl { get; private set; }

	public StiRegionMapElementEditorForm()
		: this(null)
	{
	}

	public StiRegionMapElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			CreateMapControl();
			LocalizeInternal();
			ActivateDictionaryPanel();
		}
	}

	public override void Localize()
	{
		LocalizeInternal();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiMap");
		if (RegionControl != null)
		{
			RegionControl.Localize();
		}
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			RegionControl.ColumnsControl.ReadPropertiesFromElement();
			((ListBox)RegionControl.ColumnsControl.listBoxKey).SelectedItem = ((ListBox)RegionControl.ColumnsControl.listBoxKey).Items[0];
		}
	}

	public void RefreshPropertyGrid()
	{
		StiPropertiesPanelService service = StiPropertiesPanelService.GetService(Designer);
		if (service != null && service.ServiceEnabled)
		{
			service.UpdateComponent();
		}
	}

	internal void CalculateWindowSize()
	{
		CalculateWindowHeight();
	}

	protected override void CalculateWindowHeight()
	{
		if (RegionControl != null)
		{
			RegionControl.CalculateControlHeight();
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + ((Control)RegionControl).Height);
		}
	}

	private void CreateMapControl()
	{
		if (RegionControl == null)
		{
			StiRegionMapDataControl stiRegionMapDataControl = new StiRegionMapDataControl(this, MapElement);
			((Control)stiRegionMapDataControl).Dock = (DockStyle)1;
			RegionControl = stiRegionMapDataControl;
			((Control)RegionControl).VisibleChanged += RegionControl_VisibleChanged;
			RegionControl.Init();
			((Control)this).Controls.Add((Control)(object)RegionControl);
		}
		CalculateWindowHeight();
		CalculateWindowPosition();
	}

	private void RegionControl_VisibleChanged(object sender, EventArgs e)
	{
		CalculateWindowSize();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_maps_region_map.htm");
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		CalculateWindowPosition();
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		if (RegionControl != null)
		{
			((Control)RegionControl).VisibleChanged -= RegionControl_VisibleChanged;
			RegionControl.DetachEvents();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(934, 1100);
		((Form)this).Margin = new Padding(4);
		((Control)this).Name = "StiRegionMapElementEditorForm";
		((Control)this).Padding = new Padding(24);
		((Control)this).Text = "Map";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).ResumeLayout(false);
	}

	private void TabControl_SelectedIndexChanged1(object sender, EventArgs e)
	{
		throw new NotImplementedException();
	}
}
