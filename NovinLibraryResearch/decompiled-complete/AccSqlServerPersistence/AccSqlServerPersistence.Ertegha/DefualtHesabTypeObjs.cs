using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Ertegha;

public class DefualtHesabTypeObjs
{
	public static List<HesabTypeBusiness> ListAllHesabTypeDefault(string contextGuid)
	{
		try
		{
			return new List<HesabTypeBusiness>
			{
				CreateObject(ParentDefaults.HesabTypeGuids.Darayi, "[دارایی]".GetTranslate().FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.Bedehi, "[بدهی]".GetTranslate().FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.Sarmaye, "[سرمایه]".GetTranslate().FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, "[حساب های سود و زیان]".GetTranslate().FixString(), contextGuid),
				CreateObject(ParentDefaults.HesabTypeGuids.HesabHayeEntezami, "[حساب های انتظامی]".GetTranslate().FixString(), contextGuid)
			}.Where((HesabTypeBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllHesabTypeDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtHesabTypeObjs.cs", 34);
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
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtHesabTypeObjs.cs", 52);
			return null;
		}
	}
}
