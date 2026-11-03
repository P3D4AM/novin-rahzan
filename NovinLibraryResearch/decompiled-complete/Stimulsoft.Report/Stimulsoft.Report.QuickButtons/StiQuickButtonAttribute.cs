using System;
using System.Collections;

namespace Stimulsoft.Report.QuickButtons;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class StiQuickButtonAttribute : Attribute
{
	internal class Sorter : IComparer
	{
		int IComparer.Compare(object x, object y)
		{
			StiQuickButtonAttribute stiQuickButtonAttribute = x as StiQuickButtonAttribute;
			StiQuickButtonAttribute stiQuickButtonAttribute2 = y as StiQuickButtonAttribute;
			return -stiQuickButtonAttribute.Order.CompareTo(stiQuickButtonAttribute2.Order);
		}
	}

	public string QuickButtonTypeName { get; }

	public int Order { get; }

	public StiQuickButtonAttribute(Type type)
		: this(type, -1)
	{
	}

	public StiQuickButtonAttribute(Type type, int order)
		: this(type.AssemblyQualifiedName, order)
	{
	}

	public StiQuickButtonAttribute(string quickButtonTypeName)
		: this(quickButtonTypeName, -1)
	{
	}

	public StiQuickButtonAttribute(string quickButtonTypeName, int order)
	{
		QuickButtonTypeName = quickButtonTypeName;
		Order = order;
	}
}
