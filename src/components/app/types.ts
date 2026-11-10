'use client'

export type Role = 'SUPER_ADMIN' | 'ADMIN' | 'APPROVER' | 'EMPLOYEE'

export interface SessionUser {
  id: string
  username: string
  role: Role
  fullName: string
  personId: string | null
}

export interface Person {
  id: string
  personnelCode: string
  firstName: string
  lastName: string
  nationalId?: string | null
  fatherName?: string | null
  gender?: string | null
  birthDate?: string | null
  phone?: string | null
  email?: string | null
  address?: string | null
  photoUrl?: string | null
  departmentId?: string | null
  jobTitleId?: string | null
  employmentTypeId?: string | null
  hireDate?: string | null
  endDate?: string | null
  status: string
  pin?: string
  qrToken?: string
  deviceUserId?: number | null
  annualLeaveDays?: number
  defaultShiftId?: string | null
  notes?: string | null
  department?: { name: string } | null
  jobTitle?: { name: string } | null
  employmentType?: { name: string } | null
  defaultShift?: { id: string; name: string; color: string } | null
}

export interface ShiftSegmentT {
  id?: string
  order: number
  startMin: number
  endMin: number
  breakMin: number
}

export interface ShiftT {
  id: string
  name: string
  color: string
  type: string
  lateGraceMin: number
  earlyGraceMin: number
  overtimeEligible: boolean
  isActive: boolean
  segments: ShiftSegmentT[]
}

export interface DailyRecordT {
  id: string
  personId: string
  date: string
  status: string
  shiftId?: string | null
  firstIn?: string | null
  lastOut?: string | null
  workedMinutes: number
  lateMinutes: number
  earlyLeaveMinutes: number
  overtimeMinutes: number
  leaveTypeId?: string | null
  leaveMinutes: number
  person?: { id: string; firstName: string; lastName: string; personnelCode: string; department?: { name: string } | null; jobTitle?: { name: string } | null }
  shift?: { id: string; name: string; color: string } | null
  leaveType?: { name: string } | null
}

export interface LeaveRequestT {
  id: string
  personId: string
  leaveTypeId: string
  kind: string
  startDateTime: string
  endDateTime: string
  durationMinutes: number
  reason?: string | null
  status: string
  decisionNote?: string | null
  decidedAt?: string | null
  createdAt: string
  person?: { id: string; firstName: string; lastName: string; personnelCode: string }
  leaveType?: { id: string; name: string; color: string; paid: boolean }
}

export interface OvertimeT {
  id: string
  personId: string
  date: string
  minutes: number
  reason?: string | null
  status: string
  person?: { id: string; firstName: string; lastName: string; personnelCode: string }
}

export interface DeviceT {
  id: string
  name: string
  type: string
  ipAddress?: string | null
  port: number
  protocol: string
  location?: string | null
  serialNumber?: string | null
  password?: number | null
  enabled: boolean
  status: string
  lastSyncAt?: string | null
  lastSyncCount: number
  lastLogAt?: string | null
  syncIntervalMin?: number
}

export interface NotificationT {
  id: string
  title: string
  body: string
  type: string
  read: boolean
  createdAt: string
}

export interface Bootstrap {
  departments: { id: string; name: string; description?: string | null }[]
  jobTitles: { id: string; name: string }[]
  employmentTypes: { id: string; name: string }[]
  shifts: ShiftT[]
  rotations: { id: string; name: string; pattern: string; isActive: boolean }[]
  leaveTypes: { id: string; name: string; paid: boolean; color: string; requiresApproval: boolean; countsTowardsBalance: boolean }[]
  devices: DeviceT[]
  users: { id: string; username: string; role: string; fullName: string; active: boolean; personnelId: string | null }[]
  rules: { id: string; type: string; enabled: boolean; thresholdMin: number; notifyRole: string }[]
  holidays: { id: string; date: string; title: string; type: string }[]
  settings: Record<string, string>
  today: { jy: number; jm: number; jd: number }
}

export const STATUS_FA: Record<string, string> = {
  PRESENT: 'حاضر',
  ABSENT: 'غایب',
  LEAVE: 'مرخصی',
  HOLIDAY: 'تعطیل رسمی',
  OFF: 'تعطیلی',
  INCOMPLETE: 'ناقص',
  PENDING: 'در انتظار',
  MISSION: 'مأموریت',
}

export const STATUS_COLOR: Record<string, string> = {
  PRESENT: 'bg-emerald-100 text-emerald-800 dark:bg-emerald-900 dark:text-emerald-200',
  ABSENT: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200',
  LEAVE: 'bg-amber-100 text-amber-800 dark:bg-amber-900 dark:text-amber-200',
  HOLIDAY: 'bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-200',
  OFF: 'bg-gray-100 text-gray-600 dark:bg-gray-800 dark:text-gray-300',
  INCOMPLETE: 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200',
  PENDING: 'bg-slate-100 text-slate-600 dark:bg-slate-800 dark:text-slate-300',
  MISSION: 'bg-cyan-100 text-cyan-800 dark:bg-cyan-900 dark:text-cyan-200',
}

export const ROLE_FA: Record<Role, string> = {
  SUPER_ADMIN: 'مدیر ارشد',
  ADMIN: 'مدیر منابع انسانی',
  APPROVER: 'تأییدکننده',
  EMPLOYEE: 'کارمند',
}

export async function api<T = unknown>(url: string, options?: RequestInit): Promise<T> {
  const res = await fetch(url, { ...options, headers: { 'Content-Type': 'application/json', ...(options?.headers || {}) } })
  const data = await res.json().catch(() => ({}))
  if (!res.ok) throw new Error((data as { error?: string }).error || `خطا (${res.status})`)
  return data as T
}

export function full_name(p?: { firstName?: string; lastName?: string } | null): string {
  return p ? `${p.firstName ?? ''} ${p.lastName ?? ''}`.trim() : '-'
}
