using Application.Contracts.Persistence;
using Application.Features.DoctorProfiles.CQRS.Handlers;
using Application.Features.DoctorProfiles.CQRS.Queris;
using Application.Features.DoctorProfiles.DTOs;
using Application.Profiles;
using Application.Responses;
using Application.UnitTest.Mocks;
using AutoMapper;
using Moq;
using Shouldly;

namespace Application.UnitTest.DoctorProfiles.Queries
{
    public class FilterDoctorProfilesQueryHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly FilterDoctorProfilesQueryHandler _handler;

        public FilterDoctorProfilesQueryHandlerTests()
        {
            _mockUow = MockUnitOfWork.GetUnitOfWork();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new FilterDoctorProfilesQueryHandler(_mockUow.Object, _mapper);
        }


        [Fact]
        public async Task Handle_WithAllParameters_ReturnsFilteredDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery
            {
                SpecialityNames = new List<string> { "Cardiology" },
                InstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                ExperienceYears = 0,
                EducationName = "Medical College"
            };

            var expectedDoctorProfiles = new List<DoctorProfileDto>
                {
                    new DoctorProfileDto
                    {
                        Id = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3304"),
                        FullName ="Dr. Emily Johnson",
                        PhotoUrl = "photo3",
                        MainInstitutionId =  Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                        YearsOfExperience = 2
                    }
                };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            result.Value.ShouldNotBeNull();
            var actualDoctorProfiles = result.Value;
            actualDoctorProfiles.ShouldNotBeEmpty();
            actualDoctorProfiles.Count.ShouldBe(expectedDoctorProfiles.Count);

            for (int i = 0; i < actualDoctorProfiles.Count; i++)
            {
                var actualProfile = actualDoctorProfiles[i];
                var expectedProfile = expectedDoctorProfiles[i];

                actualProfile.FullName.ShouldBe(expectedProfile.FullName);
                actualProfile.Id.ShouldBe(expectedProfile.Id);
                // actualProfile.YearsOfExperience.ShouldBe(expectedProfile.YearsOfExperience);
                actualProfile.MainInstitutionId.ShouldBe(expectedProfile.MainInstitutionId);
            }
        }

        [Fact]
        public async Task Handle_WithNoParameters_ReturnsAllDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery();

            var expectedDoctorProfiles = new List<DoctorProfileDetailDto> { };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            // result.Value.ShouldBeNull();


        }

        [Fact]
        public async Task Handle_WithSpecialityNames_ReturnsFilteredDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery
            {
                InstitutionId = null,
                SpecialityNames = new List<string> { "Internal Medicine" },
                ExperienceYears = -1,
                EducationName = null,
                pageNumber = 0,
                pageSize = 0,
            };

            var expectedDoctorProfiles = new List<DoctorProfileDto>
                {
                    new DoctorProfileDto
                    {
                        Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
                        FullName = "Dr. John Smith",
                        PhotoUrl = "photo1",
                        MainInstitutionId =  Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3302"),
                        YearsOfExperience = 0
                    }
                };


            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            result.Value.ShouldNotBeNull();
            var actualDoctorProfiles = result.Value;
            actualDoctorProfiles.ShouldNotBeEmpty();
            actualDoctorProfiles.Count.ShouldBe(expectedDoctorProfiles.Count);

            for (int i = 0; i < actualDoctorProfiles.Count; i++)
            {
                var actualProfile = actualDoctorProfiles[i];
                var expectedProfile = expectedDoctorProfiles[i];

                actualProfile.FullName.ShouldBe(expectedProfile.FullName);
                actualProfile.Id.ShouldBe(expectedProfile.Id);
                actualProfile.MainInstitutionId.ShouldBe(expectedProfile.MainInstitutionId);
            }


        }

        [Fact]
        public async Task Handle_WithInstitutionId_ReturnsFilteredDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery
            {
                InstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                pageNumber = 0,
                pageSize = 0,

            };

            var expectedDoctorProfiles = new List<DoctorProfileDto>
                {
                    new DoctorProfileDto
                    {
                        Id = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3304"),
                        FullName = "Dr. John Smith",
                        PhotoUrl = "photo3",
                        MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                        YearsOfExperience = 0
                    }
                };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            result.Value.ShouldNotBeNull();
            var actualDoctorProfiles = result.Value;
            actualDoctorProfiles.ShouldNotBeEmpty();
            actualDoctorProfiles.Count.ShouldBe(expectedDoctorProfiles.Count);

            for (int i = 0; i < actualDoctorProfiles.Count; i++)
            {
                var actualProfile = actualDoctorProfiles[i];
                var expectedProfile = expectedDoctorProfiles[i];

                actualProfile.Id.ShouldBe(expectedProfile.Id);
                actualProfile.MainInstitutionId.ShouldBe(expectedProfile.MainInstitutionId);
            }
        }

        [Fact]
        public async Task Handle_WithCareerStartTime_ReturnsFilteredDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery
            {
                ExperienceYears = 0,
                pageNumber = 0,
                pageSize = 0
            };

            var expectedDoctorProfiles = new List<DoctorProfileDto>
                {
                    new DoctorProfileDto
                    {
                        Id = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3301"),
                        FullName = "Dr. John Smith",
                        PhotoUrl = "photo3",
                        YearsOfExperience = 0,
                        MainInstitutionId = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3302"),
                }};



            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            result.Value.ShouldNotBeNull();
            var actualDoctorProfiles = result.Value;
            actualDoctorProfiles.ShouldNotBeEmpty();
            // actualDoctorProfiles.Count.ShouldBe(expectedDoctorProfiles.Count);

            for (int i = 0; i < expectedDoctorProfiles.Count; i++)
            {
                var actualProfile = actualDoctorProfiles[i];
                var expectedProfile = expectedDoctorProfiles[i];

                // actualProfile.PhotoUrl.ShouldBe(expectedProfile.PhotoUrl);
                actualProfile.FullName.ShouldBe(expectedProfile.FullName);
                actualProfile.Id.ShouldBe(expectedProfile.Id);
                // actualProfile.YearsOfExperience.ShouldBe(expectedProfile.YearsOfExperience);
                actualProfile.MainInstitutionId.ShouldBe(expectedProfile.MainInstitutionId);
            }

        }

        [Fact]
        public async Task Handle_WithEducationInstitutionName_ReturnsFilteredDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery
            {
                EducationName = "Medical College",
                pageNumber = 0,
                pageSize = 0,
            };

            var expectedDoctorProfiles = new List<DoctorProfileDto>
            {
                new DoctorProfileDto
                {
                    Id = Guid.Parse("3f2504e0-4f89-41d3-9a0c-0305e82c3304"),
                    FullName = "Dr. Emily Johnson",
                    PhotoUrl = "photo3",
                    MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3305"),
                    YearsOfExperience = 2
                }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            result.Value.ShouldNotBeNull();
            var actualDoctorProfiles = result.Value;
            actualDoctorProfiles.ShouldNotBeEmpty();
            actualDoctorProfiles.Count.ShouldBe(expectedDoctorProfiles.Count);

            for (int i = 0; i < actualDoctorProfiles.Count; i++)
            {
                var actualProfile = actualDoctorProfiles[i];
                var expectedProfile = expectedDoctorProfiles[i];

                actualProfile.FullName.ShouldBe(expectedProfile.FullName);
                actualProfile.Id.ShouldBe(expectedProfile.Id);
                actualProfile.MainInstitutionId.ShouldBe(expectedProfile.MainInstitutionId);
            }
        }

        [Fact]
        public async Task Handle_WithPagination_ReturnsPagedDoctorProfiles()
        {
            // Arrange
            var query = new FilterDoctorProfilesQuery
            {
                pageNumber = 2,
                pageSize = 2
            };

            var expectedDoctorProfiles = new List<DoctorProfileDto>
            {
                new DoctorProfileDto
                    {
                        Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3308"),
                        FullName = "Dr. Sophia Miller",
                        About = "Experienced and caring doctor",
                        Email = "sophia.miller@example.com",
                        MainInstitutionId = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3307"),

                    }
            };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<Result<List<DoctorProfileDto>>>();
            result.Value.ShouldNotBeNull();
            var actualDoctorProfiles = result.Value;
            actualDoctorProfiles.ShouldNotBeEmpty();
            actualDoctorProfiles.Count.ShouldBe(expectedDoctorProfiles.Count);

            for (int i = 0; i < actualDoctorProfiles.Count; i++)
            {
                var actualProfile = actualDoctorProfiles[i];
                var expectedProfile = expectedDoctorProfiles[i];

                actualProfile.FullName.ShouldBe(expectedProfile.FullName);
                actualProfile.Id.ShouldBe(expectedProfile.Id);
                actualProfile.MainInstitutionId.ShouldBe(expectedProfile.MainInstitutionId);
            }
        }



    }
}