using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AllowFileSizeAttribute : ValidationAttribute
    {
        #region Public / Protected Properties

        public double FileSize { get; set; } = 1.5;
        public new string ErrorMessage { get; set; }

        #endregion


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Initialization  
            IFormFile file = value as IFormFile;
            bool isValid = true;

            // Settings.  
            int allowedFileSize = Convert.ToInt32(FileSize * 1024 * 1024);

            // Verification.  
            if (file != null)
            {
                // Initialization.  
                long fileSize = file.Length;

                // Settings.  
                isValid = fileSize <= allowedFileSize;
            }

            // Info  
            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}