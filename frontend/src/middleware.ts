// src/middleware.ts
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import { jwtDecode, JwtPayload } from "jwt-decode";

export function middleware(request: NextRequest) {
	const token = request.cookies.get("token")?.value;

	// Public routes that don't require authentication
	const publicRoutes = ['/login', '/register', '/forgot-password'];
	const isPublicRoute = publicRoutes.includes(request.nextUrl.pathname);

	// Check if the route starts with /dashboard
	const isDashboardRoute = request.nextUrl.pathname.startsWith('/dashboard');

	// If no token exists and trying to access dashboard
	if (!token && isDashboardRoute) {
		return NextResponse.redirect(new URL('/login', request.url));
	}

	// If token exists, verify it
	if (token) {
		try {
			const decoded: JwtPayload | any = jwtDecode(token);
			const exp = decoded.exp;
			const now = new Date().getTime() / 1000;

			// If token is expired
			if (!exp || now > exp) {
				if (isDashboardRoute) {
					return NextResponse.redirect(new URL('/login', request.url));
				}
			}

			// Check if user has a company
			const companyId = decoded?.CompanyId;
			const isCompanyRoute = request.nextUrl.pathname === '/dashboard/company' &&
				request.nextUrl.searchParams.get('tab') === 'company';

			if (isDashboardRoute && !companyId && !isCompanyRoute) {
				return NextResponse.redirect(new URL('/dashboard/company?tab=company', request.url));
			}

			// If token is valid and user is trying to access public routes
			if (isPublicRoute) {
				return NextResponse.redirect(new URL('/dashboard', request.url));
			}
		} catch (error) {
			// If token is invalid
			if (isDashboardRoute) {
				return NextResponse.redirect(new URL('/login', request.url));
			}
		}
	}

	return NextResponse.next();
}

// Configure which routes should be handled by the middleware
export const config = {
	matcher: [
		/*
		 * Match all request paths except for the ones starting with:
		 * - api (API routes)
		 * - _next/static (static files)
		 * - _next/image (image optimization files)
		 * - favicon.ico (favicon file)
		 */
		'/((?!api|_next/static|_next/image|favicon.ico).*)',
	],
}