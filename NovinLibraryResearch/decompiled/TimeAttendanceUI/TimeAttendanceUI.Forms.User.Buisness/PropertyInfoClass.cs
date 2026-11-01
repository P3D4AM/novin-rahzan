using System;
using System.Collections.Generic;
using System.Reflection;
using PacketParser;
using SmartAssembly.Attributes;

namespace TimeAttendanceUI.Forms.User.Buisness;

[Serializable]
[DoNotPruneType]
public class PropertyInfoClass
{
	private readonly object _cls;

	private List<PropertyInfoClass> _listPropInfo;

	public string PersianName { get; set; }

	public string Name { get; set; }

	public bool? Checked
	{
		get
		{
			bool.TryParse(PropInfo.GetValue(_cls).ToString(), out var result);
			return result;
		}
		set
		{
			PropInfo.SetValue(_cls, value);
		}
	}

	public PropertyInfo PropInfo { get; set; }

	public List<PropertyInfoClass> ListInfo
	{
		get
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			if (_listPropInfo == null)
			{
				_listPropInfo = new List<PropertyInfoClass>();
				PropertyInfo[] properties = _cls.GetType().GetProperties();
				foreach (PropertyInfo propertyInfo in properties)
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(inherit: true);
					PersianName val = new PersianName(" ");
					object[] array = customAttributes;
					foreach (object obj in array)
					{
						if (obj is PersianName)
						{
							val = (PersianName)((obj is PersianName) ? obj : null);
							break;
						}
					}
					_listPropInfo.Add(new PropertyInfoClass(propertyInfo, val.Text, propertyInfo.Name, _cls));
				}
			}
			return _listPropInfo;
		}
	}

	public PropertyInfoClass(object cls)
	{
		_cls = cls;
	}

	public PropertyInfoClass(PropertyInfo propInfo, string persianName, string name, object cls)
	{
		PropInfo = propInfo;
		Name = name;
		PersianName = persianName;
		_cls = cls;
	}
}
