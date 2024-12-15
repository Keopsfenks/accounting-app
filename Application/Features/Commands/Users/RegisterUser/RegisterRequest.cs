﻿using MediatR;
using TS.Result;

namespace Application.Features.Commands.Users.CreateUser;

public sealed record RegisterRequest(
	string     FirstName,
	string     LastName,
	string     UserName,
	string     Email,
	string     Password,
	bool       IsAdmin) : IRequest<Result<RegisterResponse>>;