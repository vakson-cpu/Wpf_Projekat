//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekat
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tim()
        {
            this.Pobedes = new HashSet<Pobede>();
        }
    
        public int Id { get; set; }
        public string Naziv_Skole { get; set; }
        public int id_ucenika { get; set; }
        public Nullable<int> Razred { get; set; }
    
        public virtual Skola Skola { get; set; }
        public virtual Ucenik Ucenik { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pobede> Pobedes { get; set; }
    }
}
