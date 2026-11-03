using System;
using Newtonsoft.Json;
using PacketParser.Access;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class KardexViewModel
{
	private clsKardexAccess _access;

	public Guid ProductGroupGuid { get; set; }

	public Guid ProductGuid { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public string ProductVahedName { get; set; }

	public decimal ProductOrderPoint { get; set; }

	public Guid DepotGuid { get; set; }

	public string DepotName { get; set; }

	public Document AsType { get; set; }

	public Guid? ProductV2Guid { get; set; }

	public decimal ProductV2Qt { get; set; }

	public string ProductV2Name { get; set; }

	public Guid? ProductV3Guid { get; set; }

	public decimal ProductV3Qt { get; set; }

	public string ProductV3Name { get; set; }

	public Guid? ProductV4Guid { get; set; }

	public decimal ProductV4Qt { get; set; }

	public string ProductV4Name { get; set; }

	public bool PreRemManfiOnGardesh { get; set; }

	public DateTime PreDate { get; set; }

	public decimal PreDiscount { get; set; }

	public decimal? PreDiscountForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_Takhfif)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = PreDiscount;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreDiscountForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 48);
			}
			return result;
		}
	}

	public double PreVorrodQt { get; set; }

	public decimal? PreVorrodQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderBuy:
						if (!_access.Qt_Buy_Vorod)
						{
							result = null;
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Qt_BackSell_Vorod)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Ezafat_Vorod && PreVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Qt_Tolid_Mahsol_Vorod && PreVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Vorod && PreVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = (decimal)PreVorrodQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreVorrodQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 75);
			}
			return result;
		}
	}

	public decimal PreVorrodSum { get; set; }

	public decimal? PreVorrodSumForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderBuy:
						if (!_access.Rialy_Buy_Vorod)
						{
							result = null;
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Rialy_BackSell_Vorod)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Rialy_Anbargardani_Ezafat_Vorod && PreVorrodSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Rialy_Tolid_Mahsol_Vorod && PreVorrodSum > 0m)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Rialy_Enteghal_Vorod && PreVorrodSum > 0m)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = PreVorrodSum;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreVorrodSumForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 102);
			}
			return result;
		}
	}

	public double PreKhoroojQt { get; set; }

	public decimal? PreKhoroojQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderSell:
						if (!_access.Qt_Sell_Khoroj)
						{
							result = null;
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Qt_BackBuy_Khoroj)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Kasr_Khoroj && PreKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Qt_Tolid_Mavad_Khoroj && PreKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Khoroj && PreKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Zayeat:
						if (!_access.Qt_Zayeat_Khoroj)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = (decimal)PreKhoroojQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreKhoroojQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 130);
			}
			return result;
		}
	}

	public decimal PreKhorojSum { get; set; }

	public decimal? PreKhoroojSumForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderSell:
						if (!_access.Rialy_Sell_Khoroj)
						{
							result = null;
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Rialy_BackBuy_Khoroj)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Rialy_Anbargardani_Kasr_khoroj && PreKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Rialy_Tolid_Mavad_Khoroj && PreKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Rialy_Enteghal_Khoroj && PreKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Zayeat:
						if (!_access.Rialy_Zayeat_Khoroj)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = PreKhorojSum;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreKhoroojSumForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 158);
			}
			return result;
		}
	}

	public decimal PreRemUnitPrice { get; set; }

	public decimal PreTotalSood { get; set; }

	public decimal? PreTotalSoodForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_TotalSood)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = PreTotalSood;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreTotalSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 174);
			}
			return result;
		}
	}

	public decimal PreRemTotalPrice { get; set; }

	public decimal? PreRemTotalPriceForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Qt_Mande)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = PreRemTotalPrice;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PreRemTotalPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 189);
			}
			return result;
		}
	}

	public decimal PreVisitorPrice { get; set; }

	public decimal PreUnitSood
	{
		get
		{
			if (!(PreKhoroojQt > 0.0))
			{
				return 0m;
			}
			return Math.Round(PreTotalSood / (decimal)PreKhoroojQt, DigitForRound);
		}
	}

	public bool BetweenRemManfiOnGardesh { get; set; }

	public DateTime BetweenDate { get; set; }

	public decimal BetweenDiscount { get; set; }

	public decimal? BetweenDiscountForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_Takhfif)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = BetweenDiscount;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenDiscountForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 209);
			}
			return result;
		}
	}

	public double BetweenVorrodQt { get; set; }

	public decimal? BetweenVorrodQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderBuy:
						if (!_access.Qt_Buy_Vorod)
						{
							result = null;
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Qt_BackSell_Vorod)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Ezafat_Vorod && BetweenVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Qt_Tolid_Mahsol_Vorod && BetweenVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Vorod && BetweenVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = (decimal)BetweenVorrodQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenVorrodQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 236);
			}
			return result;
		}
	}

	public decimal BetweenVorrodSum { get; set; }

	public decimal? BetweenVorrodSumForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderBuy:
						if (!_access.Rialy_Buy_Vorod)
						{
							result = null;
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Rialy_BackSell_Vorod)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Rialy_Anbargardani_Ezafat_Vorod && BetweenVorrodSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Rialy_Tolid_Mahsol_Vorod && BetweenVorrodSum > 0m)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Rialy_Enteghal_Vorod && BetweenVorrodSum > 0m)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = BetweenVorrodSum;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenVorrodSumForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 263);
			}
			return result;
		}
	}

	public double BetweenKhoroojQt { get; set; }

	public decimal? BetweenKhoroojQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderSell:
						if (!_access.Qt_Sell_Khoroj)
						{
							result = null;
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Qt_BackBuy_Khoroj)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Kasr_Khoroj && BetweenKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Qt_Tolid_Mavad_Khoroj && BetweenKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Khoroj && BetweenKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Zayeat:
						if (!_access.Qt_Zayeat_Khoroj)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = (decimal)BetweenKhoroojQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenKhoroojQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 291);
			}
			return result;
		}
	}

	public decimal BetweenKhorojSum { get; set; }

	public decimal? BetweenKhoroojSumForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderSell:
						if (!_access.Rialy_Sell_Khoroj)
						{
							result = null;
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Rialy_BackBuy_Khoroj)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Rialy_Anbargardani_Kasr_khoroj && BetweenKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Rialy_Tolid_Mavad_Khoroj && BetweenKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Rialy_Enteghal_Khoroj && BetweenKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Zayeat:
						if (!_access.Rialy_Zayeat_Khoroj)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = BetweenKhorojSum;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenKhoroojSumForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 319);
			}
			return result;
		}
	}

	public decimal BetweenRemUnitPrice { get; set; }

	public decimal BetweenTotalSood { get; set; }

	public decimal? BetweenTotalSoodForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_TotalSood)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = BetweenTotalSood;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenTotalSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 335);
			}
			return result;
		}
	}

	public decimal BetweenRemTotalPrice { get; set; }

	public decimal? BetweenRemTotalPriceForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Qt_Mande)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = BetweenRemTotalPrice;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BetweenRemTotalPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 350);
			}
			return result;
		}
	}

	public decimal BetweenVisitorPrice { get; set; }

	public decimal BetweenUnitSood
	{
		get
		{
			if (!(BetweenKhoroojQt > 0.0))
			{
				return 0m;
			}
			return Math.Round(BetweenTotalSood / (decimal)BetweenKhoroojQt, DigitForRound);
		}
	}

	public bool AfterRemManfiOnGardesh { get; set; }

	public DateTime AfterDate { get; set; }

	public decimal AfterDiscount { get; set; }

	public decimal? AfterDiscountForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_Takhfif)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = AfterDiscount;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterDiscountForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 370);
			}
			return result;
		}
	}

	public double AfterVorrodQt { get; set; }

	public decimal? AfterVorrodQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderBuy:
						if (!_access.Qt_Buy_Vorod)
						{
							result = null;
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Qt_BackSell_Vorod)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Ezafat_Vorod && AfterVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Qt_Tolid_Mahsol_Vorod && AfterVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Vorod && AfterVorrodQt > 0.0)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = (decimal)AfterVorrodQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterVorrodQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 397);
			}
			return result;
		}
	}

	public decimal AfterVorrodSum { get; set; }

	public decimal? AfterVorrodSumForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderBuy:
						if (!_access.Rialy_Buy_Vorod)
						{
							result = null;
						}
						break;
					case Document.OrderBackSell:
						if (!_access.Rialy_BackSell_Vorod)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Rialy_Anbargardani_Ezafat_Vorod && AfterVorrodSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mahsul:
						if (!_access.Rialy_Tolid_Mahsol_Vorod && AfterVorrodSum > 0m)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Rialy_Enteghal_Vorod && AfterVorrodSum > 0m)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = AfterVorrodSum;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterVorrodSumForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 424);
			}
			return result;
		}
	}

	public double AfterKhoroojQt { get; set; }

	public decimal? AfterKhoroojQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderSell:
						if (!_access.Qt_Sell_Khoroj)
						{
							result = null;
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Qt_BackBuy_Khoroj)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Qt_Anbargardani_Kasr_Khoroj && AfterKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Qt_Tolid_Mavad_Khoroj && AfterKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Qt_Enteghal_Khoroj && AfterKhoroojQt > 0.0)
						{
							result = null;
						}
						break;
					case Document.Zayeat:
						if (!_access.Qt_Zayeat_Khoroj)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = (decimal)AfterKhoroojQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterKhoroojQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 452);
			}
			return result;
		}
	}

	public decimal AfterKhorojSum { get; set; }

	public decimal? AfterKhoroojSumForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null)
				{
					switch (AsType)
					{
					case Document.OrderSell:
						if (!_access.Rialy_Sell_Khoroj)
						{
							result = null;
						}
						break;
					case Document.OrderBackBuy:
						if (!_access.Rialy_BackBuy_Khoroj)
						{
							result = null;
						}
						break;
					case Document.AnbarGardani:
						if (!_access.Rialy_Anbargardani_Kasr_khoroj && AfterKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Tolid:
					case Document.Tolid_Mavad:
						if (!_access.Rialy_Tolid_Mavad_Khoroj && AfterKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.EnteghalBeyneAnbar:
						if (!_access.Rialy_Enteghal_Khoroj && AfterKhorojSum > 0m)
						{
							result = null;
						}
						break;
					case Document.Zayeat:
						if (!_access.Rialy_Zayeat_Khoroj)
						{
							result = null;
						}
						break;
					}
				}
				if (result.HasValue)
				{
					result = AfterKhorojSum;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterKhoroojSumForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 480);
			}
			return result;
		}
	}

	public decimal AfterRemUnitPrice { get; set; }

	public decimal AfterTotalSood { get; set; }

	public decimal? AfterTotalSoodForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_TotalSood)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = AfterTotalSood;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterTotalSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 496);
			}
			return result;
		}
	}

	public decimal AfterRemTotalPrice { get; set; }

	public decimal? AfterRemTotalPriceForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Qt_Mande)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = AfterRemTotalPrice;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AfterRemTotalPriceForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 511);
			}
			return result;
		}
	}

	public decimal AfterVisitorPrice { get; set; }

	public decimal AfterUnitSood
	{
		get
		{
			if (!(AfterKhoroojQt > 0.0))
			{
				return 0m;
			}
			return Math.Round(AfterTotalSood / (decimal)AfterKhoroojQt, DigitForRound);
		}
	}

	public double TotalVorodQt => PreVorrodQt + BetweenVorrodQt + AfterVorrodQt;

	public decimal TotalVorodSum => PreVorrodSum + BetweenVorrodSum + AfterVorrodSum;

	public double TotalKhorojQt => PreKhoroojQt + BetweenKhoroojQt + AfterKhoroojQt;

	public decimal TotalKhorojSum => PreKhorojSum + BetweenKhorojSum + AfterKhorojSum;

	public double TotalRemQt => TotalVorodQt - TotalKhorojQt;

	public decimal? TotalRemQtForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Qt_Mande)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = (decimal)TotalRemQt;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalRemQtForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 533);
			}
			return result;
		}
	}

	public decimal TotalRemTotalPrice => AfterRemTotalPrice;

	public decimal TotalRemUnitPrice => AfterRemUnitPrice;

	public decimal TotalSood => AfterTotalSood;

	public decimal? TotalSoodForDisplay
	{
		get
		{
			decimal? result = default(decimal);
			try
			{
				if (_access != null && !_access.Rialy_TotalSood)
				{
					result = null;
				}
				if (result.HasValue)
				{
					result = TotalSood;
					return result;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TotalSoodForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 550);
			}
			return result;
		}
	}

	[JsonIgnore]
	public short DigitForRound { get; }

	public KardexViewModel(clsKardexAccess access, short digitForRound)
	{
		try
		{
			_access = access;
			DigitForRound = digitForRound;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\Kardex\\KardexViewModel.cs", 565);
		}
	}
}
