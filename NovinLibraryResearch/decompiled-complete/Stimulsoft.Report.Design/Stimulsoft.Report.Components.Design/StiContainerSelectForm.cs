using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Components.Design;

public class StiContainerSelectForm : Form
{
	private StiComponent component;

	private List<StiContainer> datas;

	private IContainer components;

	private ImageList ilData;

	private Button btOk;

	private Button btCancel;

	private StiListBoxV2 lbPanels;

	private StiGroupLine stiGroupLine1;

	public StiContainerSelectForm(StiComponent component)
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
			this.component = component;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "ContainerSelectForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void ThisLoad(object sender, EventArgs e)
	{
		StiComponentsCollection stiComponentsCollection = component.Page.Report.GetComponents();
		((ListBox)lbPanels).Items.Clear();
		((ListBox)lbPanels).Items.Add((object)("[" + StiLocalization.Get("Report", "NotAssigned") + "]"));
		((ListControl)lbPanels).SelectedIndex = 0;
		datas = new List<StiContainer>();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (!(item is StiContainer stiContainer) || item == component || item is StiBand || item is StiPage)
			{
				continue;
			}
			StiContainer parent = component.Parent;
			while (parent != null && parent != stiContainer)
			{
				parent = parent.Parent;
			}
			if (parent == null)
			{
				datas.Add(stiContainer);
				((ListBox)lbPanels).Items.Add((object)item.Name);
				if (item == ((IStiClone)component).Container)
				{
					((ListControl)lbPanels).SelectedIndex = ((ListBox)lbPanels).Items.Count - 1;
				}
			}
		}
	}

	private void tbCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void tbOk_Click(object sender, EventArgs e)
	{
		if (((ListControl)lbPanels).SelectedIndex > 0)
		{
			((IStiClone)component).Container = datas[((ListControl)lbPanels).SelectedIndex - 1];
		}
		else
		{
			((IStiClone)component).Container = null;
		}
		((Form)this).Close();
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_panels_cloning.htm");
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		components = new Container();
		ilData = new ImageList(components);
		lbPanels = new StiListBoxV2();
		btOk = new Button();
		btCancel = new Button();
		stiGroupLine1 = new StiGroupLine();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		ilData.ColorDepth = (ColorDepth)32;
		ilData.ImageSize = new Size(16, 16);
		ilData.TransparentColor = Color.Magenta;
		((Control)lbPanels).Anchor = (AnchorStyles)15;
		((Control)lbPanels).BackColor = SystemColors.Window;
		((Control)lbPanels).Location = new Point(12, 12);
		((Control)lbPanels).Name = "lbPanels";
		((Control)lbPanels).Size = new Size(368, 308);
		((Control)lbPanels).TabIndex = 0;
		((Control)lbPanels).DoubleClick += tbOk_Click;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(224, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += tbOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(305, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += tbCancel_Click;
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
		((Control)this).Name = "StiContainerSelectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select container for clone";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Form)this).Load += ThisLoad;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
