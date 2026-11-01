using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendanceDAL.Settings;

[Serializable]
[DoNotPruneType]
public class SaleKari
{
	public string TitleSalekari { get; set; }

	public string CodeEkhtesasi { get; set; }

	public string Tel { get; set; }

	public string Address { get; set; }

	public string Fax { get; set; }

	public string Desc { get; set; }

	public string SabtNo { get; set; }

	public DateTime DateM1 { get; set; }

	public DateTime DateM2 { get; set; }

	public async Task ImportDataFromOldSettingAsync(string contextGuid, CancellationToken token)
	{
		string text = await SettingAttendance.GetValueAsync(contextGuid, "TitleSaleMali", 0, token);
		if (text.Trim() == "")
		{
			text = " شرکت مهندسی " + ResourceManagement.TextBrandResourceManagement.CompanyName + " ";
		}
		TitleSalekari = text;
		text = await SettingAttendance.GetValueAsync(contextGuid, "CodeEkh", 0, token);
		if (text.Trim() == "")
		{
			text = "";
		}
		CodeEkhtesasi = text;
		text = await SettingAttendance.GetValueAsync(contextGuid, "TelSH", 0, token);
		if (text.Trim() == "")
		{
			text = ResourceManagement.TextBrandResourceManagement.Tel;
		}
		Tel = text;
		text = await SettingAttendance.GetValueAsync(contextGuid, "AddSh", 0, token);
		if (text.Trim() == "")
		{
			text = ResourceManagement.TextBrandResourceManagement.Address;
		}
		Address = text;
		text = await SettingAttendance.GetValueAsync(contextGuid, "FaxSH", 0, token);
		if (text.Trim() == "")
		{
			text = ResourceManagement.TextBrandResourceManagement.Fax;
		}
		Fax = text;
		text = await SettingAttendance.GetValueAsync(contextGuid, "DescSH", 0, token);
		if (text.Trim() == "")
		{
			text = "";
		}
		Desc = text;
		text = await SettingAttendance.GetValueAsync(contextGuid, "SabtNO", 0, token);
		if (text.Trim() == "")
		{
			text = "";
		}
		SabtNo = text;
	}
}
