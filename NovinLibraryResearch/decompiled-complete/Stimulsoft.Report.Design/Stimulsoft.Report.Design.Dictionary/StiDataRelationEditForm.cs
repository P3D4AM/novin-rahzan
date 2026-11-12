using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Extensions;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataRelationEditForm : StiFormUX
{
	private bool locked;

	private string oldValue;

	private bool firstCheckInNewForm;

	private IContainer components;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiComboBox comboBoxParentDataSource;

	private StiComboBox comboBoxChildDataSource;

	private ImageList imageListButtons;

	private StiColumnListBox listBoxParentColumns;

	private StiColumnListBox listBoxChildColumns;

	private StiTextBox textBoxAlias;

	private StiTextBox textBoxName;

	private ErrorProvider errorProvider;

	private StiButtonPanelUX groupLineButtons;

	private StiTextBox textBoxNameInSource;

	private StiLabelUX labelNameInSource;

	private StiLabelUX labelAlias;

	private StiLabelUX labelName;

	private StiButtonUX buttonSaveCopy;

	private StiCheckBoxInfo checkBoxActiveRelation;

	private StiOffice2013GroupBox groupBoxRelationSettings;

	private Label labelErrorMessage;

	private StiPanelUX panelTop;

	private StiLabelUX labelChild;

	private StiLabelUX labelParent;

	public bool CopyModeActivated { get; private set; }

	public string RelationOriginalName { get; set; }

	public bool RelationIsActive
	{
		get
		{
			return ((CheckBox)checkBoxActiveRelation).Checked;
		}
		set
		{
			((CheckBox)checkBoxActiveRelation).Checked = value;
		}
	}

	public string RelationNameInSource
	{
		get
		{
			return ((Control)textBoxNameInSource).Text;
		}
		set
		{
			((Control)textBoxNameInSource).Text = value;
		}
	}

	public string RelationName
	{
		get
		{
			return ((Control)textBoxName).Text;
		}
		set
		{
			((Control)textBoxName).Text = value;
		}
	}

	public string RelationAlias
	{
		get
		{
			return ((Control)textBoxAlias).Text;
		}
		set
		{
			((Control)textBoxAlias).Text = value;
		}
	}

	public StiDataSource RelationParentDataSource
	{
		get
		{
			return ((ComboBox)comboBoxParentDataSource).SelectedItem as StiDataSource;
		}
		set
		{
			((ComboBox)comboBoxParentDataSource).SelectedItem = value;
		}
	}

	public StiDataSource RelationChildDataSource
	{
		get
		{
			return ((ComboBox)comboBoxChildDataSource).SelectedItem as StiDataSource;
		}
		set
		{
			((ComboBox)comboBoxChildDataSource).SelectedItem = value;
		}
	}

	public string[] RelationParentColumns
	{
		get
		{
			return listBoxParentColumns.GetSelectedItems();
		}
		set
		{
			((ListBox)listBoxParentColumns).ClearSelected();
			(from c in value?.Select((string c) => ((ListBox)listBoxParentColumns).Items.IndexOf((object)c))
				where c != -1
				select c).ToList().ForEach(delegate(int i)
			{
				((ListBox)listBoxParentColumns).SetSelected(i, true);
			});
		}
	}

	public string[] RelationChildColumns
	{
		get
		{
			return listBoxChildColumns.GetSelectedItems();
		}
		set
		{
			((ListBox)listBoxChildColumns).ClearSelected();
			(from c in value?.Select((string c) => ((ListBox)listBoxChildColumns).Items.IndexOf((object)c))
				where c != -1
				select c).ToList().ForEach(delegate(int i)
			{
				((ListBox)listBoxChildColumns).SetSelected(i, true);
			});
		}
	}

	public StiDataRelationEditForm(bool isEditMode)
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
			((Control)this).MaximumSize = new Size(((Control)this).Width, Screen.PrimaryScreen.WorkingArea.Height * 2);
			((Control)this).MinimumSize = new Size(((Control)this).Width, ((Control)this).MinimumSize.Height);
			Localize();
			((Form)this).ShowIcon = false;
			firstCheckInNewForm = true;
			((Control)buttonSaveCopy).Visible = isEditMode;
			HideErrorMessage();
			DoTheming();
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)labelNameInSource).Text = Loc.GetCleaned("Report", "LabelNameInSource");
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)groupBoxRelationSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)checkBoxActiveRelation).Text = StiLocalization.Get("Report", "ActiveRelation");
		checkBoxActiveRelation.SetToolTip(StiLocalization.Get("HelpDesigner", "ActiveRelation"));
		((Control)labelParent).Text = Loc.GetCleaned("FormDictionaryDesigner", "Parent");
		((Control)labelChild).Text = Loc.GetCleaned("FormDictionaryDesigner", "Child");
		((Control)buttonSaveCopy).Text = StiLocalization.Get("Buttons", "SaveCopy");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		checkBoxActiveRelation.SetToolTip(Loc.Get("HelpDesigner", "ActiveRelation"));
		StiButtonSizeHelper.CheckSizeOfButton((Button)(object)buttonSaveCopy);
	}

	private void DoTheming()
	{
		((Control)labelErrorMessage).ForeColor = StiUX.ErrorForeground;
		((Control)labelErrorMessage).BackColor = StiUX.ErrorBackground;
	}

	private void DoScaling()
	{
		((Control)comboBoxParentDataSource).Width = ((Control)listBoxParentColumns).Width;
		((Control)comboBoxChildDataSource).Width = ((Control)listBoxChildColumns).Width;
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	public void ReadRelation(StiDataRelation relation)
	{
		AddRelationDataSources(relation.Dictionary.DataSources.ToList());
		RelationOriginalName = relation.Name;
		RelationNameInSource = relation.NameInSource;
		RelationName = relation.Name;
		RelationAlias = relation.Alias;
		RelationParentDataSource = relation.ParentSource;
		RelationChildDataSource = relation.ChildSource;
		RelationIsActive = relation.Active;
		((Control)comboBoxParentDataSource).Text = ((relation.ParentSource != null) ? relation.ParentSource.Name : "");
		((Control)comboBoxChildDataSource).Text = ((relation.ChildSource != null) ? relation.ChildSource.Name : "");
		RefreshParentColumns(relation.ParentSource, relation.ParentColumns);
		RefreshChildColumns(relation.ChildSource, relation.ChildColumns);
		RelationParentColumns = relation.ParentColumns;
		RelationChildColumns = relation.ChildColumns;
	}

	private void RefreshParentColumns(StiDataSource dataSource, string[] keys = null)
	{
		locked = true;
		((ListBox)listBoxParentColumns).Items.Clear();
		if (dataSource != null)
		{
			List<string> names = (from c in (from c in dataSource.Columns.ToList()
					where !(c is StiCalcDataColumn)
					select c).ToList()
				select c.Name).ToList();
			object[] array;
			if (keys != null)
			{
				ObjectCollection items = ((ListBox)listBoxParentColumns).Items;
				array = keys;
				items.AddRange(array);
				keys.ToList().ForEach(delegate(string k)
				{
					names.Remove(k);
				});
			}
			ObjectCollection items2 = ((ListBox)listBoxParentColumns).Items;
			array = names.ToArray();
			items2.AddRange(array);
		}
		locked = false;
	}

	private void RefreshChildColumns(StiDataSource dataSource, string[] keys = null)
	{
		locked = true;
		((ListBox)listBoxChildColumns).Items.Clear();
		if (dataSource != null)
		{
			List<string> names = (from c in (from c in dataSource.Columns.ToList()
					where !(c is StiCalcDataColumn)
					select c).ToList()
				select c.Name).ToList();
			object[] array;
			if (keys != null)
			{
				ObjectCollection items = ((ListBox)listBoxChildColumns).Items;
				array = keys;
				items.AddRange(array);
				keys.ToList().ForEach(delegate(string k)
				{
					names.Remove(k);
				});
			}
			ObjectCollection items2 = ((ListBox)listBoxChildColumns).Items;
			array = names.ToArray();
			items2.AddRange(array);
		}
		locked = false;
	}

	public StiDataRelation GetRelation()
	{
		return new StiDataRelation(RelationNameInSource, RelationName, RelationAlias, RelationParentDataSource, RelationChildDataSource, RelationParentColumns, RelationChildColumns, RelationIsActive);
	}

	public void WriteRelation(StiDataRelation relation)
	{
		relation.NameInSource = RelationNameInSource;
		relation.Name = RelationName;
		relation.Alias = RelationAlias;
		relation.ParentSource = RelationParentDataSource;
		relation.ChildSource = RelationChildDataSource;
		relation.ParentColumns = RelationParentColumns;
		relation.ChildColumns = RelationChildColumns;
		relation.Active = RelationIsActive;
	}

	public void AddRelationDataSources(List<StiDataSource> dataSources)
	{
		locked = true;
		dataSources.ForEach(delegate(StiDataSource d)
		{
			((ComboBox)comboBoxParentDataSource).Items.Add((object)d);
			((ComboBox)comboBoxChildDataSource).Items.Add((object)d);
		});
		locked = false;
	}

	private bool CheckErrors()
	{
		if (CheckEmptyNameInSourceError())
		{
			return true;
		}
		if (CheckEmptyNameError())
		{
			return true;
		}
		if (CheckEmptyAliasError())
		{
			return true;
		}
		if (CheckIdenticalColumnsError())
		{
			return true;
		}
		if (CheckColumnTypesError())
		{
			return true;
		}
		if (!firstCheckInNewForm)
		{
			if (CheckDuplicateNamesError())
			{
				return true;
			}
			if (CheckColumnCountError())
			{
				return true;
			}
			if (CheckParentDataSource())
			{
				return true;
			}
			if (CheckChildDataSourceErrors())
			{
				return true;
			}
			if (CheckParentColumnsError())
			{
				return true;
			}
			if (CheckChildColumnsError())
			{
				return true;
			}
		}
		HideErrorMessage();
		return false;
	}

	private bool CheckEmptyNameInSourceError()
	{
		if (((Control)textBoxNameInSource).Text.Length != 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "NameInSource")));
		return true;
	}

	private bool CheckEmptyNameError()
	{
		if (((Control)textBoxName).Text.Length != 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "Name")));
		return true;
	}

	private bool CheckEmptyAliasError()
	{
		if (((Control)textBoxAlias).Text.Length != 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "Alias")));
		return true;
	}

	private bool CheckDuplicateNamesError()
	{
		if (((Control)textBoxName).Text == RelationOriginalName)
		{
			return false;
		}
		StiDataSource relationChildDataSource = RelationChildDataSource;
		if (relationChildDataSource != null && relationChildDataSource.ParentRelationList().Count((StiDataRelation r) => r.Name == ((Control)textBoxName).Text) == 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "NameExists"), ((Control)textBoxName).Text));
		return true;
	}

	private bool CheckParentDataSource()
	{
		if (((Control)comboBoxParentDataSource).Text.Length != 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "ParentSource")));
		return true;
	}

	private bool CheckChildDataSourceErrors()
	{
		if (((Control)comboBoxChildDataSource).Text.Length != 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "ChildSource")));
		return true;
	}

	private bool CheckParentColumnsError()
	{
		if (((ListBox)listBoxParentColumns).SelectedItems.Count > 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "ParentColumns")));
		return true;
	}

	private bool CheckChildColumnsError()
	{
		if (((ListBox)listBoxChildColumns).SelectedItems.Count > 0)
		{
			return false;
		}
		ShowErrorMessage(string.Format(Loc.Get("Errors", "FieldRequire"), Loc.Get("PropertyMain", "ChildColumns")));
		return true;
	}

	private bool CheckIdenticalColumnsError()
	{
		if (((Control)comboBoxParentDataSource).Text != ((Control)comboBoxChildDataSource).Text)
		{
			return false;
		}
		if (RelationParentColumns.Length != RelationChildColumns.Length)
		{
			return false;
		}
		for (int i = 0; i < RelationParentColumns.Length; i++)
		{
			StiDataColumn stiDataColumn = RelationParentDataSource.Columns[RelationParentColumns[i]];
			StiDataColumn stiDataColumn2 = RelationChildDataSource.Columns[RelationChildColumns[i]];
			if (stiDataColumn == stiDataColumn2)
			{
				ShowErrorMessage("ParentKey and ChildKey are identical.");
				return true;
			}
		}
		return false;
	}

	private bool CheckColumnTypesError()
	{
		if (RelationParentColumns.Length != RelationChildColumns.Length)
		{
			return false;
		}
		for (int i = 0; i < RelationParentColumns.Length; i++)
		{
			StiDataColumn stiDataColumn = RelationParentDataSource.Columns[RelationParentColumns[i]];
			StiDataColumn stiDataColumn2 = RelationChildDataSource.Columns[RelationChildColumns[i]];
			if (stiDataColumn != null && stiDataColumn2 != null)
			{
				Type type = stiDataColumn.Type;
				Type type2 = stiDataColumn2.Type;
				if (type != type2)
				{
					ShowErrorMessage("The '" + GetTypeString(type) + "' type of the '" + RelationParentColumns[i] + "' column is not equal to '" + GetTypeString(type2) + "' type of '" + RelationChildColumns[i] + "'.");
					return true;
				}
			}
		}
		return false;
	}

	private bool CheckColumnCountError()
	{
		if (RelationParentColumns.Length == RelationChildColumns.Length)
		{
			return false;
		}
		ShowErrorMessage("The count of parent columns is not equal to the count of child columns!");
		return true;
	}

	private void ShowErrorMessage(string message)
	{
		((Control)labelErrorMessage).Text = message;
		((Control)labelErrorMessage).Visible = true;
		((Control)buttonOk).Enabled = false;
		((Control)buttonSaveCopy).Enabled = false;
	}

	private void HideErrorMessage()
	{
		((Control)labelErrorMessage).Visible = false;
		((Control)buttonOk).Enabled = true;
		((Control)buttonSaveCopy).Enabled = true;
	}

	private string GetTypeString(Type type)
	{
		return StiTypeConverter.ToString(type);
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		firstCheckInNewForm = false;
		if (!CheckErrors())
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_relation_creating_relation.htm");
	}

	private void This_SaveCopyClick(object sender, EventArgs e)
	{
		CopyModeActivated = true;
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_Load(object sender, EventArgs e)
	{
		CheckErrors();
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void ComboBoxParentDataSource_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			RefreshParentColumns(((ComboBox)comboBoxParentDataSource).SelectedItem as StiDataSource);
			CheckErrors();
		}
	}

	private void ComboBoxChildDataSource_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			RefreshChildColumns(((ComboBox)comboBoxChildDataSource).SelectedItem as StiDataSource);
			CheckErrors();
		}
	}

	private void ListBoxColumns_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!locked)
		{
			CheckErrors();
		}
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)textBoxName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)textBoxAlias).Text)
		{
			((Control)textBoxAlias).Text = ((Control)textBoxName).Text;
		}
		CheckErrors();
	}

	private void TextBox_Leave(object sender, EventArgs e)
	{
		CheckErrors();
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2c: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataRelationEditForm));
		textBoxNameInSource = new StiTextBox();
		labelNameInSource = new StiLabelUX();
		imageListButtons = new ImageList(components);
		labelAlias = new StiLabelUX();
		textBoxAlias = new StiTextBox();
		labelName = new StiLabelUX();
		textBoxName = new StiTextBox();
		errorProvider = new ErrorProvider(components);
		labelErrorMessage = new Label();
		panelTop = new StiPanelUX();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		groupBoxRelationSettings = new StiOffice2013GroupBox();
		labelChild = new StiLabelUX();
		labelParent = new StiLabelUX();
		comboBoxParentDataSource = new StiComboBox();
		checkBoxActiveRelation = new StiCheckBoxInfo();
		listBoxChildColumns = new StiColumnListBox();
		listBoxParentColumns = new StiColumnListBox();
		comboBoxChildDataSource = new StiComboBox();
		groupLineButtons = new StiButtonPanelUX();
		buttonSaveCopy = new StiButtonUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)panelTop).SuspendLayout();
		((Control)groupBoxRelationSettings).SuspendLayout();
		((Control)groupLineButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)textBoxNameInSource).Anchor = (AnchorStyles)13;
		((Control)textBoxNameInSource).BackColor = SystemColors.Window;
		((Control)textBoxNameInSource).Location = new Point(192, 12);
		((Control)textBoxNameInSource).Name = "textBoxNameInSource";
		((Control)textBoxNameInSource).Size = new Size(206, 21);
		((Control)textBoxNameInSource).TabIndex = 0;
		((Control)textBoxNameInSource).Leave += TextBox_Leave;
		((Control)labelNameInSource).ForeColor = Color.DimGray;
		((Control)labelNameInSource).Location = new Point(12, 12);
		((Control)labelNameInSource).Name = "labelNameInSource";
		((Control)labelNameInSource).Size = new Size(174, 16);
		((Control)labelNameInSource).TabIndex = 12;
		((Control)labelNameInSource).Text = "Name in Source";
		((Label)labelNameInSource).TextAlign = (ContentAlignment)16;
		imageListButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageListButtons.ImageStream");
		imageListButtons.TransparentColor = Color.Magenta;
		imageListButtons.Images.SetKeyName(0, "");
		imageListButtons.Images.SetKeyName(1, "");
		imageListButtons.Images.SetKeyName(2, "");
		imageListButtons.Images.SetKeyName(3, "");
		((Control)labelAlias).ForeColor = Color.DimGray;
		((Control)labelAlias).Location = new Point(12, 81);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(174, 16);
		((Control)labelAlias).TabIndex = 15;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)textBoxAlias).Anchor = (AnchorStyles)13;
		((Control)textBoxAlias).BackColor = SystemColors.Window;
		((Control)textBoxAlias).Location = new Point(192, 78);
		((Control)textBoxAlias).Name = "textBoxAlias";
		((Control)textBoxAlias).Size = new Size(206, 21);
		((Control)textBoxAlias).TabIndex = 2;
		((Control)textBoxAlias).Leave += TextBox_Leave;
		((Control)labelName).ForeColor = Color.DimGray;
		((Control)labelName).Location = new Point(12, 48);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(174, 16);
		((Control)labelName).TabIndex = 13;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)textBoxName).Anchor = (AnchorStyles)13;
		((Control)textBoxName).BackColor = SystemColors.Window;
		((Control)textBoxName).Location = new Point(192, 45);
		((Control)textBoxName).Name = "textBoxName";
		((Control)textBoxName).Size = new Size(206, 21);
		((Control)textBoxName).TabIndex = 1;
		((Control)textBoxName).Enter += TextBoxName_Enter;
		((Control)textBoxName).Leave += TextBoxName_Leave;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)labelErrorMessage).BackColor = Color.FromArgb(252, 220, 220);
		((Control)labelErrorMessage).Dock = (DockStyle)2;
		((Control)labelErrorMessage).Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelErrorMessage).ForeColor = Color.Red;
		((Control)labelErrorMessage).Location = new Point(0, 386);
		((Control)labelErrorMessage).Margin = new Padding(2, 0, 2, 0);
		((Control)labelErrorMessage).Name = "labelErrorMessage";
		((Control)labelErrorMessage).Padding = new Padding(12, 2, 12, 2);
		((Control)labelErrorMessage).Size = new Size(411, 39);
		((Control)labelErrorMessage).TabIndex = 2;
		((Control)labelErrorMessage).Text = "Error message";
		labelErrorMessage.TextAlign = (ContentAlignment)16;
		((Control)labelErrorMessage).Visible = false;
		((Control)panelTop).Controls.Add((Control)(object)labelName);
		((Control)panelTop).Controls.Add((Control)(object)textBoxName);
		((Control)panelTop).Controls.Add((Control)(object)labelAlias);
		((Control)panelTop).Controls.Add((Control)(object)textBoxAlias);
		((Control)panelTop).Controls.Add((Control)(object)labelNameInSource);
		((Control)panelTop).Controls.Add((Control)(object)textBoxNameInSource);
		((Control)panelTop).Dock = (DockStyle)1;
		((Control)panelTop).Location = new Point(0, 0);
		((Control)panelTop).Margin = new Padding(2);
		((Control)panelTop).Name = "panelTop";
		((Control)panelTop).Size = new Size(411, 111);
		((Control)panelTop).TabIndex = 0;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(241, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(322, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)labelChild);
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)labelParent);
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)comboBoxParentDataSource);
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)checkBoxActiveRelation);
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)listBoxChildColumns);
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)listBoxParentColumns);
		((Control)groupBoxRelationSettings).Controls.Add((Control)(object)comboBoxChildDataSource);
		((Control)groupBoxRelationSettings).Dock = (DockStyle)5;
		groupBoxRelationSettings.HeaderHeight = 24;
		((Control)groupBoxRelationSettings).Location = new Point(0, 111);
		((Control)groupBoxRelationSettings).Name = "groupBoxRelationSettings";
		((Control)groupBoxRelationSettings).Size = new Size(411, 275);
		((Control)groupBoxRelationSettings).TabIndex = 1;
		((Control)groupBoxRelationSettings).TabStop = false;
		((Control)groupBoxRelationSettings).Text = "Settings";
		((Control)labelChild).ForeColor = Color.DimGray;
		((Control)labelChild).Location = new Point(206, 29);
		((Control)labelChild).Name = "labelChild";
		((Control)labelChild).Size = new Size(174, 16);
		((Control)labelChild).TabIndex = 17;
		((Control)labelChild).Text = "Child";
		((Label)labelChild).TextAlign = (ContentAlignment)16;
		((Control)labelParent).ForeColor = Color.DimGray;
		((Control)labelParent).Location = new Point(12, 29);
		((Control)labelParent).Name = "labelParent";
		((Control)labelParent).Size = new Size(174, 16);
		((Control)labelParent).TabIndex = 16;
		((Control)labelParent).Text = "Parent";
		((Label)labelParent).TextAlign = (ContentAlignment)16;
		((Control)comboBoxParentDataSource).BackColor = SystemColors.Window;
		comboBoxParentDataSource.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxParentDataSource).DropDownWidth = 300;
		((Control)comboBoxParentDataSource).Location = new Point(12, 50);
		((ComboBox)comboBoxParentDataSource).MaxDropDownItems = 15;
		((Control)comboBoxParentDataSource).Name = "comboBoxParentDataSource";
		((Control)comboBoxParentDataSource).Size = new Size(184, 21);
		((Control)comboBoxParentDataSource).TabIndex = 0;
		((ComboBox)comboBoxParentDataSource).SelectedIndexChanged += ComboBoxParentDataSource_SelectedIndexChanged;
		((Control)checkBoxActiveRelation).Anchor = (AnchorStyles)6;
		((Control)checkBoxActiveRelation).AutoSize = true;
		((Control)checkBoxActiveRelation).Location = new Point(12, 248);
		((Control)checkBoxActiveRelation).Name = "checkBoxActiveRelation";
		((Control)checkBoxActiveRelation).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxActiveRelation).Size = new Size(110, 17);
		((Control)checkBoxActiveRelation).TabIndex = 4;
		((Control)checkBoxActiveRelation).Text = "Active Relation";
		((Control)listBoxChildColumns).Anchor = (AnchorStyles)15;
		((Control)listBoxChildColumns).BackColor = SystemColors.Window;
		listBoxChildColumns.DrawSelectedNumber = true;
		((Control)listBoxChildColumns).ForeColor = SystemColors.ControlText;
		((Control)listBoxChildColumns).Location = new Point(206, 80);
		((Control)listBoxChildColumns).Name = "listBoxChildColumns";
		((ListBox)listBoxChildColumns).SelectionMode = (SelectionMode)3;
		((Control)listBoxChildColumns).Size = new Size(192, 157);
		((Control)listBoxChildColumns).TabIndex = 3;
		((ListBox)listBoxChildColumns).SelectedIndexChanged += ListBoxColumns_SelectedIndexChanged;
		((Control)listBoxParentColumns).Anchor = (AnchorStyles)7;
		((Control)listBoxParentColumns).BackColor = SystemColors.Window;
		listBoxParentColumns.DrawSelectedNumber = true;
		((Control)listBoxParentColumns).ForeColor = SystemColors.ControlText;
		((Control)listBoxParentColumns).Location = new Point(12, 80);
		((Control)listBoxParentColumns).Name = "listBoxParentColumns";
		((ListBox)listBoxParentColumns).SelectionMode = (SelectionMode)3;
		((Control)listBoxParentColumns).Size = new Size(184, 157);
		((Control)listBoxParentColumns).TabIndex = 2;
		((ListBox)listBoxParentColumns).SelectedIndexChanged += ListBoxColumns_SelectedIndexChanged;
		((Control)comboBoxChildDataSource).Anchor = (AnchorStyles)13;
		((Control)comboBoxChildDataSource).BackColor = SystemColors.Window;
		comboBoxChildDataSource.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxChildDataSource).DropDownWidth = 300;
		((Control)comboBoxChildDataSource).Location = new Point(206, 50);
		((ComboBox)comboBoxChildDataSource).MaxDropDownItems = 15;
		((Control)comboBoxChildDataSource).Name = "comboBoxChildDataSource";
		((Control)comboBoxChildDataSource).Size = new Size(192, 21);
		((Control)comboBoxChildDataSource).TabIndex = 1;
		((ComboBox)comboBoxChildDataSource).SelectedIndexChanged += ComboBoxChildDataSource_SelectedIndexChanged;
		((Control)groupLineButtons).BackColor = Color.White;
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonSaveCopy);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 425);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(411, 46);
		((Control)groupLineButtons).TabIndex = 3;
		((Control)buttonSaveCopy).Anchor = (AnchorStyles)6;
		((Control)buttonSaveCopy).Location = new Point(12, 12);
		((Control)buttonSaveCopy).Name = "buttonSaveCopy";
		((Control)buttonSaveCopy).Size = new Size(93, 24);
		((Control)buttonSaveCopy).TabIndex = 0;
		((Control)buttonSaveCopy).Text = "Save a Copy";
		((ButtonBase)buttonSaveCopy).UseVisualStyleBackColor = true;
		((Control)buttonSaveCopy).Visible = false;
		((Control)buttonSaveCopy).Click += This_SaveCopyClick;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(411, 471);
		((Control)this).Controls.Add((Control)(object)groupBoxRelationSettings);
		((Control)this).Controls.Add((Control)(object)panelTop);
		((Control)this).Controls.Add((Control)(object)labelErrorMessage);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(412, 349);
		((Control)this).Name = "StiDataRelationEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit Relation";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)panelTop).ResumeLayout(false);
		((Control)groupBoxRelationSettings).ResumeLayout(false);
		((Control)groupBoxRelationSettings).PerformLayout();
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
