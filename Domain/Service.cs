using Domain.Common;

namespace Domain;

public class Service : BaseDomainEntity
{
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public ICollection<InstitutionProfile> Institutions { get; set; } = new List<InstitutionProfile>();
}
