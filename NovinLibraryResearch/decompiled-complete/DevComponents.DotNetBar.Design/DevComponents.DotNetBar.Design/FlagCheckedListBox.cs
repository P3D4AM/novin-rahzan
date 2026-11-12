using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Design;

internal class FlagCheckedListBox : CheckedListBox
{
	private Container components;

	private bool isUpdatingCheckStates;

	private Type enumType;

	private Enum enumValue;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Enum EnumValue
	{
		get
		{
			object obj = Enum.ToObject(enumType, GetCurrentValue());
			return (Enum)obj;
		}
		set
		{
			((ObjectCollection)((CheckedListBox)this).Items).Clear();
			enumValue = value;
			enumType = value.GetType();
			FillEnumMembers();
			ApplyEnumValue();
		}
	}

	public FlagCheckedListBox()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((CheckedListBox)this).CheckOnClick = true;
	}

	public FlagCheckedListBoxItem Add(int v, string c)
	{
		FlagCheckedListBoxItem flagCheckedListBoxItem = new FlagCheckedListBoxItem(v, c);
		((ObjectCollection)((CheckedListBox)this).Items).Add((object)flagCheckedListBoxItem);
		return flagCheckedListBoxItem;
	}

	public FlagCheckedListBoxItem Add(FlagCheckedListBoxItem item)
	{
		((ObjectCollection)((CheckedListBox)this).Items).Add((object)item);
		return item;
	}

	protected override void OnItemCheck(ItemCheckEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		((CheckedListBox)this).OnItemCheck(e);
		if (!isUpdatingCheckStates)
		{
			FlagCheckedListBoxItem composite = ((ObjectCollection)((CheckedListBox)this).Items)[e.Index] as FlagCheckedListBoxItem;
			UpdateCheckedItems(composite, e.NewValue);
		}
	}

	protected void UpdateCheckedItems(int value)
	{
		isUpdatingCheckStates = true;
		for (int i = 0; i < ((ObjectCollection)((CheckedListBox)this).Items).Count; i++)
		{
			FlagCheckedListBoxItem flagCheckedListBoxItem = ((ObjectCollection)((CheckedListBox)this).Items)[i] as FlagCheckedListBoxItem;
			if (flagCheckedListBoxItem.value == 0)
			{
				((CheckedListBox)this).SetItemChecked(i, value == 0);
			}
			else if ((flagCheckedListBoxItem.value & value) == flagCheckedListBoxItem.value && flagCheckedListBoxItem.value != 0)
			{
				((CheckedListBox)this).SetItemChecked(i, true);
			}
			else
			{
				((CheckedListBox)this).SetItemChecked(i, false);
			}
		}
		isUpdatingCheckStates = false;
	}

	protected void UpdateCheckedItems(FlagCheckedListBoxItem composite, CheckState cs)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (composite.value == 0)
		{
			UpdateCheckedItems(0);
		}
		int num = 0;
		for (int i = 0; i < ((ObjectCollection)((CheckedListBox)this).Items).Count; i++)
		{
			FlagCheckedListBoxItem flagCheckedListBoxItem = ((ObjectCollection)((CheckedListBox)this).Items)[i] as FlagCheckedListBoxItem;
			if (((CheckedListBox)this).GetItemChecked(i))
			{
				num |= flagCheckedListBoxItem.value;
			}
		}
		num = (((int)cs != 0) ? (num | composite.value) : (num & ~composite.value));
		UpdateCheckedItems(num);
	}

	public int GetCurrentValue()
	{
		int num = 0;
		for (int i = 0; i < ((ObjectCollection)((CheckedListBox)this).Items).Count; i++)
		{
			FlagCheckedListBoxItem flagCheckedListBoxItem = ((ObjectCollection)((CheckedListBox)this).Items)[i] as FlagCheckedListBoxItem;
			if (((CheckedListBox)this).GetItemChecked(i))
			{
				num |= flagCheckedListBoxItem.value;
			}
		}
		return num;
	}

	private void FillEnumMembers()
	{
		string[] names = Enum.GetNames(enumType);
		foreach (string text in names)
		{
			object value = Enum.Parse(enumType, text);
			int v = (int)Convert.ChangeType(value, typeof(int));
			Add(v, text);
		}
	}

	private void ApplyEnumValue()
	{
		int value = (int)Convert.ChangeType(enumValue, typeof(int));
		UpdateCheckedItems(value);
	}
}
