using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace AccSqlServerPersistence.Ertegha;

public class HotelPrdGroupsDefultObjs
{
	public static List<PrdGroupBusiness> ListAllGroupsDefault(string contextGuid)
	{
		try
		{
			return new List<PrdGroupBusiness>
			{
				CreateObject(contextGuid, Guid.Parse("4853fbf3-ec73-4299-b45d-01bfe1e6c64b"), "[سوئیت اداری  Office Suite]".GetTranslate(), ""),
				CreateObject(contextGuid, Guid.Parse("7c91e824-0af1-4907-8279-ffab05ecd27c"), "[اتاق یک نفره]", " [اتاق یک نفره که معادل انگلیسی کلمه سینگل است و فقط یک تخت دارد]")
			}.Where((PrdGroupBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllGroupsDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\HotelPrdGroupsDefultObjs.cs", 27);
			return new List<PrdGroupBusiness>();
		}
	}

	private static PrdGroupBusiness CreateObject(string contextGuid, Guid guid, string name, string desc)
	{
		try
		{
			return new PrdGroupBusiness(contextGuid)
			{
				Guid = guid,
				Name = name,
				Status = true,
				Modified = DateTime.Now,
				Description = desc,
				ChangeStatus = EntityChangeSt.New,
				Code = 0L,
				ContextGuid = contextGuid,
				FullCode = 0L,
				Level = 0,
				ParentGuid = Guid.Empty,
				Special = false,
				TechInfoData = new PrdGroupTechnicalInformations().ToString(),
				Type = EnProductType.Room
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\HotelPrdGroupsDefultObjs.cs", 57);
			return null;
		}
	}
}
