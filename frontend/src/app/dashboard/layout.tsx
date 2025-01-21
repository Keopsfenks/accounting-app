import React, {ReactNode} from "react";
import {CompanyProvider} from "@/Context/CompanyContext";
import DashboardHeader from "@/components/dashboard/dashboard-header";

export default function DashboardLayout({children}: {
	children: ReactNode
}) {
	return (
		<CompanyProvider>
			<DashboardHeader/>
			{children}
		</CompanyProvider>
	)
}