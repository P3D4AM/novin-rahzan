/**
 * Device FORM defaults & type/protocol/port harmonization (device-form fix).
 * =====================================================================
 * Pure & synchronous — NO React, NO DB, NO clock → unit-testable from
 * scripts/test-device-config.ts. This module is the SINGLE source of the
 * UI-layer rules; the handlers in devices.tsx only spread its results.
 *
 * Rules (docs-approved):
 *   * NEW Novin Pardaz NP device defaults to the FKATTEND family on port
 *     4238 — the approved NP756+ path (README «اتصال دستگاه NP756+»).
 *     SBXPC stays fully selectable and keeps its suggested port 5005.
 *   * ZKTeco defaults to TCP transport on port 4370.
 *   * The protocol family is an EXPLICIT choice (never guessed from the
 *     port). Selecting a family suggests its port; the port remains freely
 *     editable afterwards. Only a type/protocol SELECTION may touch the
 *     port — unrelated form edits never rewrite a custom port.
 *   * Existing devices are NEVER migrated: the edit form keeps the stored
 *     family/port verbatim (legacy rows without family still fall back to
 *     SBXPC in the EDIT form — backward compatibility).
 *   * The server payload never carries a contradictory selection: a ZK
 *     device is sent with the inert legacy family SBXPC (exactly the
 *     historical default for ZK rows), an NP device with a valid family.
 *
 * The BACKEND fallback in device-validation.ts is untouched by this module
 * (create-time empty family still normalizes to SBXPC there); the form now
 * always sends an explicit family, so the backend default is not reachable
 * from the UI path.
 */

export type DeviceTypeKey = 'NOVIN_NP' | 'ZKTECO'
export type NpProtocolFamily = 'FKATTEND' | 'SBXPC'

/** docs-approved default family for a NEW NP device (NP756+ path) */
export const NP_DEFAULT_FAMILY: NpProtocolFamily = 'FKATTEND'

/** suggested (default) TCP ports — must match DEFAULT_PORT_BY_FAMILY in device-validation.ts */
export const DEFAULT_PORT_FKATTEND = 4238
export const DEFAULT_PORT_SBXPC = 5005
export const DEFAULT_PORT_ZKTECO = 4370

export function isNpProtocolFamily(v: unknown): v is NpProtocolFamily {
  return v === 'FKATTEND' || v === 'SBXPC'
}

/** Suggested port for an NP protocol family selection (FKATTEND → 4238, SBXPC → 5005). */
export function suggestedPortForNpFamily(family: NpProtocolFamily): number {
  return family === 'FKATTEND' ? DEFAULT_PORT_FKATTEND : DEFAULT_PORT_SBXPC
}

/** Connection trio a NEW device form starts with (NP → FKATTEND/4238, transport TCP). */
export interface DeviceFormConnection {
  type: DeviceTypeKey
  protocolFamily: NpProtocolFamily
  port: number
  protocol: string
}

/** Defaults for the «new device» form: Novin Pardaz NP over the approved FKATTEND path. */
export function newDeviceFormDefaults(): DeviceFormConnection {
  return { type: 'NOVIN_NP', protocolFamily: NP_DEFAULT_FAMILY, port: DEFAULT_PORT_FKATTEND, protocol: 'TCP' }
}

/**
 * Harmonize the connection after a TYPE change (the old handler changed the
 * port but left the family untouched — an NP/FKATTEND device could end up on
 * port 5005). Predictable rules:
 *   * → NOVIN_NP: keep the previously selected NP family when one exists
 *     (an explicit SBXPC choice survives a ZK round-trip), otherwise fall
 *     back to the approved FKATTEND default; port := that family's suggestion.
 *   * → ZKTECO: port := 4370. The NP family is kept as form memory (it is
 *     inert for ZK and never sent — see serverPayloadFamilyFor).
 */
export function harmonizeTypeSelection(type: DeviceTypeKey, currentFamily: unknown): { type: DeviceTypeKey; protocolFamily: NpProtocolFamily; port: number } {
  if (type === 'NOVIN_NP') {
    const family = isNpProtocolFamily(currentFamily) ? currentFamily : NP_DEFAULT_FAMILY
    return { type, protocolFamily: family, port: suggestedPortForNpFamily(family) }
  }
  // ZKTECO — suggested port 4370; the NP family is kept only as inert memory
  const family = isNpProtocolFamily(currentFamily) ? currentFamily : NP_DEFAULT_FAMILY
  return { type, protocolFamily: family, port: DEFAULT_PORT_ZKTECO }
}

/**
 * Harmonize after an NP PROTOCOL FAMILY selection: FKATTEND suggests 4238,
 * SBXPC suggests 5005. The user stays free to edit the port afterwards
 * (this function returns a SUGGESTION, never a clamp).
 */
export function harmonizeProtocolSelection(family: NpProtocolFamily): { protocolFamily: NpProtocolFamily; port: number } {
  return { protocolFamily: family, port: suggestedPortForNpFamily(family) }
}

/**
 * Family actually sent to /api/devices. Contradictory selections never reach
 * the payload: ZK devices always carry the inert legacy family SBXPC (the
 * historical default for every ZK row — a ZK+FKATTEND combo is meaningless),
 * NP devices carry a valid family (defensively normalized).
 */
export function serverPayloadFamilyFor(type: DeviceTypeKey, family: unknown): NpProtocolFamily {
  if (type === 'NOVIN_NP') return isNpProtocolFamily(family) ? family : NP_DEFAULT_FAMILY
  return 'SBXPC'
}
