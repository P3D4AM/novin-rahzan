using System;
using System.Drawing;
using System.Linq;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace FormCustom;

public class clsShowError
{
	public string Text { get; private set; }

	public string Title { get; private set; }

	public string FullText { get; private set; }

	public enType Type { get; set; }

	public Image Icon { get; set; }

	public string MoreDetail { get; set; } = "...";


	public static ENSource Source { get; set; }

	public void SetText(string value)
	{
		try
		{
			FullText = value;
			ExceptionMessageParser exceptionMessageParser = ExceptionMessageParser.Get(value);
			if (!string.IsNullOrEmpty(exceptionMessageParser?.PersianMessageForUi))
			{
				Text = exceptionMessageParser.PersianMessageForUi;
			}
			else
			{
				Text = value.GetTranslate();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetText", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\clsShowError.cs", 30);
		}
	}

	public void SetTitle(string value)
	{
		try
		{
			ExceptionMessageParser exceptionMessageParser = ExceptionMessageParser.Get(value);
			if (!string.IsNullOrEmpty(exceptionMessageParser?.PersianMessageForUi))
			{
				value = exceptionMessageParser.PersianMessageForUi;
			}
			string text = value;
			if (!string.IsNullOrEmpty(value))
			{
				text = value.GetTranslate() ?? "";
			}
			if (text.Length > 500)
			{
				text = text.Substring(0, 500);
			}
			string[] source = text.Replace("\r", "").Split(new char[1] { '\n' });
			source = source.Where((string q) => !string.IsNullOrEmpty(q)).ToArray();
			if (source.Count() > 5)
			{
				text = string.Join("\r\n", source.Take(5));
			}
			Title = text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTitle", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\clsShowError.cs", 47);
		}
	}
}
