'use client'
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Search } from 'lucide-react'
import TeamSwitcher from "./team-switcher"
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {authService} from "@/services/AuthService";
import {
	NavigationMenu, NavigationMenuContent,
	NavigationMenuItem, NavigationMenuLink,
	NavigationMenuList,
	NavigationMenuTrigger
} from "@/components/ui/navigation-menu";
import {navItems} from "@/Models/Navigation";
import Link from "next/link";
import {HTMLAttributes} from "react";

export default function DashboardHeader() {
	return (
		<header className="border-b">
			<div className="flex h-16 items-center px-4">
				<TeamSwitcher />
				<MainNav className="mx-6" />
				<div className="ml-auto flex items-center space-x-4">
					<Search className="h-4 w-4 shrink-0 opacity-50" />
					<Input
						type="search"
						placeholder="Search..."
						className="md:w-[100px] lg:w-[300px]"
					/>
					<UserNav />
				</div>
			</div>
		</header>
	)
}

function MainNav({ className }: HTMLAttributes<HTMLElement>) {
	return (
		<nav className={`flex items-center space-x-4 lg:space-x-6 ${className}`}>
			<NavigationMenu>
				<NavigationMenuList>
					{navItems.map((item, index) => (
						item.isActive ? (
							<NavigationMenuItem key={`nav-item-${index}`}>
								<NavigationMenuTrigger className="gap-1 items-center justify-center">
									{item.icon && (
										<item.icon className="h-4 w-4 shrink-0 opacity-50" />
									)}
									{item.title}
								</NavigationMenuTrigger>
								<NavigationMenuContent>
									<ul className="grid gap-3 p-4 md:w-[400px] lg:w-[500px] lg:grid-cols-[.75fr_1fr]">
										{item.description && (
											<li key={`description-${index}`} className="row-span-3">
												<div className="flex h-full w-full select-none flex-col justify-end rounded-md bg-gradient-to-b from-muted/50 to-muted p-6 no-underline outline-none focus:shadow-md">
													{item.icon && (
														<item.icon className="h-4 w-4 shrink-0 opacity-50" />
													)}
													<div className="mb-2 mt-2 text-lg font-medium">
														{item.title}
													</div>
													<span className="text-sm leading-tight text-muted-foreground">
                        {item.description}
                    </span>
												</div>
											</li>
										)}
										{item.items?.map((subItem, subIndex) => (
											<li key={`sub-item-${index}-${subIndex}`}>
												<NavigationMenuLink asChild>
													<Link
														href={subItem.url}
														className="block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"
													>
														{subItem.icon && (
															<subItem.icon className="h-4 w-4 shrink-0 opacity-50" />
														)}
														<div className="text-sm font-medium leading-none">
															{subItem.title}
														</div>
														{subItem.description && (
															<span className="line-clamp-2 text-sm leading-snug text-muted-foreground">
                                								{subItem.description}
                            								</span>
														)}
													</Link>
												</NavigationMenuLink>
											</li>
										))}
									</ul>
								</NavigationMenuContent>
							</NavigationMenuItem>
						) : (
							<NavigationMenuItem key={`nav-item-${index}`}>
								<NavigationMenuLink asChild>
									<Link href={item.url} className="flex justify-center items-center h-[2.25rem] select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foregroundgap-1">
										<span className="flex justify-center items-center gap-1">
											{item.icon && (
												<item.icon className="h-4 w-4 shrink-0 opacity-50" />
											)}
											<div className="text-sm font-medium leading-none">
												{item.title}
											</div>
											{item.description && (
												<span className="line-clamp-2 text-sm leading-snug text-muted-foreground">
													{item.description}
												</span>
											)}
										</span>
									</Link>
								</NavigationMenuLink>
							</NavigationMenuItem>
						)
					))}
				</NavigationMenuList>
			</NavigationMenu>
		</nav>
	)
}

function UserNav() {
	return (
		<DropdownMenu>
			<DropdownMenuTrigger asChild>
				<Button variant="ghost" className="relative h-8 w-8 rounded-full">
					<Avatar className="h-8 w-8">
						<AvatarImage src="/avatars/01.png" alt={authService.user.id} />
						<AvatarFallback>
							{(() => {
								const nameParts = authService.user.fullname.split(" ");
								const initials = nameParts[0][0] + nameParts[1][0]; // "S" + "G"
								return initials.toUpperCase();
							})()}
						</AvatarFallback>
					</Avatar>
				</Button>
			</DropdownMenuTrigger>
			<DropdownMenuContent className="w-56" align="end" forceMount>
				<DropdownMenuLabel className="font-normal">
					<div className="flex flex-col space-y-1">
						<p className="text-sm font-medium leading-none">
							{`${authService.user.username} (${authService.user.fullname})`}
						</p>
						<p className="text-xs leading-none text-muted-foreground">
							{authService.user.email}
						</p>
					</div>
				</DropdownMenuLabel>
				<DropdownMenuSeparator />
				<DropdownMenuItem>
					Profile
				</DropdownMenuItem>
				<DropdownMenuItem>
					Settings
				</DropdownMenuItem>
				<DropdownMenuSeparator />
				<DropdownMenuItem>
					Log out
				</DropdownMenuItem>
			</DropdownMenuContent>
		</DropdownMenu>
	)
}