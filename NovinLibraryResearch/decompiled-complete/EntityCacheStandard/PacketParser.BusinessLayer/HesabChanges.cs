using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class HesabChanges
{
	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	internal decimal Account { get; set; }

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

	public EnMoeinTafsilRelationLevel Level { get; set; } = EnMoeinTafsilRelationLevel.One;

}
