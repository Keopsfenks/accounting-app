import {
	BookUser, Building2, ChartColumnStacked, ClipboardList, Landmark, ListMinus,
	ListOrdered, ListPlus,
	LucideIcon,
	LucideLayoutDashboard,
	Package, PackageCheck, PackageMinus, PackagePlus, ShoppingCart,
	UserMinus,
	UserPlus,
	Users, Vault
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
		url: "/customer",
		icon: Users,
		description: "Customer management, product sales and invoice management",
		isActive: true,
		items: [
			{
				title: "Customer Management",
				url: "/dashboard/customer?tab=management",
				icon: BookUser,
				description: "Click to customer management"
			},
			{
				title: "Product Sales",
				url: "/dashboard/customer?tab=sales",
				icon: ShoppingCart,
				description: "Click to product sales"
			},
			{
				title: "Invoice Management",
				url: "/dashboard/customer?tab=invoice",
				icon: ClipboardList,
				description: "Click to invoice management"
			}
		]
	},
	{
		title: "Companies",
		url: "/company",
		icon: Package,
		description: "Company, cash register and bank management",
		isActive: true,
		items: [
			{
				title: "Company Management",
				icon: Building2,
				url: "/dashboard/company?tab=company",
				description: "Click to product management"
			},
			{
				title: "Cash Register Management",
				icon: Vault,
				url: "/dashboard/company?tab=cash-register",
				description: "Click to cash register management"
			},
			{
				title: "Bank Management",
				icon: Landmark,
				url: "/dashboard/company?tab=bank",
				description: "Click to bank management"
			}
		]
	},
	{
		title: "Products",
		url: "/product",
		icon: ListOrdered,
		description: "Product and Category management",
		isActive: true,
		items: [
			{
				title: "Product Management",
				url: "/dashboard/product?tab=product",
				icon: ShoppingCart,
				description: "Click to products management"
			},
			{
				title: "Category Management",
				url: "/dashboard/product?tab=category",
				icon: ChartColumnStacked,
				description: "Click to category management"
			}
		]
	}
]