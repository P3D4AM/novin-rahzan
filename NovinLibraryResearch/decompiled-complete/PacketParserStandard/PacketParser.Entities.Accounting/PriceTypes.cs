using System;
using System.Collections.Generic;

namespace PacketParser.Entities.Accounting;

public class PriceTypes : EnumForUi<EnPriceType>
{
	public static List<PriceTypes> GetSellPrices()
	{
		List<PriceTypes> list = new List<PriceTypes>();
		try
		{
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.none
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.LastSellToAll
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.LastSellToCus
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell1
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell2
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell3
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell4
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell5
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell6
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell7
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell8
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.Regular
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSellPrices", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\PriceTypes.cs", 26);
		}
		return list;
	}

	public static List<PriceTypes> GetSellPrices_fixedTo8Items()
	{
		List<PriceTypes> list = new List<PriceTypes>();
		try
		{
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell1
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell2
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell3
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell4
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell5
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell6
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell7
			});
			list.Add(new PriceTypes
			{
				ValueMember = EnPriceType.sell8
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSellPrices_fixedTo8Items", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\PriceTypes.cs", 43);
		}
		return list;
	}
}
