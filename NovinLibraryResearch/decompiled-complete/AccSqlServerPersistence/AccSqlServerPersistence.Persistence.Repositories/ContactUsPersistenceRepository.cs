using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class ContactUsPersistenceRepository : PersistenceSyncRepository<ContactUsBusiness, ContactUs>, IContactUsRepository, ISyncRepository<ContactUsBusiness>, IEditableDelivery, IRepository<ContactUsBusiness>, INoGuidRepository<ContactUsBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ContactUsBusiness, ContactUs>, IItemAsyncMapper<ContactUs, ContactUsBusiness>
	{
		public Task<ContactUs> MapAsync(string contextGuid, ContactUsBusiness source, ContactUs destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs"))
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
						destination = new ContactUs();
					}
					destination.ReferencedId = source.ReferencedId;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Address = source.Address;
					destination.CodeMeli = source.CodeMeli;
					destination.DateBirth = source.DateBirth;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Email = source.Email;
					destination.Family = source.Family;
					destination.Fax = source.Fax;
					destination.Jensiat = source.Jensiat;
					destination.Message = source.Message;
					destination.Mob = source.Mob;
					destination.Mob2 = source.Mob2;
					destination.NahveAshnaee = source.NahveAshnaee;
					destination.Name = source.Name;
					destination.Ostan = source.Ostan;
					destination.SabegheKari = source.SabegheKari;
					destination.Senf = source.Senf;
					destination.Shahrestan = source.Shahrestan;
					destination.Shoghl = source.Shoghl;
					destination.Subject = source.Subject;
					destination.Tahsilat = source.Tahsilat;
					destination.Tel = source.Tel;
					destination.Tel2 = source.Tel2;
					destination.Type = source.Type;
					destination.WebSite = source.WebSite;
					destination.Zip = source.Zip;
					destination.Ip = source.Ip;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs", 125);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<ContactUsBusiness> MapAsync(string contextGuid, ContactUs source, ContactUsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs"))
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
						destination = new ContactUsBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.ReferencedId = source.ReferencedId;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Address = source.Address;
					destination.CodeMeli = source.CodeMeli;
					destination.DateBirth = source.DateBirth;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Email = source.Email;
					destination.Family = source.Family;
					destination.Fax = source.Fax;
					destination.Jensiat = source.Jensiat;
					destination.Message = source.Message;
					destination.Mob = source.Mob;
					destination.Mob2 = source.Mob2;
					destination.NahveAshnaee = source.NahveAshnaee;
					destination.Name = source.Name;
					destination.Ostan = source.Ostan;
					destination.SabegheKari = source.SabegheKari;
					destination.Senf = source.Senf;
					destination.Shahrestan = source.Shahrestan;
					destination.Shoghl = source.Shoghl;
					destination.Subject = source.Subject;
					destination.Tahsilat = source.Tahsilat;
					destination.Tel = source.Tel;
					destination.Tel2 = source.Tel2;
					destination.Type = source.Type;
					destination.WebSite = source.WebSite;
					destination.Zip = source.Zip;
					destination.Ip = source.Ip;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs", 184);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<ContactUs>> MapListAsync(string contextGuid, IEnumerable<ContactUsBusiness> source, List<ContactUs> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ContactUsBusiness>> MapListAsync(string contextGuid, IEnumerable<ContactUs> source, List<ContactUsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ContactUs, ContactUsBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ContactUsBusiness, ContactUs> MapToDatabase => Mapper;

	public override IItemMapper<ContactUs, ContactUsBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ContactUsPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<ContactUsBusiness>> GetAllAsync(TypeContact type, bool? status, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs", 26));
			IEnumerable<ContactUsBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<ContactUs> queryable = ((IQueryable<ContactUs>)c.ContactUs)?.Where((ContactUs s) => s.Type == type);
					if (status.HasValue)
					{
						queryable = queryable?.Where((ContactUs q) => (bool?)q.Status == status);
					}
					IItemAsyncMapper<ContactUs, ContactUsBusiness> mapFromDatabase = MapFromDatabase;
					string contextGuid = ContextGuid;
					result = await mapFromDatabase.MapListAsync(contextGuid, await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ContactUs>(queryable, token) : null), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ContactUsPersistenceRepository.cs", 59);
		}
	}
}
