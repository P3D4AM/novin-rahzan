using System;
using System.Collections.Generic;
using System.Reflection;
using PacketParser;
using PacketParser.Attendance.Access;
using PacketParser.Entities.Attendance;
using SmartAssembly.Attributes;

namespace TimeAttendanceUI.Forms.User.Buisness;

[Serializable]
[DoNotPruneType]
public class AccessLevelEnumItems
{
	private object _cls;

	private PropertyInfoClass _info;

	public string Name { get; set; }

	public string PersianName { get; set; }

	public CheckBoxGridviewEnumType Checked { get; set; } = (CheckBoxGridviewEnumType)1;


	public PropertyInfoClass Info => _info ?? (_info = new PropertyInfoClass(_cls));

	public AccessLevelEnumItems(object cls)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_cls = cls;
	}

	public AccessLevelEnumItems(string name, string persianname, object cls)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Name = name;
		_cls = cls;
		PersianName = persianname;
	}

	public static List<AccessLevelEnumItems> ListInfo(AccessLevelClass access)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		List<AccessLevelEnumItems> list = new List<AccessLevelEnumItems>();
		PropertyInfo[] properties = ((object)access).GetType().GetProperties();
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
			if (!(val.Text.Trim() == ""))
			{
				list.Add(new AccessLevelEnumItems(propertyInfo.Name, val.Text, propertyInfo.GetValue(access)));
			}
		}
		return list;
	}
}
