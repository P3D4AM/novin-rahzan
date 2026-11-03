using System;
using PacketParser.Entities;

namespace PacketParser.AI.Prompt;

public class ProductPrompt : BasePrompt
{
	private WebProduct _prd;

	private string _existsOnWp;

	private string _url;

	public new int MaxTokens { get; set; }

	public new double Temperature { get; set; }

	public override string Prompt => "کالایی با نام \"" + _prd.Name + "\" دارم. مشخصات کامل کالا:" + Json.ToStringJson(_prd) + "\r\n" + ((!string.IsNullOrEmpty(_existsOnWp)) ? ("مشخصات کالای تعریف شده فعلی در وردپرس: " + _existsOnWp + "\r\n") : "") + "آدرس وب سایت من که کالا رو توش درج میکنم:" + _url + "\r\nخروجی باید فقط و فقط یک JSON معتبر باشد و هیچ متن، توضیح، توضیح اضافه یا علامت دیگری قبل یا بعد از JSON نوشته نشود. ساختار JSON باید دقیقا\u064b به شکل زیر باشد و همه\u200cی فیلدها باید مقداردهی شوند: {\"FullContent\":\"\",\"QuickContent\":\"\",\"SeoTitle\":\"\",\"SeoDescription\":\"\",\"SeometaKey\":\"\"} راهنما: - FullContent: متن کامل محصول شامل H1 و H2. - QuickContent: خلاصه\u200cای کوتاه و دقیق از محصول. - SeoTitle: عنوان سئو که شامل نام محصول باشد. - SeoDescription: توضیح سئو شامل نام محصول و ویژگی\u200cهای کلیدی. - SeometaKey: کلمات کلیدی سئو که شامل نام محصول باشند. ممنوعیت\u200cها: - هیچ متن یا کاراکتر اضافی خارج از JSON ننویس. - هیچ تصویر یا لینک اضافه نکن مگر مطمئن باشی واقعی و معتبر است. - در صورت شک داشتن به لینک، هیچ لینکی تولید نکن.";

	public ProductPrompt(WebProduct prd, string existsOnWp, string url, int maxTokens = 1000, double temperature = 0.7)
		: base(maxTokens, temperature)
	{
		if (string.IsNullOrWhiteSpace(prd?.Name))
		{
			throw new ArgumentException("نام محصول نمی\u200cتواند خالی باشد.");
		}
		_prd = prd;
		_existsOnWp = existsOnWp;
		_url = url;
		MaxTokens = maxTokens;
		Temperature = temperature;
	}

	public override string ToString()
	{
		return $"ProductPrompt: {_prd?.Name}, MaxTokens: {MaxTokens}, Temperature: {Temperature}";
	}
}
