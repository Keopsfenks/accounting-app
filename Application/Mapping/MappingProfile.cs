using Application.Features.Commands.Users.CreateUser;
using Application.Features.Commands.Users.UpdateUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public sealed class MappingProfile : Profile {
	public MappingProfile() {
		CreateMap<RegisterRequest, AppUser>();
		CreateMap<UpdateRequest, AppUser>();
	}
}