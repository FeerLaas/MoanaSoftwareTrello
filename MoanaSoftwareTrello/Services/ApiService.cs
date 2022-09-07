namespace MoanaSoftwareTrello.Services
{
    public class ApiService //: IHostedService
    {
        private swaggerClient swc;
        public ApiService()
        {
             
        }

        //public Task StartAsync(CancellationToken cancellationToken)
        //{
        //    swc = new swaggerClient("http://193.201.187.29:84", new HttpClient());


        //    Task.Run(async () => {
        //        UserCredentialRequest user = new UserCredentialRequest();
        //        user.Email = "test@gmail.com";
        //        user.Password = "test";
        //        var t =await swc.SignUpAsync(user);

        //    });
        //    return Task.CompletedTask;
        //}

        //public Task StopAsync(CancellationToken cancellationToken)
        //{

        //    return Task.CompletedTask;
        //}
    }
}
