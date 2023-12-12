
using MediatR;
using Application.Responses;
using Application.Features.InstitutionProfiles.DTOs;

namespace Application.Features.InstitutionProfiles.CQRS.Queries
{
    public class InstitutionProfileSearchQuery: IRequest<Result<List<InstitutionProfileDto>>>
    {
        public ICollection<string>? ServiceNames { get; set; } = new List<string>();
        public int OperationYears { get; set; } = -1;
        public bool OpenStatus { get; set; } = false;
        public string Name { get; set; } = "";
        public int pageNumber {get;set;} = 0;
        public int pageSize {get;set;} = 0;
        public double? latitude {get;set;}
        public double? longitude {get;set;}
        public double? maxDistance {get;set;}

    }
}