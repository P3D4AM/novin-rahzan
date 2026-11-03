using PacketParser.Settings.Advertise;

namespace PacketParser.Entities.Interfaces.Advertise;

public interface IAdvDepartment : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	AdvDepartmentChatList ChatList { get; set; }

	string ChatData { get; set; }

	bool IsSendChat { get; set; }

	string Description { get; set; }

	int ChatCount { get; set; }

	AdvDepartmentChatCity ChatCity { get; set; }

	string ChatCityData { get; set; }
}
