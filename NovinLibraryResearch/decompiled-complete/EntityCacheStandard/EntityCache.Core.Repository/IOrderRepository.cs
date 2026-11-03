using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Core.Repository;

public interface IOrderRepository : IEntityRepository<OrderBusiness, OrderReportBusiness>, IEditableDelivery, IDisposable
{
	IOrderDetailRepository OrderDetailRepo { get; }

	Task<Guid> GetById2Async(bool? status, CancellationToken token, string tranName, int id2, Document factorType);

	Task<int> NextId2Async(Document factorType, bool? status, int? statusId, CancellationToken token, string tranName);

	Task<bool> CustomerIsSend(Guid guid, CancellationToken token, string tranName);

	Task<IEnumerable<OrderReportBusiness>> GetAllAsync(OrderFilter filter, CancellationToken token, string tranName);

	Task<OrderBusiness> GetAsyncByReceptionGuidAsync(Guid receptionGuid, CancellationToken token, string tranName);

	Task<OrderBusiness> GetAsyncByPardakhtGuidAsync(Guid pardakhtGuid, CancellationToken token, string tranName);

	Task<OrderBusiness> GetFromDbAsync(Guid guid, Document factorType, CancellationToken token, string tranName);

	Task<IEnumerable<OrderBusiness>> GetAllAsyncSearchAsync(string search, CancellationToken token, short tasvieshode, short printshode, bool? status, string tranName);

	Task<decimal> TotalSumOfSales(Guid tafsilGuid, DateTime date, Document orderType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAsync(OrderBusiness item, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SetPrintCountAsync(OrderBusiness item, DateTime dateM, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateCustomerNameAsync(OrderBusiness item, string name, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> OrderSortAsync(Document orderType, CancellationToken token, string tranName);

	Task<OrderBusiness> GetAsync(Guid guid, Document factorType, CancellationToken token, string tranName);

	Task<OrderBusiness> GetPreviewsOrderAsync(Document factorType, Guid guid, Guid? cashierGuid, CancellationToken token, string tranName);

	Task<OrderBusiness> GetNextOrderAsync(Document factorType, Guid guid, Guid? cashierGuid, CancellationToken token, string tranName);

	Task<OrderBusiness> GetFirstOrderAsync(Document factorType, DateTime date, Guid? cashierGuid, CancellationToken token, string tranName);

	Task<OrderBusiness> GetLastOrderAsync(Document factorType, DateTime date, Guid? cashierGuid, CancellationToken token, string tranName);

	Task<List<Guid>> GetNotTransferedAsync(Document factorType, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(OperationTransfer filter, CancellationToken token, string tranName);

	Task<int> NextAttachmentNoAsync(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateAttachmentNoAsync(Guid orderGuid, int attachmentNo, CancellationToken token, string tranName);

	Task<Guid> GetDepotGuidAsync(Guid detailGuid, CancellationToken token, string tranName);

	Task<OrderReportBusiness> GetLastOrderByTafsilGuidAsync(Guid tafsilGuid, Document orderType, CancellationToken token, string tranName);

	Task<short> GetNextDailyFactorNoAsync(DateTime dateM, CancellationToken token, string tranName);

	Task<IEnumerable<OrderNotPayedViewModel>> GetAllOrderNotPayedAsync(DateTime orderDate, Guid tafsilGuid, Guid orderGuid, CancellationToken token, string tranName);

	Task<decimal> GetVisitorSumPriceAsync(OrderFilter filter, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(bool value, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(Guid tafsilGuid, string name, CancellationToken token, string tranName);

	Task<IEnumerable<OrderDetailSerialModel>> GetAllDetailSerialAsync(string serial, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(Guid? guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(Document factorType, bool kardexAffective, CancellationToken token, string tranName);

	Task<IEnumerable<OrderTtmsReport>> GetOrderTtmsReportAsync(List<Guid> orderGuidList, CancellationToken token, string tranName);

	Task<OrderReportBusiness> GetReportById2Async(int id2, Document type, bool status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(Guid orderGuid, int statusId, CancellationToken token, string tranName);

	Task<List<ENSource>> GetAllExistingSourceAsync(CancellationToken token, string tranName);

	Task<List<OrderReportBusiness>> GetAllMoghayerat50Async(List<Guid> kolGuidList, CancellationToken token, string tranName);

	Task<IEnumerable<OrdersForUpdateCashBackModel>> GetAllForUpdateCashBackAsync(DateTime? startDate, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateCashBackAsync(Guid orderGuid, Guid? cashBackGuid, bool isProgressed, CancellationToken token, string tranName);

	Task<decimal> GetRelatedBackSellSumAsync(Guid orderGuid, CancellationToken token, string tranName);
}
