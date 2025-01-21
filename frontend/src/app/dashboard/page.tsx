'use client'
import React, {useEffect, useState} from 'react';
import {CustomerModel} from "@/ResponseModel/CustomerModel";
import {BankModel} from "@/ResponseModel/BankModel";
import {CashRegisterModel} from "@/ResponseModel/CashRegisterModel";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";

import { Button } from "@/components/ui/button"
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs"
import { CalendarIcon } from 'lucide-react'
import MetricCard from "@/components/dashboard/overview/MatricCard";
import OverviewChart from "@/components/dashboard/overview/OverviewChart";
import RecentSales from "@/components/dashboard/overview/RecentSales";
import {TLICON} from "@/lib/utils";
import RecentCashProceeds from "@/components/dashboard/overview/RecentCashProceeds";

export default function Dashboard() {
	const [customers, setCustomers] = useState<CustomerModel[]>([]);
	const [bank, setBank] = useState<BankModel[]>([]);
	const [cashRegister, setCashRegister] = useState<CashRegisterModel[]>([]);
	const [customerPageNumber, setCustomerPageNumber] = useState(0)
	const [bankPageNumber, setBankPageNumber] = useState(0)
	const [cashRegisterPageNumber, setCashRegisterPageNumber] = useState(0)

	useEffect(() => {
		async function getCustomers() {
			try {
				const response = await http.get<CustomerModel[]>("/Customer/GetAllCustomers", {
					params: {
						pageSize: 5,
						pageNumber: customerPageNumber
					}
				})
				if (response.data && response.isSuccessful) {
					setCustomers(response.data)
				}
			} catch (err: any) {
				toast({
					variant: "destructive",
					title: "Error",
					description: err,
				})
			}
		}
		async function getBank() {
			try {
				const response = await http.get<BankModel[]>("/Bank/GetAllBanks", {
					params: {
						pageSize: 5,
						pageNumber: bankPageNumber
					}
				})
				if (response.data && response.isSuccessful) {
					setBank(response.data)
				}
			} catch (err: any) {
				toast({
					variant: "destructive",
					title: "Error",
					description: err,
				})
			}
		}
		async function getCashRegister() {
			try {
				const response = await http.get<CashRegisterModel[]>("/CashRegister/GetAllCashRegister", {
					params: {
						pageSize: 5,
						pageNumber: cashRegisterPageNumber
					}
				})
				if (response.data && response.isSuccessful) {
					setCashRegister(response.data)
				}
			} catch (err: any) {
				toast({
					variant: "destructive",
					title: "Error",
					description: err,
				})
			}
		}
		getCustomers().then();
		getBank().then();
		getCashRegister().then();
		toast({
			variant: "default",
			title: "Success",
			description: "Data loaded successfully",
		})
	}, []);

	return (
		<div className="p-6 space-y-6">
			<div className="flex items-center justify-between space-y-2">
				<h2 className="text-3xl font-bold tracking-tight">Dashboard</h2>
				<div className="flex items-center space-x-2">
					<Button variant="outline" className="w-[260px] justify-start text-left font-normal">
						<CalendarIcon className="mr-2 h-4 w-4" />
						28 Ocak 2024 - 28 Ocak 2024
					</Button>
					<Button>Download</Button>
				</div>
			</div>

			<Tabs defaultValue="overview">
				<TabsList>
					<TabsTrigger value="overview">Overview</TabsTrigger>
					
				</TabsList>
				<TabsContent value="overview" className="space-y-4">
					<div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
						<MetricCard title="Total Revenue"
									value={customers.reduce((total, customer) => total + customer.deposit, 0) + TLICON}
									description="Total revenue count" icon="dollar"/>
						<MetricCard title="Total Receivable"
									value={customers.reduce((total, customer) => total + customer.withdrawal, 0) + TLICON}
									description="Total receivable count" icon="receivable"/>
						<MetricCard title="Customers" value={customers.length.toString()}
									description="Total customer counts" icon="users"/>
						<MetricCard
							title="Sales"
							value={customers.reduce((total, transaction) =>
									total +
									((transaction.invoices?.length || 0) + (transaction.details?.length || 0))
								, 0).toString()}
							description="Total transaction count"
							icon="credit-card"
						/>
					</div>
					<OverviewChart className="" customerDetail={customers.flatMap(customer => customer.details)}
								   customerInvoice={customers.flatMap(customer => customer.invoices)}/>
					<div className="grid gap-4 md:grid-cols-2 lg:grid-cols-2">
						<RecentSales className="w-full" customers={customers}/>
						<RecentCashProceeds className="w-full" customers={customers}/>
					</div>

				</TabsContent>
			</Tabs>
		</div>
	)
}

