using System.IO;
using PacketParser;
using PacketParser.Entities.Attendance;
using Stimulsoft.Report;
using TimeAttendancePrint.Properties;

namespace TimeAttendancePrint.SetSTI;

public class ClsGeneral
{
	private EnumPrintShow _printShowType;

	public string FolderName => "حضور-غیاب و حقوق-دستمزد";

	public StiReport GetSti(StiReport sti, Print.EnPrinterType paper, EnumPrintShow printShowType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected I4, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected I4, but got Unknown
		_printShowType = printShowType;
		ReportPath.CreatePath(ReportPath.ReportPath_Renamed);
		ReportPath.CreatePath(ReportPath.ReportPath_Renamed + "\\" + FolderName);
		string text = ReportPath.ReportPath_Renamed + "\\" + FolderName + "\\" + Utilities.GetAttReportFileName<EnumPrintShow>(_printShowType) + ".mrt";
		if (paper == Print.EnPrinterType.AllA5Printers)
		{
			text = ReportPath.ReportPath_Renamed + "\\" + FolderName + "\\" + Utilities.GetAttReportFileName<EnumPrintShow>(_printShowType).TrimEnd(new char[1] { '4' }) + "5.mrt";
		}
		if (!File.Exists(text))
		{
			switch (paper)
			{
			case Print.EnPrinterType.AllA4Printers:
				switch (printShowType - 1)
				{
				case 0:
					File.WriteAllBytes(text, Resources.Taradodkamel_A4);
					break;
				case 1:
					File.WriteAllBytes(text, Resources.Personel_A4);
					break;
				case 2:
					File.WriteAllBytes(text, Resources.listFish_A4);
					break;
				case 3:
					File.WriteAllBytes(text, Resources.Listasnad_A4);
					break;
				case 4:
					File.WriteAllBytes(text, Resources.Sanad_A4);
					break;
				case 5:
					File.WriteAllBytes(text, Resources.Fish_A4);
					break;
				case 6:
					File.WriteAllBytes(text, Resources.Holiday_A4);
					break;
				case 7:
					File.WriteAllBytes(text, Resources.TaradodJame_A4);
					break;
				case 8:
					File.WriteAllBytes(text, Resources.ListGharardad_A4);
					break;
				case 9:
					File.WriteAllBytes(text, Resources.Gharadad_A4);
					break;
				case 10:
					File.WriteAllBytes(text, Resources.TaradodKolPerson_A4);
					break;
				case 11:
					File.WriteAllBytes(text, Resources.MorakhasiDaily_A4);
					break;
				case 12:
					File.WriteAllBytes(text, Resources.AllTaradod_A4);
					break;
				case 13:
					File.WriteAllBytes(text, Resources.InsuranceList_A4);
					break;
				case 14:
					File.WriteAllBytes(text, Resources.Vacations_A4);
					break;
				case 15:
					File.WriteAllBytes(text, Resources.AgreDedDet_A4);
					break;
				case 16:
					File.WriteAllBytes(text, Resources.VacationsTajmi_A4);
					break;
				}
				break;
			case Print.EnPrinterType.AllA5Printers:
				switch (printShowType - 1)
				{
				case 0:
					File.WriteAllBytes(text, Resources.Taradodkamel_A5);
					break;
				case 1:
					File.WriteAllBytes(text, Resources.Personel_A5);
					break;
				case 2:
					File.WriteAllBytes(text, Resources.listFish_A5);
					break;
				case 3:
					File.WriteAllBytes(text, Resources.Listasnad_A5);
					break;
				case 4:
					File.WriteAllBytes(text, Resources.Sanad_A5);
					break;
				case 5:
					File.WriteAllBytes(text, Resources.Fish_A5);
					break;
				case 6:
					File.WriteAllBytes(text, Resources.Holiday_A5);
					break;
				case 7:
					File.WriteAllBytes(text, Resources.TaradodJame_A5);
					break;
				case 8:
					File.WriteAllBytes(text, Resources.ListGharardad_A5);
					break;
				case 9:
					File.WriteAllBytes(text, Resources.Gharadad_A5);
					break;
				case 10:
					File.WriteAllBytes(text, Resources.TaradodKolPerson_A5);
					break;
				case 11:
					File.WriteAllBytes(text, Resources.MorakhasiDaily_A5);
					break;
				case 12:
					File.WriteAllBytes(text, Resources.AllTaradod_A5);
					break;
				case 13:
					File.WriteAllBytes(text, Resources.InsuranceList_A5);
					break;
				case 14:
					File.WriteAllBytes(text, Resources.Vacations_A5);
					break;
				case 15:
					File.WriteAllBytes(text, Resources.AgreDedDet_A5);
					break;
				case 16:
					File.WriteAllBytes(text, Resources.VacationsTajmi_A5);
					break;
				}
				break;
			}
		}
		sti.Load(text);
		return sti;
	}
}
