using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class SortableBindingList<T> : BindingList<T>
{
	private class PropertyComparer : Comparer<T>
	{
		private readonly IComparer _comparer;

		private readonly ListSortDirection _direction;

		private readonly PropertyDescriptor _prop;

		public PropertyComparer(PropertyDescriptor prop, ListSortDirection direction)
		{
			if (!prop.ComponentType.IsAssignableFrom(typeof(T)))
			{
				throw new MissingMemberException(typeof(T).Name, prop.Name);
			}
			_prop = prop;
			_direction = direction;
			PropertyInfo declaredProperty = typeof(Comparer<>).MakeGenericType(prop.PropertyType).GetTypeInfo().GetDeclaredProperty("Default");
			_comparer = (IComparer)declaredProperty.GetValue(null, null);
		}

		public override int Compare(T left, T right)
		{
			object value = _prop.GetValue(left);
			object value2 = _prop.GetValue(right);
			if (_direction != 0)
			{
				return _comparer.Compare(value2, value);
			}
			return _comparer.Compare(value, value2);
		}

		public static bool CanSort(Type type)
		{
			if (!(type.GetInterface("IComparable") != null))
			{
				if (type.GetTypeInfo().IsGenericType)
				{
					return type.GetGenericTypeDefinition() == typeof(Nullable<>);
				}
				return false;
			}
			return true;
		}
	}

	private bool _isSorted;

	private ListSortDirection _sortDirection;

	private PropertyDescriptor _sortProperty;

	protected override bool IsSortedCore => _isSorted;

	protected override ListSortDirection SortDirectionCore => _sortDirection;

	protected override PropertyDescriptor SortPropertyCore => _sortProperty;

	protected override bool SupportsSortingCore => true;

	public SortableBindingList([JetBrains.Annotations.NotNull] List<T> list)
		: base((IList<T>)list)
	{
	}

	protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
	{
		if (PropertyComparer.CanSort(prop.PropertyType))
		{
			((List<T>)base.Items).Sort(new PropertyComparer(prop, direction));
			_sortDirection = direction;
			_sortProperty = prop;
			_isSorted = true;
			OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}
	}

	protected override void RemoveSortCore()
	{
		_isSorted = false;
		_sortProperty = null;
	}
}
