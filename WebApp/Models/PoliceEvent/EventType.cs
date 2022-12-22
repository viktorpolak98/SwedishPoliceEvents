using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.PoliceEvent
{
    /// <summary>
    /// Enum with different types of events 
    /// </summary>
    public enum EventType
    {
        [Display(Name = "Alkogollagen")]
        Alkohollagen,

        [Display(Name = "Anträffad död")]
        Anträffad_död,

        [Display(Name = "Anträffat gods")]
        Anträffat_gods,

        [Display(Name = "Arbetsplatsolycka")]
        Arbetsplatsolycka,

        [Display(Name = "Bedrägeri")]
        Bedrägeri,

        [Display(Name = "Bombhot")]
        Bombhot,

        [Display(Name = "Brand")]
        Brand,

        [Display(Name = "Brand automatlarm")]
        Brand_automatlarm,

        [Display(Name = "Bråk")]
        Bråk,

        [Display(Name = "Detonation")]
        Detonation,

        [Display(Name = "Djur skadat omhändertaget")]
        Djur_skadat_omhändertaget,

        [Display(Name = "Ekobrott")]
        Ekobrott,

        [Display(Name = "Farligt föremål, misstänkt")]
        Farligt_föremål_misstänkt,

        [Display(Name = "Fjällräddning")]
        Fjällräddning,

        [Display(Name = "Fylleri/LOB")]
        Fylleri_LOB,

        [Display(Name = "Förfalskningsbrott")]
        Förfalskningsbrott,

        [Display(Name = "Försvunnen person")]
        Försvunnen_person,

        [Display(Name = "Gränskontroll")]
        Gränskontroll,

        [Display(Name = "Häleri")]
        Häleri,

        [Display(Name = "Inbrott")]
        Inbrott,

        [Display(Name = "Inbrott, försök")]
        Inbrott_försök,

        [Display(Name = "Knivlagen")]
        Knivlagen,

        [Display(Name = "Kontroll person/fordon")]
        Kontroll_person_fordon,

        [Display(Name = "Lagen om hundar och katter")]
        Lagen_om_hundar_och_katter,

        [Display(Name = "Larm inbrott")]
        Larm_inbrott,

        [Display(Name = "Larm Överfall")]
        Larm_överfall,

        [Display(Name = "Miljöbrott")]
        Miljöbrott,

        [Display(Name = "Missbruk av urkund")]
        Missbruk_av_urkund,

        [Display(Name = "Misshandel")]
        Misshandel,

        [Display(Name = "Misshandel, grov")]
        Misshandel_grov,

        [Display(Name = "Mord/dråp")]
        Mord_dråp,

        [Display(Name = "Mord/dråp, försök")]
        Mord_dråp_försök,

        [Display(Name = "Motorfordon, anträffat stulet")]
        Motorfordon_anträffat_stulet,

        [Display(Name = "Motorfordon, stöld")]
        Motorfordon_stöld,

        [Display(Name = "Narkotikabrott")]
        Narkotikabrott,

        [Display(Name = "Naturkatastrof")]
        Naturkatastrof,

        [Display(Name = "Ofog barn/ungdom")]
        Ofog_barn_ungdom,

        [Display(Name = "Ofredande/förargelse")]
        Ofredande_förargelse,

        [Display(Name = "Olaga frihetsberövande")]
        Olaga_frihetsberövande,

        [Display(Name = "Olaga hot")]
        Olaga_hot,


        [Display(Name = "Olaga intrång")]
        Olaga_intrång,

        [Display(Name = "Olaga intrång/hemfridsbrott")]
        Olaga_intrång_hemfridsbrott,


        [Display(Name = "Olovlig körning")]
        Olovlig_körning,

        [Display(Name = "Ordningslagen")]
        Ordningslagen,

        [Display(Name = "Polisinsats/kommendering")]
        Polisinsats_kommendering,

        [Display(Name = "Rattfylleri")]
        Rattfylleri,

        [Display(Name = "Rån")]
        Rån,

        [Display(Name = "Rån väpnat")]
        Rån_väpnat,

        [Display(Name = "Rån övrigt")]
        Rån_övrigt,

        [Display(Name = "Rån, försök")]
        Rån_försök,

        [Display(Name = "Räddningsinsats")]
        Räddningsinsats,

        [Display(Name = "Sammanfattning dag")]
        Sammanfattning_dag,

        [Display(Name = "Sammanfattning dygn")]
        Sammanfattning_dygn,

        [Display(Name = "Sammanfattning eftermiddag")]
        Sammanfattning_eftermiddag,

        [Display(Name = "Sammanfattning förmiddag")]
        Sammanfattning_förmiddag,

        [Display(Name = "Sammanfattning helg")]
        Sammanfattning_helg,

        [Display(Name = "Sammanfattning kväll")]
        Sammanfattning_kväll,

        [Display(Name = "Sammanfattning kväll och natt")]
        Sammanfattning_kväll_och_natt,

        [Display(Name = "Sammanfattning natt")]
        Sammanfattning_natt,

        [Display(Name = "Sammanfattning vecka")]
        Sammanfattning_vecka,

        [Display(Name = "Sedlighetsbrott")]
        Sedlighetsbrott,

        [Display(Name = "Sjukdom/olycksfall")]
        Sjukdom_olycksfall,

        [Display(Name = "Sjölagen")]
        Sjölagen,

        [Display(Name = "Skadegörelse")]
        Skadegörelse,

        [Display(Name = "Skottlossning")]
        Skottlossning,

        [Display(Name = "Skottlossning, misstänkt")]
        Skottlossning_misstänkt,

        [Display(Name = "Spridning smittsamma kemikalier")]
        Spridning_smittsamma_kemikalier,

        [Display(Name = "Stöld")]
        Stöld,

        [Display(Name = "Stöld, försök")]
        Stöld_försök,

        [Display(Name = "Stöld, ringa")]
        Stöld_ringa,

        [Display(Name = "Stöld/inbrott")]
        Stöld_inbrott,

        [Display(Name = "Tillfälligt obemannat")]
        Tillfälligt_obemannat,

        [Display(Name = "Trafikbrott")]
        Trafikbrott,

        [Display(Name = "Trafikhinder")]
        Trafikhinder,

        [Display(Name = "Trafikkontroll")]
        Trafikkontroll,

        [Display(Name = "Trafikolycka")]
        Trafikolycka,

        [Display(Name = "Trafikolycka, personskada")]
        Trafikolycka_personskada,

        [Display(Name = "Trafikolycka, singel")]
        Trafikolycka_singel,

        [Display(Name = "Trafikolycka, smitning från")]
        Trafikolycka_smitning_från,

        [Display(Name = "Trafikolycka, vilt")]
        Trafikolycka_vilt,

        [Display(Name = "Uppdatering")]
        Uppdatering,

        [Display(Name = "Utlänningslagen")]
        Utlänningslagen,

        [Display(Name = "Vapenlagen")]
        Vapenlagen,

        [Display(Name = "Varningslarm/haveri")]
        Varningslarm_haveri,

        [Display(Name = "Våld/hot mot tjänsteman")]
        Våld_hot_mot_tjänsteman,

        [Display(Name = "Våldtäkt")]
        Våldtäkt,

        [Display(Name = "Våldtäkt, försök")]
        Våldtäkt_försök,

        [Display(Name = "Vållande till kroppsskada")]
        Vållande_till_kroppsskada,

        [Display(Name = "Övrigt")]
        Övrigt

    }
}
