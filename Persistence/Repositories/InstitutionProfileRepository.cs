using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InstitutionProfileRepository : GenericRepository<InstitutionProfile>, IInstitutionProfileRepository
    {

        private readonly HakimHubDbContext _dbContext;

        public InstitutionProfileRepository(HakimHubDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<InstitutionProfile>> GetAllPopulated()
        {
            return await _dbContext.Set<InstitutionProfile>()
                .Include(x => x.Address)
                .Include(x => x.Logo)
                .Include(x => x.Banner)
                .Include(x => x.InstitutionAvailability)
                .Include(x => x.Services)
                .Include(x => x.Banner)
                .AsNoTracking().ToListAsync();
        }

        public async Task<InstitutionProfile> GetPopulatedInstitution(Guid id)
        {
            return await _dbContext.Set<InstitutionProfile>()
                .Include(x => x.Address)
                .Include(x => x.Logo)
                .Include(x => x.Banner)
                .Include(x => x.Services)
                .Include(x => x.Photos)
                .Include(x => x.InstitutionAvailability)
                .Include(x => x.Doctors)
                    .ThenInclude(doctor => doctor.Photo) // Include the Photo of each Doctor
                .Include(x => x.Doctors)
                    .ThenInclude(doctor => doctor.Specialities) // Include the Specialities of each Doctor
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<InstitutionProfile>> GetByYears(int years)
        {
            DateTime startDate = DateTime.Today.AddYears(-years);
            return await _dbContext.Set<InstitutionProfile>()
                .Include(x => x.InstitutionAvailability)
                .Where(x => x.EstablishedOn <= startDate)
                .ToListAsync();
        }

        public async Task<List<InstitutionProfile>> GetByService(Guid ServiceId)
        {
            return await _dbContext.Set<InstitutionProfile>()
                .Include(x => x.Services)
                .Where(x => x.Services.Any(s => s.Id == ServiceId))
                .ToListAsync();
        }

        public async Task<List<InstitutionProfile>> Search(ICollection<string> serviceNames, int operationYears, bool openStatus, string name, int pageNumber, int pageSize, double? latitude, double? longitude, double? maxDistance)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                MaxDepth = 32
            };

            IQueryable<InstitutionProfile> query = _dbContext.Set<InstitutionProfile>()
                .Include(x => x.Address)
                .Include(x => x.Logo)
                .Include(x => x.Banner)
                .Include(x => x.Services)
                .Include(x => x.Photos)
                .Include(x => x.InstitutionAvailability)
                .Include(x => x.Doctors)
                    .ThenInclude(doctor => doctor.Photo) // Include the Photo of each Doctor
                .Include(x => x.Doctors)
                    .ThenInclude(doctor => doctor.Specialities);

            
            

                    // Filter by latitude, longitude, and distance
            if (latitude.HasValue && longitude.HasValue && maxDistance.HasValue)
            {
                double userLat = latitude.Value;
                double userLon = longitude.Value;
                double maxDist = maxDistance.Value;

                query = query.Where(x =>
                    _dbContext.CalculateDistance(userLat, userLon, x.Address.Latitude ?? 0, x.Address.Longitude ?? 0) <= maxDist
                );
            }


            // Filter by institution name
            if (!string.IsNullOrEmpty(name))
            {
                string searchTerm = name.ToLower();
                query = query.Where(x => x.InstitutionName.ToLower().Contains(searchTerm));
            }

            // Filter by service names
            foreach (string serviceName in serviceNames)
            {
                if (!string.IsNullOrEmpty(serviceName))
                {
                    query = query.Where(x => x.Services.Any(service => service.ServiceName == serviceName));
                }
            }

            // Filter by operation years
            if (operationYears > 0)
            {
                DateTime startDate = DateTime.Today.AddYears(-operationYears);
                query = query.Where(x => x.EstablishedOn <= startDate);
            }

            if (openStatus)
            {
                var currentDate = DateTime.UtcNow.Date;
                var currentTime = DateTime.UtcNow.TimeOfDay;

                var institutionIds = query.Select(x => x.Id).ToList();
                var currentDayOfWeek = (int)currentDate.DayOfWeek + 1; // Adding 1 to match the numbering convention

                var availabilities = await _dbContext.Set<InstitutionAvailability>()
                    .Where(avail => institutionIds.Contains(avail.InstitutionId) &&
                        (avail.StartDay <= avail.EndDay
                            ? (avail.StartDay <= (DayOfWeek)(currentDayOfWeek % 7) && (DayOfWeek)(currentDayOfWeek % 7) <= avail.EndDay)
                            : (avail.StartDay <= (DayOfWeek)(currentDayOfWeek % 7) || (DayOfWeek)(currentDayOfWeek % 7) <= avail.EndDay))
                    )
                    .ToListAsync();

                var profiles = await query.ToListAsync();
                profiles = profiles.Where(x => x.InstitutionAvailability != null &&
                    availabilities.Any(a => a.InstitutionId == x.Id &&
                        (a.TwentyFourHours ||
                         (TimeSpan.Parse(a.Opening) <= currentTime && currentTime <= TimeSpan.Parse(a.Closing))))
                ).ToList();

                // Apply pagination if page number and page size are given
                if (pageNumber > 0 && pageSize > 0)
                {
                    int filteredTotalCount = profiles.Count;
                    int totalPages = (int)Math.Ceiling((double)filteredTotalCount / pageSize);
                    int skip = (pageNumber - 1) * pageSize;
                    profiles = profiles.Skip(skip).Take(pageSize).ToList();
                }

                return profiles;
                
            }

        // Apply pagination if page number and page size are given
            if (pageNumber > 0 && pageSize > 0)
            {
                int skip = (pageNumber - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
            }

    
            return await query.ToListAsync();
        }





        public async Task<List<InstitutionProfile>> Search(string institutionName)
        {
            IQueryable<InstitutionProfile> query = _dbContext.Set<InstitutionProfile>()
                .Include(x => x.Services)
                .Include(x => x.InstitutionAvailability)
                .Include(x => x.Logo)
                .Include(x => x.Banner)
                .Include(x => x.Address);

            if (!string.IsNullOrEmpty(institutionName))
            {
                string searchTerm = institutionName.ToLower();
                query = query.Where(x => x.InstitutionName.ToLower().Contains(searchTerm));
            }

            return await query.ToListAsync();
        }




        private static bool IsTimeWithinRange(string opening, string closing, TimeSpan currentTime)
        {
            if (TimeSpan.TryParseExact(opening, "hh\\:mm", CultureInfo.InvariantCulture, out var openingTime) &&
                TimeSpan.TryParseExact(closing, "hh\\:mm", CultureInfo.InvariantCulture, out var closingTime))
            {
                return openingTime <= currentTime && closingTime >= currentTime;
            }
            return false;
        }

    private static double  CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // Earth's radius in kilometers

        double latRad1 = DegreesToRadians(lat1);
        double lonRad1 = DegreesToRadians(lon1);
        double latRad2 = DegreesToRadians(lat2);
        double lonRad2 = DegreesToRadians(lon2);

        double deltaLat = latRad2 - latRad1;
        double deltaLon = lonRad2 - lonRad1;

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(latRad1) * Math.Cos(latRad2) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;

        return distance;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }


    }
}