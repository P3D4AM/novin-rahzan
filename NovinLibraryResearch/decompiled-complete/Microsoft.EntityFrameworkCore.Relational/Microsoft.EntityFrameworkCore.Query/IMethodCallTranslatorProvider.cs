using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public interface IMethodCallTranslatorProvider
{
	SqlExpression Translate(IModel model, SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments);
}
