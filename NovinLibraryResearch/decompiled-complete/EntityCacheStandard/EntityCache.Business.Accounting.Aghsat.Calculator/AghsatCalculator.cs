using System;
using System.Collections.Generic;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.Aghsat.Calculator;

public class AghsatCalculator : IAghsatBusiness, IHasContext
{
	private readonly List<AghsatMBusiness> _aghsat;

	public string ContextGuid { get; private set; }

	public AghsatCalculator(string contextGuid, List<AghsatMBusiness> aghsat)
	{
		ContextGuid = contextGuid;
		_aghsat = aghsat;
	}

	public decimal Calculate_VajheEltezam(decimal vajheltezam)
	{
		decimal due_Date_Interval = default(decimal);
		decimal jarimeh = default(decimal);
		_aghsat.ForEach(delegate(AghsatMBusiness item)
		{
			due_Date_Interval = item.DateSarResid.Subtract(DateTime.Now).Days;
			if (due_Date_Interval > 0m)
			{
				jarimeh = due_Date_Interval * vajheltezam;
			}
		});
		return jarimeh;
	}
}
