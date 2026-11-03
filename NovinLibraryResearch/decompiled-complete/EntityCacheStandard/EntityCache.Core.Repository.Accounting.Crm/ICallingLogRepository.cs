using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using EntityCache.Business.Crm.Report;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Crm;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using PacketParser.EventArgs;
using PacketParser.Filters.Crm;

namespace EntityCache.Core.Repository.Accounting.Crm;

public interface ICallingLogRepository : INoGuidRepository<CallingLogBusiness>, IDisposable
{
	Task<CallingLogBusiness> GetLast(Guid coGuid, CancellationToken token, string tranName);

	Task<CallingLogBusiness> GetFromDatabaseAsync(string uuid, CancellationToken token, string tranName);

	Task<List<CallingLogBusiness>> GetAllNotSentAsync(int count, CancellationToken token, string tranName);

	Task<List<CallingLogBusiness>> GetLastRecordsByTime(int count, CancellationToken token, string tranName);

	Task<List<CallingLogBusiness>> SearchAsync<TKey>(Expression<Func<ICallingLog, bool>> predicate, Expression<Func<ICallingLog, TKey>> orderby, int takeCount, bool descending, CancellationToken token, string tranName);

	Task<IEnumerable<CallingLogBusiness>> GetAllAsync(long number, string tafsilName, CancellationToken token, DateTime startDate, DateTime endDate, short maxCount, EnumNotification? type, string tranName, Guid? tafsilGuid);

	Task<ReturnedSaveFuncInfo> DeleteRangeAsync(List<Guid> eConnectionData, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName);

	Task<CallingLogBusiness> GetAsync(string uuid, CancellationToken token, string tranName);

	Task<CallingLogReportBusiness> GetReportAsync(string uuid, CancellationToken token, string tranName);

	Task<IEnumerable<CallingLogReportBusiness>> GetAllAsync(CallingLogFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateMessageStatusAsync(string uuid, string message, short statusId, CancellationToken token, string tranName);

	Task<List<CallTitelSuggest>> GetCallTitelsSuggestAsync(CancellationToken token, string tranName);
}
