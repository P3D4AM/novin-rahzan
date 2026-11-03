using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class DeliveryClient
{
	public static async Task<ReturnedSaveFuncInfo> ResetDelivery(Guid pGuid, EntityPart Ppart, short pDepId, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("UserName", DepAdmin.UserName);
			dictionary.Add("Password", DepAdmin.Password);
			dictionary.Add("Guid", pGuid.ToString());
			short num = (short)Ppart;
			dictionary.Add("part", num.ToString());
			dictionary.Add("DepID", pDepId.ToString());
			string url = HttpHelpers.ApiUrl + "Department/Delivery/DeliveryResetSatus";
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await url.GetFromApi<ReturnedSaveFuncInfo>(token, 3, dictionary), ReturnedState.None, "ResetDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DeliveryClient.cs", 27);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DeliveryClient.cs", 31);
			ret.AddReturnedValue(ex, "", "ResetDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\DeliveryClient.cs", 32);
		}
		return ret;
	}
}
