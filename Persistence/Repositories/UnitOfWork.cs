
using Application.Contracts.Persistence;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HakimHubDbContext _context;


        private ISpecialityRepository _specialityRepository;
        private IEducationRepository _educationRepository;
        private IDoctorAvailabilityRepository _doctorAvailabilityRepository;
        private IInstitutionAvailabilityRepository _institutionAvailabilityRepository;

        private IAddressRepository _addressRepository;
        private IInstitutionProfileRepository _institutionProfileRepository;

        private IExperienceRepository _experienceRepository;
        private IServiceRepository _serviceRepository;

        private IDoctorProfileRepository _doctorProfileRepository;



        public UnitOfWork(HakimHubDbContext context)
        {
            _context = context;
        }

        public ISpecialityRepository SpecialityRepository
        {
            get
            {
                return _specialityRepository ??= new SpecialityRepository(_context);
            }
        }

        public IEducationRepository EducationRepository
        {
            get
            {
                return _educationRepository ??= new EducationRepository(_context);
            }
        }
        public IDoctorAvailabilityRepository DoctorAvailabilityRepository
        {
            get
            {
                return _doctorAvailabilityRepository = new DoctorAvailabilityRepository(_context);
            }
        }

        public IInstitutionAvailabilityRepository InstitutionAvailabilityRepository
        {
            get
            {
                return _institutionAvailabilityRepository = new InstitutionAvailabilityRepository(_context);
            }
        }

        public IAddressRepository AddressRepository
        {
            get
            {
                return _addressRepository = new AddressRepository(_context);
            }
        }

        public IInstitutionProfileRepository InstitutionProfileRepository
        {
            get
            {
                return _institutionProfileRepository = new InstitutionProfileRepository(_context);
            }
        }

        public IExperienceRepository ExperienceRepository
        {
            get
            {
                return _experienceRepository ??= new ExperienceRepository(_context);
            }
        }
        public IServiceRepository ServiceRepository
        {
            get
            {
                return _serviceRepository ??= new ServiceRepository(_context);
            }
        }

        public IDoctorProfileRepository DoctorProfileRepository {
            get{
                return _doctorProfileRepository??= new DoctorProfileRepository(_context);
            }
        }



        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}