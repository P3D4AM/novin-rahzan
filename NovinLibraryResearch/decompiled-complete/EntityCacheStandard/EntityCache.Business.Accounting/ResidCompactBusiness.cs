using System;
using PacketParser;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class ResidCompactBusiness : IHasContext
{
	private decimal _qt;

	private string _desc = "";

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public long ID2 { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public string Description
	{
		get
		{
			if (string.IsNullOrEmpty(_desc))
			{
				_desc = PrdName;
			}
			return _desc;
		}
		set
		{
			_desc = value;
		}
	}

	public DateTime Date1 { get; set; } = DateTime.Now;


	public DateTime Date2 { get; set; } = DateTime.Now;


	public Guid? GuidVahed { get; set; }

	public string VahedName { get; set; }

	public decimal Price { get; set; }

	public decimal Qt
	{
		get
		{
			return _qt.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_qt = value;
		}
	}

	public decimal Beyane { get; set; }

	public decimal Mandeh { get; set; }

	public string PrdModel { get; set; }

	public string PrdColor { get; set; }

	public string PrdSerial { get; set; }

	public Guid? TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public string Problems { get; set; }

	public string Darkhast { get; set; }

	public string Date1Sh => Date1.MiladiToCalture();

	public string Time1
	{
		get
		{
			string text = Date1.Hour.ToString();
			string text2 = Date1.Minute.ToString();
			if (Date1.Hour < 10)
			{
				text = "0" + text;
			}
			if (Date1.Minute < 10)
			{
				text2 = "0" + text2;
			}
			return text + ":" + text2;
		}
	}

	public string PrdName { get; set; }

	public string PrdCode { get; set; }

	public string StateDisplay { get; set; }

	public string ContextGuid { get; set; }

	public bool IsChecked { get; set; } = true;


	public short StatusId { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	public string Reserve { get; set; }

	public Guid SanadStatusReportGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public ResidCompactBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
