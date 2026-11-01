using System.Resources;
using System.Runtime.CompilerServices;
using FxResources.System.Configuration.ConfigurationManager;

namespace System;

internal static class SR
{
	private static ResourceManager s_resourceManager;

	internal static ResourceManager ResourceManager => s_resourceManager ?? (s_resourceManager = new ResourceManager(typeof(FxResources.System.Configuration.ConfigurationManager.SR)));

	internal static string Parameter_Invalid => GetResourceString("Parameter_Invalid");

	internal static string Parameter_NullOrEmpty => GetResourceString("Parameter_NullOrEmpty");

	internal static string Property_NullOrEmpty => GetResourceString("Property_NullOrEmpty");

	internal static string Property_Invalid => GetResourceString("Property_Invalid");

	internal static string Unexpected_Error => GetResourceString("Unexpected_Error");

	internal static string Wrapped_exception_message => GetResourceString("Wrapped_exception_message");

	internal static string Config_error_loading_XML_file => GetResourceString("Config_error_loading_XML_file");

	internal static string Config_exception_creating_section_handler => GetResourceString("Config_exception_creating_section_handler");

	internal static string Config_exception_creating_section => GetResourceString("Config_exception_creating_section");

	internal static string Config_tag_name_invalid => GetResourceString("Config_tag_name_invalid");

	internal static string Config_add_configurationsection_already_added => GetResourceString("Config_add_configurationsection_already_added");

	internal static string Config_add_configurationsection_already_exists => GetResourceString("Config_add_configurationsection_already_exists");

	internal static string Config_add_configurationsection_in_location_config => GetResourceString("Config_add_configurationsection_in_location_config");

	internal static string Config_add_configurationsectiongroup_already_added => GetResourceString("Config_add_configurationsectiongroup_already_added");

	internal static string Config_add_configurationsectiongroup_already_exists => GetResourceString("Config_add_configurationsectiongroup_already_exists");

	internal static string Config_add_configurationsectiongroup_in_location_config => GetResourceString("Config_add_configurationsectiongroup_in_location_config");

	internal static string Config_allow_exedefinition_error_application => GetResourceString("Config_allow_exedefinition_error_application");

	internal static string Config_allow_exedefinition_error_machine => GetResourceString("Config_allow_exedefinition_error_machine");

	internal static string Config_allow_exedefinition_error_roaminguser => GetResourceString("Config_allow_exedefinition_error_roaminguser");

	internal static string Config_appsettings_declaration_invalid => GetResourceString("Config_appsettings_declaration_invalid");

	internal static string Config_base_attribute_locked => GetResourceString("Config_base_attribute_locked");

	internal static string Config_base_collection_item_locked_cannot_clear => GetResourceString("Config_base_collection_item_locked_cannot_clear");

	internal static string Config_base_collection_item_locked => GetResourceString("Config_base_collection_item_locked");

	internal static string Config_base_cannot_add_items_above_inherited_items => GetResourceString("Config_base_cannot_add_items_above_inherited_items");

	internal static string Config_base_cannot_add_items_below_inherited_items => GetResourceString("Config_base_cannot_add_items_below_inherited_items");

	internal static string Config_base_cannot_remove_inherited_items => GetResourceString("Config_base_cannot_remove_inherited_items");

	internal static string Config_base_collection_elements_may_not_be_removed => GetResourceString("Config_base_collection_elements_may_not_be_removed");

	internal static string Config_base_collection_entry_already_exists => GetResourceString("Config_base_collection_entry_already_exists");

	internal static string Config_base_collection_entry_already_removed => GetResourceString("Config_base_collection_entry_already_removed");

	internal static string Config_base_collection_entry_not_found => GetResourceString("Config_base_collection_entry_not_found");

	internal static string Config_base_element_cannot_have_multiple_child_elements => GetResourceString("Config_base_element_cannot_have_multiple_child_elements");

	internal static string Config_base_element_locked => GetResourceString("Config_base_element_locked");

	internal static string Config_base_expected_to_find_element => GetResourceString("Config_base_expected_to_find_element");

	internal static string Config_base_invalid_attribute_to_lock => GetResourceString("Config_base_invalid_attribute_to_lock");

	internal static string Config_base_invalid_attribute_to_lock_by_add => GetResourceString("Config_base_invalid_attribute_to_lock_by_add");

	internal static string Config_base_invalid_element_key => GetResourceString("Config_base_invalid_element_key");

	internal static string Config_base_invalid_element_to_lock => GetResourceString("Config_base_invalid_element_to_lock");

	internal static string Config_base_invalid_element_to_lock_by_add => GetResourceString("Config_base_invalid_element_to_lock_by_add");

	internal static string Config_base_property_is_not_a_configuration_element => GetResourceString("Config_base_property_is_not_a_configuration_element");

	internal static string Config_base_read_only => GetResourceString("Config_base_read_only");

	internal static string Config_base_required_attribute_locked => GetResourceString("Config_base_required_attribute_locked");

	internal static string Config_base_required_attribute_lock_attempt => GetResourceString("Config_base_required_attribute_lock_attempt");

	internal static string Config_base_required_attribute_missing => GetResourceString("Config_base_required_attribute_missing");

	internal static string Config_base_section_invalid_content => GetResourceString("Config_base_section_invalid_content");

	internal static string Config_base_unrecognized_attribute => GetResourceString("Config_base_unrecognized_attribute");

	internal static string Config_base_unrecognized_element => GetResourceString("Config_base_unrecognized_element");

	internal static string Config_base_unrecognized_element_name => GetResourceString("Config_base_unrecognized_element_name");

	internal static string Config_base_value_cannot_contain => GetResourceString("Config_base_value_cannot_contain");

	internal static string Config_cannot_edit_configurationsection_in_location_config => GetResourceString("Config_cannot_edit_configurationsection_in_location_config");

	internal static string Config_cannot_edit_configurationsection_parentsection => GetResourceString("Config_cannot_edit_configurationsection_parentsection");

	internal static string Config_cannot_edit_configurationsection_when_location_locked => GetResourceString("Config_cannot_edit_configurationsection_when_location_locked");

	internal static string Config_cannot_edit_configurationsection_when_locked => GetResourceString("Config_cannot_edit_configurationsection_when_locked");

	internal static string Config_cannot_edit_configurationsection_when_not_attached => GetResourceString("Config_cannot_edit_configurationsection_when_not_attached");

	internal static string Config_cannot_edit_configurationsection_when_it_is_implicit => GetResourceString("Config_cannot_edit_configurationsection_when_it_is_implicit");

	internal static string Config_cannot_edit_configurationsection_when_it_is_undeclared => GetResourceString("Config_cannot_edit_configurationsection_when_it_is_undeclared");

	internal static string Config_cannot_edit_configurationsectiongroup_in_location_config => GetResourceString("Config_cannot_edit_configurationsectiongroup_in_location_config");

	internal static string Config_cannot_edit_configurationsectiongroup_when_not_attached => GetResourceString("Config_cannot_edit_configurationsectiongroup_when_not_attached");

	internal static string Config_cannot_edit_locationattriubtes => GetResourceString("Config_cannot_edit_locationattriubtes");

	internal static string Config_cannot_open_config_source => GetResourceString("Config_cannot_open_config_source");

	internal static string Config_client_config_init_error => GetResourceString("Config_client_config_init_error");

	internal static string Config_client_config_too_many_configsections_elements => GetResourceString("Config_client_config_too_many_configsections_elements");

	internal static string Config_configmanager_open_noexe => GetResourceString("Config_configmanager_open_noexe");

	internal static string Config_configsection_parentnotvalid => GetResourceString("Config_configsection_parentnotvalid");

	internal static string Config_connectionstrings_declaration_invalid => GetResourceString("Config_connectionstrings_declaration_invalid");

	internal static string Config_data_read_count_mismatch => GetResourceString("Config_data_read_count_mismatch");

	internal static string Config_element_no_context => GetResourceString("Config_element_no_context");

	internal static string Config_empty_lock_attributes_except => GetResourceString("Config_empty_lock_attributes_except");

	internal static string Config_empty_lock_element_except => GetResourceString("Config_empty_lock_element_except");

	internal static string Config_exception_in_config_section_handler => GetResourceString("Config_exception_in_config_section_handler");

	internal static string Config_file_doesnt_have_root_configuration => GetResourceString("Config_file_doesnt_have_root_configuration");

	internal static string Config_file_has_changed => GetResourceString("Config_file_has_changed");

	internal static string Config_getparentconfigurationsection_first_instance => GetResourceString("Config_getparentconfigurationsection_first_instance");

	internal static string Config_inconsistent_location_attributes => GetResourceString("Config_inconsistent_location_attributes");

	internal static string Config_invalid_attributes_for_write => GetResourceString("Config_invalid_attributes_for_write");

	internal static string Config_invalid_boolean_attribute => GetResourceString("Config_invalid_boolean_attribute");

	internal static string Config_invalid_node_type => GetResourceString("Config_invalid_node_type");

	internal static string Config_location_location_not_allowed => GetResourceString("Config_location_location_not_allowed");

	internal static string Config_location_path_invalid_character => GetResourceString("Config_location_path_invalid_character");

	internal static string Config_location_path_invalid_first_character => GetResourceString("Config_location_path_invalid_first_character");

	internal static string Config_location_path_invalid_last_character => GetResourceString("Config_location_path_invalid_last_character");

	internal static string Config_missing_required_attribute => GetResourceString("Config_missing_required_attribute");

	internal static string Config_more_data_than_expected => GetResourceString("Config_more_data_than_expected");

	internal static string Config_name_value_file_section_file_invalid_root => GetResourceString("Config_name_value_file_section_file_invalid_root");

	internal static string Config_namespace_invalid => GetResourceString("Config_namespace_invalid");

	internal static string Config_no_stream_to_write => GetResourceString("Config_no_stream_to_write");

	internal static string Config_not_allowed_to_encrypt_this_section => GetResourceString("Config_not_allowed_to_encrypt_this_section");

	internal static string Config_object_is_null => GetResourceString("Config_object_is_null");

	internal static string Config_operation_not_runtime => GetResourceString("Config_operation_not_runtime");

	internal static string Config_properties_may_not_be_derived_from_configuration_section => GetResourceString("Config_properties_may_not_be_derived_from_configuration_section");

	internal static string Config_provider_must_implement_type => GetResourceString("Config_provider_must_implement_type");

	internal static string Config_root_section_group_cannot_be_edited => GetResourceString("Config_root_section_group_cannot_be_edited");

	internal static string Config_section_allow_definition_attribute_invalid => GetResourceString("Config_section_allow_definition_attribute_invalid");

	internal static string Config_section_allow_exe_definition_attribute_invalid => GetResourceString("Config_section_allow_exe_definition_attribute_invalid");

	internal static string Config_section_cannot_be_used_in_location => GetResourceString("Config_section_cannot_be_used_in_location");

	internal static string Config_section_locked => GetResourceString("Config_section_locked");

	internal static string Config_sections_must_be_unique => GetResourceString("Config_sections_must_be_unique");

	internal static string Config_source_cannot_be_shared => GetResourceString("Config_source_cannot_be_shared");

	internal static string Config_source_parent_conflict => GetResourceString("Config_source_parent_conflict");

	internal static string Config_source_file_format => GetResourceString("Config_source_file_format");

	internal static string Config_source_invalid_format => GetResourceString("Config_source_invalid_format");

	internal static string Config_source_invalid_chars => GetResourceString("Config_source_invalid_chars");

	internal static string Config_source_requires_file => GetResourceString("Config_source_requires_file");

	internal static string Config_source_syntax_error => GetResourceString("Config_source_syntax_error");

	internal static string Config_system_already_set => GetResourceString("Config_system_already_set");

	internal static string Config_tag_name_already_defined => GetResourceString("Config_tag_name_already_defined");

	internal static string Config_tag_name_already_defined_at_this_level => GetResourceString("Config_tag_name_already_defined_at_this_level");

	internal static string Config_tag_name_cannot_be_location => GetResourceString("Config_tag_name_cannot_be_location");

	internal static string Config_tag_name_cannot_begin_with_config => GetResourceString("Config_tag_name_cannot_begin_with_config");

	internal static string Config_type_doesnt_inherit_from_type => GetResourceString("Config_type_doesnt_inherit_from_type");

	internal static string Config_unexpected_element_end => GetResourceString("Config_unexpected_element_end");

	internal static string Config_unexpected_element_name => GetResourceString("Config_unexpected_element_name");

	internal static string Config_unexpected_node_type => GetResourceString("Config_unexpected_node_type");

	internal static string Config_unrecognized_configuration_section => GetResourceString("Config_unrecognized_configuration_section");

	internal static string Config_write_failed => GetResourceString("Config_write_failed");

	internal static string Converter_timespan_not_in_second => GetResourceString("Converter_timespan_not_in_second");

	internal static string Converter_unsupported_value_type => GetResourceString("Converter_unsupported_value_type");

	internal static string Decryption_failed => GetResourceString("Decryption_failed");

	internal static string Default_value_conversion_error_from_string => GetResourceString("Default_value_conversion_error_from_string");

	internal static string Default_value_wrong_type => GetResourceString("Default_value_wrong_type");

	internal static string DPAPI_bad_data => GetResourceString("DPAPI_bad_data");

	internal static string Empty_attribute => GetResourceString("Empty_attribute");

	internal static string EncryptedNode_not_found => GetResourceString("EncryptedNode_not_found");

	internal static string EncryptedNode_is_in_invalid_format => GetResourceString("EncryptedNode_is_in_invalid_format");

	internal static string Encryption_failed => GetResourceString("Encryption_failed");

	internal static string IndexOutOfRange => GetResourceString("IndexOutOfRange");

	internal static string Invalid_enum_value => GetResourceString("Invalid_enum_value");

	internal static string Must_add_to_config_before_protecting_it => GetResourceString("Must_add_to_config_before_protecting_it");

	internal static string No_converter => GetResourceString("No_converter");

	internal static string No_exception_information_available => GetResourceString("No_exception_information_available");

	internal static string Property_name_reserved => GetResourceString("Property_name_reserved");

	internal static string Item_name_reserved => GetResourceString("Item_name_reserved");

	internal static string Basicmap_item_name_reserved => GetResourceString("Basicmap_item_name_reserved");

	internal static string ProtectedConfigurationProvider_not_found => GetResourceString("ProtectedConfigurationProvider_not_found");

	internal static string Regex_validator_error => GetResourceString("Regex_validator_error");

	internal static string String_null_or_empty => GetResourceString("String_null_or_empty");

	internal static string Subclass_validator_error => GetResourceString("Subclass_validator_error");

	internal static string Top_level_conversion_error_from_string => GetResourceString("Top_level_conversion_error_from_string");

	internal static string Top_level_conversion_error_to_string => GetResourceString("Top_level_conversion_error_to_string");

	internal static string Top_level_validation_error => GetResourceString("Top_level_validation_error");

	internal static string Type_cannot_be_resolved => GetResourceString("Type_cannot_be_resolved");

	internal static string TypeNotPublic => GetResourceString("TypeNotPublic");

	internal static string Unrecognized_initialization_value => GetResourceString("Unrecognized_initialization_value");

	internal static string Validation_scalar_range_violation_not_different => GetResourceString("Validation_scalar_range_violation_not_different");

	internal static string Validation_scalar_range_violation_not_equal => GetResourceString("Validation_scalar_range_violation_not_equal");

	internal static string Validation_scalar_range_violation_not_in_range => GetResourceString("Validation_scalar_range_violation_not_in_range");

	internal static string Validation_scalar_range_violation_not_outside_range => GetResourceString("Validation_scalar_range_violation_not_outside_range");

	internal static string Validator_Attribute_param_not_validator => GetResourceString("Validator_Attribute_param_not_validator");

	internal static string Validator_does_not_support_elem_type => GetResourceString("Validator_does_not_support_elem_type");

	internal static string Validator_does_not_support_prop_type => GetResourceString("Validator_does_not_support_prop_type");

	internal static string Validator_element_not_valid => GetResourceString("Validator_element_not_valid");

	internal static string Validator_method_not_found => GetResourceString("Validator_method_not_found");

	internal static string Validator_min_greater_than_max => GetResourceString("Validator_min_greater_than_max");

	internal static string Validator_scalar_resolution_violation => GetResourceString("Validator_scalar_resolution_violation");

	internal static string Validator_string_invalid_chars => GetResourceString("Validator_string_invalid_chars");

	internal static string Validator_string_max_length => GetResourceString("Validator_string_max_length");

	internal static string Validator_string_min_length => GetResourceString("Validator_string_min_length");

	internal static string Validator_value_type_invalid => GetResourceString("Validator_value_type_invalid");

	internal static string Validator_multiple_validator_attributes => GetResourceString("Validator_multiple_validator_attributes");

	internal static string Validator_timespan_value_must_be_positive => GetResourceString("Validator_timespan_value_must_be_positive");

	internal static string WrongType_of_Protected_provider => GetResourceString("WrongType_of_Protected_provider");

	internal static string Config_element_locking_not_supported => GetResourceString("Config_element_locking_not_supported");

	internal static string Protection_provider_syntax_error => GetResourceString("Protection_provider_syntax_error");

	internal static string Protection_provider_invalid_format => GetResourceString("Protection_provider_invalid_format");

	internal static string Cannot_declare_or_remove_implicit_section => GetResourceString("Cannot_declare_or_remove_implicit_section");

	internal static string Config_reserved_attribute => GetResourceString("Config_reserved_attribute");

	internal static string Filename_in_SaveAs_is_used_already => GetResourceString("Filename_in_SaveAs_is_used_already");

	internal static string Provider_Already_Initialized => GetResourceString("Provider_Already_Initialized");

	internal static string Config_provider_name_null_or_empty => GetResourceString("Config_provider_name_null_or_empty");

	internal static string CollectionReadOnly => GetResourceString("CollectionReadOnly");

	internal static string Config_source_not_under_config_dir => GetResourceString("Config_source_not_under_config_dir");

	internal static string Config_source_invalid => GetResourceString("Config_source_invalid");

	internal static string Location_invalid_inheritInChildApplications_in_machine_or_root_web_config => GetResourceString("Location_invalid_inheritInChildApplications_in_machine_or_root_web_config");

	internal static string Cannot_change_both_AllowOverride_and_OverrideMode => GetResourceString("Cannot_change_both_AllowOverride_and_OverrideMode");

	internal static string Config_section_override_mode_attribute_invalid => GetResourceString("Config_section_override_mode_attribute_invalid");

	internal static string Invalid_override_mode_declaration => GetResourceString("Invalid_override_mode_declaration");

	internal static string Machine_config_file_not_found => GetResourceString("Machine_config_file_not_found");

	internal static string ObjectDisposed_StreamClosed => GetResourceString("ObjectDisposed_StreamClosed");

	internal static string Unable_to_convert_type_from_string => GetResourceString("Unable_to_convert_type_from_string");

	internal static string Unable_to_convert_type_to_string => GetResourceString("Unable_to_convert_type_to_string");

	internal static string Could_not_create_from_default_value => GetResourceString("Could_not_create_from_default_value");

	internal static string Could_not_create_from_default_value_2 => GetResourceString("Could_not_create_from_default_value_2");

	internal static string UserSettingsNotSupported => GetResourceString("UserSettingsNotSupported");

	internal static string SettingsSaveFailed => GetResourceString("SettingsSaveFailed");

	internal static string SettingsSaveFailedNoSection => GetResourceString("SettingsSaveFailedNoSection");

	internal static string UnknownUserLevel => GetResourceString("UnknownUserLevel");

	internal static string BothScopeAttributes => GetResourceString("BothScopeAttributes");

	internal static string NoScopeAttributes => GetResourceString("NoScopeAttributes");

	internal static string SettingsPropertyNotFound => GetResourceString("SettingsPropertyNotFound");

	internal static string SettingsPropertyReadOnly => GetResourceString("SettingsPropertyReadOnly");

	internal static string SettingsPropertyWrongType => GetResourceString("SettingsPropertyWrongType");

	internal static string ProviderInstantiationFailed => GetResourceString("ProviderInstantiationFailed");

	internal static string ProviderTypeLoadFailed => GetResourceString("ProviderTypeLoadFailed");

	internal static string AppSettingsReaderNoKey => GetResourceString("AppSettingsReaderNoKey");

	internal static string AppSettingsReaderCantParse => GetResourceString("AppSettingsReaderCantParse");

	internal static string AppSettingsReaderEmptyString => GetResourceString("AppSettingsReaderEmptyString");

	internal static string Config_invalid_integer_attribute => GetResourceString("Config_invalid_integer_attribute");

	internal static string Config_base_required_attribute_empty => GetResourceString("Config_base_required_attribute_empty");

	internal static string Config_base_elements_only => GetResourceString("Config_base_elements_only");

	internal static string Config_base_no_child_nodes => GetResourceString("Config_base_no_child_nodes");

	internal static string InvalidNullEmptyArgument => GetResourceString("InvalidNullEmptyArgument");

	internal static string DuplicateFileName => GetResourceString("DuplicateFileName");

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool UsingResourceKeys()
	{
		return false;
	}

	internal static string GetResourceString(string resourceKey, string defaultString = null)
	{
		if (UsingResourceKeys())
		{
			return defaultString ?? resourceKey;
		}
		string text = null;
		try
		{
			text = ResourceManager.GetString(resourceKey);
		}
		catch (MissingManifestResourceException)
		{
		}
		if (defaultString != null && resourceKey.Equals(text))
		{
			return defaultString;
		}
		return text;
	}

	internal static string Format(string resourceFormat, object p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(resourceFormat, p1);
	}

	internal static string Format(string resourceFormat, object p1, object p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(resourceFormat, p1, p2);
	}

	internal static string Format(string resourceFormat, object p1, object p2, object p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(resourceFormat, p1, p2, p3);
	}

	internal static string Format(string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			if (UsingResourceKeys())
			{
				return resourceFormat + ", " + string.Join(", ", args);
			}
			return string.Format(resourceFormat, args);
		}
		return resourceFormat;
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1);
		}
		return string.Format(provider, resourceFormat, p1);
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2);
		}
		return string.Format(provider, resourceFormat, p1, p2);
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, object p1, object p2, object p3)
	{
		if (UsingResourceKeys())
		{
			return string.Join(", ", resourceFormat, p1, p2, p3);
		}
		return string.Format(provider, resourceFormat, p1, p2, p3);
	}

	internal static string Format(IFormatProvider provider, string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			if (UsingResourceKeys())
			{
				return resourceFormat + ", " + string.Join(", ", args);
			}
			return string.Format(provider, resourceFormat, args);
		}
		return resourceFormat;
	}
}
