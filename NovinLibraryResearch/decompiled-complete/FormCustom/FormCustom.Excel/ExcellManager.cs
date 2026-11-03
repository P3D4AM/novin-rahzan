using System;
using System.Windows.Forms;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.Excel;

public static class ExcellManager
{
	public static string EmptyName = "<فاقد نام>".GetTranslate();

	public static string GetValue(this DataGridView DGrid, int col, int row)
	{
		string result = "";
		try
		{
			if (row < 0 || col <= 0)
			{
				return result;
			}
			col--;
			if (DGrid[col, row].Value != null)
			{
				result = DGrid[col, row].Value.ToString().FixString().Trim();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Excel\\ExcellManager.cs", 22);
		}
		return result;
	}

	public static decimal GetDecimalValue(this DataGridView DGrid, int col, int row)
	{
		try
		{
			return DGrid.GetValue(col, row).RemoveNoNumbers().ParseToDecimal();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDecimalValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Excel\\ExcellManager.cs", 32);
		}
		return 0m;
	}
}
