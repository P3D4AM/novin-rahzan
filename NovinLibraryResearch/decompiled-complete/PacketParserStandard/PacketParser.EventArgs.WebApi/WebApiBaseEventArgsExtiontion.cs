using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs.WebApi;

public static class WebApiBaseEventArgsExtiontion
{
	public static WebApiBaseEventArgs<object> CastToNoKey<T>(this WebApiBaseEventArgs<T> item)
	{
		WebApiBaseEventArgs<object> webApiBaseEventArgs = new WebApiBaseEventArgs<object>();
		try
		{
			webApiBaseEventArgs.UserName = item.UserName;
			webApiBaseEventArgs.PassWord = item.PassWord;
			webApiBaseEventArgs.Key = null;
			webApiBaseEventArgs.ENSource = item.ENSource;
			webApiBaseEventArgs.AndroidIme = item.AndroidIme;
			webApiBaseEventArgs.CountInPage = item.CountInPage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CastToNoKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\WebApi\\WebApiBaseEventArgsExtiontion.cs", 21);
		}
		return webApiBaseEventArgs;
	}

	public static WebApiBaseEventArgs<T> CastToKeyNull<T>(this WebApiBaseEventArgs<object> item)
	{
		WebApiBaseEventArgs<T> webApiBaseEventArgs = new WebApiBaseEventArgs<T>();
		try
		{
			webApiBaseEventArgs.UserName = item.UserName;
			webApiBaseEventArgs.PassWord = item.PassWord;
			webApiBaseEventArgs.ENSource = item.ENSource;
			webApiBaseEventArgs.AndroidIme = item.AndroidIme;
			webApiBaseEventArgs.CountInPage = item.CountInPage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CastToKeyNull", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\WebApi\\WebApiBaseEventArgsExtiontion.cs", 35);
		}
		return webApiBaseEventArgs;
	}
}
