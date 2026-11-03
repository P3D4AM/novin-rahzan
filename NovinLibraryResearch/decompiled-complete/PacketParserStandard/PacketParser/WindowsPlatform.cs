using System;
using System.Security.Principal;
using PacketParser.Entities;

namespace PacketParser;

public class WindowsPlatform : IWindowsPlatform
{
	public string GetUsername()
	{
		try
		{
			string name = WindowsIdentity.GetCurrent().Name;
			string[] array = name.Split(new char[1] { '\\' });
			if (array.Length > 1)
			{
				return array[1];
			}
			return name;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUsername", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WindowsPlatform.cs", 24);
			return "cant get user error:" + ex.Message;
		}
	}
}
