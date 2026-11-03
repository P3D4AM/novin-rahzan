using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using PacketParser.Entities;

namespace FormCustom;

public class PropertyComparer<T> : IComparer<T>
{
	private readonly IComparer comparer;

	private PropertyDescriptor propertyDescriptor;

	private int reverse;

	public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
	{
		try
		{
			propertyDescriptor = property;
			Type type = typeof(Comparer<>).MakeGenericType(property.PropertyType);
			comparer = (IComparer)type.InvokeMember("Default", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null);
			SetListSortDirection(direction);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PropertyComparer.cs", 26);
		}
	}

	public int Compare(T x, T y)
	{
		return reverse * comparer.Compare(propertyDescriptor.GetValue(x), propertyDescriptor.GetValue(y));
	}

	private void SetPropertyDescriptor(PropertyDescriptor descriptor)
	{
		propertyDescriptor = descriptor;
	}

	private void SetListSortDirection(ListSortDirection direction)
	{
		try
		{
			reverse = ((direction == ListSortDirection.Ascending) ? 1 : (-1));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetListSortDirection", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PropertyComparer.cs", 45);
		}
	}

	public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
	{
		try
		{
			SetPropertyDescriptor(descriptor);
			SetListSortDirection(direction);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPropertyAndDirection", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\PropertyComparer.cs", 55);
		}
	}
}
