using Microsoft.AspNetCore.DataProtection;

namespace WebMvc.Events
{
    public class DataProtector : IDataProtector
    {
        public IDataProtector CreateProtector(string purpose)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Protect(byte[] plaintext)
        {
            throw new System.NotImplementedException();
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            throw new System.NotImplementedException();
        }
    }
}