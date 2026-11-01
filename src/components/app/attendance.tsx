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
import { JalaliDateInput, MonthPicker, faTime, faDigits, durationFa, monthLabel, toG, monthGrid } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { RefreshCw, Pencil, Search, Loader2, Trash2, Plus } from 'lucide-react'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

export function AttendanceView({ user, boot, reloadBoot }: Props) {
  return (
    <Tabs defaultValue="daily" dir="rtl">
      <TabsList className="mb-4">
        <TabsTrigger value="daily">کارکرد روزانه</TabsTrigger>
        <TabsTrigger value="punches">ترددهای خام</TabsTrigger>
      </TabsList>
      <TabsContent value="daily"><DailyTab boot={boot} /></TabsContent>
      <TabsContent value="punches"><PunchesTab boot={boot} user={user} reloadBoot={reloadBoot} /></TabsContent>
    </Tabs>
  )
}

/* ---------------- daily attendance ---------------- */
function isoToday() { return new Date().toISOString().slice(0, 10) }

function DailyTab({ boot }: { boot: Bootstrap }) {
  const today = boot.today
  const [from, setFrom] = useState(new Date(Date.now() - 6 * 86400000).toISOString().slice(0, 10))
  const [to, setTo] = useState(isoToday())
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
          <input type="date" dir="ltr" value={from} onChange={(e) => setFrom(e.target.value)} className="border rounded-md px-2 py-1.5 bg-background" />
          <span className="text-muted-foreground">تا</span>
          <input type="date" dir="ltr" value={to} onChange={(e) => setTo(e.target.value)} className="border rounded-md px-2 py-1.5 bg-background" />
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
                    <TableCell className="text-xs" dir="ltr">{faDigits(r.date)}</TableCell>
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
function PunchesTab({ boot, user, reloadBoot }: { boot: Bootstrap; user: SessionUser; reloadBoot: () => void }) {
  const [punches, setPunches] = useState<Array<{ id: string; personId: string; timestamp: string; direction: string; source: string; edited: boolean; editReason?: string | null; person?: { firstName: string; lastName: string; personnelCode: string } }>>([])
  const [total, setTotal] = useState(0)
  const [q, setQ] = useState('')
  const [source, setSource] = useState('all')
  const [page, setPage] = useState(1)
  const [loading, setLoading] = useState(false)
  const [editOpen, setEditOpen] = useState(false)
  const [editPunch, setEditPunch] = useState<{ id: string; personId: string; timestamp: string } | null>(null)
  const [editTs, setEditTs] = useState('')
  const [editReason, setEditReason] = useState('')
  const [addOpen, setAddOpen] = useState(false)
  const [addForm, setAddForm] = useState({ personId: '', date: isoToday(), time: '09:00', direction: 'AUTO', reason: '' })
  const [persons, setPersons] = useState<Person[]>([])
  const [saving, setSaving] = useState(false)
  const { toast } = useToast()
  const isAdmin = user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'
  const pageSize = 50

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const load = async () => {
    setLoading(true)
    try {
      const from = new Date(Date.now() - 7 * 86400000).toISOString().slice(0, 10)
      const res = await api<{ punches: typeof punches; total: number }>(`/api/punches?from=${from}&to=${isoToday()}&q=${encodeURIComponent(q)}&source=${source === 'all' ? '' : source}&page=${page}&pageSize=${pageSize}`)
      setPunches(res.punches)
      setTotal(res.total)
    } finally { setLoading(false) }
  }
  useEffect(() => { load()   }, [q, source, page])

  const saveEdit = async () => {
    if (!editPunch || !editTs) return
    setSaving(true)
    try {
      await api('/api/punches', { method: 'PUT', body: JSON.stringify({ id: editPunch.id, timestamp: new Date(editTs).toISOString(), reason: editReason }) })
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
      const ts = new Date(`${addForm.date}T${addForm.time}:00`)
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
                      {faDigits(p.timestamp.slice(0, 10))} · {faTime(p.timestamp)}
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
                          <Button variant="ghost" size="icon" className="h-7 w-7" onClick={() => { setEditPunch(p); setEditTs(p.timestamp.slice(0, 16)); setEditReason(''); setEditOpen(true) }}><Pencil className="h-3.5 w-3.5" /></Button>
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
            <div className="space-y-1.5">
              <Label>زمان جدید</Label>
              <input type="datetime-local" dir="ltr" value={editTs} onChange={(e) => setEditTs(e.target.value)} className="w-full border rounded-md px-3 py-2 text-sm bg-background" />
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
              <div className="space-y-1.5"><Label>تاریخ</Label><input type="date" dir="ltr" value={addForm.date} onChange={(e) => setAddForm({ ...addForm, date: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" /></div>
              <div className="space-y-1.5"><Label>ساعت</Label><input type="time" dir="ltr" value={addForm.time} onChange={(e) => setAddForm({ ...addForm, time: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" /></div>
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
