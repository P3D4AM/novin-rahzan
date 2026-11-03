using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EntityCache.Business.Accounting.Models;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class MoadelBusiness : IHasContext
{
	private decimal _previewsPrice;

	private List<RasGiriDateRemModel> _ret = new List<RasGiriDateRemModel>();

	private List<MoadelDetBussiness> _list_Input = new List<MoadelDetBussiness>();

	private DateTime _startDate;

	private DateTime _endDate;

	private ENMoadel _algoritm;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public List<RasGiriDateRemModel> Ret => _ret;

	public ENMoadel Algoritm => _algoritm;

	public decimal Moadel { get; private set; }

	public int TotalDays => _ret?.Count ?? 0;

	public decimal TotalMonth
	{
		get
		{
			if ((_ret?.Count ?? 0) <= 0)
			{
				return 0m;
			}
			return $"{_ret.Count / 30}/{_ret.Count % 30}".ParseToDecimal();
		}
	}

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public MoadelBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitAfterPoint;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\MoadelBusiness.cs", 47);
		}
	}

	public MoadelBusiness(string contextGuid, List<MoadelDetBussiness> list, DateTime startDate, DateTime endDate, ENMoadel algoritm)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = ClsCache.GetCache(ContextGuid).MultiLanguageConfig.DigitAfterPoint;
			_startDate = DateTime.Now;
			_endDate = DateTime.Now;
			if (list != null)
			{
				list = list.OrderBy((MoadelDetBussiness e) => e.DateSH)?.ToList();
			}
			_list_Input = list;
			_startDate = startDate;
			_endDate = endDate;
			_algoritm = algoritm;
			CalcData();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\MoadelBusiness.cs", 71);
			throw;
		}
	}

	private ReturnedSaveFuncInfo CalcData()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (_endDate.Subtract(_startDate).Days > 1000)
				{
					_startDate = (from q in _list_Input
						orderby q.DateM
						select q.DateM).FirstOrDefault();
					_endDate = (from q in _list_Input
						orderby q.DateM descending
						select q.DateM).FirstOrDefault();
				}
				if (_endDate.Subtract(_startDate).Days > 1000)
				{
					returnedSaveFuncInfo.AddError("بازه راس گیری نباید بیشتر از 1000 روز باشد. لطفا مجددا تلاش کنید".GetTranslate(), "", "CalcData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\MoadelBusiness.cs", 89);
					return returnedSaveFuncInfo;
				}
				int num = 0;
				decimal num2 = default(decimal);
				for (int i = 0; i <= _endDate.Subtract(_startDate).Days; i++)
				{
					DateTime dateTime = _startDate.AddDays(i);
					decimal price = GetPrice(dateTime);
					Ret.Add(new RasGiriDateRemModel(ContextGuid)
					{
						DateM = dateTime,
						Remain = price
					});
					num++;
					num2 += price;
				}
				Moadel = Math.Round(num2 / (decimal)num, DigitForRound);
				return returnedSaveFuncInfo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CalcData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\MoadelBusiness.cs", 109);
				throw;
			}
		}
	}

	private decimal GetPrice(DateTime date_)
	{
		try
		{
			string dateSH = date_.MiladiToCalture();
			List<MoadelDetBussiness> list = _list_Input?.Where((MoadelDetBussiness q) => q.DateSH == dateSH)?.ToList();
			if (list == null || !list.Any())
			{
				return _previewsPrice;
			}
			if (list.Count == 1)
			{
				_previewsPrice = list[0].Remain;
			}
			else if ((list.Count > 1) & (_algoritm == ENMoadel.Maximum))
			{
				_previewsPrice = (from q in list
					orderby q.Remain descending
					select q.Remain).First();
			}
			else if ((list.Count > 1) & (_algoritm == ENMoadel.Minimum))
			{
				_previewsPrice = (from q in list
					orderby q.Remain
					select q.Remain).First();
			}
			else if ((list.Count > 1) & (_algoritm == ENMoadel.Average))
			{
				decimal num = default(decimal);
				foreach (MoadelDetBussiness item in list)
				{
					num += item.Remain;
				}
				_previewsPrice = Math.Round(num / (decimal)list.Count);
			}
			return _previewsPrice;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\MoadelBusiness.cs", 135);
			throw;
		}
	}
}
