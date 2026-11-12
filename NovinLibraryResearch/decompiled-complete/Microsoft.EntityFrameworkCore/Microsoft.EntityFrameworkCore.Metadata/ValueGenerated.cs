using System;

namespace Microsoft.EntityFrameworkCore.Metadata;

[Flags]
public enum ValueGenerated
{
	Never = 0,
	OnAdd = 1,
	OnUpdate = 2,
	OnAddOrUpdate = 3
}
