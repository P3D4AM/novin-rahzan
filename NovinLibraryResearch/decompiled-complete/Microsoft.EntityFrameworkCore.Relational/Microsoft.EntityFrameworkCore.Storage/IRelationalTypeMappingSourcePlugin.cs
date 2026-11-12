namespace Microsoft.EntityFrameworkCore.Storage;

public interface IRelationalTypeMappingSourcePlugin
{
	RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo);
}
