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
    
    public partial class Statistika
    {
        public int Id { get; set; }
        public Nullable<int> BrPoena { get; set; }
        public Nullable<double> Efikasnost { get; set; }
        public Nullable<int> ID_UCENIKA { get; set; }
    
        public virtual Ucenik Ucenik { get; set; }
    }
}
