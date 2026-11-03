using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.DalInterfaces;

public interface ICheckActivation
{
	ReturnedSaveFuncInfoWithValue<HozoorPackage> GetHozorType();

	ReturnedSaveFuncInfoWithValue<Level_Hozor> CheckActivation_Hozor();

	string LkSerial();

	ReturnedSaveFuncInfoWithValue<string> SoftwareSerial();
}
