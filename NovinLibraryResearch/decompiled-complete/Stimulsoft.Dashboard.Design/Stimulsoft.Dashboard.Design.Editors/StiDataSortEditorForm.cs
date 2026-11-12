using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiDataSortEditorForm : StiElementEditorForm
{
	private IContainer components;

	private ToolTip toolTip;

	private StiDataSortListBox listBoxSort;

	private ContextMenuBar contextMenuBar;

	private ButtonItem menuExpression;

	internal IStiUserSorts DataSortElement => base.Element as IStiUserSorts;

	internal StiDataSortRule DataSort => listBoxSort.SelectedSortItem?.Sort;

	internal bool IsKeysPresent => ((ListBox)listBoxSort).Items.Count > 0;

	public override bool IsAnyPresent => IsKeysPresent;

	public override bool AllowResize => false;

	public StiDataSortEditorForm()
		: this(null)
	{
	}

	public StiDataSortEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		foreach (StiDataSortListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiDataSortListBox
			select c).Cast<StiDataSortListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowMultipleElements = true;
		}
		ReadPropertiesFromElement();
		LocalizeInternal();
		ActivateDictionaryPanel();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxSort).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("PropertyMain", "Sort");
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxSort.SelectedSortItem = listBoxSort.FirstItem();
		}
	}

	public override void ReadPropertiesFromElement()
	{
		ReadFiltersPropertiesFromElement();
	}

	private void ReadFiltersPropertiesFromElement()
	{
		((ListBox)listBoxSort).BeginUpdate();
		((ListBox)listBoxSort).Items.Clear();
		listBoxSort.AddItems(DataSortElement.UserSorts);
		listBoxSort.SelectFirstItem();
		((ListBox)listBoxSort).EndUpdate();
	}

	public override void WritePropertiesToElement()
	{
		WriteFiltersPropertiesToElement();
		RefreshElementView();
	}

	private void WriteFiltersPropertiesToElement()
	{
		DataSortElement.UserSorts = (from t in listBoxSort.ToItems()
			select t.Sort).ToList();
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToKeys();
	}

	private void AttachEventsToKeys()
	{
		listBoxSort.ElementsChanged += delegate
		{
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxSort.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxSort.ElementsDragDrop += delegate
		{
			if (!StiDragDropColumnHelper.IsDataColumnDragged(Designer))
			{
				StiDragDropColumnHelper.IsTableElementDragged(Designer);
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxSort.ElementsSelectedChanged += delegate
		{
			base.Element.Select();
			RefreshUiPanel();
		};
	}

	public override void RefreshUiPanel()
	{
		if (base.LockRefresh)
		{
			return;
		}
		base.LockRefresh = true;
		((Control)this).SuspendLayout();
		try
		{
			base.LockRefresh = true;
			CalculateWindowHeight();
			CalculateWindowPosition();
			base.LockRefresh = false;
		}
		finally
		{
			((Control)this).ResumeLayout();
			base.LockRefresh = false;
		}
	}

	protected override void CalculateWindowHeight()
	{
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_data_filtering_filters.htm");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip = new ToolTip(components);
		listBoxSort = new StiDataSortListBox();
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)this).SuspendLayout();
		((Control)listBoxSort).AllowDrop = true;
		((Control)listBoxSort).Dock = (DockStyle)5;
		((Control)listBoxSort).Location = new Point(12, 12);
		((Control)listBoxSort).Margin = new Padding(2);
		((Control)listBoxSort).Name = "listBoxSort";
		((Control)listBoxSort).Size = new Size(298, 287);
		((Control)listBoxSort).TabIndex = 1;
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(216, 114);
		((Control)contextMenuBar).Margin = new Padding(2, 1, 2, 1);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(100, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 14;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(322, 311);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)listBoxSort);
		((Form)this).Margin = new Padding(2);
		((Control)this).Name = "StiDataSortEditorForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "Sort";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
