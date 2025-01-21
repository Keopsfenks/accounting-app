import React, {useEffect, useState} from 'react'
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import {Company} from "@/Models/Company";
import {useCompanies} from "@/Context/CompanyContext";
import {Button} from "@/components/ui/button";
import {cn} from "@/lib/utils";
import {Avatar} from "@/components/ui/avatar";
import {ChevronsUpDown, LucideCommand} from "lucide-react";
import {http} from "@/services/HttpService";
import {toast} from "@/hooks/use-toast";
import {authService} from "@/services/AuthService";
import {LoginModel} from "@/ResponseModel/LoginModel";


type PopoverTriggerProps = React.ComponentPropsWithoutRef<typeof PopoverTrigger>

interface TeamSwitcherProps extends PopoverTriggerProps {}

export default function TeamSwitcher({className} : TeamSwitcherProps) {
	const companies: Company[] = useCompanies();

	const [open, setOpen] = useState(false);
	const [selectedTeam, setSelectedTeam] = useState<Company | undefined>(undefined);

	useEffect(() => {
		const company = companies.find((company) => company.Id === authService.user.companyId)
		setSelectedTeam(company);
	}, [companies]);

	const changeCompany = async (id: string) => {
		try {
			const response = await http.post<LoginModel>(`/Auth/ChangeCompany`, { companyId: id });

			if (response.data && response.isSuccessful) {
				toast({
					title: "Success",
					description: "Page changed successfully",
				})
				localStorage.setItem("token", response.data.token);
				document.cookie = `token=${response.data.token}; path=/; max-age=86400`; // 24 saat
				window.location.reload();
			} else if (response.errorMessages) {
				toast({
					title: "Error",
					description: response.errorMessages,
					variant: "destructive",
				})
			}
		} catch (err: any) {
			toast({
				title: "Error",
				description: err.message,
				variant: "destructive"
			});
		}
	}

	const filteredCompanies = companies.filter((company) => company.Name.toLowerCase().includes(""));

	return (
		<Popover open={open} onOpenChange={setOpen}>
			<PopoverTrigger asChild>
				<Button
				variant="outline"
				role="combobox"
				aria-expanded={open}
				aria-label="Select a Page"
				className={cn("w-[200px] justify-between", className)}
				>
					<Avatar className="items-center h-5 w-5">
						<LucideCommand />
					</Avatar>
					{selectedTeam?.Name}
					<ChevronsUpDown className="ml-auto h-4 w-4 shrink-0 opacity-50" />
				</Button>
			</PopoverTrigger>
			<PopoverContent className="w-[200px] p-0">
				<div className="border-b">
					{filteredCompanies.map((company) => (
						<Button
						key={company.Id}
						variant="ghost"
						className="w-full text-left"
						onClick={async () => {
							setSelectedTeam(company);
							setOpen(false);
							await changeCompany(company.Id);
						}}
						>
							{company.Name}
						</Button>
					))}
				</div>
			</PopoverContent>
		</Popover>
	)
}
 