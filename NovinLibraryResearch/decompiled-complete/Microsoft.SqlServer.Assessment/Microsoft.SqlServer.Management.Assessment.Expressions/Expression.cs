using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

public abstract class Expression
{
	internal const char VariablePrefix = '@';

	public static implicit operator Expression(int value)
	{
		return new Literal<decimal>(value);
	}

	public static implicit operator Expression(decimal value)
	{
		return new Literal<decimal>(value);
	}

	public static implicit operator Expression(string value)
	{
		return new Literal<string>(value);
	}

	public static implicit operator Expression(DateTime value)
	{
		return new Literal<DateTime>(value);
	}

	public static implicit operator Expression(bool value)
	{
		return new Literal<bool>(value);
	}

	internal object Evaluate(IEvaluationContext data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		try
		{
			if (!data.CallStack.Add(this))
			{
				throw new SqlAssessmentEvaluationException(Resources.ErrorRecursionDetected);
			}
			try
			{
				return EvaluateCore(data);
			}
			catch (Exception ex)
			{
				if (ex is StackOverflowException || ex is OutOfMemoryException || ex is SqlAssessmentEvaluationException)
				{
					throw;
				}
				throw new SqlAssessmentEvaluationException(Resources.ErrorEvalOther, ex);
			}
		}
		finally
		{
			data.CallStack.Remove(this);
		}
	}

	internal virtual object EvaluateCore(IEvaluationContext data)
	{
		return null;
	}

	internal abstract IEnumerable<string> GetNames();
}
