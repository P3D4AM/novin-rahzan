using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Sequence : IMutableSequence, ISequence, IConventionSequence
{
	private class SequenceData
	{
		public string Name { get; set; }

		public string Schema { get; set; }

		public long StartValue { get; set; }

		public int IncrementBy { get; set; }

		public long? MinValue { get; set; }

		public long? MaxValue { get; set; }

		public Type ClrType { get; set; }

		public bool IsCyclic { get; set; }

		public string Serialize()
		{
			StringBuilder stringBuilder = new StringBuilder();
			EscapeAndQuote(stringBuilder, Name);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, Schema);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, StartValue);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, IncrementBy);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, MinValue);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, MaxValue);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, ClrType.Name);
			stringBuilder.Append(", ");
			EscapeAndQuote(stringBuilder, IsCyclic);
			return stringBuilder.ToString();
		}

		public static SequenceData Deserialize([JetBrains.Annotations.NotNull] string value)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(value, "value");
			try
			{
				SequenceData sequenceData = new SequenceData();
				int position = 0;
				sequenceData.Name = ExtractValue(value, ref position);
				sequenceData.Schema = ExtractValue(value, ref position);
				sequenceData.StartValue = AsLong(ExtractValue(value, ref position)).Value;
				sequenceData.IncrementBy = (int)AsLong(ExtractValue(value, ref position)).Value;
				sequenceData.MinValue = AsLong(ExtractValue(value, ref position));
				sequenceData.MaxValue = AsLong(ExtractValue(value, ref position));
				sequenceData.ClrType = AsType(ExtractValue(value, ref position));
				sequenceData.IsCyclic = AsBool(ExtractValue(value, ref position));
				return sequenceData;
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(RelationalStrings.BadSequenceString, innerException);
			}
		}

		private static string ExtractValue(string value, ref int position)
		{
			position = value.IndexOf('\'', position) + 1;
			int num = value.IndexOf('\'', position);
			while (num + 1 < value.Length && value[num + 1] == '\'')
			{
				num = value.IndexOf('\'', num + 2);
			}
			string text = value.Substring(position, num - position).Replace("''", "'");
			position = num + 1;
			if (text.Length != 0)
			{
				return text;
			}
			return null;
		}

		private static long? AsLong(string value)
		{
			if (value != null)
			{
				return long.Parse(value, CultureInfo.InvariantCulture);
			}
			return null;
		}

		private static Type AsType(string value)
		{
			if (!(value == typeof(long).Name))
			{
				if (!(value == typeof(int).Name))
				{
					if (!(value == typeof(short).Name))
					{
						if (!(value == typeof(decimal).Name))
						{
							return typeof(byte);
						}
						return typeof(decimal);
					}
					return typeof(short);
				}
				return typeof(int);
			}
			return typeof(long);
		}

		private static bool AsBool(string value)
		{
			if (value != null)
			{
				return bool.Parse(value);
			}
			return false;
		}

		private static void EscapeAndQuote(StringBuilder builder, object value)
		{
			builder.Append("'");
			if (value != null)
			{
				builder.Append(value.ToString().Replace("'", "''"));
			}
			builder.Append("'");
		}
	}

	private readonly IModel _model;

	private readonly string _annotationName;

	public static readonly Type DefaultClrType = typeof(long);

	public const int DefaultIncrementBy = 1;

	public const int DefaultStartValue = 1;

	public static readonly long? DefaultMaxValue = null;

	public static readonly long? DefaultMinValue = null;

	public static readonly bool DefaultIsCyclic = false;

	public virtual IMutableModel Model => (IMutableModel)_model;

	IConventionSequenceBuilder IConventionSequence.Builder => new SequenceBuilder(this);

	public virtual string Name => GetData().Name;

	public virtual string Schema => GetData().Schema ?? Model.GetDefaultSchema();

	public virtual long StartValue
	{
		get
		{
			return GetData().StartValue;
		}
		set
		{
			SetStartValue(value, ConfigurationSource.Explicit);
		}
	}

	public virtual int IncrementBy
	{
		get
		{
			return GetData().IncrementBy;
		}
		set
		{
			SetIncrementBy(value, ConfigurationSource.Explicit);
		}
	}

	public virtual long? MinValue
	{
		get
		{
			return GetData().MinValue;
		}
		set
		{
			SetMinValue(value, ConfigurationSource.Explicit);
		}
	}

	public virtual long? MaxValue
	{
		get
		{
			return GetData().MaxValue;
		}
		set
		{
			SetMaxValue(value, ConfigurationSource.Explicit);
		}
	}

	public static IReadOnlyCollection<Type> SupportedTypes { get; } = (IReadOnlyCollection<Type>)(object)new Type[5]
	{
		typeof(byte),
		typeof(long),
		typeof(int),
		typeof(short),
		typeof(decimal)
	};


	public virtual Type ClrType
	{
		get
		{
			return GetData().ClrType;
		}
		set
		{
			SetClrType(value, ConfigurationSource.Explicit);
		}
	}

	public virtual bool IsCyclic
	{
		get
		{
			return GetData().IsCyclic;
		}
		set
		{
			SetIsCyclic(value, ConfigurationSource.Explicit);
		}
	}

	IConventionModel IConventionSequence.Model => (IConventionModel)Model;

	IModel ISequence.Model => _model;

	public Sequence([JetBrains.Annotations.NotNull] IMutableModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		_model = model;
		_annotationName = BuildSequenceAnnotationName("Relational:Sequence:", name, schema);
		string value = new SequenceData
		{
			Name = name,
			Schema = schema,
			ClrType = DefaultClrType,
			IncrementBy = 1,
			StartValue = 1L
		}.Serialize();
		if (configurationSource == ConfigurationSource.Explicit)
		{
			model.AddAnnotation(_annotationName, value);
		}
		else
		{
			((IConventionModel)model).AddAnnotation(_annotationName, value, configurationSource == ConfigurationSource.DataAnnotation);
		}
	}

	public Sequence([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] string annotationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotationName, "annotationName");
		_model = model;
		_annotationName = annotationName;
	}

	public static IEnumerable<Sequence> GetSequences([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] string annotationPrefix)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotationPrefix, "annotationPrefix");
		return from a in model.GetAnnotations()
			where a.Name.StartsWith(annotationPrefix, StringComparison.Ordinal)
			select new Sequence(model, a.Name);
	}

	public virtual void SetStartValue(long? startValue, ConfigurationSource configurationSource)
	{
		SequenceData data = GetData();
		data.StartValue = startValue ?? 1;
		SetData(data);
	}

	public virtual ConfigurationSource? GetStartValueConfigurationSource()
	{
		return ConfigurationSource.Explicit;
	}

	public virtual void SetIncrementBy(int? incrementBy, ConfigurationSource configurationSource)
	{
		SequenceData data = GetData();
		data.IncrementBy = incrementBy ?? 1;
		SetData(data);
	}

	public virtual ConfigurationSource? GetIncrementByConfigurationSource()
	{
		return ConfigurationSource.Explicit;
	}

	public virtual void SetMinValue(long? minValue, ConfigurationSource configurationSource)
	{
		SequenceData data = GetData();
		data.MinValue = minValue ?? DefaultMinValue;
		SetData(data);
	}

	public virtual ConfigurationSource? GetMinValueConfigurationSource()
	{
		return ConfigurationSource.Explicit;
	}

	public virtual void SetMaxValue(long? maxValue, ConfigurationSource configurationSource)
	{
		SequenceData data = GetData();
		data.MaxValue = maxValue ?? DefaultMaxValue;
		SetData(data);
	}

	public virtual ConfigurationSource? GetMaxValueConfigurationSource()
	{
		return ConfigurationSource.Explicit;
	}

	public virtual void SetClrType(Type clrType, ConfigurationSource configurationSource)
	{
		if ((object)clrType == null)
		{
			clrType = DefaultClrType;
		}
		if (!SupportedTypes.Contains(clrType))
		{
			throw new ArgumentException(RelationalStrings.BadSequenceType);
		}
		SequenceData data = GetData();
		data.ClrType = clrType;
		SetData(data);
	}

	public virtual ConfigurationSource? GetClrTypeConfigurationSource()
	{
		return ConfigurationSource.Explicit;
	}

	public virtual void SetIsCyclic(bool? cyclic, ConfigurationSource configurationSource)
	{
		SequenceData data = GetData();
		data.IsCyclic = cyclic ?? DefaultIsCyclic;
		SetData(data);
	}

	public virtual ConfigurationSource? GetIsCyclicConfigurationSource()
	{
		return ConfigurationSource.Explicit;
	}

	public virtual ConfigurationSource GetConfigurationSource()
	{
		return ((IConventionModel)Model).FindAnnotation(_annotationName).GetConfigurationSource();
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		((Model)Model).FindAnnotation(_annotationName).UpdateConfigurationSource(configurationSource);
	}

	private SequenceData GetData()
	{
		return SequenceData.Deserialize((string)Model[_annotationName]);
	}

	private void SetData(SequenceData data)
	{
		Model[_annotationName] = data.Serialize();
	}

	public static ISequence FindSequence([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema)
	{
		string text = BuildSequenceAnnotationName("Relational:Sequence:", name, schema);
		if (model[text] != null)
		{
			return new Sequence(model, text);
		}
		return null;
	}

	public static ISequence RemoveSequence([JetBrains.Annotations.NotNull] IMutableModel model, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] string schema = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(schema, "schema");
		string text = BuildSequenceAnnotationName("Relational:Sequence:", name, schema);
		if (model.RemoveAnnotation(text) != null)
		{
			return new Sequence(model, text);
		}
		return null;
	}

	private static string BuildSequenceAnnotationName(string annotationPrefix, string name, string schema)
	{
		return annotationPrefix + schema + "." + name;
	}

	void IConventionSequence.SetStartValue(long? startValue, bool fromDataAnnotation)
	{
		SetStartValue(startValue, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionSequence.SetIncrementBy(int? incrementBy, bool fromDataAnnotation)
	{
		SetIncrementBy(incrementBy, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionSequence.SetMinValue(long? minValue, bool fromDataAnnotation)
	{
		SetMinValue(minValue, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionSequence.SetMaxValue(long? maxValue, bool fromDataAnnotation)
	{
		SetMaxValue(maxValue, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionSequence.SetClrType(Type clrType, bool fromDataAnnotation)
	{
		SetClrType(clrType, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	void IConventionSequence.SetIsCyclic(bool? cyclic, bool fromDataAnnotation)
	{
		SetIsCyclic(cyclic, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
