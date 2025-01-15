'use client'

import React, {createContext, useContext, useState, useEffect, FC, ReactNode} from 'react';
import {http} from "@/services/HttpService";
import {CompanyModel} from "@/ResponseModel/CompanyModel";
import {Company} from "@/Models/Company";
import {authService} from "@/services/AuthService";
import {UserModel} from "@/Models/User";

const CompanyContext = createContext<Company[] | undefined>(undefined);

export const CompanyProvider: FC<{ children: ReactNode }> = ({ children }) => {
	const [companies, setCompanies] = useState<Company[]>([]);

	useEffect(() => {
		authService.isAuthenticated();
		const companyData = authService.user.companies;
		console.log("companyData", companyData);
		companyData.map((company: CompanyModel) => {
			const newCompany: Company = {
				Id: company.Id,
				Name: company.Name,
				TaxNumber: company.TaxId,
				TaxDepartment: company.TaxDepartment,
				Address: company.Address,
				Role: company.UserRoles.map((role) => role.RoleName).join(", "),
			};
			setCompanies((companies) => [...companies, newCompany]);
		});
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
