//using AuthenticationService.Repositories;
//using EventBus.Events;
//using EventBus.Implementations;

//namespace AuthenticationService.Sevices.ProfileCompletionConsumerService
//{
//    public class ProfileCompletionConsumer : EventConsumer
//    {
//        private readonly IServiceProvider _services;

//        public ProfileCompletionConsumer(
//            RabbitMQConnection connection,
//            IServiceProvider services
//        ) : base(connection)
//        {
//            _services = services;
//            Subscribe<ProfileCompletedEvent>("auth-profile-completed", HandleEvent);
//        }

//        private async void HandleEvent(ProfileCompletedEvent @event)
//        {
//            using var scope = _services.CreateScope();
//            var authRepo = scope.ServiceProvider.GetRequiredService<IAuthRepository>();
//            await authRepo.MarkProfileAsCompleted(@event.UserId);
//        }
//    }


//    //public class ProfileCompletionConsumer : EventConsumer
//    //{
//    //    private readonly IServiceProvider _services;

//    //    public ProfileCompletionConsumer(IServiceProvider services)
//    //        : base(services.CreateScope().ServiceProvider.GetRequiredService<RabbitMQConnection>())
//    //    {
//    //        _services = services;
//    //        Subscribe<ProfileCompletedEvent>("auth-profile-completed", HandleEvent);
//    //    }

//    //    private async void HandleEvent(ProfileCompletedEvent @event)
//    //    {
//    //        using var scope = _services.CreateScope();
//    //        var authRepo = scope.ServiceProvider.GetRequiredService<IAuthRepository>();
//    //        await authRepo.MarkProfileAsCompleted(@event.UserId);
//    //    }
//}



