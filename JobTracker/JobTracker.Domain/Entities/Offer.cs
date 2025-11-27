using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class Offer
    {
        public int Id { get; private set; }
        public int ApplicationId { get; private set; }
        public decimal Salary { get; private set; }
        public DateTime OfferDate { get; private set; }
        public DateTime? Deadline { get; private set; }
        public string? Benefits { get; private set; }

        public JobApplication JobApplication { get; private set; }

        private Offer() { }
        public Offer(int applicationId, decimal salary, DateTime offerDate, DateTime? deadline, string? benefits)
        {
            ApplicationId = applicationId;
            Salary = salary;
            OfferDate = offerDate;
            Deadline = deadline;
            Benefits = benefits;
        }
    }
}
