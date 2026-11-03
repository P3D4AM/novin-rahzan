using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class TestWooCommerse
{
	public static async Task<ReturnedSaveFuncInfo> SendDataToCategeory(WebUser UserInfo)
	{
		new HttpResponseMessage(HttpStatusCode.BadRequest);
		return new ReturnedSaveFuncInfo();
	}

	private static async Task<ReturnedSaveFuncInfo> DeleteCategeory(WebUser UserInfo, WooSendCategoryRecponseModel model)
	{
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			await Task.Delay(3000);
			using HttpClient client = new HttpClient();
			string requestUri = UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/products/categories/" + model.id + "?force=true&consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			string ck = UserInfo.WordPressBasicSettings.Ck;
			string cs = UserInfo.WordPressBasicSettings.Cs;
			string parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(ck + ":" + cs));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
			response = await client.DeleteAsync(requestUri);
			response.EnsureSuccessStatusCode();
			ret.AddInformation("تست ارسال داده و وردپرس به درستی پاس شد", "", "DeleteCategeory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 128);
		}
		catch (HttpRequestException ex)
		{
			try
			{
				WebErrorLog.Instance.StartLog(ex, $"Id=>{model.id}یجاد گروه کالا به ئرستی انجام شد ولیکن سیستم موفق به حذف گروه ایجاد شده نبود {model.name}/Uri=>{UserInfo.WordPressBasicSettings.HostUrl}", "DeleteCategeory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 136);
				await HttpHelpers.ReadJsonResponseAsync<WoocommerceError>(response);
				ret.AddError("ایجاد گروه کالا به ئرستی انجام شد ولیکن سیستم موفق به حذف گروه ایجاد شده نبود", "", "DeleteCategeory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 138);
			}
			catch
			{
				ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "", "DeleteCategeory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 142);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "DeleteCategeory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 147);
			ret.AddReturnedValue(ex2, "", "DeleteCategeory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 148);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> TestValidSSl(string HostUrl)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (await HostUrl.CheckWebAddressExsistAsync(IsSSl: true))
			{
				ret.AddInformation("سایت دارای SSl معتبر است ", "", "TestValidSSl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 159);
			}
			else
			{
				ret.AddError(EnSystem.None, "سایت دارای SSl معتبر نیست", "", "TestValidSSl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 161);
			}
			await Task.Delay(1000);
		}
		catch (HttpRequestException ex)
		{
			ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "", "TestValidSSl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 166);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "TestValidSSl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 170);
			ret.AddReturnedValue(ex2, "", "TestValidSSl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 171);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> TestValidSite(string HostUrl)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (await HostUrl.CheckWebAddressExsistAsync(IsSSl: false))
			{
				ret.AddInformation("سایت درست و در دسترس است", "", "TestValidSite", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 182);
			}
			else
			{
				ret.AddError(EnSystem.None, "سایت در دسترس نیست", "", "TestValidSite", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 184);
			}
			await Task.Delay(1000);
		}
		catch (HttpRequestException ex)
		{
			ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "", "TestValidSite", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 189);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "TestValidSite", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 193);
			ret.AddReturnedValue(ex2, "", "TestValidSite", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 194);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> TestGetKala(WebUser UserInfo)
	{
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			new Uri(UserInfo.WordPressBasicSettings.HostUrl + "wp-json/wc/v3/products");
			using HttpClient client = new HttpClient();
			string ck = UserInfo.WordPressBasicSettings.Ck;
			string cs = UserInfo.WordPressBasicSettings.Cs;
			string parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(ck + ":" + cs));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
			response = await client.GetAsync(UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/products/categories/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs);
			ret.value = "/wp-json/wc/v3/products/categories/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			response.EnsureSuccessStatusCode();
			ret.AddInformation("کالاها از سایت وردپرس دریافت شد", "", "TestGetKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 216);
		}
		catch (HttpRequestException ex)
		{
			try
			{
				WebErrorLog.Instance.StartLog(ex, "", "TestGetKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 223);
				WoocommerceError woocommerceError = await HttpHelpers.ReadJsonResponseAsync<WoocommerceError>(response);
				ret.AddError(EnSystem.None, woocommerceError.message + "/// Code=>" + woocommerceError.data.status, "TestGetKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 225);
			}
			catch
			{
				ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "TestGetKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 229);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "TestGetKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 234);
			ret.AddReturnedValue(ex2, "", "TestGetKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 235);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> TestGetGroupKala(WebUser UserInfo)
	{
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			new Uri(UserInfo.WordPressBasicSettings.HostUrl + "wp-json/wc/v3/products/categories");
			using HttpClient client = new HttpClient();
			string ck = UserInfo.WordPressBasicSettings.Ck;
			string cs = UserInfo.WordPressBasicSettings.Cs;
			string parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(ck + ":" + cs));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
			response = await client.GetAsync(UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/products/categories/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs);
			ret.value = "/wp-json/wc/v3/products/categories/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			response.EnsureSuccessStatusCode();
			ret.AddInformation("گروه کالاها از سایت وردپرس دریافت شد", "", "TestGetGroupKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 257);
		}
		catch (HttpRequestException ex)
		{
			try
			{
				WebErrorLog.Instance.StartLog(ex, "", "TestGetGroupKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 264);
				WoocommerceError woocommerceError = await HttpHelpers.ReadJsonResponseAsync<WoocommerceError>(response);
				ret.AddError(EnSystem.None, woocommerceError.message + "/// Code=>" + woocommerceError.data.status, "TestGetGroupKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 266);
			}
			catch
			{
				ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "TestGetGroupKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 270);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "TestGetGroupKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 275);
			ret.AddReturnedValue(ex2, "", "TestGetGroupKala", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 276);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> TestGetCustomer(WebUser UserInfo)
	{
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			new Uri(UserInfo.WordPressBasicSettings.HostUrl + "wp-json/wc/v3/customers");
			using HttpClient client = new HttpClient();
			string ck = UserInfo.WordPressBasicSettings.Ck;
			string cs = UserInfo.WordPressBasicSettings.Cs;
			string parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(ck + ":" + cs));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
			response = await client.GetAsync(UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/products/categories/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs);
			ret.value = "/wp-json/wc/v3/products/categories/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			response.EnsureSuccessStatusCode();
			ret.AddInformation("لیست اشخاص از سایت وردپرس دریافت شد", "", "TestGetCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 298);
		}
		catch (HttpRequestException ex)
		{
			try
			{
				WebErrorLog.Instance.StartLog(ex, "", "TestGetCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 305);
				WoocommerceError woocommerceError = await HttpHelpers.ReadJsonResponseAsync<WoocommerceError>(response);
				ret.AddError(EnSystem.None, woocommerceError.message + "/// Code=>" + woocommerceError.data.status, "TestGetCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 307);
			}
			catch
			{
				ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "TestGetCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 311);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "TestGetCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 316);
			ret.AddReturnedValue(ex2, "", "TestGetCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 317);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> TestGetVersion(WebUser UserInfo)
	{
		HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			new Uri(UserInfo.WordPressBasicSettings.HostUrl + "wp-json/wc/v3/version");
			using HttpClient client = new HttpClient();
			string ck = UserInfo.WordPressBasicSettings.Ck;
			string cs = UserInfo.WordPressBasicSettings.Cs;
			string parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(ck + ":" + cs));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
			response = await client.GetAsync(UserInfo.WordPressBasicSettings.HostUrl ?? "");
			ReturnedSaveFuncInfoWithValue<UrlDataResponseModel> returnedSaveFuncInfoWithValue = await WpNpApiUtilities.InqueryWpExtentitionVerionAsync(UserInfo, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 337);
			response.EnsureSuccessStatusCode();
			ret.AddInformation(returnedSaveFuncInfoWithValue.value.Data ?? "", "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 339);
			if (ret.HasError)
			{
				ret.AddInformation("خطا", "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 340);
			}
		}
		catch (HttpRequestException ex)
		{
			try
			{
				WebErrorLog.Instance.StartLog(ex, "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 347);
				WoocommerceError woocommerceError = await HttpHelpers.ReadJsonResponseAsync<WoocommerceError>(response);
				ret.AddError(EnSystem.None, woocommerceError.message + "/// Code=>" + woocommerceError.data.status, "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 349);
			}
			catch
			{
				ret.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 353);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 358);
			ret.AddReturnedValue(ex2, "", "TestGetVersion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 359);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<UrlDataResponseModel>> ViewDataPageproducts(WebUser UserInfo)
	{
		ReturnedSaveFuncInfoWithValue<UrlDataResponseModel> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<UrlDataResponseModel>();
		returnedSaveFuncInfoWithValue.value = new UrlDataResponseModel();
		try
		{
			returnedSaveFuncInfoWithValue.value.Url = UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/products/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			if (!string.IsNullOrEmpty(UserInfo.WordPressBasicSettings.HostUrl))
			{
				returnedSaveFuncInfoWithValue.value.Data = "/wp-json/wc/v3/products/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			}
			else
			{
				returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "Url Not Valid", "ViewDataPageproducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 376);
			}
		}
		catch (HttpRequestException ex)
		{
			returnedSaveFuncInfoWithValue.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "ViewDataPageproducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 380);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ViewDataPageproducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 384);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex2, "", "ViewDataPageproducts", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 385);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<UrlDataResponseModel>> ViewDataPageCustomer(WebUser UserInfo)
	{
		ReturnedSaveFuncInfoWithValue<UrlDataResponseModel> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<UrlDataResponseModel>();
		returnedSaveFuncInfoWithValue.value = new UrlDataResponseModel();
		try
		{
			returnedSaveFuncInfoWithValue.value.Url = UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/Customers/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			if (!string.IsNullOrEmpty(UserInfo.WordPressBasicSettings.HostUrl))
			{
				returnedSaveFuncInfoWithValue.value.Data = "/wp-json/wc/v3/Customers/?consumer_key=" + UserInfo.WordPressBasicSettings.Ck + "&consumer_secret=" + UserInfo.WordPressBasicSettings.Cs;
			}
			else
			{
				returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "Url Not Valid", "ViewDataPageCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 403);
			}
		}
		catch (HttpRequestException ex)
		{
			returnedSaveFuncInfoWithValue.AddError(EnSystem.None, ex.Message + "=>" + ex.InnerException?.Message, "ViewDataPageCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 408);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ViewDataPageCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 413);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex2, "", "ViewDataPageCustomer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\DepartmentApiModel\\woocommerceTestModel.cs", 414);
		}
		return returnedSaveFuncInfoWithValue;
	}
}
