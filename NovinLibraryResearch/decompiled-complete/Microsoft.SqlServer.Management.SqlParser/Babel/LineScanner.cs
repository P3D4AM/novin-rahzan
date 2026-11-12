using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.SqlParser.Parser;

namespace Babel;

public class LineScanner
{
	private enum SqlCmdState
	{
		NotSqlCmd,
		SqlCmd,
		SqlCmdEOF
	}

	private Scanner lex;

	private string source;

	private string batchSeparator;

	private bool isSqlCmdModeEnabled;

	private SqlCmdState sqlCmdState;

	private static Regex sqlCmdRegex;

	private static TokenDefinition defaultDefinition;

	private static Dictionary<int, TokenDefinition> definitions;

	private static Dictionary<string, TokenType> colorOverrides;

	internal static string[] Keywords;

	internal static string[] Operators;

	internal static string[] SqlCMDRegExp;

	internal static string[] SystemFunctions;

	internal static string[] SystemStoredProcedures;

	internal static string[] SystemViews;

	internal static string[] Builtins;

	public bool IsSqlCmdModeEnabled
	{
		get
		{
			return isSqlCmdModeEnabled;
		}
		set
		{
			isSqlCmdModeEnabled = value;
		}
	}

	public string BatchSeparator
	{
		get
		{
			return batchSeparator;
		}
		set
		{
			batchSeparator = value;
			ParseOptions parseOptions = new ParseOptions();
			parseOptions.BatchSeparator = batchSeparator;
			lex = new Scanner(parseOptions);
		}
	}

	public LineScanner()
	{
		ParseOptions options = new ParseOptions();
		lex = new Scanner(options);
	}

	static LineScanner()
	{
		defaultDefinition = new TokenDefinition(TokenType.Text, TokenTriggers.None);
		Keywords = new string[844]
		{
			"abort", "abort_after_wait", "absent", "absolute", "accelerated_database_recovery", "accent_sensitivity", "acceptable_cursopt", "acp", "action", "activation",
			"address", "admin", "advanced", "aes_128", "aes_192", "aes_256", "affinity", "after", "aggregate", "algorithm",
			"all_constraints", "all_errormsgs", "all_indexes", "all_levels", "all_results", "allow_connections", "allow_dup_row", "allow_encrypted_value_modifications", "allow_page_locks", "allow_row_locks",
			"allow_snapshot_isolation", "altercolumn", "always", "anonymous", "ansi_defaults", "ansi_null_default", "ansi_null_dflt_off", "ansi_null_dflt_on", "ansi_nulls", "ansi_padding",
			"ansi_warnings", "appdomain", "append", "application", "arithabort", "arithignore", "assembly", "asymmetric", "async", "asynchronous_commit",
			"at", "atan2", "atomic", "attach", "attach_force_rebuild_log", "attach_rebuild_log", "audit", "auth_realm", "authentication", "auto",
			"auto_cleanup", "auto_close", "auto_create_statistics", "auto_shrink", "auto_update_statistics", "auto_update_statistics_async", "automated_backup_preference", "automatic", "automatic_tuning", "autopilot",
			"availability", "availability_mode", "backup_priority", "backup_storage_redundancy", "base64", "basic", "batches", "batchsize", "before", "bigint",
			"binary", "binding", "bit", "block", "blockers", "blocksize", "bmk", "broker", "broker_instance", "bucket_count",
			"buffer", "buffercount", "bulk_logged", "call", "caller", "card", "catalog", "catch", "cert", "certificate",
			"change_retention", "change_tracking", "change_tracking_context", "changes", "char", "character", "character_set", "check_expiration", "check_policy", "checkconstraints",
			"checkindex", "classification", "cleanup_policy", "clear", "clear_port", "cluster_type", "codepage", "collection", "column_encryption_key", "column_master_key",
			"columnstore", "columnstore_archive", "colv_80_to_100", "colv_100_to_80", "commit_differential_base", "committed", "compatibility_level", "compress_all_row_groups", "compression", "compression_delay",
			"concat_null_yields_null", "concatenate", "configuration", "connect", "continue_after_error", "contract", "contract_name", "control", "conversation", "conversation_group_id",
			"conversation_handle", "copy", "copy_only", "count_rows", "counter", "credential", "critical", "cryptographic", "cryptographic_provider", "cube",
			"cursor_close_on_commit", "cursor_default", "custom", "data", "data_compression", "data_deletion", "data_flush_interval_seconds", "data_mirroring", "data_purity", "data_retention",
			"data_source", "database_name", "database_snapshot", "datafiletype", "date_correlation_optimization", "date", "datefirst", "dateformat", "date_format", "datetime",
			"datetime2", "datetimeoffset", "days", "db_chaining", "db_failover", "dbid", "dbidexec", "dbo_only", "deadlock_priority", "dec",
			"decimal", "decrypt", "decrypt_a", "decryption", "default_database", "default_language", "default_logon_domain", "default_schema", "definition", "delay",
			"delayed_durability", "delimitedtext", "density_vector", "dependent", "des", "description", "desired_state", "desx", "dhcp", "differential",
			"digest", "disable", "disable_broker", "disable_def_cnst_chk", "disabled", "disk", "distribution", "drop_existing", "dtc_support", "dts_buffers",
			"dump", "durability", "dynamic", "edge", "elements", "emergency", "empty", "enable", "enable_broker", "enabled",
			"encoding", "encrypted", "encrypted_value", "encryption", "encryption_type", "endpoint", "endpoint_url", "enhancedintegrity", "entry", "error_broker_conversations",
			"errorfile", "estimateonly", "event", "executable", "expand", "expiredate", "expiry_date", "explicit", "external_access", "failover",
			"failover_mode", "failure_condition_level", "fast", "fast_forward", "fastfirstrow", "federated_service_account", "field_terminator", "fieldterminator", "filelistonly", "filegroup",
			"filename", "filestream", "filestream_log", "filestream_on", "filetable", "file_format", "filter", "filter_column", "fips_flagger", "fire_triggers",
			"first", "firstrow", "first_row", "float", "flush_interval_seconds", "fmtonly", "following", "force", "force_failover_allow_data_loss", "force_last_good_plan",
			"force_service_allow_data_loss", "forced", "forceplan", "formatfile", "format_options", "format_type", "formsof", "forward_only", "free_cursors", "free_exec_context",
			"fullscan", "fulltext", "fulltextall", "fulltextkey", "generated", "get", "geography", "geometry", "global", "governor",
			"guid", "hadoop", "hardening", "hash", "hashed", "header_limit", "headeronly", "health_check_timeout", "hidden", "hierarchyid",
			"high", "histogram", "histogram_steps", "hits_cursors", "hits_exec_context", "hours", "http", "identity_value", "ifnull", "ignore_constraints",
			"ignore_dup_key", "ignore_dup_row", "ignore_triggers", "image", "immediate", "implicit_transactions", "include", "include_null_values", "infinite", "inflectional",
			"information_type", "information_type_id", "inherit", "init", "initiator", "input_options", "insensitive", "instead", "int", "integer",
			"integrated", "intermediate", "interval_length_minutes", "into", "inuse_cursors", "inuse_exec_context", "io", "ip", "isabout", "iso_week",
			"isolation", "job_tracker_location", "json", "keep", "keep_nulls", "keep_replication", "keepdefaults", "keepfixed", "keepidentity", "keepnulls",
			"kerberos", "key", "key_path", "key_source", "key_store_provider_name", "keyset", "kilobytes_per_batch", "label", "label_id", "labelonly",
			"langid", "language", "last", "lastrow", "ledger", "legacy_cardinality_estimation", "length", "level", "library", "lifetime",
			"lineage_80_to_100", "lineage_100_to_80", "listener", "listener_ip", "listener_port", "listener_url", "load", "loadhistory", "lob_compaction", "local",
			"local_service_name", "locate", "location", "lock_escalation", "lock_timeout", "lockres", "login", "login_type", "loop", "low",
			"manual", "mark_in_use_for_removal", "masked", "master", "match", "max_queue_readers", "max_duration", "max_outstanding_io_per_volume", "maxdop", "maxerrors",
			"maxlength", "maxtransfersize", "max_plans_per_query", "max_storage_size_mb", "mediadescription", "medianame", "mediapassword", "medium", "memogroup", "memory_optimized",
			"merge", "message", "message_forward_size", "message_forwarding", "microsecond", "millisecond", "minutes", "mirror_address", "misses_cursors", "misses_exec_context",
			"mixed", "modify", "money", "move", "multi_user", "must_change", "name", "namespace", "nanosecond", "native",
			"native_compilation", "nchar", "ncharacter", "never", "new_account", "new_broker", "newname", "next", "no", "no_browsetable",
			"no_checksum", "no_compression", "no_infomsgs", "no_triggers", "no_truncate", "nocount", "node", "noexec", "noexpand", "noformat",
			"noinit", "nolock", "nonatomic", "nondurable", "none", "norecompute", "norecovery", "noreset", "norewind", "noskip",
			"notification", "nounload", "now", "nowait", "ntext", "ntlm", "numeric", "numeric_roundabort", "nvarchar", "object",
			"objid", "oem", "offline", "old_account", "online", "operation_mode", "openjson", "optimistic", "optimize_for_sequential_key", "orc",
			"out", "output", "output_options", "override", "owner", "ownership", "pad_index", "page", "page_checksum", "page_verify",
			"pagecount", "paglock", "param", "parameter_sniffing", "parameter_type_expansion", "parameterization", "parquet", "parseonly", "partial", "partition",
			"partner", "password", "path", "pause", "percentage", "permission_set", "persisted", "persistent_log_buffer", "persistent_version_store_filegroup", "period",
			"per_db", "physical_only", "plan_forcing_mode", "policy", "pool", "population", "port", "ports", "preceding", "precision",
			"predicate", "presume_abort", "primary", "primary_role", "prior", "priority", "priority_level", "private", "procedure_name", "profile",
			"provider", "query_capture_mode", "query_governor_cost_limit", "query_optimizer_hotfixes", "query_store", "queue", "quoted_identifier", "range", "raw", "rcfile",
			"rc2", "rc4", "rc4_128", "rdbms", "read_committed_snapshot", "read_only", "read_only_routing_list", "read_only_routing_url", "read_write", "readcommitted",
			"readcommittedlock", "readonly", "readpast", "readuncommitted", "readwrite", "real", "rebuild", "receive", "recmodel_70backcomp", "recompile",
			"recovery", "recursive", "recursive_triggers", "redo_queue", "reject_sample_value", "reject_type", "reject_value", "rejected_row_location", "relative", "remote",
			"remote_data_archive", "remote_proc_transactions", "remote_service_name", "remove", "removed_cursors", "removed_exec_context", "reorganize", "repeat", "repeatable", "repeatableread",
			"replica", "replicated", "replnick_100_to_80", "replnickarray_80_to_100", "replnickarray_100_to_80", "required", "required_synchronized_secondaries_to_commit", "required_cursopt", "resample", "reset",
			"resource", "resource_manager_location", "restart", "restricted_user", "resumable", "resume", "retaindays", "retention", "retention_period", "rewind",
			"rewindonly", "returns", "robust", "role", "rollup", "root", "round_robin", "route", "row", "rowdump",
			"rowlock", "row_terminator", "rows", "rows_per_batch", "rowsets_only", "rowterminator", "rowversion", "rsa_1024", "rsa_2048", "rsa_3072",
			"rsa_4096", "rsa_512", "safe", "safety", "sample", "schemabinding", "scoped", "scroll", "scroll_locks", "sddl",
			"secexpr", "secondary", "secondary_only", "secondary_role", "secondary_type", "secret", "security", "securityaudit", "seeding_mode", "selective",
			"self", "send", "sensitivity", "sent", "sequence", "serde_method", "serializable", "server", "service", "service_broker",
			"service_name", "service_objective", "session_timeout", "session", "sessions", "seterror", "setopts", "sets", "shard_map_manager", "shard_map_name",
			"sharded", "shared_memory", "show_statistics", "showplan_all", "showplan_text", "showplan_xml", "showplan_xml_with_recompile", "shrinkdb", "sid", "signature",
			"simple", "single_blob", "single_clob", "single_nclob", "single_user", "singleton", "site", "size_based_cleanup_mode", "skip", "smalldatetime",
			"smallint", "smallmoney", "snapshot", "snapshot_import", "snapshotrestorephase", "soap", "softnuma", "sort_in_tempdb", "sorted_data", "sorted_data_reorg",
			"spatial", "sql", "sql_bigint", "sql_binary", "sql_bit", "sql_char", "sql_date", "sql_decimal", "sql_double", "sql_float",
			"sql_guid", "sql_handle", "sql_longvarbinary", "sql_longvarchar", "sql_numeric", "sql_real", "sql_smallint", "sql_time", "sql_timestamp", "sql_tinyint",
			"sql_tsi_day", "sql_tsi_frac_second", "sql_tsi_hour", "sql_tsi_minute", "sql_tsi_month", "sql_tsi_quarter", "sql_tsi_second", "sql_tsi_week", "sql_tsi_year", "sql_type_date",
			"sql_type_time", "sql_type_timestamp", "sql_varbinary", "sql_varchar", "sql_variant", "sql_wchar", "sql_wlongvarchar", "ssl", "ssl_port", "stale_page_detection",
			"standard", "standby", "start", "start_date", "started", "stat_header", "state", "statement", "static", "statistics_incremental",
			"statistics_norecompute", "statistics_only", "statman", "stats_stream", "status", "stop", "stop_on_error", "stopat", "stopatmark", "stopbeforemark",
			"stoplist", "stopped", "stream", "string_delimiter", "subject", "supplemental_logging", "supported", "suspend", "symmetric", "synchronous_commit",
			"synonym", "sysname", "system", "system_time", "system_versioning", "tableresults", "tablock", "tablockx", "take", "tape",
			"target", "target_index", "target_partition", "tcp", "temporal_history_retention", "text", "textimage_on", "thesaurus", "throw", "time",
			"timeout", "timestamp", "tinyint", "torn_page_detection", "track_columns_updated", "transfer", "triple_des", "triple_des_3key", "trustworthy", "try",
			"tsql", "type", "type_desc", "type_warning", "tzoffset", "uid", "unbounded", "uncommitted", "uniqueidentifier", "unlimited",
			"unload", "unlock", "unsafe", "updlock", "url", "useplan", "useroptions", "use_type_default", "using", "utcdatetime",
			"valid_xml", "validation", "value", "varbinary", "varchar", "verbose", "verifyonly", "version", "view_metadata", "virtual_device",
			"visiblity", "wait_at_low_priority", "webmethod", "weekday", "weight", "well_formed_xml", "widechar", "widechar_ansi", "widenative", "windows",
			"within", "witness", "without", "without_array_wrapper", "workload", "wsdl", "wsfc", "xact_abort", "xlock", "xml",
			"xmlschema", "xquery", "xsinil", "zone"
		};
		Operators = new string[50]
		{
			"=", "*", ";", "+", ":", "%", "{", "}", ">", "!",
			"<", "-", "~", "/", "&", "|", "^", "::", "+=", "-=",
			"*=", "/=", "%=", "&=", "|=", "^=", "all", "and", "any", "apply",
			"between", "contained", "cross", "exists", "in", "inner", "is", "join", "left", "like",
			"matched", "not", "null", "or", "outer", "pivot", "right", "some", "source", "unpivot"
		};
		SqlCMDRegExp = new string[9] { ":connect", ":error", ":?exit", ":on\\s+error", ":out", ":?quit", ":r", ":setvar", ":?!!" };
		SystemFunctions = new string[62]
		{
			"$action", "$identity", "$partition", "$rowguid", "$sys_change_xdes_id", "approx_count_distinct", "avg", "changetable", "checksum_agg", "coalesce",
			"connection_htoken", "count", "count_big", "current_user", "databaseproperty", "decryptbyasmkey", "decryptbykeyautoasmkey", "dense_rank", "disable_txf_on_filestream", "encrypt_a",
			"encryptbyasmkey", "fileasimage", "fileasntext", "fileastext", "filecontents", "filepermissions", "get_transaction_context", "getpathname", "grouping", "grouping_id",
			"has_access_by_sd", "has_access_by_sd_ex", "hierarchical_type_id", "i4_row_number", "impersonationtoken", "item_domain", "item_domain_is_root", "item_domain_limit", "lag", "lead",
			"max", "max_access_mask", "min", "normalizedigits", "ntile", "nullif", "rank", "rfsshortname", "row_number", "session_user",
			"stdev", "stdevp", "step_direction", "string_agg", "string_split", "sum", "sys.fn_builtin_permissions", "system_user", "try_convert", "var",
			"varp", "verifysignedbyasmkey"
		};
		SystemStoredProcedures = new string[1854]
		{
			"sp_activedirectory_obj", "sp_activedirectory_scp", "sp_activedirectory_start", "sp_add_agent_parameter", "sp_add_agent_profile", "sp_add_alert", "sp_add_alert_internal", "sp_add_category", "sp_add_data_file_recover_suspect_db", "sp_add_dtscategory",
			"sp_add_dtspackage", "sp_add_job", "sp_add_jobschedule", "sp_add_jobserver", "sp_add_jobstep", "sp_add_jobstep_internal", "sp_add_log_file_recover_suspect_db", "sp_add_log_shipping_alert_job", "sp_add_log_shipping_monitor_jobs", "sp_add_log_shipping_primary",
			"sp_add_log_shipping_primary_database", "sp_add_log_shipping_primary_secondary", "sp_add_log_shipping_secondary", "sp_add_log_shipping_secondary_database", "sp_add_log_shipping_secondary_primary", "sp_add_maintenance_plan", "sp_add_maintenance_plan_db", "sp_add_maintenance_plan_job", "sp_add_notification", "sp_add_operator",
			"sp_add_proxy", "sp_add_schedule", "sp_add_targetservergroup", "sp_add_targetsvrgrp_member", "sp_addapprole", "sp_addarticle", "sp_adddatatype", "sp_adddatatypemapping", "sp_adddistpublisher", "sp_adddistributiondb",
			"sp_adddistributor", "sp_adddynamicsnapshot_job", "sp_addextendedproc", "sp_addextendedproperty", "sp_addfunctionalunittocomponent", "sp_addlinkedserver", "sp_addlinkedsrvlogin", "sp_addlogin", "sp_addlogreader_agent", "sp_addmergealternatepublisher",
			"sp_addmergearticle", "sp_addmergefilter", "sp_addmergelogsettings", "sp_addmergepartition", "sp_addmergepublication", "sp_addmergepullsubscription", "sp_addmergepullsubscription_agent", "sp_addmergepushsubscription_agent", "sp_addmergesubscription", "sp_addmessage",
			"sp_addpublication", "sp_addpublication_snapshot", "sp_addpullsubscription", "sp_addpullsubscription_agent", "sp_addpushsubscription_agent", "sp_addqreader_agent", "sp_addqueued_artinfo", "sp_addremotelogin", "sp_addrole", "sp_addrolemember",
			"sp_addscriptexec", "sp_addserver", "sp_addsrvrolemember", "sp_addsubscriber", "sp_addsubscriber_schedule", "sp_addsubscription", "sp_addsynctriggers", "sp_addsynctriggerscore", "sp_addtabletocontents", "sp_addtask",
			"sp_addtype", "sp_addumpdevice", "sp_adduser", "sp_adjustpublisheridentityrange", "sp_altermessage", "sp_apply_job_to_targets", "sp_approlepassword", "sp_article_validation", "sp_articlecolumn", "sp_articlefilter",
			"sp_articleview", "sp_assemblies_rowset", "sp_assemblies_rowset_rmt", "sp_assemblies_rowset2", "sp_assembly_dependencies_rowset", "sp_assembly_dependencies_rowset_rmt", "sp_assembly_dependencies_rowset2", "sp_attach_db", "sp_attach_schedule", "sp_attach_single_file_db",
			"sp_attachsubscription", "sp_autostats", "sp_batch_params", "sp_bcp_dbcmptlevel", "sp_bindefault", "sp_bindrule", "sp_bindsession", "sp_browsemergesnapshotfolder", "sp_browsereplcmds", "sp_browsesnapshotfolder",
			"sp_can_tlog_be_applied", "sp_catalogs", "sp_catalogs_rowset", "sp_catalogs_rowset_rmt", "sp_catalogs_rowset2", "sp_cdc_add_job", "sp_cdc_change_job", "sp_cdc_cleanup_change_table", "sp_cdc_disable_db", "sp_cdc_disable_db_change_data_capture",
			"sp_cdc_disable_table", "sp_cdc_disable_table_change_data_capture", "sp_cdc_drop_job", "sp_cdc_enable_db", "sp_cdc_enable_db_change_data_capture", "sp_cdc_enable_table", "sp_cdc_enable_table_change_data_capture", "sp_cdc_generate_wrapper_function", "sp_cdc_get_captured_columns", "sp_cdc_get_ddl_history",
			"sp_cdc_help_change_data_capture", "sp_cdc_help_jobs", "sp_cdc_restoredb", "sp_cdc_scan", "sp_cdc_start_job", "sp_cdc_stop_job", "sp_cdc_vupgrade", "sp_certify_removable", "sp_change_agent_parameter", "sp_change_agent_profile",
			"sp_change_log_shipping_primary_database", "sp_change_log_shipping_secondary_database", "sp_change_log_shipping_secondary_primary", "sp_change_monitor_role", "sp_change_subscription_properties", "sp_change_users_login", "sp_changearticle", "sp_changearticlecolumndatatype", "sp_changedbowner", "sp_changedistpublisher",
			"sp_changedistributiondb", "sp_changedistributor_password", "sp_changedistributor_property", "sp_changedynamicsnapshot_job", "sp_changelogreader_agent", "sp_changemergearticle", "sp_changemergefilter", "sp_changemergelogsettings", "sp_changemergepublication", "sp_changemergepullsubscription",
			"sp_changemergesubscription", "sp_changeobjectowner", "sp_changepublication", "sp_changepublication_snapshot", "sp_changeqreader_agent", "sp_changereplicationserverpasswords", "sp_changesubscriber", "sp_changesubscriber_schedule", "sp_changesubscription", "sp_changesubscriptiondtsinfo",
			"sp_changesubstatus", "sp_check_constbytable_rowset", "sp_check_constbytable_rowset2", "sp_check_constraints_rowset", "sp_check_constraints_rowset2", "sp_check_dynamic_filters", "sp_check_for_owned_jobs", "sp_check_for_owned_jobsteps", "sp_check_for_sync_trigger", "sp_check_join_filter",
			"sp_check_log_shipping_monitor_alert", "sp_check_publication_access", "sp_check_removable", "sp_check_subset_filter", "sp_check_sync_trigger", "sp_checkinvalidivarticle", "sp_checkoraclepackageversion", "sp_cleanmergelogfiles", "sp_cleanup_data_retention", "sp_cleanup_log_shipping_history",
			"sp_cleanupdbreplication", "sp_cleanupwebtask", "sp_clear_dbmaintplan_by_db", "sp_column_privileges", "sp_column_privileges_ex", "sp_column_privileges_rowset", "sp_column_privileges_rowset_rmt", "sp_column_privileges_rowset2", "sp_columns", "sp_columns_100",
			"sp_columns_100_rowset", "sp_columns_100_rowset2", "sp_columns_90", "sp_columns_90_rowset", "sp_columns_90_rowset_rmt", "sp_columns_90_rowset2", "sp_columns_ex", "sp_columns_ex_100", "sp_columns_ex_90", "sp_columns_rowset",
			"sp_columns_rowset_rmt", "sp_columns_rowset2", "sp_configure", "sp_configure_peerconflictdetection", "sp_constr_col_usage_rowset", "sp_constr_col_usage_rowset2", "sp_control_dbmasterkey_password", "sp_control_plan_guide", "sp_convert_jobid_to_char", "sp_convertwebtasks",
			"sp_copymergesnapshot", "sp_copysnapshot", "sp_copysubscription", "sp_create_log_shipping_monitor_account", "sp_create_plan_guide", "sp_create_plan_guide_from_plan_handle", "sp_create_removable", "sp_createmergepalrole", "sp_createorphan", "sp_createstats",
			"sp_createtranpalrole", "sp_cursor", "sp_cursor_list", "sp_cursorclose", "sp_cursorexecute", "sp_cursorfetch", "sp_cursoropen", "sp_cursoroption", "sp_cursorprepare", "sp_cursorprepexec",
			"sp_cursorunprepare", "sp_cycle_agent_errorlog", "sp_cycle_errorlog", "sp_databases", "sp_datatype_info", "sp_datatype_info_100", "sp_datatype_info_90", "sp_db_vardecimal_storage_format", "sp_dbcmptlevel", "sp_dbfixedrolepermission",
			"sp_dbmmonitoraddmonitoring", "sp_dbmmonitorchangealert", "sp_dbmmonitorchangemonitoring", "sp_dbmmonitordropalert", "sp_dbmmonitordropmonitoring", "sp_dbmmonitorhelpalert", "sp_dbmmonitorhelpmonitoring", "sp_dbmmonitorresults", "sp_dbmmonitorupdate", "sp_dboption",
			"sp_dbremove", "sp_ddopen", "sp_defaultdb", "sp_defaultlanguage", "sp_delete_alert", "sp_delete_all_msx_jobs", "sp_delete_backuphistory", "sp_delete_category", "sp_delete_database_backuphistory", "sp_delete_http_namespace_reservation",
			"sp_delete_job", "sp_delete_job_references", "sp_delete_jobschedule", "sp_delete_jobserver", "sp_delete_jobstep", "sp_delete_jobsteplog", "sp_delete_log_shipping_alert_job", "sp_delete_log_shipping_monitor_info", "sp_delete_log_shipping_monitor_jobs", "sp_delete_log_shipping_primary",
			"sp_delete_log_shipping_primary_database", "sp_delete_log_shipping_primary_secondary", "sp_delete_log_shipping_secondary", "sp_delete_log_shipping_secondary_database", "sp_delete_log_shipping_secondary_primary", "sp_delete_maintenance_plan", "sp_delete_maintenance_plan_db", "sp_delete_maintenance_plan_job", "sp_delete_notification", "sp_delete_operator",
			"sp_delete_proxy", "sp_delete_schedule", "sp_delete_targetserver", "sp_delete_targetservergroup", "sp_delete_targetsvrgrp_member", "sp_deletemergeconflictrow", "sp_deletepeerrequesthistory", "sp_deletetracertokenhistory", "sp_denylogin", "sp_depends",
			"sp_describe_cursor", "sp_describe_cursor_columns", "sp_describe_cursor_tables", "sp_detach_db", "sp_detach_schedule", "sp_disableagentoffload", "sp_distcounters", "sp_downloaded_row_limiter", "sp_drop_agent_parameter", "sp_drop_agent_profile",
			"sp_drop_dtscategory", "sp_drop_dtspackage", "sp_drop_streaming_job", "sp_dropalias", "sp_dropanonymousagent", "sp_dropanonymoussubscription", "sp_dropapprole", "sp_droparticle", "sp_dropdatatypemapping", "sp_dropdevice",
			"sp_dropdistpublisher", "sp_dropdistributiondb", "sp_dropdistributor", "sp_dropdynamicsnapshot_job", "sp_dropextendedproc", "sp_dropextendedproperty", "sp_droplinkedsrvlogin", "sp_droplogin", "sp_dropmergealternatepublisher", "sp_dropmergearticle",
			"sp_dropmergefilter", "sp_dropmergelogsettings", "sp_dropmergepartition", "sp_dropmergepublication", "sp_dropmergepullsubscription", "sp_dropmergesubscription", "sp_dropmessage", "sp_droporphans", "sp_droppublication", "sp_droppublisher",
			"sp_droppullsubscription", "sp_dropremotelogin", "sp_dropreplsymmetrickey", "sp_droprole", "sp_droprolemember", "sp_dropserver", "sp_dropsrvrolemember", "sp_dropsubscriber", "sp_dropsubscription", "sp_droptask",
			"sp_droptype", "sp_dropuser", "sp_dropwebtask", "sp_dsninfo", "sp_dump_dtslog_all", "sp_dump_dtspackagelog", "sp_dump_dtssteplog", "sp_dump_dtstasklog", "sp_enable_heterogeneous_subscription", "sp_enable_sql_debug",
			"sp_enableagentoffload", "sp_enlist_tsx", "sp_enum_dtscategories", "sp_enum_dtspackagelog", "sp_enum_dtspackages", "sp_enum_dtssteplog", "sp_enum_dtstasklog", "sp_enum_login_for_proxy", "sp_enum_oledb_providers", "sp_enum_proxy_for_subsystem",
			"sp_enum_sqlagent_subsystems", "sp_enum_sqlagent_subsystems_internal", "sp_enumcodepages", "sp_enumcustomresolvers", "sp_enumdsn", "sp_enumeratependingschemachanges", "sp_enumerrorlogs", "sp_enumfullsubscribers", "sp_enumoledbdatasources", "sp_estimated_rowsize_reduction_for_vardecimal",
			"sp_execute", "sp_executesql", "sp_execute_external_script", "sp_expired_subscription_cleanup", "sp_externalmailqueuelistener", "sp_fetchlobfromcookie", "sp_filestream_configure", "sp_firstonly_bitmap", "sp_fkeys", "sp_flush_commit_table",
			"sp_foreign_keys_rowset", "sp_foreign_keys_rowset_rmt", "sp_foreign_keys_rowset2", "sp_foreign_keys_rowset3", "sp_foreignkeys", "sp_fulltext_catalog", "sp_fulltext_column", "sp_fulltext_database", "sp_fulltext_freelist", "sp_fulltext_getdata",
			"sp_fulltext_keymappings", "sp_fulltext_pendingchanges", "sp_fulltext_recycle_crawl_log", "sp_fulltext_service", "sp_fulltext_table", "sp_fuzzylookuptablemaintenanceinstall", "sp_fuzzylookuptablemaintenanceinvoke", "sp_fuzzylookuptablemaintenanceuninstall", "sp_generate_agent_parameter", "sp_generate_database_ledger_digest",
			"sp_generate_server_description", "sp_generate_target_server_job_assignment_sql", "sp_generatefilters", "sp_get_chunked_jobstep_params", "sp_get_composite_job_info", "sp_get_distributor", "sp_get_dtspackage", "sp_get_dtsversion", "sp_get_job_alerts", "sp_get_job_status_mergesubscription_agent",
			"sp_get_jobstep_db_username", "sp_get_log_shipping_monitor_info", "sp_get_mergepublishedarticleproperties", "sp_get_message_description", "sp_get_oracle_publisher_metadata", "sp_get_query_template", "sp_get_schedule_description", "sp_get_script", "sp_get_sqlagent_properties", "sp_get_streaming_job",
			"sp_getagentparameterlist", "sp_getapplock", "sp_getattachmentdata", "sp_getbindtoken", "sp_getdefaultdatatypemapping", "sp_getmergedeletetype", "sp_getpublisherlink", "sp_getqueuedarticlesynctraninfo", "sp_getqueuedrows", "sp_getschemalock",
			"sp_getsqlqueueversion", "sp_getsubscription_status_hsnapshot", "sp_getsubscriptiondtspackagename", "sp_gettopologyinfo", "sp_grant_login_to_proxy", "sp_grant_proxy_to_subsystem", "sp_grant_publication_access", "sp_grantdbaccess", "sp_grantlogin", "sp_help",
			"sp_help_agent_default", "sp_help_agent_parameter", "sp_help_agent_profile", "sp_help_alert", "sp_help_category", "sp_help_datatype_mapping", "sp_help_downloadlist", "sp_help_fulltext_catalog_components", "sp_help_fulltext_catalogs", "sp_help_fulltext_catalogs_cursor",
			"sp_help_fulltext_columns", "sp_help_fulltext_columns_cursor", "sp_help_fulltext_system_components", "sp_help_fulltext_tables", "sp_help_fulltext_tables_cursor", "sp_help_job", "sp_help_jobactivity", "sp_help_jobcount", "sp_help_jobhistory", "sp_help_jobhistory_full",
			"sp_help_jobhistory_sem", "sp_help_jobhistory_summary", "sp_help_jobs_in_schedule", "sp_help_jobschedule", "sp_help_jobserver", "sp_help_jobstep", "sp_help_jobsteplog", "sp_help_log_shipping_alert_job", "sp_help_log_shipping_monitor", "sp_help_log_shipping_monitor_primary",
			"sp_help_log_shipping_monitor_secondary", "sp_help_log_shipping_primary_database", "sp_help_log_shipping_primary_secondary", "sp_help_log_shipping_secondary_database", "sp_help_log_shipping_secondary_primary", "sp_help_maintenance_plan", "sp_help_notification", "sp_help_operator", "sp_help_operator_jobs", "sp_help_peerconflictdetection",
			"sp_help_proxy", "sp_help_publication_access", "sp_help_schedule", "sp_help_targetserver", "sp_help_targetservergroup", "sp_helpallowmerge_publication", "sp_helparticle", "sp_helparticlecolumns", "sp_helparticledts", "sp_helpconstraint",
			"sp_helpdatatypemap", "sp_helpdb", "sp_helpdbfixedrole", "sp_helpdevice", "sp_helpdistpublisher", "sp_helpdistributiondb", "sp_helpdistributor", "sp_helpdistributor_properties", "sp_helpdynamicsnapshot_job", "sp_helpextendedproc",
			"sp_helpfile", "sp_helpfilegroup", "sp_helpindex", "sp_helplanguage", "sp_helplinkedsrvlogin", "sp_helplogins", "sp_helplogreader_agent", "sp_helpmergealternatepublisher", "sp_helpmergearticle", "sp_helpmergearticlecolumn",
			"sp_helpmergearticleconflicts", "sp_helpmergeconflictrows", "sp_helpmergedeleteconflictrows", "sp_helpmergefilter", "sp_helpmergelogfiles", "sp_helpmergelogfileswithdata", "sp_helpmergelogsettings", "sp_helpmergepartition", "sp_helpmergepublication", "sp_helpmergepullsubscription",
			"sp_helpmergesubscription", "sp_helpntgroup", "sp_helppeerrequests", "sp_helppeerresponses", "sp_helppublication", "sp_helppublication_snapshot", "sp_helppublicationsync", "sp_helppullsubscription", "sp_helpqreader_agent", "sp_helpremotelogin",
			"sp_helpreplfailovermode", "sp_helpreplicationdb", "sp_helpreplicationdboption", "sp_helpreplicationoption", "sp_helprole", "sp_helprolemember", "sp_helprotect", "sp_helpserver", "sp_helpsort", "sp_helpsrvrole",
			"sp_helpsrvrolemember", "sp_helpstats", "sp_helpsubscriberinfo", "sp_helpsubscription", "sp_helpsubscription_properties", "sp_helpsubscriptionerrors", "sp_helptext", "sp_helptracertokenhistory", "sp_helptracertokens", "sp_helptrigger",
			"sp_helpuser", "sp_helpxactsetjob", "sp_http_generate_wsdl_complex", "sp_http_generate_wsdl_defaultcomplexorsimple", "sp_http_generate_wsdl_defaultsimpleorcomplex", "sp_http_generate_wsdl_simple", "sp_identitycolumnforreplication", "sp_ih_lr_getcachedata", "sp_ihadd_sync_command", "sp_iharticlecolumn",
			"sp_ihget_loopback_detection", "sp_ihscriptidxfile", "sp_ihscriptschfile", "sp_ihvalidaterowfilter", "sp_ihxactsetjob", "sp_indexes", "sp_indexes_100_rowset", "sp_indexes_100_rowset2", "sp_indexes_90_rowset", "sp_indexes_90_rowset_rmt",
			"sp_indexes_90_rowset2", "sp_indexes_rowset", "sp_indexes_rowset_rmt", "sp_indexes_rowset2", "sp_indexoption", "sp_invalidate_textptr", "sp_is_makegeneration_needed", "sp_is_sqlagent_starting", "sp_isprohibited", "sp_ivindexhasnullcols",
			"sp_jobhistory_row_limiter", "sp_lightweightmergemetadataretentioncleanup", "sp_link_publication", "sp_linkedservers", "sp_linkedservers_rowset", "sp_linkedservers_rowset2", "sp_lock", "sp_log_dtspackage_begin", "sp_log_dtspackage_end", "sp_log_dtsstep_begin",
			"sp_log_dtsstep_end", "sp_log_dtstask", "sp_log_shipping_get_date_from_file", "sp_log_shipping_in_sync", "sp_log_shipping_monitor_backup", "sp_log_shipping_monitor_restore", "sp_logshippinginstallmetadata", "sp_lookupcustomresolver", "sp_mailitemresultsets", "sp_maintplan_close_logentry",
			"sp_maintplan_delete_log", "sp_maintplan_delete_plan", "sp_maintplan_delete_subplan", "sp_maintplan_open_logentry", "sp_maintplan_start", "sp_maintplan_subplans_by_job", "sp_maintplan_update_log", "sp_maintplan_update_subplan", "sp_maintplan_update_subplan_tsx", "sp_make_dtspackagename",
			"sp_makewebtask", "sp_manage_jobs_by_login", "sp_mapdown_bitmap", "sp_markpendingschemachange", "sp_marksubscriptionvalidation", "sp_mergearticlecolumn", "sp_mergecleanupmetadata", "sp_mergedummyupdate", "sp_mergemetadataretentioncleanup", "sp_mergesubscription_cleanup",
			"sp_mergesubscriptionsummary", "sp_modify_dtscategory", "sp_monitor", "sp_ms_replication_installed", "sp_msacquireheadofqueuelock", "sp_msacquireserverresourcefordynamicsnapshot", "sp_msacquireslotlock", "sp_msacquiresnapshotdeliverysessionlock", "sp_msactivate_auto_sub", "sp_msactivatelogbasedarticleobject",
			"sp_msactivateprocedureexecutionarticleobject", "sp_msadd_anonymous_agent", "sp_msadd_article", "sp_msadd_compensating_cmd", "sp_msadd_distribution_agent", "sp_msadd_distribution_history", "sp_msadd_dynamic_snapshot_location", "sp_msadd_filteringcolumn", "sp_msadd_log_shipping_error_detail", "sp_msadd_log_shipping_history_detail",
			"sp_msadd_logreader_agent", "sp_msadd_logreader_history", "sp_msadd_merge_agent", "sp_msadd_merge_anonymous_agent", "sp_msadd_merge_history", "sp_msadd_merge_history90", "sp_msadd_merge_subscription", "sp_msadd_mergereplcommand", "sp_msadd_mergesubentry_indistdb", "sp_msadd_publication",
			"sp_msadd_qreader_agent", "sp_msadd_qreader_history", "sp_msadd_repl_alert", "sp_msadd_repl_command", "sp_msadd_repl_commands27hp", "sp_msadd_repl_error", "sp_msadd_replcmds_mcit", "sp_msadd_replmergealert", "sp_msadd_snapshot_agent", "sp_msadd_snapshot_history",
			"sp_msadd_subscriber_info", "sp_msadd_subscriber_schedule", "sp_msadd_subscription", "sp_msadd_subscription_3rd", "sp_msadd_tracer_history", "sp_msadd_tracer_token", "sp_msaddanonymousreplica", "sp_msadddynamicsnapshotjobatdistributor", "sp_msaddguidcolumn", "sp_msaddguidindex",
			"sp_msaddinitialarticle", "sp_msaddinitialpublication", "sp_msaddinitialschemaarticle", "sp_msaddinitialsubscription", "sp_msaddlightweightmergearticle", "sp_msaddmergedynamicsnapshotjob", "sp_msaddmergetriggers", "sp_msaddmergetriggers_from_template", "sp_msaddmergetriggers_internal", "sp_msaddpeerlsn",
			"sp_msaddsubscriptionarticles", "sp_msadjust_pub_identity", "sp_msagent_retry_stethoscope", "sp_msagent_stethoscope", "sp_msallocate_new_identity_range", "sp_msalreadyhavegeneration", "sp_msanonymous_status", "sp_msarticlecleanup", "sp_msbrowsesnapshotfolder", "sp_mscache_agent_parameter",
			"sp_mscdc_capture_job", "sp_mscdc_cleanup_job", "sp_mscdc_db_ddl_event", "sp_mscdc_ddl_event", "sp_mscdc_logddl", "sp_mscdc_updatemetadata", "sp_mschange_article", "sp_mschange_distribution_agent_properties", "sp_mschange_logreader_agent_properties", "sp_mschange_merge_agent_properties",
			"sp_mschange_mergearticle", "sp_mschange_mergepublication", "sp_mschange_originatorid", "sp_mschange_priority", "sp_mschange_publication", "sp_mschange_retention", "sp_mschange_retention_period_unit", "sp_mschange_snapshot_agent_properties", "sp_mschange_subscription_dts_info", "sp_mschangearticleresolver",
			"sp_mschangedynamicsnapshotjobatdistributor", "sp_mschangedynsnaplocationatdistributor", "sp_mschangeobjectowner", "sp_mscheck_agent_instance", "sp_mscheck_jet_subscriber", "sp_mscheck_logicalrecord_metadatamatch", "sp_mscheck_merge_subscription_count", "sp_mscheck_pub_identity", "sp_mscheck_pull_access", "sp_mscheck_snapshot_agent",
			"sp_mscheck_subscription", "sp_mscheck_subscription_expiry", "sp_mscheck_subscription_partition", "sp_mscheck_tran_retention", "sp_mscheckexistsgeneration", "sp_mscheckexistsrecguid", "sp_mscheckidentityrange", "sp_mschecksharedagentforpublication", "sp_mschecksnapshotstatus", "sp_mscleanup_agent_entry",
			"sp_mscleanup_conflict", "sp_mscleanup_publication_adinfo", "sp_mscleanup_subscription_distside_entry", "sp_mscleanupdynamicsnapshotfolder", "sp_mscleanupdynsnapshotvws", "sp_mscleanupforpullreinit", "sp_mscleanupmergepublisher", "sp_mscleanupmergepublisher_internal", "sp_msclear_dynamic_snapshot_location", "sp_msclearresetpartialsnapshotprogressbit",
			"sp_mscomputelastsentgen", "sp_mscomputemergearticlescreationorder", "sp_mscomputemergeunresolvedrefs", "sp_msconflicttableexists", "sp_mscreate_all_article_repl_views", "sp_mscreate_article_repl_views", "sp_mscreate_dist_tables", "sp_mscreate_logical_record_views", "sp_mscreate_sub_tables", "sp_mscreatedisabledmltrigger",
			"sp_mscreatedummygeneration", "sp_mscreateglobalreplica", "sp_mscreatelightweightinsertproc", "sp_mscreatelightweightmultipurposeproc", "sp_mscreatelightweightprocstriggersconstraints", "sp_mscreatelightweightupdateproc", "sp_mscreatemergedynamicsnapshot", "sp_mscreateretry", "sp_msdbuseraccess", "sp_msdbuserpriv",
			"sp_msdefer_check", "sp_msdelete_tracer_history", "sp_msdeletefoldercontents", "sp_msdeletemetadataactionrequest", "sp_msdeletepeerconflictrow", "sp_msdeleteretry", "sp_msdeletetranconflictrow", "sp_msdelgenzero", "sp_msdelrow", "sp_msdelrowsbatch",
			"sp_msdelrowsbatch_downloadonly", "sp_msdelsubrows", "sp_msdelsubrowsbatch", "sp_msdependencies", "sp_msdetect_nonlogged_shutdown", "sp_msdetectinvalidpeerconfiguration", "sp_msdetectinvalidpeersubscription", "sp_msdist_activate_auto_sub", "sp_msdist_adjust_identity", "sp_msdistpublisher_cleanup",
			"sp_msdistribution_counters", "sp_msdistributoravailable", "sp_msdodatabasesnapshotinitiation", "sp_msdopartialdatabasesnapshotinitiation", "sp_msdrop_6x_publication", "sp_msdrop_6x_replication_agent", "sp_msdrop_anonymous_entry", "sp_msdrop_article", "sp_msdrop_distribution_agent", "sp_msdrop_distribution_agentid_dbowner_proxy",
			"sp_msdrop_dynamic_snapshot_agent", "sp_msdrop_logreader_agent", "sp_msdrop_merge_agent", "sp_msdrop_merge_subscription", "sp_msdrop_publication", "sp_msdrop_qreader_history", "sp_msdrop_snapshot_agent", "sp_msdrop_snapshot_dirs", "sp_msdrop_subscriber_info", "sp_msdrop_subscription",
			"sp_msdrop_subscription_3rd", "sp_msdroparticleconstraints", "sp_msdroparticletombstones", "sp_msdropconstraints", "sp_msdropdynsnapshotvws", "sp_msdropfkreferencingarticle", "sp_msdropmergearticle", "sp_msdropmergedynamicsnapshotjob", "sp_msdropretry", "sp_msdroptemptable",
			"sp_msdummyupdate", "sp_msdummyupdate_logicalrecord", "sp_msdummyupdate90", "sp_msdummyupdatelightweight", "sp_msdynamicsnapshotjobexistsatdistributor", "sp_msenable_publication_for_het_sub", "sp_msensure_single_instance", "sp_msenum_distribution", "sp_msenum_distribution_s", "sp_msenum_distribution_sd",
			"sp_msenum_logicalrecord_changes", "sp_msenum_logreader", "sp_msenum_logreader_s", "sp_msenum_logreader_sd", "sp_msenum_merge", "sp_msenum_merge_agent_properties", "sp_msenum_merge_s", "sp_msenum_merge_sd", "sp_msenum_merge_subscriptions", "sp_msenum_merge_subscriptions_90_publication",
			"sp_msenum_merge_subscriptions_90_publisher", "sp_msenum_metadataaction_requests", "sp_msenum_qreader", "sp_msenum_qreader_s", "sp_msenum_qreader_sd", "sp_msenum_replication_agents", "sp_msenum_replication_job", "sp_msenum_replqueues", "sp_msenum_replsqlqueues", "sp_msenum_snapshot",
			"sp_msenum_snapshot_s", "sp_msenum_snapshot_sd", "sp_msenum_subscriptions", "sp_msenumallpublications", "sp_msenumallsubscriptions", "sp_msenumarticleslightweight", "sp_msenumchanges", "sp_msenumchanges_belongtopartition", "sp_msenumchanges_notbelongtopartition", "sp_msenumchangesdirect",
			"sp_msenumchangeslightweight", "sp_msenumcolumns", "sp_msenumcolumnslightweight", "sp_msenumdeletes_forpartition", "sp_msenumdeleteslightweight", "sp_msenumdeletesmetadata", "sp_msenumdistributionagentproperties", "sp_msenumerate_pal", "sp_msenumgenerations", "sp_msenumgenerations90",
			"sp_msenumpartialchanges", "sp_msenumpartialchangesdirect", "sp_msenumpartialdeletes", "sp_msenumpubreferences", "sp_msenumreplicas", "sp_msenumreplicas90", "sp_msenumretries", "sp_msenumschemachange", "sp_msenumsubscriptions", "sp_msenumthirdpartypublicationvendornames",
			"sp_msestimatemergesnapshotworkload", "sp_msestimatesnapshotworkload", "sp_msevalsubscriberinfo", "sp_msevaluate_change_membership_for_all_articles_in_pubid", "sp_msevaluate_change_membership_for_pubid", "sp_msevaluate_change_membership_for_row", "sp_msexecwithlsnoutput", "sp_msfast_delete_trans", "sp_msfetchadjustidentityrange", "sp_msfetchidentityrange",
			"sp_msfillupmissingcols", "sp_msfilterclause", "sp_msfix_6x_tasks", "sp_msfixlineageversions", "sp_msfixsubcolumnbitmaps", "sp_msfixupbeforeimagetables", "sp_msflush_access_cache", "sp_msforce_drop_distribution_jobs", "sp_msforcereenumeration", "sp_msforeach_worker",
			"sp_msforeachdb", "sp_msforeachtable", "sp_msgenerateexpandproc", "sp_msget_agent_names", "sp_msget_attach_state", "sp_msget_ddl_after_regular_snapshot", "sp_msget_dynamic_snapshot_location", "sp_msget_identity_range_info", "sp_msget_jobstate", "sp_msget_last_transaction",
			"sp_msget_latest_peerlsn", "sp_msget_load_hint", "sp_msget_log_shipping_new_sessionid", "sp_msget_logicalrecord_lineage", "sp_msget_max_used_identity", "sp_msget_min_seqno", "sp_msget_msmerge_rowtrack_colinfo", "sp_msget_new_xact_seqno", "sp_msget_oledbinfo", "sp_msget_partitionid_eval_proc",
			"sp_msget_publication_from_taskname", "sp_msget_publisher_rpc", "sp_msget_repl_cmds_anonymous", "sp_msget_repl_commands", "sp_msget_repl_error", "sp_msget_session_statistics", "sp_msget_shared_agent", "sp_msget_snapshot_history", "sp_msget_subscriber_partition_id", "sp_msget_subscription_dts_info",
			"sp_msget_subscription_guid", "sp_msget_synctran_commands", "sp_msget_type_wrapper", "sp_msgetagentoffloadinfo", "sp_msgetalertinfo", "sp_msgetalternaterecgens", "sp_msgetarticlereinitvalue", "sp_msgetchangecount", "sp_msgetconflictinsertproc", "sp_msgetconflicttablename",
			"sp_msgetcurrentprincipal", "sp_msgetdatametadatabatch", "sp_msgetdbversion", "sp_msgetdynamicsnapshotapplock", "sp_msgetdynsnapvalidationtoken", "sp_msgetisvalidwindowsloginfromdistributor", "sp_msgetlastrecgen", "sp_msgetlastsentgen", "sp_msgetlastsentrecgens", "sp_msgetlastupdatedtime",
			"sp_msgetlightweightmetadatabatch", "sp_msgetmakegenerationapplock", "sp_msgetmakegenerationapplock_90", "sp_msgetmaxbcpgen", "sp_msgetmaxsnapshottimestamp", "sp_msgetmergeadminapplock", "sp_msgetmetadata_changedlogicalrecordmembers", "sp_msgetmetadatabatch", "sp_msgetmetadatabatch90", "sp_msgetmetadatabatch90new",
			"sp_msgetonerow", "sp_msgetonerowlightweight", "sp_msgetpeerconflictrow", "sp_msgetpeerlsns", "sp_msgetpeertopeercommands", "sp_msgetpeerwinnerrow", "sp_msgetpubinfo", "sp_msgetreplicainfo", "sp_msgetreplicastate", "sp_msgetrowmetadata",
			"sp_msgetrowmetadatalightweight", "sp_msgetserverproperties", "sp_msgetsetupbelong_cost", "sp_msgetsubscriberinfo", "sp_msgetsupportabilitysettings", "sp_msgettrancftsrcrow", "sp_msgettranconflictrow", "sp_msgetversion", "sp_msgrantconnectreplication", "sp_mshaschangeslightweight",
			"sp_mshasdbaccess", "sp_mshelp_article", "sp_mshelp_distdb", "sp_mshelp_distribution_agentid", "sp_mshelp_identity_property", "sp_mshelp_logreader_agentid", "sp_mshelp_merge_agentid", "sp_mshelp_profile", "sp_mshelp_profilecache", "sp_mshelp_publication",
			"sp_mshelp_repl_agent", "sp_mshelp_replication_status", "sp_mshelp_replication_table", "sp_mshelp_snapshot_agent", "sp_mshelp_snapshot_agentid", "sp_mshelp_subscriber_info", "sp_mshelp_subscription", "sp_mshelp_subscription_status", "sp_mshelpcolumns", "sp_mshelpconflictpublications",
			"sp_mshelpcreatebeforetable", "sp_mshelpdestowner", "sp_mshelpdynamicsnapshotjobatdistributor", "sp_mshelpfulltextindex", "sp_mshelpfulltextscript", "sp_mshelpindex", "sp_mshelplogreader_agent", "sp_mshelpmergearticles", "sp_mshelpmergeconflictcounts", "sp_mshelpmergedynamicsnapshotjob",
			"sp_mshelpmergeidentity", "sp_mshelpmergeschemaarticles", "sp_mshelpobjectpublications", "sp_mshelpreplicationtriggers", "sp_mshelpsnapshot_agent", "sp_mshelpsummarypublication", "sp_mshelptracertokenhistory", "sp_mshelptracertokens", "sp_mshelptranconflictcounts", "sp_mshelptype",
			"sp_mshelpvalidationdate", "sp_msifexistssubscription", "sp_msindexspace", "sp_msinit_publication_access", "sp_msinit_subscription_agent", "sp_msinitdynamicsubscriber", "sp_msinsert_identity", "sp_msinsertdeleteconflict", "sp_msinserterrorlineage", "sp_msinsertgenerationschemachanges",
			"sp_msinsertgenhistory", "sp_msinsertlightweightschemachange", "sp_msinsertschemachange", "sp_msinvalidate_snapshot", "sp_msisnonpkukupdateinconflict", "sp_msispeertopeeragent", "sp_msispkupdateinconflict", "sp_msispublicationqueued", "sp_msisreplmergeagent", "sp_msissnapshotitemapplied",
			"sp_mskilldb", "sp_mslock_auto_sub", "sp_mslock_distribution_agent", "sp_mslocktable", "sp_msloginmappings", "sp_msmakearticleprocs", "sp_msmakebatchinsertproc", "sp_msmakebatchupdateproc", "sp_msmakeconflictinsertproc", "sp_msmakectsview",
			"sp_msmakedeleteproc", "sp_msmakedynsnapshotvws", "sp_msmakeexpandproc", "sp_msmakegeneration", "sp_msmakeinsertproc", "sp_msmakemetadataselectproc", "sp_msmakeselectproc", "sp_msmakesystableviews", "sp_msmakeupdateproc", "sp_msmap_partitionid_to_generations",
			"sp_msmarkreinit", "sp_msmatchkey", "sp_msmerge_alterschemaonly", "sp_msmerge_altertrigger", "sp_msmerge_alterview", "sp_msmerge_ddldispatcher", "sp_msmerge_getgencount", "sp_msmerge_getgencur_public", "sp_msmerge_is_snapshot_required", "sp_msmerge_log_identity_range_allocations",
			"sp_msmerge_parsegenlist", "sp_msmerge_upgrade_subscriber", "sp_msmergesubscribedb", "sp_msmergeupdatelastsyncinfo", "sp_msneedmergemetadataretentioncleanup", "sp_msnonsqlddl", "sp_msnonsqlddlforschemaddl", "sp_msobjectprivs", "sp_mspeerapplyresponse", "sp_mspeerapplytopologyinfo",
			"sp_mspeerconflictdetection_statuscollection_applyresponse", "sp_mspeerconflictdetection_statuscollection_sendresponse", "sp_mspeerconflictdetection_topology_applyresponse", "sp_mspeerdbinfo", "sp_mspeersendresponse", "sp_mspeersendtopologyinfo", "sp_mspeertopeerfwdingexec", "sp_mspost_auto_proc", "sp_mspostapplyscript_forsubscriberprocs", "sp_msprep_exclusive",
			"sp_msprepare_mergearticle", "sp_msprofile_in_use", "sp_msproxiedmetadata", "sp_msproxiedmetadatabatch", "sp_msproxiedmetadatalightweight", "sp_mspub_adjust_identity", "sp_mspublication_access", "sp_mspublicationcleanup", "sp_mspublicationview", "sp_msquery_syncstates",
			"sp_msquerysubtype", "sp_msrecordsnapshotdeliveryprogress", "sp_msreenable_check", "sp_msrefresh_anonymous", "sp_msrefresh_publisher_idrange", "sp_msregenerate_mergetriggersprocs", "sp_msregisterdynsnapseqno", "sp_msregistermergesnappubid", "sp_msregistersubscription", "sp_msreinit_failed_subscriptions",
			"sp_msreinit_hub", "sp_msreinit_subscription", "sp_msreinitoverlappingmergepublications", "sp_msreleasedynamicsnapshotapplock", "sp_msreleasemakegenerationapplock", "sp_msreleasemergeadminapplock", "sp_msreleaseslotlock", "sp_msreleasesnapshotdeliverysessionlock", "sp_msremove_mergereplcommand", "sp_msremoveoffloadparameter",
			"sp_msrepl_agentstatussummary", "sp_msrepl_backup_complete", "sp_msrepl_backup_start", "sp_msrepl_check_publisher", "sp_msrepl_createdatatypemappings", "sp_msrepl_distributionagentstatussummary", "sp_msrepl_dropdatatypemappings", "sp_msrepl_enumarticlecolumninfo", "sp_msrepl_enumpublications", "sp_msrepl_enumpublishertables",
			"sp_msrepl_enumsubscriptions", "sp_msrepl_enumtablecolumninfo", "sp_msrepl_fixpalrole", "sp_msrepl_getdistributorinfo", "sp_msrepl_getpkfkrelation", "sp_msrepl_gettype_mappings", "sp_msrepl_helparticlermo", "sp_msrepl_init_backup_lsns", "sp_msrepl_isdbowner", "sp_msrepl_islastpubinsharedsubscription",
			"sp_msrepl_isuserinanypal", "sp_msrepl_linkedservers_rowset", "sp_msrepl_mergeagentstatussummary", "sp_msrepl_pal_rolecheck", "sp_msrepl_raiserror", "sp_msrepl_schema", "sp_msrepl_setnfr", "sp_msrepl_snapshot_helparticlecolumns", "sp_msrepl_snapshot_helppublication", "sp_msrepl_startup",
			"sp_msrepl_startup_internal", "sp_msrepl_subscription_rowset", "sp_msrepl_testadminconnection", "sp_msrepl_testconnection", "sp_msreplagentjobexists", "sp_msreplcheck_permission", "sp_msreplcheck_pull", "sp_msreplcheck_subscribe", "sp_msreplcheck_subscribe_withddladmin", "sp_msreplcheckoffloadserver",
			"sp_msreplcopyscriptfile", "sp_msreplraiserror", "sp_msreplremoveuncdir", "sp_msreplupdateschema", "sp_msrequestreenumeration", "sp_msrequestreenumeration_lightweight", "sp_msreset_attach_state", "sp_msreset_queued_reinit", "sp_msreset_subscription", "sp_msreset_subscription_seqno",
			"sp_msreset_synctran_bit", "sp_msreset_transaction", "sp_msresetsnapshotdeliveryprogress", "sp_msrestoresavedforeignkeys", "sp_msretrieve_publication_attributes", "sp_msscript_article_view", "sp_msscript_dri", "sp_msscript_pub_upd_trig", "sp_msscript_sync_del_proc", "sp_msscript_sync_del_trig",
			"sp_msscript_sync_ins_proc", "sp_msscript_sync_ins_trig", "sp_msscript_sync_upd_proc", "sp_msscript_sync_upd_trig", "sp_msscriptcustomdelproc", "sp_msscriptcustominsproc", "sp_msscriptcustomupdproc", "sp_msscriptdatabase", "sp_msscriptdb_worker", "sp_msscriptforeignkeyrestore",
			"sp_msscriptsubscriberprocs", "sp_msscriptviewproc", "sp_mssendtosqlqueue", "sp_msset_dynamic_filter_options", "sp_msset_logicalrecord_metadata", "sp_msset_new_identity_range", "sp_msset_oledb_prop", "sp_msset_snapshot_xact_seqno", "sp_msset_sub_guid", "sp_msset_subscription_properties",
			"sp_mssetaccesslist", "sp_mssetalertinfo", "sp_mssetartprocs", "sp_mssetbit", "sp_mssetconflictscript", "sp_mssetconflicttable", "sp_mssetcontext_bypasswholeddleventbit", "sp_mssetcontext_replagent", "sp_mssetgentozero", "sp_mssetlastrecgen",
			"sp_mssetlastsentgen", "sp_mssetreplicainfo", "sp_mssetreplicaschemaversion", "sp_mssetreplicastatus", "sp_mssetrowmetadata", "sp_mssetserverproperties", "sp_mssetsubscriberinfo", "sp_mssettopology", "sp_mssetup_identity_range", "sp_mssetup_partition_groups",
			"sp_mssetup_use_partition_groups", "sp_mssetupbelongs", "sp_mssetupnosyncsubwithlsnatdist", "sp_mssharedfixeddisk", "sp_mssqldmo70_version", "sp_mssqldmo80_version", "sp_mssqldmo90_version", "sp_mssqlole_version", "sp_mssqlole65_version", "sp_msstartdistribution_agent",
			"sp_msstartmerge_agent", "sp_msstartsnapshot_agent", "sp_msstopdistribution_agent", "sp_msstopmerge_agent", "sp_msstopsnapshot_agent", "sp_mssub_check_identity", "sp_mssub_set_identity", "sp_mssubscription_status", "sp_mssubscriptionvalidated", "sp_mstablechecks",
			"sp_mstablekeys", "sp_mstablerefs", "sp_mstablespace", "sp_mstestbit", "sp_mstran_ddlrepl", "sp_mstran_is_snapshot_required", "sp_mstrypurgingoldsnapshotdeliveryprogress", "sp_msuniquename", "sp_msunmarkifneeded", "sp_msunmarkreplinfo",
			"sp_msunmarkschemaobject", "sp_msunregistersubscription", "sp_msupdate_agenttype_default", "sp_msupdate_singlelogicalrecordmetadata", "sp_msupdate_subscriber_info", "sp_msupdate_subscriber_schedule", "sp_msupdate_subscriber_tracer_history", "sp_msupdate_subscription", "sp_msupdate_tracer_history", "sp_msupdatecachedpeerlsn",
			"sp_msupdategenhistory", "sp_msupdateinitiallightweightsubscription", "sp_msupdatelastsyncinfo", "sp_msupdatepeerlsn", "sp_msupdaterecgen", "sp_msupdatereplicastate", "sp_msupdatesysmergearticles", "sp_msuplineageversion", "sp_msuploadsupportabilitydata", "sp_msuselightweightreplication",
			"sp_msvalidate_dest_recgen", "sp_msvalidate_subscription", "sp_msvalidate_wellpartitioned_articles", "sp_msvalidatearticle", "sp_mswritemergeperfcounter", "sp_msx_defect", "sp_msx_enlist", "sp_msx_get_account", "sp_msx_set_account", "sp_multi_server_job_summary",
			"sp_notify_operator", "sp_oacreate", "sp_oadestroy", "sp_oageterrorinfo", "sp_oagetproperty", "sp_oamethod", "sp_oasetproperty", "sp_oastop", "sp_objectfilegroup", "sp_oledb_database",
			"sp_oledb_defdb", "sp_oledb_deflang", "sp_oledb_language", "sp_oledb_ro_usrname", "sp_oledbinfo", "sp_orbitmap", "sp_password", "sp_peerconflictdetection_tableaug", "sp_pkeys", "sp_post_msx_operation",
			"sp_postagentinfo", "sp_posttracertoken", "sp_prepare", "sp_prepexec", "sp_prepexecrpc", "sp_primary_keys_rowset", "sp_primary_keys_rowset_rmt", "sp_primary_keys_rowset2", "sp_primarykeys", "sp_procedure_params_100_managed",
			"sp_procedure_params_100_rowset", "sp_procedure_params_100_rowset2", "sp_procedure_params_90_rowset", "sp_procedure_params_90_rowset2", "sp_procedure_params_managed", "sp_procedure_params_rowset", "sp_procedure_params_rowset2", "sp_procedures_rowset", "sp_procedures_rowset2", "sp_process_dialogtimer",
			"sp_processlogshippingmonitorhistory", "sp_processlogshippingmonitorprimary", "sp_processlogshippingmonitorsecondary", "sp_processlogshippingretentioncleanup", "sp_processmail", "sp_processresponse", "sp_procoption", "sp_prop_oledb_provider", "sp_provider_types_100_rowset", "sp_provider_types_90_rowset",
			"sp_provider_types_rowset", "sp_publication_validation", "sp_publicationsummary", "sp_publishdb", "sp_publisherproperty", "sp_purge_jobhistory", "sp_readerrorlog", "sp_readrequest", "sp_readwebtask", "sp_reassign_dtspackagecategory",
			"sp_reassign_dtspackageowner", "sp_recompile", "sp_refresh_heterogeneous_publisher", "sp_refresh_log_shipping_monitor", "sp_refreshsqlmodule", "sp_refreshsubscriptions", "sp_refreshview", "sp_register_custom_scripting", "sp_registercustomresolver", "sp_reinitmergepullsubscription",
			"sp_reinitmergesubscription", "sp_reinitpullsubscription", "sp_reinitsubscription", "sp_releaseapplock", "sp_releaseschemalock", "sp_remoteoption", "sp_remove_job_from_targets", "sp_remove_log_shipping_monitor_account", "sp_removedbreplication", "sp_removedistpublisherdbreplication",
			"sp_removesrvreplication", "sp_rename", "sp_renamedb", "sp_repladdcolumn", "sp_replcleanupccsprocs", "sp_replcmds", "sp_replcounters", "sp_replddlparser", "sp_repldeletequeuedtran", "sp_repldone",
			"sp_repldropcolumn", "sp_replflush", "sp_replgetparsedddlcmd", "sp_replhelp", "sp_replica", "sp_replication_agent_checkup", "sp_replicationdboption", "sp_replincrementlsn", "sp_replmonitorchangepublicationthreshold", "sp_replmonitorhelpmergesession",
			"sp_replmonitorhelpmergesessiondetail", "sp_replmonitorhelpmergesubscriptionmoreinfo", "sp_replmonitorhelppublication", "sp_replmonitorhelppublicationthresholds", "sp_replmonitorhelppublisher", "sp_replmonitorhelpsubscription", "sp_replmonitorrefreshjob", "sp_replmonitorsubscriptionpendingcmds", "sp_replpostsyncstatus", "sp_replqueuemonitor",
			"sp_replrestart", "sp_replrethrow", "sp_replsendtoqueue", "sp_replsetoriginator", "sp_replsetsyncstatus", "sp_replshowcmds", "sp_replsqlqgetrows", "sp_replsync", "sp_repltrans", "sp_replwritetovarbin",
			"sp_requestpeerresponse", "sp_requestpeertopologyinfo", "sp_reserve_http_namespace", "sp_reset_connection", "sp_reset_session_context", "sp_resetsnapshotdeliveryprogress", "sp_resetstatus", "sp_resign_database", "sp_resolve_logins", "sp_restoredbreplication",
			"sp_restoremergeidentityrange", "sp_resync_targetserver", "sp_resyncexecute", "sp_resyncexecutesql", "sp_resyncmergesubscription", "sp_resyncprepare", "sp_resyncuniquetable", "sp_revoke_login_from_proxy", "sp_revoke_proxy_from_subsystem", "sp_revoke_publication_access",
			"sp_revokedbaccess", "sp_revokelogin", "sp_runmailquery", "sp_runwebtask", "sp_schemafilter", "sp_schemata_rowset", "sp_script_reconciliation_delproc", "sp_script_reconciliation_insproc", "sp_script_reconciliation_sinsproc", "sp_script_reconciliation_vdelproc",
			"sp_script_reconciliation_xdelproc", "sp_script_synctran_commands", "sp_scriptdelproc", "sp_scriptdynamicupdproc", "sp_scriptinsproc", "sp_scriptmappedupdproc", "sp_scriptpublicationcustomprocs", "sp_scriptsinsproc", "sp_scriptsubconflicttable", "sp_scriptsupdproc",
			"sp_scriptupdproc", "sp_scriptvdelproc", "sp_scriptvupdproc", "sp_scriptxdelproc", "sp_scriptxupdproc", "sp_sem_add_message", "sp_sem_drop_message", "sp_send_dbmail", "sp_sendmailmessage", "sp_sendmailqueues",
			"sp_server_info", "sp_serveroption", "sp_set_local_time", "sp_set_session_context", "sp_set_sqlagent_properties", "sp_setapprole", "sp_setautosapasswordanddisable", "sp_setdefaultdatatypemapping", "sp_setnetname", "sp_setobdcertificate",
			"sp_setoraclepackageversion", "sp_setreplfailovermode", "sp_setsubscriptionxactseqno", "sp_settriggerorder", "sp_setuserbylogin", "sp_showcolv", "sp_showlineage", "sp_showpendingchanges", "sp_showrowreplicainfo", "sp_spaceused",
			"sp_special_columns", "sp_special_columns_100", "sp_special_columns_90", "sp_sproc_columns", "sp_sproc_columns_100", "sp_sproc_columns_90", "sp_sqlagent_check_msx_version", "sp_sqlagent_get_perf_counters", "sp_sqlagent_get_startup_info", "sp_sqlagent_has_server_access",
			"sp_sqlagent_is_member", "sp_sqlagent_is_srvrolemember", "sp_sqlagent_log_jobhistory", "sp_sqlagent_notify", "sp_sqlagent_probe_msx", "sp_sqlagent_refresh_job", "sp_sqlexec", "sp_srvrolepermission", "sp_ssis_addfolder", "sp_ssis_addlogentry",
			"sp_ssis_checkexists", "sp_ssis_deletefolder", "sp_ssis_deletepackage", "sp_ssis_getfolder", "sp_ssis_getpackage", "sp_ssis_getpackageroles", "sp_ssis_listfolders", "sp_ssis_listpackages", "sp_ssis_putpackage", "sp_ssis_renamefolder",
			"sp_ssis_setpackageroles", "sp_start_job", "sp_start_streaming_job", "sp_start_user_instance", "sp_startmergepullsubscription_agent", "sp_startmergepushsubscription_agent", "sp_startpublication_snapshot", "sp_startpullsubscription_agent", "sp_startpushsubscription_agent", "sp_statistics",
			"sp_statistics_100", "sp_statistics_rowset", "sp_statistics_rowset2", "sp_stop_job", "sp_stop_streaming_job", "sp_stopmergepullsubscription_agent", "sp_stopmergepushsubscription_agent", "sp_stoppublication_snapshot", "sp_stoppullsubscription_agent", "sp_stoppushsubscription_agent",
			"sp_stored_procedures", "sp_subscribe", "sp_subscription_cleanup", "sp_subscriptionsummary", "sp_syscollector_create_collection_item", "sp_syscollector_create_collection_set", "sp_syscollector_create_collector_type", "sp_syscollector_create_jobs", "sp_syscollector_create_tsql_query_collector", "sp_syscollector_delete_collection_item",
			"sp_syscollector_delete_collection_set", "sp_syscollector_delete_collector_type", "sp_syscollector_delete_execution_log_tree", "sp_syscollector_delete_jobs", "sp_syscollector_disable_collector", "sp_syscollector_enable_collector", "sp_syscollector_event_oncollectionbegin", "sp_syscollector_event_oncollectionend", "sp_syscollector_event_oncollectionstart", "sp_syscollector_event_oncollectionstop",
			"sp_syscollector_event_onerror", "sp_syscollector_event_onpackagebegin", "sp_syscollector_event_onpackageend", "sp_syscollector_event_onpackageupdate", "sp_syscollector_event_onstatsupdate", "sp_syscollector_get_collection_set_execution_status", "sp_syscollector_get_trace_info", "sp_syscollector_get_tsql_query_collector_package_ids", "sp_syscollector_get_warehouse_connection_string", "sp_syscollector_run_collection_set",
			"sp_syscollector_set_cache_directory", "sp_syscollector_set_cache_window", "sp_syscollector_set_warehouse_database_name", "sp_syscollector_set_warehouse_instance_name", "sp_syscollector_sql_text_lookup", "sp_syscollector_start_collection_set", "sp_syscollector_start_collection_set_jobs", "sp_syscollector_stop_collection_set", "sp_syscollector_stop_collection_set_jobs", "sp_syscollector_text_query_plan_lookpup",
			"sp_syscollector_update_collection_item", "sp_syscollector_update_collection_set", "sp_syscollector_update_job_proxy", "sp_syscollector_upload_collection_set", "sp_syscollector_validate_xml", "sp_syscollector_verify_collection_item", "sp_syscollector_verify_collection_set", "sp_syscollector_verify_collector_state", "sp_syscollector_verify_collector_type", "sp_syscollector_verify_event_log_id",
			"sp_sysmail_activate", "sp_sysmanagement_add_shared_registered_server", "sp_sysmanagement_add_shared_server_group", "sp_sysmanagement_delete_shared_registered_server", "sp_sysmanagement_delete_shared_server_group", "sp_sysmanagement_move_shared_registered_server", "sp_sysmanagement_move_shared_server_group", "sp_sysmanagement_rename_shared_registered_server", "sp_sysmanagement_rename_shared_server_group", "sp_sysmanagement_update_shared_registered_server",
			"sp_sysmanagement_update_shared_server_group", "sp_sysmanagement_verify_shared_server_type", "sp_syspolicy_add_condition", "sp_syspolicy_add_policy", "sp_syspolicy_add_policy_category", "sp_syspolicy_add_policy_category_subscription", "sp_syspolicy_add_target_set", "sp_syspolicy_add_target_set_level", "sp_syspolicy_create_job", "sp_syspolicy_delete_condition",
			"sp_syspolicy_delete_policy", "sp_syspolicy_delete_policy_category", "sp_syspolicy_delete_policy_category_subscription", "sp_syspolicy_delete_policy_execution_history", "sp_syspolicy_delete_target_set", "sp_syspolicy_dispatch_event", "sp_syspolicy_events_reader", "sp_syspolicy_execute_policy", "sp_syspolicy_log_policy_execution_detail", "sp_syspolicy_log_policy_execution_end",
			"sp_syspolicy_log_policy_execution_start", "sp_syspolicy_rename_condition", "sp_syspolicy_rename_policy", "sp_syspolicy_rename_policy_category", "sp_syspolicy_subscribe_to_policy_category", "sp_syspolicy_unsubscribe_from_policy_category", "sp_syspolicy_update_condition", "sp_syspolicy_update_ddl_trigger", "sp_syspolicy_update_event_notification", "sp_syspolicy_update_policy",
			"sp_syspolicy_update_policy_category", "sp_syspolicy_update_policy_category_subscription", "sp_syspolicy_update_target_set_level", "sp_syspolicy_verify_condition_identifiers", "sp_syspolicy_verify_policy_category_identifiers", "sp_syspolicy_verify_policy_identifiers", "sp_table_constraints_rowset", "sp_table_constraints_rowset2", "sp_table_privileges", "sp_table_privileges_ex",
			"sp_table_privileges_rowset", "sp_table_privileges_rowset_rmt", "sp_table_privileges_rowset2", "sp_table_statistics_rowset", "sp_table_statistics2_rowset", "sp_table_type_columns_100", "sp_table_type_columns_100_rowset", "sp_table_type_pkeys", "sp_table_type_primary_keys_rowset", "sp_table_types",
			"sp_table_types_rowset", "sp_table_validation", "sp_tablecollations", "sp_tablecollations_100", "sp_tablecollations_90", "sp_tableoption", "sp_tables", "sp_tables_ex", "sp_tables_info_90_rowset", "sp_tables_info_90_rowset_64",
			"sp_tables_info_90_rowset2", "sp_tables_info_90_rowset2_64", "sp_tables_info_rowset", "sp_tables_info_rowset_64", "sp_tables_info_rowset2", "sp_tables_info_rowset2_64", "sp_tables_rowset", "sp_tables_rowset_rmt", "sp_tables_rowset2", "sp_tableswc",
			"sp_target_server_summary", "sp_testlinkedserver", "sp_trace_create", "sp_trace_generateevent", "sp_trace_getdata", "sp_trace_setevent", "sp_trace_setfilter", "sp_trace_setstatus", "sp_try_set_session_context", "sp_unbindefault",
			"sp_unbindrule", "sp_uniquetaskname", "sp_unprepare", "sp_unregister_custom_scripting", "sp_unregistercustomresolver", "sp_unsetapprole", "sp_unsubscribe", "sp_update_agent_profile", "sp_update_alert", "sp_update_category",
			"sp_update_job", "sp_update_jobschedule", "sp_update_jobstep", "sp_update_log_shipping_monitor_info", "sp_update_notification", "sp_update_operator", "sp_update_proxy", "sp_update_replication_job_parameter", "sp_update_schedule", "sp_update_targetservergroup",
			"sp_update_user_instance", "sp_updateextendedproperty", "sp_updatestats", "sp_upgrade_log_shipping", "sp_user_counter1", "sp_user_counter10", "sp_user_counter2", "sp_user_counter3", "sp_user_counter4", "sp_user_counter5",
			"sp_user_counter6", "sp_user_counter7", "sp_user_counter8", "sp_user_counter9", "sp_usertypes_rowset", "sp_usertypes_rowset_rmt", "sp_usertypes_rowset2", "sp_validatecache", "sp_validatelogins", "sp_validatemergepublication",
			"sp_validatemergepullsubscription", "sp_validatemergesubscription", "sp_validlang", "sp_validname", "sp_verify_alert", "sp_verify_category", "sp_verify_category_identifiers", "sp_verify_credential_identifiers", "sp_verify_database_ledger", "sp_verify_database_ledger_from_digest_storage",
			"sp_verify_job", "sp_verify_job_date", "sp_verify_job_identifiers", "sp_verify_job_time", "sp_verify_jobproc_caller", "sp_verify_jobstep", "sp_verify_login_identifiers", "sp_verify_notification", "sp_verify_operator", "sp_verify_operator_identifiers",
			"sp_verify_performance_condition", "sp_verify_proxy", "sp_verify_proxy_identifiers", "sp_verify_proxy_permissions", "sp_verify_schedule", "sp_verify_schedule_identifiers", "sp_verify_subsystem", "sp_verify_subsystem_identifiers", "sp_verify_subsystems", "sp_verifypublisher",
			"sp_views_rowset", "sp_views_rowset2", "sp_vupgrade_mergeobjects", "sp_vupgrade_mergetables", "sp_vupgrade_replication", "sp_vupgrade_replsecurity_metadata", "sp_who", "sp_who2", "sp_write_sysjobstep_log", "sp_xml_preparedocument",
			"sp_xml_removedocument", "sp_xml_schema_rowset", "sp_xml_schema_rowset2", "sp_xp_cmdshell_proxy_account", "sysmail_add_account_sp", "sysmail_add_principalprofile_sp", "sysmail_add_profile_sp", "sysmail_add_profileaccount_sp", "sysmail_alter_user_credential_sp", "sysmail_configure_sp",
			"sysmail_create_user_credential_sp", "sysmail_delete_account_sp", "sysmail_delete_log_sp", "sysmail_delete_mailitems_sp", "sysmail_delete_principalprofile_sp", "sysmail_delete_profile_sp", "sysmail_delete_profileaccount_sp", "sysmail_drop_user_credential_sp", "sysmail_help_account_sp", "sysmail_help_admin_account_sp",
			"sysmail_help_configure_sp", "sysmail_help_configure_value_sp", "sysmail_help_principalprofile_sp", "sysmail_help_profile_sp", "sysmail_help_profileaccount_sp", "sysmail_help_queue_sp", "sysmail_help_status_sp", "sysmail_logmailevent_sp", "sysmail_start_sp", "sysmail_stop_sp",
			"sysmail_update_account_sp", "sysmail_update_principalprofile_sp", "sysmail_update_profile_sp", "sysmail_update_profileaccount_sp", "sysmail_verify_account_sp", "sysmail_verify_accountparams_sp", "sysmail_verify_principal_sp", "sysmail_verify_profile_sp", "xp_adsirequest", "xp_availablemedia",
			"xp_cleanupwebtask", "xp_cmdshell", "xp_convertwebtask", "xp_create_subdir", "xp_delete_file", "xp_deletemail", "xp_dirtree", "xp_dropwebtask", "xp_enum_oledb_providers", "xp_enumcodepages",
			"xp_enumerrorlogs", "xp_enumgroups", "xp_fileexist", "xp_findnextmsg", "xp_fixeddrives", "xp_get_mapi_default_profile", "xp_get_mapi_profiles", "xp_get_script", "xp_get_tape_devices", "xp_getnetname",
			"xp_grantlogin", "xp_instance_regaddmultistring", "xp_instance_regdeletekey", "xp_instance_regdeletevalue", "xp_instance_regenumkeys", "xp_instance_regenumvalues", "xp_instance_regread", "xp_instance_regremovemultistring", "xp_instance_regwrite", "xp_logevent",
			"xp_loginconfig", "xp_logininfo", "xp_makewebtask", "xp_msadenabled", "xp_msadsiobjreg", "xp_msadsiobjregdb", "xp_msadsireg", "xp_msver", "xp_msx_enlist", "xp_passagentinfo",
			"xp_prop_oledb_provider", "xp_qv", "xp_readerrorlog", "xp_readmail", "xp_readwebtask", "xp_regaddmultistring", "xp_regdeletekey", "xp_regdeletevalue", "xp_regenumkeys", "xp_regenumvalues",
			"xp_regread", "xp_regremovemultistring", "xp_regwrite", "xp_repl_convert_encrypt_sysadmin_wrapper", "xp_replposteor", "xp_revokelogin", "xp_runwebtask", "xp_sendmail", "xp_servicecontrol", "xp_sprintf",
			"xp_sqlagent_enum_jobs", "xp_sqlagent_is_starting", "xp_sqlagent_monitor", "xp_sqlagent_notify", "xp_sqlagent_param", "xp_sqlmaint", "xp_sscanf", "xp_startmail", "xp_stopmail", "xp_subdirs",
			"xp_sysmail_activate", "xp_sysmail_attachment_load", "xp_sysmail_format_query", "xp_test_mapi_profile"
		};
		SystemViews = new string[648]
		{
			"all_columns", "all_objects", "all_parameters", "all_sql_modules", "all_views", "allocation_units", "assemblies", "assembly_files", "assembly_modules", "assembly_references",
			"assembly_types", "asymmetric_keys", "availability_databases_cluster", "availability_group_listener_ip_addresses", "availability_group_listeners", "availability_groups", "availability_groups_cluster", "availability_read_only_routing_lists", "availability_replicas", "backup_devices",
			"certificates", "change_tracking_databases", "change_tracking_tables", "check_constraints", "column_domain_usage", "column_encryption_key_values", "column_encryption_keys", "column_master_keys", "column_privileges", "column_store_dictionaries",
			"column_store_row_groups", "column_store_segments", "column_type_usages", "column_xml_schema_collection_usages", "columns", "computed_columns", "configurations", "constraint_column_usage", "constraint_table_usage", "conversation_endpoints",
			"conversation_groups", "conversation_priorities", "credentials", "crypt_properties", "cryptographic_providers", "data_spaces", "database_audit_specification_details", "database_audit_specifications", "database_automatic_tuning_mode", "database_automatic_tuning_options",
			"database_credentials", "database_files", "database_filestream_options", "database_fragments", "database_ledger_blocks", "database_ledger_digest_locations", "database_ledger_transactions", "database_mirroring", "database_mirroring_endpoints", "database_mirroring_witnesses",
			"database_permissions", "database_principal_aliases", "database_principals", "database_query_store_options", "database_recovery_status", "database_role_members", "database_scoped_configurations", "database_scoped_credentials", "databases", "default_constraints",
			"destination_data_spaces", "dm_audit_actions", "dm_audit_class_type_map", "dm_broker_activated_tasks", "dm_broker_connections", "dm_broker_forwarded_messages", "dm_broker_queue_monitors", "dm_cache_hit_stats", "dm_cache_size", "dm_cache_stats",
			"dm_cdc_errors", "dm_cdc_log_scan_sessions", "dm_clr_appdomains", "dm_clr_loaded_assemblies", "dm_clr_properties", "dm_clr_tasks", "dm_column_encryption_enclave", "dm_column_encryption_enclave_operation_stats", "dm_column_store_object_pool", "dm_cryptographic_provider_algorithms",
			"dm_cryptographic_provider_keys", "dm_cryptographic_provider_properties", "dm_cryptographic_provider_sessions", "dm_database_encryption_keys", "dm_db_column_store_row_group_operational_stats", "dm_db_column_store_row_group_physical_stats", "dm_db_database_page_allocations", "dm_db_external_language_stats", "dm_db_file_space_usage", "dm_db_fts_index_physical_stats",
			"dm_db_incremental_stats_properties", "dm_db_index_operational_stats", "dm_db_index_physical_stats", "dm_db_index_usage_stats", "dm_db_log_info", "dm_db_log_space_usage", "dm_db_log_stats", "dm_db_mirroring_auto_page_repair", "dm_db_mirroring_connections", "dm_db_mirroring_past_actions",
			"dm_db_missing_index_columns", "dm_db_missing_index_details", "dm_db_missing_index_group_stats", "dm_db_missing_index_group_stats_query", "dm_db_missing_index_groups", "dm_db_objects_disabled_on_compatibility_level_change", "dm_db_page_info", "dm_db_partition_stats", "dm_db_persisted_sku_features", "dm_db_rda_migration_status",
			"dm_db_rda_schema_update_status", "dm_db_script_level", "dm_db_session_space_usage", "dm_db_stats_histogram", "dm_db_stats_properties", "dm_db_stats_properties_internal", "dm_db_task_space_usage", "dm_db_tuning_recommendations", "dm_db_uncontained_entities", "dm_db_xtp_checkpoint_files",
			"dm_db_xtp_checkpoint_internals", "dm_db_xtp_checkpoint_stats", "dm_db_xtp_gc_cycle_stats", "dm_db_xtp_hash_index_stats", "dm_db_xtp_index_stats", "dm_db_xtp_memory_consumers", "dm_db_xtp_nonclustered_index_stats", "dm_db_xtp_object_stats", "dm_db_xtp_table_memory_stats", "dm_db_xtp_transactions",
			"dm_distributed_exchange_stats", "dm_enumerate_blobdirectory", "dm_exec_background_job_queue", "dm_exec_background_job_queue_stats", "dm_exec_cached_plan_dependent_objects", "dm_exec_cached_plans", "dm_exec_compute_node_errors", "dm_exec_compute_node_status", "dm_exec_compute_nodes", "dm_exec_connections",
			"dm_exec_cursors", "dm_exec_describe_first_result_set", "dm_exec_describe_first_result_set_for_object", "dm_exec_distributed_request_steps", "dm_exec_distributed_requests", "dm_exec_distributed_sql_requests", "dm_exec_dms_services", "dm_exec_dms_workers", "dm_exec_external_operations", "dm_exec_external_work",
			"dm_exec_function_stats", "dm_exec_input_buffer", "dm_exec_plan_attributes", "dm_exec_procedure_stats", "dm_exec_query_memory_grants", "dm_exec_query_optimizer_info", "dm_exec_query_optimizer_memory_gateways", "dm_exec_query_parallel_workers", "dm_exec_query_plan", "dm_exec_query_plan_stats",
			"dm_exec_query_profiles", "dm_exec_query_resource_semaphores", "dm_exec_query_statistics_xml", "dm_exec_query_stats", "dm_exec_query_transformation_stats", "dm_exec_requests", "dm_exec_session_wait_stats", "dm_exec_sessions", "dm_exec_sql_text", "dm_exec_text_query_plan",
			"dm_exec_trigger_stats", "dm_exec_valid_use_hints", "dm_exec_xml_handles", "dm_filestream_oob_handles", "dm_filestream_oob_requests", "dm_external_script_execution_stats", "dm_external_script_requests", "dm_external_script_resource_usage_stats", "dm_filestream_file_io_handles", "dm_filestream_file_io_requests",
			"dm_filestream_non_transacted_handles", "dm_fts_active_catalogs", "dm_fts_fdhosts", "dm_fts_index_keywords", "dm_fts_index_keywords_by_document", "dm_fts_index_keywords_by_property", "dm_fts_index_keywords_position_by_document", "dm_fts_index_population", "dm_fts_memory_buffers", "dm_fts_memory_pools",
			"dm_fts_outstanding_batches", "dm_fts_parser", "dm_fts_population_ranges", "dm_fts_semantic_similarity_population", "dm_hadr_ag_threads", "dm_hadr_auto_page_repair", "dm_hadr_automatic_seeding", "dm_hadr_availability_group_states", "dm_hadr_availability_replica_cluster_nodes", "dm_hadr_availability_replica_cluster_states",
			"dm_hadr_availability_replica_states", "dm_hadr_cluster", "dm_hadr_cluster_members", "dm_hadr_cluster_networks", "dm_hadr_database_replica_cluster_states", "dm_hadr_database_replica_states", "dm_hadr_db_threads", "dm_hadr_instance_node_map", "dm_hadr_name_id_map", "dm_hadr_physical_seeding_stats",
			"dm_hpc_device_stats", "dm_hpc_thread_proxy_stats", "dm_io_backup_tapes", "dm_io_cluster_shared_drives", "dm_io_cluster_valid_path_names", "dm_io_pending_io_requests", "dm_io_virtual_file_stats", "dm_logconsumer_cachebufferrefs", "dm_logconsumer_privatecachebuffers", "dm_logpool_consumers",
			"dm_logpool_hashentries", "dm_logpool_sharedcachebuffers", "dm_logpool_stats", "dm_logpoolmgr_freepools", "dm_logpoolmgr_respoolsize", "dm_logpoolmgr_stats", "dm_os_buffer_descriptors", "dm_os_buffer_pool_extension_configuration", "dm_os_child_instances", "dm_os_cluster_nodes",
			"dm_os_cluster_properties", "dm_os_dispatcher_pools", "dm_os_dispatchers", "dm_os_enumerate_filesystem", "dm_os_enumerate_fixed_drives", "dm_os_file_exists", "dm_os_host_info", "dm_os_hosts", "dm_os_job_object", "dm_os_latch_stats",
			"dm_os_loaded_modules", "dm_os_memory_allocations", "dm_os_memory_broker_clerks", "dm_os_memory_brokers", "dm_os_memory_cache_clock_hands", "dm_os_memory_cache_counters", "dm_os_memory_cache_entries", "dm_os_memory_cache_hash_tables", "dm_os_memory_clerks", "dm_os_memory_nodes",
			"dm_os_memory_node_access_stats", "dm_os_memory_objects", "dm_os_memory_pools", "dm_os_nodes", "dm_os_performance_counters", "dm_os_process_memory", "dm_os_ring_buffers", "dm_os_schedulers", "dm_os_server_diagnostics_log_configurations", "dm_os_spinlock_stats",
			"dm_os_stacks", "dm_os_sublatches", "dm_os_sys_info", "dm_os_sys_memory", "dm_os_tasks", "dm_os_threads", "dm_os_virtual_address_dump", "dm_os_volume_stats", "dm_os_wait_stats", "dm_os_waiting_tasks",
			"dm_os_windows_info", "dm_os_worker_local_storage", "dm_os_workers", "dm_qn_subscriptions", "dm_repl_articles", "dm_repl_schemas", "dm_repl_tranhash", "dm_repl_traninfo", "dm_resource_governor_configuration", "dm_resource_governor_external_resource_pool_affinity",
			"dm_resource_governor_external_resource_pools", "dm_resource_governor_resource_pool_affinity", "dm_resource_governor_resource_pool_volumes", "dm_resource_governor_resource_pools", "dm_resource_governor_workload_groups", "dm_server_audit_status", "dm_server_memory_dumps", "dm_server_registry", "dm_server_services", "dm_sql_referenced_entities",
			"dm_sql_referencing_entities", "dm_tcp_listener_states", "dm_tran_aborted_transactions", "dm_tran_active_snapshot_database_transactions", "dm_tran_active_transactions", "dm_tran_commit_table", "dm_tran_current_snapshot", "dm_tran_current_transaction", "dm_tran_database_transactions", "dm_tran_global_recovery_transactions",
			"dm_tran_global_transactions", "dm_tran_global_transactions_enlistments", "dm_tran_global_transactions_log", "dm_tran_locks", "dm_tran_persistent_version_store_stats", "dm_tran_session_transactions", "dm_tran_top_version_generators", "dm_tran_transactions_snapshot", "dm_tran_version_store", "dm_tran_version_store_space_usage",
			"dm_xe_map_values", "dm_xe_object_columns", "dm_xe_objects", "dm_xe_packages", "dm_xe_session_event_actions", "dm_xe_session_events", "dm_xe_session_object_columns", "dm_xe_session_targets", "dm_xe_sessions", "dm_xtp_gc_queue_stats",
			"dm_xtp_gc_stats", "dm_xtp_system_memory_consumers", "dm_xtp_threads", "dm_xtp_transaction_recent_rows", "dm_xtp_transaction_stats", "domain_constraints", "domains", "edge_constraint_clauses", "edge_constraints", "endpoint_webmethods",
			"endpoints", "event_notification_event_types", "event_notifications", "events", "external_job_streams", "external_streaming_jobs", "external_streams", "extended_procedures", "extended_properties", "filegroup_fragments",
			"external_data_sources", "external_file_formats", "external_languages", "external_language_files", "external_libraries", "external_libraries_installed", "external_library_files", "external_library_setup_errors", "external_tables", "filegroups",
			"filetable_system_defined_objects", "filetables", "fn_builtin_permissions", "fn_check_object_signatures", "fn_column_store_row_groups", "fn_db_backup_file_snapshots", "fn_dblog", "fn_dblog_xtp", "fn_dbslog", "fn_dump_dblog",
			"fn_enumcurrentprincipals", "fn_dump_dblog_xtp", "fn_full_dblog", "fn_get_audit_file", "fn_get_sql", "fn_hadr_distributed_ag_database_replica", "fn_hadr_distributed_ag_replica", "fn_helpcollations", "fn_helpdatatypemap", "fn_listextendedproperty",
			"fn_msxe_read_event_stream", "fn_my_permissions", "fn_replgetcolidfrombitmap", "fn_rowdumpcracker", "fn_servershareddrives", "fn_stmt_sql_handle_from_sql_stmt", "fn_syscollector_get_execution_details", "fn_syscollector_get_execution_log_tree", "fn_syscollector_get_execution_stats", "fn_trace_geteventinfo",
			"fn_trace_getfilterinfo", "fn_trace_getinfo", "fn_trace_gettable", "fn_translate_permissions", "fn_validate_plan_guide", "fn_virtualfilestats", "fn_virtualservernodes", "fn_xe_file_target_read_file", "foreign_key_columns", "foreign_keys",
			"fulltext_catalogs", "fulltext_document_types", "fulltext_index_catalog_usages", "fulltext_index_columns", "fulltext_index_fragments", "fulltext_indexes", "fulltext_languages", "fulltext_semantic_language_statistics_database", "fulltext_semantic_languages", "fulltext_stoplists",
			"fulltext_stopwords", "fulltext_system_stopwords", "function_order_columns", "hash_indexes", "http_endpoints", "identity_columns", "index_columns", "index_resumable_operations", "indexes", "information_schema",
			"internal_partitions", "internal_tables", "key_column_usage", "key_constraints", "key_encryptions", "ledger_column_history", "ledger_table_history", "linked_logins", "login_token", "masked_columns",
			"master_database_fragments", "master_files", "master_key_passwords", "memory_optimized_tables_internal_attributes", "message_type_xml_schema_collection_usages", "messages", "module_assembly_usages", "msdatatype_mappings", "numbered_procedure_parameters", "numbered_procedures",
			"objects", "openkeys", "parameter_type_usages", "parameter_xml_schema_collection_usages", "parameters", "partition_fragments", "partition_functions", "partition_parameters", "partition_range_values", "partition_schemes",
			"partitions", "periods", "physical_filegroups", "plan_guides", "procedures", "recovery_units", "query_context_settings", "query_store_plan", "query_store_plan_feedback", "query_store_query",
			"query_store_query_text", "query_store_query_variant", "query_store_runtime_stats", "query_store_runtime_stats_interval", "query_store_wait_stats", "referential_constraints", "registered_search_properties", "registered_search_property_lists", "remote_data_archive_databases", "remote_data_archive_tables",
			"remote_logins", "remote_service_bindings", "resource_governor_configuration", "resource_governor_external_resource_pool_affinity", "resource_governor_external_resource_pools", "resource_governor_resource_pool_affinity", "resource_governor_resource_pools", "resource_governor_workload_groups", "routes", "routine_columns",
			"routines", "schemas", "schemata", "securable_classes", "security_policies", "security_predicates", "selective_xml_index_namespaces", "selective_xml_index_paths", "sensitivity_classifications", "sequences",
			"server_assembly_modules", "server_audit_specification_details", "server_audit_specifications", "server_audits", "server_event_notifications", "server_event_session_actions", "server_event_session_events", "server_event_session_fields", "server_event_session_targets", "server_event_sessions",
			"server_events", "server_file_audits", "server_permissions", "server_principal_credentials", "server_principals", "server_role_members", "server_sql_modules", "server_trigger_events", "server_triggers", "servers",
			"service_broker_endpoints", "service_contract_message_usages", "service_contract_usages", "service_contracts", "service_message_types", "service_queue_usages", "service_queues", "services", "soap_endpoints", "spatial_index_tessellations",
			"spatial_indexes", "spatial_reference_systems", "sql_dependencies", "sql_expression_dependencies", "sql_feature_restrictions", "sql_logins", "sql_modules", "stats", "stats_columns", "symmetric_keys",
			"synonyms", "sys", "sysaltfiles", "syscacheobjects", "syscharsets", "syscollector_collection_items", "syscollector_collection_sets", "syscollector_collector_types", "syscollector_config_store", "syscollector_execution_log",
			"syscollector_execution_log_full", "syscollector_execution_stats", "syscolumns", "syscomments", "sysconfigures", "sysconstraints", "syscscontainers", "syscurconfigs", "syscursorcolumns", "syscursorrefs",
			"syscursors", "syscursortables", "sysdatabases", "sysdatatypemappings", "sysdepends", "sysdevices", "sysfilegroups", "sysfiles", "sysforeignkeys", "sysfulltextcatalogs",
			"sysindexes", "sysindexkeys", "sysjobs_view", "syslanguages", "syslockinfo", "syslogins", "sysmail_allitems", "sysmail_event_log", "sysmail_faileditems", "sysmail_mailattachments",
			"sysmail_sentitems", "sysmail_unsentitems", "sysmaintplan_plans", "sysmanagement_shared_registered_servers", "sysmanagement_shared_server_groups", "sysmembers", "sysmessages", "sysobjects", "sysoledbusers", "sysopentapes",
			"sysoriginatingservers_view", "sysperfinfo", "syspermissions", "syspolicy_conditions", "syspolicy_fn_get_bad_filters", "syspolicy_policies", "syspolicy_policy_categories", "syspolicy_policy_category_subscriptions", "syspolicy_policy_execution_history", "syspolicy_policy_execution_history_details",
			"syspolicy_system_health_state", "syspolicy_target_set_levels", "syspolicy_target_sets", "sysprocesses", "sysprotects", "sysproxyloginsubsystem_view", "sysreferences", "sysremotelogins", "sysschedules_localserver_view", "sysservers",
			"systargetservers_view", "system_columns", "system_components_surface_area_configuration", "system_internals_allocation_units", "system_internals_partition_columns", "system_internals_partitions", "system_objects", "system_parameters", "system_sql_modules", "system_views",
			"systypes", "sysusers", "table_constraints", "table_privileges", "table_types", "tables", "tcp_endpoints", "time_zone_info", "trace_categories", "trace_columns",
			"trace_event_bindings", "trace_events", "trace_subclass_values", "traces", "transmission_queue", "trigger_event_types", "trigger_events", "triggers", "trusted_assemblies", "type_assembly_usages",
			"types", "user_token", "via_endpoints", "view_column_usage", "view_table_usage", "views", "xml_indexes", "xml_schema_attributes", "xml_schema_collections", "xml_schema_component_placements",
			"xml_schema_components", "xml_schema_elements", "xml_schema_facets", "xml_schema_model_groups", "xml_schema_namespaces", "xml_schema_types", "xml_schema_wildcard_namespaces", "xml_schema_wildcards"
		};
		Builtins = new string[473]
		{
			"@@connections", "@@cpu_busy", "@@cursor_rows", "@@datefirst", "@@dbts", "@@def_sortorder_id", "@@default_langid", "@@error", "@@fetch_status", "@@identity",
			"@@idle", "@@io_busy", "@@langid", "@@language", "@@lock_timeout", "@@max_connections", "@@max_precision", "@@microsoftversion", "@@nestlevel", "@@options",
			"@@pack_received", "@@pack_sent", "@@packet_errors", "@@procid", "@@remserver", "@@rowcount", "@@servername", "@@servicename", "@@spid", "@@textsize",
			"@@timeticks", "@@total_errors", "@@total_read", "@@total_write", "@@trancount", "@@version", "abs", "acos", "actual_partition_fragment_id", "app_name",
			"applock_mode", "applock_test", "ascii", "asin", "assemblyproperty", "asymkey_id", "asymkeyproperty", "atan", "atn2", "audit_action_property",
			"batchbitmapprobe", "bcpcollationname", "binary_checksum", "bit_length", "bitmapprobe", "bmktopage", "brick_id", "buildresparsevector", "cast", "ceiling",
			"cert_id", "certencoded", "certprivatekey", "certproperty", "change_tracking_cleanup_version", "change_tracking_current_version", "change_tracking_hardened_cleanup_version", "change_tracking_is_column_in_mask", "change_tracking_max_cleanup_version", "change_tracking_min_valid_version",
			"charindex", "checksum", "choose", "cloud_databasepropertyex", "clouddb_sql_variant_to_var_binary", "clouddb_var_binary_to_sql_variant", "col_length", "col_name", "collate", "collationname",
			"collationproperty", "collationpropertyfromid", "columnproperty", "columnpropertyex", "columns_updated", "colv_80_to_90", "colv_90_to_80", "comparecompressedscalars", "comparevardecimal", "compress",
			"compressnumeric", "compressscalar", "concat", "concat_ws", "connectionproperty", "contains", "containstablerank", "context_info", "convert", "convertfulltextkeyr4",
			"convertfulltextkeyr8", "convertresvtostring", "convertsesvtoresv", "convertsvtoxml", "convertxmltoresv", "cos", "cot", "crypt_gen_random", "csequal", "curdate",
			"current_request_id", "current_time", "current_timestamp", "current_transaction_id", "cursor_send_donecount", "cursor_status", "cursorqueryswitch", "curtime", "cwt_rowid", "database_principal_id",
			"databasepropertyex", "datalength", "date_bucket", "dateadd", "datediff", "datediff_big", "datefromparts", "datename", "datepart", "datetime2fromparts",
			"datetimefromparts", "datetimeoffsetfromparts", "day", "dayname", "dayofmonth", "dayofweek", "dayofyear", "db_id", "db_name", "dbccbinaryequal",
			"dbccbinaryequallegacylob", "dbprop", "decompress", "decompressnumeric", "decompressscalar", "decryptbyasymkey", "decryptbycert", "decryptbykey", "decryptbykeyautoasymkey", "decryptbykeyautocert",
			"decryptbypassphrase", "default_domain", "degrees", "difference", "encryptbyasymkey", "encryptbycert", "encryptbykey", "encryptbypassphrase", "entity_id", "entity_name",
			"entitypropertyex", "eomonth", "error_line", "error_message", "error_number", "error_procedure", "error_severity", "error_state", "eventdata", "exp",
			"extract", "fetch_range", "fetch_status", "file_id", "file_idex", "file_name", "filegroup_id", "filegroup_name", "filegroupproperty", "fileproperty",
			"filepropertybyid", "filestreamrename", "filetable_check_valid_attributes", "filetable_filename_matches", "filetable_lock_all_descendants", "filetable_parents_are_directories", "filetablerootpath", "floor", "forceorder", "format",
			"formatmessage", "freetexttablerank", "fulltext_display_term", "fulltext_normalize_term", "fulltextcatalogproperty", "fulltextnotify", "fulltextserviceproperty", "gen_norm_tables", "gendbnamefrompath", "get_cloud_partition_max_size",
			"get_filestream_transaction_context", "get_new_rowversion", "get_publickey_token_from_file", "get_sid", "get_transmission_status", "getancestor", "getancestorforspatialbinary", "getansinull", "getattributelimit", "getbinarysparsevector",
			"getchecksum", "getconditionalidentity", "getdate", "getdateodbc", "getdbxdesid", "getdefault", "getdescendantlimit", "getdescendantlimitforspatialbinary", "getfileextension", "getfilenamespacepath",
			"getgifromnid", "getidentity", "getimage", "getmaxversion", "getntext", "getnumericlsn", "getparent", "getpathlocator", "getreplpeerversion", "getsecurablename",
			"getsequencenext", "getsilentidentity", "getsparsecolumnvalue", "getsubsecurablename", "getsystemassemblybytes", "gettext", "gettimestamp", "geturifromnid", "getutcdate", "greatest",
			"has_access", "has_dbaccess", "has_perms_by_id", "has_perms_by_name", "hashbytes", "host_id", "host_name", "hour", "ident_current", "ident_incr",
			"ident_seed", "identityproperty", "iif", "index_col", "indexkey_property", "indexproperty", "integertoordpath", "intersectbitmaps", "is_callersigned", "is_member",
			"is_objectsigned", "is_rolemember", "is_srvrolemember", "isancestorof", "isautofetch", "isdate", "isfilenamevalid", "isjson", "isnull", "isnumeric",
			"ispaluser", "json_modify", "json_query", "json_value", "key_guid", "key_id", "key_name", "least", "len", "likerangeend",
			"likerangeinfo", "likerangestart", "lineage_80_to_90", "lineage_90_to_80", "linkedserverproperty", "lockconversationgroup", "log", "log10", "loginproperty", "lookup",
			"lower", "ltrim", "manufactureconstructordpath", "manufacturesubnodeordpath", "matrix_downcast_safe", "mergesparsevector", "min_active_rowversion", "minute", "mod", "month",
			"monthname", "newfilestreamvalue", "newfulltextdocid", "newid", "newsequentialid", "normalize", "normalize_denormalize", "nt_client", "object_definition", "object_id",
			"object_name", "object_schema_name", "objectproperty", "objectpropertyex", "objidupdate", "octet_length", "odbcprec", "odbcscale", "ordpathtostring", "original_db_name",
			"original_login", "parse", "parse_bigint", "parse_date", "parse_datetime", "parse_datetime2", "parse_datetimeoffset", "parse_float", "parse_int", "parse_money",
			"parse_numeric", "parse_real", "parse_smalldate", "parse_smallint", "parse_smallmoney", "parse_time", "parse_tinyint", "parsename", "partialdiffupdate", "partialupdate",
			"partition_fragment_id", "partition_id", "pathname", "patindex", "permission_name", "permissions", "physicalpathname", "pi", "platform", "power",
			"probe", "program_name", "publishingservername", "pwdcompare", "pwdencrypt", "qnfirenotification", "qngetcookie", "qngetid", "qngetssbinstance", "qngetssbservice",
			"qngettimeout", "qnopenssbdlg", "quarter", "quotename", "radians", "raiseerror", "raiseifnullinsert", "raiseifnullupdate", "rand", "random_db_fragment_id",
			"rangepartition", "rangepartitionnew", "remapbits", "replace", "replicate", "replnick_90_to_80", "replnickarray_80_to_90", "replnickarray_90_to_80", "reverse", "round",
			"rowcount_big", "rtrim", "schema_id", "schema_name", "scope_id", "scope_identity", "second", "serverproperty", "session_context", "sessionproperty",
			"setidentity", "shift", "sid_binary", "sign", "signbyasymkey", "signbycert", "sin", "smalldatetimefromparts", "soundex", "space",
			"sql_connection_mode", "sql_variant_property", "sqrt", "square", "statmaninit", "statmanmergeinit", "statmanmergestep", "statmanstep", "stats_date", "stats_ver_current",
			"str", "string_escape", "stringtocurs", "stringtoordpath", "stringtovarbinary", "stuff", "substring", "suser_id", "suser_name", "suser_sid",
			"suser_sname", "switchoffset", "symkeyproperty", "sysconv", "sysdatetime", "sysdatetimeoffset", "sysutcdatetime", "tan", "tertiary_weights", "textptr",
			"textvalid", "timefromparts", "timestampadd", "timestampdiff", "todatetimeoffset", "translate", "trigger_nestlevel", "trim", "try_cast", "try_parse",
			"tsequal", "type_id", "type_name", "typeproperty", "typepropertyex", "uncompress", "unicode", "update", "update_concurrency", "updatecolvbm",
			"updatelineage", "upper", "user_id", "user_name", "user_sid", "validateudtbytes", "verifysignedbyasymkey", "verifysignedbycert", "week", "x_iopbuildswitch",
			"xact_state", "xml_revalidate", "xml_schema_namespace", "xsd_cast", "xsd_cast_to_maybe_large", "xsd_cast_to_maybe_large_bin", "xsd_compare_as_strings", "xsd_node_instance_of", "xsd_treat_as", "xsdqnameconstruct",
			"xsdqnamesplit", "xtypetotds", "year"
		};
		definitions = new Dictionary<int, TokenDefinition>();
		DefineToken(40, TokenType.Delimiter, TokenTriggers.MatchBraces | TokenTriggers.ParameterStart);
		DefineToken(41, TokenType.Delimiter, TokenTriggers.MatchBraces);
		DefineToken(44, TokenType.Delimiter, TokenTriggers.None);
		DefineToken(46, TokenType.Delimiter, TokenTriggers.MemberSelect);
		foreach (Tokens value in Enum.GetValues(typeof(Tokens)))
		{
			switch (value)
			{
			case Tokens.LEX_ERROR:
				DefineToken(977, TokenType.Error, TokenTriggers.None);
				break;
			case Tokens.LEX_END_OF_LINE_COMMENT:
			case Tokens.LEX_MULTILINE_COMMENT:
				DefineToken((int)value, TokenType.Comment, TokenTriggers.None);
				break;
			case Tokens.TOKEN_ID:
			case Tokens.TOKEN_VARIABLE:
			case Tokens.TOKEN_GLOBAL_VAR:
			case Tokens.LEX_ACCUMULATE_ID:
				DefineToken((int)value, TokenType.Identifier, TokenTriggers.MemberSelect);
				break;
			case Tokens.TOKEN_STRING:
			case Tokens.LEX_ACCUMULATE_STRING:
				DefineToken((int)value, TokenType.SqlString, TokenTriggers.None);
				break;
			case Tokens.TOKEN_BINARY:
			case Tokens.TOKEN_INTEGER:
			case Tokens.TOKEN_REAL:
			case Tokens.TOKEN_MONEY:
			case Tokens.TOKEN_NUMERIC:
				DefineToken((int)value, TokenType.Number, TokenTriggers.None);
				break;
			case Tokens.TOKEN_DOUBLECOLON:
				DefineToken((int)value, TokenType.SqlOperator, TokenTriggers.None);
				break;
			default:
				DefineToken((int)value, TokenType.Keyword, TokenTriggers.None);
				break;
			}
		}
		colorOverrides = new Dictionary<string, TokenType>();
		PopulateColorOverrides(Keywords, TokenType.Keyword);
		PopulateColorOverrides(Operators, TokenType.SqlOperator);
		PopulateColorOverrides(SystemFunctions, TokenType.SqlSystemFunction);
		PopulateColorOverrides(SystemStoredProcedures, TokenType.SqlStoredProcedure);
		PopulateColorOverrides(SystemViews, TokenType.SqlSystemTable);
		PopulateColorOverrides(Builtins, TokenType.SqlSystemFunction);
		sqlCmdRegex = new Regex("(^\\s*" + string.Join(")|(^\\s*", SqlCMDRegExp) + ")", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
	}

	public bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
	{
		if (sqlCmdState == SqlCmdState.SqlCmd)
		{
			sqlCmdState = SqlCmdState.SqlCmdEOF;
			tokenInfo.StartIndex = 0;
			tokenInfo.EndIndex = source.Length - 1;
			tokenInfo.Type = TokenType.SqlCmdCommand;
			tokenInfo.Token = 990;
			return true;
		}
		if (sqlCmdState == SqlCmdState.SqlCmdEOF)
		{
			return false;
		}
		int start;
		int end;
		bool isPairMatch;
		bool isExecAutoParamHelp;
		int next = lex.GetNext(ref state, out start, out end, out isPairMatch, out isExecAutoParamHelp);
		if (next != 128)
		{
			tokenInfo.StartIndex = start;
			tokenInfo.EndIndex = end;
			tokenInfo.Token = next;
			PopulateDefiniton(tokenInfo);
			if (isPairMatch)
			{
				tokenInfo.Trigger |= TokenTriggers.MatchBraces;
			}
			if (isExecAutoParamHelp)
			{
				tokenInfo.Trigger |= TokenTriggers.ParameterStart;
			}
			return true;
		}
		return false;
	}

	public void SetSource(string source, int offset)
	{
		this.source = source;
		if (isSqlCmdModeEnabled && sqlCmdRegex.IsMatch(this.source))
		{
			sqlCmdState = SqlCmdState.SqlCmd;
			return;
		}
		sqlCmdState = SqlCmdState.NotSqlCmd;
		lex.SetSource(source, offset);
	}

	private static void PopulateColorOverrides(string[] values, TokenType overrideType)
	{
		foreach (string text in values)
		{
			colorOverrides.Add(text.ToUpperInvariant(), overrideType);
		}
	}

	public static void DefineToken(int token, TokenType type, TokenTriggers trigger)
	{
		definitions[token] = new TokenDefinition(type, trigger);
	}

	public void PopulateDefiniton(TokenInfo tokenInfo)
	{
		TokenDefinition value = (definitions.TryGetValue(tokenInfo.Token, out value) ? value : defaultDefinition);
		tokenInfo.Type = value.TokenType;
		tokenInfo.Trigger = value.TokenTriggers;
		if (tokenInfo.Type != TokenType.Comment)
		{
			string text = source.Substring(tokenInfo.StartIndex, tokenInfo.EndIndex - tokenInfo.StartIndex + 1);
			text = text.ToUpperInvariant();
			if (colorOverrides.TryGetValue(text, out var value2))
			{
				tokenInfo.Type = value2;
			}
			if (tokenInfo.Token == 845 || tokenInfo.Token == 847)
			{
				tokenInfo.Type = TokenType.Keyword;
			}
		}
	}
}
