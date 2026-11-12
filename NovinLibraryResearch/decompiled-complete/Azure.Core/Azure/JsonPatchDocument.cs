using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using Azure.Core.JsonPatch;
using Azure.Core.Serialization;

namespace Azure;

public class JsonPatchDocument
{
	private readonly ReadOnlyMemory<byte> _rawDocument;

	private readonly ObjectSerializer _serializer;

	private readonly Collection<JsonPatchOperation> _operations;

	public JsonPatchDocument()
		: this(default(ReadOnlyMemory<byte>))
	{
	}

	public JsonPatchDocument(ObjectSerializer serializer)
		: this(default(ReadOnlyMemory<byte>), serializer)
	{
	}

	public JsonPatchDocument(ReadOnlyMemory<byte> rawDocument)
		: this(rawDocument, new JsonObjectSerializer())
	{
	}

	public JsonPatchDocument(ReadOnlyMemory<byte> rawDocument, ObjectSerializer serializer)
	{
		_operations = new Collection<JsonPatchOperation>();
		_rawDocument = rawDocument;
		_serializer = serializer ?? throw new ArgumentNullException("serializer");
	}

	public void AppendAddRaw(string path, string rawJsonValue)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Add, path, null, rawJsonValue));
	}

	public void AppendAdd<T>(string path, T value)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Add, path, null, Serialize(value)));
	}

	public void AppendReplaceRaw(string path, string rawJsonValue)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Replace, path, null, rawJsonValue));
	}

	public void AppendReplace<T>(string path, T value)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Replace, path, null, Serialize(value)));
	}

	public void AppendCopy(string from, string path)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Copy, path, from, null));
	}

	public void AppendMove(string from, string path)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Move, path, from, null));
	}

	public void AppendRemove(string path)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Remove, path, null, null));
	}

	public void AppendTestRaw(string path, string rawJsonValue)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Test, path, null, rawJsonValue));
	}

	public void AppendTest<T>(string path, T value)
	{
		_operations.Add(new JsonPatchOperation(JsonPatchOperationKind.Test, path, null, Serialize(value)));
	}

	public ReadOnlyMemory<byte> ToBytes()
	{
		if (!_rawDocument.IsEmpty && _operations.Count == 0)
		{
			return _rawDocument;
		}
		using MemoryStream memoryStream = new MemoryStream();
		using (Utf8JsonWriter writer = new Utf8JsonWriter(memoryStream))
		{
			WriteTo(writer);
		}
		return MemoryExtensions.AsMemory(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
	}

	public override string ToString()
	{
		return Encoding.UTF8.GetString(ToBytes().ToArray());
	}

	private void WriteTo(Utf8JsonWriter writer)
	{
		writer.WriteStartArray();
		if (!_rawDocument.IsEmpty)
		{
			using JsonDocument jsonDocument = JsonDocument.Parse(_rawDocument);
			foreach (JsonElement item in jsonDocument.RootElement.EnumerateArray())
			{
				item.WriteTo(writer);
			}
		}
		foreach (JsonPatchOperation operation in _operations)
		{
			writer.WriteStartObject();
			writer.WriteString("op", operation.Kind.ToString());
			if (operation.From != null)
			{
				writer.WriteString("from", operation.From);
			}
			writer.WriteString("path", operation.Path);
			if (operation.RawJsonValue != null)
			{
				using JsonDocument jsonDocument2 = JsonDocument.Parse(operation.RawJsonValue);
				writer.WritePropertyName("value");
				jsonDocument2.WriteTo(writer);
			}
			writer.WriteEndObject();
		}
		writer.WriteEndArray();
	}

	private string Serialize<T>(T value)
	{
		using MemoryStream memoryStream = new MemoryStream();
		_serializer.Serialize(memoryStream, value, typeof(T), default(CancellationToken));
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}
}
