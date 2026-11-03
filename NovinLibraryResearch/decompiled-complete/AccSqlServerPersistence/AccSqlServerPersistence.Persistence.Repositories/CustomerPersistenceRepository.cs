using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class CustomerPersistenceRepository : PersistenceDetailRepository<CusBusiness, Customer>, ICustomerRepository, IDetailRepository<CusBusiness>, IEditableDelivery, IDisposable
{
	private class mapper : IItemAsyncMapper<CusBusiness, Customer>, IItemAsyncMapper<WebCus, Customer>, IItemAsyncMapper<Customer, CusBusiness>, IItemMapper<Customer, CusBusiness>
	{
		public async Task<Customer> MapAsync(string contextGuid, CusBusiness source, Customer destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new Customer();
					}
					destination.FullName = source.FullName;
					destination.Address1 = source.Address1;
					destination.Address2 = source.Address2;
					destination.CodeMeli = source.CodeMeli;
					destination.Company = source.Company;
					destination.Date = source.Date;
					destination.Description = source.Description ?? "";
					destination.EghtesadiNo = source.EghtesadiNo ?? "";
					destination.Etebar = source.Etebar;
					destination.EtebarCheck = source.EtebarCheck;
					destination.Family = source.Family;
					destination.IdPath = source.IdPath;
					destination.AreaGuid = (((source?.AreaGuid ?? Guid.Empty) == Guid.Empty) ? null : source.AreaGuid);
					destination.IsBazaryab = source.IsBazaryab;
					destination.IsSupervisor = source.IsSupervisor;
					destination.IsEtebari = source.IsEtebari;
					destination.Job = source.Job;
					destination.Location = source.Location ?? "";
					destination.Name = source.Name;
					destination.Password = source.Password ?? "";
					destination.PicFace = source.PicFace ?? "";
					destination.PicSign = source.PicSign ?? "";
					destination.Email = source.Email;
					if (string.IsNullOrEmpty(destination.Email))
					{
						destination.Email = null;
					}
					destination.SupervisorGuid = source.SupervisorGuid;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.User = source.User;
					if (string.IsNullOrEmpty(destination.User))
					{
						destination.User = null;
					}
					destination.Zip = source.Zip ?? "";
					destination.CustType = source.CustType;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.GroupGuid = source.GroupGuid;
					Customer customer = destination;
					customer.VisitorGuid = await source.GetVisitorGuidAsync(null, token, tranName);
					destination.BPercent = source.BPercent;
					destination.InPlacePayment = source.InPlacePayment;
					destination.TitleGuid = source.TitleGuid;
					destination.TafsilLevel = source.TafsilLevel;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.TaxType = source.TaxType;
					destination.ChequeBargashtiValidation = source.ChequeBargashtiValidation;
					destination.OrderTasvieTimeValidation = source.OrderTasvieTimeValidation;
					destination.RoleGuid = (((source?.RoleGuid ?? Guid.Empty) == Guid.Empty) ? null : source.RoleGuid);
					destination.PhoneNumbers = source.PhoneNumbers;
					return destination;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 589);
					throw;
				}
			}
		}

		public Task<Customer> MapAsync(string contextGuid, WebCus source, Customer destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
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
						destination = new Customer();
					}
					destination.FullName = source.FullName;
					destination.Address1 = source.Address1;
					destination.Address2 = source.Address2;
					destination.CodeMeli = source.CodeMeli;
					destination.Company = source.Company;
					destination.Date = source.Date;
					destination.Description = source.Description ?? "";
					destination.EghtesadiNo = source.EghtesadiNo ?? "";
					destination.Etebar = source.Etebar;
					destination.EtebarCheck = source.EtebarCheck;
					destination.Family = source.Family;
					destination.IdPath = source.IdPath;
					destination.AreaGuid = (((source?.AreaGuid ?? Guid.Empty) == Guid.Empty) ? null : source.AreaGuid);
					destination.IsBazaryab = source.IsBazaryab;
					destination.IsSupervisor = source.IsSupervisor;
					destination.IsEtebari = source.IsEtebari;
					destination.Job = source.Job;
					destination.Location = source.Location ?? "";
					destination.Name = source.Name;
					destination.Password = source.Password ?? "";
					destination.PicFace = source.PicFace ?? "";
					destination.PicSign = source.PicSign ?? "";
					destination.Email = source.Email;
					if (string.IsNullOrEmpty(destination.Email))
					{
						destination.Email = null;
					}
					destination.SupervisorGuid = source.SupervisorGuid;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.User = source.User;
					if (string.IsNullOrEmpty(destination.User))
					{
						destination.User = null;
					}
					destination.Zip = source.Zip ?? "";
					destination.CustType = source.CustType;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.GroupGuid = source.GroupGuid;
					destination.VisitorGuid = source.VisitorGuid;
					destination.BPercent = source.BPercent;
					destination.InPlacePayment = source.InPlacePayment;
					destination.TitleGuid = source.TitleGuid;
					destination.TafsilLevel = source.TafsilLevel;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.TaxType = source.TaxType;
					destination.ChequeBargashtiValidation = source.ChequeBargashtiValidation;
					destination.OrderTasvieTimeValidation = source.OrderTasvieTimeValidation;
					destination.RoleGuid = (((source?.RoleGuid ?? Guid.Empty) == Guid.Empty) ? null : source.RoleGuid);
					destination.PhoneNumbers = string.Join(",", source.TafsilNumbers.Select((WebTafsilNumbers s) => s.CalcedNumber.ToString()) ?? new List<string>());
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 660);
					throw;
				}
			}
		}

		public Task<CusBusiness> MapAsync(string contextGuid, Customer source, CusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public CusBusiness Map(string contextGuid, Customer source, CusBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new CusBusiness(contextGuid);
					}
					destination.ChangeStatus = EntityChangeSt.Edited;
					destination.ContextGuid = contextGuid;
					destination.FullName = source.FullName;
					destination.Address1 = source.Address1;
					destination.Address2 = source.Address2;
					destination.CodeMeli = source.CodeMeli;
					destination.Company = source.Company;
					destination.Date = source.Date;
					destination.Description = source.Description;
					destination.EghtesadiNo = source.EghtesadiNo;
					destination.Etebar = source.Etebar;
					destination.EtebarCheck = source.EtebarCheck;
					destination.Family = source.Family;
					destination.IdPath = source.IdPath;
					destination.AreaGuid = (((source?.AreaGuid ?? Guid.Empty) == Guid.Empty) ? new Guid?(Guid.Empty) : source.AreaGuid);
					destination.IsBazaryab = source.IsBazaryab;
					destination.IsSupervisor = source.IsSupervisor;
					destination.IsEtebari = source.IsEtebari;
					destination.Job = source.Job;
					destination.Location = source.Location;
					destination.Name = source.Name;
					destination.Password = source.Password;
					destination.PicFace = source.PicFace;
					destination.PicSign = source.PicSign;
					destination.Email = source.Email;
					destination.SupervisorGuid = source.SupervisorGuid;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.User = source.User;
					destination.Zip = source.Zip;
					destination.CustType = source.CustType;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.GroupGuid = source.GroupGuid;
					destination.SetVisitorGuid(source.VisitorGuid);
					destination.BPercent = source.BPercent;
					destination.InPlacePayment = source.InPlacePayment;
					destination.TitleGuid = source.TitleGuid;
					destination.TafsilLevel = source.TafsilLevel;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.TaxType = source.TaxType;
					destination.ChequeBargashtiValidation = source.ChequeBargashtiValidation;
					destination.OrderTasvieTimeValidation = source.OrderTasvieTimeValidation;
					destination.RoleGuid = source.RoleGuid;
					return destination;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 732);
					throw;
				}
			}
		}

		public Task<List<Customer>> MapListAsync(string contextGuid, IEnumerable<CusBusiness> source, List<Customer> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Customer>> MapListAsync(string contextGuid, IEnumerable<WebCus> source, List<Customer> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CusBusiness>> MapListAsync(string contextGuid, IEnumerable<Customer> source, List<CusBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Customer, CusBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CusBusiness, Customer> MapToDatabase => Mapper;

	public override IItemMapper<Customer, CusBusiness> MapFromDatabase_noneAsync => Mapper;

	internal CustomerPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<CusBusiness> GetByNameAsync(string firstName, string lastName, Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 34));
		using (new JaegerMonitor("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			CusBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Customer source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.Name == firstName && x.Family == lastName && x.Guid != guid), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 45);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<bool> CheckUserNameAsync(Guid guid, string userName, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 56));
		using (new JaegerMonitor("CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			bool result;
			try
			{
				_ = 1;
				try
				{
					result = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.User == userName && x.Guid != guid), token) == null;
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
					WebErrorLog.Instance.StartLog(ex2, "", "CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 67);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("CheckUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CusBusiness> GetByEmailAsync(string email, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByEmailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 77));
		using (new JaegerMonitor("GetByEmailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			CusBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Customer source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.Email == email), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByEmailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 88);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByEmailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CusBusiness> GetAsync(long code, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 98));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			CusBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Customer source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, Tafsil>>)((Customer p) => p.Tafsil)), (Expression<Func<Customer, bool>>)((Customer x) => x.Tafsil.Code == code), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 109);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<CusBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 119));
		using (new JaegerMonitor("GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			CusBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Customer source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.Guid == guid), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 130);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetFromDatabaseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CusBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 146));
				Customer customer = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				customer.Modified = DateTime.Now;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(customer, customer.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 149);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 153);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 154);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CusBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (CusBusiness item in items)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 168);
				if (ret.HasError)
				{
					return ret;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 174);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 175);
		}
		return ret;
	}

	public async Task<CusBusiness> GetByUserNameAsync(string userName, string pass, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetByUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 182));
			CusBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					Customer source = ((pass != null) ? (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.User == userName && x.Password == pass), token)) : (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.User == userName), token)));
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 196);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateGroupAsync(IEnumerable<Guid> customerGuids, Guid groupGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 210));
				foreach (Guid temp in customerGuids)
				{
					Customer customer = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.Guid == temp), token);
					if (customer != null)
					{
						customer.GroupGuid = groupGuid;
						((DbContext)c).Update<Customer>(customer);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 221);
				ret.AddReturnedValue(ex, "", "UpdateGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 222);
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 236);
		}
	}

	public void ResetCache(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> ChangeChequeBargashtiValidationAsync(List<Guid> lst, ReturnedState state, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(lst?.Any() ?? false))
				{
					return ret;
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 251));
				foreach (Guid item in lst)
				{
					Customer customer = await dbcontext.Customers.FindAsync(new object[1] { item });
					if (customer != null)
					{
						customer.Modified = DateTime.Now;
						customer.ServerStatus = ServerStatusForClient.None;
						customer.ChequeBargashtiValidation = state;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(customer, customer.Guid), ReturnedState.None, "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 259);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 264);
				ret.AddReturnedValue(ex, "", "ChangeChequeBargashtiValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 265);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ChangeOrderTimeValidationAsync(List<Guid> lst, ReturnedState state, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(lst?.Any() ?? false))
				{
					return ret;
				}
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 278));
				foreach (Guid item in lst)
				{
					Customer customer = await dbcontext.Customers.FindAsync(new object[1] { item });
					if (customer != null)
					{
						customer.Modified = DateTime.Now;
						customer.ServerStatus = ServerStatusForClient.None;
						customer.OrderTasvieTimeValidation = state;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(customer, customer.Guid), ReturnedState.None, "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 286);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 291);
				ret.AddReturnedValue(ex, "", "ChangeOrderTimeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 292);
			}
			return ret;
		}
	}

	public async Task<CusBusiness> GetByNationalCodeAsync(string nationalCode, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByNationalCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 298));
		using (new JaegerMonitor("GetByNationalCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			CusBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Customer source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Customer>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, bool>>)((Customer x) => x.CodeMeli == nationalCode), token);
					result = await MapFromDatabase.MapAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByNationalCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 309);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByNationalCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 325));
				Customer customer = await dbcontext.Customers.FindAsync(new object[1] { guid });
				if (customer == null)
				{
					throw new NullReferenceException($"customer with guid({guid}) not founded!");
				}
				customer.SendToServerDate = DateTime.Now;
				((DbContext)dbcontext).Update<Customer>(customer);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 333);
				ret.AddReturnedValue(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 334);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient serverStatus, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 344));
				Customer customer = await c.Customers.FindAsync(new object[1] { guid });
				if (customer == null)
				{
					return ret;
				}
				customer.ServerStatus = ServerStatusForClient.None;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(customer, guid), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 349);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 353);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 354);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(EnPriceType priceType, Guid roleGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 364));
				List<Customer> list = await EntityFrameworkQueryableExtensions.ToListAsync<Customer>(((IQueryable<Customer>)c.Customers).Where((Customer q) => q.DefPrice == priceType), token);
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (Customer item in list)
				{
					item.RoleGuid = roleGuid;
					item.ServerStatus = ServerStatusForClient.None;
					((DbContext)c).Update<Customer>(item);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 376);
				ret.AddReturnedValue(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 377);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<WebCus> items, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 387));
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (Customer item in await Mapper.MapListAsync(base.ContextGuid, items, null, tranName, token))
			{
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 392);
				if (ret.HasError)
				{
					return ret;
				}
			}
			List<TafsilNumber> tafsilNumbersDbItems = MapWebCusToTafsilNumbers(items);
			List<Guid> lst = items?.Select((WebCus q) => q.Guid)?.ToList();
			token.ThrowIfCancellationRequested();
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, lst);
			returnedSaveFuncInfoWithValue.throwExceptionIfError();
			string spName = $"Delete from Acc_TafsilNumbers Where TafsilGuid in (Select Guid From StimulQuery where RefrencedId={returnedSaveFuncInfoWithValue.value})";
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.StoredProcedure, 30, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 404);
			token.ThrowIfCancellationRequested();
			await ((DbContext)c).AddRangeAsync((IEnumerable<object>)tafsilNumbersDbItems, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 410);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 411);
		}
		return ret;
	}

	private List<TafsilNumber> MapWebCusToTafsilNumbers(List<WebCus> lst)
	{
		return lst.Select((WebCus cus) => cus.TafsilNumbers.Select((WebTafsilNumbers q) => new TafsilNumber
		{
			CalcedNumber = q.CalcedNumber,
			OriginalNumber = q.OriginalNumber,
			Prefix = q.Prefix,
			TafsilGuid = cus.Guid,
			Title = q.Title
		})).SelectMany((IEnumerable<TafsilNumber> p) => p).ToList();
	}

	public async Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(List<Guid> customerList, Guid roleGuid, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(customerList?.Any() ?? false))
			{
				return res;
			}
			BaseModelContext c = await dbContext.GetContextAsync(tranName, 60000, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 433);
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await StimulQueryBusiness.SaveAsync(base.ContextGuid, customerList);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 435);
			if (res.HasError)
			{
				return res;
			}
			string spName = ((!(roleGuid == Guid.Empty)) ? $"update Acc_Customer set RoleGuid='{roleGuid}', ServerStatus=0, Modified=GetDate() from Acc_Customer inner join StimulQuery on StimulQuery.Guid=Acc_Customer.Guid where StimulQuery.RefrencedId={returnedSaveFuncInfoWithValue.value}" : $"update Acc_Customer set RoleGuid=null, ServerStatus=0, Modified=GetDate() from Acc_Customer inner join StimulQuery on StimulQuery.Guid=Acc_Customer.Guid where StimulQuery.RefrencedId={returnedSaveFuncInfoWithValue.value}");
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await c.ExecuteNoneQueryAsync(spName, null, tranName, token, "", CommandType.Text, 30, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"), ReturnedState.None, "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 444);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 448);
			res.AddReturnedValue(ex, "", "UpdateRoleGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 449);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UpdatePhoneNumbersAsync(CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 300000, "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 459));
			List<Customer> list = await EntityFrameworkQueryableExtensions.ToListAsync<Customer>(((IQueryable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, ICollection<TafsilNumber>>((IQueryable<Customer>)EntityFrameworkQueryableExtensions.Include<Customer, Tafsil>((IQueryable<Customer>)c.Customers, (Expression<Func<Customer, Tafsil>>)((Customer q) => q.Tafsil)), (Expression<Func<Customer, ICollection<TafsilNumber>>>)((Customer q) => q.Tafsil.TafsilNumber))).Where((Customer q) => q.Tafsil.TafsilNumber != null && q.Tafsil.TafsilNumber.Any()), default(CancellationToken));
			if (list == null || !list.Any() || token.IsCancellationRequested)
			{
				return res;
			}
			foreach (Customer item in list)
			{
				item.PhoneNumbers = string.Join(",", item.Tafsil.TafsilNumber.Select((TafsilNumber p) => p.OriginalNumber));
				item.Modified = DateTime.Now;
				item.ServerStatus = ServerStatusForClient.None;
			}
			((DbContext)c).UpdateRange((IEnumerable<object>)list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 472);
			res.AddReturnedValue(ex, "", "UpdatePhoneNumbersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 473);
		}
		return res;
	}

	public async Task<List<Guid>> GetCustomerGuidListInRoleAsync(Guid roleGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetCustomerGuidListInRoleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetCustomerGuidListInRoleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 480));
			List<Guid> result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.ToListAsync<Customer>(((IQueryable<Customer>)c.Customers).Where((Customer q) => q.RoleGuid == roleGuid), token))?.Select((Customer q) => q.Guid)?.ToList();
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetCustomerGuidListInRoleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 490);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetCustomerGuidListInRoleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(Guid groupGuid, bool value, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				BaseModelContext dbcontext = await dbContext.GetContextAsync(tranName, 60000, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 505);
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)dbcontext).Database, "Update Acc_Customer Set ServerStatus = 0, modified=getdate(), Special={1} Where GroupGuid={0}", (IEnumerable<object>)new global::_003C_003Ez__ReadOnlyArray<object>(new object[2] { groupGuid, value }), token);
				await RelationalDatabaseFacadeExtensions.ExecuteSqlRawAsync(((DbContext)dbcontext).Database, "update Acc_Tafsil set IsSpecial=Acc_Customer.Special, ServerStatus=0, Modified=GETDATE() from Acc_Tafsil inner join Acc_Customer on Acc_Customer.Guid=Acc_Tafsil.Guid where Acc_Tafsil.IsSpecial<>Acc_Customer.Special", token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 511);
				ret.AddReturnedValue(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerPersistenceRepository.cs", 512);
			}
			return ret;
		}
	}
}
