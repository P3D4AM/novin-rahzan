namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public interface IMetadataFactory
{
	IAsymmetricKeyFactory AsymmetricKey { get; }

	ICertificateFactory Certificate { get; }

	IColumnFactory Column { get; }

	IConstraintFactory Constraint { get; }

	ICredentialFactory Credential { get; }

	IDatabaseFactory Database { get; }

	IDataTypeFactory DataType { get; }

	IExecutionContextFactory ExecutionContext { get; }

	IIndexFactory Index { get; }

	ILoginFactory Login { get; }

	IParameterFactory Parameter { get; }

	IPermissionFactory Permission { get; }

	IResolvedSynonymFactory ResolvedSynonym { get; }

	IRoleFactory Role { get; }

	IScalarFactory Scalar { get; }

	ISchemaFactory Schema { get; }

	IServerFactory Server { get; }

	IStoredProcedureFactory StoredProcedure { get; }

	ISynonymFactory Synonym { get; }

	ITabularFactory Tabular { get; }

	ITriggerFactory Trigger { get; }

	IUserFactory User { get; }

	IUserDefinedFunctionFactory UserDefinedFunction { get; }

	IUserDefinedTypeFactory UserDefinedType { get; }

	IVariableFactory Variable { get; }
}
