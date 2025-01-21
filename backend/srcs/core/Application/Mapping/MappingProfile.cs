using Application.Features.Commands.Banks.BankCreate;
using Application.Features.Commands.CashRegisters.CashRegisterCreate;
using Application.Features.Commands.Companies.CreateCompany;
using Application.Features.Commands.Companies.UpdateCompany;
using Application.Features.Commands.Customers.CreateCustomer;
using Application.Features.Commands.Users.RegisterUser;
using Application.Features.Commands.Users.UpdateUser;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.CompanyEntities;
using Domain.Enums;

namespace Application.Mapping;

public sealed class MappingProfile : Profile {
	public MappingProfile() {
		CreateMap<RegisterRequest, AppUser>();
		CreateMap<UpdateRequest, AppUser>();
		CreateMap<CreateCompanyRequest, Company>();
		CreateMap<UpdateCompanyRequest, Company>();
		CreateMap<CashRegisterCreateRequest, CashRegister>().ForMember(member => member.CurrencyType, options => {
			options.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyType));
		});
		CreateMap<BankCreateRequest, Bank>().ForMember(member => member.CurrencyType, options => {
			options.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyType));
		});
		CreateMap<CreateCustomerRequest, Customer>()
		   .ForMember(member => member.Type, options => {
			   options.MapFrom(map => CustomerTypeEnum.FromValue(map.CustomerType));
		   });
	}
}