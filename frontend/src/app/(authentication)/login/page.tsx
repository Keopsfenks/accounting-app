'use client'

import { useState } from 'react'
import { useRouter } from 'next/navigation'
import Link from 'next/link'
import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import * as z from 'zod'

import { Button } from '@/components/ui/button'
import {
	Form,
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import {toast} from "@/hooks/use-toast";
import {http} from "@/services/HttpService";
import {LoginModel} from "@/ResponseModel/LoginModel";
import {ToastAction} from "@/components/ui/toast";

export default function LoginPage() {
	const router = useRouter()
	const [isLoading, setIsLoading] = useState(false)
	const form = useForm<z.infer<typeof formSchema>>({
		resolver: zodResolver(formSchema),
		defaultValues: {
			emailOrUsername: '',
			password: '',
		},
	})

	function onSubmit(values: z.infer<typeof formSchema>) {
		setIsLoading(true)

		// Normally, you would send this data to your server for authentication
		console.log(values)
		setTimeout(async () => {
			console.log('Logged in successfully')
			setIsLoading(false)
			const response = await http.post<LoginModel>("/Auth/Login",
				{"emailOrUsername": values.emailOrUsername, "password": values.password});
			if (response.isSuccessful) {
				// @ts-ignore
				localStorage.setItem("token", response.data.token);
				toast({
					variant: "default",
					title: "Success",
					description: "Logged in successfully",
				})
				router.push("/dashboard");
			} else if (!response.isSuccessful) {
				toast({
					variant: "destructive",
					title: "Failed trying to log in",
					description: response.errorMessages,
					action: <ToastAction altText="Try again">Try again</ToastAction>,
				})
			}
		}, 2000)
	}

	return (
		<div className="container mx-auto flex h-screen flex-col items-center justify-center">
			<div className="w-full max-w-md space-y-8">
				<div className="text-center">
					<h1 className="text-2xl font-bold">Log in to your account</h1>
					<p className="mt-2 text-sm text-gray-600">
						Don't have an account?{' '}
						<Link href="/register" className="font-medium text-blue-600 hover:text-blue-500">
							Sign up
						</Link>
					</p>
				</div>

				<Form {...form}>
					<form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
						<FormField
							control={form.control}
							name="emailOrUsername"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Email or Username</FormLabel>
									<FormControl>
										<Input placeholder="john@example.com or johndoe" {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<FormField
							control={form.control}
							name="password"
							render={({ field }) => (
								<FormItem>
									<FormLabel>Password</FormLabel>
									<FormControl>
										<Input placeholder="********" type="password" {...field} />
									</FormControl>
									<FormMessage />
								</FormItem>
							)}
						/>
						<Button type="submit" className="w-full" disabled={isLoading}>
							{isLoading ? 'Logging in...' : 'Log in'}
						</Button>
					</form>
				</Form>
			</div>
		</div>
	)
}


const formSchema = z.object({
	emailOrUsername: z.string().min(1, {
		message: 'Email or username is required.',
	}),
	password: z.string().min(1, {
		message: 'Password is required.',
	}),
})
