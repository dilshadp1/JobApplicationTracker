using JobTracker.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Query.OffersQuery.GetOffers
{
    public class GetOffersQuery : IRequest<List<OfferDto>>
    {
        public int UserId { get; set; }
    }
}
