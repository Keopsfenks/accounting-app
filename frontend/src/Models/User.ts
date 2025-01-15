import {Company} from "@/Models/Company";
import {CompanyModel} from "@/ResponseModel/CompanyModel";

export class UserModel {
	id: string = "";
	username: string = "";
	email: string = "";
	fullname: string = "";
	companyId: string = "";
	companies: CompanyModel[] = [];
}