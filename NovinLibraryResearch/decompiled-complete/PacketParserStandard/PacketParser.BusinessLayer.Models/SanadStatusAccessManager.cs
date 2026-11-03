using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.BusinessLayer.Models;

public class SanadStatusAccessManager<T> where T : BaseSanadStatusAccess, new()
{
	public List<T> ListAccess { get; private set; }

	public SanadStatusAccessManager(List<T> listAccess, IEnumerable<ISanadStatusReport> existSanadSttausList)
	{
		try
		{
			Calculate(listAccess, existSanadSttausList);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Sanad\\SanadStatusAccessManager.cs", 20);
		}
	}

	private void Calculate(List<T> listAccess, IEnumerable<ISanadStatusReport> existSanadSttausList)
	{
		try
		{
			if (existSanadSttausList == null || !existSanadSttausList.Any())
			{
				return;
			}
			if (listAccess == null)
			{
				listAccess = new List<T>();
			}
			IEnumerable<ISanadStatusReport> enumerable = existSanadSttausList.Where((ISanadStatusReport q) => !listAccess.Select((T q) => q.StatusId).Contains(q.Id));
			if (enumerable != null && enumerable.Any())
			{
				foreach (ISanadStatusReport item2 in enumerable)
				{
					listAccess.Add(new T
					{
						StatusId = item2.Id,
						StatusName = item2.Name
					});
				}
			}
			foreach (T item in listAccess?.Where((T q) => string.IsNullOrEmpty(q.StatusName)))
			{
				ISanadStatusReport sanadStatusReport = existSanadSttausList?.SingleOrDefault((ISanadStatusReport q) => q.Id == item.StatusId);
				if (sanadStatusReport != null)
				{
					item.StatusName = sanadStatusReport.Name;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Calculate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Sanad\\SanadStatusAccessManager.cs", 40);
		}
		finally
		{
			ListAccess = listAccess;
		}
	}
}
