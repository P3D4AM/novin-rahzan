using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class ChangeTrackerFactory : IChangeTrackerFactory
{
	private readonly DbContext _context;

	private readonly IStateManager _stateManager;

	private readonly IChangeDetector _changeDetector;

	private readonly IModel _model;

	private readonly IEntityEntryGraphIterator _graphIterator;

	public ChangeTrackerFactory([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IChangeDetector changeDetector, [JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IEntityEntryGraphIterator graphIterator)
	{
		_context = currentContext.Context;
		_stateManager = stateManager;
		_changeDetector = changeDetector;
		_model = model;
		_graphIterator = graphIterator;
	}

	public virtual ChangeTracker Create()
	{
		return new ChangeTracker(_context, _stateManager, _changeDetector, _model, _graphIterator);
	}
}
