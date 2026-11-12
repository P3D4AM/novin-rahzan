using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals160;

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
			case 72:
				return TokenTransformContext.PERSISTENT_LOG_BUFFER_OPT;
			case 143:
				return TokenTransformContext.CREATE_DB_TYPE;
			case 244:
			case 257:
			case 3113:
			case 3164:
			case 3251:
			case 4292:
			case 4571:
			case 5067:
			case 5636:
			case 5648:
			case 5657:
			case 5665:
			case 5667:
			case 5816:
			case 5925:
			case 6184:
			case 6238:
			case 6280:
			case 6359:
			case 6394:
			case 6397:
				return TokenTransformContext.CREATE_DROP_ALTER;
			case 263:
			case 265:
			case 266:
			case 2436:
			case 2437:
			case 2450:
			case 2451:
			case 2454:
			case 2752:
			case 2753:
			case 2756:
			case 2757:
				return TokenTransformContext.CREATE_TAB_OPT;
			case 264:
			case 2751:
			case 4020:
			case 4024:
				return TokenTransformContext.GRAPH_ID_OR_FILETABLE;
			case 270:
			case 273:
			case 2458:
			case 2761:
				return TokenTransformContext.CREATE_TAB_WITH_OPT;
			case 448:
			case 463:
			case 468:
			case 470:
			case 472:
				return TokenTransformContext.OVER_CLAUSE;
			case 1136:
			case 1137:
			case 1139:
			case 1143:
			case 1152:
			case 1172:
			case 1175:
			case 1179:
			case 1185:
			case 1187:
			case 1192:
			case 1193:
			case 1195:
			case 1199:
			case 1202:
			case 1206:
			case 1209:
			case 1215:
			case 1217:
			case 1223:
			case 1232:
			case 1233:
			case 1235:
			case 1238:
			case 1242:
			case 1244:
			case 1248:
			case 1253:
			case 1257:
				return TokenTransformContext.COL_DEF_OPT;
			case 1155:
			case 1158:
			case 3695:
			case 3700:
			case 3702:
			case 3706:
			case 3708:
			case 3728:
			case 3732:
			case 3735:
			case 5586:
			case 5591:
			case 5593:
			case 5598:
			case 5600:
			case 6322:
				return TokenTransformContext.COL_ENCRYPTION_OR_MASTER_KEY;
			case 1938:
			case 2007:
				return TokenTransformContext.AFTER_WITH_EXEC_OPTION;
			case 2286:
			case 2289:
				return TokenTransformContext.SYSTEM_VERSIONING_OPT;
			case 2307:
			case 2310:
			case 2316:
			case 2319:
				return TokenTransformContext.CREATE_TABLE_WITH_LEDGER_OPT;
			case 2343:
			case 2347:
			case 2351:
			case 2353:
			case 2362:
			case 5243:
				return TokenTransformContext.CREATE_TAB_WITH_DATA_DELETION_OPT;
			case 2424:
			case 2473:
			case 2475:
			case 2593:
			case 2602:
			case 2604:
				return TokenTransformContext.IN_INLINE_INDEX_SYNTAX;
			case 2492:
			case 2495:
			case 2612:
			case 2799:
			case 5077:
			case 5103:
			case 5106:
			case 5193:
			case 6128:
			case 6135:
			case 6138:
				return TokenTransformContext.IND_OPTIONS;
			case 2505:
			case 2510:
				return TokenTransformContext.TAB_LOCK_OPT;
			case 2611:
			case 2615:
				return TokenTransformContext.INDEX_SORT_OPT;
			case 2844:
				return TokenTransformContext.OPT_SXI_PATH_KIND;
			case 2845:
			case 2846:
			case 2851:
			case 2856:
				return TokenTransformContext.OPT_SXI_PATH_OPTION;
			case 2861:
			case 4634:
			case 4637:
			case 4653:
			case 4658:
				return TokenTransformContext.AFTER_WITH;
			case 2972:
			case 2982:
			case 2984:
			case 2992:
			case 3004:
			case 3084:
			case 5301:
			case 5311:
			case 6225:
				return TokenTransformContext.FULLTEXT_SEARCH;
			case 2998:
			case 2999:
			case 5317:
			case 5318:
			case 6045:
			case 6046:
			case 6371:
			case 6372:
			case 7756:
			case 7757:
				return TokenTransformContext.FTS_PROPERTY;
			case 3570:
			case 3572:
				return TokenTransformContext.CREATE_USER_WITH;
			case 3960:
			case 4028:
			case 4091:
			case 4133:
			case 5735:
			case 5736:
			case 5765:
			case 6334:
			case 6337:
			case 6340:
			case 6343:
				return TokenTransformContext.EXTERNAL_TYPE;
			case 3965:
			case 3969:
			case 3981:
			case 3994:
				return TokenTransformContext.CREATE_EXTERNALTABLE_WITH;
			case 4032:
			case 4036:
			case 4042:
			case 4053:
			case 4057:
			case 4069:
				return TokenTransformContext.CREATE_EXTERNALFILEFORMAT_WITH;
			case 4095:
			case 4099:
			case 4102:
				return TokenTransformContext.CREATE_EXTERNALDATASOURCE_WITH;
			case 4143:
				return TokenTransformContext.EXTERNAL_POOL_AFFINITY_TYPE;
			case 4177:
			case 4181:
			case 4184:
			case 5781:
			case 5785:
				return TokenTransformContext.EXTERNAL_LIBRARY_FILE_SPEC_OPT;
			case 4198:
			case 4202:
			case 4205:
			case 5796:
			case 5800:
				return TokenTransformContext.EXTERNAL_LANGUAGE_FILE_SPEC_OPT;
			case 4227:
			case 4230:
			case 4250:
				return TokenTransformContext.CREATE_EXTERNALSTREAM_WITH;
			case 4302:
				return TokenTransformContext.POOL_AFFINITY_TYPE;
			case 4403:
			case 4405:
			case 4412:
			case 4420:
			case 4428:
			case 5850:
				return TokenTransformContext.SEQUENCE_OPTIONS;
			case 4447:
			case 4450:
			case 5873:
				return TokenTransformContext.AG_REPLICA_OPTION;
			case 4590:
			case 4592:
			case 4599:
			case 5934:
			case 5937:
			case 5938:
			case 5941:
			case 5944:
			case 5945:
				return TokenTransformContext.SECURITY_POLICY;
			case 4680:
			case 4682:
			case 4726:
			case 4735:
			case 4772:
			case 4777:
			case 4953:
			case 4998:
				return TokenTransformContext.ALTER_DB_OPT;
			case 4703:
			case 4706:
			case 4994:
			case 4996:
				return TokenTransformContext.GEODR_OPT;
			case 4817:
			case 4820:
			case 4871:
				return TokenTransformContext.QUERY_STORE_OPT;
			case 4901:
				return TokenTransformContext.AUTO_TUNING_DATABASE_TUNING_MODE_OPT;
			case 4932:
			case 4935:
			case 4947:
				return TokenTransformContext.REMOTE_DATA_ARCHIVE_OPT;
			case 5044:
				return TokenTransformContext.DB_SCOPED_CONFIG_OPT;
			case 5070:
				return TokenTransformContext.ALTTAB_ACTIONS;
			case 5152:
				return TokenTransformContext.ALTTAB_ENABLE_DISABLE;
			case 5169:
			case 5179:
				return TokenTransformContext.COL_ADD_DROP_OPT;
			case 5190:
			case 6119:
				return TokenTransformContext.IND_ACTIONS;
			case 5219:
			case 5222:
				return TokenTransformContext.TAB_OPT_NAME;
			case 5487:
			case 5489:
				return TokenTransformContext.ALTER_USER_WITH;
			case 5739:
			case 5742:
				return TokenTransformContext.ALTER_EXTERNALDATASOURCE_WITH;
			case 5773:
			case 5774:
			case 5788:
				return TokenTransformContext.ALTER_EXTERNAL_LIBRARY_OPT;
			case 5792:
			case 5793:
			case 5803:
				return TokenTransformContext.ALTER_EXTERNAL_LANGUAGE_OPT;
			case 6053:
			case 6057:
			case 6063:
				return TokenTransformContext.AFTER_FTS_WITH;
			case 6604:
			case 6606:
				return TokenTransformContext.DUMP_OPT;
			case 6614:
			case 6616:
			case 6617:
				return TokenTransformContext.DUMP_ENCRYPTION_OPT;
			case 6712:
			case 6714:
				return TokenTransformContext.LOAD_OPT;
			case 7108:
			case 7456:
			case 7461:
			case 7463:
			case 7655:
			case 7668:
				return TokenTransformContext.AFTER_END;
			case 7205:
			case 7206:
			case 7227:
				return TokenTransformContext.ADD;
			case 7278:
				return TokenTransformContext.ALTER_SERVER_TYPE;
			case 7352:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET;
			case 7354:
				return TokenTransformContext.AFFINITY_TYPE;
			case 7379:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG;
			case 7397:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER;
			case 7661:
			case 7775:
				return TokenTransformContext.AFTER_BEGIN;
			default:
				return TokenTransformContext.NONE;
			}
		case 206:
			switch (state.num)
			{
			case 0:
			case 6:
			case 4624:
			case 4673:
			case 4674:
			case 7451:
			case 7452:
			case 7455:
			case 7460:
			case 7631:
			case 7633:
			case 7639:
			case 7646:
			case 7651:
			case 7666:
			case 7667:
			case 7767:
			case 7769:
			case 7770:
			case 7773:
			case 7780:
			case 7783:
			case 7789:
				return TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP;
			default:
				return TokenTransformContext.NONE;
			}
		case 233:
		{
			int num = state.num;
			if ((uint)(num - 4673) <= 1u)
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
		: base("160")
	{
	}
}
