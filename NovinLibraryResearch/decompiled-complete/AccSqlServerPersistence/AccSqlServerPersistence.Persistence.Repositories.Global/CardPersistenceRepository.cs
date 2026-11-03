using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Global;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Global;

namespace AccSqlServerPersistence.Persistence.Repositories.Global;

internal class CardPersistenceRepository : PersistenceRepository<CardBusiness, Card>, ICardRepository, IRepository<CardBusiness>, INoGuidRepository<CardBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CardBusiness, Card>, IItemAsyncMapper<Card, CardBusiness>
	{
		public Task<Card> MapAsync(string contextGuid, CardBusiness source, Card destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Card();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.TextString1 = Json.ToStringJson(source.Text1);
				destination.TextString2 = Json.ToStringJson(source.Text2);
				destination.TextString3 = Json.ToStringJson(source.Text3);
				destination.TextString4 = Json.ToStringJson(source.Text4);
				destination.Theme = source.Theme;
				destination.ToolTipMessage = source.ToolTipMessage;
				destination.Image = source.Image;
				destination.BackColor = source.BackColor;
				destination.Type = source.Type;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 157);
				throw;
			}
		}

		public Task<CardBusiness> MapAsync(string contextGuid, Card source, CardBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CardBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Text1 = source.TextString1.FromJson<CardAttribute>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 174);
				destination.Text2 = source.TextString2.FromJson<CardAttribute>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 175);
				destination.Text3 = source.TextString3.FromJson<CardAttribute>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 176);
				destination.Text4 = source.TextString4.FromJson<CardAttribute>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 177);
				destination.Theme = source.Theme;
				destination.ToolTipMessage = source.ToolTipMessage;
				destination.Image = source.Image;
				destination.BackColor = source.BackColor;
				destination.Type = source.Type;
				return Task.FromResult(destination);
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 189);
				throw;
			}
		}

		public Task<List<Card>> MapListAsync(string contextGuid, IEnumerable<CardBusiness> source, List<Card> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CardBusiness>> MapListAsync(string contextGuid, IEnumerable<Card> source, List<CardBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private CardTargetRelationPersistenceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Card, CardBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Card, CardBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CardBusiness, Card> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public CardPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new CardTargetRelationPersistenceRepository(db);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 44);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CardBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext dbcontext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 52));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				Global_ModelContext global_ModelContext = dbcontext;
				returnedSaveFuncInfo.AddReturnedValue(await global_ModelContext.AddOrUpdateAsync(await Mapper.MapAsync(ContextGuid, item, null, tranName, token), item.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 53);
				if (res.HasError)
				{
					return res;
				}
				IQueryable<CardTargetRelation> queryable = ((IQueryable<CardTargetRelation>)dbcontext.CardTargetRelation).Where((CardTargetRelation q) => q.CardGuid == item.Guid);
				if (queryable != null && queryable.Any())
				{
					IQueryable<CardTargetRelation> queryable2 = ((!(item.TargetGuidList?.Any() ?? false)) ? queryable : queryable.Where((CardTargetRelation q) => !item.TargetGuidList.Contains(q.TargetGuid)));
					if (queryable2 != null && queryable2.Any())
					{
						dbcontext.CardTargetRelation.RemoveRange((IEnumerable<CardTargetRelation>)queryable2);
					}
				}
				if (res.HasError || !(item.DetailList?.Any() ?? false))
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveRangeAsync(item.DetailList?.ToList(), item.Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 66);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 70);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 71);
			}
			return res;
		}
	}

	public override async Task<CardBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 77));
		CardBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				Card card = await c.Card.FindAsync(new object[1] { guid });
				if (card == null)
				{
					result = null;
				}
				else
				{
					CardBusiness item = await MapFromDatabase.MapAsync(ContextGuid, card, null, tranName, token);
					await _detRepo.GetAllAsync(item, token, tranName);
					result = item;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 91);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<CardBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 102));
		IEnumerable<CardBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				List<Card> list = await EntityFrameworkQueryableExtensions.ToListAsync<Card>((IQueryable<Card>)c.Card, token);
				if (list == null || !list.Any())
				{
					result = null;
				}
				else
				{
					List<CardBusiness> mappedList = await MapFromDatabase.MapListAsync(ContextGuid, list, null, tranName, token);
					foreach (CardBusiness item in mappedList)
					{
						await _detRepo.GetAllAsync(item, token, tranName);
					}
					result = mappedList;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs", 117);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\CardPersistenceRepository.cs");
			}
		}
		return result;
	}
}
