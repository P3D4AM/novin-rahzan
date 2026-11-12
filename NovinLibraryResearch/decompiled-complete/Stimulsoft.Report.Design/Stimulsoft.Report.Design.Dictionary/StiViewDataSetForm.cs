using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Editor;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiViewDataSetForm : StiFormUX
{
	private DataSet dataSet;

	private IContainer components;

	private SyntaxEdit syntaxEdit1;

	private SyntaxEdit syntaxEdit2;

	private ToolTip toolTip1;

	private ImageList ilButtons;

	private TabControl tabDataView;

	public StiViewDataSetForm(DataSet dataSet)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = false;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoTheming();
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			this.dataSet = dataSet;
			((Form)this).ShowIcon = false;
		}
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)tabDataView).BackColor = StiUX.ItemBackground;
			((Control)tabDataView).ForeColor = StiUX.ItemForeground;
		}
	}

	private void Build()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		foreach (DataTable table in dataSet.Tables)
		{
			TabItem tabItem = new TabItem
			{
				Text = table.TableName
			};
			tabDataView.Tabs.Add(tabItem);
			TabControlPanel tabControlPanel = new TabControlPanel();
			((Control)tabControlPanel).Dock = (DockStyle)5;
			((Control)tabControlPanel).Padding = new Padding(1);
			((Control)tabControlPanel).Size = new Size(SystemInformation.WorkingArea.Width, SystemInformation.WorkingArea.Height);
			tabControlPanel.TabItem = tabItem;
			TabControlPanel tabControlPanel2 = tabControlPanel;
			DataGridView val = new DataGridView
			{
				BorderStyle = (BorderStyle)0,
				ReadOnly = true,
				Dock = (DockStyle)5,
				DataSource = table,
				ColumnHeadersHeight = StiScale.YYI(23.0)
			};
			if (StiUX.IsDark)
			{
				val.BackgroundColor = StiUX.ItemBackground;
				((Control)val).ForeColor = StiUX.ItemForeground;
				val.EnableHeadersVisualStyles = false;
				val.ColumnHeadersDefaultCellStyle.BackColor = StiUX.ButtonBackground;
				val.ColumnHeadersDefaultCellStyle.ForeColor = StiUX.ButtonForeground;
				val.RowHeadersDefaultCellStyle.BackColor = StiUX.ButtonBackground;
				val.RowHeadersDefaultCellStyle.ForeColor = StiUX.ButtonForeground;
				val.DefaultCellStyle.BackColor = StiUX.ItemBackground;
				val.DefaultCellStyle.ForeColor = StiUX.ItemForeground;
				val.BorderStyle = (BorderStyle)0;
				tabControlPanel2.BorderStyle = (BorderStyle)0;
				((Control)tabControlPanel2).BackColor = StiUX.ButtonBackground;
				((Control)tabControlPanel2).ForeColor = StiUX.ButtonForeground;
				tabDataView.ColorScheme.TabBackground = StiUX.ButtonBackground;
				tabDataView.ColorScheme.TabItemText = StiUX.ButtonForeground;
				tabDataView.ColorScheme.TabItemHotText = StiUX.ButtonForeground;
			}
			((Control)tabControlPanel2).Controls.Add((Control)(object)val);
			tabItem.AttachedControl = (Control)(object)tabControlPanel2;
			((Control)tabDataView).Controls.Add((Control)(object)tabControlPanel2);
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "ViewDataForm");
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

	private void This_Load(object sender, EventArgs e)
	{
		Build();
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
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiViewDataSetForm));
		ilButtons = new ImageList(components);
		syntaxEdit1 = new SyntaxEdit(components);
		syntaxEdit2 = new SyntaxEdit(components);
		toolTip1 = new ToolTip(components);
		tabDataView = new TabControl();
		((ISupportInitialize)tabDataView).BeginInit();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		ilButtons.Images.SetKeyName(1, "");
		((Control)syntaxEdit1).BackColor = SystemColors.Window;
		((Control)syntaxEdit1).Cursor = Cursors.IBeam;
		((Control)syntaxEdit1).Font = new Font("Courier New", 10f);
		((Control)syntaxEdit1).Location = new Point(0, 0);
		((Control)syntaxEdit1).Name = "syntaxEdit1";
		((Control)syntaxEdit1).Size = new Size(100, 96);
		((Control)syntaxEdit1).TabIndex = 0;
		((Control)syntaxEdit1).Text = "";
		syntaxEdit1.UseDefaultMenu = false;
		((Control)syntaxEdit2).BackColor = SystemColors.Window;
		((Control)syntaxEdit2).Cursor = Cursors.IBeam;
		((Control)syntaxEdit2).Font = new Font("Courier New", 10f);
		((Control)syntaxEdit2).Location = new Point(0, 0);
		((Control)syntaxEdit2).Name = "syntaxEdit2";
		((Control)syntaxEdit2).Size = new Size(100, 96);
		((Control)syntaxEdit2).TabIndex = 0;
		((Control)syntaxEdit2).Text = "";
		syntaxEdit2.UseDefaultMenu = false;
		((Control)tabDataView).Anchor = (AnchorStyles)15;
		((Control)tabDataView).BackColor = Color.White;
		tabDataView.CanReorderTabs = false;
		((Control)tabDataView).Location = new Point(24, 24);
		((Control)tabDataView).Margin = new Padding(6, 6, 6, 6);
		((Control)tabDataView).Name = "tabDataView";
		tabDataView.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		tabDataView.SelectedTabIndex = -1;
		((Control)tabDataView).Size = new Size(926, 852);
		tabDataView.Style = eTabStripStyle.Metro;
		tabDataView.TabAlignment = eTabStripAlignment.Bottom;
		((Control)tabDataView).TabIndex = 27;
		tabDataView.TabLayoutType = eTabLayoutType.MultilineWithNavigationBox;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(976, 902);
		((Control)this).Controls.Add((Control)(object)tabDataView);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Control)this).Name = "StiViewDataSetForm";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiViewDataForm";
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)tabDataView).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
