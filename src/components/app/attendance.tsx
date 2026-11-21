'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import { SessionUser, Bootstrap, api, DailyRecordT, STATUS_FA, STATUS_COLOR, Person, full_name } from './types'
import { JalaliDatePicker, TimeInput, faTime, faDigits, durationFa, gregKeyToJalali } from './jalali-ui'
import { tehranTodayMarker, addDaysMarker, markerKey, tehranWallClockStringToInstant, formatJalaliInstant, dayMarkerOf, formatTehranTime, jalaliOfMarker, jalaliMonthRange } from '@/lib/time'
import { useToast } from '@/hooks/use-toast'
import { RefreshCw, Pencil, Search, Loader2, Trash2, Plus, Calculator, CheckSquare } from 'lucide-react'
import { Checkbox } from '@/components/ui/checkbox'
import { Textarea } from '@/components/ui/textarea'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

export function AttendanceView({ user, boot, reloadBoot, dataVersion }: Props & { dataVersion?: number }) {
  const isAdmin = user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'
  return (
    <Tabs defaultValue="daily" dir="rtl">
      <TabsList className="mb-4">
        <TabsTrigger value="daily">کارکرد روزانه</TabsTrigger>
        {/* Phase 3 stage 3 (item 5): architecture made honest — this tab shows
            PunchRecord (the OPERATIONAL attendance layer), so its title says so;
            the RawPunch device landing zone has its own tab below. */}
        <TabsTrigger value="punches">ترددها (عملیاتی)</TabsTrigger>
        {isAdmin && <TabsTrigger value="raw">ترددهای خام دستگاه</TabsTrigger>}
      </TabsList>
      <TabsContent value="daily"><DailyTab boot={boot} user={user} dataVersion={dataVersion} /></TabsContent>
      <TabsContent value="punches"><PunchesTab boot={boot} user={user} reloadBoot={reloadBoot} dataVersion={dataVersion} /></TabsContent>
      {/* RawPunch: the traceable device history (landing zone) — admins only
          (the API enforces ADMIN too); UNMATCHED/AMBIGUOUS rows and the
          quarantine are never hidden from the administrator. */}
      {isAdmin && <TabsContent value="raw"><RawPunchTab dataVersion={dataVersion} /></TabsContent>}
    </Tabs>
  )
}

/* ---------------- daily attendance ---------------- */
// v1.6.0 (phase 1 stage 4): Tehran-based today/defaults via the central module
// (was new Date().toISOString().slice(0,10) — UTC date, browser-tz sensitive)
function tehranTodayKey() { return markerKey(tehranTodayMarker()) }
function tehranDaysAgoKey(n: number) { return markerKey(addDaysMarker(tehranTodayMarker(), -n)) }

/* ---------------- Phase 3 stage 3 (item 5): date-range presets ------------- */
export interface RangeSel { preset: string; from: string; to: string }

function rangeForPreset(preset: string): { from: string; to: string } {
  const today = tehranTodayKey()
  switch (preset) {
    case 'today': return { from: today, to: today }
    case 'd7': return { from: tehranDaysAgoKey(6), to: today }
    case 'd30': return { from: tehranDaysAgoKey(29), to: today }
    case 'd90': return { from: tehranDaysAgoKey(89), to: today }
    case 'month': {
      const { jy, jm } = jalaliOfMarker(tehranTodayMarker())
      const mr = jalaliMonthRange(jy, jm)
      return { from: markerKey(mr.startMarker), to: today }
    }
    default: return { from: tehranDaysAgoKey(6), to: today }
  }
}

/** Preset selector (امروز/۷/۳۰/۹۰ روز/ماه جاری/دلخواه) + custom Jalali pickers —
 *  shared by the punches and raw-punch tabs; the selected range is REALLY
 *  sent to the API (server-side filtering), never display-only. */
function RangePresets({ value, onChange }: { value: RangeSel; onChange: (v: RangeSel) => void }) {
  const onPreset = (preset: string) => {
    if (preset === 'custom') onChange({ preset, from: value.from, to: value.to })
    else onChange({ preset, ...rangeForPreset(preset) })
  }
  return (
    <div className="flex items-center gap-1.5 text-xs">
      <Select value={value.preset} onValueChange={onPreset}>
        <SelectTrigger className="w-32 h-8"><SelectValue /></SelectTrigger>
        <SelectContent>
          <SelectItem value="today" className="text-xs">امروز</SelectItem>
          <SelectItem value="d7" className="text-xs">۷ روز اخیر</SelectItem>
          <SelectItem value="d30" className="text-xs">۳۰ روز اخیر</SelectItem>
          <SelectItem value="d90" className="text-xs">۹۰ روز اخیر</SelectItem>
          <SelectItem value="month" className="text-xs">ماه جاری</SelectItem>
          <SelectItem value="custom" className="text-xs">بازه دلخواه</SelectItem>
        </SelectContent>
      </Select>
      {value.preset === 'custom' && (
        <>
          <span className="text-muted-foreground">از</span>
          <div className="w-32"><JalaliDatePicker value={value.from} onChange={(from) => onChange({ ...value, from })} /></div>
          <span className="text-muted-foreground">تا</span>
          <div className="w-32"><JalaliDatePicker value={value.to} onChange={(to) => onChange({ ...value, to })} /></div>
        </>
      )}
    </div>
  )
}

function DailyTab({ boot, user, dataVersion }: { boot: Bootstrap; user: SessionUser; dataVersion?: number }) {
  const today = boot.today
  const [from, setFrom] = useState(tehranDaysAgoKey(6))
  const [to, setTo] = useState(tehranTodayKey())
  const [dept, setDept] = useState('all')
  const [status, setStatus] = useState('all')
  const [q, setQ] = useState('')
  const [records, setRecords] = useState<DailyRecordT[]>([])
  const [loading, setLoading] = useState(false)
  const [recalcOpen, setRecalcOpen] = useState(false)
  const [recalcBusy, setRecalcBusy] = useState(false)
  const [page, setPage] = useState(1)
  const pageSize = 100
  const { toast } = useToast()
  const isAdmin = user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'
  // Phase 3 stage 6 (item 6): bulk selection + OVERRIDE edit on the derived rows
  const [selected, setSelected] = useState<Set<string>>(new Set())
  const [bulkOpen, setBulkOpen] = useState<'edit' | 'delete' | null>(null)
  const [bulkBusy, setBulkBusy] = useState(false)
  const [ovStatus, setOvStatus] = useState('')
  const [ovWorked, setOvWorked] = useState('')
  const [ovLate, setOvLate] = useState('')
  const [ovEarly, setOvEarly] = useState('')
  const [ovNote, setOvNote] = useState('')
  const [bulkReason, setBulkReason] = useState('')
  const toggleSel = (id: string) => setSelected((s) => { const n = new Set(s); if (n.has(id)) n.delete(id); else n.add(id); return n })

  const runDailyBulk = async (op: 'edit' | 'delete' | 'clear') => {
    const ids = [...selected]
    if (!ids.length) return
    setBulkBusy(true)
    try {
      const body = op === 'delete'
        ? { entity: 'daily', action: 'delete', ids, reason: bulkReason }
        : op === 'clear'
          ? { entity: 'daily', action: 'edit', ids, clear: true }
          : {
              entity: 'daily', action: 'edit', ids,
              ...(ovStatus ? { status: ovStatus } : {}),
              ...(ovWorked.trim() ? { workedMinutes: Number(ovWorked) } : {}),
              ...(ovLate.trim() ? { lateMinutes: Number(ovLate) } : {}),
              ...(ovEarly.trim() ? { earlyLeaveMinutes: Number(ovEarly) } : {}),
              ...(ovNote.trim() ? { note: ovNote } : {}),
            }
      const res = await api<{ ok: boolean; overridden?: number; cleared?: number; deleted?: number; notFound?: string[] }>('/api/bulk-ops', { method: 'POST', body: JSON.stringify(body) })
      const done = op === 'delete' ? res.deleted ?? 0 : res.overridden ?? res.cleared ?? 0
      toast({
        title: op === 'delete' ? 'حذف گروهی انجام شد' : op === 'clear' ? 'اصلاحات دستی پاک شد' : 'اصلاح دستی گروهی ثبت شد',
        description: `${faDigits(done)} رکورد${res.notFound?.length ? ` · ${faDigits(res.notFound.length)} یافت نشد` : ''} — در AuditLog ثبت شد.${op === 'edit' ? ' اصلاح دستی در بازمحاسبه‌های بعدی حفظ می‌شود.' : ''}`,
      })
      setSelected(new Set())
      setBulkOpen(null)
      setBulkReason('')
      load()
    } catch (err) {
      toast({ title: 'خطا در عملیات گروهی', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setBulkBusy(false) }
  }

  // Phase 3 stage 4 (item 8): the admin's RANGE RECALCULATION operation — the
  // engine materializes a DailyAttendance row for EVERY (person × day) of the
  // range (ABSENT/OFF/HOLIDAY/LEAVE even without punches), idempotent upserts
  // never duplicate rows and never silently rewrite other days (the engine's
  // dated-history + assignment resolution decides each day's shift as-of).
  const runRangeRecalc = async () => {
    setRecalcBusy(true)
    try {
      const res = await api<{ ok: boolean; count: number }>('/api/attendance', { method: 'POST', body: JSON.stringify({ from, to }) })
      toast({ title: 'بازمحاسبه انجام شد', description: `${faDigits(res.count)} روز/شخصیت برای بازهٔ ${from} تا ${to} محاسبه شد — روزهای بدون تردد با وضعیت متناسب (غیبت/تعطیلی/مرخصی) تولید شدند.` })
      load()
    } catch (err) {
      toast({ title: 'خطا در بازمحاسبه', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setRecalcBusy(false); setRecalcOpen(false) }
  }

  const load = async () => {
    setLoading(true)
    try {
      const res = await api<{ records: DailyRecordT[] }>(`/api/attendance?from=${from}&to=${to}&departmentId=${dept === 'all' ? '' : dept}&status=${status === 'all' ? '' : status}`)
      setRecords(res.records)
    } finally { setLoading(false) }
  }
  useEffect(() => { load()   }, [from, to, dept, status])
  // v1.4.3 (addendum P4): device-sync events refetch the report WITHOUT a
  // manual page refresh (role-scoped data comes from the API — no names are
  // broadcast over the socket)
  useEffect(() => { load()   }, [dataVersion])

  const filtered = records.filter((r) => {
    if (q) {
      const name = `${r.person?.firstName ?? ''} ${r.person?.lastName ?? ''}`
      if (!name.includes(q) && !(r.person?.personnelCode ?? '').includes(q)) return false
    }
    return true
  })
  const paged = filtered.slice((page - 1) * pageSize, page * pageSize)
  const pages = Math.max(1, Math.ceil(filtered.length / pageSize))

  const totals = {
    worked: records.reduce((a, r) => a + r.workedMinutes, 0),
    ot: records.reduce((a, r) => a + r.overtimeMinutes, 0),
    late: records.reduce((a, r) => a + r.lateMinutes, 0),
    absent: records.filter((r) => r.status === 'ABSENT').length,
  }

  return (
    <div className="space-y-4">
      <BulkBar count={selected.size} onClear={() => setSelected(new Set())} onEdit={isAdmin ? () => setBulkOpen('edit') : undefined} onDelete={isAdmin ? () => setBulkOpen('delete') : undefined} busy={bulkBusy} />
      <div className="flex flex-wrap items-center gap-2">
        <div className="relative flex-1 min-w-44">
          <Search className="absolute right-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input placeholder="جستجوی نام یا کد…" value={q} onChange={(e) => { setQ(e.target.value); setPage(1) }} className="pr-9" />
        </div>
        <Select value={dept} onValueChange={(v) => { setDept(v); setPage(1) }}>
          <SelectTrigger className="w-40"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه دپارتمان‌ها</SelectItem>
            {boot.departments.map((d) => <SelectItem key={d.id} value={d.id}>{d.name}</SelectItem>)}
          </SelectContent>
        </Select>
        <Select value={status} onValueChange={(v) => { setStatus(v); setPage(1) }}>
          <SelectTrigger className="w-32"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه وضعیت‌ها</SelectItem>
            <ObjectEntries obj={STATUS_FA} />
          </SelectContent>
        </Select>
        <div className="flex items-center gap-1.5 text-xs">
          <span className="text-muted-foreground">از</span>
          <div className="w-32"><JalaliDatePicker value={from} onChange={setFrom} /></div>
          <span className="text-muted-foreground">تا</span>
          <div className="w-32"><JalaliDatePicker value={to} onChange={setTo} /></div>
        </div>
        {isAdmin && (
          <Button variant="outline" size="sm" onClick={() => setRecalcOpen(true)} className="gap-1.5" title="بازمحاسبهٔ همهٔ روزهای بازه برای همهٔ پرسنل فعال — روزهای بدون تردد هم با وضعیت مناسب (غیبت/تعطیلی/مرخصی) ساخته می‌شوند">
            <Calculator className="h-4 w-4" /> بازمحاسبهٔ بازه
          </Button>
        )}
        <Button variant="outline" size="sm" onClick={load} disabled={loading}><RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} /></Button>
      </div>

      <Dialog open={recalcOpen} onOpenChange={setRecalcOpen}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader>
            <DialogTitle>بازمحاسبهٔ بازه‌ای کارکرد روزانه</DialogTitle>
          </DialogHeader>
          <div className="text-sm space-y-2">
            <p>برای همهٔ پرسنل فعال، بازهٔ <b dir="ltr">{from}</b> تا <b dir="ltr">{to}</b> دوباره محاسبه می‌شود:</p>
            <ul className="text-xs text-muted-foreground list-disc pr-5 space-y-1">
              <li>برای هر روزِ مشمول شیفت، حتی بدون تردد، رکورد با وضعیت متناسب (غیبت، تعطیلی، مرخصی) ساخته می‌شود.</li>
              <li>رکوردهای تکراری ایجاد نمی‌شوند (به‌روزرسانی همان رکورد است) و رکوردهای روزهای دیگر بازه تغییر نمی‌کنند.</li>
              <li>شیفت هر روز از تاریخچهٔ تخصیص‌ها/شیفت پیش‌فرضِ همان روز خوانده می‌شود.</li>
            </ul>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setRecalcOpen(false)}>انصراف</Button>
            <Button onClick={runRangeRecalc} disabled={recalcBusy} className="gap-1.5">{recalcBusy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} اجرای بازمحاسبه</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Phase 3 stage 6 (item 6): bulk OVERRIDE edit + bulk delete on the derived rows */}
      <Dialog open={bulkOpen === 'edit'} onOpenChange={(o) => { if (!o && !bulkBusy) setBulkOpen(null) }}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>اصلاح دستی گروهی {faDigits(selected.size)} روز</DialogTitle></DialogHeader>
          <div className="space-y-3 text-sm">
            <div className="rounded-md bg-muted/60 border p-3 text-xs leading-5 text-muted-foreground">
              کارکرد روزانه دادهٔ <b>مشتق‌شده</b> است؛ این اصلاح به‌صورت <b>Override ثبت‌شده</b> اعمال می‌شود و در بازمحاسبه‌های بعدی <b>حفظ می‌شود</b> (و با پاک‌کردن آن، مقادیر محاسباتی برمی‌گردند). مقادیر قبلی در AuditLog ثبت می‌شوند.
            </div>
            <div className="grid grid-cols-2 gap-2">
              <div className="space-y-1.5">
                <Label className="text-xs">وضعیت (اختیاری)</Label>
                <Select value={ovStatus} onValueChange={setOvStatus}>
                  <SelectTrigger><SelectValue placeholder="بدون تغییر" /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="">بدون تغییر</SelectItem>
                    <ObjectEntries obj={STATUS_FA} />
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-1.5"><Label className="text-xs">کارکرد (دقیقه، اختیاری)</Label><Input dir="ltr" type="number" min={0} max={1440} value={ovWorked} onChange={(e) => setOvWorked(e.target.value)} /></div>
              <div className="space-y-1.5"><Label className="text-xs">تأخیر (دقیقه، اختیاری)</Label><Input dir="ltr" type="number" min={0} max={1440} value={ovLate} onChange={(e) => setOvLate(e.target.value)} /></div>
              <div className="space-y-1.5"><Label className="text-xs">خروج زودهنگام (دقیقه، اختیاری)</Label><Input dir="ltr" type="number" min={0} max={1440} value={ovEarly} onChange={(e) => setOvEarly(e.target.value)} /></div>
            </div>
            <div className="space-y-1.5"><Label className="text-xs">یادداشت (اختیاری — جایگزین یادداشت محاسباتی می‌شود)</Label><Input dir="rtl" value={ovNote} onChange={(e) => setOvNote(e.target.value)} /></div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setBulkOpen(null)} disabled={bulkBusy}>انصراف</Button>
            <Button variant="outline" onClick={() => runDailyBulk('clear')} disabled={bulkBusy} className="gap-1">پاک‌کردن اصلاحات (بازگشت به محاسباتی)</Button>
            <Button onClick={() => runDailyBulk('edit')} disabled={bulkBusy || (!ovStatus && !ovWorked.trim() && !ovLate.trim() && !ovEarly.trim() && !ovNote.trim())} className="gap-1.5">{bulkBusy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} ثبت اصلاح</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
      <Dialog open={bulkOpen === 'delete'} onOpenChange={(o) => { if (!o && !bulkBusy) setBulkOpen(null) }}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>حذف گروهی {faDigits(selected.size)} رکورد کارکرد روزانه</DialogTitle></DialogHeader>
          <div className="space-y-3 text-sm">
            <div className="rounded-md bg-destructive/10 border border-destructive/30 p-3 text-xs leading-5">
              این رکوردهای مشتق‌شده حذف می‌شوند (مقادیر قبلی در AuditLog می‌ماند). ترددها و دادهٔ خام دست‌نخورده‌اند؛ با «بازمحاسبهٔ بازه» دوباره ساخته می‌شوند.
            </div>
            <BulkReasonField value={bulkReason} onChange={setBulkReason} placeholder="مثلاً: حذف رکوردهای آزمایشی" />
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setBulkOpen(null)} disabled={bulkBusy}>انصراف</Button>
            <Button variant="destructive" onClick={() => runDailyBulk('delete')} disabled={bulkBusy || !bulkReason.trim()} className="gap-1.5">{bulkBusy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} حذف نهایی</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      <div className="flex flex-wrap gap-2 text-xs">
        <Badge variant="secondary">جمع کارکرد: {durationFa(totals.worked)}</Badge>
        <Badge variant="secondary">جمع اضافه‌کاری: {durationFa(totals.ot)}</Badge>
        <Badge variant="secondary">جمع تأخیر: {durationFa(totals.late)}</Badge>
        <Badge variant="secondary">روزهای غیبت: {faDigits(totals.absent)}</Badge>
      </div>

      <Card>
        <CardContent className="p-0">
          <div className="max-h-[65vh] overflow-auto">
            <Table>
              <TableHeader className="sticky top-0 bg-background z-10">
                <TableRow>
                  {isAdmin && <TableHead className="w-8"><Checkbox checked={paged.length > 0 && paged.every((r) => selected.has(r.id))} onCheckedChange={(v) => (v ? setSelected(new Set(paged.map((r) => r.id))) : setSelected(new Set()))} aria-label="انتخاب همهٔ رکوردهای صفحه" /></TableHead>}
                  <TableHead>تاریخ</TableHead>
                  <TableHead>پرسنل</TableHead>
                  <TableHead className="hidden md:table-cell">شیفت</TableHead>
                  <TableHead>ورود</TableHead>
                  <TableHead>خروج</TableHead>
                  <TableHead>کارکرد</TableHead>
                  <TableHead className="hidden sm:table-cell">تأخیر</TableHead>
                  <TableHead className="hidden sm:table-cell">اضافه‌کاری</TableHead>
                  <TableHead>وضعیت</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {paged.map((r) => (
                  <TableRow key={r.id}>
                    {isAdmin && <TableCell className="w-8"><Checkbox checked={selected.has(r.id)} onCheckedChange={() => toggleSel(r.id)} aria-label="انتخاب این رکورد" /></TableCell>}
                    <TableCell className="text-xs" dir="ltr">{gregKeyToJalali(r.date)}</TableCell>
                    <TableCell className="font-medium text-sm">{full_name(r.person)} <span className="text-[10px] text-muted-foreground">({r.person?.personnelCode})</span></TableCell>
                    <TableCell className="hidden md:table-cell text-muted-foreground text-xs">
                      {r.shift ? (
                        <span className="inline-flex items-center gap-1"><span className="h-2 w-2 rounded-full" style={{ background: boot.shifts.find((s) => s.id === r.shiftId)?.color ?? '#999' }} />{r.shift.name}</span>
                      ) : '—'}
                    </TableCell>
                    <TableCell className="text-xs">{faTime(r.firstIn)}</TableCell>
                    <TableCell className="text-xs">{faTime(r.lastOut)}</TableCell>
                    <TableCell className="text-xs font-medium">{durationFa(r.workedMinutes)}</TableCell>
                    <TableCell className="hidden sm:table-cell text-xs">{r.lateMinutes ? <span className="text-orange-600">{durationFa(r.lateMinutes)}</span> : '—'}</TableCell>
                    <TableCell className="hidden sm:table-cell text-xs">{r.overtimeMinutes ? <span className="text-teal-600">{durationFa(r.overtimeMinutes)}</span> : '—'}</TableCell>
                    <TableCell>
                      <span className={`px-2 py-0.5 rounded-full text-[11px] ${STATUS_COLOR[r.status] ?? ''}`}>{STATUS_FA[r.status] ?? r.status}</span>
                      {r.leaveMinutes > 0 && r.status !== 'LEAVE' && <div className="text-[10px] text-amber-600 mt-0.5">مرخصی {durationFa(r.leaveMinutes)}</div>}
                    </TableCell>
                  </TableRow>
                ))}
                {paged.length === 0 && <TableRow><TableCell colSpan={10} className="text-center text-muted-foreground py-10">رکوردی نیست</TableCell></TableRow>}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>

      {pages > 1 && (
        <div className="flex items-center justify-center gap-2 text-sm">
          <Button variant="outline" size="sm" disabled={page <= 1} onClick={() => setPage(page - 1)}>قبلی</Button>
          <span className="text-muted-foreground">صفحه {faDigits(page)} از {faDigits(pages)}</span>
          <Button variant="outline" size="sm" disabled={page >= pages} onClick={() => setPage(page + 1)}>بعدی</Button>
        </div>
      )}
    </div>
  )
}

function ObjectEntries({ obj }: { obj: Record<string, string> }) {
  return <>{Object.entries(obj).map(([k, v]) => <SelectItem key={k} value={k}>{v}</SelectItem>)}</>
}

/* ---------------- Phase 3 stage 6 (item 6): bulk selection bar ---------------- */
function BulkBar({ count, onClear, onEdit, onDelete, busy }: { count: number; onClear: () => void; onEdit?: () => void; onDelete?: () => void; busy?: boolean }) {
  if (count === 0) return null
  return (
    <div className="flex flex-wrap items-center gap-2 rounded-lg border bg-muted/30 px-3 py-2 text-sm sticky top-0 z-20">
      <CheckSquare className="h-4 w-4 text-primary" />
      <span className="font-medium">{faDigits(count)} رکورد انتخاب شده</span>
      <Button variant="ghost" size="sm" onClick={onClear} disabled={busy}>لغو انتخاب</Button>
      {onEdit && <Button size="sm" variant="outline" onClick={onEdit} disabled={busy} className="gap-1"><Pencil className="h-3.5 w-3.5" /> ویرایش گروهی</Button>}
      {onDelete && <Button size="sm" variant="destructive" onClick={onDelete} disabled={busy} className="gap-1"><Trash2 className="h-3.5 w-3.5" /> حذف گروهی</Button>}
      {busy && <Loader2 className="h-4 w-4 animate-spin" />}
    </div>
  )
}

function BulkReasonField({ value, onChange, placeholder }: { value: string; onChange: (v: string) => void; placeholder: string }) {
  return (
    <div className="space-y-1.5">
      <Label>دلیل (الزامی — در AuditLog ثبت می‌شود)</Label>
      <Textarea dir="rtl" value={value} onChange={(e) => onChange(e.target.value)} placeholder={placeholder} rows={2} />
    </div>
  )
}

/* ---------------- raw punches ---------------- */
function PunchesTab({ boot, user, reloadBoot, dataVersion }: { boot: Bootstrap; user: SessionUser; reloadBoot: () => void; dataVersion?: number }) {
  const [punches, setPunches] = useState<Array<{ id: string; personId: string; timestamp: string; direction: string; source: string; edited: boolean; editReason?: string | null; person?: { firstName: string; lastName: string; personnelCode: string } }>>([])
  const [total, setTotal] = useState(0)
  const [q, setQ] = useState('')
  const [source, setSource] = useState('all')
  const [page, setPage] = useState(1)
  const [loading, setLoading] = useState(false)
  const [editOpen, setEditOpen] = useState(false)
  const [editPunch, setEditPunch] = useState<{ id: string; personId: string; timestamp: string } | null>(null)
  const [editDate, setEditDate] = useState('') // Gregorian business-day key of the punch (Tehran day)
  const [editTs, setEditTs] = useState('') // HH:MM Tehran wall clock
  const [editReason, setEditReason] = useState('')
  const [addOpen, setAddOpen] = useState(false)
  const [addForm, setAddForm] = useState({ personId: '', date: tehranTodayKey(), time: '09:00', direction: 'AUTO', reason: '' })
  const [persons, setPersons] = useState<Person[]>([])
  const [saving, setSaving] = useState(false)
  const { toast } = useToast()
  const isAdmin = user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'
  const pageSize = 50
  // Phase 3 stage 3 (item 5): REAL date-range filter with presets — the range
  // is sent to /api/punches (server-side Tehran-day window), not just displayed
  const [range, setRange] = useState<RangeSel>({ preset: 'd7', ...rangeForPreset('d7') })

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const load = async () => {
    setLoading(true)
    try {
      const res = await api<{ punches: typeof punches; total: number }>(`/api/punches?from=${range.from}&to=${range.to}&q=${encodeURIComponent(q)}&source=${source === 'all' ? '' : source}&page=${page}&pageSize=${pageSize}`)
      setPunches(res.punches)
      setTotal(res.total)
    } finally { setLoading(false) }
  }
  useEffect(() => { load()   }, [q, source, page, range])
  // v1.4.3: live refresh on device-sync (no manual page reload)
  useEffect(() => { load()   }, [dataVersion])

  const saveEdit = async () => {
    if (!editPunch || !editTs) return
    setSaving(true)
    try {
      // v1.6.0 (phase 1 stage 4): Tehran wall-clock instant (was browser-local new Date)
      const ts = tehranWallClockStringToInstant(`${editDate} ${editTs}`)
      if (!ts) { toast({ title: 'زمان نامعتبر است', variant: 'destructive' }); return }
      await api('/api/punches', { method: 'PUT', body: JSON.stringify({ id: editPunch.id, timestamp: ts.toISOString(), reason: editReason }) })
      setEditOpen(false)
      load()
      toast({ title: 'تردد ویرایش شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const remove = async (id: string) => {
    if (!confirm('حذف این تردد؟ محاسبات روزانه بازمحاسبه می‌شود.')) return
    await api(`/api/punches?id=${id}`, { method: 'DELETE' })
    load()
  }

  const addPunch = async () => {
    if (!addForm.personId || !addForm.date || !addForm.time) { toast({ title: 'همه فیلدها الزامی است', variant: 'destructive' }); return }
    setSaving(true)
    try {
      // v1.6.0 (phase 1 stage 4): Tehran wall-clock instant (was browser-local new Date)
      const ts = tehranWallClockStringToInstant(`${addForm.date} ${addForm.time}`)
      if (!ts) { toast({ title: 'زمان نامعتبر است', variant: 'destructive' }); return }
      await api('/api/punches', { method: 'POST', body: JSON.stringify({ personId: addForm.personId, timestamp: ts.toISOString(), direction: addForm.direction, reason: addForm.reason }) })
      setAddOpen(false)
      load()
      toast({ title: 'تردد ثبت شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  // Phase 3 stage 6 (item 6): multi-select + bulk operations (admins only)
  const [selected, setSelected] = useState<Set<string>>(new Set())
  const [bulkOpen, setBulkOpen] = useState<'edit' | 'delete' | null>(null)
  const [bulkBusy, setBulkBusy] = useState(false)
  const [bulkDirection, setBulkDirection] = useState('AUTO')
  const [bulkNote, setBulkNote] = useState('')
  const [bulkReason, setBulkReason] = useState('')
  const toggleSel = (id: string) => setSelected((s) => { const n = new Set(s); if (n.has(id)) n.delete(id); else n.add(id); return n })
  const selAllPage = () => setSelected(new Set(punches.map((p) => p.id)))

  const runBulk = async (op: 'edit' | 'delete') => {
    const ids = [...selected]
    if (!ids.length) return
    setBulkBusy(true)
    try {
      const body = op === 'edit'
        ? { entity: 'punch', action: 'edit', ids, direction: bulkDirection === 'AUTO' && !bulkNote.trim() ? undefined : bulkDirection, editReason: bulkNote.trim() || undefined }
        : { entity: 'punch', action: 'delete', ids, reason: bulkReason }
      const res = await api<{ ok: boolean; updated?: number; deleted?: number; notFound?: string[]; recalc?: number }>('/api/bulk-ops', { method: 'POST', body: JSON.stringify(body) })
      const done = op === 'edit' ? res.updated ?? 0 : res.deleted ?? 0
      toast({
        title: op === 'edit' ? 'ویرایش گروهی انجام شد' : 'حذف گروهی انجام شد',
        description: `${faDigits(done)} رکورد${res.notFound?.length ? ` · ${faDigits(res.notFound.length)} یافت نشد` : ''}${res.recalc ? ` · ${faDigits(res.recalc)} روز بازمحاسبه شد` : ''} — در AuditLog ثبت شد.`,
      })
      setSelected(new Set())
      setBulkOpen(null)
      setBulkNote('')
      setBulkReason('')
      load()
    } catch (err) {
      toast({ title: 'خطا در عملیات گروهی', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setBulkBusy(false) }
  }
  const SOURCE_FA: Record<string, string> = { DEVICE: 'دستگاه', KIOSK: 'کیشی', QR: 'QR', WEB: 'وب', MANUAL: 'دستی', IMPORT: 'ورود داده' }
  const pages = Math.max(1, Math.ceil(total / pageSize))

  return (
    <div className="space-y-4">
      <BulkBar count={selected.size} onClear={() => setSelected(new Set())} onEdit={isAdmin ? () => setBulkOpen('edit') : undefined} onDelete={isAdmin ? () => setBulkOpen('delete') : undefined} busy={bulkBusy} />
      <div className="flex flex-wrap items-center gap-2">
        <div className="relative flex-1 min-w-44">
          <Search className="absolute right-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input placeholder="جستجو…" value={q} onChange={(e) => { setQ(e.target.value); setPage(1) }} className="pr-9" />
        </div>
        <Select value={source} onValueChange={(v) => { setSource(v); setPage(1) }}>
          <SelectTrigger className="w-36"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه منابع</SelectItem>
            {Object.entries(SOURCE_FA).map(([k, v]) => <SelectItem key={k} value={k}>{v}</SelectItem>)}
          </SelectContent>
        </Select>
        <RangePresets value={range} onChange={(r) => { setRange(r); setPage(1) }} />
        {isAdmin && <Button onClick={() => setAddOpen(true)} className="gap-1.5"><Plus className="h-4 w-4" /> ثبت دستی</Button>}
        <Button variant="outline" size="sm" onClick={load} disabled={loading}><RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} /></Button>
      </div>

      <Card>
        <CardContent className="p-0">
          <div className="max-h-[65vh] overflow-auto">
            <Table>
              <TableHeader className="sticky top-0 bg-background z-10">
                <TableRow>
                  {isAdmin && <TableHead className="w-8"><Checkbox checked={punches.length > 0 && punches.every((p) => selected.has(p.id))} onCheckedChange={(v) => (v ? selAllPage() : setSelected(new Set()))} aria-label="انتخاب همهٔ رکوردهای صفحه" /></TableHead>}
                  <TableHead>زمان</TableHead>
                  <TableHead>پرسنل</TableHead>
                  <TableHead>نوع</TableHead>
                  <TableHead>منبع</TableHead>
                  <TableHead>یادداشت</TableHead>
                  {isAdmin && <TableHead>عملیات</TableHead>}
                </TableRow>
              </TableHeader>
              <TableBody>
                {punches.map((p) => (
                  <TableRow key={p.id} data-selected={selected.has(p.id)}>
                    {isAdmin && (
                      <TableCell className="w-8"><Checkbox checked={selected.has(p.id)} onCheckedChange={() => toggleSel(p.id)} aria-label="انتخاب این رکورد" /></TableCell>
                    )}
                    <TableCell className="text-xs" dir="ltr">
                      {formatJalaliInstant(new Date(p.timestamp), { fa: true })} · {faTime(p.timestamp)}
                    </TableCell>
                    <TableCell className="font-medium text-sm">{full_name(p.person)}</TableCell>
                    <TableCell>
                      <span className={`px-2 py-0.5 rounded-full text-[11px] ${p.direction === 'IN' ? 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900 dark:text-emerald-200' : p.direction === 'OUT' ? 'bg-red-100 text-red-700 dark:bg-red-900 dark:text-red-200' : 'bg-gray-100 dark:bg-gray-800'}`}>
                        {/* Phase 3 stage 5 (item 7): honest direction display — "AUTO" is the
                            legacy vocabulary for UNDETERMINED (the device protocol reported no
                            in/out for this record). It is shown as «نامشخص» — never as a
                            fabricated ورود/خروج; the pairing layer derives pairs positionally
                            for computation without writing a direction. */}
                        {p.direction === 'IN' ? 'ورود' : p.direction === 'OUT' ? 'خروج' : <span title="این رکورد از دستگاه جهت ورود/خروج نیامده است (پروتکل آن را گزارش نمی‌دهد)؛ جهت در محاسبات از جفت‌بندی ترددها تعیین می‌شود، نه ذخیره‌شده">نامشخص</span>}
                      </span>
                    </TableCell>
                    <TableCell><Badge variant="secondary" className="text-[10px]">{SOURCE_FA[p.source] ?? p.source}</Badge></TableCell>
                    <TableCell className="text-xs text-muted-foreground max-w-40 truncate">{p.editReason ?? (p.edited ? 'ویرایش‌شده' : '')}</TableCell>
                    {isAdmin && (
                      <TableCell>
                        <div className="flex gap-1">
                          <Button variant="ghost" size="icon" className="h-7 w-7" onClick={() => {
                            const inst = new Date(p.timestamp)
                            setEditPunch(p)
                            setEditDate(markerKey(dayMarkerOf(inst)))
                            setEditTs(formatTehranTime(inst))
                            setEditReason('')
                            setEditOpen(true)
                          }}><Pencil className="h-3.5 w-3.5" /></Button>
                          <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" onClick={() => remove(p.id)}><Trash2 className="h-3.5 w-3.5" /></Button>
                        </div>
                      </TableCell>
                    )}
                  </TableRow>
                ))}
                {punches.length === 0 && <TableRow><TableCell colSpan={6} className="text-center text-muted-foreground py-10">ترددی نیست (۷ روز اخیر)</TableCell></TableRow>}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>

      {pages > 1 && (
        <div className="flex items-center justify-center gap-2 text-sm">
          <Button variant="outline" size="sm" disabled={page <= 1} onClick={() => setPage(page - 1)}>قبلی</Button>
          <span className="text-muted-foreground">{faDigits(total)} تردد — صفحه {faDigits(page)}/{faDigits(pages)}</span>
          <Button variant="outline" size="sm" disabled={page >= pages} onClick={() => setPage(page + 1)}>بعدی</Button>
        </div>
      )}

      {/* edit dialog */}
      <Dialog open={editOpen} onOpenChange={setEditOpen}>
        <DialogContent dir="rtl" className="max-w-sm">
          <DialogHeader><DialogTitle>ویرایش تردد</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="text-sm text-muted-foreground">{editPunch && full_name(persons.find((p) => p.id === editPunch.personId))}</div>
            <div className="grid grid-cols-2 gap-2">
              <div className="space-y-1.5"><Label>تاریخ (شمسی)</Label><JalaliDatePicker value={editDate} onChange={setEditDate} /></div>
              <div className="space-y-1.5"><Label>ساعت (تهران)</Label><TimeInput value={editTs} onChange={setEditTs} /></div>
            </div>
            <div className="space-y-1.5"><Label>دلیل ویرایش</Label><Input value={editReason} onChange={(e) => setEditReason(e.target.value)} placeholder="مثلاً فراموشی کارت" /></div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setEditOpen(false)}>انصراف</Button>
            <Button onClick={saveEdit} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* add dialog */}
      <Dialog open={addOpen} onOpenChange={setAddOpen}>
        <DialogContent dir="rtl" className="max-w-sm">
          <DialogHeader><DialogTitle>ثبت تردد دستی</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="space-y-1.5">
              <Label>پرسنل</Label>
              <Select value={addForm.personId} onValueChange={(v) => setAddForm({ ...addForm, personId: v })}>
                <SelectTrigger><SelectValue placeholder="انتخاب…" /></SelectTrigger>
                <SelectContent>{persons.map((p) => <SelectItem key={p.id} value={p.id}>{full_name(p)}</SelectItem>)}</SelectContent>
              </Select>
            </div>
            <div className="grid grid-cols-2 gap-2">
              <div className="space-y-1.5"><Label>تاریخ (شمسی)</Label><JalaliDatePicker value={addForm.date} onChange={(v) => setAddForm({ ...addForm, date: v })} /></div>
              <div className="space-y-1.5"><Label>ساعت (تهران)</Label><TimeInput value={addForm.time} onChange={(v) => setAddForm({ ...addForm, time: v })} /></div>
            </div>
            <div className="space-y-1.5">
              <Label>نوع</Label>
              <Select value={addForm.direction} onValueChange={(v) => setAddForm({ ...addForm, direction: v })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="AUTO">نامشخص (بدون جهت)</SelectItem><SelectItem value="IN">ورود</SelectItem><SelectItem value="OUT">خروج</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5"><Label>یادداشت</Label><Input value={addForm.reason} onChange={(e) => setAddForm({ ...addForm, reason: e.target.value })} /></div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setAddOpen(false)}>انصراف</Button>
            <Button onClick={addPunch} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ثبت</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Phase 3 stage 6 (item 6): bulk edit / bulk delete dialogs */}
      <Dialog open={bulkOpen === 'edit'} onOpenChange={(o) => { if (!o && !bulkBusy) setBulkOpen(null) }}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>ویرایش گروهی {faDigits(selected.size)} تردد</DialogTitle></DialogHeader>
          <div className="space-y-3 text-sm">
            <p className="text-xs text-muted-foreground">پیش‌نمایش: {faDigits(selected.size)} رکورد انتخاب‌شده؛ تغییرات روی همهٔ آن‌ها اعمال و در AuditLog (با مقادیر قبلی برای بازیابی) ثبت می‌شود. روزهای متأثر بازمحاسبه می‌شوند.</p>
            <div className="space-y-1.5">
              <Label>جهت (اختیاری)</Label>
              <Select value={bulkDirection} onValueChange={setBulkDirection}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>
                  <SelectItem value="AUTO">بدون تغییر جهت</SelectItem>
                  <SelectItem value="IN">ورود</SelectItem>
                  <SelectItem value="OUT">خروج</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>یادداشت ویرایش (اختیاری — روی همهٔ رکوردها ثبت می‌شود)</Label>
              <Input dir="rtl" value={bulkNote} onChange={(e) => setBulkNote(e.target.value)} placeholder="مثلاً: اصلاح گروهی بر اساس نظر مدیر" />
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setBulkOpen(null)} disabled={bulkBusy}>انصراف</Button>
            <Button onClick={() => runBulk('edit')} disabled={bulkBusy || (bulkDirection === 'AUTO' && !bulkNote.trim())} className="gap-1.5">{bulkBusy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} اجرا</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
      <Dialog open={bulkOpen === 'delete'} onOpenChange={(o) => { if (!o && !bulkBusy) setBulkOpen(null) }}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>حذف گروهی {faDigits(selected.size)} تردد</DialogTitle></DialogHeader>
          <div className="space-y-3 text-sm">
            <div className="rounded-md bg-destructive/10 border border-destructive/30 p-3 text-xs leading-5">
              {faDigits(selected.size)} رکورد عملیاتی حذف می‌شود؛ مقادیر کامل در AuditLog ذخیره می‌شود (قابل بازیابی دستی) و روزهای متأثر بازمحاسبه خواهند شد. دادهٔ خام دستگاه دست‌نخورده می‌ماند.
            </div>
            <BulkReasonField value={bulkReason} onChange={setBulkReason} placeholder="مثلاً: ترددهای ثبت‌شدهٔ اشتباهی از دستگاه دوم" />
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setBulkOpen(null)} disabled={bulkBusy}>انصراف</Button>
            <Button variant="destructive" onClick={() => runBulk('delete')} disabled={bulkBusy || !bulkReason.trim()} className="gap-1.5">{bulkBusy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} حذف نهایی</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}

/* ---------------- Phase 3 stage 3 (item 5): RawPunch (device landing zone) ----------------
 * RawPunch = the traceable DEVICE history (immutable landing zone: every record
 * the device delivered, matched or not). PunchRecord = the OPERATIONAL layer
 * the engine consumes. The two tabs are now honest about which one they show.
 * Admin-only (the API enforces ADMIN); UNMATCHED/AMBIGUOUS rows and the
 * RejectedRecord quarantine are surfaced, never hidden. */
interface RawPunchRow {
  id: string
  deviceSerial: string
  sourceProtocol: string
  enrollNumber: string | null
  punchTime: string
  punchTimeRaw: string | null
  inOutModeRaw: number
  verifyModeRaw: number
  workCode: string | null
  matchStatus: string
  matchNote: string | null
  direction: string | null
  personnelId: string | null
  deviceId: string | null
  importedAt: string
  deletedAt?: string | null
  deletedBy?: string | null
  deleteReason?: string | null
}
interface RejectedRow {
  id: string
  deviceSerial: string
  sourceProtocol: string
  enrollNumber: string | null
  punchTimeRaw: string | null
  rawPayload: string | null
  rejectReason: string
  receivedAt: string
}

const MATCH_FA: Record<string, string> = { MATCHED: 'تطبیق‌شده', UNMATCHED: 'بدون تطبیق' }
const REJECT_FA: Record<string, string> = { INVALID_TIMESTAMP: 'مهر زمانی نامعتبر' }

function RawPunchTab({ dataVersion }: { dataVersion?: number }) {
  const { toast } = useToast()
  const [range, setRange] = useState<RangeSel>({ preset: 'd7', ...rangeForPreset('d7') })
  const [view, setView] = useState<'all' | 'unmatched' | 'ambiguous' | 'rejected' | 'deleted'>('all')
  const [serial, setSerial] = useState('')
  const [rows, setRows] = useState<RawPunchRow[]>([])
  const [rejectedRows, setRejectedRows] = useState<RejectedRow[]>([])
  const [total, setTotal] = useState(0)
  const [unmatchedCount, setUnmatchedCount] = useState(0)
  const [ambiguousCount, setAmbiguousCount] = useState(0)
  const [persons, setPersons] = useState<Person[]>([])
  const [loading, setLoading] = useState(false)
  const [page, setPage] = useState(1)
  const pageSize = 50
  // Phase 3 stage 6 (item 6): bulk selection — the raw layer is IMMUTABLE (no
  // bulk edit); the only bulk operation is the TOMBSTONE delete
  const [selected, setSelected] = useState<Set<string>>(new Set())
  const [bulkOpen, setBulkOpen] = useState(false)
  const [bulkBusy, setBulkBusy] = useState(false)
  const [bulkReason, setBulkReason] = useState('')
  const toggleSel = (id: string) => setSelected((s) => { const n = new Set(s); if (n.has(id)) n.delete(id); else n.add(id); return n })

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const load = async () => {
    setLoading(true)
    try {
      const qs = `from=${range.from}&to=${range.to}&serial=${encodeURIComponent(serial.trim())}&take=${pageSize}&skip=${(page - 1) * pageSize}`
      if (view === 'rejected') {
        const res = await api<{ rejected: RejectedRow[]; total: number }>(`/api/raw-punches?rejected=1&${qs}`)
        setRejectedRows(res.rejected)
        setRows([])
        setTotal(res.total)
        setUnmatchedCount(0)
        setAmbiguousCount(0)
      } else {
        const flag = view === 'unmatched' ? '&unmatched=1' : view === 'ambiguous' ? '&ambiguous=1' : view === 'deleted' ? '&deleted=1' : ''
        const res = await api<{ rawPunches: RawPunchRow[]; total: number; unmatchedCount: number; ambiguousCount: number }>(`/api/raw-punches?${qs}${flag}`)
        setRows(res.rawPunches)
        setRejectedRows([])
        setTotal(res.total)
        setUnmatchedCount(res.unmatchedCount)
        setAmbiguousCount(res.ambiguousCount)
      }
    } catch (err) {
      toast({ title: 'خطا در دریافت ترددهای خام', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setLoading(false) }
  }
  useEffect(() => { load(); setSelected(new Set()) }, [range, view, page, serial])
  useEffect(() => { load() }, [dataVersion])

  const runBulkDelete = async () => {
    const ids = [...selected]
    if (!ids.length || !bulkReason.trim()) return
    setBulkBusy(true)
    try {
      const res = await api<{ ok: boolean; tombstoned?: number; projectedDeleted?: number; recalc?: number; notFound?: string[] }>('/api/bulk-ops', {
        method: 'POST', body: JSON.stringify({ entity: 'raw', action: 'delete', ids, reason: bulkReason }),
      })
      toast({
        title: 'حذف گروهی (Tombstone) انجام شد',
        description: `${faDigits(res.tombstoned ?? 0)} رکورد خام علامت‌خورد${res.projectedDeleted ? ` · ${faDigits(res.projectedDeleted)} تردد عملیاتی متناظر حذف شد` : ''}${res.recalc ? ` · ${faDigits(res.recalc)} روز بازمحاسبه شد` : ''} — اصل دادهٔ دستگاه حفظ شده و در AuditLog ثبت است.`,
      })
      setSelected(new Set())
      setBulkOpen(false)
      setBulkReason('')
      load()
    } catch (err) {
      toast({ title: 'خطا در حذف گروهی', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setBulkBusy(false) }
  }

  const pages = Math.max(1, Math.ceil(total / pageSize))
  const personOf = (id: string | null) => persons.find((p) => p.id === id)

  return (
    <div className="space-y-4">
      <BulkBar count={selected.size} onClear={() => setSelected(new Set())} onDelete={() => setBulkOpen(true)} busy={bulkBusy} />
      <div className="flex flex-wrap items-center gap-2">
        <Input placeholder="سریال دستگاه…" dir="ltr" value={serial} onChange={(e) => { setSerial(e.target.value); setPage(1) }} className="w-40" />
        <Select value={view} onValueChange={(v) => { setView(v as typeof view); setPage(1) }}>
          <SelectTrigger className="w-40"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه رکوردها</SelectItem>
            <SelectItem value="unmatched">بدون تطبیق پرسنل</SelectItem>
            <SelectItem value="ambiguous">تطبیق مبهم</SelectItem>
            <SelectItem value="rejected">قرنطینه (نامعتبر)</SelectItem>
            <SelectItem value="deleted">حذف‌شده‌ها (Tombstone)</SelectItem>
          </SelectContent>
        </Select>
        <RangePresets value={range} onChange={(r) => { setRange(r); setPage(1) }} />
        <Button variant="outline" size="sm" onClick={load} disabled={loading}><RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} /></Button>
      </div>

      <div className="flex flex-wrap gap-2 text-xs">
        <Badge variant="secondary">{faDigits(total)} رکورد در بازه</Badge>
        {view !== 'rejected' && <Badge variant={unmatchedCount ? 'destructive' : 'secondary'}>{faDigits(unmatchedCount)} بدون تطبیق</Badge>}
        {view !== 'rejected' && <Badge variant={ambiguousCount ? 'destructive' : 'secondary'}>{faDigits(ambiguousCount)} تطبیق مبهم</Badge>}
        <span className="text-[11px] text-muted-foreground leading-5">این بخش سابقهٔ خام و قابل‌ردیابی دستگاه است — رکوردهای بدون تطبیق حذف نمی‌شوند و برای بررسی مدیر قابل‌مشاهده‌اند. رکورد عملیاتی (لایهٔ حضور و غیاب) در تب «ترددها (عملیاتی)» است.</span>
      </div>

      <Card>
        <CardContent className="p-0">
          <div className="max-h-[65vh] overflow-auto">
            {view === 'rejected' ? (
              <Table>
                <TableHeader className="sticky top-0 bg-background z-10">
                  <TableRow>
                    <TableHead>زمان دریافت</TableHead><TableHead>سریال دستگاه</TableHead><TableHead>شماره پرسنلی دستگاه</TableHead>
                    <TableHead>علت رد</TableHead><TableHead>مهر زمانی خام</TableHead><TableHead>رد خام</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {rejectedRows.map((r) => (
                    <TableRow key={r.id}>
                      <TableCell className="text-xs whitespace-nowrap" dir="ltr">{faDigits(r.receivedAt.replace('T', ' ').slice(0, 16))}</TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.deviceSerial || '—'}</TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.enrollNumber ?? '—'}</TableCell>
                      <TableCell className="text-xs">{REJECT_FA[r.rejectReason] ?? r.rejectReason}</TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.punchTimeRaw ?? '—'}</TableCell>
                      <TableCell className="text-[10px] font-mono text-muted-foreground max-w-40 truncate" dir="ltr" title={r.rawPayload ?? ''}>{r.rawPayload ?? '—'}</TableCell>
                    </TableRow>
                  ))}
                  {rejectedRows.length === 0 && <TableRow><TableCell colSpan={6} className="text-center text-muted-foreground py-8">رکورد قرنطینه‌ای در این بازه نیست</TableCell></TableRow>}
                </TableBody>
              </Table>
            ) : (
              <Table>
                <TableHeader className="sticky top-0 bg-background z-10">
                  <TableRow>
                    <TableHead className="w-8"><Checkbox checked={rows.length > 0 && rows.every((r) => selected.has(r.id))} onCheckedChange={(v) => (v ? setSelected(new Set(rows.map((r) => r.id))) : setSelected(new Set()))} aria-label="انتخاب همهٔ رکوردهای صفحه" /></TableHead>
                    <TableHead>زمان دستگاه</TableHead><TableHead>شماره پرسنلی دستگاه</TableHead><TableHead>وضعیت تطبیق</TableHead>
                    <TableHead>پرسنل نرم‌افزار</TableHead><TableHead>پروتکل</TableHead><TableHead>مقادیر خام</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {rows.map((r) => (
                    <TableRow key={r.id} className={r.deletedAt ? 'opacity-60' : ''}>
                      <TableCell className="w-8"><Checkbox checked={selected.has(r.id)} onCheckedChange={() => toggleSel(r.id)} disabled={!!r.deletedAt} aria-label="انتخاب این رکورد" /></TableCell>
                      <TableCell className="text-xs whitespace-nowrap">{formatJalaliInstant(new Date(r.punchTime))}<span className="block text-[10px] text-muted-foreground" dir="ltr">{r.punchTimeRaw ?? ''}</span></TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.enrollNumber ?? '—'}</TableCell>
                      <TableCell>
                        <Badge variant={r.matchStatus === 'MATCHED' ? 'secondary' : 'destructive'} className="text-[11px]">{MATCH_FA[r.matchStatus] ?? r.matchStatus}</Badge>
                        {r.matchNote && <span className="block text-[10px] text-muted-foreground mt-0.5" title={r.matchNote}>{r.matchNote}</span>}
                      </TableCell>
                      <TableCell className="text-xs">{personOf(r.personnelId) ? full_name(personOf(r.personnelId)) : '—'}</TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.sourceProtocol}</TableCell>
                      <TableCell className="text-[10px] font-mono text-muted-foreground" dir="ltr">inOut={r.inOutModeRaw} verify={r.verifyModeRaw}{r.workCode ? ` work=${r.workCode}` : ''}</TableCell>
                      {r.deletedAt && (
                        <TableCell className="text-[10px] text-destructive whitespace-nowrap" colSpan={7}>حذف‌شدهٔ عملیاتی: {r.deleteReason ?? ''} · توسط {r.deletedBy ?? ''} · {faDigits(r.deletedAt.replace('T', ' ').slice(0, 16))}</TableCell>
                      )}
                    </TableRow>
                  ))}
                  {rows.length === 0 && <TableRow><TableCell colSpan={7} className="text-center text-muted-foreground py-8">رکوردی در این بازه نیست</TableCell></TableRow>}
                </TableBody>
              </Table>
            )}
          </div>
        </CardContent>
      </Card>

      {pages > 1 && (
        <div className="flex items-center justify-between text-xs">
          <Button variant="outline" size="sm" disabled={page <= 1} onClick={() => setPage(page - 1)}>قبلی</Button>
          <span>صفحهٔ {faDigits(page)} از {faDigits(pages)}</span>
          <Button variant="outline" size="sm" disabled={page >= pages} onClick={() => setPage(page + 1)}>بعدی</Button>
        </div>
      )}

      {/* Phase 3 stage 6 (item 6): tombstone bulk delete — the received data is NEVER destroyed */}
      <Dialog open={bulkOpen} onOpenChange={(o) => { if (!o && !bulkBusy) setBulkOpen(false) }}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>حذف گروهی {faDigits(selected.size)} رکورد خام</DialogTitle></DialogHeader>
          <div className="space-y-3 text-sm">
            <div className="rounded-md bg-amber-50 dark:bg-amber-950/30 border border-amber-300 dark:border-amber-800 p-3 text-xs leading-5 text-amber-800 dark:text-amber-200">
              حذف عملیاتی با سازوکار <b>Tombstone</b>: ردِ دادهٔ دریافتی از دستگاه <b>حذف نمی‌شود</b> — فقط علامت‌گذاری می‌شود (با دلیل و نام شما) و از نمای‌های زنده کنار می‌رود؛ تردد عملیاتیِ متناظر حذف و روزهای متأثر بازمحاسبه می‌شوند؛ خواندن مجدد همان رکورد از دستگاه، آن را <b>برنمی‌گرداند</b> (اثر انگشت dedupe).
            </div>
            <BulkReasonField value={bulkReason} onChange={setBulkReason} placeholder="مثلاً: رکوردهای تکراری/آزمایشی ارسال‌شده از دستگاه" />
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setBulkOpen(false)} disabled={bulkBusy}>انصراف</Button>
            <Button variant="destructive" onClick={runBulkDelete} disabled={bulkBusy || !bulkReason.trim()} className="gap-1.5">{bulkBusy ? <Loader2 className="h-4 w-4 animate-spin" /> : null} حذف عملیاتی</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
