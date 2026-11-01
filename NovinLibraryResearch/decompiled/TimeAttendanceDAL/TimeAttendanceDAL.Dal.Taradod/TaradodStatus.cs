using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Dal.Taradod;

public class TaradodStatus
{
	private static List<TaradodStatus> _lst;

	public StatuseTaradodEnumType Type { get; set; }

	public short Count { get; set; }

	public static IReadOnlyList<TaradodStatus> Lst => _lst;

	public string ToStr()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return $"{Utilities.GetDisplay<StatuseTaradodEnumType>(Type)}({Count})";
	}

	public static void Reset()
	{
		_lst = null;
	}

	public static void AddRange(IEnumerable<StatuseTaradodEnumType> types)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Reset();
			if (types == null)
			{
				return;
			}
			foreach (StatuseTaradodEnumType type in types)
			{
				Add(type);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "AddRange", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodStatus.cs", 33);
		}
	}

	public static void Add(StatuseTaradodEnumType Type)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_lst == null)
			{
				_lst = new List<TaradodStatus>();
			}
			TaradodStatus taradodStatus = _lst?.FirstOrDefault((TaradodStatus p) => p.Type == Type);
			if (taradodStatus == null)
			{
				taradodStatus = new TaradodStatus
				{
					Type = Type
				};
				if (taradodStatus?.Count > 0)
				{
					_lst.Add(taradodStatus);
				}
			}
			taradodStatus.Count++;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Add", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodStatus.cs", 53);
		}
	}

	public static string ListToString()
	{
		return string.Join("  ,  ", Lst?.Select((TaradodStatus p) => p.ToStr()) ?? new string[1] { "" });
	}
}
