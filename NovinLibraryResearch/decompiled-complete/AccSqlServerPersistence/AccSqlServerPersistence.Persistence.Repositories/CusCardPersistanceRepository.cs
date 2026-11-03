using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Customers;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class CusCardPersistanceRepository : PersistenceRepository<CusCardBusiness, CusCard>, ICusCardRepository, IRepository<CusCardBusiness>, INoGuidRepository<CusCardBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CusCardBusiness, CusCard>, IItemAsyncMapper<CusCard, CusCardBusiness>
	{
		public Task<CusCard> MapAsync(string contextGuid, CusCardBusiness source, CusCard destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs"))
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
						destination = new CusCard();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.CusGuid = source.CusGuid;
					destination.CardName = source.CardName ?? "";
					destination.CardNo = source.CardNo ?? "";
					destination.Status = source.Status;
					destination.Pass = source.Pass ?? "";
					destination.DateStart = source.DateStart;
					destination.DateExpire = source.DateExpire;
					destination.Description = source.Description ?? "";
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 186);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<CusCardBusiness> MapAsync(string contextGuid, CusCard source, CusCardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs"))
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
						destination = new CusCardBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.CusGuid = source.CusGuid;
					destination.CardName = source.CardName;
					destination.CardNo = source.CardNo;
					destination.Status = source.Status;
					destination.Pass = source.Pass;
					destination.DateStart = source.DateStart;
					destination.DateExpire = source.DateExpire;
					destination.Description = source.Description;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 218);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<CusCard>> MapListAsync(string contextGuid, IEnumerable<CusCardBusiness> source, List<CusCard> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CusCardBusiness>> MapListAsync(string contextGuid, IEnumerable<CusCard> source, List<CusCardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CusCard, CusCardBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CusCardBusiness, CusCard> MapToDatabase => Mapper;

	public override IItemMapper<CusCard, CusCardBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public CusCardPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<CusCardBusiness>> GetAllByCusGuidAsync(Guid custGuid, bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAllByCusGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs"))
		{
			IEnumerable<CusCardBusiness> result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByCusGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 38));
					IQueryable<CusCard> queryable = ((IQueryable<CusCard>)c.CustomerCard)?.Where((CusCard x) => x.CusGuid == custGuid);
					if (status.HasValue)
					{
						queryable = queryable?.Where((CusCard q) => (bool?)q.Status == status);
					}
					result = await MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllByCusGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 47);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllByCusGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<CusCardBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs"))
		{
			IEnumerable<CusCardBusiness> result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 62));
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<CusCard> source = (IQueryable<CusCard>)c.CustomerCard;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								source = source.Where((CusCard p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%") || DbFunctionsExtensions.Like(EF.Functions, p.CardName, $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							source = source.Where((CusCard p) => p.Status == ((bool?)status).Value);
						}
						result = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(ContextGuid, source.ToList(), null, tranName, token)) : null);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 80);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CusCardBusiness> GetAsync(string cardNumber, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 91));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs"))
		{
			CusCardBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CusCard cusCard = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CusCard>((IQueryable<CusCard>)c.CustomerCard, (Expression<Func<CusCard, bool>>)((CusCard p) => p.CardNo == cardNumber), token);
					result = ((cusCard != null) ? (await MapFromDatabase.MapAsync(ContextGuid, cusCard, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 103);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 122);
		}
	}

	public async Task<IEnumerable<CustomerCardModel>> GetAllModelAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = null;
		using (new JaegerMonitor("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs"))
		{
			IEnumerable<CustomerCardModel> result;
			try
			{
				_ = 1;
				try
				{
					c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 130));
					result = await EntityFrameworkQueryableExtensions.ToListAsync<CustomerCardModel>(((IQueryable<CusCard>)c.CustomerCard).Select((CusCard q) => new CustomerCardModel
					{
						CardName = q.CardName,
						CardNo = q.CardNo,
						CustomerCode = q.Customers.Tafsil.Code,
						CustomerGuid = q.CusGuid,
						CustomerName = q.Customers.FullName,
						Description = q.Description,
						Guid = q.Guid,
						Pass = q.Pass,
						Status = q.Status
					}), token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs", 148);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CusCardPersistanceRepository.cs");
				}
			}
			return result;
		}
	}
}
