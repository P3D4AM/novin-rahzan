using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Internal;

namespace Microsoft.AspNetCore.Http.Connections;

public static class NegotiateProtocol
{
	private const string ConnectionIdPropertyName = "connectionId";

	private static readonly JsonEncodedText ConnectionIdPropertyNameBytes = JsonEncodedText.Encode("connectionId");

	private const string ConnectionTokenPropertyName = "connectionToken";

	private static readonly JsonEncodedText ConnectionTokenPropertyNameBytes = JsonEncodedText.Encode("connectionToken");

	private const string UrlPropertyName = "url";

	private static readonly JsonEncodedText UrlPropertyNameBytes = JsonEncodedText.Encode("url");

	private const string AccessTokenPropertyName = "accessToken";

	private static readonly JsonEncodedText AccessTokenPropertyNameBytes = JsonEncodedText.Encode("accessToken");

	private const string AvailableTransportsPropertyName = "availableTransports";

	private static readonly JsonEncodedText AvailableTransportsPropertyNameBytes = JsonEncodedText.Encode("availableTransports");

	private const string TransportPropertyName = "transport";

	private static readonly JsonEncodedText TransportPropertyNameBytes = JsonEncodedText.Encode("transport");

	private const string TransferFormatsPropertyName = "transferFormats";

	private static readonly JsonEncodedText TransferFormatsPropertyNameBytes = JsonEncodedText.Encode("transferFormats");

	private const string ErrorPropertyName = "error";

	private static readonly JsonEncodedText ErrorPropertyNameBytes = JsonEncodedText.Encode("error");

	private const string NegotiateVersionPropertyName = "negotiateVersion";

	private static readonly JsonEncodedText NegotiateVersionPropertyNameBytes = JsonEncodedText.Encode("negotiateVersion");

	private static ReadOnlySpan<byte> ProtocolVersionPropertyNameBytes => "ProtocolVersion"u8;

	public static void WriteResponse(NegotiationResponse response, IBufferWriter<byte> output)
	{
		ReusableUtf8JsonWriter reusableUtf8JsonWriter = ReusableUtf8JsonWriter.Get(output);
		try
		{
			Utf8JsonWriter jsonWriter = reusableUtf8JsonWriter.GetJsonWriter();
			jsonWriter.WriteStartObject();
			if (!string.IsNullOrEmpty(response.Error))
			{
				jsonWriter.WriteString(ErrorPropertyNameBytes, response.Error);
				jsonWriter.WriteEndObject();
				jsonWriter.Flush();
				return;
			}
			jsonWriter.WriteNumber(NegotiateVersionPropertyNameBytes, response.Version);
			if (!string.IsNullOrEmpty(response.Url))
			{
				jsonWriter.WriteString(UrlPropertyNameBytes, response.Url);
			}
			if (!string.IsNullOrEmpty(response.AccessToken))
			{
				jsonWriter.WriteString(AccessTokenPropertyNameBytes, response.AccessToken);
			}
			if (!string.IsNullOrEmpty(response.ConnectionId))
			{
				jsonWriter.WriteString(ConnectionIdPropertyNameBytes, response.ConnectionId);
			}
			if (response.Version > 0 && !string.IsNullOrEmpty(response.ConnectionToken))
			{
				jsonWriter.WriteString(ConnectionTokenPropertyNameBytes, response.ConnectionToken);
			}
			jsonWriter.WriteStartArray(AvailableTransportsPropertyNameBytes);
			if (response.AvailableTransports != null)
			{
				int count = response.AvailableTransports.Count;
				for (int i = 0; i < count; i++)
				{
					AvailableTransport availableTransport = response.AvailableTransports[i];
					jsonWriter.WriteStartObject();
					if (availableTransport.Transport != null)
					{
						jsonWriter.WriteString(TransportPropertyNameBytes, availableTransport.Transport);
					}
					else
					{
						jsonWriter.WriteNull(TransportPropertyNameBytes);
					}
					jsonWriter.WriteStartArray(TransferFormatsPropertyNameBytes);
					if (availableTransport.TransferFormats != null)
					{
						int count2 = availableTransport.TransferFormats.Count;
						for (int j = 0; j < count2; j++)
						{
							jsonWriter.WriteStringValue(availableTransport.TransferFormats[j]);
						}
					}
					jsonWriter.WriteEndArray();
					jsonWriter.WriteEndObject();
				}
			}
			jsonWriter.WriteEndArray();
			jsonWriter.WriteEndObject();
			jsonWriter.Flush();
		}
		finally
		{
			ReusableUtf8JsonWriter.Return(reusableUtf8JsonWriter);
		}
	}

	public static NegotiationResponse ParseResponse(ReadOnlySpan<byte> content)
	{
		try
		{
			Utf8JsonReader reader = new Utf8JsonReader(content, isFinalBlock: true, default(JsonReaderState));
			reader.CheckRead();
			reader.EnsureObjectStart();
			string text = null;
			string text2 = null;
			string text3 = null;
			string accessToken = null;
			List<AvailableTransport> list = null;
			string text4 = null;
			int num = 0;
			bool flag = false;
			while (!flag && reader.CheckRead())
			{
				switch (reader.TokenType)
				{
				case JsonTokenType.PropertyName:
					if (reader.ValueTextEquals(UrlPropertyNameBytes.EncodedUtf8Bytes))
					{
						text3 = reader.ReadAsString("url");
					}
					else if (reader.ValueTextEquals(AccessTokenPropertyNameBytes.EncodedUtf8Bytes))
					{
						accessToken = reader.ReadAsString("accessToken");
					}
					else if (reader.ValueTextEquals(ConnectionIdPropertyNameBytes.EncodedUtf8Bytes))
					{
						text = reader.ReadAsString("connectionId");
					}
					else if (reader.ValueTextEquals(ConnectionTokenPropertyNameBytes.EncodedUtf8Bytes))
					{
						text2 = reader.ReadAsString("connectionToken");
					}
					else if (reader.ValueTextEquals(NegotiateVersionPropertyNameBytes.EncodedUtf8Bytes))
					{
						num = reader.ReadAsInt32("negotiateVersion").GetValueOrDefault();
					}
					else if (reader.ValueTextEquals(AvailableTransportsPropertyNameBytes.EncodedUtf8Bytes))
					{
						reader.CheckRead();
						reader.EnsureArrayStart();
						list = new List<AvailableTransport>();
						while (reader.CheckRead())
						{
							if (reader.TokenType == JsonTokenType.StartObject)
							{
								list.Add(ParseAvailableTransport(ref reader));
							}
							else if (reader.TokenType == JsonTokenType.EndArray)
							{
								break;
							}
						}
					}
					else if (reader.ValueTextEquals(ErrorPropertyNameBytes.EncodedUtf8Bytes))
					{
						text4 = reader.ReadAsString("error");
					}
					else
					{
						if (reader.ValueTextEquals(ProtocolVersionPropertyNameBytes))
						{
							throw new InvalidOperationException("Detected a connection attempt to an ASP.NET SignalR Server. This client only supports connecting to an ASP.NET Core SignalR Server. See https://aka.ms/signalr-core-differences for details.");
						}
						reader.Skip();
					}
					break;
				case JsonTokenType.EndObject:
					flag = true;
					break;
				default:
					throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading negotiation response JSON.");
				}
			}
			if (text3 == null && text4 == null)
			{
				if (text == null)
				{
					throw new InvalidDataException("Missing required property 'connectionId'.");
				}
				if (num > 0 && text2 == null)
				{
					throw new InvalidDataException("Missing required property 'connectionToken'.");
				}
				if (list == null)
				{
					throw new InvalidDataException("Missing required property 'availableTransports'.");
				}
			}
			return new NegotiationResponse
			{
				ConnectionId = text,
				ConnectionToken = text2,
				Url = text3,
				AccessToken = accessToken,
				AvailableTransports = list,
				Error = text4,
				Version = num
			};
		}
		catch (Exception innerException)
		{
			throw new InvalidDataException("Invalid negotiation response received.", innerException);
		}
	}

	private static AvailableTransport ParseAvailableTransport(ref Utf8JsonReader reader)
	{
		AvailableTransport availableTransport = new AvailableTransport();
		while (reader.CheckRead())
		{
			switch (reader.TokenType)
			{
			case JsonTokenType.PropertyName:
				if (reader.ValueTextEquals(TransportPropertyNameBytes.EncodedUtf8Bytes))
				{
					availableTransport.Transport = reader.ReadAsString("transport");
				}
				else if (reader.ValueTextEquals(TransferFormatsPropertyNameBytes.EncodedUtf8Bytes))
				{
					reader.CheckRead();
					reader.EnsureArrayStart();
					bool flag = false;
					availableTransport.TransferFormats = new List<string>();
					while (!flag && reader.CheckRead())
					{
						switch (reader.TokenType)
						{
						case JsonTokenType.String:
							availableTransport.TransferFormats.Add(reader.GetString());
							break;
						case JsonTokenType.EndArray:
							flag = true;
							break;
						default:
							throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading transfer formats JSON.");
						}
					}
				}
				else
				{
					reader.Skip();
				}
				break;
			case JsonTokenType.EndObject:
				if (availableTransport.Transport == null)
				{
					throw new InvalidDataException("Missing required property 'transport'.");
				}
				if (availableTransport.TransferFormats == null)
				{
					throw new InvalidDataException("Missing required property 'transferFormats'.");
				}
				return availableTransport;
			default:
				throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading available transport JSON.");
			}
		}
		throw new InvalidDataException("Unexpected end when reading JSON.");
	}
}
