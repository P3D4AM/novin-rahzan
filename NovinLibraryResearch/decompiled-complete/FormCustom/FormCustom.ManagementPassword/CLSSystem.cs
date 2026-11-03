using System;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.ManagementPassword;

public class CLSSystem
{
	private const string ReKey = "X9806539";

	public static dynamic GlobalPass
	{
		get
		{
			try
			{
				string text = CLSRegistry.Read("X9806539");
				if (string.IsNullOrEmpty(text))
				{
					text = "05137597590";
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GlobalPass", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\CLSSystem.cs", 24);
				return "05137597590";
			}
		}
		set
		{
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if ((!string.IsNullOrEmpty(value)))
				{
					CLSRegistry.Write("X9806539", value.ToString());
				}
				else
				{
					MessageBox.Show("رمز خالی است ، عملیات ثبت نشد", "خطا", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GlobalPass", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ManagementPassword\\CLSSystem.cs", 44);
			}
		}
	}

	public static dynamic GlobalManagerPass => "NP12001300";

	public static dynamic DifficultManagerPass => "1Jw6}8&£%et9$H*J4[-L";
}
