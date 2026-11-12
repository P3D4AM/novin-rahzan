using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.InternalsAzure;

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
			case 163:
				return TokenTransformContext.CREATE_DB_TYPE;
			case 264:
			case 277:
			case 3133:
			case 3184:
			case 3271:
			case 4312:
			case 4591:
			case 5096:
			case 5665:
			case 5677:
			case 5686:
			case 5694:
			case 5696:
			case 5845:
			case 5954:
			case 6213:
			case 6267:
			case 6309:
			case 6388:
			case 6423:
			case 6426:
				return TokenTransformContext.CREATE_DROP_ALTER;
			case 283:
			case 285:
			case 286:
			case 2456:
			case 2457:
			case 2470:
			case 2471:
			case 2474:
			case 2772:
			case 2773:
			case 2776:
			case 2777:
				return TokenTransformContext.CREATE_TAB_OPT;
			case 284:
			case 2771:
			case 4040:
			case 4044:
				return TokenTransformContext.GRAPH_ID_OR_FILETABLE;
			case 290:
			case 293:
			case 2478:
			case 2781:
				return TokenTransformContext.CREATE_TAB_WITH_OPT;
			case 468:
			case 483:
			case 488:
			case 490:
			case 492:
				return TokenTransformContext.OVER_CLAUSE;
			case 1156:
			case 1157:
			case 1159:
			case 1163:
			case 1172:
			case 1192:
			case 1195:
			case 1199:
			case 1205:
			case 1207:
			case 1212:
			case 1213:
			case 1215:
			case 1219:
			case 1222:
			case 1226:
			case 1229:
			case 1235:
			case 1237:
			case 1243:
			case 1252:
			case 1253:
			case 1255:
			case 1258:
			case 1262:
			case 1264:
			case 1268:
			case 1273:
			case 1277:
				return TokenTransformContext.COL_DEF_OPT;
			case 1175:
			case 1178:
			case 3715:
			case 3720:
			case 3722:
			case 3726:
			case 3728:
			case 3748:
			case 3752:
			case 3755:
			case 5615:
			case 5620:
			case 5622:
			case 5627:
			case 5629:
			case 6351:
				return TokenTransformContext.COL_ENCRYPTION_OR_MASTER_KEY;
			case 1958:
			case 2027:
				return TokenTransformContext.AFTER_WITH_EXEC_OPTION;
			case 2306:
			case 2309:
				return TokenTransformContext.SYSTEM_VERSIONING_OPT;
			case 2327:
			case 2330:
			case 2336:
			case 2339:
				return TokenTransformContext.CREATE_TABLE_WITH_LEDGER_OPT;
			case 2363:
			case 2367:
			case 2371:
			case 2373:
			case 2382:
			case 5272:
				return TokenTransformContext.CREATE_TAB_WITH_DATA_DELETION_OPT;
			case 2444:
			case 2493:
			case 2495:
			case 2613:
			case 2622:
			case 2624:
				return TokenTransformContext.IN_INLINE_INDEX_SYNTAX;
			case 2512:
			case 2515:
			case 2632:
			case 2819:
			case 5106:
			case 5132:
			case 5135:
			case 5222:
			case 6157:
			case 6164:
			case 6167:
				return TokenTransformContext.IND_OPTIONS;
			case 2525:
			case 2530:
				return TokenTransformContext.TAB_LOCK_OPT;
			case 2631:
			case 2635:
				return TokenTransformContext.INDEX_SORT_OPT;
			case 2864:
				return TokenTransformContext.OPT_SXI_PATH_KIND;
			case 2865:
			case 2866:
			case 2871:
			case 2876:
				return TokenTransformContext.OPT_SXI_PATH_OPTION;
			case 2881:
			case 4654:
			case 4657:
			case 4673:
			case 4678:
				return TokenTransformContext.AFTER_WITH;
			case 2992:
			case 3002:
			case 3004:
			case 3012:
			case 3024:
			case 3104:
			case 5330:
			case 5340:
			case 6254:
				return TokenTransformContext.FULLTEXT_SEARCH;
			case 3018:
			case 3019:
			case 5346:
			case 5347:
			case 6074:
			case 6075:
			case 6400:
			case 6401:
			case 7785:
			case 7786:
				return TokenTransformContext.FTS_PROPERTY;
			case 3590:
			case 3592:
				return TokenTransformContext.CREATE_USER_WITH;
			case 3980:
			case 4048:
			case 4111:
			case 4153:
			case 5764:
			case 5765:
			case 5794:
			case 6363:
			case 6366:
			case 6369:
			case 6372:
				return TokenTransformContext.EXTERNAL_TYPE;
			case 3985:
			case 3989:
			case 4001:
			case 4014:
				return TokenTransformContext.CREATE_EXTERNALTABLE_WITH;
			case 4052:
			case 4056:
			case 4062:
			case 4073:
			case 4077:
			case 4089:
				return TokenTransformContext.CREATE_EXTERNALFILEFORMAT_WITH;
			case 4115:
			case 4119:
			case 4122:
				return TokenTransformContext.CREATE_EXTERNALDATASOURCE_WITH;
			case 4163:
				return TokenTransformContext.EXTERNAL_POOL_AFFINITY_TYPE;
			case 4197:
			case 4201:
			case 4204:
			case 5810:
			case 5814:
				return TokenTransformContext.EXTERNAL_LIBRARY_FILE_SPEC_OPT;
			case 4218:
			case 4222:
			case 4225:
			case 5825:
			case 5829:
				return TokenTransformContext.EXTERNAL_LANGUAGE_FILE_SPEC_OPT;
			case 4247:
			case 4250:
			case 4270:
				return TokenTransformContext.CREATE_EXTERNALSTREAM_WITH;
			case 4322:
				return TokenTransformContext.POOL_AFFINITY_TYPE;
			case 4423:
			case 4425:
			case 4432:
			case 4440:
			case 4448:
			case 5879:
				return TokenTransformContext.SEQUENCE_OPTIONS;
			case 4467:
			case 4470:
			case 5902:
				return TokenTransformContext.AG_REPLICA_OPTION;
			case 4610:
			case 4612:
			case 4619:
			case 5963:
			case 5966:
			case 5967:
			case 5970:
			case 5973:
			case 5974:
				return TokenTransformContext.SECURITY_POLICY;
			case 4700:
			case 4702:
			case 4752:
			case 4761:
			case 4801:
			case 4806:
			case 4982:
			case 5027:
				return TokenTransformContext.ALTER_DB_OPT;
			case 4723:
			case 4726:
			case 5023:
			case 5025:
				return TokenTransformContext.GEODR_OPT;
			case 4846:
			case 4849:
			case 4900:
				return TokenTransformContext.QUERY_STORE_OPT;
			case 4930:
				return TokenTransformContext.AUTO_TUNING_DATABASE_TUNING_MODE_OPT;
			case 4961:
			case 4964:
			case 4976:
				return TokenTransformContext.REMOTE_DATA_ARCHIVE_OPT;
			case 5073:
				return TokenTransformContext.DB_SCOPED_CONFIG_OPT;
			case 5099:
				return TokenTransformContext.ALTTAB_ACTIONS;
			case 5181:
				return TokenTransformContext.ALTTAB_ENABLE_DISABLE;
			case 5198:
			case 5208:
				return TokenTransformContext.COL_ADD_DROP_OPT;
			case 5219:
			case 6148:
				return TokenTransformContext.IND_ACTIONS;
			case 5248:
			case 5251:
				return TokenTransformContext.TAB_OPT_NAME;
			case 5516:
			case 5518:
				return TokenTransformContext.ALTER_USER_WITH;
			case 5768:
			case 5771:
				return TokenTransformContext.ALTER_EXTERNALDATASOURCE_WITH;
			case 5802:
			case 5803:
			case 5817:
				return TokenTransformContext.ALTER_EXTERNAL_LIBRARY_OPT;
			case 5821:
			case 5822:
			case 5832:
				return TokenTransformContext.ALTER_EXTERNAL_LANGUAGE_OPT;
			case 6082:
			case 6086:
			case 6092:
				return TokenTransformContext.AFTER_FTS_WITH;
			case 6633:
			case 6635:
				return TokenTransformContext.DUMP_OPT;
			case 6643:
			case 6645:
			case 6646:
				return TokenTransformContext.DUMP_ENCRYPTION_OPT;
			case 6741:
			case 6743:
				return TokenTransformContext.LOAD_OPT;
			case 7137:
			case 7485:
			case 7490:
			case 7492:
			case 7684:
			case 7697:
				return TokenTransformContext.AFTER_END;
			case 7234:
			case 7235:
			case 7256:
				return TokenTransformContext.ADD;
			case 7307:
				return TokenTransformContext.ALTER_SERVER_TYPE;
			case 7381:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET;
			case 7383:
				return TokenTransformContext.AFFINITY_TYPE;
			case 7408:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG;
			case 7426:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER;
			case 7690:
			case 7804:
				return TokenTransformContext.AFTER_BEGIN;
			default:
				return TokenTransformContext.NONE;
			}
		case 206:
			switch (state.num)
			{
			case 0:
			case 6:
			case 4644:
			case 4693:
			case 4694:
			case 7480:
			case 7481:
			case 7484:
			case 7489:
			case 7660:
			case 7662:
			case 7668:
			case 7675:
			case 7680:
			case 7695:
			case 7696:
			case 7796:
			case 7798:
			case 7799:
			case 7802:
			case 7809:
			case 7812:
			case 7818:
				return TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP;
			default:
				return TokenTransformContext.NONE;
			}
		case 233:
		{
			int num = state.num;
			if ((uint)(num - 4693) <= 1u)
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
		: base("Azure")
	{
	}
}
