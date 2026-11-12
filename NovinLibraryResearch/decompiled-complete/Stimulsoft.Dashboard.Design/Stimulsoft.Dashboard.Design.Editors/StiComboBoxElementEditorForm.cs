using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Dashboard.Design.Editors.Helpers;
using Stimulsoft.Dashboard.Design.Editors.Menu;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiComboBoxElementEditorForm : StiElementEditorForm
{
	private StiMeterListBox selectedListBox;

	private IContainer components;

	private ButtonItem menuExpression;

	private StiMeterListBox listBoxKeys;

	private ToolTip toolTip;

	private StiChartGroupLabel labelKey;

	private StiMeterListBox listBoxNames;

	private StiChartGroupLabel labelName;

	private StiPanelUX panelUi;

	private StiLabelUX labelField;

	private StiCheckBoxUX checkBoxShowAllValue;

	private StiLabelUX labelParent;

	private StiLabelUX labelSelectionMode;

	private StiComboBox comboBoxParent;

	private StiComboBox comboBoxSelectionMode;

	private ContextMenuBar contextMenuBar;

	private StiPanelBox panelBoxField;

	private HintTextBox textBoxField;

	private StiOperationButton buttonFieldMenu;

	private StiOperationButton buttonFieldEdit;

	private StiCheckBoxUX checkBoxShowBlanks;

	internal StiComboBoxElement ComboBoxElement => base.Element as StiComboBoxElement;

	internal StiNameComboBoxMeter ComboBoxName => ComboBoxElement?.NameMeter;

	internal StiKeyComboBoxMeter ComboBoxKey => ComboBoxElement?.KeyMeter;

	internal StiMeterListBox KeysListBox => listBoxKeys;

	internal StiMeterListBox NamesListBox => listBoxNames;

	internal bool IsKeysPresent => ((ListBox)listBoxKeys).Items.Count > 0;

	internal bool IsNamesPresent => ((ListBox)listBoxNames).Items.Count > 0;

	public override bool IsAnyPresent
	{
		get
		{
			if (!IsKeysPresent)
			{
				return IsNamesPresent;
			}
			return true;
		}
	}

	public override bool AllowResize => false;

	private string LocSelectionModeOne => Loc.Get("PropertyEnum", "StiItemSelectionModeOne");

	private string LocSelectionModeMulti => Loc.Get("PropertyEnum", "StiItemSelectionModeMulti");

	public StiComboBoxElementEditorForm()
		: this(null)
	{
	}

	public StiComboBoxElementEditorForm(StiElement element)
		: base(element)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		selectedListBox = listBoxKeys;
		foreach (StiMeterListBox item in (from Control c in (IEnumerable)((Control)this).Controls
			where c is StiMeterListBox
			select c).Cast<StiMeterListBox>())
		{
			item.Designer = Designer;
			item.Editor = this;
			item.AllowMultipleElements = false;
			item.AllowImage = false;
			contextMenuBar.SetContextMenuEx((Control)(object)item, menuExpression);
		}
		InitializeMenus();
		ReadPropertiesFromElement();
		LocalizeInternal();
		ActivateDictionaryPanel();
		BuildParent();
		DoScaling();
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxKeys).Invalidate();
		((Control)listBoxNames).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Components", "StiComboBox");
		((Control)labelKey).Text = Loc.Get("PropertyMain", "Key");
		((Control)labelName).Text = Loc.Get("PropertyMain", "Name");
		((Control)labelParent).Text = Loc.Get("Dashboard", "ParentElement");
		((Control)labelField).Text = Loc.Get("PropertyMain", "Field");
		((Control)labelSelectionMode).Text = Loc.Get("PropertyMain", "SelectionMode");
		((Control)checkBoxShowAllValue).Text = Loc.Get("Dashboard", "ShowAllValue");
		((Control)checkBoxShowBlanks).Text = Loc.Get("Dashboard", "ShowBlanks");
		((ComboBox)comboBoxSelectionMode).BeginUpdate();
		int selectedIndex = ((ListControl)comboBoxSelectionMode).SelectedIndex;
		((ComboBox)comboBoxSelectionMode).Items.Clear();
		((ComboBox)comboBoxSelectionMode).Items.Add((object)LocSelectionModeOne);
		((ComboBox)comboBoxSelectionMode).Items.Add((object)LocSelectionModeMulti);
		((ListControl)comboBoxSelectionMode).SelectedIndex = selectedIndex;
		((ComboBox)comboBoxSelectionMode).EndUpdate();
	}

	private void DoScaling()
	{
		((ButtonBase)buttonFieldEdit).Image = StiReportDesignImages.Actions.Edit();
		StiComboBox stiComboBox = comboBoxParent;
		int width = (((Control)comboBoxSelectionMode).Width = ((Control)panelUi).Width - ((Control)comboBoxParent).Left);
		((Control)stiComboBox).Width = width;
		StiButtonsLocator.ArrangeButtons((Control)(object)panelBoxField);
	}

	private void InitializeMenus()
	{
		menuExpression.PopupOpen += delegate
		{
			new StiComboBoxExpressionMenu(menuExpression, (TextBox)(object)textBoxField, this, selectedListBox).CreateItems();
		};
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
			listBoxKeys.SelectedMeterItem = listBoxKeys.FirstItem();
			listBoxNames.SelectedMeterItem = listBoxNames.FirstItem();
		}
	}

	public override void ReadPropertiesFromElement()
	{
		ReadKeysPropertiesFromElement();
		ReadNamesPropertiesFromElement();
	}

	private void ReadNamesPropertiesFromElement()
	{
		if (!listBoxNames.ItemsEqualTo(ComboBoxName))
		{
			((ListBox)listBoxNames).BeginUpdate();
			((ListBox)listBoxNames).Items.Clear();
			listBoxNames.AddItem(ComboBoxName);
			listBoxNames.SelectFirstItem();
			((ListBox)listBoxNames).EndUpdate();
		}
	}

	private void ReadKeysPropertiesFromElement()
	{
		if (!listBoxKeys.ItemsEqualTo(ComboBoxKey))
		{
			((ListBox)listBoxKeys).BeginUpdate();
			((ListBox)listBoxKeys).Items.Clear();
			listBoxKeys.AddItem(ComboBoxKey);
			listBoxKeys.SelectFirstItem();
			((ListBox)listBoxKeys).EndUpdate();
		}
	}

	public override void WritePropertiesToElement()
	{
		WriteNamesPropertiesToElement();
		WriteKeysPropertiesToElement();
		RefreshElementView();
	}

	private void WriteNamesPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxNames.FirstItem();
		ComboBoxElement.NameMeter = stiMeterItem?.Meter as StiNameComboBoxMeter;
	}

	private void WriteKeysPropertiesToElement()
	{
		StiMeterItem stiMeterItem = listBoxKeys.FirstItem();
		ComboBoxElement.KeyMeter = stiMeterItem?.Meter as StiKeyComboBoxMeter;
	}

	protected override void AttachEvents()
	{
		base.AttachEvents();
		AttachEventsToNames();
		AttachEventsToKeys();
	}

	private void AttachEventsToNames()
	{
		listBoxNames.ElementsConverting += ListBox_ElementsConverting;
		listBoxNames.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxNames.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxNames.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxNames).Items.Clear();
				listBoxNames.AddItem(StiMeterHelper.ComboBox.GetName(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				ComboBoxElement.Select();
				WritePropertiesToElement();
				listBoxNames.SelectedMeterItem = listBoxNames.LastItem();
				((Control)listBoxNames).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxNames).Items.Clear();
				listBoxNames.AddItem(StiMeterHelper.ComboBox.GetName(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxNames.SelectedMeterItem = listBoxNames.LastItem();
				((Control)listBoxNames).Focus();
			}
			ResetComponentHighlights();
		};
		listBoxNames.ElementsSelectedChanged += delegate
		{
			ComboBoxElement.Select();
			RefreshUiPanel();
		};
	}

	private void AttachEventsToKeys()
	{
		listBoxKeys.ElementsConverting += ListBox_ElementsConverting;
		listBoxKeys.ElementsChanged += delegate(object sender, StiElementChangedArgs changedArgs)
		{
			ProcessElementChanging(changedArgs);
			WritePropertiesToElement();
			RefreshUiPanel();
		};
		listBoxKeys.ElementsDragEnter += delegate
		{
			ResetComponentHighlights();
		};
		listBoxKeys.ElementsDragDrop += delegate
		{
			if (StiDragDropColumnHelper.IsDataColumnOrVariableDragged(Designer))
			{
				IStiAppDataCell draggedDataColumnOrVariable = StiDragDropColumnHelper.GetDraggedDataColumnOrVariable(Designer);
				if (draggedDataColumnOrVariable == null)
				{
					return;
				}
				((ListBox)listBoxKeys).Items.Clear();
				listBoxKeys.AddItem(StiMeterHelper.ComboBox.GetKey(draggedDataColumnOrVariable));
				Designer.PageView.ResetDragging();
				ComboBoxElement.Select();
				WritePropertiesToElement();
				listBoxKeys.SelectedMeterItem = listBoxKeys.LastItem();
				((Control)listBoxKeys).Focus();
			}
			else if (StiDragDropColumnHelper.IsTableElementDragged(Designer))
			{
				if (!(StiDragDropColumnHelper.GetDraggedTableElement(Designer) is StiTableElement stiTableElement))
				{
					return;
				}
				StiTableColumn stiTableColumn = stiTableElement.Columns.FirstOrDefault();
				if (stiTableColumn == null)
				{
					return;
				}
				((ListBox)listBoxKeys).Items.Clear();
				listBoxKeys.AddItem(StiMeterHelper.ComboBox.GetKey(stiTableColumn));
				Designer.PageView.ResetDragging();
				WritePropertiesToElement();
				listBoxKeys.SelectedMeterItem = listBoxKeys.LastItem();
				((Control)listBoxKeys).Focus();
			}
			ResetComponentHighlights();
			RefreshUiPanel();
		};
		listBoxKeys.ElementsSelectedChanged += delegate
		{
			ComboBoxElement.Select();
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
			int num = 0;
			foreach (StiFilterParentItem item in ((ComboBox)comboBoxParent).Items)
			{
				if (item?.Element?.Key == ComboBoxElement.ParentKey)
				{
					((ListControl)comboBoxParent).SelectedIndex = num;
				}
				num++;
			}
			((ComboBox)comboBoxSelectionMode).SelectedItem = ((ComboBoxElement.SelectionMode == StiItemSelectionMode.One) ? LocSelectionModeOne : LocSelectionModeMulti);
			((ComboBox)comboBoxSelectionMode).SelectedItem = ComboBoxElement.SelectionMode;
			((CheckBox)checkBoxShowAllValue).Checked = ComboBoxElement.ShowAllValue;
			((CheckBox)checkBoxShowBlanks).Checked = ComboBoxElement.ShowBlanks;
			StiChartGroupLabel stiChartGroupLabel = labelKey;
			StiChartGroupLabel stiChartGroupLabel2 = labelName;
			StiPanelUX stiPanelUX = panelUi;
			StiLabelUX stiLabelUX = labelField;
			StiPanelBox stiPanelBox = panelBoxField;
			bool flag = (((Control)listBoxNames).Visible = IsAnyPresent);
			bool flag3 = (((Control)stiPanelBox).Visible = flag);
			bool flag5 = (((Control)stiLabelUX).Visible = flag3);
			bool flag7 = (((Control)stiPanelUX).Visible = flag5);
			bool visible = (((Control)stiChartGroupLabel2).Visible = flag7);
			((Control)stiChartGroupLabel).Visible = visible;
			((Control)listBoxKeys).Dock = (DockStyle)(IsAnyPresent ? 1 : 5);
			SetListHeightFromCount(listBoxNames, multiple: false);
			SetListHeightFromCount(listBoxKeys, multiple: false);
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
		if (IsAnyPresent)
		{
			base.CalculateWindowHeight();
		}
		else
		{
			((Form)this).ClientSize = new Size(((Form)this).ClientSize.Width, base.PaddingCorrection + ((Control)listBoxKeys).Width);
		}
	}

	private IEnumerable<StiComponent> GetParentElements()
	{
		return from c in (base.Element.Dashboard as StiDashboard).GetComponents().ToList()
			where c != base.Element && c is IStiFilterElement
			select c;
	}

	private void BuildParent()
	{
		((ComboBox)comboBoxParent).BeginUpdate();
		((ComboBox)comboBoxParent).Items.Clear();
		((ComboBox)comboBoxParent).Items.Add((object)new StiFilterParentItem());
		foreach (StiElement parentElement in GetParentElements())
		{
			parentElement.Key = StiKeyHelper.GetOrGeneratedKey(parentElement.Key);
			((ComboBox)comboBoxParent).Items.Add((object)new StiFilterParentItem
			{
				Element = parentElement
			});
		}
		((ComboBox)comboBoxParent).EndUpdate();
	}

	internal void RefreshExpression()
	{
		base.LockRefresh = true;
		if (selectedListBox == listBoxKeys && ComboBoxKey != null)
		{
			((Control)textBoxField).Text = ComboBoxKey.Expression;
		}
		else if (selectedListBox == listBoxNames && ComboBoxName != null)
		{
			((Control)textBoxField).Text = ComboBoxName.Expression;
		}
		else
		{
			((Control)textBoxField).Text = string.Empty;
		}
		base.LockRefresh = false;
	}

	private void ButtonFieldEdit_Click(object sender, EventArgs e)
	{
		if (StiExpressionEditorHelper.RunExpressionEditor(base.Element, ref textBoxField))
		{
			RefreshElementView();
		}
	}

	private void ListBox_ElementsConverting(object sender, EventArgs e)
	{
		StiMeterItem stiMeterItem = sender as StiMeterItem;
		if (stiMeterItem.List == listBoxKeys)
		{
			stiMeterItem.Meter = StiMeterHelper.ComboBox.GetKey(stiMeterItem.Meter);
		}
		if (stiMeterItem.List == listBoxNames)
		{
			stiMeterItem.Meter = StiMeterHelper.ComboBox.GetName(stiMeterItem.Meter);
		}
	}

	private void ComboBoxSelectionMode_SelectedIndexChanged(object sender, EventArgs e)
	{
		StiComboBox stiComboBox = comboBoxSelectionMode;
		bool flag = ((stiComboBox != null) ? ((ComboBox)stiComboBox).SelectedItem : null) as string == LocSelectionModeOne;
		ComboBoxElement.SelectionMode = ((!flag) ? StiItemSelectionMode.Multi : StiItemSelectionMode.One);
		RefreshElementView();
		RefreshUiPanel();
	}

	private void ComboBoxParent_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			ComboBoxElement.ParentKey = (((ComboBox)comboBoxParent).SelectedItem as StiFilterParentItem)?.Element?.Key;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void CheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			ComboBoxElement.ShowAllValue = ((CheckBox)checkBoxShowAllValue).Checked;
			ComboBoxElement.ShowBlanks = ((CheckBox)checkBoxShowBlanks).Checked;
			RefreshElementView();
			RefreshUiPanel();
		}
	}

	private void TextBoxField_TextChanged(object sender, EventArgs e)
	{
		if (!base.LockRefresh)
		{
			if (selectedListBox == listBoxKeys && ComboBoxKey != null)
			{
				ComboBoxKey.Expression = ((Control)textBoxField).Text;
				listBoxKeys.SetSelectedText(StiLabelHelper.GetLabel(ComboBoxKey));
			}
			if (selectedListBox == listBoxNames && ComboBoxName != null)
			{
				ComboBoxName.Expression = ((Control)textBoxField).Text;
				listBoxNames.SetSelectedText(StiLabelHelper.GetLabel(ComboBoxName));
			}
		}
	}

	private void ButtonFieldMenu_Click(object sender, EventArgs e)
	{
		new StiComboBoxExpressionMenu(menuExpression, (TextBox)(object)textBoxField, this, selectedListBox).CreateItems();
		Point point = new Point(((Control)panelBoxField).Width, ((Control)panelBoxField).Height + 1);
		if (StiLeftRightHandedHelper.IsLeftHanded)
		{
			point.X -= menuExpression.PopupSize.Width;
		}
		point = ((Control)panelBoxField).PointToScreen(point);
		menuExpression.Popup(point);
	}

	private void ListBox_Enter(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_AfterSelect(object sender, EventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void ListBox_MouseDown(object sender, MouseEventArgs e)
	{
		selectedListBox = sender as StiMeterListBox;
		RefreshExpression();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards_data_filtering_combo_box.htm");
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Expected O, but got Unknown
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiComboBoxElementEditorForm));
		contextMenuBar = new ContextMenuBar();
		menuExpression = new ButtonItem();
		listBoxKeys = new StiMeterListBox();
		toolTip = new ToolTip(components);
		labelKey = new StiChartGroupLabel();
		listBoxNames = new StiMeterListBox();
		labelName = new StiChartGroupLabel();
		panelUi = new StiPanelUX();
		checkBoxShowBlanks = new StiCheckBoxUX();
		checkBoxShowAllValue = new StiCheckBoxUX();
		labelParent = new StiLabelUX();
		labelSelectionMode = new StiLabelUX();
		comboBoxParent = new StiComboBox();
		comboBoxSelectionMode = new StiComboBox();
		panelBoxField = new StiPanelBox();
		buttonFieldEdit = new StiOperationButton();
		textBoxField = new HintTextBox();
		buttonFieldMenu = new StiOperationButton();
		labelField = new StiLabelUX();
		((ISupportInitialize)contextMenuBar).BeginInit();
		((Control)panelUi).SuspendLayout();
		((Control)panelBoxField).SuspendLayout();
		((Control)this).SuspendLayout();
		contextMenuBar.AntiAlias = true;
		contextMenuBar.DockTabStripHeight = 50;
		((Control)contextMenuBar).Font = new Font("Segoe UI", 9f);
		contextMenuBar.Items.AddRange(new BaseItem[1] { menuExpression });
		contextMenuBar.Location = new Point(200, 249);
		((Control)contextMenuBar).Margin = new Padding(1);
		contextMenuBar.Name = "contextMenuBar";
		contextMenuBar.Size = new Size(100, 31);
		contextMenuBar.Stretch = true;
		contextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
		contextMenuBar.TabIndex = 12;
		contextMenuBar.TabStop = false;
		((Control)contextMenuBar).Text = "contextMenuBar";
		contextMenuBar.WrapItemsDock = true;
		menuExpression.AutoExpandOnClick = true;
		menuExpression.ImagePaddingHorizontal = 16;
		menuExpression.ImagePaddingVertical = 12;
		menuExpression.LockClosePopup = false;
		menuExpression.Name = "menuExpression";
		menuExpression.Text = "menuExpression";
		((Control)listBoxKeys).AllowDrop = true;
		((Control)listBoxKeys).Dock = (DockStyle)1;
		((Control)listBoxKeys).Location = new Point(12, 44);
		((Control)listBoxKeys).Margin = new Padding(2);
		((Control)listBoxKeys).Name = "listBoxKeys";
		((Control)listBoxKeys).Size = new Size(311, 50);
		((Control)listBoxKeys).TabIndex = 1;
		((ListBox)listBoxKeys).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxKeys).Enter += ListBox_Enter;
		((Control)listBoxKeys).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelKey).Dock = (DockStyle)1;
		((Control)labelKey).Location = new Point(12, 12);
		((Control)labelKey).Margin = new Padding(2, 0, 2, 0);
		((Control)labelKey).Name = "labelKey";
		((Control)labelKey).Size = new Size(311, 32);
		((Control)labelKey).TabIndex = 0;
		((Control)labelKey).Text = "Key";
		((Control)listBoxNames).AllowDrop = true;
		((Control)listBoxNames).Dock = (DockStyle)1;
		((Control)listBoxNames).Location = new Point(12, 126);
		((Control)listBoxNames).Margin = new Padding(2);
		((Control)listBoxNames).Name = "listBoxNames";
		((Control)listBoxNames).Size = new Size(311, 50);
		((Control)listBoxNames).TabIndex = 3;
		((ListBox)listBoxNames).SelectedIndexChanged += ListBox_AfterSelect;
		((Control)listBoxNames).Enter += ListBox_Enter;
		((Control)listBoxNames).MouseDown += new MouseEventHandler(ListBox_MouseDown);
		((Control)labelName).Dock = (DockStyle)1;
		((Control)labelName).Location = new Point(12, 94);
		((Control)labelName).Margin = new Padding(2, 0, 2, 0);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(311, 32);
		((Control)labelName).TabIndex = 2;
		((Control)labelName).Text = "Name";
		((Control)panelUi).BackColor = Color.White;
		((Control)panelUi).Controls.Add((Control)(object)checkBoxShowBlanks);
		((Control)panelUi).Controls.Add((Control)(object)checkBoxShowAllValue);
		((Control)panelUi).Controls.Add((Control)(object)labelParent);
		((Control)panelUi).Controls.Add((Control)(object)labelSelectionMode);
		((Control)panelUi).Controls.Add((Control)(object)comboBoxParent);
		((Control)panelUi).Controls.Add((Control)(object)comboBoxSelectionMode);
		((Control)panelUi).Dock = (DockStyle)2;
		((Control)panelUi).Location = new Point(12, 355);
		((Control)panelUi).Margin = new Padding(2);
		((Control)panelUi).Name = "panelUi";
		((Control)panelUi).Size = new Size(311, 139);
		((Control)panelUi).TabIndex = 6;
		((Control)checkBoxShowBlanks).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowBlanks).Location = new Point(123, 77);
		((Control)checkBoxShowBlanks).Name = "checkBoxShowBlanks";
		((Control)checkBoxShowBlanks).Size = new Size(185, 20);
		((Control)checkBoxShowBlanks).TabIndex = 3;
		((Control)checkBoxShowBlanks).Text = "Show Blanks";
		((ButtonBase)checkBoxShowBlanks).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxShowBlanks).CheckedChanged += CheckBox_CheckedChanged;
		((Control)checkBoxShowAllValue).Anchor = (AnchorStyles)13;
		((Control)checkBoxShowAllValue).Location = new Point(123, 45);
		((Control)checkBoxShowAllValue).Name = "checkBoxShowAllValue";
		((Control)checkBoxShowAllValue).Size = new Size(185, 20);
		((Control)checkBoxShowAllValue).TabIndex = 2;
		((Control)checkBoxShowAllValue).Text = "Show Select All";
		((ButtonBase)checkBoxShowAllValue).UseVisualStyleBackColor = true;
		((CheckBox)checkBoxShowAllValue).CheckedChanged += CheckBox_CheckedChanged;
		((Control)labelParent).ForeColor = Color.DimGray;
		((Control)labelParent).Location = new Point(0, 111);
		((Control)labelParent).Name = "labelParent";
		((Control)labelParent).Size = new Size(118, 17);
		((Control)labelParent).TabIndex = 4;
		((Control)labelParent).Text = "Parent";
		((Label)labelParent).TextAlign = (ContentAlignment)16;
		((Control)labelSelectionMode).ForeColor = Color.DimGray;
		((Control)labelSelectionMode).Location = new Point(0, 15);
		((Control)labelSelectionMode).Name = "labelSelectionMode";
		((Control)labelSelectionMode).Size = new Size(118, 17);
		((Control)labelSelectionMode).TabIndex = 0;
		((Control)labelSelectionMode).Text = "Selection Mode";
		((Label)labelSelectionMode).TextAlign = (ContentAlignment)16;
		((Control)comboBoxParent).Anchor = (AnchorStyles)13;
		comboBoxParent.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxParent).FormattingEnabled = true;
		((Control)comboBoxParent).Location = new Point(123, 109);
		((Control)comboBoxParent).Margin = new Padding(2);
		((Control)comboBoxParent).Name = "comboBoxParent";
		((Control)comboBoxParent).Size = new Size(188, 21);
		((Control)comboBoxParent).TabIndex = 5;
		((ComboBox)comboBoxParent).SelectedIndexChanged += ComboBoxParent_SelectedIndexChanged;
		((Control)comboBoxSelectionMode).Anchor = (AnchorStyles)13;
		comboBoxSelectionMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxSelectionMode).FormattingEnabled = true;
		((Control)comboBoxSelectionMode).Location = new Point(123, 12);
		((Control)comboBoxSelectionMode).Margin = new Padding(2);
		((Control)comboBoxSelectionMode).Name = "comboBoxSelectionMode";
		((Control)comboBoxSelectionMode).Size = new Size(188, 21);
		((Control)comboBoxSelectionMode).TabIndex = 1;
		((ComboBox)comboBoxSelectionMode).SelectedIndexChanged += ComboBoxSelectionMode_SelectedIndexChanged;
		((Control)panelBoxField).BackColor = Color.WhiteSmoke;
		((Control)panelBoxField).Controls.Add((Control)(object)buttonFieldEdit);
		((Control)panelBoxField).Controls.Add((Control)(object)textBoxField);
		((Control)panelBoxField).Controls.Add((Control)(object)buttonFieldMenu);
		((Control)panelBoxField).Dock = (DockStyle)1;
		((Control)panelBoxField).Location = new Point(12, 208);
		((Control)panelBoxField).Margin = new Padding(2);
		((Control)panelBoxField).Name = "panelBoxField";
		((Control)panelBoxField).Size = new Size(311, 25);
		((Control)panelBoxField).TabIndex = 5;
		((Control)buttonFieldEdit).Anchor = (AnchorStyles)9;
		((ButtonBase)buttonFieldEdit).Image = (Image)componentResourceManager.GetObject("buttonFieldEdit.Image");
		((Control)buttonFieldEdit).Location = new Point(265, 1);
		((Control)buttonFieldEdit).Margin = new Padding(2);
		((Control)buttonFieldEdit).Name = "buttonFieldEdit";
		((Control)buttonFieldEdit).Size = new Size(23, 23);
		((Control)buttonFieldEdit).TabIndex = 1;
		((Control)buttonFieldEdit).TabStop = false;
		((ButtonBase)buttonFieldEdit).UseVisualStyleBackColor = true;
		((Control)buttonFieldEdit).Click += ButtonFieldEdit_Click;
		((Control)textBoxField).Anchor = (AnchorStyles)13;
		((Control)textBoxField).BackColor = Color.WhiteSmoke;
		((TextBoxBase)textBoxField).BorderStyle = (BorderStyle)0;
		((Control)textBoxField).Location = new Point(4, 6);
		((Control)textBoxField).Margin = new Padding(2);
		((Control)textBoxField).Name = "textBoxField";
		((TextBoxBase)textBoxField).ReadOnly = true;
		((Control)textBoxField).Size = new Size(255, 13);
		((Control)textBoxField).TabIndex = 0;
		((Control)textBoxField).TextChanged += TextBoxField_TextChanged;
		((Control)buttonFieldMenu).Anchor = (AnchorStyles)9;
		((Control)buttonFieldMenu).Location = new Point(287, 1);
		((Control)buttonFieldMenu).Margin = new Padding(2);
		((Control)buttonFieldMenu).Name = "buttonFieldMenu";
		((Control)buttonFieldMenu).Size = new Size(23, 23);
		((Control)buttonFieldMenu).TabIndex = 2;
		((Control)buttonFieldMenu).TabStop = false;
		((ButtonBase)buttonFieldMenu).UseVisualStyleBackColor = true;
		((Control)buttonFieldMenu).Click += ButtonFieldMenu_Click;
		((Control)labelField).Dock = (DockStyle)1;
		((Control)labelField).ForeColor = Color.DimGray;
		((Control)labelField).Location = new Point(12, 176);
		((Control)labelField).Name = "labelField";
		((Control)labelField).Size = new Size(311, 32);
		((Control)labelField).TabIndex = 4;
		((Control)labelField).Text = "Field";
		((Label)labelField).TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(335, 506);
		((Control)this).Controls.Add((Control)(object)panelBoxField);
		((Control)this).Controls.Add((Control)(object)contextMenuBar);
		((Control)this).Controls.Add((Control)(object)labelField);
		((Control)this).Controls.Add((Control)(object)panelUi);
		((Control)this).Controls.Add((Control)(object)listBoxNames);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)listBoxKeys);
		((Control)this).Controls.Add((Control)(object)labelKey);
		((Form)this).Margin = new Padding(2);
		((Control)this).Name = "StiComboBoxElementEditorForm";
		((Control)this).Padding = new Padding(12);
		((Control)this).Text = "Combo Box";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((ISupportInitialize)contextMenuBar).EndInit();
		((Control)panelUi).ResumeLayout(false);
		((Control)panelBoxField).ResumeLayout(false);
		((Control)panelBoxField).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
