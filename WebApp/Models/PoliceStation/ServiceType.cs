using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public enum ServiceType
    {
        [Display(Name = "Anmälan")]
        Anmälan,

        [Display(Name = "Pass")]
        Pass,

        [Display(Name = "Tillstånd")]
        Tillstånd,

        [Display(Name = "Hittegods")]
        Hittegods,

        [Display(Name = "Vapen")]
        Vapen,

        [Display(Name = "Delgivning")]
        Delgivning,

        [Display(Name = "Cyklar")]
        Cyklar,

        [Display(Name = "Provisoriskt pass")]
        Provisoriskt_pass,

        [Display(Name = "Beslag")]
        Beslag
    }
}
