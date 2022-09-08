using MoanaSoftwareTrello.Models;

namespace MoanaSoftwareTrello.Services
{
    public class ApiService //: IHostedService
    {
        private swaggerClient swc;
        public ApiService()
        {
            swc = new swaggerClient("http://193.201.187.29:84", new HttpClient());
        }
        public async Task<SignInResponse> Login(User loginUser)
        {
            SignInResponse t;
            UserCredentialRequest user = new UserCredentialRequest();
            user.Email = loginUser.Email;
            user.Password = loginUser.Password;
            return await swc.SignInAsync(user);
        }


        public async Task<string> RegisterUser(User sourceUser)
        {
            await Task.Run(async () =>
            {
                UserCredentialRequest user = new UserCredentialRequest();
                user.Email = sourceUser.Email;
                user.Password = sourceUser.Password;
                await swc.SignUpAsync(user);
            });
            return "success";
        }
        public async Task<List<GetAllCardResponse>> GetAllCard(string token)
        {
            return (List<GetAllCardResponse>)await swc.GetAllCardAsync(token);
        }
        public async Task<GetCardResponse> GetCardById(Guid? cardId, string token)
        {
            return await swc.GetByIdAsync(cardId, token);
        }
        public async Task CreateCard(AddCardRequest card,string token)
        {
            await swc.AddAsync(card, token);
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
