
using static Domain.DoctorProfile;

public interface IDoctorProfileDto 
    {
        public string FullName{get;set;}
        public string About{get;set;}
        public string Email{get;set;}
        public DateTime CareerStartTime {get;set;}
        public GenderType Gender{get;set;}
        
    }
