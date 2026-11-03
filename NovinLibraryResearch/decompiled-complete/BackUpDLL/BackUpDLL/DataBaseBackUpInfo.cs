using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace BackUpDLL;

internal class DataBaseBackUpInfo
{
	public string BackUpName { get; set; } = "";


	public string BackUpDescription { get; set; } = "";


	public string BackUpType { get; set; } = "";


	public string ExpirationDate { get; set; } = "";


	public string Compressed { get; set; } = "";


	public string Position { get; set; } = "";


	public string DeviceType { get; set; } = "";


	public string UserName { get; set; } = "";


	public string ServerName { get; set; } = "";


	public string DatabaseName { get; set; } = "";


	public string DatabaseVersion { get; set; } = "";


	public string DatabaseCreationDate { get; set; } = "";


	public string FristLSN { get; set; } = "";


	public string LastLSN { get; set; } = "";


	public string CheckPointLSN { get; set; } = "";


	public string DatabaseBackUpLSN { get; set; } = "";


	public string BackUpstartdate { get; set; } = "";


	public string SortOrder { get; set; } = "";


	public string CodePage { get; set; } = "";


	public string UnicodeLocaleId { get; set; } = "";


	public string UniCodeComparisonStyle { get; set; } = "";


	public string CompatibilityLevel { get; set; } = "";


	public string SoftwareVendoId { get; set; } = "";


	public string SoftwareVersionMajor { get; set; } = "";


	public string MachineName { get; set; } = "";


	public string Flags { get; set; } = "";


	public string BindingId { get; set; } = "";


	public string RecoveryForkId { get; set; } = "";


	public string Collation { get; set; } = "";


	public string FamilyGuid { get; set; } = "";


	public string HasBulkLoggeddata { get; set; } = "";


	public string IsSnapshot { get; set; } = "";


	public string IsReadOnly { get; set; } = "";


	public string IsSingleUser { get; set; } = "";


	public string HasBackUpChecksums { get; set; } = "";


	public string IsDamaged { get; set; } = "";


	public string BeginsLogChain { get; set; } = "";


	public string HasIncompleteMetaData { get; set; } = "";


	public string IsForcedOffline { get; set; } = "";


	public string IsCopyOnly { get; set; } = "";


	public string FirstRecoveryForkId { get; set; } = "";


	public string ForkPointLSN { get; set; } = "";


	public string RecoveryModel { get; set; } = "";


	public string DifferentialBaseLSN { get; set; } = "";


	public string DifferentialBaseGuid { get; set; } = "";


	public string BackupTypeDescription { get; set; } = "";


	public string BackupSetGuid { get; set; } = "";


	public string LogicalName { get; set; } = "";


	public static Task<ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo>> GetAsync(string backUpAddress, string connectionString)
	{
		return LoadDataAsync(backUpAddress, connectionString);
	}

	private static ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo> LoadData(SqlDataReader reader)
	{
		ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo>();
		try
		{
			returnedSaveFuncInfoWithValue.value = new DataBaseBackUpInfo();
			returnedSaveFuncInfoWithValue.value.BackUpName = ((DbDataReader)(object)reader)[0].ToString();
			returnedSaveFuncInfoWithValue.value.BackUpDescription = ((DbDataReader)(object)reader)[1].ToString();
			returnedSaveFuncInfoWithValue.value.BackUpType = ((DbDataReader)(object)reader)[2].ToString();
			returnedSaveFuncInfoWithValue.value.ExpirationDate = ((DbDataReader)(object)reader)[3].ToString();
			returnedSaveFuncInfoWithValue.value.Compressed = ((DbDataReader)(object)reader)[4].ToString();
			returnedSaveFuncInfoWithValue.value.Position = ((DbDataReader)(object)reader)[5].ToString();
			returnedSaveFuncInfoWithValue.value.DeviceType = ((DbDataReader)(object)reader)[6].ToString();
			returnedSaveFuncInfoWithValue.value.UserName = ((DbDataReader)(object)reader)[7].ToString();
			returnedSaveFuncInfoWithValue.value.ServerName = ((DbDataReader)(object)reader)[8].ToString();
			returnedSaveFuncInfoWithValue.value.DatabaseName = ((DbDataReader)(object)reader)[9].ToString();
			returnedSaveFuncInfoWithValue.value.DatabaseVersion = ((DbDataReader)(object)reader)[10].ToString();
			returnedSaveFuncInfoWithValue.value.DatabaseCreationDate = ((DbDataReader)(object)reader)[11].ToString();
			returnedSaveFuncInfoWithValue.value.FristLSN = ((DbDataReader)(object)reader)[12].ToString();
			returnedSaveFuncInfoWithValue.value.LastLSN = ((DbDataReader)(object)reader)[13].ToString();
			returnedSaveFuncInfoWithValue.value.CheckPointLSN = ((DbDataReader)(object)reader)[14].ToString();
			returnedSaveFuncInfoWithValue.value.DatabaseBackUpLSN = ((DbDataReader)(object)reader)[15].ToString();
			returnedSaveFuncInfoWithValue.value.BackUpstartdate = ((DbDataReader)(object)reader)[16].ToString();
			returnedSaveFuncInfoWithValue.value.SortOrder = ((DbDataReader)(object)reader)[17].ToString();
			returnedSaveFuncInfoWithValue.value.CodePage = ((DbDataReader)(object)reader)[18].ToString();
			returnedSaveFuncInfoWithValue.value.UnicodeLocaleId = ((DbDataReader)(object)reader)[19].ToString();
			returnedSaveFuncInfoWithValue.value.UniCodeComparisonStyle = ((DbDataReader)(object)reader)[20].ToString();
			returnedSaveFuncInfoWithValue.value.CompatibilityLevel = ((DbDataReader)(object)reader)[21].ToString();
			returnedSaveFuncInfoWithValue.value.SoftwareVendoId = ((DbDataReader)(object)reader)[22].ToString();
			returnedSaveFuncInfoWithValue.value.SoftwareVersionMajor = ((DbDataReader)(object)reader)[23].ToString();
			returnedSaveFuncInfoWithValue.value.MachineName = ((DbDataReader)(object)reader)[24].ToString();
			returnedSaveFuncInfoWithValue.value.Flags = ((DbDataReader)(object)reader)[25].ToString();
			returnedSaveFuncInfoWithValue.value.BindingId = ((DbDataReader)(object)reader)[26].ToString();
			returnedSaveFuncInfoWithValue.value.RecoveryForkId = ((DbDataReader)(object)reader)[27].ToString();
			returnedSaveFuncInfoWithValue.value.Collation = ((DbDataReader)(object)reader)[28].ToString();
			returnedSaveFuncInfoWithValue.value.FamilyGuid = ((DbDataReader)(object)reader)[29].ToString();
			returnedSaveFuncInfoWithValue.value.HasBulkLoggeddata = ((DbDataReader)(object)reader)[30].ToString();
			returnedSaveFuncInfoWithValue.value.IsSnapshot = ((DbDataReader)(object)reader)[31].ToString();
			returnedSaveFuncInfoWithValue.value.IsReadOnly = ((DbDataReader)(object)reader)[32].ToString();
			returnedSaveFuncInfoWithValue.value.IsSingleUser = ((DbDataReader)(object)reader)[33].ToString();
			returnedSaveFuncInfoWithValue.value.HasBackUpChecksums = ((DbDataReader)(object)reader)[34].ToString();
			returnedSaveFuncInfoWithValue.value.IsDamaged = ((DbDataReader)(object)reader)[35].ToString();
			returnedSaveFuncInfoWithValue.value.BeginsLogChain = ((DbDataReader)(object)reader)[36].ToString();
			returnedSaveFuncInfoWithValue.value.HasIncompleteMetaData = ((DbDataReader)(object)reader)[37].ToString();
			returnedSaveFuncInfoWithValue.value.IsForcedOffline = ((DbDataReader)(object)reader)[38].ToString();
			returnedSaveFuncInfoWithValue.value.IsCopyOnly = ((DbDataReader)(object)reader)[39].ToString();
			returnedSaveFuncInfoWithValue.value.FirstRecoveryForkId = ((DbDataReader)(object)reader)[40].ToString();
			returnedSaveFuncInfoWithValue.value.ForkPointLSN = ((DbDataReader)(object)reader)[41].ToString();
			returnedSaveFuncInfoWithValue.value.RecoveryModel = ((DbDataReader)(object)reader)[42].ToString();
			returnedSaveFuncInfoWithValue.value.DifferentialBaseLSN = ((DbDataReader)(object)reader)[43].ToString();
			returnedSaveFuncInfoWithValue.value.DifferentialBaseGuid = ((DbDataReader)(object)reader)[44].ToString();
			returnedSaveFuncInfoWithValue.value.BackupTypeDescription = ((DbDataReader)(object)reader)[45].ToString();
			returnedSaveFuncInfoWithValue.value.BackupSetGuid = ((DbDataReader)(object)reader)[46].ToString();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseBackUpInfo.cs", 75);
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseBackUpInfo.cs", 76);
		}
		return returnedSaveFuncInfoWithValue;
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo>> LoadDataAsync(string backUpAddress, string connectionString)
	{
		ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo> ret = new ReturnedSaveFuncInfoWithValue<DataBaseBackUpInfo>();
		string command = "";
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				command = "RESTORE HEADERONLY FROM DISK =N'" + backUpAddress + "'";
				SqlCommand val = new SqlCommand(command, cn);
				((DbCommand)val).CommandType = CommandType.Text;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync())
						{
							ret = LoadData(dr);
						}
						command = (((DbCommand)(object)cmd).CommandText = "RESTORE FILELISTONLY FROM DISK = N'" + backUpAddress + "'");
						((DbDataReader)(object)dr).Close();
						SqlDataReader dr2 = await cmd.ExecuteReaderAsync();
						try
						{
							await ((DbDataReader)(object)dr2).ReadAsync();
							ret.value.LogicalName = ((DbDataReader)(object)dr2)[0].ToString();
							((DbDataReader)(object)dr2).Close();
							((DbConnection)(object)cn).Close();
						}
						finally
						{
							((IDisposable)dr2)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("Operating system error 5(Access is denied.)"))
			{
				ret.AddError(EnSystem.None, "سرویس اس کیو ال دسترسی به آدرس انتخاب شده ندارد.\r\nلطفا بکاپ را از آدرس معتبر دیگری غیر از موارد ذیل انتخاب کنید:\r\nDesctop\r\nDownload\r\nریشه درایو ها بخصوص درایو سیستم عامل(عموما درایو C)\r\nپیشنهاد میشود بکاپ از داخل یک پوشه واقع در یکی از درایو ها بازیابی گردد.\r\nCommand: " + command, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseBackUpInfo.cs", 199);
			}
			ret.AddReturnedValue(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseBackUpInfo.cs", 206);
			if (!string.IsNullOrEmpty(command))
			{
				ret.AddError(command, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseBackUpInfo.cs", 208);
			}
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\DataBaseBackUpInfo.cs", 209);
		}
		return ret;
	}
}
