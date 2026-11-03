using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class AdvDepartmentPersistenceRepository : PersistenceRepository<AdvDepartmentBusiness, AdvDepartment>, IAdvDepartmentRepository, IRepository<AdvDepartmentBusiness>, INoGuidRepository<AdvDepartmentBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AdvDepartmentBusiness, AdvDepartment>, IItemAsyncMapper<AdvDepartment, AdvDepartmentBusiness>
	{
		public Task<AdvDepartment> MapAsync(string contextGuid, AdvDepartmentBusiness source, AdvDepartment destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvDepartment();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.ChatList = source.ChatList;
				destination.ChatData = source.ChatData;
				destination.IsSendChat = source.IsSendChat;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.ChatCount = source.ChatCount;
				destination.ChatCity = source.ChatCity;
				destination.ChatCityData = source.ChatCityData;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 118);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AdvDepartmentBusiness> MapAsync(string contextGuid, AdvDepartment source, AdvDepartmentBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvDepartmentBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Title = source.Title;
				destination.ChatList = source.ChatList;
				destination.ChatData = source.ChatData;
				destination.IsSendChat = source.IsSendChat;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.ChatCount = source.ChatCount;
				destination.ChatCity = source.ChatCity;
				destination.ChatCityData = source.ChatCityData;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 149);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AdvDepartment>> MapListAsync(string contextGuid, IEnumerable<AdvDepartmentBusiness> source, List<AdvDepartment> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AdvDepartmentBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvDepartment> source, List<AdvDepartmentBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<AdvDepartment, AdvDepartmentBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AdvDepartmentBusiness, AdvDepartment> MapToDatabase => Mapper;

	public override IItemMapper<AdvDepartment, AdvDepartmentBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AdvDepartmentPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 23));
		using (new JaegerMonitor("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs"))
		{
			bool result;
			try
			{
				AdvDepartment advDepartment = ((IQueryable<AdvDepartment>)adv_ModelContext.AdvDepartment).FirstOrDefault((AdvDepartment p) => p.Title == name && p.Guid != guid);
				result = advDepartment == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 33);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AdvDepartmentBusiness> GetAsync(string depName, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 45));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs"))
		{
			AdvDepartmentBusiness result;
			try
			{
				_ = 1;
				try
				{
					AdvDepartment source = ((IQueryable<AdvDepartment>)c.AdvDepartment).FirstOrDefault((AdvDepartment p) => p.Title == depName);
					result = await MapFromDatabase.MapAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 55);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvDepartmentPersistenceRepository.cs", 79);
			throw;
		}
	}
}
