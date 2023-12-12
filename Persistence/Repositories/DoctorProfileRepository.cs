using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.Persistence;
using Application.Features.Specialities.DTOs;
using Domain;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static Domain.DoctorProfile;

namespace Persistence.Repositories
{
    public class DoctorProfileRepository : GenericRepository<DoctorProfile>, IDoctorProfileRepository
    {
        private readonly HakimHubDbContext _dbContext;
        public DoctorProfileRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       

        public async Task<DoctorProfile> GetDoctorProfileDetail(Guid Id)
        {
            var doctorProfile = await _dbContext.DoctorProfiles
            .Where(d => d.Id == Id)
            .Include(d => d.Photo)
            .Include(d => d.MainInstitution)
            .Include(d => d.Educations)
                .ThenInclude(p => p.EducationInstitutionLogo)
            .Include(d => d.Specialities)
            .Include(d => d.Experiences)
            .FirstOrDefaultAsync();


            return doctorProfile;
        }


        public async Task<List<DoctorProfile>> FilterDoctors(Guid? institutionId, string? Name, ICollection<string>? specialityNames, int experienceYears, string? educationInstitutionName,int pageNumber, int pageSize)
        {

            IQueryable<DoctorProfile> query = _dbContext.Set<DoctorProfile>()
            .Include(d => d.Photo)
            .Include(d => d.MainInstitution)
            .Include(d => d.Educations)
            .Include(d => d.Specialities)
            .Include(d => d.Experiences);
    
            // Filter by Institution ID
            if (institutionId != Guid.Empty && institutionId != new Guid())
            {
                query = query.Where(d => d.Institutions.Any(i => i.Id == institutionId))
                .Include(d => d.Specialities).Include(e => e.Educations);
            }

            // Filter by doctor name
            if (!string.IsNullOrEmpty(Name))
            {
                string searchTerm = Name.ToLower();
                query = query.Where(x => x.FullName.ToLower().Contains(searchTerm));
            }

            if (specialityNames != null && specialityNames.Any())
            {
                query = query.Where(x => x.Specialities.Any(Speciality => specialityNames.Contains(Speciality.Name)));
            }

            if (!string.IsNullOrEmpty(educationInstitutionName))
            {
                query = query.Where(d => d.Educations.Any(e => e.EducationInstitution == educationInstitutionName));
            }

            if (experienceYears >= 0)
            {
                DateTime startDate = DateTime.Today.AddYears(-experienceYears);
                query = query.Where(x => x.CareerStartTime <= startDate);
            }


             if (pageNumber > 0 && pageSize > 0)
            {
                int totalCount = await query.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                int skip = (pageNumber - 1) * pageSize;
                var pagedQuery = query.Skip(skip).Take(pageSize);
                return await pagedQuery.ToListAsync();
            }
            
            return await query.ToListAsync();


        }

       public async Task<List<DoctorProfile>> GetAllDoctors(){
         IQueryable<DoctorProfile> query = _dbContext.Set<DoctorProfile>()
            .Include(d => d.Photo)
            .Include(d => d.MainInstitution)
            .Include(d => d.Specialities);
            return await query.ToListAsync();
       }
    }
}