using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Business.Advertise.NoBisunessModels;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Advertise;

namespace EntityCache.Core.Repository.Advertise;

public interface IAdvertiseLogRepository : IRepository<AdvertiseLogBusiness>, INoGuidRepository<AdvertiseLogBusiness>, IDisposable
{
	Task<AdvertiseLogBusiness> GetLastAdvAsync(string advName, string tranName);

	Task<List<AdvertiseLogBusiness>> GetAllSpecialAsync(Expression<Func<IAdvertiseLog, bool>> where, Func<IQueryable<IAdvertiseLog>, IOrderedQueryable<IAdvertiseLog>> orderby, string includes, int takeCount, string tranName);

	Task<int> GetAllAdvInDayFromIP(string ip, AdvertiseType type, string tranName);

	Task<IEnumerable<AdvertiseLogBusiness>> GetAllAsync(string search, short state, AdvertiseType type, CancellationToken token, string tranName);

	Task<List<int>> GetAdvCountInSpecialMounthAsync(DateTime firstDate, DateTime secondDate, AdvertiseType type, string tranName);

	Task<List<int>> GetPublishedAdvCountInSpecialMounthAsync(DateTime firstDate, DateTime secondDate, AdvertiseType type, string tranName);

	Task<AdvertiseLogBusiness> GetAsync(string url, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveRangeByDateAsync(short tryCount, string tranName);

	Task<IEnumerable<AdvertiseLogViewModel>> GetAllAsync(long number, DateTime d1, DateTime d2, AdvStatusCode statusCode, AdvertiseType type, string tranName);

	Task<IEnumerable<AdvertiseLogBusiness>> GetAllNotFullAsync(DateTime date1, DateTime date2, string tranName);

	Task<IEnumerable<AdvertiseLogBusiness>> GetAllEditNeedAsync(DateTime date1, DateTime date2, string tranName);

	Task<IEnumerable<AdvertiseLogBusiness>> GetAdvCountInMountAsync(DateTime date1, DateTime date2, long number, string tranName);

	Task<IEnumerable<AdvertiseLogBusiness>> GetPublishAdvCountInMountAsync(DateTime date1, DateTime date2, long number, string tranName);

	Task<IEnumerable<AdvCountReport>> GetAllAdvCountReportAsync(string monthStart, string monthEnd, string dateSh, string ip, string tranName);

	Task<AdvertiseLogBusiness> GetLastSendAsync(long number, AdvertiseType type, string tranName);

	Task<IEnumerable<AdvertiseLogBusiness>> GetAllByNextUseAsync(DateTime date1, string tranName);
}
