using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidMapper : IItemAsyncMapper<TolidBusiness, TolidBusiness>
{
	public static TolidMapper Instance { get; private set; } = new TolidMapper();


	public async Task<TolidBusiness> MapAsync(string contextGuid, TolidBusiness source, TolidBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 14;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = await TolidBusiness.GenerateNewAsync(contextGuid, EnSystem.Mapper, token);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			TolidBusiness tolidBusiness = destination;
			tolidBusiness.SetId2(await source.GetId2Async(token, tranName));
			destination.DateM = source.DateM;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.MavadMoeinGuid = source.MavadMoeinGuid;
			destination.MavadTafsilGuid = source.MavadTafsilGuid;
			destination.MahsolMoeinGuid = source.MahsolMoeinGuid;
			destination.MahsolTafsilGuid = source.MahsolTafsilGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.SanadAffective = source.SanadAffective;
			destination.KardexAffective = source.KardexAffective;
			destination.ContextGuid = contextGuid;
			OrderBusiness mavadList = await source.GetMavadListAsync(EnSystem.Mapper, tranName, token);
			OrderBusiness exportList = await source.GetExportListAsync(EnSystem.Mapper, tranName, token);
			IReadOnlyList<IBaseHazineBusiness> hazineList = source.HazineList;
			TaxUnits taxUnits = await LocalSettingBusiness.GetTaxUnitsAsync(contextGuid);
			if ((mavadList?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness item2 in mavadList.ListKala)
				{
					OrderDetMapper instance = OrderDetMapper.GetInstance(taxUnits);
					string contextGuid2 = contextGuid;
					OrderDetailBusiness source2 = item2;
					string contextGuid3 = contextGuid;
					OrderDetailBusiness item = await instance.MapAsync(contextGuid2, source2, await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid3, await destination.GetMavadListAsync(EnSystem.Mapper, tranName, token)), tranName);
					await destination.AddMavadListAsync(EnSystem.Mapper, new List<OrderDetailBusiness> { item }, clearDets: false, tranName);
				}
			}
			if ((exportList?.ListKala?.Any()).GetValueOrDefault())
			{
				foreach (OrderDetailBusiness item3 in exportList.ListKala)
				{
					OrderDetMapper instance = OrderDetMapper.GetInstance(taxUnits);
					string contextGuid2 = contextGuid;
					OrderDetailBusiness source2 = item3;
					string contextGuid3 = contextGuid;
					await destination.AddExportAsync(EnSystem.Mapper, await instance.MapAsync(contextGuid2, source2, await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid3, await destination.GetExportListAsync(EnSystem.Mapper, tranName, token)), tranName), token, tranName);
				}
			}
			if (hazineList?.Any() ?? false)
			{
				await destination.AddHazineListAsync(await TolidHazineMapper.Instance.MapListAsync(contextGuid, (IEnumerable<TolidHazineBusiness>)hazineList, new List<TolidHazineBusiness>(), tranName, token), EnSystem.Mapper);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidMapper.cs", 79);
			throw;
		}
	}

	public Task<List<TolidBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidBusiness> source, List<TolidBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
