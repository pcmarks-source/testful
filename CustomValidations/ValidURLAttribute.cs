using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using TESTful.Models;

namespace TESTful.CustomValidations
{
    public class ValidURLAttribute : ValidationAttribute 
    {
        private TESTfulViewModel Model;
        private string RawRequestedURL;
        private Uri RequestedURL;

        public const string NullError = "Target URL can not be null.";
        public const string FormatError = "Target URL is not formatted properly.";
        public const string UnreachableError = "Target URL is unreachable.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            Model = (TESTfulViewModel)validationContext.ObjectInstance;
            RawRequestedURL = (string)value;

            if (string.IsNullOrWhiteSpace(RawRequestedURL) == true) return new ValidationResult(NullError);
            if (Uri.TryCreate(RawRequestedURL, UriKind.Absolute, out RequestedURL) == false) return new ValidationResult(FormatError);
            if (RequestedURL.IsWellFormedOriginalString() == false) return new ValidationResult(FormatError);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestedURL);
            request.Timeout = 1000;

            try {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    Model.ResolvedURL = RequestedURL;
                    return ValidationResult.Success;
                }
            } catch (WebException) {
                return new ValidationResult(UnreachableError);
            }
        }
    }
}
