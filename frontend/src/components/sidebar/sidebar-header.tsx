import React, {ChangeEvent, FormEvent, useEffect, useState} from 'react'
import { SidebarMenu, SidebarMenuButton, SidebarMenuItem, useSidebar } from "@/components/ui/sidebar"
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuShortcut,
	DropdownMenuTrigger
} from "@/components/ui/dropdown-menu"
import { ChevronsUpDown, Command, Plus, X } from 'lucide-react'
import { Company } from "@/Models/Company"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {ToastAction} from "@/components/ui/toast";

export default function CompanyHeader({
										  companies,
									  }: {
	companies: Company[]
}) {
	const [activeCompany, setActiveCompany] = useState(companies[0])
	const [addCompany, setAddCompany] = useState(false)
	const { isMobile } = useSidebar()

	const [newCompany, setNewCompany] = useState({
		name: "",
		address: "",
		taxDepartment: "",
		taxId: ""
	})

	useEffect(() => {
		if (!addCompany) {
			setNewCompany({
				name: "",
				address: "",
				taxDepartment: "",
				taxId: ""
			})
		}
	}, [addCompany])

	const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
		const { name, value } = e.target
		setNewCompany(prev => ({ ...prev, [name]: value }))
	}

	async function onAddCompany(newCompany: any) {
		const response = await http.post("/Company/CreateCompany", {
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
		});
		if (response.isSuccessful) {
			toast({
				variant: "default",
				title: "Success",
				description: "Company added successfully",
			})
		} else {
			toast({
				variant: "destructive",
				title: "Failed trying to company add",
				description: response.errorMessages,
				action: <ToastAction altText="Try again">Try again</ToastAction>,
			})
		}
	}

	const handleSubmit = (e: FormEvent) => {
		e.preventDefault();
		onAddCompany(newCompany).finally();
		setAddCompany(false);
	}

	return (
		<SidebarMenu>
			<SidebarMenuItem>
				<DropdownMenu>
					<DropdownMenuTrigger asChild>
						<SidebarMenuButton
							size="lg"
							className="data-[state=open]:bg-sidebar-accent data-[state=open]:text-sidebar-accent-foreground">
							<div className="flex aspect-square size-8 items-center justify-center rounded-lg bg-sidebar-primary text-sidebar-primary-foreground">
								{<Command className="size-4" />}
							</div>
							<div className="grid flex-1 text-left text-sm leading-tight">
								<span className="truncate font-semibold">{activeCompany.Name}</span>
								<span className="truncate text-xs">Role: {activeCompany.Role}</span>
							</div>
							<ChevronsUpDown className="ml-auto" />
						</SidebarMenuButton>
					</DropdownMenuTrigger>
					<DropdownMenuContent
						className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
						align="start"
						side={isMobile ? "bottom" : "right"}
						sideOffset={4}>
						<DropdownMenuLabel>
							{companies.map((company, index) => (
								<DropdownMenuItem
									key={company.Id}
									onClick={() => setActiveCompany(company)}
									className="gap-2 p-2">
									<div className="flex size-6 items-center justify-center rounded-sm border">
										{<Command className="size-4 shrink-0" />}
									</div>
									{company.Name}
									<DropdownMenuShortcut>CTRL{index + 1}</DropdownMenuShortcut>
								</DropdownMenuItem>
							))}
							<DropdownMenuSeparator />
							<DropdownMenuItem className="gap-2 p-2" onClick={() => setAddCompany(true)}>
								<div
									className="flex size-6 items-center justify-center rounded-md border bg-background">
									<Plus className="size-4"/>
								</div>
								<div className="font-medium text-muted-foreground">Add Company</div>
							</DropdownMenuItem>
						</DropdownMenuLabel>
					</DropdownMenuContent>
				</DropdownMenu>
			</SidebarMenuItem>

			{addCompany && (
				<div className="fixed inset-0 z-50 bg-background/80 backdrop-blur-sm flex items-center justify-center">
					<Card className="w-full max-w-lg">
						<CardHeader>
							<CardTitle className="flex items-center justify-between">
								Add New Company
								<Button variant="ghost" size="icon" onClick={() => setAddCompany(false)}>
									<X className="h-4 w-4" />
								</Button>
							</CardTitle>
						</CardHeader>
						<form onSubmit={handleSubmit}>
							<CardContent className="space-y-4">
								<div className="space-y-2">
									<Label htmlFor="name">Company Name</Label>
									<Input
										id="name"
										name="name"
										value={newCompany.name}
										onChange={handleInputChange}
										required
									/>
								</div>
								<div className="space-y-2">
									<Label htmlFor="address">Address</Label>
									<Input
										id="address"
										name="address"
										value={newCompany.address}
										onChange={handleInputChange}
										required
									/>
								</div>
								<div className="space-y-2">
									<Label htmlFor="taxDepartment">Tax Department</Label>
									<Input
										id="taxDepartment"
										name="taxDepartment"
										value={newCompany.taxDepartment}
										onChange={handleInputChange}
										required
									/>
								</div>
								<div className="space-y-2">
									<Label htmlFor="taxId">Tax ID</Label>
									<Input
										id="taxId"
										name="taxId"
										value={newCompany.taxId}
										onChange={handleInputChange}
										required
									/>
								</div>
							</CardContent>
							<CardFooter>
								<Button type="submit" className="w-full">Add Company</Button>
							</CardFooter>
						</form>
					</Card>
				</div>
			)}
		</SidebarMenu>
	)
}

