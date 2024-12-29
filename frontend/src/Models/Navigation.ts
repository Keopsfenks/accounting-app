import {
	BookUser, ClipboardList, ListMinus,
	ListOrdered, ListPlus,
	LucideIcon,
	LucideLayoutDashboard,
	Package, PackageCheck, PackageMinus, PackagePlus,
	UserMinus,
	UserPlus,
	Users
} from "lucide-react";

class Navigation {
	title: string = "";
	url: string = "";
	description?: string;
	icon?: LucideIcon;
	isActive?: boolean;
	items?: {
		title: string;
		url: string;
		description?: string;
		icon?: LucideIcon;
	}[];
}



export const navItems: Navigation[] = [
	{
		title: "Overview",
		url: "/dashboard",
		icon: LucideLayoutDashboard,
		isActive: false
	},
	{
		title: "Customers",
		url: "/dashboard",
		icon: Users,
		description: "Customer management",
		isActive: true,
		items: [
			{
				title: "Create a customer",
				url: "/dashboard",
				icon: UserPlus,
				description: "Click to create a customer"
			},
			{
				title: "Remove a customer",
				icon: UserMinus,
				url: "/dashboard",
				description: "Click to remove a customer"
			},
			{
				title: "View customers",
				icon: BookUser,
				url: "/dashboard",
				description: "View all customers"
			}
		]
	},
	{
		title: "Companies",
		url: "/dashboard",
		icon: Package,
		description: "Company management",
		isActive: true,
		items: [
			{
				title: "Create a company",
				icon: PackagePlus,
				url: "/dashboard",
				description: "Click to create a company"
			},
			{
				title: "Remove a company",
				icon: PackageMinus,
				url: "/dashboard",
				description: "Click to remove a company"
			},
			{
				title: "View companies",
				icon: PackageCheck,
				url: "/dashboard",
				description: "View all companies"
			}
		]
	},
	{
		title: "Products",
		url: "/dashboard",
		icon: ListOrdered,
		description: "Product management",
		isActive: true,
		items: [
			{
				title: "Create a product",
				url: "/dashboard",
				icon: ListPlus,
				description: "Click to create a product"
			},
			{
				title: "Remove a product",
				url: "/dashboard",
				icon: ListMinus,
				description: "Click to remove a product"
			},
			{
				title: "View products",
				url: "/dashboard",
				icon: ClipboardList,
				description: "View all products"
			}
		]
	}
]