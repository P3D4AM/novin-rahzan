using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Parser.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.Parser.Internals140;

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
			case 141:
				return TokenTransformContext.CREATE_DB_TYPE;
			case 242:
			case 255:
			case 2961:
			case 3012:
			case 3099:
			case 3994:
			case 4280:
			case 4733:
			case 5288:
			case 5300:
			case 5309:
			case 5317:
			case 5319:
			case 5437:
			case 5546:
			case 5787:
			case 5841:
			case 5953:
			case 5988:
				return TokenTransformContext.CREATE_DROP_ALTER;
			case 261:
			case 263:
			case 264:
			case 2304:
			case 2305:
			case 2318:
			case 2319:
			case 2322:
			case 2601:
			case 2602:
			case 2605:
			case 2606:
				return TokenTransformContext.CREATE_TAB_OPT;
			case 262:
			case 2600:
			case 3810:
			case 3814:
				return TokenTransformContext.GRAPH_ID_OR_FILETABLE;
			case 268:
			case 271:
			case 2326:
			case 2610:
				return TokenTransformContext.CREATE_TAB_WITH_OPT;
			case 446:
			case 461:
			case 466:
			case 468:
			case 470:
				return TokenTransformContext.OVER_CLAUSE;
			case 1084:
			case 1085:
			case 1087:
			case 1091:
			case 1100:
			case 1120:
			case 1123:
			case 1127:
			case 1133:
			case 1135:
			case 1140:
			case 1141:
			case 1143:
			case 1147:
			case 1150:
			case 1154:
			case 1157:
			case 1163:
			case 1165:
			case 1171:
			case 1180:
			case 1181:
			case 1183:
			case 1186:
			case 1190:
			case 1192:
			case 1196:
			case 1201:
			case 1205:
				return TokenTransformContext.COL_DEF_OPT;
			case 1103:
			case 1106:
			case 3543:
			case 3548:
			case 3550:
			case 3568:
			case 3572:
			case 3575:
			case 5238:
			case 5243:
			case 5245:
			case 5250:
			case 5252:
			case 5921:
				return TokenTransformContext.COL_ENCRYPTION_OR_MASTER_KEY;
			case 1885:
			case 1952:
				return TokenTransformContext.AFTER_WITH_EXEC_OPTION;
			case 2234:
			case 2237:
				return TokenTransformContext.SYSTEM_VERSIONING_OPT;
			case 2341:
			case 2343:
			case 2458:
			case 2467:
			case 2469:
				return TokenTransformContext.IN_INLINE_INDEX_SYNTAX;
			case 2359:
			case 2362:
			case 2477:
			case 2648:
			case 4762:
			case 4765:
			case 4828:
			case 4858:
			case 5731:
			case 5738:
			case 5741:
				return TokenTransformContext.IND_OPTIONS;
			case 2372:
			case 2377:
				return TokenTransformContext.TAB_LOCK_OPT;
			case 2476:
			case 2480:
				return TokenTransformContext.INDEX_SORT_OPT;
			case 2693:
				return TokenTransformContext.OPT_SXI_PATH_KIND;
			case 2694:
			case 2695:
			case 2700:
			case 2705:
				return TokenTransformContext.OPT_SXI_PATH_OPTION;
			case 2710:
			case 4343:
			case 4346:
			case 4362:
			case 4367:
				return TokenTransformContext.AFTER_WITH;
			case 2820:
			case 2830:
			case 2832:
			case 2840:
			case 2852:
			case 2932:
			case 4953:
			case 4963:
			case 5828:
				return TokenTransformContext.FULLTEXT_SEARCH;
			case 2846:
			case 2847:
			case 4969:
			case 4970:
			case 5644:
			case 5645:
			case 5965:
			case 5966:
			case 7336:
			case 7337:
				return TokenTransformContext.FTS_PROPERTY;
			case 3418:
			case 3420:
				return TokenTransformContext.CREATE_USER_WITH;
			case 3757:
			case 3904:
			case 4266:
			case 5387:
			case 5411:
			case 5933:
			case 5942:
				return TokenTransformContext.EXTERNAL_TYPE;
			case 3762:
			case 3766:
			case 3778:
			case 3791:
				return TokenTransformContext.CREATE_EXTERNALTABLE_WITH;
			case 3822:
			case 3826:
			case 3832:
			case 3842:
			case 3846:
			case 3858:
				return TokenTransformContext.CREATE_EXTERNALFILEFORMAT_WITH;
			case 3872:
			case 3876:
			case 3879:
				return TokenTransformContext.CREATE_EXTERNALDATASOURCE_WITH;
			case 3914:
				return TokenTransformContext.EXTERNAL_POOL_AFFINITY_TYPE;
			case 4004:
				return TokenTransformContext.POOL_AFFINITY_TYPE;
			case 4104:
			case 4106:
			case 4113:
			case 4121:
			case 4129:
			case 5471:
				return TokenTransformContext.SEQUENCE_OPTIONS;
			case 4148:
			case 4151:
			case 5494:
				return TokenTransformContext.AG_REPLICA_OPTION;
			case 4299:
			case 4301:
			case 4308:
			case 5555:
			case 5558:
			case 5559:
			case 5562:
			case 5565:
			case 5566:
				return TokenTransformContext.SECURITY_POLICY;
			case 4389:
			case 4391:
			case 4435:
			case 4444:
			case 4481:
			case 4486:
			case 4670:
				return TokenTransformContext.ALTER_DB_OPT;
			case 4412:
			case 4415:
				return TokenTransformContext.GEODR_OPT;
			case 4526:
			case 4529:
			case 4580:
				return TokenTransformContext.QUERY_STORE_OPT;
			case 4620:
			case 4623:
			case 4635:
				return TokenTransformContext.REMOTE_DATA_ARCHIVE_OPT;
			case 4736:
				return TokenTransformContext.ALTTAB_ACTIONS;
			case 4811:
				return TokenTransformContext.ALTTAB_ENABLE_DISABLE;
			case 4834:
			case 4844:
				return TokenTransformContext.COL_ADD_DROP_OPT;
			case 4855:
			case 5722:
				return TokenTransformContext.IND_ACTIONS;
			case 4884:
			case 4887:
				return TokenTransformContext.TAB_OPT_NAME;
			case 5139:
			case 5141:
				return TokenTransformContext.ALTER_USER_WITH;
			case 5391:
			case 5394:
				return TokenTransformContext.ALTER_EXTERNALDATASOURCE_WITH;
			case 5652:
			case 5656:
			case 5662:
				return TokenTransformContext.AFTER_FTS_WITH;
			case 6198:
			case 6200:
				return TokenTransformContext.DUMP_OPT;
			case 6208:
			case 6210:
			case 6211:
				return TokenTransformContext.DUMP_ENCRYPTION_OPT;
			case 6312:
			case 6314:
				return TokenTransformContext.LOAD_OPT;
			case 6712:
			case 7038:
			case 7043:
			case 7045:
			case 7237:
			case 7250:
				return TokenTransformContext.AFTER_END;
			case 6865:
				return TokenTransformContext.ALTER_SERVER_TYPE;
			case 6939:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET;
			case 6941:
				return TokenTransformContext.AFFINITY_TYPE;
			case 6966:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_DIAG_LOG;
			case 6984:
				return TokenTransformContext.ALTER_SERVER_CONFIG_SET_HADR_CLUSTER;
			case 7243:
			case 7355:
				return TokenTransformContext.AFTER_BEGIN;
			default:
				return TokenTransformContext.NONE;
			}
		case 206:
			switch (state.num)
			{
			case 0:
			case 6:
			case 4333:
			case 4382:
			case 4383:
			case 7033:
			case 7034:
			case 7037:
			case 7042:
			case 7213:
			case 7215:
			case 7221:
			case 7228:
			case 7233:
			case 7248:
			case 7249:
			case 7347:
			case 7349:
			case 7350:
			case 7353:
			case 7360:
			case 7363:
			case 7369:
				return TokenTransformContext.TOKEN_BEGIN_CS_LOOKUP;
			default:
				return TokenTransformContext.NONE;
			}
		case 233:
		{
			int num = state.num;
			if ((uint)(num - 4382) <= 1u)
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
		: base("140")
	{
	}
}
