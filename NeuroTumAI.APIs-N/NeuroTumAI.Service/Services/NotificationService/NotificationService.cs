using Microsoft.Extensions.Logging;
using NeuroTumAI.Core;
using NeuroTumAI.Core.Dtos.Notification;
using NeuroTumAI.Core.Entities.Notification;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.NotificationSpecs;
using NeuroTumAI.Core.Specifications.PatientSpecs;

namespace NeuroTumAI.Service.Services.NotificationService
{
	public class NotificationService : INotificationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFireBaseNotificationService _fireBaseNotificationService;
		private readonly ILogger<NotificationService> _logger;

		public NotificationService(IUnitOfWork unitOfWork, IFireBaseNotificationService fireBaseNotificationService, ILogger<NotificationService> logger)
		{
			_unitOfWork = unitOfWork;
			_fireBaseNotificationService = fireBaseNotificationService;
			_logger = logger;
		}

		public Task<IReadOnlyList<Notification>> GetNotificationsAsync(string userId, NotificationSpecParams specParams)
		{
			var notificationSpecs = new NotificationSpecifications(userId, specParams);
			return _unitOfWork.Repository<Notification>().GetAllWithSpecAsync(notificationSpecs);
		}

		public Task<int> GetNotificationsCountAsync(string userId)
		{
			var notificationSpecs = new NotificationSpecifications(userId);
			return _unitOfWork.Repository<Notification>().GetCountAsync(notificationSpecs);
		}

		public async Task SendAppointmentCancellationNotificationsAsync(List<AppointmentCancellationNotificationDto> notifications)
		{
			var patientIds = notifications.Select(N => N.PatientId);

			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpecs = new PatientSpecifications(patientIds);

			var patients = await patientRepo.GetAllWithSpecAsync(patientSpecs);

			var notificationRepo = _unitOfWork.Repository<Notification>();

			foreach (var notification in notifications)
			{
				var patient = patients.Where(P => P.Id == notification.PatientId).FirstOrDefault()!;
				var tokens = patient.ApplicationUser.DeviceTokens;

				string titleEN = "Your Appointment has been Cancelled";
				string bodyEN = $"We regret to inform you that your appointment scheduled for {notification.Date:MMMM dd, yyyy} has been cancelled.";
				string titleAR = "تم إلغاء موعدك";
				string bodyAR = $"نأسف لإبلاغك بأن موعدك المحدد في {notification.Date:dd MMMM yyyy} قد تم إلغاؤه.";

				var newNotification = new Notification()
				{
					TitleEN = titleEN,
					TitleAR = titleAR,
					BodyEN = bodyEN,
					BodyAR = bodyAR,
					Type = NotificationType.AppointmentCancellation,
					ApplicationUserId = patient.ApplicationUserId
				};

				notificationRepo.Add(newNotification);

				if (tokens.Any())
				{
					foreach (var token in tokens)
					{
						_fireBaseNotificationService.SendNotificationAsync(titleEN, bodyEN, token.FcmToken, NotificationType.AppointmentCancellation);
					}
				}
			}

			await _unitOfWork.CompleteAsync();
		}

		public async Task SendAppointmentTimeChangeNotificationsAsync(List<AppointmentTimeChangeNotificationDto> notifications, TimeOnly time)
		{
			var patientIds = notifications.Select(N => N.PatientId);

			var patientRepo = _unitOfWork.Repository<Patient>();
			var patientSpecs = new PatientSpecifications(patientIds);

			var patients = await patientRepo.GetAllWithSpecAsync(patientSpecs);

			var notificationRepo = _unitOfWork.Repository<Notification>();

			foreach (var notification in notifications)
			{
				var patient = patients.Where(P => P.Id == notification.PatientId).FirstOrDefault()!;
				var tokens = patient.ApplicationUser.DeviceTokens;

				string titleEN = "Your Appointment Time Has Been Changed";
				string bodyEN = $"Please note that your appointment on {notification.Date:MMMM dd, yyyy} has been rescheduled from {notification.OldTime:hh:mm tt} to {time:hh:mm tt}.";

				string titleAR = "تم تعديل وقت موعدك";
				string bodyAR = $"يرجى ملاحظة أن موعدك بتاريخ {notification.Date:dd MMMM yyyy} قد تم تغييره من {notification.OldTime:hh:mm tt} إلى {time:hh:mm tt}.";

				var newNotification = new Notification()
				{
					TitleEN = titleEN,
					TitleAR = titleAR,
					BodyEN = bodyEN,
					BodyAR = bodyAR,
					Type = NotificationType.AppointmentTimeChange,
					ApplicationUserId = patient.ApplicationUserId
				};

				notificationRepo.Add(newNotification);

				if (tokens.Any())
				{
					foreach (var token in tokens)
					{
						_fireBaseNotificationService.SendNotificationAsync(titleEN, bodyEN, token.FcmToken, NotificationType.AppointmentTimeChange);
					}
				}
			}

			await _unitOfWork.CompleteAsync();
		}
	}
}
