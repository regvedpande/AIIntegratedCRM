using AIIntegratedCRM.Application.Features.Leads.DTOs;
using AIIntegratedCRM.Application.Features.Contacts.DTOs;
using AIIntegratedCRM.Application.Features.Accounts.DTOs;
using AIIntegratedCRM.Application.Features.Opportunities.DTOs;
using AIIntegratedCRM.Application.Features.Activities.DTOs;
using AIIntegratedCRM.Application.Features.SupportTickets.DTOs;
using AIIntegratedCRM.Domain.Entities;
using AutoMapper;

namespace AIIntegratedCRM.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Lead, LeadDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName));
        CreateMap<Contact, ContactDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName));
        CreateMap<Account, AccountDto>();
        CreateMap<Opportunity, OpportunityDto>();
        CreateMap<Activity, ActivityDto>();
        CreateMap<SupportTicket, SupportTicketDto>();
        CreateMap<TicketComment, TicketCommentDto>();
    }
}
