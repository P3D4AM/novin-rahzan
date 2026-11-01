using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.FLRSanad;

[Serializable]
[DoNotPruneType]
public class AttSanad : IHasContext
{
	private List<ATT_SanadDet> _lstSanadDets;

	private string _userName;

	private CodeingAccounting _codeacc;

	public string ContextGuid { get; set; }

	public int Id { get; private set; }

	public int No { get; set; }

	public DateTime DateM { get; set; } = DateTime.Now;


	public short Uid { get; set; }

	public string Description { get; set; } = "";


	public decimal Sum { get; private set; }

	public async Task<List<ATT_SanadDet>> GetLstSanadDetsAsync(CancellationToken token)
	{
		try
		{
			List<ATT_SanadDet> list = _lstSanadDets;
			if (list == null)
			{
				list = (_lstSanadDets = await ATT_SanadDet.GetAllAsync(ContextGuid, Id, token));
			}
			return list;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<string> GetUserNameAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			string text = _userName;
			if (text == null)
			{
				text = (_userName = (await UserAttendance.GetAsync(ContextGuid, Uid, token)).Name);
			}
			return text;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<decimal> GetSumDebitAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await GetLstSanadDetsAsync(token)).Sum((ATT_SanadDet q) => q.Debit);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<decimal> GetSumCreditAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			return (await GetLstSanadDetsAsync(token)).Sum((ATT_SanadDet q) => q.Credit);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<AttSanad> GetAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			AttSanad attSanad = new AttSanad
			{
				ContextGuid = contextGuid
			};
			AttSanad attSanad2 = attSanad;
			attSanad2._codeacc = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).CodeingAccounting;
			AttSanad att = attSanad;
			attSanad = att;
			attSanad.No = await att.GetLastSanadNoAsync(token);
			return att;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 109);
			throw;
		}
	}

	public static async Task<AttSanad> CreateAsync(string contextGuid, List<Fish> fishs, CancellationToken token)
	{
		_ = 1;
		try
		{
			AttSanad att = await GetAsync(contextGuid, token);
			await att.InitializeAsync(fishs, token);
			return att;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 124);
			throw;
		}
	}

	private async Task InitializeAsync(List<Fish> fishs, CancellationToken token)
	{
		_ = 1;
		try
		{
			string desc4 = string.Join(",", fishs.Select((Fish q) => q.FishNo));
			await SetSanadAsync(fishs, token);
			List<ATT_SanadDet> source = await GetLstSanadDetsAsync(token);
			decimal num = source.Sum((ATT_SanadDet q) => q.Debit.RoundPrice());
			decimal num2 = source.Sum((ATT_SanadDet q) => q.Credit.RoundPrice());
			if (Math.Abs(num2 - num) <= 2m)
			{
				ATT_SanadDet aTT_SanadDet = source.FirstOrDefault((ATT_SanadDet q) => q.CodeMoein == _codeacc.MoeinTaminGov && q.CodeTafsil == _codeacc.TafsilTaminGov);
				if (aTT_SanadDet != null)
				{
					if (aTT_SanadDet.Credit == 0m)
					{
						aTT_SanadDet.Debit -= num - num2;
					}
					else
					{
						aTT_SanadDet.Credit += num - num2;
					}
				}
			}
			desc4 = ((fishs.Count != 1) ? ("[فیش های حقوقی](" + desc4 + ")") : ("[FishHoghoghi](" + desc4 + ")"));
			desc4 = desc4 + LanguageAndCalendar.Instance.CaltureCalender.GetMonthName(fishs[0].DateStartM) + " [ماه سال] " + Calendar.MiladiToCalture(fishs[0].DateStartM).Substring(0, 4);
			desc4 = Utilities.GetTranslate(desc4);
			SetDescription(desc4);
			Description = Utilities.GetTranslate("[سند] ") + desc4;
			Uid = UserAttendance.SelectedUser.ID;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private async Task SetSanadAsync(List<Fish> fishs, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<Task> lsTasks = new List<Task>();
			foreach (Fish fish in fishs)
			{
				lsTasks.Add(SetSanadFromFishAsync(fish, token));
			}
			foreach (Fish fish2 in fishs)
			{
				string description = "[حقوق] " + ((PersonBase)fish2.ClsPerson).FullName + " [فیش شماره]: " + fish2.FishNo + LanguageAndCalendar.Instance.CaltureCalender.GetMonthName(fishs[0].DateStartM) + " [ماه سال] " + Calendar.MiladiToCalture(fishs[0].DateStartM).Substring(0, 4);
				lsTasks.Add(AddDetAsync(((PersonBase)fish2.ClsPerson).CodeMoein, ((PersonBase)fish2.ClsPerson).CodeTafsil, fish2.SumKhales, 0m, description, ((PersonBase)fish2.ClsPerson).FullName, token));
			}
			foreach (Fish fish3 in fishs)
			{
				lsTasks.Add(AddDetAsync(_codeacc.MoeinEmployer.GetValueOrDefault(), _codeacc.TafsilEmployer.GetValueOrDefault(), 0m, fish3.GetValueEmployerinsurrance(), "", "[حق بیمه سهم کارفرما] ", token));
				lsTasks.Add(AddDetAsync(_codeacc.MoeinUnEmployMent.GetValueOrDefault(), _codeacc.TafsilUnEmployMent.GetValueOrDefault(), 0m, fish3.GetValueInsuranceUnemployment(), "", "[حق ییمه بیکاری] ", token));
				lsTasks.Add(AddDetAsync(_codeacc.MoeinTaminGov.GetValueOrDefault(), _codeacc.TafsilTaminGov.GetValueOrDefault(), fish3.GetValueTamin(), 0m, "", "[سازمان تامین اجتماعی] ", token));
				lsTasks.Add(AddDetAsync(_codeacc.MoeinTaxGov.GetValueOrDefault(), _codeacc.TafsilTaxGov.GetValueOrDefault(), fish3.GetValueTax(), 0m, "", "[مالیات حقوق] ", token));
			}
			await Task.WhenAll(lsTasks);
			await SetDistinctAsync(token);
			decimal sumDebit = await GetSumDebitAsync(token);
			decimal num = await GetSumCreditAsync(token);
			if (sumDebit > num)
			{
				lsTasks.Add(AddDetAsync(_codeacc.MoeinRoundFish.GetValueOrDefault(), _codeacc.TafsilRoundFish.GetValueOrDefault(), sumDebit - num, 0m, "", "[کسر ریال]", token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 220);
		}
	}

	private async Task SetSanadFromFishAsync(Fish pFish, CancellationToken token)
	{
		_ = 3;
		try
		{
			List<Task> lsTasks = new List<Task>
			{
				AddDetAsync(_codeacc.MoeinBaseSalary.GetValueOrDefault(), _codeacc.TafsilBaseSalary.GetValueOrDefault(), 0m, pFish.SumKarkerd, "", "[حقوق پایه] ", token),
				AddDetAsync(_codeacc.MoeinMission.GetValueOrDefault(), _codeacc.TafsilMission.GetValueOrDefault(), 0m, pFish.SumMission, "", "[حق ماموریت] ", token),
				AddDetAsync(_codeacc.MoeinOverTime.GetValueOrDefault(), _codeacc.TafsilOverTime.GetValueOrDefault(), 0m, pFish.SumEzafeKar, "", " [اضافه کاری]  ", token),
				AddDetAsync(_codeacc.MoeinNightTime.GetValueOrDefault(), _codeacc.TafsilNightTime.GetValueOrDefault(), 0m, pFish.SumShabKari, "", " [شب کاری]  ", token),
				AddDetAsync(_codeacc.MoeinShiftTime.GetValueOrDefault(), _codeacc.TafsilShiftTime.GetValueOrDefault(), 0m, pFish.SumNobatKari1, "", " [نوبت کاری صبح]  ", token),
				AddDetAsync(_codeacc.MoeinShiftTime.GetValueOrDefault(), _codeacc.TafsilShiftTime.GetValueOrDefault(), 0m, pFish.SumNobatKari2, "", " [نوبت کاری عصر]  ", token),
				AddDetAsync(_codeacc.MoeinShiftTime.GetValueOrDefault(), _codeacc.TafsilShiftTime.GetValueOrDefault(), 0m, pFish.SumNobatKari3, "", " [نوبت کاری شب]  ", token),
				AddDetAsync(_codeacc.MoeinHolidayTime.GetValueOrDefault(), _codeacc.TafsilHolidayTime.GetValueOrDefault(), 0m, pFish.SumJomeKari, "", " [جمعه کاری]  ", token),
				AddDetAsync(_codeacc.MoeinTatilKari.GetValueOrDefault(), _codeacc.TafsilTatilKari.GetValueOrDefault(), 0m, pFish.SumTatilKari, "", " [تعطیل کاری]  ", token)
			};
			if (pFish.VacationST == 1)
			{
				lsTasks.Add(AddDetAsync(_codeacc.MoeinVacation.GetValueOrDefault(), _codeacc.TafsilVacation.GetValueOrDefault(), 0m, (pFish.SumVacation - pFish.GetSumVacationUsed()).RoundPrice(), "", " [مرخصی]  ", token));
			}
			lsTasks.Add(AddDetAsync(_codeacc.MoeinLeaveTime.GetValueOrDefault(), _codeacc.TafsilLeaveTime.GetValueOrDefault(), pFish.SumKasrKar, 0m, "", " [کسرکار]  ", token));
			lsTasks.Add(AddDetAsync(_codeacc.MoeinJarime.GetValueOrDefault(), _codeacc.TafsilJarime.GetValueOrDefault(), pFish.SumJarime, 0m, "", " [جریمه تاخیر] ", token));
			lsTasks.Add(AddDetAsync(_codeacc.MoeinAbsentTime.GetValueOrDefault(), _codeacc.TafsilAbsentTime.GetValueOrDefault(), pFish.SumGheybat, 0m, "", " [غیبت]  ", token));
			IEnumerable<FishDetJson> enumerable = pFish.ListFishDet.Where((FishDetJson q) => q.HesabId != 5 && q.HesabId != 6);
			foreach (FishDetJson item in enumerable)
			{
				decimal tempdebit = item.Debit;
				decimal tempcredit = item.Credit;
				if (item.Debit > 0m)
				{
					tempdebit = item.TmpValue;
				}
				else
				{
					tempcredit = item.TmpValue;
				}
				if ((await item.GetFishHesabAsync(token)).PrsnTafsilUsed)
				{
					List<Task> list = lsTasks;
					list.Add(AddDetAsync(await item.GetCodeMoeinAsync(token), ((PersonBase)pFish.ClsPerson).CodeTafsil, tempdebit, tempcredit, "", item.Name, mergeWithOther: false, token));
				}
				else
				{
					List<Task> list = lsTasks;
					list.Add(AddDetAsync(await item.GetCodeMoeinAsync(token), await item.GetCodeTafsilAsync(token), tempdebit, tempcredit, "", item.Name, token));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetSanadFromFishAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 290);
		}
	}

	private async Task SetDistinctAsync(CancellationToken token)
	{
		try
		{
			List<ATT_SanadDet> list = await GetLstSanadDetsAsync(token);
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = i + 1; j < list.Count; j++)
				{
					if (list[i].CodeMoein != list[j].CodeMoein || list[i].CodeTafsil != list[j].CodeTafsil || !list[i].MergeWithOther)
					{
						continue;
					}
					list[i].Debit += list[j].Debit;
					list[i].Credit += list[j].Credit;
					if (list[i].Debit != 0m && list[i].Credit != 0m)
					{
						if (list[i].Debit > list[i].Credit)
						{
							list[i].Debit -= list[i].Credit;
							list[i].Credit = 0m;
						}
						else
						{
							list[i].Credit -= list[i].Debit;
							list[i].Debit = 0m;
						}
					}
					ATT_SanadDet aTT_SanadDet = list[j];
					decimal debit = (list[j].Credit = default(decimal));
					aTT_SanadDet.Debit = debit;
				}
			}
			_lstSanadDets = list.Where((ATT_SanadDet q) => q.Debit != 0m || q.Credit != 0m).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetDistinctAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 328);
		}
	}

	public async Task AddDetAsync(long codeMoein, long codeTafsil, decimal credit, decimal debit, string description, string titleTafsil, bool mergeWithOther = true, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(credit == 0m) || !(debit == 0m))
			{
				ATT_SanadDet item = new ATT_SanadDet(Id, debit, credit, codeMoein, codeTafsil, Utilities.GetTranslate(description), Utilities.GetTranslate(titleTafsil), mergeWithOther);
				if (!token.IsCancellationRequested)
				{
					(await GetLstSanadDetsAsync(token)).Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 344);
		}
	}

	public async Task<int> GetLastSanadNoAsync(CancellationToken token)
	{
		int sno = 0;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("SELECT isnull(max([NO]),0) +1 as MaxSanadno FROM [dbo].[ATT_Sanad]", cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["MaxSanadno"].ToString() != DBNull.Value.ToString())
						{
							sno = (int)((DbDataReader)(object)dr)["MaxSanadno"];
						}
						((DbConnection)(object)cn).Close();
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
			return sno;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetLastSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 366);
			throw;
		}
	}

	public async Task AttSanadAsync(string contextGuid, List<int> fishIds, CancellationToken token)
	{
		ContextGuid = contextGuid;
		_codeacc = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(ContextGuid, token)).CodeingAccounting;
		try
		{
			string desc2 = "";
			foreach (int fishId in fishIds)
			{
				Fish fish = await Fish.GetAsync(ContextGuid, fishId, loadClsTaradod: false, token);
				desc2 = ((!(desc2 == "")) ? (desc2 + "," + fish.FishNo) : fish.FishNo.ToString());
				Person clsPerson = fish.ClsPerson;
				string fullName = ((PersonBase)fish.ClsPerson).FullName;
				if (fish.SumKhales < 0m)
				{
					await AddDetAsync(((PersonBase)clsPerson).CodeMoein, ((PersonBase)fish.ClsPerson).CodeTafsil, 0m, -fish.SumKhales, "[FishHoghoghi](" + fish.FishNo + ") [تاریخ] " + Calendar.MiladiToCalture(fish.DateStartM) + "-" + Calendar.MiladiToCalture(fish.DateEndM) + " " + fullName, fullName, token);
				}
				else
				{
					await AddDetAsync(((PersonBase)clsPerson).CodeMoein, ((PersonBase)fish.ClsPerson).CodeTafsil, fish.SumKhales, 0m, "[FishHoghoghi](" + fish.FishNo + ") [تاریخ] " + Calendar.MiladiToCalture(fish.DateStartM) + "-" + Calendar.MiladiToCalture(fish.DateEndM) + " " + fullName, fullName, token);
				}
			}
			await AddDetAsync(6101100L, 204014L, 0m, await GetSumCreditAsync(token), "[حقوق پرسنل]", "[حقوق پرسنل]", token);
			desc2 = ((fishIds.Count != 1) ? ("[سند فیش های حقوقی](" + desc2 + ")") : ("[سند فیش حقوقی](" + desc2 + ")"));
			Description = Utilities.GetTranslate(desc2);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<AttSanad> GetAsync(string contextGuid, int id, CancellationToken token)
	{
		_ = 3;
		try
		{
			AttSanad ret = new AttSanad
			{
				ContextGuid = contextGuid
			};
			AttSanad attSanad = ret;
			attSanad._codeacc = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).CodeingAccounting;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_ATT_Sanad_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.LoadData(dr);
						}
						((DbConnection)(object)cn).Close();
						return ret;
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<AttSanad> GetAsync(string contextGuid, SqlDataReader dr, CancellationToken token)
	{
		try
		{
			AttSanad ret = new AttSanad
			{
				ContextGuid = contextGuid
			};
			AttSanad attSanad = ret;
			attSanad._codeacc = (await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token)).CodeingAccounting;
			ret.LoadData(dr);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["NO"].ToString() != DBNull.Value.ToString())
			{
				No = (int)((DbDataReader)(object)dr)["NO"];
			}
			if (((DbDataReader)(object)dr)["DateM"].ToString() != DBNull.Value.ToString())
			{
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["UID"].ToString() != DBNull.Value.ToString())
			{
				Uid = (short)((DbDataReader)(object)dr)["UID"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["Sum"].ToString() != DBNull.Value.ToString())
			{
				Sum = (decimal)((DbDataReader)(object)dr)["Sum"];
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task AddDetAsync(long codeMoein, long codeTafsil, decimal credit, decimal debit, string description, string titleTafsil, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(credit == 0m) || !(debit == 0m))
			{
				ATT_SanadDet item = new ATT_SanadDet(Id, debit, credit, codeMoein, codeTafsil, Utilities.GetTranslate(description), Utilities.GetTranslate(titleTafsil), mergeWithOther: true);
				(await GetLstSanadDetsAsync(token)).Add(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 494);
		}
	}

	private void SetDescription(string desc)
	{
		try
		{
			foreach (ATT_SanadDet item in _lstSanadDets.Where((ATT_SanadDet item) => string.IsNullOrEmpty(item.Description)))
			{
				item.Description = item.TitleTafsil + " : " + desc;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetDescription", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 506);
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
		try
		{
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlTransaction tr = cn.BeginTransaction();
			try
			{
				bool result = await SaveAsync(tr, token);
				List<ATT_SanadDet> listSanadDet = await GetLstSanadDetsAsync(token);
				await ATT_SanadDet.DeleteBySanadIdAsync(Id, tr, token);
				if (listSanadDet != null && listSanadDet.Count > 0)
				{
					foreach (ATT_SanadDet item in listSanadDet)
					{
						if (item != null)
						{
							item.SanadId = Id;
							await item.SaveAsync(tr, token);
						}
					}
				}
				((DbTransaction)(object)tr).Commit();
				((DbConnection)(object)cn).Close();
				string pDesc = "  [ثبت سند حسابداری به شماره]  : " + No + " " + Description;
				await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "Accounting.AccountingInsert", Id.ToString(), JsonConvert.SerializeObject((object)this));
				return result;
			}
			catch (Exception ex)
			{
				((DbTransaction)(object)tr).Rollback();
				((DbConnection)(object)cn).Close();
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_Sanad.cs", 542);
			}
			return false;
		}
		finally
		{
			((IDisposable)cn)?.Dispose();
		}
	}

	private async Task<bool> SaveAsync(SqlTransaction tr, CancellationToken token)
	{
		_ = 3;
		try
		{
			SqlCommand cmd = tr.Connection.CreateCommand();
			try
			{
				cmd.Transaction = tr;
				((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("NO", (object)No);
				cmd.Parameters.AddWithValue("DateM", (object)DateM);
				cmd.Parameters.AddWithValue("UID", (object)Uid);
				cmd.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
				SqlParameterCollection parameters = cmd.Parameters;
				parameters.AddWithValue("UserName", (object)(await GetUserNameAsync(token)));
				parameters = cmd.Parameters;
				parameters.AddWithValue("Sum", (object)(await GetSumDebitAsync(token)));
				if (Id <= 0)
				{
					((DbCommand)(object)cmd).CommandText = "dbo.sp_ATT_Sanad_Insert";
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						Id = Convert.ToInt16(obj.ToString());
					}
				}
				else
				{
					((DbCommand)(object)cmd).CommandText = "dbo.sp_ATT_Sanad_Update";
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				}
				return true;
			}
			finally
			{
				((IDisposable)cmd)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
		try
		{
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlTransaction tr = cn.BeginTransaction();
			try
			{
				if (!(await ATT_SanadDet.DeleteBySanadIdAsync(Id, tr, token)))
				{
					((DbTransaction)(object)tr).Rollback();
					return false;
				}
				if (!(await DeleteAsync(tr, token)))
				{
					((DbTransaction)(object)tr).Rollback();
					return false;
				}
				((DbTransaction)(object)tr).Commit();
				((DbConnection)(object)cn).Close();
				string pDesc = "  [حذف سند حسابداری به شماره] : " + No + " " + Description;
				await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "Accounting.AccountingDelete", Id.ToString(), JsonConvert.SerializeObject((object)this));
				return true;
			}
			catch (Exception ex)
			{
				((DbTransaction)(object)tr).Rollback();
				((DbConnection)(object)cn).Close();
				Log.Error(ex, "");
				throw;
			}
		}
		finally
		{
			((IDisposable)cn)?.Dispose();
		}
	}

	private async Task<bool> DeleteAsync(SqlTransaction tr, CancellationToken token)
	{
		try
		{
			SqlCommand cmd = tr.Connection.CreateCommand();
			try
			{
				cmd.Transaction = tr;
				((DbCommand)(object)cmd).CommandText = "[dbo].[sp_ATT_Sanad_DeleteRow]";
				((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("Id", (object)Id);
				await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
				return true;
			}
			finally
			{
				((IDisposable)cmd)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> Sedt2AccAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			Sanad temp = new Sanad
			{
				DateM = DateM,
				Description = Description
			};
			List<ATT_SanadDet> list = await GetLstSanadDetsAsync(token);
			if (list != null && list.Count > 0)
			{
				foreach (ATT_SanadDet item2 in list)
				{
					SanadDet item = new SanadDet
					{
						CodeMoein = item2.CodeMoein,
						CodeTafsil = item2.CodeTafsil,
						Description = item2.Description,
						Debit = item2.Debit,
						Credit = item2.Credit
					};
					temp.List.Add(item);
				}
			}
			string pDesc = "  [انتقال سند حسابداری (به سیتم مالی) شماره]  : " + No + " " + Description;
			await LogUsers.RegisterNewLogAsync(ContextGuid, pDesc, "Accounting.AccountingMove", Id.ToString(), JsonConvert.SerializeObject((object)this));
			return temp.Save();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<AttSanad>> GetAllAsync(string contextGuid, DateTime date1, DateTime date2, CancellationToken token)
	{
		List<AttSanad> ret = new List<AttSanad>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_ATT_Sanad_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					cmd.Parameters.AddWithValue("@date1", (object)date1);
					cmd.Parameters.AddWithValue("@date2", (object)date2);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							List<AttSanad> list = ret;
							list.Add(await GetAsync(contextGuid, dr, token));
						}
						((DbConnection)(object)cn).Close();
						return ret;
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
