using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Json.Serialization;

internal delegate void SerializationCallback(object o, StreamingContext context);
