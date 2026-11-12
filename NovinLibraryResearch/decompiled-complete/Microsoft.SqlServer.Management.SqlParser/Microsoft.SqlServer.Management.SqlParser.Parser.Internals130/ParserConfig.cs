using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals130;

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
			case 31:
			case 33:
				return TokenTransformContext.CREATE_DB_OPT;
			case 49:
			case 52:
				return TokenTransformContext.FILESTREAM_DB_OPT;
			case 122:
				return TokenTransformContext.CREATE_DB_TYPE;
			case 223:
			case 236:
			case 2866:
			case 2917:
			case 3004:
			case 3867:
			case 4151:
			case 4589:
			case 5133:
			case 5145:
			case 5154:
			case 5162:
			case 5164:
			case 5267:
			case 5369:
			case 5592:
			case 5646:
			case 5755:
			case 5790:
				return TokenTransformContext.CREATE_DROP_ALTER;
			case 242:
			case 244:
			case 245:
			case 2209:
			case 2210:
			case 2234:
			case 2235:
			case 2238:
			case 2495:
			case 2496:
			case 2499:
			case 2500:
			case 2503:
			case 2504:
				return TokenTransformContext.CREATE_TAB_OPT;
			case 243:
			case 2494:
				return TokenTransformContext.GRAPH_ID_OR_FILETABLE;
			case 249:
			case 252:
				return TokenTransformContext.CREATE_TAB_WITH_OPT;
			case 427:
			case 442:
			case 447:
			case 449:
			case 451:
				return TokenTransformContext.OVER_CLAUSE;
			case 1225:
			case 1226:
			case 1228:
			case 1274:
			case 1275:
			case 1277:
			case 1299:
			case 1308:
			case 1309:
			case 1311:
			case 1317:
				return TokenTransformContext.COL_DEF_OPT;
			case 1240:
			case 1243:
			case 3448:
			case 3453:
			case 3455:
			case 3473:
			case 3477:
			case 3480:
			case 5083:
			case 5088:
			case 5090:
			case 5095:
			case 5097:
			case 5726:
				return TokenTransformContext.COL_ENCRYPTION_OR_MASTER_KEY;
			case 1805:
			case 1883:
				return TokenTransformContext.AFTER_WITH_EXEC_OPTION;
			case 2166:
			case 2169:
				return TokenTransformContext.SYSTEM_VERSIONING_OPT;
			case 2251:
			case 2253:
			case 2352:
			case 2361:
			case 2363:
				return TokenTransformContext.IN_INLINE_INDEX_SYNTAX;
			case 2268:
			case 2271:
			case 2371:
			case 2540:
			case 4618:
			case 4621:
			case 4673:
			case 4703:
			case 5553:
				return TokenTransformContext.IND_OPTIONS;
			case 2281:
			case 2286:
				return TokenTransformContext.TAB_LOCK_OPT;
			case 2370:
			case 2374:
				return TokenTransformContext.INDEX_SORT_OPT;
			case 2585:
				return TokenTransformContext.OPT_SXI_PATH_KIND;
			case 2586:
			case 2587:
			case 2592:
			case 2597:
				return TokenTransformContext.OPT_SXI_PATH_OPTION;
			case 2602:
			case 4213:
			case 4216:
			case 4232:
			case 4237:
				return TokenTransformContext.AFTER_WITH;
			case 2712:
			case 2722:
			case 2724:
			case 2732:
			case 2744:
			case 2824:
			case 4798:
			case 4808:
			case 5633:
				return TokenTransformContext.FULLTEXT_SEARCH;
			case 2738:
			case 2739:
			case 4814:
			case 4815:
			case 5467:
			case 5468:
			case 5767:
			case 5768:
			case 7137:
			case 7138:
				return TokenTransformContext.FTS_PROPERTY;
			case 3323:
			case 3325:
				return TokenTransformContext.CREATE_USER_WITH;
			case 3662:
			case 3794:
			case 4137:
			case 5232:
			case 5250:
			case 5738:
			case 5747:
				return TokenTransformContext.EXTERNAL_TYPE;
			case 3667:
			case 3671:
			case 3683:
			case 3696:
				return TokenTransformContext.CREATE_EXTERNALTABLE_WITH;
			case 3719:
			case 3723:
			case 3729:
			case 3739:
			case 3743:
			case 3755:
				return TokenTransformContext.CREATE_EXTERNALFILEFORMAT_WITH;
			case 3763:
			case 3767:
			case 3770:
				return TokenTransformContext.CREATE_EXTERNALDATASOURCE_WITH;
			case 3804:
				return TokenTransformContext.EXTERNAL_POOL_AFFINITY_TYPE;
			case 3877:
				return TokenTransformContext.POOL_AFFINITY_TYPE;
			case 3977:
			case 3979:
			case 3986:
			case 3994:
			case 4002:
			case 5301:
				return TokenTransformContext.SEQUENCE_OPTIONS;
			case 4021:
			case 4024:
			case 5317:
				return TokenTransformContext.AG_REPLICA_OPTION;
			case 4159:
			case 4161:
			case 4168:
			case 5378:
			case 5381:
			case 5382:
			case 5385:
			case 5388:
			case 5389:
				return TokenTransformContext.SECURITY_POLICY;
			case 4259:
			case 4261:
			case 4305:
			case 4314:
			case 4351:
			case 4354:
			case 4524:
				return TokenTransformContext.ALTER_DB_OPT;
			case 4282:
			case 4285:
				return TokenTransformContext.GEODR_OPT;
			case 4394:
			case 4397:
			case 4445:
				return TokenTransformContext.QUERY_STORE_OPT;
			case 4474:
			case 4477:
			case 4489:
				return TokenTransformContext.REMOTE_DATA_ARCHIVE_OPT;
			case 4570:
				return TokenTransformContext.DB_SCOPED_CONFIG_OPT;
			case 4592:
				return TokenTransformContext.ALTTAB_ACTIONS;
			case 4656:
				return TokenTransformContext.ALTTAB_ENABLE_DISABLE;
			case 4679:
			case 4689:
				return TokenTransformContext.COL_ADD_DROP_OPT;
			case 4700:
			case 5544:
				return TokenTransformContext.IND_ACTIONS;
			case 4729:
			case 4732:
				return TokenTransformContext.TAB_OPT_NAME;
			case 4984:
			case 4986:
				return TokenTransformContext.ALTER_USER_WITH;
			case 5236:
			case 5239:
				return TokenTransformContext.ALTER_EXTERNALDATASOURCE_WITH;
			case 5475:
			case 5479:
			case 5485:
				return TokenTransformContext.AFTER_FTS_WITH;
			case 6000:
			case 6002:
				return TokenTransformContext.DUMP_OPT;
			case 6010:
			case 6012:
			case 6013:
				return TokenTransformContext.DUMP_ENCRYPTION_OPT;
			case 6114:
			case 6116:
				return TokenTransformContext.LOAD_OPT;
			case 6514:
			case 6839:
			case 6844:
			case 6846:
			case 7038:
			case 7051:
				return TokenTransformContext.AFTER_END;
			case 6669:
				return TokenTransformContext.ALTER_SERVER_TYPE;
			case 6743:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET;
			case 6745:
				return TokenTransformContext.AFFINITY_TYPE;
			case 6770:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG;
			case 6788:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER;
			case 7044:
			case 7156:
				return TokenTransformContext.AFTER_BEGIN;
			default:
				return TokenTransformContext.NONE;
			}
		case 206:
			switch (state.num)
			{
			case 0:
			case 6:
			case 4203:
			case 4252:
			case 4253:
			case 6834:
			case 6835:
			case 6838:
			case 6843:
			case 7014:
			case 7016:
			case 7022:
			case 7029:
			case 7034:
			case 7049:
			case 7050:
			case 7148:
			case 7150:
			case 7151:
			case 7154:
			case 7161:
			case 7164:
			case 7170:
				return TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP;
			default:
				return TokenTransformContext.NONE;
			}
		case 233:
		{
			int num = state.num;
			if ((uint)(num - 4252) <= 1u)
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
		: base("130")
	{
	}
}
