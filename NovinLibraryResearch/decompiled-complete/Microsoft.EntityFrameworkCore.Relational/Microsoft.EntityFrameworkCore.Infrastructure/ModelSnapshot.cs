using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public abstract class ModelSnapshot
{
	private IModel _model;

	public virtual IModel Model => _model ?? (_model = CreateModel());

	private IModel CreateModel()
	{
		Model model = new Model();
		ModelBuilder modelBuilder = new ModelBuilder(model);
		BuildModel(modelBuilder);
		return model;
	}

	protected abstract void BuildModel([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder);
}
