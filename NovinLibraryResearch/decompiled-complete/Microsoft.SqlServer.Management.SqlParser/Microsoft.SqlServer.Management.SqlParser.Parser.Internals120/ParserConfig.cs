using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals120;

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
			case 207:
			case 2613:
			case 2664:
			case 2751:
			case 3356:
			case 3404:
			case 3885:
			case 4371:
			case 4383:
			case 4392:
			case 4400:
			case 4402:
			case 4486:
			case 4733:
			case 4783:
			case 4870:
				return TokenTransformContext.CREATE_DROP_ALTER;
			case 213:
			case 215:
			case 216:
			case 1997:
			case 1998:
			case 2022:
			case 2023:
			case 2026:
			case 2230:
			case 2231:
			case 2234:
			case 2235:
			case 2238:
			case 2239:
				return TokenTransformContext.CREATE_TAB_OPT;
			case 214:
			case 2229:
				return TokenTransformContext.GRAPH_ID_OR_FILETABLE;
			case 220:
			case 223:
				return TokenTransformContext.CREATE_TAB_WITH_OPT;
			case 396:
			case 411:
			case 416:
			case 418:
			case 420:
				return TokenTransformContext.OVER_CLAUSE;
			case 1325:
			case 1326:
				return TokenTransformContext.OPT_STORAGE_ID;
			case 1640:
			case 1718:
				return TokenTransformContext.AFTER_WITH_EXEC_OPTION;
			case 2038:
			case 2040:
			case 2119:
			case 2121:
				return TokenTransformContext.IN_INLINE_INDEX_SYNTAX;
			case 2053:
			case 2056:
			case 2129:
			case 2275:
			case 4009:
				return TokenTransformContext.IND_OPTIONS;
			case 2066:
			case 2071:
				return TokenTransformContext.TAB_LOCK_OPT;
			case 2128:
			case 2132:
				return TokenTransformContext.INDEX_SORT_OPT;
			case 2320:
				return TokenTransformContext.OPT_SXI_PATH_KIND;
			case 2321:
			case 2322:
			case 2327:
			case 2332:
				return TokenTransformContext.OPT_SXI_PATH_OPTION;
			case 2337:
			case 3657:
			case 3660:
			case 3676:
			case 3681:
				return TokenTransformContext.AFTER_WITH;
			case 2459:
			case 2469:
			case 2471:
			case 2479:
			case 2491:
			case 2571:
			case 4073:
			case 4083:
			case 4770:
				return TokenTransformContext.FULLTEXT_SEARCH;
			case 2485:
			case 2486:
			case 4089:
			case 4090:
			case 4641:
			case 4642:
			case 4881:
			case 4882:
			case 6195:
			case 6196:
				return TokenTransformContext.FTS_PROPERTY;
			case 3068:
			case 3070:
				return TokenTransformContext.CREATE_USER_WITH;
			case 3414:
				return TokenTransformContext.POOL_AFFINITY_TYPE;
			case 3505:
			case 3507:
			case 3514:
			case 3522:
			case 3530:
			case 4518:
				return TokenTransformContext.SEQUENCE_OPTIONS;
			case 3548:
			case 3551:
			case 4532:
				return TokenTransformContext.AG_REPLICA_OPTION;
			case 3740:
			case 3743:
				return TokenTransformContext.ALTER_DB_OPT;
			case 3888:
				return TokenTransformContext.ALTTAB_ACTIONS;
			case 3937:
				return TokenTransformContext.ALTTAB_ENABLE_DISABLE;
			case 3985:
			case 3988:
				return TokenTransformContext.TAB_OPT_NAME;
			case 4240:
			case 4242:
				return TokenTransformContext.ALTER_USER_WITH;
			case 4649:
			case 4653:
			case 4659:
				return TokenTransformContext.AFTER_FTS_WITH;
			case 5112:
			case 5114:
				return TokenTransformContext.DUMP_OPT;
			case 5122:
			case 5124:
			case 5125:
				return TokenTransformContext.DUMP_ENCRYPTION_OPT;
			case 5226:
			case 5228:
				return TokenTransformContext.LOAD_OPT;
			case 5623:
			case 5926:
			case 5931:
			case 5933:
			case 6124:
			case 6221:
				return TokenTransformContext.AFTER_END;
			case 5772:
				return TokenTransformContext.ALTER_SERVER_TYPE;
			case 5836:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET;
			case 5838:
				return TokenTransformContext.AFFINITY_TYPE;
			case 5863:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG;
			case 5881:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER;
			case 6214:
				return TokenTransformContext.AFTER_BEGIN;
			default:
				return TokenTransformContext.NONE;
			}
		case 206:
			switch (state.num)
			{
			case 0:
			case 6:
			case 3695:
			case 3696:
			case 5921:
			case 5922:
			case 5925:
			case 5930:
			case 6100:
			case 6102:
			case 6108:
			case 6115:
			case 6120:
			case 6206:
			case 6208:
			case 6209:
			case 6212:
			case 6219:
			case 6220:
			case 6238:
			case 6241:
			case 6247:
				return TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP;
			case 3642:
			case 3643:
			case 3644:
			case 3647:
			case 6135:
			case 6139:
			case 6150:
			case 6151:
			case 6157:
				return TokenTransformContext.TOKEN_BEGIN_CS;
			default:
				return TokenTransformContext.NONE;
			}
		case 233:
		{
			int num = state.num;
			if ((uint)(num - 3695) <= 1u)
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
		: base("120")
	{
	}
}
