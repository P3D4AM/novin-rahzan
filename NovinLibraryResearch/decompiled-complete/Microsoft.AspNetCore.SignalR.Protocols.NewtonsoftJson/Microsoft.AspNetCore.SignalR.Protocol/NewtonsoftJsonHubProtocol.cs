using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.AspNetCore.SignalR.Protocol;

public class NewtonsoftJsonHubProtocol : IHubProtocol
{
	private const string ResultPropertyName = "result";

	private const string ItemPropertyName = "item";

	private const string InvocationIdPropertyName = "invocationId";

	private const string StreamIdsPropertyName = "streamIds";

	private const string TypePropertyName = "type";

	private const string ErrorPropertyName = "error";

	private const string TargetPropertyName = "target";

	private const string ArgumentsPropertyName = "arguments";

	private const string HeadersPropertyName = "headers";

	private const string AllowReconnectPropertyName = "allowReconnect";

	private const string ProtocolName = "json";

	private const int ProtocolVersion = 1;

	public JsonSerializer PayloadSerializer { get; }

	public string Name => "json";

	public int Version => 1;

	public TransferFormat TransferFormat => TransferFormat.Text;

	public NewtonsoftJsonHubProtocol()
		: this(Options.Create(new NewtonsoftJsonHubProtocolOptions()))
	{
	}

	public NewtonsoftJsonHubProtocol(IOptions<NewtonsoftJsonHubProtocolOptions> options)
	{
		PayloadSerializer = JsonSerializer.Create(options.Value.PayloadSerializerSettings);
	}

	public bool IsVersionSupported(int version)
	{
		return version == Version;
	}

	public bool TryParseMessage(ref ReadOnlySequence<byte> input, IInvocationBinder binder, [NotNullWhen(true)] out HubMessage? message)
	{
		if (!Microsoft.AspNetCore.Internal.TextMessageParser.TryParseMessage(ref input, out var payload))
		{
			message = null;
			return false;
		}
		Microsoft.AspNetCore.SignalR.Internal.Utf8BufferTextReader utf8BufferTextReader = Microsoft.AspNetCore.SignalR.Internal.Utf8BufferTextReader.Get(in payload);
		try
		{
			message = ParseMessage(utf8BufferTextReader, binder);
		}
		finally
		{
			Microsoft.AspNetCore.SignalR.Internal.Utf8BufferTextReader.Return(utf8BufferTextReader);
		}
		return message != null;
	}

	public void WriteMessage(HubMessage message, IBufferWriter<byte> output)
	{
		WriteMessageCore(message, output);
		Microsoft.AspNetCore.Internal.TextMessageFormatter.WriteRecordSeparator(output);
	}

	public ReadOnlyMemory<byte> GetMessageBytes(HubMessage message)
	{
		return HubProtocolExtensions.GetMessageBytes(this, message);
	}

	private HubMessage ParseMessage(Microsoft.AspNetCore.SignalR.Internal.Utf8BufferTextReader textReader, IInvocationBinder binder)
	{
		try
		{
			int? num = null;
			string text = null;
			string text2 = null;
			string error = null;
			bool hasItem = false;
			object item = null;
			JToken jToken = null;
			bool hasResult = false;
			object result = null;
			JToken jToken2 = null;
			bool hasArguments = false;
			object[] arguments = null;
			string[] streamIds = null;
			JArray jArray = null;
			ExceptionDispatchInfo exceptionDispatchInfo = null;
			Dictionary<string, string> headers = null;
			bool flag = false;
			bool allowReconnect = false;
			using (JsonTextReader jsonTextReader = JsonUtils.CreateJsonTextReader(textReader))
			{
				jsonTextReader.DateParseHandling = DateParseHandling.None;
				JsonUtils.CheckRead(jsonTextReader);
				JsonUtils.EnsureObjectStart(jsonTextReader);
				do
				{
					switch (jsonTextReader.TokenType)
					{
					case JsonToken.PropertyName:
						switch (jsonTextReader.Value?.ToString())
						{
						case "type":
						{
							int? num2 = JsonUtils.ReadAsInt32(jsonTextReader, "type");
							if (!num2.HasValue)
							{
								throw new InvalidDataException("Missing required property 'type'.");
							}
							num = num2.Value;
							break;
						}
						case "invocationId":
							text = JsonUtils.ReadAsString(jsonTextReader, "invocationId");
							break;
						case "streamIds":
						{
							JsonUtils.CheckRead(jsonTextReader);
							if (jsonTextReader.TokenType != JsonToken.StartArray)
							{
								throw new InvalidDataException(string.Format("Expected '{0}' to be of type {1}.", "streamIds", JTokenType.Array));
							}
							List<string> list = new List<string>();
							jsonTextReader.Read();
							while (jsonTextReader.TokenType != JsonToken.EndArray)
							{
								list.Add(jsonTextReader.Value?.ToString() ?? throw new InvalidDataException("Null value for 'streamIds' is not valid."));
								jsonTextReader.Read();
							}
							streamIds = list.ToArray();
							break;
						}
						case "target":
							text2 = JsonUtils.ReadAsString(jsonTextReader, "target");
							break;
						case "error":
							error = JsonUtils.ReadAsString(jsonTextReader, "error");
							break;
						case "allowReconnect":
							allowReconnect = JsonUtils.ReadAsBoolean(jsonTextReader, "allowReconnect");
							break;
						case "result":
						{
							hasResult = true;
							if (string.IsNullOrEmpty(text))
							{
								JsonUtils.CheckRead(jsonTextReader);
								jToken2 = JToken.Load(jsonTextReader);
								break;
							}
							Type returnType = binder.GetReturnType(text);
							if (!JsonUtils.ReadForType(jsonTextReader, returnType))
							{
								throw new JsonReaderException("Unexpected end when reading JSON");
							}
							result = PayloadSerializer.Deserialize(jsonTextReader, returnType);
							break;
						}
						case "item":
						{
							JsonUtils.CheckRead(jsonTextReader);
							hasItem = true;
							string text3 = null;
							if (!string.IsNullOrEmpty(text))
							{
								text3 = text;
								try
								{
									Type streamItemType = binder.GetStreamItemType(text3);
									item = PayloadSerializer.Deserialize(jsonTextReader, streamItemType);
								}
								catch (Exception source2)
								{
									return new StreamBindingFailureMessage(text3, ExceptionDispatchInfo.Capture(source2));
								}
							}
							else
							{
								jToken = JToken.Load(jsonTextReader);
							}
							break;
						}
						case "arguments":
						{
							JsonUtils.CheckRead(jsonTextReader);
							int depth = jsonTextReader.Depth;
							if (jsonTextReader.TokenType != JsonToken.StartArray)
							{
								throw new InvalidDataException(string.Format("Expected '{0}' to be of type {1}.", "arguments", JTokenType.Array));
							}
							hasArguments = true;
							if (string.IsNullOrEmpty(text2))
							{
								jArray = JArray.Load(jsonTextReader);
								break;
							}
							try
							{
								IReadOnlyList<Type> parameterTypes = binder.GetParameterTypes(text2);
								arguments = BindArguments(jsonTextReader, parameterTypes);
							}
							catch (Exception source)
							{
								exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source);
								while ((jsonTextReader.Depth == depth && jsonTextReader.TokenType == JsonToken.StartArray) || jsonTextReader.Depth > depth)
								{
									JsonUtils.CheckRead(jsonTextReader);
								}
							}
							break;
						}
						case "headers":
							JsonUtils.CheckRead(jsonTextReader);
							headers = ReadHeaders(jsonTextReader);
							break;
						default:
							JsonUtils.CheckRead(jsonTextReader);
							jsonTextReader.Skip();
							break;
						}
						break;
					case JsonToken.EndObject:
						flag = true;
						break;
					}
				}
				while (!flag && JsonUtils.CheckRead(jsonTextReader));
			}
			HubMessage message;
			switch (num)
			{
			case 1:
				if (text2 == null)
				{
					throw new InvalidDataException("Missing required property 'target'.");
				}
				if (jArray != null)
				{
					try
					{
						IReadOnlyList<Type> parameterTypes2 = binder.GetParameterTypes(text2);
						arguments = BindArguments(jArray, parameterTypes2);
					}
					catch (Exception source3)
					{
						exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source3);
					}
				}
				message = ((exceptionDispatchInfo != null) ? new InvocationBindingFailureMessage(text, text2, exceptionDispatchInfo) : BindInvocationMessage(text, text2, arguments, hasArguments, streamIds, binder));
				break;
			case 4:
				if (text2 == null)
				{
					throw new InvalidDataException("Missing required property 'target'.");
				}
				if (jArray != null)
				{
					try
					{
						IReadOnlyList<Type> parameterTypes3 = binder.GetParameterTypes(text2);
						arguments = BindArguments(jArray, parameterTypes3);
					}
					catch (Exception source5)
					{
						exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source5);
					}
				}
				message = ((exceptionDispatchInfo != null) ? new InvocationBindingFailureMessage(text, text2, exceptionDispatchInfo) : BindStreamInvocationMessage(text, text2, arguments, hasArguments, streamIds, binder));
				break;
			case 2:
				if (text == null)
				{
					throw new InvalidDataException("Missing required property 'invocationId'.");
				}
				if (jToken != null)
				{
					try
					{
						Type streamItemType2 = binder.GetStreamItemType(text);
						item = jToken.ToObject(streamItemType2, PayloadSerializer);
					}
					catch (Exception source4)
					{
						message = new StreamBindingFailureMessage(text, ExceptionDispatchInfo.Capture(source4));
						break;
					}
				}
				message = BindStreamItemMessage(text, item, hasItem, binder);
				break;
			case 3:
				if (text == null)
				{
					throw new InvalidDataException("Missing required property 'invocationId'.");
				}
				if (jToken2 != null)
				{
					Type returnType2 = binder.GetReturnType(text);
					result = jToken2.ToObject(returnType2, PayloadSerializer);
				}
				message = BindCompletionMessage(text, error, result, hasResult, binder);
				break;
			case 5:
				message = BindCancelInvocationMessage(text);
				break;
			case 6:
				return PingMessage.Instance;
			case 7:
				return BindCloseMessage(error, allowReconnect);
			case null:
				throw new InvalidDataException("Missing required property 'type'.");
			default:
				return null;
			}
			return ApplyHeaders(message, headers);
		}
		catch (JsonReaderException innerException)
		{
			throw new InvalidDataException("Error reading JSON.", innerException);
		}
	}

	private static Dictionary<string, string> ReadHeaders(JsonTextReader reader)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
		if (reader.TokenType != JsonToken.StartObject)
		{
			throw new InvalidDataException(string.Format("Expected '{0}' to be of type {1}.", "headers", JTokenType.Object));
		}
		while (reader.Read())
		{
			switch (reader.TokenType)
			{
			case JsonToken.PropertyName:
			{
				string text = reader.Value.ToString();
				JsonUtils.CheckRead(reader);
				if (reader.TokenType != JsonToken.String)
				{
					throw new InvalidDataException($"Expected header '{text}' to be of type {JTokenType.String}.");
				}
				dictionary[text] = reader.Value.ToString();
				break;
			}
			case JsonToken.EndObject:
				return dictionary;
			}
		}
		throw new JsonReaderException("Unexpected end when reading message headers");
	}

	private void WriteMessageCore(HubMessage message, IBufferWriter<byte> stream)
	{
		Microsoft.AspNetCore.Internal.Utf8BufferTextWriter utf8BufferTextWriter = Microsoft.AspNetCore.Internal.Utf8BufferTextWriter.Get(stream);
		try
		{
			using JsonTextWriter jsonTextWriter = JsonUtils.CreateJsonTextWriter(utf8BufferTextWriter);
			jsonTextWriter.WriteStartObject();
			if (!(message is InvocationMessage message2))
			{
				if (!(message is StreamInvocationMessage message3))
				{
					if (!(message is StreamItemMessage message4))
					{
						if (!(message is CompletionMessage message5))
						{
							if (!(message is CancelInvocationMessage message6))
							{
								if (!(message is PingMessage))
								{
									if (!(message is CloseMessage message7))
									{
										throw new InvalidOperationException("Unsupported message type: " + message.GetType().FullName);
									}
									WriteMessageType(jsonTextWriter, 7);
									WriteCloseMessage(message7, jsonTextWriter);
								}
								else
								{
									WriteMessageType(jsonTextWriter, 6);
								}
							}
							else
							{
								WriteMessageType(jsonTextWriter, 5);
								WriteHeaders(jsonTextWriter, message6);
								WriteCancelInvocationMessage(message6, jsonTextWriter);
							}
						}
						else
						{
							WriteMessageType(jsonTextWriter, 3);
							WriteHeaders(jsonTextWriter, message5);
							WriteCompletionMessage(message5, jsonTextWriter);
						}
					}
					else
					{
						WriteMessageType(jsonTextWriter, 2);
						WriteHeaders(jsonTextWriter, message4);
						WriteStreamItemMessage(message4, jsonTextWriter);
					}
				}
				else
				{
					WriteMessageType(jsonTextWriter, 4);
					WriteHeaders(jsonTextWriter, message3);
					WriteStreamInvocationMessage(message3, jsonTextWriter);
				}
			}
			else
			{
				WriteMessageType(jsonTextWriter, 1);
				WriteHeaders(jsonTextWriter, message2);
				WriteInvocationMessage(message2, jsonTextWriter);
			}
			jsonTextWriter.WriteEndObject();
			jsonTextWriter.Flush();
		}
		finally
		{
			Microsoft.AspNetCore.Internal.Utf8BufferTextWriter.Return(utf8BufferTextWriter);
		}
	}

	private static void WriteHeaders(JsonTextWriter writer, HubInvocationMessage message)
	{
		if (message.Headers == null || message.Headers.Count <= 0)
		{
			return;
		}
		writer.WritePropertyName("headers");
		writer.WriteStartObject();
		foreach (KeyValuePair<string, string> header in message.Headers)
		{
			writer.WritePropertyName(header.Key);
			writer.WriteValue(header.Value);
		}
		writer.WriteEndObject();
	}

	private void WriteCompletionMessage(CompletionMessage message, JsonTextWriter writer)
	{
		WriteInvocationId(message, writer);
		if (!string.IsNullOrEmpty(message.Error))
		{
			writer.WritePropertyName("error");
			writer.WriteValue(message.Error);
		}
		else if (message.HasResult)
		{
			writer.WritePropertyName("result");
			PayloadSerializer.Serialize(writer, message.Result);
		}
	}

	private static void WriteCancelInvocationMessage(CancelInvocationMessage message, JsonTextWriter writer)
	{
		WriteInvocationId(message, writer);
	}

	private void WriteStreamItemMessage(StreamItemMessage message, JsonTextWriter writer)
	{
		WriteInvocationId(message, writer);
		writer.WritePropertyName("item");
		PayloadSerializer.Serialize(writer, message.Item);
	}

	private void WriteInvocationMessage(InvocationMessage message, JsonTextWriter writer)
	{
		WriteInvocationId(message, writer);
		writer.WritePropertyName("target");
		writer.WriteValue(message.Target);
		WriteArguments(message.Arguments, writer);
		WriteStreamIds(message.StreamIds, writer);
	}

	private void WriteStreamInvocationMessage(StreamInvocationMessage message, JsonTextWriter writer)
	{
		WriteInvocationId(message, writer);
		writer.WritePropertyName("target");
		writer.WriteValue(message.Target);
		WriteArguments(message.Arguments, writer);
		WriteStreamIds(message.StreamIds, writer);
	}

	private static void WriteCloseMessage(CloseMessage message, JsonTextWriter writer)
	{
		if (message.Error != null)
		{
			writer.WritePropertyName("error");
			writer.WriteValue(message.Error);
		}
		if (message.AllowReconnect)
		{
			writer.WritePropertyName("allowReconnect");
			writer.WriteValue(value: true);
		}
	}

	private void WriteArguments(object[] arguments, JsonTextWriter writer)
	{
		writer.WritePropertyName("arguments");
		writer.WriteStartArray();
		foreach (object value in arguments)
		{
			PayloadSerializer.Serialize(writer, value);
		}
		writer.WriteEndArray();
	}

	private static void WriteStreamIds(string[] streamIds, JsonTextWriter writer)
	{
		if (streamIds != null)
		{
			writer.WritePropertyName("streamIds");
			writer.WriteStartArray();
			foreach (string value in streamIds)
			{
				writer.WriteValue(value);
			}
			writer.WriteEndArray();
		}
	}

	private static void WriteInvocationId(HubInvocationMessage message, JsonTextWriter writer)
	{
		if (!string.IsNullOrEmpty(message.InvocationId))
		{
			writer.WritePropertyName("invocationId");
			writer.WriteValue(message.InvocationId);
		}
	}

	private static void WriteMessageType(JsonTextWriter writer, int type)
	{
		writer.WritePropertyName("type");
		writer.WriteValue(type);
	}

	private static HubMessage BindCancelInvocationMessage(string invocationId)
	{
		if (string.IsNullOrEmpty(invocationId))
		{
			throw new InvalidDataException("Missing required property 'invocationId'.");
		}
		return new CancelInvocationMessage(invocationId);
	}

	private static HubMessage BindCompletionMessage(string invocationId, string error, object result, bool hasResult, IInvocationBinder binder)
	{
		if (string.IsNullOrEmpty(invocationId))
		{
			throw new InvalidDataException("Missing required property 'invocationId'.");
		}
		if (error != null && hasResult)
		{
			throw new InvalidDataException("The 'error' and 'result' properties are mutually exclusive.");
		}
		if (hasResult)
		{
			return new CompletionMessage(invocationId, error, result, hasResult: true);
		}
		return new CompletionMessage(invocationId, error, null, hasResult: false);
	}

	private static HubMessage BindStreamItemMessage(string invocationId, object item, bool hasItem, IInvocationBinder binder)
	{
		if (string.IsNullOrEmpty(invocationId))
		{
			throw new InvalidDataException("Missing required property 'invocationId'.");
		}
		if (!hasItem)
		{
			throw new InvalidDataException("Missing required property 'item'.");
		}
		return new StreamItemMessage(invocationId, item);
	}

	private static HubMessage BindStreamInvocationMessage(string invocationId, string target, object[] arguments, bool hasArguments, string[] streamIds, IInvocationBinder binder)
	{
		if (string.IsNullOrEmpty(invocationId))
		{
			throw new InvalidDataException("Missing required property 'invocationId'.");
		}
		if (!hasArguments)
		{
			throw new InvalidDataException("Missing required property 'arguments'.");
		}
		if (string.IsNullOrEmpty(target))
		{
			throw new InvalidDataException("Missing required property 'target'.");
		}
		return new StreamInvocationMessage(invocationId, target, arguments, streamIds);
	}

	private static HubMessage BindInvocationMessage(string invocationId, string target, object[] arguments, bool hasArguments, string[] streamIds, IInvocationBinder binder)
	{
		if (string.IsNullOrEmpty(target))
		{
			throw new InvalidDataException("Missing required property 'target'.");
		}
		if (!hasArguments)
		{
			throw new InvalidDataException("Missing required property 'arguments'.");
		}
		return new InvocationMessage(invocationId, target, arguments, streamIds);
	}

	private static bool ReadArgumentAsType(JsonTextReader reader, IReadOnlyList<Type> paramTypes, int paramIndex)
	{
		if (paramIndex < paramTypes.Count)
		{
			Type type = paramTypes[paramIndex];
			return JsonUtils.ReadForType(reader, type);
		}
		return reader.Read();
	}

	private object[] BindArguments(JsonTextReader reader, IReadOnlyList<Type> paramTypes)
	{
		object[] array = null;
		int num = 0;
		int num2 = 0;
		int count = paramTypes.Count;
		while (ReadArgumentAsType(reader, paramTypes, num))
		{
			if (reader.TokenType == JsonToken.EndArray)
			{
				if (num2 != count)
				{
					throw new InvalidDataException($"Invocation provides {num2} argument(s) but target expects {count}.");
				}
				return array ?? Array.Empty<object>();
			}
			if (array == null)
			{
				array = new object[count];
			}
			try
			{
				if (num < count)
				{
					array[num] = PayloadSerializer.Deserialize(reader, paramTypes[num]);
				}
				else
				{
					reader.Skip();
				}
				num2++;
				num++;
			}
			catch (Exception innerException)
			{
				throw new InvalidDataException("Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.", innerException);
			}
		}
		throw new JsonReaderException("Unexpected end when reading JSON");
	}

	private static CloseMessage BindCloseMessage(string error, bool allowReconnect)
	{
		if (error == null && !allowReconnect)
		{
			return CloseMessage.Empty;
		}
		return new CloseMessage(error, allowReconnect);
	}

	private object[] BindArguments(JArray args, IReadOnlyList<Type> paramTypes)
	{
		int count = paramTypes.Count;
		int count2 = args.Count;
		if (count != count2)
		{
			throw new InvalidDataException($"Invocation provides {count2} argument(s) but target expects {count}.");
		}
		if (count == 0)
		{
			return Array.Empty<object>();
		}
		object[] array = new object[count2];
		try
		{
			for (int i = 0; i < count; i++)
			{
				Type objectType = paramTypes[i];
				array[i] = args[i].ToObject(objectType, PayloadSerializer);
			}
			return array;
		}
		catch (Exception innerException)
		{
			throw new InvalidDataException("Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.", innerException);
		}
	}

	private static HubMessage ApplyHeaders(HubMessage message, Dictionary<string, string> headers)
	{
		if (headers != null && message is HubInvocationMessage hubInvocationMessage)
		{
			hubInvocationMessage.Headers = headers;
		}
		return message;
	}

	internal static JsonSerializerSettings CreateDefaultSerializerSettings()
	{
		return new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver()
		};
	}
}
