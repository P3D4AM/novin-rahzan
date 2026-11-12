using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.Core.Serialization;

public class JsonObjectSerializer : ObjectSerializer, IMemberNameConverter
{
	private readonly ConcurrentDictionary<MemberInfo, string?> _cache;

	private readonly JsonSerializerOptions _options;

	public JsonObjectSerializer()
		: this(new JsonSerializerOptions())
	{
	}

	public JsonObjectSerializer(JsonSerializerOptions options)
	{
		_options = options ?? throw new ArgumentNullException("options");
		_cache = new ConcurrentDictionary<MemberInfo, string>();
	}

	public override void Serialize(Stream stream, object? value, Type inputType, CancellationToken cancellationToken)
	{
		byte[] array = JsonSerializer.SerializeToUtf8Bytes(value, inputType, _options);
		stream.Write(array, 0, array.Length);
	}

	public override async ValueTask SerializeAsync(Stream stream, object? value, Type inputType, CancellationToken cancellationToken)
	{
		await JsonSerializer.SerializeAsync(stream, value, inputType, _options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override object Deserialize(Stream stream, Type returnType, CancellationToken cancellationToken)
	{
		using MemoryStream memoryStream = new MemoryStream();
		stream.CopyTo(memoryStream);
		return JsonSerializer.Deserialize(memoryStream.ToArray(), returnType, _options);
	}

	public override async ValueTask<object> DeserializeAsync(Stream stream, Type returnType, CancellationToken cancellationToken)
	{
		return await JsonSerializer.DeserializeAsync(stream, returnType, _options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	string? IMemberNameConverter.ConvertMemberName(MemberInfo member)
	{
		Argument.AssertNotNull(member, "member");
		return _cache.GetOrAdd(member, delegate(MemberInfo m)
		{
			if (m is PropertyInfo propertyInfo)
			{
				if (propertyInfo.GetIndexParameters().Length != 0)
				{
					return (string?)null;
				}
				MethodInfo? getMethod = propertyInfo.GetMethod;
				if ((object)getMethod == null || !getMethod.IsPublic)
				{
					MethodInfo? setMethod = propertyInfo.SetMethod;
					if ((object)setMethod == null || !setMethod.IsPublic)
					{
						goto IL_0059;
					}
				}
				if (propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() != null)
				{
					return (string?)null;
				}
				if (propertyInfo.GetCustomAttribute<JsonExtensionDataAttribute>() != null)
				{
					return (string?)null;
				}
				return GetPropertyName(propertyInfo);
			}
			goto IL_0059;
			IL_0059:
			return (string?)null;
		});
	}

	private string GetPropertyName(MemberInfo memberInfo)
	{
		JsonPropertyNameAttribute customAttribute = memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>(inherit: false);
		if (customAttribute != null)
		{
			return customAttribute.Name ?? throw new InvalidOperationException($"The JSON property name for '{memberInfo.DeclaringType}.{memberInfo.Name}' cannot be null.");
		}
		if (_options.PropertyNamingPolicy != null)
		{
			return _options.PropertyNamingPolicy.ConvertName(memberInfo.Name) ?? throw new InvalidOperationException($"The JSON property name for '{memberInfo.DeclaringType}.{memberInfo.Name}' cannot be null.");
		}
		return memberInfo.Name;
	}
}
