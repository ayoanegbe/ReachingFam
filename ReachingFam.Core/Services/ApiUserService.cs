using Microsoft.EntityFrameworkCore;
using ReachingFam.Core.Data;
using ReachingFam.Core.Enums;
using ReachingFam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Services
{
    public class ApiUserService(ApplicationDbContext context, CustomIDataProtection protection)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly CustomIDataProtection _protection = protection;

        public string EncryptData(string plainText)
        {
            string encrypted = _protection.Encode(plainText);

            byte[] binaryData = Encoding.UTF8.GetBytes(encrypted);
            string base64EncodedData = Convert.ToBase64String(binaryData);

            return base64EncodedData;
        }

        public string DecryptData(string encryptedText)
        {
            string base64EncodedData = encryptedText;
            byte[] binaryData = Convert.FromBase64String(base64EncodedData);

            string decodedString = Encoding.UTF8.GetString(binaryData);

            return _protection.Decode(decodedString);
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*()-_+={}[]|<>/?";
            StringBuilder res = new();
            Random rnd = new();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }

        public async Task<AuthResponse> ProcessLogin(AuthRequest request)
        {
            AuthResponse response = new();

            ApiUser user = await _context.ApiUsers.Where(x => x.ApiUserId == request.ApiUserId).FirstOrDefaultAsync();

            if (user == null)
            {
                response.Status = ResponseStatus.Unauthorized;
                response.Message = "Failed";
                return response;
            }

            string storedPassword = DecryptData(user.Password);
            string requestedPassword = DecryptData(request.Password);

            if (requestedPassword != storedPassword)
            {
                response.Status = ResponseStatus.Unauthorized;
                response.Message = "Failed";
                return response;
            }

            string tokenKey = request.ApiUserId.ToString() + ":" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            response.Token = EncryptData(tokenKey);
            response.Status = ResponseStatus.Success;
            response.Message = "Successful";
            return response;
        }

        public void CreateApiUser()
        {

        }
    }
}
