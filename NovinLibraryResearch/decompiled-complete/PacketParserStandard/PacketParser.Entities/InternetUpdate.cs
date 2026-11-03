using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class InternetUpdate : IInternetUpdate
{
	private static string url = HttpHelpers.ApiUrl + "Department/File/";

	private string _version;

	public FileManagement_Parts Source { get; set; }

	public string Version
	{
		get
		{
			return _version ?? "";
		}
		set
		{
			_version = value;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<WebFileInfo>>>> GetFromApi(List<InternetUpdate> updates)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<List<WebFileInfo>>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<List<WebFileInfo>>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebFileInfo>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<List<WebFileInfo>, List<InternetUpdate>>(updates, url + "GetRequireFiles", null, strongDeserialize: false, 15000, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\InternetUpdate.cs", 23);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\InternetUpdate.cs", 28);
			ret.AddReturnedValue(ex, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\InternetUpdate.cs", 29);
		}
		return ret;
	}
}
