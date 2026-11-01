using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace Tele;

internal class ATReciveDataSpliter
{
	private List<string> _CommandList = null;

	public List<string> CommandList => _CommandList;

	public ATReciveDataSpliter(string ATRecivedCommand)
	{
		try
		{
			_CommandList = new List<string>();
			string[] array = ATRecivedCommand.Split('\r', '\n', '~', '\u0010', '\u0010', '/');
			if (array.Length == 0)
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != "")
				{
					_CommandList.Add(array[i]);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\Tele\\Tele\\ATReciveDataSpliter.cs", 38);
		}
	}
}
