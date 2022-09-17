using MVC_StokTakip.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MVC_StokTakip.Roller
{
    public class KullaniciRoller : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        MVC_StokTakipEntities db = new MVC_StokTakipEntities();
        public override string[] GetRolesForUser(string username)
        {
            //var kullanici = db.Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == username);
            //return new string[] { kullanici.Rol };
            List<KullaniciRolleri> kullanicirolleri = db.KullaniciRolleri.Where(x => x.Kullanicilar.KullaniciAdi == username).ToList();
            string[] roller = new string[kullanicirolleri.Count];
            if (kullanicirolleri.Count > 0)
            {
                for (int i = 0; i < roller.Length; i++)
                {
                    foreach (var item in kullanicirolleri)
                    {
                        roller[i] = item.Roller.Rol.Trim();
                        kullanicirolleri.Remove(item); 
                        break;
                    }
                }
                return roller;
            }
            return new string[] { "" };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}