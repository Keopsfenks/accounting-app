'use client'

import React, { useEffect, useState } from 'react';
import { Sidebar, SidebarHeader } from "@/components/ui/sidebar";
import CompanyHeader from "@/components/sidebar/sidebar-header";
import { http } from "@/services/HttpService";
import { CompanyModel } from "@/ResponseModel/CompanyModel";
import { Company } from "@/Models/Company";
import {toast} from "@/hooks/use-toast";

export default function AppSidebar({...props}: React.ComponentProps<typeof Sidebar>) {
	const [companies, setCompanies] = useState<Company[]>([]);

	useEffect(() => {
		async function getUserToCompanies() {
			try {
				const response = await http.get("/Company/GetUserToCompanies");
				if (response.isSuccessful) {
					const updatedCompanies = await Promise.all(
						// @ts-ignore
						response.data.map(async (company: any) => {
							const companyResponse = await http.get<CompanyModel>("/Company/GetIdCompany", {
								companyId: company.companyId
							});

							if (companyResponse.isSuccessful && companyResponse.data) {
								const companyData: Company = {
									Id: companyResponse.data.id,
									Name: companyResponse.data.name,
									TaxNumber: companyResponse.data.taxNumber,
									TaxDepartment: companyResponse.data.taxDepartment,
									Address: companyResponse.data.address,
									Role: company.roleName,
								};

								return companyData;
							}
							return null;
						})
					);
					setCompanies(updatedCompanies.filter((company: object) => company !== null) as Company[]);
				}
			} catch (error) {
				console.error("Error fetching companies:", error);
			}
		}
		getUserToCompanies().finally();
	}, []);

	useEffect(() => {
		toast({
			variant: "default",
			title: "Success",
			description: "Loading companies successfully",
		})
	}, [companies]);  // companies state değiştiğinde çalışacak

	return (
		<Sidebar collapsible="icon" {...props}>
			<SidebarHeader>
				{companies.length > 0 ? (
					<CompanyHeader companies={companies} />
				) : (
					<p>Loading companies...</p>
				)}
			</SidebarHeader>
		</Sidebar>
	);
}
