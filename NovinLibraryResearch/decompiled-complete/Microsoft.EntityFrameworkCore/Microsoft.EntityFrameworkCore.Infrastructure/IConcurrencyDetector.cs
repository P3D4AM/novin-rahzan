using System;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IConcurrencyDetector
{
	IDisposable EnterCriticalSection();
}
