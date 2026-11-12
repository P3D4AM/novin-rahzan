using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.SignalR.Protocol;

public static class HandshakeProtocol
{
	private const string ProtocolPropertyName = "protocol";

	private static readonly JsonEncodedText ProtocolPropertyNameBytes = JsonEncodedText.Encode("protocol");

	private const string ProtocolVersionPropertyName = "version";

	private static readonly JsonEncodedText ProtocolVersionPropertyNameBytes = JsonEncodedText.Encode("version");

	private const string ErrorPropertyName = "error";

	private static readonly JsonEncodedText ErrorPropertyNameBytes = JsonEncodedText.Encode("error");

	private const string TypePropertyName = "type";

	private static readonly JsonEncodedText TypePropertyNameBytes = JsonEncodedText.Encode("type");

	private static readonly ReadOnlyMemory<byte> _successHandshakeData = GetSuccessHandshakeData();

	private static ReadOnlyMemory<byte> GetSuccessHandshakeData()
	{
		MemoryBufferWriter memoryBufferWriter = MemoryBufferWriter.Get();
		try
		{
			WriteResponseMessage(HandshakeResponseMessage.Empty, memoryBufferWriter);
			return memoryBufferWriter.ToArray();
		}
		finally
		{
			MemoryBufferWriter.Return(memoryBufferWriter);
		}
	}

	public static ReadOnlySpan<byte> GetSuccessfulHandshake(IHubProtocol protocol)
	{
		return _successHandshakeData.Span;
	}

	public static void WriteRequestMessage(HandshakeRequestMessage requestMessage, IBufferWriter<byte> output)
	{
		ReusableUtf8JsonWriter reusableUtf8JsonWriter = ReusableUtf8JsonWriter.Get(output);
		try
		{
			Utf8JsonWriter jsonWriter = reusableUtf8JsonWriter.GetJsonWriter();
			jsonWriter.WriteStartObject();
			jsonWriter.WriteString(ProtocolPropertyNameBytes, requestMessage.Protocol);
			jsonWriter.WriteNumber(ProtocolVersionPropertyNameBytes, requestMessage.Version);
			jsonWriter.WriteEndObject();
			jsonWriter.Flush();
		}
		finally
		{
			ReusableUtf8JsonWriter.Return(reusableUtf8JsonWriter);
		}
		TextMessageFormatter.WriteRecordSeparator(output);
	}

	public static void WriteResponseMessage(HandshakeResponseMessage responseMessage, IBufferWriter<byte> output)
	{
		ReusableUtf8JsonWriter reusableUtf8JsonWriter = ReusableUtf8JsonWriter.Get(output);
		try
		{
			Utf8JsonWriter jsonWriter = reusableUtf8JsonWriter.GetJsonWriter();
			jsonWriter.WriteStartObject();
			if (!string.IsNullOrEmpty(responseMessage.Error))
			{
				jsonWriter.WriteString(ErrorPropertyNameBytes, responseMessage.Error);
			}
			jsonWriter.WriteEndObject();
			jsonWriter.Flush();
		}
		finally
		{
			ReusableUtf8JsonWriter.Return(reusableUtf8JsonWriter);
		}
		TextMessageFormatter.WriteRecordSeparator(output);
	}

	public static bool TryParseResponseMessage(ref ReadOnlySequence<byte> buffer, [NotNullWhen(true)] out HandshakeResponseMessage? responseMessage)
	{
		if (!TextMessageParser.TryParseMessage(ref buffer, out var payload))
		{
			responseMessage = null;
			return false;
		}
		Utf8JsonReader reader = new Utf8JsonReader(payload, isFinalBlock: true, default(JsonReaderState));
		reader.CheckRead();
		reader.EnsureObjectStart();
		string error = null;
		while (reader.CheckRead())
		{
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				if (reader.ValueTextEquals(TypePropertyNameBytes.EncodedUtf8Bytes))
				{
					throw new InvalidDataException("Expected a handshake response from the server.");
				}
				if (reader.ValueTextEquals(ErrorPropertyNameBytes.EncodedUtf8Bytes))
				{
					error = reader.ReadAsString("error");
				}
				else
				{
					reader.Skip();
				}
				continue;
			}
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}
			throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading handshake response JSON.");
		}
		responseMessage = new HandshakeResponseMessage(error);
		return true;
	}

	public static bool TryParseRequestMessage(ref ReadOnlySequence<byte> buffer, [NotNullWhen(true)] out HandshakeRequestMessage? requestMessage)
	{
		if (!TextMessageParser.TryParseMessage(ref buffer, out var payload))
		{
			requestMessage = null;
			return false;
		}
		Utf8JsonReader reader = new Utf8JsonReader(payload, isFinalBlock: true, default(JsonReaderState));
		reader.CheckRead();
		reader.EnsureObjectStart();
		string text = null;
		int? num = null;
		while (reader.CheckRead())
		{
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				if (reader.ValueTextEquals(ProtocolPropertyNameBytes.EncodedUtf8Bytes))
				{
					text = reader.ReadAsString("protocol");
				}
				else if (reader.ValueTextEquals(ProtocolVersionPropertyNameBytes.EncodedUtf8Bytes))
				{
					num = reader.ReadAsInt32("version");
				}
				else
				{
					reader.Skip();
				}
				continue;
			}
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				break;
			}
			throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading handshake request JSON. Message content: {GetPayloadAsString()}");
		}
		if (text == null)
		{
			throw new InvalidDataException("Missing required property 'protocol'. Message content: " + GetPayloadAsString());
		}
		if (!num.HasValue)
		{
			throw new InvalidDataException("Missing required property 'version'. Message content: " + GetPayloadAsString());
		}
		requestMessage = new HandshakeRequestMessage(text, num.Value);
		return true;
		string GetPayloadAsString()
		{
			return Encoding.UTF8.GetString(BuffersExtensions.ToArray(in payload));
		}
	}
}
