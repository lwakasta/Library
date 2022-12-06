using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.Entity;

namespace Library.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };
            using (LibraryContext db = new LibraryContext())
            {
                Reader reader = db.Readers.Include(u => u.Role).FirstOrDefault(u => u.Login == username);
                if (reader != null && reader.Role != null)
                {
                    roles = new string[] { reader.Role.Name };
                } else if (reader == null)
                {
                    Librarian librarian = db.Librarians.Include(u => u.Role).FirstOrDefault(u => u.Login == username);
                    if (librarian != null && librarian.Role != null)
                    {
                        roles = new string[] { librarian.Role.Name };
                    }
                }
                return roles;
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (LibraryContext db = new LibraryContext())
            {
                Reader reader = db.Readers.Include(u => u.Role).FirstOrDefault(u => u.Login == username);
                if (reader != null && reader.Role != null && reader.Role.Name == roleName)
                    return true;
                else if (reader == null)
                {
                    Librarian librarian = db.Librarians.Include(u => u.Role).FirstOrDefault(u => u.Login == username);
                    if (librarian != null && librarian.Role != null && librarian.Role.Name == roleName)
                        return true;
                    else
                        return false;
                } else 
                    return false;
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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

        public override string[] GetUsersInRole(string roleName)
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