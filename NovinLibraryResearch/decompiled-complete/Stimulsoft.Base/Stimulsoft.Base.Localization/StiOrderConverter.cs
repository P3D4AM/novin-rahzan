using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Stimulsoft.Base.Localization;

public class StiOrderConverter : ExpandableObjectConverter
{
	private class OrderData : IComparable
	{
		public int Position { get; }

		public string Name { get; }

		public int CompareTo(object obj)
		{
			OrderData orderData = obj as OrderData;
			if (orderData.Position != Position)
			{
				return -orderData.Position.CompareTo(Position);
			}
			return string.Compare(Name, orderData.Name);
		}

		public OrderData(string name, int position)
		{
			Position = position;
			Name = name;
		}
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);
		List<OrderData> list = new List<OrderData>();
		foreach (PropertyDescriptor item in properties)
		{
			if (item.Attributes[typeof(StiOrderAttribute)] is StiOrderAttribute stiOrderAttribute)
			{
				list.Add(new OrderData(item.Name, stiOrderAttribute.Position));
			}
			else
			{
				list.Add(new OrderData(item.Name, 0));
			}
		}
		list.Sort();
		return properties.Sort(list.Select((OrderData orderData) => orderData.Name).ToArray());
	}
}
