using System;
using System.Collections.Generic;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace EntityCache.InsertDefault_Sqlite;

public class DefualtHesabGroupObjs
{
	public static List<HesabGroupBusiness> ListAllHesabGroup(string contextGuid)
	{
		try
		{
			return new List<HesabGroupBusiness>
			{
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.DarayiJari, "دارایی های جاری".FixString(), 11, ParentDefaults.HesabTypeGuids.Darayi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.DarayiGheyreJari, "دارایی های غیر جاری".FixString(), 12, ParentDefaults.HesabTypeGuids.Darayi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.BedehiJari, "بدهی های جاری".FixString(), 21, ParentDefaults.HesabTypeGuids.Bedehi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.BedehiGheyreJari, "بدهی های غیر جاری".FixString(), 22, ParentDefaults.HesabTypeGuids.Bedehi, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, "حقوق صاحبان سهام".FixString(), 31, ParentDefaults.HesabTypeGuids.Sarmaye, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.Hazine, "هزینه ها".FixString(), 61, ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.Daramad, "درآمد ها".FixString(), 71, ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.HesabEntezami, "حسابهای انتظامی".FixString(), 91, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.TarafHesabEntezami, "طرف حساب های انتظامی".FixString(), 92, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.HesabSanati, "حساب های صنعتی".FixString(), 93, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true),
				CreateObject(contextGuid, ParentDefaults.HesabGroupGuids.Other, "سایر حساب ها".FixString(), 94, ParentDefaults.HesabTypeGuids.HesabHayeEntezami, ST: true)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllHesabGroup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\DefualtHesabGroupObjs.cs", 33);
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
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\InsertDefault_Sqlite\\DefualtHesabGroupObjs.cs", 53);
			return null;
		}
	}
}
