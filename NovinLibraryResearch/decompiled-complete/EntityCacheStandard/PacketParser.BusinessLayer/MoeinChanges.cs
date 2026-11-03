using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class MoeinChanges
{
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

	internal static List<MoeinChanges> Convert(List<HesabChanges> lst)
	{
		using (new JaegerMonitor("Convert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\MoeinChanges.cs"))
		{
			try
			{
				List<MoeinChanges> list = new List<MoeinChanges>();
				foreach (HesabChanges item in lst?.Where((HesabChanges q) => q.Level == EnMoeinTafsilRelationLevel.One))
				{
					MoeinChanges moeinChanges = list.FirstOrDefault((MoeinChanges q) => q.MoeinGuid == item.MoeinGuid);
					if (moeinChanges == null)
					{
						moeinChanges = new MoeinChanges
						{
							Account = 0m,
							MoeinGuid = item.MoeinGuid
						};
						list.Add(moeinChanges);
					}
					moeinChanges.Account += item.Account;
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Convert", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\AccountComparer\\MoeinChanges.cs", 37);
				return null;
			}
		}
	}
}
