using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business.Accounting;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Ertegha;

public class DefaultMoeinTafsilLevelRelationsObjs
{
	internal static List<MoeinTafsilGroupRelationsBusiness> ListAllMoeinTafsilGroupRelationsDefault(string contextGuid)
	{
		try
		{
			return new List<MoeinTafsilGroupRelationsBusiness>
			{
				CreateObject(contextGuid, ParentDefaults.MoeinCoding.CLSMoein9, ParentDefaults.TafsilLevelGuids.PersonsAndCompany),
				CreateObject(contextGuid, ParentDefaults.MoeinCoding.CLSMoein11, ParentDefaults.TafsilLevelGuids.Hazineha),
				CreateObject(contextGuid, ParentDefaults.MoeinCoding.CLSMoein3, ParentDefaults.TafsilLevelGuids.Bardashts),
				CreateObject(contextGuid, ParentDefaults.MoeinCoding.CLSMoein7, ParentDefaults.TafsilLevelGuids.Sandooghs),
				CreateObject(contextGuid, ParentDefaults.MoeinCoding.CLSMoein4, ParentDefaults.TafsilLevelGuids.Banks)
			}.Where((MoeinTafsilGroupRelationsBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllMoeinTafsilGroupRelationsDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultMoeinTafsilLevelRelationsObjs.cs", 38);
			return new List<MoeinTafsilGroupRelationsBusiness>();
		}
	}

	private static MoeinTafsilGroupRelationsBusiness CreateObject(string contextGuid, Guid moeinGuid, Guid guidLevel)
	{
		try
		{
			return new MoeinTafsilGroupRelationsBusiness(contextGuid)
			{
				MoeinGuid = moeinGuid,
				TafsilGroupGuid = guidLevel
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultMoeinTafsilLevelRelationsObjs.cs", 55);
			return null;
		}
	}
}
