using AutoMapper;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Dtos.Admin;
using NeuroTumAI.Core.Dtos.Appointments;
using NeuroTumAI.Core.Dtos.Chat;
using NeuroTumAI.Core.Dtos.Clinic;
using NeuroTumAI.Core.Dtos.ContactUs;
using NeuroTumAI.Core.Dtos.Doctor;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Dtos.Review;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Admin;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Chat_Aggregate;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Entities.Contact_Us;
using NeuroTumAI.Core.Entities.Notification;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Service.Dtos.Account;

namespace NeuroTumAI.Service.Mappings
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<PatientRegisterDto, Patient>();

			CreateMap<Doctor, UserDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUser.Id))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture))
				.ForMember(D => D.Role, O => O.MapFrom(S => "Doctor"));

			CreateMap<Patient, UserDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUser.Id))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture))
				.ForMember(D => D.Role, O => O.MapFrom(S => "Patient"));

            CreateMap<Lab, UserDto>()
                .ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUser.Id))
                .ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
                .ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
                .ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
                .ForMember(D => D.Role, O => O.MapFrom(S => "Lab"));

            CreateMap<Clinic, ClinicWithDoctorDataDto>()
				.ForMember(D => D.DoctorProfilePicture, O => O.MapFrom(S => S.Doctor.ApplicationUser.ProfilePicture))
				.ForMember(D => D.DoctorFullName, O => O.MapFrom(S => S.Doctor.ApplicationUser.FullName))
				.ForMember(D => D.AverageStarRating, O => O.MapFrom(S => S.Doctor.Reviews.Any() ? S.Doctor.Reviews.Average(R => R.Stars) : 0));

			CreateMap<AddSlotDto, Slot>();
			CreateMap<Clinic, ClinicToReturnDto>();
			CreateMap<Slot, SlotToReturnDto>();
			CreateMap<Appointment, AppointmentToReturnDto>();
			CreateMap<Review, ReviewToReturnDto>();


			CreateMap<Patient, PublicPatientDto>()
				.ForMember(D => D.Id, O => O.MapFrom(S => S.ApplicationUserId))
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture));

			CreateMap<Doctor, DoctorWithReviewsDto>()
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture))
				.ForMember(D => D.AverageStarRating, O => O.MapFrom(D => D.Reviews.Any() ? D.Reviews.Average(R => R.Stars) : 0));

			CreateMap<Appointment, AppoitntmentWithPatientDto>();

			CreateMap<ChatMessage, MessageToReturnDto>();

			CreateMap<ApplicationUser, ChatUserDto>();

			CreateMap<Admin, AdminToReturnDto>();

			CreateMap<Conversation, ConversationWithLastMessageToReturnDto>()
				.ForMember(D => D.User, O => O.MapFrom(S => S.FirstUser))
				.ForMember(D => D.LastMessage, O => O.MapFrom(S => S.ChatMessages.FirstOrDefault()));


			CreateMap<Conversation, ConversationDto>()
				.ForMember(D => D.User, O => O.MapFrom(S => S.FirstUser));

			CreateMap<Doctor, PendingDoctorDto>()
				.ForMember(D => D.FullName, O => O.MapFrom(S => S.ApplicationUser.FullName))
				.ForMember(D => D.UserName, O => O.MapFrom(S => S.ApplicationUser.UserName))
				.ForMember(D => D.Gender, O => O.MapFrom(S => S.ApplicationUser.Gender))
				.ForMember(D => D.DateOfBirth, O => O.MapFrom(S => S.ApplicationUser.DateOfBirth))
				.ForMember(D => D.Email, O => O.MapFrom(S => S.ApplicationUser.Email))
				.ForMember(D => D.ProfilePicture, O => O.MapFrom(S => S.ApplicationUser.ProfilePicture));

			CreateMap<Clinic, PendingClinicDto>()
				.ForMember(D => D.DoctorName, O => O.MapFrom(S => S.Doctor.ApplicationUser.FullName))
				.ForMember(D => D.DoctorProfilePicture, O => O.MapFrom(S => S.Doctor.ApplicationUser.ProfilePicture));

			CreateMap<ContactUsMessage, ContactUsMessageToReturnDto>()
				.ForMember(D => D.PatientName, O => O.MapFrom(S => S.Patient.ApplicationUser.FullName))
				.ForMember(D => D.PatientProfilePicture, O => O.MapFrom(S => S.Patient.ApplicationUser.ProfilePicture))
				.ForMember(D => D.PatientEmail, O => O.MapFrom(S => S.Patient.ApplicationUser.Email));

			CreateMap<Appointment, AppointmentWithDoctorDto>()
				.ForMember(D => D.DoctorProfilePicture, O => O.MapFrom(S => S.Doctor.ApplicationUser.ProfilePicture))
				.ForMember(D => D.DoctorName, O => O.MapFrom(S => S.Doctor.ApplicationUser.FullName))
				.ForMember(D => D.Address, O => O.MapFrom(S => S.Clinic.Address));

			CreateMap<Notification, NotificationToReturnDto>()
				.ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, member, context) =>
					context.Items.TryGetValue("Language", out var lang) && lang?.ToString() == "ar" ? src.TitleAR : src.TitleEN))
				.ForMember(dest => dest.Body, opt => opt.MapFrom((src, dest, member, context) =>
					context.Items.TryGetValue("Language", out var lang) && lang?.ToString() == "ar" ? src.BodyAR : src.BodyEN));
		}
	}
}
