using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.SerializeData;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting;

public class PossBarcodeBusiness
{
	public static async Task<List<string>> GetAllPatternsAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			IEnumerable<BankBusiness> enumerable = await BankBusiness.GetAllAsync(contextGuid, system, "", token, true);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<string> list = new List<string>();
			foreach (BankBusiness item in enumerable?.ToList())
			{
				if (item.BarcodeFormatList == null || item.BarcodeFormatList.Count <= 0)
				{
					continue;
				}
				foreach (string barcodeFormat in item.BarcodeFormatList)
				{
					if (!string.IsNullOrEmpty(barcodeFormat))
					{
						list.Add(barcodeFormat);
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllPatternsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PossBarcodeBusiness.cs", 35);
			throw;
		}
	}

	public static async Task<List<TafsilBusiness>> CompareBarcodeAsync(string contextGuid, EnSystem system, string barcode, CancellationToken token = default(CancellationToken))
	{
		try
		{
			IEnumerable<TafsilBusiness> enumerable = await TafsilBusiness.GetAllAsync(contextGuid, system, new TafsilFilter
			{
				HesabType = HesabType.Bank
			}, token, "", "CompareBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PossBarcodeBusiness.cs");
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<TafsilBusiness> list = new List<TafsilBusiness>();
			foreach (TafsilBusiness item in enumerable?.ToList())
			{
				if (string.IsNullOrEmpty(item.SerializedData))
				{
					continue;
				}
				BankData bankData = item.SerializedData.FromJson<BankData>("CompareBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PossBarcodeBusiness.cs", 49);
				if (bankData == null || bankData.BarcodeFormatList == null || bankData.BarcodeFormatList.Count <= 0)
				{
					continue;
				}
				foreach (string barcodeFormat in bankData.BarcodeFormatList)
				{
					if (!string.IsNullOrEmpty(barcodeFormat) && new PossBarcodeSeprator(barcodeFormat, barcode).IsValid)
					{
						list.Add(item);
						break;
					}
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CompareBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PossBarcodeBusiness.cs", 65);
			throw;
		}
	}
}
