using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class DlgFlagEnumeration : Form
{
	private class FieldsComparer : IComparer<FieldInfo>
	{
		public int Compare(FieldInfo fi1, FieldInfo fi2)
		{
			if (fi1 == null || fi2 == null)
			{
				return Comparer.Default.Compare(fi1, fi2);
			}
			if (fi1 == fi2)
			{
				return 0;
			}
			if (fi1.Name == StringConsts.EmptyOption)
			{
				return -1;
			}
			if (fi2.Name == StringConsts.EmptyOption)
			{
				return 1;
			}
			return Comparer.Default.Compare(fi1.Name, fi2.Name);
		}
	}

	private Container components;

	private FlagEnumerationEditor mainEditor;

	private FlagEnumerationListBox listBox;

	private object editValue;

	private object originalValue;

	private int lockCheckUpdate;

	public object EditValue
	{
		get
		{
			return editValue;
		}
		set
		{
			if (editValue == value)
			{
				return;
			}
			editValue = value;
			originalValue = value;
			((ObjectCollection)((CheckedListBox)listBox).Items).Clear();
			List<FieldInfo> fields = GetFields(editValue.GetType());
			BeginUpdate();
			try
			{
				foreach (FieldInfo item in fields)
				{
					((CheckedListBox)listBox).Items.Add((object)item.Name, (CheckState)(IsOptionEnabled(item.Name) ? 1 : 0));
				}
			}
			finally
			{
				EndUpdate();
			}
			int num = Math.Min(((ObjectCollection)((CheckedListBox)listBox).Items).Count, 15);
			((Control)listBox).ClientSize = new Size(((Form)this).Size.Width, ((ListBox)listBox).ItemHeight * num);
			((Form)this).ClientSize = new Size(((Form)this).Size.Width, ((ListBox)listBox).ItemHeight * num);
		}
	}

	public CheckedListBox ListBox => (CheckedListBox)(object)listBox;

	public DlgFlagEnumeration()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		InitializeComponent();
		lockCheckUpdate = 0;
		((Form)this).TopLevel = false;
		((Control)this).Font = Control.DefaultFont;
		listBox = new FlagEnumerationListBox();
		((Control)listBox).Font = Control.DefaultFont;
		((ListBox)listBox).BorderStyle = (BorderStyle)0;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		((Control)listBox).SetBounds(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, clientRectangle.Height);
		((CheckedListBox)listBox).ItemCheck += new ItemCheckEventHandler(listBox_ItemCheckEventHandler);
		((Control)listBox).Visible = true;
		((Form)this).ClientSize = new Size(0, ((ListBox)listBox).ItemHeight * 8);
		((Control)this).Controls.Add((Control)(object)listBox);
	}

	public DlgFlagEnumeration(FlagEnumerationEditor editor)
		: this()
	{
		mainEditor = editor;
	}

	private void InitializeComponent()
	{
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Form)this).ClientSize = new Size(292, 266);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "DlgFlagEnumeration";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)3;
		((Control)this).Text = "DlgFlagEnumeration";
		((Form)this).TopMost = true;
	}

	protected List<FieldInfo> GetFields(Type type)
	{
		List<FieldInfo> list = new List<FieldInfo>();
		FieldInfo[] fields = type.GetFields();
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (!fieldInfo.IsSpecialName)
			{
				list.Add(fieldInfo);
			}
		}
		list.Sort(new FieldsComparer());
		return list;
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 13)
		{
			if (mainEditor != null)
			{
				mainEditor.Service.CloseDropDown();
			}
			return true;
		}
		if ((int)keyData == 27)
		{
			editValue = originalValue;
			if (mainEditor != null)
			{
				mainEditor.Service.CloseDropDown();
			}
			return true;
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	protected void listBox_ItemCheckEventHandler(object sender, ItemCheckEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		if (lockCheckUpdate != 0)
		{
			return;
		}
		BeginUpdate();
		try
		{
			string optionName = ((ObjectCollection)((CheckedListBox)listBox).Items)[e.Index].ToString();
			if ((int)e.NewValue == 1)
			{
				EnableOption(optionName);
			}
			else
			{
				DisableOption(optionName);
			}
			UpdateListBox();
		}
		finally
		{
			EndUpdate();
		}
	}

	protected void BeginUpdate()
	{
		lockCheckUpdate++;
	}

	protected void EndUpdate()
	{
		lockCheckUpdate--;
	}

	protected void SelectAll()
	{
		foreach (string item in (ObjectCollection)((CheckedListBox)listBox).Items)
		{
			if (item != StringConsts.EmptyOption)
			{
				EnableOption(item);
			}
		}
	}

	protected void ClearAll()
	{
		foreach (string item in (ObjectCollection)((CheckedListBox)listBox).Items)
		{
			if (item != StringConsts.EmptyOption)
			{
				DisableOption(item);
			}
		}
	}

	protected void UpdateListBox()
	{
		listBox.ListBoxBeginUpdate();
		try
		{
			for (int i = 0; i < ((ObjectCollection)((CheckedListBox)listBox).Items).Count; i++)
			{
				string optionName = ((ObjectCollection)((CheckedListBox)listBox).Items)[i].ToString();
				((CheckedListBox)listBox).SetItemChecked(i, IsOptionEnabled(optionName));
			}
		}
		finally
		{
			listBox.ListBoxEndUpdate();
		}
	}

	protected int GetOptionValue(string optionName)
	{
		Type type = EditValue.GetType();
		return (int)type.GetField(optionName).GetValue(EditValue);
	}

	protected void FromInt(int value)
	{
		Type type = EditValue.GetType();
		editValue = Enum.ToObject(type, value);
	}

	protected void DisableOption(string optionName)
	{
		if (optionName == StringConsts.EmptyOption)
		{
			SelectAll();
		}
		else
		{
			FromInt((int)editValue & ~GetOptionValue(optionName));
		}
	}

	protected void EnableOption(string optionName)
	{
		if (optionName == StringConsts.EmptyOption)
		{
			ClearAll();
		}
		else
		{
			FromInt((int)editValue | GetOptionValue(optionName));
		}
	}

	protected bool IsOptionEnabled(string optionName)
	{
		int num = (int)editValue;
		if (optionName == StringConsts.EmptyOption)
		{
			return num == GetOptionValue(optionName);
		}
		return (num & GetOptionValue(optionName)) == GetOptionValue(optionName);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}
}
