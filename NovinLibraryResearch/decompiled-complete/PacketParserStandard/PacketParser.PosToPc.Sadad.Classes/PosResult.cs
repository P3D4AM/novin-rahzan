using System;
using System.Reflection;
using PacketParser.Entities;

namespace PacketParser.PosToPc.Sadad.Classes;

public class PosResult : PosResultBase
{
	private string _pan;

	private string _responseCode;

	public string Amount { get; set; }

	public string ApprovalCode { get; set; }

	public string CardNo
	{
		get
		{
			return _pan;
		}
		set
		{
			_pan = value;
		}
	}

	public string MerchantId { get; set; }

	public string OrderId { get; set; }

	public string ProcessingCode { get; set; }

	public string ProcessingMessage { get; set; }

	public string ResponseCode
	{
		get
		{
			return _responseCode;
		}
		set
		{
			_responseCode = value;
		}
	}

	public string Rrn { get; set; }

	public string SaleId { get; set; }

	public string TerminalId { get; set; }

	public string TransactionDate { get; set; }

	public string TransactionNo { get; set; }

	public string TransactionTime { get; set; }

	public PosResult()
	{
		try
		{
			FieldInfo[] fields = GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				object obj = fieldInfo.GetValue(this);
				if (obj == null)
				{
					obj = "";
				}
				object obj2 = obj;
				if (Utility.IsNumericType(obj2))
				{
					obj2 = "0";
				}
				fieldInfo.SetValue(this, Convert.ChangeType(obj2, fieldInfo.FieldType));
			}
			PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanWrite)
				{
					object obj3 = propertyInfo.GetValue(this, null);
					if (obj3 == null)
					{
						obj3 = "";
					}
					object obj4 = obj3;
					if (Utility.IsNumericType(obj4))
					{
						obj4 = "0";
					}
					propertyInfo.SetValue(this, Convert.ChangeType(obj4, propertyInfo.PropertyType), null);
				}
			}
		}
		catch (Exception exception)
		{
			Utility.LogPcPos(exception);
			throw;
		}
	}

	public void SetResultMessage()
	{
		try
		{
			if (ResponseCode == "00")
			{
				ProcessingMessage = "عملیات با موفقیت انجام شد";
			}
			if (ResponseCode == "61")
			{
				ProcessingMessage = "محدودیت مبلغ";
			}
			if (ResponseCode == "03")
			{
				ProcessingMessage = "پذیرنده نامعتبر";
			}
			if (ResponseCode == "12")
			{
				ProcessingMessage = "تراکنش نا معتبر";
			}
			if (ResponseCode == "19")
			{
				ProcessingMessage = "دوباره سعی کنید";
			}
			if (ResponseCode == "25")
			{
				ProcessingMessage = "سریال پایانه تعریف نشده";
			}
			if (ResponseCode == "30")
			{
				ProcessingMessage = "فرمت اشتباه (با پشتیبانی سداد تماس بگیرید)";
			}
			if (ResponseCode == "55")
			{
				ProcessingMessage = "رمز اشتباه";
			}
			if (ResponseCode == "75")
			{
				ProcessingMessage = "رمز اشتباه بیش از حد مجاز";
			}
			if (ResponseCode == "51")
			{
				ProcessingMessage = "موجودی کافی نیست";
			}
			if (ResponseCode == "57")
			{
				ProcessingMessage = "کارت مقصد نامعتبر";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetResultMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\Logic\\PosResult.cs", 79);
		}
	}
}
