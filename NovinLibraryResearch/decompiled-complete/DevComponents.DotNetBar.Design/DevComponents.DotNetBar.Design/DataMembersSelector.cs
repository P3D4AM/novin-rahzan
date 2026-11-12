using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class DataMembersSelector : UITypeEditor
{
	private CheckedListBoxSelector _ListSelector;

	private BindingContext _BindingContext;

	public DataMembersSelector()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		_ListSelector = new CheckedListBoxSelector();
		_BindingContext = new BindingContext();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			IWindowsFormsEditorService val = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (val != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(context.Instance)["DataSource"];
				if (propertyDescriptor == null)
				{
					return value;
				}
				object value2 = propertyDescriptor.GetValue(context.Instance);
				LoadDataMembers(_ListSelector, value2, value);
				val.DropDownControl((Control)(object)_ListSelector);
				string text = "";
				{
					foreach (string checkedItem in _ListSelector.ListBox.CheckedItems)
					{
						if (text.Length > 0)
						{
							text += ",";
						}
						text += checkedItem;
					}
					return text;
				}
			}
		}
		return value;
	}

	private bool IsSelected(string str, ref string[] selected)
	{
		string text = str.ToLower();
		for (int i = 0; i < selected.Length; i++)
		{
			string text2 = selected[i];
			if (text2.Trim().ToLower() == text)
			{
				selected[i] = str;
				return true;
			}
		}
		return false;
	}

	private void LoadDataMembers(CheckedListBoxSelector listSelector, object dataSource, object value)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		((ObjectCollection)listSelector.ListBox.Items).Clear();
		string text = value as string;
		if (text == null)
		{
			text = "";
		}
		string[] selected = text.Split(new char[1] { ',' });
		string empty = string.Empty;
		if (dataSource is Type)
		{
			try
			{
				BindingSource val = new BindingSource();
				val.DataSource = dataSource;
				dataSource = val.List;
			}
			catch (Exception ex)
			{
				if (IsCriticalException(ex))
				{
					throw;
				}
			}
			catch
			{
			}
		}
		if (!IsBindableDataSource(dataSource))
		{
			return;
		}
		PropertyDescriptorCollection itemProperties = GetItemProperties(dataSource, string.Empty);
		if (itemProperties == null)
		{
			return;
		}
		for (int i = 0; i < itemProperties.Count; i++)
		{
			PropertyDescriptor propertyDescriptor = itemProperties[i];
			if (IsBindableDataMember(propertyDescriptor))
			{
				string text2 = (string.IsNullOrEmpty(empty) ? propertyDescriptor.Name : (empty + "." + propertyDescriptor.Name));
				_ListSelector.ListBox.Items.Add((object)text2, IsSelected(text2, ref selected));
			}
		}
		if (selected.Length > 1)
		{
			for (int j = 0; j < selected.Length; j++)
			{
				string text3 = selected[j];
				int num = ((ObjectCollection)_ListSelector.ListBox.Items).IndexOf((object)text3);
				((ObjectCollection)_ListSelector.ListBox.Items).RemoveAt(num);
				((ObjectCollection)_ListSelector.ListBox.Items).Insert(j, (object)text3);
				_ListSelector.ListBox.SetItemChecked(j, true);
			}
		}
	}

	private bool IsBindableDataMember(PropertyDescriptor property)
	{
		if (!typeof(byte[]).IsAssignableFrom(property.PropertyType))
		{
			ListBindableAttribute listBindableAttribute = (ListBindableAttribute)property.Attributes[typeof(ListBindableAttribute)];
			if (listBindableAttribute != null && !listBindableAttribute.ListBindable)
			{
				return false;
			}
		}
		return true;
	}

	private PropertyDescriptorCollection GetItemProperties(object dataSource, string dataMember)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		CurrencyManager val = (CurrencyManager)_BindingContext[dataSource, dataMember];
		if (val != null)
		{
			return ((BindingManagerBase)val).GetItemProperties();
		}
		return null;
	}

	private bool IsBindableDataSource(object dataSource)
	{
		if (!(dataSource is IListSource) && !(dataSource is IList) && !(dataSource is Array))
		{
			return false;
		}
		ListBindableAttribute listBindableAttribute = (ListBindableAttribute)TypeDescriptor.GetAttributes(dataSource)[typeof(ListBindableAttribute)];
		if (listBindableAttribute != null && !listBindableAttribute.ListBindable)
		{
			return false;
		}
		return true;
	}

	private static bool IsCriticalException(Exception ex)
	{
		if (!(ex is NullReferenceException) && !(ex is StackOverflowException) && !(ex is OutOfMemoryException) && !(ex is ThreadAbortException) && !(ex is ExecutionEngineException) && !(ex is IndexOutOfRangeException))
		{
			return ex is AccessViolationException;
		}
		return true;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		return (UITypeEditorEditStyle)3;
	}
}
