using System;
using System.Collections.Generic;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Taradod;

namespace TimeAttendanceDAL.Dal.FLRCheckout;

public class CheckoutDet : IHasContext
{
	public enum GharardadStatus
	{
		CheckNeeded,
		CheckIsOk
	}

	private decimal? _vacationPrice;

	private decimal? _sumRewardPrice;

	private decimal? _sumSanavatPrice;

	private string _checkoutDateSh;

	public string ContextGuid { get; set; }

	public GharardadNew Contract { get; set; }

	public int SumVacationDaily => Morakhasi.Item1;

	public int SumVacationHourly => Morakhasi.Item2;

	public double SumVacationLeft => Morakhasi.Item3;

	public Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> Morakhasi { get; private set; }

	public string CheckoutDateSh
	{
		get
		{
			if (string.IsNullOrEmpty(_checkoutDateSh))
			{
				_checkoutDateSh = Calendar.MiladiToCalture(DateTime.Now);
			}
			return _checkoutDateSh;
		}
		set
		{
			_checkoutDateSh = value;
		}
	}

	public string BtnView => "مشاهده جزئیات";

	[PersianName("شماره قرارداد")]
	public int ContractId => Contract.ID;

	public decimal SumRewardPrice
	{
		get
		{
			try
			{
				Checkout checkout = Checkout;
				if (checkout != null)
				{
					_ = checkout.HasReward;
					if (true && (Checkout?.HasReward).Value)
					{
						if (Checkout.RewardType == Checkout.RewardTypeEnum.Fixed)
						{
							_sumRewardPrice = Convert.ToDecimal(Math.Round((double)Checkout.FixedReward * Duration / 365.0));
						}
						if (Checkout.RewardType == Checkout.RewardTypeEnum.CurrentContract)
						{
							_sumRewardPrice = Convert.ToDecimal(Math.Round((double)Checkout.MinReward / 60.0 * (double)(Checkout?.DaysOfReward).Value * Duration / 365.0));
						}
					}
				}
				if (_sumRewardPrice.HasValue)
				{
					return _sumRewardPrice.Value;
				}
				return 0m;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public double Duration => (Contract.DateFinishM - Contract.DateStartM).Days + 1;

	public decimal SumSanavatPrice
	{
		get
		{
			Checkout checkout = Checkout;
			if (checkout != null)
			{
				_ = checkout.HasSanavat;
				if (true && (Checkout?.HasSanavat).Value)
				{
					if (Checkout.SanavatType == Checkout.SanavatTypeEnum.Fixed)
					{
						_sumSanavatPrice = Convert.ToDecimal(Math.Round((double)Checkout.FixSanavat * Duration / 365.0));
					}
					if (Checkout.SanavatType == Checkout.SanavatTypeEnum.CurrentContract)
					{
						_sumSanavatPrice = Convert.ToDecimal(Math.Round((double)Checkout.MinSanavat / 60.0 * (double)(Checkout?.DaysOfSanavat).Value * Duration / 365.0));
					}
				}
			}
			if (_sumSanavatPrice.HasValue)
			{
				return _sumSanavatPrice.Value;
			}
			return 0m;
		}
	}

	public decimal SumAll => SumSanavatPrice + SumRewardPrice + SumVacationPrice;

	public decimal SumVacationPrice
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			try
			{
				Checkout checkout = Checkout;
				if (checkout != null && (int)checkout.VacationType == 2)
				{
					return Convert.ToDecimal((double)VacationPrice * SumVacationLeft);
				}
				return 0m;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public int ReminedVacation { get; set; }

	public Checkout Checkout { get; }

	public bool IsChecked { get; set; }

	public decimal VacationPrice
	{
		get
		{
			try
			{
				_vacationPrice = Contract?.PriceLeave;
				if (_vacationPrice.HasValue)
				{
					return _vacationPrice.Value;
				}
				return 0m;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw;
			}
		}
	}

	public decimal SalaryOldMonthly => Contract.SalaryOneMonth;

	public decimal SalaryNewMonthly => SalaryOldMonthly * (decimal)Checkout.IncreaseSalaryPercentage + Checkout.IncreaseSalary;

	public decimal VacationPriceNew => Contract.PriceLeave * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceOverTimeNew => Contract.PriceOverTime * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceLowTimeNew => Contract.PriceLowTime * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceAbsentNew => Contract.PriceAbsent * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceMissionNew => Contract.PriceMission * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceShiftNew1 => Contract.PriceNobatKari1 * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceShiftNew2 => Contract.PriceNobatKari2 * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceShiftNew3 => Contract.PriceNobatKari3 * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceNightNew => Contract.PriceNight * (decimal)Checkout.IncreaseSalaryPercentage;

	public decimal PriceHoliday => Contract.PriceHoliday * (decimal)Checkout.IncreaseSalaryPercentage;

	public GharardadStatus CheckStatus { get; set; }

	public CheckoutDet(string contextGuid, GharardadNew contract, Checkout checkout, Tuple<int, int, double, GharardadNew, List<MorakhasiSaati>> morakhasi)
	{
		ContextGuid = contextGuid;
		try
		{
			Morakhasi = morakhasi;
			Contract = contract;
			Checkout = checkout;
			checkout.OnFixedRewardChange += Checkout_OnRewardChange;
			checkout.OnFixSanavatChange += Checkout_OnSanavatChange;
			CalculateAll();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private void CalculateAll()
	{
		CalcReward();
		CalcSanavat();
		CalcVacationDays();
		CalcVacation();
	}

	private void CalcVacation()
	{
	}

	private void CalcVacationDays()
	{
		ReminedVacation = 10;
	}

	private void CalcSanavat()
	{
	}

	private void CalcReward()
	{
	}

	private void Checkout_OnRewardChange(object sender, EventArgs e)
	{
	}

	private void Checkout_OnSanavatChange(object sender, EventArgs e)
	{
	}
}
