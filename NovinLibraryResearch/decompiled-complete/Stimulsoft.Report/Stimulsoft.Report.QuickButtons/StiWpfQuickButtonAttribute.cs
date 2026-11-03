using System;
using System.Collections;

namespace Stimulsoft.Report.QuickButtons;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class StiWpfQuickButtonAttribute : Attribute
{
	public class Sorter : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			StiWpfQuickButtonAttribute stiWpfQuickButtonAttribute = x as StiWpfQuickButtonAttribute;
			StiWpfQuickButtonAttribute stiWpfQuickButtonAttribute2 = y as StiWpfQuickButtonAttribute;
			return -stiWpfQuickButtonAttribute.Order.CompareTo(stiWpfQuickButtonAttribute2.Order);
		}
	}

	public string QuickButtonTypeName { get; }

	public int Order { get; }

	public StiWpfQuickButtonAttribute(Type type)
		: this(type, -1)
	{
	}

	public StiWpfQuickButtonAttribute(Type type, int order)
		: this(type.AssemblyQualifiedName, order)
	{
	}

	public StiWpfQuickButtonAttribute(string quickButtonTypeName)
		: this(quickButtonTypeName, -1)
	{
	}

	public StiWpfQuickButtonAttribute(string quickButtonTypeName, int order)
	{
		QuickButtonTypeName = quickButtonTypeName;
		Order = order;
	}
}
