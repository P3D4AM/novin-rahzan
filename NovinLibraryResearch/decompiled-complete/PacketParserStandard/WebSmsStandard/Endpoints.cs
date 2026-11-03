namespace WebSmsStandard;

public class Endpoints
{
	public class Npsms
	{
		private const string root = "http://webservice.novinp.ir/sms/";

		public const string SendSmsV1 = "http://webservice.novinp.ir/sms/Npsms/SendSms/";

		public const string SendSmsV2 = "http://webservice.novinp.ir/sms/Npsms/SendSmsV2/";

		public const string GetCredit = "http://webservice.novinp.ir/sms/Npsms/GetCredit/";

		public const string GetDelivery = "http://webservice.novinp.ir/sms/Npsms/GetDelivery/";

		public const string GetAllDeliveries = "http://webservice.novinp.ir/sms/Npsms/GetallDeliveries";
	}
}
