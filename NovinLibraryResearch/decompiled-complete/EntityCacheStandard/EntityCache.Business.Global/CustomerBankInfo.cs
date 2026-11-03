using System;
using System.Collections.Generic;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Banks;
using PacketParser.Entities;

namespace EntityCache.Business.Global;

public class CustomerBankInfo
{
	public EnBank BankType { get; set; }

	public string ShabaNumber
	{
		get
		{
			return ShabaModel?.ShabaNumber ?? "";
		}
		set
		{
			if (ShabaModel == null)
			{
				ShabaNumberModel shabaNumberModel2 = (ShabaModel = new ShabaNumberModel());
			}
			ShabaModel.ShabaNumber = value;
		}
	}

	[JsonIgnore]
	public ShabaNumberModel ShabaModel { get; set; }

	public string CardNumber
	{
		get
		{
			return CardModel?.CardNumber;
		}
		set
		{
			if (CardModel == null)
			{
				CardNumberModel cardNumberModel2 = (CardModel = new CardNumberModel());
			}
			CardModel.CardNumber = value;
		}
	}

	[JsonIgnore]
	public CardNumberModel CardModel { get; set; }

	public string HesabNumber { get; set; }

	public string OwnerName { get; set; }

	public string OwnerNationalCode { get; set; }

	public short Ordinal { get; set; }

	public byte[]? GetBankLogo()
	{
		return GetBankLogo(BankType);
	}

	public static byte[]? GetBankLogo(EnBank bankType)
	{
		try
		{
			return bankType switch
			{
				EnBank.Dey => ImageResourceManager.BankLogo_Dey, 
				EnBank.EghtesadNovin => ImageResourceManager.BankLogo_EghtesadNovin, 
				EnBank.Gardeshgari => ImageResourceManager.BankLogo_Gardeshgari, 
				EnBank.IranZamin => ImageResourceManager.BankLogo_IranZamin, 
				EnBank.KarAfarin => ImageResourceManager.BankLogo_KarAfarin, 
				EnBank.Keshavarzi => ImageResourceManager.BankLogo_Keshavarzi, 
				EnBank.Markazi => ImageResourceManager.BankLogo_Markazi, 
				EnBank.Maskan => ImageResourceManager.BankLogo_Maskan, 
				EnBank.MehrIran => ImageResourceManager.BankLogo_MehrIran, 
				EnBank.Mellat => ImageResourceManager.BankLogo_Mellat, 
				EnBank.Melli => ImageResourceManager.BankLogo_Melli, 
				EnBank.Parsian => ImageResourceManager.BankLogo_Parsian, 
				EnBank.Pasargad => ImageResourceManager.BankLogo_Pasargad, 
				EnBank.PostBank => ImageResourceManager.BankLogo_Post, 
				EnBank.Refah => ImageResourceManager.BankLogo_Refah, 
				EnBank.Resalat => ImageResourceManager.BankLogo_Resalat, 
				EnBank.Saderat => ImageResourceManager.BankLogo_Saderat, 
				EnBank.Saman => ImageResourceManager.BankLogo_Saman, 
				EnBank.SanatMadan => ImageResourceManager.BankLogo_SanadMadan, 
				EnBank.Sepah => ImageResourceManager.BankLogo_Sepah, 
				EnBank.Sarmaye => ImageResourceManager.BankLogo_Sarmaye, 
				EnBank.Shahr => ImageResourceManager.BankLogo_Shahr, 
				EnBank.Sina => ImageResourceManager.BankLogo_Sina, 
				EnBank.Tejarat => ImageResourceManager.BankLogo_Tejarat, 
				EnBank.ToseeSaderat => ImageResourceManager.BankLogo_ToseeSaderat, 
				EnBank.ToseeTaavon => ImageResourceManager.BankLogo_ToseeTaavon, 
				_ => null, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBankLogo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\CustomerMetaDataBusiness.cs", 131);
			throw;
		}
	}

	public static List<BankLogoModel> GetAllLogo()
	{
		try
		{
			return new List<BankLogoModel>
			{
				new BankLogoModel
				{
					BankType = EnBank.Dey,
					Image = GetBankLogo(EnBank.Dey)
				},
				new BankLogoModel
				{
					BankType = EnBank.EghtesadNovin,
					Image = GetBankLogo(EnBank.EghtesadNovin)
				},
				new BankLogoModel
				{
					BankType = EnBank.Gardeshgari,
					Image = GetBankLogo(EnBank.Gardeshgari)
				},
				new BankLogoModel
				{
					BankType = EnBank.IranZamin,
					Image = GetBankLogo(EnBank.IranZamin)
				},
				new BankLogoModel
				{
					BankType = EnBank.KarAfarin,
					Image = GetBankLogo(EnBank.KarAfarin)
				},
				new BankLogoModel
				{
					BankType = EnBank.Keshavarzi,
					Image = GetBankLogo(EnBank.Keshavarzi)
				},
				new BankLogoModel
				{
					BankType = EnBank.Markazi,
					Image = GetBankLogo(EnBank.Markazi)
				},
				new BankLogoModel
				{
					BankType = EnBank.Maskan,
					Image = GetBankLogo(EnBank.Maskan)
				},
				new BankLogoModel
				{
					BankType = EnBank.MehrIran,
					Image = GetBankLogo(EnBank.MehrIran)
				},
				new BankLogoModel
				{
					BankType = EnBank.Mellat,
					Image = GetBankLogo(EnBank.Mellat)
				},
				new BankLogoModel
				{
					BankType = EnBank.Melli,
					Image = GetBankLogo(EnBank.Melli)
				},
				new BankLogoModel
				{
					BankType = EnBank.Parsian,
					Image = GetBankLogo(EnBank.Parsian)
				},
				new BankLogoModel
				{
					BankType = EnBank.Pasargad,
					Image = GetBankLogo(EnBank.Pasargad)
				},
				new BankLogoModel
				{
					BankType = EnBank.PostBank,
					Image = GetBankLogo(EnBank.PostBank)
				},
				new BankLogoModel
				{
					BankType = EnBank.Refah,
					Image = GetBankLogo(EnBank.Refah)
				},
				new BankLogoModel
				{
					BankType = EnBank.Resalat,
					Image = GetBankLogo(EnBank.Resalat)
				},
				new BankLogoModel
				{
					BankType = EnBank.Saderat,
					Image = GetBankLogo(EnBank.Saderat)
				},
				new BankLogoModel
				{
					BankType = EnBank.Saman,
					Image = GetBankLogo(EnBank.Saman)
				},
				new BankLogoModel
				{
					BankType = EnBank.SanatMadan,
					Image = GetBankLogo(EnBank.SanatMadan)
				},
				new BankLogoModel
				{
					BankType = EnBank.Sepah,
					Image = GetBankLogo(EnBank.Sepah)
				},
				new BankLogoModel
				{
					BankType = EnBank.Sarmaye,
					Image = GetBankLogo(EnBank.Sarmaye)
				},
				new BankLogoModel
				{
					BankType = EnBank.Shahr,
					Image = GetBankLogo(EnBank.Shahr)
				},
				new BankLogoModel
				{
					BankType = EnBank.Sina,
					Image = GetBankLogo(EnBank.Sina)
				},
				new BankLogoModel
				{
					BankType = EnBank.Tejarat,
					Image = GetBankLogo(EnBank.Tejarat)
				},
				new BankLogoModel
				{
					BankType = EnBank.ToseeSaderat,
					Image = GetBankLogo(EnBank.ToseeSaderat)
				},
				new BankLogoModel
				{
					BankType = EnBank.ToseeTaavon,
					Image = GetBankLogo(EnBank.ToseeTaavon)
				}
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllLogo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\CustomerMetaDataBusiness.cs", 172);
			throw;
		}
	}
}
