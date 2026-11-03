using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.InterFaces;

public interface IPerson : IHasJustGuid
{
	bool Checked { get; set; }

	int Id { get; set; }

	short GroupId { get; set; }

	short TitleId { get; set; }

	string FullCode { get; set; }

	short Type { get; set; }

	string Name { get; set; }

	string Family { get; set; }

	string LatinFullName { get; set; }

	string FatherName { get; set; }

	DateTime DateMStartWork { get; set; }

	DateTime DateMBirthday { get; set; }

	string CodeMeli { get; set; }

	long CodeTafsil { get; set; }

	string ZipCode { get; set; }

	string Mob1 { get; set; }

	string Mob2 { get; set; }

	string Tel1 { get; set; }

	string Tel2 { get; set; }

	string Address { get; set; }

	bool St { get; set; }

	int Hid { get; set; }

	short HType { get; set; }

	string PicPath { get; set; }

	string Description { get; set; }

	string SodorSh { get; set; }

	string NoBimeh { get; set; }

	long CodeMoein { get; set; }

	string MTavalod { get; set; }

	DateTime DateMFinishW { get; set; }

	short EisarId { get; set; }

	string Shsh { get; set; }

	string SerialSh { get; set; }

	short Jensiat { get; set; }

	string MadrakTahsili { get; set; }

	bool Check { get; set; }

	string UserName { get; set; }

	byte[] HashPassword { get; set; }
}
