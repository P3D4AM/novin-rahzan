using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class PayamakSettings
{
	private string _sellMessage;

	private string _sellMessageMotafareghe;

	private string _checkMMessage;

	private string _checkShMessage;

	private string _gnMessage;

	private string _receptionMessage;

	private string _pardakhtMessage;

	private int _payamakManageDore;

	private string _payamak_Manage_Message;

	private string _ghestM_Message;

	private string ghestSH_Message;

	private string activation_Message;

	private string _receptionEnteghalMessage;

	private string _pardakhtEnteghalMessage;

	private string _productNegativeMessage;

	private string _productOrderPointMessage;

	private string _productZiroMessage;

	public string SellManagerPattern { get; set; }

	public PayamakSendStatus SellManagerStatus { get; set; }

	public string SellAccountPattern { get; set; }

	public PayamakSendStatus SellAccountStatus { get; set; }

	public string BackSellManagerPattern { get; set; }

	public PayamakSendStatus BackSellManagerStatus { get; set; }

	public string BackSellAccountPattern { get; set; }

	public PayamakSendStatus BackSellAccountStatus { get; set; }

	public string PishFactorManagerPattern { get; set; }

	public PayamakSendStatus PishFactorManagerStatus { get; set; }

	public string PishFactorAccountPattern { get; set; }

	public PayamakSendStatus PishFactorAccountStatus { get; set; }

	public string DaryaftManagerPattern { get; set; }

	public PayamakSendStatus DaryaftManagerStatus { get; set; }

	public string DaryaftAccountPattern { get; set; }

	public PayamakSendStatus DaryaftAccountStatus { get; set; }

	public string PardakhtManagerPattern { get; set; }

	public PayamakSendStatus PardakhtManagerStatus { get; set; }

	public string PardakhtAccountPattern { get; set; }

	public PayamakSendStatus PardakhtAccountStatus { get; set; }

	public PayamakSendStatus LotteryStatus { get; set; }

	public string ActivationPattern
	{
		get
		{
			if (string.IsNullOrEmpty(activation_Message))
			{
				activation_Message = "*خرسندیم که از نرم افزارهای نوین پرداز استفاده می نمایید*\r\nامکانات و زیربسته های زیر بر روی قفل شما فعال می باشد:\r\n[عنوان پکیج]\r\n[تاریخ اتمام پشتیبانی]\r\nجهت تمدید پشتیبانی می توانید بر روی لینک زیر کلیک نمایید:\r\n[ادرس سایت]\r\n[شماره تماس]";
			}
			return activation_Message;
		}
		set
		{
			activation_Message = value;
		}
	}

	public PayamakSendStatus ActivationPatternStatus { get; set; }

	[DefaultValue(false)]
	public bool SendAfterSell_Insert { get; set; }

	[DefaultValue(false)]
	public bool SendAfterSell_Update { get; set; }

	[DefaultValue(true)]
	public bool ShowForm { get; set; } = true;


	public string SellMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_sellMessage))
			{
				_sellMessage = "\ud83c\udf39فروش پایان کار نیست، بلکه آغاز یک تعهد است\r\n[نام مشتری]\r\n.فاکتور فروش شما ثبت شد\r\nمبلغ:\r\n[جمع اقلام]\r\nمانده حساب فعلی شما:\r\n[حساب فعلی]\r\nبا تشکر\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _sellMessage;
		}
		set
		{
			_sellMessage = value;
		}
	}

	public string SellMessage_Motafareghe
	{
		get
		{
			if (string.IsNullOrEmpty(_sellMessageMotafareghe))
			{
				_sellMessageMotafareghe = "\ud83c\udf39فروش پایان کار نیست، بلکه آغاز یک تعهد است\r\nمشتری گرامی\r\n.فاکتور فروش شما ثبت شد\r\nمبلغ فاکتور:\r\n[جمع اقلام]\r\nبا تشکر\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _sellMessageMotafareghe;
		}
		set
		{
			_sellMessageMotafareghe = value;
		}
	}

	public int Account_ID { get; set; }

	[DefaultValue(false)]
	public bool SendCheckM { get; set; }

	public string CheckM_Message
	{
		get
		{
			if (string.IsNullOrEmpty(_checkMMessage))
			{
				_checkMMessage = "[نام طرف حساب]\r\nبا سلام\r\nیادآوری سررسید([تاریخ چک]) چک\r\n[شماره چک]\r\nمبلغ:\r\n[مبلغ چک]\r\nبا تشکر\ud83c\udf39\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _checkMMessage;
		}
		set
		{
			_checkMMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool SendCheckSH { get; set; }

	public string CheckSH_Message
	{
		get
		{
			if (string.IsNullOrEmpty(_checkShMessage))
			{
				_checkShMessage = "یادآوری جهت پرداخت چک\r\nطرف حساب:\r\n[نام گیرنده]\r\nسریال چک:\r\n[شماره چک]\r\nسررسید:\r\n[تاریخ چک]\r\nمبلغ:\r\n[مبلغ چک]\r\nموجودی فعلی بانک:\r\n[موجودی بانک]";
			}
			return _checkShMessage;
		}
		set
		{
			_checkShMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool SendGN { get; set; }

	public string GN_Message
	{
		get
		{
			if (string.IsNullOrEmpty(_gnMessage))
			{
				_gnMessage = "مشترک گرامی؛\r\n[طرف حساب]\r\nتاریخ اتمام گارانتی محصول شما:\r\n[تاریخ اتمام گارانتی]\r\n.می باشد\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _gnMessage;
		}
		set
		{
			_gnMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool CheckAghsat_Auto { get; set; }

	[DefaultValue(3)]
	public int CheckAghsat_DaysAutoCount { get; set; } = 3;


	[DefaultValue(false)]
	public bool SendGhestM_Moavagh { get; set; }

	[DefaultValue(false)]
	public bool SendGhestM { get; set; }

	public string GhestM_Message
	{
		get
		{
			if (string.IsNullOrEmpty(_ghestM_Message))
			{
				_ghestM_Message = "[نام طرف حساب]\r\nیادآوری قسط\r\nسررسید:\r\n[تاریخ قسط])\r\nمبلغ:\r\n[مبلغ قسط]\r\nخوش حسابی شما مزید امتنان است.\ud83c\udf39\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _ghestM_Message;
		}
		set
		{
			_ghestM_Message = value;
		}
	}

	public string GhestM_PayamakMoavagh { get; set; }

	[DefaultValue(false)]
	public bool SendGhestSH { get; set; }

	public string GhestSH_Message
	{
		get
		{
			if (string.IsNullOrEmpty(ghestSH_Message))
			{
				ghestSH_Message = "سررسید قسط پرداختنی:[تاریخ قسط]\r\n[گیرنده]\r\nمبلغ: [مبلغ قسط]";
			}
			return ghestSH_Message;
		}
		set
		{
			ghestSH_Message = value;
		}
	}

	[DefaultValue(false)]
	public bool SendPayamak_Manage { get; set; }

	public string Payamak_Manage_Message
	{
		get
		{
			if (string.IsNullOrEmpty(_payamak_Manage_Message))
			{
				_payamak_Manage_Message = "[تاریخ روز]\r\n[زمان ارسال]\r\nجمع فروش:\r\n[جمع فروش]\r\nدریافت حواله:\r\n[جمع دریافتی بانک]\r\nدریافت چک([تعداد چک های دریافتی]):\r\n[جمع چک های دریافتی]\r\nدریافت نقدی\r\n:[جمع دریافتی نقدی]";
			}
			return _payamak_Manage_Message;
		}
		set
		{
			_payamak_Manage_Message = value;
		}
	}

	public string Payamak_Manage_Mobile { get; set; }

	[DefaultValue(false)]
	public bool Payamak_Manage_SendHour { get; set; }

	[DefaultValue(12)]
	public int Payamak_Manage_Hour { get; set; } = 12;


	public int Payamak_Manage_Min { get; set; }

	public int Payamak_Manage_Dore
	{
		get
		{
			if (_payamakManageDore < 30)
			{
				_payamakManageDore = 30;
			}
			return _payamakManageDore;
		}
		set
		{
			_payamakManageDore = value;
		}
	}

	[DefaultValue(false)]
	public bool RunAfterInitializeProgram { get; set; }

	[DefaultValue(false)]
	public bool SendAfterReception_Insert { get; set; }

	[DefaultValue(false)]
	public bool SendAfterReception_Update { get; set; }

	public string ReceptionMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionMessage))
			{
				_receptionMessage = "[نام مشتری]\r\nمبلغ:\r\n[جمع اقلام]\r\nاز شما دریافت شد\r\nمانده فعلی حساب شما:\r\n[حساب فعلی]\r\nبا تشکر\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _receptionMessage;
		}
		set
		{
			_receptionMessage = value;
		}
	}

	public Guid Payamak_SelectionCust_ID { get; set; }

	public string PardakhtMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtMessage))
			{
				_pardakhtMessage = "[نام مشتری]\r\nمبلغ:\r\n[جمع اقلام]\r\nبه شما پرداخت شد\r\nمانده فعلی حساب شما:\r\n[حساب فعلی]\r\nبا تشکر\r\n[عنوان فروشگاه]\r\n☎\ufe0f [تلفن فروشگاه]";
			}
			return _pardakhtMessage;
		}
		set
		{
			_pardakhtMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool SendAfterPardakht_Insert { get; set; }

	[DefaultValue(false)]
	public bool SendAfterPardakht_Update { get; set; }

	[DefaultValue(7595)]
	public int BaseHubPort { get; set; } = 7595;


	public string ServerConnectionString { get; set; }

	[DefaultValue("127.0.0.1")]
	public string BaseHubIp { get; set; } = "127.0.0.1";


	[DefaultValue("0.0.0.0")]
	public string SetupIp { get; set; } = "0.0.0.0";


	public List<OccasionCalendarModel> OccasionCalendar { get; set; }

	public PayamakSendStatus ReceptionEnteghalStatus { get; set; }

	[DefaultValue(false)]
	public bool SendAfterReception_Insert_Enteghal { get; set; }

	[DefaultValue(false)]
	public bool SendAfterReception_Update_Enteghal { get; set; }

	public string ReceptionEnteghalMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_receptionEnteghalMessage))
			{
				_receptionEnteghalMessage = "[نام مشتری] \r\nمبلغ: [مبلغ] \r\nتوسط: [طرف حساب دریافت] \r\nبه شما پرداخت شد. \r\nمانده حساب فعلی شما: \r\n[مانده حساب] \r\nبا تشکر \r\n[عنوان فروشگاه] \r\n[تلفن فروشگاه]";
			}
			return _receptionEnteghalMessage;
		}
		set
		{
			_receptionEnteghalMessage = value;
		}
	}

	public PayamakSendStatus PardakhtEnteghalStatus { get; set; }

	[DefaultValue(false)]
	public bool SendAfterPardakht_Insert_Enteghal { get; set; }

	[DefaultValue(false)]
	public bool SendAfterPardakht_Update_Enteghal { get; set; }

	public string PardakhtEnteghalMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_pardakhtEnteghalMessage))
			{
				_pardakhtEnteghalMessage = "[نام مشتری] \r\nمبلغ: [مبلغ] \r\nتوسط: [طرف حساب دریافت] \r\nاز شما دریافت شد. \r\nمانده حساب فعلی شما: \r\n[مانده حساب] \r\nبا تشکر \r\n[عنوان فروشگاه] \r\n[تلفن فروشگاه]";
			}
			return _pardakhtEnteghalMessage;
		}
		set
		{
			_pardakhtEnteghalMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool ProductOrderPointSend { get; set; }

	public string ProductOrderPointMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_productOrderPointMessage))
			{
				_productOrderPointMessage = "طبق ثبت فاکتور شماره [شماره فاکتور] \r\nکالاهای زیر در وضعیت [وضعیت] قراردارند: \r\n[کد کالا] - [نام کالا]";
			}
			return _productOrderPointMessage;
		}
		set
		{
			_productOrderPointMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool ProductZiroSend { get; set; }

	public string ProductZiroMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_productZiroMessage))
			{
				_productZiroMessage = "طبق ثبت فاکتور شماره [شماره فاکتور] \r\nکالاهای زیر در وضعیت [وضعیت] قراردارند:[کد کالا] - [نام کالا]";
			}
			return _productZiroMessage;
		}
		set
		{
			_productZiroMessage = value;
		}
	}

	[DefaultValue(false)]
	public bool ProductNegativeSend { get; set; }

	public string ProductNegativeMessage
	{
		get
		{
			if (string.IsNullOrEmpty(_productNegativeMessage))
			{
				_productNegativeMessage = "طبق ثبت فاکتور شماره [شماره فاکتور] \r\nکالاهای زیر در وضعیت [وضعیت] قراردارند: \r\n[کد کالا] - [نام کالا]";
			}
			return _productNegativeMessage;
		}
		set
		{
			_productNegativeMessage = value;
		}
	}
}
