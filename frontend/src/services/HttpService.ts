import axios, {AxiosInstance} from "axios";
import {authService} from "@/services/AuthService";

const address = "http://localhost:5242/api";

class ResultModel<T>{
	data?: T;
	errorMessages?: string[];
	isSuccessful: boolean = false;
}

export class ApiServices {
	private http: AxiosInstance;
	constructor() {
		this.http = axios.create({
			baseURL: address,
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${localStorage.getItem(authService.token)}`,
			}
		});
	}
	async get(apiUrl: string, params?: object): Promise<object> {
		try {
			const response = await this.http.get(apiUrl, {
				params: params
			});

			if (response.data) {
				return response.data;
			}

			return ResultModel<never>;
		} catch (error) {
			if (axios.isAxiosError(error)) {
				console.error("Hata:", error.message);
			}
			throw error;
		}
	}

	async post<T>(apiUrl: string, body: object): Promise<ResultModel<T>> {
		try {
			const response = await this.http.post<ResultModel<T>>(apiUrl, body);
			return response.data;
		} catch (error) {
			if (axios.isAxiosError(error)) {

				console.error("Hata:", error.message);
			}
			throw error;
		}
	}
}

export const http = new ApiServices();