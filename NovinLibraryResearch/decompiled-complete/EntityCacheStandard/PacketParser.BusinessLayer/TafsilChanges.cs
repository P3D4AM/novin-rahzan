using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class TafsilChanges
{
	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public decimal Account { get; set; }

	public decimal Debit
	{
		get
		{
			if (!(Account < 0m))
			{
				return 0m;
			}
			return -Account;
		}
	}

	public decimal Credit
	{
		get
		{
			if (!(Account > 0m))
			{
				return 0m;
			}
			return Account;
		}
	}

	internal static List<TafsilChanges> Convert(List<HesabChanges> lst)
	{
		using (new JaegerMonitor("Convert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\TafsilChanges.cs"))
		{
			try
			{
				List<TafsilChanges> list = new List<TafsilChanges>();
				foreach (HesabChanges item in lst)
				{
					TafsilChanges tafsilChanges = list?.FirstOrDefault((TafsilChanges q) => q.TafsilGuid == item.TafsilGuid && q.MoeinGuid == item.MoeinGuid);
					if (tafsilChanges == null)
					{
						tafsilChanges = new TafsilChanges
						{
							Account = 0m,
							TafsilGuid = item.TafsilGuid,
							MoeinGuid = item.MoeinGuid
						};
						list.Add(tafsilChanges);
					}
					tafsilChanges.Account += item.Account;
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Convert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\TafsilChanges.cs", 37);
				return null;
			}
		}
	}
}
