using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Helpers;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Win.Parameters.Value;

namespace Stimulsoft.Report.Win.Parameters.List;

[ToolboxItem(false)]
public class StiListButtonXControl : UserControl, IStiValueItemControl
{
	private StiListType listType;

	private StiButtonListInfo buttonInfo;

	private ButtonItem buttonAdd;

	private ButtonItem buttonRemoveAll;

	private ButtonItem buttonClose;

	private ControlContainerItem controlContainerItem;

	private FlowLayoutPanel listAllowUserValues;

	private ControlContainerItem controlContainerFind;

	private FlowLayoutPanel panelFind;

	private StiHintTextBox textBoxFind;

	private ButtonItem buttonSelectAll;

	private bool lockCheckState;

	private IContainer components;

	private ButtonX mainButton;

	public object Value => GetResult();

	public StiVariable Variable { get; }

	public StiReport Report { get; }

	public event EventHandler SelectedValue;

	public void InvokeSelectedValue()
	{
		bool flag = StiVariableHelper.ApplyVariableValue(Report, Variable, Value);
		object value = Value;
		value = ((value is IStiList) ? (value as IStiList).ToObjectArray() : value);
		if (value != null && this.SelectedValue != null)
		{
			this.SelectedValue(flag ? null : value, EventArgs.Empty);
		}
	}

	public void FilterValues(object selectedObject)
	{
		StiVariableHelper.RefreshDialogInfo(Report, Variable);
		List<StiDialogInfoItem> dialogInfoItems = Variable.DialogInfo.GetDialogInfoItems(Variable.Type);
		HashSet<StiDialogInfoItem> hashSet = new HashSet<StiDialogInfoItem>();
		object[] array = StiVariableBindingHelper.GetArray(selectedObject);
		foreach (StiDialogInfoItem item in dialogInfoItems)
		{
			if (item.ValueBinding == null)
			{
				continue;
			}
			foreach (object item2 in item.ValueBinding)
			{
				if (StiVariableBindingHelper.IsEqualBinding(item2, array) && !hashSet.Any((StiDialogInfoItem i) => i.KeyObject.Equals(item.KeyObject)))
				{
					hashSet.Add(item);
				}
			}
		}
		((Control)listAllowUserValues).Width = StiScale2.XXI(275.0);
		((Control)listAllowUserValues).Controls.Clear();
		foreach (StiDialogInfoItem item3 in hashSet)
		{
			StiDialogInfoItem dialogInfoItem = GetDialogInfoItem(item3.KeyObject, item3.Value, item3.ValueBinding);
			if (dialogInfoItem != null)
			{
				AddNewItem(dialogInfoItem, checkSize: false);
			}
		}
		RefreshItemSelectionText();
		UpdateFlowLayoutPanelSize();
	}

	private StiDialogInfoItem GetDialogInfoItem(object key, string value, List<object> valueBinding)
	{
		switch (listType)
		{
		case StiListType.DateTimeList:
			return new StiDateTimeDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.TimeSpanList:
			return new StiTimeSpanDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.CharList:
			return new StiCharDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.FloatList:
		case StiListType.DoubleList:
			return new StiDoubleDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.DecimalList:
			return new StiDecimalDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.GuidList:
			return new StiGuidDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.StringList:
			return new StiStringDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.ByteList:
		case StiListType.ShortList:
		case StiListType.IntList:
		case StiListType.LongList:
			return new StiLongDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		case StiListType.BoolList:
			return new StiBoolDialogInfoItem
			{
				KeyObject = key,
				Value = value,
				ValueBinding = valueBinding
			};
		default:
			return null;
		}
	}

	private void ButtonAdd_Click(object sender, EventArgs e)
	{
		AddNewItem(null, checkSize: true);
	}

	private void ButtonRemoveAll_Click(object sender, EventArgs e)
	{
		((Control)listAllowUserValues).SuspendLayout();
		((Control)listAllowUserValues).Controls.Clear();
		UpdateFlowLayoutPanelSize();
		((Control)listAllowUserValues).Width = StiScale2.XXI(275.0);
		((Control)listAllowUserValues).ResumeLayout(true);
	}

	private void MainButton_PopupClose(object sender, EventArgs e)
	{
		RefreshItemSelectionText();
		InvokeSelectedValue();
		StiVariableHelper.SetVariableLabel(Report, Variable, GetValuesString(allowAll: false));
	}

	private void ButtonSelectAll_Click(object sender, EventArgs e)
	{
		lockCheckState = true;
		buttonSelectAll.Checked = !buttonSelectAll.Checked;
		foreach (object item in (ArrangedElementCollection)((Control)listAllowUserValues).Controls)
		{
			CheckBox val = (CheckBox)((item is CheckBox) ? item : null);
			if (val != null && ((Control)val).Name != "buttonSelectAll")
			{
				val.Checked = buttonSelectAll.Checked;
			}
		}
		lockCheckState = false;
	}

	private void TextBox_KeyUp(object sender, EventArgs e)
	{
		StiHintTextBox val = (StiHintTextBox)((sender is StiHintTextBox) ? sender : null);
		string text = ((Control)val).Text.ToLower();
		((Control)listAllowUserValues).Visible = false;
		foreach (object item in (ArrangedElementCollection)((Control)listAllowUserValues).Controls)
		{
			CheckBox val2 = (CheckBox)((item is CheckBox) ? item : null);
			StiDialogInfoItem stiDialogInfoItem = ((Control)val2).Tag as StiDialogInfoItem;
			if (val2 != null && stiDialogInfoItem != null)
			{
				string text2 = (string.IsNullOrEmpty(stiDialogInfoItem.Value) ? stiDialogInfoItem.KeyObject.ToString() : stiDialogInfoItem.Value);
				((Control)val2).Visible = text2.ToLowerInvariant().Contains(text.ToLowerInvariant());
			}
		}
		((Control)listAllowUserValues).Visible = true;
	}

	private void RefreshItemSelectionText()
	{
		string valuesString = GetValuesString();
		((Control)mainButton).Text = ((valuesString.Length >= 23) ? (valuesString.Substring(0, 23) + "...") : valuesString);
	}

	private string GetValuesString(bool allowAll = true)
	{
		string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
		StringBuilder stringBuilder = new StringBuilder();
		if (buttonInfo.Variable.DialogInfo.AllowUserValues)
		{
			for (int i = 0; i < ((ArrangedElementCollection)((Control)listAllowUserValues).Controls).Count; i++)
			{
				if (((Control)listAllowUserValues).Controls[i] is IStiListItemControl stiListItemControl)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(listSeparator + " ");
					}
					stringBuilder.Append(stiListItemControl.GetValueStr());
				}
			}
		}
		else
		{
			bool flag = true;
			bool flag2 = true;
			for (int j = 0; j < ((ArrangedElementCollection)((Control)listAllowUserValues).Controls).Count; j++)
			{
				Control obj = ((Control)listAllowUserValues).Controls[j];
				CheckBox val = (CheckBox)(object)((obj is CheckBox) ? obj : null);
				if (val != null && val.Checked)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(listSeparator + " ");
					}
					stringBuilder.Append(((Control)val).Text);
					flag2 = false;
				}
				else
				{
					flag = false;
				}
			}
			if ((flag || flag2) && allowAll)
			{
				stringBuilder.Clear();
				if (flag2)
				{
					stringBuilder.Append("(" + string.Format(Loc.Get("Dashboard", "NSelected"), "0") + ")");
				}
				else
				{
					stringBuilder.Append("(" + Loc.Get("Report", "RangeAll") + ")");
				}
			}
		}
		return stringBuilder.ToString();
	}

	internal void UpdateFlowLayoutPanelSize()
	{
		if (((ArrangedElementCollection)((Control)listAllowUserValues).Controls).Count == 0)
		{
			((BaseItem)controlContainerItem).Visible = false;
			if (controlContainerFind != null)
			{
				((BaseItem)controlContainerFind).Visible = false;
			}
			if (buttonRemoveAll != null)
			{
				((BaseItem)buttonRemoveAll).Enabled = false;
			}
		}
		else
		{
			if (buttonRemoveAll != null)
			{
				((BaseItem)buttonRemoveAll).Enabled = buttonInfo.Variable.DialogInfo.AllowUserValues;
			}
			((BaseItem)controlContainerItem).Visible = true;
			if (controlContainerFind != null)
			{
				((BaseItem)controlContainerFind).Visible = true;
			}
		}
	}

	private void AddNewItem(StiDialogInfoItem infoItem, bool checkSize)
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		if (buttonInfo.Variable.DialogInfo.AllowUserValues)
		{
			IStiListItemControl stiListItemControl = null;
			switch (listType)
			{
			case StiListType.DateTimeList:
				stiListItemControl = new StiListDateTimeItemControl(buttonInfo, infoItem, mainButton);
				break;
			case StiListType.TimeSpanList:
				stiListItemControl = new StiListTimeSpanItemControl(buttonInfo, infoItem);
				break;
			case StiListType.CharList:
				stiListItemControl = new StiListCharItemControl(buttonInfo, infoItem);
				break;
			case StiListType.FloatList:
			case StiListType.DoubleList:
			case StiListType.DecimalList:
				stiListItemControl = new StiListDoubleItemControl(buttonInfo, infoItem);
				break;
			case StiListType.GuidList:
				stiListItemControl = new StiListGuidItemControl(buttonInfo, infoItem);
				break;
			case StiListType.StringList:
				stiListItemControl = new StiListStringItemControl(buttonInfo, infoItem);
				break;
			case StiListType.ByteList:
			case StiListType.ShortList:
			case StiListType.IntList:
			case StiListType.LongList:
				stiListItemControl = new StiListLongItemControl(buttonInfo, infoItem);
				break;
			case StiListType.BoolList:
				stiListItemControl = new StiListBoolItemControl(buttonInfo, infoItem);
				break;
			}
			if (stiListItemControl != null)
			{
				if (checkSize)
				{
					((Control)listAllowUserValues).SuspendLayout();
					((Control)listAllowUserValues).Controls.Add((Control)((stiListItemControl is UserControl) ? stiListItemControl : null));
					UpdateFlowLayoutPanelSize();
					((Control)listAllowUserValues).ResumeLayout(true);
					stiListItemControl.SetFocus();
					((ScrollableControl)listAllowUserValues).AutoScroll = true;
				}
				else
				{
					((Control)listAllowUserValues).Controls.Add((Control)((stiListItemControl is UserControl) ? stiListItemControl : null));
				}
			}
			return;
		}
		CheckBox val = new CheckBox
		{
			Width = ((Control)listAllowUserValues).Width - StiScale2.XXI(42.0),
			Height = StiScale2.YYI(20.0),
			Margin = new Padding(2),
			Checked = infoItem.Checked,
			Tag = infoItem
		};
		if (string.IsNullOrEmpty(infoItem.Value))
		{
			if (listType == StiListType.DateTimeList && infoItem.KeyObject is DateTime dateTime)
			{
				switch (buttonInfo.Variable.DialogInfo.DateTimeType)
				{
				case StiDateTimeType.DateAndTime:
					((Control)val).Text = dateTime.ToString();
					break;
				case StiDateTimeType.Date:
					((Control)val).Text = dateTime.ToShortDateString();
					break;
				case StiDateTimeType.Time:
					((Control)val).Text = dateTime.ToShortTimeString();
					break;
				}
			}
			else
			{
				((Control)val).Text = ((infoItem.KeyObject == null) ? string.Empty : infoItem.KeyObject.ToString());
			}
		}
		else
		{
			((Control)val).Text = infoItem.Value;
		}
		val.CheckedChanged += delegate
		{
			CheckStateButtonSelectAll();
		};
		((Control)listAllowUserValues).Controls.Add((Control)(object)val);
	}

	internal void RemoveItem(Control control)
	{
		((Control)listAllowUserValues).SuspendLayout();
		((Control)listAllowUserValues).Controls.Remove(control);
		UpdateFlowLayoutPanelSize();
		((Control)listAllowUserValues).ResumeLayout(true);
	}

	private void InitAllowUserValues()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		buttonAdd = new ButtonItem();
		controlContainerItem = new ControlContainerItem();
		listAllowUserValues = new FlowLayoutPanel();
		buttonRemoveAll = new ButtonItem();
		buttonClose = new ButtonItem();
		mainButton.SubItems.AddRange((BaseItem[])(object)new BaseItem[4]
		{
			(BaseItem)buttonAdd,
			(BaseItem)controlContainerItem,
			(BaseItem)buttonRemoveAll,
			(BaseItem)buttonClose
		});
		((BaseItem)buttonAdd).AutoCollapseOnClick = false;
		((BaseItem)buttonAdd).GlobalItem = false;
		((BaseItem)buttonAdd).Name = "buttonAdd";
		((BaseItem)buttonAdd).ThemeAware = true;
		((BaseItem)buttonAdd).Click += ButtonAdd_Click;
		controlContainerItem.AllowItemResize = false;
		((BaseItem)controlContainerItem).BeginGroup = true;
		controlContainerItem.Control = (Control)(object)listAllowUserValues;
		((BaseItem)controlContainerItem).GlobalItem = false;
		controlContainerItem.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)controlContainerItem).Name = "controlContainerItem";
		((BaseItem)controlContainerItem).ThemeAware = true;
		((BaseItem)controlContainerItem).Visible = false;
		((Control)listAllowUserValues).BackColor = Color.Transparent;
		((Control)listAllowUserValues).Location = new Point(0, 0);
		((Control)listAllowUserValues).MaximumSize = new Size(StiScale2.XXI(275.0), StiScale2.YYI(300.0));
		((Control)listAllowUserValues).Name = "listAllowUserValues";
		((Control)listAllowUserValues).Size = new Size(StiScale2.XXI(275.0), 0);
		((Control)listAllowUserValues).TabIndex = 0;
		listAllowUserValues.FlowDirection = (FlowDirection)1;
		listAllowUserValues.WrapContents = false;
		((Control)listAllowUserValues).Padding = new Padding(0, 0, StiScale2.XXI(20.0), 0);
		((Control)listAllowUserValues).AutoSize = true;
		((Panel)listAllowUserValues).AutoSizeMode = (AutoSizeMode)0;
		((BaseItem)buttonRemoveAll).AutoCollapseOnClick = false;
		((BaseItem)buttonRemoveAll).Enabled = false;
		((BaseItem)buttonRemoveAll).GlobalItem = false;
		((BaseItem)buttonRemoveAll).Name = "buttonRemoveAll";
		((BaseItem)buttonRemoveAll).Text = "Remove All";
		((BaseItem)buttonRemoveAll).ThemeAware = true;
		((BaseItem)buttonRemoveAll).Click += ButtonRemoveAll_Click;
		((BaseItem)buttonClose).BeginGroup = true;
		((BaseItem)buttonClose).GlobalItem = false;
		((BaseItem)buttonClose).Name = "buttonClose";
		((BaseItem)buttonClose).ThemeAware = true;
		((BaseItem)buttonClose).Text = StiLocalization.Get("Buttons", "Close");
		((BaseItem)buttonAdd).Text = StiLocalization.Get("FormDictionaryDesigner", "NewItem");
		((BaseItem)buttonRemoveAll).Text = StiLocalization.Get("Buttons", "RemoveAll");
	}

	private void Init()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		controlContainerItem = new ControlContainerItem();
		listAllowUserValues = new FlowLayoutPanel();
		controlContainerFind = new ControlContainerItem();
		panelFind = new FlowLayoutPanel();
		textBoxFind = new StiHintTextBox
		{
			Dock = (DockStyle)5,
			Hint = StiLocalization.Get("Editor", "TypeToSearch"),
			Width = StiScale2.XXI(253.0)
		};
		((Control)textBoxFind).KeyUp += new KeyEventHandler(TextBox_KeyUp);
		((Control)panelFind).Controls.Add((Control)(object)textBoxFind);
		buttonSelectAll = new ButtonItem();
		mainButton.SubItems.AddRange((BaseItem[])(object)new BaseItem[3]
		{
			(BaseItem)controlContainerFind,
			(BaseItem)buttonSelectAll,
			(BaseItem)controlContainerItem
		});
		controlContainerItem.AllowItemResize = false;
		((BaseItem)controlContainerItem).BeginGroup = true;
		controlContainerItem.Control = (Control)(object)listAllowUserValues;
		((BaseItem)controlContainerItem).GlobalItem = false;
		controlContainerItem.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)controlContainerItem).Name = "controlContainerItem";
		((BaseItem)controlContainerItem).ThemeAware = true;
		((BaseItem)controlContainerItem).Visible = false;
		controlContainerFind.AllowItemResize = false;
		((BaseItem)controlContainerFind).BeginGroup = true;
		controlContainerFind.Control = (Control)(object)textBoxFind;
		((BaseItem)controlContainerFind).GlobalItem = false;
		controlContainerFind.MenuVisibility = (eMenuVisibility)0;
		((BaseItem)controlContainerFind).Name = "controlContainerFind";
		((BaseItem)controlContainerFind).ThemeAware = true;
		((BaseItem)controlContainerFind).Visible = true;
		((ScrollableControl)listAllowUserValues).AutoScroll = true;
		((Control)listAllowUserValues).BackColor = Color.Transparent;
		((Control)listAllowUserValues).Location = new Point(0, 0);
		((Control)listAllowUserValues).MaximumSize = new Size(StiScale2.XXI(275.0), StiScale2.YYI(300.0));
		((Control)listAllowUserValues).Name = "listAllowUserValues";
		((Control)listAllowUserValues).Size = new Size(StiScale2.XXI(275.0), 0);
		((Control)listAllowUserValues).TabIndex = 0;
		listAllowUserValues.FlowDirection = (FlowDirection)1;
		listAllowUserValues.WrapContents = false;
		((Control)listAllowUserValues).Padding = new Padding(0, 0, StiScale2.XXI(20.0), 0);
		((Control)listAllowUserValues).AutoSize = true;
		((Panel)listAllowUserValues).AutoSizeMode = (AutoSizeMode)0;
		((BaseItem)buttonSelectAll).AutoCollapseOnClick = false;
		((BaseItem)buttonSelectAll).GlobalItem = false;
		((BaseItem)buttonSelectAll).BeginGroup = true;
		((BaseItem)buttonSelectAll).Name = "buttonSelectAll";
		((BaseItem)buttonSelectAll).Text = StiLocalization.Get("MainMenu", "menuEditSelectAll").Replace("&", "");
		buttonSelectAll.Checked = true;
		((BaseItem)buttonSelectAll).Click += ButtonSelectAll_Click;
	}

	private void CheckStateButtonSelectAll()
	{
		if (lockCheckState)
		{
			return;
		}
		bool @checked = true;
		foreach (object item in (ArrangedElementCollection)((Control)listAllowUserValues).Controls)
		{
			CheckBox val = (CheckBox)((item is CheckBox) ? item : null);
			if (val != null && ((Control)val).Tag is StiDialogInfoItem && !val.Checked)
			{
				@checked = false;
				break;
			}
		}
		buttonSelectAll.Checked = @checked;
	}

	public object GetResult()
	{
		IStiList stiList = null;
		switch (listType)
		{
		case StiListType.DateTimeList:
			stiList = new DateTimeList();
			break;
		case StiListType.TimeSpanList:
			stiList = new TimeSpanList();
			break;
		case StiListType.CharList:
			stiList = new CharList();
			break;
		case StiListType.ByteList:
			stiList = new ByteList();
			break;
		case StiListType.FloatList:
			stiList = new FloatList();
			break;
		case StiListType.DoubleList:
			stiList = new DoubleList();
			break;
		case StiListType.DecimalList:
			stiList = new DecimalList();
			break;
		case StiListType.GuidList:
			stiList = new GuidList();
			break;
		case StiListType.StringList:
			stiList = new StringList();
			break;
		case StiListType.ShortList:
			stiList = new ShortList();
			break;
		case StiListType.IntList:
			stiList = new IntList();
			break;
		case StiListType.LongList:
			stiList = new LongList();
			break;
		case StiListType.BoolList:
			stiList = new BoolList();
			break;
		}
		if (buttonInfo.Variable.DialogInfo.AllowUserValues)
		{
			foreach (IStiListItemControl item in (ArrangedElementCollection)((Control)listAllowUserValues).Controls)
			{
				stiList.AddElement(item.Value);
			}
		}
		else
		{
			foreach (object item2 in (ArrangedElementCollection)((Control)listAllowUserValues).Controls)
			{
				CheckBox val = (CheckBox)((item2 is CheckBox) ? item2 : null);
				if (val.Checked && ((Control)val).Tag is StiDialogInfoItem)
				{
					stiList.AddElement(((StiDialogInfoItem)((Control)val).Tag).KeyObject);
				}
			}
		}
		return stiList;
	}

	public StiListButtonXControl(StiListType type, StiVariable variable, StiReport report)
	{
		InitializeComponent();
		listType = type;
		Variable = variable;
		Report = report;
		buttonInfo = new StiButtonListInfo
		{
			Variable = variable,
			Control = this,
			Type = listType
		};
		List<StiDialogInfoItem> list = null;
		switch (type)
		{
		case StiListType.DateTimeList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(DateTimeList));
			break;
		case StiListType.TimeSpanList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(TimeSpanList));
			break;
		case StiListType.CharList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(CharList));
			break;
		case StiListType.FloatList:
		case StiListType.DoubleList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(DoubleList));
			break;
		case StiListType.DecimalList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(DecimalList));
			break;
		case StiListType.GuidList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(GuidList));
			break;
		case StiListType.StringList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(StringList));
			break;
		case StiListType.ByteList:
		case StiListType.ShortList:
		case StiListType.IntList:
		case StiListType.LongList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(LongList));
			break;
		case StiListType.BoolList:
			list = variable.DialogInfo.GetDialogInfoItems(typeof(BoolList));
			break;
		}
		if (variable.DialogInfo.AllowUserValues)
		{
			InitAllowUserValues();
			((Control)listAllowUserValues).SuspendLayout();
			if (list != null && list.Count != 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					AddNewItem(list[i], checkSize: false);
				}
				RefreshItemSelectionText();
			}
			UpdateFlowLayoutPanelSize();
			((Control)listAllowUserValues).ResumeLayout(false);
		}
		else
		{
			Init();
			((Control)listAllowUserValues).SuspendLayout();
			if (list != null && list.Count != 0)
			{
				for (int j = 0; j < list.Count; j++)
				{
					AddNewItem(list[j], checkSize: false);
				}
				RefreshItemSelectionText();
			}
			CheckStateButtonSelectAll();
			UpdateFlowLayoutPanelSize();
			((Control)listAllowUserValues).ResumeLayout(false);
		}
		((ScrollableControl)listAllowUserValues).AutoScroll = true;
		StiNewRequestFromUserDialogCreatingEventArgs stiNewRequestFromUserDialogCreatingEventArgs = new StiNewRequestFromUserDialogCreatingEventArgs(variable, ((Control)mainButton).Width);
		StiOptions.Engine.GlobalEvents.InvokeNewRequestFromUserDialogCreating(stiNewRequestFromUserDialogCreatingEventArgs);
		((Control)mainButton).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
		((Control)this).Width = stiNewRequestFromUserDialogCreatingEventArgs.Width;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		mainButton = new ButtonX();
		((Control)this).SuspendLayout();
		((Control)mainButton).AccessibleRole = (AccessibleRole)43;
		mainButton.AutoExpandOnClick = true;
		mainButton.ColorTable = (eButtonColor)2;
		mainButton.FocusCuesEnabled = false;
		mainButton.Is20Button = false;
		((Control)mainButton).Location = new Point(0, 0);
		((Control)mainButton).Name = "mainButton";
		((Control)mainButton).Size = new Size(203, 22);
		((PopupItemControl)mainButton).Style = (eDotNetBarStyle)8;
		((Control)mainButton).TabIndex = 0;
		((PopupItemControl)mainButton).ThemeAware = true;
		((PopupItemControl)mainButton).PopupClose += MainButton_PopupClose;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)mainButton);
		((Control)this).Name = "StiListButtonXControl";
		((Control)this).Size = new Size(203, 22);
		((Control)this).ResumeLayout(false);
	}
}
