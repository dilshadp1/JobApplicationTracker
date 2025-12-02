using JobTracker.Application.DTO;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.OffersQuery.GetOffersById
{
    public class GetOfferByIdQueryHandler(IOfferRepository offerRepository) : IRequestHandler<GetOfferByIdQuery, OfferDto>
    {
        public async Task<OfferDto> Handle(GetOfferByIdQuery query, CancellationToken cancellationToken)
        {
            Offer? offer = await offerRepository.GetOfferByIdWithJobAsync(query.Id);

            if (offer == null)
            {
                throw new KeyNotFoundException($"Offer with ID {query.Id} not found.");
            }

            if (offer.JobApplication.UserId != query.UserId)
            {
                throw new UnauthorizedAccessException("Not your offer.");
            }

            return new OfferDto
            {
                Id = offer.Id,
                ApplicationId = offer.ApplicationId,
                Salary = offer.Salary,
                OfferDate = offer.OfferDate,
                Deadline = offer.Deadline,
                Benefits = offer.Benefits,
                CompanyName = offer.JobApplication.Company, 
                JobPosition = offer.JobApplication.Position
            };
        }
    }
}
