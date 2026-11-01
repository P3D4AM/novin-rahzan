using System;
using System.Collections.Generic;
using PacketParser;
using PacketParser.Entities.Attendance;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL;

[Serializable]
[DoNotPruneType]
public class LstTypeTaradod
{
	public short ID { get; set; }

	public string Name { get; set; }

	public LstTypeTaradod(short pid, string xstr)
	{
		ID = pid;
		Name = xstr;
	}

	public static List<LstTypeTaradod> GetAll()
	{
		return new List<LstTypeTaradod>
		{
			new LstTypeTaradod(3, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)3)),
			new LstTypeTaradod(4, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)4)),
			new LstTypeTaradod(5, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)5)),
			new LstTypeTaradod(7, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)7)),
			new LstTypeTaradod(8, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)8)),
			new LstTypeTaradod(28, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)28)),
			new LstTypeTaradod(91, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)91)),
			new LstTypeTaradod(92, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)92)),
			new LstTypeTaradod(93, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)93)),
			new LstTypeTaradod(10, Utilities.GetDisplay<TypeTaradodEnumType>((TypeTaradodEnumType)10))
		};
	}
}
