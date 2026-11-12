using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design;

public class StiReportDataSourcesEditorForm : Form
{
	private IReferenceService service;

	private bool lockUpdate;

	private StiReport report;

	private IContainer components;

	private TreeView tvDataSources;

	private Label labelInfo;

	private ImageList ilReportDataSources;

	private Button btCancel;

	private Button btOk;

	private StiGroupLine glButtons;

	public StiReport Report => report;

	public StiReportDataSourcesEditorForm(IReferenceService service, StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.report = report;
			this.service = service;
			Build();
			((Form)this).ShowIcon = false;
		}
	}

	private bool CheckDataSource(string name)
	{
		foreach (StiReportDataSource reportDataSource in report.ReportDataSources)
		{
			if (name == reportDataSource.Name)
			{
				return true;
			}
		}
		return false;
	}

	private void Build()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		tvDataSources.Nodes.Clear();
		if (service != null)
		{
			object[] references = service.GetReferences();
			object[] array = references;
			foreach (object obj in array)
			{
				if (!(obj is StiReportDataSource) && !(obj is StiReport))
				{
					DataSet dataSet = obj as DataSet;
					IComponent component = obj as IComponent;
					if (dataSet != null && dataSet.Site != null)
					{
						string dataSetName = dataSet.DataSetName;
						TreeNode val = new TreeNode(dataSetName);
						int selectedImageIndex = (val.ImageIndex = 1);
						val.SelectedImageIndex = selectedImageIndex;
						val.Checked = CheckDataSource(dataSetName);
						val.Tag = dataSet;
						tvDataSources.Nodes.Add(val);
					}
					else if (component != null && component.Site != null)
					{
						string name = component.Site.Name;
						TreeNode val2 = new TreeNode(name);
						int selectedImageIndex = (val2.ImageIndex = 2);
						val2.SelectedImageIndex = selectedImageIndex;
						val2.Tag = component;
						val2.Checked = CheckDataSource(name);
						tvDataSources.Nodes.Add(val2);
					}
				}
			}
		}
		tvDataSources.ExpandAll();
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		report.ReportDataSources.Clear();
		foreach (TreeNode node in tvDataSources.Nodes)
		{
			TreeNode val = node;
			if (val.Checked)
			{
				StiReportDataSource stiReportDataSource = null;
				IDesignerHost designerHost = (IDesignerHost)report.Site.GetService(typeof(IDesignerHost));
				if (designerHost == null)
				{
					MessageBox.Show("IDesignerHost not finded");
				}
				else
				{
					stiReportDataSource = designerHost.CreateComponent(typeof(StiReportDataSource)) as StiReportDataSource;
					stiReportDataSource.Name = val.Text;
					stiReportDataSource.Item = val.Tag;
				}
				if (stiReportDataSource == null)
				{
					stiReportDataSource = new StiReportDataSource(val.Text, val.Tag);
				}
				report.ReportDataSources.Add(stiReportDataSource);
			}
		}
		((Form)this).Close();
	}

	private void tvDataSources_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		if (lockUpdate)
		{
			return;
		}
		if (e.Node != null && e.Node.Tag is DataTable && e.Node.Parent != null)
		{
			lockUpdate = true;
			e.Node.Parent.Checked = false;
			lockUpdate = false;
			return;
		}
		lockUpdate = true;
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			val.Checked = e.Node.Checked;
		}
		lockUpdate = false;
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiReportDataSourcesEditorForm));
		tvDataSources = new TreeView();
		ilReportDataSources = new ImageList(components);
		labelInfo = new Label();
		btCancel = new Button();
		btOk = new Button();
		glButtons = new StiGroupLine();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tvDataSources).Anchor = (AnchorStyles)15;
		tvDataSources.BorderStyle = (BorderStyle)0;
		tvDataSources.CheckBoxes = true;
		tvDataSources.HideSelection = false;
		tvDataSources.ImageIndex = 0;
		tvDataSources.ImageList = ilReportDataSources;
		((Control)tvDataSources).Location = new Point(24, 57);
		((Control)tvDataSources).Name = "tvDataSources";
		tvDataSources.SelectedImageIndex = 0;
		((Control)tvDataSources).Size = new Size(1001, 609);
		((Control)tvDataSources).TabIndex = 0;
		tvDataSources.AfterCheck += new TreeViewEventHandler(tvDataSources_AfterCheck);
		ilReportDataSources.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilReportDataSources.ImageStream");
		ilReportDataSources.TransparentColor = Color.Magenta;
		ilReportDataSources.Images.SetKeyName(0, "");
		ilReportDataSources.Images.SetKeyName(1, "");
		ilReportDataSources.Images.SetKeyName(2, "");
		((Control)labelInfo).ForeColor = Color.DimGray;
		((Control)labelInfo).Location = new Point(24, 22);
		((Control)labelInfo).Name = "labelInfo";
		((Control)labelInfo).Size = new Size(1001, 30);
		((Control)labelInfo).TabIndex = 1;
		((Control)labelInfo).Text = "Choose the data sources which are required for report creation";
		labelInfo.TextAlign = (ContentAlignment)16;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(875, 22);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(150, 44);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(713, 22);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(150, 44);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 693);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(1051, 89);
		((Control)glButtons).TabIndex = 2;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(1051, 782);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)labelInfo);
		((Control)this).Controls.Add((Control)(object)tvDataSources);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiReportDataSourcesEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Report DataSources";
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
