using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public delegate TValue SharedTableEntryValueFactory<out TValue>(string tableName, string schema, IComparer<IUpdateEntry> comparer);
