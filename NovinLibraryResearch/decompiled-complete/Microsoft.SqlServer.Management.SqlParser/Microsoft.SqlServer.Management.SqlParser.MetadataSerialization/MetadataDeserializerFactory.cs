using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal sealed class MetadataDeserializerFactory
{
	private readonly DeserializationModel model;

	public MetadataDeserializerFactory(DeserializationModel model)
	{
		this.model = model;
	}

	public IMetadataObject CreateMetadataObject(object[] values, TypeInfo typeInfo)
	{
		return typeInfo.Id switch
		{
			15 => new ApplicationRole(model, values), 
			16 => new AsymmetricKey(model, values), 
			18 => new Certificate(model, values), 
			19 => new CheckConstraint(model, values), 
			20 => new ClrDataType(model, values), 
			21 => new Collation(model, values), 
			22 => new Column(model, values), 
			24 => new Credential(model, values), 
			25 => new CursorDataType(model, values), 
			26 => new CursorParameter(model, values), 
			27 => new CursorVariable(model, values), 
			29 => new Database(model, values), 
			30 => new DatabaseDdlTrigger(model, values), 
			33 => new DatabasePermission(model, values), 
			35 => new DatabaseRole(model, values), 
			37 => new DefaultConstraint(model, values), 
			38 => new DmlTrigger(model, values), 
			39 => new ExecutionContext(model, values), 
			40 => new ExtendedStoredProcedure(model, values), 
			41 => new FileGroup(model, values), 
			42 => new ForeignKeyColumn(model, values), 
			43 => new ForeignKeyConstraint(model, values), 
			47 => new IndexedColumn(model, values), 
			49 => new Login(model, values), 
			88 => new PartitionScheme(model, values), 
			90 => new PrimaryKeyConstraint(model, values), 
			91 => new RelationalIndex(model, values), 
			93 => new ScalarDataType(model, values), 
			95 => new ScalarParameter(model, values), 
			96 => new ScalarValuedFunction(model, values), 
			97 => new ScalarVariable(model, values), 
			98 => new Schema(model, values), 
			100 => new Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals.Server(model, values), 
			101 => new ServerDdlTrigger(model, values), 
			103 => new SpatialIndex(model, values), 
			104 => new Statistics(model, values), 
			105 => new StoredProcedure(model, values), 
			106 => new Synonym(model, values), 
			107 => new SystemClrDataType(model, values), 
			108 => new SystemDataType(model, values), 
			109 => new Table(model, values), 
			110 => new TableDataType(model, values), 
			111 => new TableParameter(model, values), 
			112 => new TableValuedFunction(model, values), 
			113 => new TableVariable(model, values), 
			118 => new UdtDataMember(model, values), 
			120 => new UdtMethod(model, values), 
			121 => new UniqueConstraint(model, values), 
			123 => new User(model, values), 
			124 => new UserDefinedAggregate(model, values), 
			125 => new UserDefinedClrType(model, values), 
			126 => new UserDefinedDataType(model, values), 
			129 => new UserDefinedTableType(model, values), 
			131 => new View(model, values), 
			132 => new VoidDataType(model, values), 
			133 => new XmlDataType(model, values), 
			136 => new XmlIndex(model, values), 
			_ => null, 
		};
	}
}
