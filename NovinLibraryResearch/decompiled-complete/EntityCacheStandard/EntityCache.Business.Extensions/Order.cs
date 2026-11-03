using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace EntityCache.Business.Extensions;

public static class Order
{
	public static async Task<List<OrderDetailBusiness>> CrateNoneEqualDetailList(this OrderBusiness First, string contextGuid, OrderBusiness Second, string tranName)
	{
		List<OrderDetailBusiness> list = new List<OrderDetailBusiness>();
		try
		{
			if (First == null || Second == null)
			{
				return null;
			}
			foreach (OrderDetailBusiness currentItem in First.ListValidKala ?? new List<OrderDetailBusiness>())
			{
				if (Second.ListValidKala.Where((OrderDetailBusiness p) => p.ProductGuid == currentItem.ProductGuid && p.RowSum == currentItem.RowSum).FirstOrDefault() == null)
				{
					list.Add(currentItem);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CrateNoneEqualDetailList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Order.cs", 32);
			throw;
		}
		return list;
	}
}
