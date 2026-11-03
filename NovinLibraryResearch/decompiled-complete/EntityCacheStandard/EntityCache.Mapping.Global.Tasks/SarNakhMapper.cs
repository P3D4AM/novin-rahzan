using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using EntityCache.Business.Global.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global.Tasks;

public class SarNakhMapper : IItemAsyncMapper<SarNakhBusiness, SarNakhBusiness>, IItemAsyncMapper<SarNakhBusiness, CusBusiness>
{
	public static SarNakhMapper Instance { get; private set; } = new SarNakhMapper();


	public async Task<SarNakhBusiness> MapAsync(string contextGuid, SarNakhBusiness source, SarNakhBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 5;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new SarNakhBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Company = source.Company;
			destination.FullName = source.FullName;
			destination.CashierGuid = source.CashierGuid;
			destination.StatusId = source.StatusId;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.MetaData = source.MetaData;
			destination.Description = source.Description;
			destination.IntroductionGuid = source.IntroductionGuid;
			destination.CreationDate = source.CreationDate;
			if (source.LocationList?.Any() ?? false)
			{
				foreach (SarNakhLocationBusiness location in source.LocationList)
				{
					SarNakhLocationMapper instance = SarNakhLocationMapper.Instance;
					string contextGuid2 = contextGuid;
					SarNakhLocationBusiness source2 = location;
					await destination.AddDetailLocationListAsync(await instance.MapAsync(contextGuid2, source2, await SarNakhLocationBusiness.GetFactoryAsync(contextGuid, destination, token), tranName, token));
				}
			}
			if (source.NumbersList?.Any() ?? false)
			{
				foreach (SarNakhNumbersBusiness numbers in source.NumbersList)
				{
					SarNakhNumbersMapper instance2 = SarNakhNumbersMapper.Instance;
					string contextGuid2 = contextGuid;
					SarNakhNumbersBusiness source3 = numbers;
					await destination.AddDetailNumbersListAsync(await instance2.MapAsync(contextGuid2, source3, await SarNakhNumbersBusiness.GetFactoryAsync(contextGuid, destination, token, tranName), tranName, token));
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\SarNakh\\SarNakhMapper.cs", 58);
			throw;
		}
	}

	public async Task<CusBusiness> MapAsync(string contextGuid, SarNakhBusiness source, CusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new CusBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Company = source.Company;
			destination.Family = source.FullName;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			if ((source?.IntroductionGuid ?? Guid.Empty) != Guid.Empty)
			{
				IntroductionMethodBusiness introductionMethodBusiness = await IntroductionMethodBusiness.GetAsync(contextGuid, source.IntroductionGuid.Value, token, tranName);
				if (introductionMethodBusiness != null)
				{
					CusBusiness cusBusiness = destination;
					cusBusiness.Description = cusBusiness.Description + "\r\n" + " نحوه آشنایی".GetTranslate() + ": " + introductionMethodBusiness.Name;
				}
			}
			if (source.LocationList?.Any() ?? false)
			{
				SarNakhLocationBusiness sarNakhLocationBusiness = source.LocationList.SingleOrDefault((SarNakhLocationBusiness q) => q.Ordinal == 1);
				SarNakhLocationBusiness sarNakhLocationBusiness2 = source.LocationList.SingleOrDefault((SarNakhLocationBusiness q) => q.Ordinal == 2);
				SarNakhLocationBusiness sarNakhLocationBusiness3 = source.LocationList.SingleOrDefault((SarNakhLocationBusiness q) => q.Ordinal == 3);
				SarNakhLocationBusiness sarNakhLocationBusiness4 = source.LocationList.SingleOrDefault((SarNakhLocationBusiness q) => q.Ordinal == 4);
				IEnumerable<SarNakhLocationBusiness> enumerable = source.LocationList.Where((SarNakhLocationBusiness q) => q.Ordinal >= 5);
				List<SarNakhLocationBusiness> list = source.LocationList.Where((SarNakhLocationBusiness q) => !string.IsNullOrEmpty(q.WebSite))?.OrderBy((SarNakhLocationBusiness q) => q.Ordinal)?.ToList();
				List<SarNakhLocationBusiness> emailList = source.LocationList.Where((SarNakhLocationBusiness q) => !string.IsNullOrEmpty(q.Email))?.OrderBy((SarNakhLocationBusiness q) => q.Ordinal)?.ToList();
				if (sarNakhLocationBusiness != null)
				{
					destination.Address1 = sarNakhLocationBusiness.FullAddress;
				}
				if (sarNakhLocationBusiness2 != null)
				{
					destination.Address2 = sarNakhLocationBusiness2.FullAddress;
				}
				if (sarNakhLocationBusiness3 != null)
				{
					destination.AddDet(CustomerMetaKey.Address1, sarNakhLocationBusiness3.FullAddress);
				}
				if (sarNakhLocationBusiness4 != null)
				{
					destination.AddDet(CustomerMetaKey.Address2, sarNakhLocationBusiness4.FullAddress);
				}
				if (enumerable != null && enumerable.Any())
				{
					foreach (SarNakhLocationBusiness item in enumerable)
					{
						CusBusiness cusBusiness2 = destination;
						cusBusiness2.Description = cusBusiness2.Description + "\r\n" + " آدرس".GetTranslate() + $" {item.Ordinal}: {item.FullAddress}";
					}
				}
				if (list != null && list.Any())
				{
					foreach (SarNakhLocationBusiness item2 in list)
					{
						CusBusiness cusBusiness3 = destination;
						cusBusiness3.Description = cusBusiness3.Description + "\r\n " + "وب سایت ".GetTranslate() + $"{item2.Ordinal}: {item2.WebSite}";
					}
				}
				if (emailList?.Any() ?? false)
				{
					destination.Email = emailList.First().Email;
					IEnumerable<SarNakhLocationBusiness> enumerable2 = emailList.Where((SarNakhLocationBusiness q) => q.Ordinal != emailList.First().Ordinal);
					if (enumerable2 != null && enumerable2.Any())
					{
						foreach (SarNakhLocationBusiness item3 in enumerable2)
						{
							CusBusiness cusBusiness4 = destination;
							cusBusiness4.Description = cusBusiness4.Description + "\r\n" + " پست الکترونیک".GetTranslate() + $" {item3.Ordinal}: {item3.Email}";
						}
					}
				}
			}
			if (source.NumbersList?.Any() ?? false)
			{
				foreach (SarNakhNumbersBusiness numbers in source.NumbersList)
				{
					destination.AddTafsilNumbers(await SarNakhNumbersMapper.Instance.MapAsync(contextGuid, numbers, new TafsilNumberBusiness(contextGuid), tranName, token));
				}
			}
			destination.Description = destination.Description.Trim();
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\SarNakh\\SarNakhMapper.cs", 129);
			throw;
		}
	}

	public Task<List<SarNakhBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhBusiness> source, List<SarNakhBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CusBusiness>> MapListAsync(string contextGuid, IEnumerable<SarNakhBusiness> source, List<CusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
