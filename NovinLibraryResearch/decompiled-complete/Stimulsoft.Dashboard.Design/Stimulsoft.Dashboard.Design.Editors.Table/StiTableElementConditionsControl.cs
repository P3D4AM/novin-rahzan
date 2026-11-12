using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Table;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors.Table;

public class StiTableElementConditionsControl : StiZoneControl
{
	private ToolTip toolTip = new ToolTip();

	private StiTableElement tableElement;

	private StiTableElementCondition condition;

	private List<string> keyDataFielColumns = new List<string>();

	private List<string> keyDestinationColumns = new List<string>();

	private IContainer components;

	private StiHitTestVisibleLabel lbFieldIs;

	public StiComboBox cbCondition;

	private StiHitTestVisibleLabel lbCondition;

	private StiHitTestVisibleLabel lbValue;

	public StiGroupLine groupLine;

	public StiToolButton tbFontStyleUnderline;

	public StiToolButton tbFontStyleItalic;

	public StiToolButton tbFontStyleBold;

	private StiHitTestVisibleLabel lbBackColor;

	private ButtonX buttonPermissions;

	internal ButtonItem buttonItemAllowFontBold;

	internal ButtonItem buttonItemAllowFontItalic;

	internal ButtonItem buttonItemAllowFontUnderline;

	internal ButtonItem buttonItemAllowFontStrikeout;

	internal ButtonItem buttonItemAllowColor;

	public StiToolButton tbFontStyleStrikeout;

	private ColorPickerButton colorBoxBackColor;

	private ButtonItem buttonItemAllowBackColor;

	public StiComboBox cbDataType;

	private StiHitTestVisibleLabel lbDataType;

	private ColorPickerButton colorBoxColor;

	private StiHitTestVisibleLabel lbColor;

	public DateTimePicker dtpValue;

	private CheckBox checkBoxBool;

	public StiTreeViewBox treeBoxField;

	public StiTreeViewBox treeBoxDestination;

	private StiHitTestVisibleLabel lbDestination;

	public StiComboBox comboBoxExpression;

	private StiPanelBox panelExpression;

	private HintTextBox tbValue;

	private StiOperationButton buttonExpression;

	public bool IsExpression
	{
		get
		{
			return ((ComboBox)comboBoxExpression).SelectedItem.ToString() == StiLocalization.Get("PropertyMain", "Expression");
		}
		set
		{
			((ComboBox)comboBoxExpression).SelectedItem = (value ? StiLocalization.Get("PropertyMain", "Expression") : StiLocalization.Get("PropertyMain", "Value"));
		}
	}

	public StiTableElementConditionsControl(StiTableElement tableElement, StiTableElementCondition condition, bool isNew = true)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			base.DrawBorder = false;
			base.ShowClose = true;
			Localize();
			dtpValue.Value = DateTime.Now;
			this.tableElement = tableElement;
			this.condition = condition;
			keyDataFielColumns = condition.KeyDataFieldMeters.ToList();
			keyDestinationColumns = condition.KeyDestinationMeters.ToList();
			Build();
			if (!VisualStyleInformation.IsEnabledByUser)
			{
				buttonPermissions.ThemeAware = false;
				buttonPermissions.Style = eDotNetBarStyle.OfficeXP;
			}
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)lbCondition).Text = StiLocalization.Get("PropertyMain", "Condition");
		((Control)lbFieldIs).Text = StiLocalization.Get("PropertyMain", "DataField");
		((Control)lbDataType).Text = StiLocalization.Get("PropertyMain", "DataType");
		((Control)lbDestination).Text = StiLocalization.Get("PropertyMain", "Destination");
		((Control)lbValue).Text = StiLocalization.Get("PropertyMain", "Value");
		((Control)lbColor).Text = StiLocalization.Get("PropertyMain", "ForeColor");
		((Control)lbBackColor).Text = StiLocalization.Get("PropertyMain", "BackColor");
		buttonItemAllowFontBold.Text = StiLocalization.Get("PropertyMain", "FontBold");
		buttonItemAllowFontItalic.Text = StiLocalization.Get("PropertyMain", "FontItalic");
		buttonItemAllowFontUnderline.Text = StiLocalization.Get("PropertyMain", "FontUnderline");
		buttonItemAllowFontStrikeout.Text = StiLocalization.Get("PropertyMain", "FontStrikeout");
		buttonItemAllowColor.Text = StiLocalization.Get("PropertyMain", "Color");
		buttonItemAllowBackColor.Text = StiLocalization.Get("PropertyMain", "BackColor");
		((ComboBox)comboBoxExpression).Items.Add((object)StiLocalization.Get("PropertyMain", "Value"));
		((ComboBox)comboBoxExpression).Items.Add((object)StiLocalization.Get("PropertyMain", "Expression"));
		((ListControl)comboBoxExpression).SelectedIndex = 0;
	}

	private void ComboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ComboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void ButtonItemAllowFontName_CheckedChanged(object sender, EventArgs e)
	{
		CheckAllows();
	}

	private void PanelImage_DragOver(object sender, DragEventArgs e)
	{
		if (!e.Data.GetDataPresent(DataFormats.FileDrop, false))
		{
			return;
		}
		string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
		if (array != null)
		{
			foreach (string item in array.Where((string f) => f != null))
			{
				string ext = Path.GetExtension(item).ToLowerInvariant();
				if (StiResourceTypeHelper.IsImageType(ext))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	private void ComboBoxIconAlign_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void TreeBoxField_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		TreeNodesFill(keyDataFielColumns, e, new TreeViewEventHandler(DataFieldTreeView_AfterCheck));
	}

	private void DataFieldTreeView_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		keyDataFielColumns.Clear();
		List<string> list = new List<string>();
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		foreach (TreeNode node in val.Nodes)
		{
			TreeNode val2 = node;
			if (val2.Checked)
			{
				keyDataFielColumns.Add(val2.Tag?.ToString());
				list.Add(val2.Text);
			}
		}
		((Control)treeBoxField).Text = string.Join(", ", list);
		toolTip.SetToolTip((Control)(object)treeBoxField, ((Control)treeBoxField).Text.Replace(", ", "\n"));
	}

	private void TreeBoxDestination_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		TreeNodesFill(keyDestinationColumns, e, new TreeViewEventHandler(DestinationTreeView_AfterCheck));
	}

	private void DestinationTreeView_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		keyDestinationColumns.Clear();
		List<string> list = new List<string>();
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		foreach (TreeNode node in val.Nodes)
		{
			TreeNode val2 = node;
			if (val2.Checked)
			{
				keyDestinationColumns.Add(val2.Tag?.ToString());
				list.Add(val2.Text);
			}
		}
		((Control)treeBoxDestination).Text = string.Join(", ", list);
		toolTip.SetToolTip((Control)(object)treeBoxDestination, ((Control)treeBoxDestination).Text.Replace(", ", "\n"));
	}

	private void ButtonExpression_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Invalid comparison between Unknown and I4
		HintTextBox hintTextBox = tbValue;
		string text = ((Control)hintTextBox).Text;
		DialogResult val = StiTextEditor.EditText(tableElement.Report, ref text, StiCodeEditorMode.EditorDashboardExpression, Loc.Get("FormTitles", "ExpressionEditorForm"));
		if ((int)val == 1)
		{
			((Control)hintTextBox).Text = text;
		}
		((Control)hintTextBox).Focus();
		((TextBoxBase)hintTextBox).Select(0, 0);
	}

	private void ComboBoxExpression_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void TreeNodesFill(List<string> columns, StiTreeNodesFillEventArgs e, TreeViewEventHandler treeViewEventHandler)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		((TreeView)e.TreeView).ShowRootLines = false;
		((TreeView)e.TreeView).CheckBoxes = true;
		((Control)e.TreeView).Dock = (DockStyle)1;
		((TreeView)e.TreeView).AfterCheck += treeViewEventHandler;
		((Control)e.TreeView).Height = ((Control)e.TreeView).Parent.Height - StiScale.I(32);
		List<StiMeter> list = new List<StiMeter>();
		list.AddRange(tableElement.Columns);
		foreach (StiMeter item in list)
		{
			string label = StiLabelHelper.GetLabel(item);
			bool @checked = columns?.Contains(item.Key) ?? false;
			TreeNode val = new TreeNode(label, 0, 0)
			{
				Tag = item.Key,
				Checked = @checked
			};
			e.Nodes.Add(val);
		}
		Button val2 = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "SetAll"),
			Size = StiScale.I(new Size(100, 24)),
			Tag = e.TreeView
		};
		((Control)val2).Location = new Point(((Control)e.TreeView).Width / 2 - ((Control)val2).Width - StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4);
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val2);
		((Control)val2).Click += delegate(object sender2, EventArgs e2)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Button val7 = (Button)((sender2 is Button) ? sender2 : null);
			object tag2 = ((Control)val7).Tag;
			foreach (TreeNode node in ((TreeView)((tag2 is TreeView) ? tag2 : null)).Nodes)
			{
				TreeNode val8 = node;
				val8.Checked = true;
				foreach (TreeNode node2 in val8.Nodes)
				{
					TreeNode val9 = node2;
					val9.Checked = false;
				}
			}
		};
		Button val3 = new Button
		{
			UseVisualStyleBackColor = true,
			Text = StiLocalization.Get("Buttons", "RemoveAll"),
			Size = StiScale.I(new Size(100, 24)),
			Location = new Point(((Control)e.TreeView).Width / 2 + StiScale.I2, ((Control)e.TreeView).Height + StiScale.I4),
			Tag = e.TreeView
		};
		((Control)e.TreeView).Parent.Controls.Add((Control)(object)val3);
		((Control)val3).Click += delegate(object sender3, EventArgs e3)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			Button val4 = (Button)((sender3 is Button) ? sender3 : null);
			object tag = ((Control)val4).Tag;
			foreach (TreeNode node3 in ((TreeView)((tag is TreeView) ? tag : null)).Nodes)
			{
				TreeNode val5 = node3;
				val5.Checked = false;
				foreach (TreeNode node4 in val5.Nodes)
				{
					TreeNode val6 = node4;
					val6.Checked = false;
				}
			}
		};
		((TreeView)e.TreeView).ExpandAll();
	}

	private void DoScaling()
	{
		((ButtonBase)tbFontStyleBold).Image = StiReportDesignImages.Editor.FontStyleBold();
		((ButtonBase)tbFontStyleItalic).Image = StiReportDesignImages.Editor.FontStyleItalic();
		((ButtonBase)tbFontStyleUnderline).Image = StiReportDesignImages.Editor.FontStyleUnderline();
		((ButtonBase)tbFontStyleStrikeout).Image = StiReportDesignImages.Editor.FontStyleStrikethrough();
		buttonPermissions.Image = StiReportDesignImages.Actions.SelectPermissions();
		((Control)buttonExpression).Width = ((Control)buttonExpression).Height;
	}

	private void UpdateState()
	{
		((Control)colorBoxBackColor).Enabled = buttonItemAllowBackColor.Checked;
		((Control)dtpValue).Visible = ((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime") && !IsExpression;
		((Control)checkBoxBool).Visible = ((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean") && !IsExpression;
		((Control)panelExpression).Visible = (((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime") && ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean")) || IsExpression;
		string text = ((Control)cbCondition).Text;
		((ComboBox)cbCondition).Items.Clear();
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		if (((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString") && ((Control)cbDataType).Text != StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		}
		if (((Control)cbDataType).Text == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"))
		{
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
			((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		}
		foreach (string item in ((ComboBox)cbCondition).Items)
		{
			if (item == text)
			{
				((Control)cbCondition).Text = text;
				return;
			}
		}
		((ListControl)cbCondition).SelectedIndex = 0;
	}

	private void Build()
	{
		List<StiMeter> list = new List<StiMeter>();
		list.AddRange(tableElement.Columns);
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		foreach (StiMeter item in list)
		{
			if (keyDataFielColumns != null && keyDataFielColumns.Contains(item.Key))
			{
				string label = StiLabelHelper.GetLabel(item);
				list2.Add(label);
			}
			if (keyDestinationColumns != null && keyDestinationColumns.Contains(item.Key))
			{
				string label2 = StiLabelHelper.GetLabel(item);
				list3.Add(label2);
			}
		}
		((Control)treeBoxField).Text = string.Join(", ", list2);
		toolTip.SetToolTip((Control)(object)treeBoxField, ((Control)treeBoxField).Text.Replace(", ", "\n"));
		((Control)treeBoxDestination).Text = string.Join(", ", list3);
		toolTip.SetToolTip((Control)(object)treeBoxDestination, ((Control)treeBoxDestination).Text.Replace(", ", "\n"));
		string text = StiFilterHelper.ConvertConditionToString(condition.Condition);
		string text2 = StiFilterHelper.ConvertDataTypeToString(condition.DataType);
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThan"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionGreaterThanOrEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThan"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionLessThanOrEqualTo"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionContaining"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionNotContaining"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionBeginningWith"));
		((ComboBox)cbCondition).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterConditionEndingWith"));
		((Control)cbCondition).Text = text;
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeNumeric"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeString"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"));
		((ComboBox)cbDataType).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"));
		((Control)cbDataType).Text = text2;
		if (text2 == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeDateTime"))
		{
			DateTime.TryParse(condition.Value, new CultureInfo("en-US", useUserOverride: false), DateTimeStyles.None, out var result);
			dtpValue.Value = result;
		}
		else if (text2 == StiLocalization.Get("PropertyEnum", "StiFilterDataTypeBoolean"))
		{
			bool.TryParse(condition.Value, out var result2);
			checkBoxBool.Checked = result2;
		}
		else
		{
			((Control)tbValue).Text = condition.Value;
		}
		StiToolButton stiToolButton = tbFontStyleBold;
		Font font = condition.Font;
		stiToolButton.Pushed = font != null && font.Bold;
		StiToolButton stiToolButton2 = tbFontStyleItalic;
		Font font2 = condition.Font;
		stiToolButton2.Pushed = font2 != null && font2.Italic;
		StiToolButton stiToolButton3 = tbFontStyleUnderline;
		Font font3 = condition.Font;
		stiToolButton3.Pushed = font3 != null && font3.Underline;
		StiToolButton stiToolButton4 = tbFontStyleStrikeout;
		Font font4 = condition.Font;
		stiToolButton4.Pushed = font4 != null && font4.Strikeout;
		colorBoxColor.SelectedColor = condition.ForeColor;
		colorBoxBackColor.SelectedColor = condition.BackColor;
		StiTableConditionPermissions permissions = condition.Permissions;
		buttonItemAllowFontBold.Checked = (permissions & StiTableConditionPermissions.FontStyleBold) > StiTableConditionPermissions.None;
		buttonItemAllowFontItalic.Checked = (permissions & StiTableConditionPermissions.FontStyleItalic) > StiTableConditionPermissions.None;
		buttonItemAllowFontUnderline.Checked = (permissions & StiTableConditionPermissions.FontStyleUnderline) > StiTableConditionPermissions.None;
		buttonItemAllowFontStrikeout.Checked = (permissions & StiTableConditionPermissions.FontStyleStrikeout) > StiTableConditionPermissions.None;
		buttonItemAllowColor.Checked = (permissions & StiTableConditionPermissions.ForeColor) > StiTableConditionPermissions.None;
		buttonItemAllowBackColor.Checked = (permissions & StiTableConditionPermissions.BackColor) > StiTableConditionPermissions.None;
		IsExpression = condition.IsExpression;
	}

	public StiTableElementCondition GetTableElementCondition()
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		Stimulsoft.Report.Components.StiFilterCondition stiFilterCondition = StiFilterHelper.ConvertStringToCondition(((Control)cbCondition).Text);
		Stimulsoft.Report.Components.StiFilterDataType stiFilterDataType = StiFilterHelper.ConvertStringToDataType(((Control)cbDataType).Text);
		string value = null;
		if (stiFilterDataType != Stimulsoft.Report.Components.StiFilterDataType.DateTime || IsExpression)
		{
			value = ((stiFilterDataType != Stimulsoft.Report.Components.StiFilterDataType.Boolean || IsExpression) ? ((Control)tbValue).Text : checkBoxBool.Checked.ToString());
		}
		else
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			try
			{
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
				value = dtpValue.Value.ToShortDateString();
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
		StiTableConditionPermissions stiTableConditionPermissions = StiTableConditionPermissions.None;
		if (buttonItemAllowFontBold.Checked)
		{
			stiTableConditionPermissions |= StiTableConditionPermissions.FontStyleBold;
		}
		if (buttonItemAllowFontItalic.Checked)
		{
			stiTableConditionPermissions |= StiTableConditionPermissions.FontStyleItalic;
		}
		if (buttonItemAllowFontUnderline.Checked)
		{
			stiTableConditionPermissions |= StiTableConditionPermissions.FontStyleUnderline;
		}
		if (buttonItemAllowFontStrikeout.Checked)
		{
			stiTableConditionPermissions |= StiTableConditionPermissions.FontStyleStrikeout;
		}
		if (buttonItemAllowColor.Checked)
		{
			stiTableConditionPermissions |= StiTableConditionPermissions.ForeColor;
		}
		if (buttonItemAllowBackColor.Checked)
		{
			stiTableConditionPermissions |= StiTableConditionPermissions.BackColor;
		}
		FontStyle val = (FontStyle)0;
		if (buttonItemAllowFontBold.Checked && tbFontStyleBold.Pushed)
		{
			val = (FontStyle)(val | 1);
		}
		if (buttonItemAllowFontItalic.Checked && tbFontStyleItalic.Pushed)
		{
			val = (FontStyle)(val | 2);
		}
		if (buttonItemAllowFontUnderline.Checked && tbFontStyleUnderline.Pushed)
		{
			val = (FontStyle)(val | 4);
		}
		if (buttonItemAllowFontStrikeout.Checked && tbFontStyleStrikeout.Pushed)
		{
			val = (FontStyle)(val | 8);
		}
		Font font = new Font("Arial", 8f, val);
		Color selectedColor = colorBoxColor.SelectedColor;
		Color selectedColor2 = colorBoxBackColor.SelectedColor;
		return new StiTableElementCondition(keyDataFielColumns.ToArray(), keyDestinationColumns.ToArray(), stiFilterDataType, stiFilterCondition, value, stiTableConditionPermissions, font, selectedColor, selectedColor2, IsExpression);
	}

	private void CheckAllows()
	{
		((Control)tbFontStyleBold).Enabled = buttonItemAllowFontBold.Checked;
		((Control)tbFontStyleItalic).Enabled = buttonItemAllowFontItalic.Checked;
		((Control)tbFontStyleUnderline).Enabled = buttonItemAllowFontUnderline.Checked;
		((Control)tbFontStyleStrikeout).Enabled = buttonItemAllowFontStrikeout.Checked;
		((Control)colorBoxColor).Enabled = buttonItemAllowColor.Checked;
		((Control)colorBoxBackColor).Enabled = buttonItemAllowBackColor.Checked;
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
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Expected O, but got Unknown
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c34: Expected O, but got Unknown
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_103c: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_1307: Unknown result type (might be due to invalid IL or missing references)
		//IL_1328: Unknown result type (might be due to invalid IL or missing references)
		//IL_139b: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ff: Expected O, but got Unknown
		//IL_141c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1635: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiTableElementConditionsControl));
		lbFieldIs = new StiHitTestVisibleLabel();
		cbCondition = new StiComboBox();
		lbCondition = new StiHitTestVisibleLabel();
		lbValue = new StiHitTestVisibleLabel();
		groupLine = new StiGroupLine();
		tbFontStyleUnderline = new StiToolButton();
		tbFontStyleItalic = new StiToolButton();
		tbFontStyleBold = new StiToolButton();
		colorBoxBackColor = new ColorPickerButton();
		lbBackColor = new StiHitTestVisibleLabel();
		buttonPermissions = new ButtonX();
		buttonItemAllowFontBold = new ButtonItem();
		buttonItemAllowFontItalic = new ButtonItem();
		buttonItemAllowFontUnderline = new ButtonItem();
		buttonItemAllowFontStrikeout = new ButtonItem();
		buttonItemAllowColor = new ButtonItem();
		buttonItemAllowBackColor = new ButtonItem();
		tbFontStyleStrikeout = new StiToolButton();
		cbDataType = new StiComboBox();
		lbDataType = new StiHitTestVisibleLabel();
		colorBoxColor = new ColorPickerButton();
		lbColor = new StiHitTestVisibleLabel();
		dtpValue = new DateTimePicker();
		checkBoxBool = new CheckBox();
		treeBoxField = new StiTreeViewBox();
		treeBoxDestination = new StiTreeViewBox();
		lbDestination = new StiHitTestVisibleLabel();
		comboBoxExpression = new StiComboBox();
		panelExpression = new StiPanelBox();
		tbValue = new HintTextBox();
		buttonExpression = new StiOperationButton();
		((Control)panelExpression).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lbFieldIs).BackColor = Color.Transparent;
		((Control)lbFieldIs).ForeColor = Color.DimGray;
		((Control)lbFieldIs).Location = new Point(28, 23);
		((Control)lbFieldIs).Margin = new Padding(6, 0, 6, 0);
		((Control)lbFieldIs).Name = "lbFieldIs";
		((Control)lbFieldIs).Size = new Size(194, 25);
		((Control)lbFieldIs).TabIndex = 100;
		((Control)lbFieldIs).Text = "Data Field";
		cbCondition.AutoDropDownWidth = false;
		((Control)cbCondition).BackColor = SystemColors.Window;
		cbCondition.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)cbCondition).DropDownWidth = 300;
		((Control)cbCondition).Location = new Point(636, 54);
		((Control)cbCondition).Margin = new Padding(6);
		((Control)cbCondition).Name = "cbCondition";
		((Control)cbCondition).Size = new Size(204, 42);
		((Control)cbCondition).TabIndex = 202;
		((Control)lbCondition).BackColor = Color.Transparent;
		((Control)lbCondition).ForeColor = Color.DimGray;
		((Control)lbCondition).Location = new Point(636, 23);
		((Control)lbCondition).Margin = new Padding(6, 0, 6, 0);
		((Control)lbCondition).Name = "lbCondition";
		((Control)lbCondition).Size = new Size(208, 25);
		((Control)lbCondition).TabIndex = 102;
		((Control)lbCondition).Text = "Condition";
		((Control)lbValue).BackColor = Color.Transparent;
		((Control)lbValue).ForeColor = Color.DimGray;
		((Control)lbValue).Location = new Point(866, 23);
		((Control)lbValue).Margin = new Padding(6, 0, 6, 0);
		((Control)lbValue).Name = "lbValue";
		((Control)lbValue).Size = new Size(350, 25);
		((Control)lbValue).TabIndex = 103;
		((Control)lbValue).Text = "Value";
		((Control)groupLine).BackColor = Color.FromArgb(250, 250, 250);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Bottom;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 202);
		((Control)groupLine).Margin = new Padding(6);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(1300, 4);
		((Control)groupLine).TabIndex = 72;
		tbFontStyleUnderline.Draw3DButton = false;
		tbFontStyleUnderline.HotDragDrop = false;
		((ButtonBase)tbFontStyleUnderline).Image = (Image)componentResourceManager.GetObject("tbFontStyleUnderline.Image");
		((Control)tbFontStyleUnderline).Location = new Point(518, 137);
		((Control)tbFontStyleUnderline).Margin = new Padding(6);
		((Control)tbFontStyleUnderline).Name = "tbFontStyleUnderline";
		((Control)tbFontStyleUnderline).Size = new Size(48, 46);
		tbFontStyleUnderline.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleUnderline).TabIndex = 303;
		((ButtonBase)tbFontStyleUnderline).UseVisualStyleBackColor = false;
		tbFontStyleItalic.Draw3DButton = false;
		tbFontStyleItalic.HotDragDrop = false;
		((ButtonBase)tbFontStyleItalic).Image = (Image)componentResourceManager.GetObject("tbFontStyleItalic.Image");
		((Control)tbFontStyleItalic).Location = new Point(470, 137);
		((Control)tbFontStyleItalic).Margin = new Padding(6);
		((Control)tbFontStyleItalic).Name = "tbFontStyleItalic";
		((Control)tbFontStyleItalic).Size = new Size(48, 46);
		tbFontStyleItalic.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleItalic).TabIndex = 302;
		((ButtonBase)tbFontStyleItalic).UseVisualStyleBackColor = false;
		tbFontStyleBold.Draw3DButton = false;
		tbFontStyleBold.HotDragDrop = false;
		((ButtonBase)tbFontStyleBold).Image = (Image)componentResourceManager.GetObject("tbFontStyleBold.Image");
		((Control)tbFontStyleBold).Location = new Point(422, 137);
		((Control)tbFontStyleBold).Margin = new Padding(6);
		((Control)tbFontStyleBold).Name = "tbFontStyleBold";
		((Control)tbFontStyleBold).Size = new Size(48, 46);
		tbFontStyleBold.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleBold).TabIndex = 301;
		((ButtonBase)tbFontStyleBold).UseVisualStyleBackColor = false;
		((Control)colorBoxBackColor).AccessibleRole = (AccessibleRole)43;
		colorBoxBackColor.AutoExpandOnClick = true;
		colorBoxBackColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxBackColor.EnableMarkup = false;
		((Control)colorBoxBackColor).Location = new Point(864, 137);
		colorBoxBackColor.LockClosePopup = false;
		((Control)colorBoxBackColor).Margin = new Padding(4);
		((Control)colorBoxBackColor).Name = "colorBoxBackColor";
		((Control)colorBoxBackColor).Size = new Size(204, 46);
		((Control)colorBoxBackColor).TabIndex = 306;
		colorBoxBackColor.TextAlignment = eButtonTextAlignment.Right;
		((Control)lbBackColor).BackColor = Color.Transparent;
		((Control)lbBackColor).ForeColor = Color.DimGray;
		((Control)lbBackColor).Location = new Point(866, 106);
		((Control)lbBackColor).Margin = new Padding(6, 0, 6, 0);
		((Control)lbBackColor).Name = "lbBackColor";
		((Control)lbBackColor).Size = new Size(208, 25);
		((Control)lbBackColor).TabIndex = 34;
		((Control)lbBackColor).Text = "Back Color";
		((Control)buttonPermissions).AccessibleRole = (AccessibleRole)43;
		buttonPermissions.AutoExpandOnClick = true;
		buttonPermissions.ColorTable = eButtonColor.OrangeWithBackground;
		buttonPermissions.Image = (Image)componentResourceManager.GetObject("buttonPermissions.Image");
		((Control)buttonPermissions).Location = new Point(332, 137);
		buttonPermissions.LockClosePopup = false;
		((Control)buttonPermissions).Margin = new Padding(6);
		((Control)buttonPermissions).Name = "buttonPermissions";
		((Control)buttonPermissions).Size = new Size(82, 44);
		buttonPermissions.Style = eDotNetBarStyle.StyleManagerControlled;
		buttonPermissions.SubItems.AddRange(new BaseItem[6] { buttonItemAllowFontBold, buttonItemAllowFontItalic, buttonItemAllowFontUnderline, buttonItemAllowFontStrikeout, buttonItemAllowColor, buttonItemAllowBackColor });
		((Control)buttonPermissions).TabIndex = 300;
		buttonPermissions.ThemeAware = true;
		buttonItemAllowFontBold.AutoCheckOnClick = true;
		buttonItemAllowFontBold.AutoCollapseOnClick = false;
		buttonItemAllowFontBold.BeginGroup = true;
		buttonItemAllowFontBold.Checked = true;
		buttonItemAllowFontBold.GlobalItem = false;
		buttonItemAllowFontBold.ImagePaddingHorizontal = 16;
		buttonItemAllowFontBold.ImagePaddingVertical = 12;
		buttonItemAllowFontBold.Name = "buttonItemAllowFontBold";
		buttonItemAllowFontBold.Text = "Font Bold";
		buttonItemAllowFontBold.ThemeAware = true;
		buttonItemAllowFontBold.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontItalic.AutoCheckOnClick = true;
		buttonItemAllowFontItalic.AutoCollapseOnClick = false;
		buttonItemAllowFontItalic.Checked = true;
		buttonItemAllowFontItalic.GlobalItem = false;
		buttonItemAllowFontItalic.ImagePaddingHorizontal = 16;
		buttonItemAllowFontItalic.ImagePaddingVertical = 12;
		buttonItemAllowFontItalic.Name = "buttonItemAllowFontItalic";
		buttonItemAllowFontItalic.Text = "Font Italic";
		buttonItemAllowFontItalic.ThemeAware = true;
		buttonItemAllowFontItalic.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontUnderline.AutoCheckOnClick = true;
		buttonItemAllowFontUnderline.AutoCollapseOnClick = false;
		buttonItemAllowFontUnderline.Checked = true;
		buttonItemAllowFontUnderline.GlobalItem = false;
		buttonItemAllowFontUnderline.ImagePaddingHorizontal = 16;
		buttonItemAllowFontUnderline.ImagePaddingVertical = 12;
		buttonItemAllowFontUnderline.Name = "buttonItemAllowFontUnderline";
		buttonItemAllowFontUnderline.Text = "Font Underline";
		buttonItemAllowFontUnderline.ThemeAware = true;
		buttonItemAllowFontUnderline.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowFontStrikeout.AutoCheckOnClick = true;
		buttonItemAllowFontStrikeout.AutoCollapseOnClick = false;
		buttonItemAllowFontStrikeout.Checked = true;
		buttonItemAllowFontStrikeout.GlobalItem = false;
		buttonItemAllowFontStrikeout.ImagePaddingHorizontal = 16;
		buttonItemAllowFontStrikeout.ImagePaddingVertical = 12;
		buttonItemAllowFontStrikeout.Name = "buttonItemAllowFontStrikeout";
		buttonItemAllowFontStrikeout.Text = "Font Strikeout";
		buttonItemAllowFontStrikeout.ThemeAware = true;
		buttonItemAllowFontStrikeout.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowColor.AutoCheckOnClick = true;
		buttonItemAllowColor.AutoCollapseOnClick = false;
		buttonItemAllowColor.BeginGroup = true;
		buttonItemAllowColor.Checked = true;
		buttonItemAllowColor.GlobalItem = false;
		buttonItemAllowColor.ImagePaddingHorizontal = 16;
		buttonItemAllowColor.ImagePaddingVertical = 12;
		buttonItemAllowColor.Name = "buttonItemAllowColor";
		buttonItemAllowColor.Text = "Text Color";
		buttonItemAllowColor.ThemeAware = true;
		buttonItemAllowColor.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		buttonItemAllowBackColor.AutoCheckOnClick = true;
		buttonItemAllowBackColor.AutoCollapseOnClick = false;
		buttonItemAllowBackColor.Checked = true;
		buttonItemAllowBackColor.GlobalItem = false;
		buttonItemAllowBackColor.ImagePaddingHorizontal = 16;
		buttonItemAllowBackColor.ImagePaddingVertical = 12;
		buttonItemAllowBackColor.Name = "buttonItemAllowBackColor";
		buttonItemAllowBackColor.Text = "Back Color";
		buttonItemAllowBackColor.ThemeAware = true;
		buttonItemAllowBackColor.CheckedChanged += ButtonItemAllowFontName_CheckedChanged;
		tbFontStyleStrikeout.Draw3DButton = false;
		tbFontStyleStrikeout.HotDragDrop = false;
		((ButtonBase)tbFontStyleStrikeout).Image = (Image)componentResourceManager.GetObject("tbFontStyleStrikeout.Image");
		((Control)tbFontStyleStrikeout).Location = new Point(566, 137);
		((Control)tbFontStyleStrikeout).Margin = new Padding(6);
		((Control)tbFontStyleStrikeout).Name = "tbFontStyleStrikeout";
		((Control)tbFontStyleStrikeout).Size = new Size(48, 46);
		tbFontStyleStrikeout.Style = StiToolBarButtonStyle.PushButton;
		((Control)tbFontStyleStrikeout).TabIndex = 304;
		((ButtonBase)tbFontStyleStrikeout).UseVisualStyleBackColor = false;
		((Control)cbDataType).BackColor = SystemColors.Window;
		cbDataType.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbDataType).Location = new Point(334, 54);
		((Control)cbDataType).Margin = new Padding(6);
		((Control)cbDataType).Name = "cbDataType";
		((Control)cbDataType).Size = new Size(278, 42);
		((Control)cbDataType).TabIndex = 201;
		((ComboBox)cbDataType).SelectedIndexChanged += ComboBoxDataType_SelectedIndexChanged;
		((Control)lbDataType).BackColor = Color.Transparent;
		((Control)lbDataType).ForeColor = Color.DimGray;
		((Control)lbDataType).Location = new Point(334, 23);
		((Control)lbDataType).Margin = new Padding(6, 0, 6, 0);
		((Control)lbDataType).Name = "lbDataType";
		((Control)lbDataType).Size = new Size(194, 25);
		((Control)lbDataType).TabIndex = 101;
		((Control)lbDataType).Text = "Data Type";
		((Control)colorBoxColor).AccessibleRole = (AccessibleRole)43;
		colorBoxColor.AutoExpandOnClick = true;
		colorBoxColor.ColorTable = eButtonColor.OrangeWithBackground;
		colorBoxColor.EnableMarkup = false;
		((Control)colorBoxColor).Location = new Point(636, 137);
		colorBoxColor.LockClosePopup = false;
		((Control)colorBoxColor).Margin = new Padding(4);
		((Control)colorBoxColor).Name = "colorBoxColor";
		((Control)colorBoxColor).Size = new Size(204, 46);
		((Control)colorBoxColor).TabIndex = 305;
		colorBoxColor.TextAlignment = eButtonTextAlignment.Right;
		((Control)lbColor).BackColor = Color.Transparent;
		((Control)lbColor).ForeColor = Color.DimGray;
		((Control)lbColor).Location = new Point(636, 106);
		((Control)lbColor).Margin = new Padding(6, 0, 6, 0);
		((Control)lbColor).Name = "lbColor";
		((Control)lbColor).Size = new Size(208, 25);
		((Control)lbColor).TabIndex = 95;
		((Control)lbColor).Text = "Color";
		dtpValue.Format = (DateTimePickerFormat)2;
		((Control)dtpValue).Location = new Point(864, 54);
		((Control)dtpValue).Margin = new Padding(6);
		((Control)dtpValue).Name = "dtpValue";
		((Control)dtpValue).Size = new Size(200, 31);
		((Control)dtpValue).TabIndex = 203;
		((Control)checkBoxBool).AutoSize = true;
		((Control)checkBoxBool).Location = new Point(864, 60);
		((Control)checkBoxBool).Margin = new Padding(6);
		((Control)checkBoxBool).Name = "checkBoxBool";
		((Control)checkBoxBool).Size = new Size(28, 27);
		((Control)checkBoxBool).TabIndex = 204;
		((ButtonBase)checkBoxBool).UseVisualStyleBackColor = true;
		((Control)treeBoxField).BackColor = SystemColors.Window;
		treeBoxField.DropDownWidth = 250;
		treeBoxField.Flat = false;
		((Control)treeBoxField).ForeColor = SystemColors.ControlText;
		((Control)treeBoxField).Location = new Point(28, 54);
		((Control)treeBoxField).Margin = new Padding(6);
		treeBoxField.MaxDropDownHeight = 260;
		((Control)treeBoxField).Name = "treeBoxField";
		((Control)treeBoxField).Size = new Size(282, 42);
		((Control)treeBoxField).TabIndex = 307;
		treeBoxField.TreeNodesFill += TreeBoxField_TreeNodesFill;
		((Control)treeBoxDestination).BackColor = SystemColors.Window;
		treeBoxDestination.DropDownWidth = 250;
		treeBoxDestination.Flat = false;
		((Control)treeBoxDestination).ForeColor = SystemColors.ControlText;
		((Control)treeBoxDestination).Location = new Point(28, 137);
		((Control)treeBoxDestination).Margin = new Padding(6);
		treeBoxDestination.MaxDropDownHeight = 260;
		((Control)treeBoxDestination).Name = "treeBoxDestination";
		((Control)treeBoxDestination).Size = new Size(282, 42);
		((Control)treeBoxDestination).TabIndex = 309;
		treeBoxDestination.TreeNodesFill += TreeBoxDestination_TreeNodesFill;
		((Control)lbDestination).BackColor = Color.Transparent;
		((Control)lbDestination).ForeColor = Color.DimGray;
		((Control)lbDestination).Location = new Point(28, 106);
		((Control)lbDestination).Margin = new Padding(6, 0, 6, 0);
		((Control)lbDestination).Name = "lbDestination";
		((Control)lbDestination).Size = new Size(194, 25);
		((Control)lbDestination).TabIndex = 308;
		((Control)lbDestination).Text = "Destination";
		((Control)comboBoxExpression).Anchor = (AnchorStyles)13;
		comboBoxExpression.AutoDropDownWidth = false;
		((Control)comboBoxExpression).BackColor = SystemColors.Window;
		comboBoxExpression.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxExpression).DropDownWidth = 96;
		((Control)comboBoxExpression).Location = new Point(1092, 54);
		((Control)comboBoxExpression).Margin = new Padding(6);
		((Control)comboBoxExpression).Name = "comboBoxExpression";
		((Control)comboBoxExpression).Size = new Size(188, 42);
		((Control)comboBoxExpression).TabIndex = 310;
		((ComboBox)comboBoxExpression).SelectedIndexChanged += ComboBoxExpression_SelectedIndexChanged;
		((Control)panelExpression).BackColor = Color.White;
		((Control)panelExpression).Controls.Add((Control)(object)tbValue);
		((Control)panelExpression).Controls.Add((Control)(object)buttonExpression);
		((Control)panelExpression).Location = new Point(864, 54);
		((Control)panelExpression).Margin = new Padding(6);
		((Control)panelExpression).Name = "panelExpression";
		((Control)panelExpression).Padding = new Padding(2);
		((Control)panelExpression).Size = new Size(204, 40);
		((Control)panelExpression).TabIndex = 311;
		((Control)tbValue).Anchor = (AnchorStyles)15;
		((Control)tbValue).BackColor = SystemColors.Window;
		((TextBoxBase)tbValue).BorderStyle = (BorderStyle)0;
		((Control)tbValue).Location = new Point(8, 8);
		((Control)tbValue).Margin = new Padding(6);
		((Control)tbValue).Name = "tbValue";
		((Control)tbValue).Size = new Size(136, 24);
		((Control)tbValue).TabIndex = 0;
		((Control)buttonExpression).Dock = (DockStyle)4;
		((ButtonBase)buttonExpression).Image = (Image)componentResourceManager.GetObject("buttonExpression.Image");
		((Control)buttonExpression).Location = new Point(158, 2);
		((Control)buttonExpression).Margin = new Padding(6);
		((Control)buttonExpression).Name = "buttonExpression";
		((Control)buttonExpression).Size = new Size(44, 36);
		((Control)buttonExpression).TabIndex = 1;
		((Control)buttonExpression).TabStop = false;
		((ButtonBase)buttonExpression).UseVisualStyleBackColor = true;
		((Control)buttonExpression).Click += ButtonExpression_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelExpression);
		((Control)this).Controls.Add((Control)(object)comboBoxExpression);
		((Control)this).Controls.Add((Control)(object)treeBoxDestination);
		((Control)this).Controls.Add((Control)(object)lbDestination);
		((Control)this).Controls.Add((Control)(object)treeBoxField);
		((Control)this).Controls.Add((Control)(object)colorBoxColor);
		((Control)this).Controls.Add((Control)(object)lbColor);
		((Control)this).Controls.Add((Control)(object)cbDataType);
		((Control)this).Controls.Add((Control)(object)lbDataType);
		((Control)this).Controls.Add((Control)(object)colorBoxBackColor);
		((Control)this).Controls.Add((Control)(object)tbFontStyleStrikeout);
		((Control)this).Controls.Add((Control)(object)buttonPermissions);
		((Control)this).Controls.Add((Control)(object)tbFontStyleUnderline);
		((Control)this).Controls.Add((Control)(object)tbFontStyleItalic);
		((Control)this).Controls.Add((Control)(object)tbFontStyleBold);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Controls.Add((Control)(object)lbBackColor);
		((Control)this).Controls.Add((Control)(object)lbFieldIs);
		((Control)this).Controls.Add((Control)(object)cbCondition);
		((Control)this).Controls.Add((Control)(object)lbCondition);
		((Control)this).Controls.Add((Control)(object)lbValue);
		((Control)this).Controls.Add((Control)(object)dtpValue);
		((Control)this).Controls.Add((Control)(object)checkBoxBool);
		((Control)this).Margin = new Padding(8);
		((Control)this).Name = "StiTableElementConditionsControl";
		((Control)this).Size = new Size(1300, 206);
		((Control)panelExpression).ResumeLayout(false);
		((Control)panelExpression).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
