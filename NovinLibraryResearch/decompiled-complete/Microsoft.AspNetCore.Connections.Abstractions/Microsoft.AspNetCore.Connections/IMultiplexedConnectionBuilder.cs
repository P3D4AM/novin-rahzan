using System;

namespace Microsoft.AspNetCore.Connections;

public interface IMultiplexedConnectionBuilder
{
	IServiceProvider ApplicationServices { get; }

	IMultiplexedConnectionBuilder Use(Func<MultiplexedConnectionDelegate, MultiplexedConnectionDelegate> middleware);

	MultiplexedConnectionDelegate Build();
}
