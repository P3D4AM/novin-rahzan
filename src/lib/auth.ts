import { randomBytes, scryptSync, timingSafeEqual } from 'crypto'
import { cookies } from 'next/headers'
import { db } from '@/lib/db'

export const SESSION_COOKIE = 'hozorplus_session'
const SESSION_DAYS = 7

export type Role = 'SUPER_ADMIN' | 'ADMIN' | 'APPROVER' | 'EMPLOYEE'

export interface SessionUser {
  id: string
  username: string
  role: Role
  fullName: string
  personId: string | null
}

export function hashPassword(password: string): string {
  const salt = randomBytes(16).toString('hex')
  const hash = scryptSync(password, salt, 64).toString('hex')
  return `${salt}:${hash}`
}

export function verifyPassword(password: string, stored: string): boolean {
  const [salt, hash] = stored.split(':')
  if (!salt || !hash) return false
  const candidate = scryptSync(password, salt, 64)
  const original = Buffer.from(hash, 'hex')
  return candidate.length === original.length && timingSafeEqual(candidate, original)
}

export async function createSession(userId: string): Promise<string> {
  const token = randomBytes(32).toString('hex')
  const expiresAt = new Date(Date.now() + SESSION_DAYS * 24 * 60 * 60 * 1000)
  await db.session.create({ data: { token, userId, expiresAt } })
  return token
}

export async function destroySession(token: string) {
  await db.session.deleteMany({ where: { token } })
}

export async function getSessionUser(): Promise<SessionUser | null> {
  const cookieStore = await cookies()
  const token = cookieStore.get(SESSION_COOKIE)?.value
  if (!token) return null
  const session = await db.session.findUnique({
    where: { token },
    include: { user: true },
  })
  if (!session) return null
  if (session.expiresAt < new Date()) {
    await db.session.delete({ where: { id: session.id } }).catch(() => {})
    return null
  }
  if (!session.user.active) return null
  return {
    id: session.user.id,
    username: session.user.username,
    role: session.user.role as Role,
    fullName: session.user.fullName,
    personId: session.user.personnelId,
  }
}

const RANK: Record<Role, number> = { EMPLOYEE: 0, APPROVER: 1, ADMIN: 2, SUPER_ADMIN: 3 }

export function hasRole(user: SessionUser | null, min: Role): boolean {
  if (!user) return false
  return RANK[user.role] >= RANK[min]
}

/** API guard: returns user or throws an http error helper object */
export async function requireRole(min: Role): Promise<SessionUser> {
  const user = await getSessionUser()
  if (!user) throw new ApiError(401, 'ابتدا وارد شوید')
  if (!hasRole(user, min)) throw new ApiError(403, 'دسترسی لازم را ندارید')
  return user
}

export async function requireAuth(): Promise<SessionUser> {
  const user = await getSessionUser()
  if (!user) throw new ApiError(401, 'ابتدا وارد شوید')
  return user
}

export class ApiError extends Error {
  status: number
  constructor(status: number, message: string) {
    super(message)
    this.status = status
  }
}
