using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure.Internal;

public static class DbContextOptionsExtensions
{
	public static string BuildOptionsFragment([JetBrains.Annotations.NotNull] this IDbContextOptions contextOptions)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (IDbContextOptionsExtension extension in contextOptions.Extensions)
		{
			stringBuilder.Append(extension.Info.LogFragment);
		}
		string text = stringBuilder.ToString();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return "None";
	}
}
