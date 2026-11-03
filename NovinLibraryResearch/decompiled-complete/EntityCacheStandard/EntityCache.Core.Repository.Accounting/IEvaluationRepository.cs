using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Evaluation;
using PacketParser.BusinessLayer.Models.Evaluation;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface IEvaluationRepository : IRepository<EvaluationBusiness>, INoGuidRepository<EvaluationBusiness>, IDisposable
{
	Task<IEnumerable<EvaluationModel>> GetAllAsync(EvaluationFilter filter, CancellationToken token, string tranName);

	Task<List<EvaluationDetailModel>> GetAllDetailsAsync(Guid? guid, DateTime dateM, CancellationToken token, string tranName);

	Task<int> GetNextId2Async(CancellationToken token, string tranName);

	Task<EvaluationBusiness> GetAsync(int number, CancellationToken token, string tranName);

	Task<EvaluationBusiness> GetByAnbargardaniAsync(Guid anbarGardaniGuid, CancellationToken token, string tranName);
}
