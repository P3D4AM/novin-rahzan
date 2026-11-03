using System;
using System.Reflection;

namespace PacketParser.PosToPc.Sadad.Classes;

public class PosResultBase : System.EventArgs
{
	public string PcPosStatus => Utility.ErrorMessage(PcPosStatusCode);

	public int PcPosStatusCode { get; set; }

	public PosResultBase()
	{
		try
		{
			FieldInfo[] fields = GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				object obj = fieldInfo.GetValue(this);
				if (obj == null)
				{
					obj = ((!fieldInfo.FieldType.IsGenericType) ? "" : Activator.CreateInstance(fieldInfo.FieldType));
				}
				object obj2 = obj;
				if (Utility.IsNumericType(obj2))
				{
					obj2 = "0";
				}
				fieldInfo.SetValue(this, Convert.ChangeType(obj2, fieldInfo.FieldType));
			}
			PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanWrite)
				{
					object obj3 = propertyInfo.GetValue(this, null);
					if (obj3 == null)
					{
						obj3 = ((!propertyInfo.PropertyType.IsGenericType) ? "" : Activator.CreateInstance(propertyInfo.PropertyType));
					}
					object obj4 = obj3;
					if (Utility.IsNumericType(obj4))
					{
						obj4 = "0";
					}
					propertyInfo.SetValue(this, Convert.ChangeType(obj4, propertyInfo.PropertyType), null);
				}
			}
		}
		catch (Exception exception)
		{
			Utility.LogPcPos(exception);
			throw;
		}
	}
}
