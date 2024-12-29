import React, {useEffect, useState} from 'react'
import {Popover, PopoverContent, PopoverTrigger} from "@/components/ui/popover";
import {Company} from "@/Models/Company";
import {useCompanies} from "@/Context/CompanyContext";
import {Button} from "@/components/ui/button";
import {cn} from "@/lib/utils";
import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar";
import {Check, ChevronsUpDown, LucideCommand, PlusCircle} from "lucide-react";
import {
	Command, CommandDialog,
	CommandEmpty,
	CommandGroup,
	CommandInput,
	CommandItem,
	CommandList,
	CommandSeparator
} from "@/components/ui/command";

type PopoverTriggerProps = React.ComponentPropsWithoutRef<typeof PopoverTrigger>

interface TeamSwitcherProps extends PopoverTriggerProps {}

export default function TeamSwitcher({className} : TeamSwitcherProps) {
	const companies: Company[] = useCompanies();

	const [open, setOpen] = useState(false);
	const [selectedTeam, setSelectedTeam] = useState<Company | undefined>(undefined);

	useEffect(() => {
		setSelectedTeam(companies[0]);
	}, [companies]);

	return (
		<Popover open={open} onOpenChange={setOpen}>
			<PopoverTrigger asChild>
				<Button
				variant="outline"
				role="combobox"
				aria-expanded={open}
				aria-label="Select a Company"
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
				<Command>
					<CommandList>
						<CommandInput  placeholder="Search Company..."/>
						<CommandEmpty>No company found.</CommandEmpty>
						{companies.map((company) => (
							<CommandItem
							key={company.Id}
							onSelect={() => {
								setSelectedTeam(company);
								setOpen(false);
							}}
							className="text-sm"
							>
								<Avatar className="items-center h-5 w-5">
									<LucideCommand />
								</Avatar>
								{company.Name}
								<Check
								className={cn(
									"ml-auto h-4 w-4",
									selectedTeam?.Id === company.Id ? "text-primary" : "opacity-0"
								)}/>
							</CommandItem>
						))}
					</CommandList>
					<CommandSeparator />
					<CommandList>
						<CommandGroup>
							<CommandItem
							onSelect={() => {
								setOpen(false);
							}}
							>
								<PlusCircle className="mr-2 h-5 w-5" />
								Create Company
							</CommandItem>
						</CommandGroup>
					</CommandList>
				</Command>
			</PopoverContent>
		</Popover>
	)
}
 