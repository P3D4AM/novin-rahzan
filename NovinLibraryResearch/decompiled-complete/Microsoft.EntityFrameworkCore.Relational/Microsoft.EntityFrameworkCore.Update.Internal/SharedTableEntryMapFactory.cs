namespace Microsoft.EntityFrameworkCore.Update.Internal;

public delegate SharedTableEntryMap<TValue> SharedTableEntryMapFactory<TValue>(SharedTableEntryValueFactory<TValue> valueFactory);
