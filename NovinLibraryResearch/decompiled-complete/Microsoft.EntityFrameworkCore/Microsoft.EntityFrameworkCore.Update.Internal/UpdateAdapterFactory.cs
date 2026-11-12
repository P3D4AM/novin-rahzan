using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class UpdateAdapterFactory : IUpdateAdapterFactory
{
	private readonly ICurrentDbContext _currentContext;

	public UpdateAdapterFactory([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		_currentContext = currentContext;
	}

	public virtual IUpdateAdapter Create()
	{
		return new UpdateAdapter(_currentContext.Context.GetDependencies().StateManager);
	}

	public virtual IUpdateAdapter CreateStandalone(IModel model = null)
	{
		IStateManager stateManager = _currentContext.Context.GetDependencies().StateManager;
		if (model != null)
		{
			return new UpdateAdapter(new StateManager(stateManager.Dependencies.With(model)));
		}
		return new UpdateAdapter(new StateManager(stateManager.Dependencies));
	}
}
