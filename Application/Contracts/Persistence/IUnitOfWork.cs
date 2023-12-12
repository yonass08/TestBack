namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        ISpecialityRepository SpecialityRepository { get; }
        IEducationRepository EducationRepository { get; }

        IDoctorAvailabilityRepository DoctorAvailabilityRepository {get;}
        IInstitutionAvailabilityRepository InstitutionAvailabilityRepository{get;}
        IAddressRepository AddressRepository {get;}
        IInstitutionProfileRepository InstitutionProfileRepository {get;}
        IExperienceRepository ExperienceRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IDoctorProfileRepository DoctorProfileRepository { get; }

        Task<int> Save();

    }
}