using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace EntityCache.InsertDefault_Sqlite;

public class DefualtHesabTypeObjs
{
	public static List<HesabTypeBusiness> ListAllHesabTypeDefault(string contextGuid)
	{
		try
		{
			return new List<HesabTypeBusiness>
			{
				CreateObject(ParentDefaults.HesabTypeGuids.Darayi, "دارایی".FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.Bedehi, "بدهی".FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.Sarmaye, "سرمایه".FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, "حساب های سود و زیان".FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.HesabHayeEntezami, "حساب های انتظامی".FixString(), contextGuid)
			}.Where((HesabTypeBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllHesabTypeDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\DefualtHesabTypeObjs.cs", 31);
			return new List<HesabTypeBusiness>();
		}
	}

	private static HesabTypeBusiness CreateObject(Guid guid, string Name, string contextGuid)
	{
		try
		{
			return new HesabTypeBusiness(contextGuid)
			{
				Guid = guid,
				Name = Name
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\DefualtHesabTypeObjs.cs", 46);
			return null;
		}
	}
}
