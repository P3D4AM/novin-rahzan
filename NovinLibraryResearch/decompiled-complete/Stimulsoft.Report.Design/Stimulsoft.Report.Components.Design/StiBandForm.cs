using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.Design;

public class StiBandForm : StiFormUX
{
	private IContainer components;

	private StiDataSelectionControl DataSelection;

	private StiButtonPanelUX panelButtons;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	public StiBandForm(StiComponent component, StiDataSelectionControl.StartTab startTab)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			DoScaling();
			components = null;
			DataSelection.Component = component;
			DataSelection.TabulatorTabIndex = startTab;
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiDataBand");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		DataSelection.DoScaling();
	}

	private void This_Click(object sender, EventArgs e)
	{
		DataSelection.SaveData();
		((Form)this).Close();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_creating_lists_data_source_of_data_band.htm");
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
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		panelButtons = new StiButtonPanelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		DataSelection = new StiDataSelectionControl();
		((Control)panelButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panelButtons).Controls.Add((Control)(object)btOk);
		((Control)panelButtons).Controls.Add((Control)(object)btCancel);
		((Control)panelButtons).Dock = (DockStyle)2;
		panelButtons.LinePosition = StiLinePosition.Top;
		panelButtons.LineStyle = StiLineStyle.Dot;
		((Control)panelButtons).Location = new Point(0, 433);
		((Control)panelButtons).Name = "stiGroupLine1";
		((Control)panelButtons).Size = new Size(684, 48);
		((Control)panelButtons).TabIndex = 0;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(512, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 4;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(598, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 5;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)DataSelection).Anchor = (AnchorStyles)15;
		DataSelection.Component = null;
		((Control)DataSelection).Location = new Point(0, 0);
		((Control)DataSelection).Name = "DataSelection";
		((Control)DataSelection).Size = new Size(682, 433);
		((Control)DataSelection).TabIndex = 1;
		DataSelection.TabulatorTabIndex = StiDataSelectionControl.StartTab.DataSource;
		DataSelection.Close += This_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(684, 481);
		((Control)this).Controls.Add((Control)(object)panelButtons);
		((Control)this).Controls.Add((Control)(object)DataSelection);
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(656, 405);
		((Control)this).Name = "StiBandForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiBandForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)panelButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
