'use client';
import React, {useEffect, useState} from 'react'
import {Tabs, TabsList, TabsTrigger} from "@/components/ui/tabs";
import {TabsContent} from "@radix-ui/react-tabs";
import {useRouter, useSearchParams} from "next/navigation";
import CompanyManagement from "@/app/dashboard/company/(company-man)/company";
import CashregisterManagement from "@/app/dashboard/company/(cashregister)/cashregister";
import Bank from "@/app/dashboard/company/(bank)/bank";
export default function Company() {
	const router = useRouter();
	const searchParams = useSearchParams();
	const [activeTab, setActiveTab] = useState("overview")

	useEffect(() => {
		const tab = searchParams.get("tab");
		if (tab) {
			setActiveTab(tab);
		}
	}, [searchParams]);

	function handleTabChange(value: string) {
		setActiveTab(value);
		router.push(`/dashboard/company?tab=${value}`, undefined);
	}

	return (
		<div className="flex-1 space-y-4 p-8 pt-6">
			<div className="flex items-center justify-between space-y-2">
				<h2 className="text-3xl font-bold tracking-tight">Company Management</h2>

			</div>
			<Tabs value={activeTab} onValueChange={handleTabChange} className="space-y-4">
				<TabsList>
					<TabsTrigger value="company">Management</TabsTrigger>
					<TabsTrigger value="cash-register">Cash Register Management</TabsTrigger>
					<TabsTrigger value="bank">Bank Management</TabsTrigger>
				</TabsList>
				<TabsContent value="company">
					<CompanyManagement/>
				</TabsContent>
				<TabsContent value="cash-register">
					<CashregisterManagement/>
				</TabsContent>
				<TabsContent value="bank">
					<Bank/>
				</TabsContent>
			</Tabs>
		</div>

	)
}
