using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Business;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace FormCustom.ExtendedControls;

public class CustomeComboBox : ComboBox
{
	public async Task SetSelectedValueAsync(string contextGuid, string key, bool allowNull = false)
	{
		try
		{
			if (string.IsNullOrEmpty(key))
			{
				if (allowNull)
				{
					((ListControl)this).SelectedValue = null;
				}
				return;
			}
			string text = await LocalSettingBusiness.GetAsync(contextGuid, key);
			if (string.IsNullOrEmpty(text))
			{
				if (allowNull)
				{
					((ListControl)this).SelectedValue = Guid.Empty;
				}
				return;
			}
			DataTypeForDisplay dataTypeForDisplay = text.FromJson<DataTypeForDisplay>("SetSelectedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeComboBox.cs", 30);
			switch (dataTypeForDisplay.DataType)
			{
			case EnDataType.Short:
				((ListControl)this).SelectedValue = dataTypeForDisplay.Value.ParseToShort(0);
				break;
			case EnDataType.Int:
				((ListControl)this).SelectedValue = dataTypeForDisplay.Value.ParseToInt();
				break;
			case EnDataType.Long:
				((ListControl)this).SelectedValue = dataTypeForDisplay.Value.ParseToLong(0L);
				break;
			case EnDataType.Float:
				((ListControl)this).SelectedValue = dataTypeForDisplay.Value.ParseTofloat(0f, 3);
				break;
			case EnDataType.Double:
				((ListControl)this).SelectedValue = dataTypeForDisplay.Value.ParseToDouble(0.0, 3);
				break;
			case EnDataType.Decimal:
				((ListControl)this).SelectedValue = dataTypeForDisplay.Value.ParseToDecimal();
				break;
			case EnDataType.UniqueIdentifier:
				((ListControl)this).SelectedValue = Guid.Parse(dataTypeForDisplay.Value);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSelectedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeComboBox.cs", 42);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveSelecetdValueAsync(string contextGuid, string key, DataTypeForDisplay value)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (((ListControl)this).SelectedValue == null || value == null)
			{
				return res;
			}
			switch (value.DataType)
			{
			case EnDataType.Short:
				value.Value = ((short)((ListControl)this).SelectedValue).ToString();
				break;
			case EnDataType.Int:
				value.Value = ((int)((ListControl)this).SelectedValue).ToString();
				break;
			case EnDataType.Long:
				value.Value = ((long)((ListControl)this).SelectedValue).ToString();
				break;
			case EnDataType.Float:
				value.Value = ((float)((ListControl)this).SelectedValue).ToString();
				break;
			case EnDataType.Double:
				value.Value = ((double)((ListControl)this).SelectedValue).ToString();
				break;
			case EnDataType.Decimal:
				value.Value = ((decimal)((ListControl)this).SelectedValue).ToString();
				break;
			case EnDataType.UniqueIdentifier:
				value.Value = ((ListControl)this).SelectedValue.ToString();
				break;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await LocalSettingBusiness.SaveAsync(contextGuid, key, Json.ToStringJson(value), awaitOnSemaphore: true, 10), ReturnedState.None, "SaveSelecetdValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeComboBox.cs", 60);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveSelecetdValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeComboBox.cs", 64);
			res.AddReturnedValue(ex, "", "SaveSelecetdValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeComboBox.cs", 65);
		}
		return res;
	}
}
