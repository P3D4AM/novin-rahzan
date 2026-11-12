using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.SignalR;

public interface IInvocationBinder
{
	Type GetReturnType(string invocationId);

	IReadOnlyList<Type> GetParameterTypes(string methodName);

	Type GetStreamItemType(string streamId);
}
