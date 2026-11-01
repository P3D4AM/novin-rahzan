'use client'

import { useEffect, useMemo, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Switch } from '@/components/ui/switch'
import { Checkbox } from '@/components/ui/checkbox'
import { SessionUser, Bootstrap, api, Person, full_name } from './types'
import { MonthPicker, monthGrid, monthLabel, WEEKDAY_HEADERS, faDigits, TimeInput, JalaliDateInput, minutesToHM } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { Plus, Trash2, Pencil, Repeat, CalendarDays, Loader2, Sparkles } from 'lucide-react'
import { toGregorian } from 'jalaali-js'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

const COLORS = ['#10b981', '#f59e0b', '#8b5cf6', '#06b6d4', '#ef4444', '#ec4899', '#84cc16', '#6366f1']

export function ShiftsView({ user, boot, reloadBoot }: Props) {
  return (
    <Tabs defaultValue="shifts" dir="rtl">
      <TabsList className="mb-4">
        <TabsTrigger value="shifts">تعریف شیفت</TabsTrigger>
        <TabsTrigger value="rotations">چرخش‌های شناوری</TabsTrigger>
        <TabsTrigger value="calendar">تقویم تخصیص شیفت</TabsTrigger>
        <TabsTrigger value="holidays">تعطیلات</TabsTrigger>
      </TabsList>
      <TabsContent value="shifts"><ShiftsTab user={user} boot={boot} reloadBoot={reloadBoot} /></TabsContent>
      <TabsContent value="rotations"><RotationsTab boot={boot} reloadBoot={reloadBoot} /></TabsContent>
      <TabsContent value="calendar"><CalendarTab boot={boot} /></TabsContent>
      <TabsContent value="holidays"><HolidaysTab boot={boot} reloadBoot={reloadBoot} /></TabsContent>
    </Tabs>
  )
}

/* ---------------- Shifts CRUD ---------------- */
function ShiftsTab({ user, boot, reloadBoot }: Props) {
  const [open, setOpen] = useState(false)
  const [editing, setEditing] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)
  const { toast } = useToast()
  const isAdmin = user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'

  const blank = {
    name: '', color: COLORS[0], type: 'FIXED', lateGraceMin: 10, earlyGraceMin: 10, overtimeEligible: true,
    segments: [{ startMin: 480, endMin: 990, breakMin: 60 }],
  }
  const [form, setForm] = useState({ ...blank })

  const openNew = () => { setEditing(null); setForm({ ...blank }); setOpen(true) }
  const openEdit = (s: typeof boot.shifts[number]) => {
    setEditing(s.id)
    setForm({
      name: s.name, color: s.color, type: s.type, lateGraceMin: s.lateGraceMin, earlyGraceMin: s.earlyGraceMin,
      overtimeEligible: s.overtimeEligible, segments: s.segments.map((g) => ({ startMin: g.startMin, endMin: g.endMin, breakMin: g.breakMin })),
    })
    setOpen(true)
  }

  const save = async () => {
    if (!form.name.trim()) { toast({ title: 'نام شیفت الزامی است', variant: 'destructive' }); return }
    setSaving(true)
    try {
      if (editing) await api('/api/shifts', { method: 'PUT', body: JSON.stringify({ id: editing, ...form }) })
      else await api('/api/shifts', { method: 'POST', body: JSON.stringify(form) })
      setOpen(false)
      reloadBoot()
      toast({ title: 'ذخیره شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const remove = async (id: string) => {
    if (!confirm('حذف این شیفت؟')) return
    try {
      await api(`/api/shifts?id=${id}`, { method: 'DELETE' })
      reloadBoot()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  return (
    <div className="space-y-4">
      {isAdmin && <Button onClick={openNew} className="gap-1.5"><Plus className="h-4 w-4" /> شیفت جدید</Button>}

      <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
        {boot.shifts.map((s) => {
          const total = s.segments.reduce((a, g) => a + (g.endMin > g.startMin ? g.endMin - g.startMin : g.endMin + 1440 - g.startMin) - g.breakMin, 0)
          return (
            <Card key={s.id} className="relative overflow-hidden">
              <div className="absolute top-0 right-0 w-1.5 h-full" style={{ background: s.color }} />
              <CardHeader className="pb-2 pr-4">
                <CardTitle className="text-base flex items-center gap-2">
                  <span className="h-3 w-3 rounded-full" style={{ background: s.color }} />
                  {s.name}
                  <Badge variant="secondary" className="text-[10px]">{s.type === 'SPLIT' ? 'چندبخشی' : 'ثابت'}</Badge>
                </CardTitle>
              </CardHeader>
              <CardContent className="pr-4 space-y-2">
                <div className="text-sm space-y-1">
                  {s.segments.map((g, i) => (
                    <div key={i} className="flex justify-between text-muted-foreground">
                      <span>بازه {faDigits(i + 1)}</span>
                      <span dir="ltr" className="font-mono text-xs">{minutesToHM(g.startMin)} - {minutesToHM(g.endMin > g.startMin ? g.endMin : g.endMin)}</span>
                    </div>
                  ))}
                </div>
                <div className="flex flex-wrap gap-1.5 pt-1">
                  <Badge variant="outline" className="text-[10px]">کار مؤثر: {minutesToHM(total)}</Badge>
                  <Badge variant="outline" className="text-[10px]">تحمل تأخیر: {faDigits(s.lateGraceMin)}د</Badge>
                  {s.overtimeEligible && <Badge variant="outline" className="text-[10px]">دارای اضافه‌کاری</Badge>}
                </div>
                {isAdmin && (
                  <div className="flex gap-1 pt-2 border-t">
                    <Button variant="ghost" size="sm" onClick={() => openEdit(s)} className="gap-1 h-7"><Pencil className="h-3.5 w-3.5" /> ویرایش</Button>
                    <Button variant="ghost" size="sm" onClick={() => remove(s.id)} className="gap-1 h-7 text-destructive"><Trash2 className="h-3.5 w-3.5" /> حذف</Button>
                  </div>
                )}
              </CardContent>
            </Card>
          )
        })}
      </div>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent dir="rtl" className="max-w-lg max-h-[90vh] overflow-y-auto">
          <DialogHeader><DialogTitle>{editing ? 'ویرایش شیفت' : 'شیفت جدید'}</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="grid grid-cols-2 gap-3">
              <div className="space-y-1.5"><Label>نام شیفت *</Label><Input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="مثلاً اداری صبح" /></div>
              <div className="space-y-1.5">
                <Label>رنگ</Label>
                <div className="flex gap-1.5 flex-wrap">
                  {COLORS.map((c) => (
                    <button key={c} onClick={() => setForm({ ...form, color: c })} className={`h-7 w-7 rounded-full border-2 ${form.color === c ? 'border-foreground' : 'border-transparent'}`} style={{ background: c }} />
                  ))}
                </div>
              </div>
            </div>
            <div className="grid grid-cols-3 gap-3">
              <div className="space-y-1.5"><Label>تحمل تأخیر (دقیقه)</Label><Input type="number" dir="ltr" value={form.lateGraceMin} onChange={(e) => setForm({ ...form, lateGraceMin: Number(e.target.value) })} /></div>
              <div className="space-y-1.5"><Label>تحمل خروج زود (دقیقه)</Label><Input type="number" dir="ltr" value={form.earlyGraceMin} onChange={(e) => setForm({ ...form, earlyGraceMin: Number(e.target.value) })} /></div>
              <div className="space-y-1.5 flex items-end gap-2 pb-1">
                <Switch checked={form.overtimeEligible} onCheckedChange={(v) => setForm({ ...form, overtimeEligible: v })} id="ot" />
                <Label htmlFor="ot" className="text-xs">محاسبه اضافه‌کاری</Label>
              </div>
            </div>

            <div className="space-y-2">
              <div className="flex items-center justify-between">
                <Label>بازه‌های زمانی</Label>
                <Button variant="outline" size="sm" onClick={() => setForm({ ...form, segments: [...form.segments, { startMin: 480, endMin: 720, breakMin: 0 }] })} className="gap-1 h-7">
                  <Plus className="h-3.5 w-3.5" /> بازه جدید
                </Button>
              </div>
              {form.segments.map((seg, i) => (
                <div key={i} className="flex items-center gap-2 bg-muted/40 rounded-lg p-2">
                  <div className="flex-1"><TimeInput value={`${String(Math.floor(seg.startMin / 60)).padStart(2, '0')}:${String(seg.startMin % 60).padStart(2, '0')}`} onChange={(v) => { const [h, m] = v.split(':').map(Number); const segs = [...form.segments]; segs[i] = { ...seg, startMin: (h || 0) * 60 + (m || 0) }; setForm({ ...form, segments: segs }) }} /></div>
                  <span className="text-xs text-muted-foreground">تا</span>
                  <div className="flex-1"><TimeInput value={`${String(Math.floor(seg.endMin / 60)).padStart(2, '0')}:${String(seg.endMin % 60).padStart(2, '0')}`} onChange={(v) => { const [h, m] = v.split(':').map(Number); const segs = [...form.segments]; segs[i] = { ...seg, endMin: (h || 0) * 60 + (m || 0) }; setForm({ ...form, segments: segs }) }} /></div>
                  <div className="w-20"><Input type="number" dir="ltr" placeholder="وقفه" value={seg.breakMin} onChange={(e) => { const segs = [...form.segments]; segs[i] = { ...seg, breakMin: Number(e.target.value) }; setForm({ ...form, segments: segs }) }} className="text-xs" /></div>
                  {form.segments.length > 1 && (
                    <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" onClick={() => setForm({ ...form, segments: form.segments.filter((_, j) => j !== i) })}><Trash2 className="h-3.5 w-3.5" /></Button>
                  )}
                </div>
              ))}
              <p className="text-xs text-muted-foreground">اگر ساعت پایان کمتر از شروع باشد، شب‌گذر محسوب می‌شود (مثل شیفت امنیت شب ۲۲:۰۰ تا ۰۶:۰۰).</p>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>انصراف</Button>
            <Button onClick={save} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}

/* ---------------- Rotations ---------------- */
interface RotationApply { rotationId: string; personIds: string[]; from: string; to: string }

function RotationsTab({ boot, reloadBoot }: { boot: Bootstrap; reloadBoot: () => void }) {
  const { toast } = useToast()
  const [open, setOpen] = useState(false)
  const [saving, setSaving] = useState(false)
  const [applyOpen, setApplyOpen] = useState(false)
  const [persons, setPersons] = useState<Person[]>([])
  const [form, setForm] = useState({ name: '', steps: [{ shiftId: '', days: 2 }, { shiftId: '', days: 0 }] })
  const [apply, setApply] = useState<RotationApply>({ rotationId: '', personIds: [], from: '', to: '' })

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const save = async () => {
    if (!form.name.trim()) { toast({ title: 'نام الزامی است', variant: 'destructive' }); return }
    const pattern = form.steps.filter((s) => s.days > 0).map((s) => (s.shiftId ? { shiftId: s.shiftId, days: s.days } : { off: true, days: s.days }))
    if (!pattern.length) { toast({ title: 'حداقل یک گام لازم است', variant: 'destructive' }); return }
    setSaving(true)
    try {
      await api('/api/rotations', { method: 'PUT', body: JSON.stringify({ name: form.name, pattern }) })
      setOpen(false)
      reloadBoot()
      toast({ title: 'چرخش ذخیره شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const doApply = async () => {
    if (!apply.rotationId || !apply.personIds.length || !apply.from || !apply.to) {
      toast({ title: 'همه فیلدها را پر کنید', variant: 'destructive' }); return
    }
    setSaving(true)
    try {
      const res = await api<{ count: number }>('/api/rotations', {
        method: 'POST',
        body: JSON.stringify({ rotationId: apply.rotationId, personIds: apply.personIds, from: toIso(apply.from), to: toIso(apply.to) }),
      })
      setApplyOpen(false)
      toast({ title: 'چرخش اعمال شد', description: `${res.count} تخصیص ثبت شد` })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const parsedRotations = boot.rotations.map((r) => ({ ...r, steps: JSON.parse(r.pattern) as Array<{ shiftId?: string; off?: boolean; days: number }> }))

  return (
    <div className="space-y-4">
      <div className="flex flex-wrap gap-2">
        <Button onClick={() => setOpen(true)} className="gap-1.5"><Plus className="h-4 w-4" /> چرخش جدید</Button>
        <Button variant="outline" onClick={() => setApplyOpen(true)} className="gap-1.5"><Repeat className="h-4 w-4" /> اعمال چرخش به پرسنل</Button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {parsedRotations.map((r) => (
          <Card key={r.id}>
            <CardHeader className="pb-2"><CardTitle className="text-base flex items-center gap-2"><Repeat className="h-4 w-4 text-primary" /> {r.name}</CardTitle></CardHeader>
            <CardContent className="space-y-2">
              <div className="flex flex-wrap gap-1.5">
                {r.steps.map((s, i) => (
                  <Badge key={i} variant={s.off ? 'secondary' : 'outline'} className="text-xs gap-1">
                    {s.off ? 'استراحت' : (boot.shifts.find((x) => x.id === s.shiftId)?.name ?? '؟')}
                    <span className="text-[10px] opacity-70">{faDigits(s.days)} روز</span>
                  </Badge>
                ))}
              </div>
              <Button variant="ghost" size="sm" className="h-7 text-destructive gap-1" onClick={async () => { if (confirm('حذف؟')) { await api(`/api/rotations?id=${r.id}`, { method: 'DELETE' }); reloadBoot() } }}>
                <Trash2 className="h-3.5 w-3.5" /> حذف
              </Button>
            </CardContent>
          </Card>
        ))}
        {parsedRotations.length === 0 && <Card><CardContent className="text-center text-muted-foreground py-10">چرخشی تعریف نشده — مثال: «۳ شیفته امنیت» = ۲ روز صبح، ۲ روز عصر، ۲ روز شب، ۲ روز استراحت</CardContent></Card>}
      </div>

      {/* rotation create dialog */}
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent dir="rtl" className="max-w-lg">
          <DialogHeader><DialogTitle>چرخش شناوری جدید</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="space-y-1.5"><Label>نام چرخش *</Label><Input value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} placeholder="مثلاً ۳ شیفته امنیت" /></div>
            {form.steps.map((st, i) => (
              <div key={i} className="flex items-center gap-2 bg-muted/40 rounded-lg p-2">
                <Select value={st.shiftId || 'off'} onValueChange={(v) => { const steps = [...form.steps]; steps[i] = { ...st, shiftId: v === 'off' ? '' : v }; setForm({ ...form, steps }) }}>
                  <SelectTrigger className="flex-1"><SelectValue /></SelectTrigger>
                  <SelectContent>
                    <SelectItem value="off">استراحت (تعطیلی)</SelectItem>
                    {boot.shifts.map((s) => <SelectItem key={s.id} value={s.id}>{s.name}</SelectItem>)}
                  </SelectContent>
                </Select>
                <Input type="number" dir="ltr" className="w-20" value={st.days} onChange={(e) => { const steps = [...form.steps]; steps[i] = { ...st, days: Number(e.target.value) }; setForm({ ...form, steps }) }} />
                <span className="text-xs text-muted-foreground">روز</span>
                {form.steps.length > 2 && (
                  <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" onClick={() => setForm({ ...form, steps: form.steps.filter((_, j) => j !== i) })}><Trash2 className="h-3.5 w-3.5" /></Button>
                )}
              </div>
            ))}
            <Button variant="outline" size="sm" onClick={() => setForm({ ...form, steps: [...form.steps, { shiftId: '', days: 1 }] })} className="gap-1"><Plus className="h-3.5 w-3.5" /> گام جدید</Button>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>انصراف</Button>
            <Button onClick={save} disabled={saving}>ذخیره</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* apply dialog */}
      <Dialog open={applyOpen} onOpenChange={setApplyOpen}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle>اعمال چرخش به پرسنل</DialogTitle></DialogHeader>
          <div className="space-y-3">
            <div className="space-y-1.5">
              <Label>چرخش</Label>
              <Select value={apply.rotationId} onValueChange={(v) => setApply({ ...apply, rotationId: v })}>
                <SelectTrigger><SelectValue placeholder="انتخاب…" /></SelectTrigger>
                <SelectContent>{parsedRotations.map((r) => <SelectItem key={r.id} value={r.id}>{r.name}</SelectItem>)}</SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>از تاریخ (شمسی)</Label>
              <JalaliDateInput value={apply.from} onChange={(v) => setApply({ ...apply, from: v })} />
            </div>
            <div className="space-y-1.5">
              <Label>تا تاریخ (شمسی)</Label>
              <JalaliDateInput value={apply.to} onChange={(v) => setApply({ ...apply, to: v })} />
            </div>
            <div className="space-y-1.5">
              <Label>پرسنل ({faDigits(apply.personIds.length)} انتخاب)</Label>
              <div className="max-h-40 overflow-y-auto border rounded-lg p-2 space-y-1">
                {persons.map((p) => (
                  <label key={p.id} className="flex items-center gap-2 text-sm cursor-pointer">
                    <Checkbox checked={apply.personIds.includes(p.id)} onCheckedChange={(c) => setApply({ ...apply, personIds: c ? [...apply.personIds, p.id] : apply.personIds.filter((x) => x !== p.id) })} />
                    {full_name(p)} <span className="text-xs text-muted-foreground">({p.personnelCode})</span>
                  </label>
                ))}
              </div>
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setApplyOpen(false)}>انصراف</Button>
            <Button onClick={doApply} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} اعمال</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}

/* ---------------- Assignment calendar ---------------- */
function CalendarTab({ boot }: { boot: Bootstrap }) {
  const today = boot.today
  const [jy, setJy] = useState(today.jy)
  const [jm, setJm] = useState(today.jm)
  const [persons, setPersons] = useState<Person[]>([])
  const [assignments, setAssignments] = useState<Map<string, string>>(new Map()) // "personId|date" -> shiftId('' = off)
  const [saving, setSaving] = useState(false)
  const [selected, setSelected] = useState<{ personIds: string[]; date: string } | null>(null)
  const [assignShift, setAssignShift] = useState('keep')
  const [weekdayFilter, setWeekdayFilter] = useState<number[]>([])
  const [rangeFrom, setRangeFrom] = useState('')
  const [rangeTo, setRangeTo] = useState('')
  const { toast } = useToast()

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const load = async () => {
    const first = monthGrid(jy, jm).find(Boolean)
    const last = [...monthGrid(jy, jm)].reverse().find(Boolean)
    if (!first || !last) return
    const res = await api<{ assignments: Array<{ personId: string; date: string; shiftId: string | null }> }>(
      `/api/assignments?from=${first.gDate.toISOString()}&to=${last.gDate.toISOString()}`
    )
    const m = new Map<string, string>()
    for (const a of res.assignments) m.set(`${a.personId}|${a.date.slice(0, 10)}`, a.shiftId ?? 'off')
    setAssignments(m)
  }
  useEffect(() => { load()   }, [jy, jm])

  const cells = monthGrid(jy, jm)
  const holidaySet = new Set(boot.holidays.map((h) => h.date.slice(0, 10)))

  const bulkAssign = async () => {
    if (!selected || assignShift === 'keep') return
    setSaving(true)
    try {
      await api('/api/assignments', {
        method: 'POST',
        body: JSON.stringify({
          personIds: selected.personIds, from: selected.date, to: selected.date,
          shiftId: assignShift === 'off' ? null : assignShift,
        }),
      })
      setSelected(null)
      load()
      toast({ title: 'تخصیص ثبت شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const applyDefaultShifts = async () => {
    if (!rangeFrom || !rangeTo) { toast({ title: 'بازه تاریخ را وارد کنید', variant: 'destructive' }); return }
    setSaving(true)
    try {
      const res = await api<{ count: number }>('/api/assignments', {
        method: 'POST',
        body: JSON.stringify({ personIds: persons.map((p) => p.id), from: toIso(rangeFrom), to: toIso(rangeTo), shiftId: null, weekdays: weekdayFilter.length ? weekdayFilter : undefined }),
      })
      // null shift with no explicit shift => remove assignments so default shift applies
      toast({ title: 'انجام شد', description: `${res.count} روز پاک‌سازی شد — شیفت پیش‌فرض هر نفر اعمال می‌شود` })
      load()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  return (
    <div className="space-y-4">
      <Card>
        <CardHeader className="pb-3 flex flex-row items-center justify-between flex-wrap gap-2">
          <CardTitle className="text-base flex items-center gap-2"><CalendarDays className="h-4 w-4 text-primary" /> تقویم تخصیص شیفت — {monthLabel(jy, jm)}</CardTitle>
          <MonthPicker jy={jy} jm={jm} onChange={(y, m) => { setJy(y); setJm(m) }} />
        </CardHeader>
        <CardContent className="overflow-x-auto">
          <div className="min-w-[900px]">
            <div className="grid grid-cols-[150px_repeat(31,minmax(28px,1fr))] gap-0.5 mb-1">
              <div />
              {cells.map((c, i) => (
                <div key={i} className="text-center text-[10px] text-muted-foreground">
                  {c ? WEEKDAY_HEADERS[c.weekday] : ''}
                </div>
              ))}
            </div>
            {persons.map((p) => (
              <div key={p.id} className="grid grid-cols-[150px_repeat(31,minmax(28px,1fr))] gap-0.5 mb-0.5">
                <div className="text-xs font-medium truncate pr-2 flex items-center" title={full_name(p)}>
                  {full_name(p)}
                  <span className="text-[10px] text-muted-foreground mr-1">({p.personnelCode})</span>
                </div>
                {cells.map((c, i) => {
                  if (!c) return <div key={i} />
                  const dateStr = c.gDate.toISOString().slice(0, 10)
                  const key = `${p.id}|${dateStr}`
                  const val = assignments.get(key)
                  const isToday = today.jy === jy && today.jm === jm && today.jd === c.jd
                  const isHoliday = holidaySet.has(dateStr)
                  const isFriday = c.weekday === 6
                  const shift = val && val !== 'off' ? boot.shifts.find((s) => s.id === val) : null
                  return (
                    <button
                      key={i}
                      title={`${full_name(p)} — ${c.jd} ${monthLabel(jy, jm)}${shift ? ` — ${shift.name}` : val === 'off' ? ' — تعطیلی' : ''}`}
                      onClick={() => setSelected({ personIds: [p.id], date: dateStr })}
                      className={`h-7 rounded text-[9px] text-white hover:scale-110 hover:z-10 hover:shadow-md transition-transform relative ${isToday ? 'ring-2 ring-foreground ' : ''}`}
                      style={{ background: shift?.color ?? (val === 'off' ? '#9ca3af' : isHoliday ? '#a78bfa' : isFriday ? '#d8b4fe' : '#e5e7eb') }}
                    >
                      {shift ? '' : ''}
                    </button>
                  )
                })}
              </div>
            ))}
          </div>
          <div className="flex flex-wrap gap-2 mt-3 text-xs text-muted-foreground">
            {boot.shifts.map((s) => (
              <span key={s.id} className="inline-flex items-center gap-1"><span className="h-3 w-3 rounded" style={{ background: s.color }} /> {s.name}</span>
            ))}
            <span className="inline-flex items-center gap-1"><span className="h-3 w-3 rounded bg-gray-400" /> تعطیلی</span>
            <span className="inline-flex items-center gap-1"><span className="h-3 w-3 rounded bg-purple-400" /> تعطیل رسمی / جمعه</span>
            <span className="inline-flex items-center gap-1"><span className="h-3 w-3 rounded bg-gray-200" /> شیفت پیش‌فرض</span>
          </div>
          <p className="text-xs text-muted-foreground mt-2">روی هر خانه کلیک کنید تا شیفت آن روز تغییر کند. برای اعمال چرخش‌های چندروزه از تب «چرخش‌های شناوری» استفاده کنید.</p>
        </CardContent>
      </Card>

      <Card>
        <CardHeader className="pb-3"><CardTitle className="text-base flex items-center gap-2"><Sparkles className="h-4 w-4 text-primary" /> پاک‌سازی بازه (بازگشت به شیفت پیش‌فرض)</CardTitle></CardHeader>
        <CardContent className="flex flex-wrap items-end gap-3">
          <div className="space-y-1"><Label className="text-xs">از</Label><JalaliDateInput value={rangeFrom} onChange={setRangeFrom} /></div>
          <div className="space-y-1"><Label className="text-xs">تا</Label><JalaliDateInput value={rangeTo} onChange={setRangeTo} /></div>
          <div className="space-y-1">
            <Label className="text-xs">فقط روزهای خاص</Label>
            <div className="flex gap-1">
              {WEEKDAY_HEADERS.map((w, i) => (
                <button key={i} onClick={() => setWeekdayFilter((prev) => prev.includes(i) ? prev.filter((x) => x !== i) : [...prev, i])}
                  className={`h-7 w-7 rounded text-xs border ${weekdayFilter.includes(i) ? 'bg-primary text-primary-foreground' : 'bg-background'}`}>{w}</button>
              ))}
            </div>
          </div>
          <Button onClick={applyDefaultShifts} disabled={saving} variant="outline">{saving && <Loader2 className="h-4 w-4 animate-spin" />} اعمال</Button>
        </CardContent>
      </Card>

      {/* assign dialog */}
      <Dialog open={!!selected} onOpenChange={(o) => !o && setSelected(null)}>
        <DialogContent dir="rtl" className="max-w-xs">
          <DialogHeader><DialogTitle>تغییر شیفت این روز</DialogTitle></DialogHeader>
          {selected && (
            <div className="text-sm text-muted-foreground mb-2">
              {full_name(persons.find((p) => p.id === selected.personIds[0]))} — {faDigits(selected.date)}
            </div>
          )}
          <Select value={assignShift} onValueChange={setAssignShift}>
            <SelectTrigger><SelectValue /></SelectTrigger>
            <SelectContent>
              <SelectItem value="keep">— انتخاب کنید —</SelectItem>
              <SelectItem value="off">تعطیلی (بدون شیفت)</SelectItem>
              {boot.shifts.map((s) => <SelectItem key={s.id} value={s.id}>{s.name}</SelectItem>)}
            </SelectContent>
          </Select>
          <DialogFooter>
            <Button variant="outline" onClick={async () => { if (selected) { await api(`/api/assignments?personId=${selected.personIds[0]}&date=${selected.date}`, { method: 'DELETE' }); setSelected(null); load() } }}>حذف تخصیص</Button>
            <Button onClick={bulkAssign} disabled={saving || assignShift === 'keep'}>ثبت</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}

/* ---------------- Holidays ---------------- */
function HolidaysTab({ boot, reloadBoot }: { boot: Bootstrap; reloadBoot: () => void }) {
  const { toast } = useToast()
  const [date, setDate] = useState('')
  const [title, setTitle] = useState('')
  const [saving, setSaving] = useState(false)

  const add = async () => {
    if (!date) { toast({ title: 'تاریخ را وارد کنید', variant: 'destructive' }); return }
    setSaving(true)
    try {
      await api('/api/holidays', { method: 'POST', body: JSON.stringify({ dateJalali: date, title: title || 'تعطیل رسمی', type: 'OFFICIAL' }) })
      setDate(''); setTitle('')
      reloadBoot()
      toast({ title: 'تعطیل ثبت شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  return (
    <Card>
      <CardHeader className="pb-3"><CardTitle className="text-base">تعطیلات رسمی و سازمانی</CardTitle></CardHeader>
      <CardContent className="space-y-4">
        <div className="flex flex-wrap items-end gap-3">
          <div className="space-y-1.5"><Label className="text-xs">تاریخ (شمسی)</Label><JalaliDateInput value={date} onChange={setDate} /></div>
          <div className="space-y-1.5 flex-1 min-w-40"><Label className="text-xs">عنوان</Label><Input value={title} onChange={(e) => setTitle(e.target.value)} placeholder="مثلاً عید نوروز" /></div>
          <Button onClick={add} disabled={saving} className="gap-1.5"><Plus className="h-4 w-4" /> افزودن</Button>
        </div>
        <div className="max-h-96 overflow-y-auto">
          <Table>
            <TableHeader><TableRow><TableHead>تاریخ</TableHead><TableHead>عنوان</TableHead><TableHead>نوع</TableHead><TableHead /></TableRow></TableHeader>
            <TableBody>
              {boot.holidays.map((h) => (
                <TableRow key={h.id}>
                  <TableCell>{faDigits(h.date)}</TableCell>
                  <TableCell>{h.title}</TableCell>
                  <TableCell><Badge variant="outline">{h.type === 'OFFICIAL' ? 'رسمی' : 'سازمانی'}</Badge></TableCell>
                  <TableCell>
                    <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" onClick={async () => { if (confirm('حذف؟')) { await api(`/api/holidays?id=${h.id}`, { method: 'DELETE' }); reloadBoot() } }}>
                      <Trash2 className="h-3.5 w-3.5" />
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
              {boot.holidays.length === 0 && <TableRow><TableCell colSpan={4} className="text-center text-muted-foreground py-6">تعطیلی ثبت نشده</TableCell></TableRow>}
            </TableBody>
          </Table>
        </div>
      </CardContent>
    </Card>
  )
}

function toIso(jalali: string): string {
  const m = jalali.match(/^(\d{4})[/\-](\d{1,2})[/\-](\d{1,2})$/)
  if (!m) return ''
  const g = toGregorian(Number(m[1]), Number(m[2]), Number(m[3]))
  return new Date(Date.UTC(g.gy, g.gm - 1, g.gd)).toISOString()
}
