import {UserModel} from "@/Models/User";
import {jwtDecode, JwtPayload} from "jwt-decode";

export class AuthService {
	token: string = "";
	user: UserModel = new UserModel();

	isAuthenticated(): boolean {
		this.token = localStorage.getItem("token") ?? "";

		if (this.token === "") {
			return false;
		}

		const decode: JwtPayload | any = jwtDecode(this.token);
		const exp = decode.exp;
		const now = new Date().getTime() / 1000;

		if (!exp || now > exp) {
			return false;
		}

		this.user.id = decode?.Id ?? '';
		this.user.email = decode?.Email ?? '';
		this.user.fullname = decode?.Fullname ?? '';
		this.user.username = decode?.Username ?? '';

		console.log(this.user);

		return true;
	}
}

export const authService = new AuthService();