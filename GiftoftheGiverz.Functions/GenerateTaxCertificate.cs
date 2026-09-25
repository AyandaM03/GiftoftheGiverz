using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace GiftoftheGiverz.Functions
{
    public class GenerateTaxCertificate
    {
        [Function("GenerateTaxCertificate")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")]
            HttpRequestData req)
        {
            // Handle a GET request from the browser
            if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                var getResponse = req.CreateResponse(HttpStatusCode.OK);

                await getResponse.WriteStringAsync(
                    "GenerateTaxCertificate Azure Function is running. " +
                    "Send a POST request with donation information to generate a tax certificate.");

                return getResponse;
            }

            // Read donation information sent by the web application
            var donation = await req.ReadFromJsonAsync<DonationRequest>();

            if (donation == null)
            {
                var badResponse = req.CreateResponse(
                    HttpStatusCode.BadRequest);

                await badResponse.WriteStringAsync(
                    "Invalid donation information.");

                return badResponse;
            }

            // Generate a dummy tax certificate number
            var certificateNumber =
                "GOG-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // Create the tax certificate
            var certificate = new
            {
                CertificateNumber = certificateNumber,
                DonorName = donation.DonorName,
                DonationAmount = donation.Amount,
                Currency = donation.Currency,
                DonationReference = donation.ReferenceNumber,
                DonationType = donation.DonationType,
                IssuedDate = DateTime.Now.ToString("yyyy-MM-dd"),
                Organisation = "Gift of the Givers"
            };

            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(certificate);

            return response;
        }
    }

    public class DonationRequest
    {
        public string DonorName { get; set; } = "Anonymous Donor";

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "ZAR";

        public string ReferenceNumber { get; set; } = string.Empty;

        public string DonationType { get; set; } = "One-Time";
    }
}