using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MassScheduler.Validation
{
    /// <summary>
    /// Validation attribute to ensure that one date preceeds another.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class DateIsBefore : ValidationAttribute, IClientValidatable
    {
        private readonly string _otherDateFieldName;

        /// <summary>
        /// Validates that the property is before another date field.
        /// </summary>
        /// <param name="otherField">The field to compare to</param>
        /// <param name="errorMessage">Error to use if invalid</param>
        public DateIsBefore(string otherField, string errorMessage)
            : base(errorMessage)
        {
            _otherDateFieldName = otherField;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;

            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(_otherDateFieldName);
            if (otherProperty.PropertyType == DateTime.Now.GetType())
            {
                // Get the value of the field the attribute is assigned to
                var propertyToValidate = (DateTime)value;
                // Get the value of the "other property"
                var propertyToCompare = (DateTime)otherProperty.GetValue(validationContext.ObjectInstance, null);

                // If the property is the same or after the "other property"
                // then set an error.
                if (propertyToValidate.CompareTo(propertyToCompare) < 1)
                {
                    result = new ValidationResult(ErrorMessageString);
                }
            }
            else
            {
                result = new ValidationResult("Error validating property.  OtherProperty must be a DateTime object.");
            }

            return result;
        }

        /// <summary>
        /// For use in client side validation.
        /// </summary>
        /// <param name="metadata">The model metadata</param>
        /// <param name="contex">The controller context</param>
        /// <returns>Validation rules</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext contex)
        {
            var dateIsBeforeRules = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "dateisbefore"
            };

            dateIsBeforeRules.ValidationParameters.Add("otherfield", _otherDateFieldName);

            yield return dateIsBeforeRules;
        }
    }
}