using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketParser.Entities.Accounting;

public class EnumForUi<T> where T : Enum
{
	public T ValueMember { get; set; }

	public string DisplayMember => ValueMember.GetDisplay();

	public static List<string> DisplayMembers => (from p in GetAll()
		select p.DisplayMember)?.ToList();

	public static List<T> ValueMembers => (from p in GetAll()
		select p.ValueMember)?.ToList();

	public static List<EnumForUi<T>> GetAll()
	{
		List<EnumForUi<T>> list = new List<EnumForUi<T>>();
		try
		{
			foreach (object value in Enum.GetValues(typeof(T)))
			{
				list.Add(new EnumForUi<T>
				{
					ValueMember = (T)value
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\EnumForUi.cs", 29);
		}
		return list;
	}
}
