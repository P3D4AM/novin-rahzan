using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiCrossTabDataSourceEditForm : StiFormUX
{
	private IContainer components;

	private string oldValue;

	private StiCrossTabDataSource dataSource;

	private StiDictionary dictionary;

	private StiDataAdapterService adapter;

	private StiReport report;

	private Hashtable crossTabs;

	private ErrorProvider errorProvider;

	private StiTextBox tbName;

	private StiTextBox tbAlias;

	private StiLabelUX lbName;

	private StiLabelUX lbAlias;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private StiTreeViewUX tvCrossTabSource;

	private StiNodesMemory NodesMemory = new StiNodesMemory();

	private StiButtonPanelUX stiGroupLine1;

	private StiGroupPanelUX stiGroupLine2;

	private PictureBox imageCrossTabIsNotFound;

	private StiLabelUX labelCrossTabIsNotFound;

	public StiCrossTabDataSourceEditForm(StiCrossTabAdapterService adapter, StiDictionary dictionary, StiCrossTabDataSource dataSource)
		: this(adapter, dictionary, dataSource, newForm: false)
	{
	}

	public StiCrossTabDataSourceEditForm(StiCrossTabAdapterService adapter, StiDictionary dictionary, StiCrossTabDataSource dataSource, bool newForm)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			report = dictionary.Report;
			this.dictionary = dictionary;
			this.adapter = adapter;
			this.dataSource = dataSource;
			((Control)tbName).Text = dataSource.Name;
			((Control)tbAlias).Text = dataSource.Alias;
			Localize();
			DoScaling();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			BuildDataSources(newForm);
			((Control)tbName).Focus();
			((Control)labelCrossTabIsNotFound).Left = (((Control)this).Width - ((Control)labelCrossTabIsNotFound).Width) / 2;
			((Control)imageCrossTabIsNotFound).Left = (((Control)this).Width - ((Control)imageCrossTabIsNotFound).Width) / 2;
		}
	}

	private void Localize()
	{
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelCrossTabIsNotFound).Text = string.Format(Loc.Get("Notices", "IsNotFound"), Loc.Get("Components", "StiCrossTab"));
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void BuildDataSources(bool newForm)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		IEnumerable<StiComponent> enumerable = from c in dictionary.Report.Designer.ComponentsOnReport.ToList()
			where c is StiCrossTab
			select c;
		if (!enumerable.Any())
		{
			_003F val = imageCrossTabIsNotFound;
			Image obj = StiReportDesignImages.Components.CrossTab(StiImageSize.Double);
			((PictureBox)val).Image = StiImageResizer.GetImage((Image)(object)StiImageUtils.ConvertToDisabled((Bitmap)(object)((obj is Bitmap) ? obj : null)));
			((Control)imageCrossTabIsNotFound).Visible = true;
			((Control)labelCrossTabIsNotFound).Visible = true;
			((Control)tvCrossTabSource).Visible = false;
			return;
		}
		((Control)imageCrossTabIsNotFound).Visible = false;
		((Control)labelCrossTabIsNotFound).Visible = false;
		((Control)tvCrossTabSource).Visible = true;
		ImageList val2 = new ImageList
		{
			ImageSize = new Size(StiScale.I16, StiScale.I16),
			ColorDepth = (ColorDepth)32
		};
		val2.Images.Add("Category", StiReportImages.Dictionary.Category());
		val2.Images.Add("CrossTab", StiReportDesignImages.Components.CrossTab());
		((TreeView)tvCrossTabSource).ImageList = val2;
		string text = "Category";
		string text2 = "CrossTab";
		crossTabs = new Hashtable();
		TreeNode val3 = new TreeNode("Cross-Tabs");
		string imageKey = (val3.SelectedImageKey = text);
		val3.ImageKey = imageKey;
		TreeNode selectedNode = null;
		foreach (StiComponent item in enumerable)
		{
			TreeNode val4 = new TreeNode(item.Name);
			imageKey = (val4.SelectedImageKey = text2);
			val4.ImageKey = imageKey;
			val3.Nodes.Add(val4);
			crossTabs.Add(item.Name, item);
			if (!newForm && item.Name == dataSource.NameInSource)
			{
				selectedNode = val4;
			}
		}
		((TreeView)tvCrossTabSource).Nodes.Add(val3);
		((TreeView)tvCrossTabSource).ExpandAll();
		((TreeView)tvCrossTabSource).SelectedNode = selectedNode;
	}

	private void SaveParam(ref StiCrossTabDataSource dataSourceWithParam)
	{
		if (dataSourceWithParam == null)
		{
			dataSourceWithParam = StiActivator.CreateObject(dataSource.GetType()) as StiCrossTabDataSource;
		}
		dataSourceWithParam.Name = ((Control)tbName).Text;
		dataSourceWithParam.Alias = ((Control)tbAlias).Text;
		if (((TreeView)tvCrossTabSource).SelectedNode != null && crossTabs.ContainsKey(((TreeView)tvCrossTabSource).SelectedNode.Text))
		{
			dataSourceWithParam.NameInSource = ((TreeView)tvCrossTabSource).SelectedNode.Text;
			dataSourceWithParam.BuildColumnsForDictionaryPanel((StiCrossTab)crossTabs[((TreeView)tvCrossTabSource).SelectedNode.Text]);
		}
		else
		{
			dataSourceWithParam.NameInSource = string.Empty;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources_data_from_cross-tab.htm");
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

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void BtOk_Click(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)tbAlias).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Alias")));
			flag = false;
			((Control)tbAlias).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Empty);
		}
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Name")));
			flag = false;
			((Control)tbName).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbName, string.Empty);
		}
		if (flag)
		{
			SaveParam(ref dataSource);
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void TbName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void TbName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
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
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected O, but got Unknown
		components = new Container();
		errorProvider = new ErrorProvider(components);
		tbName = new StiTextBox();
		lbName = new StiLabelUX();
		lbAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		tvCrossTabSource = new StiTreeViewUX();
		btCancel = new StiButtonUX();
		btOk = new StiButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		stiGroupLine2 = new StiGroupPanelUX();
		imageCrossTabIsNotFound = new PictureBox();
		labelCrossTabIsNotFound = new StiLabelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)stiGroupLine2).SuspendLayout();
		((ISupportInitialize)imageCrossTabIsNotFound).BeginInit();
		((Control)this).SuspendLayout();
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).BackColor = SystemColors.Window;
		((Control)tbName).Location = new Point(138, 12);
		((Control)tbName).Margin = new Padding(2);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(319, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TbName_Enter;
		((Control)tbName).Leave += TbName_Leave;
		((Control)lbName).ForeColor = Color.DimGray;
		((Control)lbName).Location = new Point(12, 12);
		((Control)lbName).Margin = new Padding(2, 0, 2, 0);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(120, 20);
		((Control)lbName).TabIndex = 13;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)lbAlias).ForeColor = Color.DimGray;
		((Control)lbAlias).Location = new Point(12, 45);
		((Control)lbAlias).Margin = new Padding(2, 0, 2, 0);
		((Control)lbAlias).Name = "lbAlias";
		((Control)lbAlias).Size = new Size(120, 18);
		((Control)lbAlias).TabIndex = 14;
		((Control)lbAlias).Text = "Alias";
		((Label)lbAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).BackColor = SystemColors.Window;
		((Control)tbAlias).Location = new Point(138, 45);
		((Control)tbAlias).Margin = new Padding(2);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(319, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tvCrossTabSource).Anchor = (AnchorStyles)15;
		((TreeView)tvCrossTabSource).BorderStyle = (BorderStyle)0;
		((TreeView)tvCrossTabSource).HideSelection = false;
		((Control)tvCrossTabSource).Location = new Point(12, 12);
		((Control)tvCrossTabSource).Margin = new Padding(2);
		((Control)tvCrossTabSource).Name = "tvCrossTabSource";
		((Control)tvCrossTabSource).Size = new Size(445, 158);
		((Control)tvCrossTabSource).TabIndex = 2;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(382, 12);
		((Control)btCancel).Margin = new Padding(2);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(303, 12);
		((Control)btOk).Margin = new Padding(2);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += BtOk_Click;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 265);
		((Control)stiGroupLine1).Margin = new Padding(2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(469, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Control)stiGroupLine2).BackColor = Color.White;
		((Control)stiGroupLine2).Controls.Add((Control)(object)labelCrossTabIsNotFound);
		((Control)stiGroupLine2).Controls.Add((Control)(object)imageCrossTabIsNotFound);
		((Control)stiGroupLine2).Controls.Add((Control)(object)tvCrossTabSource);
		((Control)stiGroupLine2).Dock = (DockStyle)2;
		stiGroupLine2.LinePosition = StiLinePosition.Top;
		stiGroupLine2.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine2).Location = new Point(0, 81);
		((Control)stiGroupLine2).Margin = new Padding(2);
		((Control)stiGroupLine2).Name = "stiGroupLine2";
		((Control)stiGroupLine2).Size = new Size(469, 184);
		((Control)stiGroupLine2).TabIndex = 15;
		((Control)imageCrossTabIsNotFound).Anchor = (AnchorStyles)1;
		((Control)imageCrossTabIsNotFound).Location = new Point(208, 50);
		((Control)imageCrossTabIsNotFound).Name = "imageCrossTabIsNotFound";
		((Control)imageCrossTabIsNotFound).Size = new Size(60, 60);
		imageCrossTabIsNotFound.SizeMode = (PictureBoxSizeMode)2;
		imageCrossTabIsNotFound.TabIndex = 5;
		imageCrossTabIsNotFound.TabStop = false;
		((Control)labelCrossTabIsNotFound).Anchor = (AnchorStyles)1;
		((Control)labelCrossTabIsNotFound).AutoSize = true;
		((Control)labelCrossTabIsNotFound).ForeColor = Color.DimGray;
		((Control)labelCrossTabIsNotFound).Location = new Point(179, 120);
		((Control)labelCrossTabIsNotFound).Name = "labelCrossTabIsNotFound";
		((Control)labelCrossTabIsNotFound).Size = new Size(116, 13);
		((Control)labelCrossTabIsNotFound).TabIndex = 6;
		((Control)labelCrossTabIsNotFound).Text = "Cross-Tab is not found!";
		((Label)labelCrossTabIsNotFound).TextAlign = (ContentAlignment)32;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(469, 311);
		((Control)this).Controls.Add((Control)(object)stiGroupLine2);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)lbAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)lbName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(2);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCrossTabDataSourceEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit Data Source";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)stiGroupLine2).ResumeLayout(false);
		((Control)stiGroupLine2).PerformLayout();
		((ISupportInitialize)imageCrossTabIsNotFound).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
