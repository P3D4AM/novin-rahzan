using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Interaction;

public class StiInteractionEditorForm : Form
{
	public enum StartTab
	{
		PageCollapsed,
		PageCrossHeaderCollapsed,
		PageDrilldown,
		PageDrilldownParameters,
		PageSort,
		PageSelection,
		PageBookmark,
		PageHyperlink,
		PageTag,
		PageTooltip
	}

	private StiComponent comp;

	private StiInteraction interactionStore;

	private StiInteraction interaction;

	private StiBandInteraction bandInteraction;

	private StiCrossHeaderInteraction crossHeaderInteraction;

	private StiInteractionDrilldownControl drillDownControl;

	private StiInteractionDrilldownParametersControl drillDownParametersControl;

	private StiInteractionSortControl sortControl;

	private StiInteractionMainControl commonControl;

	private IContainer components;

	private StiGroupLine footerPanel;

	private Button btOk;

	private Button btCancel;

	private StiCloudTabulator tabControl;

	private StiCloudTabulatorPage pageDrillDown;

	private StiCloudTabulatorPage pageDrillDownParameters;

	private StiCloudTabulatorPage pageSort;

	private StiCloudTabulatorPage pageCommon;

	private Timer timerMarker;

	private bool IsCrossHeaderCollapsingNotEmpty
	{
		get
		{
			if (commonControl == null)
			{
				if (crossHeaderInteraction != null)
				{
					return crossHeaderInteraction.CollapsingEnabled;
				}
				return false;
			}
			return commonControl.CrossHeaderCollapsingEnabledCheckBox.Checked;
		}
	}

	private bool IsCollapsingNotEmpty
	{
		get
		{
			if (commonControl == null)
			{
				if (bandInteraction != null)
				{
					return bandInteraction.CollapsingEnabled;
				}
				return false;
			}
			return commonControl.CollapsingEnabledCheckBox.Checked;
		}
	}

	private bool IsDrillDownNotEmpty
	{
		get
		{
			if (drillDownControl == null)
			{
				return interaction.DrillDownEnabled;
			}
			return drillDownControl.IsNotEmpty;
		}
	}

	private bool IsDrillDownParametersNotEmpty
	{
		get
		{
			if (drillDownParametersControl != null)
			{
				return drillDownParametersControl.IsNotEmpty;
			}
			if (string.IsNullOrWhiteSpace(interaction.DrillDownParameter1.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter2.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter3.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter4.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter5.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter6.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter7.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter8.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter9.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter10.Name) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter1.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter2.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter3.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter4.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter5.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter6.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter7.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter8.Expression.Value) && string.IsNullOrWhiteSpace(interaction.DrillDownParameter9.Expression.Value))
			{
				return !string.IsNullOrWhiteSpace(interaction.DrillDownParameter10.Expression.Value);
			}
			return true;
		}
	}

	private bool IsSelectionNotEmpty
	{
		get
		{
			if (commonControl == null)
			{
				if (bandInteraction != null)
				{
					return bandInteraction.SelectionEnabled;
				}
				return false;
			}
			return commonControl.SelectionEnabledCheckBox.Checked;
		}
	}

	private bool IsSortingColumnNotEmpty
	{
		get
		{
			if (sortControl == null)
			{
				if (!string.IsNullOrWhiteSpace(interaction.SortingColumn))
				{
					return interaction.SortingEnabled;
				}
				return false;
			}
			return sortControl.IsNotEmpty;
		}
	}

	private bool IsBookmarkNotEmpty => !string.IsNullOrEmpty((commonControl != null) ? ((Control)commonControl.BookmarkTextBox).Text : interaction.Bookmark.Value);

	private bool IsHyperlinkNotEmpty => !string.IsNullOrEmpty((commonControl != null) ? ((Control)commonControl.HyperlinkTextBox).Text : interaction.Hyperlink.Value);

	private bool IsToolTipNotEmpty => !string.IsNullOrEmpty((commonControl != null) ? ((Control)commonControl.ToolTipTextBox).Text : interaction.ToolTip.Value);

	private bool IsTagNotEmpty => !string.IsNullOrEmpty((commonControl != null) ? ((Control)commonControl.TagTextBox).Text : interaction.Tag.Value);

	public StiInteractionEditorForm(StiComponent comp, StartTab pageState = StartTab.PageBookmark)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.comp = comp;
			interaction = comp.Interaction;
			bandInteraction = interaction as StiBandInteraction;
			crossHeaderInteraction = interaction as StiCrossHeaderInteraction;
			interactionStore = (StiInteraction)interaction.Clone();
			((Form)this).ShowIcon = false;
			Localize();
			tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
			TabControl_SelectedIndexChanged(null, null);
			DoScaling();
			SelectTabPage(pageState);
		}
	}

	private void SelectTabPage(StartTab pageState)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		Timer timer = new Timer
		{
			Interval = 200
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			switch (pageState)
			{
			case StartTab.PageBookmark:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.BookmarkTextBox).Focus();
				break;
			case StartTab.PageCollapsed:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.CollapsingExpressionTextBox).Focus();
				break;
			case StartTab.PageCrossHeaderCollapsed:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.CrossHeaderCollapsingEnabledCheckBox).Focus();
				break;
			case StartTab.PageDrilldown:
				if (!pageDrillDown.Invisible)
				{
					tabControl.SelectedTab = pageDrillDown;
				}
				break;
			case StartTab.PageDrilldownParameters:
				if (!pageDrillDownParameters.Invisible)
				{
					tabControl.SelectedTab = pageDrillDownParameters;
				}
				break;
			case StartTab.PageHyperlink:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.HyperlinkTextBox).Focus();
				break;
			case StartTab.PageSelection:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.CollapsingEnabledCheckBox).Focus();
				break;
			case StartTab.PageSort:
				if (!pageSort.Invisible)
				{
					tabControl.SelectedTab = pageSort;
				}
				break;
			case StartTab.PageTag:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.TagTextBox).Focus();
				break;
			case StartTab.PageTooltip:
				tabControl.SelectedTab = pageCommon;
				((Control)commonControl.ToolTipTextBox).Focus();
				break;
			}
		};
		timer.Start();
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = tabControl.DockPadding;
		int right = (tabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageCommon.Image = StiReportDesignImages.Tasks.InteractionCommon(StiImageSize.Double);
		pageDrillDown.Image = StiReportDesignImages.Tasks.InteractionDrillDown(StiImageSize.Double);
		pageDrillDownParameters.Image = StiReportDesignImages.Tasks.InteractionDrillDownParameters(StiImageSize.Double);
		pageSort.Image = StiReportDesignImages.Tasks.InteractionSort(StiImageSize.Double);
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("PropertyMain", "Interaction");
		((Control)pageDrillDown).Text = StiLocalization.Get("PropertyMain", "DrillDown");
		((Control)pageDrillDownParameters).Text = StiLocalization.Get("PropertyMain", "DrillDownParameters");
		((Control)pageSort).Text = StiLocalization.Get("PropertyMain", "Sort");
		((Control)pageCommon).Text = StiLocalization.Get("PropertyCategory", "CommonCategory");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok").Replace("&", string.Empty);
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel").Replace("&", string.Empty);
		drillDownControl?.Localize();
		drillDownParametersControl?.Localize();
		sortControl?.Localize();
		commonControl?.Localize();
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl.SelectedTab == pageDrillDown)
		{
			if (drillDownControl == null)
			{
				StiInteractionDrilldownControl stiInteractionDrilldownControl = new StiInteractionDrilldownControl(interaction);
				((Control)stiInteractionDrilldownControl).Dock = (DockStyle)5;
				drillDownControl = stiInteractionDrilldownControl;
				drillDownControl.GalleryDoubleClick += This_OkClick;
				((Control)pageDrillDown).Controls.Add((Control)(object)drillDownControl);
				((Control)drillDownControl.listBoxPanels).Focus();
			}
		}
		else if (tabControl.SelectedTab == pageDrillDownParameters)
		{
			if (drillDownParametersControl == null)
			{
				StiInteractionDrilldownParametersControl stiInteractionDrilldownParametersControl = new StiInteractionDrilldownParametersControl(comp, interaction);
				((Control)stiInteractionDrilldownParametersControl).Dock = (DockStyle)5;
				drillDownParametersControl = stiInteractionDrilldownParametersControl;
				((Control)pageDrillDownParameters).Controls.Add((Control)(object)drillDownParametersControl);
				((Control)drillDownParametersControl.listBoxParameters).Focus();
			}
		}
		else if (tabControl.SelectedTab == pageSort)
		{
			if (sortControl == null)
			{
				StiInteractionSortControl stiInteractionSortControl = new StiInteractionSortControl(comp, interaction);
				((Control)stiInteractionSortControl).Dock = (DockStyle)5;
				sortControl = stiInteractionSortControl;
				((Control)pageSort).Controls.Add((Control)(object)sortControl);
				((Control)sortControl.treeViewDataColumn).Focus();
			}
		}
		else if (tabControl.SelectedTab == pageCommon && commonControl == null)
		{
			StiInteractionMainControl stiInteractionMainControl = new StiInteractionMainControl(comp, interaction);
			((Control)stiInteractionMainControl).Dock = (DockStyle)5;
			commonControl = stiInteractionMainControl;
			((Control)pageCommon).Controls.Add((Control)(object)commonControl);
			((Control)commonControl.HyperlinkTextBox).Focus();
		}
	}

	private void TimerMarker_Tick(object sender, EventArgs e)
	{
		pageDrillDown.ShowMarker = IsDrillDownNotEmpty;
		pageDrillDownParameters.ShowMarker = IsDrillDownParametersNotEmpty;
		pageSort.ShowMarker = IsSortingColumnNotEmpty;
		pageCommon.ShowMarker = IsHyperlinkNotEmpty || IsBookmarkNotEmpty || IsToolTipNotEmpty || IsTagNotEmpty || IsCollapsingNotEmpty || IsSelectionNotEmpty || IsCrossHeaderCollapsingNotEmpty;
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		comp.Report.Designer.UndoRedoSave("Interaction");
		comp.Report.IsModified = true;
		if (drillDownControl != null)
		{
			drillDownControl.Save();
		}
		if (drillDownParametersControl != null)
		{
			drillDownParametersControl.Save();
		}
		if (sortControl != null)
		{
			sortControl.Save();
		}
		if (commonControl != null)
		{
			commonControl.WriteInteraction();
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		comp.Interaction = interactionStore;
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		timerMarker.Stop();
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Expected O, but got Unknown
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiInteractionEditorForm));
		footerPanel = new StiGroupLine();
		btOk = new Button();
		btCancel = new Button();
		tabControl = new StiCloudTabulator();
		pageCommon = new StiCloudTabulatorPage();
		pageDrillDown = new StiCloudTabulatorPage();
		pageDrillDownParameters = new StiCloudTabulatorPage();
		pageSort = new StiCloudTabulatorPage();
		timerMarker = new Timer(components);
		((Control)footerPanel).SuspendLayout();
		((Control)tabControl).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)footerPanel).BackColor = Color.White;
		((Control)footerPanel).Controls.Add((Control)(object)btOk);
		((Control)footerPanel).Controls.Add((Control)(object)btCancel);
		((Control)footerPanel).Dock = (DockStyle)2;
		footerPanel.LinePosition = StiLinePosition.Top;
		footerPanel.LineStyle = StiLineStyle.Dot;
		((Control)footerPanel).Location = new Point(0, 800);
		((Control)footerPanel).Margin = new Padding(6, 6, 6, 6);
		((Control)footerPanel).Name = "footerPanel";
		((Control)footerPanel).Size = new Size(1288, 88);
		((Control)footerPanel).TabIndex = 2;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(948, 23);
		((Control)btOk).Margin = new Padding(6, 6, 6, 6);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(150, 46);
		((Control)btOk).TabIndex = 4;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(1110, 23);
		((Control)btCancel).Margin = new Padding(6, 6, 6, 6);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(150, 46);
		((Control)btCancel).TabIndex = 5;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)tabControl).Controls.Add((Control)(object)pageCommon);
		((Control)tabControl).Controls.Add((Control)(object)pageDrillDown);
		((Control)tabControl).Controls.Add((Control)(object)pageDrillDownParameters);
		((Control)tabControl).Controls.Add((Control)(object)pageSort);
		((Control)tabControl).Dock = (DockStyle)5;
		((Control)tabControl).Location = new Point(0, 0);
		((Control)tabControl).Margin = new Padding(6, 6, 6, 6);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 0, 128, 0);
		tabControl.ScrollHeight = 0;
		tabControl.ScrollWidth = 100;
		((Control)tabControl).Size = new Size(1288, 800);
		((Control)tabControl).TabIndex = 3;
		tabControl.TitleBackColor = SystemColors.Control;
		tabControl.TitleInLine = false;
		tabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		tabControl.TitleWidth = 110;
		pageCommon.Dock = (DockStyle)5;
		pageCommon.Image = (Image)componentResourceManager.GetObject("pageCommon.Image");
		pageCommon.InvisibleVisible = true;
		((Control)pageCommon).Location = new Point(0, 0);
		((Control)pageCommon).Margin = new Padding(6, 6, 6, 6);
		((Control)pageCommon).Name = "pageCommon";
		((Control)pageCommon).Padding = new Padding(0, 8, 2, 8);
		((Control)pageCommon).Size = new Size(1160, 800);
		((Control)pageCommon).TabIndex = 4;
		((Control)pageCommon).Text = "Common";
		((Control)pageCommon).Visible = false;
		pageDrillDown.Dock = (DockStyle)5;
		pageDrillDown.Image = (Image)componentResourceManager.GetObject("pageDrillDown.Image");
		pageDrillDown.InvisibleVisible = false;
		((Control)pageDrillDown).Location = new Point(0, 0);
		((Control)pageDrillDown).Margin = new Padding(6, 6, 6, 6);
		((Control)pageDrillDown).Name = "pageDrillDown";
		((Control)pageDrillDown).Padding = new Padding(0, 0, 2, 0);
		((Control)pageDrillDown).Size = new Size(1330, 973);
		((Control)pageDrillDown).TabIndex = 1;
		((Control)pageDrillDown).Text = "Drilldown";
		((Control)pageDrillDown).Visible = false;
		pageDrillDownParameters.Dock = (DockStyle)5;
		pageDrillDownParameters.Image = (Image)componentResourceManager.GetObject("pageDrillDownParameters.Image");
		pageDrillDownParameters.InvisibleVisible = false;
		((Control)pageDrillDownParameters).Location = new Point(0, 0);
		((Control)pageDrillDownParameters).Margin = new Padding(6, 6, 6, 6);
		((Control)pageDrillDownParameters).Name = "pageDrillDownParameters";
		((Control)pageDrillDownParameters).Padding = new Padding(0, 0, 2, 0);
		((Control)pageDrillDownParameters).Size = new Size(1330, 973);
		((Control)pageDrillDownParameters).TabIndex = 2;
		((Control)pageDrillDownParameters).Text = "Drilldown Parameters";
		((Control)pageDrillDownParameters).Visible = false;
		pageSort.Dock = (DockStyle)5;
		pageSort.Image = (Image)componentResourceManager.GetObject("pageSort.Image");
		pageSort.InvisibleVisible = false;
		((Control)pageSort).Location = new Point(0, -358);
		((Control)pageSort).Margin = new Padding(0);
		((Control)pageSort).Name = "pageSort";
		((Control)pageSort).Padding = new Padding(0, 0, 2, 0);
		((Control)pageSort).Size = new Size(1274, 990);
		((Control)pageSort).TabIndex = 3;
		((Control)pageSort).Text = "Sort";
		((Control)pageSort).Visible = false;
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(1288, 888);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)footerPanel);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiInteractionEditorForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Interaction Editor";
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Control)footerPanel).ResumeLayout(false);
		((Control)tabControl).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
