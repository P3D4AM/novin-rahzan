using System;
using System.Collections.Generic;

namespace TimeAttendanceDAL.JsonConverter;

public class JsonTaradodFull
{
	public JsonPerson clsPerson { get; set; }

	public DateTime Date1 { get; set; } = DateTime.Now;


	public DateTime Date2 { get; set; } = DateTime.Now;


	public List<JsonTaradodFullDet> List { get; set; } = new List<JsonTaradodFullDet>();

}
