import React, {useEffect, useState} from 'react'
import {CustomerModel} from "@/ResponseModel/CustomerModel";
import {inputChangeService} from "@/services/input-change";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import CreateEditButton from "@/components/dashboard/create-edit-button";
import {ClipboardCheck, Eye, Search} from "lucide-react";
import {Input} from "@/components/ui/input";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {DeleteButton} from "@/components/dashboard/delete-button";
import ViewButton from "@/components/dashboard/view-button";
import {
	Pagination,
	PaginationContent,
	PaginationItem,
	PaginationLink,
	PaginationNext,
	PaginationPrevious
} from "@/components/ui/pagination";
import { useRouter } from 'next/navigation'
import CustomerBodyModel from "@/Models/Customer";

export default function CustomerManagement() {
	const [customers, setCustomers] = useState<CustomerModel[]>([]);
	const [pageNumber, setPageNumber] = useState(0);
	const [newCustomer, setNewCustomer] = useState(CustomerBodyModel);
	const [searchQuery, setSearchQuery] = useState('')
	const router = useRouter();

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		console.log(addrParams)
		inputChangeService(e, setNewCustomer, addrParams);
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.post('/Customer/CreateCustomer', newCustomer)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Customer created successfully",
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
				variant: "destructive",
			})
		}
	}

	const handleUpdateSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.put('/Customer/UpdateCustomer', newCustomer)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Customer updated successfully",
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
				variant: "destructive",
			})
		}
	}

	const handleDelete = async (id: string) => {
		try {
			const response = await http.delete(`/Customer/DeleteCustomer`, {
				id: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Customer is delete successfully",
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

	useEffect(() => {
		async function getCustomers() {
			try {
				const response = await http.get<CustomerModel>("/Customer/GetAllCustomers", {
					pageNumber: pageNumber,
					pageSize: 5
				});
				if (response.data && response.isSuccessful) {

					setCustomers(prevState => {
						if (Array.isArray(response.data)) {
							return [...response.data];
						}

						console.error("Expected an array, but received:", response.data);
						return prevState;
					});

					toast({
						variant: "default",
						title: "Success",
						description: "Customers are fetched successfully",
					});
				}
			} catch (e) {
				toast({
					variant: "destructive",
					title: "Error",
					description: "An error occurred while fetching customers",
				})
			}
		}
		getCustomers().then();
	}, [pageNumber]);
	
	const filteredCustomers = customers.filter(customer => customer.name.toLowerCase().includes(searchQuery.toLowerCase()));

	useEffect(() => {
		console.log('newCustomer', newCustomer);
	}, [newCustomer]);

	return (
		<div className="space-y-4">
			<div className="flex justify-between items-center">
				<h1 className="text-2xl font-semibold">Customer Management</h1>
				<CreateEditButton name="Customer" type="Create" onSubmit={handleSubmit} formValue={[
					{name: 'name', displayName: 'name', type: "input", format: "text"},
					{name: 'customerType', displayName: 'Customer Type', type: "select", options: [
						{value: 1, name: "Individual"},
						{value: 2, name: "Company"},
						]},
					{name: 'additional', displayName: 'Addtional Information', type: "optional",
						optionalSelect: {
						name: 'additional', type: "input", inputChangeOptions: "default",
							options: [
								{name: 'email', value: 'email', format: "email"},
								{name: 'phone', value: 'phone', format: "text"},
								{name: 'address', value: 'address', format: "text"},
								{name: 'city', value: 'city', format: "text"},
								{name: 'town', value: 'town', format: "text"},
								{name: 'country', value: 'country', format: "text"},
								{name: 'zipCode', value: 'zipCode', format: "text"},
								{name: 'taxId', value: 'taxId', format: "text"},
								{name: 'taxDepartment', value: 'taxDepartment', format: "text"},
								{name: 'description', value: 'description', format: "text"},
							]}
						}
				]} handleInputChange={handleInputChange} state={setNewCustomer} model={CustomerBodyModel} />
			</div>
			<div className="relative">
				<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500" />
				<Input
					className="pl-8"
					placeholder="Find a customers..."
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)}
				/>
			</div>
			<Table>
				<TableHeader>
					<TableRow>
						<TableHead>Name / Id</TableHead>
						<TableHead>Customer Type</TableHead>
						<TableHead>Email</TableHead>
						<TableHead>Phone</TableHead>
						<TableHead>Address</TableHead>
						<TableHead>City</TableHead>
						<TableHead>Town</TableHead>
						<TableHead>County</TableHead>
						<TableHead>Zip Code</TableHead>
						<TableHead>Tax Number</TableHead>
						<TableHead>Tax Department</TableHead>
						<TableHead className="text-right">Actions</TableHead>
					</TableRow>
				</TableHeader>
				<TableBody>
					{filteredCustomers.map((customer) => (
						<TableRow
							key={customer.id}
							onClick={() => window.open(`customer/${customer.id}`, '_blank', `width=${window.screen.width}, height=${window.screen.height}`)}
							className="cursor-pointer hover:bg-gray-100">
							<TableCell>
								<div>{customer.name}</div>
								<div className="text-sm text-gray-500">#{customer.id}</div>
							</TableCell>
							<TableCell>{customer.type.name}</TableCell>
							<TableCell>{customer.email}</TableCell>
							<TableCell>{customer.phone}</TableCell>
							<TableCell>{customer.address}</TableCell>
							<TableCell>{customer.city}</TableCell>
							<TableCell>{customer.town}</TableCell>
							<TableCell>{customer.country}</TableCell>
							<TableCell>{customer.zipCode}</TableCell>
							<TableCell>{customer.taxId}</TableCell>
							<TableCell>{customer.taxDepartment}</TableCell>
							<TableCell onClick={(e) => e.stopPropagation()} className="text-right">
										<ViewButton header="Customer Detail" icon={Eye} tableHeader={["id", "processor", "description", "Operation", "Deposit", "Withdraw", "Opposite", "Date"]} deleteData={null} updateData={null} createData={null} data={
											{
												details: customer.details,
												id: ["id", "processor", "description", "type", "depositAmount", "withdrawalAmount", "opposite", "date"],
												keysToCompare: ["id", "processor", "description", "type", "depositAmount", "withdrawalAmount", "opposite", "date"],
												filter: {}
											}
										} />

										<ViewButton header="Invoice Detail" icon={ClipboardCheck} tableHeader={
											["Id", "Invoice Number", "Personal Tax Number", "Personal Name", "Sub Total", "Tax Amount", "Total", "Status", "Operation", "Issue Date", "Due Date", "Details"]
										} deleteData={null} updateData={null} createData={null} data={
											{
												details: customer.invoices,
												id: ["id", "invoiceNumber", "personalTaxNumber", "personalName", "subTotal", "taxAmount", "totalAmount", "status", "type", "issueDate", "dueDate", "details"],
												keysToCompare: ["id", "invoiceNumber", "personalTaxNumber", "personalName", "subTotal", "taxAmount", "totalAmount", "status", "type", "issueDate", "dueDate", "details"],
												filter: {}
											}
										} />

										<CreateEditButton name="Customer" type="Edit" id={[{name: "id", value: customer.id}]} onSubmit={handleUpdateSubmit} formValue={[
											{name: 'name', displayName: 'name', type: "input", format: "text"},
											{name: 'customerType', displayName: 'Customer Type', type: "select", options: [
													{value: 1, name: "Individual"},
													{value: 2, name: "Company"},
												]},
											{name: 'additional', displayName: 'Addtional Information', type: "optional",
												optionalSelect: {
													name: 'additional', type: "input", inputChangeOptions: "default",
													options: [
														{name: 'email', value: 'email', format: "email"},
														{name: 'phone', value: 'phone', format: "text"},
														{name: 'address', value: 'address', format: "text"},
														{name: 'city', value: 'city', format: "text"},
														{name: 'town', value: 'town', format: "text"},
														{name: 'country', value: 'country', format: "text"},
														{name: 'zipCode', value: 'zipCode', format: "text"},
														{name: 'taxId', value: 'taxId', format: "text"},
														{name: 'taxDepartment', value: 'taxDepartment', format: "text"},
														{name: 'description', value: 'description', format: "text"},
													]}
											}
										]} handleInputChange={handleInputChange} state={setNewCustomer} model={CustomerBodyModel} />
										<DeleteButton onDelete={async () => await handleDelete(customer.id)} description={"This action cannot be undone. This will permanently delete the customer and remove all associated data."}/>
							</TableCell>
						</TableRow>
					))}
				</TableBody>
			</Table>
			<Pagination>
				<PaginationContent>
					<PaginationItem onClick={() => {
						if (pageNumber > 0) {
							setPageNumber(pageNumber - 1)
						}
					}}>
						<PaginationPrevious/>
					</PaginationItem>
					<PaginationItem>
						<PaginationLink>{pageNumber}</PaginationLink>
					</PaginationItem>
					<PaginationItem>
						<PaginationNext onClick={() => {
							if (customers.length === 5)
								setPageNumber(pageNumber + 1)
						}} />
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</div>
	)
}
