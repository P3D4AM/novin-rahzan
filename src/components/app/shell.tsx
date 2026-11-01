'use client'

import { useEffect, useRef, useState } from 'react'
import { io, Socket } from 'socket.io-client'
import { SessionUser, Bootstrap, NotificationT, api, ROLE_FA, Role } from './types'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { useTheme } from 'next-themes'
import { Toaster } from '@/components/ui/toaster'
import { useToast } from '@/hooks/use-toast'
import {
  LayoutDashboard, Users, CalendarClock, ClipboardList, Palmtree, Clock4,
  FileBarChart, Fingerprint, Bell, LogOut, Sun, Moon, Sparkles, Settings,
  CreditCard, Monitor, Menu, X, CheckCheck,
} from 'lucide-react'

import { DashboardView } from './dashboard'
import { PersonnelView } from './personnel'
import { ShiftsView } from './shifts'
import { AttendanceView } from './attendance'
import { LeaveView, OvertimeView } from './leave'
import { ReportsView } from './reports'
import { DevicesView } from './devices'
import { AssistantView } from './assistant'
import { SettingsView } from './settings-view'
import { MyCardView } from './mycard'

export type ViewId =
  | 'dashboard' | 'personnel' | 'shifts' | 'attendance' | 'leave' | 'overtime'
  | 'reports' | 'devices' | 'assistant' | 'settings' | 'mycard'

interface NavItem {
  id: ViewId
  label: string
  icon: React.ComponentType<{ className?: string }>
  minRole?: Role
  onlyPersonnel?: boolean
}

const NAV: NavItem[] = [
  { id: 'dashboard', label: 'داشبورد زنده', icon: LayoutDashboard },
  { id: 'mycard', label: 'کارت من', icon: CreditCard, onlyPersonnel: true },
  { id: 'attendance', label: 'ترددها و کارکرد', icon: ClipboardList, minRole: 'ADMIN' },
  { id: 'personnel', label: 'پرسنل و سازمان', icon: Users, minRole: 'ADMIN' },
  { id: 'shifts', label: 'شیفت‌ها و تقویم', icon: CalendarClock, minRole: 'ADMIN' },
  { id: 'leave', label: 'مرخصی‌ها', icon: Palmtree },
  { id: 'overtime', label: 'اضافه‌کاری', icon: Clock4 },
  { id: 'reports', label: 'گزارش‌ها', icon: FileBarChart, minRole: 'ADMIN' },
  { id: 'devices', label: 'دستگاه‌ها و ورود داده', icon: Fingerprint, minRole: 'ADMIN' },
  { id: 'assistant', label: 'دستیار هوشمند', icon: Sparkles, minRole: 'APPROVER' },
  { id: 'settings', label: 'تنظیمات', icon: Settings, minRole: 'ADMIN' },
]

const RANK: Record<Role, number> = { EMPLOYEE: 0, APPROVER: 1, ADMIN: 2, SUPER_ADMIN: 3 }

export function AppShell({ user, onLogout }: { user: SessionUser; onLogout: () => void }) {
  const [view, setView] = useState<ViewId>('dashboard')
  const [boot, setBoot] = useState<Bootstrap | null>(null)
  const [bootError, setBootError] = useState('')
  const [notifications, setNotifications] = useState<NotificationT[]>([])
  const [unread, setUnread] = useState(0)
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const [liveFeed, setLiveFeed] = useState<Array<{ personName: string; direction: string; time: string; source: string }>>([])
  const socketRef = useRef<Socket | null>(null)
  const { theme, setTheme } = useTheme()
  const { toast } = useToast()

  const loadBoot = async () => {
    try {
      const b = await api<Bootstrap>('/api/bootstrap')
      setBoot(b)
      setBootError('')
    } catch (err) {
      setBootError(err instanceof Error ? err.message : 'خطا در بارگذاری')
    }
  }

  const loadNotifications = async () => {
    try {
      const res = await api<{ items: NotificationT[]; unread: number }>('/api/notifications?all=1')
      setNotifications(res.items)
      setUnread(res.unread)
    } catch { /* ignore */ }
  }

  useEffect(() => {
    const t = setTimeout(() => {
      loadBoot()
      loadNotifications()
    }, 0)
    // realtime connection — two modes:
    //  • behind the gateway (Caddy ?XTransformPort): same origin, port forwarded by the proxy
    //  • standalone install (Windows launcher / direct LAN access on :3000):
    //    talk to the realtime service directly on port 3003
    const directRT = typeof window !== 'undefined' && window.location.port === '3000'
    const rtUrl = directRT ? `http://${window.location.hostname}:3003` : '/?XTransformPort=3003'
    const socket = io(rtUrl, {
      transports: ['websocket', 'polling'],
      forceNew: true,
      reconnection: true,
      reconnectionAttempts: 10,
      reconnectionDelay: 1500,
    })
    socketRef.current = socket
    socket.on('rt', (msg: { event: string; payload: Record<string, unknown> }) => {
      if (msg.event === 'punch') {
        const time = new Intl.DateTimeFormat('fa-IR', { hour: '2-digit', minute: '2-digit', timeZone: 'Asia/Tehran' }).format(new Date(msg.payload.timestamp as string))
        setLiveFeed((prev) => [{ personName: msg.payload.personName as string, direction: msg.payload.direction as string, time, source: msg.payload.source as string }, ...prev].slice(0, 30))
      }
      if (msg.event === 'notification') {
        loadNotifications()
        toast({ title: msg.payload.title as string, description: msg.payload.body as string })
      }
      if (msg.event === 'device-sync') {
        toast({ title: 'همگام‌سازی دستگاه', description: `${msg.payload.created} تردد جدید دریافت شد` })
        loadBoot()
      }
    })
    return () => { clearTimeout(t); socket.disconnect() }
  }, [])

  const markAllRead = async () => {
    await api('/api/notifications', { method: 'PUT', body: JSON.stringify({}) })
    loadNotifications()
  }

  const navItems = NAV.filter((n) => {
    if (n.minRole && RANK[user.role] < RANK[n.minRole]) return false
    if (n.onlyPersonnel && !user.personId) return false
    return true
  })

  const viewProps = { user, boot, reloadBoot: loadBoot }

  return (
    <div className="min-h-screen flex bg-background" dir="rtl">
      {/* Sidebar */}
      <aside
        className={`fixed lg:static inset-y-0 right-0 z-40 w-64 shrink-0 bg-sidebar text-sidebar-foreground border-l border-sidebar-border flex flex-col transition-transform ${sidebarOpen ? 'translate-x-0' : 'translate-x-full lg:translate-x-0'}`}
      >
        <div className="p-4 flex items-center gap-3 border-b border-sidebar-border">
          <div className="h-10 w-10 rounded-xl bg-sidebar-primary flex items-center justify-center text-sidebar-primary-foreground font-bold text-lg">ح+</div>
          <div>
            <div className="font-bold text-base leading-tight">حضور+</div>
            <div className="text-xs text-sidebar-foreground/70">{boot?.settings?.org_name || 'سامانه حضور و غیاب'}</div>
          </div>
          <Button variant="ghost" size="icon" className="lg:hidden ml-auto text-sidebar-foreground" onClick={() => setSidebarOpen(false)}>
            <X className="h-5 w-5" />
          </Button>
        </div>

        <nav className="flex-1 overflow-y-auto p-3 space-y-1">
          {navItems.map((item) => {
            const Icon = item.icon
            const active = view === item.id
            return (
              <button
                key={item.id}
                onClick={() => { setView(item.id); setSidebarOpen(false) }}
                className={`w-full flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm transition-colors ${
                  active ? 'bg-sidebar-primary text-sidebar-primary-foreground font-semibold' : 'hover:bg-sidebar-accent text-sidebar-foreground/90'
                }`}
              >
                <Icon className="h-5 w-5 shrink-0" />
                {item.label}
              </button>
            )
          })}
        </nav>

        <div className="p-3 border-t border-sidebar-border">
          <div className="flex items-center gap-2 px-2 py-2 rounded-lg bg-sidebar-accent/50">
            <div className="h-8 w-8 rounded-full bg-sidebar-primary/40 flex items-center justify-center text-xs font-bold">
              {user.fullName.split(' ').map((x) => x[0]).slice(0, 2).join('')}
            </div>
            <div className="flex-1 min-w-0">
              <div className="text-xs font-semibold truncate">{user.fullName}</div>
              <div className="text-[10px] text-sidebar-foreground/70">{ROLE_FA[user.role]}</div>
            </div>
            <Button variant="ghost" size="icon" onClick={onLogout} className="h-8 w-8 text-sidebar-foreground hover:text-red-300" title="خروج">
              <LogOut className="h-4 w-4" />
            </Button>
          </div>
        </div>
      </aside>

      {/* Main */}
      <div className="flex-1 flex flex-col min-w-0">
        <header className="sticky top-0 z-30 h-14 border-b bg-background/80 backdrop-blur flex items-center gap-2 px-4 no-print">
          <Button variant="ghost" size="icon" className="lg:hidden" onClick={() => setSidebarOpen(true)}>
            <Menu className="h-5 w-5" />
          </Button>
          <h2 className="font-semibold text-foreground hidden sm:block">{navItems.find((n) => n.id === view)?.label}</h2>
          <div className="flex-1" />
          <div className="flex items-center gap-1.5">
            <span className="hidden md:inline-flex items-center gap-1.5 text-xs text-muted-foreground px-2">
              <span className="h-2 w-2 rounded-full bg-emerald-500 animate-pulse" />
              زنده
            </span>
            <Button variant="ghost" size="icon" onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')} title="تغییر پوسته">
              {theme === 'dark' ? <Sun className="h-5 w-5" /> : <Moon className="h-5 w-5" />}
            </Button>
            <Popover>
              <PopoverTrigger asChild>
                <Button variant="ghost" size="icon" className="relative" title="اعلان‌ها">
                  <Bell className="h-5 w-5" />
                  {unread > 0 && (
                    <span className="absolute -top-0.5 -left-0.5 h-4 min-w-4 px-0.5 rounded-full bg-destructive text-[10px] text-white flex items-center justify-center">
                      {unread > 9 ? '۹+' : String(unread).replace(/\d/g, (d) => '۰۱۲۳۴۵۶۷۸۹'[Number(d)])}
                    </span>
                  )}
                </Button>
              </PopoverTrigger>
              <PopoverContent className="w-80 p-0" align="end">
                <div className="p-3 border-b flex items-center justify-between">
                  <span className="font-semibold text-sm">اعلان‌ها</span>
                  <Button variant="ghost" size="sm" onClick={markAllRead} className="h-7 text-xs gap-1">
                    <CheckCheck className="h-3.5 w-3.5" /> خواندن همه
                  </Button>
                </div>
                <div className="max-h-96 overflow-y-auto">
                  {notifications.length === 0 && <p className="p-4 text-center text-sm text-muted-foreground">اعلانی نیست</p>}
                  {notifications.map((n) => (
                    <div key={n.id} className={`p-3 border-b text-sm ${!n.read ? 'bg-accent/50' : ''}`}>
                      <div className="font-medium">{n.title}</div>
                      <div className="text-xs text-muted-foreground mt-1">{n.body}</div>
                      <div className="text-[10px] text-muted-foreground mt-1" dir="ltr">
                        {new Intl.DateTimeFormat('fa-IR', { dateStyle: 'short', timeStyle: 'short', timeZone: 'Asia/Tehran' }).format(new Date(n.createdAt))}
                      </div>
                    </div>
                  ))}
                </div>
              </PopoverContent>
            </Popover>
            {user.personId && (
              <Button variant="outline" size="sm" className="gap-1.5" onClick={() => setView('mycard')}>
                <Monitor className="h-4 w-4" /> کارت من
              </Button>
            )}
          </div>
        </header>

        <main className="flex-1 p-4 lg:p-6 min-w-0">
          {bootError ? (
            <div className="text-center text-destructive space-y-2">
              <p>{bootError}</p>
              <Button onClick={loadBoot} variant="outline">تلاش مجدد</Button>
            </div>
          ) : !boot ? (
            <div className="flex items-center justify-center h-64 text-muted-foreground">در حال بارگذاری…</div>
          ) : (
            <>
              {view === 'dashboard' && <DashboardView {...viewProps} liveFeed={liveFeed} />}
              {view === 'personnel' && <PersonnelView {...viewProps} />}
              {view === 'shifts' && <ShiftsView {...viewProps} />}
              {view === 'attendance' && <AttendanceView {...viewProps} />}
              {view === 'leave' && <LeaveView {...viewProps} />}
              {view === 'overtime' && <OvertimeView {...viewProps} />}
              {view === 'reports' && <ReportsView {...viewProps} />}
              {view === 'devices' && <DevicesView {...viewProps} />}
              {view === 'assistant' && <AssistantView user={user} />}
              {view === 'settings' && <SettingsView {...viewProps} />}
              {view === 'mycard' && user.personId && <MyCardView user={user} onPunched={() => {}} />}
            </>
          )}
        </main>

        <footer className="mt-auto border-t py-3 px-6 text-center text-xs text-muted-foreground no-print">
          حضور+ · سامانه شخصی حضور و غیاب · نسخه ۱٫۰
        </footer>
      </div>

      {sidebarOpen && <div className="fixed inset-0 bg-black/40 z-30 lg:hidden" onClick={() => setSidebarOpen(false)} />}
      <Toaster />
    </div>
  )
}
