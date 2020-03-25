using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestUser.Models;
using GestUser.Security;
using Microsoft.EntityFrameworkCore;
using Service;

namespace GestUser.Service
{
        public class UserService : IUserService
        {
            private readonly NanshopDbContext nanshopDbDbContext;
            public UserService(NanshopDbContext nanshopDbDbContext)
            {
                this.nanshopDbDbContext =nanshopDbDbContext;
            }

            public async Task<Utenti> GetUser(string UserId)
            {
                return await this.nanshopDbDbContext.Utenti
                    .Where(c => c.UserId == UserId)
                    .Include(r => r.Profili)
                    //.Where(c => c.UserId  == username && c.Password == password)
                    //.Where(c => c.Password == password) 
                    .FirstOrDefaultAsync();
            }

            public Utenti CheckUser(string UserId)
            {
                return this.nanshopDbDbContext.Utenti
                    .AsNoTracking()
                    .Where(c => c.UserId == UserId)
                    .Include(r => r.Profili)
                    .FirstOrDefault();
            }

            public Utenti CheckUserByFid(string CodFid)
            {
                return this.nanshopDbDbContext.Utenti
                    .AsNoTracking()
                    .Where(c => c.CodFidelity == CodFid)
                    .FirstOrDefault();
            }

            public async Task<ICollection<Utenti>> GetAll()
            {
                return await this.nanshopDbDbContext.Utenti
                    .Include(r => r.Profili)
                    .OrderBy(c => c.UserId)
                    .ToListAsync();
            }

            public bool Authenticate(string username, string password)
            {
                bool retVal = false;

                PasswordHasher Hasher = new PasswordHasher();

                Utenti utente = this.nanshopDbDbContext.Utenti
                    .Include(r => r.Profili)
                    .Where(c => c.UserId == username)
                    .FirstOrDefault();

                if (utente != null)
                {
                    string EncryptPwd = utente.Password;

                    retVal = Hasher.Check(EncryptPwd, password).Verified;
                }

                return retVal;
            }

            public bool InsUtente(Utenti utente)
            {
                this.nanshopDbDbContext.Add(utente);
                return Salva();
            }

            public bool UpdUtente(Utenti utente)
            {
                this.nanshopDbDbContext.Update(utente);
                return Salva();
            }

            public bool DelUtente(Utenti utente)
            {
                this.nanshopDbDbContext.Remove(utente);
                return Salva();
            }

            public bool Salva()
            {
                var saved = this.nanshopDbDbContext.SaveChanges();
                return saved >= 0 ? true : false;
            }

        }
    }
