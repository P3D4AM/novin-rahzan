using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Gauge.Design;
using Stimulsoft.Report.Design.Gauges.Editor;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Chart.Editor;

public class StiCloudGaugeForm : Form
{
	internal StiGauge OriginalGauge;

	internal StiGauge Gauge;

	internal StiCloudGaugeControl GaugeControl;

	private StiCloudGaugeStylesControl stylesControl;

	private IContainer components;

	private StiGroupLine stiGroupLine1;

	private Button btOk;

	private Button btCancel;

	private StiTabulator tabControl;

	private StiTabulatorPage pageGauge;

	private StiTabulatorPage pageStyles;

	private Panel pageGaugeContent;

	internal StiReportPropertyGrid PropertyGrid;

	private Panel pageStylesContent;

	internal StiGaugePanel GaugePanel;

	public StiCloudGaugeForm(StiGauge gauge)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			Localize();
			Gauge = gauge.Clone() as StiGauge;
			OriginalGauge = gauge;
			GaugePanel.Gauge = Gauge;
			((PropertyGrid)PropertyGrid).CommandsVisibleIfAvailable = true;
			((PropertyGrid)PropertyGrid).LargeButtons = false;
			PropertyGrid.SetStandardPropertyTabs();
			((PropertyGrid)PropertyGrid).SelectedObject = null;
			tabControl.SelectedIndex = 0;
			TabControl_OnSelectionChanged(null, null);
			((Form)this).Load += This_Load;
		}
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Gauge", "GaugeEditorForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		((Control)pageGauge).Text = StiLocalization.Get("Components", "StiGauge");
		((Control)pageStyles).Text = StiLocalization.Get("PropertyMain", "Styles");
	}

	private void This_Load(object sender, EventArgs e)
	{
		StiTabulatorPage stiTabulatorPage = pageGauge;
		bool enabled = (((Control)pageStyles).Enabled = true);
		((Control)stiTabulatorPage).Enabled = enabled;
		GaugeControl = new StiCloudGaugeControl(this);
		((Control)pageGaugeContent).Controls.Add((Control)(object)GaugeControl);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("server-manual/index.html?reports_designer_gauge_editor.htm");
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		if (GaugeControl != null)
		{
			((Control)GaugeControl.gaugePanel).Invalidate();
		}
	}

	private void TabControl_OnSelectionChanged(object sender, EventArgs e)
	{
		if (tabControl.SelectedTab == pageGauge)
		{
			if (GaugeControl != null)
			{
				GaugeControl.CheckStatePropertyGrid();
			}
		}
		else if (tabControl.SelectedTab == pageStyles)
		{
			if (stylesControl == null)
			{
				stylesControl = new StiCloudGaugeStylesControl(this);
				((Control)pageStylesContent).Controls.Add((Control)(object)stylesControl);
			}
			else
			{
				stylesControl.BuildStyles();
			}
			stylesControl.CheckPropertyGridValue();
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		int index = OriginalGauge.Parent.Components.IndexOf(OriginalGauge);
		OriginalGauge.Parent.Components[index] = Gauge;
		Gauge.Mode = StiScaleMode.V1;
		Gauge.Select();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		btOk = new Button();
		btCancel = new Button();
		tabControl = new StiTabulator();
		pageGauge = new StiTabulatorPage();
		pageGaugeContent = new Panel();
		pageStyles = new StiTabulatorPage();
		pageStylesContent = new Panel();
		stiGroupLine1 = new StiGroupLine();
		PropertyGrid = new StiReportPropertyGrid();
		GaugePanel = new StiGaugePanel();
		((Control)tabControl).SuspendLayout();
		((Control)pageGauge).SuspendLayout();
		((Control)pageStyles).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(966, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(1047, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tabControl).Anchor = (AnchorStyles)15;
		((Control)tabControl).Controls.Add((Control)(object)pageGauge);
		((Control)tabControl).Controls.Add((Control)(object)pageStyles);
		((Control)tabControl).Location = new Point(12, 12);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 22, 0, 0);
		((Control)tabControl).Size = new Size(754, 537);
		((Control)tabControl).TabIndex = 0;
		tabControl.TitleBackColor = Color.White;
		tabControl.SelectedIndexChanged += TabControl_OnSelectionChanged;
		((Control)pageGauge).Controls.Add((Control)(object)pageGaugeContent);
		pageGauge.Dock = (DockStyle)5;
		pageGauge.InvisibleVisible = true;
		((Control)pageGauge).Location = new Point(0, 22);
		((Control)pageGauge).Name = "pageGauge";
		((Control)pageGauge).Padding = new Padding(0, 0, 1, 0);
		((Control)pageGauge).Size = new Size(754, 515);
		((Control)pageGauge).TabIndex = 0;
		((Control)pageGauge).Text = "Gauge";
		((Control)pageGauge).Visible = false;
		((Control)pageGaugeContent).Anchor = (AnchorStyles)15;
		((Control)pageGaugeContent).BackColor = Color.White;
		((Control)pageGaugeContent).Location = new Point(3, 3);
		((Control)pageGaugeContent).Name = "pageGaugeContent";
		((Control)pageGaugeContent).Size = new Size(748, 509);
		((Control)pageGaugeContent).TabIndex = 0;
		((Control)pageStyles).BackColor = Color.White;
		((Control)pageStyles).Controls.Add((Control)(object)pageStylesContent);
		pageStyles.Dock = (DockStyle)5;
		pageStyles.InvisibleVisible = true;
		((Control)pageStyles).Location = new Point(0, 22);
		((Control)pageStyles).Name = "pageStyles";
		((Control)pageStyles).Padding = new Padding(0, 0, 1, 2);
		((Control)pageStyles).Size = new Size(754, 515);
		((Control)pageStyles).TabIndex = 1;
		((Control)pageStyles).Text = "Styles";
		((Control)pageStyles).Visible = false;
		((Control)pageStylesContent).Anchor = (AnchorStyles)15;
		((Control)pageStylesContent).Location = new Point(3, 3);
		((Control)pageStylesContent).Name = "pageStylesContent";
		((Control)pageStylesContent).Size = new Size(748, 509);
		((Control)pageStylesContent).TabIndex = 0;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 565);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(1134, 46);
		((Control)stiGroupLine1).TabIndex = 2;
		((PropertyGrid)PropertyGrid).CategoryForeColor = SystemColors.InactiveCaptionText;
		((Control)PropertyGrid).Cursor = Cursors.HSplit;
		((PropertyGrid)PropertyGrid).HelpBackColor = Color.White;
		((PropertyGrid)PropertyGrid).LineColor = SystemColors.Control;
		((Control)PropertyGrid).Location = new Point(787, 12);
		((Control)PropertyGrid).Name = "PropertyGrid";
		PropertyGrid.ScrollOffset = 0;
		PropertyGrid.ShowEventsTag = true;
		((Control)PropertyGrid).Size = new Size(334, 537);
		((Control)PropertyGrid).TabIndex = 1;
		((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		((Control)GaugePanel).Anchor = (AnchorStyles)15;
		((Control)GaugePanel).BackColor = Color.Transparent;
		((Control)GaugePanel).Location = new Point(277, 82);
		((Control)GaugePanel).Name = "GaugePanel";
		((Control)GaugePanel).Size = new Size(464, 443);
		((Control)GaugePanel).TabIndex = 750;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(1134, 611);
		((Control)this).Controls.Add((Control)(object)GaugePanel);
		((Control)this).Controls.Add((Control)(object)PropertyGrid);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCloudGaugeForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiCloudChartForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)tabControl).ResumeLayout(false);
		((Control)pageGauge).ResumeLayout(false);
		((Control)pageStyles).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
