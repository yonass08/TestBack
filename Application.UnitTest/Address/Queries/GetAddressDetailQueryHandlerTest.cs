using AutoMapper;
using Application.Contracts.Persistence;
using Application.Features.Addresses.CQRS.Commands;
using Application.Features.Addresses.CQRS.Handlers;
using Application.Features.Addresses.CQRS.Queries;
using Application.Features.Addresses.DTOs;
using Application.Profiles;
using Application.Responses;
using Application.UnitTest.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using  Application.Features.Specialities.CQRS.Queries;

namespace Application.UnitTest.Addresses.Queries
{
    public class GetAddressDetailQueryHandlerTest
    {

        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private Guid Id;
        private readonly GetAddressDetailQueryHandler _handler;
        public GetAddressDetailQueryHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            Id = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");

            _handler = new GetAddressDetailQueryHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task GetAddressDetail()
        {
            var result = await _handler.Handle(new GetAddressDetailQuery() { Id = Id }, CancellationToken.None);
            result.ShouldBeOfType<Result<AddressDto>>();
        }

        [Fact]
        public async Task GetNonExistingAddress()
        {
            // Set a non-existing Id
            var nonExistingId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa8");
        
            // Invoke the handler with the non-existing Id
            var result = await _handler.Handle(new GetAddressDetailQuery() { Id = nonExistingId }, CancellationToken.None);
        
            // Assert that the response is a NotFound result
            result.ShouldBeOfType<Result<AddressDto>>();
            result.IsSuccess.ShouldBeFalse();
            result.Value.ShouldBe(null);
        }
    }
}
