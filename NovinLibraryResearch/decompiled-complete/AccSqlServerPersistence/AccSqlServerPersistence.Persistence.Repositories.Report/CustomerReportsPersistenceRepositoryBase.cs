using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Customers;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Core.Repository.Report;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Persistence.Repositories.Report;

internal abstract class CustomerReportsPersistenceRepositoryBase : ICustomerReportsRepository, IDisposable
{
	private bool _isDisposed;

	protected OrderPersistenceRepository _order;

	public ContextHolder DatabaseContext { get; private set; }

	public CustomerReportsPersistenceRepositoryBase(ContextHolder context, ITableRelationShipRepository attachmentRepo)
	{
		DatabaseContext = context;
		_order = new OrderPersistenceRepository(context, attachmentRepo);
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_order.Dispose();
				_order = null;
				DatabaseContext?.Dispose();
				DatabaseContext = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepositoryBase.cs", 40);
		}
	}

	public async Task<IEnumerable<OrderReportBusiness>> GetAllLastTransactionAsync(Document factorType, CancellationToken token, string tranName)
	{
		try
		{
			return await _order.GetAllCustomerLastTransactionAsync(factorType, token, tranName);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Report\\CustomerReportsPersistenceRepositoryBase.cs", 53);
			throw;
		}
	}

	public abstract Task<IEnumerable<RasBedahiBusiness>> GetAllRasBedahi(string contextGuid, DateTime Date1, DateTime Date2, short darsad, CancellationToken token, string tranName = "");

	public abstract Task<List<TafsilCurrencyGardeshModel>> GetAllTafsilCurrencyAsync(Guid tafsilGuid, DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	public abstract Task<List<CustomerVisitPriodModel>> GetAllCustomerVisitPriodAsync(CancellationToken token, string tranName);
}
