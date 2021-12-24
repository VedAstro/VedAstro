using System;
using System.Collections.Generic;
using System.Text;

namespace Genso.Framework
{
    /// <summary>
    /// Represents a single user account
    /// Its is only the data holder of a user account, not the underlying xml strucutre or file
    /// </summary>
    public class UserAccount
    {


        public UserAccount(string id, string username, string email, string key1, string key2, string _lock)
        {
            Id = id;
            Username = username;
            Email = email;
            Key1 = key1;
            Key2 = key2;
            Lock = _lock;
        }

        public string Id { get; }
        public string Username { get; }
        public string Email { get; }
        public string Key1 { get; }
        public string Key2 { get; }
        public string Lock { get; }


    }
}
