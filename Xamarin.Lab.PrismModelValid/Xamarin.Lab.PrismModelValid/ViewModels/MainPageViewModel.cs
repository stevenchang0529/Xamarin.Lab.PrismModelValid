using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.Lab.PrismModelValid.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
    

        [Required(ErrorMessage = "請輸入密碼")]
        public string Pwd { get; set; }


        public ICommand OnLogin { get;set;}
        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Main Page";

            this.OnLogin = new DelegateCommand(async () =>
              {
                  if(await this.ValidAndAlert(pageDialogService))
                  {
                      //..驗證通過要做的事情
                  }
              });
        }

        public  List<ValidationResult> Valid()
        {
            ValidationContext context = new ValidationContext(this);
            List<ValidationResult> resultList = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, resultList, true);
            return resultList;
        }

        public async  Task<bool> ValidAndAlert(IPageDialogService pageDialogService)
        {
            var result = this.Valid().ToList();
            if (result.Any())
            {
                var message = String.Join(Environment.NewLine, result.Select(c => c.ErrorMessage));
                await pageDialogService.DisplayAlertAsync("訊息", message, "OK");
            }
            return result.Count == 0;
        }

    }

    public class IdTestAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
         
            //這裡做要驗證的事情
            return ValidationResult.Success;
        }
    }

}
