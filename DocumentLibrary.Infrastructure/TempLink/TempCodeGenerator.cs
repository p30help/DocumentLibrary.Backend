using DocumentLibrary.Domain.Contracts;
using DocumentsLibrary.Application.Common;
using Newtonsoft.Json;

namespace DocumentLibrary.Infrastructure.TempLink
{
    public class TempCodeGenerator : ITempCodeGenerator
    {
        private readonly IEncyptionService encyptionService;
        private readonly IDateProvider dateProvider;

        public TempCodeGenerator(IEncyptionService encyptionService, IDateProvider dateProvider)
        {
            this.encyptionService = encyptionService;
            this.dateProvider = dateProvider;
        }

        public string GenerateTempCode(Guid documentId, TimeSpan expirationTime)
        {
            var expiration = dateProvider.UtcNow.Add(expirationTime);

            var model = new TempCodeModel(documentId, expiration);
            var json = JsonConvert.SerializeObject(model);

            var encyptionText = encyptionService.Encrypt(json);

            var base64Text = Base64Encode(encyptionText);

            return base64Text;
        }

        static string Base64Encode(string text)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textBytes);
        }
        static string Base64Decode(string base64)
        {
            var base64Bytes = System.Convert.FromBase64String(base64);
            return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }

        public TempCode ValidateTempCode(string generatedCode)
        {
            try
            {
                var text = Base64Decode(generatedCode);
                var json = encyptionService.Decrypt(text);
                var model = JsonConvert.DeserializeObject<TempCodeModel>(json);

                if (model == null || model.expiration < dateProvider.UtcNow)
                {
                    return new TempCode() { IsValid = false };
                }

                return new TempCode() { IsValid = true, DocumentId = model!.id };
            }
            catch
            {
                return new TempCode() { IsValid = false };
            }
        }

        record TempCodeModel(Guid id, DateTimeOffset expiration);
    }
}
