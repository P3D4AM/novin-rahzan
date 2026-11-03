using System;
using System.Windows.Forms;
using PacketParser.Assistance;
using PacketParser.Entities;

namespace FormCustom;

public class TextManager
{
	public static void SetNumber(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode >= 96 && (int)e.KeyCode <= 105) || ((int)e.KeyCode >= 48 && (int)e.KeyCode <= 57 && !e.Shift) || (int)e.KeyCode == 13 || (int)e.KeyCode == 38 || (int)e.KeyCode == 40 || (int)e.KeyCode == 8 || (int)e.KeyCode == 46 || (int)e.KeyCode == 39 || (int)e.KeyCode == 37)
			{
				e.SuppressKeyPress = false;
			}
			else
			{
				e.SuppressKeyPress = true;
			}
			if ((int)e.KeyCode == 187 || (int)e.KeyCode == 107)
			{
				double num = 0.0;
				TextBox val = (TextBox)((sender is TextBox) ? sender : null);
				if (val != null)
				{
					num = double.Parse(((Control)val).Text.Replace(NumberManager.NumberGroupingFormat, "") + "00");
					if (num < 1000000000000.0)
					{
						((Control)val).Text = num.ToString();
						((TextBoxBase)val).SelectionStart = 99;
					}
				}
				else
				{
					ComboBox val2 = (ComboBox)((sender is ComboBox) ? sender : null);
					if (val2 != null)
					{
						num = double.Parse(((Control)val2).Text.Replace(NumberManager.NumberGroupingFormat, "") + "00");
						if (num < 1000000000000.0)
						{
							((Control)val2).Text = num.ToString();
							val2.SelectionStart = 99;
						}
					}
				}
			}
			if ((int)e.KeyCode != 189 && (int)e.KeyCode != 109)
			{
				return;
			}
			double num2 = 0.0;
			TextBox val3 = (TextBox)((sender is TextBox) ? sender : null);
			if (val3 != null)
			{
				num2 = double.Parse(((Control)val3).Text.Replace(NumberManager.NumberGroupingFormat, "") + "000");
				if (num2 < 1000000000000.0)
				{
					((Control)val3).Text = num2.ToString();
					((TextBoxBase)val3).SelectionStart = 99;
				}
				return;
			}
			ComboBox val4 = (ComboBox)((sender is ComboBox) ? sender : null);
			if (val4 != null)
			{
				num2 = double.Parse(((Control)val4).Text.Replace(NumberManager.NumberGroupingFormat, "") + "000");
				if (num2 < 1000000000000.0)
				{
					((Control)val4).Text = num2.ToString();
					val4.SelectionStart = 99;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\TextManager.cs", 67);
		}
	}

	public static void SetQuantity(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode >= 96 && (int)e.KeyCode <= 105) || ((int)e.KeyCode >= 48 && (int)e.KeyCode <= 57 && !e.Shift) || (int)e.KeyCode == 13 || (int)e.KeyCode == 38 || (int)e.KeyCode == 40 || (int)e.KeyCode == 8 || (int)e.KeyCode == 46 || (int)e.KeyCode == 39 || (int)e.KeyCode == 37 || (int)e.KeyCode == 110 || (int)e.KeyCode == 190)
			{
				e.SuppressKeyPress = false;
			}
			else
			{
				e.SuppressKeyPress = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetQuantity", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\TextManager.cs", 89);
		}
	}

	public static void SetFloat(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Invalid comparison between Unknown and I4
		try
		{
			if (((int)e.KeyCode >= 96 && (int)e.KeyCode <= 105) || ((int)e.KeyCode >= 48 && (int)e.KeyCode <= 57 && !e.Shift) || (int)e.KeyCode == 13 || (int)e.KeyCode == 38 || (int)e.KeyCode == 40 || (int)e.KeyCode == 8 || (int)e.KeyCode == 46 || (int)e.KeyCode == 39 || (int)e.KeyCode == 37 || (int)e.KeyCode == 190 || (int)e.KeyCode == 110 || (int)e.KeyCode == 191)
			{
				e.SuppressKeyPress = false;
			}
			else
			{
				e.SuppressKeyPress = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFloat", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\TextManager.cs", 112);
		}
	}

	public static bool IsNumber(KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		try
		{
			return ((int)e.KeyCode >= 96 && (int)e.KeyCode <= 105) || ((int)e.KeyCode >= 48 && (int)e.KeyCode <= 57 && !e.Shift);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\TextManager.cs", 124);
			return false;
		}
	}
}
