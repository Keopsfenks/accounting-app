import React, {ReactNode} from "react";
import {
	SidebarInset,
	SidebarProvider,
} from "@/components/ui/sidebar"
import AppSidebar from "@/components/sidebar/app-sidebar";

export default function DashboardLayout({children}: {
	children: ReactNode
}) {
	return (
		<SidebarProvider>
			<AppSidebar>

			</AppSidebar>
		</SidebarProvider>
	)
}