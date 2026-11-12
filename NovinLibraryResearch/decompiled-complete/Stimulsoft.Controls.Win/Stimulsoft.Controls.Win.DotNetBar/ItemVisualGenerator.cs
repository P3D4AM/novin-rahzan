using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class ItemVisualGenerator
{
	private IBindingSupport _Parent;

	private object _DataSource;

	private bool _InSetDataConnection;

	private List<BindingDef> _Bindings = new List<BindingDef>();

	private CurrencyManager _DataManager;

	private bool _IsDataSourceInitEventHooked;

	private bool _IsDataSourceInitialized;

	private ICloneable _VisualTemplate;

	private Hashtable _Converters = new Hashtable();

	[AttributeProvider(typeof(IListSource))]
	[Description("Indicates data source for the ComboTree.")]
	[Category("Data")]
	[DefaultValue(null)]
	[RefreshProperties(RefreshProperties.Repaint)]
	public object DataSource
	{
		get
		{
			return _DataSource;
		}
		set
		{
			if (value != null && !(value is IList) && !(value is IListSource))
			{
				throw new ArgumentException("Data type is not supported for complex data binding");
			}
			if (_DataSource != value)
			{
				if (DesignMode)
				{
					_DataSource = value;
				}
				else
				{
					SetDataConnection(value, force: true);
				}
			}
		}
	}

	private bool DesignMode
	{
		get
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			if (_Parent is IComponent && ((IComponent)_Parent).Site != null)
			{
				return ((IComponent)_Parent).Site.DesignMode;
			}
			if (_Parent is Control && ((Component)(Control)_Parent).Site != null)
			{
				return ((Component)(Control)_Parent).Site.DesignMode;
			}
			return false;
		}
	}

	public List<BindingDef> Bindings
	{
		get
		{
			return _Bindings;
		}
		set
		{
			if (value != _Bindings)
			{
				List<BindingDef> bindings = _Bindings;
				_Bindings = value;
				OnBindingsChanged(bindings, value);
			}
		}
	}

	internal CurrencyManager DataManager => _DataManager;

	public ICloneable VisualTemplate
	{
		get
		{
			return _VisualTemplate;
		}
		set
		{
			if (value != _VisualTemplate)
			{
				ICloneable visualTemplate = _VisualTemplate;
				_VisualTemplate = value;
				OnVisualTemplateChanged(visualTemplate, value);
			}
		}
	}

	public ItemVisualGenerator(IBindingSupport parent)
	{
		_Parent = parent;
	}

	private void SetDataConnection(object newDataSource, bool force)
	{
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		bool flag = _DataSource != newDataSource;
		if (_InSetDataConnection)
		{
			return;
		}
		try
		{
			if (!(force || flag))
			{
				return;
			}
			_InSetDataConnection = true;
			IList list = ((DataManager != null) ? DataManager.List : null);
			bool flag2 = DataManager == null;
			UnwireDataSource();
			_DataSource = newDataSource;
			WireDataSource();
			if (_IsDataSourceInitialized)
			{
				CurrencyManager val = null;
				if (newDataSource != null && _Parent.BindingContext != null && newDataSource != Convert.DBNull)
				{
					string text = "";
					if (_Bindings != null && _Bindings.Count > 0)
					{
						BindingMemberInfo bindingMemberInfo = _Bindings[0].BindingMemberInfo;
						text = ((BindingMemberInfo)(ref bindingMemberInfo)).BindingPath;
					}
					val = (CurrencyManager)_Parent.BindingContext[newDataSource, text];
				}
				if (_DataManager != val)
				{
					if (_DataManager != null)
					{
						_DataManager.ItemChanged -= new ItemChangedEventHandler(DataManager_ItemChanged);
						((BindingManagerBase)_DataManager).PositionChanged -= DataManager_PositionChanged;
					}
					_DataManager = val;
					if (_DataManager != null)
					{
						_DataManager.ItemChanged += new ItemChangedEventHandler(DataManager_ItemChanged);
						((BindingManagerBase)_DataManager).PositionChanged += DataManager_PositionChanged;
					}
				}
				if (_DataManager != null && flag && !ValidateDisplayMembers(_Bindings))
				{
					throw new ArgumentException("Wrong Bindings parameter", "newBindings");
				}
				if (_DataManager != null && (flag || force) && (flag || (force && (list != _DataManager.List || flag2))))
				{
					DataManager_ItemChanged(_DataManager, null);
				}
			}
			_Converters.Clear();
		}
		finally
		{
			_InSetDataConnection = false;
		}
	}

	private bool ValidateDisplayMembers(List<BindingDef> members)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (members == null || members.Count == 0)
		{
			return true;
		}
		foreach (BindingDef member in members)
		{
			_ = member.BindingMemberInfo;
			if (!BindingMemberInfoInDataManager(member.BindingMemberInfo))
			{
				return false;
			}
		}
		return true;
	}

	private bool BindingMemberInfoInDataManager(BindingMemberInfo bindingMemberInfo)
	{
		if (_DataManager != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_DataManager).GetItemProperties();
			int count = itemProperties.Count;
			for (int i = 0; i < count; i++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[i].PropertyType) && itemProperties[i].Name.Equals(((BindingMemberInfo)(ref bindingMemberInfo)).BindingField))
				{
					return true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				if (!typeof(IList).IsAssignableFrom(itemProperties[j].PropertyType) && string.Compare(itemProperties[j].Name, ((BindingMemberInfo)(ref bindingMemberInfo)).BindingField, ignoreCase: true, CultureInfo.CurrentCulture) == 0)
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

	private void UnwireDataSource()
	{
		if (_DataSource is IComponent)
		{
			((IComponent)_DataSource).Disposed -= DataSourceDisposed;
		}
		if (_DataSource is ISupportInitializeNotification supportInitializeNotification && _IsDataSourceInitEventHooked)
		{
			supportInitializeNotification.Initialized -= DataSourceInitialized;
			_IsDataSourceInitEventHooked = false;
		}
	}

	private void DataSourceDisposed(object sender, EventArgs e)
	{
		SetDataConnection(null, force: true);
	}

	private void WireDataSource()
	{
		if (_DataSource is IComponent)
		{
			((IComponent)_DataSource).Disposed += DataSourceDisposed;
		}
		if (_DataSource is ISupportInitializeNotification { IsInitialized: false } supportInitializeNotification)
		{
			supportInitializeNotification.Initialized += DataSourceInitialized;
			_IsDataSourceInitEventHooked = true;
			_IsDataSourceInitialized = false;
		}
		else
		{
			_IsDataSourceInitialized = true;
		}
	}

	private void DataSourceInitialized(object sender, EventArgs e)
	{
		SetDataConnection(_DataSource, force: true);
	}

	private void DataManager_ItemChanged(object sender, ItemChangedEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		if (_Parent is Control)
		{
			Control val = (Control)_Parent;
			if (val.InvokeRequired)
			{
				val.Invoke((Delegate)new ItemChangedEventHandler(DataManager_ItemChanged), new object[2] { sender, e });
				return;
			}
		}
		if (_DataManager == null)
		{
			return;
		}
		if (e == null || e.Index == -1)
		{
			SetItemsCore(_DataManager.List);
			if (_Parent.AllowSelection)
			{
				_Parent.SelectedIndex = ((BindingManagerBase)_DataManager).Position;
			}
		}
		else
		{
			SetItemCore(e.Index, _DataManager.List[e.Index]);
		}
	}

	private void DataManager_PositionChanged(object sender, EventArgs e)
	{
		if (_DataManager != null && _Parent.AllowSelection)
		{
			_Parent.SelectedIndex = ((BindingManagerBase)_DataManager).Position;
		}
	}

	protected virtual void OnVisualTemplateChanged(ICloneable oldValue, ICloneable newValue)
	{
	}

	protected virtual void SetItemsCore(IList items)
	{
		_Parent.BeginUpdate();
		_Parent.ItemsCollection.Clear();
		for (int i = 0; i < items.Count; i++)
		{
			object item = items[i];
			CreateVisualItem(_Parent.ItemsCollection, item, i, _Bindings);
		}
		_Parent.EndUpdate();
	}

	private object CreateVisualItem(IList parent, object item, int itemIndex, List<BindingDef> bindings)
	{
		object obj = _VisualTemplate.Clone();
		parent.Add(obj);
		SetVisualItemData(obj, item, bindings, itemIndex);
		DataItemVisualEventArgs dataItemVisualEventArgs = new DataItemVisualEventArgs(obj, item);
		_Parent.InvokeDataNodeCreated(dataItemVisualEventArgs);
		return dataItemVisualEventArgs.Visual;
	}

	private void SetVisualItemData(object visualItem, object item, List<BindingDef> bindings, int bindingIndex)
	{
		PropertyInfo property = visualItem.GetType().GetProperty("Tag");
		if (property != null)
		{
			property.SetValue(visualItem, new ItemBindingData(item, bindingIndex), null);
		}
		foreach (BindingDef binding in bindings)
		{
			binding.Update(this, visualItem, item);
		}
	}

	protected virtual void SetItemCore(int index, object value)
	{
		object obj = _Parent.ItemsCollection[index];
		if (obj != null)
		{
			_Parent.BeginUpdate();
			SetVisualItemData(obj, value, _Bindings, index);
			_Parent.EndUpdate();
		}
	}

	public virtual void RefreshItems()
	{
		if (_DataManager != null)
		{
			SetItemsCore(_DataManager.List);
			if (_Parent.AllowSelection)
			{
				_Parent.SelectedIndex = ((BindingManagerBase)_DataManager).Position;
			}
		}
	}

	internal TypeConverter GetFieldConverter(string fieldName)
	{
		if (_Converters.ContainsKey(fieldName))
		{
			return (TypeConverter)_Converters[fieldName];
		}
		if (_DataManager != null)
		{
			PropertyDescriptorCollection itemProperties = ((BindingManagerBase)_DataManager).GetItemProperties();
			if (itemProperties != null)
			{
				PropertyDescriptor propertyDescriptor = itemProperties.Find(fieldName, ignoreCase: true);
				if (propertyDescriptor != null)
				{
					_Converters.Add(fieldName, propertyDescriptor.Converter);
					return propertyDescriptor.Converter;
				}
			}
		}
		return null;
	}
}
