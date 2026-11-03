using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Issabel;

namespace FormCustom.Activation;

public class ActivationSettings
{
	private string sqlServerDatabaseConnectionString;

	public string SqlServerDatabaseConnectionString
	{
		get
		{
			return sqlServerDatabaseConnectionString.FixSqlServerConnectionString();
		}
		set
		{
			sqlServerDatabaseConnectionString = value;
		}
	}

	public string ApiHostAddress { get; set; }

	public string ApiUser { get; set; }

	public string ApiPass { get; set; }

	public string SignalrHostAddress { get; set; }

	public string SignalrUser { get; set; }

	public string SignalrPass { get; set; }

	public IsabelleClientSettings IsabelleClientSettings { get; set; }

	public static string FileName => Path.Combine(Application.StartupPath, "ActivationSettings.json");

	public static ActivationSettings Get(string contextGuid, bool displayUiIfSettingIsNull = true)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		try
		{
			if (!File.Exists(FileName))
			{
				if (!displayUiIfSettingIsNull)
				{
					return null;
				}
				if ((int)((Form)new FrmPublicSetting(contextGuid)).ShowDialog() == 1)
				{
					return File.ReadAllText(FileName).DecryptString().FromJson<ActivationSettings>("Get", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationSettings.cs", 40);
				}
				return null;
			}
			ActivationSettings activationSettings = File.ReadAllText(FileName).DecryptString().FromJson<ActivationSettings>("Get", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationSettings.cs", 46);
			try
			{
				((DbConnection)new SqlConnection(activationSettings.SqlServerDatabaseConnectionString)).Open();
			}
			catch (Exception)
			{
				activationSettings.SqlServerDatabaseConnectionString = "Data Source=192.168.1.10;Initial Catalog=Activation;User ID=NP;Password=N@vin@051$Pardaz#;MultipleActiveResultSets=True;TrustServerCertificate=True";
				((DbConnection)new SqlConnection(activationSettings.SqlServerDatabaseConnectionString)).Open();
				activationSettings.CreateSetting();
			}
			if (string.IsNullOrEmpty(activationSettings?.SqlServerDatabaseConnectionString))
			{
				throw new Exception("SqlServerDatabaseConnectionString for activation setting is null or empty / jsonFilePath:" + FileName);
			}
			return activationSettings;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationSettings.cs", 64);
			((Form)new FrmPublicSetting(contextGuid)).ShowDialog();
			return File.ReadAllText(FileName).DecryptString().FromJson<ActivationSettings>("Get", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationSettings.cs", 67);
		}
	}

	public ReturnedSaveFuncInfo CreateSetting()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			File.WriteAllText(FileName, Json.ToStringJson(this, indented: true).EncryptData());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateSetting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationSettings.cs", 79);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CreateSetting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Activation\\ActivationSettings.cs", 80);
		}
		return returnedSaveFuncInfo;
	}
}
