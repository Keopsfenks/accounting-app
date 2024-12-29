'use client'

import React, {createContext, useContext, useState, useEffect, FC, ReactNode} from 'react';
import {http} from "@/services/HttpService";
import {CompanyModel} from "@/ResponseModel/CompanyModel";
import {Company} from "@/Models/Company";

const CompanyContext = createContext<Company[] | undefined>(undefined);

export const CompanyProvider: FC<{ children: ReactNode }> = ({ children }) => {
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

	return (
		<CompanyContext.Provider value={companies}>
			{children}
		</CompanyContext.Provider>
	);
};

export const useCompanies = () => {
	const context = useContext(CompanyContext);
	if (context === undefined) {
		throw new Error('useCompanies must be used within a CompanyProvider');
	}
	return context;
};
