using System;
using PacketParser.Entities;

namespace PacketParser.Activation;

public class CLSLKCustomer
{
	public static void SendSMS(WebCus customer)
	{
		using (new JaegerMonitor("SendSMS", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKCustomer.cs"))
		{
			try
			{
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendSMS", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKCustomer.cs", 315);
			}
		}
	}
}
