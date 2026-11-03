using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using PacketParser.Entities;

namespace PacketParser.CoreChangeLogger;

public static class ObjectComparer
{
	public class GetValueordinal : Attribute
	{
		public readonly string Value;

		public GetValueordinal(string value)
		{
			Value = value;
		}
	}

	private static string GetDisplayName(PropertyInfo prop)
	{
		try
		{
			return prop.GetCustomAttribute<PersianNameAttribute.PersianName>()?.Text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDisplayName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 22);
			throw;
		}
	}

	private static string GetValueOrdinal(PropertyInfo prop)
	{
		try
		{
			if (!(Attribute.GetCustomAttribute(prop, typeof(GetValueordinal)) is GetValueordinal { Value: var value }))
			{
				return string.Empty;
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValueOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 43);
			throw;
		}
	}

	private static string GetDisplayName(object prop)
	{
		try
		{
			return prop.GetType().GetCustomAttribute<DisplayAttribute>()?.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDisplayName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 67);
			throw;
		}
	}

	private static string GetEnumDisplayName(Enum value)
	{
		try
		{
			return (value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>())?.Name ?? value.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetEnumDisplayName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 82);
			throw;
		}
	}

	private static bool CompareDataType(object oldobject, object newObj)
	{
		try
		{
			if (oldobject != null && newObj != null)
			{
				if (oldobject.GetType() == newObj.GetType())
				{
					return true;
				}
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CompareDataType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 103);
			throw;
		}
	}

	public static string CompareObjects(object oldObj, object newObj)
	{
		try
		{
			List<string> list = new List<string>();
			if (oldObj == null || newObj == null)
			{
				list.Add("یکی از آبجکت\u200cها نال است.");
				return string.Join(Environment.NewLine, list);
			}
			string ordinal = GetOrdinal(oldObj);
			string displayName = GetDisplayName(oldObj);
			Type type = oldObj.GetType();
			if (type.IsEnum)
			{
				if (type.GetCustomAttribute<FlagsAttribute>() != null)
				{
					list.AddRange(CompareFlagsManually((Enum)oldObj, (Enum)newObj));
				}
				else if (!object.Equals(oldObj, newObj))
				{
					string enumDisplayName = GetEnumDisplayName((Enum)oldObj);
					string enumDisplayName2 = GetEnumDisplayName((Enum)newObj);
					if (!string.IsNullOrEmpty(ordinal))
					{
						if (displayName != null)
						{
							list.Add(" در " + displayName + ": " + ordinal + " مقدار تغییر کرده: از  '" + enumDisplayName + "' به  '" + enumDisplayName2 + "' ");
						}
						else
						{
							list.Add("در " + ordinal + ": مقدار تغییر کرده: از '" + enumDisplayName + "'  به  '" + enumDisplayName2 + "' ");
						}
					}
					else if (displayName != null)
					{
						list.Add(" در" + displayName + ": مقدار تغییر کرده: از '" + enumDisplayName + "' به  '" + enumDisplayName2 + "' ");
					}
					else
					{
						list.Add("مقدار تغییر کرده: از  '" + enumDisplayName + "' به  '" + enumDisplayName2 + "' ");
					}
				}
				return string.Join(Environment.NewLine, list);
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.GetIndexParameters().Length != 0)
				{
					continue;
				}
				string displayName2 = GetDisplayName(propertyInfo);
				if (displayName2 == null && !propertyInfo.PropertyType.IsEnum)
				{
					continue;
				}
				if (!CompareDataType(oldObj, newObj))
				{
					return "نوع داده های ورودی یکسان نیست";
				}
				object value = propertyInfo.GetValue(oldObj);
				object value2 = propertyInfo.GetValue(newObj);
				if (value == null && value2 == null)
				{
					continue;
				}
				if (propertyInfo.PropertyType.IsEnum)
				{
					if (propertyInfo.PropertyType.GetCustomAttribute<FlagsAttribute>() != null)
					{
						list.AddRange(CompareFlagsManually((Enum)value, (Enum)value2));
					}
					else
					{
						if (object.Equals(value, value2))
						{
							continue;
						}
						GetEnumDisplayName((Enum)value);
						GetEnumDisplayName((Enum)value2);
						if (displayName2 == null)
						{
							_ = propertyInfo.Name;
						}
						if (!string.IsNullOrEmpty(ordinal))
						{
							if (displayName != null)
							{
								list.Add("در " + displayName + " " + ordinal + "  " + displayName2 + "تغییر کرده: از " + string.Format(" '{0}'", value ?? "null") + " به " + string.Format(" '{0}'", value2 ?? "null") + " ");
							}
							else
							{
								list.Add("در " + ordinal + " :   " + displayName2 + "تغییر کرده: از  " + string.Format("'{0}'", value ?? "null") + " به " + string.Format("'{0}'", value2 ?? "null") + " ");
							}
						}
						else if (displayName != null)
						{
							list.Add("در " + displayName + " " + displayName2 + "  تغییر کرده: از  " + string.Format("'{0}'", value ?? "null") + " به  " + string.Format("'{0}'", value2 ?? "null") + " ");
						}
						else
						{
							list.Add(displayName2 + "  تغییر کرده: از  " + string.Format("'{0}'", value ?? "null") + " به " + string.Format(" '{0}'", value2 ?? "null") + " ");
						}
					}
				}
				else if (propertyInfo.PropertyType == typeof(bool) || Nullable.GetUnderlyingType(propertyInfo.PropertyType) == typeof(bool))
				{
					bool? flag = (bool?)value;
					bool? flag2 = (bool?)value2;
					if (flag == flag2)
					{
						continue;
					}
					string text = ((flag.HasValue && flag.Value) ? " فعال" : " غیرفعال");
					string text2 = ((flag2.HasValue && flag2.Value) ? " فعال" : " غیرفعال");
					if (!string.IsNullOrEmpty(ordinal))
					{
						if (displayName != null)
						{
							list.Add("در  " + displayName + "   " + ordinal + " " + displayName2 + " تغییر کرده: از  '" + text + "'  به  '" + text2 + "' ");
						}
						else
						{
							list.Add(ordinal + " تغییر کرده: از '" + text + "' به '" + text2 + "' ");
						}
					}
					else if (displayName != null)
					{
						list.Add("در " + displayName + " " + displayName2 + " تغییر کرده: از  '" + text + "' به '" + text2 + "'");
					}
					else
					{
						list.Add(displayName2 + "  تغییر کرده: از  '" + text + "' به  '" + text2 + "' ");
					}
				}
				else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) && propertyInfo.PropertyType != typeof(string))
				{
					List<object> list2 = (value as IEnumerable)?.Cast<object>().ToList();
					List<object> list3 = (value2 as IEnumerable)?.Cast<object>().ToList();
					for (int j = 0; j < list2.Count; j = checked(j + 1))
					{
						if (j < list2.Count)
						{
							string item = CompareObjects(list2[j], list3[j]);
							list.Add(item);
						}
					}
				}
				else if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
				{
					string text3 = CompareObjects(value, value2);
					list.AddRange(text3.Split(Environment.NewLine.ToCharArray()));
				}
				else
				{
					if ((value != null || value2 == null) && (value == null || value2 != null) && (value == null || value.Equals(value2)))
					{
						continue;
					}
					if (!string.IsNullOrEmpty(ordinal))
					{
						if (displayName != null)
						{
							list.Add("در " + displayName + ordinal + " " + displayName2 + " تغییر کرده: از " + string.Format("'{0}'", value ?? " null ") + " به " + string.Format("'{0}'", value2 ?? " null ") + "  ");
						}
						else
						{
							list.Add("در " + ordinal + " :   " + displayName2 + " تغییر کرده: از  " + string.Format("'{0}'", value ?? " null ") + "  به  " + string.Format("'{0}'", value2 ?? " null ") + "   ");
						}
					}
					else if (displayName != null)
					{
						list.Add("در  " + displayName + " " + displayName2 + "تغییر کرده: از  " + string.Format("'{0}'", value ?? " null ") + "به " + string.Format(" '{0}'", value2 ?? " null "));
					}
					else
					{
						list.Add(displayName2 + " تغییر کرده از :  " + string.Format("'{0}'", value ?? " خالی ") + "  به   " + string.Format("'{0}'", value2 ?? " خالی "));
					}
				}
			}
			return string.Join(" ", list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CompareObjects", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 318);
			throw;
		}
	}

	private static List<string> CompareFlagsManually(Enum oldValue, Enum newValue)
	{
		try
		{
			List<string> list = new List<string>();
			int oldVal = Convert.ToInt32(oldValue);
			int newVal = Convert.ToInt32(newValue);
			IEnumerable<Enum> enumerable = from Enum f in Enum.GetValues(oldValue.GetType())
				where (Convert.ToInt32(f) & oldVal) == Convert.ToInt32(f) && Convert.ToInt32(f) != 0
				select f;
			IEnumerable<Enum> enumerable2 = from Enum f in Enum.GetValues(newValue.GetType())
				where (Convert.ToInt32(f) & newVal) == Convert.ToInt32(f) && Convert.ToInt32(f) != 0
				select f;
			List<Enum> list2 = enumerable.Except(enumerable2).ToList();
			List<Enum> list3 = enumerable2.Except(enumerable).ToList();
			if (list2.Count == 1 && list3.Count == 1)
			{
				list.Add($"{list2[0]}" + " به " + $"{list3[0]}" + "  تغییر کرده.");
			}
			else
			{
				foreach (Enum item in list2)
				{
					list.Add($"{item}" + " حذف شده. ");
				}
				foreach (Enum item2 in list3)
				{
					list.Add($"{item2}" + " اضافه شده. ");
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CompareFlagsManually", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 356);
			throw;
		}
	}

	private static string GetOrdinal(object value)
	{
		try
		{
			Type type = value.GetType();
			new List<string>();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				string valueOrdinal = GetValueOrdinal(propertyInfo);
				if (!string.IsNullOrEmpty(valueOrdinal))
				{
					object value2 = propertyInfo.GetValue(value);
					if (value2 != null)
					{
						return valueOrdinal + " " + value2.ToString();
					}
				}
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\CoreChangeLogger\\ObjectComparer.cs", 387);
			throw;
		}
	}
}
