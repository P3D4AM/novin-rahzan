using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TimeAttendanceDAL.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.13.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)(object)SettingsBase.Synchronized((SettingsBase)(object)new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("ALTER DATABASE [dbName] SET ARITHABORT ON WITH NO_WAIT\r\nALTER DATABASE [dbName] SET AUTO_CLOSE ON WITH NO_WAIT\r\nALTER DATABASE [dbName] SET AUTO_SHRINK ON WITH NO_WAIT\r\nALTER DATABASE [dbName] SET CURSOR_CLOSE_ON_COMMIT ON WITH NO_WAIT\r\nALTER DATABASE [dbName] SET CURSOR_DEFAULT  LOCAL WITH NO_WAIT\r\nALTER DATABASE [dbName] SET AUTO_UPDATE_STATISTICS_ASYNC ON WITH NO_WAIT")]
	public string DatabaseSettings => (string)((SettingsBase)this)["DatabaseSettings"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(/*Could not decode attribute arguments.*/)]
	[DefaultSettingValue("Data Source=.\\mssql;Initial Catalog=Attendance1;User ID=sa;Password=05137597590")]
	public string Attendance1ConnectionString => (string)((SettingsBase)this)["Attendance1ConnectionString"];

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("select\t(select Val from SettingAttendance where name ='TitleSaleMali') as N'عنوان شرکت',\r\n\t\t(select Val from SettingAttendance where name ='CodeEkh') as N'کد اقتصادی',\r\n\t\t(select Val from SettingAttendance where name ='TelSH') as N'تلفن',\r\n\t\t(select Val from SettingAttendance where name ='AddSh') as N'آدرس',\r\n\t\t(select Val from SettingAttendance where name ='FaxSH') as N'فکس',\r\n\t\t(select Val from SettingAttendance where name ='DescSH') as N'شرح شرکت',\r\n        (select Val from SettingAttendance where name ='SabtNO') as N'شماره ثبت شرکت'")]
	public string SQLQuery_Company
	{
		get
		{
			return (string)((SettingsBase)this)["SQLQuery_Company"];
		}
		set
		{
			((SettingsBase)this)["SQLQuery_Company"] = value;
		}
	}

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(/*Could not decode attribute arguments.*/)]
	[DefaultSettingValue("Data Source=.\\mssql;Initial Catalog=Attendance5;Persist Security Info=True;User ID=sa;Password=05137597590")]
	public string Attendance5ConnectionString => (string)((SettingsBase)this)["Attendance5ConnectionString"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(/*Could not decode attribute arguments.*/)]
	[DefaultSettingValue("Data Source=.\\mssql;Initial Catalog=Attendance6;Persist Security Info=True;User ID=sa;Password=05137597590")]
	public string Attendance6ConnectionString => (string)((SettingsBase)this)["Attendance6ConnectionString"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(/*Could not decode attribute arguments.*/)]
	[DefaultSettingValue("Data Source=.\\mssql;Initial Catalog=Attendance1;Integrated Security=True")]
	public string Attendance1ConnectionString1 => (string)((SettingsBase)this)["Attendance1ConnectionString1"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(/*Could not decode attribute arguments.*/)]
	[DefaultSettingValue("Data Source=SOVEYZI-PC\\MSSQL;Initial Catalog=Attendance3;Integrated Security=True")]
	public string Attendance3ConnectionString => (string)((SettingsBase)this)["Attendance3ConnectionString"];

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[SpecialSetting(/*Could not decode attribute arguments.*/)]
	[DefaultSettingValue("Data Source=.\\mssql;Initial Catalog=Attendance5;Integrated Security=True")]
	public string Attendance5ConnectionString1 => (string)((SettingsBase)this)["Attendance5ConnectionString1"];
}
