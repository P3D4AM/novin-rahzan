using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Message;

public class WebMessageTemplate
{
	private static string apiUrl = HttpHelpers.ApiUrl;

	public int ID { get; set; }

	public int DepID { get; set; }

	public string TemplateName { get; set; }

	public string TemplateText { get; set; }

	public DateTime CreationDate { get; set; } = DateTime.Now;


	public DateTime Modified { get; set; }

	public static WebMessageTemplate Factory(string templatename, string templatetext)
	{
		try
		{
			return new WebMessageTemplate
			{
				TemplateName = templatename,
				TemplateText = templatetext,
				Modified = DateTime.Now
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 32);
			return null;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebMessageTemplate>>> GetAllAsync(string userName, string password, CancellationToken token, string apiAddress = null)
	{
		ReturnedSaveFuncInfoWithValue<List<WebMessageTemplate>> response = new ReturnedSaveFuncInfoWithValue<List<WebMessageTemplate>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.None, userName, password).GetAdminToken(token);
			Dictionary<string, string> headers = new Dictionary<string, string> { { "Authorization", returnedSaveFuncInfoWithValue.value } };
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfoWithValue<List<WebMessageTemplate>>>> returnedSaveFuncInfoWithValue2 = await ((apiAddress ?? apiUrl) + "Message/GetTemplates").GetFromApi<ReturnedSaveFuncInfoWithValue<List<WebMessageTemplate>>>(token, 10, headers);
			if (returnedSaveFuncInfoWithValue2.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue2.ErrorMessage, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 47);
				return response;
			}
			response = returnedSaveFuncInfoWithValue2.value.Data;
			if (response == null)
			{
				if (response == null)
				{
					response = new ReturnedSaveFuncInfoWithValue<List<WebMessageTemplate>>();
				}
				response.value = new List<WebMessageTemplate>();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 59);
			await response.AddErrorAsync(ex.Message, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 60);
		}
		return response;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string userName, string password, CancellationToken token, string apiAddress = null)
	{
		ReturnedSaveFuncInfo response = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<ReturnedSaveFuncInfo, WebMessageTemplate>(this, (apiAddress ?? apiUrl) + "Message/SaveTemplates", LoginInfo.CreateLoginInfo(userName, password, ENSource.None), new Dictionary<string, string>(), 15000, 5, token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue.ErrorMessage, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 74);
				return response;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 80);
			await response.AddErrorAsync(ex.Message, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 81);
		}
		return response;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(string userName, string password, CancellationToken token, string apiAddress = null)
	{
		ReturnedSaveFuncInfo response = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApiAsync<ReturnedSaveFuncInfo, int>(ID, (apiAddress ?? apiUrl) + "Message/DeleteTemplate", LoginInfo.CreateLoginInfo(userName, password, ENSource.None), new Dictionary<string, string>(), 15000, 5, token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				await response.AddErrorAsync(returnedSaveFuncInfoWithValue.ErrorMessage, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 95);
				return response;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 101);
			await response.AddErrorAsync(ex.Message, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageTemplate.cs", 102);
		}
		return response;
	}
}
