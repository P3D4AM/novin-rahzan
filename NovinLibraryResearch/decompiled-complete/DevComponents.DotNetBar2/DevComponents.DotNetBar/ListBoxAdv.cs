using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Primitives;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(ToolboxIconResFinder), "ListBoxAdv.ico")]
[ComVisible(false)]
[Designer("DevComponents.DotNetBar.Design.ListBoxAdvDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
public class ListBoxAdv : ItemPanelBase
{
	private string _ᬮ;

	private CustomCollection<object> _ᡨ = new CustomCollection<object>();

	private bool _ᬯ;

	private int _ᬰ = 20;

	private eSelectionMode _ᬱ = eSelectionMode.One;

	private bool _ᬲ;

	private CustomCollection<ListBoxItem> _ᬳ;

	private ListBoxItem __1B34;

	private string __1B35 = "";

	private CheckStateConvertEventHandler _1B36;

	private string _ض = "";

	private Dictionary<string, PropertyDescriptor> _ص = new Dictionary<string, PropertyDescriptor>();

	private ListBoxAdvItemCheckEventHandler _1B37;

	private bool _ߍ;

	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Description("List of items displayed in list box.")]
	[Localizable(true)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[MergableProperty(false)]
	public CustomCollection<object> Items => _ᡨ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public IList Views => GetItemsCollection();

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether check-boxes are visible inside list box items.")]
	public bool CheckBoxesVisible
	{
		get
		{
			return _ᬯ;
		}
		set
		{
			if (value != _ᬯ)
			{
				bool ᬯ = _ᬯ;
				_ᬯ = value;
				OnCheckBoxesVisibleChanged(ᬯ, value);
			}
		}
	}

	[DefaultValue(20)]
	[Description("Indicates fixed item height. Maybe set to 0 to indicate that each item should be sized based on its content.")]
	[Category("Appearance")]
	public int ItemHeight
	{
		get
		{
			return _ᬰ;
		}
		set
		{
			if (value != _ᬰ)
			{
				int ᬰ = _ᬰ;
				_ᬰ = value;
				OnItemHeightChanged(ᬰ, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Specifies the selection mode used by the control.")]
	[DefaultValue(eSelectionMode.One)]
	public eSelectionMode SelectionMode
	{
		get
		{
			return _ᬱ;
		}
		set
		{
			if (value != _ᬱ)
			{
				eSelectionMode ᬱ = _ᬱ;
				_ᬱ = value;
				OnSelectionModeChanged(ᬱ, value);
			}
		}
	}

	[Bindable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override int SelectedIndex
	{
		get
		{
			return _SelectedIndex;
		}
		set
		{
			if (_ᬳ.Count > 0)
			{
				_1B3B(eEventSource.Code);
			}
			SetSelected(value, value: true);
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<int> SelectedIndices => _1B3C();

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public CustomCollection<ListBoxItem> SelectedItems => _ᬳ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object SelectedItem
	{
		get
		{
			if (_ᬳ.Count == 0)
			{
				return null;
			}
			ListBoxItem listBoxItem = _ᬳ[0];
			if (listBoxItem.Tag is ItemBindingData)
			{
				return ((ItemBindingData)listBoxItem.Tag).DataItem;
			}
			return listBoxItem;
		}
		set
		{
			if (value == null)
			{
				foreach (ListBoxItem item in _ᬳ)
				{
					item.SetIsSelected(isSelected: false, eEventSource.Code);
				}
				return;
			}
			ListBoxItem listBoxItem = FindListBoxItem(value);
			if (listBoxItem == null)
			{
				throw new InvalidOperationException("SelectedValue set cannot be found in Items collection");
			}
			listBoxItem.SetIsSelected(isSelected: true, eEventSource.Code);
		}
	}

	[Description("Indicates property name which will provide the value to display for this list box when data-binding is used and DataSource is set.")]
	[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultValue("")]
	[Category("Data")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public string DisplayMember
	{
		get
		{
			return __1B35;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != __1B35)
			{
				string oldValue = __1B35;
				__1B35 = value;
				OnDisplayMemberChanged(oldValue, value);
			}
		}
	}

	[TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Description("Indicates property name which will provide the value for CheckState of the list box item when data-binding is used and DataSource is set.")]
	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Category("Data")]
	[DefaultValue("")]
	public string CheckStateMember
	{
		get
		{
			return _ᬮ;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _ᬮ)
			{
				string ᬮ = _ᬮ;
				_ᬮ = value;
				OnCheckStateMemberChanged(ᬮ, value);
			}
		}
	}

	[Browsable(false)]
	public IEnumerable<object> SelectedValues
	{
		get
		{
			List<int> selectedIndexes = SelectedIndices;
			foreach (int index in selectedIndexes)
			{
				yield return _1B40(index);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object SelectedValue
	{
		get
		{
			if (base.DataSource == null)
			{
				return null;
			}
			int selectedIndex = SelectedIndex;
			return _1B40(selectedIndex);
		}
		set
		{
			if (string.IsNullOrEmpty(_ض))
			{
				throw new InvalidOperationException("ValueMember property not set");
			}
			int num = _1B42(value);
			if (num < 0)
			{
				throw new InvalidPropertyValueException("value not found.");
			}
			SelectedItem = m_ItemContainer.SubItems[num];
		}
	}

	[Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version= 2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Category("Data")]
	[Description("Indicates property name which will return the value for the selected item when SelectedValue property is accessed.")]
	[DefaultValue("")]
	public string ValueMember
	{
		get
		{
			return _ض;
		}
		set
		{
			if (value == null)
			{
				value = "";
			}
			if (value != _ض)
			{
				string ض = _ض;
				_ض = value;
				OnValueMemberChanged(ض, value);
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public List<ListBoxItem> CheckedItems
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			List<ListBoxItem> list = new List<ListBoxItem>();
			foreach (BaseItem subItem in m_ItemContainer.SubItems)
			{
				if (subItem is ListBoxItem listBoxItem && (int)listBoxItem.CheckState != 0)
				{
					list.Add(listBoxItem);
				}
			}
			return list;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether the items interprets an ampersand character (&) in the control's Text property to be an access key prefix character.")]
	public bool UseMnemonic
	{
		get
		{
			return _ߍ;
		}
		set
		{
			_ߍ = value;
			m_ItemContainer.NeedRecalcSize = true;
		}
	}

	[Description("Occurs when using data-binding with CheckStateMember specified and it allows you to convert a property value to CheckState.")]
	public event CheckStateConvertEventHandler CheckStateConvert
	{
		add
		{
			CheckStateConvertEventHandler checkStateConvertEventHandler = _1B36;
			CheckStateConvertEventHandler checkStateConvertEventHandler2;
			do
			{
				checkStateConvertEventHandler2 = checkStateConvertEventHandler;
				CheckStateConvertEventHandler value2 = (CheckStateConvertEventHandler)Delegate.Combine(checkStateConvertEventHandler2, value);
				checkStateConvertEventHandler = Interlocked.CompareExchange(ref _1B36, value2, checkStateConvertEventHandler2);
			}
			while ((object)checkStateConvertEventHandler != checkStateConvertEventHandler2);
		}
		remove
		{
			CheckStateConvertEventHandler checkStateConvertEventHandler = _1B36;
			CheckStateConvertEventHandler checkStateConvertEventHandler2;
			do
			{
				checkStateConvertEventHandler2 = checkStateConvertEventHandler;
				CheckStateConvertEventHandler value2 = (CheckStateConvertEventHandler)Delegate.Remove(checkStateConvertEventHandler2, value);
				checkStateConvertEventHandler = Interlocked.CompareExchange(ref _1B36, value2, checkStateConvertEventHandler2);
			}
			while ((object)checkStateConvertEventHandler != checkStateConvertEventHandler2);
		}
	}

	[Description("Occurs when CheckState of an ListBoxItem changes and it allows you to cancel the change.")]
	public event ListBoxAdvItemCheckEventHandler ItemCheck
	{
		add
		{
			ListBoxAdvItemCheckEventHandler listBoxAdvItemCheckEventHandler = _1B37;
			ListBoxAdvItemCheckEventHandler listBoxAdvItemCheckEventHandler2;
			do
			{
				listBoxAdvItemCheckEventHandler2 = listBoxAdvItemCheckEventHandler;
				ListBoxAdvItemCheckEventHandler value2 = (ListBoxAdvItemCheckEventHandler)Delegate.Combine(listBoxAdvItemCheckEventHandler2, value);
				listBoxAdvItemCheckEventHandler = Interlocked.CompareExchange(ref _1B37, value2, listBoxAdvItemCheckEventHandler2);
			}
			while ((object)listBoxAdvItemCheckEventHandler != listBoxAdvItemCheckEventHandler2);
		}
		remove
		{
			ListBoxAdvItemCheckEventHandler listBoxAdvItemCheckEventHandler = _1B37;
			ListBoxAdvItemCheckEventHandler listBoxAdvItemCheckEventHandler2;
			do
			{
				listBoxAdvItemCheckEventHandler2 = listBoxAdvItemCheckEventHandler;
				ListBoxAdvItemCheckEventHandler value2 = (ListBoxAdvItemCheckEventHandler)Delegate.Remove(listBoxAdvItemCheckEventHandler2, value);
				listBoxAdvItemCheckEventHandler = Interlocked.CompareExchange(ref _1B37, value2, listBoxAdvItemCheckEventHandler2);
			}
			while ((object)listBoxAdvItemCheckEventHandler != listBoxAdvItemCheckEventHandler2);
		}
	}

	public ListBoxAdv()
	{
		_ᬳ = new CustomCollection<ListBoxItem>();
		m_ItemContainer.LayoutOrientation = eOrientation.Vertical;
		EnsureBindingGenerator();
		ItemVisualGenerator itemGenerator = ItemGenerator;
		itemGenerator.CustomCollectionBinding = true;
		itemGenerator.DataSource = _ᡨ;
	}

	protected override void OnDataSourceChanged(object newDataSource)
	{
		ClearDescriptorsCache();
		EnsureBindingGenerator();
		ItemVisualGenerator itemGenerator = ItemGenerator;
		if (newDataSource != null || !itemGenerator.CustomCollectionBinding)
		{
			if (newDataSource == null)
			{
				itemGenerator.CustomCollectionBinding = true;
				itemGenerator.DataSource = _ᡨ;
			}
			else
			{
				itemGenerator.CustomCollectionBinding = false;
				itemGenerator.DataSource = newDataSource;
			}
			if (itemGenerator.DataSource != newDataSource)
			{
				itemGenerator.DataSource = newDataSource;
				OnDataSourceChanged(EventArgs.Empty);
			}
		}
	}

	protected virtual void OnCheckBoxesVisibleChanged(bool oldValue, bool newValue)
	{
		m_ItemContainer.NeedRecalcSize = true;
		RecalcLayout();
	}

	protected virtual void OnItemHeightChanged(int oldValue, int newValue)
	{
	}

	protected override BaseItem GetDefaultItemTemplate()
	{
		return new ListBoxItem();
	}

	protected virtual void OnSelectionModeChanged(eSelectionMode oldValue, eSelectionMode newValue)
	{
	}

	protected override void OnItemAdded(BaseItem item, EventArgs e)
	{
		ListBoxItem listBoxItem = item as ListBoxItem;
		bool flag = false;
		if (listBoxItem != null && listBoxItem.IsSelected)
		{
			if (_ᬱ == eSelectionMode.One)
			{
				if (_ᬳ.Count > 0)
				{
					ListBoxItem[] array = new ListBoxItem[_ᬳ.Count];
					_ᬳ.CopyTo(array);
					ListBoxItem[] array2 = array;
					foreach (ListBoxItem listBoxItem2 in array2)
					{
						listBoxItem2.IsSelected = false;
					}
				}
				_ᬳ.Add(listBoxItem);
				_1B39();
				flag = true;
			}
			else if (_ᬱ == eSelectionMode.MultiSimple)
			{
				_ᬳ.Add(listBoxItem);
				_1B39();
				flag = true;
			}
		}
		if (flag)
		{
			_1B38();
		}
		base.OnItemAdded(item, e);
	}

	private void _1B38()
	{
		OnSelectedIndexChanged(EventArgs.Empty);
	}

	private void _1B39()
	{
		if (_ᬳ.Count == 0)
		{
			_SelectedIndex = -1;
			return;
		}
		ListBoxItem value = _ᬳ[_ᬳ.Count - 1];
		_SelectedIndex = m_ItemContainer.SubItems.IndexOf(value);
	}

	protected override void OnItemRemoved(BaseItem item, ItemRemovedEventArgs e)
	{
		ListBoxItem listBoxItem = item as ListBoxItem;
		bool flag = false;
		if (listBoxItem != null && listBoxItem.IsSelected && _ᬳ.Contains(listBoxItem))
		{
			_ᬳ.Remove(listBoxItem);
			_1B39();
			flag = true;
		}
		if (flag)
		{
			_1B38();
		}
		base.OnItemRemoved(item, e);
	}

	internal void _1B3A(ListBoxItem _06EC, eEventSource ᎂ)
	{
		if (_ᬲ)
		{
			return;
		}
		_ᬲ = true;
		bool flag = false;
		if (_06EC.IsSelected)
		{
			if (_ᬱ == eSelectionMode.One && _ᬳ.Count > 0)
			{
				_1B3B(ᎂ);
			}
			if (!_ᬳ.Contains(_06EC))
			{
				_ᬳ.Add(_06EC);
				flag = true;
				_1B39();
			}
		}
		else if (_ᬳ.Contains(_06EC))
		{
			_ᬳ.Remove(_06EC);
			flag = true;
			_1B39();
		}
		if (flag)
		{
			((Control)this).Invalidate();
			_1B38();
		}
		_ᬲ = false;
	}

	internal void _1B3B(eEventSource ᎂ)
	{
		if (_ᬳ.Count != 0)
		{
			ListBoxItem[] array = new ListBoxItem[_ᬳ.Count];
			_ᬳ.CopyTo(array);
			ListBoxItem[] array2 = array;
			foreach (ListBoxItem listBoxItem in array2)
			{
				listBoxItem.SetIsSelected(isSelected: false, ᎂ);
			}
			_ᬳ.Clear();
		}
	}

	public void SetSelected(int index, bool value)
	{
		SetSelected(index, value, eEventSource.Code);
	}

	public void SetSelected(int index, bool value, eEventSource source)
	{
		if (index == -1)
		{
			_1B3B(source);
		}
		else if (m_ItemContainer.SubItems[index] is ListBoxItem listBoxItem)
		{
			listBoxItem.SetIsSelected(value, source);
		}
	}

	public void SelectNextItem()
	{
		int num = ((SelectedIndex != -1) ? (SelectedIndex + 1) : 0);
		if (num >= m_ItemContainer.SubItems.Count)
		{
			return;
		}
		for (int i = num; i < m_ItemContainer.SubItems.Count; i++)
		{
			if (m_ItemContainer.SubItems[i] is ListBoxItem { Visible: not false } listBoxItem)
			{
				listBoxItem.IsSelected = true;
				EnsureVisible(listBoxItem);
				break;
			}
		}
	}

	public void SelectPreviousItem()
	{
		int num = ((SelectedIndex != -1) ? (SelectedIndex - 1) : 0);
		if (num < 0)
		{
			return;
		}
		for (int num2 = num; num2 >= 0; num2--)
		{
			if (m_ItemContainer.SubItems[num2] is ListBoxItem { Visible: not false } listBoxItem)
			{
				listBoxItem.IsSelected = true;
				EnsureVisible(listBoxItem);
				break;
			}
		}
	}

	private List<int> _1B3C()
	{
		List<int> list = new List<int>();
		if (_ᬳ.Count == 0)
		{
			return list;
		}
		foreach (ListBoxItem item in _ᬳ)
		{
			list.Add(m_ItemContainer.SubItems.IndexOf(item));
		}
		return list;
	}

	public ListBoxItem FindListBoxItem(object value)
	{
		if (value is ListBoxItem)
		{
			return (ListBoxItem)value;
		}
		foreach (BaseItem subItem in m_ItemContainer.SubItems)
		{
			if (subItem is ListBoxItem listBoxItem && listBoxItem.Tag is ItemBindingData && ((ItemBindingData)listBoxItem.Tag).DataItem == value)
			{
				return listBoxItem;
			}
		}
		return null;
	}

	protected override void OnKeyboardItemSelected(BaseItem item)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.OnKeyboardItemSelected(item);
		if (item is ListBoxItem)
		{
			ListBoxItem listBoxItem = (ListBoxItem)item;
			if (_ᬱ == eSelectionMode.One)
			{
				listBoxItem.SetIsSelected(isSelected: true, eEventSource.Keyboard);
				return;
			}
			if (_ᬱ == eSelectionMode.MultiExtended)
			{
				if ((Control.ModifierKeys & 0x10000) == 0)
				{
					_1B3B(eEventSource.Keyboard);
					listBoxItem.SetIsSelected(isSelected: true, eEventSource.Keyboard);
				}
				else if (listBoxItem.IsSelected && __1B34 != null && __1B34.IsSelected)
				{
					__1B34.SetIsSelected(isSelected: false, eEventSource.Keyboard);
				}
				else
				{
					listBoxItem.SetIsSelected(isSelected: true, eEventSource.Keyboard);
				}
			}
			if (__1B34 != null)
			{
				__1B34.OnLostFocus();
			}
			__1B34 = listBoxItem;
			__1B34.OnGotFocus();
		}
		else
		{
			if (__1B34 != null)
			{
				__1B34.OnLostFocus();
			}
			__1B34 = null;
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		if (__1B34 != null)
		{
			__1B34.OnLostFocus();
		}
		__1B34 = null;
		((Control)this).OnLostFocus(e);
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && _ᬱ == eSelectionMode.MultiSimple && __1B34 != null)
		{
			__1B34.SetIsSelected(!__1B34.IsSelected, eEventSource.Keyboard);
		}
		base.OnKeyDown(e);
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		if ((keyData & 0x60000) == 0)
		{
			Keys val = (Keys)(keyData & 0xFFFF);
			if ((int)val == 37 || (int)val == 39 || (int)val == 38 || (int)val == 40)
			{
				_0A81(new KeyEventArgs(val));
				return true;
			}
		}
		return base.ProcessDialogKey(keyData);
	}

	protected virtual void OnDisplayMemberChanged(string oldValue, string newValue)
	{
		EnsureBindingGenerator();
		ItemVisualGenerator itemGenerator = ItemGenerator;
		foreach (BindingDef binding in itemGenerator.Bindings)
		{
			if (binding.PropertyName == "Text")
			{
				itemGenerator.Bindings.Remove(binding);
				break;
			}
		}
		if (!string.IsNullOrEmpty(newValue))
		{
			BindingDef item = new BindingDef("Text", newValue);
			itemGenerator.Bindings.Add(item);
		}
		itemGenerator.RefreshItems();
	}

	protected virtual void OnCheckStateMemberChanged(string oldValue, string newValue)
	{
		EnsureBindingGenerator();
		ItemVisualGenerator itemGenerator = ItemGenerator;
		foreach (BindingDef binding in itemGenerator.Bindings)
		{
			if (binding.PropertyName == "CheckState")
			{
				binding.Format -= _1B3D;
				itemGenerator.Bindings.Remove(binding);
				break;
			}
		}
		if (!string.IsNullOrEmpty(newValue))
		{
			BindingDef bindingDef = new BindingDef("CheckState", newValue);
			bindingDef.FormattingEnabled = true;
			bindingDef.Format += _1B3D;
			itemGenerator.Bindings.Add(bindingDef);
		}
		itemGenerator.RefreshItems();
	}

	protected virtual void OnCheckStateConvert(CheckStateConvertEventArgs e)
	{
		_1B36?.Invoke(this, e);
	}

	private void _1B3D(object _٠, DataConvertEventArgs ؿ)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (_1B36 != null)
		{
			CheckStateConvertEventArgs checkStateConvertEventArgs = new CheckStateConvertEventArgs(((ConvertEventArgs)ؿ).Value);
			OnCheckStateConvert(checkStateConvertEventArgs);
			if (checkStateConvertEventArgs.CheckState.HasValue)
			{
				((ConvertEventArgs)ؿ).Value = checkStateConvertEventArgs.CheckState.Value;
				return;
			}
		}
		((ConvertEventArgs)ؿ).Value = _1B3E(((ConvertEventArgs)ؿ).Value);
	}

	private CheckState _1B3E(object ԝ)
	{
		if (ԝ is SqlBoolean sqlBoolean)
		{
			if (!sqlBoolean.IsTrue)
			{
				if (!sqlBoolean.IsNull)
				{
					return (CheckState)0;
				}
				return (CheckState)2;
			}
			return (CheckState)1;
		}
		if (ԝ is int?)
		{
			if (ԝ != null)
			{
				if (((int?)ԝ).Value != 0)
				{
					return (CheckState)1;
				}
				return (CheckState)0;
			}
			return (CheckState)2;
		}
		if (ԝ is int)
		{
			if ((int)ԝ != -1)
			{
				if ((int)ԝ == 0)
				{
					return (CheckState)0;
				}
				return (CheckState)1;
			}
			return (CheckState)2;
		}
		if (ԝ is long)
		{
			if ((long)ԝ != -1)
			{
				if ((long)ԝ == 0)
				{
					return (CheckState)0;
				}
				return (CheckState)1;
			}
			return (CheckState)2;
		}
		if (ԝ is short)
		{
			if ((short)ԝ != -1)
			{
				if ((short)ԝ == 0)
				{
					return (CheckState)0;
				}
				return (CheckState)1;
			}
			return (CheckState)2;
		}
		if (ԝ is float)
		{
			if ((float)ԝ != -1f)
			{
				if ((float)ԝ == 0f)
				{
					return (CheckState)0;
				}
				return (CheckState)1;
			}
			return (CheckState)2;
		}
		if (ԝ is double)
		{
			if ((double)ԝ != -1.0)
			{
				if ((double)ԝ == 0.0)
				{
					return (CheckState)0;
				}
				return (CheckState)1;
			}
			return (CheckState)2;
		}
		if (ԝ is byte)
		{
			if ((byte)ԝ == 0)
			{
				return (CheckState)0;
			}
			return (CheckState)1;
		}
		if (ԝ is uint)
		{
			if ((uint)ԝ == 0)
			{
				return (CheckState)0;
			}
			return (CheckState)1;
		}
		if (ԝ is ulong)
		{
			if ((ulong)ԝ == 0)
			{
				return (CheckState)0;
			}
			return (CheckState)1;
		}
		if (ԝ is bool)
		{
			if (!(bool)ԝ)
			{
				return (CheckState)0;
			}
			return (CheckState)1;
		}
		return (CheckState)2;
	}

	private bool _1B3F(object ԝ)
	{
		if (ԝ != null)
		{
			return ԝ == DBNull.Value;
		}
		return true;
	}

	private object _1B40(int _1B41)
	{
		if (_1B41 < 0)
		{
			return null;
		}
		ItemVisualGenerator itemGenerator = ItemGenerator;
		if (itemGenerator._1AFD == null || itemGenerator._1AFD.List == null)
		{
			return null;
		}
		object item = itemGenerator._1AFD.List[_1B41];
		return GetPropertyValue(item, _ض);
	}

	private int _1B42(object ԝ)
	{
		ItemVisualGenerator itemGenerator = ItemGenerator;
		if (itemGenerator._1AFD == null || itemGenerator._1AFD.List == null)
		{
			return -1;
		}
		IList list = itemGenerator._1AFD.List;
		for (int i = 0; i < list.Count; i++)
		{
			object propertyValue = GetPropertyValue(list[i], _ض);
			if (propertyValue.Equals(ԝ))
			{
				return i;
			}
		}
		return -1;
	}

	protected virtual void OnValueMemberChanged(string oldValue, string newValue)
	{
	}

	public void ClearDescriptorsCache()
	{
		_ص.Clear();
	}

	protected object GetPropertyValue(object item, string fieldName)
	{
		if (item != null && fieldName.Length > 0)
		{
			try
			{
				PropertyDescriptor value = null;
				string key = item.GetType().ToString() + ":" + fieldName;
				if (!_ص.TryGetValue(key, out value))
				{
					ItemVisualGenerator itemGenerator = ItemGenerator;
					value = ((itemGenerator._1AFD == null) ? TypeDescriptor.GetProperties(item).Find(fieldName, ignoreCase: true) : ((BindingManagerBase)itemGenerator._1AFD).GetItemProperties().Find(fieldName, ignoreCase: true));
					if (value != null)
					{
						_ص.Add(key, value);
					}
				}
				if (value != null)
				{
					item = value.GetValue(item);
				}
			}
			catch
			{
			}
		}
		return item;
	}

	public int FindString(string s)
	{
		return FindString(s, 0);
	}

	public int FindString(string s, int startIndex)
	{
		if (startIndex < 0)
		{
			startIndex = 0;
		}
		for (int i = startIndex; i < m_ItemContainer.SubItems.Count; i++)
		{
			if (m_ItemContainer.SubItems[i].Text.StartsWith(s))
			{
				return i;
			}
		}
		return -1;
	}

	public int FindStringExact(string s)
	{
		return FindStringExact(s, 0);
	}

	public int FindStringExact(string s, int startIndex)
	{
		if (startIndex < 0)
		{
			startIndex = 0;
		}
		for (int i = startIndex; i < m_ItemContainer.SubItems.Count; i++)
		{
			if (m_ItemContainer.SubItems[i].Text == s)
			{
				return i;
			}
		}
		return -1;
	}

	internal void _1B43(ListBoxAdvItemCheckEventArgs ؿ)
	{
		OnItemCheck(ؿ);
	}

	protected virtual void OnItemCheck(ListBoxAdvItemCheckEventArgs e)
	{
		_1B37?.Invoke(this, e);
	}

	public void SetItemCheckState(int index, CheckState value)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		ListBoxItem listBoxItem = m_ItemContainer.SubItems[index] as ListBoxItem;
		listBoxItem.CheckState = value;
	}
}
