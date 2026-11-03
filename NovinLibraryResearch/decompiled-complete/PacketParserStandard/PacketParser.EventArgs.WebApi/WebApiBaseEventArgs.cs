using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs.WebApi;

public class WebApiBaseEventArgs<T>
{
	public string UserName { get; set; }

	public string PassWord { get; set; }

	public ENSource ENSource { get; set; }

	public long AndroidIme { get; set; }

	public short CountInPage { get; set; }

	public short Pages { get; set; }

	public T Key { get; set; }

	public WebApiBaseEventArgs()
	{
	}

	public WebApiBaseEventArgs(T key, string userName, string password, ENSource eNSource = ENSource.Accounting, long androidIme = 0L, short countInPage = 100, short pages = 1)
	{
		try
		{
			UserName = userName;
			PassWord = password;
			Key = key;
			ENSource = eNSource;
			AndroidIme = androidIme;
			CountInPage = countInPage;
			Pages = pages;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\WebApi\\WebApiBaseEventArgs.cs", 32);
		}
	}
}
