using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class PropertySettingsCollection : Collection<PropertySettings>
{
	private Dictionary<PropertyDescriptor, PropertySettings> __2EB1 = new Dictionary<PropertyDescriptor, PropertySettings>();

	private AdvPropertyGrid __2487;

	public virtual PropertySettings this[string propertyName]
	{
		get
		{
			using (IEnumerator<PropertySettings> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PropertySettings current = enumerator.Current;
					if (current.PropertyName == propertyName)
					{
						return current;
					}
				}
			}
			return null;
		}
	}

	public virtual PropertySettings this[PropertyDescriptor propertyDescriptor]
	{
		get
		{
			using (IEnumerator<PropertySettings> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PropertySettings current = enumerator.Current;
					if (current.PropertyDescriptor.Equals(propertyDescriptor))
					{
						return current;
					}
				}
			}
			return null;
		}
	}

	public virtual PropertySettings this[PropertyDescriptor propertyDescriptor, string propertyName]
	{
		get
		{
			PropertySettings result = null;
			using IEnumerator<PropertySettings> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				PropertySettings current = enumerator.Current;
				if (current.PropertyDescriptor != null && current.PropertyDescriptor.Equals(propertyDescriptor))
				{
					return current;
				}
				if (current.PropertyName == propertyName)
				{
					result = current;
				}
			}
			return result;
		}
	}

	public AdvPropertyGrid Owner => __2487;

	public PropertySettingsCollection(AdvPropertyGrid owner)
	{
		__2487 = owner;
	}

	protected override void InsertItem(int index, PropertySettings item)
	{
		base.InsertItem(index, item);
		_2EB2(item);
	}

	private void _2EB2(PropertySettings _06EC)
	{
		__2487.ⶰ(_06EC);
	}

	protected override void ClearItems()
	{
		PropertySettings[] array = new PropertySettings[base.Count];
		CopyTo(array, 0);
		base.ClearItems();
		PropertySettings[] array2 = array;
		foreach (PropertySettings propertySettings in array2)
		{
			_2EB3(propertySettings);
		}
	}

	protected override void RemoveItem(int index)
	{
		PropertySettings propertySettings = base[index];
		base.RemoveItem(index);
		_2EB3(propertySettings);
	}

	private void _2EB3(PropertySettings _108F)
	{
		__2487.ⶲ(_108F);
	}

	protected override void SetItem(int index, PropertySettings item)
	{
		PropertySettings propertySettings = base[index];
		base.SetItem(index, item);
		_2EB3(propertySettings);
		_2EB2(item);
	}
}
