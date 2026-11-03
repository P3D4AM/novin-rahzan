using PacketParser;
using PacketParser.Payamak.Entities;

namespace PayamakUi.Classes.Entities;

public class SMSTemplate : BaseSmsTemplate
{
	public string DateRegisterPersian => base.DateRegister.MiladiToShamsi();
}
