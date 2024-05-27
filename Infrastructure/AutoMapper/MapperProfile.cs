using AutoMapper;
using Domain;
using Domain.DTOs.BookingDTOs;
using Domain.DTOs.PaymentDTOs;
using Domain.DTOs.RoomDTOs;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Booking, AddBookingDto>().ReverseMap();
        CreateMap<Booking, GetBookingDto>().ReverseMap();
        CreateMap<Booking, UpdateBookingDto>().ReverseMap();
        
        CreateMap<Payment, AddPaymentDto>().ReverseMap();
        CreateMap<Payment, GetPaymentDto>().ReverseMap();
        CreateMap<Payment, UpdatePaymentDto>().ReverseMap();
        
        CreateMap<Room, AddRoomDto>().ReverseMap();
        CreateMap<Room, GetRoomDto>().ReverseMap();
        CreateMap<Room, UpdateRoomDto>().ReverseMap();




        
    }
}