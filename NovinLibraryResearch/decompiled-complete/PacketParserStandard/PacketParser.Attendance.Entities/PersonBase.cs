using System;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.Entities;

public class PersonBase : IPerson, IHasJustGuid
{
	private string _latinFullName = "";

	public bool Checked { get; set; }

	public int Id { get; set; }

	public short GroupId { get; set; }

	public short TitleId { get; set; }

	public string FullCode { get; set; } = "";


	public short Type { get; set; }

	public string Name { get; set; } = "";


	public string Family { get; set; } = "";


	public string LatinFullName
	{
		get
		{
			if (!string.IsNullOrEmpty(_latinFullName))
			{
				return _latinFullName;
			}
			_latinFullName = FullName.Negasht().Replace("-", "").Replace(" ", "");
			if (string.IsNullOrEmpty(_latinFullName))
			{
				_latinFullName = FullName;
			}
			return _latinFullName;
		}
		set
		{
			_latinFullName = value;
		}
	}

	public string FatherName { get; set; } = "";


	public virtual DateTime DateMStartWork { get; set; }

	public virtual DateTime DateMBirthday { get; set; }

	public string CodeMeli { get; set; } = "";


	public long CodeTafsil { get; set; }

	public string ZipCode { get; set; } = "";


	public string Mob1 { get; set; } = "";


	public string Mob2 { get; set; } = "";


	public string Tel1 { get; set; } = "";


	public string Tel2 { get; set; } = "";


	public string Address { get; set; } = "";


	public bool St { get; set; } = true;


	public Guid Guid { get; set; } = Guid.NewGuid();


	public int Hid { get; set; }

	public short HType { get; set; }

	public string PicPath { get; set; }

	public string Description { get; set; } = "";


	public string SodorSh { get; set; } = "";


	public string NoBimeh { get; set; } = "";


	public long CodeMoein { get; set; }

	public string MTavalod { get; set; } = "";


	public virtual DateTime DateMFinishW { get; set; }

	public short EisarId { get; set; }

	public string Shsh { get; set; } = "";


	public string SerialSh { get; set; } = "";


	public short Jensiat { get; set; }

	public string MadrakTahsili { get; set; } = "";


	public bool Check { get; set; }

	public string UserName { get; set; } = "";


	public byte[] HashPassword { get; set; }

	public string AllNumbers => Tel1 + "," + Tel2 + "," + Mob1 + "," + Mob2;

	public string FullName => Family + " " + Name;

	public Guid? GuidDepartment { get; set; }
}
