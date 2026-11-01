using System;
using System.Collections.Generic;
using System.Drawing;
using PacketParser.Attendance;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Other;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.JsonConverter;

public class JsonTaradodFullDet : IHasContext
{
	public ATT_TaradodDet AttTaradodDet;

	public Color ColorKk = Color.Black;

	public Person Person11;

	public Morakhasidaily Morakhasi;

	public GharardadNew Gharadad;

	public Holiday Holiday;

	public List<ShiftDet> ShiftDets = new List<ShiftDet>();

	public List<LogOnPersonel> List = new List<LogOnPersonel>();

	public CoupleTaradod[] CoupleTaradod = new CoupleTaradod[8];

	public short DayofShMonth;

	public int? KasrKar;

	public int? EzafeKar;

	public int? Karkard;

	public int? SumShiftMinuts;

	public StatuseTaradodEnumType StatuseTaradod;

	public string ContextGuid { get; set; }

	public DateTime DateM { get; private set; } = DateTime.Now;


	public Color ColorEk { get; private set; } = Color.Black;


	public JsonTaradodFullDet(string contextGuid)
	{
		ContextGuid = contextGuid;
		Person11 = new Person(contextGuid);
		AttTaradodDet = new ATT_TaradodDet(ContextGuid);
	}
}
