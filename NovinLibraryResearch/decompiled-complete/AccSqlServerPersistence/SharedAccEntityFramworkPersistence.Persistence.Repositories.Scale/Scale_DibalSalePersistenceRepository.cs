using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Scale;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Scale;

internal class Scale_DibalSalePersistenceRepository : PersistenceRepository<Scale_DibalSaleBusiness, Scale_DibalSale>, IScale_DibalSaleRepository, IRepository<Scale_DibalSaleBusiness>, INoGuidRepository<Scale_DibalSaleBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_DibalSale, Scale_DibalSaleBusiness>, IItemAsyncMapper<Scale_DibalSaleBusiness, Scale_DibalSale>
	{
		public Task<Scale_DibalSaleBusiness> MapAsync(string contextGuid, Scale_DibalSale source, Scale_DibalSaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new Scale_DibalSaleBusiness(contextGuid);
				}
				destination.Raw = source.Raw;
				destination.Date = source.Date;
				destination.RawDate = source.RawDate;
				destination.RawTime = source.RawTime;
				destination.Amount1 = source.Amount1;
				destination.Amount2 = source.Amount2;
				destination.Price1 = source.Price1;
				destination.Price2 = source.Price2;
				destination.Discount1 = source.Discount1;
				destination.Discount2 = source.Discount2;
				destination.AmountSign1 = source.AmountSign1;
				destination.AmountSign2 = source.AmountSign2;
				destination.AnimalLink1 = source.AnimalLink1;
				destination.AnimalLink2 = source.AnimalLink2;
				destination.AnimalNumber1 = source.AnimalNumber1;
				destination.AnimalNumber2 = source.AnimalNumber2;
				destination.ClientKey = source.ClientKey;
				destination.FactorNumber = source.FactorNumber;
				destination.OperationType1 = source.OperationType1;
				destination.OperationType2 = source.OperationType2;
				destination.Parsed = source.Parsed;
				destination.Rounding = source.Rounding;
				destination.RoundingSign = source.RoundingSign;
				destination.SectionMessageNumber = source.SectionMessageNumber;
				destination.SectionNumber = source.SectionNumber;
				destination.Seller = source.Seller;
				destination.TotalDiscount = source.TotalDiscount;
				destination.TotalSum = source.TotalSum;
				destination.TotalSumSign = source.TotalSumSign;
				destination.VatType1 = source.VatType1;
				destination.VatType2 = source.VatType2;
				destination.Weight1 = source.Weight1;
				destination.Weight2 = source.Weight2;
				destination.Key = source.Key;
				destination.Key2 = source.Key2;
				destination.Group = source.Group;
				destination.Code1 = source.Code1;
				destination.Code2 = source.Code2;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_DibalSalePersistenceRepository.cs", 99);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_DibalSale> MapAsync(string contextGuid, Scale_DibalSaleBusiness source, Scale_DibalSale destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new Scale_DibalSale();
				}
				destination.Raw = source.Raw;
				destination.Date = source.Date;
				destination.RawDate = source.RawDate;
				destination.RawTime = source.RawTime;
				destination.Amount1 = source.Amount1;
				destination.Amount2 = source.Amount2;
				destination.Price1 = source.Price1;
				destination.Price2 = source.Price2;
				destination.Discount1 = source.Discount1;
				destination.Discount2 = source.Discount2;
				destination.AmountSign1 = source.AmountSign1;
				destination.AmountSign2 = source.AmountSign2;
				destination.AnimalLink1 = source.AnimalLink1;
				destination.AnimalLink2 = source.AnimalLink2;
				destination.AnimalNumber1 = source.AnimalNumber1;
				destination.AnimalNumber2 = source.AnimalNumber2;
				destination.ClientKey = source.ClientKey;
				destination.FactorNumber = source.FactorNumber;
				destination.OperationType1 = source.OperationType1;
				destination.OperationType2 = source.OperationType2;
				destination.Parsed = source.Parsed;
				destination.Rounding = source.Rounding;
				destination.RoundingSign = source.RoundingSign;
				destination.SectionMessageNumber = source.SectionMessageNumber;
				destination.SectionNumber = source.SectionNumber;
				destination.Seller = source.Seller;
				destination.TotalDiscount = source.TotalDiscount;
				destination.TotalSum = source.TotalSum;
				destination.TotalSumSign = source.TotalSumSign;
				destination.VatType1 = source.VatType1;
				destination.VatType2 = source.VatType2;
				destination.Weight1 = source.Weight1;
				destination.Weight2 = source.Weight2;
				destination.Key = source.Key;
				destination.Key2 = source.Key2;
				destination.Group = source.Group;
				destination.Code1 = source.Code1;
				destination.Code2 = source.Code2;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_DibalSalePersistenceRepository.cs", 158);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_DibalSaleBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_DibalSale> source, List<Scale_DibalSaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_DibalSale>> MapListAsync(string contextGuid, IEnumerable<Scale_DibalSaleBusiness> source, List<Scale_DibalSale> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_DibalSale, Scale_DibalSaleBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_DibalSale, Scale_DibalSaleBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_DibalSaleBusiness, Scale_DibalSale> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public Scale_DibalSalePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_DibalSalePersistenceRepository.cs", 41);
		}
	}
}
