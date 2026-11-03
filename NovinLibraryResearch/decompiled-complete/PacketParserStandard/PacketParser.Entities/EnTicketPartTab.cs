namespace PacketParser.Entities;

public enum EnTicketPartTab : short
{
	[PersianNameAttribute.PersianName("مشخصات عمومی")]
	PublicInfo,
	[PersianNameAttribute.PersianName("کاربران")]
	Users,
	[PersianNameAttribute.PersianName("سطوح دسترسی")]
	Access
}
