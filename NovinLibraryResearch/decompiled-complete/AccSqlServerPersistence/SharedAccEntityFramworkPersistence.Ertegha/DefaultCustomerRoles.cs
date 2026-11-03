using System;
using System.Collections.Generic;
using EntityCache.Business.Customers;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace SharedAccEntityFramworkPersistence.Ertegha;

public class DefaultCustomerRoles
{
	private static List<CustomerRoleBusiness> list = new List<CustomerRoleBusiness>();

	private static CustomerRoleBusiness SetDef(string contextGuid, string name, Guid guid)
	{
		try
		{
			return new CustomerRoleBusiness(contextGuid)
			{
				Name = name,
				Guid = guid,
				Modified = DateTime.Now,
				Status = true,
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = contextGuid,
				Description = "",
				MetaData = null,
				WordPressName = name,
				PriceTypeGuid = ParentDefaults.PriceLevel.Sell1
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDef", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultCustomerRoles.cs", 35);
			return null;
		}
	}

	public static List<CustomerRoleBusiness> ListDefaultData(string contextGuid)
	{
		try
		{
			list.Add(SetDef(contextGuid, "[ویراستار]".GetTranslate(), ParentDefaults.CustomerRole.Editor));
			list.Add(SetDef(contextGuid, "[نویسنده]".GetTranslate(), ParentDefaults.CustomerRole.Author));
			list.Add(SetDef(contextGuid, "[مشارکت کننده]".GetTranslate(), ParentDefaults.CustomerRole.Contributor));
			list.Add(SetDef(contextGuid, "[مشترک]".GetTranslate(), ParentDefaults.CustomerRole.Subscriber));
			list.Add(SetDef(contextGuid, "[مشتری]".GetTranslate(), ParentDefaults.CustomerRole.Customer));
			list.Add(SetDef(contextGuid, "[مدیریت]".GetTranslate(), ParentDefaults.CustomerRole.Administrator));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListDefaultData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultCustomerRoles.cs", 53);
			return null;
		}
	}
}
