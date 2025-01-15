export class Company {
	Id: string = "";
	Name: string = "";
	TaxNumber: string = "";
	TaxDepartment: string = "";
	Address: string = "";
	Role: string = "";
}

class CompanyBody {
	id: string = '';
	name: string =  '';
	address: string =  '';
	taxDepartment: string =  '';
	taxId: string =  '';
}

const CompanyBodyModel = new CompanyBody();

export default CompanyBodyModel;