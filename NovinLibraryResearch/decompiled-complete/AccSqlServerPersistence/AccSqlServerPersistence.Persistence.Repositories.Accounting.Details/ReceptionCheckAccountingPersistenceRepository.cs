using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting.Cheque;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting.Details;

internal class ReceptionCheckAccountingPersistenceRepository : ReceptionCheckMasterPersistenceRepository
{
	private bool _isDisposed;

	public ReceptionCheckAccountingPersistenceRepository(ContextHolder db)
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
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 30);
		}
	}

	public override async Task<ReceptionCheckBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 34));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs"))
		{
			ReceptionCheckBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					ReceptionCheck obj = (await EntityFrameworkQueryableExtensions.ToListAsync<ReceptionCheck>(RelationalQueryableExtensions.FromSqlRaw<ReceptionCheck>(c.ReceptionCheck, $"select * from Acc_ReceptionCheck with (updlock, holdlock) where Guid='{guid}'", Array.Empty<object>()), token)).SingleOrDefault();
					if (!string.IsNullOrEmpty(tranName))
					{
						await c.ExecuteNoneQueryAsync($"update Acc_ReceptionCheck set modified=getdate() where Guid='{guid}'", null, tranName, token, "", CommandType.Text, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs");
					}
					ReceptionCheckBusiness receptionCheckBusiness = ((obj == null) ? null : (await MapFromDatabase.MapAsync(base.ContextGuid, obj, null, tranName, token)));
					result = receptionCheckBusiness;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user"))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 49);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(CancellationToken token, string tranName, Guid? tafsilGuid)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 60));
		IEnumerable<ReceptionCheckReportBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 19;
			try
			{
				IEnumerable<ReceptionCheckReportBusiness> enumerable = await GetAllMojodDuringTheCourceAsync(c, tafsilGuid);
				IEnumerable<ReceptionCheckReportBusiness> enumerable2;
				if (enumerable != null)
				{
					IEnumerable<ReceptionCheckReportBusiness> first = enumerable.Union(await GetAllBargashtiDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first2 = first.Union(await GetAllKharjShodeDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first3 = first2.Union(await GetAllVagozarShodeDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first4 = first3.Union(await GetAllNaghdShodeDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first5 = first4.Union(await GetAllBatelShodeDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first6 = first5.Union(await GetAllVosolShodeDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first7 = first6.Union(await GetAllMojodBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first8 = first7.Union(await GetAllBargashtiBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first9 = first8.Union(await GetAllKharjShodeBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first10 = first9.Union(await GetAllVagozarShodeBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first11 = first10.Union(await GetAllNaghdShodeBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first12 = first11.Union(await GetAllBatelShodeBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first13 = first12.Union(await GetAllVosolShodeBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first14 = first13.Union(await GetAllEntezamiMojodDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first15 = first14.Union(await GetAllEntezamiMojodBeginningTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first16 = first15.Union(await GetAllEntezamiOdatDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first17 = first16.Union(await GetAllOdatShodeDuringTheCourceAsync(c, tafsilGuid));
					IEnumerable<ReceptionCheckReportBusiness> first18 = first17.Union(await GetAllOdatShodeBeginingTheCourceAsync(c, tafsilGuid));
					enumerable2 = first18.Union(await GetAllEntezamiOdatBeginningTheCourceAsync(c, tafsilGuid));
				}
				else
				{
					enumerable2 = null;
				}
				IEnumerable<ReceptionCheckReportBusiness> enumerable3 = enumerable2;
				result = ((!token.IsCancellationRequested && !base.IsDisposed) ? enumerable3?.ToList() : null);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 90);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 101));
		IEnumerable<BedehiTalabModel> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<ReceptionCheck> queryable = (IQueryable<ReceptionCheck>)c.ReceptionCheck;
				if (filter != null)
				{
					if (filter.DateFrom.HasValue)
					{
						queryable = queryable?.Where((ReceptionCheck q) => q.Date >= filter.DateFrom);
					}
					if (filter.DateTo.HasValue)
					{
						queryable = queryable?.Where((ReceptionCheck q) => q.Date <= filter.DateTo);
					}
					if (filter.Count > 0)
					{
						queryable = queryable?.Take(filter.Count);
					}
				}
				result = ((queryable != null && queryable.Any()) ? (from q in await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token)
					group q by q.DateSh)?.Select((IGrouping<string, ReceptionCheckBusiness> q) => new BedehiTalabModel
				{
					Count = q.Count(),
					DateSh = q.Key,
					Sum = q.Sum((ReceptionCheckBusiness p) => p.Price)
				}) : null);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2.Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 124);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs");
			}
		}
		return result;
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllMojodDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 1 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionItem.FullName,
				Place = q.DebitTafsil.Name,
				PlaceGuid = q.DebitTafsilGuid,
				DateToPlace = null,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllMojodAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllMojodDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 188);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllMojodBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 1 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = null,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = q.DebitTafsil.Name,
				PlaceGuid = q.DebitTafsilGuid,
				DateToPlace = null,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllMojodAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllMojodBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 247);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllBargashtiDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 2 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation p) => (int)p.NewState == 2).DateM,
				TafsilName = q.ReceptionItem.FullName,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllBargashtiAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllBargashtiDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 305);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllBargashtiBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 2 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation p) => (int)p.NewState == 2).DateM,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllBargashtiAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllBargashtiBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 363);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllKharjShodeDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 3 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)((IQueryable<ReceptionCheck>)EntityFrameworkQueryableExtensions.Include<ReceptionCheck, ICollection<PardakhtCheckM>>(queryable, (Expression<Func<ReceptionCheck, ICollection<PardakhtCheckM>>>)((ReceptionCheck q) => q.PardakhtCheckMItem))).Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionItem.FullName,
				Place = (from q in q.ChequeMOperation
					where (int)q.NewState == 3
					orderby q.Ordinal descending
					select q).FirstOrDefault().DebitTafsil.Name,
				PlaceGuid = (from q in q.ChequeMOperation
					where (int)q.NewState == 3
					orderby q.Ordinal descending
					select q).FirstOrDefault().DebitTafsilGuid,
				DateToPlace = (from q in q.ChequeMOperation
					where (int)q.NewState == 3
					orderby q.Ordinal descending
					select q).FirstOrDefault().DateM,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllKharjShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllKharjShodeDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 423);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllKharjShodeBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 3 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = null,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = (from p in q.ChequeMOperation
					where (int)p.NewState == 3
					select p into q
					orderby q.Ordinal descending
					select q).FirstOrDefault().DebitTafsil.Name,
				PlaceGuid = (from p in q.ChequeMOperation
					where (int)p.NewState == 3
					select p into q
					orderby q.Ordinal descending
					select q).FirstOrDefault().DebitTafsil.Guid,
				DateToPlace = (from p in q.ChequeMOperation
					where (int)p.NewState == 3
					select p into q
					orderby q.Ordinal descending
					select q).FirstOrDefault().DateM,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllKharjShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllKharjShodeBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 482);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllVagozarShodeDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 4 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionItem.FullName,
				Place = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 4).DebitTafsil.Name,
				PlaceGuid = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 4).DebitTafsil.Guid,
				DateToPlace = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 4).DateM,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVagozarShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllVagozarShodeDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 541);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllVagozarShodeBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 4 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = null,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 4).DebitTafsil.Name,
				PlaceGuid = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 4).DebitTafsil.Guid,
				DateToPlace = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 4).DateM,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVagozarShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllVagozarShodeBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 600);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllNaghdShodeDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 5 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionItem.FullName,
				Place = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 5).DebitTafsil.Name,
				PlaceGuid = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 5).DebitTafsil.Guid,
				DateToPlace = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 5).DateM,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllNaghdShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllNaghdShodeDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 659);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllNaghdShodeBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 5 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = null,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 5).DebitTafsil.Name,
				PlaceGuid = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 5).DebitTafsil.Guid,
				DateToPlace = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 5).DateM,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllNaghdShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllNaghdShodeBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 718);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllBatelShodeDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 6 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionItem.FullName,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllBatelShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllBatelShodeDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 776);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllBatelShodeBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 6 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = null,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllBatelShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllBatelShodeBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 834);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllVosolShodeDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 7 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = q.ChequeMOperation.OrderByDescending((ChequeMOperation q) => q.Ordinal).FirstOrDefault((ChequeMOperation q) => (int)q.NewState == 7).DateM,
				TafsilName = q.ReceptionItem.FullName,
				Place = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 7).DebitTafsil.Name,
				PlaceGuid = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 7).DebitTafsil.Guid,
				DateToPlace = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 7).DateM,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVosolShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllVosolShodeDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 893);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllVosolShodeBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 7 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = q.ChequeMOperation.OrderByDescending((ChequeMOperation q) => q.Ordinal).FirstOrDefault((ChequeMOperation q) => (int)q.NewState == 7).DateM,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 7).DebitTafsil.Name,
				PlaceGuid = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 7).DebitTafsil.Guid,
				DateToPlace = q.ChequeMOperation.OrderByDescending((ChequeMOperation p) => p.Ordinal).FirstOrDefault((ChequeMOperation a) => (int)a.NewState == 7).DateM,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVosolShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllVosolShodeBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 952);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllEntezamiMojodDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 8 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = null,
				A2re_PardazandeTafsilGuid = null,
				A2re_ReceptionDate = null,
				Aval2re = false,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = null,
				CreditA2reTafsilGuid = null,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = null,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.ReceptionChequeEntezami.CashierGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionChequeEntezami.ReceptionDateM,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionChequeEntezami.DebitTafsilItem.Name,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.ReceptionChequeEntezami.DebitTafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVosolShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllEntezamiMojodDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 1010);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllEntezamiMojodBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 8 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.ReceptionChequeEntezami.CreditMoeinGuid,
				A2re_PardazandeTafsilGuid = q.ReceptionChequeEntezami.CreditTafsilGuid,
				A2re_ReceptionDate = q.ReceptionChequeEntezami.ReceptionDateM,
				Aval2re = true,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.ReceptionChequeEntezami.DebitMoeinGuid,
				CreditA2reTafsilGuid = q.ReceptionChequeEntezami.DebitTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = null,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.ReceptionChequeEntezami.CashierGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionChequeEntezami.ReceptionDateM,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionChequeEntezami.DebitTafsilItem.Name,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.ReceptionChequeEntezami.DebitTafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVosolShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllEntezamiMojodBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 1068);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllEntezamiOdatDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 9 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = null,
				A2re_PardazandeTafsilGuid = null,
				A2re_ReceptionDate = null,
				Aval2re = false,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = null,
				CreditA2reTafsilGuid = null,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = null,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.ReceptionChequeEntezami.CashierGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionChequeEntezami.ReceptionDateM,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionChequeEntezami.DebitTafsilItem.Name,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.ReceptionChequeEntezami.DebitTafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVosolShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllEntezamiOdatDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 1126);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllEntezamiOdatBeginningTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 9 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.ReceptionChequeEntezami.CreditMoeinGuid,
				A2re_PardazandeTafsilGuid = q.ReceptionChequeEntezami.CreditTafsilGuid,
				A2re_ReceptionDate = q.ReceptionChequeEntezami.ReceptionDateM,
				Aval2re = true,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.ReceptionChequeEntezami.DebitMoeinGuid,
				CreditA2reTafsilGuid = q.ReceptionChequeEntezami.DebitTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = null,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.ReceptionChequeEntezami.CashierGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionChequeEntezami.ReceptionDateM,
				DateBargashtOrVosol = null,
				TafsilName = q.ReceptionChequeEntezami.DebitTafsilItem.Name,
				Place = "",
				DateToPlace = null,
				TafsilGuid = q.ReceptionChequeEntezami.DebitTafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllVosolShodeAsync", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllEntezamiOdatBeginningTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 1184);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllOdatShodeDuringTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 10 && !q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => q.ReceptionItem.TafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)((IQueryable<ReceptionCheck>)EntityFrameworkQueryableExtensions.Include<ReceptionCheck, ICollection<PardakhtCheckM>>(queryable, (Expression<Func<ReceptionCheck, ICollection<PardakhtCheckM>>>)((ReceptionCheck q) => q.PardakhtCheckMItem))).Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.ReceptionItem.Date,
				DateBargashtOrVosol = (from q in q.ChequeMOperation
					where (int)q.NewState == 10
					orderby q.Ordinal descending
					select q).FirstOrDefault().DateM,
				TafsilName = q.ReceptionItem.FullName,
				DateToPlace = (from q in q.ChequeMOperation
					where (int)q.NewState == 10
					orderby q.Ordinal descending
					select q).FirstOrDefault().DateM,
				TafsilGuid = q.ReceptionItem.TafsilGuid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllOdatShode", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllOdatShodeDuringTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 1242);
			throw;
		}
	}

	private Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllOdatShodeBeginingTheCourceAsync(Acc_ModelContext c, Guid? tafsilGuid)
	{
		try
		{
			IQueryable<ReceptionCheck> queryable = ((IQueryable<ReceptionCheck>)c.ReceptionCheck)?.Where((ReceptionCheck q) => (int)q.Status == 10 && q.Aval2re);
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				queryable = queryable?.Where((ReceptionCheck q) => (q.A2re_PardazandeTafsilGuid ?? Guid.Empty) != Guid.Empty && q.A2re_PardazandeTafsilGuid == ((Guid?)tafsilGuid).Value);
			}
			if (queryable == null || !queryable.Any())
			{
				return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)new List<ReceptionCheckReportBusiness>());
			}
			return Task.FromResult((IEnumerable<ReceptionCheckReportBusiness>)queryable.Select((ReceptionCheck q) => new ReceptionCheckReportBusiness
			{
				Guid = q.Guid,
				A2re_PardazandeMoeinGuid = q.A2re_PardazandeMoeinGuid,
				A2re_PardazandeTafsilGuid = q.A2re_PardazandeTafsilGuid,
				A2re_ReceptionDate = q.A2re_ReceptionDate,
				Aval2re = q.Aval2re,
				BankName = q.BankName,
				ContextGuid = ContextGuid,
				CreditA2reMoeinGuid = q.CreditA2reMoeinGuid,
				CreditA2reTafsilGuid = q.CreditA2reTafsilGuid,
				Date = q.Date,
				DebitMoeinGuid = q.DebitMoeinGuid,
				DebitTafsilGuid = q.DebitTafsilGuid,
				Description = q.Description,
				HesabNum = q.HesabNum,
				IsEnteghali = q.IsEnteghali,
				MasterGuid = q.MasterGuid,
				Modified = q.Modified,
				Number = q.Number,
				Personal = q.Personal,
				PoshtNomreh = q.PoshtNomreh,
				Price = q.Price,
				Status = q.Status,
				UserGuid = q.UserGuid,
				VisitorGuid = q.VisitorGuid,
				ReceptionDate = q.A2re_ReceptionDate,
				DateBargashtOrVosol = (from q in q.ChequeMOperation
					where (int)q.NewState == 10
					orderby q.Ordinal descending
					select q).FirstOrDefault().DateM,
				TafsilName = q.A2re_TafsilItem.Name,
				Place = q.DebitTafsil.Name,
				PlaceGuid = q.DebitTafsilGuid,
				DateToPlace = (from q in q.ChequeMOperation
					where (int)q.NewState == 10
					orderby q.Ordinal descending
					select q).FirstOrDefault().DateM,
				TafsilGuid = q.A2re_TafsilItem.Guid,
				SanadNo = q.SanadNo,
				MetaData = q.MetaData.FromJson<ReceptionChequeMetaData>("ReceptionCheckAccountingPersistenceRepository", "GetAllOdatShode", 0),
				SayadiRegistered = q.SayadiRegistered,
				SayadiNationalCode = q.SayadiNationalCode,
				SayadiIdentity = q.SayadiIdentity,
				SayadiName = q.SayadiName
			}));
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
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllOdatShodeBeginingTheCourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Khazane\\Reception\\ReceptionCheckAccountingPersistenceRepository.cs", 1301);
			throw;
		}
	}
}
