﻿using MediatR;
using AutoMapper;

using BloodBank.Domain.Entities;
using BloodBank.Domain.Interfaces;
using BloodBank.Domain.DomainResults;
using BloodBank.Domain.DomainErrors;

namespace BloodBank.Application.Donors.Commands.Handlers;

internal sealed class CreateDonorCommandHandler : IRequestHandler<CreateDonorCommand, Result<int>>
{
    private readonly IDonorRepository _donorRepository;
    private readonly IMapper _mapper;

    public CreateDonorCommandHandler(IDonorRepository donorRepository, IMapper mapper)
    {
        _donorRepository = donorRepository;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
    {
        var donor = _mapper.Map<Donor>(request);

        _donorRepository.Create(donor);

        var created = await _donorRepository.SaveChangesAsync();

        if (!created)
            return DonorErrors.CannotBeCreated;

        return donor.Id;
    }
}