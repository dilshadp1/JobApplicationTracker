using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.OffersQuery.GetOffers
{
    public class GetOffersQueryHandler(IOfferRepository offerRepository)
            : IRequestHandler<GetOffersQuery, List<OfferDto>>
    {
        public async Task<List<OfferDto>> Handle(GetOffersQuery query, CancellationToken cancellationToken)
        {
            List<Offer> offers = await offerRepository.GetAllOffersByUserIdAsync(query.UserId);

            return offers.Select(o => new OfferDto
            {
                Id = o.Id,
                ApplicationId = o.ApplicationId,
                Salary = o.Salary,
                OfferDate = o.OfferDate,
                Deadline = o.Deadline,
                Benefits = o.Benefits,

                CompanyName = o.JobApplication.Company,
                JobPosition = o.JobApplication.Position
            }).ToList();
        }
    }
}
