using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projeto.WEB.Validators
{
    public class DateValidator:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                return (DateTime)value < DateTime.Now;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}