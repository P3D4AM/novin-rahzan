using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities.Accounting;

namespace EntityCache.Business.Global;

public interface ISmsTemplateHelper
{
	List<SmsParserTemplate> GetAll();

	Task<List<SmsParserTemplate>> GetAllFromServer();

	void Save(List<SmsParserTemplate> smsTemplates);
}
