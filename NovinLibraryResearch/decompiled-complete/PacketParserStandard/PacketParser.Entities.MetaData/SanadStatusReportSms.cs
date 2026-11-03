namespace PacketParser.Entities.MetaData;

public class SanadStatusReportSms
{
	private string _pattern;

	public bool IsSendSms { get; set; }

	public string MessagePattern
	{
		get
		{
			if (string.IsNullOrEmpty(_pattern))
			{
				_pattern = "[نام مشتری] عزیز ❤\ufe0f\r\nسفارش شما با شماره [شماره رسید] در مرحله [نام وضعیت] می باشد.\r\nتا آخرین مرحله کنار شما هستیم ! \ud83c\udf3a\r\n\ud83c\udfe2 [نام شرکت]\r\n☎\ufe0f [تلفن]\r\n\ud83d\udcf1 [همراه]";
			}
			return _pattern;
		}
		set
		{
			_pattern = value;
		}
	}

	public PayamakSendStatus SendStatus { get; set; }
}
