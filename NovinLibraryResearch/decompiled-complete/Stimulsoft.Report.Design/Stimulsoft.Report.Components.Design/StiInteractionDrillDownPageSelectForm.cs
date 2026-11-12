using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.Design;

public class StiInteractionDrillDownPageSelectForm : Form
{
	private StiInteraction interaction;

	private StiSeriesInteraction seriesInteraction;

	private List<StiPage> datas;

	private IContainer components;

	private ImageList ilData;

	private Button btOk;

	private Button btCancel;

	private StiListBoxV2 lbPanels;

	private StiGroupLine stiGroupLine1;

	public StiInteractionDrillDownPageSelectForm(StiInteraction interaction, StiSeriesInteraction seriesInteraction)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.interaction = interaction;
			this.seriesInteraction = seriesInteraction;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "InteractionDrillDownPageSelectForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void This_Load(object sender, EventArgs e)
	{
		((ListBox)lbPanels).Items.Clear();
		((ListBox)lbPanels).Items.Add((object)("[" + Loc.Get("Report", "NotAssigned") + "]"));
		((ListControl)lbPanels).SelectedIndex = 0;
		datas = new List<StiPage>();
		StiReport stiReport = ((interaction != null) ? interaction.ParentComponent.Report : seriesInteraction.ParentComponent.Report);
		foreach (StiPage page in stiReport.Pages)
		{
			if (interaction != null && page != interaction.ParentComponent.Page)
			{
				datas.Add(page);
				((ListBox)lbPanels).Items.Add((object)page.Name);
				if (page == interaction.DrillDownPage)
				{
					((ListControl)lbPanels).SelectedIndex = ((ListBox)lbPanels).Items.Count - 1;
				}
			}
			if (seriesInteraction != null && page != seriesInteraction.ParentComponent.Page)
			{
				datas.Add(page);
				((ListBox)lbPanels).Items.Add((object)page.Name);
				if (page == seriesInteraction.DrillDownPage)
				{
					((ListControl)lbPanels).SelectedIndex = ((ListBox)lbPanels).Items.Count - 1;
				}
			}
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (interaction != null)
		{
			if (((ListControl)lbPanels).SelectedIndex > 0)
			{
				interaction.DrillDownPage = datas[((ListControl)lbPanels).SelectedIndex - 1];
			}
			else
			{
				interaction.DrillDownPage = null;
			}
		}
		if (seriesInteraction != null)
		{
			if (((ListControl)lbPanels).SelectedIndex > 0)
			{
				seriesInteraction.DrillDownPage = datas[((ListControl)lbPanels).SelectedIndex - 1];
			}
			else
			{
				seriesInteraction.DrillDownPage = null;
			}
		}
		((Form)this).Close();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_interaction_drill_down_reports_drill_down_report_using_report_pages.htm");
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		components = new Container();
		ilData = new ImageList(components);
		btOk = new Button();
		btCancel = new Button();
		lbPanels = new StiListBoxV2();
		stiGroupLine1 = new StiGroupLine();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		ilData.ColorDepth = (ColorDepth)32;
		ilData.ImageSize = new Size(16, 16);
		ilData.TransparentColor = Color.Magenta;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(224, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(305, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)lbPanels).Anchor = (AnchorStyles)15;
		((Control)lbPanels).BackColor = Color.White;
		lbPanels.BorderStyle = (BorderStyle)0;
		lbPanels.IntegralHeight = false;
		((Control)lbPanels).Location = new Point(12, 12);
		((Control)lbPanels).Name = "lbPanels";
		((Control)lbPanels).Size = new Size(368, 309);
		((Control)lbPanels).TabIndex = 0;
		((Control)lbPanels).DoubleClick += This_OkClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 336);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(392, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(392, 382);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)lbPanels);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiInteractionDrillDownPageSelectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select page";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
