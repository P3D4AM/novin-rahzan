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
import { RefreshCw, Pencil, Search, Loader2, Trash2, Plus } from 'lucide-react'

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
      <TabsContent value="daily"><DailyTab boot={boot} dataVersion={dataVersion} /></TabsContent>
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

function DailyTab({ boot, dataVersion }: { boot: Bootstrap; dataVersion?: number }) {
  const today = boot.today
  const [from, setFrom] = useState(tehranDaysAgoKey(6))
  const [to, setTo] = useState(tehranTodayKey())
  const [dept, setDept] = useState('all')
  const [status, setStatus] = useState('all')
  const [q, setQ] = useState('')
  const [records, setRecords] = useState<DailyRecordT[]>([])
  const [loading, setLoading] = useState(false)
  const [page, setPage] = useState(1)
  const pageSize = 100

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
        <Button variant="outline" size="sm" onClick={load} disabled={loading}><RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} /></Button>
      </div>

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
                {paged.length === 0 && <TableRow><TableCell colSpan={9} className="text-center text-muted-foreground py-10">رکوردی نیست</TableCell></TableRow>}
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

  const SOURCE_FA: Record<string, string> = { DEVICE: 'دستگاه', KIOSK: 'کیشی', QR: 'QR', WEB: 'وب', MANUAL: 'دستی', IMPORT: 'ورود داده' }
  const pages = Math.max(1, Math.ceil(total / pageSize))

  return (
    <div className="space-y-4">
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
                  <TableRow key={p.id}>
                    <TableCell className="text-xs" dir="ltr">
                      {formatJalaliInstant(new Date(p.timestamp), { fa: true })} · {faTime(p.timestamp)}
                    </TableCell>
                    <TableCell className="font-medium text-sm">{full_name(p.person)}</TableCell>
                    <TableCell>
                      <span className={`px-2 py-0.5 rounded-full text-[11px] ${p.direction === 'IN' ? 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900 dark:text-emerald-200' : p.direction === 'OUT' ? 'bg-red-100 text-red-700 dark:bg-red-900 dark:text-red-200' : 'bg-gray-100 dark:bg-gray-800'}`}>
                        {p.direction === 'IN' ? 'ورود' : p.direction === 'OUT' ? 'خروج' : 'خودکار'}
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
                  <SelectItem value="AUTO">خودکار</SelectItem><SelectItem value="IN">ورود</SelectItem><SelectItem value="OUT">خروج</SelectItem>
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
  const [view, setView] = useState<'all' | 'unmatched' | 'ambiguous' | 'rejected'>('all')
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
        const flag = view === 'unmatched' ? '&unmatched=1' : view === 'ambiguous' ? '&ambiguous=1' : ''
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
  useEffect(() => { load() }, [range, view, page, serial])
  useEffect(() => { load() }, [dataVersion])

  const pages = Math.max(1, Math.ceil(total / pageSize))
  const personOf = (id: string | null) => persons.find((p) => p.id === id)

  return (
    <div className="space-y-4">
      <div className="flex flex-wrap items-center gap-2">
        <Input placeholder="سریال دستگاه…" dir="ltr" value={serial} onChange={(e) => { setSerial(e.target.value); setPage(1) }} className="w-40" />
        <Select value={view} onValueChange={(v) => { setView(v as typeof view); setPage(1) }}>
          <SelectTrigger className="w-40"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه رکوردها</SelectItem>
            <SelectItem value="unmatched">بدون تطبیق پرسنل</SelectItem>
            <SelectItem value="ambiguous">تطبیق مبهم</SelectItem>
            <SelectItem value="rejected">قرنطینه (نامعتبر)</SelectItem>
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
                    <TableHead>زمان دستگاه</TableHead><TableHead>شماره پرسنلی دستگاه</TableHead><TableHead>وضعیت تطبیق</TableHead>
                    <TableHead>پرسنل نرم‌افزار</TableHead><TableHead>پروتکل</TableHead><TableHead>مقادیر خام</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {rows.map((r) => (
                    <TableRow key={r.id}>
                      <TableCell className="text-xs whitespace-nowrap">{formatJalaliInstant(new Date(r.punchTime))}<span className="block text-[10px] text-muted-foreground" dir="ltr">{r.punchTimeRaw ?? ''}</span></TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.enrollNumber ?? '—'}</TableCell>
                      <TableCell>
                        <Badge variant={r.matchStatus === 'MATCHED' ? 'secondary' : 'destructive'} className="text-[11px]">{MATCH_FA[r.matchStatus] ?? r.matchStatus}</Badge>
                        {r.matchNote && <span className="block text-[10px] text-muted-foreground mt-0.5" title={r.matchNote}>{r.matchNote}</span>}
                      </TableCell>
                      <TableCell className="text-xs">{personOf(r.personnelId) ? full_name(personOf(r.personnelId)) : '—'}</TableCell>
                      <TableCell className="text-xs" dir="ltr">{r.sourceProtocol}</TableCell>
                      <TableCell className="text-[10px] font-mono text-muted-foreground" dir="ltr">inOut={r.inOutModeRaw} verify={r.verifyModeRaw}{r.workCode ? ` work=${r.workCode}` : ''}</TableCell>
                    </TableRow>
                  ))}
                  {rows.length === 0 && <TableRow><TableCell colSpan={6} className="text-center text-muted-foreground py-8">رکوردی در این بازه نیست</TableCell></TableRow>}
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
    </div>
  )
}
