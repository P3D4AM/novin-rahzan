using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class KardexSumModel
{
	public Document SanadType { get; set; }

	public KardexType? Type { get; set; }

	public decimal RowDiscount { get; set; }

	public string RowDiscountForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return RowDiscount.NpToString("N0", 0);
		}
	}

	public decimal? SumOfRowSumExcept_TaxAvarez { get; set; }

	public string SumOfRowSumExcept_TaxAvarezForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumOfRowSumExcept_TaxAvarez.GetValueOrDefault().NpToString("N0", 0);
		}
	}

	public decimal SumQuantity { get; set; }

	public string SumQuantityForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.QT && Type != KardexType.Both)
			{
				return "---";
			}
			return SumQuantity.NpToString("", 0);
		}
	}

	public decimal Bazaryab1Sum { get; set; }

	public string Bazaryab1SumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return Bazaryab1Sum.NpToString("N0", 0);
		}
	}

	public decimal Bazaryab2Sum { get; set; }

	public string Bazaryab2SumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return Bazaryab2Sum.NpToString("N0", 0);
		}
	}

	public decimal Bazaryab3Sum { get; set; }

	public string Bazaryab3SumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return Bazaryab3Sum.NpToString("N0", 0);
		}
	}

	public decimal Bazaryab4Sum { get; set; }

	public string Bazaryab4SumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return Bazaryab4Sum.NpToString("N0", 0);
		}
	}

	public decimal BazaryabSum => Bazaryab1Sum + Bazaryab2Sum + Bazaryab3Sum + Bazaryab4Sum;

	public string BazaryabSumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return BazaryabSum.NpToString("N0", 0);
		}
	}

	public decimal? SumHazine1 { get; set; }

	public string SumHazine1ForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumHazine1.GetValueOrDefault().NpToString("N0", 0);
		}
	}

	public decimal SumHazine2 { get; set; }

	public string SumHazine2ForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumHazine2.NpToString("N0", 0);
		}
	}

	public decimal SumHazine3 { get; set; }

	public string SumHazine3ForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumHazine3.NpToString("N0", 0);
		}
	}

	public decimal SumHazine4 { get; set; }

	public string SumHazine4ForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumHazine4.NpToString("N0", 0);
		}
	}

	public decimal SumHazine5 { get; set; }

	public string SumHazine5ForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumHazine5.NpToString("N0", 0);
		}
	}

	public decimal SumHazine => SumHazine1 ?? (0m + SumHazine2 + SumHazine3 + SumHazine4 + SumHazine5);

	public string SumHazineForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return SumHazine.NpToString("N0", 0);
		}
	}

	public decimal? TotalSood { get; set; }

	public string TotalSoodForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return TotalSood.GetValueOrDefault().NpToString("N0", 0);
		}
	}

	public decimal TaxSum { get; set; }

	public string TaxSumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return TaxSum.NpToString("N0", 0);
		}
	}

	public decimal? AvarezSum { get; set; }

	public string AvarezSumForDisplay
	{
		get
		{
			if (Type.HasValue && Type != KardexType.Dec && Type != KardexType.Both)
			{
				return "---";
			}
			return AvarezSum.GetValueOrDefault().NpToString("N0", 0);
		}
	}

	public int CountOfOrders { get; set; }
}
