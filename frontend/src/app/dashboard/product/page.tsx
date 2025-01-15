'use client';
import React, {useEffect, useState} from 'react'
import {Tabs, TabsList, TabsTrigger} from "@/components/ui/tabs";
import {TabsContent} from "@radix-ui/react-tabs";
import {useRouter, useSearchParams} from "next/navigation";
import ProductManagement from "@/app/dashboard/product/(product-man)/product";
import CategoryManagement from "@/app/dashboard/product/(category-man)/category";
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
		router.push(`/dashboard/product?tab=${value}`, undefined);
	}

	return (
		<div className="flex-1 space-y-4 p-8 pt-6">
			<div className="flex items-center justify-between space-y-2">
				<h2 className="text-3xl font-bold tracking-tight">Product&Category Management</h2>

			</div>
			<Tabs value={activeTab} onValueChange={handleTabChange} className="space-y-4">
				<TabsList>
					<TabsTrigger value="product">Product Management</TabsTrigger>
					<TabsTrigger value="category">Category Management</TabsTrigger>
				</TabsList>
				<TabsContent value="product">
					<ProductManagement />
				</TabsContent>
				<TabsContent value="category">
					<CategoryManagement />
				</TabsContent>

			</Tabs>
		</div>

	)
}
