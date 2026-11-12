using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Connections;

public delegate Task MultiplexedConnectionDelegate(MultiplexedConnectionContext connection);
