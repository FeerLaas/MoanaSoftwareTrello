using MoanaSoftwareTrello.Models;

namespace MoanaSoftwareTrello.Services
{
    public class ApiService
    {
        private swaggerClient swc;
        public ApiService(IConfiguration configuration)
        {
            swc = new swaggerClient(configuration.GetValue<string>("ApiUrl:Path"), new HttpClient());
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
        public async Task<GetCardResponse> GetCardById(string cardId, string token)
        {
            return await swc.GetByIdAsync(cardId, token);
        }
        public async Task CreateCard(AddCardRequest card,string token)
        {
            await swc.AddAsync(card, token);
        }
        public async Task UpdateCard(UpdateCardRequest card, string token)
        {
            await swc.UpdateAsync(card, token);
        }
        public async Task DeleteCard(DeleteCardRequest card,string token)
        {
            await swc.DeleteAsync(card, token);
        }
        public async Task<List<GetAllUserResponse>> GetAllUser(string token)
        {
            return (List<GetAllUserResponse>)await swc.GetAllAsync(token);
        }


    }
}
