using AutoMapper;
using EduPlatform.Application.DTOs.Payment;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<Payment, PaymentDTO>();

            CreateMap<PaymentDTO, Payment>()
                .ForMember(dest => dest.Enrollment, op => op.Ignore());
        }
    }
}
