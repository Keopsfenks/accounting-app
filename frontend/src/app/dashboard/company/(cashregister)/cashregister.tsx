import React, {useEffect, useState} from 'react'
import CreateEditButton from "@/components/dashboard/create-edit-button";
import {Input} from "@/components/ui/input";
import {Search} from "lucide-react";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table";
import {CashRegisterDetails, CashRegisterModel} from "@/ResponseModel/CashRegisterModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {DeleteButton} from "@/components/dashboard/delete-button";
import {
	Pagination,
	PaginationContent,
	PaginationItem,
	PaginationLink, PaginationNext,
	PaginationPrevious
} from '@/components/ui/pagination';
import ViewButton from "@/components/dashboard/view-button";
import {inputChangeService} from "@/services/input-change";
import {BankModel} from "@/ResponseModel/BankModel";
import CashRegisterBodyModel from "@/Models/CashRegister";

export default function CashregisterManagement() {
	const [cashRegisters, setCashRegisters] = useState<CashRegisterModel[]>([]);
	const [banks, setBanks] = useState<BankModel[]>([]);
	const [pageNumber, setPageNumber] = useState(0)
	const [newCashRegister, setNewCashRegister] = useState(CashRegisterBodyModel);

	const handleInputChange = (
		e: React.ChangeEvent<HTMLInputElement> | null,
		addrParams?: { name: string; value: string }[]
	) => {
		inputChangeService(e, setNewCashRegister, addrParams);
	}

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()
		try {
			const response = await http.post('/CashRegister/CreateCashRegister', {
				name: newCashRegister.name,
				currencyType: newCashRegister.currencyType
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Cash Register created successfully",
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
			const response = await http.put('/CashRegister/UpdateCashRegister', {
				id: newCashRegister.id,
				name: newCashRegister.name,
				currencyType: newCashRegister.currencyType
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Cash register updated successfully",
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

	const handleDeleteCashRegister = async (id: string) => {
		try {
			const response = await http.delete(`/CashRegister/DeleteCashRegister`, {
				id: id
			})
			if (response.isSuccessful) {
				toast({
					title: "Success",
					description: "Cash register delete successfully",
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
	}, [pageNumber]);


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
	}, []);


	const [searchQuery, setSearchQuery] = useState('')

	const filteredCashRegisters = cashRegisters.filter(cashregister =>
		cashregister.name.toLowerCase().includes(searchQuery.toLowerCase())
	)


	return (
		<div className="space-y-4">
			<div className="flex justify-between items-center">
				<h1 className="text-3xl font-bold">Cash Registers</h1>
				<CreateEditButton name={"Cash Register"} type="Create" onSubmit={handleSubmit} formValue={[
					{name: "name", displayName: "Name", type: "input", options: []},
					{name: "currencyType", displayName: "Currency", type: "select", options: [
						{value: "1", name: "TRY"},
						{value: "2", name: "USD"},
						{value: "3", name: "EURO"},
						]},
				]} handleInputChange={handleInputChange} state={setNewCashRegister} model={CashRegisterBodyModel} />
			</div>
			<div className="relative">
				<Search className="absolute left-2 top-1/2 transform -translate-y-1/2 text-gray-500" />
				<Input
					className="pl-8"
					placeholder="Find a cash register..."
					value={searchQuery}
					onChange={(e) => setSearchQuery(e.target.value)}
				/>
			</div>

			<Table>
				<TableHeader>
					<TableRow>
						<TableHead>Cash Register / Id</TableHead>
						<TableHead>Deposit</TableHead>
						<TableHead>Withdraw</TableHead>
						<TableHead>Balance</TableHead>
						<TableHead>Currency</TableHead>
					</TableRow>
				</TableHeader>
				<TableBody>
					{filteredCashRegisters.map(cashregister => (
						<TableRow key={cashregister.id}>
							<TableCell>
								<div>{cashregister.name}</div>
								<div className="text-sm text-gray-500 max-w-xs overflow-x-auto">
									<div className="whitespace-nowrap">
										#{cashregister.id}
									</div>
								</div>
							</TableCell>
							<TableCell>{cashregister.depositAmount}</TableCell>
							<TableCell>{cashregister.withdrawalAmount}</TableCell>
							<TableCell>{cashregister.balanceAmount}</TableCell>
							<TableCell>{cashregister.currencyType.name}</TableCell>
							<TableCell className="text-right">
								<ViewButton<CashRegisterDetails> header={"Cash Register Details"} tableHeader={
									["Id", "Processor", "Description", "Deposit", "Withdraw", "Opposite", "Date"]
								} id={[{name: "cashRegisterId", value: cashregister.id}]} data={
									{
										details: cashregister.details,
										id: ["id", "processor", "description", "depositAmount", "withdrawalAmount", "opposite", "date"],
										keysToCompare: ["id", "processor", "description", "depositAmount", "withdrawalAmount", "opposite", "date"],
										filter: {}
									}
								} deleteData={
									{
										endpoint: "/CashRegisterDetail/DeleteCashRegisterDetail",
										name: "Cash Register Detail"
									}
								} createData={
									{
										name: "Cash Register Detail",
										endpoint: "/CashRegisterDetail/CreateCashRegisterDetail",
										body: {
											cashRegisterId: '',
											date	: '',
											type: '',
											amount: 0,
											cashRegisterDetailId: null,
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
											{name: "cashRegisterDetailId", exclusiveOptional: true, displayName: "Cash Register", type: "optional", optionalSelect: {
												name: "cashRegisterDetailId", type: "select", inputChangeOptions: "default",
													options: cashRegisters.map(item => {
														return {name: item.name, value: item.id}
													}).filter(item => item.value !== cashregister.id)
												}},
											{name: "oppositeBankId", exclusiveOptional: true, displayName: "Bank", type: "optional", optionalSelect: {
												name: "oppositeBankId", type: "select", inputChangeOptions: "default",
													options: banks.map(item => {
														return {name: item.name, value: item.id}
												}),
											}},
											{name: "description", displayName: "Description", type: "input", options: []},
										]
									}
								} updateData={
									{
										endpoint: "/CashRegisterDetail/UpdateCashRegisterDetail",
										name: "Cash Register Detail"
									}
								}/>
								<CreateEditButton name={"Cash Register"} id={[{name: "id", value: cashregister.id}]} type="Edit" onSubmit={handleUpdateSubmit} formValue={[
									{name: "name", displayName: "Name", type: "input", options: []},
									{name: "currencyType", displayName: "Currency", type: "select", options: [
										{name: "TRY", value: "1"},
										{name: "USD", value: "2"},
										{name: "EURO", value: "3"},
									]},
								]} handleInputChange={handleInputChange} state={setNewCashRegister} model={CashRegisterBodyModel}  />
								<DeleteButton onDelete={async () => await handleDeleteCashRegister(cashregister.id)} description={"This action cannot be undone. This will permanently delete the cash register and remove all associated data."}/>
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
							if (cashRegisters.length === 5)
								setPageNumber(pageNumber + 1)
						}} />
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</div>
	)
}
