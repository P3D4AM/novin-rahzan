using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal;

public class CheckDataValidation
{
	public static async void SaveOrder(string ConnectionUniqueId, List<WebOrder> orderList)
	{
		ClsConnectionData cn = ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
		foreach (WebOrder order in orderList)
		{
			if (cn.Source != ENSource.Android && cn.Source != ENSource.Android_HesabYar && cn.Source != ENSource.Baje)
			{
				if (cn.Source != ENSource.Android_SefareshGir && cn.Source != ENSource.WebSite)
				{
					throw new Exception($"Source of {cn.Source} cannot save Order type {order.FactorType}");
				}
				IHesabTree obj = await cn.GetHesabInfoAsync();
				if (obj?.Guid == Guid.Empty)
				{
					throw new Exception("Customer not logged in");
				}
				if (obj?.TafsilGuid != order.TafsilGuid)
				{
					throw new Exception("Customer could not Save order for another Customer");
				}
			}
		}
	}

	public static void SaveReception(string ConnectionUniqueId)
	{
		ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
	}

	public static void SavePardakht(string ConnectionUniqueId)
	{
		ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
	}

	public static void SaveProducts(string ConnectionUniqueId)
	{
		ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
	}

	public static void SaveCustomers(string ConnectionUniqueId)
	{
		ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
	}

	public static void SaveHesabTree(string ConnectionUniqueId)
	{
		ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
	}
}
