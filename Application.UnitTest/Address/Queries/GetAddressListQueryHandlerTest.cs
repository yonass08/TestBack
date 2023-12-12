using AutoMapper;
using Application.Contracts.Persistence;
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

namespace Application.UnitTest.Addresses.Queries
{
    public class GetAddressListQueryHandlerTest
    {

        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly GetAddressListQueryHandler _handler;
        public GetAddressListQueryHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetAddressListQueryHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task GetAddressList()
        {
            var result = await _handler.Handle(new GetAddressListQuery(), CancellationToken.None);
            result.ShouldBeOfType<Result<List<AddressDto>>>();
            result.Value.Count.ShouldBe(2);
            result.IsSuccess.ShouldBeTrue();
            result.Error.ShouldBe(null);

        }
    }
}