'use client'
import React, {useEffect, useState} from 'react'
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableHeader,
	TableRow,
} from "@/components/ui/table"

import {Search} from 'lucide-react'
import { useCompanies } from "@/Context/CompanyContext"
import { toast } from "@/hooks/use-toast"
import { http } from "@/services/HttpService"
import {DeleteButton} from "@/components/dashboard/delete-button";
import CreateEditButton from "@/components/dashboard/create-edit-button";
import {inputChangeService} from "@/services/input-change";
import CompanyBodyModel from "@/Models/Company";
import {LoginModel} from "@/ResponseModel/LoginModel";

export default function CompanyManagement() {
	const companies = useCompanies()
	const [searchQuery, setSearchQuery] = useState('')
	const [newCompany, setNewCompany] = useState(CompanyBodyModel)


	const filteredCompanies = companies.filter(company =>
		company.Name.toLowerCase().includes(searchQuery.toLowerCase()) ||
		company.TaxNumber.toLowerCase().includes(searchQuery.toLowerCase())
	)

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		inputChangeService(e, setNewCompany, addrParams);
	}


	const changeCompany = async (id: string) => {
		try {
			const response = await http.post<LoginModel>(`/Auth/ChangeCompany`, { companyId: null });

			if (response.data && response.isSuccessful) {
				toast({
					title: "Success",
					description: "Page changed successfully",
				})
				// @ts-ignore
				localStorage.setItem("token", response.data.token);
				document.cookie = `token=${response.data.token}; path=/; max-age=86400`; // 24 saat
				window.location.reload();
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}
		} catch (err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive"
			});
		}
	}

	const handleDeleteCompany = async (id: string) => {
		try {
			const response = await http.delete(`/Company/DeleteCompany`, {
				companyId: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Page deleted successfully",
				})
				await changeCompany("");
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}
		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive"
			});
		}
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.post('/Company/CreateCompany', {
				name: newCompany.name,
				address: newCompany.address,
				taxDepartment: newCompany.taxDepartment,
				taxId: newCompany.taxId,
				database: {
					server: "GURBUZ",
					databaseName: newCompany.name + newCompany.taxId + "DB",
					userId: "",
					password: ""
				}
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Company created successfully",
				})
				await changeCompany("");
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}

		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive",
			})
		}
	}


	const handleUpdateSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.put('/Company/UpdateCompany', {
				id: newCompany.id,
				name: newCompany.name,
				address: newCompany.address,
				taxDepartment: newCompany.taxDepartment,
				taxId: newCompany.taxId,
				database: {
					server: "GURBUZ",
					databaseName: newCompany.name + newCompany.taxId + "DB",
					userId: "",
					password: ""
				}
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Page created successfully",
				})
				await changeCompany("");
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}

		} catch(err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive",
			})
		}
	}

	return (
		<div className="space-y-4">
			<div className="flex justify-between items-center">
				<h1 className="text-2xl font-bold">Companies</h1>
				<CreateEditButton name={"Company"} type={"Create"} onSubmit={handleSubmit} formValue={
					[
						{name: "name", displayName: "Name", type: "input", options: []},
						{name: "taxId", displayName: "Tax Number", format: "number", type: "input", options: []},
						{name: "taxDepartment", displayName: "Tax Department", type: "input", options: []},
						{name: "address", displayName: "Address", type: "input", options: []}
					]
				} handleInputChange={handleInputChange} state={setNewCompany} model={CompanyBodyModel} />
			</div>
			<div className="relative">
				<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500" />
				<Input
					className="pl-8"
					placeholder="Find a company..."
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)}
				/>
			</div>

			<Table>
				<TableHeader>
					<TableRow>
						<TableHead>Company Name / Id</TableHead>
						<TableHead>Tax Number</TableHead>
						<TableHead>Tax Department</TableHead>
						<TableHead>Address</TableHead>
						<TableHead>Role</TableHead>
						<TableHead className="text-right">Actions</TableHead>
					</TableRow>
				</TableHeader>
				<TableBody>
					{filteredCompanies.map((company) => (
						<TableRow key={company.Id}>
							<TableCell>
								<div>{company.Name}</div>
								<div className="text-sm text-gray-500">#{company.Id}</div>
							</TableCell>
							<TableCell>{company.TaxNumber}</TableCell>
							<TableCell>{company.TaxDepartment}</TableCell>
							<TableCell>{company.Address}</TableCell>
							<TableCell>{company.Role}</TableCell>
							<TableCell className="text-right">
									<CreateEditButton name={"Company"} id={[{name: "id", value: company.Id}]} type={"Edit"} onSubmit={handleUpdateSubmit} formValue={
										[
											{name: "name", displayName: "Name", type: "input", options: []},
											{name: "taxId", displayName: "Tax Number", format: "number", type: "input", options: []},
											{name: "taxDepartment", displayName: "Tax Department", type: "input", options: []},
											{name: "address", displayName: "Address", type: "input", options: []}
										]
									} handleInputChange={handleInputChange} state={setNewCompany} model={CompanyBodyModel} />
									<DeleteButton
										onDelete={async () => { await handleDeleteCompany(company.Id) }}
										description="This action cannot be undone. This will permanently delete the company and remove all associated data."
									/>
							</TableCell>
						</TableRow>
					))}
				</TableBody>
			</Table>

			<div className="flex justify-between items-center">
				<Button variant="outline" disabled>Previous</Button>
				<div>Page 1</div>
				<Button variant="outline" disabled>Next</Button>
			</div>
		</div>
	)
}
