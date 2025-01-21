'use client'

import React, {useState} from "react";
import {
	Dialog,
	DialogContent,
	DialogHeader,
	DialogTitle,
	DialogTrigger,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import {
	Select,
	SelectContent,
	SelectGroup,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Checkbox } from "@/components/ui/checkbox";
import { DatePicker } from "@/components/dashboard/date-picker";
import { format } from "date-fns"
import {Edit} from "lucide-react";

export interface formValue {
	name: string;
	displayName: string;
	type: "input" | "select" | "date" | "optional";
	format?: React.HTMLInputTypeAttribute;
	options?: {
		value: number | string;
		name: string;
	}[];
	optionalSelect?: {
		name: string;
		type: "input" | "select" | "date";
		options: {
			value: number | string;
			name: string;
		}[];
	};
}

type CreateEditButtonProps = {
	name: string;
	id?: {name: string, value: string}[];
	type: "Create" | "Edit";
	onSubmit: (e: React.FormEvent) => void;
	formValue: formValue[];
	handleInputChange: (e: React.ChangeEvent<HTMLInputElement> | any, addrParam?: {name: any, value: any}[]) => void;
};

export default ({
					name,
					id,
					type,
					onSubmit,
					formValue,
					handleInputChange,
				}: CreateEditButtonProps) => {
	const [isOpen, setIsOpen] = useState(false);
	const [optionalSelections, setOptionalSelections] = useState<string | null>(null);


	function toggleOptional(itemName: string) {
		setOptionalSelections((prev) => (prev === itemName ? null : itemName));
		handleInputChange(null, [{name: itemName, value: null}]);
	}

	return (
		<Dialog open={isOpen} onOpenChange={setIsOpen}>
			<DialogTrigger asChild>
				{type === "Edit" ? (
					<Button onClick={() => handleInputChange(null, id)} variant="ghost" size="icon" className="h-8 w-8">
						<Edit className="h-4 w-4" />
					</Button>
				) : (
					<Button onClick={() => handleInputChange(null, id)}>
						Create {name}
					</Button>
				)}
			</DialogTrigger>
			<DialogContent className="sm:max-w-[425px]">
				<DialogHeader>
					<DialogTitle>{type} New {name}</DialogTitle>
				</DialogHeader>
				<form onSubmit={onSubmit} className="space-y-4">
					{formValue.map((item, index) => (
						<div key={index}>
							{item.type === "input" && (
								<div className="space-y-2">
									<Label htmlFor={item.name}>{name} {item.displayName}</Label>
									<Input
										id={item.name}
										name={item.name}
										type={item.format}
										onChange={handleInputChange}
										required
									/>
								</div>
							)}
							{item.type === "select" && item.options && (
								<div className="space-y-2">
									<Label>{item.displayName}</Label>
									<Select onValueChange={(value) => handleInputChange(null, [{name: item.name, value: value}])}>
										<SelectTrigger className="w-full">
											<SelectValue placeholder={`Select ${item.displayName}`} />
										</SelectTrigger>
										<SelectContent>
											<SelectGroup>
												{item.options.map((option, idx) => (
													<SelectItem key={idx} value={option.value.toString()}>
														{option.name}
													</SelectItem>
												))}
											</SelectGroup>
										</SelectContent>
									</Select>
								</div>
							)}
							{item.type === "date" && (
								<div className="space-y-2">
									<Label>{name} {item.displayName}</Label>
									<DatePicker
										onSelect={(date) => handleInputChange(null, [{name: item.name, value: format(date!, "yyyy-MM-dd")}])}
									/>
								</div>
							)}
							{item.type === "optional" && (
								<div className="space-y-2">
									<div className="flex items-center space-x-2">
										<Checkbox
											id={`optional-${item.name}`}
											checked={optionalSelections === item.name}
											onCheckedChange={() => toggleOptional(item.name)}
										/>
										<Label htmlFor={`optional-${item.name}`}>{item.displayName}</Label>
									</div>
									{optionalSelections === item.name && item.optionalSelect && (
										<Select onValueChange={(value) => handleInputChange(null, [{name: item.optionalSelect!.name, value: value}])}>
											<SelectTrigger className="w-full">
												<SelectValue placeholder={`Select ${item.optionalSelect.name}`} />
											</SelectTrigger>
											<SelectContent>
												<SelectGroup>
													{item.optionalSelect.options.map((option, idx) => (
														<SelectItem key={idx} value={option.value.toString()}>
															{option.name}
														</SelectItem>
													))}
												</SelectGroup>
											</SelectContent>
										</Select>
									)}
								</div>
							)}
						</div>
					))}
					<Button type="submit">{type} {name}</Button>
				</form>
			</DialogContent>
		</Dialog>
	);
}
