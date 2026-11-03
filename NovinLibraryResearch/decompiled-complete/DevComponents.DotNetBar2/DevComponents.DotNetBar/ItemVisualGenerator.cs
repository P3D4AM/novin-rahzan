using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using DevComponents.DotNetBar.Primitives;

namespace DevComponents.DotNetBar;

public class ItemVisualGenerator
{
	private IBindingSupport _ᗚ;

	private object _ا;

	private bool _ث;

	private List<BindingDef> __1AFE = new List<BindingDef>();

	private CurrencyManager _ر;

	private bool _ز;

	private bool _س;

	private ICloneable __1AFF;

	private Hashtable _ج = new Hashtable();

	private bool __1B00;

	[DefaultValue(null)]
	[RefreshProperties(RefreshProperties.Repaint)]
	[AttributeProvider(typeof(IListSource))]
	[Category("Data")]
	[Description("Indicates data source for the ComboTree.")]
	public object DataSource
	{
		get
		{
			return _ا;
		}
		set
		{
			if (value != null && !(value is IList) && !(value is IListSource))
			{
				throw new ArgumentException("Data type is not supported for complex data binding");
			}
			if (_ا != value)
			{
				if (ޅ)
				{
					_ا = value;
				}
				else
				{
					_06DA(value, _06DD: true);
				}
			}
		}
	}

	private bool ޅ
	{
		get
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			if (_ᗚ is IComponent && ((IComponent)_ᗚ).Site != null)
			{
				return ((IComponent)_ᗚ).Site.DesignMode;
			}
			if (_ᗚ is Control && ((Component)(Control)_ᗚ).Site != null)
			{
				return ((Component)(Control)_ᗚ).Site.DesignMode;
			}
			return false;
		}
	}

	public List<BindingDef> Bindings
	{
		get
		{
			return __1AFE;
		}
		set
		{
			if (value != __1AFE)
			{
				List<BindingDef> oldValue = __1AFE;
				__1AFE = value;
				OnBindingsChanged(oldValue, value);
			}
		}
	}

	internal CurrencyManager _1AFD => _ر;

	public ICloneable VisualTemplate
	{
		get
		{
			return __1AFF;
		}
		set
		{
			if (value != __1AFF)
			{
				ICloneable oldValue = __1AFF;
				__1AFF = value;
				OnVisualTemplateChanged(oldValue, value);
			}
		}
	}

	public bool CustomCollectionBinding
	{
		get
		{
			return __1B00;
		}
		set
		{
			__1B00 = value;
		}
	}

	public bool IsCustomCollection
	{
		get
		{
			if (__1B00)
			{
				return _ا is CustomCollection<object>;
			}
			return false;
		}
	}

	public ItemVisualGenerator(IBindingSupport parent)
	{
		_ᗚ = parent;
	}

	private void _06DA(object _06DB, bool _06DD)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		bool flag = _ا != _06DB;
		if (_ث)
		{
			return;
		}
		try
		{
			if (!_06DD && !flag)
			{
				return;
			}
			_ث = true;
			IList list = ((_1AFD != null) ? _1AFD.List : null);
			bool flag2 = _1AFD == null;
			_۱();
			_ا = _06DB;
			_۳();
			if (_س && !IsCustomCollection)
			{
				CurrencyManager val = null;
				if (_06DB != null && _ᗚ.BindingContext != null && _06DB != Convert.DBNull)
				{
					string text = "";
					if (__1AFE != null && __1AFE.Count > 0)
					{
						BindingMemberInfo bindingMemberInfo = __1AFE[0].BindingMemberInfo;
						text = ((BindingMemberInfo)(ref bindingMemberInfo)).BindingPath;
					}
					val = (CurrencyManager)_ᗚ.BindingContext[_06DB, text];
				}
				if (_ر != val)
				{
					if (_ر != null)
					{
						_ر.ItemChanged -= new ItemChangedEventHandler(_06E4);
						((BindingManagerBase)_ر).PositionChanged -= ۥ;
					}
					_ر = val;
					if (_ر != null)
					{
						_ر.ItemChanged += new ItemChangedEventHandler(_06E4);
						((BindingManagerBase)_ر).PositionChanged += ۥ;
					}
				}
				if (_ر != null && flag && !_06DE(__1AFE))
				{
					throw new ArgumentException("Wrong Bindings parameter", "newBindings");
				}
				if (_ر != null && (flag || _06DD) && (flag || (_06DD && (list != _ر.List || flag2))))
				{
					_06E4(_ر, null);
				}
			}
			_ج.Clear();
		}
		finally
		{
			_ث = false;
		}
	}

	private bool _06DE(List<BindingDef> _06DF)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (_06DF == null || _06DF.Count == 0)
		{
			return true;
		}
		foreach (BindingDef item in _06DF)
		{
			_ = item.BindingMemberInfo;
			if (!_06E0(item.BindingMemberInfo))
			{
				return false;
			}
		}
		return true;
	}

	private bool _06E0(BindingMemberInfo _06E1)
	{
		if (_ر != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_ر).GetItemProperties();
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(((BindingMemberInfo)(ref _06E1)).BindingField))
				{
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && string.Compare(itemProperties[j].Name, ((BindingMemberInfo)(ref _06E1)).BindingField, ignoreCase: true, CultureInfo.CurrentCulture) == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected virtual void OnBindingsChanged(List<BindingDef> oldValue, List<BindingDef> newValue)
	{
	}

	private void _۱()
	{
		if (IsCustomCollection)
		{
			CustomCollection<object> customCollection = (CustomCollection<object>)_ا;
			customCollection.CollectionChanged -= _1B01;
			return;
		}
		if (_ا is IComponent)
		{
			((IComponent)_ا).Disposed -= _۲;
		}
		if (_ا is ISupportInitializeNotification supportInitializeNotification && _ز)
		{
			supportInitializeNotification.Initialized -= _۴;
			_ز = false;
		}
	}

	private void _1B01(object _٠, NotifyCollectionChangedEventArgs ؿ)
	{
		_ᗚ.BeginUpdate();
		try
		{
			if (ؿ.Action == NotifyCollectionChangedAction.Add)
			{
				int num = ؿ.NewStartingIndex;
				{
					foreach (object newItem in ؿ.NewItems)
					{
						_1B02(_ᗚ.ItemsCollection, newItem, num, __1AFE);
						num++;
					}
					return;
				}
			}
			if (ؿ.Action == NotifyCollectionChangedAction.Remove)
			{
				_ᗚ.ItemsCollection.RemoveAt(ؿ.OldStartingIndex);
			}
			else if (ؿ.Action == NotifyCollectionChangedAction.Replace)
			{
				SetItemCore(ؿ.NewStartingIndex, ؿ.NewItems[0]);
			}
			else if (ؿ.Action == NotifyCollectionChangedAction.Reset)
			{
				RefreshItems();
			}
		}
		finally
		{
			_ᗚ.EndUpdate();
		}
	}

	private void _۲(object _٠, EventArgs ؿ)
	{
		_06DA(null, _06DD: true);
	}

	private void _۳()
	{
		if (IsCustomCollection)
		{
			CustomCollection<object> customCollection = (CustomCollection<object>)_ا;
			customCollection.CollectionChanged += _1B01;
			_س = true;
			return;
		}
		if (_ا is IComponent)
		{
			((IComponent)_ا).Disposed += _۲;
		}
		if (_ا is ISupportInitializeNotification { IsInitialized: false } supportInitializeNotification)
		{
			supportInitializeNotification.Initialized += _۴;
			_ز = true;
			_س = false;
		}
		else
		{
			_س = true;
		}
	}

	private void _۴(object _٠, EventArgs ؿ)
	{
		_06DA(_ا, _06DD: true);
	}

	private void _06E4(object _٠, ItemChangedEventArgs ؿ)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (_ᗚ is Control)
		{
			Control val = (Control)_ᗚ;
			if (val.InvokeRequired)
			{
				val.Invoke((Delegate)new ItemChangedEventHandler(_06E4), new object[2] { _٠, ؿ });
				return;
			}
		}
		if (_ر == null)
		{
			return;
		}
		if (ؿ == null || ؿ.Index == -1)
		{
			SetItemsCore(_ر.List);
			if (_ᗚ.AllowSelection)
			{
				_ᗚ.SelectedIndex = ((BindingManagerBase)_ر).Position;
			}
		}
		else
		{
			SetItemCore(ؿ.Index, _ر.List[ؿ.Index]);
		}
	}

	private void ۥ(object _٠, EventArgs ؿ)
	{
		if (_ر != null && _ᗚ.AllowSelection)
		{
			_ᗚ.SelectedIndex = ((BindingManagerBase)_ر).Position;
		}
	}

	protected virtual void OnVisualTemplateChanged(ICloneable oldValue, ICloneable newValue)
	{
	}

	protected virtual void SetItemsCore(IList items)
	{
		_ᗚ.BeginUpdate();
		_ᗚ.ItemsCollection.Clear();
		for (int i = 0; i < items.Count; i++)
		{
			object obj = items[i];
			_1B02(_ᗚ.ItemsCollection, obj, i, __1AFE);
		}
		_ᗚ.EndUpdate();
	}

	private object _1B02(IList ԟ, object _06EC, int _06ED, List<BindingDef> _1B03)
	{
		if (IsCustomCollection && _06EC is ListBoxItem)
		{
			if (ԟ.Count <= _06ED)
			{
				ԟ.Add(_06EC);
			}
			else
			{
				ԟ.Insert(_06ED, _06EC);
			}
			if (_1B03.Count == 0)
			{
				return _06EC;
			}
			_1B04(_06EC, _06EC, _1B03, _06ED);
			return _06EC;
		}
		object obj = __1AFF.Clone();
		if (ԟ.Count <= _06ED)
		{
			ԟ.Add(obj);
		}
		else
		{
			ԟ.Insert(_06ED, obj);
		}
		_1B04(obj, _06EC, _1B03, _06ED);
		DataItemVisualEventArgs dataItemVisualEventArgs = new DataItemVisualEventArgs(obj, _06EC);
		_ᗚ.InvokeDataNodeCreated(dataItemVisualEventArgs);
		return dataItemVisualEventArgs.Visual;
	}

	private void _1B04(object ᡲ, object _06EC, List<BindingDef> _1B03, int _۰)
	{
		ᡲ.GetType().GetProperty("Tag")?.SetValue(ᡲ, new ItemBindingData(_06EC, _۰), null);
		if (_1B03.Count == 0)
		{
			if (ᡲ is BaseItem)
			{
				if (_06EC is string)
				{
					((BaseItem)ᡲ).Text = (string)_06EC;
				}
				else if (_06EC != null)
				{
					((BaseItem)ᡲ).Text = _06EC.ToString();
				}
			}
			return;
		}
		foreach (BindingDef item in _1B03)
		{
			item.Update(this, ᡲ, _06EC);
		}
	}

	protected virtual void SetItemCore(int index, object value)
	{
		object obj = _ᗚ.ItemsCollection[index];
		if (obj != null)
		{
			_ᗚ.BeginUpdate();
			_1B04(obj, value, __1AFE, index);
			_ᗚ.EndUpdate();
		}
	}

	public virtual void RefreshItems()
	{
		_ᗚ.ItemsCollection.Clear();
		if (_ر != null)
		{
			SetItemsCore(_ر.List);
			if (_ᗚ.AllowSelection)
			{
				_ᗚ.SelectedIndex = ((BindingManagerBase)_ر).Position;
			}
		}
		else if (_ا is IList)
		{
			SetItemsCore((IList)_ا);
		}
	}

	internal TypeConverter _06E2(string _06E3)
	{
		if (_ج.ContainsKey(_06E3))
		{
			return (TypeConverter)_ج[_06E3];
		}
		if (_ر != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_ر).GetItemProperties();
			if (itemProperties != null)
			{
				PropertyDescriptor propertyDescriptor = itemProperties.Find(_06E3, ignoreCase: true);
				if (propertyDescriptor != null)
				{
					_ج.Add(_06E3, propertyDescriptor.Converter);
					return propertyDescriptor.Converter;
				}
			}
		}
		return null;
	}
}
