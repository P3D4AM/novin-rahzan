using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalMethodCallTranslatorProvider : IMethodCallTranslatorProvider
{
	private readonly List<IMethodCallTranslator> _plugins = new List<IMethodCallTranslator>();

	private readonly List<IMethodCallTranslator> _translators = new List<IMethodCallTranslator>();

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	public RelationalMethodCallTranslatorProvider([JetBrains.Annotations.NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies)
	{
		_plugins.AddRange(dependencies.Plugins.SelectMany((IMethodCallTranslatorPlugin p) => p.Translators));
		ISqlExpressionFactory sqlExpressionFactory = dependencies.SqlExpressionFactory;
		_translators.AddRange(new IMethodCallTranslator[7]
		{
			new EqualsTranslator(sqlExpressionFactory),
			new StringMethodTranslator(sqlExpressionFactory),
			new ContainsTranslator(sqlExpressionFactory),
			new LikeTranslator(sqlExpressionFactory),
			new EnumHasFlagTranslator(sqlExpressionFactory),
			new GetValueOrDefaultTranslator(sqlExpressionFactory),
			new ComparisonTranslator(sqlExpressionFactory)
		});
		_sqlExpressionFactory = sqlExpressionFactory;
	}

	public virtual SqlExpression Translate(IModel model, SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments)
	{
		IDbFunction dbFunction = model.FindDbFunction(method);
		if (dbFunction != null)
		{
			return dbFunction.Translation?.Invoke(arguments.Select((SqlExpression e) => _sqlExpressionFactory.ApplyDefaultTypeMapping(e)).ToList()) ?? _sqlExpressionFactory.Function(dbFunction.Schema, dbFunction.Name, arguments, method.ReturnType);
		}
		return (from t in _plugins.Concat(_translators)
			select t.Translate(instance, method, arguments)).FirstOrDefault((SqlExpression t) => t != null);
	}

	protected virtual void AddTranslators(IEnumerable<IMethodCallTranslator> translators)
	{
		_translators.InsertRange(0, translators);
	}
}
