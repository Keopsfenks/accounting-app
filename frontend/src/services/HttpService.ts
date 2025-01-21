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
		authService.isAuthenticated();
		this.http = axios.create({
			baseURL: address,
			headers: {
				"Content-Type": "application/json",
				Authorization: `Bearer ${authService.token}`,
			}
		});
	}
	async get<T>(apiUrl: string, params?: object): Promise<ResultModel<T>> {
		try {
			const response = await this.http.get<ResultModel<T>>(apiUrl, {
				params: params
			});

			return response.data;
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
	async put<T>(apiUrl: string, body: object): Promise<ResultModel<T>> {
		try {
			const response = await this.http.put<ResultModel<T>>(apiUrl, body);

			return response.data;
		} catch (error) {
			if (axios.isAxiosError(error)) {
				console.error("Hata:", error.message);
			}
			throw error;
		}
	}
	async delete<T>(apiUrl: string, body?: object): Promise<ResultModel<T>> {
		try {
			const response = await this.http.delete<ResultModel<T>>(apiUrl, {
				data: body
			});

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