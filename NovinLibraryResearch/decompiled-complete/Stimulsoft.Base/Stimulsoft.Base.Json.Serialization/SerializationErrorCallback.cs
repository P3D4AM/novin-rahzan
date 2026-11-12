using System.Runtime.Serialization;

namespace Stimulsoft.Base.Json.Serialization;

public delegate void SerializationErrorCallback(object o, StreamingContext context, ErrorContext errorContext);
