using System.Threading.Tasks;

namespace EntityCache.Business.Global;

public interface AndroidCacheFactory
{
	ISmsTemplateHelper SmsTemplateHelper { get; }

	Task<string> GetContextGuid();
}
