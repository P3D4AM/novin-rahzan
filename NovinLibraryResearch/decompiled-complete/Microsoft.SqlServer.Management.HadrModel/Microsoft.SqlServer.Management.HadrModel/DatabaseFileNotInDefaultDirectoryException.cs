using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.HadrModel;

public class DatabaseFileNotInDefaultDirectoryException : HadrValidationErrorException
{
	public DatabaseFileNotInDefaultDirectoryException(string defaultDataFolder, string defaultLogFolder, IEnumerable<string> filesNotInDefaultDirectory)
		: base(string.Format(CultureInfo.InvariantCulture, Resource.DatabaseFileNotInDefaultDirectoryException, defaultDataFolder, defaultLogFolder, string.Join(CultureInfo.CurrentUICulture.TextInfo.ListSeparator, filesNotInDefaultDirectory)))
	{
	}
}
