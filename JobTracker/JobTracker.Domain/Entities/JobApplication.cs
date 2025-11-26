using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class JobApplication
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Company { get; private set; }
        public string Position { get; private set; }
        public ApplicationStatus Status { get; private set; }
        public DateTime AppliedDate { get; private set; }
        public string? JobUrl { get; private set; }
        public decimal? SalaryExpectation { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public User User { get; private set; }

        public ICollection<Interview> Interviews { get; private set; }
        public Offer? Offer { get; private set; }

        public JobApplication(int userId,string company,string position,DateTime appliedDate,string? jobUrl,decimal? salaryExpectation,string? notes) {

            UserId = userId;
            Company = company;
            Position = position;
            AppliedDate = appliedDate;
            Status = ApplicationStatus.Applied;
            CreatedAt = DateTime.UtcNow;
            Interviews = new List<Interview>();

            JobUrl = jobUrl;
            SalaryExpectation = salaryExpectation;  
            Notes = notes;
        }

        public DateTime UpdateStatus(ApplicationStatus status) { 
            Status = status;
            return UpdatedAt = DateTime.UtcNow;
        }

    }
}