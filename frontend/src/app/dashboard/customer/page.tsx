'use client';

import React, {useEffect, useState} from 'react'
import {Tabs, TabsList, TabsTrigger} from "@/components/ui/tabs";
import {TabsContent} from "@radix-ui/react-tabs";
import {useRouter, useSearchParams} from "next/navigation";
import CustomerManagement from "@/app/dashboard/customer/(customer)/customer";
export default function Product() {
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
		router.push(`/dashboard/customer?tab=${value}`, undefined);
	}

	return (
		<div className="flex-1 space-y-4 p-8 pt-6">
			<div className="flex items-center justify-between space-y-2">
				<h2 className="text-3xl font-bold tracking-tight">Customer Management</h2>

			</div>
			<Tabs value={activeTab} onValueChange={handleTabChange} className="space-y-4">
				<TabsList>
					<TabsTrigger value="management">Customer Management</TabsTrigger>
					<TabsTrigger value="sales">Sales Management</TabsTrigger>
					<TabsTrigger value="invoice">Invoice Management</TabsTrigger>
				</TabsList>
				<TabsContent value="management">
					<CustomerManagement />
				</TabsContent>
				<TabsContent value="sales">
				</TabsContent>
				<TabsContent value="invoice">
				</TabsContent>
			</Tabs>
		</div>

	)
}
