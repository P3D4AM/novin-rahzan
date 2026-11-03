using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Note;

namespace EntityCache.Core.Repository.Accounting.Note;

public interface INoteGroupRepository : IRepository<NoteGroupBusiness>, INoGuidRepository<NoteGroupBusiness>, IDisposable
{
	Task<NoteGroupBusiness> CheckNameAsync(Guid guid, string Name, CancellationToken token, string tranName);

	Task<int> CountGroupChildsAsync(Guid groupGuid, CancellationToken token, string tranName);

	Task<List<NoteGroupBusiness>> GetAllAsync(Guid parentGuid, CancellationToken token, string tranName);
}
