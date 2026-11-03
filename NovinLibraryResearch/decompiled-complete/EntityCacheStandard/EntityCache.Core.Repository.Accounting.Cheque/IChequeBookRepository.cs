using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository.Accounting.Cheque;

public interface IChequeBookRepository : IRepository<ChequeBookBusiness>, INoGuidRepository<ChequeBookBusiness>, IDisposable
{
	Task<List<ChequeBookBusiness>> GetAllAsync(Guid? bankGuid, CancellationToken token, string tranName);

	Task<ChequeBookBusiness> GetAsync(string serialNumber, Guid bankGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAllAsync(Guid? bankGuid, short tryCount, CancellationToken token, string tranName);

	Task<string> NextSerialAsync(Guid BankGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(long number1, long number2, Guid bankGuid, bool isSadershode, short tryCount, CancellationToken token, string tranName);
}
