using SponsorshipApp.Application.Interfaces;
using SponsorshipApp.Domain.Models;
using SponsorshipApp.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SponsorshipApp.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ISponsorshipPlanRepository _planRepo;
        private readonly ICommunityProjectRepository _projectRepo;

        public PaymentService(ISponsorshipPlanRepository planRepo, ICommunityProjectRepository projectRepo)
        {
            _planRepo = planRepo;
            _projectRepo = projectRepo;
        }

        public Payment ProcessNow(Guid planId)
        {
            var plan = _planRepo.GetById(planId);
            if (plan == null) return null;

            var payment = new Payment { PlanId = planId, Amount = plan.Amount };
            plan.Payments.Add(payment);
            plan.LastPaymentDate = DateTime.UtcNow;

            var project = _projectRepo.GetById(plan.ProjectId);
            if (project != null)
            {
                project.TotalRaised += plan.Amount;
                _projectRepo.Update(project);
            }

            _planRepo.Update(plan);

            return payment;
        }

        public List<Payment> GetPayments(Guid planId)
        {
            var plan = _planRepo.GetById(planId);
            return plan?.Payments ?? new();
        }
    }
}
