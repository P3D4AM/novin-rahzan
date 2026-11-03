using System;
using PacketParser.DefaultsCoding;

namespace PacketParser.Settings.Accounting.AccCoding;

public class BackBuyCodingSettings
{
	private Guid credit_MoeinGuid_Product = Guid.Empty;

	private Guid credit_TafsilGuid_Product = Guid.Empty;

	private Guid debit_MoeinGuid_Discount = Guid.Empty;

	private Guid debit_TafsilGuid_Discount = Guid.Empty;

	private Guid credit_MoeinGuid_Keraye = Guid.Empty;

	private Guid credit_TafsilGuid_Keraye = Guid.Empty;

	private Guid credit_MoeinGuid_Tax = Guid.Empty;

	private Guid credit_TafsilGuid_Tax = Guid.Empty;

	private Guid credit_MoeinGuid_Avarez = Guid.Empty;

	private Guid credit_TafsilGuid_Avarez = Guid.Empty;

	public int Credit_Moein_Product { get; set; } = 33;


	public int Credit_Tafsil_Product { get; set; } = 29775;


	public int Debit_Moein_Discount { get; set; } = 21;


	public int Debit_Tafsil_Discount { get; set; } = 29774;


	public int Credit_Moein_Keraye { get; set; } = 31;


	public int Credit_Tafsil_Keraye { get; set; } = 29771;


	public int Credit_Moein_Tax { get; set; } = 49;


	public int Credit_Tafsil_Tax { get; set; } = 29773;


	public float Percent_Tax { get; set; } = 6f;


	public int Credit_Moein_Avarez { get; set; } = 29843;


	public int Credit_Tafsil_Avarez { get; set; } = 29772;


	public float Percent_Avarez { get; set; } = 3f;


	public Guid Credit_MoeinGuid_Product
	{
		get
		{
			if (credit_MoeinGuid_Product == Guid.Empty)
			{
				credit_MoeinGuid_Product = ParentDefaults.MoeinCoding.CLSMoein33;
			}
			return credit_MoeinGuid_Product;
		}
		set
		{
			credit_MoeinGuid_Product = value;
		}
	}

	public Guid Credit_TafsilGuid_Product
	{
		get
		{
			if (credit_TafsilGuid_Product == Guid.Empty)
			{
				credit_TafsilGuid_Product = ParentDefaults.TafsilCoding.CLSTafsil29775;
			}
			return credit_TafsilGuid_Product;
		}
		set
		{
			credit_TafsilGuid_Product = value;
		}
	}

	public Guid Debit_MoeinGuid_Discount
	{
		get
		{
			if (debit_MoeinGuid_Discount == Guid.Empty)
			{
				debit_MoeinGuid_Discount = ParentDefaults.MoeinCoding.CLSMoein21;
			}
			return debit_MoeinGuid_Discount;
		}
		set
		{
			debit_MoeinGuid_Discount = value;
		}
	}

	public Guid Debit_TafsilGuid_Discount
	{
		get
		{
			if (debit_TafsilGuid_Discount == Guid.Empty)
			{
				debit_TafsilGuid_Discount = ParentDefaults.TafsilCoding.CLSTafsil29774;
			}
			return debit_TafsilGuid_Discount;
		}
		set
		{
			debit_TafsilGuid_Discount = value;
		}
	}

	public Guid Credit_MoeinGuid_Keraye
	{
		get
		{
			if (credit_MoeinGuid_Keraye == Guid.Empty)
			{
				credit_MoeinGuid_Keraye = ParentDefaults.MoeinCoding.CLSMoein31;
			}
			return credit_MoeinGuid_Keraye;
		}
		set
		{
			credit_MoeinGuid_Keraye = value;
		}
	}

	public Guid Credit_TafsilGuid_Keraye
	{
		get
		{
			if (credit_TafsilGuid_Keraye == Guid.Empty)
			{
				credit_TafsilGuid_Keraye = ParentDefaults.TafsilCoding.CLSTafsil29771;
			}
			return credit_TafsilGuid_Keraye;
		}
		set
		{
			credit_TafsilGuid_Keraye = value;
		}
	}

	public Guid Credit_MoeinGuid_Tax
	{
		get
		{
			if (credit_MoeinGuid_Tax == Guid.Empty)
			{
				credit_MoeinGuid_Tax = ParentDefaults.MoeinCoding.CLSMoein49;
			}
			return credit_MoeinGuid_Tax;
		}
		set
		{
			credit_MoeinGuid_Tax = value;
		}
	}

	public Guid Credit_TafsilGuid_Tax
	{
		get
		{
			if (credit_TafsilGuid_Tax == Guid.Empty)
			{
				credit_TafsilGuid_Tax = ParentDefaults.TafsilCoding.CLSTafsil29773;
			}
			return credit_TafsilGuid_Tax;
		}
		set
		{
			credit_TafsilGuid_Tax = value;
		}
	}

	public Guid Credit_MoeinGuid_Avarez
	{
		get
		{
			if (credit_MoeinGuid_Avarez == Guid.Empty)
			{
				credit_MoeinGuid_Avarez = ParentDefaults.MoeinCoding.CLSMoein29843;
			}
			return credit_MoeinGuid_Avarez;
		}
		set
		{
			credit_MoeinGuid_Avarez = value;
		}
	}

	public Guid Credit_TafsilGuid_Avarez
	{
		get
		{
			if (credit_TafsilGuid_Avarez == Guid.Empty)
			{
				credit_TafsilGuid_Avarez = ParentDefaults.TafsilCoding.CLSTafsil29772;
			}
			return credit_TafsilGuid_Avarez;
		}
		set
		{
			credit_TafsilGuid_Avarez = value;
		}
	}
}
