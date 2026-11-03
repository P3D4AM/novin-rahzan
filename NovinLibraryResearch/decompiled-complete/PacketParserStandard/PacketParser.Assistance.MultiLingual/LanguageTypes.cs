namespace PacketParser.Assistance.MultiLingual;

public enum LanguageTypes : short
{
	[MultiLangaugeAttribute.KeyName("Farsi")]
	[PersianNameAttribute.PersianName("فارسی")]
	Farsi,
	[MultiLangaugeAttribute.KeyName("English")]
	[PersianNameAttribute.PersianName("English")]
	English,
	[MultiLangaugeAttribute.KeyName("Georgian")]
	[PersianNameAttribute.PersianName("Georgian")]
	Georgian,
	[MultiLangaugeAttribute.KeyName("Russian")]
	[PersianNameAttribute.PersianName("Russian")]
	Russian,
	[MultiLangaugeAttribute.KeyName("Arbic")]
	[PersianNameAttribute.PersianName("العربی")]
	Arbic,
	[MultiLangaugeAttribute.KeyName("Spanish")]
	[PersianNameAttribute.PersianName("Spanish")]
	Spanish,
	[MultiLangaugeAttribute.KeyName("Turkish")]
	[PersianNameAttribute.PersianName("Turkish")]
	Turkish,
	[MultiLangaugeAttribute.KeyName("EnterNational")]
	[PersianNameAttribute.PersianName("InterNational")]
	EnterNational
}
