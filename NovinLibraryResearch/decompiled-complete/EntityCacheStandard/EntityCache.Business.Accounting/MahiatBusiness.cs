using System;
using System.Collections.Generic;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting;

public class MahiatBusiness
{
	public short Id { get; set; }

	public string Name { get; set; }

	public MahiatBusiness(ENMahiat mahiat)
	{
		try
		{
			Id = (short)mahiat;
			Name = mahiat.GetDisplay().GetTranslate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MahiatBusiness.cs", 22);
		}
	}

	public static List<MahiatBusiness> GetAll()
	{
		return new List<MahiatBusiness>
		{
			new MahiatBusiness(ENMahiat.Debit_Credit),
			new MahiatBusiness(ENMahiat.JustCredit),
			new MahiatBusiness(ENMahiat.JustDebit)
		};
	}
}
