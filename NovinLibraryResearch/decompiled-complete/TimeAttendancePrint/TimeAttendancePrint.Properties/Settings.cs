using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TimeAttendancePrint.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.13.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)(object)SettingsBase.Synchronized((SettingsBase)(object)new Settings());

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("select\t(select Val from SettingAttendance where name ='TitleSaleMali') as N'عنوان شرکت',\r\n\t\t(select Val from SettingAttendance where name ='CodeEkh') as N'کد اقتصادی',\r\n\t\t(select Val from SettingAttendance where name ='TelSH') as N'تلفن',\r\n\t\t(select Val from SettingAttendance where name ='AddSh') as N'آدرس',\r\n\t\t(select Val from SettingAttendance where name ='FaxSH') as N'فکس',\r\n\t\t(select Val from SettingAttendance where name ='DescSH') as N'شرح شرکت',\r\n        (select Val from SettingAttendance where name ='SabtNO') as N'شماره ثبت شرکت'")]
	public string SQLQuery_Company => (string)((SettingsBase)this)["SQLQuery_Company"];
}
