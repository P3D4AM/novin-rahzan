using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface ILazyLoader : IDisposable
{
	void SetLoaded([NotNull] object entity, [NotNull][CallerMemberName] string navigationName = "", bool loaded = true);

	void Load([NotNull] object entity, [NotNull][CallerMemberName] string navigationName = null);

	Task LoadAsync([NotNull] object entity, CancellationToken cancellationToken = default(CancellationToken), [NotNull][CallerMemberName] string navigationName = null);
}
