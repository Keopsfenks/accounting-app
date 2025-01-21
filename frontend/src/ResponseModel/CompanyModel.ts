import {Company} from "@/Models/Company";

interface userModel {
	RoleName: (value: userModel, index: number, array: userModel[]) => void;
	UserRoles: [Company[]];
}

export interface CompanyModel {
	Id: string;
	Name: string;
	TaxId: string;
	TaxDepartment: string;
	Address: string;
	UserRoles: userModel[];

}