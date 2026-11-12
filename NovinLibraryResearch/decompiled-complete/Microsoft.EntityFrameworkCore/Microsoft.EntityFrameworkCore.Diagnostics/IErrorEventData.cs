using System;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IErrorEventData
{
	Exception Exception { get; }
}
