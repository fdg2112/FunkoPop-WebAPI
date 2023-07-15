using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Google.Apis.Util.Store;
using System.Threading.Tasks;
using System.Threading;

namespace Logic
{
    public class UserLogic : BaseLogic, ICRUD<User, int>
    {
        public UserLogic() { }

        private readonly string clientId = "786922175766-loef3sj7qqqo008nmrb8btqbhgijnghm.apps.googleusercontent.com";
        private readonly string clientSecret = "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";
        private readonly string redirectUri = "TU_REDIRECT_URI";

        public async Task<User> AuthenticateWithGoogle(string code)
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { Oauth2Service.Scope.UserinfoProfile, Oauth2Service.Scope.UserinfoEmail },
                DataStore = new FileDataStore("TokenStorage")
            });

            var token = await flow.ExchangeCodeForTokenAsync("", code, redirectUri, CancellationToken.None);

            var credentials = new UserCredential(flow, "", token);

            var oauthService = new Oauth2Service(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credentials
            });

            var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();

            var user = new User
            {
                Uid = userInfo.Id,
                Email = userInfo.Email,
                Firstname = userInfo.GivenName,
                Lastname = userInfo.FamilyName
            };
             
            return user;
        }
        public List<User> GetAll()
        {
            try
            {
                return _context.User.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios.", ex);
            }
        }

        public bool Finded(int id)
        {
            if (_context.User.Find(id) != null) return true;
            else return false;
        }

        public void Add(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
                if (string.IsNullOrEmpty(user.Firstname)) throw new ArgumentException("El campo Nombre del usuario no puede estar vacío.", nameof(user.Firstname));
                if (string.IsNullOrEmpty(user.Lastname)) throw new ArgumentException("El campo Apellido del usuario no puede estar vacío.", nameof(user.Lastname));
                if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("El campo Email del usuario no puede estar vacío.", nameof(user.Email));
                if (user.Firstname.Length > 255) throw new ArgumentException("El límite del campo Nombre de Usuario es de 255 caracteres.", nameof(user.Firstname));
                if (user.Lastname.Length > 255) throw new ArgumentException("El límite del campo Apellido de Usuario es de 255 caracteres.", nameof(user.Lastname));
                if (user.Email.Length > 45) throw new ArgumentException("El límite del campo Email de Usuario es de 45 caracteres.", nameof(user.Email));
                if (user.Phone != null && user.Phone.Length > 20) throw new ArgumentException("El límite del campo Teléfono de Usuario es de 20 caracteres.", nameof(user.Phone));
                if (string.IsNullOrEmpty(user.Role)) throw new ArgumentException("No se especificó el rol del usuario.", nameof(user.Role));
                _context.User.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar agregar el usuario. " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var elementToRemove = _context.User.Find(id) ?? throw new Exception($"El usuario con ID {id} no existe.");
                _context.User.Remove(elementToRemove);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException.InnerException;
                if (innerException != null && innerException.Message.Contains("FK_"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(innerException.Message, @"(?<=tabla ')([^']*)");
                    if (match.Success)
                    {
                        string tableName = match.Value;
                        throw new Exception($"No se puede eliminar el usuario porque está relacionado con la tabla {tableName}.");
                    }
                }
                throw new Exception("No se ha podido eliminar el usuario.");
            }
            catch (Exception ex)
            {
                throw new Exception($"No se ha podido eliminar el usuario. {ex.Message}");
            }
        }

        public void Update(User user)
        {
            try
            {
                if (user == null) throw new ArgumentNullException(nameof(user), "El usuario no puede ser nulo.");
                var existingUser = _context.User.FirstOrDefault(u => u.Id == user.Id) ?? throw new ArgumentException("El usuario no existe en la base de datos.", nameof(user.Id));
                existingUser.Firstname = user.Firstname;
                existingUser.Lastname = user.Lastname;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Role = user.Role;
                existingUser.Active = user.Active;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al intentar actualizar el usuario. " + ex.Message, ex);
            }
        }

        public User Get(int id)
        {
            try
            {
                return _context.User.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario.", ex);
            }
        }
    }
}
