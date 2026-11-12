using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Reflection;

namespace Serilog.Exceptions.Destructurers;

public class ReflectionBasedDestructurer : IExceptionDestructurer
{
	private const string IdLabel = "$id";

	private const string RefLabel = "$ref";

	private const string CyclicReferenceMessage = "Cyclic reference";

	private const string IQueryableDestructureSkippedMessage = "IQueryable skipped";

	private readonly int destructuringDepth;

	private readonly ReflectionInfoExtractor reflectionInfoExtractor = new ReflectionInfoExtractor();

	public Type[] TargetTypes => new Type[1] { typeof(Exception) };

	public ReflectionBasedDestructurer(int destructuringDepth)
	{
		if (destructuringDepth <= 0)
		{
			throw new ArgumentOutOfRangeException("destructuringDepth", destructuringDepth, Resources.DestructuringDepthMustBeMoreThanZero);
		}
		this.destructuringDepth = destructuringDepth;
	}

	public void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		if (propertiesBag == null)
		{
			throw new ArgumentNullException("propertiesBag");
		}
		if (destructureException == null)
		{
			throw new ArgumentNullException("destructureException");
		}
		int nextCyclicRefId = 1;
		Dictionary<object, IDictionary<string, object?>> destructuredObjects = new Dictionary<object, IDictionary<string, object>>();
		ExceptionDestructurer.DestructureCommonExceptionProperties(exception, propertiesBag, destructureException, (IDictionary data) => DestructureValueDictionary(data, 1, destructuredObjects, ref nextCyclicRefId));
		ReflectionInfo orCreateReflectionInfo = reflectionInfoExtractor.GetOrCreateReflectionInfo(exception.GetType());
		AppendProperties(exception, orCreateReflectionInfo.PropertiesExceptBaseOnes, propertiesBag.AddProperty, destructuredObjects, 0, ref nextCyclicRefId);
		AppendTypeIfPossible(propertiesBag, exception.GetType());
	}

	private static string? GetOrGenerateRefId(ref int nextCyclicRefId, IDictionary<string, object?> destructuredObject)
	{
		string result;
		if (destructuredObject.TryGetValue("$id", out object value))
		{
			result = (string)value;
		}
		else
		{
			int num = nextCyclicRefId;
			nextCyclicRefId++;
			result = (string)(destructuredObject["$id"] = num.ToString(CultureInfo.InvariantCulture));
		}
		return result;
	}

	private static object DestructureUri(Uri value)
	{
		return value.ToString();
	}

	private static void AppendTypeIfPossible(IExceptionPropertiesBag propertiesBag, Type valueType)
	{
		if (propertiesBag.ContainsProperty("Type"))
		{
			if (!propertiesBag.ContainsProperty("$Type"))
			{
				propertiesBag.AddProperty("$Type", valueType.FullName);
			}
		}
		else
		{
			propertiesBag.AddProperty("Type", valueType.FullName);
		}
	}

	private void AppendProperties(object value, ReflectionPropertyInfo[] reflectionPropertyInfos, Action<string, object?> addPropertyAction, IDictionary<object, IDictionary<string, object?>> destructuredObjects, int level, ref int nextCyclicRefId)
	{
		foreach (ReflectionPropertyInfo reflectionPropertyInfo in reflectionPropertyInfos)
		{
			try
			{
				object value2 = reflectionPropertyInfo.Getter(value);
				int nextCyclicRefId2 = nextCyclicRefId;
				object arg = DestructureValue(value2, level + 1, destructuredObjects, ref nextCyclicRefId2);
				nextCyclicRefId = nextCyclicRefId2;
				addPropertyAction(reflectionPropertyInfo.Name, arg);
			}
			catch (TargetInvocationException ex)
			{
				Exception innerException = ex.InnerException;
				if (innerException != null)
				{
					addPropertyAction(reflectionPropertyInfo.Name, "threw " + innerException.GetType().FullName + ": " + innerException.Message);
				}
			}
			catch (Exception ex2)
			{
				addPropertyAction(reflectionPropertyInfo.Name, "threw " + ex2.GetType().FullName + ": " + ex2.Message);
			}
		}
	}

	private object? DestructureValue(object? value, int level, IDictionary<object, IDictionary<string, object?>> destructuredObjects, ref int nextCyclicRefId)
	{
		if (value == null)
		{
			return null;
		}
		Type type = value.GetType();
		TypeInfo typeInfo = type.GetTypeInfo();
		if (value is MemberInfo)
		{
			return value;
		}
		if (type.GetTypeCode() != TypeCode.Object || typeInfo.IsValueType)
		{
			return value;
		}
		if (level > destructuringDepth)
		{
			return value;
		}
		if (value is IDictionary value2)
		{
			return DestructureValueDictionary(value2, level, destructuredObjects, ref nextCyclicRefId);
		}
		if (value is IQueryable)
		{
			return "IQueryable skipped";
		}
		if (value is IEnumerable value3)
		{
			return DestructureValueEnumerable(value3, level, destructuredObjects, ref nextCyclicRefId);
		}
		if (value is Uri value4)
		{
			return DestructureUri(value4);
		}
		if (value is CancellationToken cancellationToken)
		{
			return OperationCanceledExceptionDestructurer.DestructureCancellationToken(in cancellationToken);
		}
		if (value is Task task)
		{
			return DestructureTask(task, level, destructuredObjects, ref nextCyclicRefId);
		}
		return DestructureObject(value, type, level, destructuredObjects, ref nextCyclicRefId);
	}

	private object DestructureValueEnumerable(IEnumerable value, int level, IDictionary<object, IDictionary<string, object?>> destructuredObjects, ref int nextCyclicRefId)
	{
		if (destructuredObjects.ContainsKey(value))
		{
			return new Dictionary<string, object> { { "$ref", "Cyclic reference" } };
		}
		destructuredObjects.Add(value, new Dictionary<string, object>());
		List<object> list = new List<object>();
		foreach (object item in value.Cast<object>())
		{
			list.Add(DestructureValue(item, level + 1, destructuredObjects, ref nextCyclicRefId));
		}
		return list;
	}

	private object DestructureValueDictionary(IDictionary value, int level, IDictionary<object, IDictionary<string, object?>> destructuredObjects, ref int nextCyclicRefId)
	{
		if (destructuredObjects.TryGetValue(value, out IDictionary<string, object> value2))
		{
			string orGenerateRefId = GetOrGenerateRefId(ref nextCyclicRefId, value2);
			return new Dictionary<string, object> { { "$ref", orGenerateRefId } };
		}
		Dictionary<string, object> dictionary = value.ToStringObjectDictionary();
		destructuredObjects.Add(value, dictionary);
		foreach (KeyValuePair<string, object> item in dictionary.ToDictionary((KeyValuePair<string, object> k) => k.Key, (KeyValuePair<string, object> v) => v.Value))
		{
			dictionary[item.Key] = DestructureValue(item.Value, level + 1, destructuredObjects, ref nextCyclicRefId);
		}
		return dictionary;
	}

	private IDictionary<string, object?> DestructureObject(object value, Type valueType, int level, IDictionary<object, IDictionary<string, object?>> destructuredObjects, ref int nextCyclicRefId)
	{
		if (destructuredObjects.TryGetValue(value, out IDictionary<string, object> value2))
		{
			string orGenerateRefId = GetOrGenerateRefId(ref nextCyclicRefId, value2);
			return new Dictionary<string, object> { { "$ref", orGenerateRefId } };
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		destructuredObjects.Add(value, dictionary);
		ReflectionPropertyInfo[] properties = reflectionInfoExtractor.GetOrCreateReflectionInfo(valueType).Properties;
		foreach (ReflectionPropertyInfo reflectionPropertyInfo in properties)
		{
			try
			{
				object value3 = reflectionPropertyInfo.Getter(value);
				object value4 = DestructureValue(value3, level + 1, destructuredObjects, ref nextCyclicRefId);
				dictionary.Add(reflectionPropertyInfo.Name, value4);
			}
			catch (TargetInvocationException ex)
			{
				Exception innerException = ex.InnerException;
				if (innerException != null)
				{
					dictionary.Add(reflectionPropertyInfo.Name, "threw " + innerException.GetType().FullName + ": " + innerException.Message);
				}
			}
			catch (Exception ex2)
			{
				dictionary.Add(reflectionPropertyInfo.Name, "threw " + ex2.GetType().FullName + ": " + ex2.Message);
			}
		}
		return dictionary;
	}

	private object DestructureTask(Task task, int level, IDictionary<object, IDictionary<string, object?>> destructuredObjects, ref int nextCyclicRefId)
	{
		if (destructuredObjects.TryGetValue(task, out IDictionary<string, object> value))
		{
			string orGenerateRefId = GetOrGenerateRefId(ref nextCyclicRefId, value);
			return new SortedList<string, object> { { "$ref", orGenerateRefId } };
		}
		SortedList<string, object> sortedList = new SortedList<string, object>();
		destructuredObjects.Add(task, sortedList);
		sortedList["Id"] = task.Id;
		sortedList["Status"] = task.Status.ToString("G");
		sortedList["CreationOptions"] = task.CreationOptions.ToString("F");
		if (task.IsFaulted)
		{
			sortedList["Exception"] = DestructureValue(task.Exception, level, destructuredObjects, ref nextCyclicRefId);
		}
		return sortedList;
	}
}
