namespace Microsoft.EntityFrameworkCore.Storage;

public interface ITypeMappingSourcePlugin
{
	CoreTypeMapping FindMapping(in TypeMappingInfo mappingInfo);
}
