using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiGridColumnsForm : Form
{
	private StiGridControl gridControl;

	private IContainer components;

	private ImageList ilButtons;

	private TreeView tvColumns;

	private StiToolButton tbColumnDelete;

	private StiToolButton tbColumnNew;

	private ToolTip toolTip1;

	public StiToolBar tbTool;

	private StiReportPropertyGrid pgColumns;

	private StiToolButton tbAddAllColumns;

	public StiGridColumnsForm(StiGridControl gridControl)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.gridControl = gridControl;
			Build();
			Localize();
			pgColumns.SetStandardPropertyTabs();
			if (tvColumns.Nodes.Count > 0)
			{
				tvColumns.SelectedNode = tvColumns.Nodes[0];
			}
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			int @int = StiSettings.GetInt("StiGridColumnsForm", "Spl");
			if (@int != -1)
			{
				((Control)pgColumns).Width = @int;
			}
			((Form)this).ShowIcon = false;
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Columns");
		((Control)tbAddAllColumns).Text = StiLocalization.Get("Buttons", "AddAllColumns");
		toolTip1.SetToolTip((Control)(object)tbColumnNew, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ColumnNew")));
		toolTip1.SetToolTip((Control)(object)tbColumnDelete, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Delete")));
	}

	private void DoScaling()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		((ButtonBase)tbColumnNew).Image = StiImageResizer.GetImage(((ButtonBase)tbColumnNew).Image);
		((ButtonBase)tbColumnDelete).Image = StiReportDesignImages.Actions.Remove();
		ImageList val = new ImageList();
		val.ImageSize = StiScale.I(new Size(16, 16));
		val.ColorDepth = (ColorDepth)32;
		val.Images.Add(StiImageResizer.GetImage(StiDataBuilder.Images.Images["DataColumn"], allowDispose: false));
		tvColumns.ImageList = val;
	}

	private void Build()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		StiGridColumn stiGridColumn = null;
		if (tvColumns.SelectedNode != null)
		{
			stiGridColumn = tvColumns.SelectedNode.Tag as StiGridColumn;
		}
		tvColumns.Nodes.Clear();
		foreach (StiGridColumn column in gridControl.Columns)
		{
			TreeNode val = new TreeNode(column.ToString(), 0, 0)
			{
				Tag = column
			};
			tvColumns.Nodes.Add(val);
			if (column == stiGridColumn)
			{
				tvColumns.SelectedNode = val;
			}
		}
	}

	private void TreeViewColumns_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (tvColumns.SelectedNode != null)
		{
			((PropertyGrid)pgColumns).SelectedObject = tvColumns.SelectedNode.Tag;
		}
	}

	private void ButtonColumnNew_Click(object sender, EventArgs e)
	{
		gridControl.Columns.Add(new StiGridColumn());
		Build();
		tvColumns.SelectedNode = tvColumns.Nodes[tvColumns.Nodes.Count - 1];
	}

	private void ButtonColumnDelete_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		if ((int)MessageBox.Show(StiLocalization.Get("Questions", "qnRemove"), StiLocalization.Get("FormDictionaryDesigner", "title"), (MessageBoxButtons)4) != 6)
		{
			return;
		}
		foreach (TreeNode node in tvColumns.Nodes)
		{
			TreeNode val = node;
			if (val.Checked)
			{
				gridControl.Columns.Remove(val.Tag as StiGridColumn);
			}
		}
		Build();
	}

	private void ButtonAddAllColumns_Click(object sender, EventArgs e)
	{
		if (gridControl.DataSource == null)
		{
			return;
		}
		foreach (StiDataColumn column2 in gridControl.DataSource.Columns)
		{
			StiGridColumn column = new StiGridColumn
			{
				DataTextField = gridControl.DataSourceName + "." + column2.Name,
				HeaderText = column2.Alias
			};
			gridControl.Columns.Add(column);
		}
		Build();
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("StiGridColumnsForm", "Spl", ((Control)pgColumns).Width);
		StiSettings.Save();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).Close();
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGridColumnsForm));
		ilButtons = new ImageList(components);
		tvColumns = new TreeView();
		tbColumnDelete = new StiToolButton();
		tbColumnNew = new StiToolButton();
		toolTip1 = new ToolTip(components);
		tbAddAllColumns = new StiToolButton();
		tbTool = new StiToolBar();
		pgColumns = new StiReportPropertyGrid();
		((Control)tbTool).SuspendLayout();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		ilButtons.Images.SetKeyName(2, "");
		((Control)tvColumns).Anchor = (AnchorStyles)15;
		tvColumns.CheckBoxes = true;
		tvColumns.HideSelection = false;
		tvColumns.ImageIndex = 0;
		tvColumns.ImageList = ilButtons;
		((Control)tvColumns).Location = new Point(24, 77);
		((Control)tvColumns).Margin = new Padding(6, 6, 6, 6);
		((Control)tvColumns).Name = "tvColumns";
		tvColumns.SelectedImageIndex = 0;
		tvColumns.ShowLines = false;
		tvColumns.ShowPlusMinus = false;
		((Control)tvColumns).Size = new Size(524, 800);
		((Control)tvColumns).TabIndex = 0;
		tvColumns.AfterSelect += new TreeViewEventHandler(TreeViewColumns_AfterSelect);
		((Control)tbColumnDelete).Dock = (DockStyle)3;
		tbColumnDelete.Draw3DButton = false;
		tbColumnDelete.DropDownMenu = null;
		tbColumnDelete.HotDragDrop = false;
		((ButtonBase)tbColumnDelete).ImageIndex = 2;
		((ButtonBase)tbColumnDelete).ImageList = ilButtons;
		((Control)tbColumnDelete).Location = new Point(62, 6);
		((Control)tbColumnDelete).Margin = new Padding(6, 6, 6, 6);
		((Control)tbColumnDelete).Name = "tbColumnDelete";
		((Control)tbColumnDelete).Size = new Size(50, 48);
		((Control)tbColumnDelete).TabIndex = 3;
		((ButtonBase)tbColumnDelete).UseVisualStyleBackColor = false;
		((Control)tbColumnDelete).Click += ButtonColumnDelete_Click;
		((Control)tbColumnNew).Dock = (DockStyle)3;
		tbColumnNew.Draw3DButton = false;
		tbColumnNew.DropDownMenu = null;
		tbColumnNew.HotDragDrop = false;
		((ButtonBase)tbColumnNew).ImageIndex = 1;
		((ButtonBase)tbColumnNew).ImageList = ilButtons;
		((Control)tbColumnNew).Location = new Point(12, 6);
		((Control)tbColumnNew).Margin = new Padding(6, 6, 6, 6);
		((Control)tbColumnNew).Name = "tbColumnNew";
		((Control)tbColumnNew).Size = new Size(50, 48);
		((Control)tbColumnNew).TabIndex = 2;
		((ButtonBase)tbColumnNew).UseVisualStyleBackColor = false;
		((Control)tbColumnNew).Click += ButtonColumnNew_Click;
		((Control)tbAddAllColumns).Dock = (DockStyle)3;
		tbAddAllColumns.Draw3DButton = false;
		tbAddAllColumns.DropDownMenu = null;
		tbAddAllColumns.HotDragDrop = false;
		((Control)tbAddAllColumns).Location = new Point(112, 6);
		((Control)tbAddAllColumns).Margin = new Padding(6, 6, 6, 6);
		((Control)tbAddAllColumns).Name = "tbAddAllColumns";
		((Control)tbAddAllColumns).Size = new Size(224, 48);
		((Control)tbAddAllColumns).TabIndex = 3;
		((Control)tbAddAllColumns).Text = "Add All Columns";
		((ButtonBase)tbAddAllColumns).UseVisualStyleBackColor = false;
		((Control)tbAddAllColumns).Click += ButtonAddAllColumns_Click;
		((Control)tbTool).Controls.Add((Control)(object)tbAddAllColumns);
		((Control)tbTool).Controls.Add((Control)(object)tbColumnDelete);
		((Control)tbTool).Controls.Add((Control)(object)tbColumnNew);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Margin = new Padding(6, 6, 6, 6);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(12, 6, 6, 8);
		((Control)tbTool).Size = new Size(998, 62);
		((Control)tbTool).TabIndex = 52;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)pgColumns).Anchor = (AnchorStyles)11;
		((PropertyGrid)pgColumns).CategoryForeColor = SystemColors.InactiveCaptionText;
		((PropertyGrid)pgColumns).HelpBackColor = Color.White;
		((PropertyGrid)pgColumns).LineColor = SystemColors.ScrollBar;
		((Control)pgColumns).Location = new Point(574, 77);
		((Control)pgColumns).Margin = new Padding(6, 6, 6, 6);
		((Control)pgColumns).Name = "pgColumns";
		pgColumns.ScrollOffset = 0;
		pgColumns.ShowEventsTag = true;
		((Control)pgColumns).Size = new Size(400, 804);
		((Control)pgColumns).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(998, 902);
		((Control)this).Controls.Add((Control)(object)tvColumns);
		((Control)this).Controls.Add((Control)(object)pgColumns);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiGridColumnsForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiGridColumnsForm";
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)tbTool).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
