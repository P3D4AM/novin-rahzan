using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models;

public class ExceptionMessageParser
{
	private static Dictionary<string, string> errorsForUser = new Dictionary<string, string>
	{
		{
			"OutOfMemory".ToLower(),
			"حافضه رم کافی وجود ندارد. لطفا برنامه های بلا استفاده رو ببندید و در صورت تکرار خطا نسبت به ارتقاء مشخصات سخت افزاری اقدام نمایید."
		},
		{
			"Not enough storage is available to process this command".ToLower(),
			"حافضه دیسک کافی وجود ندارد؛ در صورتی که اطمینان دارید فضای فاکی دیسک روی این سیستم وجود دارد حجم بانک اطلاعات و فضای دیسک سیستم سرور را بررسی فرمایید."
		},
		{
			"Access is denied".ToLower(),
			"دسترسی به فایل وجود ندارد؛ لطفا دسترسی لازمه را به برنامه بدهید."
		},
		{
			"Connection Timeout Expired".ToLower(),
			"خطا در برقراری ارتباط با بانک اطلاعاتی - از استارت بودن سرویس sql اطمینان حاصل فرمایید. اطمینان پیدا کنید که در صورتی که از سرویس sql در شبکه استفاده میکنید ارتباط با سرور برقرار است."
		},
		{
			"Access to the path".ToLower(),
			"دسترسی به فایل وجود ندارد؛ لطفا دسترسی لازمه را به برنامه بدهید."
		},
		{
			"There is not enough space on the disk.".ToLower(),
			"حافضه دیسک کافی وجود ندارد؛ لطفا نسبت به ارتقاء هارد دیسک خود یا حذف فایل های اضافی اقدام نمایید."
		},
		{
			"Could not allocate a new page for database.".ToLower(),
			"فضای کافی دیسک جهت بانک اطلاعاتی وجود ندارد. ممکن است حجم بانک اطلاعات از حد مرز مجاز 4 گیگ تجاوز کرده باشد"
		},
		{
			"insufficient disk space in filegroup.".ToLower(),
			"فضای کافی دیسک جهت بانک اطلاعاتی وجود ندارد. ممکن است حجم بانک اطلاعات از حد مرز مجاز 4 گیگ تجاوز کرده باشد"
		},
		{
			"RPC server is unavailable".ToLower(),
			"پرینتر در دسترس نیست؛ در صورتی که از پرینتر شبکه استفاده میکنید از روشن بودن آن اطمینان حاصل فرمایید."
		},
		{
			"The transaction log for database 'tempdb' is full.".ToLower(),
			"فضای کافی برای افزایش حجم دیتابیس روی هارد شما وجود ندارد."
		},
		{
			"does not support style 'Regular'".ToLower(),
			"خطای فونت"
		},
		{
			"The remote name could not be resolved: 'npsms.ir'".ToLower(),
			"عدم دسترسی به اینترنت"
		},
		{
			"The 'Microsoft.ACE.OLEDB.12.0' provider is not registered".ToLower(),
			"انجین اکسس نصب نشده است"
		},
		{
			"The Microsoft Access database engine cannot open or write to the file".ToLower(),
			"فایل اکسل مورد نظر جایی دیگر در حال ویرایش است یا اینکه شما دسترسی ویرایش ندارید"
		},
		{
			"because it does not exist or you do not have permissions".ToLower(),
			"شیء مورد نظر وجود ندارد یا فاقد دسترسی است"
		},
		{
			"not enough space".ToLower(),
			"فضای دیسک پر شده است"
		},
		{
			"Disk full".ToLower(),
			"فضای دیسک پر شده است"
		},
		{
			"Not enough storage".ToLower(),
			"فضای دیسک پر شده است"
		},
		{
			"Failed to allocate a".ToLower(),
			"فضای رم کافی وجود ندارد"
		},
		{
			"xception of type 'System.OutOfMemoryException".ToLower(),
			"فضای رم کافی وجود ندارد"
		},
		{
			"Java.Lang.Error: OutOfMemoryError".ToLower(),
			"فضای رم کافی روی گوشی اندروید وجود ندارد"
		},
		{
			"disk I/O error".ToLower(),
			"فضای رم کافی روی گوشی اندروید وجود ندارد"
		},
		{
			"Could not load file or assembly".ToLower(),
			"فایل وجود ندارد"
		},
		{
			"does not support style".ToLower(),
			"فونت مورد نیاز نصب نیست"
		},
		{
			"Could not allocate a new page for database".ToLower(),
			"فضای کافی برای دیتابیس وجود ندارد"
		},
		{
			"The media is write protected".ToLower(),
			"دیتابیس در وضعیت عادی نیست"
		},
		{
			"file not found".ToLower(),
			"فایل پیدا نشد"
		},
		{
			"token is Cancelled".ToLower(),
			"عدم دریافت پاسخ در زمان مقرر"
		},
		{
			"Token has been cancelled".ToLower(),
			"عدم دریافت پاسخ در زمان مقرر"
		}
	};

	public string PersianMessageForUi { get; set; }

	public Exception Exception { get; set; }

	public static ExceptionMessageParser Get(Exception ex)
	{
		try
		{
			ExceptionMessageParser exceptionMessageParser = new ExceptionMessageParser
			{
				Exception = ex
			};
			foreach (string key in errorsForUser.Keys)
			{
				if (ex.Message.ToLower().Contains(key) && errorsForUser.TryGetValue(key, out var value))
				{
					exceptionMessageParser.PersianMessageForUi = value;
					return exceptionMessageParser;
				}
			}
			if (ex.InnerException != null)
			{
				return Get(ex.InnerException);
			}
			if (ex.Message.ToLower().Contains("Access to the registry key") && ex.Message.ToLower().Contains("is denied"))
			{
				exceptionMessageParser.PersianMessageForUi = "دسترسی به رجیستری برای برنامه وجود ندارد.";
			}
			return exceptionMessageParser;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\ExceptionMessageParser.cs", 72);
			throw;
		}
	}

	public static ExceptionMessageParser Get(string exceptionMessage)
	{
		try
		{
			ExceptionMessageParser exceptionMessageParser = new ExceptionMessageParser();
			foreach (string key in errorsForUser.Keys)
			{
				if (exceptionMessage.ToLower().Contains(key) && errorsForUser.TryGetValue(key, out var value))
				{
					exceptionMessageParser.PersianMessageForUi = value;
					return exceptionMessageParser;
				}
			}
			if (exceptionMessage.ToLower().Contains("Access to the registry key") && exceptionMessage.ToLower().Contains("is denied"))
			{
				exceptionMessageParser.PersianMessageForUi = "دسترسی به رجیستری برای برنامه وجود ندارد.";
			}
			return exceptionMessageParser;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\Models\\ExceptionMessageParser.cs", 96);
			throw;
		}
	}
}
