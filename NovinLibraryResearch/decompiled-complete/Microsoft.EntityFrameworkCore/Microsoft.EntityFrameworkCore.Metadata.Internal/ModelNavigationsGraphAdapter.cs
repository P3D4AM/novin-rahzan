using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class ModelNavigationsGraphAdapter : Graph<IConventionEntityType>
{
	private readonly IConventionModel _model;

	public override IEnumerable<IConventionEntityType> Vertices => _model.GetEntityTypes();

	public ModelNavigationsGraphAdapter([JetBrains.Annotations.NotNull] IConventionModel model)
	{
		_model = model;
	}

	public override IEnumerable<IConventionEntityType> GetOutgoingNeighbors(IConventionEntityType from)
	{
		return (from fk in @from.GetForeignKeys()
			where fk.DependentToPrincipal != null
			select fk.PrincipalEntityType).Union(from fk in @from.GetReferencingForeignKeys()
			where fk.PrincipalToDependent != null
			select fk.DeclaringEntityType);
	}

	public override IEnumerable<IConventionEntityType> GetIncomingNeighbors(IConventionEntityType to)
	{
		return (from fk in to.GetForeignKeys()
			where fk.PrincipalToDependent != null
			select fk.PrincipalEntityType).Union(from fk in to.GetReferencingForeignKeys()
			where fk.DependentToPrincipal != null
			select fk.DeclaringEntityType);
	}
}
