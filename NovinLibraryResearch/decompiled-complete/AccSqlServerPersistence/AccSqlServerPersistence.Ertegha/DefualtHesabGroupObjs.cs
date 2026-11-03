using System;
using System.Collections.Generic;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Ertegha;

public class DefualtHesabGroupObjs
{
	public static List<HesabGroupBusiness> ListAllHesabGroup(string contextGuid)
	{
		try
		{
			return new List<HesabGroupBusiness>
			{
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.DarayiJari, "[دارایی های جاری]".GetTranslate().FixString(), 11, ParentDefaults.HesabTypeGuids.Darayi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.DarayiGheyreJari, "[دارایی های غیر جاری]".GetTranslate().FixString(), 12, ParentDefaults.HesabTypeGuids.Darayi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.BedehiJari, "[بدهی های جاری]".GetTranslate().FixString(), 21, ParentDefaults.HesabTypeGuids.Bedehi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.BedehiGheyreJari, "[بدهی های غیر جاری]".GetTranslate().FixString(), 22, ParentDefaults.HesabTypeGuids.Bedehi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, "[حقوق صاحبان سهام]".GetTranslate().FixString(), 31, ParentDefaults.HesabTypeGuids.Sarmaye, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.Hazine, "[هزینه ها]".GetTranslate().FixString(), 61, ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.Daramad, "[درآمد ها]".GetTranslate().FixString(), 71, ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.HesabEntezami, "[حسابهای انتظامی]".GetTranslate().FixString(), 91, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.TarafHesabEntezami, "[طرف حساب های انتظامی]".GetTranslate().FixString(), 92, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.HesabSanati, "[حساب های صنعتی]".GetTranslate().FixString(), 81, ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.Other, "[سایر حساب ها]".GetTranslate().FixString(), 94, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllHesabGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtHesabGroupObjs.cs", 47);
			return new List<HesabGroupBusiness>();
		}
	}

	private static HesabGroupBusiness CreateObject(string contextGuid, Guid guid, string Name, int Code, Guid hesabTypeGuid, bool ST)
	{
		try
		{
			return new HesabGroupBusiness(contextGuid)
			{
				Guid = guid,
				Name = Name,
				Code = Code,
				Status = ST,
				TypeGuid = hesabTypeGuid,
				Modified = DateTime.Now
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtHesabGroupObjs.cs", 68);
			return null;
		}
	}
}
