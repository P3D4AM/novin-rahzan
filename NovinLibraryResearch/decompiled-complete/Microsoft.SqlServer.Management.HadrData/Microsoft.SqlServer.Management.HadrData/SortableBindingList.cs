using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.SqlServer.Management.HadrData;

internal class SortableBindingList<T> : BindingList<T>
{
	private PropertyDescriptor propertyDescriptor;

	private ListSortDirection listSortDirection;

	private bool isSorted;

	protected override bool SupportsSortingCore => true;

	protected override bool IsSortedCore => isSorted;

	protected override PropertyDescriptor SortPropertyCore => propertyDescriptor;

	protected override ListSortDirection SortDirectionCore => listSortDirection;

	public SortableBindingList()
	{
	}

	public SortableBindingList(IList<T> list)
		: this()
	{
		base.Items.Clear();
		foreach (T item in list)
		{
			Add(item);
		}
	}

	protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
	{
		List<T> obj = base.Items as List<T>;
		propertyDescriptor = prop;
		listSortDirection = direction;
		isSorted = true;
		obj.Sort(GetComparisonDelegate(prop, direction));
	}

	protected override void RemoveSortCore()
	{
		isSorted = false;
		propertyDescriptor = base.SortPropertyCore;
		listSortDirection = base.SortDirectionCore;
	}

	protected virtual Comparison<T> GetComparisonDelegate(PropertyDescriptor propertyDescriptor, ListSortDirection direction)
	{
		return delegate(T t1, T t2)
		{
			int num = ((direction == ListSortDirection.Ascending) ? 1 : (-1));
			int num2 = ((!(propertyDescriptor.PropertyType == typeof(string))) ? Comparer.Default.Compare(propertyDescriptor.GetValue(t1), propertyDescriptor.GetValue(t2)) : StringComparer.CurrentCulture.Compare(propertyDescriptor.GetValue(t1), propertyDescriptor.GetValue(t2)));
			return num * num2;
		};
	}
}
