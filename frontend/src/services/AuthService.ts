// src/services/AuthService.ts
import { UserModel } from "@/Models/User";
import { jwtDecode, JwtPayload } from "jwt-decode";

export class AuthService {
	private static instance: AuthService;
	token: string = "";
	user: UserModel = new UserModel();

	private constructor() {
		this.isAuthenticated();
	}

	static getInstance(): AuthService {
		if (!AuthService.instance) {
			AuthService.instance = new AuthService();
		}
		return AuthService.instance;
	}

	isAuthenticated(): boolean {
		try {
			this.token = localStorage.getItem("token") ?? "";

			if (this.token === "") {
				return false;
			}

			const decode: JwtPayload | any = jwtDecode(this.token);
			const exp = decode.exp;
			const now = new Date().getTime() / 1000;

			if (!exp || now > exp) {
				this.logout();
				return false;
			}

			this.user = {
				id: decode?.Id ?? '',
				email: decode?.Email ?? '',
				fullname: decode?.Fullname ?? '',
				username: decode?.Username ?? '',
				companyId: decode?.CompanyId ?? '',
				companies: JSON.parse(decode?.Companies) ?? []
			};

			return true;
		} catch (error) {
			console.error("Auth check error:", error);
			this.logout();
			return false;
		}
	}

	hasCompany(): boolean {
		return !!this.user.companyId;
	}

	logout(): void {
		localStorage.removeItem("token");
		document.cookie = "token=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT";
		this.token = "";
		this.user = new UserModel();
	}

	getToken(): string {
		return this.token;
	}

	getUser(): UserModel {
		return this.user;
	}
}

export const authService = AuthService.getInstance();