using Application.Features.Commands.Companies.CreateCompany;
using Application.Features.Commands.Companies.UpdateCompany;
using Application.Features.Commands.Users.RegisterUser;
using Application.Features.Commands.Users.UpdateUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public sealed class MappingProfile : Profile {
	public MappingProfile() {
		CreateMap<RegisterRequest, AppUser>();
		CreateMap<UpdateRequest, AppUser>();
		CreateMap<CreateCompanyRequest, Company>();
		CreateMap<UpdateCompanyRequest, Company>();
	}
}