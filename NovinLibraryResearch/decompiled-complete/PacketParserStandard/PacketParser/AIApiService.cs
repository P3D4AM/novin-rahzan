using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PacketParser.AI.Prompt;
using PacketParser.AI.Response;
using PacketParser.Entities;

namespace PacketParser;

public class AIApiService : IDisposable
{
	private readonly string _apiKey;

	private readonly string _url;

	private readonly HttpClient _httpClient;

	private readonly JsonSerializerOptions _jsonOptions;

	private bool _disposed;

	public AIApiService(string apiKey, string url = "http://AI.NovinPApi.ir/api/Generate")
	{
		_apiKey = apiKey ?? throw new ArgumentNullException("apiKey");
		_url = url ?? throw new ArgumentNullException("url");
		_httpClient = new HttpClient();
		_httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
	}

	public async Task<ApiResponseWrapper> SendRequestAsync(string prompt, int maxTokens = 1000, double temperature = 0.7)
	{
		if (string.IsNullOrWhiteSpace(prompt))
		{
			throw new ArgumentException("Prompt نمی\u200cتواند خالی باشد.", "prompt");
		}
		string content2 = JsonSerializer.Serialize(new { prompt, maxTokens, temperature }, _jsonOptions);
		StringContent content = new StringContent(content2, Encoding.UTF8, "application/json");
		int retries = 3;
		checked
		{
			for (int attempt = 0; attempt < retries; attempt++)
			{
				try
				{
					using HttpResponseMessage response = await _httpClient.PostAsync(_url, content);
					response.EnsureSuccessStatusCode();
					return JsonSerializer.Deserialize<ApiResponseWrapper>(await response.Content.ReadAsStringAsync(), _jsonOptions);
				}
				catch (HttpRequestException ex) when (attempt < retries - 1)
				{
					Console.WriteLine($"خطای شبکه، تلاش مجدد {attempt + 1}/{retries}... {ex.Message}");
					await Task.Delay(1000);
				}
			}
			throw new HttpRequestException("پس از چند تلاش، اتصال به API موفق نبود.");
		}
	}

	public async Task<AIWpProductResponseModel> GenerateProductContentAsync(ProductPrompt prompt, int maxTokens = 1000)
	{
		ApiResponseWrapper apiResponseWrapper = await SendRequestAsync(prompt.Prompt, maxTokens);
		if (apiResponseWrapper == null || string.IsNullOrEmpty(apiResponseWrapper.Response))
		{
			return null;
		}
		try
		{
			return JsonSerializer.Deserialize<AIWpProductResponseModel>(apiResponseWrapper.Response, _jsonOptions);
		}
		catch (Exception ex)
		{
			Console.WriteLine("خطا در تجزیه JSON داخلی: " + ex.Message);
			throw;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_httpClient.Dispose();
			}
			_disposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public async Task<string> GenerateProductFullContentAsync(WebProduct prd, string url, string existsOnWp)
	{
		return (await SendRequestAsync("برای کالایی با نام " + prd.Name + " شرح کامل محصول را جهت استفاده در وب سایت ورد پرس بصورت دقیق تولید کن بصورتی که برای موتور جستجو بهینه سازی کامل انجام شده باشد و همچنین تگهای H1 , H2 طبق اصاول سئو به درستی استفاده شده باشند.\r\nقانون حق نداری لینک ها یا تصاویری را بگذاری که وجود خارجی ندارند حتما باید معتبر و بهتر است از سایت خودمون(" + url + ") باشند\r\nاین مشخصات را در حال حاضر از کالا دارم غیر از نام:\r\n" + Json.ToStringJson(prd) + "\r\nآدرس سایت ام که کالا میخواد توش اضافه و یا بروز رسانی بشه این هست: " + url + "\r\n" + (string.IsNullOrEmpty(existsOnWp) ? "" : ("مشخصات کالای موجود در ورد پرس:" + existsOnWp + "\r\n")) + "قوانین: تحت هیچ شرایط هیچ توضیح اضافی یا متن دیگری و یا پیشنهاد ای به من نده که همین داده رو عینا میخواهم در مشخصات کامل کالا درج کنم"))?.Response;
	}

	public async Task<string> GenerateSeoDataFieldsAsync(WebProduct prd, string url, string existsOnWp)
	{
		return (await SendRequestAsync("برای کالایی با نام " + prd.Name + " که مشخصات ذیل را نیز از آن کالا دارم:\r\n" + Json.ToStringJson(prd) + "\r\nآدرس سایت ام که کالا میخواد توش اضافه و یا بروز رسانی بشه این هست: " + url + "\r\n" + (string.IsNullOrEmpty(existsOnWp) ? "" : ("مشخصات کالای موجود در ورد پرس:" + existsOnWp + "\r\n")) + "لطفا مشخصات سئو را جهت استفاده برای افزونه سئو سایت وردپرس در سه سطر به من بده\r\nقانون فقط و فقط در سه سطر بهم پاسخ بده\r\nهیچ نوشته اضافه ای درج نکن و فقط در هر سطر به ترتیت داده خودش رو درج کن\r\nسطر1: SeoTitle\r\nسطر2: SeoDescription\r\nسط31: SeoFocusKeyword"))?.Response;
	}

	public async Task<string> GenerateProductQuickTextAsync(WebProduct prd, string url, string existsOnWp)
	{
		return (await SendRequestAsync("برای کالایی با نام " + prd.Name + " شرح کوتاه و مختصر جهت درج در وب سایت ورد پرس با در نظر گرفتن اصاول سئو تولید کن\r\nقانون حق نداری لینک ها یا تصاویری را بگذاری که وجود خارجی ندارند حتما باید معتبر و بهتر است از سایت خودمون(" + url + ") باشند\r\nاین مشخصات را در حال حاضر از کالا دارم غیر از نام:\r\n" + Json.ToStringJson(prd) + "\r\nآدرس سایت ام که کالا میخواد توش اضافه و یا بروز رسانی بشه این هست: " + url + "\r\n" + (string.IsNullOrEmpty(existsOnWp) ? "" : ("مشخصات کالای موجود در ورد پرس:" + existsOnWp + "\r\n")) + "قانون: تحت هیچ شرایط توضیح و یا متن اضافه و یا پیشنهاد دیگری بهم نده که میخواهم همین متن را عینا در شرح مختصر کالا درج کنم"))?.Response;
	}
}
