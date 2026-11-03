using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Filters;
using PacketParser.Mapping;

namespace PacketParser.Entities;

public class WebGardeshHesab : IGardeshHesab, IItemAsyncMapper<WebGardeshHesab, WebGardeshHesab>, IReceiver
{
	private static string apiUrl = HttpHelpers.ApiUrl + "Gardesh/";

	public Guid? AsGuid { get; set; }

	public Document AsType { get; set; }

	public DocumentDet AsTypeEnum { get; set; }

	public string AutoDescriptionShort { get; set; }

	public decimal Bazaryab { get; set; }

	public decimal Credit { get; set; }

	public string CreditForDisplay { get; set; }

	public DateTime Date { get; set; }

	public DateTime DateMoaser { get; set; }

	public string DateMoaserSh { get; set; }

	public string DateSh { get; set; }

	public decimal Debit { get; set; }

	public string DebitForDisplay { get; set; }

	public Guid DecimalGuid { get; set; }

	public short depId { get; set; }

	public Guid DepotGuid { get; set; }

	public string DescForDisplay { get; set; }

	public string Description { get; set; }

	public decimal Discount { get; set; }

	public string DiscountForDisplay { get; }

	public decimal DiscountPercent { get; set; }

	public Guid Guid { get; set; }

	public int Id2 { get; set; }

	public bool Ischecked { get; set; }

	public bool IsFactor { get; set; }

	public Guid KolGuid { get; set; }

	public DateTime Modified { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid PrdGuid { get; set; }

	public IProduct PrdInfo { get; set; }

	public decimal Price { get; set; }

	public string PriceForDisplay { get; set; }

	public decimal QT { get; set; }

	public decimal QT2 { get; set; }

	public decimal Rem { get; set; }

	public string RemForDisplay { get; set; }

	public string RowDesc { get; set; }

	public string RowSumForDisplay { get; set; }

	public string SanadDesc { get; set; }

	public int SanadNO { get; set; }

	public int SanadSt { get; set; }

	public string Serial { get; set; }

	public decimal Sood { get; set; }

	public Guid? TafsilGuid { get; set; }

	public List<GardeshTafsil> LstTafsiliItems { get; set; }

	public string TafsiliItems { get; set; }

	public string TafsilItemName { get; set; }

	public string Tashkhis { get; set; }

	public Guid UGuid { get; set; }

	public string UserName { get; set; }

	public string vahedForDisplay { get; set; }

	public Guid VGuid { get; set; }

	public async Task<WebGardeshHesab> MapAsync(string contextGuid, WebGardeshHesab source, WebGardeshHesab destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebGardeshHesab();
			}
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.AsTypeEnum = source.AsTypeEnum;
			destination.AutoDescriptionShort = source.AutoDescriptionShort;
			destination.Bazaryab = source.Bazaryab;
			destination.Credit = source.Credit;
			destination.CreditForDisplay = source.CreditForDisplay;
			destination.Date = source.Date;
			destination.DateMoaser = source.DateMoaser;
			destination.DateMoaserSh = source.DateMoaserSh;
			destination.DateSh = source.DateSh;
			destination.Debit = source.Debit;
			destination.DebitForDisplay = source.DebitForDisplay;
			destination.DecimalGuid = source.DecimalGuid;
			destination.depId = source.depId;
			destination.DepotGuid = source.DepotGuid;
			destination.DescForDisplay = source.DescForDisplay;
			destination.Description = source.Description;
			destination.Discount = source.Discount;
			destination.DiscountPercent = source.DiscountPercent;
			destination.Guid = source.Guid;
			destination.Id2 = source.Id2;
			destination.Ischecked = source.Ischecked;
			destination.IsFactor = source.IsFactor;
			destination.KolGuid = source.KolGuid;
			destination.Modified = source.Modified;
			destination.MoeinGuid = source.MoeinGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.Price = source.Price;
			destination.PriceForDisplay = source.PriceForDisplay;
			destination.QT = source.QT;
			destination.QT2 = source.QT2;
			destination.Rem = source.Rem;
			destination.RemForDisplay = source.RemForDisplay;
			destination.RowDesc = source.RowDesc;
			destination.RowSumForDisplay = source.RowSumForDisplay;
			destination.SanadDesc = source.SanadDesc;
			destination.SanadNO = source.SanadNO;
			destination.SanadSt = source.SanadSt;
			destination.Serial = source.Serial;
			destination.Sood = source.Sood;
			destination.TafsilGuid = source.TafsilGuid;
			destination.TafsiliItems = source.TafsiliItems;
			destination.TafsilItemName = source.TafsilItemName;
			destination.Tashkhis = source.Tashkhis;
			destination.UGuid = source.UGuid;
			destination.UserName = source.UserName;
			destination.vahedForDisplay = source.vahedForDisplay;
			destination.VGuid = source.VGuid;
			destination.PrdInfo = source.PrdInfo;
			if (source.LstTafsiliItems != null)
			{
				WebGardeshHesab webGardeshHesab = destination;
				webGardeshHesab.LstTafsiliItems = await new GardeshTafsil().MapListAsync(contextGuid, source.LstTafsiliItems, new List<GardeshTafsil>(), tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshHesab.cs", 138);
		}
		return destination;
	}

	public Task<List<WebGardeshHesab>> MapListAsync(string contextGuid, IEnumerable<WebGardeshHesab> source, List<WebGardeshHesab> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionEnumName(short action)
	{
		throw new NotImplementedException();
	}

	public string GetPacketActionName(short action)
	{
		throw new NotImplementedException();
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebGardeshHesab>>> GetDataAsync(string userName, string password, GardeshTafsiliFilter filter, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebGardeshHesab>> result = new ReturnedSaveFuncInfoWithValue<List<WebGardeshHesab>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> returnedSaveFuncInfoWithValue = await DepClient.GetByUserAsync(userName, password, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshHesab.cs", 166);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null || returnedSaveFuncInfoWithValue.value.Data == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshHesab.cs", 170);
				return result;
			}
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await new RestTokenManagement(returnedSaveFuncInfoWithValue.value.Data.Type, userName, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshHesab.cs", 174);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue2.value ?? "") ?? ""
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebGardeshHesab>>> returnedSaveFuncInfoWithValue3 = await HttpHelpers.PostToApi<List<WebGardeshHesab>, GardeshTafsiliFilter>(filter, apiUrl + "GetAll", headers, strongDeserialize: false, 15000, 3);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshHesab.cs", 181);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue3.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebGardeshHesab.cs", 187);
		}
		return result;
	}
}
