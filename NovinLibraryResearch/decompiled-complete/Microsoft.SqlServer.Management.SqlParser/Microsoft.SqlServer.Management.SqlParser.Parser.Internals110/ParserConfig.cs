using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals110;

internal class ParserConfig : Babel.ParserGenerator.ParserConfig
{
	public override OptLookup GetOptLookupByContext(TokenTransformContext context)
	{
		return context switch
		{
			TokenTransformContext.CREATE_DROP_ALTER => OptLookup.OPT_CREATE_DROP_ALTER_SUBTOKENS, 
			TokenTransformContext.ADD => OptLookup.OPT_ADD_SUBTOKENS, 
			TokenTransformContext.AFTER_END => OptLookup.OPT_AFTER_END_SUBTOKENS, 
			TokenTransformContext.AFTER_WITH => OptLookup.OPT_AFTER_WITH_SUBTOKENS, 
			TokenTransformContext.AFTER_WITH_EXEC_OPTION => OptLookup.OPT_AFTER_WITH_EXEC_OPTION_SUBTOKENS, 
			TokenTransformContext.CREATE_DB_OPT => OptLookup.OPT_CREATE_DB_OPT_SUBTOKENS, 
			TokenTransformContext.CREATE_DB_TYPE => OptLookup.OPT_CREATE_DB_TYPE_SUBTOKENS, 
			TokenTransformContext.ALTER_DB_OPT => OptLookup.OPT_ALTER_DB_OPT_SUBTOKENS, 
			TokenTransformContext.DUMP_OPT => OptLookup.OPT_DUMP_OPT_SUBTOKENS, 
			TokenTransformContext.DUMP_ENCRYPTION_OPT => OptLookup.OPT_DUMP_ENCRYPTION_OPT_SUBTOKENS, 
			TokenTransformContext.LOAD_OPT => OptLookup.OPT_LOAD_OPT_SUBTOKENS, 
			TokenTransformContext.TAB_OPT_NAME => OptLookup.OPT_TAB_OPT_NAME_SUBTOKENS, 
			TokenTransformContext.INDEX_SORT_OPT => OptLookup.OPT_INDEX_SORT_OPT_SUBTOKENS, 
			TokenTransformContext.OPT_STORAGE_ID => OptLookup.OPT_OPT_STORAGE_ID_SUBTOKENS, 
			TokenTransformContext.IND_OPTIONS => OptLookup.OPT_IND_OPTIONS_SUBTOKENS, 
			TokenTransformContext.IND_ACTIONS => OptLookup.OPT_IND_ACTIONS_SUBTOKENS, 
			TokenTransformContext.ALTTAB_ACTIONS => OptLookup.OPT_ALTTAB_ACTIONS_SUBTOKENS, 
			TokenTransformContext.SEQUENCE_OPTIONS => OptLookup.OPT_SEQUENCE_OPTIONS_SUBTOKENS, 
			TokenTransformContext.ALTTAB_ENABLE_DISABLE => OptLookup.OPT_ALTTAB_ENABLE_DISABLE_SUBTOKENS, 
			TokenTransformContext.AFFINITY_TYPE => OptLookup.OPT_AFFINITY_TYPE_SUBTOKENS, 
			TokenTransformContext.POOL_AFFINITY_TYPE => OptLookup.OPT_POOL_AFFINITY_TYPE_SUBTOKENS, 
			TokenTransformContext.EXTERNAL_POOL_AFFINITY_TYPE => OptLookup.OPT_EXTERNAL_POOL_AFFINITY_TYPE_SUBTOKENS, 
			TokenTransformContext.EXTERNAL_LANGUAGE_FILE_SPEC_OPT => OptLookup.OPT_EXTERNAL_LANGUAGE_FILE_SPEC_OPT_SUBTOKENS, 
			TokenTransformContext.ALTER_EXTERNAL_LANGUAGE_OPT => OptLookup.OPT_ALTER_EXTERNAL_LANGUAGE_OPT_SUBTOKENS, 
			TokenTransformContext.EXTERNAL_LIBRARY_FILE_SPEC_OPT => OptLookup.OPT_EXTERNAL_LIBRARY_FILE_SPEC_OPT_SUBTOKENS, 
			TokenTransformContext.ALTER_EXTERNAL_LIBRARY_OPT => OptLookup.OPT_ALTER_EXTERNAL_LIBRARY_OPT_SUBTOKENS, 
			TokenTransformContext.ALTER_SERVER_TYPE => OptLookup.OPT_ALTER_SERVER_TYPE_SUBTOKENS, 
			TokenTransformContext.FULLTEXT_SEARCH => OptLookup.OPT_FULLTEXT_SEARCH_SUBTOKENS, 
			TokenTransformContext.FTS_PROPERTY => OptLookup.OPT_FTS_PROPERTY_SUBTOKENS, 
			TokenTransformContext.AFTER_FTS_WITH => OptLookup.OPT_AFTER_FTS_WITH_SUBTOKENS, 
			TokenTransformContext.CREATE_USER_WITH => OptLookup.OPT_CREATE_USER_WITH_SUBTOKENS, 
			TokenTransformContext.ALTER_USER_WITH => OptLookup.OPT_ALTER_USER_WITH_SUBTOKENS, 
			TokenTransformContext.AG_REPLICA_OPTION => OptLookup.OPT_AG_REPLICA_OPTION_SUBTOKENS, 
			TokenTransformContext.ALTER_SERVER_CONFIG_SET => OptLookup.OPT_ALTER_SERVER_CONFIG_SET_SUBTOKENS, 
			TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG => OptLookup.OPT_ALTER_SERVER_CONFIG_SET_DIAG_LOG_SUBTOKENS, 
			TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER => OptLookup.OPT_ALTER_SERVER_CONFIG_SET_HADR_CLUSTER_SUBTOKENS, 
			TokenTransformContext.OVER_CLAUSE => OptLookup.OPT_OVER_CLAUSE_SUBTOKENS, 
			TokenTransformContext.AFTER_BEGIN => OptLookup.OPT_AFTER_BEGIN_SUBTOKENS, 
			TokenTransformContext.FILESTREAM_DB_OPT => OptLookup.OPT_FILESTREAM_DB_OPT_SUBTOKENS, 
			TokenTransformContext.CREATE_TAB_OPT => OptLookup.OPT_CREATE_TAB_OPT_SUBTOKENS, 
			TokenTransformContext.CREATE_TAB_WITH_OPT => OptLookup.OPT_CREATE_TAB_WITH_OPT_SUBTOKENS, 
			TokenTransformContext.SYSTEM_VERSIONING_OPT => OptLookup.OPT_SYSTEM_VERSIONING_OPT_SUBTOKENS, 
			TokenTransformContext.OPT_SXI_PATH_KIND => OptLookup.OPT_OPT_SXI_PATH_KIND_SUBTOKENS, 
			TokenTransformContext.OPT_SXI_PATH_OPTION => OptLookup.OPT_OPT_SXI_PATH_OPTION_SUBTOKENS, 
			TokenTransformContext.IN_INLINE_INDEX_SYNTAX => OptLookup.OPT_IN_INLINE_INDEX_SYNTAX_SUBTOKENS, 
			TokenTransformContext.CREATE_EXTERNALFILEFORMAT_WITH => OptLookup.OPT_CREATE_EXTERNALFILEFORMAT_WITH_SUBTOKENS, 
			TokenTransformContext.CREATE_EXTERNALDATASOURCE_WITH => OptLookup.OPT_CREATE_EXTERNALDATASOURCE_WITH_SUBTOKENS, 
			TokenTransformContext.ALTER_EXTERNALDATASOURCE_WITH => OptLookup.OPT_ALTER_EXTERNALDATASOURCE_WITH_SUBTOKENS, 
			TokenTransformContext.CREATE_EXTERNALTABLE_WITH => OptLookup.OPT_CREATE_EXTERNALTABLE_WITH_SUBTOKENS, 
			TokenTransformContext.EXTERNAL_TYPE => OptLookup.OPT_EXTERNAL_TYPE_SUBTOKENS, 
			TokenTransformContext.SECURITY_POLICY => OptLookup.OPT_SECURITY_POLICY_SUBTOKENS, 
			TokenTransformContext.QUERY_STORE_OPT => OptLookup.OPT_QUERY_STORE_OPT_SUBTOKENS, 
			TokenTransformContext.REMOTE_DATA_ARCHIVE_OPT => OptLookup.OPT_REMOTE_DATA_ARCHIVE_OPT_SUBTOKENS, 
			TokenTransformContext.TAB_LOCK_OPT => OptLookup.OPT_TAB_LOCK_OPT_SUBTOKENS, 
			TokenTransformContext.COL_ENCRYPTION_OR_MASTER_KEY => OptLookup.OPT_COL_ENCRYPTION_OR_MASTER_KEY_SUBTOKENS, 
			TokenTransformContext.GEODR_OPT => OptLookup.OPT_GEODR_OPT_SUBTOKENS, 
			TokenTransformContext.COL_ADD_DROP_OPT => OptLookup.OPT_COL_ADD_DROP_OPT_SUBTOKENS, 
			TokenTransformContext.COL_DEF_OPT => OptLookup.OPT_COL_DEF_OPT_SUBTOKENS, 
			TokenTransformContext.DB_SCOPED_CONFIG_OPT => OptLookup.OPT_DB_SCOPED_CONFIG_OPT_SUBTOKENS, 
			TokenTransformContext.GRAPH_ID_OR_FILETABLE => OptLookup.OPT_GRAPH_ID_OR_FILETABLE_SUBTOKENS, 
			TokenTransformContext.PERSISTENT_LOG_BUFFER_OPT => OptLookup.OPT_PERSISTENT_LOG_BUFFER_OPT_SUBTOKENS, 
			TokenTransformContext.AUTO_TUNING_DATABASE_TUNING_MODE_OPT => OptLookup.OPT_AUTO_TUNING_DATABASE_TUNING_MODE_OPT_SUBTOKENS, 
			TokenTransformContext.CREATE_TABLE_WITH_LEDGER_OPT => OptLookup.OPT_CREATE_TABLE_WITH_LEDGER_OPT_SUBTOKENS, 
			TokenTransformContext.CREATE_TAB_WITH_DATA_DELETION_OPT => OptLookup.OPT_CREATE_TAB_WITH_DATA_DELETION_OPT_SUBTOKENS, 
			TokenTransformContext.CREATE_EXTERNALSTREAM_WITH => OptLookup.OPT_CREATE_EXTERNALSTREAM_WITH_SUBTOKENS, 
			_ => null, 
		};
	}

	public override TokenTransformContext GetTransformContext(State state, int token)
	{
		switch (token)
		{
		case 173:
			switch (state.num)
			{
			case 29:
			case 31:
				return TokenTransformContext.CREATE_DB_OPT;
			case 47:
			case 50:
				return TokenTransformContext.FILESTREAM_DB_OPT;
			case 205:
			case 2557:
			case 2608:
			case 2695:
			case 3300:
			case 3347:
			case 3786:
			case 4270:
			case 4282:
			case 4291:
			case 4299:
			case 4301:
			case 4383:
			case 4613:
			case 4663:
			case 4750:
				return TokenTransformContext.CREATE_DROP_ALTER;
			case 211:
			case 213:
			case 214:
			case 1993:
			case 1994:
			case 2018:
			case 2019:
			case 2022:
			case 2185:
			case 2186:
			case 2189:
			case 2190:
			case 2193:
			case 2194:
				return TokenTransformContext.CREATE_TAB_OPT;
			case 212:
			case 2184:
				return TokenTransformContext.GRAPH_ID_OR_FILETABLE;
			case 218:
			case 221:
				return TokenTransformContext.CREATE_TAB_WITH_OPT;
			case 394:
			case 409:
			case 414:
			case 416:
			case 418:
				return TokenTransformContext.OVER_CLAUSE;
			case 1327:
			case 1328:
				return TokenTransformContext.OPT_STORAGE_ID;
			case 1642:
			case 1720:
				return TokenTransformContext.AFTER_WITH_EXEC_OPTION;
			case 2045:
			case 2083:
				return TokenTransformContext.INDEX_SORT_OPT;
			case 2046:
			case 2049:
			case 2230:
			case 3874:
			case 3908:
				return TokenTransformContext.IND_OPTIONS;
			case 2275:
				return TokenTransformContext.OPT_SXI_PATH_KIND;
			case 2276:
			case 2277:
			case 2282:
			case 2287:
				return TokenTransformContext.OPT_SXI_PATH_OPTION;
			case 2292:
			case 3596:
			case 3599:
			case 3615:
			case 3620:
				return TokenTransformContext.AFTER_WITH;
			case 2403:
			case 2413:
			case 2415:
			case 2423:
			case 2435:
			case 2515:
			case 3972:
			case 3982:
			case 4650:
				return TokenTransformContext.FULLTEXT_SEARCH;
			case 2429:
			case 2430:
			case 3988:
			case 3989:
			case 4522:
			case 4523:
			case 4759:
			case 4760:
			case 6016:
			case 6017:
				return TokenTransformContext.FTS_PROPERTY;
			case 3012:
			case 3014:
				return TokenTransformContext.CREATE_USER_WITH;
			case 3357:
				return TokenTransformContext.POOL_AFFINITY_TYPE;
			case 3448:
			case 3450:
			case 3457:
			case 3465:
			case 3473:
			case 4411:
				return TokenTransformContext.SEQUENCE_OPTIONS;
			case 3491:
			case 3494:
			case 4425:
				return TokenTransformContext.AG_REPLICA_OPTION;
			case 3679:
			case 3682:
				return TokenTransformContext.ALTER_DB_OPT;
			case 3789:
				return TokenTransformContext.ALTTAB_ACTIONS;
			case 3838:
				return TokenTransformContext.ALTTAB_ENABLE_DISABLE;
			case 3884:
			case 3887:
				return TokenTransformContext.TAB_OPT_NAME;
			case 4139:
			case 4141:
				return TokenTransformContext.ALTER_USER_WITH;
			case 4530:
			case 4534:
			case 4540:
				return TokenTransformContext.AFTER_FTS_WITH;
			case 5086:
			case 5088:
				return TokenTransformContext.LOAD_OPT;
			case 5467:
			case 5747:
			case 5752:
			case 5754:
			case 5945:
				return TokenTransformContext.AFTER_END;
			case 5615:
				return TokenTransformContext.ALTER_SERVER_TYPE;
			case 5679:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET;
			case 5681:
				return TokenTransformContext.AFFINITY_TYPE;
			case 5706:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG;
			case 5724:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER;
			default:
				return TokenTransformContext.NONE;
			}
		case 206:
			switch (state.num)
			{
			case 0:
			case 6:
			case 3634:
			case 3635:
			case 5742:
			case 5743:
			case 5746:
			case 5751:
			case 5921:
			case 5923:
			case 5929:
			case 5936:
			case 5941:
			case 6027:
			case 6029:
			case 6030:
			case 6033:
			case 6036:
			case 6039:
			case 6045:
				return TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP;
			case 3581:
			case 3582:
			case 3583:
			case 3586:
			case 5956:
			case 5960:
			case 5971:
			case 5972:
			case 5978:
				return TokenTransformContext.TOKEN_BEGIN_CS;
			default:
				return TokenTransformContext.NONE;
			}
		case 233:
		{
			int num = state.num;
			if ((uint)(num - 3634) <= 1u)
			{
				return TokenTransformContext.TOKEN_END_CS_LOOKUP;
			}
			return TokenTransformContext.NONE;
		}
		default:
			return TokenTransformContext.NONE;
		}
	}

	public ParserConfig()
		: base("110")
	{
	}
}
