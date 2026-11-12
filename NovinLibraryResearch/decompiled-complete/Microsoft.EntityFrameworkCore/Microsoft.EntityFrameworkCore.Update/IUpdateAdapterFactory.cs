using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update;

public interface IUpdateAdapterFactory
{
	IUpdateAdapter Create();

	IUpdateAdapter CreateStandalone([JetBrains.Annotations.CanBeNull] IModel model = null);
}
