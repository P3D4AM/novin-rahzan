using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Ertegha;

public class TafsilLevelDefultObjs
{
	internal static List<TafsilGroupBusiness> ListAllTafsilGroupsDefault(string contextGuid)
	{
		try
		{
			return new List<TafsilGroupBusiness>
			{
				CreateObject(contextGuid, ParentDefaults.TafsilLevelGuids.PersonsAndCompany, "اشخاص و شرکت ها".GetTranslate().FixString(), "CUS", 1L, 199999L, ParentDefaults.MoeinCoding.CLSMoein9),
				CreateObject(contextGuid, ParentDefaults.TafsilLevelGuids.Hazineha, "هزینه ها".GetTranslate().FixString(), "HAZ", 204000L, 204999L, ParentDefaults.MoeinCoding.CLSMoein11),
				CreateObject(contextGuid, ParentDefaults.TafsilLevelGuids.Bardashts, "برداشت ها".GetTranslate().FixString(), "BAR", 205000L, 205999L, ParentDefaults.MoeinCoding.CLSMoein3),
				CreateObject(contextGuid, ParentDefaults.TafsilLevelGuids.Sandooghs, "صندوق ها".GetTranslate().FixString(), "SAN", 202000L, 202999L, ParentDefaults.MoeinCoding.CLSMoein7),
				CreateObject(contextGuid, ParentDefaults.TafsilLevelGuids.Banks, "بانک ها".GetTranslate().FixString(), "BNK", 203000L, 203999L, ParentDefaults.MoeinCoding.CLSMoein4),
				CreateObject(contextGuid, ParentDefaults.TafsilLevelGuids.Others, "سایر".GetTranslate().FixString(), "OTH", 210000L, 219999L, Guid.Empty)
			}.Where((TafsilGroupBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllTafsilGroupsDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\TafsilLevelDefultObjs.cs", 41);
			return new List<TafsilGroupBusiness>();
		}
	}

	private static TafsilGroupBusiness CreateObject(string contextGuid, Guid guid, string name, string tbl, long codeMin, long codeMax, Guid moeinGuid)
	{
		try
		{
			TafsilGroupBusiness tafsilGroupBusiness = new TafsilGroupBusiness(contextGuid);
			tafsilGroupBusiness.Guid = guid;
			tafsilGroupBusiness.Name = name;
			tafsilGroupBusiness.CodeMin = codeMin;
			tafsilGroupBusiness.CodeMax = codeMax;
			tafsilGroupBusiness.Tbl = tbl;
			tafsilGroupBusiness.Status = true;
			tafsilGroupBusiness.Modified = DateTime.Now;
			tafsilGroupBusiness.MoeinSellGuid = moeinGuid;
			tafsilGroupBusiness.MoeinBuyGuid = moeinGuid;
			tafsilGroupBusiness.MoeinBackSellGuid = moeinGuid;
			tafsilGroupBusiness.MoeinBackBuyGuid = moeinGuid;
			tafsilGroupBusiness.MoeinReceptionGuid = moeinGuid;
			tafsilGroupBusiness.MoeinPardakhtGuid = moeinGuid;
			tafsilGroupBusiness.MoeinAmaniMaNazdDigaranGuid = moeinGuid;
			tafsilGroupBusiness.MoeinAmaniDigaranNazdMaGuid = moeinGuid;
			tafsilGroupBusiness.Description = "";
			tafsilGroupBusiness.SetMoeinGuid(moeinGuid);
			return tafsilGroupBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\TafsilLevelDefultObjs.cs", 74);
			return null;
		}
	}
}
