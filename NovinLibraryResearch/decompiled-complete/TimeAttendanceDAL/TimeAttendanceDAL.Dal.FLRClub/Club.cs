using System;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Enums;

namespace TimeAttendanceDAL.Dal.FLRClub;

public class Club : IHasContext
{
	public bool[] ContractDays = new bool[7];

	public string ContextGuid { get; }

	public Person ClsPerson { get; set; }

	public EnumsHoghogh.EnumkarkardSt ContractType { get; set; } = EnumsHoghogh.EnumkarkardSt.Monthly;


	public DateTime ContractStartDate { get; set; } = DateTime.Now;


	public DateTime ContractEndDate { get; set; } = DateTime.Now.AddMonths(1);


	public decimal ContractHour { get; set; } = 100m;


	public Club(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
