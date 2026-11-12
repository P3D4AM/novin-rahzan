using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalMemberTranslatorProvider : IMemberTranslatorProvider
{
	private readonly List<IMemberTranslator> _plugins = new List<IMemberTranslator>();

	private readonly List<IMemberTranslator> _translators = new List<IMemberTranslator>();

	public RelationalMemberTranslatorProvider(RelationalMemberTranslatorProviderDependencies dependencies)
	{
		_plugins.AddRange(dependencies.Plugins.SelectMany((IMemberTranslatorPlugin p) => p.Translators));
		_translators.AddRange(new NullableMemberTranslator[1]
		{
			new NullableMemberTranslator(dependencies.SqlExpressionFactory)
		});
	}

	public virtual SqlExpression Translate(SqlExpression instance, MemberInfo member, Type returnType)
	{
		return (from t in _plugins.Concat(_translators)
			select t.Translate(instance, member, returnType)).FirstOrDefault((SqlExpression t) => t != null);
	}

	protected virtual void AddTranslators(IEnumerable<IMemberTranslator> translators)
	{
		_translators.InsertRange(0, translators);
	}
}
