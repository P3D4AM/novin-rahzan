using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane.Pardakht;

namespace EntityCache.Business.Accounting.Khazane.Pardakht;

public class PardakhtChequeEntezamiBusiness : IPardakhtChequeEntezami, IHasModified, IHasContext, IHasChangeStatus
{
	private PardakhtCheckShBusiness _cheque;

	public Guid ChequeGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid CashierGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public int SanadNo { get; set; }

	public string Description { get; set; }

	public DateTime PardakhtDateM { get; set; } = DateTime.Now;


	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public PardakhtChequeEntezamiBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public async Task<PardakhtCheckShBusiness> GetPardakhtChequeAsync(ENCheckSHStatus defStatus, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_cheque == null)
			{
				_cheque = await PardakhtCheckShBusiness.GetAsync(ContextGuid, ChequeGuid, token, tranName);
			}
			if (_cheque == null)
			{
				_cheque = new PardakhtCheckShBusiness(ContextGuid)
				{
					Status = defStatus
				};
			}
			return _cheque;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtChequeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 47);
			throw;
		}
	}

	public void SetPardakht(PardakhtCheckShBusiness value)
	{
		_cheque = value;
	}

	public static Task<PardakhtChequeEntezamiBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.PardakhtChequeEntezami.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 6;
			try
			{
				if (_cheque == null)
				{
					ret.AddError("چک معتبر نمی باشد", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 62);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 63), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 63);
				}
				transactionHasBeenCreated = true;
				if (!ret.HasError && !_cheque.Aval2re && system == EnSystem.UserInterface)
				{
					if (SanadNo <= 0)
					{
						SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
					}
					SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
					List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, ChequeGuid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 70);
				}
				if (ret.HasError)
				{
					result = ret;
					goto IL_0731;
				}
				_cheque.SanadNo = SanadNo;
				_cheque.UserGuid = CashierGuid;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakhtCheckSh.SaveAsync(new List<PardakhtCheckShBusiness> { _cheque }, _cheque.MasterGuid ?? Guid.Empty, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 75);
				if (ret.HasError)
				{
					result = ret;
					goto IL_0731;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.PardakhtChequeEntezami.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 77);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 81);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 82);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 87), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 87);
			}
		}
		return ret;
		IL_0731:
		return result;
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, DebitTafsilGuid.Value, tranName, token, "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs");
			BankBusiness bankBusiness = await BankBusiness.GetAsync(ContextGuid, _cheque.BankTafsilGuid, system, token, tranName);
			SanadBusiness Sanad = new SanadBusiness(ContextGuid, 0)
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = ContextGuid,
				DateM = DateTime.Now,
				Description = "چک پرداختی انتظامی (" + bankBusiness?.Name + " - " + _cheque.Number + ") ** طرف حساب " + tafsil.Name,
				Guid = Guid.NewGuid(),
				IsTemp = true,
				Modified = DateTime.Now,
				SanadNo = SanadNo,
				SanadSt = SanadSt.Movaghat,
				SanadType = SanadType.Automatic,
				Source = source,
				UserGuid = CashierGuid,
				Status = true
			};
			Sanad_DetBusiness debit = new Sanad_DetBusiness(ContextGuid, system, ChequeGuid, Document.CheckSanad, Sanad.NextOrdinal())
			{
				Credit = 0m,
				Debit = _cheque.Price,
				DateSarresid = _cheque.DateSaresid,
				RowDesc = Sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = Sanad.Guid,
				Modified = DateTime.Now,
				Serial = _cheque.Number,
				Status = true
			};
			await debit.SetMoeinGuidAsync(DebitMoeinGuid);
			await debit.SetTafsilGuidAsync(DebitTafsilGuid, "", token);
			await Sanad.AddToListSanadAsync(debit, system, token);
			Sanad_DetBusiness credit = new Sanad_DetBusiness(ContextGuid, system, ChequeGuid, Document.CheckSanad, Sanad.NextOrdinal())
			{
				Credit = _cheque.Price,
				Debit = 0m,
				DateSarresid = _cheque.DateSaresid,
				RowDesc = Sanad.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = Sanad.Guid,
				Modified = DateTime.Now,
				Serial = _cheque.Number,
				Status = true
			};
			await credit.SetMoeinGuidAsync(CreditMoeinGuid);
			await credit.SetTafsilGuidAsync(CreditTafsilGuid, "", token);
			await Sanad.AddToListSanadAsync(credit, system, token);
			return Sanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 151);
			throw;
		}
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (system == EnSystem.Transfer)
			{
				return null;
			}
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, DebitTafsilGuid.Value, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs");
			BankBusiness bankBusiness = await BankBusiness.GetAsync(ContextGuid, _cheque.BankTafsilGuid, system, token, tranName);
			List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
			list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
			{
				DebitMoeinGuid = DebitMoeinGuid,
				DebitTafsilGuid = DebitTafsilGuid,
				Date = PardakhtDateM,
				AsGuid = ChequeGuid,
				AsType = DocumentDet.ChequeSanad,
				AsTypeDocument = Document.CheckSanad,
				Debit = 0m,
				Credit = _cheque.Price,
				CashierGuid = CashierGuid,
				SanadNo = SanadNo,
				SanadSt = SanadSt.Movaghat,
				Id2 = 0,
				CreditMoeinGuid = CreditMoeinGuid,
				CreditTafsilGuid = CreditTafsilGuid,
				SanadDesc = "چک پرداختی انتظامی (" + bankBusiness?.Name + " - " + _cheque.Number + ") ** طرف حساب " + tafsil.Name,
				RDesc = Description,
				DateMoaser = PardakhtDateM,
				DetailGuid = ChequeGuid,
				Guid = Guid.NewGuid(),
				DecimalGuid = null,
				DecimalPrice = 0m,
				DecimalQt = 0m,
				DepotGuid = null,
				Modified = DateTime.Now,
				Price = 0m,
				ProductGuid = null,
				Qt = 0m,
				Qt2 = 0m,
				RBazaryab = 0m,
				RDiscount = 0m,
				Serial = _cheque.Number,
				Status = true,
				VahedGuid = null
			});
			list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
			{
				DebitMoeinGuid = DebitMoeinGuid,
				DebitTafsilGuid = DebitTafsilGuid,
				Date = PardakhtDateM,
				AsGuid = ChequeGuid,
				AsType = DocumentDet.ChequeSanad,
				AsTypeDocument = Document.CheckSanad,
				Debit = _cheque.Price,
				Credit = 0m,
				CashierGuid = CashierGuid,
				SanadNo = SanadNo,
				SanadSt = SanadSt.Movaghat,
				Id2 = 0,
				CreditMoeinGuid = CreditMoeinGuid,
				CreditTafsilGuid = CreditTafsilGuid,
				SanadDesc = "چک پرداختی انتظامی (" + bankBusiness?.Name + " - " + _cheque.Number + ") ** طرف حساب " + tafsil.Name,
				RDesc = Description,
				DateMoaser = PardakhtDateM,
				DetailGuid = ChequeGuid,
				Guid = Guid.NewGuid(),
				DecimalGuid = null,
				DecimalPrice = 0m,
				DecimalQt = 0m,
				DepotGuid = null,
				Modified = DateTime.Now,
				Price = 0m,
				ProductGuid = null,
				Qt = 0m,
				Qt2 = 0m,
				RBazaryab = 0m,
				RDiscount = 0m,
				Serial = _cheque.Number,
				Status = true,
				VahedGuid = null
			});
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiBusiness.cs", 241);
			throw;
		}
	}
}
