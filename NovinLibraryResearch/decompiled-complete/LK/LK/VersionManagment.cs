using System;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace LK;

public class VersionManagment
{
	private static Level_Accounting _Code;

	public static ReturnedSaveFuncInfoWithValue<Level_Accounting> Code(ILkUiFactory owner, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<Level_Accounting>();
		try
		{
			if (_Code == Level_Accounting.none)
			{
				ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue2 = CLSActivation.ApplicationType(owner, ignoreHttpExceptions);
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "Code", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\VersionManagment.cs", 32);
				_Code = returnedSaveFuncInfoWithValue2.value;
			}
			returnedSaveFuncInfoWithValue.value = _Code;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Code", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\VersionManagment.cs", 39);
			returnedSaveFuncInfoWithValue.value = Level_Accounting.none;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Code", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\VersionManagment.cs", 41);
		}
		return returnedSaveFuncInfoWithValue;
	}
}
