using System;
using PacketParser.Entities;

namespace TimeAttendanceDAL.JsonConverter;

public class JsonPerson
{
	public int ID { get; private set; }

	public short GroupID { get; set; }

	public short TitleID { get; set; }

	public string FullCode { get; set; } = "";


	public short Type { get; set; }

	public string Name { get; set; } = "";


	public string Family { get; set; } = "";


	public string FullName { get; set; } = "";


	public string FatherName { get; set; } = "";


	public DateTime DateMStartWork { get; set; } = DateTime.Now;


	public DateTime DateMFinishW { get; set; } = DateTime.MaxValue;


	public DateTime DateMBirthday { get; set; } = DateTime.MaxValue;


	public string CodeMeli { get; set; } = "";


	public long CodeTafsil { get; set; }

	public string ZipCode { get; set; } = "";


	public string Mob1 { get; set; } = "";


	public string Mob2 { get; set; } = "";


	public string Tel1 { get; set; } = "";


	public string Tel2 { get; set; } = "";


	public string Adress { get; set; } = "";


	public bool ST { get; set; } = true;


	public Guid GUID { get; set; } = Guid.Empty;


	public int HID { get; set; }

	public short HType { get; set; }

	public string PicPath { get; set; }

	public object Pic { get; set; }

	public string Description { get; set; } = "";


	public string SodorSH { get; set; } = "";


	public string NOBimeh { get; set; } = "";


	public long CodeMoein { get; set; }

	public string MTavalod { get; set; } = "";


	public short EisarID { get; set; }

	public string SHSH { get; set; } = "";


	public string SerialSH { get; set; } = "";


	public short Jensiat { get; set; }

	public string MadrakTahsili { get; set; } = "";


	public bool Check { get; set; }

	public string UserName { get; set; }

	public ServerStatusForClient sentToServerStatus { get; set; }
}
