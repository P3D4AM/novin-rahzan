using System.Collections.Concurrent;
using PacketParser.Entities.Interfaces;

namespace TimeAttendanceDAL.Utility.Settings;

public class ClsSystem : IHasContext
{
	private const string ReKey = "X9806539";

	private static ConcurrentDictionary<string, ClsSystem> _dic = new ConcurrentDictionary<string, ClsSystem>();

	public string ContextGuid { get; set; }

	public string GlobalPass
	{
		get
		{
			string text = ClsRegistry.Read("X9806539");
			if (text == "")
			{
				text = "05137597590";
			}
			return text;
		}
		set
		{
			if (value != "")
			{
				ClsRegistry.Write("X9806539", value);
			}
			else
			{
				Utility.MessageBoxShowError("رمز خالی است ، عملیات انجام نشد", ContextGuid);
			}
		}
	}

	public ClsSystem(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static ClsSystem GetClsSystem(string contexGuid)
	{
		_dic.TryGetValue(contexGuid, out var value);
		if (value == null)
		{
			value = new ClsSystem(contexGuid);
			_dic.TryAdd(contexGuid, value);
		}
		return value;
	}
}
