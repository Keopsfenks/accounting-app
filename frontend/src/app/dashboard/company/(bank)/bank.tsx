import React, {useEffect, useState} from 'react'
import {BankModel} from "@/ResponseModel/BankModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {CashRegisterModel} from "@/ResponseModel/CashRegisterModel";
import CreateEditButton from "@/components/dashboard/create-edit-button";
import {Search} from "lucide-react";
import {Input} from "@/components/ui/input";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import ViewButton from "@/components/dashboard/view-button";
import {DeleteButton} from "@/components/dashboard/delete-button";
import {
	Pagination,
	PaginationContent,
	PaginationItem,
	PaginationLink,
	PaginationNext,
	PaginationPrevious
} from "@/components/ui/pagination";
import {inputChangeService} from "@/services/input-change";
import BankBodyModel from "@/Models/Bank";



export default function Bank() {
	const [banks, setBanks] = useState<BankModel[]>([]);
	const [cashRegisters, setCashRegisters] = useState<CashRegisterModel[]>([]);
	const [pageNumber, setPageNumber] = useState(0)
	const [newBank, setNewBank] = useState(BankBodyModel);
	const [searchQuery, setSearchQuery] = useState('')

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		inputChangeService(e, setNewBank, addrParams);
	}


	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.post('/Bank/CreateBank', newBank)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Bank created successfully",
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
			const response = await http.put('/Bank/UpdateBank', newBank)
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Bank updated successfully",
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
			const response = await http.delete(`/Bank/BankDelete`, {
				id: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Bank delete successfully",
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
		async function getBanks() {
			try {
				const response = await http.get<BankModel>("/Bank/GetAllBanks", {
					pageNumber: pageNumber,
					pageSize: 5
				});
				if (response.data && response.isSuccessful) {

					setBanks(prevState => {
						if (Array.isArray(response.data)) {
							return [...response.data];
						}

						console.error("Expected an array, but received:", response.data);
						return prevState;
					});

					toast({
						variant: "default",
						title: "Success",
						description: "Banks are fetched successfully",
					});
				}
			} catch (e) {
				toast({
					variant: "destructive",
					title: "Error",
					description: "An error occurred while fetching banks",
				})
			}
		}
		getBanks().then();
	}, [pageNumber]);

	useEffect(() => {
		async function getCashRegisters() {
			try {
				const response = await http.get<CashRegisterModel>("/CashRegister/GetAllCashRegister", {
					pageNumber: pageNumber,
					pageSize: 5
				});
				if (response.data && response.isSuccessful) {

					setCashRegisters(prevState => {
						if (Array.isArray(response.data)) {
							return [...response.data];
						}

						console.error("Expected an array, but received:", response.data);
						return prevState;
					});

					toast({
						variant: "default",
						title: "Success",
						description: "Cash Registers are fetched successfully",
					});
				}
			} catch (e) {
				toast({
					variant: "destructive",
					title: "Error",
					description: "An error occurred while fetching cash registers",
				})
			}
		}
		getCashRegisters().then();
	}, []);

	const filteredCashRegisters = banks.filter(bank =>
		bank.name.toLowerCase().includes(searchQuery.toLowerCase())
	)

	return (
		<div className="space-y-4">
			<div className="flex justify-between items-center">
				<h1 className="text-3xl font-bold">Banks</h1>
				<CreateEditButton name="Bank" type="Create" onSubmit={handleSubmit} formValue={[
					{name: "name", displayName: "Name", type: "input", options: []},
					{name: "iban", displayName: "Iban", type: "input", options: []},
					{name: "currencyType", displayName: "Currency", type: "select", options: [
							{value: "1", name: "TRY"},
							{value: "2", name: "USD"},
							{value: "3", name: "EURO"},
						]},
				]} handleInputChange={handleInputChange} state={setNewBank} model={BankBodyModel} />
			</div>
			<div className="relative">
				<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500" />
				<Input
					className="pl-8"
					placeholder="Find a banks..."
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)} />
			</div>
			<Table>
				<TableHeader>
					<TableHead>Bank / Id</TableHead>
					<TableHead>Iban</TableHead>
					<TableHead>Deposit</TableHead>
					<TableHead>Withdraw</TableHead>
					<TableHead>Balance</TableHead>
					<TableHead>Currency</TableHead>
				</TableHeader>
				<TableBody>
					{filteredCashRegisters.map((bank) => (
						<TableRow key={bank.id}>
							<TableCell>
								<div>{bank.name}</div>
								<div className="text-sm text-gray-500">{bank.id}</div>
							</TableCell>
							<TableCell>{bank.iban}</TableCell>
							<TableCell>{bank.depositAmount}</TableCell>
							<TableCell>{bank.withdrawAmount}</TableCell>
							<TableCell>{bank.balance}</TableCell>
							<TableCell>{bank.currencyType.name}</TableCell>
							<TableCell className="text-right">
									<ViewButton header="Bank Details" tableHeader={["Id", "Processor", "Description", "Deposit", "Withdraw", "Opposite", "Date"]} id={[{name: "bankId", value: bank.id}]} deleteData={
										{
											endpoint: "/BankDetail/BankDetailDelete",
											name: "Bank Detail"
										}
									} updateData={
										{
											endpoint: "/BankDetail/UpdateBankDetail",
											name: "Bank Detail"
										}
									} createData={
										{
											name: "Bank Detail",
											endpoint: "/BankDetail/CreateBankDetail",
											body: {
												bankId: '',
												date: '',
												type: 0,
												amount: 0, oppositeCashRegisterId: null,
												oppositeBankId: null,
												description: ''
											},
											formValue: [
												{name: "date", displayName: "Date", type: "date", options: []},
												{name: "type", displayName: "Operation Type", type: "select", options: [
														{name: "Deposit", value: "0"},
														{name: "Withdraw", value: "1"}
													]},
												{name: "amount", displayName: "Amount", type: "input",format: "number", options: []},
												{name: "oppositeCashRegisterId", exclusiveOptional: true,  displayName: "Cash Register", type: "optional", optionalSelect: {
													name: "oppositeCashRegisterId", inputChangeOptions: "default", type: "select",
														options: cashRegisters.map(item => {
															return {name: item.name, value: item.id}
														})
													}},
												{name: "oppositeBankId", exclusiveOptional: true,  displayName: "Bank", type: "optional", optionalSelect: {
														name: "oppositeBankId", inputChangeOptions: "default", type: "select",
														options: banks.map(item => {
															return {name: item.name, value: item.id}
														}).filter(item => item.value !== bank.id)
													}},
												{name: "description", displayName: "Description", type: "input", options: []},
											]
										}
									} data={
										{
											details: bank.details,
											id: ["id", "processor", "description", "depositAmount", "withdrawalAmount", "opposite", "date"],
											keysToCompare: ["id", "processor", "description", "depositAmount", "withdrawalAmount", "opposite", "date"],
											filter: {}
										}
									} />
									<CreateEditButton name={"Bank"} id={[{name: "id", value: bank.id}]} type="Edit" onSubmit={handleUpdateSubmit} formValue={[
										{name: "name", displayName: "Name", type: "input", options: []},
										{name: "iban", displayName: "Iban", type: "input", options: []},
										{name: "currencyType", displayName: "Currency", type: "select", options: [
												{name: "TRY", value: "1"},
												{name: "USD", value: "2"},
												{name: "EURO", value: "3"},
											]},
									]} handleInputChange={handleInputChange} state={setNewBank} model={BankBodyModel}/>
									<DeleteButton onDelete={async () => await handleDelete(bank.id)} description={"This action cannot be undone. This will permanently delete the bank and remove all associated data."}/>
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
							if (banks.length === 5)
								setPageNumber(pageNumber + 1)
						}} />
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</div>
	)
}
