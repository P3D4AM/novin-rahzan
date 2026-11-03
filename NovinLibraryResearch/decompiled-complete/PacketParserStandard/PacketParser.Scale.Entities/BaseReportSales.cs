using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Scale.Entities.Interfaces.Reports;

namespace PacketParser.Scale.Entities;

public class BaseReportSales : IReportSales
{
	private DateTime _dateM = DateTime.Now;

	public int AccFactorNo { get; set; }

	public decimal AccountingDiscount { get; set; }

	public decimal AccountingSum { get; set; }

	public decimal AccountingTax { get; set; }

	public decimal AccountingTotalSum { get; set; }

	public Guid AccRegisterGuid { get; set; }

	public DateTime AccRegisterTime { get; set; }

	public decimal AghlamSumCalced
	{
		get
		{
			decimal result = default(decimal);
			if (List == null || List.Count == 0)
			{
				return default(decimal);
			}
			foreach (IReportSaleDet item in List)
			{
				if (item != null)
				{
					result += item.TotalPriceOnScale;
				}
			}
			return result;
		}
	}

	public int ClerkNo { get; set; }

	public short Count { get; set; }

	public string CustFullName { get; set; }

	public DateTime DateM
	{
		get
		{
			if (_dateM.Year > 2100 && _dateM.Year < 2015)
			{
				_dateM = DateTime.Now;
			}
			return _dateM;
		}
		set
		{
			_dateM = value;
		}
	}

	public string DateSh => DateM.MiladiToCalture();

	public decimal DiscountOnScale { get; set; }

	public int FactorNo { get; set; }

	public int Id { get; set; }

	public List<IReportSaleDet> List { get; set; }

	public int MemId { get; set; }

	public string PortIp { get; set; }

	public DateTime ReadDateM { get; set; }

	public string ReadDateSh => ReadDateM.MiladiToCalture();

	public ReadStatus ReadSt { get; set; }

	public int ScaleId { get; set; }

	public string ScaleName { get; set; }

	public string ScaleNo { get; set; }

	public decimal SumOnScale { get; set; }

	public decimal TaxOnScale { get; set; }

	public string Time => DateM.ToShortTimeString();

	public string TotalBarcode { get; set; }

	public decimal TotalSumOnScale { get; set; }

	public string ReadStatusString => ReadSt.GetDisplay().GetTranslate();

	public bool AccRegistered => AccRegisterGuid != Guid.Empty;
}
