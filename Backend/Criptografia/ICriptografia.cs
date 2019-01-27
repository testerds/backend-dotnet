using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Criptografia
{
    public interface ICriptografia
    {
        string Encrypt(string password);
        string Decrypt(string password);
    }
}
