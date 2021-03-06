//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaresoftHMISDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class OpthalmologyExaminationEntry
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public int OPDNo { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string RightLids { get; set; }
        public string ClWear { get; set; }
        public string LenseType { get; set; }
        public string Wearing { get; set; }
        public string ManufacturingCo { get; set; }
        public string RightOdPower { get; set; }
        public string LeftOdPower { get; set; }
        public string RightOs { get; set; }
        public string LeftOs { get; set; }
        public string RightIop { get; set; }
        public string LeftIop { get; set; }
        public string LeftLids { get; set; }
        public string RightTearFilm { get; set; }
        public string LeftTearFilm { get; set; }
        public string RightCunjuctiva { get; set; }
        public string LeftCunjuctiva { get; set; }
        public string RightCornea { get; set; }
        public string LeftCornea { get; set; }
        public string RightIris { get; set; }
        public string LeftIris { get; set; }
        public string RightPupil { get; set; }
        public string LeftPupil { get; set; }
        public string RightLens { get; set; }
        public string LeftLens { get; set; }
        public string RightGlucomaVa { get; set; }
        public string LeftGlucomaVa { get; set; }
        public string RightGlucomaIop { get; set; }
        public string LeftGlucomaIop { get; set; }
        public string RightDClarity { get; set; }
        public string LeftDClarity { get; set; }
        public string RightVifa { get; set; }
        public string LeftVifa { get; set; }
        public string RightUncorrectedVisual { get; set; }
        public string LeftUncorrectedVisual { get; set; }
        public string RightCorrectedVisual { get; set; }
        public string LeftCorrectedVisual { get; set; }
        public string RightDistanceSphere1 { get; set; }
        public string LeftDistanceSphere1 { get; set; }
        public string RightDistanceSphere2 { get; set; }
        public string RightDistanceSphere3 { get; set; }
        public string LeftDistanceSphere2 { get; set; }
        public string LeftDistanceSphere3 { get; set; }
        public string RightDistanceCyl1 { get; set; }
        public string RightDistanceCyl2 { get; set; }
        public string RightDistanceCyl3 { get; set; }
        public string LeftDistanceCyl1 { get; set; }
        public string LeftDistanceCyl2 { get; set; }
        public string LeftDistanceCyl3 { get; set; }
        public string RightDistanceAxis1 { get; set; }
        public string RightDistanceAxis2 { get; set; }
        public string RightDistanceAxis3 { get; set; }
        public string LeftDistanceAxis1 { get; set; }
        public string LeftDistanceAxis2 { get; set; }
        public string LeftDistanceAxis3 { get; set; }
        public string RightDistanceVn1 { get; set; }
        public string RightDistanceVn2 { get; set; }
        public string RightDistanceVn3 { get; set; }
        public string LeftDistanceVn1 { get; set; }
        public string LeftDistanceVn2 { get; set; }
        public string LeftDistanceVn3 { get; set; }
        public string RightKeratometry { get; set; }
        public string LeftKeratometry { get; set; }
        public string RightPachymetry { get; set; }
        public string LeftPachymetry { get; set; }
        public string Topography { get; set; }
        public string UsgAScan { get; set; }
        public string UsgBScan { get; set; }
        public string Advice { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
