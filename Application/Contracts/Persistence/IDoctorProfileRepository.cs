using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Specialities.DTOs;
using Domain;
using static Domain.DoctorProfile;

namespace Application.Contracts.Persistence
{
    public interface IDoctorProfileRepository : IGenericRepository<DoctorProfile>
    {
        public Task<DoctorProfile> GetDoctorProfileDetail(Guid Id);
        public Task<List<DoctorProfile>> GetAllDoctors();

        public Task<List<DoctorProfile>> FilterDoctors(Guid? institutionId, string? Name, ICollection<string>? specialityNames = null, int experienceYears = -1, string? educationInstitutionName = null,int pageNumber=0, int pageSize = 0);
        
    }
}