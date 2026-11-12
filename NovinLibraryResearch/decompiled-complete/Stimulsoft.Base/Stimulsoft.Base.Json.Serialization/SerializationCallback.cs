using System.Runtime.Serialization;

namespace Stimulsoft.Base.Json.Serialization;

public delegate void SerializationCallback(object o, StreamingContext context);
