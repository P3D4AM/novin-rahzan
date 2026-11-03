using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public static class EnumHelper
{
	public static Dictionary<TEnum, string> GetPersianNamedValues<TEnum>() where TEnum : struct, Enum
	{
		try
		{
			return (from f in typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public)
				select new
				{
					Value = (TEnum)f.GetValue(null),
					Attr = f.GetCustomAttribute<PersianNameAttribute.PersianName>()
				} into x
				where x.Attr != null
				select x).ToDictionary(x => x.Value, x => x.Attr.Text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPersianNamedValues", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\EnumHelper.cs", 28);
			return null;
		}
	}
}
