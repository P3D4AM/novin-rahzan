using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Design;

public static class DbContextActivator
{
	public static DbContext CreateInstance([JetBrains.Annotations.NotNull] Type contextType, [JetBrains.Annotations.CanBeNull] Assembly startupAssembly = null, [JetBrains.Annotations.CanBeNull] IOperationReportHandler reportHandler = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(contextType, "contextType");
		return new DbContextOperations(new OperationReporter(reportHandler), contextType.Assembly, startupAssembly ?? contextType.Assembly, Array.Empty<string>()).CreateContext(contextType.FullName);
	}
}
