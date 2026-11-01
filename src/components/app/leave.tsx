'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { SessionUser, Bootstrap, api, LeaveRequestT, Person, full_name, ROLE_FA } from './types'
import { faDigits, durationFa } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { Plus, Check, X, Trash2, Loader2, Palmtree } from 'lucide-react'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

export function LeaveView({ user, boot, reloadBoot }: Props) {
  const [items, setItems] = useState<LeaveRequestT[]>([])
  const [status, setStatus] = useState('all')
  const [open, setOpen] = useState(false)
  const [saving, setSaving] = useState(false)
  const [persons, setPersons] = useState<Person[]>([])
  const isEmployee = user.role === 'EMPLOYEE'
  const canDecide = user.role === 'APPROVER' || user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'
  const { toast } = useToast()

  const [form, setForm] = useState({ personId: '', leaveTypeId: '', kind: 'DAILY', date: '', time: '08:00', endDate: '', endTime: '16:00', reason: '' })

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const load = async () => {
    const mine = isEmployee ? '&mine=1' : ''
    const res = await api<{ items: LeaveRequestT[] }>(`/api/leave/requests?status=${status === 'all' ? '' : status}${mine}`)
    setItems(res.items)
  }
  useEffect(() => { load()   }, [status])

  const openNew = () => {
    setForm({
      personId: user.personId ?? '', leaveTypeId: boot.leaveTypes[0]?.id ?? '', kind: 'DAILY',
      date: '', time: '08:00', endDate: '', endTime: '16:00', reason: '',
    })
    setOpen(true)
  }

  const submit = async () => {
    if (!form.leaveTypeId || !form.date) { toast({ title: 'نوع و تاریخ شروع الزامی است', variant: 'destructive' }); return }
    setSaving(true)
    try {
      const isHourly = form.kind === 'HOURLY'
      const start = new Date(`${form.date}T${isHourly ? form.time : '08:00'}:00`)
      const end = isHourly ? new Date(`${form.date}T${form.endTime}:00`) : new Date(`${form.endDate || form.date}T16:00:00`)
      if (end <= start) { toast({ title: 'پایان باید بعد از شروع باشد', variant: 'destructive' }); return }
      await api('/api/leave/requests', {
        method: 'POST',
        body: JSON.stringify({
          personId: user.role === 'EMPLOYEE' ? undefined : form.personId || undefined,
          leaveTypeId: form.leaveTypeId, kind: form.kind,
          startDateTime: start.toISOString(), endDateTime: end.toISOString(), reason: form.reason,
        }),
      })
      setOpen(false)
      load()
      toast({ title: 'درخواست ثبت شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const decide = async (id: string, newStatus: string) => {
    try {
      await api('/api/leave/requests', { method: 'PUT', body: JSON.stringify({ id, status: newStatus }) })
      load()
      toast({ title: newStatus === 'APPROVED' ? 'تأیید شد' : newStatus === 'REJECTED' ? 'رد شد' : 'لغو شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const remove = async (id: string) => {
    if (!confirm('حذف درخواست؟')) return
    try {
      await api(`/api/leave/requests?id=${id}`, { method: 'DELETE' })
      load()
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const STATUS_BADGE: Record<string, string> = {
    PENDING: 'bg-slate-100 text-slate-700 dark:bg-slate-800 dark:text-slate-200',
    APPROVED: 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900 dark:text-emerald-200',
    REJECTED: 'bg-red-100 text-red-700 dark:bg-red-900 dark:text-red-200',
    CANCELLED: 'bg-gray-100 text-gray-500 dark:bg-gray-800',
  }
  const STATUS_TEXT: Record<string, string> = { PENDING: 'در انتظار', APPROVED: 'تأییدشده', REJECTED: 'ردشده', CANCELLED: 'لغوشده' }

  return (
    <div className="space-y-4">
      <div className="flex flex-wrap items-center gap-2">
        <Select value={status} onValueChange={setStatus}>
          <SelectTrigger className="w-36"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه وضعیت‌ها</SelectItem>
            {Object.entries(STATUS_TEXT).map(([k, v]) => <SelectItem key={k} value={k}>{v}</SelectItem>)}
          </SelectContent>
        </Select>
        <Button onClick={openNew} className="gap-1.5"><Plus className="h-4 w-4" /> {isEmployee ? 'درخواست مرخصی' : 'ثبت مرخصی'}</Button>
        <div className="flex gap-1.5 flex-wrap">
          {boot.leaveTypes.map((t) => (
            <Badge key={t.id} variant="outline" className="gap-1">
              <span className="h-2 w-2 rounded-full" style={{ background: t.color }} />
              {t.name} {t.paid ? '(استحقاقی)' : '(بدون حقوق)'}
            </Badge>
          ))}
        </div>
      </div>

      <Card>
        <CardContent className="p-0">
          <div className="max-h-[70vh] overflow-auto">
            <Table>
              <TableHeader className="sticky top-0 bg-background z-10">
                <TableRow>
                  <TableHead>پرسنل</TableHead>
                  <TableHead>نوع</TableHead>
                  <TableHead>بازه</TableHead>
                  <TableHead>مدت</TableHead>
                  <TableHead className="hidden md:table-cell">دلیل</TableHead>
                  <TableHead>وضعیت</TableHead>
                  {(canDecide || true) && <TableHead>عملیات</TableHead>}
                </TableRow>
              </TableHeader>
              <TableBody>
                {items.map((r) => (
                  <TableRow key={r.id}>
                    <TableCell className="font-medium text-sm">{full_name(r.person)}</TableCell>
                    <TableCell>
                      <span className="inline-flex items-center gap-1.5 text-sm">
                        <span className="h-2 w-2 rounded-full" style={{ background: r.leaveType?.color }} />
                        {r.leaveType?.name}
                      </span>
                    </TableCell>
                    <TableCell className="text-xs" dir="ltr">
                      {faDigits(r.startDateTime.slice(0, 10))}{r.kind === 'HOURLY' ? ` · ${new Date(r.startDateTime).toISOString().slice(11, 16)}` : ''} → {faDigits(r.endDateTime.slice(0, 10))}
                    </TableCell>
                    <TableCell className="text-xs">{r.kind === 'DAILY' ? `${faDigits(Math.round(r.durationMinutes / 480))} روز` : durationFa(r.durationMinutes)}</TableCell>
                    <TableCell className="hidden md:table-cell text-xs text-muted-foreground max-w-40 truncate">{r.reason}</TableCell>
                    <TableCell>
                      <span className={`px-2 py-0.5 rounded-full text-[11px] ${STATUS_BADGE[r.status]}`}>{STATUS_TEXT[r.status]}</span>
                      {r.decisionNote && <div className="text-[10px] text-muted-foreground mt-0.5 max-w-36 truncate">{r.decisionNote}</div>}
                    </TableCell>
                    <TableCell>
                      <div className="flex gap-1">
                        {r.status === 'PENDING' && canDecide && (
                          <>
                            <Button variant="ghost" size="icon" className="h-7 w-7 text-emerald-600" title="تأیید" onClick={() => decide(r.id, 'APPROVED')}><Check className="h-4 w-4" /></Button>
                            <Button variant="ghost" size="icon" className="h-7 w-7 text-red-600" title="رد" onClick={() => decide(r.id, 'REJECTED')}><X className="h-4 w-4" /></Button>
                          </>
                        )}
                        {(r.status === 'PENDING' || user.role !== 'EMPLOYEE') && (
                          <Button variant="ghost" size="icon" className="h-7 w-7 text-destructive" title="حذف" onClick={() => remove(r.id)}><Trash2 className="h-3.5 w-3.5" /></Button>
                        )}
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
                {items.length === 0 && <TableRow><TableCell colSpan={7} className="text-center text-muted-foreground py-10">درخواستی نیست</TableCell></TableRow>}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent dir="rtl" className="max-w-md">
          <DialogHeader><DialogTitle className="flex items-center gap-2"><Palmtree className="h-5 w-5 text-primary" /> {isEmployee ? 'درخواست مرخصی' : 'ثبت مرخصی'}</DialogTitle></DialogHeader>
          <div className="space-y-3">
            {!isEmployee && (
              <div className="space-y-1.5">
                <Label>پرسنل</Label>
                <Select value={form.personId} onValueChange={(v) => setForm({ ...form, personId: v })}>
                  <SelectTrigger><SelectValue placeholder={user.personId ? 'خودم' : 'انتخاب…'} /></SelectTrigger>
                  <SelectContent>
                    {user.personId && <SelectItem value={user.personId}>خودم ({user.fullName})</SelectItem>}
                    {persons.map((p) => <SelectItem key={p.id} value={p.id}>{full_name(p)}</SelectItem>)}
                  </SelectContent>
                </Select>
              </div>
            )}
            <div className="space-y-1.5">
              <Label>نوع مرخصی</Label>
              <Select value={form.leaveTypeId} onValueChange={(v) => setForm({ ...form, leaveTypeId: v })}>
                <SelectTrigger><SelectValue /></SelectTrigger>
                <SelectContent>{boot.leaveTypes.map((t) => <SelectItem key={t.id} value={t.id}>{t.name}{t.paid ? ' (با حقوق)' : ' (بدون حقوق)'}</SelectItem>)}</SelectContent>
              </Select>
            </div>
            <div className="space-y-1.5">
              <Label>نوع بازه</Label>
              <div className="flex gap-2">
                <Button type="button" variant={form.kind === 'DAILY' ? 'default' : 'outline'} className="flex-1" onClick={() => setForm({ ...form, kind: 'DAILY' })}>روزانه</Button>
                <Button type="button" variant={form.kind === 'HOURLY' ? 'default' : 'outline'} className="flex-1" onClick={() => setForm({ ...form, kind: 'HOURLY' })}>ساعتی</Button>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-2">
              <div className="space-y-1.5">
                <Label>{form.kind === 'HOURLY' ? 'روز' : 'از تاریخ'}</Label>
                <input type="date" dir="ltr" value={form.date} onChange={(e) => setForm({ ...form, date: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" />
              </div>
              {form.kind === 'DAILY' ? (
                <div className="space-y-1.5">
                  <Label>تا تاریخ</Label>
                  <input type="date" dir="ltr" value={form.endDate} onChange={(e) => setForm({ ...form, endDate: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" />
                </div>
              ) : (
                <>
                  <div className="space-y-1.5">
                    <Label>از ساعت</Label>
                    <input type="time" dir="ltr" value={form.time} onChange={(e) => setForm({ ...form, time: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" />
                  </div>
                  <div className="space-y-1.5">
                    <Label>تا ساعت</Label>
                    <input type="time" dir="ltr" value={form.endTime} onChange={(e) => setForm({ ...form, endTime: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" />
                  </div>
                </>
              )}
            </div>
            <div className="space-y-1.5"><Label>دلیل</Label><Textarea value={form.reason} onChange={(e) => setForm({ ...form, reason: e.target.value })} rows={2} placeholder="مثلاً امور شخصی / بیماری" /></div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>انصراف</Button>
            <Button onClick={submit} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ثبت درخواست</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}

/* ---------------- Overtime ---------------- */
export function OvertimeView({ user, boot, reloadBoot }: Props) {
  const [items, setItems] = useState<Array<{ id: string; personId: string; date: string; minutes: number; reason?: string | null; status: string; person?: { firstName: string; lastName: string; personnelCode: string } }>>([])
  const [status, setStatus] = useState('all')
  const [open, setOpen] = useState(false)
  const [saving, setSaving] = useState(false)
  const [persons, setPersons] = useState<Person[]>([])
  const canDecide = user.role === 'APPROVER' || user.role === 'ADMIN' || user.role === 'SUPER_ADMIN'
  const isEmployee = user.role === 'EMPLOYEE'
  const { toast } = useToast()
  const [form, setForm] = useState({ personId: '', date: new Date().toISOString().slice(0, 10), hours: 2, minutes: 0, reason: '' })

  useEffect(() => { api<{ personnel: Person[] }>('/api/personnel').then((r) => setPersons(r.personnel)).catch(() => {}) }, [])

  const load = async () => {
    const res = await api<{ items: typeof items }>(`/api/overtime?status=${status === 'all' ? '' : status}`)
    setItems(res.items)
  }
  useEffect(() => { load()   }, [status])

  const submit = async () => {
    const total = Number(form.hours) * 60 + Number(form.minutes)
    if (!form.date || total <= 0) { toast({ title: 'تاریخ و مدت معتبر وارد کنید', variant: 'destructive' }); return }
    setSaving(true)
    try {
      await api('/api/overtime', {
        method: 'POST',
        body: JSON.stringify({ personId: isEmployee ? undefined : form.personId || undefined, date: form.date, minutes: total, reason: form.reason }),
      })
      setOpen(false)
      load()
      toast({ title: 'درخواست ثبت شد' })
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setSaving(false) }
  }

  const decide = async (id: string, newStatus: string) => {
    await api('/api/overtime', { method: 'PUT', body: JSON.stringify({ id, status: newStatus }) })
    load()
  }

  const STATUS_BADGE: Record<string, string> = {
    PENDING: 'bg-slate-100 text-slate-700 dark:bg-slate-800 dark:text-slate-200',
    APPROVED: 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900 dark:text-emerald-200',
    REJECTED: 'bg-red-100 text-red-700 dark:bg-red-900 dark:text-red-200',
  }
  const STATUS_TEXT: Record<string, string> = { PENDING: 'در انتظار', APPROVED: 'تأییدشده', REJECTED: 'ردشده' }

  return (
    <div className="space-y-4">
      <div className="flex flex-wrap items-center gap-2">
        <Select value={status} onValueChange={setStatus}>
          <SelectTrigger className="w-36"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه وضعیت‌ها</SelectItem>
            {Object.entries(STATUS_TEXT).map(([k, v]) => <SelectItem key={k} value={k}>{v}</SelectItem>)}
          </SelectContent>
        </Select>
        <Button onClick={() => { setForm({ ...form, personId: user.personId ?? '' }); setOpen(true) }} className="gap-1.5"><Plus className="h-4 w-4" /> ثبت اضافه‌کاری</Button>
        <div className="text-xs text-muted-foreground flex gap-3">
          <span>ضریب عادی: {faDigits(boot.settings?.ot_rate_normal ?? '1.4')}</span>
          <span>جمعه: {faDigits(boot.settings?.ot_rate_friday ?? '1.6')}</span>
          <span>تعطیل: {faDigits(boot.settings?.ot_rate_holiday ?? '2')}</span>
        </div>
      </div>

      <Card>
        <CardContent className="p-0">
          <div className="max-h-[70vh] overflow-auto">
            <Table>
              <TableHeader className="sticky top-0 bg-background z-10">
                <TableRow>
                  <TableHead>پرسنل</TableHead>
                  <TableHead>تاریخ</TableHead>
                  <TableHead>مدت</TableHead>
                  <TableHead className="hidden md:table-cell">دلیل</TableHead>
                  <TableHead>وضعیت</TableHead>
                  {canDecide && <TableHead>عملیات</TableHead>}
                </TableRow>
              </TableHeader>
              <TableBody>
                {items.map((r) => (
                  <TableRow key={r.id}>
                    <TableCell className="font-medium text-sm">{full_name(r.person)}</TableCell>
                    <TableCell className="text-xs" dir="ltr">{faDigits(r.date.slice(0, 10))}</TableCell>
                    <TableCell className="text-sm font-medium text-teal-600">{durationFa(r.minutes)}</TableCell>
                    <TableCell className="hidden md:table-cell text-xs text-muted-foreground max-w-40 truncate">{r.reason}</TableCell>
                    <TableCell><span className={`px-2 py-0.5 rounded-full text-[11px] ${STATUS_BADGE[r.status]}`}>{STATUS_TEXT[r.status]}</span></TableCell>
                    {canDecide && (
                      <TableCell>
                        {r.status === 'PENDING' && (
                          <div className="flex gap-1">
                            <Button variant="ghost" size="icon" className="h-7 w-7 text-emerald-600" onClick={() => decide(r.id, 'APPROVED')}><Check className="h-4 w-4" /></Button>
                            <Button variant="ghost" size="icon" className="h-7 w-7 text-red-600" onClick={() => decide(r.id, 'REJECTED')}><X className="h-4 w-4" /></Button>
                          </div>
                        )}
                      </TableCell>
                    )}
                  </TableRow>
                ))}
                {items.length === 0 && <TableRow><TableCell colSpan={6} className="text-center text-muted-foreground py-10">درخواستی نیست</TableCell></TableRow>}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>

      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent dir="rtl" className="max-w-sm">
          <DialogHeader><DialogTitle>ثبت درخواست اضافه‌کاری</DialogTitle></DialogHeader>
          <div className="space-y-3">
            {!isEmployee && (
              <div className="space-y-1.5">
                <Label>پرسنل</Label>
                <Select value={form.personId} onValueChange={(v) => setForm({ ...form, personId: v })}>
                  <SelectTrigger><SelectValue placeholder={user.personId ? 'خودم' : 'انتخاب…'} /></SelectTrigger>
                  <SelectContent>
                    {user.personId && <SelectItem value={user.personId}>خودم ({user.fullName})</SelectItem>}
                    {persons.map((p) => <SelectItem key={p.id} value={p.id}>{full_name(p)}</SelectItem>)}
                  </SelectContent>
                </Select>
              </div>
            )}
            <div className="grid grid-cols-2 gap-2">
              <div className="space-y-1.5"><Label>تاریخ</Label><input type="date" dir="ltr" value={form.date} onChange={(e) => setForm({ ...form, date: e.target.value })} className="w-full border rounded-md px-3 py-2 text-sm bg-background" /></div>
              <div className="space-y-1.5"><Label>مدت (ساعت)</Label><Input type="number" dir="ltr" min="0" step="0.5" value={form.hours} onChange={(e) => setForm({ ...form, hours: Number(e.target.value) })} /></div>
            </div>
            <div className="space-y-1.5"><Label>دلیل</Label><Textarea value={form.reason} onChange={(e) => setForm({ ...form, reason: e.target.value })} rows={2} /></div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={() => setOpen(false)}>انصراف</Button>
            <Button onClick={submit} disabled={saving}>{saving && <Loader2 className="h-4 w-4 animate-spin" />} ثبت</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
