using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataControlForm : StiFormUX
{
	private StiDataAdapterService adapter;

	private IContainer components;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	public StiDataControl dataControl;

	private StiButtonPanelUX stiGroupLine1;

	public string NameInSource { get; private set; }

	public string DataSourceName { get; private set; }

	public string DataSourceAlias { get; private set; }

	public StiDataControlForm(StiDictionary dict, StiDataAdapterService adapter, string nameInSource)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.adapter = adapter;
			NameInSource = nameInSource;
			Build(dict, adapter, nameInSource);
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			Localize();
			((Control)dataControl.tvData).DoubleClick += This_OkClick;
			((Form)this).ShowIcon = false;
		}
	}

	private void Build(StiDictionary dict, StiDataAdapterService adapter, string nameInSource)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		dataControl.Build(dict.DataStore, adapter);
		((TreeView)dataControl.tvData).ExpandAll();
		if (((TreeView)dataControl.tvData).Nodes.Count <= 0)
		{
			return;
		}
		foreach (StiData item in dict.DataStore)
		{
			if (item.Name.ToLowerInvariant() != nameInSource.ToLowerInvariant())
			{
				continue;
			}
			{
				foreach (TreeNode node in ((TreeView)dataControl.tvData).Nodes[0].Nodes)
				{
					TreeNode val = node;
					if (val.Tag == item)
					{
						((TreeView)dataControl.tvData).SelectedNode = val;
						break;
					}
				}
				break;
			}
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "NameInSource");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((TreeView)dataControl.tvData).SelectedNode != null && ((TreeView)dataControl.tvData).SelectedNode.Tag != null && ((TreeView)dataControl.tvData).SelectedNode.Tag is StiData)
		{
			StiData data = ((TreeView)dataControl.tvData).SelectedNode.Tag as StiData;
			StiDataTableSource stiDataTableSource = new StiDataTableSource();
			adapter.SetDataSourceNames(data, stiDataTableSource);
			NameInSource = stiDataTableSource.NameInSource;
			DataSourceName = stiDataTableSource.Name;
			DataSourceAlias = stiDataTableSource.Alias;
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
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
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		dataControl = new StiDataControl();
		stiGroupLine1 = new StiButtonPanelUX();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(281, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 6;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(200, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 5;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)dataControl).Anchor = (AnchorStyles)15;
		((Control)dataControl).Location = new Point(12, 12);
		((Control)dataControl).Name = "dataControl";
		((Control)dataControl).Size = new Size(344, 383);
		((Control)dataControl).TabIndex = 0;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 410);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(368, 46);
		((Control)stiGroupLine1).TabIndex = 7;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(368, 456);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)dataControl);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataControlForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiDataControlForm";
		((Form)this).Closed += This_Closed;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
