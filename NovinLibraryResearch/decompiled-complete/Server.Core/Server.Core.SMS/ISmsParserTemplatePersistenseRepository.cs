using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Accounting;

namespace Server.Core.SMS;

public interface ISmsParserTemplatePersistenseRepository
{
	Task<ReturnedSaveFuncInfoWithValue<List<SmsParserTemplate>>> GetAll();

	Task<ReturnedSaveFuncInfo> Save(List<SmsParserTemplate> smsParserTemplates);

	Task<ReturnedSaveFuncInfo> SaveAsync(SmsParserTemplate smsParserTemplate);

	Task<ReturnedSaveFuncInfo> DeleteAsync(Guid guid);
}
