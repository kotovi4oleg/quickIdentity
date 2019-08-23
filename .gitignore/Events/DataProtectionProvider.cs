using Microsoft.AspNetCore.DataProtection;

namespace WebMvc.Events
{
    public class DataProtectionProvider : IDataProtectionProvider {
        public IDataProtector CreateProtector(string purpose) {
            throw new System.NotImplementedException();
        }
    }
}