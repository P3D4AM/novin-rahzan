using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Dal.FLRCheckout;

public class Checkout : IHasContext
{
	public enum RewardTypeEnum : short
	{
		[PersianName("محاسبه عیدی و پاداش بر اساس مبلغ مشخص")]
		Fixed,
		[PersianName("محاسبه عیدی و پاداش بر اساس قرارداد جاری ")]
		CurrentContract
	}

	public enum SanavatTypeEnum : short
	{
		[PersianName("محاسبه سنوات بر اساس حداقل قانون کار")]
		Fixed,
		[PersianName("محاسبه سنوات بر اساس قرارداد جاری ")]
		CurrentContract
	}

	private decimal _fixedReward;

	private decimal _fixedSanavat;

	private List<CheckoutDet> _checkoutDetList;

	private DateTime? _contractDateStartM;

	private int _FirstNewContractNo;

	public string ContextGuid { get; set; }

	public DateTime ContractDateStartM
	{
		get
		{
			if (_contractDateStartM.HasValue)
			{
				return _contractDateStartM.Value;
			}
			_contractDateStartM = Utilities.CalendarToMiladi(Calendar.MiladiToCalture(DateTime.Now).Substring(0, 4) + "/01/01");
			if (_contractDateStartM < new DateTime(2020, 3, 20))
			{
				_contractDateStartM = new DateTime(2020, 3, 20);
			}
			return _contractDateStartM.Value;
		}
		set
		{
			_contractDateStartM = value;
		}
	}

	public string ContractDateStartSh
	{
		get
		{
			return Calendar.MiladiToCalture(ContractDateStartM);
		}
		set
		{
			if (value != null)
			{
				ContractDateStartM = Utilities.CalendarToMiladi(value);
			}
		}
	}

	public short DaysOfReward { get; set; } = 60;


	public short DaysOfSanavat { get; set; } = 60;


	public decimal FixedReward
	{
		get
		{
			return _fixedReward;
		}
		set
		{
			if (!(_fixedReward == value))
			{
				_fixedReward = value;
				this.OnFixedRewardChange?.Invoke(this, new EventArgs());
			}
		}
	}

	public decimal FixSanavat
	{
		get
		{
			return _fixedSanavat;
		}
		set
		{
			if (!(_fixedSanavat == value))
			{
				_fixedSanavat = value;
				this.OnFixSanavatChange?.Invoke(this, new EventArgs());
			}
		}
	}

	public bool HasReward { get; set; }

	public bool HasSanavat { get; set; } = true;


	public decimal MaxReward { get; set; }

	public decimal MaxSanavat { get; set; }

	public decimal MinReward { get; set; }

	public decimal MinSanavat { get; set; }

	public decimal IncreaseSalary { get; set; }

	public decimal HousingBenefit { get; set; } = 2000000m;


	public decimal ChildrenBenefit { get; set; } = 1835427m;


	public decimal GroceryCoupon { get; set; } = 4000000m;


	public float IncreaseSalaryPercentage { get; set; } = 15f;


	public IList<int> ContractIds { get; set; }

	public IList<int> TamdidContractIds { get; set; } = new List<int>();


	public RewardTypeEnum RewardType { get; set; }

	public VacationTypeEnum VacationType { get; set; }

	public SanavatTypeEnum SanavatType { get; set; }

	public event EventHandler OnFixedRewardChange;

	public event EventHandler OnFixSanavatChange;

	public async Task<List<CheckoutDet>> GetCheckoutDetListAsync()
	{
		_ = 3;
		try
		{
			if (_checkoutDetList == null || ContractIds == null || ContractIds.Count <= 0)
			{
				_checkoutDetList = new List<CheckoutDet>();
				IList<int> contractIds = ContractIds;
				if (contractIds != null && contractIds.Count > 0)
				{
					foreach (int contractId2 in ContractIds)
					{
						GharardadNew Contract = await GharardadNew.GetAsync(ContextGuid, contractId2);
						Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> morakhasi = await Morakhasidaily.CalcLeaveBalanceAsync(ContextGuid, Contract);
						_checkoutDetList.Add(new CheckoutDet(ContextGuid, Contract, this, morakhasi));
					}
				}
			}
			else
			{
				for (int num = _checkoutDetList.Count - 1; num >= 0; num--)
				{
					if (!ContractIds.Contains(_checkoutDetList[num].ContractId))
					{
						_checkoutDetList.Remove(_checkoutDetList[num]);
					}
				}
				foreach (int contractId in ContractIds)
				{
					bool x = _checkoutDetList.Any((CheckoutDet p) => p.ContractId == contractId);
					GharardadNew Contract = await GharardadNew.GetAsync(ContextGuid, contractId);
					Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> morakhasi2 = await Morakhasidaily.CalcLeaveBalanceAsync(ContextGuid, Contract);
					if (!x)
					{
						_checkoutDetList.Add(new CheckoutDet(ContextGuid, Contract, this, morakhasi2));
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
		return _checkoutDetList;
	}

	public async Task<int> GetFirstNewContractNoAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_FirstNewContractNo == 0)
			{
				_FirstNewContractNo = await GetMaxContractNoAsync(ContextGuid, token) + 1;
				return _FirstNewContractNo;
			}
			return ++_FirstNewContractNo;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public void SetFirstNewContractNo(int value)
	{
		_FirstNewContractNo = value;
	}

	public Checkout(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private static async Task<int> GetMaxContractNoAsync(string contextGuid, CancellationToken token)
	{
		_ = 2;
		try
		{
			int No = 0;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_Gharardad_GetMaxNo", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["No"].ToString() != "")
						{
							No = (int)((DbDataReader)(object)dr)["No"];
						}
						((DbConnection)(object)cn).Close();
						((Component)(object)cmd).Dispose();
						return No;
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
