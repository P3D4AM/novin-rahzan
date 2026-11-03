using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Entities;

namespace FormCustom;

public class SortableBindingList<T> : BindingList<T>
{
	private readonly ConcurrentDictionary<Type, PropertyComparer<T>> comparers;

	private bool isSorted;

	private ListSortDirection listSortDirection;

	private PropertyDescriptor propertyDescriptor;

	protected override bool SupportsSortingCore => true;

	protected override bool IsSortedCore => isSorted;

	protected override PropertyDescriptor SortPropertyCore => propertyDescriptor;

	protected override ListSortDirection SortDirectionCore => listSortDirection;

	protected override bool SupportsSearchingCore => true;

	public SortableBindingList()
		: base((IList<T>)new List<T>())
	{
		try
		{
			comparers = new ConcurrentDictionary<Type, PropertyComparer<T>>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 29);
		}
	}

	public SortableBindingList(IEnumerable<T> enumeration)
		: base((IList<T>)new List<T>(enumeration))
	{
		try
		{
			comparers = new ConcurrentDictionary<Type, PropertyComparer<T>>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 37);
		}
	}

	protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
	{
		try
		{
			List<T> obj = (List<T>)base.Items;
			Type propertyType = property.PropertyType;
			if (!comparers.TryGetValue(propertyType, out var value))
			{
				value = new PropertyComparer<T>(property, direction);
				comparers.TryAdd(propertyType, value);
			}
			value.SetPropertyAndDirection(property, direction);
			obj.Sort(value);
			propertyDescriptor = property;
			listSortDirection = direction;
			isSorted = true;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplySortCore", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 62);
		}
	}

	protected override void RemoveSortCore()
	{
		try
		{
			isSorted = false;
			propertyDescriptor = base.SortPropertyCore;
			listSortDirection = base.SortDirectionCore;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveSortCore", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 73);
		}
	}

	protected override int FindCore(PropertyDescriptor property, object key)
	{
		int count = base.Count;
		try
		{
			for (int i = 0; i < count; i++)
			{
				T val = base[i];
				if (property.GetValue(val).Equals(key))
				{
					return i;
				}
			}
			return -1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FindCore", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 89);
			return -1;
		}
	}

	public void AddRange(IEnumerable<T> items)
	{
		try
		{
			base.RaiseListChangedEvents = false;
			foreach (T item in items)
			{
				Add(item);
			}
			base.RaiseListChangedEvents = true;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRange", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 104);
		}
	}

	public void RemoveRange(IEnumerable<T> items)
	{
		try
		{
			base.RaiseListChangedEvents = false;
			foreach (T item in items)
			{
				Remove(item);
			}
			base.RaiseListChangedEvents = true;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRange", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 117);
		}
	}

	public void ResetItems(IEnumerable<T> items)
	{
		try
		{
			if (items == null)
			{
				items = new List<T>();
			}
			base.RaiseListChangedEvents = false;
			Clear();
			foreach (T item in items)
			{
				Add(item);
			}
			base.RaiseListChangedEvents = true;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetItems", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\SortableBindingList.cs", 132);
		}
	}
}
