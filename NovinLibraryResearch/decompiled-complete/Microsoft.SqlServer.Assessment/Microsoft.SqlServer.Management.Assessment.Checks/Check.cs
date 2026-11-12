using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Management.Assessment.Configuration;
using Microsoft.SqlServer.Management.Assessment.Expressions;
using Microsoft.SqlServer.Management.Assessment.Logics;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Checks;

[DebuggerDisplay("Check {Id}")]
public class Check : ICheck
{
	private string name = string.Empty;

	public string Description { get; set; }

	public string DisplayName { get; set; }

	public string Id
	{
		get
		{
			return name;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException(Resources.ErrorValueIsAWhiteSpaceString, "value");
			}
			if (char.IsWhiteSpace(value[value.Length - 1]))
			{
				throw new ArgumentException(Resources.ErrorValueEndsWithWhiteSpace, "value");
			}
			if (char.IsWhiteSpace(value[0]))
			{
				throw new ArgumentException(Resources.ErrorValueStartsWithWhiteSpace, "value");
			}
			name = value;
		}
	}

	public string HelpLink { get; set; }

	public List<ProbeReference> Probes { get; } = new List<ProbeReference>();


	public bool Enabled { get; set; } = true;


	public SeverityLevel Level { get; set; } = SeverityLevel.Warning;


	public HashSet<string> Tags { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);


	public string Message
	{
		get
		{
			return AssessmentMessage?.ToString() ?? string.Empty;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			AssessmentMessage = new ParameterizedText
			{
				Template = value
			};
		}
	}

	public Condition Condition { get; set; }

	public IPattern<ISqlObjectLocator> Target { get; set; }

	public string OriginName { get; set; }

	public Version OriginVersion { get; set; }

	public IDictionary<string, Expression> Parameters { get; } = new Dictionary<string, Expression>();


	internal ParameterizedText AssessmentMessage { get; set; } = ParameterizedText.Empty;


	public ILogicsProvider Logics { get; set; }

	internal IDictionary<string, Expression> Locals { get; set; }

	public override string ToString()
	{
		return DisplayName ?? base.ToString();
	}

	internal virtual IAssessmentResult GetRecommendations(ISqlObjectLocator target, PermutationContext data)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		try
		{
			if (Locals != null)
			{
				foreach (KeyValuePair<string, Expression> local in Locals)
				{
					object value = local.Value.Evaluate(data);
					data.ProbeContext.Data[local.Key] = Utils.ToLiteral(value);
				}
			}
			object value2 = Condition?.Evaluate(data);
			if (value2.Normalize<bool>())
			{
				return null;
			}
		}
		catch (SqlAssessmentEvaluationException innerException)
		{
			return new CheckError(this, Resources.ErrorConditionEvaluationFailed, innerException, target.Type, target.Urn);
		}
		string message = AssessmentMessage.ToString(data);
		return new AssessmentNote(this, message, target.Urn, target.Type, HelpLink, Level);
	}
}
