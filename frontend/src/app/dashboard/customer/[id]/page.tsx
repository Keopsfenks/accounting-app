'use client'

import React, {Usable, use, useEffect, useState} from "react";
import {CustomerModel} from "@/ResponseModel/CustomerModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"
import { ScrollArea } from "@/components/ui/scroll-area"
import {
	ChevronLeft,
	MessageSquare,
	MapPin,
	HardDriveDownload, Edit, Search, CalendarIcon, ChevronUp, ChevronDown,
} from 'lucide-react'
import {Tooltip, TooltipContent, TooltipProvider, TooltipTrigger} from "@/components/ui/tooltip";
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import { cn } from "@/lib/utils";
import {format} from "date-fns";
import {tr} from "date-fns/locale";

import {DeleteButton} from "@/components/dashboard/delete-button";

import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectLabel,
	SelectTrigger,
	SelectValue
} from "@/components/ui/select"
import {Card} from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {useCompanies} from "@/Context/CompanyContext";
import {InvoiceForm} from "@/components/dashboard/invoice-form";
import {Calendar} from "@/components/ui/calendar";
import {ExpandedInvoicePanel} from "@/components/dashboard/invoice-detail";
import {ProductModel} from "@/ResponseModel/ProductModel";
import {SalesDetail} from "@/components/dashboard/sales-detail";
import {SalesForm} from "@/components/dashboard/sales-form";


interface Params {
	id: string;
}



export default function CustomerPage({ params }: {
	params: Usable<Params>
}) {
	const { id } = use(params);
	const [customer, setCustomer] = useState<CustomerModel>({
		id: "",
		name: "",
		type: {
			name: "",
			value: 1,
		},

		description: null,
		email: null,
		phone: null,
		address: null,
		city: null,
		town: null,
		country: null,
		zipCode: null,
		taxId: null,
		taxDepartment: null,

		details: [],
		invoices: [],

		deposit: 0,
		withdrawal: 0,
		debit: 0,
	});
	const [searchQueryInvoice, setSearchQueryInvoice] = useState('')
	const [searchQueryDetail, setSearchQueryDetail] = useState('')
	const [date, setDate] = useState<Date | undefined>()
	const [activeView, setActiveView] = useState('invoices');
	const companies = useCompanies();
	const [expandedRow, setExpandedRow] = useState<string | null>(null);
	const [products, setProducts] = useState<ProductModel[]>([]);
	const toggleRow = (id: string) => {
		setExpandedRow(expandedRow === id ? null : id);
	};

	useEffect(() => {
		async function fetchCustomerDetail() {
			try {
				const response = await http.get<CustomerModel>('/Customer/GetIdToCustomer', {
					id: id
				})

				if (response.data && response.isSuccessful) {
					setCustomer(response.data);

					toast({
						variant: "default",
						title: "Success",
						description: "Customer are fetched successfully",
					});
				}
			} catch (error) {
				toast({
					variant: "destructive",
					title: "Error",
					description: "An error occurred while fetching customers",
				});
			}
		}
		fetchCustomerDetail().then();
	}, []);

	const filteredDetails = customer.details.filter((detail) =>
		detail.depositAmount.toString().includes(searchQueryDetail) ||
		detail.withdrawalAmount.toString().includes(searchQueryDetail) ||
		detail.issueDate.toString().toLowerCase().includes(searchQueryDetail.toLowerCase()) ||
		detail.operationType.name.toString().toLowerCase().includes(searchQueryDetail.toLowerCase()));


	const filteredInvoices = customer.invoices.filter((invoice) =>
		invoice.invoiceNumber.toString().includes(searchQueryInvoice)
		|| invoice.dueDate?.toString().toLowerCase().includes(searchQueryInvoice.toLowerCase())
		|| invoice.issueDate.toString().toLowerCase().includes(searchQueryInvoice.toLowerCase())
		|| invoice.status.name.toString().toLowerCase().includes(searchQueryInvoice.toLowerCase())
		|| invoice.type.name.toString().toLowerCase().includes(searchQueryInvoice.toLowerCase()));

	useEffect(() => {
		console.log(customer);
	}, [customer]);

	useEffect(() => {
		console.log(date);
	}, [date]);

	useEffect(() => {
		async function fetchProducts() {
			try {
				const response = await http.get<ProductModel>("/Product/GetAllProducts")

				if (response.data && response.isSuccessful) {
					setProducts(response.data as unknown as ProductModel[]);
				} else if (response.errorMessages) {
					toast({
						title: "Error",
						description: response.errorMessages,
						variant: "destructive",
					})
				}
			} catch (error: any) {
				toast({
					title: "Error",
					description: error.message,
					variant: "destructive",
				})
			}
		}
		fetchProducts().then();
	}, []);


	const handleDelete = async (id: string, endpoint: string, name: string) => {
		console.log(id)
		try {
			const response = await http.delete(endpoint, {
				id: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: {name} + " delete successfully",
				})
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

	function parseDate(dateString: string) {
		const months = [
			"Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran",
			"Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"
		];

		const [day, monthName, year] = dateString.split(" ");
		const month = months.indexOf(monthName); // Türkçe ay ismini buluyor
		return new Date(`${year}-${month + 1}-${day}`);
	}

	return (
		<div className="min-h-screen bg-gray-50">
			<div className="container mx-auto p-4">
				{/* Header */}
				<header className="flex justify-start gap-1 items-center mb-6">
					<Button variant="ghost" size="icon">
						<ChevronLeft className="h-4 w-4" />
					</Button>
					<h1 className="text-xl font-semibold">Customer Operations</h1>
				</header>

				<div className="grid lg:grid-cols-[300px,1fr] gap-6">
					{/* Sidebar */}
					<div className="space-y-6">
						{/* Profile Card */}
						<Card className="overflow-hidden">
							<div
								className="relative h-40 bg-gradient-to-r from-indigo-700 to-indigo-950 font-bold flex items-center justify-center text-white">
								{customer.type.name}
							</div>
							<div className="p-6 pt-6">
								<h2 className="text-xl font-semibold break-words">{customer.name}</h2>
								<p className="text-gray-500 text-[0.650rem] break-words">{customer.id}</p>
								<div className="mt-3">
									<h4 className="text-sm font-bold">Contact Information:</h4>
									<p className="text-sm break-words"><span>Email:</span> {customer.email}</p>
									<p className="text-sm break-words">Phone: {customer.phone}</p>
								</div>
								<div className="flex gap-2 mt-4 flex-wrap">
									<Button className="flex-1" onClick={() => window.location.href = `mailto:${customer.email}`}>
										<MessageSquare className="h-4 w-4 mr-2" />
										Send Mail
									</Button>
									<TooltipProvider>
										<Tooltip>
											<TooltipTrigger asChild>
												<Button variant="outline" size="icon">
													<HardDriveDownload className="h-4 w-4" />
												</Button>
											</TooltipTrigger>
											<TooltipContent>Download Data</TooltipContent>
										</Tooltip>
									</TooltipProvider>
									<TooltipProvider>
										<Tooltip>
											<TooltipTrigger asChild>
												<Button variant="outline" size="icon">
													<Edit className="h-4 w-4" />
												</Button>
											</TooltipTrigger>
											<TooltipContent>Edit Customer</TooltipContent>
										</Tooltip>
									</TooltipProvider>
								</div>
							</div>
						</Card>
						{/* Notes */}
						<Card className="p-4">
							<h3 className="font-semibold mb-2">Description</h3>
							<p className="text-sm text-gray-500">
								{customer.description}
							</p>
						</Card>

						{/* Address */}
						<Card className="p-4">
							<h3 className="font-semibold mb-2">Address</h3>
							<p className="text-sm text-gray-500">{customer.address}</p>
							<p className="text-sm text-gray-500">{customer.town}/{customer.city} {customer.country}</p>
							<Button
								variant="outline"
								className="w-full mt-2"
								onClick={() => {
									const address = encodeURIComponent(customer.address ?? "");
									const town = encodeURIComponent(customer.town ?? "");
									const city = encodeURIComponent(customer.city ?? "");
									const url = `https://www.google.com/maps?q=${address}+${town}+${city}`;
									window.open(url, "_blank");
								}}
							>
								<MapPin className="h-4 w-4 mr-2" />
								View map
							</Button>
						</Card>


						{/* Attachments */}
						<Card className="p-4">
							<div className="flex justify-between items-center mb-4">
								<h3 className="font-semibold">Additional Information</h3>
							</div>
							<div className="space-y-2">
								<h4 className="text-sm font-semibold">Tax ID:</h4>
								<p className="text-sm text-gray-500">{customer.taxId}</p>
								<h4 className="text-sm font-semibold">Tax Department:</h4>
								<p className="text-sm text-gray-500">{customer.taxDepartment}</p>
								<h4 className="text-sm font-semibold">Zipcode:</h4>
								<p className="text-sm text-gray-500">{customer.zipCode}</p>
							</div>
						</Card>
					</div>

					{/* Main Content */}
					<div className="space-y-6">
						{/* Search and Filters */}
						<div className="flex gap-4 relative">
							<div className="flex items-center space-x-2 w-[28rem] max-w-md min-w-min">
								<Search className="text-gray-500"/>
								<Input
									placeholder={activeView === "invoices" ? "Search Invoices..." : "Search Details..."}
									className="flex-1"  // Ensure Input takes remaining space
									value={activeView === "invoices" ? searchQueryInvoice : searchQueryDetail}
									onChange={(e) => activeView === "invoices" ? setSearchQueryInvoice(e.target.value) : setSearchQueryDetail(e.target.value)}
								/>
							</div>
							<Select onValueChange={(value) => activeView === "invoices"
								? setSearchQueryInvoice(value) : setSearchQueryDetail(value)}>
								<SelectTrigger className="w-[180px]">
									<SelectValue placeholder="Draft"/>
								</SelectTrigger>
								<SelectContent>
									<SelectGroup>
										<SelectLabel>Operation Type</SelectLabel>
										<SelectItem value="Purchase">Purchase</SelectItem>
										<SelectItem value="Selling">Selling</SelectItem>
									</SelectGroup>
									{activeView === "invoices" ? (
										<SelectGroup>
											<SelectLabel>Invoice Status</SelectLabel>
											<SelectItem value="Draft">Draft</SelectItem>
											<SelectItem value="Approved">Sent</SelectItem>
											<SelectItem value="Paid">Paid</SelectItem>
											<SelectItem value="Cancelled">Cancelled</SelectItem>
										</SelectGroup> ) : null}
								</SelectContent>

							</Select>
							<Popover>
								<PopoverTrigger asChild>
									<Button
										variant={"outline"}
										className={cn(
											"w-[240px] justify-start text-left font-normal bg-[#f9fafb]",
											!date && "text-muted-foreground"
										)}
									>
										<CalendarIcon/>
										{date ? format(date, "PPP") : <span>Pick a date</span>}
									</Button>
								</PopoverTrigger>
								<PopoverContent className="w-auto p-0 bg-[#f9fafb]" align="start">
									<Calendar
										mode="single"
										selected={date}
										onSelect={(date) => {
											const formattedDate = date ? format(date, "d MMMM yyyy", { locale: tr }) : "";

											if (activeView === "invoices") {
												setSearchQueryInvoice(formattedDate);
											} else {
												setSearchQueryDetail(formattedDate);
											}
										}}
									/>
								</PopoverContent>
							</Popover>
							<div className="grid grid-cols-2 space-x-2">
								<InvoiceForm customer={customer} companies={companies} products={products} />
								<SalesForm customer={customer} companies={companies} products={products} />

							</div>
						</div>

						{/* Statistics */}
						<div className="grid grid-cols-1 md:grid-cols-4 gap-4">
							<Card className="p-4">
								<div className="text-sm text-gray-500">Operation Total</div>
								<div className="text-2xl font-semibold">{activeView == "invoices" ? (
									filteredInvoices.length
								) : (
									filteredDetails.length
								)}</div>
							</Card>
							<Card className="p-4 bg-green-50">
								<div className="text-sm text-green-700">Deposit Amount</div>
								<div className="text-2xl font-semibold text-green-700">{customer.deposit}</div>
							</Card>
							<Card className="p-4 bg-red-50">
								<div className="text-sm text-red-700">Withdraw Amount</div>
								<div className="text-2xl font-semibold text-red-700">{customer.withdrawal}</div>
							</Card>
							<Card className="p-4 bg-yellow-50">
								<div className="text-sm text-yellow-700">Debit Amount</div>
								<div className="text-2xl font-semibold text-yellow-700">{customer.debit}</div>
							</Card>
						</div>

						<div>
							<Select onValueChange={(value) => setActiveView(value)} defaultValue="invoices">
								<SelectTrigger
									className="flex justify-center gap-2 h-12 bg-gradient-to-r from-indigo-700 to-indigo-950 text-white font-bold text-center">
									<SelectValue placeholder="Select view"/>
								</SelectTrigger>
								<SelectContent className="bg-gradient-to-r from-indigo-700 to-indigo-950 text-white">
									<SelectItem
										className="flex justify-center hover:bg-indigo-950 focus:bg-indigo-950 focus:text-white"
										value="invoices">Invoices</SelectItem>
									<SelectItem
										className="flex justify-center hover:bg-indigo-950 focus:bg-indigo-950 focus:text-white"
										value="details">Details</SelectItem>
								</SelectContent>
							</Select>
						</div>

						{/* Invoices Table */}
						<Card>
							<ScrollArea className="h-[600px]">
								{activeView === "invoices" ? (
									<Table>
										<TableHeader>
											<TableRow>
												<TableHead>Invoice Number</TableHead>
												<TableHead>Status</TableHead>
												<TableHead>Operation Type</TableHead>
												<TableHead>Issue Date</TableHead>
												<TableHead>Due Date</TableHead>
												<TableHead className="text-right">Actions</TableHead>
											</TableRow>
										</TableHeader>
										<TableBody>
											{filteredInvoices.map((invoice) => (
												<React.Fragment key={invoice.id}>
													<TableRow
														className={`cursor-pointer hover:bg-gray-100 dark:hover:bg-gray-700 ${
															invoice.dueDate && (
																parseDate(invoice.dueDate) < new Date() ? "bg-red-100 dark:bg-red-700" : ""
															)
														}`}
														onClick={() => toggleRow(invoice.id)}
													>
														<TableCell>{invoice.invoiceNumber}</TableCell>
														<TableCell><StatusBadge status={invoice.status}/></TableCell>
														<TableCell><TypeBadge type={invoice.type}/></TableCell>
														<TableCell>{invoice.issueDate}</TableCell>
														<TableCell>{invoice.dueDate}</TableCell>
														<TableCell className="text-right">
															<div className="flex items-center justify-end">
																<DeleteButton
																	onDelete={async () => handleDelete(invoice.id, "/Invoice/DeleteInvoice", "Invoice")}
																	description="This action cannot be undone. This will permanently delete the invoice and remove all associated data."
																/>
																{expandedRow === invoice.id ? <ChevronUp className="ml-2" /> : <ChevronDown className="ml-2" />}
															</div>
														</TableCell>
													</TableRow>
													<TableRow>
														<TableCell colSpan={6} className="p-0">
															<ExpandedInvoicePanel
																invoice={invoice}
																companies={companies}
																isExpanded={expandedRow === invoice.id}
																products={products}
															/>
														</TableCell>
													</TableRow>
												</React.Fragment>
											))}
										</TableBody>
									</Table>
								) : <Table>
									<TableHeader>
										<TableRow>
											<TableHead>Description</TableHead>
											<TableHead>Status</TableHead>
											<TableHead>Operation Type</TableHead>
											<TableHead>Issue Date</TableHead>
											<TableHead>Due Date</TableHead>
											<TableHead className="text-right">Actions</TableHead>
										</TableRow>
									</TableHeader>
									<TableBody>
										{filteredDetails.map((detail) => (
											<React.Fragment key={detail.id}>
												<TableRow
													className={`cursor-pointer hover:bg-gray-100 dark:hover:bg-gray-700 ${
														detail.dueDate && (
															parseDate(detail.dueDate) < new Date() ? "bg-red-100 dark:bg-red-700" : ""
														)
													}`}
													onClick={() => toggleRow(detail.id)}
												>
													<TableCell>{detail.description}</TableCell>
													<TableCell><StatusBadge status={detail.totalAmount === 0 ? {name: "Paid", value: 1} : {name: "Unpaid", value: 2}}/></TableCell>
													<TableCell><TypeBadge type={detail.operationType}/></TableCell>
													<TableCell>{detail.issueDate}</TableCell>
													<TableCell>{detail.dueDate}</TableCell>
													<TableCell className="text-right">
														<div className="flex items-center justify-end">
															<DeleteButton
																onDelete={async () => handleDelete(detail.id, "/CustomerDetail/DeleteCustomerDetail", "Transaction")}
																description="This action cannot be undone. This will permanently delete the transaction and remove all associated data."
															/>
															{expandedRow === detail.id ? <ChevronUp className="ml-2" /> : <ChevronDown className="ml-2" />}
														</div>
													</TableCell>
												</TableRow>
												<TableRow>
													<TableCell colSpan={6} className="p-0">
														<SalesDetail
															detail={detail}
															companies={companies}
															isExpanded={expandedRow === detail.id}
															products={products}
														/>
													</TableCell>
												</TableRow>
											</React.Fragment>
										))}
									</TableBody>
								</Table>}
							</ScrollArea>
						</Card>
					</div>
				</div>
			</div>
		</div>
	)
}

function StatusBadge({status}: { status: { name: string; value: number } }) {
	const statusStyles = {
		1: "bg-green-100 text-green-800",
		2: "bg-red-100 text-red-800",
	}

	return (
		<Badge variant="secondary" className={statusStyles[status.value as keyof typeof statusStyles]}>
			{status.name.charAt(0).toUpperCase() + status.name.slice(1)}
		</Badge>
	)
}

function TypeBadge({type}: { type: { name: string; value: number } }) {
	const typeStyles = {
		1: "bg-green-100 text-green-800",
		2: "bg-red-100 text-red-800",
	}

	return (
		<Badge variant="secondary" className={typeStyles[type.value as keyof typeof typeStyles]}>
			{type.name.charAt(0).toUpperCase() + type.name.slice(1)}
		</Badge>
	)
}